using System.Globalization;
using StbImageSharp;

namespace RecompOne.Runtime.Hle;

//pres lddr
public sealed class SlangParameter
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public float Default { get; init; }
    public float Min { get; init; }
    public float Max { get; init; }
    public float Step { get; init; }
    public float Value { get; internal set; }
}

internal enum SlangScale
{
    Source,
    Viewport,
    Absolute
}

internal enum SlangWrap
{
    ClampToBorder,
    ClampToEdge,
    Repeat,
    MirroredRepeat
}

internal sealed class SlangPass
{
    public string Path = "";
    public string Vertex = "";
    public string Fragment = "";
    public string? Alias;
    public string? Format;
    public bool Linear;
    public SlangWrap Wrap;
    public bool MipmapInput;
    public SlangScale ScaleTypeX = SlangScale.Source;
    public SlangScale ScaleTypeY = SlangScale.Source;
    public float ScaleX = 1f;
    public float ScaleY = 1f;
    public int FrameCountMod;
}

internal sealed class SlangTexture
{
    public string Name = "";
    public int Width, Height;
    public byte[] Rgba = [];
    public bool Linear;
    public SlangWrap Wrap;
    public bool Mipmap;
}

internal sealed class SlangPreset
{
    private const int MaxReferenceDepth = 16;

    private static readonly string[] ShaderExtensions = [".slang", ".h", ".inc", ".glsl", ".hlsl", ".fx", ".fxh"];

    private static readonly Dictionary<string, string> Wildcards = new(StringComparer.Ordinal)
    {
        ["$CORE$"] = "RecompOne",
        ["$GAME$"] = "RecompOne",
        ["$VID-DRV$"] = "glcore",
        ["$VID-DRV-SHADER-EXT$"] = "slang",
        ["$VID-DRV-PRESET-EXT$"] = "slangp",
        ["$CORE-REQ-ROT$"] = "CORE-REQ-ROT-0",
        ["$VID-ALLOW-CORE-ROT$"] = "VID-ALLOW-CORE-ROT-OFF",
        ["$VID-USER-ROT$"] = "VID-USER-ROT-0",
        ["$VID-FINAL-ROT$"] = "VID-FINAL-ROT-0",
        ["$SCREEN-ORIENT$"] = "SCREEN-ORIENT-0",
        ["$VIEW-ASPECT-ORIENT$"] = "VIEW-ASPECT-ORIENT-HORZ",
        ["$CORE-ASPECT-ORIENT$"] = "CORE-ASPECT-ORIENT-HORZ"
    };

    public readonly List<SlangPass> Passes = [];
    public readonly List<SlangTexture> Textures = [];
    public readonly List<SlangParameter> Parameters = [];

    private readonly record struct Entry(string Value, string Dir);

