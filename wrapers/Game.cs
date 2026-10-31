// Codice originale di MediEvilRecomp (BlackLabelHQ), adattato alla versione PAL italiana.
// MediEvilRecomp non ha una licenza esplicita: i diritti su questo codice restano ai suoi
// autori. Questo file NON e' rilasciato sotto GPL-3.0: la GPL del fork copre solo i file
// scritti da zero per il port (vedi la sezione Licenza del README).
// Indirizzi e costanti PAL: https://github.com/Leproide

using RecompOne.Runtime.Memory;

namespace MediEvil;

// PAL: offset rispetto a $gp tradotti a mano (tools/pal-port non li riconosce in questo
// file), tutti verificati come corrispondenze esatte USA -> PAL:
// 0x568->0x558, 0x5E0->0x5D0, 0x7F0->0x7E0, 0x470->0x460, 0x474->0x464, 0x484->0x474, 0x7C0->0x7B0
// Valori ITA/ESP in patches/GameAddresses.cs (config/versions.json): nella versione
// spagnola $gp vale 0x800ED8C8 e questi slot sono 8 byte piu' in basso.

public static class Game
{
    public const uint Gp = Recompiled.Addr.GpBase;

    public const uint CurrentLevelAddr = Recompiled.Addr.CurrentLevel;
    public const uint PlayerSlot = Recompiled.Addr.GpPlayer;
    public const uint MainViewportSlot = Recompiled.Addr.GpMainViewPort;
    public const uint ViewportRootSlot = Recompiled.Addr.GpViewportRoot;
    public const uint CameraSlot = Recompiled.Addr.GpCamera;
    public const uint CameraEntitySlot = Recompiled.Addr.GpCameraEntity;
    public const uint CameraPluginSlot = Recompiled.Addr.GpCameraPlugin;
    public const uint TextureListSlot = Recompiled.Addr.GpTextureList;
    public const uint TextureAttributesAddr = Recompiled.Addr.TextureAttributes;

    internal static IMemory M => RecompOne.Runtime.Runtime.Mem!;

    public static bool Available => RecompOne.Runtime.Runtime.Mem != null;

    public static uint GpWord(uint slot) => M.ReadU32(Gp + slot);

    public static int CurrentLevel => (int)M.ReadU32(CurrentLevelAddr);
    public static uint PlayerAddr => GpWord(PlayerSlot);
    public static uint MainViewportAddr => GpWord(MainViewportSlot);
    public static uint CameraAddr => GpWord(CameraSlot);
    public static uint CameraEntityAddr => GpWord(CameraEntitySlot);
    public static uint TextureListAddr => GpWord(TextureListSlot);

    public static byte TextureAttributes(int index) => M.ReadU8(TextureAttributesAddr + (uint)index);

    public static bool IsRam(uint address) => (address & 0xFF000000u) == 0x80000000u && (address & 0x00FFFFFFu) < RecompOne.Runtime.Runtime.RamSize;
}

[Flags]
public enum TextureAttribute : byte
{
    NotGround = 1 << 0,
    Water = 1 << 1,
    Mud = 1 << 2,
    DeadlyMud = 1 << 3,
    Corn = 1 << 4,
    Special1 = 1 << 5,
    Special2 = 1 << 6,
}
