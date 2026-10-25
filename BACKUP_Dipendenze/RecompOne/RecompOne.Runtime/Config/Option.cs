namespace RecompOne.Runtime.Config;

internal interface IOption
{
    string Key { get; }
    Type ValueType { get; }
    void Reload();
}

public sealed class Option<T> : IOption
{
    private Func<T, T>? _normalize;
    private T _value = default!;
    private volatile bool _cached;

    internal Option(string key, T fallback)
    {
        Key = key;
        Default = fallback;
    }

    public string Key { get; }

    public T Default { get; private set; }

    public bool IsSet => Settings.Has(Key);

    public event Action<T>? Changed;

    Type IOption.ValueType => typeof(T);

    public T Value
    {
        get
        {
            if (_cached) return _value;
            _value = Normalize(Settings.Get(Key, Default));
            _cached = true;
            return _value;
        }
        set => Settings.Set(Key, Normalize(value));
    }

    public Option<T> Normalized(Func<T, T> normalize)
    {
        _normalize = normalize;
        _cached = false;
        return this;
    }

    public Option<T> DefaultTo(T fallback)
    {
        Default = fallback;
        _cached = false;
        return this;
    }

    public void Reset()
    {
        Settings.Remove(Key);
    }

    void IOption.Reload()
    {
        _cached = false;
        Changed?.Invoke(Value);
    }

    private T Normalize(T value)
    {
        return _normalize == null ? value : _normalize(value);
    }

    public static implicit operator T(Option<T> option)
    {
        return option.Value;
    }

    public override string ToString()
    {
        return $"{Key}={Value}";
    }
}

public static class Options
{
    public static Option<bool> Bool(string key, bool fallback = false)
    {
        return Define(key, fallback);
    }

    public static Option<int> Int(string key, int fallback = 0)
    {
        return Define(key, fallback);
    }

    public static Option<float> Float(string key, float fallback = 0f)
    {
        return Define(key, fallback);
    }

    public static Option<string> String(string key, string fallback = "")
    {
        return Define(key, fallback);
    }

    public static Option<TEnum> Enum<TEnum>(string key, TEnum fallback) where TEnum : struct, Enum
    {
        return Define(key, fallback);
    }

    public static Option<T> Define<T>(string key, T fallback)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        Settings.EnsureLoaded();
        if (Settings.Find(key) is { } existing)
            return existing as Option<T> ?? throw new InvalidOperationException(
                $"Option '{key}' is already defined as {existing.ValueType.Name}.");

        var option = new Option<T>(key, fallback);
        Settings.Register(option);
        return option;
    }

    public static bool TryGet<T>(string key, out Option<T> option)
    {
        option = (Settings.Find(key) as Option<T>)!;
        return option != null;
    }
}
