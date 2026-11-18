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
    public const bool French = false;
#elif LINGUA_FR
    public const string Code = "fra";
    public const string Serial = "SCES-01492";
    /// <summary>eseguibile di avvio indicato da SYSTEM.CNF</summary>
    public const string BootFile = "SCES_014.92";
    /// <summary>titolo della finestra</summary>
    public const string Title = "MediEvilRecomp FRA";
    /// <summary>repository GitHub da cui scaricare gli aggiornamenti</summary>
    public const string UpdateRepo = "Leproide/MediEvilRecomp-FRA";
    public const bool Spanish = false;
    public const bool French = true;
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
    public const bool French = false;
#endif
}

/// <summary>Indirizzi e offset $gp usati dalle patch scritte a mano.</summary>
public static class Addr
{
    /// <summary>CD_cbsync, callback "sync" di libcd dentro MEDIEVIL.EXE (data)</summary>
    public const uint CdCbSync = GameVersion.Spanish ? 0x800EB6E0u : GameVersion.French ? 0x800EB7B8u : 0x800EB660u;
    /// <summary>CD_cbready, callback "ready" di libcd dentro MEDIEVIL.EXE (data)</summary>
    public const uint CdCbReady = GameVersion.Spanish ? 0x800EB6E4u : GameVersion.French ? 0x800EB7BCu : 0x800EB664u;
    /// <summary>modalita' "emulate" di StUnSetRing (1 = non toccare i callback) (data)</summary>
    public const uint CdEmulateFlag = GameVersion.Spanish ? 0x800EB700u : GameVersion.French ? 0x800EB7D8u : 0x800EB680u;
    /// <summary>struttura del lettore filmati (+0x40 = filmato finito) (data)</summary>
    public const uint MoviePlayerState = GameVersion.Spanish ? 0x800F1A28u : GameVersion.French ? 0x800F1AECu : 0x800F19B0u;
    /// <summary>descrittore del filmato corrente (gp)</summary>
    public const uint GpMovieInfo = GameVersion.Spanish ? 0x75Cu : GameVersion.French ? 0x748u : 0x764u;
    /// <summary>ultimo fotogramma letto (gp)</summary>
    public const uint GpMovieFrame = GameVersion.Spanish ? 0x758u : GameVersion.French ? 0x744u : 0x760u;
    /// <summary>fotogramma precedente (gp)</summary>
    public const uint GpMoviePrevFrame = GameVersion.Spanish ? 0x760u : GameVersion.French ? 0x74Cu : 0x768u;
    /// <summary>struttura FreeTerrData (buffer delle primitive del terreno) (data)</summary>
    public const uint FreeTerrData = GameVersion.Spanish ? 0x800EF09Cu : GameVersion.French ? 0x800EF160u : 0x800EF024u;
    /// <summary>tabella della nebbia del terreno (data)</summary>
    public const uint TerrainFog = GameVersion.Spanish ? 0x800F1A94u : GameVersion.French ? 0x800F1B58u : 0x800F1A1Cu;
    /// <summary>vettori dei piani di vista (4 SVECTOR) (data)</summary>
    public const uint ViewPlaneSvecs = GameVersion.Spanish ? 0x800EECE0u : GameVersion.French ? 0x800EEDA4u : 0x800EEC68u;
    /// <summary>livello corrente (data)</summary>
    public const uint CurrentLevel = GameVersion.Spanish ? 0x800EF108u : GameVersion.French ? 0x800EF1CCu : 0x800EF090u;
    /// <summary>mappa corrente (data)</summary>
    public const uint CurrentMap = GameVersion.Spanish ? 0x800EF158u : GameVersion.French ? 0x800EF21Cu : 0x800EF0E0u;
    /// <summary>distanza di vista del terreno (data)</summary>
    public const uint ViewDistance = GameVersion.Spanish ? 0x800EF0C0u : GameVersion.French ? 0x800EF184u : 0x800EF048u;
    /// <summary>puntatore al viewport principale (gp)</summary>
    public const uint GpMainViewPort = GameVersion.Spanish ? 0x5C8u : GameVersion.French ? 0x5B4u : 0x5D0u;
    /// <summary>bersaglio della telecamera (gp)</summary>
    public const uint GpCameraTarget = GameVersion.Spanish ? 0x548u : GameVersion.French ? 0x534u : 0x550u;
    /// <summary>profondita' di proiezione (gp)</summary>
    public const uint GpProjDepth = GameVersion.Spanish ? 0x598u : GameVersion.French ? 0x584u : 0x5A0u;
    /// <summary>punto base della griglia (gp)</summary>
    public const uint GpBasePoint = GameVersion.Spanish ? 0x59Cu : GameVersion.French ? 0x588u : 0x5A4u;
    /// <summary>mondo della schermata del titolo (gp)</summary>
    public const uint GpTitleWorld = GameVersion.Spanish ? 0x630u : GameVersion.French ? 0x61Cu : 0x638u;
    /// <summary>variabile della griglia (candidata 0) (gp)</summary>
    public const uint GpGridSlot0 = GameVersion.Spanish ? 0x594u : GameVersion.French ? 0x580u : 0x59Cu;
    /// <summary>variabile della griglia (candidata 1) (gp)</summary>
    public const uint GpGridSlot1 = GameVersion.Spanish ? 0x5A4u : GameVersion.French ? 0x590u : 0x5ACu;
    /// <summary>variabile della griglia (candidata 2) (gp)</summary>
    public const uint GpGridSlot2 = GameVersion.Spanish ? 0x5A8u : GameVersion.French ? 0x594u : 0x5B0u;
    /// <summary>variabile della griglia (candidata 3) (gp)</summary>
    public const uint GpGridSlot3 = GameVersion.Spanish ? 0x5ACu : GameVersion.French ? 0x598u : 0x5B4u;
    /// <summary>variabile della griglia (candidata 4) (gp)</summary>
    public const uint GpGridSlot4 = GameVersion.Spanish ? 0x5B0u : GameVersion.French ? 0x59Cu : 0x5B8u;
    /// <summary>stato dei trucchi, u16 (mod Trucchi) (data)</summary>
    public const uint CheatState = GameVersion.Spanish ? 0x800EF11Au : GameVersion.French ? 0x800EF1DEu : 0x800EF0A2u;
    /// <summary>tabella "DUST TO DUST", primo tasto Giu' (mod Trucchi) (data)</summary>
    public const uint CheatTableDust = GameVersion.Spanish ? 0x800CE09Cu : GameVersion.French ? 0x800CE0C4u : 0x800CE0B4u;
    /// <summary>tabella trucchi estesa, primo tasto Triangolo (mod Trucchi) (data)</summary>
    public const uint CheatTableExtended = GameVersion.Spanish ? 0x800CE0C4u : GameVersion.French ? 0x800CE0ECu : 0x800CE0DCu;
    /// <summary>LibCrypt: chiave a 16 bit (func_80065FC4 la usa per decifrare il Mausoleo) (data)</summary>
    public const uint LcKey = GameVersion.Spanish ? 0x800DB32Cu : GameVersion.French ? 0x800DB404u : 0x800DB2ACu;
    /// <summary>LibCrypt: numero di bit a 1 (data)</summary>
    public const uint LcCount = GameVersion.Spanish ? 0x800DB330u : GameVersion.French ? 0x800DB408u : 0x800DB2B0u;
    /// <summary>LibCrypt: puntatore alla voce corrente della tabella dei settori (data)</summary>
    public const uint LcTablePtr = GameVersion.Spanish ? 0x800DB3B4u : GameVersion.French ? 0x800DB48Cu : 0x800DB334u;
    /// <summary>LibCrypt: tabella dei settori (voci da 4 byte BCD: mm, ss, settore di partenza, fotogramma bersaglio) (data)</summary>
    public const uint LcTableBase = GameVersion.Spanish ? 0x800DB334u : GameVersion.French ? 0x800DB40Cu : 0x800DB2B4u;
    /// <summary>LibCrypt: controllo avviato dalla mappa (0/1/2), scritto da LANDMAP (data)</summary>
    public const uint LcState = GameVersion.Spanish ? 0x800EDA8Cu : GameVersion.French ? 0x800EDB50u : 0x800EDA14u;
    /// <summary>LibCrypt: jal 0x80065F8C in LANDMAP che avvia il controllo (code)</summary>
    public const uint LcLandmapCall = GameVersion.Spanish ? 0x80011E98u : GameVersion.French ? 0x80011E60u : 0x80011EA0u;
    /// <summary>tabella delle risorse di MEDRES.MWD (voci da 32 byte; la 0xD6 e' il Mausoleo cifrato) (data)</summary>
    public const uint MwdTable = GameVersion.Spanish ? 0x800CE5C0u : GameVersion.French ? 0x800CE5E8u : 0x800CE5D8u;
    /// <summary>valore di $gp in MEDIEVIL.EXE (wrapers/Game.cs) (data)</summary>
    public const uint GpBase = GameVersion.Spanish ? 0x800ED8C8u : GameVersion.French ? 0x800ED9A0u : 0x800ED848u;
    /// <summary>puntatore al giocatore (gp)</summary>
    public const uint GpPlayer = GameVersion.Spanish ? 0x550u : GameVersion.French ? 0x53Cu : 0x558u;
    /// <summary>radice della lista dei viewport (gp)</summary>
    public const uint GpViewportRoot = GameVersion.Spanish ? 0x7D8u : GameVersion.French ? 0x7C4u : 0x7E0u;
    /// <summary>dati della telecamera (gp)</summary>
    public const uint GpCamera = GameVersion.Spanish ? 0x458u : GameVersion.French ? 0x444u : 0x460u;
    /// <summary>entita' della telecamera (gp)</summary>
    public const uint GpCameraEntity = GameVersion.Spanish ? 0x45Cu : GameVersion.French ? 0x448u : 0x464u;
    /// <summary>plugin attivo della telecamera (gp)</summary>
    public const uint GpCameraPlugin = GameVersion.Spanish ? 0x46Cu : GameVersion.French ? 0x458u : 0x474u;
    /// <summary>lista delle texture (gp)</summary>
    public const uint GpTextureList = GameVersion.Spanish ? 0x7A8u : GameVersion.French ? 0x794u : 0x7B0u;
    /// <summary>dimensione del pool dei poligoni dinamici (gp)</summary>
    public const uint GpPolyPoolSize = GameVersion.Spanish ? 0x804u : GameVersion.French ? 0x7F0u : 0x80Cu;
    /// <summary>dimensione del buffer dei poligoni dinamici (gp)</summary>
    public const uint GpPolyBufferSize = GameVersion.Spanish ? 0x800u : GameVersion.French ? 0x7ECu : 0x808u;
    /// <summary>attributi delle texture (data)</summary>
    public const uint TextureAttributes = GameVersion.Spanish ? 0x800EDAA0u : GameVersion.French ? 0x800EDB64u : 0x800EDA28u;
    /// <summary>pool dei poligoni dinamici di MR (data)</summary>
    public const uint MrDynamicPools = GameVersion.Spanish ? 0x800EE0D0u : GameVersion.French ? 0x800EE194u : 0x800EE058u;
    /// <summary>fine dei pool dei poligoni dinamici di MR (data)</summary>
    public const uint MrDynamicPoolEnds = GameVersion.Spanish ? 0x800EE0D8u : GameVersion.French ? 0x800EE19Cu : 0x800EE060u;
    /// <summary>funzione MRAllocMem di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRAllocMem = GameVersion.Spanish ? 0x800A40B8u : GameVersion.French ? 0x800A4120u : 0x800A40B8u;
    /// <summary>funzione MRFreeMem di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRFreeMem = GameVersion.Spanish ? 0x800A4330u : GameVersion.French ? 0x800A4398u : 0x800A4330u;
    /// <summary>funzione MRInitialiseResources di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRInitialiseResources = GameVersion.Spanish ? 0x800A7EC8u : GameVersion.French ? 0x800A7F30u : 0x800A7EC8u;
    /// <summary>funzione MRLoadResource di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRLoadResource = GameVersion.Spanish ? 0x800A80C8u : GameVersion.French ? 0x800A8130u : 0x800A80C8u;
    /// <summary>funzione MRProcessResource di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRProcessResource = GameVersion.Spanish ? 0x800A845Cu : GameVersion.French ? 0x800A84C4u : 0x800A845Cu;
    /// <summary>funzione MRUnloadResource di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRUnloadResource = GameVersion.Spanish ? 0x800A85E0u : GameVersion.French ? 0x800A8648u : 0x800A85E0u;
    /// <summary>funzione PowerPackerDecrunch di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnPowerPackerDecrunch = GameVersion.Spanish ? 0x800BA550u : GameVersion.French ? 0x800BA5B8u : 0x800BA550u;
    /// <summary>funzione MRAllocateDynamicPolys di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRAllocateDynamicPolys = GameVersion.Spanish ? 0x800A483Cu : GameVersion.French ? 0x800A48A4u : 0x800A483Cu;
    /// <summary>funzione MRFreeDynamicPolys di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRFreeDynamicPolys = GameVersion.Spanish ? 0x800A48ACu : GameVersion.French ? 0x800A4914u : 0x800A48ACu;
    /// <summary>funzione MRRenderViewport di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRRenderViewport = GameVersion.Spanish ? 0x8009FDF4u : GameVersion.French ? 0x8009FE5Cu : 0x8009FDF4u;
    /// <summary>funzione MRKillViewport di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRKillViewport = GameVersion.Spanish ? 0x8009F370u : GameVersion.French ? 0x8009F3D8u : 0x8009F370u;
    /// <summary>funzione MRKillAllViewports di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRKillAllViewports = GameVersion.Spanish ? 0x8009BC78u : GameVersion.French ? 0x8009BCE0u : 0x8009BC78u;
    /// <summary>funzione MRUpdateViewportRenderMatrices di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRUpdateViewportRenderMatrices = GameVersion.Spanish ? 0x800A04FCu : GameVersion.French ? 0x800A0564u : 0x800A04FCu;
    /// <summary>funzione CamUpdate di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnCamUpdate = GameVersion.Spanish ? 0x8002D274u : GameVersion.French ? 0x8002D2DCu : 0x8002D274u;
    /// <summary>funzione FadeUpdate di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnFadeUpdate = GameVersion.Spanish ? 0x8004EB60u : GameVersion.French ? 0x8004EBC8u : 0x8004EB60u;
    /// <summary>funzione EntLibPointOccluded di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnEntLibPointOccluded = GameVersion.Spanish ? 0x800494C8u : GameVersion.French ? 0x80049530u : 0x800494C8u;
    /// <summary>funzione LevelStartUp di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnLevelStartUp = GameVersion.Spanish ? 0x80065618u : GameVersion.French ? 0x80065680u : 0x80065618u;
    /// <summary>funzione LevelShutDown di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnLevelShutDown = GameVersion.Spanish ? 0x800658BCu : GameVersion.French ? 0x80065924u : 0x800658BCu;
    /// <summary>funzione FreeTerrShutDown di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnFreeTerrShutDown = GameVersion.Spanish ? 0x8005153Cu : GameVersion.French ? 0x800515A4u : 0x8005153Cu;
    /// <summary>funzione TerrainFogShutdown di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnTerrainFogShutdown = GameVersion.Spanish ? 0x8007A140u : GameVersion.French ? 0x8007A1A8u : 0x8007A140u;
    /// <summary>lista degli oggetti attorno a Dan (DistantObjectsPatch) (data)</summary>
    public const uint ThingList = GameVersion.Spanish ? 0x800EEAC8u : GameVersion.French ? 0x800EEB8Cu : 0x800EEA50u;
    /// <summary>lista degli oggetti svegli, sentinella (DistantObjectsPatch) (data)</summary>
    public const uint ThingAwakeList = GameVersion.Spanish ? 0x800EEB60u : GameVersion.French ? 0x800EEC24u : 0x800EEAE8u;
    /// <summary>punto di riferimento per la distanza degli oggetti (DistantObjectsPatch) (data)</summary>
    public const uint ThingRefPoint = GameVersion.Spanish ? 0x800EEBF8u : GameVersion.French ? 0x800EECBCu : 0x800EEB80u;
    /// <summary>puntatore alle soglie sveglia/disattivazione degli oggetti (DistantObjectsPatch) (data)</summary>
    public const uint ThingRangePtr = GameVersion.Spanish ? 0x800EEC0Cu : GameVersion.French ? 0x800EECD0u : 0x800EEB94u;
    /// <summary>12 liste libere dell'allocatore del gioco, 0x14 byte l'una (HeapExpansion) (data)</summary>
    public const uint HeapFreeBins = GameVersion.Spanish ? 0x800F3B80u : GameVersion.French ? 0x800F3C44u : 0x800F3B08u;
    /// <summary>puntatore all'array dei poligoni del terreno (TerrainEdgeFade) (data)</summary>
    public const uint TerrainPolyArrayPtr = GameVersion.Spanish ? 0x800EDE54u : GameVersion.French ? 0x800EDF18u : 0x800EDDDCu;
    /// <summary>puntatore all'array dei vertici del terreno (TerrainEdgeFade) (data)</summary>
    public const uint TerrainVertexArrayPtr = GameVersion.Spanish ? 0x800EDE58u : GameVersion.French ? 0x800EDF1Cu : 0x800EDDE0u;
    /// <summary>tipo oggetto: runa sulla mano dei cancelli (DistantObjectsPatch); mappa solo "inferred": verificato a mano, struttura del tipo identica a parte i puntatori (spostati di -0x18 / +0x80 come il resto dei dati); FRA: +0x10, stessa verifica (24 parole su 24) (fixed)</summary>
    public const uint TypeRuneHand = GameVersion.Spanish ? 0x800D73A8u : GameVersion.French ? 0x800D73D0u : 0x800D73C0u;
    /// <summary>tipo oggetto: casse, anche esplosive (DistantObjectsPatch); mappa solo "inferred": verificato a mano, struttura del tipo identica a parte i puntatori (spostati di -0x18 / +0x80 come il resto dei dati); FRA: +0x10, stessa verifica (24 parole su 24) (fixed)</summary>
    public const uint TypeChest = GameVersion.Spanish ? 0x800D5274u : GameVersion.French ? 0x800D529Cu : 0x800D528Cu;
}

