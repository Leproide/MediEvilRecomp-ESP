// Codice originale di MediEvilRecomp (BlackLabelHQ), adattato alla versione PAL italiana.
// MediEvilRecomp non ha una licenza esplicita: i diritti su questo codice restano ai suoi
// autori. Questo file NON e' rilasciato sotto GPL-3.0: la GPL del fork copre solo i file
// scritti da zero per il port (vedi la sezione Licenza del README).
// Indirizzi e costanti PAL: https://github.com/Leproide
//
// Correzioni PAL ITA (https://github.com/Leproide):
//  - inquadratura che tremava: se in un frame il gioco non ricalcola la telecamera, la
//    patch trovava ancora la propria matrice/posizione dell'aggiornamento prima e ci
//    applicava sopra di nuovo la rotazione (scatto di un frame). Ora la patch ricorda
//    i valori scritti e in quel caso riparte dai valori originali del gioco;
//  - avvicinamento ai muri e alle recinzioni: il controllo di collisione che alternava
//    "colpito/libero" faceva entrare e uscire la telecamera a ogni frame. Ora entra
//    in fretta ma senza scatti ed esce solo dopo qualche frame senza ostacoli.

using RecompOne.Runtime;
using RecompOne.Runtime.Context;
using RecompOne.Runtime.Hardware;
using RecompOne.Runtime.Memory;

namespace Recompiled;

public static class CameraControlPatch
{
    private const uint MainViewport = Addr.GpMainViewPort;
    private const uint CameraData = Addr.GpCamera;
    private const uint ViewportCamera = 0x7C;
    private const uint FocusPos = 0x28;
    private const uint FocalOffset = 0x38;
    private const uint FrameMatrix = 0x0C;
    private const uint FramePosition = 0x20;
    private const uint FrameYaw = 0x34;
    private const uint ActivePlugin = Addr.GpCameraPlugin;

    private const int PluginLookWithDan = 16;
    private const int PluginIntermission = 20;
    private const int PluginDeath = 26;

    private const uint PointOccluded = Addr.FnEntLibPointOccluded;
    private const uint OccludeExtensive = 1u << 0;
    private const uint OccludeCamera = 1u << 3;
    private const uint ScratchBelowStack = 0x200;
    private const float WallMargin = 48f;
    private const float MinDistance = 96f;

    private const float FrameRate = 25f; // PAL: telecamera aggiornata 25 volte al secondo (USA 30)
    private const float YawSpeed = 2.6f;
    private const float PitchSpeed = 1.6f;
    private const float MaxPitch = 0.55f;
    private const float StickDeadzone = 0.18f;
    private const float WalkThreshold = 0.35f;
    private const float RecenterDelay = 1.5f;
    private const float RecenterRate = 0.08f;
    private const float CutDistance = 2500f;
    private const float AngleUnits = 4096f;

    private const float SettledAngle = 0.01f;

    private static float _yaw;
    private static float _pitch;
    private static float _heading;
    private static bool _free;
    private static float _reach = 1f;
    private const float ReachRecover = 0.12f;
    private const float ReachApproach = 0.45f;
    private const int ReachHoldFrames = 8;
    private static int _reachHold;