    public static SlangPreset Load(string path)
    {
        var entries = new Dictionary<string, Entry>(StringComparer.Ordinal);
        Read(Path.GetFullPath(path), entries, 0);

        var preset = new SlangPreset();
        if (!entries.TryGetValue("shaders", out var countEntry) ||
            !int.TryParse(countEntry.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var count) ||
            count <= 0)
            throw new InvalidDataException("the preset has no shaders");

        var seen = new HashSet<string>(StringComparer.Ordinal);
        for (var i = 0; i < count; i++)
        {
            if (!entries.TryGetValue($"shader{i}", out var shaderEntry))
                throw new InvalidDataException($"shader{i} is missing");

            var shaderPath = Resolve(shaderEntry);
            var source = SlangSource.Load(shaderPath);
            var pass = new SlangPass
            {
                Path = shaderPath,
                Vertex = source.Vertex,
                Fragment = source.Fragment,
                Format = source.Format,
                Alias = Text(entries, $"alias{i}") is { Length: > 0 } alias ? alias : source.Name,
                Linear = Bool(entries, $"filter_linear{i}", false),
                Wrap = Wrap(Text(entries, $"wrap_mode{i}")),
                MipmapInput = Bool(entries, $"mipmap_input{i}", false),
                FrameCountMod = (int)Number(entries, $"frame_count_mod{i}", 0f)
            };

            if (pass.Format == null)
            {
                if (Bool(entries, $"float_framebuffer{i}", false)) pass.Format = "R16G16B16A16_SFLOAT";
                else if (Bool(entries, $"srgb_framebuffer{i}", false)) pass.Format = "R8G8B8A8_SRGB";
            }

            var last = i == count - 1;
            var type = Text(entries, $"scale_type{i}");
            var typeX = Text(entries, $"scale_type_x{i}") ?? type;
            var typeY = Text(entries, $"scale_type_y{i}") ?? type;
            var fallback = last && typeX == null && typeY == null ? SlangScale.Viewport : SlangScale.Source;
            pass.ScaleTypeX = Scale(typeX, fallback);
            pass.ScaleTypeY = Scale(typeY, fallback);

            var scale = Number(entries, $"scale{i}", 1f);
            pass.ScaleX = Number(entries, $"scale_x{i}", scale);
            pass.ScaleY = Number(entries, $"scale_y{i}", scale);

            preset.Passes.Add(pass);

            foreach (var parameter in source.Parameters)
                if (seen.Add(parameter.Name))
                    preset.Parameters.Add(parameter);
        }

        for (var i = 0; i < preset.Parameters.Count; i++)
        {
            var parameter = preset.Parameters[i];
            var value = Number(entries, parameter.Name, parameter.Default);
            preset.Parameters[i] = new SlangParameter
            {
                Name = parameter.Name,
                Description = parameter.Description,
                Default = value,
                Min = parameter.Min,
                Max = parameter.Max,
                Step = parameter.Step,
                Value = value
            };
        }

        if (Text(entries, "textures") is { } textures)
            foreach (var name in textures.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!entries.TryGetValue(name, out var textureEntry))
                    throw new InvalidDataException($"texture {name} no path");
                preset.Textures.Add(LoadTexture(name, Resolve(textureEntry), entries));
            }

