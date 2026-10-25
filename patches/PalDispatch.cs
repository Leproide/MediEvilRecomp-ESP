// SPDX-License-Identifier: GPL-3.0-or-later
// PalDispatch - address based calls into recompiled overlays (PAL port)
// Author: https://github.com/Leproide
//
// RecompOne appends "_<overlay>" to a function name when the same address
// exists in more than one overlay, so the emitted name cannot be known before
// recompiling. Patches call overlay code through the dispatch tables instead.

using RecompOne.Runtime.Context;
using RecompOne.Runtime.Memory;

namespace Recompiled;

internal static class PalDispatch
{
    public static readonly IReadOnlyDictionary<uint, Action<CpuContext, IMemory>> Landmap = new LandmapDispatchTable().Functions;
    public static readonly IReadOnlyDictionary<uint, Action<CpuContext, IMemory>> Tl = new TlDispatchTable().Functions;
}
