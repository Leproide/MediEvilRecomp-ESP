// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp - tabella degli indirizzi per versione (ITA / ESP)
// GENERATO da tools/pal-port/version_port.py gen-cs a partire da config/versions.json:
// non modificare a mano, modifica la tabella e rigenera.
// Author: https://github.com/Leproide
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version. See <https://www.gnu.org/licenses/>.
//
// La versione si sceglie in compilazione (-p:Lingua=es definisce LINGUA_ES).
// Senza simboli si compila la versione italiana con gli stessi valori di prima.

namespace Recompiled;

/// <summary>Versione del gioco per cui e' compilato l'eseguibile.</summary>
public static class GameVersion
{
#if LINGUA_ES
    public const string Code = "esp";
    public const string Serial = "SCES-01495";
    /// <summary>eseguibile di avvio indicato da SYSTEM.CNF</summary>
    public const string BootFile = "SCES_014.95";
    /// <summary>titolo della finestra</summary>
    public const string Title = "MediEvilRecomp ESP";
    /// <summary>repository GitHub da cui scaricare gli aggiornamenti</summary>
    public const string UpdateRepo = "Leproide/MediEvilRecomp-ESP";
    public const bool Spanish = true;
#else
    public const string Code = "ita";
    public const string Serial = "SCES-01494";
    /// <summary>eseguibile di avvio indicato da SYSTEM.CNF</summary>
    public const string BootFile = "SCES_014.94";
    /// <summary>titolo della finestra</summary>
    public const string Title = "MediEvilRecomp";
    /// <summary>repository GitHub da cui scaricare gli aggiornamenti</summary>
    public const string UpdateRepo = "Leproide/MediEvilRecomp-ITA";
    public const bool Spanish = false;
#endif
}