        return preset;
    }

    public static IReadOnlyList<string> References(string fileName, string text)
    {
        var found = new List<string>();
        if (!ShaderExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
        {
            var entries = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var raw in text.Split('\n'))
            {
                var line = raw.Trim();
                if (ReferenceTarget(line) is { } reference)
                {
                    found.Add(reference);
                    if (Expand(reference) is var expanded && expanded != reference) found.Add(expanded);
                    continue;
                }

                if (Pair(line) is { } pair) entries[pair.Key] = pair.Value;
            }

            foreach (var (key, value) in entries)
                if (IsShaderKey(key))
                    found.Add(value);

            if (entries.TryGetValue("textures", out var textures))
                foreach (var name in textures.Split(';',
                             StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    if (entries.TryGetValue(name, out var texture))
                        found.Add(texture);
        }
        else
        {
            foreach (var raw in text.Split('\n'))
                if (SlangSource.IncludeTarget(raw) is { } include)
                    found.Add(include);
        }

        return found;
    }

    private static bool IsShaderKey(string key)
    {
        if (!key.StartsWith("shader", StringComparison.Ordinal) || key.Length == 6) return false;
        for (var i = 6; i < key.Length; i++)
            if (!char.IsAsciiDigit(key[i]))
                return false;
        return true;
    }

    private static void Read(string path, Dictionary<string, Entry> entries, int depth)
    {
        if (depth > MaxReferenceDepth) throw new InvalidDataException("preset references are nested too deep");
        if (!File.Exists(path)) throw new FileNotFoundException($"preset not found: {path}");

        var dir = Path.GetDirectoryName(path) ?? "";
        foreach (var raw in File.ReadAllLines(path))
        {
            var line = raw.Trim();
            if (ReferenceTarget(line) is { } reference)
            {
                var expanded = Path.GetFullPath(Path.Combine(dir, Expand(reference)));
                Read(File.Exists(expanded) ? expanded : Path.GetFullPath(Path.Combine(dir, reference)), entries,
                    depth + 1);
                continue;
            }

            if (Pair(line) is { } pair) entries[pair.Key] = new Entry(pair.Value, dir);
        }
    }

    private static string Expand(string path)
    {
        if (!path.Contains('$')) return path;
        foreach (var (key, value) in Wildcards) path = path.Replace(key, value, StringComparison.Ordinal);
        return path;
    }

    private static string? ReferenceTarget(string line)
    {
        if (!line.StartsWith("#reference", StringComparison.Ordinal)) return null;
        var value = line[10..].Trim();
        return Unquote(value) is { Length: > 0 } target ? target : null;
    }

    private static KeyValuePair<string, string>? Pair(string line)
    {
        if (line.Length == 0 || line[0] == '#' || line.StartsWith("//", StringComparison.Ordinal)) return null;
        var eq = line.IndexOf('=');
        if (eq <= 0) return null;

        var key = line[..eq].Trim();
        var value = line[(eq + 1)..].Trim();
        if (value.StartsWith('"'))
        {
            var end = value.IndexOf('"', 1);
            value = end < 0 ? value[1..] : value[1..end];
        }
        else
        {
            var comment = value.IndexOf('#');
            if (comment >= 0) value = value[..comment];
            var slashes = value.IndexOf("//", StringComparison.Ordinal);
            if (slashes >= 0) value = value[..slashes];
            value = value.Trim();
        }

        return key.Length == 0 ? null : new KeyValuePair<string, string>(key, value);
    }

    private static string Unquote(string value)
    {
        value = value.Trim();
        if (value.StartsWith('"'))
        {
            var end = value.IndexOf('"', 1);
            return end < 0 ? value[1..] : value[1..end];
        }

        return value;
    }

    private static string Resolve(Entry entry)
    {
        return Path.GetFullPath(Path.Combine(entry.Dir, entry.Value.Replace('\\', '/')));
    }

    private static string? Text(Dictionary<string, Entry> entries, string key)
    {
        return entries.TryGetValue(key, out var entry) ? entry.Value : null;
    }

    private static bool Bool(Dictionary<string, Entry> entries, string key, bool fallback)
    {
        return Text(entries, key) switch
        {
            null => fallback,
            "true" or "1" => true,
            "false" or "0" => false,
            _ => fallback
        };
    }

    private static float Number(Dictionary<string, Entry> entries, string key, float fallback)
    {
        return Text(entries, key) is { } text &&
               float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
            ? value
            : fallback;
    }

    private static SlangScale Scale(string? type, SlangScale fallback)
    {
        return type switch
        {
            "source" => SlangScale.Source,
            "viewport" => SlangScale.Viewport,
            "absolute" => SlangScale.Absolute,
            _ => fallback
        };
    }

    private static SlangWrap Wrap(string? mode)
    {
        return mode switch
        {
            "clamp_to_edge" => SlangWrap.ClampToEdge,
            "repeat" => SlangWrap.Repeat,
            "mirrored_repeat" => SlangWrap.MirroredRepeat,
            _ => SlangWrap.ClampToBorder
        };
    }

    private static SlangTexture LoadTexture(string name, string path, Dictionary<string, Entry> entries)
    {
        if (!File.Exists(path)) throw new FileNotFoundException($"texture {name} not found: {path}");
        var image = ImageResult.FromMemory(File.ReadAllBytes(path), ColorComponents.RedGreenBlueAlpha);
        return new SlangTexture
        {
            Name = name,
            Width = image.Width,
            Height = image.Height,
            Rgba = image.Data,
            Linear = Bool(entries, $"{name}_linear", false),
            Wrap = Wrap(Text(entries, $"{name}_wrap_mode")),
            Mipmap = Bool(entries, $"{name}_mipmap", false)
        };
    }
}
