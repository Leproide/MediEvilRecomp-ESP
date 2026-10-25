namespace RecompOne.Runtime.Hle;

public readonly record struct CapturedImage(int Margin, int Width, int Height);

public sealed record CaptureOverlay(byte[] Rgba, int Width, int Height, int X, int Y, bool Behind = true);

/// <summary>
/// GPU BACKEND
/// </summary>
public interface IGpuBackend
{
    // false in headless or if gl init failed
    bool Ready { get; }

    // submit
    void SetDrawEnv(in HleDrawEnv env);
    void DrawTri(in HleVertex a, in HleVertex b, in HleVertex c, in PrimFlags f);
    void DrawRect(in HleRect r, in PrimFlags f);
    void DrawLine(in HleVertex a, in HleVertex b, in PrimFlags f);
    void FillRect(int x, int y, int w, int h, ushort color15);
    void FillDisplayMargins(int x, int y, int w, int h, ushort color15) { }
    void CopyVram(int sx, int sy, int dx, int dy, int w, int h);
    void WriteVram(int x, int y, int w, int h, ReadOnlySpan<ushort> px);
    void ReadVram(int x, int y, int w, int h, Span<ushort> px);

    //add other stuff
    int RegisterImage(ReadOnlySpan<byte> rgba, int width, int height);
    int ReserveImage() => -1;
    void CaptureTarget(int image, IReadOnlyList<CaptureOverlay> overlays) { }
    bool TryGetCapture(int image, out CapturedImage capture)
    {
        capture = default;
        return false;
    }
    void ReleaseImage(int image) { }

    // these touch gl
    void BeginScene() { }
    void Flush();
    void Present(in HleDispEnv disp);
}
