using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace RecompOne.Runtime.Hle;

internal sealed partial class SlangSource //basic slang source 
{
    private const int MaxIncludeDepth = 32;

    private const string Extensions = "#extension GL_ARB_shading_language_420pack : enable\n" +
                                      "#extension GL_ARB_arrays_of_arrays : enable\n" +
                                      "#extension GL_ARB_gpu_shader5 : enable\n" +
                                      "#extension GL_ARB_shader_bit_encoding : enable\n" +
                                      "#extension GL_ARB_texture_query_levels : enable\n";

    public string Vertex = "";
    public string Fragment = "";
    public string? Name;
    public string? Format;
    public readonly List<SlangParameter> Parameters = [];

    private enum Stage
    {
        Shared,
        Vertex,
        Fragment
    }

    public static SlangSource Load(string path)
    {
        var source = new SlangSource();
        var shared = new StringBuilder();
        var vertex = new StringBuilder();
        var fragment = new StringBuilder();
        var stage = Stage.Shared;

        foreach (var line in Expand(path, 0))
        {
            var trimmed = line.TrimStart();
            if (trimmed.StartsWith("#version", StringComparison.Ordinal)) continue;

            if (trimmed.StartsWith("#pragma", StringComparison.Ordinal))
            {
                var words = trimmed[7..].TrimStart();
                if (words.StartsWith("stage", StringComparison.Ordinal))
                {
                    var name = words[5..].Trim();
                    stage = name == "vertex" ? Stage.Vertex : name == "fragment" ? Stage.Fragment : stage;
                    continue;
                }

                if (words.StartsWith("parameter", StringComparison.Ordinal))
                {
                    if (Parameter(words[9..]) is { } parameter && source.Parameters.All(p => p.Name != parameter.Name))
                        source.Parameters.Add(parameter);
                    continue;
                }

                if (words.StartsWith("name", StringComparison.Ordinal))
                {
                    source.Name = words[4..].Trim();
                    continue;
                }

                if (words.StartsWith("format", StringComparison.Ordinal))
                {
                    source.Format = words[6..].Trim();
                    continue;
                }
            }

            (stage switch
            {
                Stage.Vertex => vertex,
                Stage.Fragment => fragment,
                _ => shared
            }).Append(line).Append('\n');
        }

        if (vertex.Length == 0 || fragment.Length == 0)
            throw new InvalidDataException($"{Path.GetFileName(path)} needs a vertex and a fragment stage");

        source.Vertex = shared + vertex.ToString();
        source.Fragment = shared + fragment.ToString();
        return source;
    }

    public static string? IncludeTarget(string line)
    {
        var trimmed = line.TrimStart();
        if (!trimmed.StartsWith("#include", StringComparison.Ordinal)) return null;
        var rest = trimmed[8..].Trim();
        if (!rest.StartsWith('"')) return null;
        var end = rest.IndexOf('"', 1);
        return end > 1 ? rest[1..end] : null;
    }

    private static IEnumerable<string> Expand(string path, int depth)
    {
        if (depth > MaxIncludeDepth) throw new InvalidDataException("includes ware nested too deep");
        if (!File.Exists(path)) throw new FileNotFoundException($"shader file not found: {path}");

        var dir = Path.GetDirectoryName(path) ?? "";
        foreach (var line in File.ReadAllLines(path))
        {
            if (IncludeTarget(line) is { } include)
            {
                foreach (var inner in Expand(Path.GetFullPath(Path.Combine(dir, include)), depth + 1))
                    yield return inner;
                continue;
            }

            yield return line;
        }
    }

    private static SlangParameter? Parameter(string text)
    {
        var match = ParameterPattern().Match(text);
        if (!match.Success) return null;

        float Read(int group, float fallback)
        {
            return match.Groups[group].Success &&
                   float.TryParse(match.Groups[group].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) ? value : fallback;
        }

        var initial = Read(3, 0f);
        return new SlangParameter
        {
            Name = match.Groups[1].Value,
            Description = match.Groups[2].Value,
            Default = initial,
            Min = Read(4, initial),
            Max = Read(5, initial),
            Step = Read(6, 0f),
            Value = initial
        };
    }

    public static string ToGl(string body, bool vertexStage, int version)
    {
        var keepLocations = version >= 410;
        var converted = LayoutPattern().Replace(body, match =>
        {
            var next = match.Groups[2].Value;
            var varying = vertexStage ? next == "out" : next == "in";
            var qualifiers = new List<string>();
            var pushConstant = false;

            foreach (var raw in match.Groups[1].Value.Split(','))
            {
                var qualifier = raw.Trim();
                var key = qualifier.Split('=')[0].Trim();
                switch (key)
                {
                    case "":
                    case "set":
                    case "binding":
                        continue;
                    case "push_constant":
                        pushConstant = true;
                        continue;
                    case "location" when varying && !keepLocations:
                        continue;
                    default:
                        qualifiers.Add(qualifier);
                        break;
                }
            }

            if (pushConstant && !qualifiers.Contains("std140")) qualifiers.Insert(0, "std140");
            var prefix = qualifiers.Count == 0 ? "" : $"layout({string.Join(", ", qualifiers)}) ";
            return prefix + match.Groups[2].Value;
        });

        converted = converted.Replace("gl_VertexIndex", "gl_VertexID").Replace("gl_InstanceIndex", "gl_InstanceID");
        return version >= 450 ? $"#version {version}\n{converted}" : $"#version {version}\n{Extensions}{converted}";
    }

    [GeneratedRegex("""^\s*(\w+)\s+"([^"]*)"\s+([-+.\deE]+)(?:\s+([-+.\deE]+))?(?:\s+([-+.\deE]+))?(?:\s+([-+.\deE]+))?""")] //this is very useful
    private static partial Regex ParameterPattern();

    [GeneratedRegex(@"layout\s*\(([^)]*)\)\s*(\w*)")]
    private static partial Regex LayoutPattern();
}
