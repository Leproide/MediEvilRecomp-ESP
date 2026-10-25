using RecompOne.Runtime.Memory;
using Recompiled;
using RecompOne.Runtime.Dispatch;

if (AutoUpdater.HandleRelaunch(args)) return 0;

var title = GameVersion.Title; // "MediEvilRecomp" nella build italiana (patches/GameAddresses.cs)

var asm = System.Reflection.Assembly.GetExecutingAssembly();

if (Array.Find(asm.GetManifestResourceNames(), n => n.EndsWith(".languages.json", StringComparison.OrdinalIgnoreCase)) is { } languagesRes)
    RecompOne.Runtime.Runtime.AddLanguages(asm, languagesRes);

if (Array.Find(asm.GetManifestResourceNames(), n => n.EndsWith(".MediEvilRecomp.ico", StringComparison.OrdinalIgnoreCase)) is { } iconRes)
{
    using var iconStream = asm.GetManifestResourceStream(iconRes)!;
    using var iconMem = new MemoryStream();
    iconStream.CopyTo(iconMem);
    RecompOne.Runtime.Runtime.SetIcon(iconMem.ToArray());
}


RecompOne.Runtime.Runtime.Defaults(cfg =>
{
    cfg.Default("BetterRendering", true);
});
RecompOne.Runtime.Pgxp.Pgxp.Supported = false;
RecompOne.Runtime.Hle.NativeGeometry.Enabled = false;

Dispatcher.Tolerant = true;

GameDisc.Register();
TerrainPatch.Register();
WidescreenPatch.Register();
WidescreenSettings.Register();
AutoUpdater.Register();

RecompOne.Runtime.Runtime.Run(() => Entry.Run(new PSMemory(0x00400000), args.Length > 0 ? args[0] : null, title));
return 0;