/// <summary>Immediati delle copie di funzioni ricompilate (CullPatch,
/// EntityFloatFunctions, LandMapPatch). Il nome e' l'istruzione italiana che
/// li produce (Overlay_Indirizzo) oppure, per gli indirizzi di codice,
/// Overlay_Code_IndirizzoItaliano.</summary>
public static class RAddr
{
    public const uint Game_80021DA4 = GameVersion.Spanish ? 0xF09Cu : GameVersion.French ? 0xF160u : 0xF024u; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DA4, istr. FRA 80021E0C
    public const uint Game_80021DAC = GameVersion.Spanish ? 0xE070u : GameVersion.French ? 0xE134u : 0xDFF8u; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DAC, istr. FRA 80021E14
    public const uint Game_80021DB0 = GameVersion.Spanish ? 0x1A94u : GameVersion.French ? 0x1B58u : 0x1A1Cu; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DB0, istr. FRA 80021E18
    public const uint Game_80021DB4 = GameVersion.Spanish ? 0xDE58u : GameVersion.French ? 0xDF1Cu : 0xDDE0u; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DB4, istr. FRA 80021E1C
    public const uint Game_80021DB8 = GameVersion.Spanish ? 0xDE54u : GameVersion.French ? 0xDF18u : 0xDDDCu; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DB8, istr. FRA 80021E20
    public const uint Game_80024B08 = GameVersion.Spanish ? 0x0448u : GameVersion.French ? 0x0434u : 0x0450u; // render/CullPatch.cs:func_80024B00, istr. ESP 80024B08, istr. FRA 80024B70
    public const uint Game_80025120 = GameVersion.Spanish ? 0x0F38u : GameVersion.French ? 0x0E74u : 0x0FB0u; // render/CullPatch.cs:func_80024B00, istr. ESP 80025120, istr. FRA 80025188
    public const uint Game_80025294 = GameVersion.Spanish ? 0x05C8u : GameVersion.French ? 0x05B4u : 0x05D0u; // render/CullPatch.cs:func_80024B00, istr. ESP 80025294, istr. FRA 800252FC
    public const uint Game_80025298 = GameVersion.Spanish ? 0x0F44u : GameVersion.French ? 0x0E80u : 0x0FBCu; // render/CullPatch.cs:func_80024B00, istr. ESP 80025298, istr. FRA 80025300
    public const uint Game_800252D8 = GameVersion.Spanish ? 0x0448u : GameVersion.French ? 0x0434u : 0x0450u; // render/CullPatch.cs:func_800252D0, istr. ESP 800252D8, istr. FRA 80025340
    public const uint Game_80025690 = GameVersion.Spanish ? 0x0E14u : GameVersion.French ? 0x0D50u : 0x0E8Cu; // render/CullPatch.cs:func_800252D0, istr. ESP 80025690, istr. FRA 800256F8
    public const uint Game_800258B0 = GameVersion.Spanish ? 0x05C8u : GameVersion.French ? 0x05B4u : 0x05D0u; // render/CullPatch.cs:func_800252D0, istr. ESP 800258B0, istr. FRA 80025918
    public const uint Game_8009A6C0 = GameVersion.Spanish ? 0x2360u : GameVersion.French ? 0x229Cu : 0x23D8u; // render/CullPatch.cs:func_8009A694, istr. ESP 8009A6C0, istr. FRA 8009A728
    public const uint Game_800A10E8 = GameVersion.Spanish ? 0x1F28u : GameVersion.French ? 0x1E64u : 0x1FA0u; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A10E8, istr. FRA 800A1150
    public const uint Game_800A13D8 = GameVersion.Spanish ? 0x2360u : GameVersion.French ? 0x229Cu : 0x23D8u; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A13D8, istr. FRA 800A1440
    public const uint Game_800A1AA0 = GameVersion.Spanish ? 0x3B30u : GameVersion.French ? 0x3BF4u : 0x3AB8u; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A1AA0, istr. FRA 800A1B08
    public const uint Game_800A1D04 = GameVersion.Spanish ? 0x6DC4u : GameVersion.French ? 0x6DECu : 0x6DDCu; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A1D04, istr. FRA 800A1D6C
    public const uint Game_800B37A8 = GameVersion.Spanish ? 0x78C4u : GameVersion.French ? 0x78ECu : 0x78DCu; // render/CullPatch.cs:func_800B3748, istr. ESP 800B37A8, istr. FRA 800B3810
    public const uint Game_800B37FC = GameVersion.Spanish ? 0x07A8u : GameVersion.French ? 0x0794u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B37FC, istr. FRA 800B3864
    public const uint Game_800B39B8 = GameVersion.Spanish ? 0x07A8u : GameVersion.French ? 0x0794u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B39B8, istr. FRA 800B3A20
    public const uint Game_800B3BF8 = GameVersion.Spanish ? 0x07A8u : GameVersion.French ? 0x0794u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B3BF8, istr. FRA 800B3C60
    public const uint Game_800B3DB4 = GameVersion.Spanish ? 0x07A8u : GameVersion.French ? 0x0794u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B3DB4, istr. FRA 800B3E1C
    public const uint Game_800B4064 = GameVersion.Spanish ? 0x7904u : GameVersion.French ? 0x792Cu : 0x791Cu; // render/CullPatch.cs:func_800B3748, istr. ESP 800B4064, istr. FRA 800B40CC
    public const uint Game_800B4160 = GameVersion.Spanish ? 0x07A8u : GameVersion.French ? 0x0794u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B4160, istr. FRA 800B41C8
    public const uint Game_800B425C = GameVersion.Spanish ? 0x1F28u : GameVersion.French ? 0x1E64u : 0x1FA0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B425C, istr. FRA 800B42C4
    public const uint Game_800B4410 = GameVersion.Spanish ? 0x2360u : GameVersion.French ? 0x229Cu : 0x23D8u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B4410, istr. FRA 800B4478
    public const uint Game_800B49D0 = GameVersion.Spanish ? 0x7944u : GameVersion.French ? 0x796Cu : 0x795Cu; // render/CullPatch.cs:func_800B3748, istr. ESP 800B49D0, istr. FRA 800B4A38
    public const uint Game_800B4E1C = GameVersion.Spanish ? 0x2C38u : GameVersion.French ? 0x2B60u : 0x2CB8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E1C, istr. FRA 800B4E84
    public const uint Game_800B4E24 = GameVersion.Spanish ? 0x2C24u : GameVersion.French ? 0x2B4Cu : 0x2CA4u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E24, istr. FRA 800B4E8C
    public const uint Game_800B4E34 = GameVersion.Spanish ? 0x6D9Cu : GameVersion.French ? 0x6E60u : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E34, istr. FRA 800B4E9C
    public const uint Game_800B4E44 = GameVersion.Spanish ? 0x2C70u : GameVersion.French ? 0x2B98u : 0x2CF0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E44, istr. FRA 800B4EAC
    public const uint Game_800B4E54 = GameVersion.Spanish ? 0x2C6Cu : GameVersion.French ? 0x2B94u : 0x2CECu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E54, istr. FRA 800B4EBC
    public const uint Game_800B4E64 = GameVersion.Spanish ? 0x2C68u : GameVersion.French ? 0x2B90u : 0x2CE8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E64, istr. FRA 800B4ECC
    public const uint Game_800B4E74 = GameVersion.Spanish ? 0x2C64u : GameVersion.French ? 0x2B8Cu : 0x2CE4u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E74, istr. FRA 800B4EDC
    public const uint Game_800B4E84 = GameVersion.Spanish ? 0x2C60u : GameVersion.French ? 0x2B88u : 0x2CE0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E84, istr. FRA 800B4EEC
    public const uint Game_800B4E94 = GameVersion.Spanish ? 0x2C5Cu : GameVersion.French ? 0x2B84u : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E94, istr. FRA 800B4EFC
    public const uint Game_800B4EA8 = GameVersion.Spanish ? 0x2C3Cu : GameVersion.French ? 0x2B64u : 0x2CBCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4EA8, istr. FRA 800B4F10
    public const uint Game_800B4EB0 = GameVersion.Spanish ? 0x2C4Cu : GameVersion.French ? 0x2B74u : 0x2CCCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4EB0, istr. FRA 800B4F18
    public const uint Game_800B4ECC = GameVersion.Spanish ? 0x6D54u : GameVersion.French ? 0x6E18u : 0x6CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4ECC, istr. FRA 800B4F34
    public const uint Game_800B4ED4 = GameVersion.Spanish ? 0x6D0Cu : GameVersion.French ? 0x6DD0u : 0x6C94u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4ED4, istr. FRA 800B4F3C
    public const uint Game_800B4F44 = GameVersion.Spanish ? 0x2C38u : GameVersion.French ? 0x2B60u : 0x2CB8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4F44, istr. FRA 800B4FAC
    public const uint Game_800B4FD8 = GameVersion.Spanish ? 0x6D9Cu : GameVersion.French ? 0x6E60u : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4FD8, istr. FRA 800B5040
    public const uint Game_800B4FEC = GameVersion.Spanish ? 0x2C18u : GameVersion.French ? 0x2B40u : 0x2C98u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4FEC, istr. FRA 800B5054
    public const uint Game_800B4FFC = GameVersion.Spanish ? 0x2C30u : GameVersion.French ? 0x2B58u : 0x2CB0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4FFC, istr. FRA 800B5064
    public const uint Game_800B5040 = GameVersion.Spanish ? 0x35A8u : GameVersion.French ? 0x34D0u : 0x3628u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5040, istr. FRA 800B50A8
    public const uint Game_800B5048 = GameVersion.Spanish ? 0x2C30u : GameVersion.French ? 0x2B58u : 0x2CB0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5048, istr. FRA 800B50B0
    public const uint Game_800B5050 = GameVersion.Spanish ? 0x2C2Cu : GameVersion.French ? 0x2B54u : 0x2CACu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5050, istr. FRA 800B50B8
    public const uint Game_800B505C = GameVersion.Spanish ? 0x2C1Cu : GameVersion.French ? 0x2B44u : 0x2C9Cu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B505C, istr. FRA 800B50C4
    public const uint Game_800B5068 = GameVersion.Spanish ? 0x2C30u : GameVersion.French ? 0x2B58u : 0x2CB0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5068, istr. FRA 800B50D0
    public const uint Game_800B5278 = GameVersion.Spanish ? 0x2C28u : GameVersion.French ? 0x2B50u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5278, istr. FRA 800B52E0
    public const uint Game_800B5298 = GameVersion.Spanish ? 0x2C28u : GameVersion.French ? 0x2B50u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5298, istr. FRA 800B5300
    public const uint Game_800B5378 = GameVersion.Spanish ? 0x2C28u : GameVersion.French ? 0x2B50u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5378, istr. FRA 800B53E0
    public const uint Game_800B539C = GameVersion.Spanish ? 0x2C28u : GameVersion.French ? 0x2B50u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B539C, istr. FRA 800B5404
    public const uint Game_800B53B0 = GameVersion.Spanish ? 0x2C28u : GameVersion.French ? 0x2B50u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B53B0, istr. FRA 800B5418
    public const uint Game_800B5410 = GameVersion.Spanish ? 0x6D9Cu : GameVersion.French ? 0x6E60u : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5410, istr. FRA 800B5478
    public const uint Game_800B5440 = GameVersion.Spanish ? 0x6D9Cu : GameVersion.French ? 0x6E60u : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5440, istr. FRA 800B54A8
    public const uint Game_800B5484 = GameVersion.Spanish ? 0x2C5Cu : GameVersion.French ? 0x2B84u : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5484, istr. FRA 800B54EC
    public const uint Game_800B5550 = GameVersion.Spanish ? 0x2C5Cu : GameVersion.French ? 0x2B84u : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5550, istr. FRA 800B55B8
    public const uint Game_800B5648 = GameVersion.Spanish ? 0x2C5Cu : GameVersion.French ? 0x2B84u : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5648, istr. FRA 800B56B0
    public const uint Game_800B56B4 = GameVersion.Spanish ? 0x7984u : GameVersion.French ? 0x79ACu : 0x799Cu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B56B4, istr. FRA 800B571C
    public const uint Landmap_80012BC4 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80012BBC, istr. ESP 80012BBC, istr. FRA 80012B84
    public const uint Landmap_80012BE4 = GameVersion.Spanish ? 0x4528u : GameVersion.French ? 0x44F0u : 0x4530u; // widescreen/LandMapPatch.cs:func_80012BBC, istr. ESP 80012BDC, istr. FRA 80012BA4
    public const uint Landmap_80012F88 = GameVersion.Spanish ? 0x14D4u : GameVersion.French ? 0x15ACu : 0x1454u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80012F80, istr. FRA 80012F48
    public const uint Landmap_80012FD8 = GameVersion.Spanish ? 0x4EB0u : GameVersion.French ? 0x4E78u : 0x4EB8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80012FD0, istr. FRA 80012F98
    public const uint Landmap_80012FE4 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80012FDC, istr. FRA 80012FA4
    public const uint Landmap_80013274 = GameVersion.Spanish ? 0x213Cu : GameVersion.French ? 0x2078u : 0x21B4u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 8001326C, istr. FRA 80013234
    public const uint Landmap_80013284 = GameVersion.Spanish ? 0x4EB0u : GameVersion.French ? 0x4E78u : 0x4EB8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 8001327C, istr. FRA 80013244
    public const uint Landmap_8001328C = GameVersion.Spanish ? 0x21B0u : GameVersion.French ? 0x20ECu : 0x2228u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80013284, istr. FRA 8001324C
    public const uint Landmap_800132D0 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 800132C8, istr. FRA 80013290
    public const uint Landmap_80013438 = GameVersion.Spanish ? 0x0F08u : GameVersion.French ? 0x0E44u : 0x0F80u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80013430, istr. FRA 800133F8
    public const uint Landmap_80013C40 = GameVersion.Spanish ? 0x4EB0u : GameVersion.French ? 0x4E78u : 0x4EB8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013C38, istr. FRA 80013C00
    public const uint Landmap_80013C78 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013C70, istr. FRA 80013C38
    public const uint Landmap_80013C90 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013C88, istr. FRA 80013C50
    public const uint Landmap_80013CE8 = GameVersion.Spanish ? 0x0F08u : GameVersion.French ? 0x0E44u : 0x0F80u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013CE0, istr. FRA 80013CA8
    public const uint Landmap_80013DD4 = GameVersion.Spanish ? 0x14D4u : GameVersion.French ? 0x15ACu : 0x1454u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013DCC, istr. FRA 80013D94
    public const uint Landmap_80013E04 = GameVersion.Spanish ? 0x14D4u : GameVersion.French ? 0x15ACu : 0x1454u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013DFC, istr. FRA 80013DC4
    public const uint Landmap_80013E58 = GameVersion.Spanish ? 0x14D4u : GameVersion.French ? 0x15ACu : 0x1454u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013E50, istr. FRA 80013E18
    public const uint Landmap_80013EB0 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013EA8, istr. FRA 80013E70
    public const uint Landmap_Code_80012FDC = GameVersion.Spanish ? 0x80012FD4u : GameVersion.French ? 0x80012F9Cu : 0x80012FDCu; // widescreen/LandMapPatch.cs:func_80013328_landmap
    public const uint Landmap_Code_80013328 = GameVersion.Spanish ? 0x80013320u : GameVersion.French ? 0x800132E8u : 0x80013328u; // widescreen/LandMapPatch.cs:func_80012FDC_landmap
    public const uint Landmap_Code_80013510 = GameVersion.Spanish ? 0x80013508u : GameVersion.French ? 0x800134D0u : 0x80013510u; // widescreen/LandMapPatch.cs:func_80012F70
    public const uint Landmap_Code_80013558 = GameVersion.Spanish ? 0x80013550u : GameVersion.French ? 0x80013518u : 0x80013558u; // widescreen/LandMapPatch.cs:func_80012F70
    public const uint Landmap_Code_800135A8 = GameVersion.Spanish ? 0x800135A0u : GameVersion.French ? 0x80013568u : 0x800135A8u; // widescreen/LandMapPatch.cs:func_80012F70
    public const uint Landmap_Code_80013778 = GameVersion.Spanish ? 0x80013770u : GameVersion.French ? 0x80013738u : 0x80013778u; // widescreen/LandMapPatch.cs:func_80012FDC_landmap
    public const uint Landmap_Code_80013798 = GameVersion.Spanish ? 0x80013790u : GameVersion.French ? 0x80013758u : 0x80013798u; // widescreen/LandMapPatch.cs:func_80012FDC_landmap
    public const uint Landmap_Code_80013D88 = GameVersion.Spanish ? 0x80013D80u : GameVersion.French ? 0x80013D48u : 0x80013D88u; // widescreen/LandMapPatch.cs:func_80013C8C
    public const uint Landmap_Code_80013D90 = GameVersion.Spanish ? 0x80013D88u : GameVersion.French ? 0x80013D50u : 0x80013D90u; // widescreen/LandMapPatch.cs:func_80013C8C
    public const uint Landmap_Code_80013D98 = GameVersion.Spanish ? 0x80013D90u : GameVersion.French ? 0x80013D58u : 0x80013D98u; // widescreen/LandMapPatch.cs:func_80013C3C
    public const uint Landmap_Code_80013DA0 = GameVersion.Spanish ? 0x80013D98u : GameVersion.French ? 0x80013D60u : 0x80013DA0u; // widescreen/LandMapPatch.cs:func_80013C3C
    public const uint Landmap_Code_80013DBC = GameVersion.Spanish ? 0x80013DB4u : GameVersion.French ? 0x80013D7Cu : 0x80013DBCu; // widescreen/LandMapPatch.cs:func_80013C3C
    public const uint Landmap_Code_80013FF0 = GameVersion.Spanish ? 0x80013FE8u : GameVersion.French ? 0x80013FB0u : 0x80013FF0u; // widescreen/LandMapPatch.cs:func_80013C8C
    public const uint Tl_8001097C = GameVersion.Spanish ? 0x2360u : GameVersion.French ? 0x229Cu : 0x23D8u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010984, istr. FRA 80010984
    public const uint Tl_80010988 = GameVersion.Spanish ? 0x2360u : GameVersion.French ? 0x229Cu : 0x23D8u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010990, istr. FRA 80010990
    public const uint Tl_800109EC = GameVersion.Spanish ? 0x0F40u : GameVersion.French ? 0x0E7Cu : 0x0FB8u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 800109F4, istr. FRA 800109F4
    public const uint Tl_80010A1C = GameVersion.Spanish ? 0x7ED8u : GameVersion.French ? 0x7ED8u : 0x7EE0u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010A24, istr. FRA 80010A24
    public const uint Tl_80010A7C = GameVersion.Spanish ? 0x0EB4u : GameVersion.French ? 0x0DF0u : 0x0F2Cu; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010A84, istr. FRA 80010A84
    public const uint Tl_80010C3C = GameVersion.Spanish ? 0x14D4u : GameVersion.French ? 0x15ACu : 0x1454u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010C44, istr. FRA 80010C44
    public const uint Tl_80010D90 = GameVersion.Spanish ? 0x0F08u : GameVersion.French ? 0x0E44u : 0x0F80u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010D98, istr. FRA 80010D98
    public const uint Tl_80010E10 = GameVersion.Spanish ? 0x0F08u : GameVersion.French ? 0x0E44u : 0x0F80u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010E18, istr. FRA 80010E18
    public const uint Tl_Code_800104BC = GameVersion.Spanish ? 0x800104C4u : GameVersion.French ? 0x800104C4u : 0x800104BCu; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_800105B8 = GameVersion.Spanish ? 0x800105C0u : GameVersion.French ? 0x800105C0u : 0x800105B8u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_8001084C = GameVersion.Spanish ? 0x80010854u : GameVersion.French ? 0x80010854u : 0x8001084Cu; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_800108A0 = GameVersion.Spanish ? 0x800108A8u : GameVersion.French ? 0x800108A8u : 0x800108A0u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_80010A78 = GameVersion.Spanish ? 0x80010A80u : GameVersion.French ? 0x80010A80u : 0x80010A78u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_80010B14 = GameVersion.Spanish ? 0x80010B1Cu : GameVersion.French ? 0x80010B1Cu : 0x80010B14u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_80010F00 = GameVersion.Spanish ? 0x80010F08u : GameVersion.French ? 0x80010F08u : 0x80010F00u; // render/CullPatch.cs:func_80010800_tl
}