/// <summary>Indirizzi e offset $gp usati dalle patch scritte a mano.</summary>
public static class Addr
{
    /// <summary>CD_cbsync, callback "sync" di libcd dentro MEDIEVIL.EXE (data)</summary>
    public const uint CdCbSync = GameVersion.Spanish ? 0x800EB6E0u : 0x800EB660u;
    /// <summary>CD_cbready, callback "ready" di libcd dentro MEDIEVIL.EXE (data)</summary>
    public const uint CdCbReady = GameVersion.Spanish ? 0x800EB6E4u : 0x800EB664u;
    /// <summary>modalita' "emulate" di StUnSetRing (1 = non toccare i callback) (data)</summary>
    public const uint CdEmulateFlag = GameVersion.Spanish ? 0x800EB700u : 0x800EB680u;
    /// <summary>struttura del lettore filmati (+0x40 = filmato finito) (data)</summary>
    public const uint MoviePlayerState = GameVersion.Spanish ? 0x800F1A28u : 0x800F19B0u;
    /// <summary>descrittore del filmato corrente (gp)</summary>
    public const uint GpMovieInfo = GameVersion.Spanish ? 0x75Cu : 0x764u;
    /// <summary>ultimo fotogramma letto (gp)</summary>
    public const uint GpMovieFrame = GameVersion.Spanish ? 0x758u : 0x760u;
    /// <summary>fotogramma precedente (gp)</summary>
    public const uint GpMoviePrevFrame = GameVersion.Spanish ? 0x760u : 0x768u;
    /// <summary>struttura FreeTerrData (buffer delle primitive del terreno) (data)</summary>
    public const uint FreeTerrData = GameVersion.Spanish ? 0x800EF09Cu : 0x800EF024u;
    /// <summary>tabella della nebbia del terreno (data)</summary>
    public const uint TerrainFog = GameVersion.Spanish ? 0x800F1A94u : 0x800F1A1Cu;
    /// <summary>vettori dei piani di vista (4 SVECTOR) (data)</summary>
    public const uint ViewPlaneSvecs = GameVersion.Spanish ? 0x800EECE0u : 0x800EEC68u;
    /// <summary>livello corrente (data)</summary>
    public const uint CurrentLevel = GameVersion.Spanish ? 0x800EF108u : 0x800EF090u;
    /// <summary>mappa corrente (data)</summary>
    public const uint CurrentMap = GameVersion.Spanish ? 0x800EF158u : 0x800EF0E0u;
    /// <summary>distanza di vista del terreno (data)</summary>
    public const uint ViewDistance = GameVersion.Spanish ? 0x800EF0C0u : 0x800EF048u;
    /// <summary>puntatore al viewport principale (gp)</summary>
    public const uint GpMainViewPort = GameVersion.Spanish ? 0x5C8u : 0x5D0u;
    /// <summary>bersaglio della telecamera (gp)</summary>
    public const uint GpCameraTarget = GameVersion.Spanish ? 0x548u : 0x550u;
    /// <summary>profondita' di proiezione (gp)</summary>
    public const uint GpProjDepth = GameVersion.Spanish ? 0x598u : 0x5A0u;
    /// <summary>punto base della griglia (gp)</summary>
    public const uint GpBasePoint = GameVersion.Spanish ? 0x59Cu : 0x5A4u;
    /// <summary>mondo della schermata del titolo (gp)</summary>
    public const uint GpTitleWorld = GameVersion.Spanish ? 0x630u : 0x638u;
    /// <summary>variabile della griglia (candidata 0) (gp)</summary>
    public const uint GpGridSlot0 = GameVersion.Spanish ? 0x594u : 0x59Cu;
    /// <summary>variabile della griglia (candidata 1) (gp)</summary>
    public const uint GpGridSlot1 = GameVersion.Spanish ? 0x5A4u : 0x5ACu;
    /// <summary>variabile della griglia (candidata 2) (gp)</summary>
    public const uint GpGridSlot2 = GameVersion.Spanish ? 0x5A8u : 0x5B0u;
    /// <summary>variabile della griglia (candidata 3) (gp)</summary>
    public const uint GpGridSlot3 = GameVersion.Spanish ? 0x5ACu : 0x5B4u;
    /// <summary>variabile della griglia (candidata 4) (gp)</summary>
    public const uint GpGridSlot4 = GameVersion.Spanish ? 0x5B0u : 0x5B8u;
    /// <summary>stato dei trucchi, u16 (mod Trucchi) (data)</summary>
    public const uint CheatState = GameVersion.Spanish ? 0x800EF11Au : 0x800EF0A2u;
    /// <summary>tabella "DUST TO DUST", primo tasto Giu' (mod Trucchi) (data)</summary>
    public const uint CheatTableDust = GameVersion.Spanish ? 0x800CE09Cu : 0x800CE0B4u;
    /// <summary>tabella trucchi estesa, primo tasto Triangolo (mod Trucchi) (data)</summary>
    public const uint CheatTableExtended = GameVersion.Spanish ? 0x800CE0C4u : 0x800CE0DCu;
    /// <summary>LibCrypt: chiave a 16 bit (func_80065FC4 la usa per decifrare il Mausoleo) (data)</summary>
    public const uint LcKey = GameVersion.Spanish ? 0x800DB32Cu : 0x800DB2ACu;
    /// <summary>LibCrypt: numero di bit a 1 (data)</summary>
    public const uint LcCount = GameVersion.Spanish ? 0x800DB330u : 0x800DB2B0u;
    /// <summary>LibCrypt: puntatore alla voce corrente della tabella dei settori (data)</summary>
    public const uint LcTablePtr = GameVersion.Spanish ? 0x800DB3B4u : 0x800DB334u;
    /// <summary>LibCrypt: tabella dei settori (voci da 4 byte BCD: mm, ss, settore di partenza, fotogramma bersaglio) (data)</summary>
    public const uint LcTableBase = GameVersion.Spanish ? 0x800DB334u : 0x800DB2B4u;
    /// <summary>LibCrypt: controllo avviato dalla mappa (0/1/2), scritto da LANDMAP (data)</summary>
    public const uint LcState = GameVersion.Spanish ? 0x800EDA8Cu : 0x800EDA14u;
    /// <summary>LibCrypt: jal 0x80065F8C in LANDMAP che avvia il controllo (code)</summary>
    public const uint LcLandmapCall = GameVersion.Spanish ? 0x80011E98u : 0x80011EA0u;
    /// <summary>tabella delle risorse di MEDRES.MWD (voci da 32 byte; la 0xD6 e' il Mausoleo cifrato) (data)</summary>
    public const uint MwdTable = GameVersion.Spanish ? 0x800CE5C0u : 0x800CE5D8u;
}

