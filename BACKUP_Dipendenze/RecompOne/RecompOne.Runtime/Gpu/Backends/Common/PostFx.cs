namespace RecompOne.Runtime.Hle;

//obj for pfx dat
public static class PostFx
{
    private static readonly object _gate = new();
    private static SlangPreset? _preset;
    private static int _version;
    private static int _paramVersion;

    public static string? Path { get; private set; }

    public static string? Name { get; private set; }

    public static string? Error { get; internal set; }

    public static bool NativeInput { get; set; } = true;

    public static bool Active
    {
        get
        {
            lock (_gate)
            {
                return _preset != null;
            }
        }
    }

    public static IReadOnlyList<SlangParameter> Parameters
    {
        get
        {
            lock (_gate)
            {
                return _preset?.Parameters.ToArray() ?? [];
            }
        }
    }

    internal static SlangPreset? Preset
    {
        get
        {
            lock (_gate)
            {
                return _preset;
            }
        }
    }

    internal static int Version
    {
        get
        {
            lock (_gate)
            {
                return _version;
            }
        }
    }

    internal static int ParamVersion
    {
        get
        {
            lock (_gate)
            {
                return _paramVersion;
            }
        }
    }

    public static bool Load(string presetPath)
    {
        SlangPreset preset;
        try
        {
            preset = SlangPreset.Load(presetPath);
        }
        catch (Exception e)
        {
            lock (_gate)
            {
                Clear();
                Error = e.Message;
            }

            return false;
        }

        lock (_gate)
        {
            _preset = preset;
            Path = System.IO.Path.GetFullPath(presetPath);
            Name = System.IO.Path.GetFileNameWithoutExtension(presetPath);
            Error = null;
            _version++;
            _paramVersion++;
        }

        return true;
    }

    public static void Clear()
    {
        lock (_gate)
        {
            _preset = null;
            Path = null;
            Name = null;
            Error = null;
            _version++;
            _paramVersion++;
        }
    }

    public static void SetParam(string name, float value)
    {
        lock (_gate)
        {
            var parameter = _preset?.Parameters.Find(p => p.Name == name);
            if (parameter == null || parameter.Value == value) return;
            parameter.Value = value;
            _paramVersion++;
        }
    }

    public static void SetParams(IEnumerable<KeyValuePair<string, float>> values)
    {
        lock (_gate)
        {
            if (_preset == null) return;
            foreach (var (name, value) in values)
                if (_preset.Parameters.Find(p => p.Name == name) is { } parameter)
                    parameter.Value = value;
            _paramVersion++;
        }
    }

    public static void ResetParams()
    {
        lock (_gate)
        {
            if (_preset == null) return;
            foreach (var parameter in _preset.Parameters) parameter.Value = parameter.Default;
            _paramVersion++;
        }
    }

    public static IReadOnlyList<string> References(string fileName, string text)
    {
        return SlangPreset.References(fileName, text);
    }

    internal static (string Name, float Value)[] SnapshotParams()
    {
        lock (_gate)
        {
            if (_preset == null) return [];
            var values = new (string, float)[_preset.Parameters.Count];
            for (var i = 0; i < values.Length; i++)
                values[i] = (_preset.Parameters[i].Name, _preset.Parameters[i].Value);
            return values;
        }
    }
}
