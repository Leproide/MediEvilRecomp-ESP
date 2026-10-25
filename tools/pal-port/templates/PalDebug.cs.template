// SPDX-License-Identifier: GPL-3.0-or-later
// PalDebug - opt-in runtime tracing for the PAL port
// Author: https://github.com/Leproide
//
// Enables RecompOne's built-in log channels at startup, chosen with the
// MEDIEVIL_DEBUG environment variable (comma separated):
//   cd, irq, bios, sdk, vsync, gpu, dma, spu, mdec, all
// Example (PowerShell):
//   $env:MEDIEVIL_DEBUG = "cd,irq"; dotnet run > debug.txt 2>&1
// Nothing changes when the variable is not set.

using System.Runtime.CompilerServices;

namespace Recompiled;

internal static class PalDebug
{
    [ModuleInitializer]
    internal static void Init()
    {
        var spec = Environment.GetEnvironmentVariable("MEDIEVIL_DEBUG");
        if (string.IsNullOrWhiteSpace(spec)) return;

        var on = new HashSet<string>(
            spec.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
            StringComparer.OrdinalIgnoreCase);
        bool all = on.Contains("all");

        RecompOne.Runtime.Log.CdOn = all || on.Contains("cd");
        RecompOne.Runtime.Log.IrqOn = all || on.Contains("irq");
        RecompOne.Runtime.Log.BiosOn = all || on.Contains("bios");
        RecompOne.Runtime.Log.SdkOn = all || on.Contains("sdk");
        RecompOne.Runtime.Log.VSyncOn = all || on.Contains("vsync");
        RecompOne.Runtime.Log.GpuOn = all || on.Contains("gpu");
        RecompOne.Runtime.Log.DmaOn = all || on.Contains("dma");
        RecompOne.Runtime.Log.SpuOn = all || on.Contains("spu");
        RecompOne.Runtime.Log.MdecOn = all || on.Contains("mdec");

        Console.WriteLine($"[PalDebug] log channels: {spec}");
    }
}
