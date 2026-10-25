using System.Diagnostics;
using System.Globalization;
using Silk.NET.OpenGL;

namespace RecompOne.Runtime.Hle;

//roughly copied from
internal sealed unsafe class SlangChain : IDisposable
{
    private const int MaxSize = 8192;
    private const int TextureSrgbDecode = 0x8A48;
    private const int SkipDecode = 0x8A4A;

    private static readonly float[] Mvp =
    [
        2f, 0f, 0f, 0f,
        0f, 2f, 0f, 0f,
        0f, 0f, 1f, 0f,
        -1f, -1f, 0f, 1f
    ];

    private sealed class Target
    {
        public uint Tex, Fbo;
        public int W, H;
        public string? Format;
    }

    private sealed class Member
    {
        public string Name = "";
        public UniformType Type;
        public int Offset;
        public int Location = -1;
    }

    private sealed class Block
    {
        public uint Binding;
        public uint Buffer;
        public byte[] Data = [];
        public Member[] Members = [];
    }

    private sealed class Sampler
    {
        public string Name = "";
        public int Location;
    }

    private sealed class Pass
    {
        public SlangPass Def = null!;
        public uint Program;
        public Block[] Blocks = [];
        public Member[] Loose = [];
        public Sampler[] Samplers = [];
        public Target Output = new();
        public Target? Previous;
    }

    private readonly record struct Bound(uint Tex, int W, int H, bool Linear, SlangWrap Wrap, bool Mipmap, bool Generate = false);

    private readonly GL _gl;
    private readonly int _glsl;
    private readonly Dictionary<(bool, SlangWrap, bool), uint> _samplers = [];
    private readonly Dictionary<string, float> _params = new(StringComparer.Ordinal);
    private readonly Stopwatch _clock = Stopwatch.StartNew();

    private Pass[] _passes = [];
    private (SlangTexture Def, uint Tex)[] _luts = [];
    private Target[] _history = [];
    private uint _vao, _vbo, _readFbo;
    private int _version = -1, _paramVersion = -1;
    private bool _ready;
    private uint _frame;
    private long _lastTicks;
    private int _viewW, _viewH;
    private float _aspect;

    public SlangChain(GL gl)
    {
        _gl = gl;
        var major = gl.GetInteger(GetPName.MajorVersion);
        var minor = gl.GetInteger(GetPName.MinorVersion);
        var version = major * 100 + minor * 10;
        _glsl = version >= 450 ? 450 : version >= 420 ? 420 : version >= 410 ? 410 : 330;
    }

    public uint Apply(uint source, int sourceW, int sourceH, int nativeW, int nativeH, int viewW, int viewH,
        float aspect, out int outW, out int outH)
    {
        outW = sourceW;
        outH = sourceH;
        if (!Sync()) return source;

        SyncParams();

        var native = PostFx.NativeInput && nativeW > 0 && nativeH > 0;
        var originalW = native ? nativeW : sourceW;
        var originalH = native ? nativeH : sourceH;
        _viewW = viewW > 0 ? viewW : originalW;
        _viewH = viewH > 0 ? viewH : originalH;
        _aspect = aspect > 0f ? aspect : (float)originalW / originalH;

        CaptureOriginal(source, sourceW, sourceH, originalW, originalH);

        var srcW = originalW;
        var srcH = originalH;
        foreach (var pass in _passes)
        {
            var w = Measure(pass.Def.ScaleTypeX, pass.Def.ScaleX, srcW, _viewW);
            var h = Measure(pass.Def.ScaleTypeY, pass.Def.ScaleY, srcH, _viewH);
            if (pass.Previous != null) (pass.Output, pass.Previous) = (pass.Previous, pass.Output);
            Ensure(pass.Output, w, h, pass.Def.Format);
            if (pass.Previous != null) Ensure(pass.Previous, w, h, pass.Def.Format);
            srcW = w;
            srcH = h;
        }

        var now = _clock.ElapsedTicks;
        var delta = _lastTicks == 0 ? 0 : (uint)((now - _lastTicks) * 1_000_000 / Stopwatch.Frequency);
        _lastTicks = now;

        _gl.Disable(EnableCap.DepthTest);
        _gl.Disable(EnableCap.Blend);
        _gl.Disable(EnableCap.ScissorTest);
        _gl.Disable(EnableCap.CullFace);
        _gl.BindVertexArray(_vao);

        var units = 0;
        for (var i = 0; i < _passes.Length; i++)
            units = Math.Max(units, Draw(i, delta));

        for (var unit = 0; unit < units; unit++) _gl.BindSampler((uint)unit, 0);
        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.Disable(EnableCap.FramebufferSrgb);
        _frame++;

        var last = _passes[^1].Output;
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, last.Fbo);
        _gl.ColorMask(false, false, false, true);
        _gl.ClearColor(0f, 0f, 0f, 1f);
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        _gl.ColorMask(true, true, true, true);
        _gl.BindTexture(TextureTarget.Texture2D, last.Tex);
        if (last.Format == "R8G8B8A8_SRGB")
            _gl.TexParameter(TextureTarget.Texture2D, (TextureParameterName)TextureSrgbDecode, (int)SkipDecode);