/// <summary>Chiamate dirette a funzioni ricompilate. Il nome generato contiene
/// l'indirizzo, che cambia nelle versioni il cui codice e' spostato: il nome qui
/// e' quello italiano (Overlay_Indirizzo), la funzione chiamata quella della
/// versione compilata.</summary>
public static class GameFn
{
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_80021E34(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_80021E9C(c, m);
#else
        => MediEvil_game.func_80021E34(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_80021E5C(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_80021EC4(c, m);
#else
        => MediEvil_game.func_80021E5C(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_80022070(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800220D8(c, m);
#else
        => MediEvil_game.func_80022070(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_80022460(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800224C8(c, m);
#else
        => MediEvil_game.func_80022460(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_80022AF0(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_80022B58(c, m);
#else
        => MediEvil_game.func_80022AF0(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800494C8(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_80049530(c, m);
#else
        => MediEvil_game.func_800494C8(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_8009A540(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_8009A5A8(c, m);
#else
        => MediEvil_game.func_8009A540(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_8009A694(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_8009A6FC(c, m);
#else
        => MediEvil_game.func_8009A694(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A2E90(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A2EF8(c, m);
#else
        => MediEvil_game.func_800A2E90(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4264(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A42CC(c, m);
#else
        => MediEvil_game.func_800A4264(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4434(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A449C(c, m);
#else
        => MediEvil_game.func_800A4434(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4464(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A44CC(c, m);
#else
        => MediEvil_game.func_800A4464(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A48FC(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A4964(c, m);
#else
        => MediEvil_game.func_800A48FC(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4D78(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A4DE0(c, m);
#else
        => MediEvil_game.func_800A4D78(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4E88(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A4EF0(c, m);
#else
        => MediEvil_game.func_800A4E88(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4EB8(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A4F20(c, m);
#else
        => MediEvil_game.func_800A4EB8(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4FC8(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A5030(c, m);
#else
        => MediEvil_game.func_800A4FC8(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800ABA38(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800ABAA0(c, m);
#else
        => MediEvil_game.func_800ABA38(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800AF490(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800AF4F8(c, m);
#else
        => MediEvil_game.func_800AF490(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B00E4(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B014C(c, m);
#else
        => MediEvil_game.func_800B00E4(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B02BC(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B0324(c, m);
#else
        => MediEvil_game.func_800B02BC(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B04D4(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B053C(c, m);
#else
        => MediEvil_game.func_800B04D4(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B06D8(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B0740(c, m);
#else
        => MediEvil_game.func_800B06D8(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B097C(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B09E4(c, m);
#else
        => MediEvil_game.func_800B097C(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B0B3C(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B0BA4(c, m);
#else
        => MediEvil_game.func_800B0B3C(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B0D34(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B0D9C(c, m);
#else
        => MediEvil_game.func_800B0D34(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B0F20(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B0F88(c, m);
#else
        => MediEvil_game.func_800B0F20(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B50BC(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B5124(c, m);
#else
        => MediEvil_game.func_800B50BC(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B5A44(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B5AAC(c, m);
#else
        => MediEvil_game.func_800B5A44(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B5E20(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B5E88(c, m);
#else
        => MediEvil_game.func_800B5E20(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B62B4(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B631C(c, m);
#else
        => MediEvil_game.func_800B62B4(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B67A0(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B6808(c, m);
#else
        => MediEvil_game.func_800B67A0(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B6EEC(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B6F54(c, m);
#else
        => MediEvil_game.func_800B6EEC(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B72A0(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B7308(c, m);
#else
        => MediEvil_game.func_800B72A0(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B7738(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B77A0(c, m);
#else
        => MediEvil_game.func_800B7738(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B7A80(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B7AE8(c, m);
#else
        => MediEvil_game.func_800B7A80(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B7E38(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B7EA0(c, m);
#else
        => MediEvil_game.func_800B7E38(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B8304(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B836C(c, m);
#else
        => MediEvil_game.func_800B8304(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B8A24(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B8A8C(c, m);
#else
        => MediEvil_game.func_800B8A24(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B90E8(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B9150(c, m);
#else
        => MediEvil_game.func_800B90E8(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800BFE04(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800BFE6C(c, m);
#else
        => MediEvil_game.func_800BFE04(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800C0028(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800C0090(c, m);
#else
        => MediEvil_game.func_800C0028(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800C00F4(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800C015C(c, m);
#else
        => MediEvil_game.func_800C00F4(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800C0A64(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800C0ACC(c, m);
#else
        => MediEvil_game.func_800C0A64(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800C0E0C(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800C0E74(c, m);
#else
        => MediEvil_game.func_800C0E0C(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800C0F24(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800C0F8C(c, m);
#else
        => MediEvil_game.func_800C0F24(c, m);
#endif
}
