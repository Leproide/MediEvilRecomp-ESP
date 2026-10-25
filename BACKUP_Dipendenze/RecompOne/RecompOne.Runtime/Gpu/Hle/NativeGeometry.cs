using System.Numerics;

namespace RecompOne.Runtime.Hle;

//servers for sending float data for the gpu for the game patches

public readonly record struct NativeVertex(Vector3 Camera, Vector2 Screen, int Transform)
{
    public bool IgnoreDepth { get; init; }
}

public static class NativeGeometry
{
    private readonly record struct Entry(uint Word, NativeVertex Vertex);
    private static readonly Dictionary<uint, Entry> Vertices = new();
    private static bool _enabled;
    public static bool DepthBuffer { get; set; }
    
    public static bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            if (!value) Vertices.Clear();
        }
    }
    
    public static void Store(uint address, uint word, in NativeVertex vertex)
    {
        if (!Enabled) return;
        Vertices[address & 0x1FFFFFFFu] = new Entry(word, vertex);
    }
    
    public static void Store(uint address, uint word, Vector3 camera, Vector2 screen, int transform)
    {
        var vertex = new NativeVertex(camera, screen, transform);
        Store(address, word, in vertex);
    }
    
    public static bool TryTake(uint address, uint word, out NativeVertex vertex)
    {
        vertex = default;
        var key = address & 0x1FFFFFFFu;
        if (!Enabled || !Vertices.TryGetValue(key, out var entry) || entry.Word != word)
            return false;
        Vertices.Remove(key);
        vertex = entry.Vertex;
        return float.IsFinite(vertex.Camera.X) && float.IsFinite(vertex.Camera.Y) &&
            float.IsFinite(vertex.Camera.Z) && vertex.Camera.Z > 0f &&
            float.IsFinite(vertex.Screen.X) && float.IsFinite(vertex.Screen.Y);
    }
}