        outW = last.W;
        outH = last.H;
        return last.Tex;
    }

    private bool Sync()
    {
        var version = PostFx.Version;
        if (version == _version) return _ready;
        _version = version;
        _paramVersion = -1;
        Release();

        var preset = PostFx.Preset;
        if (preset == null) return false;

        try
        {
            Build(preset);
            PostFx.Error = null;
            _ready = true;
        }
        catch (Exception e)
        {
            Release();
            PostFx.Error = e.Message;
            Console.WriteLine($"[gpu-pfx] {e.Message}");
        }

        return _ready;
    }

    private void SyncParams()
    {
        var version = PostFx.ParamVersion;
        if (version == _paramVersion) return;
        _paramVersion = version;
        _params.Clear();
        foreach (var (name, value) in PostFx.SnapshotParams()) _params[name] = value;
    }

    private void Build(SlangPreset preset)
    {
        _vao = _gl.GenVertexArray();
        _vbo = _gl.GenBuffer();
        _gl.BindVertexArray(_vao);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        float[] quad = [0f, 0f, 1f, 0f, 0f, 1f, 1f, 1f];
        fixed (float* p = quad)
        {
            _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(quad.Length * sizeof(float)), p,
                BufferUsageARB.StaticDraw);
        }

        _gl.EnableVertexAttribArray(0);
        _gl.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), (void*)0);
        _gl.EnableVertexAttribArray(1);
        _gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), (void*)0);
        _readFbo = _gl.GenFramebuffer();

        var passes = new Pass[preset.Passes.Count];
        for (var i = 0; i < passes.Length; i++)
        {
            var def = preset.Passes[i];
            var name = Path.GetFileName(def.Path);
            var program = GlShaders.Build(_gl, SlangSource.ToGl(def.Vertex, true, _glsl),
                SlangSource.ToGl(def.Fragment, false, _glsl), name, out var error);
            if (program == 0) throw new InvalidOperationException($"{name}: {error ?? "failed to build"}");

            passes[i] = new Pass { Def = def, Program = program };
            _passes = passes[..(i + 1)];
            Reflect(passes[i]);
        }

        _passes = passes;

        var historyDepth = 0;
        foreach (var pass in _passes)
        {
            foreach (var name in Names(pass))
            {
                if (Indexed(name, "OriginalHistorySize", out var n) || Indexed(name, "OriginalHistory", out n))
                    historyDepth = Math.Max(historyDepth, n);

                for (var j = 0; j < _passes.Length; j++)
                    if (IsFeedbackOf(name, j))
                        _passes[j].Previous ??= new Target();
            }
        }

        _history = new Target[historyDepth + 1];
        for (var i = 0; i < _history.Length; i++) _history[i] = new Target();

        _luts = new (SlangTexture, uint)[preset.Textures.Count];
        for (var i = 0; i < _luts.Length; i++) _luts[i] = (preset.Textures[i], Upload(preset.Textures[i]));
    }

    private IEnumerable<string> Names(Pass pass)
    {
        foreach (var sampler in pass.Samplers) yield return sampler.Name;
        foreach (var member in pass.Loose) yield return member.Name;
        foreach (var block in pass.Blocks)
        foreach (var member in block.Members)
            yield return member.Name;
    }

    private bool IsFeedbackOf(string name, int index)
    {
        if (Indexed(name, "PassFeedbackSize", out var n) || Indexed(name, "PassFeedback", out n))
            return n == index;
        var alias = _passes[index].Def.Alias;
        return alias != null && (name == alias + "Feedback" || name == alias + "FeedbackSize");
    }

    private void Reflect(Pass pass)
    {
        var program = pass.Program;
        _gl.GetProgram(program, ProgramPropertyARB.ActiveUniformBlocks, out var blockCount);
        var blocks = new Block[blockCount];
        for (var i = 0; i < blockCount; i++)
        {
            _gl.GetActiveUniformBlock(program, (uint)i, UniformBlockPName.UniformBlockDataSize, out var size);
            var block = new Block { Binding = (uint)i, Data = new byte[size], Buffer = _gl.GenBuffer() };
            _gl.UniformBlockBinding(program, (uint)i, block.Binding);
            _gl.BindBuffer(BufferTargetARB.UniformBuffer, block.Buffer);
            _gl.BufferData(BufferTargetARB.UniformBuffer, (nuint)size, null, BufferUsageARB.DynamicDraw);
            blocks[i] = block;
        }

        var members = new List<Member>[blockCount];
        for (var i = 0; i < blockCount; i++) members[i] = [];
        var loose = new List<Member>();
        var samplers = new List<Sampler>();

        _gl.GetProgram(program, ProgramPropertyARB.ActiveUniforms, out var count);
        for (uint index = 0; index < count; index++)
        {
            var raw = _gl.GetActiveUniform(program, index, out _, out var type);
            var name = Clean(raw);
            int blockIndex, offset;
            _gl.GetActiveUniforms(program, 1, &index, UniformPName.UniformBlockIndex, &blockIndex);
            _gl.GetActiveUniforms(program, 1, &index, UniformPName.UniformOffset, &offset);

            if (blockIndex >= 0)
            {
                members[blockIndex].Add(new Member { Name = name, Type = type, Offset = offset });
                continue;
            }

            var location = _gl.GetUniformLocation(program, raw);
            if (location < 0) continue;
            if (type is UniformType.Sampler2D) samplers.Add(new Sampler { Name = name, Location = location });
            else loose.Add(new Member { Name = name, Type = type, Location = location });
        }

        for (var i = 0; i < blockCount; i++) blocks[i].Members = members[i].ToArray();
        pass.Blocks = blocks;
        pass.Loose = loose.ToArray();
        pass.Samplers = samplers.ToArray();
    }

    private static string Clean(string name)
    {
        var dot = name.LastIndexOf('.');
        if (dot >= 0) name = name[(dot + 1)..];
        var bracket = name.IndexOf('[');
        return bracket >= 0 ? name[..bracket] : name;
    }

    private int Draw(int index, uint delta)
    {
        var pass = _passes[index];
        var output = pass.Output;
        var source = Source(index);

        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, output.Fbo);
        _gl.Viewport(0, 0, (uint)output.W, (uint)output.H);
        if (output.Format == "R8G8B8A8_SRGB") _gl.Enable(EnableCap.FramebufferSrgb);
        else _gl.Disable(EnableCap.FramebufferSrgb);
        _gl.UseProgram(pass.Program);

        foreach (var block in pass.Blocks)
        {
            foreach (var member in block.Members) Uniform(pass, index, member, block.Data, source, delta);
            _gl.BindBuffer(BufferTargetARB.UniformBuffer, block.Buffer);
            fixed (byte* p = block.Data)
            {
                _gl.BufferSubData(BufferTargetARB.UniformBuffer, 0, (nuint)block.Data.Length, p);
            }

            _gl.BindBufferBase(BufferTargetARB.UniformBuffer, block.Binding, block.Buffer);
        }

        foreach (var member in pass.Loose) Uniform(pass, index, member, null, source, delta);

        var unit = 0;
        foreach (var sampler in pass.Samplers)
        {
            var bound = Texture(index, sampler.Name, source);
            _gl.ActiveTexture(TextureUnit.Texture0 + unit);
            _gl.BindTexture(TextureTarget.Texture2D, bound?.Tex ?? 0);
            if (bound is { } b)
            {
                if (b.Generate) _gl.GenerateMipmap(TextureTarget.Texture2D);
                _gl.BindSampler((uint)unit, SamplerFor(b.Linear, b.Wrap, b.Mipmap));
            }

            _gl.Uniform1(sampler.Location, unit);
            unit++;
        }

        _gl.DrawArrays(PrimitiveType.TriangleStrip, 0, 4);
        return unit;
    }

    private Bound Source(int index)
    {
        var def = _passes[index].Def;
        var target = index == 0 ? _history[0] : _passes[index - 1].Output;
        return new Bound(target.Tex, target.W, target.H, def.Linear, def.Wrap, def.MipmapInput, def.MipmapInput);
    }

    private Bound? Texture(int index, string name, Bound source)
    {
        var def = _passes[index].Def;
        if (name == "Source") return source;
        if (name == "Original") return Of(_history[0], def);
        if (Indexed(name, "OriginalHistory", out var n)) return n < _history.Length ? Of(_history[n], def) : null;
        if (Indexed(name, "PassOutput", out n)) return n < index ? Output(n, def) : null;
        if (Indexed(name, "PassFeedback", out n)) return Feedback(n, def);

        for (var i = 0; i < _passes.Length; i++)
        {
            var alias = _passes[i].Def.Alias;
            if (alias == null) continue;
            if (name == alias && i < index) return Output(i, def);
            if (name == alias + "Feedback") return Feedback(i, def);
        }

        foreach (var (lut, tex) in _luts)
            if (lut.Name == name)
                return new Bound(tex, lut.Width, lut.Height, lut.Linear, lut.Wrap, lut.Mipmap);

        return null;
    }

    private static Bound Of(Target target, SlangPass def)
    {
        return new Bound(target.Tex, target.W, target.H, def.Linear, def.Wrap, false);
    }

    private Bound Output(int index, SlangPass reader)
    {
        var target = _passes[index].Output;
        var next = index + 1 < _passes.Length ? _passes[index + 1].Def : reader;
        return new Bound(target.Tex, target.W, target.H, next.Linear, next.Wrap, false);
    }

    private Bound? Feedback(int index, SlangPass reader)
    {
        if (index < 0 || index >= _passes.Length || _passes[index].Previous is not { } target) return null;
        var next = index + 1 < _passes.Length ? _passes[index + 1].Def : reader;
        return new Bound(target.Tex, target.W, target.H, next.Linear, next.Wrap, false);
    }

    private void Uniform(Pass pass, int index, Member member, byte[]? data, Bound source, uint delta)
    {
        var name = member.Name;
        if (name == "MVP")
        {
            Matrix(member, data);
            return;
        }

        if (Size(pass, index, name, source) is { } size)
        {
            Set(member, data, size.W, size.H, 1.0 / size.W, 1.0 / size.H);
            return;
        }

        switch (name)
        {
            case "FrameCount":
                Set(member, data, pass.Def.FrameCountMod > 0 ? _frame % (uint)pass.Def.FrameCountMod : _frame);
                return;
            case "FrameDirection":
            case "CurrentSubFrame":
            case "TotalSubFrames":
                Set(member, data, 1);
                return;
            case "FrameTimeDelta":
                Set(member, data, delta);
                return;
            case "OriginalFPS":
                Set(member, data, 60);
                return;
            case "Rotation":
                Set(member, data, 0);
                return;
            case "OriginalAspect":
            case "OriginalAspectRotated":
                Set(member, data, _aspect);
                return;
        }

        if (_params.TryGetValue(name, out var value)) Set(member, data, value);
    }

    private (int W, int H)? Size(Pass pass, int index, string name, Bound source)
    {
        switch (name)
        {
            case "OutputSize":
                return (pass.Output.W, pass.Output.H);
            case "SourceSize":
                return (source.W, source.H);
            case "OriginalSize":
                return (_history[0].W, _history[0].H);
            case "FinalViewportSize":
                return (_viewW, _viewH);
        }

        if (!name.Contains("Size", StringComparison.Ordinal)) return null;

        if (Indexed(name, "OriginalHistorySize", out var n))
            return n < _history.Length ? (_history[n].W, _history[n].H) : null;
        if (Indexed(name, "PassOutputSize", out n))
            return n < _passes.Length ? (_passes[n].Output.W, _passes[n].Output.H) : null;
        if (Indexed(name, "PassFeedbackSize", out n))
            return n < _passes.Length ? (_passes[n].Output.W, _passes[n].Output.H) : null;

        if (!name.EndsWith("Size", StringComparison.Ordinal)) return null;
        var stem = name[..^4];
        foreach (var other in _passes)
        {
            var alias = other.Def.Alias;
            if (alias != null && (stem == alias || stem == alias + "Feedback")) return (other.Output.W, other.Output.H);
        }

        foreach (var (lut, _) in _luts)
            if (lut.Name == stem)
                return (lut.Width, lut.Height);

        return null;
    }

    private static bool Indexed(string name, string prefix, out int index)
    {
        index = 0;
        return name.Length > prefix.Length && name.StartsWith(prefix, StringComparison.Ordinal) &&
               int.TryParse(name.AsSpan(prefix.Length), NumberStyles.None, CultureInfo.InvariantCulture, out index);
    }

    private void Matrix(Member member, byte[]? data)
    {
        if (member.Type != UniformType.FloatMat4) return;
        if (data == null)
        {
            fixed (float* p = Mvp)
            {
                _gl.UniformMatrix4(member.Location, 1, false, p);
            }

            return;
        }

        System.Buffer.BlockCopy(Mvp, 0, data, member.Offset, Mvp.Length * sizeof(float));
    }

    private void Set(Member member, byte[]? data, double x, double y = 0, double z = 0, double w = 0)
    {
        var floats = member.Type switch
        {
            UniformType.Float => 1,
            UniformType.FloatVec2 => 2,
            UniformType.FloatVec3 => 3,
            UniformType.FloatVec4 => 4,
            _ => 0
        };

        if (floats > 0)
        {
            Span<float> values = [(float)x, (float)y, (float)z, (float)w];
            if (data != null)
            {
                for (var i = 0; i < floats; i++)
                    BitConverter.TryWriteBytes(data.AsSpan(member.Offset + i * 4), values[i]);
                return;
            }

            switch (floats)
            {
                case 1: _gl.Uniform1(member.Location, values[0]); break;
                case 2: _gl.Uniform2(member.Location, values[0], values[1]); break;
                case 3: _gl.Uniform3(member.Location, values[0], values[1], values[2]); break;
                default: _gl.Uniform4(member.Location, values[0], values[1], values[2], values[3]); break;
            }

            return;
        }

        switch (member.Type)
        {
            case UniformType.UnsignedInt:
            case UniformType.Bool:
                var u = x <= 0 ? 0u : (uint)Math.Min(x, uint.MaxValue);
                if (data != null) BitConverter.TryWriteBytes(data.AsSpan(member.Offset), u);
                else _gl.Uniform1(member.Location, u);
                break;
            case UniformType.Int:
                var s = (int)Math.Clamp(x, int.MinValue, int.MaxValue);
                if (data != null) BitConverter.TryWriteBytes(data.AsSpan(member.Offset), s);
                else _gl.Uniform1(member.Location, s);
                break;
        }
    }

    private static int Measure(SlangScale type, float scale, int source, int viewport)
    {
        var size = type switch
        {
            SlangScale.Viewport => viewport * scale,
            SlangScale.Absolute => scale,
            _ => source * scale
        };

        return Math.Clamp((int)MathF.Round(size), 1, MaxSize);
    }

    private void CaptureOriginal(uint source, int sourceW, int sourceH, int w, int h)
    {
        if (_history.Length > 1)
        {
            var oldest = _history[^1];
            Array.Copy(_history, 0, _history, 1, _history.Length - 1);
            _history[0] = oldest;
        }

        var target = _history[0];
        Ensure(target, w, h, null);
        for (var i = 1; i < _history.Length; i++)
            if (_history[i].Tex == 0)
                Ensure(_history[i], w, h, null);

        _gl.BindFramebuffer(FramebufferTarget.ReadFramebuffer, _readFbo);
        _gl.FramebufferTexture2D(FramebufferTarget.ReadFramebuffer, FramebufferAttachment.ColorAttachment0,
            TextureTarget.Texture2D, source, 0);
        _gl.BindFramebuffer(FramebufferTarget.DrawFramebuffer, target.Fbo);
        _gl.BlitFramebuffer(0, 0, sourceW, sourceH, 0, 0, w, h, ClearBufferMask.ColorBufferBit,
            BlitFramebufferFilter.Nearest);
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    private void Ensure(Target target, int w, int h, string? format)
    {
        if (target.Tex != 0 && target.W == w && target.H == h && target.Format == format) return;

        if (target.Tex == 0)
        {
            target.Tex = _gl.GenTexture();
            target.Fbo = _gl.GenFramebuffer();
        }

        var (internalFormat, pixelFormat, pixelType) = Formats(format);
        _gl.BindTexture(TextureTarget.Texture2D, target.Tex);
        _gl.TexImage2D(TextureTarget.Texture2D, 0, internalFormat, (uint)w, (uint)h, 0, pixelFormat, pixelType, null);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.ClampToEdge);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.ClampToEdge);
        _gl.BindFramebuffer(FramebufferTarget.Framebuffer, target.Fbo);
        _gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
            TextureTarget.Texture2D, target.Tex, 0);
        _gl.ClearColor(0f, 0f, 0f, 0f);
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        target.W = w;
        target.H = h;
        target.Format = format;
    }

    private static (InternalFormat, PixelFormat, PixelType) Formats(string? format)
    {
        return format switch
        {
            "R8G8B8A8_SRGB" => (InternalFormat.Srgb8Alpha8, PixelFormat.Rgba, PixelType.UnsignedByte),
            "A2B10G10R10_UNORM_PACK32" => (InternalFormat.Rgb10A2, PixelFormat.Rgba, PixelType.UnsignedByte),
            "R16G16B16A16_SFLOAT" => (InternalFormat.Rgba16f, PixelFormat.Rgba, PixelType.Float),
            "R32G32B32A32_SFLOAT" => (InternalFormat.Rgba32f, PixelFormat.Rgba, PixelType.Float),
            "R16G16_SFLOAT" => (InternalFormat.RG16f, PixelFormat.RG, PixelType.Float),
            "R32G32_SFLOAT" => (InternalFormat.RG32f, PixelFormat.RG, PixelType.Float),
            "R16_SFLOAT" => (InternalFormat.R16f, PixelFormat.Red, PixelType.Float),
            "R32_SFLOAT" => (InternalFormat.R32f, PixelFormat.Red, PixelType.Float),
            "R8_UNORM" => (InternalFormat.R8, PixelFormat.Red, PixelType.UnsignedByte),
            "R8G8_UNORM" => (InternalFormat.RG8, PixelFormat.RG, PixelType.UnsignedByte),
            _ => (InternalFormat.Rgba8, PixelFormat.Rgba, PixelType.UnsignedByte)
        };
    }

    private uint Upload(SlangTexture lut)
    {
        var tex = _gl.GenTexture();
        _gl.BindTexture(TextureTarget.Texture2D, tex);
        fixed (byte* p = lut.Rgba)
        {
            _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, (uint)lut.Width, (uint)lut.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, p);
        }

        if (lut.Mipmap) _gl.GenerateMipmap(TextureTarget.Texture2D);
        return tex;
    }

    private uint SamplerFor(bool linear, SlangWrap wrap, bool mipmap)
    {
        if (_samplers.TryGetValue((linear, wrap, mipmap), out var sampler)) return sampler;

        sampler = _gl.GenSampler();
        var mag = linear ? GLEnum.Linear : GLEnum.Nearest;
        var min = mipmap ? linear ? GLEnum.LinearMipmapLinear : GLEnum.NearestMipmapNearest : mag;
        var mode = wrap switch
        {
            SlangWrap.ClampToEdge => GLEnum.ClampToEdge,
            SlangWrap.Repeat => GLEnum.Repeat,
            SlangWrap.MirroredRepeat => GLEnum.MirroredRepeat,
            _ => GLEnum.ClampToBorder
        };

        _gl.SamplerParameter(sampler, SamplerParameterI.MinFilter, (int)min);
        _gl.SamplerParameter(sampler, SamplerParameterI.MagFilter, (int)mag);
        _gl.SamplerParameter(sampler, SamplerParameterI.WrapS, (int)mode);
        _gl.SamplerParameter(sampler, SamplerParameterI.WrapT, (int)mode);
        _samplers[(linear, wrap, mipmap)] = sampler;
        return sampler;
    }

    private void Destroy(Target? target)
    {
        if (target == null) return;
        if (target.Tex != 0) _gl.DeleteTexture(target.Tex);
        if (target.Fbo != 0) _gl.DeleteFramebuffer(target.Fbo);
        target.Tex = target.Fbo = 0;
    }

    private void Release()
    {
        foreach (var pass in _passes)
        {
            if (pass.Program != 0) _gl.DeleteProgram(pass.Program);
            foreach (var block in pass.Blocks) _gl.DeleteBuffer(block.Buffer);
            Destroy(pass.Output);
            Destroy(pass.Previous);
        }

        foreach (var target in _history) Destroy(target);
        foreach (var (_, tex) in _luts) _gl.DeleteTexture(tex);
        if (_vao != 0) _gl.DeleteVertexArray(_vao);
        if (_vbo != 0) _gl.DeleteBuffer(_vbo);
        if (_readFbo != 0) _gl.DeleteFramebuffer(_readFbo);

        _passes = [];
        _history = [];
        _luts = [];
        _vao = _vbo = _readFbo = 0;
        _ready = false;
        _frame = 0;
        _lastTicks = 0;
    }

    public void Dispose()
    {
        Release();
        foreach (var sampler in _samplers.Values) _gl.DeleteSampler(sampler);
        _samplers.Clear();
    }
}
