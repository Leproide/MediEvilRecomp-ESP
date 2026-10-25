// PAL port: addresses/constants rewritten for the PAL release by
// medievil-pal-port (GPL-3.0-or-later), Author: https://github.com/Leproide
// Original code: MediEvilRecomp by BlackLabelHQ - their license terms apply to it.

using RecompOne.Runtime.Context;
using RecompOne.Runtime.Dispatch;
using RecompOne.Runtime.Memory;

namespace Recompiled;

//this is a mess
public static class EntityFloatFunctions
{
    public static void func_800B00E4(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x07000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        L800B016C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x18u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32((c.A2 + 0x14u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T8 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T8 <= 0) {
            goto L800B026C;
        }
        RecompOne.Runtime.Gte.Avsz3();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B026C;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B026C;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x18u), _sw); }
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        if (0u == c.V1) {
            goto L800B0224;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B0228;
        L800B0224: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B0228: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x4u), _sw); }
        c.T8 = 0u | 0x0007u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.T8 = mem.ReadU32((c.A2 + 0x8u));
        c.T9 = mem.ReadU32((c.A2 + 0xCu));
        c.T7 = mem.ReadU16((c.A2 + 0x10u));
        mem.WriteU32((c.A3 + 0xCu), c.T8);
        mem.WriteU32((c.A3 + 0x14u), c.T9);
        mem.WriteU16((c.A3 + 0x1Cu), (ushort)c.T7);
        c.A3 = c.A3 + 0x20u;
        L800B026C: ;
        c.A2 = c.A2 + 0x18u;
        c.S5 = c.S5 - 0x1u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B016C;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B02BC(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x09000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        c.T3 = c.T3 + c.A0;
        L800B0350: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x1Cu;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T3 = (uint)(short)mem.ReadU16((c.T8 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T3 = c.T3 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        { var _lw = mem.ReadU32((c.A2 + 0x18u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T2 = c.T2 << 3;
        if ((int)c.T9 > 0) {
            c.T2 = c.T2 + c.A0;
            goto L800B03DC;
        }
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T9 >= 0) {
            goto L800B0484;
        }
        L800B03DC: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B0484;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B0484;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x18u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x20u), _sw); }
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        if (0u == c.V1) {
            goto L800B0434;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B0438;
        L800B0434: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B0438: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x4u), _sw); }
        c.T8 = 0u | 0x0009u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.T8 = mem.ReadU32((c.A2 + 0xCu));
        c.T9 = mem.ReadU32((c.A2 + 0x10u));
        c.T4 = mem.ReadU16((c.A2 + 0x14u));
        c.T5 = mem.ReadU16((c.A2 + 0x16u));
        mem.WriteU32((c.A3 + 0xCu), c.T8);
        mem.WriteU32((c.A3 + 0x14u), c.T9);
        mem.WriteU16((c.A3 + 0x24u), (ushort)c.T4);
        mem.WriteU16((c.A3 + 0x1Cu), (ushort)c.T5);
        c.A3 = c.A3 + 0x28u;
        L800B0484: ;
        c.S5 = c.S5 - 0x1u;
        c.A2 = c.A2 + 0x1Cu;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B0350;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B04D4(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x09000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        L800B055C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x1Cu;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32((c.A2 + 0x18u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T8 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T8 <= 0) {
            goto L800B0688;
        }
        RecompOne.Runtime.Gte.Avsz3();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B0688;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
            goto L800B0688;
        }
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x20u), _sw); }
        c.T5 = c.T5 << 3;
        c.T6 = c.T6 << 3;
        c.T5 = c.T5 + c.A1;
        c.T6 = c.T6 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        if (0u == c.V1) {
            goto L800B0638;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B063C;
        L800B0638: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B063C: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        c.T8 = 0u | 0x0009u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.T8 = mem.ReadU32((c.A2 + 0xCu));
        c.T9 = mem.ReadU32((c.A2 + 0x10u));
        c.T7 = mem.ReadU16((c.A2 + 0x14u));
        mem.WriteU32((c.A3 + 0xCu), c.T8);
        mem.WriteU32((c.A3 + 0x18u), c.T9);
        mem.WriteU16((c.A3 + 0x24u), (ushort)c.T7);
        c.A3 = c.A3 + 0x28u;
        L800B0688: ;
        c.A2 = c.A2 + 0x1Cu;
        c.S5 = c.S5 - 0x1u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B055C;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B06D8(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x0C000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        c.T3 = c.T3 + c.A0;
        L800B076C: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x24u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T3 = (uint)(short)mem.ReadU16((c.T8 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T3 = c.T3 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        { var _lw = mem.ReadU32((c.A2 + 0x20u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T2 = c.T2 << 3;
        if ((int)c.T9 > 0) {
            c.T2 = c.T2 + c.A0;
            goto L800B07F8;
        }
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T9 >= 0) {
            goto L800B092C;
        }
        L800B07F8: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B092C;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
            goto L800B092C;
        }
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        c.T7 = (uint)(short)mem.ReadU16((c.A2 + 0xEu));
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x20u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x2Cu), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x2Cu), _sw); }
        c.T5 = c.T5 << 3;
        c.T7 = c.T7 << 3;
        c.T5 = c.T5 + c.A1;
        c.T7 = c.T7 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        if (0u == c.V1) {
            goto L800B08B4;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B08FC;
        L800B08B4: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B08FC: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x28u), _sw); }
        c.T8 = 0u | 0x000Cu;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.T8 = mem.ReadU32((c.A2 + 0x10u));
        c.T9 = mem.ReadU32((c.A2 + 0x14u));
        c.T4 = mem.ReadU16((c.A2 + 0x18u));
        c.T5 = mem.ReadU16((c.A2 + 0x1Au));
        mem.WriteU32((c.A3 + 0xCu), c.T8);
        mem.WriteU32((c.A3 + 0x18u), c.T9);
        mem.WriteU16((c.A3 + 0x30u), (ushort)c.T4);
        mem.WriteU16((c.A3 + 0x24u), (ushort)c.T5);
        c.A3 = c.A3 + 0x34u;
        L800B092C: ;
        c.S5 = c.S5 - 0x1u;
        c.A2 = c.A2 + 0x24u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B076C;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B097C(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x04000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        L800B0A04: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0xCu;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32((c.A2 + 0x8u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T8 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T8 <= 0) {
            goto L800B0AEC;
        }
        RecompOne.Runtime.Gte.Avsz3();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B0AEC;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B0AEC;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0xCu), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x10u), _sw); }
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        if (0u == c.V1) {
            goto L800B0ABC;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B0AC0;
        L800B0ABC: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B0AC0: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x4u), _sw); }
        c.T8 = 0u | 0x0004u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.A3 = c.A3 + 0x14u;
        L800B0AEC: ;
        c.A2 = c.A2 + 0xCu;
        c.S5 = c.S5 - 0x1u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B0A04;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B0B3C(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x05000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        c.T3 = c.T3 + c.A0;
        L800B0BD0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x10u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T3 = (uint)(short)mem.ReadU16((c.T8 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T3 = c.T3 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        { var _lw = mem.ReadU32((c.A2 + 0xCu)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T2 = c.T2 << 3;
        if ((int)c.T9 > 0) {
            c.T2 = c.T2 + c.A0;
            goto L800B0C5C;
        }
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T9 >= 0) {
            goto L800B0CE4;
        }
        L800B0C5C: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B0CE4;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B0CE4;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0xCu), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x14u), _sw); }
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        if (0u == c.V1) {
            goto L800B0CB4;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B0CB8;
        L800B0CB4: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B0CB8: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x4u), _sw); }
        c.T8 = 0u | 0x0005u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.A3 = c.A3 + 0x18u;
        L800B0CE4: ;
        c.S5 = c.S5 - 0x1u;
        c.A2 = c.A2 + 0x10u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B0BD0;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B0D34(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x06000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        L800B0DBC: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x10u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32((c.A2 + 0xCu)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T8 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T8 <= 0) {
            goto L800B0ED0;
        }
        RecompOne.Runtime.Gte.Avsz3();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B0ED0;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
            goto L800B0ED0;
        }
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x18u), _sw); }
        c.T5 = c.T5 << 3;
        c.T6 = c.T6 << 3;
        c.T5 = c.T5 + c.A1;
        c.T6 = c.T6 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        if (0u == c.V1) {
            goto L800B0E98;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B0E9C;
        L800B0E98: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B0E9C: ;
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x14u), _sw); }
        c.T8 = 0u | 0x0006u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.A3 = c.A3 + 0x1Cu;
        L800B0ED0: ;
        c.A2 = c.A2 + 0x10u;
        c.S5 = c.S5 - 0x1u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B0DBC;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B0F20(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x34u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x08000000u;
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        c.T3 = c.T3 + c.A0;
        L800B0FB4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x14u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T3 = (uint)(short)mem.ReadU16((c.T8 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T3 = c.T3 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        { var _lw = mem.ReadU32((c.A2 + 0x10u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T2 = c.T2 << 3;
        if ((int)c.T9 > 0) {
            c.T2 = c.T2 + c.A0;
            goto L800B1040;
        }
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T9 >= 0) {
            goto L800B1154;
        }
        L800B1040: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B1154;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
            goto L800B1154;
        }
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        c.T7 = (uint)(short)mem.ReadU16((c.A2 + 0xEu));
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x18u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x20u), _sw); }
        c.T5 = c.T5 << 3;
        c.T7 = c.T7 << 3;
        c.T5 = c.T5 + c.A1;
        c.T7 = c.T7 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        if (0u == c.V1) {
            goto L800B10FC;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x14u), _sw); }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B1144;
        L800B10FC: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x14u), _sw); }
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B1144: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        c.T8 = 0u | 0x0008u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.A3 = c.A3 + 0x24u;
        L800B1154: ;
        c.S5 = c.S5 - 0x1u;
        c.A2 = c.A2 + 0x14u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B0FB4;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x34u;
        return;
    }
    public static void func_800B5A44(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x30u;
        c.T7 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T2 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x40u));
        c.T5 = 0x1F800000u;
        c.T5 = mem.ReadU16((c.T5 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        c.V1 = c.T2;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.T8 = mem.ReadU32((c.T0 + 0x20u));
        c.S2 = mem.ReadU16((c.T0 + 0x24u));
        c.S1 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S0 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.S3 = mem.ReadU32((c.SP + 0x44u));
        c.T6 = mem.ReadU32((c.SP + 0x48u));
        if (c.V1 == 0u) {
            c.T2 = c.T2 - 0x1u;
            goto L800B5DF0;
        }
        c.T2 = c.T2 - 0x1u;
        c.T4 = 0x00FF0000u;
        c.T4 = c.T4 | 0xFFFFu;
        c.T9 = 0xFF000000u;
        c.A2 = c.A2 + 0x10u;
        c.A3 = c.A3 + 0x1Cu;
        L800B5AE8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T0);
        c.S5 = mem.ReadU32((c.T0 + 0x4u));
        c.S6 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T7 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T6 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B5B88;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B5B88: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = c.T0 + 0x34u;
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.V0, _sw); }
        c.V0 = c.T6 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B5BB0;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 <= 0) {
            goto L800B5DD0;
        }
        L800B5BB0: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S2 & 31u));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < (int)c.S0 ? 1u : 0u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.T0 + 0x38u), c.V1);
            goto L800B5DD0;
        }
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.V0 = (int)c.V1 < (int)c.S1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B5DD0;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.T1 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.T1 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.T1 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.T1 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.T1 + 0x18u), _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x12u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
            goto L800B5C34;
        }
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xAu));
        if ((int)c.V0 >= 0) {
            goto L800B5C28;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        if ((int)c.V0 < 0) {
            goto L800B5DD0;
        }
        L800B5C28: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x12u));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        L800B5C34: ;
        if (c.V0 != 0u) {
            c.V0 = c.T6 & 0x0020u;
            goto L800B5C64;
        }
        c.V0 = c.T6 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xAu));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.T6 & 0x0020u;
            goto L800B5C64;
        }
        c.V0 = c.T6 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.T6 & 0x0020u;
            goto L800B5DD0;
        }
        c.V0 = c.T6 & 0x0020u;
        L800B5C64: ;
        if (c.V0 != 0u) {
            c.V1 = c.T6 & 0x0006u;
            goto L800B5D4C;
        }
        c.V1 = c.T6 & 0x0006u;
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L800B5CFC;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            goto L800B5CFC;
        }
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = c.SP + 0x8u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B5D14;
        L800B5CFC: ;
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B5D14: ;
        if (c.S3 == 0u) {
            goto L800B5D30;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        goto L800B5D40;
        L800B5D30: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        L800B5D40: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        goto L800B5D58;
        L800B5D4C: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x18u), c.V0);
        L800B5D58: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T8;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T9;
        c.V0 = c.V0 & c.T4;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T8;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.T4;
        c.V0 = c.V0 & c.T9;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x00000007u;
        mem.WriteU8((c.A3 - 0x19u), (byte)c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x8u));
        mem.WriteU32((c.A3 - 0x10u), c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x4u));
        mem.WriteU32((c.A3 - 0x8u), c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.T1 = c.T1 + 0x20u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x20u;
        L800B5DD0: ;
        c.A2 = c.A2 + 0x18u;
        c.T3 = c.T3 + 0x18u;
        c.V1 = c.T2;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T2 = c.T2 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B5AE8;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B5DF0: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x30u;
        return;
    }
    public static void func_800B5E20(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x38u;
        c.T6 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x34u), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T5 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x48u));
        c.T4 = 0x1F800000u;
        c.T4 = mem.ReadU16((c.T4 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        c.V1 = c.T5;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.S0 = mem.ReadU32((c.T0 + 0x20u));
        c.S4 = mem.ReadU16((c.T0 + 0x24u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S3 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x6u));
        c.S2 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        c.S5 = mem.ReadU32((c.SP + 0x4Cu));
        c.T8 = mem.ReadU32((c.SP + 0x50u));
        c.T5 = c.T5 - 0x1u;
        if (c.V1 == 0u) {
            mem.WriteU32((c.SP + 0x10u), c.S2);
            goto L800B6280;
        }
        mem.WriteU32((c.SP + 0x10u), c.S2);
        c.T9 = c.T0 + 0x34u;
        c.T7 = 0x00FF0000u;
        c.T7 = c.T7 | 0xFFFFu;
        c.S1 = 0xFF000000u;
        c.A2 = c.A2 + 0x16u;
        c.A3 = c.A3 + 0x1Cu;
        L800B5EE0: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S6 = mem.ReadU32(c.T0);
        c.S7 = mem.ReadU32((c.T0 + 0x4u));
        c.S2 = mem.ReadU32((c.T0 + 0xCu));
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.T2 = 0u;
        c.V0 = c.T1 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(12, c.V0, _sw); }
        c.S6 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.T9, _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.V0 = mem.ReadU32((c.A2 + 0x2u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T8 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B5FC0;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B5FC0: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            goto L800B6008;
        }
        RecompOne.Runtime.Gte.Nclip();
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.T9, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 < 0) {
            c.V0 = c.T8 & 0x0002u;
            goto L800B5FF8;
        }
        c.V0 = c.T8 & 0x0002u;
        c.T2 = 0x00000001u;
        L800B5FF8: ;
        if (c.V0 != 0u) {
            goto L800B6008;
        }
        if (c.T2 != 0u) {
            goto L800B6260;
        }
        L800B6008: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S4 & 31u));
        c.V1 = c.V0 + c.V1;
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.S7 = mem.ReadU32((c.SP + 0x10u));
        c.V0 = (int)c.V1 < (int)c.S7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = (int)c.V1 < (int)c.S3 ? 1u : 0u;
            goto L800B6260;
        }
        c.V0 = (int)c.V1 < (int)c.S3 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.T1 + 0x10u;
            goto L800B6260;
        }
        c.A0 = c.T1 + 0x10u;
        c.V1 = c.T1 + 0x18u;
        c.V0 = c.T1 + 0x20u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.A0, _sw); Recompiled.EntityFloatProjection.Store(12, c.A0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); Recompiled.EntityFloatProjection.Store(13, c.V1, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(14, c.V0, _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x12u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
            goto L800B60AC;
        }
        c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xAu));
        if ((int)c.V0 >= 0) {
            goto L800B60A0;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        if ((int)c.V0 >= 0) {
            goto L800B60A0;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x6u));
        if ((int)c.V0 < 0) {
            goto L800B6260;
        }
        L800B60A0: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x12u));
        c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
        L800B60AC: ;
        if (c.V0 != 0u) {
            c.V0 = c.T8 & 0x0020u;
            goto L800B60F0;
        }
        c.V0 = c.T8 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xAu));
        c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.T8 & 0x0020u;
            goto L800B60F0;
        }
        c.V0 = c.T8 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.T8 & 0x0020u;
            goto L800B60F0;
        }
        c.V0 = c.T8 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x6u));
        c.V0 = (int)c.V0 < (int)c.T4 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.T8 & 0x0020u;
            goto L800B6260;
        }
        c.V0 = c.T8 & 0x0020u;
        L800B60F0: ;
        if (c.V0 != 0u) {
            c.V1 = c.T8 & 0x0006u;
            goto L800B61D0;
        }
        c.V1 = c.T8 & 0x0006u;
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L800B6180;
        }
        if (c.T2 == 0u) {
            goto L800B6180;
        }
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = c.SP + 0x8u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B6198;
        L800B6180: ;
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B6198: ;
        if (c.S5 == 0u) {
            goto L800B61B4;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        goto L800B61C4;
        L800B61B4: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        L800B61C4: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        goto L800B61DC;
        L800B61D0: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x18u), c.V0);
        L800B61DC: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.S0;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S1;
        c.V0 = c.V0 & c.T7;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.S0;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.T7;
        c.V0 = c.V0 & c.S1;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x00000009u;
        mem.WriteU8((c.A3 - 0x19u), (byte)c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0xAu));
        mem.WriteU32((c.A3 - 0x10u), c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x6u));
        mem.WriteU32((c.A3 - 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        mem.WriteU16((c.A3 + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.T1 = c.T1 + 0x28u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x28u;
        L800B6260: ;
        c.A2 = c.A2 + 0x1Cu;
        c.T3 = c.T3 + 0x1Cu;
        c.V1 = c.T5;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T5 = c.T5 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B5EE0;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B6280: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S7 = mem.ReadU32((c.SP + 0x34u));
        c.S6 = mem.ReadU32((c.SP + 0x30u));
        c.S5 = mem.ReadU32((c.SP + 0x2Cu));
        c.S4 = mem.ReadU32((c.SP + 0x28u));
        c.S3 = mem.ReadU32((c.SP + 0x24u));
        c.S2 = mem.ReadU32((c.SP + 0x20u));
        c.S1 = mem.ReadU32((c.SP + 0x1Cu));
        c.S0 = mem.ReadU32((c.SP + 0x18u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x38u;
        return;
    }
    public static void func_800B62B4(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x40u;
        c.T6 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x38u), c.S6);
        mem.WriteU32((c.SP + 0x34u), c.S5);
        mem.WriteU32((c.SP + 0x30u), c.S4);
        mem.WriteU32((c.SP + 0x2Cu), c.S3);
        mem.WriteU32((c.SP + 0x28u), c.S2);
        mem.WriteU32((c.SP + 0x24u), c.S1);
        mem.WriteU32((c.SP + 0x20u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T2 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x50u));
        c.T5 = 0x1F800000u;
        c.T5 = mem.ReadU16((c.T5 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        c.V1 = c.T2;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.T8 = mem.ReadU32((c.T0 + 0x20u));
        c.S2 = mem.ReadU16((c.T0 + 0x24u));
        c.S1 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S0 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.S3 = mem.ReadU32((c.SP + 0x54u));
        c.T7 = mem.ReadU32((c.SP + 0x58u));
        if (c.V1 == 0u) {
            c.T2 = c.T2 - 0x1u;
            goto L800B6770;
        }
        c.T2 = c.T2 - 0x1u;
        c.T4 = 0x00FF0000u;
        c.T4 = c.T4 | 0xFFFFu;
        c.T9 = 0xFF000000u;
        c.A2 = c.A2 + 0x14u;
        c.A3 = c.A3 + 0x24u;
        L800B6358: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T0);
        c.S5 = mem.ReadU32((c.T0 + 0x4u));
        c.S6 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T7 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B63F8;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B63F8: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = c.T0 + 0x34u;
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.V0, _sw); }
        c.V0 = c.T7 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B6420;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 <= 0) {
            goto L800B6750;
        }
        L800B6420: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S2 & 31u));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < (int)c.S0 ? 1u : 0u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.T0 + 0x38u), c.V1);
            goto L800B6750;
        }
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.V0 = (int)c.V1 < (int)c.S1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B6750;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.T1 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.T1 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.T1 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.T1 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.T1 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.T1 + 0x20u), _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x1Au));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
            goto L800B64A4;
        }
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xEu));
        if ((int)c.V0 >= 0) {
            goto L800B6498;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        if ((int)c.V0 < 0) {
            goto L800B6750;
        }
        L800B6498: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x1Au));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        L800B64A4: ;
        if (c.V0 != 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B64D4;
        }
        c.V0 = c.T7 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xEu));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B64D4;
        }
        c.V0 = c.T7 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B6750;
        }
        c.V0 = c.T7 & 0x0020u;
        L800B64D4: ;
        if (c.V0 != 0u) {
            c.V1 = c.T7 & 0x0006u;
            goto L800B66B4;
        }
        c.V1 = c.T7 & 0x0006u;
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L800B663C;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            c.A0 = c.SP + 0x8u;
            goto L800B663C;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B6678;
        L800B663C: ;
        c.A0 = mem.ReadU16((c.A2 - 0xEu));
        c.V1 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.A0 = c.A0 << 3;
        c.A0 = c.A1 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.A1 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B6678: ;
        if (c.S3 == 0u) {
            goto L800B6694;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B66A0;
        L800B6694: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B66A0: ;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T1 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T1 + 0x1Cu), _sw); }
        goto L800B66D8;
        L800B66B4: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x20u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x14u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x8u), c.V0);
        L800B66D8: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T8;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T9;
        c.V0 = c.V0 & c.T4;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T8;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.T4;
        c.V0 = c.V0 & c.T9;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x00000009u;
        mem.WriteU8((c.A3 - 0x21u), (byte)c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x8u));
        mem.WriteU32((c.A3 - 0x18u), c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x4u));
        mem.WriteU32((c.A3 - 0xCu), c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.T1 = c.T1 + 0x28u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x28u;
        L800B6750: ;
        c.A2 = c.A2 + 0x1Cu;
        c.T3 = c.T3 + 0x1Cu;
        c.V1 = c.T2;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T2 = c.T2 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B6358;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B6770: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S6 = mem.ReadU32((c.SP + 0x38u));
        c.S5 = mem.ReadU32((c.SP + 0x34u));
        c.S4 = mem.ReadU32((c.SP + 0x30u));
        c.S3 = mem.ReadU32((c.SP + 0x2Cu));
        c.S2 = mem.ReadU32((c.SP + 0x28u));
        c.S1 = mem.ReadU32((c.SP + 0x24u));
        c.S0 = mem.ReadU32((c.SP + 0x20u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x40u;
        return;
    }
    public static void func_800B67A0(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x58u;
        c.T9 = c.A0;
        c.T5 = c.A2;
        mem.WriteU32((c.SP + 0x54u), c.S7);
        mem.WriteU32((c.SP + 0x50u), c.S6);
        mem.WriteU32((c.SP + 0x4Cu), c.S5);
        mem.WriteU32((c.SP + 0x48u), c.S4);
        mem.WriteU32((c.SP + 0x44u), c.S3);
        mem.WriteU32((c.SP + 0x40u), c.S2);
        mem.WriteU32((c.SP + 0x3Cu), c.S1);
        mem.WriteU32((c.SP + 0x38u), c.S0);
        c.V0 = mem.ReadU16(c.T5);
        c.T6 = mem.ReadU16((c.T5 - 0x2u));
        c.T1 = mem.ReadU32((c.SP + 0x68u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32(c.T1, c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x2u));
        c.T7 = 0x1F800000u;
        c.T7 = mem.ReadU16((c.T7 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x4u));
        c.T4 = mem.ReadU32((c.T1 + 0x20u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x6u));
        c.S4 = mem.ReadU16((c.T1 + 0x24u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0xCu), c.V0);
        c.S7 = mem.ReadU32((c.SP + 0x6Cu));
        c.S1 = mem.ReadU32((c.SP + 0x70u));
        c.T0 = c.A3;
        mem.WriteU32((c.SP + 0x28u), c.S4);
        c.S5 = mem.ReadU32((c.T1 + 0x28u));
        c.V1 = c.T6;
        mem.WriteU32((c.SP + 0x2Cu), c.S5);
        c.S6 = mem.ReadU32((c.T1 + 0x2Cu));
        c.T6 = c.T6 - 0x1u;
        if (c.V1 == 0u) {
            mem.WriteU32((c.SP + 0x30u), c.S6);
            goto L800B6EB8;
        }
        mem.WriteU32((c.SP + 0x30u), c.S6);
        c.S2 = c.T1 + 0x34u;
        c.S0 = c.S1 & 0x0006u;
        c.S3 = 0x00000006u;
        c.T2 = 0x00FF0000u;
        c.T2 = c.T2 | 0xFFFFu;
        c.T8 = 0xFF000000u;
        c.A2 = c.A2 + 0x1Au;
        c.A3 = c.A3 + 0x24u;
        L800B6870: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T1);
        c.S5 = mem.ReadU32((c.T1 + 0x4u));
        c.S6 = mem.ReadU32((c.T1 + 0xCu));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32(c.T1, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x10u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0xCu), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.T3 = 0u;
        c.S4 = mem.ReadU32((c.T1 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.S2, _sw); }
        c.V0 = c.T0 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(12, c.V0, _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        c.V0 = mem.ReadU32((c.A2 + 0x6u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.S1 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B693C;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B693C: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = mem.ReadU16((c.A2 + 0xEu));
        c.V1 = mem.ReadU32((c.T1 + 0x34u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        if ((int)c.V1 > 0) {
            mem.WriteU32((c.T1 + 0x8u), c.V0);
            goto L800B698C;
        }
        mem.WriteU32((c.T1 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.S2, _sw); }
        c.V0 = mem.ReadU32((c.T1 + 0x34u));
        if ((int)c.V0 < 0) {
            c.V0 = c.S1 & 0x0002u;
            goto L800B697C;
        }
        c.V0 = c.S1 & 0x0002u;
        c.T3 = 0x00000001u;
        L800B697C: ;
        if (c.V0 != 0u) {
            goto L800B698C;
        }
        if (c.T3 != 0u) {
            goto L800B6E98;
        }
        L800B698C: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.V0 = c.T1 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.S5 = mem.ReadU32((c.SP + 0x28u));
        c.V1 = (uint)(short)mem.ReadU16((c.T1 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S5 & 31u));
        c.V1 = c.V0 + c.V1;
        mem.WriteU32((c.T1 + 0x38u), c.V1);
        c.S6 = mem.ReadU32((c.SP + 0x30u));
        c.V0 = (int)c.V1 < (int)c.S6 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B6E98;
        }
        c.S4 = mem.ReadU32((c.SP + 0x2Cu));
        c.V0 = (int)c.V1 < (int)c.S4 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.T0 + 0x14u;
            goto L800B6E98;
        }
        c.A0 = c.T0 + 0x14u;
        c.V1 = c.T0 + 0x20u;
        c.V0 = c.T0 + 0x2Cu;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.A0, _sw); Recompiled.EntityFloatProjection.Store(12, c.A0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); Recompiled.EntityFloatProjection.Store(13, c.V1, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(14, c.V0, _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x1Au));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
            goto L800B6A40;
        }
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xEu));
        if ((int)c.V0 >= 0) {
            goto L800B6A34;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        if ((int)c.V0 >= 0) {
            goto L800B6A34;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xAu));
        if ((int)c.V0 < 0) {
            goto L800B6E98;
        }
        L800B6A34: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x1Au));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        L800B6A40: ;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B6A84;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0xEu));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B6A84;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x2u));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B6A84;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xAu));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B6E98;
        }
        c.V0 = c.S1 & 0x0020u;
        L800B6A84: ;
        if (c.V0 != 0u) {
            goto L800B6DE4;
        }
        if (c.S0 != c.S3) {
            goto L800B6C40;
        }
        if (c.T3 == 0u) {
            c.A0 = c.SP + 0x8u;
            goto L800B6C40;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0x12u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x12u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x12u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x10u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x10u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x10u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x20u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x22u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x24u), (ushort)c.V0);
        c.V0 = c.SP + 0x20u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B6C7C;
        L800B6C40: ;
        c.A0 = mem.ReadU16((c.A2 - 0x12u));
        c.V1 = mem.ReadU16((c.A2 - 0x10u));
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.A0 = c.A0 << 3;
        c.A0 = c.A1 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.A1 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B6C7C: ;
        if (c.S7 == 0u) {
            goto L800B6D30;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T0 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T0 + 0x1Cu), _sw); }
        if (c.S0 != c.S3) {
            goto L800B6D04;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B6D04;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B6D1C;
        L800B6D04: ;
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B6D1C: ;
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T0 + 0x28u;
        goto L800B6DD8;
        L800B6D30: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T0 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T0 + 0x1Cu), _sw); }
        if (c.S0 != c.S3) {
            goto L800B6DB0;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B6DB0;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B6DC8;
        L800B6DB0: ;
        c.V0 = mem.ReadU16((c.A2 - 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B6DC8: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T0 + 0x28u;
        L800B6DD8: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        c.V0 = 0x0000000Cu;
        goto L800B6E60;
        L800B6DE4: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x20u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x14u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x8u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x4u), c.V0);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x0000000Cu;
        L800B6E60: ;
        mem.WriteU8((c.A3 - 0x21u), (byte)c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0xAu));
        mem.WriteU32((c.A3 - 0x18u), c.V0);
        c.V0 = mem.ReadU32((c.A2 - 0x6u));
        mem.WriteU32((c.A3 - 0xCu), c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        mem.WriteU16((c.A3 + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.T0 = c.T0 + 0x34u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x34u;
        L800B6E98: ;
        c.A2 = c.A2 + 0x24u;
        c.T5 = c.T5 + 0x24u;
        c.V1 = c.T6;
        c.V0 = mem.ReadU32((c.T1 + 0x44u));
        c.T6 = c.T6 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T1 + 0x44u), c.V0);
            goto L800B6870;
        }
        mem.WriteU32((c.T1 + 0x44u), c.V0);
        L800B6EB8: ;
        mem.WriteU32((c.T1 + 0x40u), c.T5);
        c.S7 = mem.ReadU32((c.SP + 0x54u));
        c.S6 = mem.ReadU32((c.SP + 0x50u));
        c.S5 = mem.ReadU32((c.SP + 0x4Cu));
        c.S4 = mem.ReadU32((c.SP + 0x48u));
        c.S3 = mem.ReadU32((c.SP + 0x44u));
        c.S2 = mem.ReadU32((c.SP + 0x40u));
        c.S1 = mem.ReadU32((c.SP + 0x3Cu));
        c.S0 = mem.ReadU32((c.SP + 0x38u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T0);
        c.SP = c.SP + 0x58u;
        return;
    }
    public static void func_800B6EEC(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x44u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x09000000u;
        mem.WriteU32((c.SP + 0x34u), c.S6);
        mem.WriteU32((c.SP + 0x38u), c.S7);
        c.T9 = 0x800E0000u;
        c.At = 0x00010000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At - RAddr.Game_800B6F60));
        c.S6 = mem.ReadU8((c.T5 + 0x4u));
        c.S7 = mem.ReadU8((c.T5 + 0x5u));
        c.S6 = c.S6 + 0x40u;
        c.S7 = c.S7 + 0x40u;
        mem.WriteU32((c.SP + 0x3Cu), c.S6);
        mem.WriteU32((c.SP + 0x40u), c.S7);
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        L800B6FA8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x18u;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32((c.A2 + 0x14u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T8 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T8 <= 0) {
            goto L800B7250;
        }
        RecompOne.Runtime.Gte.Avsz3();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B7250;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B7250;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x20u), _sw); }
        c.T9 = 0x1F800000u;
        c.At = 0x00000000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At + 0x38u));
        c.T6 = mem.ReadU32(c.T5);
        c.T7 = mem.ReadU32((c.T5 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T6);
        RecompOne.Runtime.Gte.WriteControl(1, c.T7);
        c.T6 = mem.ReadU32((c.T5 + 0x8u));
        c.T7 = mem.ReadU32((c.T5 + 0xCu));
        c.T9 = mem.ReadU32((c.T5 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T6);
        RecompOne.Runtime.Gte.WriteControl(3, c.T7);
        RecompOne.Runtime.Gte.WriteControl(4, c.T9);
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.S6 = mem.ReadU32((c.SP + 0x3Cu));
        c.S7 = mem.ReadU32((c.SP + 0x40u));
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0xCu), (ushort)c.T5);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0x18u), (ushort)c.T5);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 2, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0x24u), (ushort)c.T5);
        c.T9 = 0x1F800000u;
        c.At = 0x00000000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At + 0x34u));
        c.T6 = mem.ReadU32(c.T5);
        c.T7 = mem.ReadU32((c.T5 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T6);
        RecompOne.Runtime.Gte.WriteControl(1, c.T7);
        c.T6 = mem.ReadU32((c.T5 + 0x8u));
        c.T7 = mem.ReadU32((c.T5 + 0xCu));
        c.T9 = mem.ReadU32((c.T5 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T6);
        RecompOne.Runtime.Gte.WriteControl(3, c.T7);
        RecompOne.Runtime.Gte.WriteControl(4, c.T9);
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xEu));
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0x10u));
        c.T5 = c.T5 << 3;
        c.T6 = c.T6 << 3;
        c.T5 = c.T5 + c.A1;
        c.T6 = c.T6 + c.A1;
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        if (0u == c.V1) {
            goto L800B71EC;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B71F0;
        L800B71EC: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B71F0: ;
        c.S6 = mem.ReadU32((c.SP + 0x34u));
        c.S7 = mem.ReadU32((c.SP + 0x38u));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        c.T9 = 0x800E0000u;
        c.T8 = 0u | 0x0009u;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.At = 0x00010000u;
        c.At = c.At + c.T9;
        c.T9 = mem.ReadU32((c.At - RAddr.Game_800B7234));
        c.T7 = (uint)(short)mem.ReadU16((c.T9 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.T9 + 0xAu));
        mem.WriteU16((c.A3 + 0xEu), (ushort)c.T7);
        mem.WriteU16((c.A3 + 0x1Au), (ushort)c.T8);
        c.A3 = c.A3 + 0x28u;
        L800B7250: ;
        c.A2 = c.A2 + 0x18u;
        c.S5 = c.S5 - 0x1u;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B6FA8;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x44u;
        return;
    }
    public static void func_800B72A0(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.V0 = mem.ReadU32((c.SP + 0x10u));
        c.V1 = mem.ReadU32((c.SP + 0x14u));
        c.SP = c.SP - 0x44u;
        mem.WriteU32((c.SP + 0x10u), c.S0);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x30u), c.FP);
        c.S0 = mem.ReadU32((c.V0 + 0x20u));
        c.S1 = (uint)(short)mem.ReadU16((c.V0 + 0x24u));
        c.S2 = mem.ReadU32((c.V0 + 0x28u));
        c.S3 = mem.ReadU32((c.V0 + 0x2Cu));
        c.S4 = (uint)(short)mem.ReadU16((c.V0 + 0x26u));
        c.S5 = c.A2 - 0x4u;
        c.S5 = mem.ReadU32(c.S5);
        c.S5 = (uint)((int)c.S5 >> 16);
        c.S6 = 0x00FF0000u;
        c.S6 = c.S6 | 0xFFFFu;
        c.S7 = 0x0C000000u;
        mem.WriteU32((c.SP + 0x34u), c.S6);
        mem.WriteU32((c.SP + 0x38u), c.S7);
        c.T9 = 0x800E0000u;
        c.At = 0x00010000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At - RAddr.Game_800B7314));
        c.S6 = mem.ReadU8((c.T5 + 0x4u));
        c.S7 = mem.ReadU8((c.T5 + 0x5u));
        c.S6 = c.S6 + 0x40u;
        c.S7 = c.S7 + 0x40u;
        mem.WriteU32((c.SP + 0x3Cu), c.S6);
        mem.WriteU32((c.SP + 0x40u), c.S7);
        c.FP = mem.ReadU32((c.V0 + 0x44u));
        c.T0 = (uint)(short)mem.ReadU16(c.A2);
        c.T1 = (uint)(short)mem.ReadU16((c.A2 + 0x2u));
        c.T2 = (uint)(short)mem.ReadU16((c.A2 + 0x4u));
        c.T3 = (uint)(short)mem.ReadU16((c.A2 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T2 = c.T2 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T2 = c.T2 + c.A0;
        c.T3 = c.T3 + c.A0;
        L800B7368: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        { var _lw = mem.ReadU32(c.T0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.T1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T3); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T3 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.T8 = c.A2 + 0x1Cu;
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.T0 = (uint)(short)mem.ReadU16(c.T8);
        c.T1 = (uint)(short)mem.ReadU16((c.T8 + 0x2u));
        c.T3 = (uint)(short)mem.ReadU16((c.T8 + 0x6u));
        c.T0 = c.T0 << 3;
        c.T1 = c.T1 << 3;
        c.T3 = c.T3 << 3;
        c.T0 = c.T0 + c.A0;
        c.T1 = c.T1 + c.A0;
        c.T3 = c.T3 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        { var _lw = mem.ReadU32(c.T2); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T2 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x8u), _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        { var _lw = mem.ReadU32((c.A2 + 0x18u)); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.T2 = (uint)(short)mem.ReadU16((c.T8 + 0x4u));
        c.T2 = c.T2 << 3;
        if ((int)c.T9 > 0) {
            c.T2 = c.T2 + c.A0;
            goto L800B73F4;
        }
        c.T2 = c.T2 + c.A0;
        RecompOne.Runtime.Gte.Nclip();
        c.T9 = RecompOne.Runtime.Gte.Read(24);
        if ((int)c.T9 >= 0) {
            goto L800B76E8;
        }
        L800B73F4: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x8u));
        c.T8 = RecompOne.Runtime.Gte.Read(7);
        c.T4 = c.T4 << 3;
        c.T8 = (uint)((int)c.T8 >> (int)(c.S1 & 31u));
        c.T8 = c.T8 + c.S4;
        c.At = (int)c.T8 < (int)c.S3 ? 1u : 0u;
        if (c.At != 0u) {
            c.T4 = c.T4 + c.A1;
            goto L800B76E8;
        }
        c.T4 = c.T4 + c.A1;
        c.At = (int)c.T8 < (int)c.S2 ? 1u : 0u;
        if (c.At == 0u) {
            goto L800B76E8;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.A3 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.A3 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.A3 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.A3 + 0x20u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.A3 + 0x2Cu), _sw); Recompiled.EntityFloatProjection.Store(14, (c.A3 + 0x2Cu), _sw); }
        c.T9 = 0x1F800000u;
        c.At = 0x00000000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At + 0x38u));
        c.T6 = mem.ReadU32(c.T5);
        c.T7 = mem.ReadU32((c.T5 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T6);
        RecompOne.Runtime.Gte.WriteControl(1, c.T7);
        c.T6 = mem.ReadU32((c.T5 + 0x8u));
        c.T7 = mem.ReadU32((c.T5 + 0xCu));
        c.T9 = mem.ReadU32((c.T5 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T6);
        RecompOne.Runtime.Gte.WriteControl(3, c.T7);
        RecompOne.Runtime.Gte.WriteControl(4, c.T9);
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.S6 = mem.ReadU32((c.SP + 0x3Cu));
        c.S7 = mem.ReadU32((c.SP + 0x40u));
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0xCu), (ushort)c.T5);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0xCu));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0x18u), (ushort)c.T5);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0xEu));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0x30u), (ushort)c.T5);
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 1, 3);
        c.T4 = (uint)(short)mem.ReadU16((c.A2 + 0x10u));
        c.T4 = c.T4 << 3;
        c.T4 = c.T4 + c.A1;
        { var _lw = mem.ReadU32(c.T4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        c.T5 = RecompOne.Runtime.Gte.Read(25);
        c.T5 = (uint)((int)c.T5 >> 6);
        c.T5 = c.T5 + c.S6;
        c.T6 = RecompOne.Runtime.Gte.Read(26);
        c.T6 = 0u - c.T6;
        c.T6 = (uint)((int)c.T6 >> 6);
        c.T6 = c.T6 + c.S7;
        c.T6 = c.T6 << 8;
        c.T5 = c.T6 + c.T5;
        mem.WriteU16((c.A3 + 0x24u), (ushort)c.T5);
        c.T9 = 0x1F800000u;
        c.At = 0x00000000u;
        c.At = c.At + c.T9;
        c.T5 = mem.ReadU32((c.At + 0x34u));
        c.T6 = mem.ReadU32(c.T5);
        c.T7 = mem.ReadU32((c.T5 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T6);
        RecompOne.Runtime.Gte.WriteControl(1, c.T7);
        c.T6 = mem.ReadU32((c.T5 + 0x8u));
        c.T7 = mem.ReadU32((c.T5 + 0xCu));
        c.T9 = mem.ReadU32((c.T5 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T6);
        RecompOne.Runtime.Gte.WriteControl(3, c.T7);
        RecompOne.Runtime.Gte.WriteControl(4, c.T9);
        c.T5 = (uint)(short)mem.ReadU16((c.A2 + 0xAu));
        c.T7 = (uint)(short)mem.ReadU16((c.A2 + 0xEu));
        c.T5 = c.T5 << 3;
        c.T7 = c.T7 << 3;
        c.T5 = c.T5 + c.A1;
        c.T7 = c.T7 + c.A1;
        { var _lw = mem.ReadU32(c.T5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.T5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.T7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.T7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        c.S6 = mem.ReadU32((c.SP + 0x34u));
        c.S7 = mem.ReadU32((c.SP + 0x38u));
        if (0u == c.V1) {
            goto L800B766C;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0x14u));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        goto L800B76B4;
        L800B766C: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.T6 = (uint)(short)mem.ReadU16((c.A2 + 0x14u));
        c.T9 = c.A3 & c.S6;
        c.T8 = c.T8 << 2;
        c.T8 = c.T8 + c.S0;
        c.At = mem.ReadU32(c.T8);
        mem.WriteU32(c.T8, c.T9);
        c.At = c.At | c.S7;
        mem.WriteU32(c.T9, c.At);
        c.T6 = c.T6 << 3;
        c.T6 = c.T6 + c.A1;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.A3 + 0x4u), _sw); }
        { var _lw = mem.ReadU32(c.T6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.T6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.A3 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x1Cu), _sw); }
        RecompOne.Runtime.Gte.NccsOp(12, true);
        L800B76B4: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.A3 + 0x28u), _sw); }
        c.T9 = 0x800E0000u;
        c.T8 = 0u | 0x000Cu;
        mem.WriteU8((c.A3 + 0x3u), (byte)c.T8);
        c.At = 0x00010000u;
        c.At = c.At + c.T9;
        c.T9 = mem.ReadU32((c.At - RAddr.Game_800B76CC));
        c.T7 = (uint)(short)mem.ReadU16((c.T9 + 0x6u));
        c.T8 = (uint)(short)mem.ReadU16((c.T9 + 0xAu));
        mem.WriteU16((c.A3 + 0xEu), (ushort)c.T7);
        mem.WriteU16((c.A3 + 0x1Au), (ushort)c.T8);
        c.A3 = c.A3 + 0x34u;
        L800B76E8: ;
        c.S5 = c.S5 - 0x1u;
        c.A2 = c.A2 + 0x1Cu;
        if ((int)c.S5 > 0) {
            c.FP = c.FP - 0x1u;
            goto L800B7368;
        }
        c.FP = c.FP - 0x1u;
        c.T8 = 0x1F800000u;
        c.T8 = c.T8 | 0x0010u;
        mem.WriteU32(c.T8, c.A3);
        mem.WriteU32((c.V0 + 0x40u), c.A2);
        mem.WriteU32((c.V0 + 0x44u), c.FP);
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.FP = mem.ReadU32((c.SP + 0x30u));
        c.SP = c.SP + 0x44u;
        return;
    }
    public static void func_800B7738(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x30u;
        c.T5 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T2 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        c.V1 = c.T2;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.T7 = mem.ReadU32((c.T0 + 0x20u));
        c.S2 = mem.ReadU16((c.T0 + 0x24u));
        c.S1 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S0 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.S3 = mem.ReadU32((c.SP + 0x44u));
        c.T6 = mem.ReadU32((c.SP + 0x48u));
        if (c.V1 == 0u) {
            c.T2 = c.T2 - 0x1u;
            goto L800B7A50;
        }
        c.T2 = c.T2 - 0x1u;
        c.T9 = c.T6 & 0x0006u;
        c.T4 = 0x00FF0000u;
        c.T4 = c.T4 | 0xFFFFu;
        c.T8 = 0xFF000000u;
        c.A2 = c.A2 + 0x6u;
        c.A3 = c.A3 + 0x3u;
        L800B77D8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T0);
        c.S5 = mem.ReadU32((c.T0 + 0x4u));
        c.S6 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.V0 = mem.ReadU32((c.A2 + 0x2u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T6 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B7878;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B7878: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = c.T0 + 0x34u;
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.V0, _sw); }
        c.V0 = c.T6 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B78A0;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 <= 0) {
            goto L800B7A30;
        }
        L800B78A0: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S2 & 31u));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < (int)c.S0 ? 1u : 0u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.T0 + 0x38u), c.V1);
            goto L800B7A30;
        }
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.V0 = (int)c.V1 < (int)c.S1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B7A30;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.T1 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.T1 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.T1 + 0xCu), _sw); Recompiled.EntityFloatProjection.Store(13, (c.T1 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.T1 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.T1 + 0x10u), _sw); }
        c.V0 = c.T6 & 0x0020u;
        if (c.V0 != 0u) {
            c.V0 = 0x00000006u;
            goto L800B79D0;
        }
        c.V0 = 0x00000006u;
        if (c.T9 != c.V0) {
            goto L800B7980;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            goto L800B7980;
        }
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = c.SP + 0x8u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B7998;
        L800B7980: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B7998: ;
        if (c.S3 == 0u) {
            goto L800B79B4;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        goto L800B79C4;
        L800B79B4: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        L800B79C4: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        goto L800B79DC;
        L800B79D0: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x1u), c.V0);
        L800B79DC: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T7;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T4;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.V1 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.T1 & c.T4;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.T7;
        c.V0 = mem.ReadU32(c.V1);
        c.T1 = c.T1 + 0x14u;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.A0;
        mem.WriteU32(c.V1, c.V0);
        c.V0 = 0x00000004u;
        mem.WriteU8(c.A3, (byte)c.V0);
        c.A3 = c.A3 + 0x14u;
        L800B7A30: ;
        c.A2 = c.A2 + 0xCu;
        c.T3 = c.T3 + 0xCu;
        c.V1 = c.T2;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T2 = c.T2 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B77D8;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B7A50: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x30u;
        return;
    }
    public static void func_800B7A80(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x30u;
        c.T5 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x2Cu), c.S7);
        mem.WriteU32((c.SP + 0x28u), c.S6);
        mem.WriteU32((c.SP + 0x24u), c.S5);
        mem.WriteU32((c.SP + 0x20u), c.S4);
        mem.WriteU32((c.SP + 0x1Cu), c.S3);
        mem.WriteU32((c.SP + 0x18u), c.S2);
        mem.WriteU32((c.SP + 0x14u), c.S1);
        mem.WriteU32((c.SP + 0x10u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T4 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        c.V1 = c.T4;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.T9 = mem.ReadU32((c.T0 + 0x20u));
        c.S3 = mem.ReadU16((c.T0 + 0x24u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S2 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x6u));
        c.S1 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        c.S4 = mem.ReadU32((c.SP + 0x44u));
        c.T7 = mem.ReadU32((c.SP + 0x48u));
        if (c.V1 == 0u) {
            c.T4 = c.T4 - 0x1u;
            goto L800B7E04;
        }
        c.T4 = c.T4 - 0x1u;
        c.T8 = c.T0 + 0x34u;
        c.T6 = 0x00FF0000u;
        c.T6 = c.T6 | 0xFFFFu;
        c.S0 = 0xFF000000u;
        c.A2 = c.A2 + 0x8u;
        c.A3 = c.A3 + 0x3u;
        L800B7B34: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S5 = mem.ReadU32(c.T0);
        c.S6 = mem.ReadU32((c.T0 + 0x4u));
        c.S7 = mem.ReadU32((c.T0 + 0xCu));
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.T2 = 0u;
        c.S5 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.T8, _sw); }
        c.V0 = c.T1 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(12, c.V0, _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T7 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B7C00;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B7C00: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V1 = mem.ReadU32((c.T0 + 0x34u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T5 + c.V0;
        if ((int)c.V1 > 0) {
            mem.WriteU32((c.T0 + 0x8u), c.V0);
            goto L800B7C50;
        }
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.T8, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 < 0) {
            c.V0 = c.T7 & 0x0002u;
            goto L800B7C40;
        }
        c.V0 = c.T7 & 0x0002u;
        c.T2 = 0x00000001u;
        L800B7C40: ;
        if (c.V0 != 0u) {
            goto L800B7C50;
        }
        if (c.T2 != 0u) {
            goto L800B7DE4;
        }
        L800B7C50: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S3 & 31u));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < (int)c.S1 ? 1u : 0u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.T0 + 0x38u), c.V1);
            goto L800B7DE4;
        }
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.V0 = (int)c.V1 < (int)c.S2 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.T1 + 0xCu;
            goto L800B7DE4;
        }
        c.A0 = c.T1 + 0xCu;
        c.V1 = c.T1 + 0x10u;
        c.V0 = c.T1 + 0x14u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.A0, _sw); Recompiled.EntityFloatProjection.Store(12, c.A0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); Recompiled.EntityFloatProjection.Store(13, c.V1, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(14, c.V0, _sw); }
        c.V0 = c.T7 & 0x0020u;
        if (c.V0 != 0u) {
            c.V1 = c.T7 & 0x0006u;
            goto L800B7D84;
        }
        c.V1 = c.T7 & 0x0006u;
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L800B7D34;
        }
        if (c.T2 == 0u) {
            goto L800B7D34;
        }
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = c.SP + 0x8u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B7D4C;
        L800B7D34: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B7D4C: ;
        if (c.S4 == 0u) {
            goto L800B7D68;
        }
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        goto L800B7D78;
        L800B7D68: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T1 + 0x4u;
        L800B7D78: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        goto L800B7D90;
        L800B7D84: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x1u), c.V0);
        L800B7D90: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T9;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S0;
        c.V0 = c.V0 & c.T6;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.V1 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.T1 & c.T6;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.T9;
        c.V0 = mem.ReadU32(c.V1);
        c.T1 = c.T1 + 0x18u;
        c.V0 = c.V0 & c.S0;
        c.V0 = c.V0 | c.A0;
        mem.WriteU32(c.V1, c.V0);
        c.V0 = 0x00000005u;
        mem.WriteU8(c.A3, (byte)c.V0);
        c.A3 = c.A3 + 0x18u;
        L800B7DE4: ;
        c.A2 = c.A2 + 0x10u;
        c.T3 = c.T3 + 0x10u;
        c.V1 = c.T4;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T4 = c.T4 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B7B34;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B7E04: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S7 = mem.ReadU32((c.SP + 0x2Cu));
        c.S6 = mem.ReadU32((c.SP + 0x28u));
        c.S5 = mem.ReadU32((c.SP + 0x24u));
        c.S4 = mem.ReadU32((c.SP + 0x20u));
        c.S3 = mem.ReadU32((c.SP + 0x1Cu));
        c.S2 = mem.ReadU32((c.SP + 0x18u));
        c.S1 = mem.ReadU32((c.SP + 0x14u));
        c.S0 = mem.ReadU32((c.SP + 0x10u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x30u;
        return;
    }
    public static void func_800B7E38(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x40u;
        c.T6 = c.A0;
        c.T3 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x3Cu), c.S7);
        mem.WriteU32((c.SP + 0x38u), c.S6);
        mem.WriteU32((c.SP + 0x34u), c.S5);
        mem.WriteU32((c.SP + 0x30u), c.S4);
        mem.WriteU32((c.SP + 0x2Cu), c.S3);
        mem.WriteU32((c.SP + 0x28u), c.S2);
        mem.WriteU32((c.SP + 0x24u), c.S1);
        mem.WriteU32((c.SP + 0x20u), c.S0);
        c.V0 = mem.ReadU16(c.T3);
        c.T2 = mem.ReadU16((c.T3 - 0x2u));
        c.T0 = mem.ReadU32((c.SP + 0x50u));
        c.T5 = 0x1F800000u;
        c.T5 = mem.ReadU16((c.T5 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        c.V1 = c.T2;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x2u));
        c.T8 = mem.ReadU32((c.T0 + 0x20u));
        c.S3 = mem.ReadU16((c.T0 + 0x24u));
        c.S2 = mem.ReadU32((c.T0 + 0x28u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T3 + 0x4u));
        c.S1 = mem.ReadU32((c.T0 + 0x2Cu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.S4 = mem.ReadU32((c.SP + 0x54u));
        c.T7 = mem.ReadU32((c.SP + 0x58u));
        if (c.V1 == 0u) {
            c.T2 = c.T2 - 0x1u;
            goto L800B82D0;
        }
        c.T2 = c.T2 - 0x1u;
        c.S0 = 0x00000006u;
        c.T4 = 0x00FF0000u;
        c.T4 = c.T4 | 0xFFFFu;
        c.T9 = 0xFF000000u;
        c.A2 = c.A2 + 0xAu;
        c.A3 = c.A3 + 0x3u;
        L800B7EE4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S5 = mem.ReadU32(c.T0);
        c.S6 = mem.ReadU32((c.T0 + 0x4u));
        c.S7 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T6 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.V0 = mem.ReadU32((c.A2 + 0x2u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.T7 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B7F84;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B7F84: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = c.T0 + 0x34u;
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.V0, _sw); }
        c.V0 = c.T7 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B7FAC;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 <= 0) {
            goto L800B82B0;
        }
        L800B7FAC: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S3 & 31u));
        c.V1 = c.V0 + c.V1;
        c.V0 = (int)c.V1 < (int)c.S1 ? 1u : 0u;
        if (c.V0 != 0u) {
            mem.WriteU32((c.T0 + 0x38u), c.V1);
            goto L800B82B0;
        }
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.V0 = (int)c.V1 < (int)c.S2 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B82B0;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.T1 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.T1 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.T1 + 0x10u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.T1 + 0x18u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.T1 + 0x18u), _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x7u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
            goto L800B8030;
        }
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xFu));
        if ((int)c.V0 >= 0) {
            goto L800B8024;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x17u));
        if ((int)c.V0 < 0) {
            goto L800B82B0;
        }
        L800B8024: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x7u));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        L800B8030: ;
        if (c.V0 != 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B8060;
        }
        c.V0 = c.T7 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xFu));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B8060;
        }
        c.V0 = c.T7 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x17u));
        c.V0 = (int)c.V0 < (int)c.T5 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.T7 & 0x0020u;
            goto L800B82B0;
        }
        c.V0 = c.T7 & 0x0020u;
        L800B8060: ;
        if (c.V0 != 0u) {
            c.V0 = c.T7 & 0x0006u;
            goto L800B823C;
        }
        c.V0 = c.T7 & 0x0006u;
        if (c.V0 != c.S0) {
            goto L800B81C4;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            c.A0 = c.SP + 0x8u;
            goto L800B81C4;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B8200;
        L800B81C4: ;
        c.A0 = mem.ReadU16((c.A2 - 0x4u));
        c.V1 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = mem.ReadU16(c.A2);
        c.A0 = c.A0 << 3;
        c.A0 = c.A1 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.A1 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B8200: ;
        if (c.S4 == 0u) {
            goto L800B821C;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B8228;
        L800B821C: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B8228: ;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T1 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T1 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T1 + 0x14u), _sw); }
        goto L800B8260;
        L800B823C: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x1u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x9u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x11u), c.V0);
        L800B8260: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T8;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T9;
        c.V0 = c.V0 & c.T4;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.V1 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.T1 & c.T4;
        c.V1 = c.V1 << 2;
        c.V1 = c.V1 + c.T8;
        c.V0 = mem.ReadU32(c.V1);
        c.T1 = c.T1 + 0x1Cu;
        c.V0 = c.V0 & c.T9;
        c.V0 = c.V0 | c.A0;
        mem.WriteU32(c.V1, c.V0);
        mem.WriteU8(c.A3, (byte)c.S0);
        c.A3 = c.A3 + 0x1Cu;
        L800B82B0: ;
        c.A2 = c.A2 + 0x10u;
        c.T3 = c.T3 + 0x10u;
        c.V1 = c.T2;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T2 = c.T2 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B7EE4;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B82D0: ;
        mem.WriteU32((c.T0 + 0x40u), c.T3);
        c.S7 = mem.ReadU32((c.SP + 0x3Cu));
        c.S6 = mem.ReadU32((c.SP + 0x38u));
        c.S5 = mem.ReadU32((c.SP + 0x34u));
        c.S4 = mem.ReadU32((c.SP + 0x30u));
        c.S3 = mem.ReadU32((c.SP + 0x2Cu));
        c.S2 = mem.ReadU32((c.SP + 0x28u));
        c.S1 = mem.ReadU32((c.SP + 0x24u));
        c.S0 = mem.ReadU32((c.SP + 0x20u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x40u;
        return;
    }
    public static void func_800B8304(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x58u;
        c.T9 = c.A0;
        c.T5 = c.A2;
        mem.WriteU32((c.SP + 0x54u), c.S7);
        mem.WriteU32((c.SP + 0x50u), c.S6);
        mem.WriteU32((c.SP + 0x4Cu), c.S5);
        mem.WriteU32((c.SP + 0x48u), c.S4);
        mem.WriteU32((c.SP + 0x44u), c.S3);
        mem.WriteU32((c.SP + 0x40u), c.S2);
        mem.WriteU32((c.SP + 0x3Cu), c.S1);
        mem.WriteU32((c.SP + 0x38u), c.S0);
        c.V0 = mem.ReadU16(c.T5);
        c.T6 = mem.ReadU16((c.T5 - 0x2u));
        c.T1 = mem.ReadU32((c.SP + 0x68u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32(c.T1, c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x2u));
        c.T7 = 0x1F800000u;
        c.T7 = mem.ReadU16((c.T7 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x4u));
        c.T4 = mem.ReadU32((c.T1 + 0x20u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.T5 + 0x6u));
        c.S4 = mem.ReadU16((c.T1 + 0x24u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0xCu), c.V0);
        c.S7 = mem.ReadU32((c.SP + 0x6Cu));
        c.S1 = mem.ReadU32((c.SP + 0x70u));
        c.T0 = c.A3;
        mem.WriteU32((c.SP + 0x28u), c.S4);
        c.S5 = mem.ReadU32((c.T1 + 0x28u));
        c.V1 = c.T6;
        mem.WriteU32((c.SP + 0x2Cu), c.S5);
        c.S6 = mem.ReadU32((c.T1 + 0x2Cu));
        c.T6 = c.T6 - 0x1u;
        if (c.V1 == 0u) {
            mem.WriteU32((c.SP + 0x30u), c.S6);
            goto L800B89F0;
        }
        mem.WriteU32((c.SP + 0x30u), c.S6);
        c.S2 = c.T1 + 0x34u;
        c.S0 = c.S1 & 0x0006u;
        c.S3 = 0x00000006u;
        c.T2 = 0x00FF0000u;
        c.T2 = c.T2 | 0xFFFFu;
        c.T8 = 0xFF000000u;
        c.A2 = c.A2 + 0xCu;
        c.A3 = c.A3 + 0x3u;
        L800B83D4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T1);
        c.S5 = mem.ReadU32((c.T1 + 0x4u));
        c.S6 = mem.ReadU32((c.T1 + 0xCu));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S5); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S5 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32(c.T1, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        mem.WriteU32((c.T1 + 0xCu), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.T3 = 0u;
        c.S4 = mem.ReadU32((c.T1 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.S2, _sw); }
        c.V0 = c.T0 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(12, c.V0, _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.S1 & 0x0010u;
        if (c.V0 == 0u) {
            goto L800B84A0;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B84A0: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V1 = mem.ReadU32((c.T1 + 0x34u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T9 + c.V0;
        if ((int)c.V1 > 0) {
            mem.WriteU32((c.T1 + 0x8u), c.V0);
            goto L800B84F0;
        }
        mem.WriteU32((c.T1 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.S2, _sw); }
        c.V0 = mem.ReadU32((c.T1 + 0x34u));
        if ((int)c.V0 < 0) {
            c.V0 = c.S1 & 0x0002u;
            goto L800B84E0;
        }
        c.V0 = c.S1 & 0x0002u;
        c.T3 = 0x00000001u;
        L800B84E0: ;
        if (c.V0 != 0u) {
            goto L800B84F0;
        }
        if (c.T3 != 0u) {
            goto L800B89D0;
        }
        L800B84F0: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.V0 = c.T1 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.S5 = mem.ReadU32((c.SP + 0x28u));
        c.V1 = (uint)(short)mem.ReadU16((c.T1 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S5 & 31u));
        c.V1 = c.V0 + c.V1;
        mem.WriteU32((c.T1 + 0x38u), c.V1);
        c.S6 = mem.ReadU32((c.SP + 0x30u));
        c.V0 = (int)c.V1 < (int)c.S6 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B89D0;
        }
        c.S4 = mem.ReadU32((c.SP + 0x2Cu));
        c.V0 = (int)c.V1 < (int)c.S4 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.T0 + 0x10u;
            goto L800B89D0;
        }
        c.A0 = c.T0 + 0x10u;
        c.V1 = c.T0 + 0x18u;
        c.V0 = c.T0 + 0x20u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.A0, _sw); Recompiled.EntityFloatProjection.Store(12, c.A0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); Recompiled.EntityFloatProjection.Store(13, c.V1, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(14, c.V0, _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x7u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
            goto L800B85A4;
        }
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xFu));
        if ((int)c.V0 >= 0) {
            goto L800B8598;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x17u));
        if ((int)c.V0 >= 0) {
            goto L800B8598;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x1Fu));
        if ((int)c.V0 < 0) {
            goto L800B89D0;
        }
        L800B8598: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x7u));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        L800B85A4: ;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B85E8;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0xFu));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B85E8;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x17u));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B85E8;
        }
        c.V0 = c.S1 & 0x0020u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x1Fu));
        c.V0 = (int)c.V0 < (int)c.T7 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.V0 = c.S1 & 0x0020u;
            goto L800B89D0;
        }
        c.V0 = c.S1 & 0x0020u;
        L800B85E8: ;
        if (c.V0 != 0u) {
            goto L800B8948;
        }
        if (c.S0 != c.S3) {
            goto L800B87A4;
        }
        if (c.T3 == 0u) {
            c.A0 = c.SP + 0x8u;
            goto L800B87A4;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x20u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x22u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.A1;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x24u), (ushort)c.V0);
        c.V0 = c.SP + 0x20u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B87E0;
        L800B87A4: ;
        c.A0 = mem.ReadU16((c.A2 - 0x4u));
        c.V1 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.A0 = c.A0 << 3;
        c.A0 = c.A1 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.A1 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B87E0: ;
        if (c.S7 == 0u) {
            goto L800B8894;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T0 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T0 + 0x14u), _sw); }
        if (c.S0 != c.S3) {
            goto L800B8868;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B8868;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B8880;
        L800B8868: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B8880: ;
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T0 + 0x1Cu;
        goto L800B893C;
        L800B8894: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T0 + 0xCu), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T0 + 0x14u), _sw); }
        if (c.S0 != c.S3) {
            goto L800B8914;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B8914;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B892C;
        L800B8914: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.A1 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B892C: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T0 + 0x1Cu;
        L800B893C: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        c.V0 = 0x00000008u;
        goto L800B89C4;
        L800B8948: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x1u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x9u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x11u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x19u), c.V0);
        c.V0 = mem.ReadU32((c.T1 + 0x38u));
        c.V1 = mem.ReadU32(c.T0);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T4;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.T8;
        c.V0 = c.V0 & c.T2;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T0, c.V1);
        c.A0 = mem.ReadU32((c.T1 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T4;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T0 & c.T2;
        c.V0 = c.V0 & c.T8;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x00000008u;
        L800B89C4: ;
        mem.WriteU8(c.A3, (byte)c.V0);
        c.A3 = c.A3 + 0x24u;
        c.T0 = c.T0 + 0x24u;
        L800B89D0: ;
        c.A2 = c.A2 + 0x14u;
        c.T5 = c.T5 + 0x14u;
        c.V1 = c.T6;
        c.V0 = mem.ReadU32((c.T1 + 0x44u));
        c.T6 = c.T6 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T1 + 0x44u), c.V0);
            goto L800B83D4;
        }
        mem.WriteU32((c.T1 + 0x44u), c.V0);
        L800B89F0: ;
        mem.WriteU32((c.T1 + 0x40u), c.T5);
        c.S7 = mem.ReadU32((c.SP + 0x54u));
        c.S6 = mem.ReadU32((c.SP + 0x50u));
        c.S5 = mem.ReadU32((c.SP + 0x4Cu));
        c.S4 = mem.ReadU32((c.SP + 0x48u));
        c.S3 = mem.ReadU32((c.SP + 0x44u));
        c.S2 = mem.ReadU32((c.SP + 0x40u));
        c.S1 = mem.ReadU32((c.SP + 0x3Cu));
        c.S0 = mem.ReadU32((c.SP + 0x38u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T0);
        c.SP = c.SP + 0x58u;
        return;
    }
    public static void func_800B8A24(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x68u;
        mem.WriteU32((c.SP + 0x40u), c.S0);
        c.S0 = c.A0;
        c.T2 = c.A1;
        c.T7 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x60u), c.FP);
        mem.WriteU32((c.SP + 0x5Cu), c.S7);
        mem.WriteU32((c.SP + 0x58u), c.S6);
        mem.WriteU32((c.SP + 0x54u), c.S5);
        mem.WriteU32((c.SP + 0x50u), c.S4);
        mem.WriteU32((c.SP + 0x4Cu), c.S3);
        mem.WriteU32((c.SP + 0x48u), c.S2);
        mem.WriteU32((c.SP + 0x44u), c.S1);
        c.T3 = mem.ReadU16((c.T7 - 0x2u));
        c.V0 = mem.ReadU16(c.T7);
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_800B8A64));
        c.T0 = mem.ReadU32((c.SP + 0x78u));
        c.T9 = 0x1F800000u;
        c.T9 = mem.ReadU16((c.T9 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.A0 = mem.ReadU8((c.V1 + 0x4u));
        c.V1 = mem.ReadU8((c.V1 + 0x5u));
        c.V0 = c.S0 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T7 + 0x2u));
        c.A1 = c.T3;
        c.V0 = c.V0 << 3;
        c.V0 = c.S0 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T7 + 0x4u));
        c.S4 = mem.ReadU32((c.T0 + 0x20u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S0 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.S2 = mem.ReadU32((c.SP + 0x80u));
        c.T3 = c.T3 - 0x1u;
        mem.WriteU32((c.SP + 0x38u), c.S4);
        c.S6 = mem.ReadU16((c.T0 + 0x24u));
        c.S3 = c.A0 + 0x40u;
        mem.WriteU32((c.SP + 0x34u), c.S6);
        c.FP = mem.ReadU32((c.T0 + 0x28u));
        c.S7 = mem.ReadU32((c.T0 + 0x2Cu));
        c.S1 = c.V1 + 0x40u;
        if (c.A1 == 0u) {
            mem.WriteU32((c.SP + 0x30u), c.S7);
            goto L800B90B0;
        }
        mem.WriteU32((c.SP + 0x30u), c.S7);
        c.T8 = c.SP + 0x20u;
        c.A1 = 0x00FF0000u;
        c.A1 = c.A1 | 0xFFFFu;
        c.S5 = 0xFF000000u;
        c.A2 = c.A2 + 0x10u;
        c.A3 = c.A3 + 0x1Au;
        L800B8AF4: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S4 = mem.ReadU32(c.T0);
        c.S6 = mem.ReadU32((c.T0 + 0x4u));
        c.S7 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S0 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.S0 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.S0 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.S2 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B8B94;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B8B94: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = c.T0 + 0x34u;
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.V0, _sw); }
        c.V0 = c.S2 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B8BBC;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 <= 0) {
            goto L800B9090;
        }
        L800B8BBC: ;
        RecompOne.Runtime.Gte.Avsz3();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.S4 = mem.ReadU32((c.SP + 0x34u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S4 & 31u));
        c.V1 = c.V0 + c.V1;
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.S6 = mem.ReadU32((c.SP + 0x30u));
        c.V0 = (int)c.V1 < (int)c.S6 ? 1u : 0u;
        if (c.V0 != 0u) {
            c.V0 = (int)c.V1 < (int)c.FP ? 1u : 0u;
            goto L800B9090;
        }
        c.V0 = (int)c.V1 < (int)c.FP ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B9090;
        }
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32((c.T1 + 0x8u), _sw); Recompiled.EntityFloatProjection.Store(12, (c.T1 + 0x8u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32((c.T1 + 0x14u), _sw); Recompiled.EntityFloatProjection.Store(13, (c.T1 + 0x14u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32((c.T1 + 0x20u), _sw); Recompiled.EntityFloatProjection.Store(14, (c.T1 + 0x20u), _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x10u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.T9 ? 1u : 0u;
            goto L800B8C4C;
        }
        c.V0 = (int)c.V0 < (int)c.T9 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x4u));
        if ((int)c.V0 >= 0) {
            goto L800B8C40;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x8u));
        if ((int)c.V0 < 0) {
            goto L800B9090;
        }
        L800B8C40: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x10u));
        c.V0 = (int)c.V0 < (int)c.T9 ? 1u : 0u;
        L800B8C4C: ;
        if (c.V0 != 0u) {
            goto L800B8C7C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x4u));
        c.V0 = (int)c.V0 < (int)c.T9 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B8C7C;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x8u));
        c.V0 = (int)c.V0 < (int)c.T9 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B9090;
        }
        L800B8C7C: ;
        c.S7 = 0x1F800000u;
        c.S7 = mem.ReadU32((c.S7 + 0x38u));
        c.T4 = mem.ReadU32(c.S7);
        c.T5 = mem.ReadU32((c.S7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S7 + 0x8u));
        c.T5 = mem.ReadU32((c.S7 + 0xCu));
        c.T6 = mem.ReadU32((c.S7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V0 = mem.ReadU16((c.A2 - 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.T8, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.T8 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.T8 + 0x8u), _sw); }
        c.S4 = mem.ReadU32((c.T0 + 0x10u));
        { var _lw = mem.ReadU32(c.S4); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S4 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V1 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = mem.ReadU32((c.SP + 0x24u));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S1 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 - 0xEu), (ushort)c.V1);
        c.V0 = mem.ReadU16((c.A2 - 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.T8, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.T8 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.T8 + 0x8u), _sw); }
        c.S6 = mem.ReadU32((c.T0 + 0x10u));
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V1 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = mem.ReadU32((c.SP + 0x24u));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S1 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 - 0x2u), (ushort)c.V1);
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.T8, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.T8 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.T8 + 0x8u), _sw); }
        c.V1 = mem.ReadU32((c.SP + 0x20u));
        c.V0 = mem.ReadU32((c.SP + 0x24u));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S1 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 + 0xAu), (ushort)c.V1);
        c.S7 = 0x1F800000u;
        c.S7 = mem.ReadU32((c.S7 + 0x34u));
        c.T4 = mem.ReadU32(c.S7);
        c.T5 = mem.ReadU32((c.S7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S7 + 0x8u));
        c.T5 = mem.ReadU32((c.S7 + 0xCu));
        c.T6 = mem.ReadU32((c.S7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.V0 = c.S2 & 0x0020u;
        if (c.V0 != 0u) {
            c.V1 = c.S2 & 0x0006u;
            goto L800B8FF4;
        }
        c.V1 = c.S2 & 0x0006u;
        c.V0 = 0x00000006u;
        if (c.V1 != c.V0) {
            goto L800B8F74;
        }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 > 0) {
            c.A0 = c.SP + 0x8u;
            goto L800B8F74;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B8FB0;
        L800B8F74: ;
        c.A0 = mem.ReadU16((c.A2 - 0x4u));
        c.V1 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = mem.ReadU16(c.A2);
        c.A0 = c.A0 << 3;
        c.A0 = c.T2 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.T2 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B8FB0: ;
        c.S4 = mem.ReadU32((c.SP + 0x7Cu));
        if (c.S4 == 0u) {
            goto L800B8FD4;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        goto L800B8FE0;
        L800B8FD4: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        L800B8FE0: ;
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T1 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T1 + 0x1Cu), _sw); }
        goto L800B9018;
        L800B8FF4: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x16u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0xAu), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x2u), c.V0);
        L800B9018: ;
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.S6 = mem.ReadU32((c.SP + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.S6;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S5;
        c.V0 = c.V0 & c.A1;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.S6;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.A1;
        c.V0 = c.V0 & c.S5;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x00000009u;
        mem.WriteU8((c.A3 - 0x17u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_800B906C));
        c.V0 = mem.ReadU16((c.V1 + 0x6u));
        mem.WriteU16((c.A3 - 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.V1 + 0xAu));
        c.T1 = c.T1 + 0x28u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x28u;
        L800B9090: ;
        c.A2 = c.A2 + 0x18u;
        c.T7 = c.T7 + 0x18u;
        c.V1 = c.T3;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T3 = c.T3 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B8AF4;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B90B0: ;
        mem.WriteU32((c.T0 + 0x40u), c.T7);
        c.FP = mem.ReadU32((c.SP + 0x60u));
        c.S7 = mem.ReadU32((c.SP + 0x5Cu));
        c.S6 = mem.ReadU32((c.SP + 0x58u));
        c.S5 = mem.ReadU32((c.SP + 0x54u));
        c.S4 = mem.ReadU32((c.SP + 0x50u));
        c.S3 = mem.ReadU32((c.SP + 0x4Cu));
        c.S2 = mem.ReadU32((c.SP + 0x48u));
        c.S1 = mem.ReadU32((c.SP + 0x44u));
        c.S0 = mem.ReadU32((c.SP + 0x40u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x68u;
        return;
    }
    public static void func_800B90E8(CpuContext c, IMemory m)
    {
        var mem = (PSMemory)m;
        c.SP = c.SP - 0x78u;
        mem.WriteU32((c.SP + 0x64u), c.S5);
        c.S5 = c.A0;
        c.T2 = c.A1;
        c.T8 = c.A2;
        c.T1 = c.A3;
        mem.WriteU32((c.SP + 0x70u), c.FP);
        mem.WriteU32((c.SP + 0x6Cu), c.S7);
        mem.WriteU32((c.SP + 0x68u), c.S6);
        mem.WriteU32((c.SP + 0x60u), c.S4);
        mem.WriteU32((c.SP + 0x5Cu), c.S3);
        mem.WriteU32((c.SP + 0x58u), c.S2);
        mem.WriteU32((c.SP + 0x54u), c.S1);
        mem.WriteU32((c.SP + 0x50u), c.S0);
        c.V0 = mem.ReadU16(c.T8);
        c.T9 = mem.ReadU16((c.T8 - 0x2u));
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_800B9128));
        c.T0 = mem.ReadU32((c.SP + 0x88u));
        c.S1 = 0x1F800000u;
        c.S1 = mem.ReadU16((c.S1 + 0x9Au));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        c.A0 = mem.ReadU8((c.V1 + 0x4u));
        c.V1 = mem.ReadU8((c.V1 + 0x5u));
        c.A1 = c.T9;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.T8 + 0x2u));
        c.T9 = c.T9 - 0x1u;
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.T8 + 0x4u));
        c.T7 = mem.ReadU32((c.T0 + 0x20u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        c.V0 = mem.ReadU16((c.T8 + 0x6u));
        c.S0 = mem.ReadU16((c.T0 + 0x24u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        mem.WriteU32((c.SP + 0x38u), c.S0);
        c.S6 = mem.ReadU32((c.T0 + 0x28u));
        c.S3 = c.A0 + 0x40u;
        mem.WriteU32((c.SP + 0x3Cu), c.S6);
        c.S7 = mem.ReadU32((c.T0 + 0x2Cu));
        c.S2 = c.V1 + 0x40u;
        if (c.A1 == 0u) {
            mem.WriteU32((c.SP + 0x40u), c.S7);
            goto L800B9A48;
        }
        mem.WriteU32((c.SP + 0x40u), c.S7);
        c.FP = c.T0 + 0x34u;
        c.S0 = c.SP + 0x28u;
        c.A1 = 0x00FF0000u;
        c.A1 = c.A1 | 0xFFFFu;
        c.S4 = 0xFF000000u;
        c.A2 = c.A2 + 0x14u;
        c.S6 = mem.ReadU32((c.SP + 0x90u));
        c.A3 = c.A3 + 0x1Au;
        mem.WriteU32((c.SP + 0x4Cu), c.S0);
        c.S6 = c.S6 & 0x0006u;
        mem.WriteU32((c.SP + 0x48u), c.S6);
        L800B91D8: ;
        RecompOne.Runtime.Interrupts.Poll(c, m);
        c.S7 = mem.ReadU32(c.T0);
        c.S0 = mem.ReadU32((c.T0 + 0x4u));
        c.S6 = mem.ReadU32((c.T0 + 0xCu));
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.S0); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.S0 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        Recompiled.EntityFloatProjection.Rtpt(12, false);
        c.V0 = mem.ReadU16((c.A2 + 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32(c.T0, c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32((c.T0 + 0x4u), c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0xEu));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        mem.WriteU32((c.T0 + 0xCu), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        c.T3 = 0u;
        c.S7 = mem.ReadU32((c.T0 + 0x8u));
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.FP, _sw); }
        c.V0 = c.T1 + 0x8u;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(12, c.V0, _sw); }
        Recompiled.EntityFloatProjection.Rtps(12, false);
        c.V0 = mem.ReadU32((c.A2 + 0x4u));
        c.S0 = mem.ReadU32((c.SP + 0x90u));
        mem.WriteU32(c.SP, c.V0);
        c.V0 = c.S0 & 0x0018u;
        if (c.V0 == 0u) {
            goto L800B92A4;
        }
        c.V0 = mem.ReadU8((c.SP + 0x3u));
        c.V0 = c.V0 | 0x0002u;
        mem.WriteU8((c.SP + 0x3u), (byte)c.V0);
        L800B92A4: ;
        { var _lw = mem.ReadU32(c.SP); RecompOne.Runtime.Gte.Write(6, _lw); }
        c.V0 = mem.ReadU16((c.A2 + 0xCu));
        c.V1 = mem.ReadU32((c.T0 + 0x34u));
        c.V0 = c.V0 << 3;
        c.V0 = c.S5 + c.V0;
        if ((int)c.V1 > 0) {
            mem.WriteU32((c.T0 + 0x8u), c.V0);
            goto L800B9300;
        }
        mem.WriteU32((c.T0 + 0x8u), c.V0);
        RecompOne.Runtime.Gte.Nclip();
        { var _sw = RecompOne.Runtime.Gte.Read(24); mem.WriteU32(c.FP, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x34u));
        if ((int)c.V0 < 0) {
            goto L800B92E4;
        }
        c.T3 = 0x00000001u;
        L800B92E4: ;
        c.S6 = mem.ReadU32((c.SP + 0x90u));
        c.V0 = c.S6 & 0x0002u;
        if (c.V0 != 0u) {
            goto L800B9300;
        }
        if (c.T3 != 0u) {
            goto L800B9A28;
        }
        L800B9300: ;
        RecompOne.Runtime.Gte.Avsz4();
        c.V0 = c.T0 + 0x38u;
        { var _sw = RecompOne.Runtime.Gte.Read(7); mem.WriteU32(c.V0, _sw); }
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.S7 = mem.ReadU32((c.SP + 0x38u));
        c.V1 = (uint)(short)mem.ReadU16((c.T0 + 0x26u));
        c.V0 = (uint)((int)c.V0 >> (int)(c.S7 & 31u));
        c.V1 = c.V0 + c.V1;
        mem.WriteU32((c.T0 + 0x38u), c.V1);
        c.S0 = mem.ReadU32((c.SP + 0x40u));
        c.V0 = (int)c.V1 < (int)c.S0 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B9A28;
        }
        c.S6 = mem.ReadU32((c.SP + 0x3Cu));
        c.V0 = (int)c.V1 < (int)c.S6 ? 1u : 0u;
        if (c.V0 == 0u) {
            c.A0 = c.T1 + 0x14u;
            goto L800B9A28;
        }
        c.A0 = c.T1 + 0x14u;
        c.V1 = c.T1 + 0x20u;
        c.V0 = c.T1 + 0x2Cu;
        { var _sw = RecompOne.Runtime.Gte.Read(12); mem.WriteU32(c.A0, _sw); Recompiled.EntityFloatProjection.Store(12, c.A0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(13); mem.WriteU32(c.V1, _sw); Recompiled.EntityFloatProjection.Store(13, c.V1, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(14); mem.WriteU32(c.V0, _sw); Recompiled.EntityFloatProjection.Store(14, c.V0, _sw); }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x10u));
        if ((int)c.V0 >= 0) {
            c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
            goto L800B93B4;
        }
        c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x4u));
        if ((int)c.V0 >= 0) {
            goto L800B93A8;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x8u));
        if ((int)c.V0 >= 0) {
            goto L800B93A8;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x14u));
        if ((int)c.V0 < 0) {
            goto L800B9A28;
        }
        L800B93A8: ;
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x10u));
        c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
        L800B93B4: ;
        if (c.V0 != 0u) {
            goto L800B93F8;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 - 0x4u));
        c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B93F8;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x8u));
        c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
        if (c.V0 != 0u) {
            goto L800B93F8;
        }
        c.V0 = (uint)(short)mem.ReadU16((c.A3 + 0x14u));
        c.V0 = (int)c.V0 < (int)c.S1 ? 1u : 0u;
        if (c.V0 == 0u) {
            goto L800B9A28;
        }
        L800B93F8: ;
        c.S7 = 0x1F800000u;
        c.S7 = mem.ReadU32((c.S7 + 0x38u));
        c.T4 = mem.ReadU32(c.S7);
        c.T5 = mem.ReadU32((c.S7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S7 + 0x8u));
        c.T5 = mem.ReadU32((c.S7 + 0xCu));
        c.T6 = mem.ReadU32((c.S7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.V0 = mem.ReadU16((c.A2 - 0xCu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V0 = mem.ReadU16((c.A2 - 0xAu));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        c.S0 = mem.ReadU32((c.SP + 0x4Cu));
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.S0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.S0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.S0 + 0x8u), _sw); }
        c.S6 = mem.ReadU32((c.T0 + 0x10u));
        { var _lw = mem.ReadU32(c.S6); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S6 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.V0 = mem.ReadU32((c.SP + 0x2Cu));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S2 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 - 0xEu), (ushort)c.V1);
        c.V0 = mem.ReadU16((c.A2 - 0x8u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.S0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.S0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.S0 + 0x8u), _sw); }
        c.S7 = mem.ReadU32((c.T0 + 0x10u));
        { var _lw = mem.ReadU32(c.S7); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S7 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.V0 = mem.ReadU32((c.SP + 0x2Cu));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S2 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 - 0x2u), (ushort)c.V1);
        c.V0 = mem.ReadU16((c.A2 - 0x6u));
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        mem.WriteU32((c.T0 + 0x10u), c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.S0, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.S0 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.S0 + 0x8u), _sw); }
        c.S0 = mem.ReadU32((c.T0 + 0x10u));
        { var _lw = mem.ReadU32(c.S0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.S0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        RecompOne.Runtime.Gte.MvmvaOp(12, false, 0, 0, 3);
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.V0 = mem.ReadU32((c.SP + 0x2Cu));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S2 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 + 0x16u), (ushort)c.V1);
        c.S6 = mem.ReadU32((c.SP + 0x4Cu));
        { var _sw = RecompOne.Runtime.Gte.Read(25); mem.WriteU32(c.S6, _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(26); mem.WriteU32((c.S6 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(27); mem.WriteU32((c.S6 + 0x8u), _sw); }
        c.V1 = mem.ReadU32((c.SP + 0x28u));
        c.V0 = mem.ReadU32((c.SP + 0x2Cu));
        c.V1 = (uint)((int)c.V1 >> 6);
        c.V1 = c.S3 + c.V1;
        c.V0 = 0u - c.V0;
        c.V0 = (uint)((int)c.V0 >> 6);
        c.V0 = c.S2 + c.V0;
        c.V0 = c.V0 << 8;
        c.V1 = c.V1 + c.V0;
        mem.WriteU16((c.A3 + 0xAu), (ushort)c.V1);
        c.S7 = 0x1F800000u;
        c.S7 = mem.ReadU32((c.S7 + 0x34u));
        c.T4 = mem.ReadU32(c.S7);
        c.T5 = mem.ReadU32((c.S7 + 0x4u));
        RecompOne.Runtime.Gte.WriteControl(0, c.T4);
        RecompOne.Runtime.Gte.WriteControl(1, c.T5);
        c.T4 = mem.ReadU32((c.S7 + 0x8u));
        c.T5 = mem.ReadU32((c.S7 + 0xCu));
        c.T6 = mem.ReadU32((c.S7 + 0x10u));
        RecompOne.Runtime.Gte.WriteControl(2, c.T4);
        RecompOne.Runtime.Gte.WriteControl(3, c.T5);
        RecompOne.Runtime.Gte.WriteControl(4, c.T6);
        c.S0 = mem.ReadU32((c.SP + 0x90u));
        c.V0 = c.S0 & 0x0020u;
        if (c.V0 != 0u) {
            c.S7 = 0x00000006u;
            goto L800B9984;
        }
        c.S7 = 0x00000006u;
        c.S6 = mem.ReadU32((c.SP + 0x48u));
        if (c.S6 != c.S7) {
            goto L800B97C8;
        }
        if (c.T3 == 0u) {
            c.A0 = c.SP + 0x8u;
            goto L800B97C8;
        }
        c.A0 = c.SP + 0x8u;
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x8u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xAu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x4u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x10u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x12u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x14u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x18u), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Au), (ushort)c.V0);
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x1Cu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16(c.V0);
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x20u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x2u));
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x22u), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.V0 = c.V0 << 3;
        c.V0 = c.V0 + c.T2;
        c.V0 = mem.ReadU16((c.V0 + 0x4u));
        c.V1 = c.SP + 0x10u;
        c.V0 = 0u - c.V0;
        mem.WriteU16((c.SP + 0x24u), (ushort)c.V0);
        c.V0 = c.SP + 0x20u;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        goto L800B9804;
        L800B97C8: ;
        c.A0 = mem.ReadU16((c.A2 - 0x4u));
        c.V1 = mem.ReadU16((c.A2 - 0x2u));
        c.V0 = mem.ReadU16((c.A2 + 0x2u));
        c.A0 = c.A0 << 3;
        c.A0 = c.T2 + c.A0;
        c.V1 = c.V1 << 3;
        c.V1 = c.T2 + c.V1;
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        { var _lw = mem.ReadU32(c.A0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.A0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        { var _lw = mem.ReadU32(c.V1); RecompOne.Runtime.Gte.Write(2, _lw); }
        { var _lw = mem.ReadU32((c.V1 + 0x4u)); RecompOne.Runtime.Gte.Write(3, _lw); }
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(4, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(5, _lw); }
        L800B9804: ;
        c.S0 = mem.ReadU32((c.SP + 0x8Cu));
        if (c.S0 == 0u) {
            goto L800B98C8;
        }
        RecompOne.Runtime.Gte.NcdtOp(12, true);
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T7;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S4;
        c.V0 = c.V0 & c.A1;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T7;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.A1;
        c.V0 = c.V0 & c.S4;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T1 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T1 + 0x1Cu), _sw); }
        c.S6 = mem.ReadU32((c.SP + 0x48u));
        c.S7 = 0x00000006u;
        if (c.S6 != c.S7) {
            goto L800B989C;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B989C;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B98B4;
        L800B989C: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B98B4: ;
        RecompOne.Runtime.Gte.NcdsOp(12, true);
        c.V0 = c.T1 + 0x28u;
        goto L800B9978;
        L800B98C8: ;
        RecompOne.Runtime.Gte.NcctOp(12, true);
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T7;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S4;
        c.V0 = c.V0 & c.A1;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T7;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.A1;
        c.V0 = c.V0 & c.S4;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        { var _sw = RecompOne.Runtime.Gte.Read(20); mem.WriteU32((c.T1 + 0x4u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(21); mem.WriteU32((c.T1 + 0x10u), _sw); }
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32((c.T1 + 0x1Cu), _sw); }
        c.S0 = mem.ReadU32((c.SP + 0x48u));
        c.S6 = 0x00000006u;
        if (c.S0 != c.S6) {
            goto L800B9950;
        }
        if (c.T3 == 0u) {
            c.V0 = c.SP + 0x18u;
            goto L800B9950;
        }
        c.V0 = c.SP + 0x18u;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        goto L800B9968;
        L800B9950: ;
        c.V0 = mem.ReadU16(c.A2);
        c.V0 = c.V0 << 3;
        c.V0 = c.T2 + c.V0;
        { var _lw = mem.ReadU32(c.V0); RecompOne.Runtime.Gte.Write(0, _lw); }
        { var _lw = mem.ReadU32((c.V0 + 0x4u)); RecompOne.Runtime.Gte.Write(1, _lw); }
        L800B9968: ;
        RecompOne.Runtime.Gte.NccsOp(12, true);
        c.V0 = c.T1 + 0x28u;
        L800B9978: ;
        { var _sw = RecompOne.Runtime.Gte.Read(22); mem.WriteU32(c.V0, _sw); }
        c.V0 = 0x0000000Cu;
        goto L800B9A00;
        L800B9984: ;
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0x16u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 - 0xAu), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0x2u), c.V0);
        c.V0 = mem.ReadU32(c.SP);
        mem.WriteU32((c.A3 + 0xEu), c.V0);
        c.V0 = mem.ReadU32((c.T0 + 0x38u));
        c.V1 = mem.ReadU32(c.T1);
        c.V0 = c.V0 << 2;
        c.V0 = c.V0 + c.T7;
        c.V0 = mem.ReadU32(c.V0);
        c.V1 = c.V1 & c.S4;
        c.V0 = c.V0 & c.A1;
        c.V1 = c.V1 | c.V0;
        mem.WriteU32(c.T1, c.V1);
        c.A0 = mem.ReadU32((c.T0 + 0x38u));
        c.A0 = c.A0 << 2;
        c.A0 = c.A0 + c.T7;
        c.V0 = mem.ReadU32(c.A0);
        c.V1 = c.T1 & c.A1;
        c.V0 = c.V0 & c.S4;
        c.V0 = c.V0 | c.V1;
        mem.WriteU32(c.A0, c.V0);
        c.V0 = 0x0000000Cu;
        L800B9A00: ;
        mem.WriteU8((c.A3 - 0x17u), (byte)c.V0);
        c.V1 = mem.ReadU32((c.GP + RAddr.Game_800B9A04));
        c.V0 = mem.ReadU16((c.V1 + 0x6u));
        mem.WriteU16((c.A3 - 0xCu), (ushort)c.V0);
        c.V0 = mem.ReadU16((c.V1 + 0xAu));
        c.T1 = c.T1 + 0x34u;
        mem.WriteU16(c.A3, (ushort)c.V0);
        c.A3 = c.A3 + 0x34u;
        L800B9A28: ;
        c.A2 = c.A2 + 0x1Cu;
        c.T8 = c.T8 + 0x1Cu;
        c.V1 = c.T9;
        c.V0 = mem.ReadU32((c.T0 + 0x44u));
        c.T9 = c.T9 - 0x1u;
        c.V0 = c.V0 - 0x1u;
        if (c.V1 != 0u) {
            mem.WriteU32((c.T0 + 0x44u), c.V0);
            goto L800B91D8;
        }
        mem.WriteU32((c.T0 + 0x44u), c.V0);
        L800B9A48: ;
        mem.WriteU32((c.T0 + 0x40u), c.T8);
        c.FP = mem.ReadU32((c.SP + 0x70u));
        c.S7 = mem.ReadU32((c.SP + 0x6Cu));
        c.S6 = mem.ReadU32((c.SP + 0x68u));
        c.S5 = mem.ReadU32((c.SP + 0x64u));
        c.S4 = mem.ReadU32((c.SP + 0x60u));
        c.S3 = mem.ReadU32((c.SP + 0x5Cu));
        c.S2 = mem.ReadU32((c.SP + 0x58u));
        c.S1 = mem.ReadU32((c.SP + 0x54u));
        c.S0 = mem.ReadU32((c.SP + 0x50u));
        c.At = 0x1F800000u;
        mem.WriteU32((c.At + 0x10u), c.T1);
        c.SP = c.SP + 0x78u;
        return;
    }
}