/// <summary>Immediati delle copie di funzioni ricompilate (CullPatch,
/// EntityFloatFunctions, LandMapPatch). Il nome e' l'istruzione italiana che
/// li produce (Overlay_Indirizzo) oppure, per gli indirizzi di codice,
/// Overlay_Code_IndirizzoItaliano.</summary>
public static class RAddr
{
    public const uint Game_80021DA4 = GameVersion.Spanish ? 0xF09Cu : 0xF024u; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DA4
    public const uint Game_80021DAC = GameVersion.Spanish ? 0xE070u : 0xDFF8u; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DAC
    public const uint Game_80021DB0 = GameVersion.Spanish ? 0x1A94u : 0x1A1Cu; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DB0
    public const uint Game_80021DB4 = GameVersion.Spanish ? 0xDE58u : 0xDDE0u; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DB4
    public const uint Game_80021DB8 = GameVersion.Spanish ? 0xDE54u : 0xDDDCu; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DB8
    public const uint Game_80024B08 = GameVersion.Spanish ? 0x0448u : 0x0450u; // render/CullPatch.cs:func_80024B00, istr. ESP 80024B08
    public const uint Game_80025120 = GameVersion.Spanish ? 0x0F38u : 0x0FB0u; // render/CullPatch.cs:func_80024B00, istr. ESP 80025120
    public const uint Game_80025294 = GameVersion.Spanish ? 0x05C8u : 0x05D0u; // render/CullPatch.cs:func_80024B00, istr. ESP 80025294
    public const uint Game_80025298 = GameVersion.Spanish ? 0x0F44u : 0x0FBCu; // render/CullPatch.cs:func_80024B00, istr. ESP 80025298
    public const uint Game_800252D8 = GameVersion.Spanish ? 0x0448u : 0x0450u; // render/CullPatch.cs:func_800252D0, istr. ESP 800252D8
    public const uint Game_80025690 = GameVersion.Spanish ? 0x0E14u : 0x0E8Cu; // render/CullPatch.cs:func_800252D0, istr. ESP 80025690
    public const uint Game_800258B0 = GameVersion.Spanish ? 0x05C8u : 0x05D0u; // render/CullPatch.cs:func_800252D0, istr. ESP 800258B0
    public const uint Game_8009A6C0 = GameVersion.Spanish ? 0x2360u : 0x23D8u; // render/CullPatch.cs:func_8009A694, istr. ESP 8009A6C0
    public const uint Game_800A10E8 = GameVersion.Spanish ? 0x1F28u : 0x1FA0u; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A10E8
    public const uint Game_800A13D8 = GameVersion.Spanish ? 0x2360u : 0x23D8u; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A13D8
    public const uint Game_800A1AA0 = GameVersion.Spanish ? 0x3B30u : 0x3AB8u; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A1AA0
    public const uint Game_800A1D04 = GameVersion.Spanish ? 0x6DC4u : 0x6DDCu; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A1D04
    public const uint Game_800B37A8 = GameVersion.Spanish ? 0x78C4u : 0x78DCu; // render/CullPatch.cs:func_800B3748, istr. ESP 800B37A8
    public const uint Game_800B37FC = GameVersion.Spanish ? 0x07A8u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B37FC
    public const uint Game_800B39B8 = GameVersion.Spanish ? 0x07A8u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B39B8
    public const uint Game_800B3BF8 = GameVersion.Spanish ? 0x07A8u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B3BF8
    public const uint Game_800B3DB4 = GameVersion.Spanish ? 0x07A8u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B3DB4
    public const uint Game_800B4064 = GameVersion.Spanish ? 0x7904u : 0x791Cu; // render/CullPatch.cs:func_800B3748, istr. ESP 800B4064
    public const uint Game_800B4160 = GameVersion.Spanish ? 0x07A8u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B4160
    public const uint Game_800B425C = GameVersion.Spanish ? 0x1F28u : 0x1FA0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B425C
    public const uint Game_800B4410 = GameVersion.Spanish ? 0x2360u : 0x23D8u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B4410
    public const uint Game_800B49D0 = GameVersion.Spanish ? 0x7944u : 0x795Cu; // render/CullPatch.cs:func_800B3748, istr. ESP 800B49D0
    public const uint Game_800B4E1C = GameVersion.Spanish ? 0x2C38u : 0x2CB8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E1C
    public const uint Game_800B4E24 = GameVersion.Spanish ? 0x2C24u : 0x2CA4u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E24
    public const uint Game_800B4E34 = GameVersion.Spanish ? 0x6D9Cu : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E34
    public const uint Game_800B4E44 = GameVersion.Spanish ? 0x2C70u : 0x2CF0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E44
    public const uint Game_800B4E54 = GameVersion.Spanish ? 0x2C6Cu : 0x2CECu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E54
    public const uint Game_800B4E64 = GameVersion.Spanish ? 0x2C68u : 0x2CE8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E64
    public const uint Game_800B4E74 = GameVersion.Spanish ? 0x2C64u : 0x2CE4u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E74
    public const uint Game_800B4E84 = GameVersion.Spanish ? 0x2C60u : 0x2CE0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E84
    public const uint Game_800B4E94 = GameVersion.Spanish ? 0x2C5Cu : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E94
    public const uint Game_800B4EA8 = GameVersion.Spanish ? 0x2C3Cu : 0x2CBCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4EA8
    public const uint Game_800B4EB0 = GameVersion.Spanish ? 0x2C4Cu : 0x2CCCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4EB0
    public const uint Game_800B4ECC = GameVersion.Spanish ? 0x6D54u : 0x6CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4ECC
    public const uint Game_800B4ED4 = GameVersion.Spanish ? 0x6D0Cu : 0x6C94u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4ED4
    public const uint Game_800B4F44 = GameVersion.Spanish ? 0x2C38u : 0x2CB8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4F44
    public const uint Game_800B4FD8 = GameVersion.Spanish ? 0x6D9Cu : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4FD8
    public const uint Game_800B4FEC = GameVersion.Spanish ? 0x2C18u : 0x2C98u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4FEC
    public const uint Game_800B4FFC = GameVersion.Spanish ? 0x2C30u : 0x2CB0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4FFC
    public const uint Game_800B5040 = GameVersion.Spanish ? 0x35A8u : 0x3628u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5040
    public const uint Game_800B5048 = GameVersion.Spanish ? 0x2C30u : 0x2CB0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5048
    public const uint Game_800B5050 = GameVersion.Spanish ? 0x2C2Cu : 0x2CACu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5050
    public const uint Game_800B505C = GameVersion.Spanish ? 0x2C1Cu : 0x2C9Cu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B505C
    public const uint Game_800B5068 = GameVersion.Spanish ? 0x2C30u : 0x2CB0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5068
    public const uint Game_800B5278 = GameVersion.Spanish ? 0x2C28u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5278
    public const uint Game_800B5298 = GameVersion.Spanish ? 0x2C28u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5298
    public const uint Game_800B5378 = GameVersion.Spanish ? 0x2C28u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5378
    public const uint Game_800B539C = GameVersion.Spanish ? 0x2C28u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B539C
    public const uint Game_800B53B0 = GameVersion.Spanish ? 0x2C28u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B53B0
    public const uint Game_800B5410 = GameVersion.Spanish ? 0x6D9Cu : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5410
    public const uint Game_800B5440 = GameVersion.Spanish ? 0x6D9Cu : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5440
    public const uint Game_800B5484 = GameVersion.Spanish ? 0x2C5Cu : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5484
    public const uint Game_800B5550 = GameVersion.Spanish ? 0x2C5Cu : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5550
    public const uint Game_800B5648 = GameVersion.Spanish ? 0x2C5Cu : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5648
    public const uint Game_800B56B4 = GameVersion.Spanish ? 0x7984u : 0x799Cu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B56B4
    public const uint Game_800B6F60 = GameVersion.Spanish ? 0x1F8Cu : 0x2004u; // render/EntityFloatFunctions.cs:func_800B6EEC, istr. ESP 800B6F60
    public const uint Game_800B7234 = GameVersion.Spanish ? 0x1F8Cu : 0x2004u; // render/EntityFloatFunctions.cs:func_800B6EEC, istr. ESP 800B7234
    public const uint Game_800B7314 = GameVersion.Spanish ? 0x1F8Cu : 0x2004u; // render/EntityFloatFunctions.cs:func_800B72A0, istr. ESP 800B7314
    public const uint Game_800B76CC = GameVersion.Spanish ? 0x1F8Cu : 0x2004u; // render/EntityFloatFunctions.cs:func_800B72A0, istr. ESP 800B76CC
    public const uint Game_800B8A64 = GameVersion.Spanish ? 0x07ACu : 0x07B4u; // render/EntityFloatFunctions.cs:func_800B8A24, istr. ESP 800B8A64
    public const uint Game_800B906C = GameVersion.Spanish ? 0x07ACu : 0x07B4u; // render/EntityFloatFunctions.cs:func_800B8A24, istr. ESP 800B906C
    public const uint Game_800B9128 = GameVersion.Spanish ? 0x07ACu : 0x07B4u; // render/EntityFloatFunctions.cs:func_800B90E8, istr. ESP 800B9128
    public const uint Game_800B9A04 = GameVersion.Spanish ? 0x07ACu : 0x07B4u; // render/EntityFloatFunctions.cs:func_800B90E8, istr. ESP 800B9A04
    public const uint Landmap_80012BC4 = GameVersion.Spanish ? 0x4EC0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80012BBC, istr. ESP 80012BBC
    public const uint Landmap_80012BE4 = GameVersion.Spanish ? 0x4528u : 0x4530u; // widescreen/LandMapPatch.cs:func_80012BBC, istr. ESP 80012BDC
    public const uint Landmap_80012F88 = GameVersion.Spanish ? 0x14D4u : 0x1454u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80012F80
    public const uint Landmap_80012FD8 = GameVersion.Spanish ? 0x4EB0u : 0x4EB8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80012FD0
    public const uint Landmap_80012FE4 = GameVersion.Spanish ? 0x4EC0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80012FDC
    public const uint Landmap_80013274 = GameVersion.Spanish ? 0x213Cu : 0x21B4u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 8001326C
    public const uint Landmap_80013284 = GameVersion.Spanish ? 0x4EB0u : 0x4EB8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 8001327C
    public const uint Landmap_8001328C = GameVersion.Spanish ? 0x21B0u : 0x2228u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80013284
    public const uint Landmap_800132D0 = GameVersion.Spanish ? 0x4EC0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 800132C8
    public const uint Landmap_80013438 = GameVersion.Spanish ? 0x0F08u : 0x0F80u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80013430
    public const uint Landmap_80013C40 = GameVersion.Spanish ? 0x4EB0u : 0x4EB8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013C38
    public const uint Landmap_80013C78 = GameVersion.Spanish ? 0x4EC0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013C70
    public const uint Landmap_80013C90 = GameVersion.Spanish ? 0x4EC0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013C88
    public const uint Landmap_80013CE8 = GameVersion.Spanish ? 0x0F08u : 0x0F80u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013CE0
    public const uint Landmap_80013DD4 = GameVersion.Spanish ? 0x14D4u : 0x1454u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013DCC
    public const uint Landmap_80013E04 = GameVersion.Spanish ? 0x14D4u : 0x1454u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013DFC
    public const uint Landmap_80013E58 = GameVersion.Spanish ? 0x14D4u : 0x1454u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013E50
    public const uint Landmap_80013EB0 = GameVersion.Spanish ? 0x4EC0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013EA8
    public const uint Landmap_Code_80012FDC = GameVersion.Spanish ? 0x80012FD4u : 0x80012FDCu; // widescreen/LandMapPatch.cs:func_80013328_landmap
    public const uint Landmap_Code_80013328 = GameVersion.Spanish ? 0x80013320u : 0x80013328u; // widescreen/LandMapPatch.cs:func_80012FDC_landmap
    public const uint Landmap_Code_80013510 = GameVersion.Spanish ? 0x80013508u : 0x80013510u; // widescreen/LandMapPatch.cs:func_80012F70
    public const uint Landmap_Code_80013558 = GameVersion.Spanish ? 0x80013550u : 0x80013558u; // widescreen/LandMapPatch.cs:func_80012F70
    public const uint Landmap_Code_800135A8 = GameVersion.Spanish ? 0x800135A0u : 0x800135A8u; // widescreen/LandMapPatch.cs:func_80012F70
    public const uint Landmap_Code_80013778 = GameVersion.Spanish ? 0x80013770u : 0x80013778u; // widescreen/LandMapPatch.cs:func_80012FDC_landmap
    public const uint Landmap_Code_80013798 = GameVersion.Spanish ? 0x80013790u : 0x80013798u; // widescreen/LandMapPatch.cs:func_80012FDC_landmap
    public const uint Landmap_Code_80013D88 = GameVersion.Spanish ? 0x80013D80u : 0x80013D88u; // widescreen/LandMapPatch.cs:func_80013C8C
    public const uint Landmap_Code_80013D90 = GameVersion.Spanish ? 0x80013D88u : 0x80013D90u; // widescreen/LandMapPatch.cs:func_80013C8C
    public const uint Landmap_Code_80013D98 = GameVersion.Spanish ? 0x80013D90u : 0x80013D98u; // widescreen/LandMapPatch.cs:func_80013C3C
    public const uint Landmap_Code_80013DA0 = GameVersion.Spanish ? 0x80013D98u : 0x80013DA0u; // widescreen/LandMapPatch.cs:func_80013C3C
    public const uint Landmap_Code_80013DBC = GameVersion.Spanish ? 0x80013DB4u : 0x80013DBCu; // widescreen/LandMapPatch.cs:func_80013C3C
    public const uint Landmap_Code_80013FF0 = GameVersion.Spanish ? 0x80013FE8u : 0x80013FF0u; // widescreen/LandMapPatch.cs:func_80013C8C
    public const uint Tl_8001097C = GameVersion.Spanish ? 0x2360u : 0x23D8u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010984
    public const uint Tl_80010988 = GameVersion.Spanish ? 0x2360u : 0x23D8u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010990
    public const uint Tl_800109EC = GameVersion.Spanish ? 0x0F40u : 0x0FB8u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 800109F4
    public const uint Tl_80010A1C = GameVersion.Spanish ? 0x7ED8u : 0x7EE0u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010A24
    public const uint Tl_80010A7C = GameVersion.Spanish ? 0x0EB4u : 0x0F2Cu; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010A84
    public const uint Tl_80010C3C = GameVersion.Spanish ? 0x14D4u : 0x1454u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010C44
    public const uint Tl_80010D90 = GameVersion.Spanish ? 0x0F08u : 0x0F80u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010D98
    public const uint Tl_80010E10 = GameVersion.Spanish ? 0x0F08u : 0x0F80u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010E18
    public const uint Tl_Code_800104BC = GameVersion.Spanish ? 0x800104C4u : 0x800104BCu; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_800105B8 = GameVersion.Spanish ? 0x800105C0u : 0x800105B8u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_8001084C = GameVersion.Spanish ? 0x80010854u : 0x8001084Cu; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_800108A0 = GameVersion.Spanish ? 0x800108A8u : 0x800108A0u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_80010A78 = GameVersion.Spanish ? 0x80010A80u : 0x80010A78u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_80010B14 = GameVersion.Spanish ? 0x80010B1Cu : 0x80010B14u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_80010F00 = GameVersion.Spanish ? 0x80010F08u : 0x80010F00u; // render/CullPatch.cs:func_80010800_tl
}
