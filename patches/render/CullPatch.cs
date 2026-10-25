// PAL port: addresses/constants rewritten for the PAL release by
// medievil-pal-port (GPL-3.0-or-later), Author: https://github.com/Leproide
// Original code: MediEvilRecomp by BlackLabelHQ - their license terms apply to it.

using RecompOne.Runtime.Context;
using RecompOne.Runtime.Dispatch;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Memory;

namespace Recompiled;

//copied from the recompiled code to apply the correct culling
public static class CullPatch
{
    const uint RenderPolyPtr = Addr.FreeTerrData + 0x10u; // FreeTerrData.renderPoly
    const uint RenderPtrBase = 0x80310000u;

    const int StageWidth = 512;

    static int Widened => (int)MathF.Ceiling(StageWidth * Ratio);

    static float Ratio
    {
        get
        {
            float wide = Display.WideAspect;
            if (wide <= 0f) return 1f;

            float source = Display.SourceAspect > 0f ? Display.SourceAspect : 4f / 3f;
            return wide / source;
        }
    }

    static float _cachedWide = float.NaN;
    static float _cachedSource = float.NaN;
    static int _bias;
    static uint _maskX;

    static void Refresh()
    {
        float wide = Display.WideAspect;
        float source = Display.SourceAspect;
        if (wide == _cachedWide && source == _cachedSource) return;

        _cachedWide = wide;
        _cachedSource = source;

        int widened = Widened;
        _bias = (widened - StageWidth) / 2;

        uint bound = StageWidth;
        while (bound < (uint)widened) bound <<= 1;
        _maskX = ~(bound - 1u) & 0xFFFFu;
    }

    public static uint CullMaskX
    {
        get
        {
            Refresh();
            return _maskX;
        }
    }

    const uint VpDispW = 0x1F800098u;

    static int CullEdgeFor(int dispW)
    {
        Refresh();
        return (int)MathF.Round(dispW * (Ratio - 1f) * 0.5f);
    }

    static int CullEdgeX(PSMemory mem)
    {
        return CullEdgeFor((int)mem.ReadU16(VpDispW));
    }

    public static uint CullBias(uint packed)
    {
        Refresh();
        int x = (short)packed + _bias;
        return (packed & 0xFFFF0000u) | ((uint)x & 0xFFFFu);
    }