    // ultimo aggiornamento: valori originali del gioco e valori scritti dalla patch
    private static bool _wrote;
    private static readonly short[] _baseMatrix = new short[9];
    private static readonly short[] _wroteMatrix = new short[9];
    private static int _baseX, _baseY, _baseZ, _baseYaw;
    private static int _wroteX, _wroteY, _wroteZ, _wroteYaw;
    private static readonly bool DiagOn = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("MEDIEVIL_DEBUG"));
    private static int _staleLogs;
    private static float _idle;
    private static bool _hasLast;
    private static float _lastX, _lastY, _lastZ;

    public static void AfterCameraUpdate(CpuContext c, IMemory m)
    {
        if (!QualityOfLife.CameraControl)
        {
            Release();
            _hasLast = false;
            return;
        }

        var viewport = m.ReadU32(c.GP + MainViewport);
        var camera = m.ReadU32(c.GP + CameraData);
        if (!IsRam(viewport) || !IsRam(camera)) return;
        if (Scripted(c, m))
        {
            Release();
            return;
        }

        var frame = m.ReadU32(viewport + ViewportCamera);
        if (!IsRam(frame)) return;

        // valori del gioco; se il gioco non ha ricalcolato la telecamera ci sono ancora
        // quelli scritti da noi: si riparte dagli originali salvati
        Span<short> gameMatrix = stackalloc short[9];
        for (var i = 0; i < 9; i++) gameMatrix[i] = (short)m.ReadU16(frame + FrameMatrix + (uint)(i * 2));
        var rawX = (int)m.ReadU32(frame + FramePosition);
        var rawY = (int)m.ReadU32(frame + FramePosition + 4);
        var rawZ = (int)m.ReadU32(frame + FramePosition + 8);
        var rawYaw = (int)m.ReadU32(frame + FrameYaw);
        if (_wrote && rawX == _wroteX && rawY == _wroteY && rawZ == _wroteZ && rawYaw == _wroteYaw &&
            gameMatrix.SequenceEqual((ReadOnlySpan<short>)_wroteMatrix))
        {
            if (DiagOn && _staleLogs++ < 40) Console.WriteLine("[CameraControl] telecamera non ricalcolata dal gioco: uso i valori originali");
            _baseMatrix.CopyTo(gameMatrix);
            rawX = _baseX;
            rawY = _baseY;
            rawZ = _baseZ;
            rawYaw = _baseYaw;
        }

        _wrote = false;
        gameMatrix.CopyTo(_baseMatrix);
        _baseX = rawX;
        _baseY = rawY;
        _baseZ = rawZ;
        _baseYaw = rawYaw;

        var px = (float)rawX;
        var py = (float)rawY;
        var pz = (float)rawZ;
        if (_hasLast && Distance(px - _lastX, py - _lastY, pz - _lastZ) > CutDistance) Release();
        _lastX = px;
        _lastY = py;
        _lastZ = pz;
        _hasLast = true;

        var gameYaw = (rawYaw >> 16) * (MathF.PI * 2f) / AngleUnits;
        Steer(gameYaw);
        if (MathF.Abs(_yaw) < 1e-4f && MathF.Abs(_pitch) < 1e-4f) return;

        var fx = (float)(int)m.ReadU32(camera + FocusPos) + (int)m.ReadU32(camera + FocalOffset);
        var fy = (float)(int)m.ReadU32(camera + FocusPos + 4) + (int)m.ReadU32(camera + FocalOffset + 4);
        var fz = (float)(int)m.ReadU32(camera + FocusPos + 8) + (int)m.ReadU32(camera + FocalOffset + 8);

        Span<float> matrix = stackalloc float[9];
        for (var i = 0; i < 9; i++) matrix[i] = gameMatrix[i] / 4096f;

        Span<float> yaw = stackalloc float[9];
        AxisRotation(0f, 1f, 0f, _yaw, yaw);
        Span<float> rotated = stackalloc float[9];
        Multiply(yaw, matrix, rotated);
        var (ox, oy, oz) = Apply(yaw, px - fx, py - fy, pz - fz);

        var rx = rotated[0];
        var ry = rotated[3];
        var rz = rotated[6];
        var length = Distance(rx, ry, rz);
        if (length > 1e-4f)
        {
            Span<float> pitch = stackalloc float[9];
            AxisRotation(rx / length, ry / length, rz / length, _pitch, pitch);
            Span<float> tilted = stackalloc float[9];
            Multiply(pitch, rotated, tilted);
            tilted.CopyTo(rotated);
            (ox, oy, oz) = Apply(pitch, ox, oy, oz);
        }

        (ox, oy, oz) = Collide(c, m, fx, fy, fz, px - fx, py - fy, pz - fz, ox, oy, oz);

        for (var i = 0; i < 9; i++)
        {
            _wroteMatrix[i] = (short)Math.Clamp(MathF.Round(rotated[i] * 4096f), -32768f, 32767f);
            m.WriteU16(frame + FrameMatrix + (uint)(i * 2), (ushort)_wroteMatrix[i]);
        }

        _wroteX = (int)MathF.Round(fx + ox);
        _wroteY = (int)MathF.Round(fy + oy);
        _wroteZ = (int)MathF.Round(fz + oz);
        m.WriteU32(frame + FramePosition, (uint)_wroteX);
        m.WriteU32(frame + FramePosition + 4, (uint)_wroteY);
        m.WriteU32(frame + FramePosition + 8, (uint)_wroteZ);

        var turn = (int)MathF.Round(_yaw / (MathF.PI * 2f) * AngleUnits);
        _wroteYaw = rawYaw + (turn << 16);
        m.WriteU32(frame + FrameYaw, (uint)_wroteYaw);
        _wrote = true;
    }

    private static bool Scripted(CpuContext c, IMemory m)
    {
        var plugin = m.ReadU32(c.GP + ActivePlugin);
        if (!IsRam(plugin)) return true;
        var type = (int)m.ReadU32(plugin);
        return type is PluginIntermission or PluginDeath or PluginLookWithDan;
    }

    private static (float X, float Y, float Z) Collide(CpuContext c, IMemory m, float fx, float fy, float fz,
        float gx, float gy, float gz, float ox, float oy, float oz)
    {
        var full = Distance(ox, oy, oz);
        var target = 1f;
        if (full >= 1f && !Trace(c, m, fx, fy, fz, gx, gy, gz, out _))
        {
            if (Trace(c, m, fx, fy, fz, ox, oy, oz, out var reach))
                target = Math.Clamp(reach - WallMargin, MathF.Min(MinDistance, full), full) / full;
        }

        // verso il personaggio: veloce ma graduale; all'indietro solo dopo qualche frame
        // libero, cosi' un ostacolo che compare e sparisce (recinzioni) non fa pompare
        if (target < _reach - 0.002f)
        {
            _reach += (target - _reach) * ReachApproach;
            _reachHold = ReachHoldFrames;
        }
        else if (_reachHold > 0) _reachHold--;
        else _reach += (target - _reach) * ReachRecover;
        return (ox * _reach, oy * _reach, oz * _reach);
    }

    private static bool Trace(CpuContext c, IMemory m, float fx, float fy, float fz, float dx, float dy, float dz,
        out float reach)
    {
        var scratch = (c.SP - ScratchBelowStack) & ~0xFu;
        var from = scratch;
        var to = scratch + 0x10;
        var clip = scratch + 0x20;
        WriteVec(m, from, fx, fy, fz);
        WriteVec(m, to, fx + dx, fy + dy, fz + dz);
        WriteVec(m, clip, fx + dx, fy + dy, fz + dz);

        Span<uint> registers = stackalloc uint[32];
        for (var i = 0; i < 32; i++) registers[i] = c[i];
        var hi = c.HI;
        var lo = c.LO;
        Span<uint> data = stackalloc uint[32];
        Span<uint> control = stackalloc uint[32];
        for (var i = 0; i < 32; i++)
        {
            data[i] = Gte.Read(i);
            control[i] = Gte.ReadControl(i);
        }

        c.A0 = from;
        c.A1 = to;
        c.A2 = clip;
        c.A3 = OccludeExtensive | OccludeCamera;
        c.SP = scratch - 0x40;
        c.RA = 0;
        MediEvil_game.func_800494C8(c, m);
        var hit = c.V0 != 0;

        for (var i = 1; i < 32; i++) c[i] = registers[i];
        c.HI = hi;
        c.LO = lo;
        for (var i = 0; i < 32; i++) Gte.WriteControl(i, control[i]);
        Gte.Write(15, data[15]);
        Gte.Write(28, data[28]);
        Gte.Write(30, data[30]);
        for (var i = 0; i < 32; i++)
            if (i is not (15 or 28 or 29 or 30 or 31))
                Gte.Write(i, data[i]);

        reach = hit
            ? Distance((int)m.ReadU32(clip) - fx, (int)m.ReadU32(clip + 4) - fy, (int)m.ReadU32(clip + 8) - fz)
            : Distance(dx, dy, dz);
        return hit;
    }

    private static void WriteVec(IMemory m, uint address, float x, float y, float z)
    {
        m.WriteU32(address, (uint)(int)MathF.Round(x));
        m.WriteU32(address + 4, (uint)(int)MathF.Round(y));
        m.WriteU32(address + 8, (uint)(int)MathF.Round(z));
        m.WriteU32(address + 12, 0);
    }

    private static void Release()
    {
        _reach = 1f;
        _reachHold = 0;
        _yaw = _pitch = 0f;
        _idle = 0f;
        _free = false;
    }

    private static void Steer(float gameYaw)
    {
        var x = Stick(Controller.RightX);
        var y = Stick(Controller.RightY);
        if (QualityOfLife.InvertCameraX) x = -x;
        if (QualityOfLife.InvertCameraY) y = -y;

        if (x != 0f || y != 0f)
        {
            if (!_free) _heading = gameYaw + _yaw;
            _free = true;
            _heading = Wrap(_heading + x * YawSpeed / FrameRate);
            _pitch = Math.Clamp(_pitch + y * PitchSpeed / FrameRate, -MaxPitch, MaxPitch);
            _idle = 0f;
        }
        else if (_free && QualityOfLife.CameraRecenter && Walking())
        {
            _idle += 1f / FrameRate;
            if (_idle >= RecenterDelay)
            {
                var back = Wrap(gameYaw - _heading);
                _heading = Wrap(_heading + back * RecenterRate);
                _pitch -= _pitch * RecenterRate;
                if (MathF.Abs(back) < SettledAngle && MathF.Abs(_pitch) < SettledAngle) Release();
            }
        }

        _yaw = _free ? Wrap(_heading - gameYaw) : 0f;
    }

    private static bool Walking()
    {
        return Distance((Controller.LeftX - 128) / 127f, (Controller.LeftY - 128) / 127f, 0f) > WalkThreshold;
    }

    private static float Stick(byte value)
    {
        var v = (value - 128) / 127f;
        if (MathF.Abs(v) < StickDeadzone) return 0f;
        return MathF.Sign(v) * (MathF.Abs(v) - StickDeadzone) / (1f - StickDeadzone);
    }

    private static float Wrap(float angle)
    {
        while (angle > MathF.PI) angle -= MathF.PI * 2f;
        while (angle < -MathF.PI) angle += MathF.PI * 2f;
        return angle;
    }
    //mtx word
    private static void AxisRotation(float x, float y, float z, float angle, Span<float> r)
    {
        var c = MathF.Cos(angle);
        var s = MathF.Sin(angle);
        var t = 1f - c;
        r[0] = t * x * x + c;
        r[1] = t * x * y - s * z;
        r[2] = t * x * z + s * y;
        r[3] = t * x * y + s * z;
        r[4] = t * y * y + c;
        r[5] = t * y * z - s * x;
        r[6] = t * x * z - s * y;
        r[7] = t * y * z + s * x;
        r[8] = t * z * z + c;
    }

    private static void Multiply(ReadOnlySpan<float> a, ReadOnlySpan<float> b, Span<float> result)
    {
        for (var row = 0; row < 3; row++)
        for (var col = 0; col < 3; col++)
            result[row * 3 + col] = a[row * 3] * b[col] + a[row * 3 + 1] * b[3 + col] + a[row * 3 + 2] * b[6 + col];
    }

    private static (float X, float Y, float Z) Apply(ReadOnlySpan<float> r, float x, float y, float z)
    {
        return (r[0] * x + r[1] * y + r[2] * z, r[3] * x + r[4] * y + r[5] * z, r[6] * x + r[7] * y + r[8] * z);
    }

    private static float Distance(float x, float y, float z)
    {
        return MathF.Sqrt(x * x + y * y + z * z);
    }

    private static bool IsRam(uint address)
    {
        return (address & 0xFF000000u) == 0x80000000u && (address & 0x00FFFFFFu) < 0x00200000u;
    }
}