    public static void func_80021D5C(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        mem.WriteU32(RenderPolyPtr, RenderPtrBase);
        c.SP = c.SP - 0xB0u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
       
        mem.WriteU32((c.SP + 0x14u), c.S1);
       
        mem.WriteU32((c.SP + 0x18u), c.S2);
       
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
       
        mem.WriteU32((c.SP + 0x20u), c.S4);
       
        mem.WriteU32((c.SP + 0x24u), c.S5);
       
        mem.WriteU32((c.SP + 0x28u), c.S6);
       
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
       
        mem.WriteU32((c.SP + 0x30u), c.FP);
       
        c.V0 = 0x800E0000u;
        c.T0 = 0x1F800000u;
        c.T1 = 0x800E0000u;
        c.T4 = 0x800F0000u;
        c.A2 = 0x800E0000u;
        c.S0 = 0x800E0000u;
        c.S3 = 0x1F800000u;
        c.S5 = 0x1F800000u;
        c.V0 = c.V0 | RAddr.Game_80021DA4;
        c.T0 = c.T0 | 0x0004u;
        c.T1 = c.T1 | RAddr.Game_80021DAC;
        c.T4 = c.T4 | RAddr.Game_80021DB0;
        c.A2 = c.A2 | RAddr.Game_80021DB4;
        c.S0 = c.S0 | RAddr.Game_80021DB8;
        c.S3 = c.S3 | 0x008Cu;
        c.S5 = c.S5 | 0x009Cu;
        c.T0 = mem.ReadU32(c.T0);
        c.T1 = mem.ReadU32(c.T1);
        c.T4 = mem.ReadU32(c.T4);
        c.A2 = mem.ReadU32(c.A2);
        c.S0 = mem.ReadU32(c.S0);
        c.S3 = (uint)(short)mem.ReadU16(c.S3);
        c.S5 = mem.ReadU32(c.S5);
        c.T0 = c.T0 << 2;
        c.T0 = c.V0 + c.T0;
        c.A0 = mem.ReadU32((c.T0 + 0x4u));
        c.V1 = mem.ReadU32((c.T0 + 0x18u));
        c.S4 = mem.ReadU32((c.V0 + 0x2Cu));
        c.V1 = c.V1 - 0xD0u;
        c.T2 = mem.ReadU32((c.V0 + 0xCu));
        c.T3 = mem.ReadU32((c.V0 + 0x30u));
        c.S2 = 0u;
        mem.WriteU32((c.SP + 0x34u), c.T1);
       
        mem.WriteU32((c.SP + 0x40u), c.T4);
       
        mem.WriteU32((c.SP + 0x38u), c.T2);
       
        mem.WriteU32((c.SP + 0x3Cu), c.T3);
       
        c.S6 = 0x09000000u;
        c.S7 = 0x0C000000u;
        c.FP = 0x00FF0000u;
        c.FP = c.FP | 0xFFFFu;
        c.T6 = mem.ReadU32((c.V0 + 0x14u));
        c.T7 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x58u), c.T6);
       
        mem.WriteU32((c.SP + 0x5Cu), c.T7);
       
        L80021E34: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T0 = mem.ReadU32((c.SP + 0x3Cu));
        c.T1 = mem.ReadU32((c.SP + 0x38u));
        if ((int)c.T0 <= 0) {
            goto L80022AF8;
        }
        c.S1 = mem.ReadU16(c.T1);
        c.T0 = c.T0 - 0x1u;
        c.A3 = mem.ReadU32((c.T1 + 0x4u));
        c.T1 = c.T1 + 0x8u;
        mem.WriteU32((c.SP + 0x3Cu), c.T0);
       
        mem.WriteU32((c.SP + 0x38u), c.T1);
       
        L80021E5C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if ((int)c.S1 <= 0) {
            c.T0 = mem.ReadU16(c.A3);
            goto L80021E34;
        }
        c.T0 = mem.ReadU16(c.A3);
        c.S1 = c.S1 - 0x1u;
        c.T1 = c.T0 << 4;
        c.T2 = c.T0 << 2;
        c.A1 = c.S0 + c.T1;
        c.A1 = c.A1 + c.T2;
        c.T9 = mem.ReadU16((c.A1 + 0xAu));
        c.A3 = c.A3 + 0x2u;
        c.At = c.T9 & 0x0004u;
        if (c.At != 0u) {
            c.T0 = c.T9 & 0x0001u;
            goto L80021E5C;
        }
        c.T0 = c.T9 & 0x0001u;
        if (c.T0 != 0u) {
            goto L80022460;
        }
        c.T0 = (uint)(short)mem.ReadU16(c.A1);
        c.T1 = (uint)(short)mem.ReadU16((c.A1 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A1 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A2;
        c.T1 = c.T1 + c.A2;
        c.T2 = c.T2 + c.A2;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T2));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T2 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
       
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
       
        mem.WriteU32((c.SP + 0x48u), c.T1);
       
        RecompOne.Runtime.Gte.Nclip();
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022AF0;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            c.T0 = RecompOne.Runtime.Gte.Read(24);
           
            goto L80022AF0;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        c.T5 = 0xFF000000u;
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        c.T5 = c.T5 | CullMaskX;
        if ((int)c.T0 <= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80021E5C;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        c.T1 = RecompOne.Runtime.Gte.Read(12);
       
        c.T2 = RecompOne.Runtime.Gte.Read(13);
       
        c.T3 = RecompOne.Runtime.Gte.Read(14);
       
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T2) & c.T7;
        c.T7 = CullBias(c.T3) & c.T7;
        if (c.T7 != 0u) {
            goto L80021E5C;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
       
        mem.WriteU32((c.A0 + 0x14u), c.T2);
       
        mem.WriteU32((c.A0 + 0x20u), c.T3);
        TerrainFloatProjection.Store(12, c.A0 + 0x8u, c.T1);
        TerrainFloatProjection.Store(13, c.A0 + 0x14u, c.T2);
        TerrainFloatProjection.Store(14, c.A0 + 0x20u, c.T3);
       
        c.T0 = RecompOne.Runtime.Gte.Read(17);
       
        c.T1 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021F6C;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021F6C: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T1 < (int)c.T2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80021F84;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80021F84: ;
        c.T1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.At = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80021E5C;
        }
        c.At = c.T9 & 0x0018u;
        if (c.At == 0u) {
            c.T8 = c.T9 & 0x0010u;
            goto L80021FF0;
        }
        c.T8 = c.T9 & 0x0010u;
        if (c.T8 == 0u) {
            goto L80021FE8;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(17);
       
        c.T1 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021FC8;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021FC8: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80021FE0;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80021FE0: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L80022000;
        L80021FE8: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L80022000;
        L80021FF0: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.T0 = RecompOne.Runtime.Gte.Read(7);
       
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L80022000: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80022014;
        }
        c.T0 = c.T1 + 0u;
        L80022014: ;
        c.T0 = TerrainPatch.NearOtz(c.T0);
        c.T1 = (int)c.T0 < TerrainPatch.NearClip ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x5Cu));
            goto L80021E5C;
        }
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
       
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
       
        c.S2 = c.S2 + 0x1u;
        c.T6 = mem.ReadU16((c.A1 + 0x8u));
        c.T5 = mem.ReadU32((c.SP + 0x34u));
        c.T6 = c.T6 << 2;
        c.T5 = c.T5 + c.T6;
        c.T5 = mem.ReadU32(c.T5);
        c.T1 = mem.ReadU16((c.A1 + 0xCu));
        c.T2 = mem.ReadU16((c.A1 + 0xEu));
        c.T3 = mem.ReadU16((c.A1 + 0x10u));
        c.T7 = mem.ReadU16((c.T5 + 0xAu));
        c.T8 = mem.ReadU16((c.T5 + 0x6u));
        mem.WriteU16((c.A0 + 0xCu), (ushort)c.T1);
        mem.WriteU16((c.A0 + 0x18u), (ushort)c.T2);
        mem.WriteU16((c.A0 + 0x24u), (ushort)c.T3);
        mem.WriteU16((c.A0 + 0x1Au), (ushort)c.T7);
        mem.WriteU16((c.A0 + 0xEu), (ushort)c.T8);
        c.At = c.T9 & 0x0200u;
        if (c.At == 0u) {
            c.T1 = 0x34000000u;
            goto L80022084;
        }
        c.T1 = 0x34000000u;
        c.T1 = 0x36000000u;
        L80022084: ;
        c.T2 = mem.ReadU32((c.SP + 0x44u));
        c.T8 = mem.ReadU32((c.SP + 0x40u));
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.T2 = RecompOne.Runtime.Gte.Read(17);
       
        c.At = c.At | c.T1;
        c.T2 = (uint)((int)c.T2 >> 5);
        c.At = c.At | c.T3;
        c.T2 = c.T2 << 1;
        c.At = c.At | c.T4;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x48u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(18);
       
        mem.WriteU32((c.A0 + 0x4u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x4Cu));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        mem.WriteU32((c.A0 + 0x10u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        c.T3 = mem.ReadU32((c.SP + 0xA8u));
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        if ((int)c.T3 > 0) {
            c.T3 = (uint)((int)c.T3 >> 1);
            goto L80022168;
        }
        c.T3 = (uint)((int)c.T3 >> 1);
        c.T3 = 0u - c.T3;
        L80022168: ;
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        mem.WriteU32((c.SP + 0xACu), c.T0);
       
        c.T2 = (int)c.T3 < 2000 ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
            goto L80022180;
        }
        c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T9 = c.A0 & c.FP;
            goto L800221D0;
        }
        L80022180: ;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        goto L80021E5C;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        goto L80021E5C;
        L800221D0: ;
        c.T0 = c.A0 + 0x28u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        mem.WriteU32((c.SP + 0x98u), c.T0);
       
        mem.WriteU32((c.SP + 0x9Cu), c.T1);
       
        mem.WriteU32((c.SP + 0xA0u), c.T2);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0x8u));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x14u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x20u));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        c.T7 = 0x55550000u;
        c.T7 = c.T7 | 0x5556u;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0xAu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x16u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x22u));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T9 = c.T9 << 16;
        c.T6 = c.T6 & 0xFFFFu;
        c.T6 = c.T6 | c.T9;
        mem.WriteU32((c.T0 + 0x20u), c.T6);
       
        mem.WriteU32((c.T1 + 0x20u), c.T6);
       
        mem.WriteU32((c.T2 + 0x20u), c.T6);
       
        c.T3 = mem.ReadU8((c.A0 + 0x6u));
        c.T4 = mem.ReadU8((c.A0 + 0x12u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Eu));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = mem.ReadU8((c.A0 + 0x5u));
        c.T4 = mem.ReadU8((c.A0 + 0x11u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Du));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T3 = mem.ReadU8((c.A0 + 0x4u));
        c.T4 = mem.ReadU8((c.A0 + 0x10u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Cu));
        c.T3 = c.T3 + c.T4;
        c.T3 = c.T5 + c.T3;
        { var _r = (long)(int)c.T3 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T3 = (uint)((int)c.T3 >> 31);
        c.T8 = c.HI;
        c.T3 = c.T8 - c.T3;
        c.T3 = c.T3 & 0x00FFu;
        c.T6 = c.T6 & 0x00FFu;
        c.T9 = c.T9 & 0x00FFu;
        c.T6 = c.T6 << 16;
        c.T9 = c.T9 << 8;
        c.T6 = c.T6 | c.T9;
        c.T6 = c.T6 | c.T3;
        mem.WriteU32((c.T0 + 0x1Cu), c.T6);
       
        mem.WriteU32((c.T1 + 0x1Cu), c.T6);
       
        mem.WriteU32((c.T2 + 0x1Cu), c.T6);
       
        c.T3 = mem.ReadU8((c.A0 + 0xCu));
        c.T4 = mem.ReadU8((c.A0 + 0x18u));
        c.T5 = mem.ReadU8((c.A0 + 0x24u));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = mem.ReadU8((c.A0 + 0xDu));
        c.T4 = mem.ReadU8((c.A0 + 0x19u));
        c.T5 = mem.ReadU8((c.A0 + 0x25u));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T6 = c.T6 & 0x00FFu;
        c.T9 = c.T9 & 0x00FFu;
        c.T9 = c.T9 << 8;
        c.T6 = c.T6 | c.T9;
        mem.WriteU16((c.T0 + 0x24u), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0x24u), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x24u), (ushort)c.T6);
        c.T3 = mem.ReadU32((c.A0 + 0x8u));
        c.T4 = mem.ReadU32((c.A0 + 0x14u));
        c.T5 = mem.ReadU32((c.A0 + 0x4u));
        c.T6 = mem.ReadU16((c.A0 + 0x10u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x12u));
        c.T7 = c.T7 & 0xFF00u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T0 + 0x8u), c.T3);
       
        mem.WriteU32((c.T0 + 0x14u), c.T4);
       
        mem.WriteU32((c.T0 + 0x4u), c.T5);
       
        mem.WriteU32((c.T0 + 0x10u), c.T6);
       
        mem.WriteU32((c.T1 + 0x8u), c.T4);
       
        mem.WriteU32((c.T1 + 0x4u), c.T6);
       
        mem.WriteU32((c.T2 + 0x14u), c.T3);
       
        mem.WriteU32((c.T2 + 0x10u), c.T5);
       
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0xCu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x18u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x1Au));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0xEu));
        mem.WriteU16((c.T0 + 0xCu), (ushort)c.T3);
        mem.WriteU16((c.T0 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T0 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T0 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x18u), (ushort)c.T3);
        mem.WriteU16((c.T2 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.T6);
        c.T3 = mem.ReadU32((c.A0 + 0x20u));
        c.T4 = mem.ReadU16((c.A0 + 0x1Cu));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x1Eu));
        c.T7 = c.T7 & 0xFF00u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T4 = c.T7 | c.T4;
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x24u));
        mem.WriteU32((c.T1 + 0x14u), c.T3);
       
        mem.WriteU32((c.T1 + 0x10u), c.T4);
       
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T5);
        mem.WriteU32((c.T2 + 0x8u), c.T3);
       
        mem.WriteU32((c.T2 + 0x4u), c.T4);
       
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T5);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.A0 = c.T2 + 0x28u;
        goto L80021E5C;
        L80022460: ;
        c.T0 = (uint)(short)mem.ReadU16(c.A1);
        c.T1 = (uint)(short)mem.ReadU16((c.A1 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A1 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A1 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A2;
        c.T1 = c.T1 + c.A2;
        c.T2 = c.T2 + c.A2;
        c.T3 = c.T3 + c.A2;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T2));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T2 + 0x4u)));
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        TerrainFloatProjection.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        c.T3 = mem.ReadU16((c.T3 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
       
        mem.WriteU32((c.SP + 0x48u), c.T1);
       
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
       
        mem.WriteU32((c.SP + 0x50u), c.T3);
       
        RecompOne.Runtime.Gte.Nclip();
        RecompOne.Runtime.Gte.Write(0, c.T4);
        RecompOne.Runtime.Gte.Write(1, c.T5);
        c.T5 = 0xFF000000u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = (c.SP + 0x54u); mem.WriteU32(_ad, _sw); }
        c.T5 = c.T5 | CullMaskX;
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        TerrainFloatProjection.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022AF0;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80022AF0;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        if ((int)c.T0 > 0) {
            c.At = c.T9 & 0x0018u;
            goto L80022528;
        }
        c.At = c.T9 & 0x0018u;
        RecompOne.Runtime.Gte.Nclip();
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        if ((int)c.T0 >= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80021E5C;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        L80022528: ;
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.T3 = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80022544;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80022544: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.T3 = (int)c.T0 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T1 = c.T2 + 0u;
            goto L8002255C;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L8002255C: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.T3 = (int)c.T1 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80022574;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80022574: ;
        c.T2 = c.T0 + 0u;
        c.T1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.T0 = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.T0 == 0u) {
            goto L80021E5C;
        }
        if (c.At == 0u) {
            c.T8 = c.T9 & 0x0010u;
            goto L800225FC;
        }
        c.T8 = c.T9 & 0x0010u;
        if (c.T8 == 0u) {
            goto L800225F0;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L800225B8;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L800225B8: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T2 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T1 = c.T2 + 0u;
            goto L800225D0;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L800225D0: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L800225E8;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L800225E8: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002260C;
        L800225F0: ;
        c.T0 = c.T2 + 0u;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002260C;
        L800225FC: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T0 = RecompOne.Runtime.Gte.Read(7);
       
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L8002260C: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80022620;
        }
        c.T0 = c.T1 + 0u;
        L80022620: ;
        c.T0 = TerrainPatch.NearOtz(c.T0);
        c.T1 = (int)c.T0 < TerrainPatch.NearClip ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x54u));
            goto L80021E5C;
        }
        c.T1 = mem.ReadU32((c.SP + 0x54u));
        c.T3 = RecompOne.Runtime.Gte.Read(12);
       
        c.T4 = RecompOne.Runtime.Gte.Read(13);
       
        c.T6 = RecompOne.Runtime.Gte.Read(14);
       
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T3) & c.T7;
        c.T7 = CullBias(c.T4) & c.T7;
        c.T7 = CullBias(c.T6) & c.T7;
        if (c.T7 != 0u) {
            goto L80021E5C;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
       
        mem.WriteU32((c.A0 + 0x14u), c.T3);
       
        mem.WriteU32((c.A0 + 0x20u), c.T4);
       
        mem.WriteU32((c.A0 + 0x2Cu), c.T6);
        TerrainFloatProjection.StoreFirst(c.A0 + 0x8u, c.T1);
        TerrainFloatProjection.Store(12, c.A0 + 0x14u, c.T3);
        TerrainFloatProjection.Store(13, c.A0 + 0x20u, c.T4);
        TerrainFloatProjection.Store(14, c.A0 + 0x2Cu, c.T6);
       
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
       
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
       
        c.S2 = c.S2 + 0x1u;
        c.T6 = mem.ReadU16((c.A1 + 0x8u));
        c.T5 = mem.ReadU32((c.SP + 0x34u));
        c.T6 = c.T6 << 2;
        c.T5 = c.T5 + c.T6;
        c.T5 = mem.ReadU32(c.T5);
        c.T1 = mem.ReadU16((c.A1 + 0xCu));
        c.T2 = mem.ReadU16((c.A1 + 0xEu));
        c.T3 = mem.ReadU16((c.A1 + 0x10u));
        c.T4 = mem.ReadU16((c.A1 + 0x12u));
        mem.WriteU16((c.A0 + 0xCu), (ushort)c.T1);
        mem.WriteU16((c.A0 + 0x18u), (ushort)c.T2);
        mem.WriteU16((c.A0 + 0x24u), (ushort)c.T3);
        mem.WriteU16((c.A0 + 0x30u), (ushort)c.T4);
        c.T7 = mem.ReadU16((c.T5 + 0xAu));
        c.T8 = mem.ReadU16((c.T5 + 0x6u));
        mem.WriteU16((c.A0 + 0x1Au), (ushort)c.T7);
        mem.WriteU16((c.A0 + 0xEu), (ushort)c.T8);
        c.At = c.T9 & 0x0200u;
        if (c.At == 0u) {
            c.T1 = 0x3C000000u;
            goto L800226D0;
        }
        c.T1 = 0x3C000000u;
        c.T1 = 0x3E000000u;
        L800226D0: ;
        c.T2 = mem.ReadU32((c.SP + 0x44u));
        c.T8 = mem.ReadU32((c.SP + 0x40u));
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.T2 = RecompOne.Runtime.Gte.Read(16);
       
        c.At = c.At | c.T1;
        c.T2 = (uint)((int)c.T2 >> 5);
        c.At = c.At | c.T3;
        c.T2 = c.T2 << 1;
        c.At = c.At | c.T4;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x48u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(17);
       
        mem.WriteU32((c.A0 + 0x4u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x4Cu));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(18);
       
        mem.WriteU32((c.A0 + 0x10u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x50u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        c.T3 = mem.ReadU32((c.SP + 0xA8u));
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        if ((int)c.T3 > 0) {
            c.T3 = (uint)((int)c.T3 >> 1);
            goto L800227FC;
        }
        c.T3 = (uint)((int)c.T3 >> 1);
        c.T3 = 0u - c.T3;
        L800227FC: ;
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        mem.WriteU32((c.SP + 0xACu), c.T0);
       
        c.T2 = (int)c.T3 < 1000 ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
            goto L80022814;
        }
        c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T9 = c.A0 & c.FP;
            goto L8002283C;
        }
        L80022814: ;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x34u;
        goto L80021E5C;
        L8002283C: ;
        c.T0 = c.A0 + 0x34u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        c.T3 = c.T2 + 0x28u;
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x8u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x14u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x20u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Cu));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0xAu));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x16u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x22u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Eu));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 16;
        c.T8 = c.T8 & 0xFFFFu;
        c.T8 = c.T8 | c.T9;
        mem.WriteU32((c.T0 + 0x20u), c.T8);
       
        mem.WriteU32((c.T1 + 0x20u), c.T8);
       
        mem.WriteU32((c.T2 + 0x20u), c.T8);
       
        mem.WriteU32((c.T3 + 0x20u), c.T8);
       
        c.T4 = mem.ReadU8((c.A0 + 0x12u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Eu));
        c.T6 = mem.ReadU8((c.A0 + 0x11u));
        c.T7 = mem.ReadU8((c.A0 + 0x1Du));
        c.T8 = c.T4 + c.T5;
        c.T8 = (uint)((int)c.T8 >> 1);
        c.T4 = mem.ReadU8((c.A0 + 0x10u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Cu));
        c.T9 = c.T6 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 1);
        c.T4 = c.T4 + c.T5;
        c.T4 = (uint)((int)c.T4 >> 1);
        c.T8 = c.T8 << 16;
        c.T9 = c.T9 << 8;
        c.T4 = c.T4 | c.T8;
        c.T4 = c.T4 | c.T9;
        mem.WriteU32((c.T0 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T1 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T2 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T3 + 0x1Cu), c.T4);
       
        c.T4 = mem.ReadU8((c.A0 + 0xCu));
        c.T5 = mem.ReadU8((c.A0 + 0x18u));
        c.T6 = mem.ReadU8((c.A0 + 0x24u));
        c.T7 = mem.ReadU8((c.A0 + 0x30u));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = mem.ReadU8((c.A0 + 0xDu));
        c.T5 = mem.ReadU8((c.A0 + 0x19u));
        c.T6 = mem.ReadU8((c.A0 + 0x25u));
        c.T7 = mem.ReadU8((c.A0 + 0x31u));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 8;
        c.T8 = c.T8 | c.T9;
        mem.WriteU16((c.T0 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T1 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T2 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T3 + 0x24u), (ushort)c.T8);
        c.T9 = mem.ReadU32((c.A0 + 0x8u));
        c.T4 = mem.ReadU32((c.A0 + 0x14u));
        c.T5 = mem.ReadU16((c.A0 + 0x4u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T5 = c.T7 | c.T5;
        c.T6 = mem.ReadU16((c.A0 + 0x10u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x12u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T0 + 0x8u), c.T9);
       
        mem.WriteU32((c.T0 + 0x14u), c.T4);
       
        mem.WriteU32((c.T0 + 0x4u), c.T5);
       
        mem.WriteU32((c.T0 + 0x10u), c.T6);
       
        mem.WriteU32((c.T1 + 0x8u), c.T4);
       
        mem.WriteU32((c.T1 + 0x4u), c.T6);
       
        mem.WriteU32((c.T3 + 0x14u), c.T9);
       
        mem.WriteU32((c.T3 + 0x10u), c.T5);
       
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0xCu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x18u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x1Au));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0xEu));
        mem.WriteU16((c.T0 + 0xCu), (ushort)c.T9);
        mem.WriteU16((c.T0 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T0 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T0 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T3 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T3 + 0xEu), (ushort)c.T6);
        c.T9 = mem.ReadU32((c.A0 + 0x20u));
        c.T4 = mem.ReadU16((c.A0 + 0x1Cu));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x1Eu));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T4 = c.T7 | c.T4;
        c.T5 = mem.ReadU32((c.A0 + 0x2Cu));
        c.T6 = mem.ReadU16((c.A0 + 0x28u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x2Au));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T1 + 0x14u), c.T5);
       
        mem.WriteU32((c.T1 + 0x10u), c.T6);
       
        mem.WriteU32((c.T2 + 0x8u), c.T5);
       
        mem.WriteU32((c.T2 + 0x14u), c.T9);
       
        mem.WriteU32((c.T2 + 0x4u), c.T6);
       
        mem.WriteU32((c.T2 + 0x10u), c.T4);
       
        mem.WriteU32((c.T3 + 0x4u), c.T4);
       
        mem.WriteU32((c.T3 + 0x8u), c.T9);
       
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x30u));
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0x24u));
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0xCu), (ushort)c.T9);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T3 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.A0 = c.T3 + 0x28u;
        goto L80021E5C;
        L80022AF0: ;
        L80022AF8: ;
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        mem.WriteU32((c.V0 + 0x34u), c.S2);
       
        L80022B00: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if ((int)c.S2 <= 0) {
            goto L80022B28;
        }
        c.T1 = c.T1 - 0x4u;
        c.T0 = mem.ReadU32(c.T1);
        c.S2 = c.S2 - 0x1u;
        c.T9 = (uint)(short)mem.ReadU16((c.T0 + 0xAu));
        c.T9 = c.T9 & 0xFFFBu;
        mem.WriteU16((c.T0 + 0xAu), (ushort)c.T9);
        goto L80022B00;
        L80022B28: ;
        mem.WriteU32((c.V0 + 0x10u), c.T1);
       
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0xB0u;
        return;
    }

    public static void func_80021E5C(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        L80021E5C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if ((int)c.S1 <= 0) {
            c.T0 = mem.ReadU16(c.A3);
            MediEvil_game.func_80021E34(c, m);
            return;
        }
        c.T0 = mem.ReadU16(c.A3);
        c.S1 = c.S1 - 0x1u;
        c.T1 = c.T0 << 4;
        c.T2 = c.T0 << 2;
        c.A1 = c.S0 + c.T1;
        c.A1 = c.A1 + c.T2;
        c.T9 = mem.ReadU16((c.A1 + 0xAu));
        c.A3 = c.A3 + 0x2u;
        c.At = c.T9 & 0x0004u;
        if (c.At != 0u) {
            c.T0 = c.T9 & 0x0001u;
            goto L80021E5C;
        }
        c.T0 = c.T9 & 0x0001u;
        if (c.T0 != 0u) {
            MediEvil_game.func_80022460(c, m);
            return;
        }
        c.T0 = (uint)(short)mem.ReadU16(c.A1);
        c.T1 = (uint)(short)mem.ReadU16((c.A1 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A1 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A2;
        c.T1 = c.T1 + c.A2;
        c.T2 = c.T2 + c.A2;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T2));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T2 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
       
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
       
        mem.WriteU32((c.SP + 0x48u), c.T1);
       
        RecompOne.Runtime.Gte.Nclip();
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            MediEvil_game.func_80022AF0(c, m);
            return;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            c.T0 = RecompOne.Runtime.Gte.Read(24);
           
            MediEvil_game.func_80022AF0(c, m);
            return;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        c.T5 = 0xFF000000u;
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        c.T5 = c.T5 | CullMaskX;
        if ((int)c.T0 <= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80021E5C;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        c.T1 = RecompOne.Runtime.Gte.Read(12);
       
        c.T2 = RecompOne.Runtime.Gte.Read(13);
       
        c.T3 = RecompOne.Runtime.Gte.Read(14);
       
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T2) & c.T7;
        c.T7 = CullBias(c.T3) & c.T7;
        if (c.T7 != 0u) {
            goto L80021E5C;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
       
        mem.WriteU32((c.A0 + 0x14u), c.T2);
       
        mem.WriteU32((c.A0 + 0x20u), c.T3);
        TerrainFloatProjection.Store(12, c.A0 + 0x8u, c.T1);
        TerrainFloatProjection.Store(13, c.A0 + 0x14u, c.T2);
        TerrainFloatProjection.Store(14, c.A0 + 0x20u, c.T3);
       
        c.T0 = RecompOne.Runtime.Gte.Read(17);
       
        c.T1 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021F6C;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021F6C: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T1 < (int)c.T2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80021F84;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80021F84: ;
        c.T1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.At = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80021E5C;
        }
        c.At = c.T9 & 0x0018u;
        if (c.At == 0u) {
            c.T8 = c.T9 & 0x0010u;
            goto L80021FF0;
        }
        c.T8 = c.T9 & 0x0010u;
        if (c.T8 == 0u) {
            goto L80021FE8;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(17);
       
        c.T1 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80021FC8;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80021FC8: ;
        c.T1 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80021FE0;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80021FE0: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L80022000;
        L80021FE8: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L80022000;
        L80021FF0: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.T0 = RecompOne.Runtime.Gte.Read(7);
       
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L80022000: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80022014;
        }
        c.T0 = c.T1 + 0u;
        L80022014: ;
        c.T0 = TerrainPatch.NearOtz(c.T0);
        c.T1 = (int)c.T0 < TerrainPatch.NearClip ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x5Cu));
            goto L80021E5C;
        }
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
       
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
       
        c.S2 = c.S2 + 0x1u;
        c.T6 = mem.ReadU16((c.A1 + 0x8u));
        c.T5 = mem.ReadU32((c.SP + 0x34u));
        c.T6 = c.T6 << 2;
        c.T5 = c.T5 + c.T6;
        c.T5 = mem.ReadU32(c.T5);
        c.T1 = mem.ReadU16((c.A1 + 0xCu));
        c.T2 = mem.ReadU16((c.A1 + 0xEu));
        c.T3 = mem.ReadU16((c.A1 + 0x10u));
        c.T7 = mem.ReadU16((c.T5 + 0xAu));
        c.T8 = mem.ReadU16((c.T5 + 0x6u));
        mem.WriteU16((c.A0 + 0xCu), (ushort)c.T1);
        mem.WriteU16((c.A0 + 0x18u), (ushort)c.T2);
        mem.WriteU16((c.A0 + 0x24u), (ushort)c.T3);
        mem.WriteU16((c.A0 + 0x1Au), (ushort)c.T7);
        MediEvil_game.func_80022070(c, m);
    }

    public static void func_80022070(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        mem.WriteU16((c.A0 + 0xEu), (ushort)c.T8);
        c.At = c.T9 & 0x0200u;
        if (c.At == 0u) {
            c.T1 = 0x34000000u;
            goto L80022084;
        }
        c.T1 = 0x34000000u;
        c.T1 = 0x36000000u;
        L80022084: ;
        c.T2 = mem.ReadU32((c.SP + 0x44u));
        c.T8 = mem.ReadU32((c.SP + 0x40u));
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.T2 = RecompOne.Runtime.Gte.Read(17);
       
        c.At = c.At | c.T1;
        c.T2 = (uint)((int)c.T2 >> 5);
        c.At = c.At | c.T3;
        c.T2 = c.T2 << 1;
        c.At = c.At | c.T4;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x48u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(18);
       
        mem.WriteU32((c.A0 + 0x4u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x4Cu));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        mem.WriteU32((c.A0 + 0x10u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        c.T3 = mem.ReadU32((c.SP + 0xA8u));
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        if ((int)c.T3 > 0) {
            c.T3 = (uint)((int)c.T3 >> 1);
            goto L80022168;
        }
        c.T3 = (uint)((int)c.T3 >> 1);
        c.T3 = 0u - c.T3;
        L80022168: ;
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        mem.WriteU32((c.SP + 0xACu), c.T0);
       
        c.T2 = (int)c.T3 < 2000 ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
            goto L80022180;
        }
        c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T9 = c.A0 & c.FP;
            goto L800221D0;
        }
        L80022180: ;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        MediEvil_game.func_80021E5C(c, m);
        return;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x28u;
        MediEvil_game.func_80021E5C(c, m);
        return;
        L800221D0: ;
        c.T0 = c.A0 + 0x28u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        mem.WriteU32((c.SP + 0x98u), c.T0);
       
        mem.WriteU32((c.SP + 0x9Cu), c.T1);
       
        mem.WriteU32((c.SP + 0xA0u), c.T2);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0x8u));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x14u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x20u));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        c.T7 = 0x55550000u;
        c.T7 = c.T7 | 0x5556u;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0xAu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x16u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x22u));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T9 = c.T9 << 16;
        c.T6 = c.T6 & 0xFFFFu;
        c.T6 = c.T6 | c.T9;
        mem.WriteU32((c.T0 + 0x20u), c.T6);
       
        mem.WriteU32((c.T1 + 0x20u), c.T6);
       
        mem.WriteU32((c.T2 + 0x20u), c.T6);
       
        c.T3 = mem.ReadU8((c.A0 + 0x6u));
        c.T4 = mem.ReadU8((c.A0 + 0x12u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Eu));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = mem.ReadU8((c.A0 + 0x5u));
        c.T4 = mem.ReadU8((c.A0 + 0x11u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Du));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T3 = mem.ReadU8((c.A0 + 0x4u));
        c.T4 = mem.ReadU8((c.A0 + 0x10u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Cu));
        c.T3 = c.T3 + c.T4;
        c.T3 = c.T5 + c.T3;
        { var _r = (long)(int)c.T3 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T3 = (uint)((int)c.T3 >> 31);
        c.T8 = c.HI;
        c.T3 = c.T8 - c.T3;
        c.T3 = c.T3 & 0x00FFu;
        c.T6 = c.T6 & 0x00FFu;
        c.T9 = c.T9 & 0x00FFu;
        c.T6 = c.T6 << 16;
        c.T9 = c.T9 << 8;
        c.T6 = c.T6 | c.T9;
        c.T6 = c.T6 | c.T3;
        mem.WriteU32((c.T0 + 0x1Cu), c.T6);
       
        mem.WriteU32((c.T1 + 0x1Cu), c.T6);
       
        mem.WriteU32((c.T2 + 0x1Cu), c.T6);
       
        c.T3 = mem.ReadU8((c.A0 + 0xCu));
        c.T4 = mem.ReadU8((c.A0 + 0x18u));
        c.T5 = mem.ReadU8((c.A0 + 0x24u));
        c.T6 = c.T3 + c.T4;
        c.T6 = c.T5 + c.T6;
        { var _r = (long)(int)c.T6 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T6 = (uint)((int)c.T6 >> 31);
        c.T8 = c.HI;
        c.T6 = c.T8 - c.T6;
        c.T3 = mem.ReadU8((c.A0 + 0xDu));
        c.T4 = mem.ReadU8((c.A0 + 0x19u));
        c.T5 = mem.ReadU8((c.A0 + 0x25u));
        c.T9 = c.T3 + c.T4;
        c.T9 = c.T5 + c.T9;
        { var _r = (long)(int)c.T9 * (int)c.T7; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = (uint)((int)c.T9 >> 31);
        c.T8 = c.HI;
        c.T9 = c.T8 - c.T9;
        c.T6 = c.T6 & 0x00FFu;
        c.T9 = c.T9 & 0x00FFu;
        c.T9 = c.T9 << 8;
        c.T6 = c.T6 | c.T9;
        mem.WriteU16((c.T0 + 0x24u), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0x24u), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x24u), (ushort)c.T6);
        c.T3 = mem.ReadU32((c.A0 + 0x8u));
        c.T4 = mem.ReadU32((c.A0 + 0x14u));
        c.T5 = mem.ReadU32((c.A0 + 0x4u));
        c.T6 = mem.ReadU16((c.A0 + 0x10u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x12u));
        c.T7 = c.T7 & 0xFF00u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T0 + 0x8u), c.T3);
       
        mem.WriteU32((c.T0 + 0x14u), c.T4);
       
        mem.WriteU32((c.T0 + 0x4u), c.T5);
       
        mem.WriteU32((c.T0 + 0x10u), c.T6);
       
        mem.WriteU32((c.T1 + 0x8u), c.T4);
       
        mem.WriteU32((c.T1 + 0x4u), c.T6);
       
        mem.WriteU32((c.T2 + 0x14u), c.T3);
       
        mem.WriteU32((c.T2 + 0x10u), c.T5);
       
        c.T3 = (uint)(short)mem.ReadU16((c.A0 + 0xCu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x18u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x1Au));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0xEu));
        mem.WriteU16((c.T0 + 0xCu), (ushort)c.T3);
        mem.WriteU16((c.T0 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T0 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T0 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x18u), (ushort)c.T3);
        mem.WriteU16((c.T2 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.T6);
        c.T3 = mem.ReadU32((c.A0 + 0x20u));
        c.T4 = mem.ReadU16((c.A0 + 0x1Cu));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x1Eu));
        c.T7 = c.T7 & 0xFF00u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T4 = c.T7 | c.T4;
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x24u));
        mem.WriteU32((c.T1 + 0x14u), c.T3);
       
        mem.WriteU32((c.T1 + 0x10u), c.T4);
       
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T5);
        mem.WriteU32((c.T2 + 0x8u), c.T3);
       
        mem.WriteU32((c.T2 + 0x4u), c.T4);
       
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T5);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.A0 = c.T2 + 0x28u;
        MediEvil_game.func_80021E5C(c, m);
        return;
        c.T0 = (uint)(short)mem.ReadU16(c.A1);
        c.T1 = (uint)(short)mem.ReadU16((c.A1 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A1 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A1 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A2;
        c.T1 = c.T1 + c.A2;
        c.T2 = c.T2 + c.A2;
        c.T3 = c.T3 + c.A2;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T2));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T2 + 0x4u)));
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        TerrainFloatProjection.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        c.T3 = mem.ReadU16((c.T3 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
       
        mem.WriteU32((c.SP + 0x48u), c.T1);
       
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
       
        mem.WriteU32((c.SP + 0x50u), c.T3);
       
        RecompOne.Runtime.Gte.Nclip();
        RecompOne.Runtime.Gte.Write(0, c.T4);
        RecompOne.Runtime.Gte.Write(1, c.T5);
        c.T5 = 0xFF000000u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = (c.SP + 0x54u); mem.WriteU32(_ad, _sw); }
        c.T5 = c.T5 | CullMaskX;
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        TerrainFloatProjection.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022AF0;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80022AF0;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        if ((int)c.T0 > 0) {
            c.At = c.T9 & 0x0018u;
            goto L80022528;
        }
        c.At = c.T9 & 0x0018u;
        RecompOne.Runtime.Gte.Nclip();
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        if ((int)c.T0 >= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            MediEvil_game.func_80021E5C(c, m);
            return;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        L80022528: ;
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.T3 = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80022544;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80022544: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.T3 = (int)c.T0 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T1 = c.T2 + 0u;
            goto L8002255C;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L8002255C: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.T3 = (int)c.T1 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80022574;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80022574: ;
        c.T2 = c.T0 + 0u;
        c.T1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.T0 = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.T0 == 0u) {
            MediEvil_game.func_80021E5C(c, m);
            return;
        }
        if (c.At == 0u) {
            c.T8 = c.T9 & 0x0010u;
            goto L800225FC;
        }
        c.T8 = c.T9 & 0x0010u;
        if (c.T8 == 0u) {
            goto L800225F0;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L800225B8;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L800225B8: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T2 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T1 = c.T2 + 0u;
            goto L800225D0;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L800225D0: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L800225E8;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L800225E8: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002260C;
        L800225F0: ;
        c.T0 = c.T2 + 0u;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002260C;
        L800225FC: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T0 = RecompOne.Runtime.Gte.Read(7);
       
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L8002260C: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80022620;
        }
        c.T0 = c.T1 + 0u;
        L80022620: ;
        c.T0 = TerrainPatch.NearOtz(c.T0);
        c.T1 = (int)c.T0 < TerrainPatch.NearClip ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x54u));
            MediEvil_game.func_80021E5C(c, m);
            return;
        }
        c.T1 = mem.ReadU32((c.SP + 0x54u));
        c.T3 = RecompOne.Runtime.Gte.Read(12);
       
        c.T4 = RecompOne.Runtime.Gte.Read(13);
       
        c.T6 = RecompOne.Runtime.Gte.Read(14);
       
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T3) & c.T7;
        c.T7 = CullBias(c.T4) & c.T7;
        c.T7 = CullBias(c.T6) & c.T7;
        if (c.T7 != 0u) {
            MediEvil_game.func_80021E5C(c, m);
            return;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
       
        mem.WriteU32((c.A0 + 0x14u), c.T3);
       
        mem.WriteU32((c.A0 + 0x20u), c.T4);
       
        mem.WriteU32((c.A0 + 0x2Cu), c.T6);
        TerrainFloatProjection.StoreFirst(c.A0 + 0x8u, c.T1);
        TerrainFloatProjection.Store(12, c.A0 + 0x14u, c.T3);
        TerrainFloatProjection.Store(13, c.A0 + 0x20u, c.T4);
        TerrainFloatProjection.Store(14, c.A0 + 0x2Cu, c.T6);
       
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
       
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
       
        c.S2 = c.S2 + 0x1u;
        c.T6 = mem.ReadU16((c.A1 + 0x8u));
        c.T5 = mem.ReadU32((c.SP + 0x34u));
        c.T6 = c.T6 << 2;
        c.T5 = c.T5 + c.T6;
        c.T5 = mem.ReadU32(c.T5);
        c.T1 = mem.ReadU16((c.A1 + 0xCu));
        c.T2 = mem.ReadU16((c.A1 + 0xEu));
        c.T3 = mem.ReadU16((c.A1 + 0x10u));
        c.T4 = mem.ReadU16((c.A1 + 0x12u));
        mem.WriteU16((c.A0 + 0xCu), (ushort)c.T1);
        mem.WriteU16((c.A0 + 0x18u), (ushort)c.T2);
        mem.WriteU16((c.A0 + 0x24u), (ushort)c.T3);
        mem.WriteU16((c.A0 + 0x30u), (ushort)c.T4);
        c.T7 = mem.ReadU16((c.T5 + 0xAu));
        c.T8 = mem.ReadU16((c.T5 + 0x6u));
        mem.WriteU16((c.A0 + 0x1Au), (ushort)c.T7);
        mem.WriteU16((c.A0 + 0xEu), (ushort)c.T8);
        c.At = c.T9 & 0x0200u;
        if (c.At == 0u) {
            c.T1 = 0x3C000000u;
            goto L800226D0;
        }
        c.T1 = 0x3C000000u;
        c.T1 = 0x3E000000u;
        L800226D0: ;
        c.T2 = mem.ReadU32((c.SP + 0x44u));
        c.T8 = mem.ReadU32((c.SP + 0x40u));
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.T2 = RecompOne.Runtime.Gte.Read(16);
       
        c.At = c.At | c.T1;
        c.T2 = (uint)((int)c.T2 >> 5);
        c.At = c.At | c.T3;
        c.T2 = c.T2 << 1;
        c.At = c.At | c.T4;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x48u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(17);
       
        mem.WriteU32((c.A0 + 0x4u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x4Cu));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(18);
       
        mem.WriteU32((c.A0 + 0x10u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x50u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        c.T3 = mem.ReadU32((c.SP + 0xA8u));
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        if ((int)c.T3 > 0) {
            c.T3 = (uint)((int)c.T3 >> 1);
            goto L800227FC;
        }
        c.T3 = (uint)((int)c.T3 >> 1);
        c.T3 = 0u - c.T3;
        L800227FC: ;
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        mem.WriteU32((c.SP + 0xACu), c.T0);
       
        c.T2 = (int)c.T3 < 1000 ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
            goto L80022814;
        }
        c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T9 = c.A0 & c.FP;
            goto L8002283C;
        }
        L80022814: ;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x34u;
        MediEvil_game.func_80021E5C(c, m);
        return;
        L8002283C: ;
        c.T0 = c.A0 + 0x34u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        c.T3 = c.T2 + 0x28u;
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x8u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x14u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x20u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Cu));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0xAu));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x16u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x22u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Eu));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 16;
        c.T8 = c.T8 & 0xFFFFu;
        c.T8 = c.T8 | c.T9;
        mem.WriteU32((c.T0 + 0x20u), c.T8);
       
        mem.WriteU32((c.T1 + 0x20u), c.T8);
       
        mem.WriteU32((c.T2 + 0x20u), c.T8);
       
        mem.WriteU32((c.T3 + 0x20u), c.T8);
       
        c.T4 = mem.ReadU8((c.A0 + 0x12u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Eu));
        c.T6 = mem.ReadU8((c.A0 + 0x11u));
        c.T7 = mem.ReadU8((c.A0 + 0x1Du));
        c.T8 = c.T4 + c.T5;
        c.T8 = (uint)((int)c.T8 >> 1);
        c.T4 = mem.ReadU8((c.A0 + 0x10u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Cu));
        c.T9 = c.T6 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 1);
        c.T4 = c.T4 + c.T5;
        c.T4 = (uint)((int)c.T4 >> 1);
        c.T8 = c.T8 << 16;
        c.T9 = c.T9 << 8;
        c.T4 = c.T4 | c.T8;
        c.T4 = c.T4 | c.T9;
        mem.WriteU32((c.T0 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T1 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T2 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T3 + 0x1Cu), c.T4);
       
        c.T4 = mem.ReadU8((c.A0 + 0xCu));
        c.T5 = mem.ReadU8((c.A0 + 0x18u));
        c.T6 = mem.ReadU8((c.A0 + 0x24u));
        c.T7 = mem.ReadU8((c.A0 + 0x30u));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = mem.ReadU8((c.A0 + 0xDu));
        c.T5 = mem.ReadU8((c.A0 + 0x19u));
        c.T6 = mem.ReadU8((c.A0 + 0x25u));
        c.T7 = mem.ReadU8((c.A0 + 0x31u));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 8;
        c.T8 = c.T8 | c.T9;
        mem.WriteU16((c.T0 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T1 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T2 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T3 + 0x24u), (ushort)c.T8);
        c.T9 = mem.ReadU32((c.A0 + 0x8u));
        c.T4 = mem.ReadU32((c.A0 + 0x14u));
        c.T5 = mem.ReadU16((c.A0 + 0x4u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T5 = c.T7 | c.T5;
        c.T6 = mem.ReadU16((c.A0 + 0x10u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x12u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T0 + 0x8u), c.T9);
       
        mem.WriteU32((c.T0 + 0x14u), c.T4);
       
        mem.WriteU32((c.T0 + 0x4u), c.T5);
       
        mem.WriteU32((c.T0 + 0x10u), c.T6);
       
        mem.WriteU32((c.T1 + 0x8u), c.T4);
       
        mem.WriteU32((c.T1 + 0x4u), c.T6);
       
        mem.WriteU32((c.T3 + 0x14u), c.T9);
       
        mem.WriteU32((c.T3 + 0x10u), c.T5);
       
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0xCu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x18u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x1Au));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0xEu));
        mem.WriteU16((c.T0 + 0xCu), (ushort)c.T9);
        mem.WriteU16((c.T0 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T0 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T0 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T3 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T3 + 0xEu), (ushort)c.T6);
        c.T9 = mem.ReadU32((c.A0 + 0x20u));
        c.T4 = mem.ReadU16((c.A0 + 0x1Cu));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x1Eu));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T4 = c.T7 | c.T4;
        c.T5 = mem.ReadU32((c.A0 + 0x2Cu));
        c.T6 = mem.ReadU16((c.A0 + 0x28u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x2Au));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T1 + 0x14u), c.T5);
       
        mem.WriteU32((c.T1 + 0x10u), c.T6);
       
        mem.WriteU32((c.T2 + 0x8u), c.T5);
       
        mem.WriteU32((c.T2 + 0x14u), c.T9);
       
        mem.WriteU32((c.T2 + 0x4u), c.T6);
       
        mem.WriteU32((c.T2 + 0x10u), c.T4);
       
        mem.WriteU32((c.T3 + 0x4u), c.T4);
       
        mem.WriteU32((c.T3 + 0x8u), c.T9);
       
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x30u));
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0x24u));
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0xCu), (ushort)c.T9);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T3 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.A0 = c.T3 + 0x28u;
        MediEvil_game.func_80021E5C(c, m);
        return;
        L80022AF0: ;
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        mem.WriteU32((c.V0 + 0x34u), c.S2);
       
        L80022B00: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if ((int)c.S2 <= 0) {
            goto L80022B28;
        }
        c.T1 = c.T1 - 0x4u;
        c.T0 = mem.ReadU32(c.T1);
        c.S2 = c.S2 - 0x1u;
        c.T9 = (uint)(short)mem.ReadU16((c.T0 + 0xAu));
        c.T9 = c.T9 & 0xFFFBu;
        mem.WriteU16((c.T0 + 0xAu), (ushort)c.T9);
        goto L80022B00;
        L80022B28: ;
        mem.WriteU32((c.V0 + 0x10u), c.T1);
       
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0xB0u;
        return;
    }

    public static void func_80022460(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.T0 = (uint)(short)mem.ReadU16(c.A1);
        c.T1 = (uint)(short)mem.ReadU16((c.A1 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A1 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A1 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A2;
        c.T1 = c.T1 + c.A2;
        c.T2 = c.T2 + c.A2;
        c.T3 = c.T3 + c.A2;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T2));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T2 + 0x4u)));
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        TerrainFloatProjection.Rtpt(12, false);
        c.T0 = mem.ReadU16((c.T0 + 0x6u));
        c.T1 = mem.ReadU16((c.T1 + 0x6u));
        c.T2 = mem.ReadU16((c.T2 + 0x6u));
        c.T3 = mem.ReadU16((c.T3 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.T0);
       
        mem.WriteU32((c.SP + 0x48u), c.T1);
       
        mem.WriteU32((c.SP + 0x4Cu), c.T2);
       
        mem.WriteU32((c.SP + 0x50u), c.T3);
       
        RecompOne.Runtime.Gte.Nclip();
        RecompOne.Runtime.Gte.Write(0, c.T4);
        RecompOne.Runtime.Gte.Write(1, c.T5);
        c.T5 = 0xFF000000u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = (c.SP + 0x54u); mem.WriteU32(_ad, _sw); }
        c.T5 = c.T5 | CullMaskX;
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        TerrainFloatProjection.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x58u));
        c.At = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.At != 0u) {
            c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
            goto L80022AF0;
        }
        c.T1 = (int)c.S2 < (int)c.T2 ? 1u : 0u;
        if (c.T1 == 0u) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            goto L80022AF0;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        if ((int)c.T0 > 0) {
            c.At = c.T9 & 0x0018u;
            goto L80022528;
        }
        c.At = c.T9 & 0x0018u;
        RecompOne.Runtime.Gte.Nclip();
        c.T0 = RecompOne.Runtime.Gte.Read(24);
       
        if ((int)c.T0 >= 0) {
            mem.WriteU32((c.SP + 0xA8u), c.T0);
           
            MediEvil_game.func_80021E5C(c, m);
            return;
        }
        mem.WriteU32((c.SP + 0xA8u), c.T0);
       
        L80022528: ;
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.T3 = (int)c.T0 < (int)c.T1 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T2 = c.T1 + 0u;
            goto L80022544;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L80022544: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.T3 = (int)c.T0 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T1 = c.T2 + 0u;
            goto L8002255C;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L8002255C: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.T3 = (int)c.T1 < (int)c.T2 ? 1u : 0u;
        if (c.T3 == 0u) {
            c.T0 = c.T2 + 0u;
            goto L80022574;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L80022574: ;
        c.T2 = c.T0 + 0u;
        c.T1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.T0 = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.T0 == 0u) {
            MediEvil_game.func_80021E5C(c, m);
            return;
        }
        if (c.At == 0u) {
            c.T8 = c.T9 & 0x0010u;
            goto L800225FC;
        }
        c.T8 = c.T9 & 0x0010u;
        if (c.T8 == 0u) {
            goto L800225F0;
        }
        c.T0 = RecompOne.Runtime.Gte.Read(16);
       
        c.T1 = RecompOne.Runtime.Gte.Read(17);
       
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T2 = c.T1 + 0u;
            goto L800225B8;
        }
        c.T2 = c.T1 + 0u;
        c.T2 = c.T0 + 0u;
        L800225B8: ;
        c.T0 = RecompOne.Runtime.Gte.Read(18);
       
        c.At = (int)c.T2 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            c.T1 = c.T2 + 0u;
            goto L800225D0;
        }
        c.T1 = c.T2 + 0u;
        c.T1 = c.T0 + 0u;
        L800225D0: ;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        c.At = (int)c.T2 < (int)c.T1 ? 1u : 0u;
        if (c.At == 0u) {
            c.T0 = c.T2 + 0u;
            goto L800225E8;
        }
        c.T0 = c.T2 + 0u;
        c.T0 = c.T1 + 0u;
        L800225E8: ;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002260C;
        L800225F0: ;
        c.T0 = c.T2 + 0u;
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        goto L8002260C;
        L800225FC: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T0 = RecompOne.Runtime.Gte.Read(7);
       
        c.T0 = (uint)((int)c.T0 >> (int)(c.S3 & 31u));
        L8002260C: ;
        c.T1 = c.S4 - 0x3u;
        c.At = (int)c.T1 < (int)c.T0 ? 1u : 0u;
        if (c.At == 0u) {
            goto L80022620;
        }
        c.T0 = c.T1 + 0u;
        L80022620: ;
        c.T0 = TerrainPatch.NearOtz(c.T0);
        c.T1 = (int)c.T0 < TerrainPatch.NearClip ? 1u : 0u;
        if (c.T1 != 0u) {
            c.T1 = mem.ReadU32((c.SP + 0x54u));
            MediEvil_game.func_80021E5C(c, m);
            return;
        }
        c.T1 = mem.ReadU32((c.SP + 0x54u));
        c.T3 = RecompOne.Runtime.Gte.Read(12);
       
        c.T4 = RecompOne.Runtime.Gte.Read(13);
       
        c.T6 = RecompOne.Runtime.Gte.Read(14);
       
        c.T7 = CullBias(c.T1) & c.T5;
        c.T7 = CullBias(c.T3) & c.T7;
        c.T7 = CullBias(c.T4) & c.T7;
        c.T7 = CullBias(c.T6) & c.T7;
        if (c.T7 != 0u) {
            MediEvil_game.func_80021E5C(c, m);
            return;
        }
        mem.WriteU32((c.A0 + 0x8u), c.T1);
       
        mem.WriteU32((c.A0 + 0x14u), c.T3);
       
        mem.WriteU32((c.A0 + 0x20u), c.T4);
       
        mem.WriteU32((c.A0 + 0x2Cu), c.T6);
        TerrainFloatProjection.StoreFirst(c.A0 + 0x8u, c.T1);
        TerrainFloatProjection.Store(12, c.A0 + 0x14u, c.T3);
        TerrainFloatProjection.Store(13, c.A0 + 0x20u, c.T4);
        TerrainFloatProjection.Store(14, c.A0 + 0x2Cu, c.T6);
       
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        c.T9 = c.T9 | 0x0004u;
        mem.WriteU32(c.T1, c.A1);
       
        mem.WriteU16((c.A1 + 0xAu), (ushort)c.T9);
        c.T1 = c.T1 + 0x4u;
        mem.WriteU32((c.SP + 0x5Cu), c.T1);
       
        c.S2 = c.S2 + 0x1u;
        c.T6 = mem.ReadU16((c.A1 + 0x8u));
        c.T5 = mem.ReadU32((c.SP + 0x34u));
        c.T6 = c.T6 << 2;
        c.T5 = c.T5 + c.T6;
        c.T5 = mem.ReadU32(c.T5);
        c.T1 = mem.ReadU16((c.A1 + 0xCu));
        c.T2 = mem.ReadU16((c.A1 + 0xEu));
        c.T3 = mem.ReadU16((c.A1 + 0x10u));
        c.T4 = mem.ReadU16((c.A1 + 0x12u));
        mem.WriteU16((c.A0 + 0xCu), (ushort)c.T1);
        mem.WriteU16((c.A0 + 0x18u), (ushort)c.T2);
        mem.WriteU16((c.A0 + 0x24u), (ushort)c.T3);
        mem.WriteU16((c.A0 + 0x30u), (ushort)c.T4);
        c.T7 = mem.ReadU16((c.T5 + 0xAu));
        c.T8 = mem.ReadU16((c.T5 + 0x6u));
        mem.WriteU16((c.A0 + 0x1Au), (ushort)c.T7);
        mem.WriteU16((c.A0 + 0xEu), (ushort)c.T8);
        c.At = c.T9 & 0x0200u;
        if (c.At == 0u) {
            c.T1 = 0x3C000000u;
            goto L800226D0;
        }
        c.T1 = 0x3C000000u;
        c.T1 = 0x3E000000u;
        L800226D0: ;
        c.T2 = mem.ReadU32((c.SP + 0x44u));
        c.T8 = mem.ReadU32((c.SP + 0x40u));
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.T2 = RecompOne.Runtime.Gte.Read(16);
       
        c.At = c.At | c.T1;
        c.T2 = (uint)((int)c.T2 >> 5);
        c.At = c.At | c.T3;
        c.T2 = c.T2 << 1;
        c.At = c.At | c.T4;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x48u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(17);
       
        mem.WriteU32((c.A0 + 0x4u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x4Cu));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(18);
       
        mem.WriteU32((c.A0 + 0x10u), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        c.T2 = mem.ReadU32((c.SP + 0x50u));
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        c.At = c.T2 & 0xF8F8u;
        c.T3 = c.T2 & 0x0700u;
        c.T4 = c.T2 & 0x0003u;
        c.T3 = c.T3 << 13;
        c.T4 = c.T4 << 19;
        c.At = c.At | c.T3;
        c.At = c.At | c.T4;
        c.T2 = RecompOne.Runtime.Gte.Read(19);
       
        mem.WriteU32((c.A0 + 0x1Cu), RecompOne.Runtime.Gte.Read(22));
        c.T2 = (uint)((int)c.T2 >> 5);
        c.T2 = c.T2 << 1;
        c.T1 = c.T8 + c.T2;
        c.T1 = mem.ReadU16(c.T1);
        c.T3 = mem.ReadU32((c.SP + 0xA8u));
        RecompOne.Runtime.Gte.Write(6, c.At);
        RecompOne.Runtime.Gte.Write(8, c.T1);
        if ((int)c.T3 > 0) {
            c.T3 = (uint)((int)c.T3 >> 1);
            goto L800227FC;
        }
        c.T3 = (uint)((int)c.T3 >> 1);
        c.T3 = 0u - c.T3;
        L800227FC: ;
        RecompOne.Runtime.Gte.Execute(0x4A780010u);
        mem.WriteU32((c.SP + 0xACu), c.T0);
       
        c.T2 = (int)c.T3 < 1000 ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
            goto L80022814;
        }
        c.T2 = (int)c.T0 < TerrainPatch.SubdivOtz ? 1u : 0u;
        if (c.T2 != 0u) {
            c.T9 = c.A0 & c.FP;
            goto L8002283C;
        }
        L80022814: ;
        c.T9 = c.A0 & c.FP;
        c.T0 = c.T0 << 2;
        c.T0 = c.T0 + c.S5;
        c.At = mem.ReadU32(c.T0);
        mem.WriteU32(c.T0, c.T9);
       
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
       
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.A0 = c.A0 + 0x34u;
        MediEvil_game.func_80021E5C(c, m);
        return;
        L8002283C: ;
        c.T0 = c.A0 + 0x34u;
        c.T1 = c.T0 + 0x28u;
        c.T2 = c.T1 + 0x28u;
        c.T3 = c.T2 + 0x28u;
        mem.WriteU32((c.A0 + 0x28u), RecompOne.Runtime.Gte.Read(22));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x8u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x14u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x20u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Cu));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0xAu));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x16u));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0x22u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x2Eu));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 16;
        c.T8 = c.T8 & 0xFFFFu;
        c.T8 = c.T8 | c.T9;
        mem.WriteU32((c.T0 + 0x20u), c.T8);
       
        mem.WriteU32((c.T1 + 0x20u), c.T8);
       
        mem.WriteU32((c.T2 + 0x20u), c.T8);
       
        mem.WriteU32((c.T3 + 0x20u), c.T8);
       
        c.T4 = mem.ReadU8((c.A0 + 0x12u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Eu));
        c.T6 = mem.ReadU8((c.A0 + 0x11u));
        c.T7 = mem.ReadU8((c.A0 + 0x1Du));
        c.T8 = c.T4 + c.T5;
        c.T8 = (uint)((int)c.T8 >> 1);
        c.T4 = mem.ReadU8((c.A0 + 0x10u));
        c.T5 = mem.ReadU8((c.A0 + 0x1Cu));
        c.T9 = c.T6 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 1);
        c.T4 = c.T4 + c.T5;
        c.T4 = (uint)((int)c.T4 >> 1);
        c.T8 = c.T8 << 16;
        c.T9 = c.T9 << 8;
        c.T4 = c.T4 | c.T8;
        c.T4 = c.T4 | c.T9;
        mem.WriteU32((c.T0 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T1 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T2 + 0x1Cu), c.T4);
       
        mem.WriteU32((c.T3 + 0x1Cu), c.T4);
       
        c.T4 = mem.ReadU8((c.A0 + 0xCu));
        c.T5 = mem.ReadU8((c.A0 + 0x18u));
        c.T6 = mem.ReadU8((c.A0 + 0x24u));
        c.T7 = mem.ReadU8((c.A0 + 0x30u));
        c.T8 = c.T4 + c.T5;
        c.T8 = c.T8 + c.T6;
        c.T8 = c.T8 + c.T7;
        c.T8 = (uint)((int)c.T8 >> 2);
        c.T4 = mem.ReadU8((c.A0 + 0xDu));
        c.T5 = mem.ReadU8((c.A0 + 0x19u));
        c.T6 = mem.ReadU8((c.A0 + 0x25u));
        c.T7 = mem.ReadU8((c.A0 + 0x31u));
        c.T9 = c.T4 + c.T5;
        c.T9 = c.T9 + c.T6;
        c.T9 = c.T9 + c.T7;
        c.T9 = (uint)((int)c.T9 >> 2);
        c.T9 = c.T9 << 8;
        c.T8 = c.T8 | c.T9;
        mem.WriteU16((c.T0 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T1 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T2 + 0x24u), (ushort)c.T8);
        mem.WriteU16((c.T3 + 0x24u), (ushort)c.T8);
        c.T9 = mem.ReadU32((c.A0 + 0x8u));
        c.T4 = mem.ReadU32((c.A0 + 0x14u));
        c.T5 = mem.ReadU16((c.A0 + 0x4u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T5 = c.T7 | c.T5;
        c.T6 = mem.ReadU16((c.A0 + 0x10u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x12u));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T0 + 0x8u), c.T9);
       
        mem.WriteU32((c.T0 + 0x14u), c.T4);
       
        mem.WriteU32((c.T0 + 0x4u), c.T5);
       
        mem.WriteU32((c.T0 + 0x10u), c.T6);
       
        mem.WriteU32((c.T1 + 0x8u), c.T4);
       
        mem.WriteU32((c.T1 + 0x4u), c.T6);
       
        mem.WriteU32((c.T3 + 0x14u), c.T9);
       
        mem.WriteU32((c.T3 + 0x10u), c.T5);
       
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0xCu));
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x18u));
        c.T5 = (uint)(short)mem.ReadU16((c.A0 + 0x1Au));
        c.T6 = (uint)(short)mem.ReadU16((c.A0 + 0xEu));
        mem.WriteU16((c.T0 + 0xCu), (ushort)c.T9);
        mem.WriteU16((c.T0 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T0 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T0 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T1 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T2 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.T6);
        mem.WriteU16((c.T3 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0x1Au), (ushort)c.T5);
        mem.WriteU16((c.T3 + 0xEu), (ushort)c.T6);
        c.T9 = mem.ReadU32((c.A0 + 0x20u));
        c.T4 = mem.ReadU16((c.A0 + 0x1Cu));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x1Eu));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T4 = c.T7 | c.T4;
        c.T5 = mem.ReadU32((c.A0 + 0x2Cu));
        c.T6 = mem.ReadU16((c.A0 + 0x28u));
        c.T7 = (uint)(short)mem.ReadU16((c.A0 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.A0 + 0x2Au));
        c.T7 = c.T7 & 0x0200u;
        c.T8 = c.T8 & 0x00FFu;
        c.T7 = c.T7 | 0x3400u;
        c.T7 = c.T7 | c.T8;
        c.T7 = c.T7 << 16;
        c.T6 = c.T7 | c.T6;
        mem.WriteU32((c.T1 + 0x14u), c.T5);
       
        mem.WriteU32((c.T1 + 0x10u), c.T6);
       
        mem.WriteU32((c.T2 + 0x8u), c.T5);
       
        mem.WriteU32((c.T2 + 0x14u), c.T9);
       
        mem.WriteU32((c.T2 + 0x4u), c.T6);
       
        mem.WriteU32((c.T2 + 0x10u), c.T4);
       
        mem.WriteU32((c.T3 + 0x4u), c.T4);
       
        mem.WriteU32((c.T3 + 0x8u), c.T9);
       
        c.T4 = (uint)(short)mem.ReadU16((c.A0 + 0x30u));
        c.T9 = (uint)(short)mem.ReadU16((c.A0 + 0x24u));
        mem.WriteU16((c.T1 + 0x18u), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0xCu), (ushort)c.T4);
        mem.WriteU16((c.T2 + 0x18u), (ushort)c.T9);
        mem.WriteU16((c.T3 + 0xCu), (ushort)c.T9);
        c.T8 = mem.ReadU32((c.SP + 0xACu));
        c.T9 = c.T0 & c.FP;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S5;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T1 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T2 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.T9 = c.T3 & c.FP;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
       
        c.At = c.At | c.S6;
        mem.WriteU32(c.T9, c.At);
       
        c.A0 = c.T3 + 0x28u;
        MediEvil_game.func_80021E5C(c, m);
        return;
        L80022AF0: ;
        c.T1 = mem.ReadU32((c.SP + 0x5Cu));
        mem.WriteU32((c.V0 + 0x34u), c.S2);
       
        L80022B00: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if ((int)c.S2 <= 0) {
            goto L80022B28;
        }
        c.T1 = c.T1 - 0x4u;
        c.T0 = mem.ReadU32(c.T1);
        c.S2 = c.S2 - 0x1u;
        c.T9 = (uint)(short)mem.ReadU16((c.T0 + 0xAu));
        c.T9 = c.T9 & 0xFFFBu;
        mem.WriteU16((c.T0 + 0xAu), (ushort)c.T9);
        goto L80022B00;
        L80022B28: ;
        mem.WriteU32((c.V0 + 0x10u), c.T1);
       
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0xB0u;
        return;
    }

    public static void func_80024B00(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x88u;
        mem.WriteU32((c.SP + 0x6Cu), c.S1);
       
        c.S1 = mem.ReadU32((c.GP + RAddr.Game_80024B08));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        mem.WriteU32((c.SP + 0x68u), c.S0);
       
        mem.WriteU32((c.SP + 0x84u), c.RA);
       
        mem.WriteU32((c.SP + 0x80u), c.S6);
       
        mem.WriteU32((c.SP + 0x7Cu), c.S5);
       
        mem.WriteU32((c.SP + 0x78u), c.S4);
       
        mem.WriteU32((c.SP + 0x74u), c.S3);
       
        mem.WriteU32((c.SP + 0x70u), c.S2);
       
        c.A1 = mem.ReadU32((c.S1 + 0xCu));
        c.S0 = c.A0 + 0u;
        c.RA = 0x80024B3Cu;
        MediEvil_game.func_800A4EB8(c, m);
        c.S3 = 0u + 0u;
        c.S2 = c.S3 + 0u;
        c.A0 = c.S0 + 0u;
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.V1 = mem.ReadU16((c.S0 + 0x14u));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.V0 = mem.ReadU16((c.V0 + 0x14u));
        c.A2 = c.A2 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.V1 = mem.ReadU16((c.A0 + 0x18u));
        c.V0 = mem.ReadU16((c.V0 + 0x18u));
        c.A1 = mem.ReadU16((c.S1 + 0x2Cu));
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 - c.A1;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.V1 = mem.ReadU16((c.A0 + 0x1Cu));
        c.V0 = mem.ReadU16((c.V0 + 0x1Cu));
        c.A1 = c.SP + 0x10u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.RA = 0x80024BA0u;
        MediEvil_game.func_800A48FC(c, m);
        c.S4 = 0x1F800000u;
        c.S4 = mem.ReadU32((c.S4 + 0x34u));
        c.T4 = mem.ReadU32(c.S4);
        c.T5 = mem.ReadU32((c.S4 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S4 + 0x8u));
        c.T5 = mem.ReadU32((c.S4 + 0xCu));
        c.T6 = mem.ReadU32((c.S4 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.S4 + 0x14u));
        c.T5 = mem.ReadU32((c.S4 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.S4 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T3 = mem.ReadU32((c.S1 + 0x1Cu));
        c.V1 = mem.ReadU16((c.S1 + 0xAu));
        c.T2 = mem.ReadU32((c.S1 + 0x18u));
        if (c.V1 == 0u) {
            c.T8 = c.S3 + 0u;
            goto L80024D04;
        }
        c.T8 = c.S3 + 0u;
        c.T8 = 0x00000001u;
        c.V0 = mem.ReadU32((c.S1 + 0x4u));
        c.V1 = c.V1 - 0x1u;
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            mem.WriteU16((c.S1 + 0xAu), (ushort)c.V1);
            goto L80024C84;
        }
        mem.WriteU16((c.S1 + 0xAu), (ushort)c.V1);
        c.V0 = mem.ReadU32((c.S1 + 0x30u));
        c.V1 = mem.ReadU8(c.V0);
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x48u), c.V0);
       
        c.V0 = mem.ReadU32((c.S1 + 0x30u));
        c.V1 = mem.ReadU8((c.V0 + 0x1u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
       
        c.V0 = mem.ReadU32((c.S1 + 0x30u));
        c.V1 = mem.ReadU8((c.V0 + 0x2u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x50u), c.V0);
       
        L80024C84: ;
        c.V0 = mem.ReadU32((c.S1 + 0x4u));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            goto L80024D04;
        }
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V1 = mem.ReadU8(c.V0);
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x58u), c.V0);
       
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V1 = mem.ReadU8((c.V0 + 0x1u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x5Cu), c.V0);
       
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V1 = mem.ReadU8((c.V0 + 0x2u));
        c.V0 = mem.ReadU16((c.S1 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU32((c.SP + 0x60u), c.V0);
       
        L80024D04: ;
        c.T5 = mem.ReadU16((c.S1 + 0x28u));
        if ((int)c.T5 <= 0) {
            c.S0 = 0x00000003u;
            goto L80025290;
        }
        c.S0 = 0x00000003u;
        c.T4 = 0xFF000000u;
        c.T4 = c.T4 | CullMaskX;
        c.T7 = 0x00FF0000u;
        c.T7 = c.T7 | 0xFFFFu;
        c.T6 = 0xFF000000u;
        c.T9 = c.SP + 0x18u;
        c.T1 = c.T3 + 0xAu;
        L80024D30: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.A0 = (uint)(short)mem.ReadU16((c.T1 - 0x6u));
        c.V1 = (uint)(short)mem.ReadU16((c.T1 - 0x4u));
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.A0 = c.A0 << 3;
        c.A0 = c.T2 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.T2 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.A0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.A0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.V1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.V1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.V0 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.V1 = mem.ReadU32(c.T3);
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L80024DD4;
        }
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0xCu;
        c.T0 = c.T3 + c.V0;
        c.S3 = c.T0 + 0u;
        c.V0 = c.T0 + 0x8u;
        mem.WriteU32((c.SP + 0x18u), c.V0);
       
        c.V0 = c.T0 + 0x14u;
        mem.WriteU32((c.SP + 0x1Cu), c.V0);
       
        c.V0 = c.T0 + 0x20u;
        mem.WriteU32((c.SP + 0x20u), c.V0);
       
        c.V0 = c.T0 + 0x2Cu;
        goto L80024E1C;
        L80024DD4: ;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0xCu;
        c.T0 = c.T3 + c.V0;
        c.S2 = c.T0 + 0u;
        c.V0 = c.T0 + 0x10u;
        mem.WriteU32((c.SP + 0x18u), c.V0);
       
        c.V0 = c.T0 + 0x18u;
        mem.WriteU32((c.SP + 0x1Cu), c.V0);
       
        c.V0 = c.T0 + 0x20u;
        mem.WriteU32((c.SP + 0x20u), c.V0);
       
        c.V0 = c.T0 + 0x28u;
        L80024E1C: ;
        mem.WriteU32((c.SP + 0x24u), c.V0);
       
        c.S5 = mem.ReadU32((c.SP + 0x18u));
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.S5; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw, (mem.ReadU32(c.S1 + 0x4u) & 1u) != 0); }
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.V0 + 0x4u)));
        TerrainFloatProjection.Rtps(12, false);
        c.V0 = 0x00000001u;
        if (c.T8 != c.V0) {
            c.V0 = 0x00000006u;
            goto L8002509C;
        }
        c.V0 = 0x00000006u;
        c.V1 = mem.ReadU32(c.T3);
        if (c.V1 != c.V0) {
            c.A0 = c.SP + 0x58u;
            goto L80024E90;
        }
        c.A0 = c.SP + 0x58u;
        c.A0 = c.SP + 0x48u;
        c.V0 = c.S3 + 0x4u;
        mem.WriteU32((c.SP + 0x28u), c.V0);
       
        c.V0 = c.S3 + 0x10u;
        mem.WriteU32((c.SP + 0x2Cu), c.V0);
       
        c.V0 = c.S3 + 0x1Cu;
        mem.WriteU32((c.SP + 0x30u), c.V0);
       
        c.V0 = c.S3 + 0x28u;
        goto L80024EAC;
        L80024E90: ;
        c.V0 = c.S2 + 0xCu;
        mem.WriteU32((c.SP + 0x28u), c.V0);
       
        c.V0 = c.S2 + 0x14u;
        mem.WriteU32((c.SP + 0x2Cu), c.V0);
       
        c.V0 = c.S2 + 0x1Cu;
        mem.WriteU32((c.SP + 0x30u), c.V0);
       
        c.V0 = c.S2 + 0x24u;
        L80024EAC: ;
        mem.WriteU32((c.SP + 0x34u), c.V0);
       
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V1 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x38u), c.V1);
       
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x3Cu), c.V0);
       
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x40u), c.V0);
       
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x6u));
        mem.WriteU32((c.SP + 0x44u), c.V0);
       
        c.V0 = mem.ReadU32(c.A0);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8(c.V1, (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x38u));
        c.V0 = mem.ReadU32((c.A0 + 0x4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x1u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x38u));
        c.V0 = mem.ReadU32((c.A0 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x2u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x3Cu));
        c.V0 = mem.ReadU32(c.A0);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x2Cu));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8(c.V1, (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x3Cu));
        c.V0 = mem.ReadU32((c.A0 + 0x4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x2Cu));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x1u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x3Cu));
        c.V0 = mem.ReadU32((c.A0 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x2Cu));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x2u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = mem.ReadU32(c.A0);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x30u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8(c.V1, (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = mem.ReadU32((c.A0 + 0x4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x30u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x1u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = mem.ReadU32((c.A0 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x30u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x2u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x44u));
        c.V0 = mem.ReadU32(c.A0);
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x34u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8(c.V1, (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x44u));
        c.V0 = mem.ReadU32((c.A0 + 0x4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x34u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x1u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.SP + 0x44u));
        c.V0 = mem.ReadU32((c.A0 + 0x8u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = mem.ReadU32((c.SP + 0x34u));
        c.S4 = c.LO;
        c.V0 = (uint)((int)c.S4 >> 8);
        mem.WriteU8((c.V1 + 0x2u), (byte)c.V0);
        L8002509C: ;
        c.T1 = c.T1 + 0x74u;
        c.T3 = c.T3 + 0x74u;
        c.V0 = c.SP + 0x38u;
        mem.WriteU32(c.V0, RecompOne.Runtime.Gte.Read(16));
        mem.WriteU32((c.V0 + 0x4u), RecompOne.Runtime.Gte.Read(17));
        mem.WriteU32((c.V0 + 0x8u), RecompOne.Runtime.Gte.Read(18));
        mem.WriteU32((c.V0 + 0xCu), RecompOne.Runtime.Gte.Read(19));
        c.V1 = mem.ReadU32((c.SP + 0x44u));
        c.A2 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = (int)c.V1 < (int)c.A2 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800250D4;
        }
        c.V1 = c.A2 + 0u;
        L800250D4: ;
        c.A1 = mem.ReadU32((c.SP + 0x3Cu));
        c.V0 = (int)c.V1 < (int)c.A1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800250EC;
        }
        c.V1 = c.A1 + 0u;
        L800250EC: ;
        c.A0 = mem.ReadU32((c.SP + 0x38u));
        c.V0 = (int)c.V1 < (int)c.A0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L80025104;
        }
        c.V1 = c.A0 + 0u;
        L80025104: ;
        if ((int)c.V1 < 0) {
            goto L80025284;
        }
        c.V0 = mem.ReadU32((c.S1 + 0x4u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = 0x800F0000u;
            goto L8002512C;
        }
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - RAddr.Game_80025120));
        c.A0 = c.V0 - 0x1u;
        goto L80025168;
        L8002512C: ;
        c.A0 = c.A0 + c.A1;
        c.A0 = c.A0 + c.A2;
        c.V0 = mem.ReadU32((c.SP + 0x44u));
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU16((c.V1 + 0x8Cu));
        c.A0 = c.A0 + c.V0;
        c.V1 = c.V1 + 0x2u;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.A0 = (uint)((int)c.A0 >> (int)(c.V1 & 31u));
        c.V0 = (int)c.A0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = (int)c.A0 < 2 ? 1u : 0u;
            goto L80025284;
        }
        c.V0 = (int)c.A0 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L80025284;
        }
        L80025168: ;
        c.S5 = mem.ReadU32((c.SP + 0x1Cu));
        c.S6 = mem.ReadU32((c.SP + 0x20u));
        c.S4 = mem.ReadU32((c.SP + 0x24u));
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.S5; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw, (mem.ReadU32(c.S1 + 0x4u) & 1u) != 0); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); var _ad = c.S6; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(13, _ad, _sw, (mem.ReadU32(c.S1 + 0x4u) & 1u) != 0); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); var _ad = c.S4; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(14, _ad, _sw, (mem.ReadU32(c.S1 + 0x4u) & 1u) != 0); }
        c.V0 = mem.ReadU32((c.SP + 0x18u));
        c.V0 = mem.ReadU32(c.V0);
        c.V0 = CullBias(c.V0) & c.T4;
        if (c.V0 == 0u) {
            goto L800251F0;
        }
        c.V0 = mem.ReadU32((c.SP + 0x1Cu));
        c.V0 = mem.ReadU32(c.V0);
        c.V0 = CullBias(c.V0) & c.T4;
        if (c.V0 == 0u) {
            goto L800251F0;
        }
        c.V0 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = mem.ReadU32(c.V0);
        c.V0 = CullBias(c.V0) & c.T4;
        if (c.V0 == 0u) {
            goto L800251F0;
        }
        c.V0 = mem.ReadU32((c.SP + 0x24u));
        c.V0 = mem.ReadU32(c.V0);
        c.V0 = CullBias(c.V0) & c.T4;
        if (c.V0 != 0u) {
            c.A1 = 0u + 0u;
            goto L80025230;
        }
        c.A1 = 0u + 0u;
        L800251F0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.A0 = c.A0 << 2;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.V1 = mem.ReadU32(c.T0);
        c.A0 = c.A0 + c.V0;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.V1 & c.T6;
        c.V0 = c.V0 & c.T7;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
       
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T7;
        c.V0 = c.V0 & c.T6;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
       
        goto L80025284;
        L80025230: ;
        c.A3 = 0x00000003u;
        c.A2 = c.T9 + 0xCu;
        L80025238: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32(c.A2);
        c.V1 = mem.ReadU32(c.V0);
        c.V0 = CullBias(c.V1) & CullMaskX;
        if (c.V0 == 0u) {
            c.V0 = c.V1 & c.T6;
            goto L80025264;
        }
        c.V0 = c.V1 & c.T6;
        if (c.V0 != 0u) {
            goto L80025278;
        }
        c.A1 = c.A1 | 0x0001u;
        goto L80025270;
        L80025264: ;
        if (c.V0 == 0u) {
            goto L80025278;
        }
        c.A1 = c.A1 | 0x0002u;
        L80025270: ;
        if (c.A1 == c.S0) {
            goto L800251F0;
        }
        L80025278: ;
        c.A3 = c.A3 - 0x1u;
        if ((int)c.A3 >= 0) {
            c.A2 = c.A2 - 0x4u;
            goto L80025238;
        }
        c.A2 = c.A2 - 0x4u;
        L80025284: ;
        c.T5 = c.T5 - 0x1u;
        if ((int)c.T5 > 0) {
            goto L80024D30;
        }
        L80025290: ;
        c.V0 = 0x800F0000u;
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_80025294));
        c.V0 = mem.ReadU16((c.V0 - RAddr.Game_80025298));
        mem.WriteU16((c.V1 + 0xC0u), (ushort)c.V0);
        RecompOne.Runtime.Gte.WriteControl(26, c.V0);
        c.RA = mem.ReadU32((c.SP + 0x84u));
        c.S6 = mem.ReadU32((c.SP + 0x80u));
        c.S5 = mem.ReadU32((c.SP + 0x7Cu));
        c.S4 = mem.ReadU32((c.SP + 0x78u));
        c.S3 = mem.ReadU32((c.SP + 0x74u));
        c.S2 = mem.ReadU32((c.SP + 0x70u));
        c.S1 = mem.ReadU32((c.SP + 0x6Cu));
        c.S0 = mem.ReadU32((c.SP + 0x68u));
        c.SP = c.SP + 0x88u;
        return;
    }

    public static void func_800252D0(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x78u;
        mem.WriteU32((c.SP + 0x5Cu), c.S1);
       
        c.S1 = mem.ReadU32((c.GP + RAddr.Game_800252D8));
        mem.WriteU32((c.SP + 0x70u), c.S6);
       
        mem.WriteU32((c.SP + 0x74u), c.RA);
       
        mem.WriteU32((c.SP + 0x6Cu), c.S5);
       
        mem.WriteU32((c.SP + 0x68u), c.S4);
       
        mem.WriteU32((c.SP + 0x64u), c.S3);
       
        mem.WriteU32((c.SP + 0x60u), c.S2);
       
        mem.WriteU32((c.SP + 0x58u), c.S0);
       
        c.V0 = mem.ReadU32((c.S1 + 0x30u));
        c.S4 = mem.ReadU32((c.S1 + 0x20u));
        c.T3 = c.V0 + 0xCu;
        c.V0 = mem.ReadU16((c.S1 + 0xAu));
        c.T0 = mem.ReadU32((c.S1 + 0x24u));
        if (c.V0 == 0u) {
            c.S6 = c.A0 + 0u;
            goto L80025598;
        }
        c.S6 = c.A0 + 0u;
        c.T2 = 0u + 0u;
        c.S2 = mem.ReadU32((c.S1 + 0x28u));
        c.S3 = mem.ReadU32((c.S1 + 0x2Cu));
        c.A3 = mem.ReadU16((c.S1 + 0x8u));
        c.V1 = mem.ReadU32((c.S1 + 0x34u));
        c.V0 = c.V0 - 0x1u;
        if ((int)c.V1 <= 0) {
            mem.WriteU16((c.S1 + 0xAu), (ushort)c.V0);
            goto L80025598;
        }
        mem.WriteU16((c.S1 + 0xAu), (ushort)c.V0);
        c.T1 = c.S2 + 0x4u;
        L80025338: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.S2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8(c.V0);
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V1 << 3;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0x8u;
        c.A2 = c.S2 + c.V0;
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0xCu), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.S2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x1u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0xDu), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.S2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0xEu), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8(c.V0);
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x14u), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x1u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x15u), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x16u), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8(c.V0);
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x1Cu), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x1u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x1Du), (byte)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T0;
        c.V0 = mem.ReadU8((c.V0 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A2 + 0x1Eu), (byte)c.V0);
        c.V0 = mem.ReadU8(c.T3);
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0x8u;
        c.A1 = c.S3 + c.V0;
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A1 + 0xCu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.T3 + 0x1u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A1 + 0xDu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.T3 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 8);
        mem.WriteU8((c.A1 + 0xEu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A1 + 0xCu));
        c.V1 = mem.ReadU8((c.A1 + 0xDu));
        c.A0 = mem.ReadU8((c.A1 + 0xEu));
        mem.WriteU8((c.A1 + 0x14u), (byte)c.V0);
        mem.WriteU8((c.A1 + 0x15u), (byte)c.V1);
        mem.WriteU8((c.A1 + 0x16u), (byte)c.A0);
        c.V0 = mem.ReadU8((c.A2 + 0x14u));
        mem.WriteU8((c.A1 + 0x1Cu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A2 + 0x15u));
        mem.WriteU8((c.A1 + 0x1Du), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A2 + 0x16u));
        mem.WriteU8((c.A1 + 0x1Eu), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A2 + 0x1Cu));
        c.T2 = c.T2 + 0x1u;
        mem.WriteU8((c.A1 + 0x24u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A2 + 0x1Du));
        c.S2 = c.S2 + 0x50u;
        mem.WriteU8((c.A1 + 0x25u), (byte)c.V0);
        c.V0 = mem.ReadU8((c.A2 + 0x1Eu));
        c.T1 = c.T1 + 0x50u;
        mem.WriteU8((c.A1 + 0x26u), (byte)c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V0 = (int)c.T2 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S3 = c.S3 + 0x60u;
            goto L80025338;
        }
        c.S3 = c.S3 + 0x60u;
        L80025598: ;
        c.S5 = 0x1F800000u;
        c.V0 = 0x00001000u;
        c.S2 = mem.ReadU32((c.S1 + 0x28u));
        c.S3 = mem.ReadU32((c.S1 + 0x2Cu));
        c.S0 = c.S5 + 0x60u;
        mem.WriteU32((c.S5 + 0x60u), c.V0);
       
        mem.WriteU32((c.S0 + 0x4u), 0u);
        mem.WriteU32((c.S0 + 0x8u), c.V0);
       
        mem.WriteU32((c.S0 + 0xCu), 0u);
        mem.WriteU16((c.S0 + 0x10u), (ushort)c.V0);
        c.V1 = mem.ReadU32((c.S1 + 0x4u));
        c.V0 = c.V1 & 0x0080u;
        if (c.V0 == 0u) {
            c.A0 = c.S6 + 0u;
            goto L80025824;
        }
        c.A0 = c.S6 + 0u;
        c.V0 = mem.ReadU16((c.S6 + 0x14u));
        c.A1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.S6 + 0x18u));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.S6 + 0x1Cu));
        c.A2 = c.A2 + 0x14u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.RA = 0x8002560Cu;
        MediEvil_game.func_800A48FC(c, m);
        c.T4 = mem.ReadU32(c.S6);
        c.T5 = mem.ReadU32((c.S6 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S6 + 0x8u));
        c.T5 = mem.ReadU32((c.S6 + 0xCu));
        c.T6 = mem.ReadU32((c.S6 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T8 = 0x1F800000u;
        c.T8 = mem.ReadU32((c.T8 + 0x34u));
        c.T4 = mem.ReadU32((c.T8 + 0x14u));
        c.T5 = mem.ReadU32((c.T8 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T8 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.A0 = mem.ReadU16((c.V0 + 0x14u));
        mem.WriteU16((c.SP + 0x18u), (ushort)c.A0);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.A1 = mem.ReadU16((c.V0 + 0x18u));
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.A1);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.A2 = mem.ReadU16((c.V0 + 0x1Cu));
        c.V0 = 0x800F0000u;
        c.V1 = mem.ReadU32((c.V0 - RAddr.Game_80025690));
        c.V1 = c.V1 + 0xCu;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.A2);
        c.A3 = (uint)(short)mem.ReadU16((c.S1 + 0x38u));
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.V0 = c.A0 + c.V0;
        mem.WriteU16((c.SP + 0x28u), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0x8u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.V0 = c.A1 + c.V0;
        mem.WriteU16((c.SP + 0x2Au), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0xEu));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.V0 = c.A2 + c.V0;
        mem.WriteU16((c.SP + 0x2Cu), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0x4u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.A0 = c.A0 - c.V0;
        mem.WriteU16((c.SP + 0x20u), (ushort)c.A0);
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0xAu));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.A1 = c.A1 - c.V0;
        mem.WriteU16((c.SP + 0x22u), (ushort)c.A1);
        c.V0 = (uint)(short)mem.ReadU16((c.V1 + 0x10u));
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A1 = c.SP + 0x20u;
        c.A3 = c.SP + 0x18u;
        c.T8 = c.LO;
        c.V0 = (uint)((int)c.T8 >> 12);
        c.A2 = c.A2 - c.V0;
        mem.WriteU16((c.SP + 0x24u), (ushort)c.A2);
        c.A2 = c.SP + 0x28u;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.A3));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.A3 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.A2));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.A2 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.A1));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.A1 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.V0 = c.SP + 0x50u;
        mem.WriteU32(c.V0, RecompOne.Runtime.Gte.Read(19));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x50u));
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.V1 = (uint)((int)c.V1 >> (int)(c.A0 & 31u));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0x50u), c.V1);
           
            goto L80025C20;
        }
        mem.WriteU32((c.SP + 0x50u), c.V1);
       
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L80025C20;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.A3; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); var _ad = c.A2; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(13, _ad, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); var _ad = c.A1; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(14, _ad, _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.SP + 0x2Au));
        c.V1 = (uint)(short)mem.ReadU16((c.SP + 0x1Au));
        c.V0 = c.V0 - c.V1;
        c.V1 = (uint)(short)mem.ReadU16((c.S1 + 0x38u));
        c.V0 = c.V0 << 12;
        if (c.V1 != 0u) { if ((int)c.V0 == int.MinValue && (int)c.V1 == -1) { c.LO = 0x80000000u; c.HI = 0u; } else { c.LO = (uint)((int)c.V0 / (int)c.V1); c.HI = (uint)((int)c.V0 % (int)c.V1); } }
        c.A3 = c.LO;
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.V0 = (uint)(short)mem.ReadU16(c.V0);
        { var _r = (long)(int)c.A3 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.V0 = (uint)((int)c.V1 >> 12);
        mem.WriteU16((c.S5 + 0x60u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S1 + 0xCu));
        c.V0 = (uint)(short)mem.ReadU16((c.V0 + 0x8u));
        { var _r = (long)(int)c.A3 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.V0 = (uint)((int)c.V1 >> 12);
        mem.WriteU16((c.S0 + 0x8u), (ushort)c.V0);
        goto L80025864;
        L80025824: ;
        c.V0 = c.V1 & 0x0100u;
        if (c.V0 == 0u) {
            goto L80025848;
        }
        c.V0 = mem.ReadU16((c.S1 + 0x3Cu));
        mem.WriteU16((c.S5 + 0x60u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.S1 + 0x3Eu));
        mem.WriteU16((c.S0 + 0x8u), (ushort)c.V0);
        L80025848: ;
        c.V0 = mem.ReadU16((c.S1 + 0x18u));
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V1 = mem.ReadU16((c.S1 + 0x1Au));
        c.V0 = 0x00000005u;
        mem.WriteU32((c.SP + 0x50u), c.V0);
       
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V1);
        L80025864: ;
        c.V0 = mem.ReadU32((c.S1 + 0x4u));
        c.V0 = c.V0 & 0x0200u;
        if (c.V0 == 0u) {
            c.V0 = 0x00001000u;
            goto L800258AC;
        }
        c.V0 = 0x00001000u;
        c.S0 = c.SP + 0x30u;
        mem.WriteU32((c.SP + 0x30u), c.V0);
       
        mem.WriteU32((c.SP + 0x34u), 0u);
        mem.WriteU32((c.SP + 0x38u), c.V0);
       
        mem.WriteU32((c.SP + 0x3Cu), 0u);
        mem.WriteU16((c.SP + 0x40u), (ushort)c.V0);
        c.A0 = (uint)(short)mem.ReadU16((c.S1 + 0x3Au));
        c.A1 = c.S0 + 0u;
        c.RA = 0x8002589Cu;
        MediEvil_game.func_800A4464(c, m);
        c.A0 = 0x1F800000u;
        c.A0 = c.A0 + 0x60u;
        c.A1 = c.S0 + 0u;
        c.RA = 0x800258ACu;
        MediEvil_game.func_800A4D78(c, m);
        L800258AC: ;
        c.A1 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.GP + RAddr.Game_800258B0));
        c.A1 = c.A1 + 0x60u;
        c.A0 = c.A0 + 0x80u;
        c.RA = 0x800258C0u;
        MediEvil_game.func_800A4FC8(c, m);
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 + 0x60u;
        c.T4 = mem.ReadU32((c.T8 + 0x14u));
        c.T5 = mem.ReadU32((c.T8 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T8 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 + 0x60u;
        c.T4 = mem.ReadU32(c.T8);
        c.T5 = mem.ReadU32((c.T8 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T8 + 0x8u));
        c.T5 = mem.ReadU32((c.T8 + 0xCu));
        c.T6 = mem.ReadU32((c.T8 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T8 = 0u + 0u;
        c.V1 = (uint)(short)mem.ReadU16((c.SP + 0x18u));
        c.V0 = (uint)(short)mem.ReadU16((c.SP + 0x1Au));
        RecompOne.Runtime.Gte.WriteControl(5, c.V1);
        RecompOne.Runtime.Gte.WriteControl(6, c.V0);
        RecompOne.Runtime.Gte.WriteControl(7, c.T8);
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        if ((int)c.V0 <= 0) {
            c.T2 = c.T8 + 0u;
            goto L80025C20;
        }
        c.T2 = c.T8 + 0u;
        c.T1 = c.SP + 0x10u;
        c.A3 = 0xFF000000u;
        c.A3 = c.A3 | CullMaskX;
        c.T0 = 0x00FF0000u;
        c.T0 = c.T0 | 0xFFFFu;
        c.T7 = 0xFF000000u;
        c.T3 = c.S2 + 0x4u;
        L80025954: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V1 << 3;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0x8u;
        c.A2 = c.S2 + c.V0;
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + 0x8u;
        c.V1 = (uint)(short)mem.ReadU16(c.S2);
        c.A1 = c.S3 + c.V0;
        c.V1 = c.V1 << 3;
        c.V1 = c.S4 + c.V1;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.V1));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.V1 + 0x4u)));
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 0);
        c.V0 = (uint)(short)mem.ReadU16((c.T3 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S4 + c.V0;
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.V0 + 0x4u)));
        c.T4 = RecompOne.Runtime.Gte.Read(9);
       
        c.T5 = RecompOne.Runtime.Gte.Read(10);
       
        c.T6 = RecompOne.Runtime.Gte.Read(11);
       
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A2 + 0x10u), c.V0);
       
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 0);
        c.V0 = (uint)(short)mem.ReadU16(c.T3);
        c.V0 = c.V0 << 3;
        c.V0 = c.S4 + c.V0;
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.V0 + 0x4u)));
        c.T4 = RecompOne.Runtime.Gte.Read(9);
       
        c.T5 = RecompOne.Runtime.Gte.Read(10);
       
        c.T6 = RecompOne.Runtime.Gte.Read(11);
       
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A2 + 0x18u), c.V0);
       
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 2, 0);
        c.T4 = RecompOne.Runtime.Gte.Read(9);
       
        c.T5 = RecompOne.Runtime.Gte.Read(10);
       
        c.T6 = RecompOne.Runtime.Gte.Read(11);
       
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = (uint)(short)mem.ReadU16((c.A2 + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x10u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            mem.WriteU32((c.A2 + 0x20u), c.V1);
           
            goto L80025A94;
        }
        mem.WriteU32((c.A2 + 0x20u), c.V1);
       
        c.V0 = (uint)(short)mem.ReadU16((c.A2 + 0x18u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            goto L80025A94;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A2 + 0x20u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 != 0u) {
            goto L80025AD4;
        }
        L80025A94: ;
        c.A0 = mem.ReadU32((c.SP + 0x50u));
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.V1 = mem.ReadU32(c.A2);
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.V0;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.V1 & c.T7;
        c.V0 = c.V0 & c.T0;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A2, c.V1);
       
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.A2 & c.T0;
        c.V0 = c.V0 & c.T7;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
       
        L80025AD4: ;
        c.V0 = (uint)(short)mem.ReadU16(c.S3);
        c.V0 = c.V0 << 3;
        c.V0 = c.S4 + c.V0;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.V0 + 0x4u)));
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 0);
        c.V0 = (uint)(short)mem.ReadU16((c.S3 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S4 + c.V0;
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.V0 + 0x4u)));
        c.T4 = RecompOne.Runtime.Gte.Read(9);
       
        c.T5 = RecompOne.Runtime.Gte.Read(10);
       
        c.T6 = RecompOne.Runtime.Gte.Read(11);
       
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A1 + 0x10u), c.V0);
       
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 0);
        c.T4 = RecompOne.Runtime.Gte.Read(9);
       
        c.T5 = RecompOne.Runtime.Gte.Read(10);
       
        c.T6 = RecompOne.Runtime.Gte.Read(11);
       
        mem.WriteU16(c.T1, (ushort)c.T4);
        mem.WriteU16((c.T1 + 0x2u), (ushort)c.T5);
        mem.WriteU16((c.T1 + 0x4u), (ushort)c.T6);
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        mem.WriteU32((c.A1 + 0x18u), c.V0);
       
        c.V0 = mem.ReadU32((c.A2 + 0x18u));
        mem.WriteU32((c.A1 + 0x20u), c.V0);
       
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x10u));
        c.V1 = mem.ReadU32((c.A2 + 0x20u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            mem.WriteU32((c.A1 + 0x28u), c.V1);
           
            goto L80025BC0;
        }
        mem.WriteU32((c.A1 + 0x28u), c.V1);
       
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x18u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            goto L80025BC0;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x20u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 == 0u) {
            goto L80025BC0;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A1 + 0x28u));
        c.V0 = CullBias(c.V0) & c.A3;
        if (c.V0 != 0u) {
            goto L80025C00;
        }
        L80025BC0: ;
        c.A0 = mem.ReadU32((c.SP + 0x50u));
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.V1 = mem.ReadU32(c.A1);
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.V0;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.V1 & c.T7;
        c.V0 = c.V0 & c.T0;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A1, c.V1);
       
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.A1 & c.T0;
        c.V0 = c.V0 & c.T7;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
       
        L80025C00: ;
        c.T2 = c.T2 + 0x1u;
        c.T3 = c.T3 + 0x50u;
        c.S2 = c.S2 + 0x50u;
        c.V0 = mem.ReadU32((c.S1 + 0x34u));
        c.V0 = (int)c.T2 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S3 = c.S3 + 0x60u;
            goto L80025954;
        }
        c.S3 = c.S3 + 0x60u;
        L80025C20: ;
        c.RA = mem.ReadU32((c.SP + 0x74u));
        c.S6 = mem.ReadU32((c.SP + 0x70u));
        c.S5 = mem.ReadU32((c.SP + 0x6Cu));
        c.S4 = mem.ReadU32((c.SP + 0x68u));
        c.S3 = mem.ReadU32((c.SP + 0x64u));
        c.S2 = mem.ReadU32((c.SP + 0x60u));
        c.S1 = mem.ReadU32((c.SP + 0x5Cu));
        c.S0 = mem.ReadU32((c.SP + 0x58u));
        c.SP = c.SP + 0x78u;
        return;
    }
    
    public static void func_80010800_tl(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x148u;
        mem.WriteU32((c.SP + 0x130u), c.S4);
       
        c.S4 = c.A0 + 0u;
        mem.WriteU32((c.SP + 0x124u), c.S1);
       
        c.S1 = 0x1F800000u;
        mem.WriteU32((c.SP + 0x120u), c.S0);
       
        c.S0 = 0x1F800000u;
        mem.WriteU32((c.SP + 0x144u), c.RA);
       
        mem.WriteU32((c.SP + 0x140u), c.FP);
       
        mem.WriteU32((c.SP + 0x13Cu), c.S7);
       
        mem.WriteU32((c.SP + 0x138u), c.S6);
       
        mem.WriteU32((c.SP + 0x134u), c.S5);
       
        mem.WriteU32((c.SP + 0x12Cu), c.S3);
       
        mem.WriteU32((c.SP + 0x128u), c.S2);
       
        c.A1 = mem.ReadU32((c.S4 + 0x4u));
        c.A0 = mem.ReadU32((c.S1 + 0x88u));
        c.A2 = mem.ReadU32((c.S0 + 0x34u));
        c.A0 = c.A0 + 0xA0u;
        c.RA = RAddr.Tl_Code_8001084C;
        MediEvil_game.func_800A4EB8(c, m);
        c.A1 = c.SP + 0x10u;
        c.V0 = mem.ReadU32((c.S4 + 0x4u));
        c.A3 = mem.ReadU32((c.S1 + 0x88u));
        c.V0 = mem.ReadU16((c.V0 + 0x14u));
        c.V1 = mem.ReadU16((c.A3 + 0xB4u));
        c.A2 = mem.ReadU32((c.S0 + 0x34u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S4 + 0x4u));
        c.V1 = mem.ReadU16((c.A3 + 0xB8u));
        c.V0 = mem.ReadU16((c.V0 + 0x18u));
        c.A0 = c.A3 + 0xA0u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S4 + 0x4u));
        c.V1 = mem.ReadU16((c.A3 + 0xBCu));
        c.V0 = mem.ReadU16((c.V0 + 0x1Cu));
        c.A2 = c.A2 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.RA = RAddr.Tl_Code_800108A0;
        MediEvil_game.func_800A48FC(c, m);
        c.S0 = c.S0 + 0x34u;
        c.T7 = mem.ReadU32(c.S0);
        c.T4 = mem.ReadU32(c.T7);
        c.T5 = mem.ReadU32((c.T7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T7 + 0x8u));
        c.T5 = mem.ReadU32((c.T7 + 0xCu));
        c.T6 = mem.ReadU32((c.T7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T7 + 0x14u));
        c.T5 = mem.ReadU32((c.T7 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T7 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.V0 = mem.ReadU32((c.S4 + 0x30u));
        c.T8 = mem.ReadU32((c.S4 + 0x30u));
        c.V1 = c.V0 + 0x8u;
        c.V0 = c.V0 + 0x10u;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.T8));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.T8 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.V1));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.V1 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.V0 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.A1 = c.SP + 0x18u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.A1; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw); }
        c.A0 = c.SP + 0x20u;
        { var _sw = RecompOne.Runtime.Gte.Read(13); var _ad = c.A0; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(13, _ad, _sw); }
        c.V0 = c.SP + 0x28u;
        { var _sw = RecompOne.Runtime.Gte.Read(14); var _ad = c.V0; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(14, _ad, _sw); }
        c.V1 = 0xFF000000u;
        c.V0 = mem.ReadU32((c.SP + 0x18u));
        c.V1 = c.V1 | CullMaskX;
        c.V0 = CullBias(c.V0) & c.V1;
        if (c.V0 == 0u) {
            c.V0 = c.SP + 0xE8u;
            goto L800109E4;
        }
        c.V0 = c.SP + 0xE8u;
        c.V0 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = CullBias(c.V0) & c.V1;
        if (c.V0 == 0u) {
            c.V0 = c.SP + 0xE8u;
            goto L800109E4;
        }
        c.V0 = c.SP + 0xE8u;
        c.V0 = mem.ReadU32((c.SP + 0x28u));
        c.V0 = CullBias(c.V0) & c.V1;
        if (c.V0 == 0u) {
            c.V0 = c.SP + 0xE8u;
            goto L800109E4;
        }
        c.V0 = c.SP + 0xE8u;
        c.T7 = 0x800F0000u;
        c.T7 = c.T7 - RAddr.Tl_8001097C;
        c.T8 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.S4 + 0x30u));
        c.T8 = c.T8 - RAddr.Tl_80010988;
        c.V0 = c.V0 + 0x18u;
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.V0));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.V0 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.T7));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.T7 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.T8));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.T8 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.A1; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); var _ad = c.A0; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(13, _ad, _sw); }
        c.V0 = mem.ReadU32((c.SP + 0x18u));
        c.V0 = CullBias(c.V0) & c.V1;
        if (c.V0 == 0u) {
            c.V0 = c.SP + 0xE8u;
            goto L800109E4;
        }
        c.V0 = c.SP + 0xE8u;
        c.V0 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = CullBias(c.V0) & c.V1;
        if (c.V0 != 0u) {
            c.V0 = c.SP + 0xE8u;
            goto L80011074;
        }
        c.V0 = c.SP + 0xE8u;
        L800109E4: ;
        mem.WriteU32(c.V0, RecompOne.Runtime.Gte.Read(19));
        c.V0 = 0x800F0000u;
        c.A0 = (uint)(short)mem.ReadU16((c.V0 - RAddr.Tl_800109EC));
        c.A2 = mem.ReadU32((c.SP + 0xE8u));
        c.V0 = (int)c.A0 < (int)c.A2 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V1 = c.A0 - c.A2;
            goto L80011074;
        }
        c.V1 = c.A0 - c.A2;
        c.V1 = c.V1 << 16;
        if (c.A0 != 0u) { if ((int)c.V1 == int.MinValue && (int)c.A0 == -1) { c.LO = 0x80000000u; c.HI = 0u; } else { c.LO = (uint)((int)c.V1 / (int)c.A0); c.HI = (uint)((int)c.V1 % (int)c.A0); } }
        c.V1 = c.LO;
        c.V0 = 0x1F800000u;
        c.A0 = 0x80020000u;
        c.A1 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V0 = mem.ReadU32((c.A0 - RAddr.Tl_80010A1C));
        c.A0 = (uint)((int)c.A2 >> (int)(c.A1 & 31u));
        c.V0 = (int)c.A0 < (int)c.V0 ? 1u : 0u;
        mem.WriteU32((c.SP + 0xE8u), c.A0);
       
        if (c.V0 != 0u) {
            mem.WriteU32((c.S4 + 0x9Cu), c.V1);
           
            goto L80011074;
        }
        mem.WriteU32((c.S4 + 0x9Cu), c.V1);
       
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x88u));
        c.V0 = mem.ReadU16((c.V0 + 0x68u));
        c.V1 = c.A0 + 0u;
        c.A0 = c.V0 - 0x1u;
        c.V0 = (int)c.A0 < (int)c.V1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L80010A5C;
        }
        c.V1 = c.A0 + 0u;
        L80010A5C: ;
        c.V0 = mem.ReadU32(c.S4);
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.S4 + 0x98u), c.V1);
           
            goto L80010A78;
        }
        mem.WriteU32((c.S4 + 0x98u), c.V1);
       
        c.A0 = c.S4 + 0u;
        c.RA = RAddr.Tl_Code_80010A78;
        PalDispatch.Tl[RAddr.Tl_Code_800104BC](c, m);
        L80010A78: ;
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - RAddr.Tl_80010A7C));
        if (c.V0 != 0u) {
            goto L80010B38;
        }
        c.A0 = mem.ReadU32(c.S4);
        c.V0 = c.A0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L80010B38;
        }
        c.V1 = mem.ReadU32((c.S4 + 0xACu));
        if (c.V1 == 0u) {
            c.V0 = c.A0 & 0x0004u;
            goto L80010B0C;
        }
        c.V0 = c.A0 & 0x0004u;
        if (c.V0 == 0u) {
            goto L80010AEC;
        }
        c.V0 = mem.ReadU32((c.S4 + 0xA8u));
        c.A0 = mem.ReadU32((c.S4 + 0xA8u));
        c.V0 = (uint)((int)c.V0 >> 5);
        c.V0 = c.V1 + c.V0;
        mem.WriteU32((c.S4 + 0xACu), c.V0);
       
        c.V0 = (int)c.V0 < (int)c.A0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V1 = 0xFFFFFFFBu;
            goto L80010B38;
        }
        c.V1 = 0xFFFFFFFBu;
        c.V0 = mem.ReadU32(c.S4);
        mem.WriteU32((c.S4 + 0xACu), c.A0);
       
        c.V0 = CullBias(c.V0) & c.V1;
        mem.WriteU32(c.S4, c.V0);
       
        goto L80010B38;
        L80010AEC: ;
        c.V0 = mem.ReadU32((c.S4 + 0xA8u));
        c.V0 = (uint)((int)c.V0 >> 5);
        c.V0 = c.V1 - c.V0;
        if ((int)c.V0 > 0) {
            mem.WriteU32((c.S4 + 0xACu), c.V0);
           
            goto L80010B38;
        }
        mem.WriteU32((c.S4 + 0xACu), c.V0);
       
        mem.WriteU32((c.S4 + 0xACu), 0u);
        goto L80010B38;
        L80010B0C: ;
        c.RA = RAddr.Tl_Code_80010B14;
        MediEvil_game.func_800A4434(c, m);
        c.V0 = c.V0 & 0x003Fu;
        if (c.V0 != 0u) {
            goto L80010B38;
        }
        c.V0 = mem.ReadU32((c.S4 + 0xA8u));
        c.V1 = mem.ReadU32(c.S4);
        c.V0 = (uint)((int)c.V0 >> 5);
        c.V1 = c.V1 | 0x0004u;
        mem.WriteU32((c.S4 + 0xACu), c.V0);
       
        mem.WriteU32(c.S4, c.V1);
       
        L80010B38: ;
        c.V1 = mem.ReadU32((c.S4 + 0x8Cu));
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T0 = 0x00FF0000u;
        c.T0 = c.T0 | 0xFFFFu;
        c.V0 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.V0 + 0x4u));
        c.A3 = 0xFF000000u;
        c.A2 = c.A1 << 2;
        c.A2 = c.S4 + c.A2;
        c.A1 = c.A1 << 5;
        c.A1 = c.A1 + 0x44u;
        c.A1 = c.S4 + c.A1;
        c.V0 = mem.ReadU32((c.A2 + 0x34u));
        c.V1 = c.LO;
        c.A0 = c.V1 << 1;
        c.A0 = c.A0 + c.V1;
        c.A0 = c.A0 << 5;
        c.A0 = c.A0 + c.V0;
        c.V0 = 0x1F800000u;
        c.T1 = mem.ReadU32((c.V0 + 0x88u));
        c.V0 = mem.ReadU32((c.S4 + 0x98u));
        c.V1 = mem.ReadU32((c.T1 + 0x78u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = mem.ReadU32((c.A0 - 0x20u));
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.A3;
        c.V0 = c.V0 & c.T0;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32((c.A0 - 0x20u), c.V1);
       
        c.V1 = mem.ReadU32((c.S4 + 0x98u));
        c.V0 = mem.ReadU32((c.T1 + 0x78u));
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        c.A1 = c.A1 & c.T0;
        c.V0 = c.V0 & c.A3;
        c.V0 = c.V0 | c.A1;
        mem.WriteU32(c.V1, c.V0);
       
        c.T4 = mem.ReadU32((c.S4 + 0x28u));
        c.S7 = mem.ReadU32((c.S4 + 0x2Cu));
        c.T7 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = c.T4 + 0x8u;
        mem.WriteU32((c.SP + 0x118u), c.T7);
       
        c.V0 = mem.ReadU32((c.S4 + 0x94u));
        c.T3 = mem.ReadU32((c.A2 + 0x3Cu));
        c.V0 = (int)c.V0 < 4096 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.FP = c.S7 + 0x8u;
            goto L80010C18;
        }
        c.FP = c.S7 + 0x8u;
        c.V0 = mem.ReadU32((c.S4 + 0x8Cu));
        c.V0 = c.V0 + 0x1u;
        mem.WriteU32((c.SP + 0x114u), c.V0);
       
        goto L80010C24;
        L80010C18: ;
        c.T8 = mem.ReadU32((c.S4 + 0x8Cu));
        mem.WriteU32((c.SP + 0x114u), c.T8);
       
        L80010C24: ;
        c.T7 = mem.ReadU32((c.SP + 0x114u));
        if ((int)c.T7 <= 0) {
            c.T5 = 0u + 0u;
            goto L80011074;
        }
        c.T5 = 0u + 0u;
        c.T9 = 0x800F0000u;
        c.V0 = 0x800E0000u;
        c.T6 = c.V0 + RAddr.Tl_80010C3C;
        c.T2 = c.SP + 0xC0u;
        c.T1 = c.T4 + 0x4u;
        L80010C48: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S5 = c.SP + 0x38u;
        c.V1 = mem.ReadU32((c.S4 + 0x90u));
        c.S0 = 0x00000001u;
        c.V0 = c.V1 + c.S0;
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.S3 = c.SP + 0x80u;
            goto L80010F4C;
        }
        c.S3 = c.SP + 0x80u;
        c.S6 = 0x00000050u;
        c.S2 = c.SP + 0x84u;
        c.S1 = c.SP + 0x3Cu;
        L80010C70: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.A0 = c.S0 << 12;
        if (c.V1 != 0u) { if ((int)c.A0 == int.MinValue && (int)c.V1 == -1) { c.LO = 0x80000000u; c.HI = 0u; } else { c.LO = (uint)((int)c.A0 / (int)c.V1); c.HI = (uint)((int)c.A0 % (int)c.V1); } }
        c.A0 = c.LO;
        c.V0 = (uint)(short)mem.ReadU16(c.S7);
        c.V1 = (uint)(short)mem.ReadU16(c.T4);
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16(c.T4);
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S5, (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.S7 + 0x2u));
        c.V1 = (uint)(short)mem.ReadU16((c.T1 - 0x2u));
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16((c.T1 - 0x2u));
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.S1 - 0x2u), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.S7 + 0x4u));
        c.V1 = (uint)(short)mem.ReadU16(c.T1);
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16(c.T1);
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S1, (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16(c.FP);
        c.V1 = (uint)(short)mem.ReadU16(c.T0);
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16(c.T0);
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S3, (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.FP + 0x2u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x2u));
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16((c.T0 + 0x2u));
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.S2 - 0x2u), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.FP + 0x4u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x4u));
        c.V0 = c.V0 - c.V1;
        { var _r = (long)(int)c.V0 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16((c.T0 + 0x4u));
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S2, (ushort)c.V0);
        c.V0 = mem.ReadU32(c.S4);
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L80010E90;
        }
        c.A1 = mem.ReadU32((c.S4 + 0xACu));
        if (c.A1 == 0u) {
            goto L80010E90;
        }
        c.V1 = mem.ReadU32((c.T9 - RAddr.Tl_80010D90));
        c.V0 = mem.ReadU32((c.S4 + 0xA0u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T7 = c.LO;
        mem.WriteU32((c.SP + 0x11Cu), c.T7);
       
        c.V0 = mem.ReadU32((c.S4 + 0xA4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = (uint)(short)mem.ReadU16(c.S5);
        c.V1 = (uint)(short)mem.ReadU16((c.S1 - 0x2u));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.T7;
        c.V1 = c.V1 << 4;
        c.A0 = c.LO;
        c.A0 = c.V1 + c.A0;
        c.V1 = c.V0 & 0x0FFFu;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.T6;
        c.V0 = c.A0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T6;
        c.V1 = (uint)(short)mem.ReadU16(c.V1);
        c.V0 = (uint)(short)mem.ReadU16(c.V0);
        c.V1 = c.V1 + c.V0;
        { var _r = (long)(int)c.V1 * (int)c.A1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16(c.S1);
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S1, (ushort)c.V0);
        c.V1 = mem.ReadU32((c.T9 - RAddr.Tl_80010E10));
        c.V0 = mem.ReadU32((c.S4 + 0xA0u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T7 = c.LO;
        mem.WriteU32((c.SP + 0x11Cu), c.T7);
       
        c.V0 = mem.ReadU32((c.S4 + 0xA4u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = (uint)(short)mem.ReadU16(c.S3);
        c.V1 = (uint)(short)mem.ReadU16((c.S2 - 0x2u));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.T7;
        c.V1 = c.V1 << 4;
        c.A0 = c.LO;
        c.A0 = c.V1 + c.A0;
        c.V1 = c.V0 & 0x0FFFu;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.T6;
        c.V0 = c.A0 & 0x0FFFu;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T6;
        c.V1 = (uint)(short)mem.ReadU16(c.V1);
        c.V0 = (uint)(short)mem.ReadU16(c.V0);
        c.A0 = mem.ReadU32((c.S4 + 0xACu));
        c.V1 = c.V1 + c.V0;
        { var _r = (long)(int)c.V1 * (int)c.A0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = mem.ReadU16(c.S2);
        c.V1 = c.LO;
        c.V1 = (uint)((int)c.V1 >> 12);
        c.V0 = c.V0 + c.V1;
        mem.WriteU16(c.S2, (ushort)c.V0);
        L80010E90: ;
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        if (c.V0 != 0u) { if ((int)c.S6 == int.MinValue && (int)c.V0 == -1) { c.LO = 0x80000000u; c.HI = 0u; } else { c.LO = (uint)((int)c.S6 / (int)c.V0); c.HI = (uint)((int)c.S6 % (int)c.V0); } }
        c.V0 = c.LO;
        c.V1 = mem.ReadU32((c.S4 + 0x9Cu));
        c.V0 = c.V0 + 0x8u;
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.S0 << 2;
        c.V0 = c.T2 + c.V0;
        c.V1 = c.LO;
        c.A3 = (uint)((int)c.V1 >> 16);
        mem.WriteU32(c.V0, c.A3);
       
        c.V0 = mem.ReadU32((c.S4 + 0x8Cu));
        c.V0 = (int)c.T5 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.S5 + 0u;
            goto L80010F24;
        }
        c.A0 = c.S5 + 0u;
        c.A2 = mem.ReadU32((c.SP + 0x118u));
        c.A1 = c.S3 + 0u;
        mem.WriteU32((c.SP + 0xF0u), c.T0);
       
        mem.WriteU32((c.SP + 0xF4u), c.T1);
       
        mem.WriteU32((c.SP + 0xF8u), c.T2);
       
        mem.WriteU32((c.SP + 0xFCu), c.T3);
       
        mem.WriteU32((c.SP + 0x100u), c.T4);
       
        mem.WriteU32((c.SP + 0x104u), c.T5);
       
        mem.WriteU32((c.SP + 0x108u), c.T6);
       
        mem.WriteU32((c.SP + 0x110u), c.T9);
       
        c.RA = RAddr.Tl_Code_80010F00;
        PalDispatch.Tl[RAddr.Tl_Code_800105B8](c, m);
        mem.WriteU32((c.SP + 0x118u), c.V0);
       
        c.T9 = mem.ReadU32((c.SP + 0x110u));
        c.T6 = mem.ReadU32((c.SP + 0x108u));
        c.T5 = mem.ReadU32((c.SP + 0x104u));
        c.T4 = mem.ReadU32((c.SP + 0x100u));
        c.T3 = mem.ReadU32((c.SP + 0xFCu));
        c.T2 = mem.ReadU32((c.SP + 0xF8u));
        c.T1 = mem.ReadU32((c.SP + 0xF4u));
        c.T0 = mem.ReadU32((c.SP + 0xF0u));
        L80010F24: ;
        c.S1 = c.S1 + 0x8u;
        c.S5 = c.S5 + 0x8u;
        c.S2 = c.S2 + 0x8u;
        c.S3 = c.S3 + 0x8u;
        c.V1 = mem.ReadU32((c.S4 + 0x90u));
        c.S0 = c.S0 + 0x1u;
        c.V0 = c.V1 + 0x1u;
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S6 = c.S6 + 0x50u;
            goto L80010C70;
        }
        c.S6 = c.S6 + 0x50u;
        L80010F4C: ;
        c.V0 = mem.ReadU32(c.T4);
        c.S5 = c.SP + 0x30u;
        mem.WriteU32((c.SP + 0x30u), c.V0);
       
        c.V0 = mem.ReadU16(c.T1);
        mem.WriteU16((c.S5 + 0x4u), (ushort)c.V0);
        c.V0 = 0x52000000u;
        mem.WriteU32((c.SP + 0xC0u), c.V0);
       
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        c.S0 = 0x00000001u;
        c.V0 = c.V0 + c.S0;
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.S3 = c.SP + 0x38u;
            goto L80010FC4;
        }
        c.S3 = c.SP + 0x38u;
        c.A1 = 0x52000000u;
        c.A0 = c.T2 + 0x4u;
        L80010F8C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32(c.A0);
        c.S0 = c.S0 + 0x1u;
        c.V1 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        c.V1 = c.V1 << 8;
        c.V1 = c.V1 + c.V0;
        c.V1 = c.V1 + c.A1;
        mem.WriteU32(c.A0, c.V1);
       
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        c.V0 = c.V0 + 0x1u;
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.A0 = c.A0 + 0x4u;
            goto L80010F8C;
        }
        c.A0 = c.A0 + 0x4u;
        L80010FC4: ;
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        if ((int)c.V0 <= 0) {
            c.S0 = 0u + 0u;
            goto L80011050;
        }
        c.S0 = 0u + 0u;
        c.A0 = c.T3 + 0x10u;
        L80010FD8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        RecompOne.Runtime.Gte.Write(0, mem.ReadU32(c.S5));
        RecompOne.Runtime.Gte.Write(1, mem.ReadU32((c.S5 + 0x4u)));
        RecompOne.Runtime.Gte.Write(2, mem.ReadU32(c.S3));
        RecompOne.Runtime.Gte.Write(3, mem.ReadU32((c.S3 + 0x4u)));
        RecompOne.Runtime.Gte.Write(4, mem.ReadU32(c.S3));
        RecompOne.Runtime.Gte.Write(5, mem.ReadU32((c.S3 + 0x4u)));
        TerrainFloatProjection.Rtpt(12, false);
        c.V0 = c.S0 << 2;
        c.V0 = c.T2 + c.V0;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.S0 + 0x1u;
        mem.WriteU32((c.A0 - 0xCu), c.V0);
       
        c.V0 = c.V1 << 2;
        c.V0 = c.T2 + c.V0;
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.A0 - 0x4u), c.V0);
       
        c.V0 = c.T3 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); var _ad = c.V0; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(12, _ad, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); var _ad = c.A0; mem.WriteU32(_ad, _sw); TerrainFloatProjection.Store(13, _ad, _sw); }
        c.A0 = c.A0 + 0x14u;
        c.T3 = c.T3 + 0x14u;
        c.S5 = c.S5 + 0x8u;
        c.V0 = mem.ReadU32((c.S4 + 0x90u));
        c.S0 = c.V1 + 0u;
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S3 = c.S3 + 0x8u;
            goto L80010FD8;
        }
        c.S3 = c.S3 + 0x8u;
        L80011050: ;
        c.T1 = c.T1 + 0x8u;
        c.T4 = c.T4 + 0x8u;
        c.T0 = c.T0 + 0x8u;
        c.S7 = c.S7 + 0x8u;
        c.T7 = mem.ReadU32((c.SP + 0x114u));
        c.T5 = c.T5 + 0x1u;
        c.V0 = (int)c.T5 < (int)c.T7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.FP = c.FP + 0x8u;
            goto L80010C48;
        }
        c.FP = c.FP + 0x8u;
        L80011074: ;
        c.RA = mem.ReadU32((c.SP + 0x144u));
        c.FP = mem.ReadU32((c.SP + 0x140u));
        c.S7 = mem.ReadU32((c.SP + 0x13Cu));
        c.S6 = mem.ReadU32((c.SP + 0x138u));
        c.S5 = mem.ReadU32((c.SP + 0x134u));
        c.S4 = mem.ReadU32((c.SP + 0x130u));
        c.S3 = mem.ReadU32((c.SP + 0x12Cu));
        c.S2 = mem.ReadU32((c.SP + 0x128u));
        c.S1 = mem.ReadU32((c.SP + 0x124u));
        c.S0 = mem.ReadU32((c.SP + 0x120u));
        c.SP = c.SP + 0x148u;
        return;
    }

    public static void func_8009A540(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x18u;
        c.A3 = c.A0 + 0u;
        c.T4 = 0u + 0u;
        c.T3 = 0x00000001u;
        c.T2 = 0x00000002u;
        L8009A554: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.A0 = c.A0 + 0x8u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        if (c.T2 != 0u) {
            c.A0 = c.A0 + 0x8u;
            goto L8009A5CC;
        }
        c.A0 = c.A0 + 0x8u;
        c.V0 = (uint)(short)mem.ReadU16(c.A3);
        c.V1 = (uint)(short)mem.ReadU16((c.A3 + 0x38u));
        c.V0 = c.V0 + c.V1;
        c.V0 = (uint)((int)c.V0 >> 1);
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x2u));
        c.V1 = (uint)(short)mem.ReadU16((c.A3 + 0x3Au));
        c.V0 = c.V0 + c.V1;
        c.V0 = (uint)((int)c.V0 >> 1);
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x4u));
        c.V1 = (uint)(short)mem.ReadU16((c.A3 + 0x3Cu));
        c.V0 = c.V0 + c.V1;
        c.V0 = (uint)((int)c.V0 >> 1);
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = c.SP + 0x10u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L8009A5D8;
        L8009A5CC: ;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.A0 = c.A0 + 0x8u;
        L8009A5D8: ;
        TerrainFloatProjection.Rtpt(12, false);
        c.A2 = c.SP + 0u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.SP, _sw); TerrainFloatProjection.Store(12, c.SP, _sw); }
        c.V0 = c.SP + 0x4u;
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V0, _sw); TerrainFloatProjection.Store(13, c.V0, _sw); }
        if (c.T2 != 0u) {
            c.V0 = c.SP + 0x8u;
            goto L8009A604;
        }
        c.V0 = c.SP + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw); }
        c.V0 = c.SP + 0x8u;
        L8009A604: ;
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); TerrainFloatProjection.Store(14, c.V0, _sw); }
        c.V1 = 0x00000002u;
        c.T1 = 0x1F800000u;
        c.T1 = mem.ReadU16((c.T1 + 0x98u));
        c.T1 = (uint)((int)c.T1 + CullEdgeX(mem));
        c.T0 = 0x1F800000u;
        c.T0 = mem.ReadU16((c.T0 + 0x9Au));
        L8009A61C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.A2);
        if ((int)c.V0 < -CullEdgeX(mem)) {
            c.V0 = (int)c.V0 < (int)c.T1 ? 1u : 0u;
            goto L8009A650;
        }
        c.V0 = (int)c.V0 < (int)c.T1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L8009A650;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        if ((int)c.V0 < 0) {
            c.V0 = (int)c.V0 < (int)c.T0 ? 1u : 0u;
            goto L8009A650;
        }
        c.V0 = (int)c.V0 < (int)c.T0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L8009A650;
        }
        c.T4 = c.T4 | c.T3;
        L8009A650: ;
        c.T3 = c.T3 << 1;
        c.A2 = c.A2 + 0x4u;
        c.V0 = c.V1 + 0u;
        if (c.V0 != 0u) {
            c.V1 = c.V1 - 0x1u;
            goto L8009A61C;
        }
        c.V1 = c.V1 - 0x1u;
        c.V0 = c.T2 + 0u;
        if (c.V0 != 0u) {
            c.T2 = c.T2 - 0x1u;
            goto L8009A554;
        }
        c.T2 = c.T2 - 0x1u;
        if (c.T4 == 0u) {
            c.V1 = 0x000000FFu;
            goto L8009A688;
        }
        c.V1 = 0x000000FFu;
        if (c.T4 == c.V1) {
            c.V0 = 0x00000002u;
            goto L8009A68C;
        }
        c.V0 = 0x00000002u;
        c.V0 = 0x00000001u;
        goto L8009A68C;
        L8009A688: ;
        c.V0 = 0u + 0u;
        L8009A68C: ;
        c.SP = c.SP + 0x18u;
        return;
    }

    public static void func_8009A694(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x20u;
        c.V1 = c.SP + 0u;
        c.A2 = 0x00000002u;
        L8009A6A0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.A0 = c.A0 + 0x8u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        if (c.A2 != 0u) {
            c.A0 = c.A0 + 0x8u;
            goto L8009A6D4;
        }
        c.A0 = c.A0 + 0x8u;
        c.T0 = 0x800F0000u;
        c.T0 = c.T0 - RAddr.Game_8009A6C0;
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L8009A6E0;
        L8009A6D4: ;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.A0 = c.A0 + 0x8u;
        L8009A6E0: ;
        TerrainFloatProjection.Rtpt(12, false);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V1, _sw); TerrainFloatProjection.Store(12, c.V1, _sw); }
        c.V1 = c.V1 + 0x4u;
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); TerrainFloatProjection.Store(13, c.V1, _sw); }
        if (c.A2 != 0u) {
            c.V1 = c.V1 + 0x4u;
            goto L8009A70C;
        }
        c.V1 = c.V1 + 0x4u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw); }
        c.V0 = c.A2 + 0u;
        goto L8009A718;
        L8009A70C: ;
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V1, _sw); TerrainFloatProjection.Store(14, c.V1, _sw); }
        c.V1 = c.V1 + 0x4u;
        c.V0 = c.A2 + 0u;
        L8009A718: ;
        if (c.V0 != 0u) {
            c.A2 = c.A2 - 0x1u;
            goto L8009A6A0;
        }
        c.A2 = c.A2 - 0x1u;
        c.A2 = 0u + 0u;
        c.A1 = c.A2 + 0u;
        c.V1 = c.SP + 0u;
        c.A0 = 0x00000008u;
        L8009A730: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.V1);
        c.V1 = c.V1 + 0x4u;
        c.A0 = c.A0 - 0x1u;
        c.V0 = (int)c.V0 >= -CullEdgeX(mem) ? 1u : 0u;
        if (c.A0 != 0u) {
            c.A1 = c.A1 + c.V0;
            goto L8009A730;
        }
        c.A1 = c.A1 + c.V0;
        if (c.A1 == 0u) {
            c.A2 = c.A2 + c.A1;
            goto L8009A804;
        }
        c.A2 = c.A2 + c.A1;
        c.A1 = 0u + 0u;
        c.V1 = c.SP + 0u;
        c.A0 = 0x00000008u;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU16((c.A3 + 0x98u));
        c.A3 = (uint)((int)c.A3 + CullEdgeX(mem));
        L8009A768: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.V1);
        c.V1 = c.V1 + 0x4u;
        c.A0 = c.A0 - 0x1u;
        c.V0 = (int)c.V0 < (int)c.A3 ? 1u : 0u;
        if (c.A0 != 0u) {
            c.A1 = c.A1 + c.V0;
            goto L8009A768;
        }
        c.A1 = c.A1 + c.V0;
        if (c.A1 == 0u) {
            c.A2 = c.A2 + c.A1;
            goto L8009A804;
        }
        c.A2 = c.A2 + c.A1;
        c.A1 = 0u + 0u;
        c.V1 = c.SP + 0x2u;
        c.A0 = 0x00000008u;
        L8009A794: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.V1);
        c.V1 = c.V1 + 0x4u;
        c.A0 = c.A0 - 0x1u;
        c.V0 = ~(0u | c.V0);
        c.V0 = c.V0 >> 31;
        if (c.A0 != 0u) {
            c.A1 = c.A1 + c.V0;
            goto L8009A794;
        }
        c.A1 = c.A1 + c.V0;
        if (c.A1 == 0u) {
            c.A2 = c.A2 + c.A1;
            goto L8009A804;
        }
        c.A2 = c.A2 + c.A1;
        c.A1 = 0u + 0u;
        c.V1 = c.SP + 0x2u;
        c.A0 = 0x00000008u;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU16((c.A3 + 0x9Au));
        L8009A7CC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = (uint)(short)mem.ReadU16(c.V1);
        c.V1 = c.V1 + 0x4u;
        c.A0 = c.A0 - 0x1u;
        c.V0 = (int)c.A3 < (int)c.V0 ? 1u : 0u;
        c.V0 = c.V0 ^ 0x0001u;
        if (c.A0 != 0u) {
            c.A1 = c.A1 + c.V0;
            goto L8009A7CC;
        }
        c.A1 = c.A1 + c.V0;
        if (c.A1 == 0u) {
            c.A2 = c.A2 + c.A1;
            goto L8009A804;
        }
        c.A2 = c.A2 + c.A1;
        c.V1 = 0x00000020u;
        if (c.A2 == c.V1) {
            c.V0 = 0x00000002u;
            goto L8009A808;
        }
        c.V0 = 0x00000002u;
        c.V0 = 0x00000001u;
        goto L8009A808;
        L8009A804: ;
        c.V0 = 0u + 0u;
        L8009A808: ;
        c.SP = c.SP + 0x20u;
        return;
    }


    public static void func_800B3748(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x70u;
        c.T6 = c.A0;
        c.T4 = c.T6 + 0x10u;
        c.T5 = 0u;
        c.V0 = mem.ReadU32((c.T6 + 0x8u));
        c.V1 = mem.ReadU32((c.T6 + 0xCu));
        c.V0 = c.V0 | 0x0004u;
        if (c.V1 == 0u) {
            mem.WriteU32((c.T6 + 0x8u), c.V0);
            goto L800B4020;
        }
        mem.WriteU32((c.T6 + 0x8u), c.V0);
        c.T3 = 0u | 0xFFFFu;
        c.A3 = 0x80800000u;
        c.A3 = c.A3 | 0x8081u;
        L800B3778: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T2 = mem.ReadU16((c.T4 + 0x8u));
        c.T1 = mem.ReadU32((c.T4 + 0x10u));
        if (c.T2 == 0u) {
            goto L800B4008;
        }
        L800B3788: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T0 = mem.ReadU16((c.T1 + 0x2u));
        c.V0 = mem.ReadU16(c.T1);
        c.V1 = c.V0 & 0xFFFFu;
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.T1 = c.T1 + 0x4u;
            goto L800B3FFC;
        }
        c.T1 = c.T1 + 0x4u;
        c.V0 = 0x800C0000u;
        c.V0 = c.V0 + RAddr.Game_800B37A8;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        switch (c.V0)
        {
            case 0x800B37C4u: goto L800B37C4;
            case 0x800B37D4u: goto L800B37D4;
            case 0x800B37E0u: goto L800B37E0;
            case 0x800B399Cu: goto L800B399C;
            case 0x800B3BC0u: goto L800B3BC0;
            case 0x800B3BCCu: goto L800B3BCC;
            case 0x800B3BDCu: goto L800B3BDC;
            case 0x800B3D98u: goto L800B3D98;
            case 0x800B3FFCu: goto L800B3FFC;
            case 0x800B3FBCu: goto L800B3FBC;
            case 0x800B3FD0u: goto L800B3FD0;
            case 0x800B3FE4u: goto L800B3FE4;
            case 0x800B40B4u: goto L800B40B4;
            case 0x800B40F4u: goto L800B40F4;
            case 0x800B40BCu: goto L800B40BC;
            case 0x800B40D8u: goto L800B40D8;
            case 0x800B40FCu: goto L800B40FC;
            case 0x800B4104u: goto L800B4104;
            case 0x800B411Cu: goto L800B411C;
            case 0x800B4140u: goto L800B4140;
            case 0x800B40D0u: goto L800B40D0;
            case 0x800B413Cu: goto L800B413C;
            case 0x800B49ECu: goto L800B49EC;
            case 0x800B4A40u: goto L800B4A40;
            case 0x800B4A94u: goto L800B4A94;
            case 0x800B4AE8u: goto L800B4AE8;
            case 0x800B4B3Cu: goto L800B4B3C;
            case 0x800B4B90u: goto L800B4B90;
            case 0x800B4BE4u: goto L800B4BE4;
            case 0x800B4C38u: goto L800B4C38;
            case 0x800B4D34u: goto L800B4D34;
            case 0x800B4C8Cu: goto L800B4C8C;
            case 0x800B4CE0u: goto L800B4CE0;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B37C4: ;
        c.V1 = c.T0 & 0xFFFFu;
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        goto L800B3FF0;
        L800B37D4: ;
        c.V0 = c.T0 & 0xFFFFu;
        c.V0 = c.V0 << 4;
        goto L800B3FF4;
        L800B37E0: ;
        c.V0 = c.T0 - 0x1u;
        c.T0 = c.V0;
        c.V0 = c.V0 & 0xFFFFu;
        if (c.V0 == c.T3) {
            c.V0 = c.T2 & 0xFFFFu;
            goto L800B4000;
        }
        c.V0 = c.T2 & 0xFFFFu;
        c.A2 = c.T1 + 0x11u;
        L800B37F8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16((c.A2 + 0x1u));
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_800B37FC));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.A1 = mem.ReadU32(c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0xAu));
        mem.WriteU16((c.A2 - 0x3u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0x6u));
        mem.WriteU16((c.A2 - 0x7u), (ushort)c.V0);
        c.V0 = mem.ReadU8((c.A2 - 0x9u));
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x8u));
        mem.WriteU8((c.A2 - 0x9u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x5u));
        mem.WriteU8((c.A2 - 0x8u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x4u));
        mem.WriteU8((c.A2 - 0x5u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x1u));
        mem.WriteU8((c.A2 - 0x4u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        mem.WriteU8((c.A2 - 0x1u), (byte)c.V1);
        c.V1 = mem.ReadU8(c.A2);
        c.V0 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        { var _r = (long)(int)c.V1 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T1 = c.T1 + 0x18u;
        c.T2 = c.T2 - 0x1u;
        c.A0 = c.T0 - 0x1u;
        c.T0 = c.A0;
        c.A0 = c.A0 & 0xFFFFu;
        c.V0 = c.HI;
        c.V1 = c.V0 + c.V1;
        c.V0 = mem.ReadU8((c.A1 + 0x5u));
        c.V1 = (uint)((int)c.V1 >> 7);
        c.V0 = c.V0 + c.V1;
        mem.WriteU8(c.A2, (byte)c.V0);
        if (c.A0 != c.T3) {
            c.A2 = c.A2 + 0x18u;
            goto L800B37F8;
        }
        c.A2 = c.A2 + 0x18u;
        c.V0 = c.T2 & 0xFFFFu;
        goto L800B4000;
        L800B399C: ;
        c.V0 = c.T0 - 0x1u;
        c.T0 = c.V0;
        c.V0 = c.V0 & 0xFFFFu;
        if (c.V0 == c.T3) {
            c.V0 = c.T2 & 0xFFFFu;
            goto L800B4000;
        }
        c.V0 = c.T2 & 0xFFFFu;
        c.A2 = c.T1 + 0x17u;
        L800B39B4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16((c.A2 - 0xDu));
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_800B39B8));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.A1 = mem.ReadU32(c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0xAu));
        mem.WriteU16((c.A2 - 0x5u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0x6u));
        mem.WriteU16((c.A2 - 0x9u), (ushort)c.V0);
        c.V0 = mem.ReadU8((c.A2 - 0xBu));
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0xAu));
        mem.WriteU8((c.A2 - 0xBu), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x7u));
        mem.WriteU8((c.A2 - 0xAu), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x6u));
        mem.WriteU8((c.A2 - 0x7u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x3u));
        mem.WriteU8((c.A2 - 0x6u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x2u));
        mem.WriteU8((c.A2 - 0x3u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x1u));
        mem.WriteU8((c.A2 - 0x2u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        mem.WriteU8((c.A2 - 0x1u), (byte)c.V1);
        c.V1 = mem.ReadU8(c.A2);
        c.V0 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.T1 = c.T1 + 0x1Cu;
        c.T2 = c.T2 - 0x1u;
        { var _r = (long)(int)c.V1 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A0 = c.T0 - 0x1u;
        c.T0 = c.A0;
        c.A0 = c.A0 & 0xFFFFu;
        c.V0 = c.HI;
        c.V1 = c.V0 + c.V1;
        c.V0 = mem.ReadU8((c.A1 + 0x5u));
        c.V1 = (uint)((int)c.V1 >> 7);
        c.V0 = c.V0 + c.V1;
        mem.WriteU8(c.A2, (byte)c.V0);
        if (c.A0 != c.T3) {
            c.A2 = c.A2 + 0x1Cu;
            goto L800B39B4;
        }
        c.A2 = c.A2 + 0x1Cu;
        c.V0 = c.T2 & 0xFFFFu;
        goto L800B4000;
        L800B3BC0: ;
        c.V0 = c.T0 & 0xFFFFu;
        c.V0 = c.V0 << 4;
        goto L800B3FF4;
        L800B3BCC: ;
        c.V1 = c.T0 & 0xFFFFu;
        c.V0 = c.V1 << 2;
        c.V0 = c.V0 + c.V1;
        goto L800B3FF0;
        L800B3BDC: ;
        c.V0 = c.T0 - 0x1u;
        c.T0 = c.V0;
        c.V0 = c.V0 & 0xFFFFu;
        if (c.V0 == c.T3) {
            c.V0 = c.T2 & 0xFFFFu;
            goto L800B4000;
        }
        c.V0 = c.T2 & 0xFFFFu;
        c.A2 = c.T1 + 0x15u;
        L800B3BF4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16((c.A2 + 0x1u));
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_800B3BF8));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.A1 = mem.ReadU32(c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0xAu));
        mem.WriteU16((c.A2 - 0x3u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0x6u));
        mem.WriteU16((c.A2 - 0x7u), (ushort)c.V0);
        c.V0 = mem.ReadU8((c.A2 - 0x9u));
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x8u));
        mem.WriteU8((c.A2 - 0x9u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x5u));
        mem.WriteU8((c.A2 - 0x8u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x4u));
        mem.WriteU8((c.A2 - 0x5u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x1u));
        mem.WriteU8((c.A2 - 0x4u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        mem.WriteU8((c.A2 - 0x1u), (byte)c.V1);
        c.V1 = mem.ReadU8(c.A2);
        c.V0 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        { var _r = (long)(int)c.V1 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T1 = c.T1 + 0x1Cu;
        c.T2 = c.T2 - 0x1u;
        c.A0 = c.T0 - 0x1u;
        c.T0 = c.A0;
        c.A0 = c.A0 & 0xFFFFu;
        c.V0 = c.HI;
        c.V1 = c.V0 + c.V1;
        c.V0 = mem.ReadU8((c.A1 + 0x5u));
        c.V1 = (uint)((int)c.V1 >> 7);
        c.V0 = c.V0 + c.V1;
        mem.WriteU8(c.A2, (byte)c.V0);
        if (c.A0 != c.T3) {
            c.A2 = c.A2 + 0x1Cu;
            goto L800B3BF4;
        }
        c.A2 = c.A2 + 0x1Cu;
        c.V0 = c.T2 & 0xFFFFu;
        goto L800B4000;
        L800B3D98: ;
        c.V0 = c.T0 - 0x1u;
        c.T0 = c.V0;
        c.V0 = c.V0 & 0xFFFFu;
        if (c.V0 == c.T3) {
            c.V0 = c.T2 & 0xFFFFu;
            goto L800B4000;
        }
        c.V0 = c.T2 & 0xFFFFu;
        c.A2 = c.T1 + 0x1Bu;
        L800B3DB0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16((c.A2 + 0x1u));
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_800B3DB4));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.A1 = mem.ReadU32(c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0xAu));
        mem.WriteU16((c.A2 - 0x5u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A1 + 0x6u));
        mem.WriteU16((c.A2 - 0x9u), (ushort)c.V0);
        c.V0 = mem.ReadU8((c.A2 - 0xBu));
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0xAu));
        mem.WriteU8((c.A2 - 0xBu), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x7u));
        mem.WriteU8((c.A2 - 0xAu), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x6u));
        mem.WriteU8((c.A2 - 0x7u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x3u));
        mem.WriteU8((c.A2 - 0x6u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x2u));
        mem.WriteU8((c.A2 - 0x3u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x5u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.A2 - 0x1u));
        mem.WriteU8((c.A2 - 0x2u), (byte)c.V1);
        c.V1 = mem.ReadU8((c.A1 + 0x2u));
        { var _r = (long)(int)c.V0 * (int)c.V1; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V0 = c.LO;
        { var _r = (long)(int)c.V0 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.HI;
        c.V0 = c.V1 + c.V0;
        c.V1 = mem.ReadU8((c.A1 + 0x4u));
        c.V0 = (uint)((int)c.V0 >> 7);
        c.V1 = c.V1 + c.V0;
        mem.WriteU8((c.A2 - 0x1u), (byte)c.V1);
        c.V1 = mem.ReadU8(c.A2);
        c.V0 = mem.ReadU8((c.A1 + 0x3u));
        { var _r = (long)(int)c.V1 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.V1 = c.LO;
        c.T1 = c.T1 + 0x24u;
        c.T2 = c.T2 - 0x1u;
        { var _r = (long)(int)c.V1 * (int)c.A3; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.A0 = c.T0 - 0x1u;
        c.T0 = c.A0;
        c.A0 = c.A0 & 0xFFFFu;
        c.V0 = c.HI;
        c.V1 = c.V0 + c.V1;
        c.V0 = mem.ReadU8((c.A1 + 0x5u));
        c.V1 = (uint)((int)c.V1 >> 7);
        c.V0 = c.V0 + c.V1;
        mem.WriteU8(c.A2, (byte)c.V0);
        if (c.A0 != c.T3) {
            c.A2 = c.A2 + 0x24u;
            goto L800B3DB0;
        }
        c.A2 = c.A2 + 0x24u;
        c.V0 = c.T2 & 0xFFFFu;
        goto L800B4000;
        L800B3FBC: ;
        c.V1 = c.T0 & 0xFFFFu;
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 3;
        goto L800B3FF4;
        L800B3FD0: ;
        c.V1 = c.T0 & 0xFFFFu;
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 3;
        goto L800B3FF4;
        L800B3FE4: ;
        c.V1 = c.T0 & 0xFFFFu;
        c.V0 = c.V1 << 3;
        c.V0 = c.V0 - c.V1;
        L800B3FF0: ;
        c.V0 = c.V0 << 2;
        L800B3FF4: ;
        c.T1 = c.T1 + c.V0;
        c.T2 = c.T2 - c.T0;
        L800B3FFC: ;
        c.V0 = c.T2 & 0xFFFFu;
        L800B4000: ;
        if (c.V0 != 0u) {
            goto L800B3788;
        }
        L800B4008: ;
        c.T5 = c.T5 + 0x1u;
        c.V1 = mem.ReadU32((c.T6 + 0xCu));
        c.V0 = c.T5 & 0xFFFFu;
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.T4 = c.T4 + 0x2Cu;
            goto L800B3778;
        }
        c.T4 = c.T4 + 0x2Cu;
        L800B4020: ;
        c.SP = c.SP + 0x70u;
        return;
        c.SP = c.SP - 0x38u;
        mem.WriteU32((c.SP + 0x28u), c.S2);
        c.S2 = c.A0;
        mem.WriteU32((c.SP + 0x24u), c.S1);
        c.S1 = 0u;
        c.V0 = 0xFFFFFFFBu;
        mem.WriteU32((c.SP + 0x30u), c.S4);
        mem.WriteU32((c.SP + 0x2Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S0);
        c.V1 = mem.ReadU32((c.S2 + 0x8u));
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        c.V1 = c.V1 & c.V0;
        if (c.A0 == 0u) {
            mem.WriteU32((c.S2 + 0x8u), c.V1);
            goto L800B4228;
        }
        mem.WriteU32((c.S2 + 0x8u), c.V1);
        c.V0 = 0x800C0000u;
        c.S3 = c.V0 + RAddr.Game_800B4064;
        c.T3 = 0u;
        c.S0 = c.S2 + 0x20u;
        L800B4070: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T9 = mem.ReadU16((c.S0 - 0x8u));
        c.A0 = mem.ReadU32(c.S0);
        if (c.T9 == 0u) {
            goto L800B4210;
        }
        L800B4080: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T5 = 0u;
        c.V0 = mem.ReadU16(c.A0);
        c.T8 = mem.ReadU16((c.A0 + 0x2u));
        c.V1 = c.V0 & 0xFFFFu;
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.A0 + 0x4u;
            goto L800B4140;
        }
        c.A0 = c.A0 + 0x4u;
        c.V0 = c.V1 << 2;
        c.V0 = c.V0 + c.S3;
        c.V0 = mem.ReadU32(c.V0);
        switch (c.V0)
        {
            case 0x800B40B4u: goto L800B40B4;
            case 0x800B40F4u: goto L800B40F4;
            case 0x800B40BCu: goto L800B40BC;
            case 0x800B40D8u: goto L800B40D8;
            case 0x800B40FCu: goto L800B40FC;
            case 0x800B4104u: goto L800B4104;
            case 0x800B411Cu: goto L800B411C;
            case 0x800B4140u: goto L800B4140;
            case 0x800B40D0u: goto L800B40D0;
            case 0x800B413Cu: goto L800B413C;
            case 0x800B49ECu: goto L800B49EC;
            case 0x800B4A40u: goto L800B4A40;
            case 0x800B4A94u: goto L800B4A94;
            case 0x800B4AE8u: goto L800B4AE8;
            case 0x800B4B3Cu: goto L800B4B3C;
            case 0x800B4B90u: goto L800B4B90;
            case 0x800B4BE4u: goto L800B4BE4;
            case 0x800B4C38u: goto L800B4C38;
            case 0x800B4D34u: goto L800B4D34;
            case 0x800B4C8Cu: goto L800B4C8C;
            case 0x800B4CE0u: goto L800B4CE0;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B40B4: ;
        c.A2 = 0x0000000Cu;
        goto L800B4140;
        L800B40BC: ;
        c.T5 = 0x00000003u;
        c.T6 = c.A0 + 0x12u;
        c.T1 = c.A0 + 0x8u;
        c.T0 = c.A0 + 0xCu;
        c.A3 = c.A0 + 0x10u;
        L800B40D0: ;
        c.A2 = 0x00000018u;
        goto L800B4140;
        L800B40D8: ;
        c.T5 = 0x00000004u;
        c.T6 = c.A0 + 0xAu;
        c.T1 = c.A0 + 0xCu;
        c.T0 = c.A0 + 0x10u;
        c.A3 = c.A0 + 0x14u;
        c.T4 = c.A0 + 0x16u;
        goto L800B413C;
        L800B40F4: ;
        c.A2 = 0x00000010u;
        goto L800B4140;
        L800B40FC: ;
        c.A2 = 0x00000014u;
        goto L800B4140;
        L800B4104: ;
        c.T5 = 0x00000003u;
        c.T6 = c.A0 + 0x16u;
        c.T1 = c.A0 + 0xCu;
        c.T0 = c.A0 + 0x10u;
        c.A3 = c.A0 + 0x14u;
        goto L800B413C;
        L800B411C: ;
        c.T5 = 0x00000004u;
        c.T6 = c.A0 + 0x1Cu;
        c.T1 = c.A0 + 0x10u;
        c.T0 = c.A0 + 0x14u;
        c.A3 = c.A0 + 0x18u;
        c.T4 = c.A0 + 0x1Au;
        c.A2 = 0x00000024u;
        goto L800B4140;
        L800B413C: ;
        c.A2 = 0x0000001Cu;
        L800B4140: ;
        if ((int)c.T5 <= 0) {
            c.V0 = c.T8 & 0xFFFFu;
            goto L800B41EC;
        }
        c.V0 = c.T8 & 0xFFFFu;
        c.T2 = c.T8 & 0xFFFFu;
        if ((int)c.T2 < 0) {
            c.V0 = c.A2 << 1;
            goto L800B41E8;
        }
        c.V0 = c.A2 << 1;
        c.T7 = c.V0 - 0x2u;
        c.A1 = c.A2 - 0x1u;
        L800B415C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.T6);
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_800B4160));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = mem.ReadU32(c.V0);
        c.T6 = c.T6 + c.T7;
        mem.WriteU8(c.T1, (byte)0u);
        mem.WriteU8(c.T1, (byte)0u);
        c.T1 = c.T1 + 0x1u;
        mem.WriteU8(c.T1, (byte)0u);
        mem.WriteU8(c.T1, (byte)0u);
        c.T1 = c.T1 + c.A1;
        mem.WriteU8(c.T0, (byte)0u);
        mem.WriteU8(c.T0, (byte)0u);
        c.T0 = c.T0 + 0x1u;
        mem.WriteU8(c.T0, (byte)0u);
        mem.WriteU8(c.T0, (byte)0u);
        c.T0 = c.T0 + c.A1;
        mem.WriteU8(c.A3, (byte)0u);
        mem.WriteU8(c.A3, (byte)0u);
        c.A3 = c.A3 + 0x1u;
        mem.WriteU8(c.A3, (byte)0u);
        mem.WriteU8(c.A3, (byte)0u);
        c.V0 = 0x00000004u;
        if (c.T5 != c.V0) {
            c.A3 = c.A3 + c.A1;
            goto L800B41DC;
        }
        c.A3 = c.A3 + c.A1;
        mem.WriteU8(c.T4, (byte)0u);
        mem.WriteU8(c.T4, (byte)0u);
        c.T4 = c.T4 + 0x1u;
        mem.WriteU8(c.T4, (byte)0u);
        mem.WriteU8(c.T4, (byte)0u);
        c.T4 = c.T4 + c.A1;
        L800B41DC: ;
        c.T2 = c.T2 - 0x1u;
        if ((int)c.T2 >= 0) {
            goto L800B415C;
        }
        L800B41E8: ;
        c.V0 = c.T8 & 0xFFFFu;
        L800B41EC: ;
        { var _r = (long)(int)c.A2 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = c.T9 - c.T8;
        c.S4 = c.LO;
        c.V0 = c.S4 >> 2;
        c.V0 = c.V0 << 2;
        c.A0 = c.A0 + c.V0;
        c.V0 = c.T9 & 0xFFFFu;
        if (c.V0 != 0u) {
            goto L800B4080;
        }
        L800B4210: ;
        c.S1 = c.S1 + 0x1u;
        c.V1 = mem.ReadU32((c.S2 + 0xCu));
        c.V0 = c.S1 & 0xFFFFu;
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S0 = c.S0 + 0x2Cu;
            goto L800B4070;
        }
        c.S0 = c.S0 + 0x2Cu;
        L800B4228: ;
        c.S4 = mem.ReadU32((c.SP + 0x30u));
        c.S3 = mem.ReadU32((c.SP + 0x2Cu));
        c.S2 = mem.ReadU32((c.SP + 0x28u));
        c.S1 = mem.ReadU32((c.SP + 0x24u));
        c.S0 = mem.ReadU32((c.SP + 0x20u));
        c.SP = c.SP + 0x38u;
        return;
        c.SP = c.SP - 0xB8u;
        mem.WriteU32((c.SP + 0xA4u), c.S5);
        c.S5 = c.A0;
        c.V1 = 0x800F0000u;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 - RAddr.Game_800B425C;
        mem.WriteU32((c.SP + 0xB4u), c.RA);
        mem.WriteU32((c.SP + 0xB0u), c.FP);
        mem.WriteU32((c.SP + 0xACu), c.S7);
        mem.WriteU32((c.SP + 0xA8u), c.S6);
        mem.WriteU32((c.SP + 0xA0u), c.S4);
        mem.WriteU32((c.SP + 0x9Cu), c.S3);
        mem.WriteU32((c.SP + 0x98u), c.S2);
        mem.WriteU32((c.SP + 0x94u), c.S1);
        mem.WriteU32((c.SP + 0x90u), c.S0);
        mem.WriteU8((c.SP + 0x84u), (byte)c.A2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x10u));
        c.V0 = mem.ReadU32(c.V0);
        c.S7 = c.A1;
        c.V1 = c.V1 < c.V0 ? 1u : 0u;
        if (c.V1 == 0u) {
            mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
            goto L800B4DD0;
        }
        mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
        c.S2 = mem.ReadU32((c.S5 + 0x8u));
        c.S4 = 0u;
        c.FP = mem.ReadU32((c.S2 + 0x2Cu));
        c.V1 = mem.ReadU16((c.S2 + 0xAu));
        c.T1 = mem.ReadU32((c.FP + 0x4u));
        c.V0 = c.V1 & 0x0020u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0x88u), c.T1);
            goto L800B42DC;
        }
        mem.WriteU32((c.SP + 0x88u), c.T1);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x90u));
        c.S4 = c.S4 < c.V0 ? 1u : 0u;
        L800B42DC: ;
        c.V0 = mem.ReadU16((c.S5 + 0xEu));
        c.V0 = c.V0 & 0x0040u;
        if (c.V0 == 0u) {
            c.S1 = c.S7 + 0xA0u;
            goto L800B42F4;
        }
        c.S1 = c.S7 + 0xA0u;
        c.S1 = mem.ReadU32((c.S5 + 0x18u));
        L800B42F4: ;
        c.V0 = c.V1 & 0x0080u;
        if (c.V0 == 0u) {
            goto L800B430C;
        }
        c.A1 = mem.ReadU32((c.S2 + 0xCu));
        goto L800B4318;
        L800B430C: ;
        c.V0 = mem.ReadU32((c.S2 + 0xCu));
        c.A1 = c.V0 + 0x60u;
        L800B4318: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x38u));
        if (c.V0 == c.A1) {
            goto L800B4344;
        }
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A1);
        c.A0 = c.S1;
        c.RA = 0x800B4344u;
        MediEvil_game.func_800A4EB8(c, m);
        L800B4344: ;
        c.A1 = c.S2 + 0x10u;
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.A0 + 0x38u));
        c.A2 = c.SP + 0x70u;
        c.RA = 0x800B4358u;
        MediEvil_game.func_800A48FC(c, m);
        c.A0 = c.S1;
        c.A1 = c.SP + 0x68u;
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = 0x1F800000u;
        c.T0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU16((c.SP + 0x70u));
        c.A3 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.T0 + 0x14u));
        c.A2 = c.A2 + 0x14u;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x18u));
        c.V1 = mem.ReadU16((c.SP + 0x74u));
        c.A3 = mem.ReadU16((c.S1 + 0x18u));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x1Cu));
        c.V1 = mem.ReadU16((c.SP + 0x78u));
        c.A3 = mem.ReadU16((c.S1 + 0x1Cu));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B43C0u;
        MediEvil_game.func_800A48FC(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = mem.ReadU32((c.T2 + 0x34u));
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T2 + 0x14u));
        c.T5 = mem.ReadU32((c.T2 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T2 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T3 = 0x800F0000u;
        c.T3 = c.T3 - RAddr.Game_800B4410;
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.A1 = c.SP + 0x80u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw);  }
        c.V0 = TerrainPatch.MeshClipDistance(mem.ReadU16((c.FP + 0x8u)), m);
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4DD0;
        }
        c.V1 = mem.ReadU8((c.SP + 0x84u));
        c.T1 = mem.ReadU32((c.SP + 0x88u));
        c.A2 = mem.ReadU32(c.FP);
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V1 = mem.ReadU32((c.T1 + 0x4u));
        c.V0 = c.V0 + 0x10u;
        c.S3 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.V1 = c.A2 & 0x0008u;
        c.A0 = mem.ReadU32((c.V0 + 0x8u));
        if (c.V1 != 0u) {
            goto L800B44E8;
        }
        if (c.A0 == 0u) {
            c.V0 = c.A2 & 0x0010u;
            goto L800B44E8;
        }
        c.V0 = c.A2 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B44B4;
        }
        c.RA = 0x800B44ACu;
        MediEvil_game.func_8009A694(c, m);
        goto L800B44BC;
        L800B44B4: ;
        c.RA = 0x800B44BCu;
        MediEvil_game.func_8009A540(c, m);
        L800B44BC: ;
        if (c.V0 == 0u) {
            goto L800B4DD0;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Eu));
        c.V1 = c.V1 >> (int)(c.V0 & 31u);
        c.V1 = c.V1 < c.A0 ? 1u : 0u;
        if (c.V1 == 0u) {
            goto L800B4DD0;
        }
        L800B44E8: ;
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        if (c.V1 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B4800;
        }
        c.V0 = 0x00000002u;
        c.V0 = mem.ReadU32((c.V1 + 0x8u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 != 0u) {
            c.V0 = c.V1 + 0x18u;
            goto L800B464C;
        }
        c.V0 = c.V1 + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        c.A1 = mem.ReadU32((c.V1 + 0x14u));
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        if (c.A1 == c.A0) {
            goto L800B461C;
        }
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            c.V0 = c.A1 + 0x60u;
            goto L800B454C;
        }
        c.V0 = c.A1 + 0x60u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A0);
        c.A0 = c.S1;
        goto L800B4558;
        L800B454C: ;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.V0);
        c.A0 = c.S1;
        L800B4558: ;
        c.S0 = 0x1F800000u;
        c.S0 = c.S0 + 0x60u;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.S0;
        c.RA = 0x800B4570u;
        MediEvil_game.func_800A4EB8(c, m);
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x38u));
        c.V1 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.A2 + 0x14u));
        c.A0 = c.S1;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x18u));
        c.V1 = mem.ReadU16((c.A0 + 0x18u));
        c.A1 = c.SP + 0x68u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x1Cu));
        c.V1 = mem.ReadU16((c.A0 + 0x1Cu));
        c.A2 = c.S0 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B45B8u;
        MediEvil_game.func_800A48FC(c, m);
        c.T4 = mem.ReadU32(c.S0);
        c.T5 = mem.ReadU32((c.S0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S0 + 0x8u));
        c.T5 = mem.ReadU32((c.S0 + 0xCu));
        c.T6 = mem.ReadU32((c.S0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.S0 + 0x14u));
        c.T5 = mem.ReadU32((c.S0 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.S0 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        goto L800B4630;
        L800B461C: ;
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = c.V1 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        L800B4630: ;
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        c.A0 = mem.ReadU32((c.V0 + 0x28u));
        c.V1 = c.V1 + c.A0;
        mem.WriteU32((c.V0 + 0x10u), c.V1);
        L800B464C: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32((c.V1 + 0x20u));
        mem.WriteU32((c.SP + 0x40u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU16((c.V0 + 0xEu));
        mem.WriteU16((c.SP + 0x44u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU16((c.V0 + 0xCu));
        c.V0 = 0x00000001u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        mem.WriteU32((c.SP + 0x48u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x50u), c.V1);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU32((c.V0 + 0x8u));
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B46D8;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.A1 = c.V0 - 0x1u;
        goto L800B4718;
        L800B46D8: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V0 = (uint)((int)c.V1 >> (int)(c.V0 & 31u));
        c.A1 = c.V0;
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4DD0;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B4DD0;
        }
        L800B4718: ;
        c.V0 = mem.ReadU32((c.SP + 0x50u));
        c.A0 = mem.ReadU16((c.SP + 0x44u));
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V1 = c.V1 >> 1;
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.SP + 0x46u), (ushort)c.V0);
        c.V0 = c.V0 << 16;
        c.A2 = (uint)((int)c.V0 >> 16);
        c.V0 = 0u - c.A2;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU16((c.V1 + 0x8Cu));
        c.V0 = c.V0 << (int)(c.A0 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.V1 & 31u));
        c.V0 = (int)c.V0 < 3 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B4774;
        }
        c.V0 = 0x00000002u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V0 = c.V0 + c.A2;
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        goto L800B4778;
        L800B4774: ;
        mem.WriteU32((c.SP + 0x4Cu), 0u);
        L800B4778: ;
        c.A0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.A0 + 0x8u));
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 != 0u) {
            c.A3 = 0x00FF0000u;
            goto L800B4830;
        }
        c.A3 = 0x00FF0000u;
        c.A3 = c.A3 | 0xFFFFu;
        c.V0 = c.V1 | 0x0001u;
        c.A1 = c.A1 << 16;
        c.A1 = (uint)((int)c.A1 >> 14);
        c.A2 = 0xFF000000u;
        mem.WriteU32((c.A0 + 0x8u), c.V0);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.A0 = mem.ReadU32((c.SP + 0x40u));
        c.A1 = c.A1 + c.V0;
        c.V1 = mem.ReadU32(c.A0);
        c.V0 = mem.ReadU32(c.A1);
        c.V1 = c.V1 & c.A2;
        c.V0 = c.V0 & c.A3;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A0, c.V1);
        c.A0 = mem.ReadU32(c.A1);
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = mem.ReadU32((c.SP + 0x40u));
        c.A0 = c.A0 & c.A2;
        c.V1 = c.V1 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - 0x4u;
        c.V0 = c.V0 & c.A3;
        c.A0 = c.A0 | c.V0;
        mem.WriteU32(c.A1, c.A0);
        goto L800B4830;
        L800B4800: ;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x9Cu));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Cu));
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU16((c.A1 + 0x8Eu));
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        mem.WriteU32((c.SP + 0x50u), 0u);
        mem.WriteU16((c.SP + 0x46u), (ushort)0u);
        mem.WriteU32((c.SP + 0x40u), c.V1);
        mem.WriteU16((c.SP + 0x44u), (ushort)c.A0);
        mem.WriteU32((c.SP + 0x48u), c.A1);
        L800B4830: ;
        c.V0 = mem.ReadU16((c.S5 + 0x1Eu));
        c.V0 = c.V0 & 0x0003u;
        if (c.V0 != 0u) {
            c.A0 = c.S2;
            goto L800B487C;
        }
        c.A0 = c.S2;
        c.V0 = mem.ReadU32((c.S2 + 0x8u));
        c.V1 = 0x00030000u;
        c.V0 = c.V0 & c.V1;
        if (c.V0 != c.V1) {
            c.A2 = c.S5 + 0x10u;
            goto L800B4880;
        }
        c.A2 = c.S5 + 0x10u;
        c.V0 = mem.ReadU16((c.S7 + 0x11Eu));
        if (c.V0 == 0u) {
            c.A0 = c.S7 + 0x12Cu;
            goto L800B48E4;
        }
        c.A0 = c.S7 + 0x12Cu;
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            c.A0 = c.S2;
            goto L800B48E0;
        }
        c.A0 = c.S2;
        L800B487C: ;
        c.A2 = c.S5 + 0x10u;
        L800B4880: ;
        c.A1 = mem.ReadU16((c.S5 + 0x1Eu));
        c.A3 = c.S5 + 0x14u;
        c.RA = 0x800B488Cu;
        MediEvil_game.func_800ABA38(c, m);
        c.S6 = c.V0;
        c.A0 = 0x1F800000u;
        c.A0 = c.A0 + 0x3Cu;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.A0;
        c.RA = 0x800B48A8u;
        MediEvil_game.func_800A4EB8(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = c.T2 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        goto L800B492C;
        L800B48E0: ;
        c.A0 = c.S7 + 0x12Cu;
        L800B48E4: ;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = 0x1F800000u;
        c.A2 = c.A2 + 0x3Cu;
        c.RA = 0x800B48F8u;
        MediEvil_game.func_800A4EB8(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = c.T3 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        c.S6 = 0u;
        L800B492C: ;
        c.T1 = 0x1F800000u;
        c.T1 = mem.ReadU32((c.T1 + 0x34u));
        c.T4 = mem.ReadU32(c.T1);
        c.T5 = mem.ReadU32((c.T1 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T1 + 0x8u));
        c.T5 = mem.ReadU32((c.T1 + 0xCu));
        c.T6 = mem.ReadU32((c.T1 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T1 + 0x14u));
        c.T5 = mem.ReadU32((c.T1 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T1 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.A2 = mem.ReadU32((c.S3 + 0x10u));
        c.A0 = mem.ReadU16((c.S3 + 0x8u));
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.S3 = mem.ReadU32(c.V0);
        c.V1 = mem.ReadU16((c.S5 + 0xEu));
        c.S2 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 | 0x0001u;
        c.S1 = c.V1 & 0x003Eu;
        if (c.A0 == 0u) {
            mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
            goto L800B4D40;
        }
        mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
        c.S0 = c.SP + 0x20u;
        L800B49B0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A2 = c.A2 + 0x4u;
            goto L800B4D34;
        }
        c.A2 = c.A2 + 0x4u;
        c.V0 = 0x800C0000u;
        c.V0 = c.V0 + RAddr.Game_800B49D0;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        switch (c.V0)
        {
            case 0x800B49ECu: goto L800B49EC;
            case 0x800B4A40u: goto L800B4A40;
            case 0x800B4A94u: goto L800B4A94;
            case 0x800B4AE8u: goto L800B4AE8;
            case 0x800B4B3Cu: goto L800B4B3C;
            case 0x800B4B90u: goto L800B4B90;
            case 0x800B4BE4u: goto L800B4BE4;
            case 0x800B4C38u: goto L800B4C38;
            case 0x800B4D34u: goto L800B4D34;
            case 0x800B4C8Cu: goto L800B4C8C;
            case 0x800B4CE0u: goto L800B4CE0;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B49EC: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A18;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A10u;
        MediEvil_game.func_800B097C(c, m);
        goto L800B4D2C;
        L800B4A18: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A38u;
        MediEvil_game.func_800B7738(c, m);
        goto L800B4D2C;
        L800B4A40: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A6C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A64u;
        MediEvil_game.func_800B0B3C(c, m);
        goto L800B4D2C;
        L800B4A6C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A8Cu;
        MediEvil_game.func_800B7A80(c, m);
        goto L800B4D2C;
        L800B4A94: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4AC0;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4AB8u;
        MediEvil_game.func_800B00E4(c, m);
        goto L800B4D2C;
        L800B4AC0: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4AE0u;
        MediEvil_game.func_800B5A44(c, m);
        goto L800B4D2C;
        L800B4AE8: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B14;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B0Cu;
        MediEvil_game.func_800B02BC(c, m);
        goto L800B4D2C;
        L800B4B14: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B34u;
        MediEvil_game.func_800B5E20(c, m);
        goto L800B4D2C;
        L800B4B3C: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B68;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B60u;
        MediEvil_game.func_800B0D34(c, m);
        goto L800B4D2C;
        L800B4B68: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B88u;
        MediEvil_game.func_800B7E38(c, m);
        goto L800B4D2C;
        L800B4B90: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4BBC;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4BB4u;
        MediEvil_game.func_800B0F20(c, m);
        goto L800B4D2C;
        L800B4BBC: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4BDCu;
        MediEvil_game.func_800B8304(c, m);
        goto L800B4D2C;
        L800B4BE4: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C10;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C08u;
        MediEvil_game.func_800B04D4(c, m);
        goto L800B4D2C;
        L800B4C10: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C30u;
        MediEvil_game.func_800B62B4(c, m);
        goto L800B4D2C;
        L800B4C38: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C64;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C5Cu;
        MediEvil_game.func_800B06D8(c, m);
        goto L800B4D2C;
        L800B4C64: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C84u;
        MediEvil_game.func_800B67A0(c, m);
        goto L800B4D2C;
        L800B4C8C: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4CB8;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4CB0u;
        MediEvil_game.func_800B6EEC(c, m);
        goto L800B4D2C;
        L800B4CB8: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4CD8u;
        MediEvil_game.func_800B8A24(c, m);
        goto L800B4D2C;
        L800B4CE0: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4D0C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4D04u;
        MediEvil_game.func_800B72A0(c, m);
        goto L800B4D2C;
        L800B4D0C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4D2Cu;
        MediEvil_game.func_800B90E8(c, m);
        L800B4D2C: ;
        c.A2 = mem.ReadU32((c.SP + 0x60u));
        c.A0 = mem.ReadU32((c.SP + 0x64u));
        L800B4D34: ;
        if (c.A0 != 0u) {
            goto L800B49B0;
        }
        L800B4D40: ;
        c.V0 = c.S6 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = c.S7 + 0x14Cu;
            goto L800B4D74;
        }
        c.V0 = c.S7 + 0x14Cu;
        c.T4 = mem.ReadU32(c.V0);
        c.T5 = mem.ReadU32((c.V0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(16, c.T4);
        RecompOne.Runtime.Gte.WriteControl(17, c.T5);
        c.T4 = mem.ReadU32((c.V0 + 0x8u));
        c.T5 = mem.ReadU32((c.V0 + 0xCu));
        c.T6 = mem.ReadU32((c.V0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(18, c.T4);
        RecompOne.Runtime.Gte.WriteControl(19, c.T5);
        RecompOne.Runtime.Gte.WriteControl(20, c.T6);
        L800B4D74: ;
        c.V0 = c.S6 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B4DA4;
        }
        c.T2 = mem.ReadU8((c.S7 + 0x16Cu));
        c.T3 = mem.ReadU8((c.S7 + 0x16Du));
        c.T1 = mem.ReadU8((c.S7 + 0x16Eu));
        c.T4 = c.T2 << 4;
        c.T5 = c.T3 << 4;
        c.T6 = c.T1 << 4;
        RecompOne.Runtime.Gte.WriteControl(13, c.T4);
        RecompOne.Runtime.Gte.WriteControl(14, c.T5);
        RecompOne.Runtime.Gte.WriteControl(15, c.T6);
        L800B4DA4: ;
        c.V0 = mem.ReadU32(c.FP);
        c.V0 = c.V0 & 0x2000u;
        if (c.V0 == 0u) {
            c.A2 = c.S5;
            goto L800B4DD0;
        }
        c.A2 = c.S5;
        c.A3 = 0x00FF0000u;
        c.T2 = mem.ReadU32((c.SP + 0x88u));
        c.A1 = mem.ReadU8((c.SP + 0x84u));
        c.A0 = mem.ReadU32((c.T2 + 0x4u));
        c.A3 = c.A3 | 0xFFFFu;
        c.RA = 0x800B4DD0u;
        MediEvil_game.func_800AF490(c, m);
        L800B4DD0: ;
        c.RA = mem.ReadU32((c.SP + 0xB4u));
        c.FP = mem.ReadU32((c.SP + 0xB0u));
        c.S7 = mem.ReadU32((c.SP + 0xACu));
        c.S6 = mem.ReadU32((c.SP + 0xA8u));
        c.S5 = mem.ReadU32((c.SP + 0xA4u));
        c.S4 = mem.ReadU32((c.SP + 0xA0u));
        c.S3 = mem.ReadU32((c.SP + 0x9Cu));
        c.S2 = mem.ReadU32((c.SP + 0x98u));
        c.S1 = mem.ReadU32((c.SP + 0x94u));
        c.S0 = mem.ReadU32((c.SP + 0x90u));
        c.SP = c.SP + 0xB8u;
        return;
    }

    public static void func_800B4028(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x38u;
        mem.WriteU32((c.SP + 0x28u), c.S2);
        c.S2 = c.A0;
        mem.WriteU32((c.SP + 0x24u), c.S1);
        c.S1 = 0u;
        c.V0 = 0xFFFFFFFBu;
        mem.WriteU32((c.SP + 0x30u), c.S4);
        mem.WriteU32((c.SP + 0x2Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S0);
        c.V1 = mem.ReadU32((c.S2 + 0x8u));
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        c.V1 = c.V1 & c.V0;
        if (c.A0 == 0u) {
            mem.WriteU32((c.S2 + 0x8u), c.V1);
            goto L800B4228;
        }
        mem.WriteU32((c.S2 + 0x8u), c.V1);
        c.V0 = 0x800C0000u;
        c.S3 = c.V0 + RAddr.Game_800B4064;
        c.T3 = 0u;
        c.S0 = c.S2 + 0x20u;
        L800B4070: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T9 = mem.ReadU16((c.S0 - 0x8u));
        c.A0 = mem.ReadU32(c.S0);
        if (c.T9 == 0u) {
            goto L800B4210;
        }
        L800B4080: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T5 = 0u;
        c.V0 = mem.ReadU16(c.A0);
        c.T8 = mem.ReadU16((c.A0 + 0x2u));
        c.V1 = c.V0 & 0xFFFFu;
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.A0 + 0x4u;
            goto L800B4140;
        }
        c.A0 = c.A0 + 0x4u;
        c.V0 = c.V1 << 2;
        c.V0 = c.V0 + c.S3;
        c.V0 = mem.ReadU32(c.V0);
        switch (c.V0)
        {
            case 0x800B40B4u: goto L800B40B4;
            case 0x800B40F4u: goto L800B40F4;
            case 0x800B40BCu: goto L800B40BC;
            case 0x800B40D8u: goto L800B40D8;
            case 0x800B40FCu: goto L800B40FC;
            case 0x800B4104u: goto L800B4104;
            case 0x800B411Cu: goto L800B411C;
            case 0x800B4140u: goto L800B4140;
            case 0x800B40D0u: goto L800B40D0;
            case 0x800B413Cu: goto L800B413C;
            case 0x800B49ECu: goto L800B49EC;
            case 0x800B4A40u: goto L800B4A40;
            case 0x800B4A94u: goto L800B4A94;
            case 0x800B4AE8u: goto L800B4AE8;
            case 0x800B4B3Cu: goto L800B4B3C;
            case 0x800B4B90u: goto L800B4B90;
            case 0x800B4BE4u: goto L800B4BE4;
            case 0x800B4C38u: goto L800B4C38;
            case 0x800B4D34u: goto L800B4D34;
            case 0x800B4C8Cu: goto L800B4C8C;
            case 0x800B4CE0u: goto L800B4CE0;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B40B4: ;
        c.A2 = 0x0000000Cu;
        goto L800B4140;
        L800B40BC: ;
        c.T5 = 0x00000003u;
        c.T6 = c.A0 + 0x12u;
        c.T1 = c.A0 + 0x8u;
        c.T0 = c.A0 + 0xCu;
        c.A3 = c.A0 + 0x10u;
        L800B40D0: ;
        c.A2 = 0x00000018u;
        goto L800B4140;
        L800B40D8: ;
        c.T5 = 0x00000004u;
        c.T6 = c.A0 + 0xAu;
        c.T1 = c.A0 + 0xCu;
        c.T0 = c.A0 + 0x10u;
        c.A3 = c.A0 + 0x14u;
        c.T4 = c.A0 + 0x16u;
        goto L800B413C;
        L800B40F4: ;
        c.A2 = 0x00000010u;
        goto L800B4140;
        L800B40FC: ;
        c.A2 = 0x00000014u;
        goto L800B4140;
        L800B4104: ;
        c.T5 = 0x00000003u;
        c.T6 = c.A0 + 0x16u;
        c.T1 = c.A0 + 0xCu;
        c.T0 = c.A0 + 0x10u;
        c.A3 = c.A0 + 0x14u;
        goto L800B413C;
        L800B411C: ;
        c.T5 = 0x00000004u;
        c.T6 = c.A0 + 0x1Cu;
        c.T1 = c.A0 + 0x10u;
        c.T0 = c.A0 + 0x14u;
        c.A3 = c.A0 + 0x18u;
        c.T4 = c.A0 + 0x1Au;
        c.A2 = 0x00000024u;
        goto L800B4140;
        L800B413C: ;
        c.A2 = 0x0000001Cu;
        L800B4140: ;
        if ((int)c.T5 <= 0) {
            c.V0 = c.T8 & 0xFFFFu;
            goto L800B41EC;
        }
        c.V0 = c.T8 & 0xFFFFu;
        c.T2 = c.T8 & 0xFFFFu;
        if ((int)c.T2 < 0) {
            c.V0 = c.A2 << 1;
            goto L800B41E8;
        }
        c.V0 = c.A2 << 1;
        c.T7 = c.V0 - 0x2u;
        c.A1 = c.A2 - 0x1u;
        L800B415C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.T6);
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_800B4160));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = mem.ReadU32(c.V0);
        c.T6 = c.T6 + c.T7;
        mem.WriteU8(c.T1, (byte)0u);
        mem.WriteU8(c.T1, (byte)0u);
        c.T1 = c.T1 + 0x1u;
        mem.WriteU8(c.T1, (byte)0u);
        mem.WriteU8(c.T1, (byte)0u);
        c.T1 = c.T1 + c.A1;
        mem.WriteU8(c.T0, (byte)0u);
        mem.WriteU8(c.T0, (byte)0u);
        c.T0 = c.T0 + 0x1u;
        mem.WriteU8(c.T0, (byte)0u);
        mem.WriteU8(c.T0, (byte)0u);
        c.T0 = c.T0 + c.A1;
        mem.WriteU8(c.A3, (byte)0u);
        mem.WriteU8(c.A3, (byte)0u);
        c.A3 = c.A3 + 0x1u;
        mem.WriteU8(c.A3, (byte)0u);
        mem.WriteU8(c.A3, (byte)0u);
        c.V0 = 0x00000004u;
        if (c.T5 != c.V0) {
            c.A3 = c.A3 + c.A1;
            goto L800B41DC;
        }
        c.A3 = c.A3 + c.A1;
        mem.WriteU8(c.T4, (byte)0u);
        mem.WriteU8(c.T4, (byte)0u);
        c.T4 = c.T4 + 0x1u;
        mem.WriteU8(c.T4, (byte)0u);
        mem.WriteU8(c.T4, (byte)0u);
        c.T4 = c.T4 + c.A1;
        L800B41DC: ;
        c.T2 = c.T2 - 0x1u;
        if ((int)c.T2 >= 0) {
            goto L800B415C;
        }
        L800B41E8: ;
        c.V0 = c.T8 & 0xFFFFu;
        L800B41EC: ;
        { var _r = (long)(int)c.A2 * (int)c.V0; c.LO = (uint)_r; c.HI = (uint)(_r >> 32); }
        c.T9 = c.T9 - c.T8;
        c.S4 = c.LO;
        c.V0 = c.S4 >> 2;
        c.V0 = c.V0 << 2;
        c.A0 = c.A0 + c.V0;
        c.V0 = c.T9 & 0xFFFFu;
        if (c.V0 != 0u) {
            goto L800B4080;
        }
        L800B4210: ;
        c.S1 = c.S1 + 0x1u;
        c.V1 = mem.ReadU32((c.S2 + 0xCu));
        c.V0 = c.S1 & 0xFFFFu;
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S0 = c.S0 + 0x2Cu;
            goto L800B4070;
        }
        c.S0 = c.S0 + 0x2Cu;
        L800B4228: ;
        c.S4 = mem.ReadU32((c.SP + 0x30u));
        c.S3 = mem.ReadU32((c.SP + 0x2Cu));
        c.S2 = mem.ReadU32((c.SP + 0x28u));
        c.S1 = mem.ReadU32((c.SP + 0x24u));
        c.S0 = mem.ReadU32((c.SP + 0x20u));
        c.SP = c.SP + 0x38u;
        return;
        c.SP = c.SP - 0xB8u;
        mem.WriteU32((c.SP + 0xA4u), c.S5);
        c.S5 = c.A0;
        c.V1 = 0x800F0000u;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 - RAddr.Game_800B425C;
        mem.WriteU32((c.SP + 0xB4u), c.RA);
        mem.WriteU32((c.SP + 0xB0u), c.FP);
        mem.WriteU32((c.SP + 0xACu), c.S7);
        mem.WriteU32((c.SP + 0xA8u), c.S6);
        mem.WriteU32((c.SP + 0xA0u), c.S4);
        mem.WriteU32((c.SP + 0x9Cu), c.S3);
        mem.WriteU32((c.SP + 0x98u), c.S2);
        mem.WriteU32((c.SP + 0x94u), c.S1);
        mem.WriteU32((c.SP + 0x90u), c.S0);
        mem.WriteU8((c.SP + 0x84u), (byte)c.A2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x10u));
        c.V0 = mem.ReadU32(c.V0);
        c.S7 = c.A1;
        c.V1 = c.V1 < c.V0 ? 1u : 0u;
        if (c.V1 == 0u) {
            mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
            goto L800B4DD0;
        }
        mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
        c.S2 = mem.ReadU32((c.S5 + 0x8u));
        c.S4 = 0u;
        c.FP = mem.ReadU32((c.S2 + 0x2Cu));
        c.V1 = mem.ReadU16((c.S2 + 0xAu));
        c.T1 = mem.ReadU32((c.FP + 0x4u));
        c.V0 = c.V1 & 0x0020u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0x88u), c.T1);
            goto L800B42DC;
        }
        mem.WriteU32((c.SP + 0x88u), c.T1);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x90u));
        c.S4 = c.S4 < c.V0 ? 1u : 0u;
        L800B42DC: ;
        c.V0 = mem.ReadU16((c.S5 + 0xEu));
        c.V0 = c.V0 & 0x0040u;
        if (c.V0 == 0u) {
            c.S1 = c.S7 + 0xA0u;
            goto L800B42F4;
        }
        c.S1 = c.S7 + 0xA0u;
        c.S1 = mem.ReadU32((c.S5 + 0x18u));
        L800B42F4: ;
        c.V0 = c.V1 & 0x0080u;
        if (c.V0 == 0u) {
            goto L800B430C;
        }
        c.A1 = mem.ReadU32((c.S2 + 0xCu));
        goto L800B4318;
        L800B430C: ;
        c.V0 = mem.ReadU32((c.S2 + 0xCu));
        c.A1 = c.V0 + 0x60u;
        L800B4318: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x38u));
        if (c.V0 == c.A1) {
            goto L800B4344;
        }
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A1);
        c.A0 = c.S1;
        c.RA = 0x800B4344u;
        MediEvil_game.func_800A4EB8(c, m);
        L800B4344: ;
        c.A1 = c.S2 + 0x10u;
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.A0 + 0x38u));
        c.A2 = c.SP + 0x70u;
        c.RA = 0x800B4358u;
        MediEvil_game.func_800A48FC(c, m);
        c.A0 = c.S1;
        c.A1 = c.SP + 0x68u;
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = 0x1F800000u;
        c.T0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU16((c.SP + 0x70u));
        c.A3 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.T0 + 0x14u));
        c.A2 = c.A2 + 0x14u;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x18u));
        c.V1 = mem.ReadU16((c.SP + 0x74u));
        c.A3 = mem.ReadU16((c.S1 + 0x18u));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x1Cu));
        c.V1 = mem.ReadU16((c.SP + 0x78u));
        c.A3 = mem.ReadU16((c.S1 + 0x1Cu));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B43C0u;
        MediEvil_game.func_800A48FC(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = mem.ReadU32((c.T2 + 0x34u));
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T2 + 0x14u));
        c.T5 = mem.ReadU32((c.T2 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T2 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T3 = 0x800F0000u;
        c.T3 = c.T3 - RAddr.Game_800B4410;
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.A1 = c.SP + 0x80u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw);  }
        c.V0 = TerrainPatch.MeshClipDistance(mem.ReadU16((c.FP + 0x8u)), m);
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4DD0;
        }
        c.V1 = mem.ReadU8((c.SP + 0x84u));
        c.T1 = mem.ReadU32((c.SP + 0x88u));
        c.A2 = mem.ReadU32(c.FP);
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V1 = mem.ReadU32((c.T1 + 0x4u));
        c.V0 = c.V0 + 0x10u;
        c.S3 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.V1 = c.A2 & 0x0008u;
        c.A0 = mem.ReadU32((c.V0 + 0x8u));
        if (c.V1 != 0u) {
            goto L800B44E8;
        }
        if (c.A0 == 0u) {
            c.V0 = c.A2 & 0x0010u;
            goto L800B44E8;
        }
        c.V0 = c.A2 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B44B4;
        }
        c.RA = 0x800B44ACu;
        MediEvil_game.func_8009A694(c, m);
        goto L800B44BC;
        L800B44B4: ;
        c.RA = 0x800B44BCu;
        MediEvil_game.func_8009A540(c, m);
        L800B44BC: ;
        if (c.V0 == 0u) {
            goto L800B4DD0;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Eu));
        c.V1 = c.V1 >> (int)(c.V0 & 31u);
        c.V1 = c.V1 < c.A0 ? 1u : 0u;
        if (c.V1 == 0u) {
            goto L800B4DD0;
        }
        L800B44E8: ;
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        if (c.V1 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B4800;
        }
        c.V0 = 0x00000002u;
        c.V0 = mem.ReadU32((c.V1 + 0x8u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 != 0u) {
            c.V0 = c.V1 + 0x18u;
            goto L800B464C;
        }
        c.V0 = c.V1 + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        c.A1 = mem.ReadU32((c.V1 + 0x14u));
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        if (c.A1 == c.A0) {
            goto L800B461C;
        }
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            c.V0 = c.A1 + 0x60u;
            goto L800B454C;
        }
        c.V0 = c.A1 + 0x60u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A0);
        c.A0 = c.S1;
        goto L800B4558;
        L800B454C: ;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.V0);
        c.A0 = c.S1;
        L800B4558: ;
        c.S0 = 0x1F800000u;
        c.S0 = c.S0 + 0x60u;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.S0;
        c.RA = 0x800B4570u;
        MediEvil_game.func_800A4EB8(c, m);
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x38u));
        c.V1 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.A2 + 0x14u));
        c.A0 = c.S1;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x18u));
        c.V1 = mem.ReadU16((c.A0 + 0x18u));
        c.A1 = c.SP + 0x68u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x1Cu));
        c.V1 = mem.ReadU16((c.A0 + 0x1Cu));
        c.A2 = c.S0 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B45B8u;
        MediEvil_game.func_800A48FC(c, m);
        c.T4 = mem.ReadU32(c.S0);
        c.T5 = mem.ReadU32((c.S0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S0 + 0x8u));
        c.T5 = mem.ReadU32((c.S0 + 0xCu));
        c.T6 = mem.ReadU32((c.S0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.S0 + 0x14u));
        c.T5 = mem.ReadU32((c.S0 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.S0 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        goto L800B4630;
        L800B461C: ;
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = c.V1 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        L800B4630: ;
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        c.A0 = mem.ReadU32((c.V0 + 0x28u));
        c.V1 = c.V1 + c.A0;
        mem.WriteU32((c.V0 + 0x10u), c.V1);
        L800B464C: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32((c.V1 + 0x20u));
        mem.WriteU32((c.SP + 0x40u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU16((c.V0 + 0xEu));
        mem.WriteU16((c.SP + 0x44u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU16((c.V0 + 0xCu));
        c.V0 = 0x00000001u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        mem.WriteU32((c.SP + 0x48u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x50u), c.V1);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU32((c.V0 + 0x8u));
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B46D8;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.A1 = c.V0 - 0x1u;
        goto L800B4718;
        L800B46D8: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V0 = (uint)((int)c.V1 >> (int)(c.V0 & 31u));
        c.A1 = c.V0;
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4DD0;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B4DD0;
        }
        L800B4718: ;
        c.V0 = mem.ReadU32((c.SP + 0x50u));
        c.A0 = mem.ReadU16((c.SP + 0x44u));
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V1 = c.V1 >> 1;
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.SP + 0x46u), (ushort)c.V0);
        c.V0 = c.V0 << 16;
        c.A2 = (uint)((int)c.V0 >> 16);
        c.V0 = 0u - c.A2;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU16((c.V1 + 0x8Cu));
        c.V0 = c.V0 << (int)(c.A0 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.V1 & 31u));
        c.V0 = (int)c.V0 < 3 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B4774;
        }
        c.V0 = 0x00000002u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V0 = c.V0 + c.A2;
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        goto L800B4778;
        L800B4774: ;
        mem.WriteU32((c.SP + 0x4Cu), 0u);
        L800B4778: ;
        c.A0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.A0 + 0x8u));
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 != 0u) {
            c.A3 = 0x00FF0000u;
            goto L800B4830;
        }
        c.A3 = 0x00FF0000u;
        c.A3 = c.A3 | 0xFFFFu;
        c.V0 = c.V1 | 0x0001u;
        c.A1 = c.A1 << 16;
        c.A1 = (uint)((int)c.A1 >> 14);
        c.A2 = 0xFF000000u;
        mem.WriteU32((c.A0 + 0x8u), c.V0);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.A0 = mem.ReadU32((c.SP + 0x40u));
        c.A1 = c.A1 + c.V0;
        c.V1 = mem.ReadU32(c.A0);
        c.V0 = mem.ReadU32(c.A1);
        c.V1 = c.V1 & c.A2;
        c.V0 = c.V0 & c.A3;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A0, c.V1);
        c.A0 = mem.ReadU32(c.A1);
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = mem.ReadU32((c.SP + 0x40u));
        c.A0 = c.A0 & c.A2;
        c.V1 = c.V1 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - 0x4u;
        c.V0 = c.V0 & c.A3;
        c.A0 = c.A0 | c.V0;
        mem.WriteU32(c.A1, c.A0);
        goto L800B4830;
        L800B4800: ;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x9Cu));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Cu));
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU16((c.A1 + 0x8Eu));
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        mem.WriteU32((c.SP + 0x50u), 0u);
        mem.WriteU16((c.SP + 0x46u), (ushort)0u);
        mem.WriteU32((c.SP + 0x40u), c.V1);
        mem.WriteU16((c.SP + 0x44u), (ushort)c.A0);
        mem.WriteU32((c.SP + 0x48u), c.A1);
        L800B4830: ;
        c.V0 = mem.ReadU16((c.S5 + 0x1Eu));
        c.V0 = c.V0 & 0x0003u;
        if (c.V0 != 0u) {
            c.A0 = c.S2;
            goto L800B487C;
        }
        c.A0 = c.S2;
        c.V0 = mem.ReadU32((c.S2 + 0x8u));
        c.V1 = 0x00030000u;
        c.V0 = c.V0 & c.V1;
        if (c.V0 != c.V1) {
            c.A2 = c.S5 + 0x10u;
            goto L800B4880;
        }
        c.A2 = c.S5 + 0x10u;
        c.V0 = mem.ReadU16((c.S7 + 0x11Eu));
        if (c.V0 == 0u) {
            c.A0 = c.S7 + 0x12Cu;
            goto L800B48E4;
        }
        c.A0 = c.S7 + 0x12Cu;
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            c.A0 = c.S2;
            goto L800B48E0;
        }
        c.A0 = c.S2;
        L800B487C: ;
        c.A2 = c.S5 + 0x10u;
        L800B4880: ;
        c.A1 = mem.ReadU16((c.S5 + 0x1Eu));
        c.A3 = c.S5 + 0x14u;
        c.RA = 0x800B488Cu;
        MediEvil_game.func_800ABA38(c, m);
        c.S6 = c.V0;
        c.A0 = 0x1F800000u;
        c.A0 = c.A0 + 0x3Cu;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.A0;
        c.RA = 0x800B48A8u;
        MediEvil_game.func_800A4EB8(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = c.T2 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        goto L800B492C;
        L800B48E0: ;
        c.A0 = c.S7 + 0x12Cu;
        L800B48E4: ;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = 0x1F800000u;
        c.A2 = c.A2 + 0x3Cu;
        c.RA = 0x800B48F8u;
        MediEvil_game.func_800A4EB8(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = c.T3 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        c.S6 = 0u;
        L800B492C: ;
        c.T1 = 0x1F800000u;
        c.T1 = mem.ReadU32((c.T1 + 0x34u));
        c.T4 = mem.ReadU32(c.T1);
        c.T5 = mem.ReadU32((c.T1 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T1 + 0x8u));
        c.T5 = mem.ReadU32((c.T1 + 0xCu));
        c.T6 = mem.ReadU32((c.T1 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T1 + 0x14u));
        c.T5 = mem.ReadU32((c.T1 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T1 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.A2 = mem.ReadU32((c.S3 + 0x10u));
        c.A0 = mem.ReadU16((c.S3 + 0x8u));
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.S3 = mem.ReadU32(c.V0);
        c.V1 = mem.ReadU16((c.S5 + 0xEu));
        c.S2 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 | 0x0001u;
        c.S1 = c.V1 & 0x003Eu;
        if (c.A0 == 0u) {
            mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
            goto L800B4D40;
        }
        mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
        c.S0 = c.SP + 0x20u;
        L800B49B0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A2 = c.A2 + 0x4u;
            goto L800B4D34;
        }
        c.A2 = c.A2 + 0x4u;
        c.V0 = 0x800C0000u;
        c.V0 = c.V0 + RAddr.Game_800B49D0;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        switch (c.V0)
        {
            case 0x800B49ECu: goto L800B49EC;
            case 0x800B4A40u: goto L800B4A40;
            case 0x800B4A94u: goto L800B4A94;
            case 0x800B4AE8u: goto L800B4AE8;
            case 0x800B4B3Cu: goto L800B4B3C;
            case 0x800B4B90u: goto L800B4B90;
            case 0x800B4BE4u: goto L800B4BE4;
            case 0x800B4C38u: goto L800B4C38;
            case 0x800B4D34u: goto L800B4D34;
            case 0x800B4C8Cu: goto L800B4C8C;
            case 0x800B4CE0u: goto L800B4CE0;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B49EC: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A18;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A10u;
        MediEvil_game.func_800B097C(c, m);
        goto L800B4D2C;
        L800B4A18: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A38u;
        MediEvil_game.func_800B7738(c, m);
        goto L800B4D2C;
        L800B4A40: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A6C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A64u;
        MediEvil_game.func_800B0B3C(c, m);
        goto L800B4D2C;
        L800B4A6C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A8Cu;
        MediEvil_game.func_800B7A80(c, m);
        goto L800B4D2C;
        L800B4A94: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4AC0;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4AB8u;
        MediEvil_game.func_800B00E4(c, m);
        goto L800B4D2C;
        L800B4AC0: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4AE0u;
        MediEvil_game.func_800B5A44(c, m);
        goto L800B4D2C;
        L800B4AE8: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B14;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B0Cu;
        MediEvil_game.func_800B02BC(c, m);
        goto L800B4D2C;
        L800B4B14: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B34u;
        MediEvil_game.func_800B5E20(c, m);
        goto L800B4D2C;
        L800B4B3C: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B68;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B60u;
        MediEvil_game.func_800B0D34(c, m);
        goto L800B4D2C;
        L800B4B68: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B88u;
        MediEvil_game.func_800B7E38(c, m);
        goto L800B4D2C;
        L800B4B90: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4BBC;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4BB4u;
        MediEvil_game.func_800B0F20(c, m);
        goto L800B4D2C;
        L800B4BBC: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4BDCu;
        MediEvil_game.func_800B8304(c, m);
        goto L800B4D2C;
        L800B4BE4: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C10;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C08u;
        MediEvil_game.func_800B04D4(c, m);
        goto L800B4D2C;
        L800B4C10: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C30u;
        MediEvil_game.func_800B62B4(c, m);
        goto L800B4D2C;
        L800B4C38: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C64;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C5Cu;
        MediEvil_game.func_800B06D8(c, m);
        goto L800B4D2C;
        L800B4C64: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C84u;
        MediEvil_game.func_800B67A0(c, m);
        goto L800B4D2C;
        L800B4C8C: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4CB8;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4CB0u;
        MediEvil_game.func_800B6EEC(c, m);
        goto L800B4D2C;
        L800B4CB8: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4CD8u;
        MediEvil_game.func_800B8A24(c, m);
        goto L800B4D2C;
        L800B4CE0: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4D0C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4D04u;
        MediEvil_game.func_800B72A0(c, m);
        goto L800B4D2C;
        L800B4D0C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4D2Cu;
        MediEvil_game.func_800B90E8(c, m);
        L800B4D2C: ;
        c.A2 = mem.ReadU32((c.SP + 0x60u));
        c.A0 = mem.ReadU32((c.SP + 0x64u));
        L800B4D34: ;
        if (c.A0 != 0u) {
            goto L800B49B0;
        }
        L800B4D40: ;
        c.V0 = c.S6 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = c.S7 + 0x14Cu;
            goto L800B4D74;
        }
        c.V0 = c.S7 + 0x14Cu;
        c.T4 = mem.ReadU32(c.V0);
        c.T5 = mem.ReadU32((c.V0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(16, c.T4);
        RecompOne.Runtime.Gte.WriteControl(17, c.T5);
        c.T4 = mem.ReadU32((c.V0 + 0x8u));
        c.T5 = mem.ReadU32((c.V0 + 0xCu));
        c.T6 = mem.ReadU32((c.V0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(18, c.T4);
        RecompOne.Runtime.Gte.WriteControl(19, c.T5);
        RecompOne.Runtime.Gte.WriteControl(20, c.T6);
        L800B4D74: ;
        c.V0 = c.S6 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B4DA4;
        }
        c.T2 = mem.ReadU8((c.S7 + 0x16Cu));
        c.T3 = mem.ReadU8((c.S7 + 0x16Du));
        c.T1 = mem.ReadU8((c.S7 + 0x16Eu));
        c.T4 = c.T2 << 4;
        c.T5 = c.T3 << 4;
        c.T6 = c.T1 << 4;
        RecompOne.Runtime.Gte.WriteControl(13, c.T4);
        RecompOne.Runtime.Gte.WriteControl(14, c.T5);
        RecompOne.Runtime.Gte.WriteControl(15, c.T6);
        L800B4DA4: ;
        c.V0 = mem.ReadU32(c.FP);
        c.V0 = c.V0 & 0x2000u;
        if (c.V0 == 0u) {
            c.A2 = c.S5;
            goto L800B4DD0;
        }
        c.A2 = c.S5;
        c.A3 = 0x00FF0000u;
        c.T2 = mem.ReadU32((c.SP + 0x88u));
        c.A1 = mem.ReadU8((c.SP + 0x84u));
        c.A0 = mem.ReadU32((c.T2 + 0x4u));
        c.A3 = c.A3 | 0xFFFFu;
        c.RA = 0x800B4DD0u;
        MediEvil_game.func_800AF490(c, m);
        L800B4DD0: ;
        c.RA = mem.ReadU32((c.SP + 0xB4u));
        c.FP = mem.ReadU32((c.SP + 0xB0u));
        c.S7 = mem.ReadU32((c.SP + 0xACu));
        c.S6 = mem.ReadU32((c.SP + 0xA8u));
        c.S5 = mem.ReadU32((c.SP + 0xA4u));
        c.S4 = mem.ReadU32((c.SP + 0xA0u));
        c.S3 = mem.ReadU32((c.SP + 0x9Cu));
        c.S2 = mem.ReadU32((c.SP + 0x98u));
        c.S1 = mem.ReadU32((c.SP + 0x94u));
        c.S0 = mem.ReadU32((c.SP + 0x90u));
        c.SP = c.SP + 0xB8u;
        return;
    }

    public static void func_800B4244(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0xB8u;
        mem.WriteU32((c.SP + 0xA4u), c.S5);
        c.S5 = c.A0;
        c.V1 = 0x800F0000u;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 - RAddr.Game_800B425C;
        mem.WriteU32((c.SP + 0xB4u), c.RA);
        mem.WriteU32((c.SP + 0xB0u), c.FP);
        mem.WriteU32((c.SP + 0xACu), c.S7);
        mem.WriteU32((c.SP + 0xA8u), c.S6);
        mem.WriteU32((c.SP + 0xA0u), c.S4);
        mem.WriteU32((c.SP + 0x9Cu), c.S3);
        mem.WriteU32((c.SP + 0x98u), c.S2);
        mem.WriteU32((c.SP + 0x94u), c.S1);
        mem.WriteU32((c.SP + 0x90u), c.S0);
        mem.WriteU8((c.SP + 0x84u), (byte)c.A2);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x10u));
        c.V0 = mem.ReadU32(c.V0);
        c.S7 = c.A1;
        c.V1 = c.V1 < c.V0 ? 1u : 0u;
        if (c.V1 == 0u) {
            mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
            goto L800B4DD0;
        }
        mem.WriteU8((c.SP + 0x85u), (byte)c.A3);
        c.S2 = mem.ReadU32((c.S5 + 0x8u));
        c.S4 = 0u;
        c.FP = mem.ReadU32((c.S2 + 0x2Cu));
        c.V1 = mem.ReadU16((c.S2 + 0xAu));
        c.T1 = mem.ReadU32((c.FP + 0x4u));
        c.V0 = c.V1 & 0x0020u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0x88u), c.T1);
            goto L800B42DC;
        }
        mem.WriteU32((c.SP + 0x88u), c.T1);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x90u));
        c.S4 = c.S4 < c.V0 ? 1u : 0u;
        L800B42DC: ;
        c.V0 = mem.ReadU16((c.S5 + 0xEu));
        c.V0 = c.V0 & 0x0040u;
        if (c.V0 == 0u) {
            c.S1 = c.S7 + 0xA0u;
            goto L800B42F4;
        }
        c.S1 = c.S7 + 0xA0u;
        c.S1 = mem.ReadU32((c.S5 + 0x18u));
        L800B42F4: ;
        c.V0 = c.V1 & 0x0080u;
        if (c.V0 == 0u) {
            goto L800B430C;
        }
        c.A1 = mem.ReadU32((c.S2 + 0xCu));
        goto L800B4318;
        L800B430C: ;
        c.V0 = mem.ReadU32((c.S2 + 0xCu));
        c.A1 = c.V0 + 0x60u;
        L800B4318: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x38u));
        if (c.V0 == c.A1) {
            goto L800B4344;
        }
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A1);
        c.A0 = c.S1;
        c.RA = 0x800B4344u;
        MediEvil_game.func_800A4EB8(c, m);
        L800B4344: ;
        c.A1 = c.S2 + 0x10u;
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.A0 + 0x38u));
        c.A2 = c.SP + 0x70u;
        c.RA = 0x800B4358u;
        MediEvil_game.func_800A48FC(c, m);
        c.A0 = c.S1;
        c.A1 = c.SP + 0x68u;
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = 0x1F800000u;
        c.T0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU16((c.SP + 0x70u));
        c.A3 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.T0 + 0x14u));
        c.A2 = c.A2 + 0x14u;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x18u));
        c.V1 = mem.ReadU16((c.SP + 0x74u));
        c.A3 = mem.ReadU16((c.S1 + 0x18u));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x1Cu));
        c.V1 = mem.ReadU16((c.SP + 0x78u));
        c.A3 = mem.ReadU16((c.S1 + 0x1Cu));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B43C0u;
        MediEvil_game.func_800A48FC(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = mem.ReadU32((c.T2 + 0x34u));
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T2 + 0x14u));
        c.T5 = mem.ReadU32((c.T2 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T2 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T3 = 0x800F0000u;
        c.T3 = c.T3 - RAddr.Game_800B4410;
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.A1 = c.SP + 0x80u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw);  }
        c.V0 = TerrainPatch.MeshClipDistance(mem.ReadU16((c.FP + 0x8u)), m);
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4DD0;
        }
        c.V1 = mem.ReadU8((c.SP + 0x84u));
        c.T1 = mem.ReadU32((c.SP + 0x88u));
        c.A2 = mem.ReadU32(c.FP);
        c.V0 = c.V1 << 1;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 2;
        c.V1 = mem.ReadU32((c.T1 + 0x4u));
        c.V0 = c.V0 + 0x10u;
        c.S3 = c.V1 + c.V0;
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.V1 = c.A2 & 0x0008u;
        c.A0 = mem.ReadU32((c.V0 + 0x8u));
        if (c.V1 != 0u) {
            goto L800B44E8;
        }
        if (c.A0 == 0u) {
            c.V0 = c.A2 & 0x0010u;
            goto L800B44E8;
        }
        c.V0 = c.A2 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B44B4;
        }
        c.RA = 0x800B44ACu;
        MediEvil_game.func_8009A694(c, m);
        goto L800B44BC;
        L800B44B4: ;
        c.RA = 0x800B44BCu;
        MediEvil_game.func_8009A540(c, m);
        L800B44BC: ;
        if (c.V0 == 0u) {
            goto L800B4DD0;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x80u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Eu));
        c.V1 = c.V1 >> (int)(c.V0 & 31u);
        c.V1 = c.V1 < c.A0 ? 1u : 0u;
        if (c.V1 == 0u) {
            goto L800B4DD0;
        }
        L800B44E8: ;
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        if (c.V1 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B4800;
        }
        c.V0 = 0x00000002u;
        c.V0 = mem.ReadU32((c.V1 + 0x8u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 != 0u) {
            c.V0 = c.V1 + 0x18u;
            goto L800B464C;
        }
        c.V0 = c.V1 + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        c.A1 = mem.ReadU32((c.V1 + 0x14u));
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        if (c.A1 == c.A0) {
            goto L800B461C;
        }
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            c.V0 = c.A1 + 0x60u;
            goto L800B454C;
        }
        c.V0 = c.A1 + 0x60u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A0);
        c.A0 = c.S1;
        goto L800B4558;
        L800B454C: ;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.V0);
        c.A0 = c.S1;
        L800B4558: ;
        c.S0 = 0x1F800000u;
        c.S0 = c.S0 + 0x60u;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.S0;
        c.RA = 0x800B4570u;
        MediEvil_game.func_800A4EB8(c, m);
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x38u));
        c.V1 = mem.ReadU16((c.S1 + 0x14u));
        c.V0 = mem.ReadU16((c.A2 + 0x14u));
        c.A0 = c.S1;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x68u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x18u));
        c.V1 = mem.ReadU16((c.A0 + 0x18u));
        c.A1 = c.SP + 0x68u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x1Cu));
        c.V1 = mem.ReadU16((c.A0 + 0x1Cu));
        c.A2 = c.S0 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x6Cu), (ushort)c.V0);
        c.RA = 0x800B45B8u;
        MediEvil_game.func_800A48FC(c, m);
        c.T4 = mem.ReadU32(c.S0);
        c.T5 = mem.ReadU32((c.S0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S0 + 0x8u));
        c.T5 = mem.ReadU32((c.S0 + 0xCu));
        c.T6 = mem.ReadU32((c.S0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.S0 + 0x14u));
        c.T5 = mem.ReadU32((c.S0 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.S0 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        goto L800B4630;
        L800B461C: ;
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = c.V1 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        L800B4630: ;
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        c.A0 = mem.ReadU32((c.V0 + 0x28u));
        c.V1 = c.V1 + c.A0;
        mem.WriteU32((c.V0 + 0x10u), c.V1);
        L800B464C: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = c.V0 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32((c.V1 + 0x20u));
        mem.WriteU32((c.SP + 0x40u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU16((c.V0 + 0xEu));
        mem.WriteU16((c.SP + 0x44u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU16((c.V0 + 0xCu));
        c.V0 = 0x00000001u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        mem.WriteU32((c.SP + 0x48u), c.V0);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x50u), c.V1);
        c.V0 = mem.ReadU32((c.S5 + 0x20u));
        c.V0 = mem.ReadU32((c.V0 + 0x8u));
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B46D8;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.A1 = c.V0 - 0x1u;
        goto L800B4718;
        L800B46D8: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V0 = (uint)((int)c.V1 >> (int)(c.V0 & 31u));
        c.A1 = c.V0;
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B4DD0;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B4DD0;
        }
        L800B4718: ;
        c.V0 = mem.ReadU32((c.SP + 0x50u));
        c.A0 = mem.ReadU16((c.SP + 0x44u));
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V1 = c.V1 >> 1;
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.SP + 0x46u), (ushort)c.V0);
        c.V0 = c.V0 << 16;
        c.A2 = (uint)((int)c.V0 >> 16);
        c.V0 = 0u - c.A2;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU16((c.V1 + 0x8Cu));
        c.V0 = c.V0 << (int)(c.A0 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.V1 & 31u));
        c.V0 = (int)c.V0 < 3 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B4774;
        }
        c.V0 = 0x00000002u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V0 = c.V0 + c.A2;
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        goto L800B4778;
        L800B4774: ;
        mem.WriteU32((c.SP + 0x4Cu), 0u);
        L800B4778: ;
        c.A0 = mem.ReadU32((c.S5 + 0x20u));
        c.V1 = mem.ReadU32((c.A0 + 0x8u));
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 != 0u) {
            c.A3 = 0x00FF0000u;
            goto L800B4830;
        }
        c.A3 = 0x00FF0000u;
        c.A3 = c.A3 | 0xFFFFu;
        c.V0 = c.V1 | 0x0001u;
        c.A1 = c.A1 << 16;
        c.A1 = (uint)((int)c.A1 >> 14);
        c.A2 = 0xFF000000u;
        mem.WriteU32((c.A0 + 0x8u), c.V0);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.A0 = mem.ReadU32((c.SP + 0x40u));
        c.A1 = c.A1 + c.V0;
        c.V1 = mem.ReadU32(c.A0);
        c.V0 = mem.ReadU32(c.A1);
        c.V1 = c.V1 & c.A2;
        c.V0 = c.V0 & c.A3;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A0, c.V1);
        c.A0 = mem.ReadU32(c.A1);
        c.V1 = mem.ReadU32((c.SP + 0x48u));
        c.V0 = mem.ReadU32((c.SP + 0x40u));
        c.A0 = c.A0 & c.A2;
        c.V1 = c.V1 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - 0x4u;
        c.V0 = c.V0 & c.A3;
        c.A0 = c.A0 | c.V0;
        mem.WriteU32(c.A1, c.A0);
        goto L800B4830;
        L800B4800: ;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x9Cu));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Cu));
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU16((c.A1 + 0x8Eu));
        mem.WriteU32((c.SP + 0x4Cu), c.V0);
        mem.WriteU32((c.SP + 0x50u), 0u);
        mem.WriteU16((c.SP + 0x46u), (ushort)0u);
        mem.WriteU32((c.SP + 0x40u), c.V1);
        mem.WriteU16((c.SP + 0x44u), (ushort)c.A0);
        mem.WriteU32((c.SP + 0x48u), c.A1);
        L800B4830: ;
        c.V0 = mem.ReadU16((c.S5 + 0x1Eu));
        c.V0 = c.V0 & 0x0003u;
        if (c.V0 != 0u) {
            c.A0 = c.S2;
            goto L800B487C;
        }
        c.A0 = c.S2;
        c.V0 = mem.ReadU32((c.S2 + 0x8u));
        c.V1 = 0x00030000u;
        c.V0 = c.V0 & c.V1;
        if (c.V0 != c.V1) {
            c.A2 = c.S5 + 0x10u;
            goto L800B4880;
        }
        c.A2 = c.S5 + 0x10u;
        c.V0 = mem.ReadU16((c.S7 + 0x11Eu));
        if (c.V0 == 0u) {
            c.A0 = c.S7 + 0x12Cu;
            goto L800B48E4;
        }
        c.A0 = c.S7 + 0x12Cu;
        c.V0 = mem.ReadU16((c.S2 + 0xAu));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            c.A0 = c.S2;
            goto L800B48E0;
        }
        c.A0 = c.S2;
        L800B487C: ;
        c.A2 = c.S5 + 0x10u;
        L800B4880: ;
        c.A1 = mem.ReadU16((c.S5 + 0x1Eu));
        c.A3 = c.S5 + 0x14u;
        c.RA = 0x800B488Cu;
        MediEvil_game.func_800ABA38(c, m);
        c.S6 = c.V0;
        c.A0 = 0x1F800000u;
        c.A0 = c.A0 + 0x3Cu;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = c.A0;
        c.RA = 0x800B48A8u;
        MediEvil_game.func_800A4EB8(c, m);
        c.T2 = 0x1F800000u;
        c.T2 = c.T2 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T2);
        c.T5 = mem.ReadU32((c.T2 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T2 + 0x8u));
        c.T5 = mem.ReadU32((c.T2 + 0xCu));
        c.T6 = mem.ReadU32((c.T2 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        goto L800B492C;
        L800B48E0: ;
        c.A0 = c.S7 + 0x12Cu;
        L800B48E4: ;
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.A2 = 0x1F800000u;
        c.A2 = c.A2 + 0x3Cu;
        c.RA = 0x800B48F8u;
        MediEvil_game.func_800A4EB8(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = c.T3 + 0x3Cu;
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        c.S6 = 0u;
        L800B492C: ;
        c.T1 = 0x1F800000u;
        c.T1 = mem.ReadU32((c.T1 + 0x34u));
        c.T4 = mem.ReadU32(c.T1);
        c.T5 = mem.ReadU32((c.T1 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T1 + 0x8u));
        c.T5 = mem.ReadU32((c.T1 + 0xCu));
        c.T6 = mem.ReadU32((c.T1 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T1 + 0x14u));
        c.T5 = mem.ReadU32((c.T1 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T1 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.A2 = mem.ReadU32((c.S3 + 0x10u));
        c.A0 = mem.ReadU16((c.S3 + 0x8u));
        c.V0 = mem.ReadU8((c.SP + 0x85u));
        c.V1 = mem.ReadU32((c.S3 + 0xCu));
        c.V0 = c.V0 << 4;
        c.V0 = c.V0 + c.V1;
        c.S3 = mem.ReadU32(c.V0);
        c.V1 = mem.ReadU16((c.S5 + 0xEu));
        c.S2 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 | 0x0001u;
        c.S1 = c.V1 & 0x003Eu;
        if (c.A0 == 0u) {
            mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
            goto L800B4D40;
        }
        mem.WriteU16((c.S5 + 0xEu), (ushort)c.V1);
        c.S0 = c.SP + 0x20u;
        L800B49B0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A2 = c.A2 + 0x4u;
            goto L800B4D34;
        }
        c.A2 = c.A2 + 0x4u;
        c.V0 = 0x800C0000u;
        c.V0 = c.V0 + RAddr.Game_800B49D0;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        switch (c.V0)
        {
            case 0x800B49ECu: goto L800B49EC;
            case 0x800B4A40u: goto L800B4A40;
            case 0x800B4A94u: goto L800B4A94;
            case 0x800B4AE8u: goto L800B4AE8;
            case 0x800B4B3Cu: goto L800B4B3C;
            case 0x800B4B90u: goto L800B4B90;
            case 0x800B4BE4u: goto L800B4BE4;
            case 0x800B4C38u: goto L800B4C38;
            case 0x800B4D34u: goto L800B4D34;
            case 0x800B4C8Cu: goto L800B4C8C;
            case 0x800B4CE0u: goto L800B4CE0;
            case 0x800B56C4u: goto L800B56C4;
            case 0x800B56D0u: goto L800B56D0;
            case 0x800B56DCu: goto L800B56DC;
            case 0x800B56E8u: goto L800B56E8;
            case 0x800B56F4u: goto L800B56F4;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B49EC: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A18;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A10u;
        MediEvil_game.func_800B097C(c, m);
        goto L800B4D2C;
        L800B4A18: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A38u;
        MediEvil_game.func_800B7738(c, m);
        goto L800B4D2C;
        L800B4A40: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4A6C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4A64u;
        MediEvil_game.func_800B0B3C(c, m);
        goto L800B4D2C;
        L800B4A6C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4A8Cu;
        MediEvil_game.func_800B7A80(c, m);
        goto L800B4D2C;
        L800B4A94: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4AC0;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4AB8u;
        MediEvil_game.func_800B00E4(c, m);
        goto L800B4D2C;
        L800B4AC0: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4AE0u;
        MediEvil_game.func_800B5A44(c, m);
        goto L800B4D2C;
        L800B4AE8: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B14;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B0Cu;
        MediEvil_game.func_800B02BC(c, m);
        goto L800B4D2C;
        L800B4B14: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B34u;
        MediEvil_game.func_800B5E20(c, m);
        goto L800B4D2C;
        L800B4B3C: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4B68;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4B60u;
        MediEvil_game.func_800B0D34(c, m);
        goto L800B4D2C;
        L800B4B68: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4B88u;
        MediEvil_game.func_800B7E38(c, m);
        goto L800B4D2C;
        L800B4B90: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4BBC;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4BB4u;
        MediEvil_game.func_800B0F20(c, m);
        goto L800B4D2C;
        L800B4BBC: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4BDCu;
        MediEvil_game.func_800B8304(c, m);
        goto L800B4D2C;
        L800B4BE4: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C10;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C08u;
        MediEvil_game.func_800B04D4(c, m);
        goto L800B4D2C;
        L800B4C10: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C30u;
        MediEvil_game.func_800B62B4(c, m);
        goto L800B4D2C;
        L800B4C38: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4C64;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4C5Cu;
        MediEvil_game.func_800B06D8(c, m);
        goto L800B4D2C;
        L800B4C64: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4C84u;
        MediEvil_game.func_800B67A0(c, m);
        goto L800B4D2C;
        L800B4C8C: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4CB8;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4CB0u;
        MediEvil_game.func_800B6EEC(c, m);
        goto L800B4D2C;
        L800B4CB8: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4CD8u;
        MediEvil_game.func_800B8A24(c, m);
        goto L800B4D2C;
        L800B4CE0: ;
        if (c.S1 != 0u) {
            mem.WriteU32((c.SP + 0x64u), c.A0);
            goto L800B4D0C;
        }
        mem.WriteU32((c.SP + 0x64u), c.A0);
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        c.RA = 0x800B4D04u;
        MediEvil_game.func_800B72A0(c, m);
        goto L800B4D2C;
        L800B4D0C: ;
        c.A0 = c.S3;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A1 = c.S2;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S4);
        mem.WriteU32((c.SP + 0x18u), c.S1);
        c.RA = 0x800B4D2Cu;
        MediEvil_game.func_800B90E8(c, m);
        L800B4D2C: ;
        c.A2 = mem.ReadU32((c.SP + 0x60u));
        c.A0 = mem.ReadU32((c.SP + 0x64u));
        L800B4D34: ;
        if (c.A0 != 0u) {
            goto L800B49B0;
        }
        L800B4D40: ;
        c.V0 = c.S6 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = c.S7 + 0x14Cu;
            goto L800B4D74;
        }
        c.V0 = c.S7 + 0x14Cu;
        c.T4 = mem.ReadU32(c.V0);
        c.T5 = mem.ReadU32((c.V0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(16, c.T4);
        RecompOne.Runtime.Gte.WriteControl(17, c.T5);
        c.T4 = mem.ReadU32((c.V0 + 0x8u));
        c.T5 = mem.ReadU32((c.V0 + 0xCu));
        c.T6 = mem.ReadU32((c.V0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(18, c.T4);
        RecompOne.Runtime.Gte.WriteControl(19, c.T5);
        RecompOne.Runtime.Gte.WriteControl(20, c.T6);
        L800B4D74: ;
        c.V0 = c.S6 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800B4DA4;
        }
        c.T2 = mem.ReadU8((c.S7 + 0x16Cu));
        c.T3 = mem.ReadU8((c.S7 + 0x16Du));
        c.T1 = mem.ReadU8((c.S7 + 0x16Eu));
        c.T4 = c.T2 << 4;
        c.T5 = c.T3 << 4;
        c.T6 = c.T1 << 4;
        RecompOne.Runtime.Gte.WriteControl(13, c.T4);
        RecompOne.Runtime.Gte.WriteControl(14, c.T5);
        RecompOne.Runtime.Gte.WriteControl(15, c.T6);
        L800B4DA4: ;
        c.V0 = mem.ReadU32(c.FP);
        c.V0 = c.V0 & 0x2000u;
        if (c.V0 == 0u) {
            c.A2 = c.S5;
            goto L800B4DD0;
        }
        c.A2 = c.S5;
        c.A3 = 0x00FF0000u;
        c.T2 = mem.ReadU32((c.SP + 0x88u));
        c.A1 = mem.ReadU8((c.SP + 0x84u));
        c.A0 = mem.ReadU32((c.T2 + 0x4u));
        c.A3 = c.A3 | 0xFFFFu;
        c.RA = 0x800B4DD0u;
        MediEvil_game.func_800AF490(c, m);
        L800B4DD0: ;
        c.RA = mem.ReadU32((c.SP + 0xB4u));
        c.FP = mem.ReadU32((c.SP + 0xB0u));
        c.S7 = mem.ReadU32((c.SP + 0xACu));
        c.S6 = mem.ReadU32((c.SP + 0xA8u));
        c.S5 = mem.ReadU32((c.SP + 0xA4u));
        c.S4 = mem.ReadU32((c.SP + 0xA0u));
        c.S3 = mem.ReadU32((c.SP + 0x9Cu));
        c.S2 = mem.ReadU32((c.SP + 0x98u));
        c.S1 = mem.ReadU32((c.SP + 0x94u));
        c.S0 = mem.ReadU32((c.SP + 0x90u));
        c.SP = c.SP + 0xB8u;
        return;
        c.SP = c.SP - 0x20u;
        mem.WriteU32((c.SP + 0x14u), c.S1);
        c.S1 = c.A0;
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.RA);
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4E1C), 0u);
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4E24), 0u);
        c.S2 = c.A1;
        c.RA = 0x800B4E30u;
        MediEvil_game.func_800C0A64(c, m);
        c.S0 = 0x800F0000u;
        c.S0 = c.S0 + RAddr.Game_800B4E34;
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x4FC8u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4E44), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x4F60u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4E54), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x50CCu;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4E64), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x5188u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4E74), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x5404u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4E84), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x543Cu;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4E94), c.V0);
        c.V0 = 0x800B0000u;
        c.V0 = c.V0 + 0x50BCu;
        c.A0 = c.S0;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4EA8), c.S0);
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4EB0), c.V0);
        c.A1 = 0x000001E0u;
        c.RA = 0x800B4EBCu;
        MediEvil_game.func_800C0F24(c, m);
        c.T0 = 0u;
        c.T1 = 0x000000FFu;
        c.A0 = c.S0 + 0x40u;
        c.A3 = 0x800F0000u;
        c.A3 = c.A3 + RAddr.Game_800B4ECC;
        c.A2 = 0x800F0000u;
        c.A2 = c.A2 + RAddr.Game_800B4ED4;
        mem.WriteU32((c.S0 + 0x30u), c.S1);
        mem.WriteU32((c.S0 + 0x120u), c.S2);
        L800B4EE0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32((c.A0 - 0x10u));
        c.A1 = c.S0 + 0x5Du;
        mem.WriteU32((c.A0 - 0x34u), 0u);
        mem.WriteU32((c.A0 - 0x30u), c.S0);
        mem.WriteU8(c.V0, (byte)c.T1);
        c.V0 = mem.ReadU32((c.A0 - 0x10u));
        c.V1 = 0x00000005u;
        mem.WriteU8((c.V0 + 0x1u), (byte)0u);
        mem.WriteU32((c.A0 - 0x4u), c.A2);
        mem.WriteU32(c.A0, c.A3);
        L800B4F08: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        mem.WriteU8(c.A1, (byte)c.T1);
        c.V1 = c.V1 - 0x1u;
        if ((int)c.V1 >= 0) {
            c.A1 = c.A1 + 0x1u;
            goto L800B4F08;
        }
        c.A1 = c.A1 + 0x1u;
        c.A3 = c.A3 + 0x23u;
        c.A2 = c.A2 + 0x23u;
        c.T0 = c.T0 + 0x1u;
        c.A0 = c.A0 + 0xF0u;
        c.V0 = (int)c.T0 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.S0 = c.S0 + 0xF0u;
            goto L800B4EE0;
        }
        c.S0 = c.S0 + 0xF0u;
        c.RA = 0x800B4F3Cu;
        MediEvil_game.func_800BFE04(c, m);
        c.V0 = 0x00000001u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B4F44), c.V0);
        c.RA = mem.ReadU32((c.SP + 0x1Cu));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x20u;
        return;
        c.V0 = mem.ReadU8((c.A0 + 0x49u));
        if (c.V0 == 0u) {
            goto L800B4FC0;
        }
        c.V1 = c.A0 + 0x5Du;
        c.A1 = 0x000000FFu;
        c.V0 = 0x00000005u;
        mem.WriteU8((c.A0 + 0x49u), (byte)0u);
        mem.WriteU8((c.A0 + 0x46u), (byte)0u);
        mem.WriteU16((c.A0 + 0xE6u), (ushort)0u);
        mem.WriteU32((c.A0 + 0x14u), 0u);
        mem.WriteU32((c.A0 + 0x18u), 0u);
        mem.WriteU8((c.A0 + 0xE3u), (byte)0u);
        mem.WriteU8((c.A0 + 0xE4u), (byte)0u);
        mem.WriteU16((c.A0 + 0xE6u), (ushort)0u);
        mem.WriteU8((c.A0 + 0xE9u), (byte)0u);
        mem.WriteU8((c.A0 + 0xEAu), (byte)0u);
        mem.WriteU32(c.A0, 0u);
        mem.WriteU32((c.A0 + 0x4u), 0u);
        mem.WriteU32((c.A0 + 0x8u), 0u);
        L800B4FB0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        mem.WriteU8(c.V1, (byte)c.A1);
        c.V0 = c.V0 - 0x1u;
        if ((int)c.V0 >= 0) {
            c.V1 = c.V1 + 0x1u;
            goto L800B4FB0;
        }
        c.V1 = c.V1 + 0x1u;
        L800B4FC0: ;
        return;
        c.SP = c.SP - 0x28u;
        c.A1 = c.A0;
        mem.WriteU32((c.SP + 0x14u), c.S1);
        c.S1 = 0x800F0000u;
        c.S1 = c.S1 + RAddr.Game_800B4FD8;
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        c.S3 = 0xFFFFFFF7u;
        mem.WriteU32((c.SP + 0x18u), c.S2);
        c.S2 = 0x800F0000u;
        c.S2 = c.S2 - RAddr.Game_800B4FEC;
        mem.WriteU32((c.SP + 0x20u), c.RA);
        mem.WriteU32((c.SP + 0x10u), c.S0);
        L800B4FF8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V1 = 0x800F0000u;
        c.V1 = mem.ReadU32((c.V1 - RAddr.Game_800B4FFC));
        c.V0 = c.V1 << 4;
        c.V0 = c.V0 - c.V1;
        c.V0 = c.V0 << 4;
        if (c.A1 == c.S3) {
            c.S0 = c.V0 + c.S1;
            goto L800B503C;
        }
        c.S0 = c.V0 + c.S1;
        if (c.A1 != 0u) {
            c.V0 = c.V1 << 2;
            goto L800B502C;
        }
        c.V0 = c.V1 << 2;
        c.V0 = c.V0 + c.S2;
        mem.WriteU32(c.V0, 0u);
        goto L800B503C;
        L800B502C: ;
        c.A0 = c.S0;
        c.RA = 0x800B5034u;
        MediEvil_game.func_800C0E0C(c, m);
        c.A0 = c.S0;
        c.RA = 0x800B503Cu;
        MediEvil_game.func_800B50BC(c, m);
        L800B503C: ;
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - RAddr.Game_800B5040));
        c.V1 = 0x800F0000u;
        c.V1 = mem.ReadU32((c.V1 - RAddr.Game_800B5048));
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B5050), 0u);
        mem.WriteU16((c.V0 + 0xAu), (ushort)0u);
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - RAddr.Game_800B505C));
        c.V1 = c.V1 + 0x1u;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B5068), c.V1);
        c.V0 = (int)c.V0 < (int)c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = 0x00000001u;
            goto L800B5094;
        }
        c.V0 = 0x00000001u;
        c.A0 = c.V1 << 4;
        c.A0 = c.A0 - c.V1;
        c.A0 = c.A0 << 4;
        c.A0 = c.A0 + c.S1;
        c.RA = 0x800B508Cu;
        MediEvil_game.func_800C00F4(c, m);
        c.A1 = 0u | 0xFFFFu;
        goto L800B5098;
        L800B5094: ;
        c.A1 = 0u | 0xFFFFu;
        L800B5098: ;
        if (c.V0 == 0u) {
            goto L800B4FF8;
        }
        c.RA = mem.ReadU32((c.SP + 0x20u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x28u;
        return;
        c.V0 = mem.ReadU8((c.A0 + 0x37u));
        mem.WriteU8((c.A0 + 0x37u), (byte)0u);
        mem.WriteU8((c.A0 + 0x38u), (byte)c.V0);
        return;
        c.V0 = mem.ReadU8((c.A0 + 0x45u));
        c.A1 = mem.ReadU8((c.A0 + 0x37u));
        if (c.A1 == 0u) {
            c.V1 = c.V0 - 0x3u;
            goto L800B50F4;
        }
        c.V1 = c.V0 - 0x3u;
        c.V0 = 0x0000004Du;
        if (c.A1 == c.V0) {
            goto L800B513C;
        }
        goto L800B515C;
        L800B50F4: ;
        c.V0 = (int)c.V1 < 6 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.A0 + c.V1;
            goto L800B5110;
        }
        c.V0 = c.A0 + c.V1;
        c.V0 = mem.ReadU8((c.V0 + 0x57u));
        if (c.V0 == 0u) {
            c.V0 = 0u;
            goto L800B5180;
        }
        c.V0 = 0u;
        L800B5110: ;
        c.V0 = mem.ReadU8((c.A0 + 0x34u));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0u;
            goto L800B5180;
        }
        c.V0 = 0u;
        c.V0 = mem.ReadU32((c.A0 + 0x28u));
        c.V0 = c.V0 + c.V1;
        L800B5130: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8(c.V0);
        goto L800B5180;
        L800B513C: ;
        c.V0 = mem.ReadU8((c.A0 + 0x36u));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x000000FFu;
            goto L800B5180;
        }
        c.V0 = 0x000000FFu;
        c.V0 = mem.ReadU32((c.A0 + 0x2Cu));
        c.V0 = c.V0 + c.V1;
        goto L800B5130;
        L800B515C: ;
        c.V0 = mem.ReadU8((c.A0 + 0x36u));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0u;
            goto L800B5180;
        }
        c.V0 = 0u;
        c.V0 = mem.ReadU32((c.A0 + 0x2Cu));
        c.V0 = c.V0 + c.V1;
        c.V0 = mem.ReadU8(c.V0);
        L800B5180: ;
        return;
        c.SP = c.SP - 0x18u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.S0 = c.A0;
        c.A0 = c.S0 + 0x57u;
        mem.WriteU32((c.SP + 0x14u), c.RA);
        c.A1 = 0x00000006u;
        c.RA = 0x800B51A4u;
        MediEvil_game.func_800C0F24(c, m);
        c.V0 = mem.ReadU16((c.S0 + 0xE6u));
        if (c.V0 == 0u) {
            goto L800B5300;
        }
        c.V0 = mem.ReadU32((c.S0 + 0x28u));
        if (c.V0 == 0u) {
            goto L800B5300;
        }
        c.V0 = mem.ReadU8((c.S0 + 0x34u));
        c.V0 = c.V0 < 0x00000007u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.T1 = 0x00000006u;
            goto L800B51DC;
        }
        c.T1 = 0x00000006u;
        c.T1 = mem.ReadU8((c.S0 + 0x34u));
        L800B51DC: ;
        c.V0 = mem.ReadU8((c.S0 + 0xE9u));
        if (c.V0 == 0u) {
            c.T0 = 0u;
            goto L800B53F4;
        }
        c.T0 = 0u;
        c.T3 = 0x00000001u;
        c.T2 = 0u;
        L800B51F4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU32((c.S0 + 0x4u));
        c.A2 = 0u;
        c.V0 = c.T2 + c.V0;
        c.V0 = mem.ReadU8((c.V0 + 0x2u));
        if (c.V0 == 0u) {
            c.A3 = 0x00000001u;
            goto L800B5214;
        }
        c.A3 = 0x00000001u;
        c.A3 = 0x000000FFu;
        L800B5214: ;
        c.A1 = c.S0 + 0x5Du;
        c.A0 = mem.ReadU32((c.S0 + 0x28u));
        if (c.T1 == 0u) {
            c.V1 = 0u;
            goto L800B525C;
        }
        c.V1 = 0u;
        L800B5224: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8(c.A1);
        if (c.V0 != c.T0) {
            goto L800B5248;
        }
        c.V0 = mem.ReadU8(c.A0);
        c.V0 = c.V0 & c.A3;
        if (c.V0 != 0u) {
            goto L800B529C;
        }
        L800B5248: ;
        c.A1 = c.A1 + 0x1u;
        c.V1 = c.V1 + 0x1u;
        c.V0 = (int)c.V1 < (int)c.T1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.A0 = c.A0 + 0x1u;
            goto L800B5224;
        }
        c.A0 = c.A0 + 0x1u;
        L800B525C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if (c.A2 == 0u) {
            goto L800B52E4;
        }
        c.V0 = mem.ReadU32((c.S0 + 0x4u));
        c.V0 = c.T2 + c.V0;
        c.V1 = mem.ReadU8((c.V0 + 0x3u));
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - RAddr.Game_800B5278));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < 61 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B52A4;
        }
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B5298), c.V1);
        goto L800B52A8;
        L800B529C: ;
        c.A2 = 0x00000001u;
        goto L800B525C;
        L800B52A4: ;
        c.A2 = 0u;
        L800B52A8: ;
        if (c.A2 == 0u) {
            goto L800B52E4;
        }
        c.A1 = c.S0 + 0x5Du;
        c.A0 = c.S0 + 0x57u;
        if (c.T1 == 0u) {
            c.V1 = 0u;
            goto L800B52E4;
        }
        c.V1 = 0u;
        L800B52C0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU8(c.A1);
        if (c.V0 != c.T0) {
            c.A1 = c.A1 + 0x1u;
            goto L800B52D4;
        }
        c.A1 = c.A1 + 0x1u;
        mem.WriteU8(c.A0, (byte)c.T3);
        L800B52D4: ;
        c.V1 = c.V1 + 0x1u;
        c.V0 = (int)c.V1 < (int)c.T1 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.A0 = c.A0 + 0x1u;
            goto L800B52C0;
        }
        c.A0 = c.A0 + 0x1u;
        L800B52E4: ;
        c.V0 = mem.ReadU8((c.S0 + 0xE9u));
        c.T0 = c.T0 + 0x1u;
        c.V0 = (int)c.T0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.T2 = c.T2 + 0x5u;
            goto L800B51F4;
        }
        c.T2 = c.T2 + 0x5u;
        goto L800B53F4;
        L800B5300: ;
        c.V1 = mem.ReadU8((c.S0 + 0xE8u));
        c.V0 = c.V1 - 0x4u;
        c.V0 = c.V0 < 0x00000002u ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = 0x00000007u;
            goto L800B5320;
        }
        c.V0 = 0x00000007u;
        if (c.V1 != c.V0) {
            goto L800B53B4;
        }
        L800B5320: ;
        c.V0 = mem.ReadU16((c.S0 + 0xE6u));
        if (c.V0 != 0u) {
            goto L800B53B4;
        }
        c.V0 = mem.ReadU8((c.S0 + 0x34u));
        c.V0 = c.V0 < 0x00000002u ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V1 = 0x00000040u;
            goto L800B53B4;
        }
        c.V1 = 0x00000040u;
        c.A0 = mem.ReadU32((c.S0 + 0x28u));
        c.V0 = mem.ReadU8(c.A0);
        c.V0 = c.V0 & 0x00C0u;
        if (c.V0 != c.V1) {
            goto L800B53F4;
        }
        c.V0 = mem.ReadU8((c.A0 + 0x1u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 == 0u) {
            goto L800B53F4;
        }
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - RAddr.Game_800B5378));
        c.V0 = c.V0 + 0xAu;
        c.V0 = (int)c.V0 < 61 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000001u;
            goto L800B53F4;
        }
        c.V0 = 0x00000001u;
        mem.WriteU8((c.S0 + 0x58u), (byte)c.V0);
        mem.WriteU8((c.S0 + 0x57u), (byte)c.V0);
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - RAddr.Game_800B539C));
        c.V0 = c.V0 + 0xAu;
        c.At = 0x800F0000u;
        mem.WriteU32((c.At - RAddr.Game_800B53B0), c.V0);
        goto L800B53F4;
        L800B53B4: ;
        c.V1 = mem.ReadU8((c.S0 + 0xE8u));
        c.V0 = 0x00000003u;
        if (c.V1 != c.V0) {
            c.V0 = 0x00000001u;
            goto L800B53CC;
        }
        c.V0 = 0x00000001u;
        mem.WriteU8((c.S0 + 0x57u), (byte)c.V0);
        goto L800B53F4;
        L800B53CC: ;
        c.V0 = mem.ReadU16((c.S0 + 0xE6u));
        if (c.V0 != 0u) {
            c.V0 = 0x00000001u;
            goto L800B53F4;
        }
        c.V0 = 0x00000001u;
        c.V1 = 0x00000005u;
        c.A0 = c.S0 + 0x5u;
        L800B53E4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        mem.WriteU8((c.A0 + 0x57u), (byte)c.V0);
        c.V1 = c.V1 - 0x1u;
        if ((int)c.V1 >= 0) {
            c.A0 = c.A0 - 0x1u;
            goto L800B53E4;
        }
        c.A0 = c.A0 - 0x1u;
        L800B53F4: ;
        c.RA = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x18u;
        return;
        c.A1 = 0u;
        c.A2 = 0x00000010u;
        c.V1 = 0x800F0000u;
        c.V1 = c.V1 + RAddr.Game_800B5410;
        L800B5414: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        if (c.A0 == c.V1) {
            c.V0 = c.A2;
            goto L800B5434;
        }
        c.V0 = c.A2;
        c.A2 = c.A2 + 0x10u;
        c.A1 = c.A1 + 0x1u;
        c.V0 = (int)c.A1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V1 = c.V1 + 0xF0u;
            goto L800B5414;
        }
        c.V1 = c.V1 + 0xF0u;
        c.V0 = 0x000000FFu;
        L800B5434: ;
        return;
        c.V0 = 0x800F0000u;
        c.V0 = c.V0 + RAddr.Game_800B5440;
        c.A0 = c.A0 & 0x00F0u;
        if (c.A0 == 0u) {
            goto L800B5454;
        }
        c.V0 = c.V0 + 0xF0u;
        L800B5454: ;
        return;
        c.SP = c.SP - 0x18u;
        mem.WriteU32((c.SP + 0x10u), c.RA);
        c.RA = 0x800B5470u;
        MediEvil_game.func_800C0028(c, m);
        c.RA = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x18u;
        return;
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - RAddr.Game_800B5484));
        c.SP = c.SP - 0x18u;
        mem.WriteU32((c.SP + 0x10u), c.RA);
        c.RA = 0x800B5498u;
        Dispatcher.Call(c, m, c.V0);
        c.V1 = c.V0;
        c.V0 = mem.ReadU8((c.V1 + 0x37u));
        if (c.V0 != 0u) {
            goto L800B54F4;
        }
        c.V0 = mem.ReadU8((c.V1 + 0x38u));
        if (c.V0 != 0u) {
            goto L800B54F4;
        }
        c.V0 = mem.ReadU32((c.V1 + 0x10u));
        if (c.V1 == c.V0) {
            goto L800B54DC;
        }
        c.V0 = mem.ReadU8((c.V1 + 0x39u));
        if (c.V0 != 0u) {
            goto L800B54F4;
        }
        L800B54DC: ;
        c.V0 = mem.ReadU32((c.V1 + 0x30u));
        c.V0 = mem.ReadU8(c.V0);
        if (c.V0 == 0u) {
            goto L800B5538;
        }
        L800B54F4: ;
        c.A0 = mem.ReadU8((c.V1 + 0x49u));
        c.V0 = 0x00000003u;
        if (c.A0 == c.V0) {
            c.V0 = (int)c.A0 < 4 ? 1u : 0u;
            goto L800B5530;
        }
        c.V0 = (int)c.A0 < 4 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000002u;
            goto L800B551C;
        }
        c.V0 = 0x00000002u;
        if (c.A0 == c.V0) {
            c.V0 = 0x00000001u;
            goto L800B553C;
        }
        c.V0 = 0x00000001u;
        goto L800B5538;
        L800B551C: ;
        c.V0 = 0x00000006u;
        if (c.A0 == c.V0) {
            c.V0 = 0x00000004u;
            goto L800B553C;
        }
        c.V0 = 0x00000004u;
        goto L800B5538;
        L800B5530: ;
        c.V0 = 0x00000001u;
        goto L800B553C;
        L800B5538: ;
        c.V0 = mem.ReadU8((c.V1 + 0x49u));
        L800B553C: ;
        c.RA = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x18u;
        return;
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - RAddr.Game_800B5550));
        c.SP = c.SP - 0x20u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.S0 = c.A1;
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.RA);
        c.S1 = c.A2;
        c.RA = 0x800B5570u;
        Dispatcher.Call(c, m, c.V0);
        c.V1 = c.V0;
        c.V0 = 0x00000003u;
        if (c.S0 == c.V0) {
            c.V0 = (int)c.S0 < 4 ? 1u : 0u;
            goto L800B55D4;
        }
        c.V0 = (int)c.S0 < 4 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000001u;
            goto L800B55A0;
        }
        c.V0 = 0x00000001u;
        if (c.S0 == c.V0) {
            c.V0 = 0x00000002u;
            goto L800B55BC;
        }
        c.V0 = 0x00000002u;
        if (c.S0 == c.V0) {
            c.V0 = 0u;
            goto L800B55C8;
        }
        c.V0 = 0u;
        goto L800B5630;
        L800B55A0: ;
        c.V0 = 0x00000004u;
        if (c.S0 == c.V0) {
            c.V0 = 0x00000064u;
            goto L800B55E0;
        }
        c.V0 = 0x00000064u;
        if (c.S0 == c.V0) {
            c.V0 = 0u;
            goto L800B5620;
        }
        c.V0 = 0u;
        goto L800B5630;
        L800B55BC: ;
        c.V0 = mem.ReadU8((c.V1 + 0xE8u));
        goto L800B5630;
        L800B55C8: ;
        c.V0 = mem.ReadU16((c.V1 + 0xE6u));
        goto L800B5630;
        L800B55D4: ;
        c.V0 = mem.ReadU8((c.V1 + 0xE4u));
        goto L800B5630;
        L800B55E0: ;
        if ((int)c.S1 >= 0) {
            goto L800B55F4;
        }
        c.V0 = mem.ReadU8((c.V1 + 0xE3u));
        goto L800B5630;
        L800B55F4: ;
        c.V0 = mem.ReadU8((c.V1 + 0xE3u));
        c.V0 = (int)c.S1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.S1 << 1;
            goto L800B562C;
        }
        c.V0 = c.S1 << 1;
        c.V1 = mem.ReadU32(c.V1);
        c.V0 = c.V0 + c.V1;
        c.V0 = mem.ReadU16(c.V0);
        goto L800B5630;
        L800B5620: ;
        c.V0 = mem.ReadU32((c.V1 + 0x4Cu));
        goto L800B5630;
        L800B562C: ;
        c.V0 = 0u;
        L800B5630: ;
        c.RA = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x20u;
        return;
        c.V0 = 0x800F0000u;
        c.V0 = mem.ReadU32((c.V0 - RAddr.Game_800B5648));
        c.SP = c.SP - 0x20u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.S0 = c.A1;
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.RA);
        c.S1 = c.A2;
        c.RA = 0x800B5668u;
        Dispatcher.Call(c, m, c.V0);
        if ((int)c.S0 >= 0) {
            c.V1 = c.V0;
            goto L800B567C;
        }
        c.V1 = c.V0;
        c.V0 = mem.ReadU8((c.V1 + 0xE9u));
        goto L800B5704;
        L800B567C: ;
        c.V0 = mem.ReadU8((c.V1 + 0xE9u));
        c.V0 = (int)c.S0 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.S0 << 2;
            goto L800B5700;
        }
        c.V0 = c.S0 << 2;
        c.V1 = mem.ReadU32((c.V1 + 0x4u));
        c.V0 = c.V0 + c.S0;
        c.A2 = c.S1 - 0x1u;
        c.V1 = c.V1 + c.V0;
        c.V0 = c.A2 < 0x00000005u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.A2 << 2;
            goto L800B5700;
        }
        c.V0 = c.A2 << 2;
        c.At = 0x800C0000u;
        c.At = c.At + c.V0;
        c.V0 = mem.ReadU32((c.At + RAddr.Game_800B56B4));
        switch (c.V0)
        {
            case 0x800B56C4u: goto L800B56C4;
            case 0x800B56D0u: goto L800B56D0;
            case 0x800B56DCu: goto L800B56DC;
            case 0x800B56E8u: goto L800B56E8;
            case 0x800B56F4u: goto L800B56F4;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800B56C4: ;
        c.V0 = mem.ReadU8(c.V1);
        goto L800B5704;
        L800B56D0: ;
        c.V0 = mem.ReadU8((c.V1 + 0x1u));
        goto L800B5704;
        L800B56DC: ;
        c.V0 = mem.ReadU8((c.V1 + 0x2u));
        goto L800B5704;
        L800B56E8: ;
        c.V0 = mem.ReadU8((c.V1 + 0x3u));
        goto L800B5704;
        L800B56F4: ;
        c.V0 = mem.ReadU8((c.V1 + 0x4u));
        goto L800B5704;
        L800B5700: ;
        c.V0 = 0u;
        L800B5704: ;
        c.RA = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.SP = c.SP + 0x20u;
        return;
    }

    public static void func_800A10D8(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x108u;
        c.V1 = 0x800F0000u;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = c.V1 - RAddr.Game_800A10E8;
        mem.WriteU32((c.SP + 0x104u), c.RA);
        mem.WriteU32((c.SP + 0x100u), c.FP);
        mem.WriteU32((c.SP + 0xFCu), c.S7);
        mem.WriteU32((c.SP + 0xF8u), c.S6);
        mem.WriteU32((c.SP + 0xF4u), c.S5);
        mem.WriteU32((c.SP + 0xF0u), c.S4);
        mem.WriteU32((c.SP + 0xECu), c.S3);
        mem.WriteU32((c.SP + 0xE8u), c.S2);
        mem.WriteU32((c.SP + 0xE4u), c.S1);
        mem.WriteU32((c.SP + 0xE0u), c.S0);
        mem.WriteU32((c.SP + 0x108u), c.A0);
        mem.WriteU32((c.SP + 0x10Cu), c.A1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x10u));
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 < c.V0 ? 1u : 0u;
        if (c.V1 == 0u) {
            c.S5 = 0u;
            goto L800A219C;
        }
        c.S5 = 0u;
        c.T1 = mem.ReadU32((c.A0 + 0x8u));
        mem.WriteU32((c.SP + 0x94u), c.T1);
        c.T2 = mem.ReadU32((c.T1 + 0x2Cu));
        mem.WriteU32((c.SP + 0x98u), c.T2);
        c.V0 = mem.ReadU32((c.T2 + 0x4u));
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.SP + 0xA0u), c.V0);
        c.V1 = mem.ReadU16((c.V0 + 0x8u));
        c.V0 = c.V1 & 0x1000u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0xB8u), 0u);
            goto L800A219C;
        }
        mem.WriteU32((c.SP + 0xB8u), 0u);
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 != 0u) {
            goto L800A219C;
        }
        c.T3 = mem.ReadU32((c.SP + 0xA0u));
        c.S3 = mem.ReadU32((c.T3 + 0x20u));
        c.S4 = mem.ReadU32((c.T3 + 0x14u));
        c.A0 = (uint)(short)mem.ReadU16((c.S3 + 0x12u));
        c.S2 = mem.ReadU32(c.S3);
        if ((int)c.A0 < 0) {
            goto L800A219C;
        }
        c.V0 = mem.ReadU32((c.S2 + 0x8u));
        c.V1 = mem.ReadU32((c.V0 + 0x4u));
        c.V0 = c.A0 << 4;
        c.V1 = c.V1 + c.V0;
        mem.WriteU32((c.SP + 0x9Cu), c.V1);
        c.A0 = (uint)(short)mem.ReadU16((c.S3 + 0x10u));
        c.T7 = mem.ReadU16((c.S3 + 0x10u));
        if ((int)c.A0 < 0) {
            mem.WriteU16((c.SP + 0xA8u), (ushort)c.T7);
            goto L800A219C;
        }
        mem.WriteU16((c.SP + 0xA8u), (ushort)c.T7);
        c.V0 = mem.ReadU16((c.T1 + 0xAu));
        c.V0 = c.V0 & 0x0020u;
        if (c.V0 == 0u) {
            c.FP = c.S5;
            goto L800A11F8;
        }
        c.FP = c.S5;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x90u));
        c.FP = c.S5 < c.V0 ? 1u : 0u;
        L800A11F8: ;
        c.V1 = mem.ReadU8(c.S4);
        c.V0 = 0x00000031u;
        if (c.V1 != c.V0) {
            c.V1 = c.A0 << 1;
            goto L800A1228;
        }
        c.V1 = c.A0 << 1;
        c.T1 = mem.ReadU32((c.SP + 0x9Cu));
        c.V1 = mem.ReadU32((c.T1 + 0x8u));
        c.V0 = c.A0 << 1;
        c.V0 = c.V0 + c.V1;
        c.S0 = mem.ReadU16(c.V0);
        goto L800A1248;
        L800A1228: ;
        c.T2 = mem.ReadU32((c.SP + 0x9Cu));
        c.V0 = mem.ReadU32((c.T2 + 0x8u));
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU16(c.V1);
        c.S0 = c.V0 - 0x1u;
        L800A1248: ;
        c.T3 = mem.ReadU32((c.SP + 0xA0u));
        c.T7 = mem.ReadU32((c.SP + 0x9Cu));
        c.V0 = mem.ReadU16((c.T3 + 0xAu));
        c.T7 = mem.ReadU16((c.T7 + 0x2u));
        c.V0 = c.V0 & 0x0010u;
        if (c.V0 == 0u) {
            mem.WriteU16((c.SP + 0xB0u), (ushort)c.T7);
            goto L800A1298;
        }
        mem.WriteU16((c.SP + 0xB0u), (ushort)c.T7);
        c.V0 = mem.ReadU16((c.T3 + 0x8u));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            goto L800A128C;
        }
        c.V0 = mem.ReadU32(0x00000034u);
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.SP + 0xB8u), c.V0);
        goto L800A1298;
        L800A128C: ;
        c.T1 = mem.ReadU32((c.S3 + 0x1Cu));
        mem.WriteU32((c.SP + 0xB8u), c.T1);
        L800A1298: ;
        c.T2 = mem.ReadU32((c.SP + 0x94u));
        c.V0 = mem.ReadU16((c.T2 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            goto L800A12C0;
        }
        c.A1 = mem.ReadU32((c.T2 + 0xCu));
        goto L800A12D4;
        L800A12C0: ;
        c.T3 = mem.ReadU32((c.SP + 0x94u));
        c.V0 = mem.ReadU32((c.T3 + 0xCu));
        c.A1 = c.V0 + 0x60u;
        L800A12D4: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x38u));
        if (c.V0 == c.A1) {
            c.A2 = c.SP + 0x80u;
            goto L800A1308;
        }
        c.A2 = c.SP + 0x80u;
        c.T7 = mem.ReadU32((c.SP + 0x10Cu));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A1);
        c.A0 = c.T7 + 0xA0u;
        c.RA = 0x800A1304u;
        MediEvil_game.func_800A4EB8(c, m);
        c.A2 = c.SP + 0x80u;
        L800A1308: ;
        c.T1 = mem.ReadU32((c.SP + 0x94u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU32((c.A0 + 0x38u));
        c.A1 = c.T1 + 0x10u;
        c.RA = 0x800A131Cu;
        MediEvil_game.func_800A48FC(c, m);
        c.A1 = c.SP + 0x78u;
        c.T2 = mem.ReadU32((c.SP + 0x10Cu));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.T0 = 0x1F800000u;
        c.T0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU16((c.SP + 0x80u));
        c.A0 = c.T2 + 0xA0u;
        c.A2 = c.A2 + 0x14u;
        c.V0 = mem.ReadU16((c.T0 + 0x14u));
        c.A3 = mem.ReadU16((c.T2 + 0xB4u));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x78u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x18u));
        c.V1 = mem.ReadU16((c.SP + 0x84u));
        c.A3 = mem.ReadU16((c.T2 + 0xB8u));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x7Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T0 + 0x1Cu));
        c.V1 = mem.ReadU16((c.SP + 0x88u));
        c.A3 = mem.ReadU16((c.T2 + 0xBCu));
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - c.A3;
        mem.WriteU16((c.SP + 0x7Cu), (ushort)c.V0);
        c.RA = 0x800A1388u;
        MediEvil_game.func_800A48FC(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = mem.ReadU32((c.T3 + 0x34u));
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T3 + 0x14u));
        c.T5 = mem.ReadU32((c.T3 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T3 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T7 = 0x800F0000u;
        c.T7 = c.T7 - RAddr.Game_800A13D8;
        { var _lw = mem.ReadU32(c.T7); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.T7 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.A1 = c.SP + 0x90u;
        { var _sw = RecompOne.Runtime.Gte.Read(19); mem.WriteU32(c.A1, _sw);  }
        c.T1 = mem.ReadU32((c.SP + 0x98u));
        c.V1 = mem.ReadU32((c.SP + 0x90u));
        c.V0 = TerrainPatch.MeshClipDistance(mem.ReadU16((c.T1 + 0x8u)), m);
        c.V0 = c.V0 < c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800A219C;
        }
        c.V1 = mem.ReadU16((c.S2 + 0x2u));
        c.V0 = c.V1 & 0x0002u;
        if (c.V0 == 0u) {
            c.A0 = c.S0 << 16;
            goto L800A1448;
        }
        c.A0 = c.S0 << 16;
        c.V0 = mem.ReadU32((c.S2 + 0x10u));
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.A0 = (uint)((int)c.A0 >> 10);
        c.A0 = c.V0 + c.A0;
        goto L800A1458;
        L800A1448: ;
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 == 0u) {
            goto L800A148C;
        }
        c.A0 = mem.ReadU32((c.S2 + 0xCu));
        L800A1458: ;
        c.RA = 0x800A1460u;
        MediEvil_game.func_8009A540(c, m);
        if (c.V0 == 0u) {
            goto L800A219C;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x90u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Eu));
        c.V1 = c.V1 >> (int)(c.V0 & 31u);
        c.V1 = c.V1 < c.A0 ? 1u : 0u;
        if (c.V1 == 0u) {
            goto L800A219C;
        }
        L800A148C: ;
        c.T2 = mem.ReadU32((c.SP + 0x108u));
        c.V1 = mem.ReadU32((c.T2 + 0x20u));
        if (c.V1 == 0u) {
            c.V0 = 0x00000002u;
            goto L800A17D8;
        }
        c.V0 = 0x00000002u;
        c.V0 = mem.ReadU32((c.V1 + 0x8u));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 != 0u) {
            c.V0 = c.V1 + 0x18u;
            goto L800A1618;
        }
        c.V0 = c.V1 + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw);  }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw);  }
        c.T3 = mem.ReadU32((c.SP + 0x94u));
        c.A1 = mem.ReadU32((c.V1 + 0x14u));
        c.A0 = mem.ReadU32((c.T3 + 0xCu));
        if (c.A1 == c.A0) {
            goto L800A15E0;
        }
        c.V0 = mem.ReadU16((c.T3 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            c.S0 = 0x1F800000u;
            goto L800A14FC;
        }
        c.S0 = 0x1F800000u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.A0);
        c.S0 = c.S0 + 0x60u;
        goto L800A150C;
        L800A14FC: ;
        c.V0 = c.A1 + 0x60u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.V0);
        c.S0 = c.S0 + 0x60u;
        L800A150C: ;
        c.A2 = c.S0;
        c.T7 = mem.ReadU32((c.SP + 0x10Cu));
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU32((c.A1 + 0x38u));
        c.S1 = c.T7 + 0xA0u;
        c.A0 = c.S1;
        c.RA = 0x800A1528u;
        MediEvil_game.func_800A4EB8(c, m);
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x38u));
        c.T1 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = mem.ReadU16((c.A2 + 0x14u));
        c.V1 = mem.ReadU16((c.T1 + 0xB4u));
        c.A0 = c.S1;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x78u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x18u));
        c.V1 = mem.ReadU16((c.T1 + 0xB8u));
        c.A1 = c.SP + 0x78u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x7Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x1Cu));
        c.V1 = mem.ReadU16((c.T1 + 0xBCu));
        c.A2 = c.S0 + 0x14u;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x7Cu), (ushort)c.V0);
        c.RA = 0x800A1574u;
        MediEvil_game.func_800A48FC(c, m);
        c.T4 = mem.ReadU32(c.S0);
        c.T5 = mem.ReadU32((c.S0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S0 + 0x8u));
        c.T5 = mem.ReadU32((c.S0 + 0xCu));
        c.T6 = mem.ReadU32((c.S0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.S0 + 0x14u));
        c.T5 = mem.ReadU32((c.S0 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.S0 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.T2 = mem.ReadU32((c.SP + 0x108u));
        c.V0 = mem.ReadU32((c.T2 + 0x20u));
        c.V0 = c.V0 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        goto L800A15F4;
        L800A15E0: ;
        RecompOne.Runtime.Gte.Rtps(12, false);
        c.V0 = c.V1 + 0x10u;
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32(c.V0, _sw);  }
        L800A15F4: ;
        c.T3 = mem.ReadU32((c.SP + 0x108u));
        c.V0 = mem.ReadU32((c.T3 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        c.A0 = mem.ReadU32((c.V0 + 0x28u));
        c.V1 = c.V1 + c.A0;
        mem.WriteU32((c.V0 + 0x10u), c.V1);
        L800A1618: ;
        c.T7 = mem.ReadU32((c.SP + 0x108u));
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x4u));
        c.V1 = mem.ReadU32((c.T7 + 0x20u));
        c.V0 = c.V0 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32((c.V1 + 0x20u));
        mem.WriteU32((c.SP + 0x50u), c.V0);
        c.V0 = mem.ReadU32((c.T7 + 0x20u));
        c.V0 = mem.ReadU16((c.V0 + 0xEu));
        mem.WriteU16((c.SP + 0x54u), (ushort)c.V0);
        c.V0 = mem.ReadU32((c.T7 + 0x20u));
        c.V1 = mem.ReadU16((c.V0 + 0xCu));
        c.V0 = 0x00000001u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        mem.WriteU32((c.SP + 0x58u), c.V0);
        c.V0 = mem.ReadU32((c.T7 + 0x20u));
        c.V1 = mem.ReadU32((c.V0 + 0x10u));
        mem.WriteU32((c.SP + 0x60u), c.V1);
        c.V0 = mem.ReadU32((c.T7 + 0x20u));
        c.V0 = mem.ReadU32((c.V0 + 0x8u));
        c.V0 = c.V0 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800A16A8;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.A1 = c.V0 - 0x1u;
        goto L800A16E8;
        L800A16A8: ;
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V0 = (uint)((int)c.V1 >> (int)(c.V0 & 31u));
        c.A1 = c.V0;
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = (int)c.V1 < 2 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800A219C;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Eu));
        c.V0 = (int)c.V1 < (int)c.V0 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800A219C;
        }
        L800A16E8: ;
        c.V0 = mem.ReadU32((c.SP + 0x60u));
        c.A0 = mem.ReadU16((c.SP + 0x54u));
        c.V1 = mem.ReadU32((c.SP + 0x58u));
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V1 = c.V1 >> 1;
        c.V0 = c.V0 + c.V1;
        mem.WriteU16((c.SP + 0x56u), (ushort)c.V0);
        c.V0 = c.V0 << 16;
        c.A2 = (uint)((int)c.V0 >> 16);
        c.V0 = 0u - c.A2;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU16((c.V1 + 0x8Cu));
        c.V0 = c.V0 << (int)(c.A0 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.V1 & 31u));
        c.V0 = (int)c.V0 < 3 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = 0x00000002u;
            goto L800A1744;
        }
        c.V0 = 0x00000002u;
        c.V0 = c.V0 << (int)(c.V1 & 31u);
        c.V0 = (uint)((int)c.V0 >> (int)(c.A0 & 31u));
        c.V0 = c.V0 + c.A2;
        mem.WriteU32((c.SP + 0x5Cu), c.V0);
        goto L800A1748;
        L800A1744: ;
        mem.WriteU32((c.SP + 0x5Cu), 0u);
        L800A1748: ;
        c.T1 = mem.ReadU32((c.SP + 0x108u));
        c.A0 = mem.ReadU32((c.T1 + 0x20u));
        c.V1 = mem.ReadU32((c.A0 + 0x8u));
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 != 0u) {
            c.A3 = 0x00FF0000u;
            goto L800A1808;
        }
        c.A3 = 0x00FF0000u;
        c.A3 = c.A3 | 0xFFFFu;
        c.V0 = c.V1 | 0x0001u;
        c.A1 = c.A1 << 16;
        c.A1 = (uint)((int)c.A1 >> 14);
        c.A2 = 0xFF000000u;
        mem.WriteU32((c.A0 + 0x8u), c.V0);
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU32((c.V0 + 0x9Cu));
        c.A0 = mem.ReadU32((c.SP + 0x50u));
        c.A1 = c.A1 + c.V0;
        c.V1 = mem.ReadU32(c.A0);
        c.V0 = mem.ReadU32(c.A1);
        c.V1 = c.V1 & c.A2;
        c.V0 = c.V0 & c.A3;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.A0, c.V1);
        c.A0 = mem.ReadU32(c.A1);
        c.V1 = mem.ReadU32((c.SP + 0x58u));
        c.V0 = mem.ReadU32((c.SP + 0x50u));
        c.A0 = c.A0 & c.A2;
        c.V1 = c.V1 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = c.V0 - 0x4u;
        c.V0 = c.V0 & c.A3;
        c.A0 = c.A0 | c.V0;
        mem.WriteU32(c.A1, c.A0);
        goto L800A1808;
        L800A17D8: ;
        c.V1 = 0x1F800000u;
        c.V1 = mem.ReadU32((c.V1 + 0x9Cu));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Cu));
        c.A1 = 0x1F800000u;
        c.A1 = mem.ReadU16((c.A1 + 0x8Eu));
        mem.WriteU32((c.SP + 0x5Cu), c.V0);
        mem.WriteU32((c.SP + 0x60u), 0u);
        mem.WriteU16((c.SP + 0x56u), (ushort)0u);
        mem.WriteU32((c.SP + 0x50u), c.V1);
        mem.WriteU16((c.SP + 0x54u), (ushort)c.A0);
        mem.WriteU32((c.SP + 0x58u), c.A1);
        L800A1808: ;
        c.V0 = mem.ReadU16((c.S2 + 0x6u));
        c.V1 = mem.ReadU32((c.S4 + 0x18u));
        c.T2 = mem.ReadU32((c.SP + 0x108u));
        c.T3 = mem.ReadU32((c.SP + 0xA0u));
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.V1;
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.SP + 0xA4u), 0u);
        c.V0 = c.V0 + 0x10u;
        mem.WriteU32((c.SP + 0xBCu), c.V0);
        c.V1 = mem.ReadU16((c.T2 + 0xEu));
        c.V0 = mem.ReadU16((c.T3 + 0x8u));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            c.S7 = c.V1 & 0x003Eu;
            goto L800A1864;
        }
        c.S7 = c.V1 & 0x003Eu;
        c.V0 = mem.ReadU32((c.S5 + 0x2Cu));
        if (c.V0 == 0u) {
            goto L800A1870;
        }
        c.V0 = mem.ReadU32(c.V0);
        mem.WriteU32((c.SP + 0xA4u), c.V0);
        goto L800A1870;
        L800A1864: ;
        c.S3 = mem.ReadU32((c.S3 + 0x14u));
        mem.WriteU32((c.SP + 0xA4u), c.S3);
        L800A1870: ;
        c.T7 = mem.ReadU32((c.SP + 0x108u));
        c.V0 = mem.ReadU16((c.T7 + 0x1Eu));
        c.V0 = c.V0 & 0x0003u;
        if (c.V0 != 0u) {
            c.V1 = 0x00030000u;
            goto L800A18D4;
        }
        c.V1 = 0x00030000u;
        c.T1 = mem.ReadU32((c.SP + 0x94u));
        c.V0 = mem.ReadU32((c.T1 + 0x8u));
        c.V0 = c.V0 & c.V1;
        if (c.V0 != c.V1) {
            goto L800A18D4;
        }
        c.T2 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = mem.ReadU16((c.T2 + 0x11Eu));
        if (c.V0 == 0u) {
            goto L800A18F4;
        }
        c.V0 = mem.ReadU16((c.T1 + 0xAu));
        c.V0 = c.V0 & 0x0004u;
        if (c.V0 == 0u) {
            goto L800A18F4;
        }
        L800A18D4: ;
        c.T3 = mem.ReadU32((c.SP + 0x108u));
        c.A0 = mem.ReadU32((c.SP + 0x94u));
        c.A2 = c.T3 + 0x10u;
        c.A1 = mem.ReadU16((c.T3 + 0x1Eu));
        c.A3 = c.T3 + 0x14u;
        c.RA = 0x800A18ECu;
        MediEvil_game.func_800ABA38(c, m);
        mem.WriteU32((c.SP + 0xD0u), c.V0);
        goto L800A1938;
        L800A18F4: ;
        c.T7 = mem.ReadU32((c.SP + 0x10Cu));
        c.V1 = mem.ReadU32((c.T7 + 0x12Cu));
        c.V0 = 0x1F800000u;
        mem.WriteU32((c.V0 + 0x3Cu), c.V1);
        c.V1 = mem.ReadU32((c.T7 + 0x130u));
        c.V0 = c.V0 + 0x3Cu;
        mem.WriteU32((c.V0 + 0x4u), c.V1);
        c.V1 = mem.ReadU32((c.T7 + 0x134u));
        mem.WriteU32((c.V0 + 0x8u), c.V1);
        c.V1 = mem.ReadU32((c.T7 + 0x138u));
        mem.WriteU32((c.V0 + 0xCu), c.V1);
        c.V1 = mem.ReadU16((c.T7 + 0x13Cu));
        mem.WriteU32((c.SP + 0xD0u), 0u);
        mem.WriteU16((c.V0 + 0x10u), (ushort)c.V1);
        L800A1938: ;
        c.T1 = mem.ReadU16((c.SP + 0xB0u));
        c.V0 = c.T1 << 16;
        if ((int)c.V0 <= 0) {
            mem.WriteU16((c.SP + 0xC8u), (ushort)0u);
            goto L800A2114;
        }
        mem.WriteU16((c.SP + 0xC8u), (ushort)0u);
        c.S6 = c.SP + 0x30u;
        L800A1950: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.T2 = mem.ReadU32((c.SP + 0xA4u));
        if (c.T2 == 0u) {
            goto L800A19AC;
        }
        c.T3 = mem.ReadU16((c.SP + 0xC8u));
        c.V0 = c.T3 << 16;
        c.V0 = (uint)((int)c.V0 >> 16);
        c.V0 = c.T2 + c.V0;
        c.V1 = mem.ReadU8(c.V0);
        c.V0 = c.V1 & 0x0008u;
        if (c.V0 != 0u) {
            goto L800A20C4;
        }
        c.T7 = mem.ReadU32((c.SP + 0xA0u));
        c.V0 = mem.ReadU16((c.T7 + 0xAu));
        c.V0 = c.V0 & 0x0001u;
        if (c.V0 == 0u) {
            c.V0 = c.V1 & 0x0001u;
            goto L800A19AC;
        }
        c.V0 = c.V1 & 0x0001u;
        if (c.V0 == 0u) {
            goto L800A20C4;
        }
        L800A19AC: ;
        c.T1 = mem.ReadU32((c.SP + 0xBCu));
        c.T2 = mem.ReadU32((c.SP + 0xA0u));
        c.S1 = mem.ReadU32((c.T1 + 0x10u));
        c.S0 = mem.ReadU16((c.T1 + 0x8u));
        c.V0 = mem.ReadU32((c.T1 + 0xCu));
        c.V1 = mem.ReadU16((c.T2 + 0xAu));
        c.S5 = mem.ReadU32(c.V0);
        c.T3 = mem.ReadU32((c.V0 + 0x8u));
        c.V1 = c.V1 & 0x0010u;
        mem.WriteU32((c.SP + 0xC0u), c.T3);
        c.S4 = mem.ReadU32((c.V0 + 0x4u));
        if (c.V1 == 0u) {
            goto L800A1A38;
        }
        c.T7 = mem.ReadU32((c.SP + 0x10Cu));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.A1 = mem.ReadU32((c.SP + 0xB8u));
        c.A0 = c.T7 + 0xA0u;
        c.RA = 0x800A19F8u;
        MediEvil_game.func_800A4EB8(c, m);
        c.T1 = mem.ReadU32((c.SP + 0xB8u));
        c.T2 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = mem.ReadU16((c.T1 + 0x14u));
        c.V1 = mem.ReadU16((c.T2 + 0xB4u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x78u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T1 + 0x18u));
        c.V1 = mem.ReadU16((c.T2 + 0xB8u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x7Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.T1 + 0x1Cu));
        c.V1 = mem.ReadU16((c.T2 + 0xBCu));
        c.S2 = c.T1;
        goto L800A1B44;
        L800A1A38: ;
        c.T3 = mem.ReadU32((c.SP + 0x94u));
        c.V0 = mem.ReadU16((c.T3 + 0xAu));
        c.V0 = c.V0 & 0x0080u;
        if (c.V0 == 0u) {
            goto L800A1A60;
        }
        c.S3 = mem.ReadU32((c.T3 + 0xCu));
        goto L800A1A74;
        L800A1A60: ;
        c.T7 = mem.ReadU32((c.SP + 0x94u));
        c.V0 = mem.ReadU32((c.T7 + 0xCu));
        c.S3 = c.V0 + 0x60u;
        L800A1A74: ;
        c.A0 = mem.ReadU32((c.SP + 0xA0u));
        c.T1 = mem.ReadU32((c.SP + 0x9Cu));
        c.T2 = mem.ReadU16((c.SP + 0xC8u));
        c.T3 = mem.ReadU16((c.SP + 0xA8u));
        c.A1 = c.SP + 0x20u;
        mem.WriteU16((c.SP + 0x24u), (ushort)0u);
        mem.WriteU32((c.SP + 0x20u), c.T1);
        mem.WriteU16((c.SP + 0x26u), (ushort)c.T2);
        mem.WriteU16((c.SP + 0x28u), (ushort)c.T3);
        c.RA = 0x800A1A9Cu;
        MediEvil_game.func_800A2E90(c, m);
        c.V1 = 0x800F0000u;
        c.S2 = c.V1 + RAddr.Game_800A1AA0;
        c.A0 = c.S3;
        c.A1 = c.V0;
        c.A2 = c.S2;
        c.RA = 0x800A1AB4u;
        MediEvil_game.func_800A4EB8(c, m);
        c.A0 = c.S3;
        c.A1 = 0x1F800000u;
        c.A1 = c.A1 + 0x80u;
        c.A2 = c.S2 + 0x14u;
        c.RA = 0x800A1AC8u;
        MediEvil_game.func_800A48FC(c, m);
        c.A1 = c.S2;
        c.V0 = mem.ReadU32((c.S2 + 0x14u));
        c.V1 = mem.ReadU32((c.S3 + 0x14u));
        c.T7 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = c.V0 + c.V1;
        mem.WriteU32((c.S2 + 0x14u), c.V0);
        c.V0 = mem.ReadU32((c.S2 + 0x18u));
        c.V1 = mem.ReadU32((c.S3 + 0x18u));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.V0 = c.V0 + c.V1;
        mem.WriteU32((c.S2 + 0x18u), c.V0);
        c.V0 = mem.ReadU32((c.S2 + 0x1Cu));
        c.V1 = mem.ReadU32((c.S3 + 0x1Cu));
        c.A0 = c.T7 + 0xA0u;
        c.V0 = c.V0 + c.V1;
        mem.WriteU32((c.S2 + 0x1Cu), c.V0);
        c.RA = 0x800A1B10u;
        MediEvil_game.func_800A4EB8(c, m);
        c.T1 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = mem.ReadU16((c.S2 + 0x14u));
        c.V1 = mem.ReadU16((c.T1 + 0xB4u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x78u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.S2 + 0x18u));
        c.V1 = mem.ReadU16((c.T1 + 0xB8u));
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x7Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.S2 + 0x1Cu));
        c.V1 = mem.ReadU16((c.T1 + 0xBCu));
        L800A1B44: ;
        c.V0 = c.V0 - c.V1;
        mem.WriteU16((c.SP + 0x7Cu), (ushort)c.V0);
        c.T2 = mem.ReadU32((c.SP + 0x10Cu));
        c.A2 = 0x1F800000u;
        c.A2 = mem.ReadU32((c.A2 + 0x34u));
        c.A1 = c.SP + 0x78u;
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x38u), c.S2);
        c.A0 = c.T2 + 0xA0u;
        c.A2 = c.A2 + 0x14u;
        c.RA = 0x800A1B74u;
        MediEvil_game.func_800A48FC(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = mem.ReadU32((c.T3 + 0x34u));
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T3 + 0x14u));
        c.T5 = mem.ReadU32((c.T3 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T3 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        c.T7 = mem.ReadU32((c.SP + 0x98u));
        c.V1 = mem.ReadU32(c.T7);
        c.V0 = c.V1 & 0x0008u;
        if (c.V0 != 0u) {
            c.A0 = 0x1F800000u;
            goto L800A1C3C;
        }
        c.A0 = 0x1F800000u;
        c.T1 = mem.ReadU32((c.SP + 0xC0u));
        if (c.T1 == 0u) {
            c.V0 = c.V1 & 0x0010u;
            goto L800A1C3C;
        }
        c.V0 = c.V1 & 0x0010u;
        if (c.V0 == 0u) {
            c.A0 = c.T1;
            goto L800A1C04;
        }
        c.A0 = c.T1;
        c.A1 = c.SP + 0x90u;
        c.RA = 0x800A1BFCu;
        MediEvil_game.func_8009A694(c, m);
        goto L800A1C10;
        L800A1C04: ;
        c.A0 = mem.ReadU32((c.SP + 0xC0u));
        c.A1 = c.SP + 0x90u;
        c.RA = 0x800A1C10u;
        MediEvil_game.func_8009A540(c, m);
        L800A1C10: ;
        if (c.V0 == 0u) {
            goto L800A20C4;
        }
        c.V0 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.V0 + 0x8Cu));
        c.V1 = mem.ReadU32((c.SP + 0x90u));
        c.A0 = 0x1F800000u;
        c.A0 = mem.ReadU16((c.A0 + 0x8Eu));
        c.V1 = c.V1 >> (int)(c.V0 & 31u);
        c.V1 = c.V1 < c.A0 ? 1u : 0u;
        if (c.V1 == 0u) {
            c.A0 = 0x1F800000u;
            goto L800A20C4;
        }
        c.A0 = 0x1F800000u;
        L800A1C3C: ;
        c.A0 = c.A0 + 0x3Cu;
        c.A1 = c.S2;
        c.T2 = mem.ReadU32((c.SP + 0x108u));
        c.A2 = 0x1F800000u;
        c.V0 = mem.ReadU16((c.T2 + 0xEu));
        c.A2 = c.A2 + 0x60u;
        c.V0 = c.V0 | 0x0001u;
        mem.WriteU16((c.T2 + 0xEu), (ushort)c.V0);
        c.RA = 0x800A1C60u;
        MediEvil_game.func_800A4EB8(c, m);
        c.T3 = 0x1F800000u;
        c.T3 = c.T3 + 0x60u;
        c.T4 = mem.ReadU32(c.T3);
        c.T5 = mem.ReadU32((c.T3 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(8, c.T4);
        RecompOne.Runtime.Gte.WriteControl(9, c.T5);
        c.T4 = mem.ReadU32((c.T3 + 0x8u));
        c.T5 = mem.ReadU32((c.T3 + 0xCu));
        c.T6 = mem.ReadU32((c.T3 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(10, c.T4);
        RecompOne.Runtime.Gte.WriteControl(11, c.T5);
        RecompOne.Runtime.Gte.WriteControl(12, c.T6);
        c.T7 = 0x1F800000u;
        c.T7 = mem.ReadU32((c.T7 + 0x34u));
        c.T4 = mem.ReadU32(c.T7);
        c.T5 = mem.ReadU32((c.T7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.T7 + 0x8u));
        c.T5 = mem.ReadU32((c.T7 + 0xCu));
        c.T6 = mem.ReadU32((c.T7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.T4 = mem.ReadU32((c.T7 + 0x14u));
        c.T5 = mem.ReadU32((c.T7 + 0x18u));
        RecompOne.Runtime.Gte.WriteControl(5, c.T4);
        c.T6 = mem.ReadU32((c.T7 + 0x1Cu));
        RecompOne.Runtime.Gte.WriteControl(6, c.T5);
        RecompOne.Runtime.Gte.WriteControl(7, c.T6);
        if (c.S0 == 0u) {
            goto L800A20C4;
        }
        L800A1CE4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.V0 = mem.ReadU16(c.S1);
        c.V0 = c.V0 << 16;
        c.V1 = (uint)((int)c.V0 >> 16);
        c.V0 = c.V1 < 0x00000010u ? 1u : 0u;
        if (c.V0 == 0u) {
            c.S1 = c.S1 + 0x4u;
            goto L800A20B8;
        }
        c.S1 = c.S1 + 0x4u;
        c.V0 = 0x800C0000u;
        c.V0 = c.V0 + RAddr.Game_800A1D04;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.V0;
        c.V0 = mem.ReadU32(c.V1);
        switch (c.V0)
        {
            case 0x800A1D20u: goto L800A1D20;
            case 0x800A1D7Cu: goto L800A1D7C;
            case 0x800A1DD8u: goto L800A1DD8;
            case 0x800A1E34u: goto L800A1E34;
            case 0x800A1E90u: goto L800A1E90;
            case 0x800A1EECu: goto L800A1EEC;
            case 0x800A1F48u: goto L800A1F48;
            case 0x800A1FA4u: goto L800A1FA4;
            case 0x800A20B8u: goto L800A20B8;
            case 0x800A2000u: goto L800A2000;
            case 0x800A205Cu: goto L800A205C;
            default: Dispatcher.Call(c, m, c.V0); return;
        }
        L800A1D20: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1D50;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1D48u;
        MediEvil_game.func_800B097C(c, m);
        goto L800A20B0;
        L800A1D50: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1D74u;
        MediEvil_game.func_800B7738(c, m);
        goto L800A20B0;
        L800A1D7C: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1DAC;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1DA4u;
        MediEvil_game.func_800B0B3C(c, m);
        goto L800A20B0;
        L800A1DAC: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1DD0u;
        MediEvil_game.func_800B7A80(c, m);
        goto L800A20B0;
        L800A1DD8: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1E08;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1E00u;
        MediEvil_game.func_800B00E4(c, m);
        goto L800A20B0;
        L800A1E08: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1E2Cu;
        MediEvil_game.func_800B5A44(c, m);
        goto L800A20B0;
        L800A1E34: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1E64;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1E5Cu;
        MediEvil_game.func_800B02BC(c, m);
        goto L800A20B0;
        L800A1E64: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1E88u;
        MediEvil_game.func_800B5E20(c, m);
        goto L800A20B0;
        L800A1E90: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1EC0;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1EB8u;
        MediEvil_game.func_800B0D34(c, m);
        goto L800A20B0;
        L800A1EC0: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1EE4u;
        MediEvil_game.func_800B7E38(c, m);
        goto L800A20B0;
        L800A1EEC: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1F1C;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1F14u;
        MediEvil_game.func_800B0F20(c, m);
        goto L800A20B0;
        L800A1F1C: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1F40u;
        MediEvil_game.func_800B8304(c, m);
        goto L800A20B0;
        L800A1F48: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1F78;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1F70u;
        MediEvil_game.func_800B04D4(c, m);
        goto L800A20B0;
        L800A1F78: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1F9Cu;
        MediEvil_game.func_800B62B4(c, m);
        goto L800A20B0;
        L800A1FA4: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A1FD4;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A1FCCu;
        MediEvil_game.func_800B06D8(c, m);
        goto L800A20B0;
        L800A1FD4: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A1FF8u;
        MediEvil_game.func_800B67A0(c, m);
        goto L800A20B0;
        L800A2000: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A2030;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A2028u;
        MediEvil_game.func_800B6EEC(c, m);
        goto L800A20B0;
        L800A2030: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A2054u;
        MediEvil_game.func_800B8A24(c, m);
        goto L800A20B0;
        L800A205C: ;
        if (c.S7 != 0u) {
            mem.WriteU32((c.SP + 0x74u), c.S0);
            goto L800A208C;
        }
        mem.WriteU32((c.SP + 0x74u), c.S0);
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        c.RA = 0x800A2084u;
        MediEvil_game.func_800B72A0(c, m);
        goto L800A20B0;
        L800A208C: ;
        c.A0 = c.S5;
        c.A1 = c.S4;
        c.A3 = 0x1F800000u;
        c.A3 = mem.ReadU32((c.A3 + 0x10u));
        c.A2 = c.S1;
        mem.WriteU32((c.SP + 0x10u), c.S6);
        mem.WriteU32((c.SP + 0x14u), c.FP);
        mem.WriteU32((c.SP + 0x18u), c.S7);
        c.RA = 0x800A20B0u;
        MediEvil_game.func_800B90E8(c, m);
        L800A20B0: ;
        c.S1 = mem.ReadU32((c.SP + 0x70u));
        c.S0 = mem.ReadU32((c.SP + 0x74u));
        L800A20B8: ;
        if (c.S0 != 0u) {
            goto L800A1CE4;
        }
        L800A20C4: ;
        c.T1 = mem.ReadU32((c.SP + 0xA0u));
        c.T2 = mem.ReadU32((c.SP + 0xBCu));
        c.V0 = mem.ReadU16((c.T1 + 0xAu));
        c.T2 = c.T2 + 0x2Cu;
        c.V0 = c.V0 & 0x0010u;
        if (c.V0 == 0u) {
            mem.WriteU32((c.SP + 0xBCu), c.T2);
            goto L800A20F0;
        }
        mem.WriteU32((c.SP + 0xBCu), c.T2);
        c.T3 = mem.ReadU32((c.SP + 0xB8u));
        c.T3 = c.T3 + 0x20u;
        mem.WriteU32((c.SP + 0xB8u), c.T3);
        L800A20F0: ;
        c.T7 = mem.ReadU16((c.SP + 0xC8u));
        c.T1 = mem.ReadU16((c.SP + 0xB0u));
        c.V0 = c.T7 + 0x1u;
        mem.WriteU16((c.SP + 0xC8u), (ushort)c.V0);
        c.V0 = c.V0 << 16;
        c.V1 = c.T1 << 16;
        c.V0 = (int)c.V0 < (int)c.V1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800A1950;
        }
        L800A2114: ;
        c.T2 = mem.ReadU32((c.SP + 0xD0u));
        c.V0 = c.T2 & 0x0001u;
        if (c.V0 == 0u) {
            goto L800A215C;
        }
        c.T3 = mem.ReadU32((c.SP + 0x10Cu));
        c.V0 = c.T3 + 0x14Cu;
        c.T4 = mem.ReadU32(c.V0);
        c.T5 = mem.ReadU32((c.V0 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(16, c.T4);
        RecompOne.Runtime.Gte.WriteControl(17, c.T5);
        c.T4 = mem.ReadU32((c.V0 + 0x8u));
        c.T5 = mem.ReadU32((c.V0 + 0xCu));
        c.T6 = mem.ReadU32((c.V0 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(18, c.T4);
        RecompOne.Runtime.Gte.WriteControl(19, c.T5);
        RecompOne.Runtime.Gte.WriteControl(20, c.T6);
        L800A215C: ;
        c.T7 = mem.ReadU32((c.SP + 0xD0u));
        c.V0 = c.T7 & 0x0002u;
        if (c.V0 == 0u) {
            goto L800A219C;
        }
        c.T1 = mem.ReadU32((c.SP + 0x10Cu));
        c.T2 = mem.ReadU8((c.T1 + 0x16Cu));
        c.T3 = mem.ReadU8((c.T1 + 0x16Du));
        c.T1 = mem.ReadU8((c.T1 + 0x16Eu));
        c.T4 = c.T2 << 4;
        c.T5 = c.T3 << 4;
        c.T6 = c.T1 << 4;
        RecompOne.Runtime.Gte.WriteControl(13, c.T4);
        RecompOne.Runtime.Gte.WriteControl(14, c.T5);
        RecompOne.Runtime.Gte.WriteControl(15, c.T6);
        L800A219C: ;
        c.RA = mem.ReadU32((c.SP + 0x104u));
        c.FP = mem.ReadU32((c.SP + 0x100u));
        c.S7 = mem.ReadU32((c.SP + 0xFCu));
        c.S6 = mem.ReadU32((c.SP + 0xF8u));
        c.S5 = mem.ReadU32((c.SP + 0xF4u));
        c.S4 = mem.ReadU32((c.SP + 0xF0u));
        c.S3 = mem.ReadU32((c.SP + 0xECu));
        c.S2 = mem.ReadU32((c.SP + 0xE8u));
        c.S1 = mem.ReadU32((c.SP + 0xE4u));
        c.S0 = mem.ReadU32((c.SP + 0xE0u));
        c.SP = c.SP + 0x108u;
        return;
    }
}
