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
    public const bool German = false;
    public const bool English = false;
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
    public const bool German = false;
    public const bool English = false;
#elif LINGUA_DE
    public const string Code = "deu";
    public const string Serial = "SCES-01493";
    /// <summary>eseguibile di avvio indicato da SYSTEM.CNF</summary>
    public const string BootFile = "SCES_014.93";
    /// <summary>titolo della finestra</summary>
    public const string Title = "MediEvilRecomp DEU";
    /// <summary>repository GitHub da cui scaricare gli aggiornamenti</summary>
    public const string UpdateRepo = "Leproide/MediEvilRecomp-DEU";
    public const bool Spanish = false;
    public const bool French = false;
    public const bool German = true;
    public const bool English = false;
#elif LINGUA_EN
    public const string Code = "eng";
    public const string Serial = "SCES-00311";
    /// <summary>eseguibile di avvio indicato da SYSTEM.CNF</summary>
    public const string BootFile = "SCES_003.11";
    /// <summary>titolo della finestra</summary>
    public const string Title = "MediEvilRecomp ENG";
    /// <summary>repository GitHub da cui scaricare gli aggiornamenti</summary>
    public const string UpdateRepo = "Leproide/MediEvilRecomp-ENG";
    public const bool Spanish = false;
    public const bool French = false;
    public const bool German = false;
    public const bool English = true;
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
    public const bool German = false;
    public const bool English = false;
#endif
}

/// <summary>Indirizzi e offset $gp usati dalle patch scritte a mano.</summary>
public static class Addr
{
    /// <summary>CD_cbsync, callback "sync" di libcd dentro MEDIEVIL.EXE (data)</summary>
    public const uint CdCbSync = GameVersion.Spanish ? 0x800EB6E0u : GameVersion.French ? 0x800EB7B8u : GameVersion.German ? 0x800EB5E0u : GameVersion.English ? 0x800EB424u : 0x800EB660u;
    /// <summary>CD_cbready, callback "ready" di libcd dentro MEDIEVIL.EXE (data)</summary>
    public const uint CdCbReady = GameVersion.Spanish ? 0x800EB6E4u : GameVersion.French ? 0x800EB7BCu : GameVersion.German ? 0x800EB5E4u : GameVersion.English ? 0x800EB428u : 0x800EB664u;
    /// <summary>modalita' "emulate" di StUnSetRing (1 = non toccare i callback) (data)</summary>
    public const uint CdEmulateFlag = GameVersion.Spanish ? 0x800EB700u : GameVersion.French ? 0x800EB7D8u : GameVersion.German ? 0x800EB600u : GameVersion.English ? 0x800EB444u : 0x800EB680u;
    /// <summary>struttura del lettore filmati (+0x40 = filmato finito) (data)</summary>
    public const uint MoviePlayerState = GameVersion.Spanish ? 0x800F1A28u : GameVersion.French ? 0x800F1AECu : GameVersion.German ? 0x800F1924u : GameVersion.English ? 0x800F1784u : 0x800F19B0u;
    /// <summary>descrittore del filmato corrente (gp)</summary>
    public const uint GpMovieInfo = GameVersion.Spanish ? 0x75Cu : GameVersion.French ? 0x748u : GameVersion.German ? 0x758u : GameVersion.English ? 0x774u : 0x764u;
    /// <summary>ultimo fotogramma letto (gp)</summary>
    public const uint GpMovieFrame = GameVersion.Spanish ? 0x758u : GameVersion.French ? 0x744u : GameVersion.German ? 0x754u : GameVersion.English ? 0x770u : 0x760u;
    /// <summary>fotogramma precedente (gp)</summary>
    public const uint GpMoviePrevFrame = GameVersion.Spanish ? 0x760u : GameVersion.French ? 0x74Cu : GameVersion.German ? 0x75Cu : GameVersion.English ? 0x778u : 0x768u;
    /// <summary>struttura FreeTerrData (buffer delle primitive del terreno) (data)</summary>
    public const uint FreeTerrData = GameVersion.Spanish ? 0x800EF09Cu : GameVersion.French ? 0x800EF160u : GameVersion.German ? 0x800EEF98u : GameVersion.English ? 0x800EEDF8u : 0x800EF024u;
    /// <summary>tabella della nebbia del terreno (data)</summary>
    public const uint TerrainFog = GameVersion.Spanish ? 0x800F1A94u : GameVersion.French ? 0x800F1B58u : GameVersion.German ? 0x800F1990u : GameVersion.English ? 0x800F17F0u : 0x800F1A1Cu;
    /// <summary>vettori dei piani di vista (4 SVECTOR) (data)</summary>
    public const uint ViewPlaneSvecs = GameVersion.Spanish ? 0x800EECE0u : GameVersion.French ? 0x800EEDA4u : GameVersion.German ? 0x800EEBDCu : GameVersion.English ? 0x800EEA3Cu : 0x800EEC68u;
    /// <summary>livello corrente (data)</summary>
    public const uint CurrentLevel = GameVersion.Spanish ? 0x800EF108u : GameVersion.French ? 0x800EF1CCu : GameVersion.German ? 0x800EF004u : GameVersion.English ? 0x800EEE64u : 0x800EF090u;
    /// <summary>mappa corrente (data)</summary>
    public const uint CurrentMap = GameVersion.Spanish ? 0x800EF158u : GameVersion.French ? 0x800EF21Cu : GameVersion.German ? 0x800EF054u : GameVersion.English ? 0x800EEEB4u : 0x800EF0E0u;
    /// <summary>distanza di vista del terreno (data)</summary>
    public const uint ViewDistance = GameVersion.Spanish ? 0x800EF0C0u : GameVersion.French ? 0x800EF184u : GameVersion.German ? 0x800EEFBCu : GameVersion.English ? 0x800EEE1Cu : 0x800EF048u;
    /// <summary>puntatore al viewport principale (gp)</summary>
    public const uint GpMainViewPort = GameVersion.Spanish ? 0x5C8u : GameVersion.French ? 0x5B4u : GameVersion.German ? 0x5C4u : GameVersion.English ? 0x5E0u : 0x5D0u;
    /// <summary>bersaglio della telecamera (gp)</summary>
    public const uint GpCameraTarget = GameVersion.Spanish ? 0x548u : GameVersion.French ? 0x534u : GameVersion.German ? 0x544u : GameVersion.English ? 0x560u : 0x550u;
    /// <summary>profondita' di proiezione (gp)</summary>
    public const uint GpProjDepth = GameVersion.Spanish ? 0x598u : GameVersion.French ? 0x584u : GameVersion.German ? 0x594u : GameVersion.English ? 0x5B0u : 0x5A0u;
    /// <summary>punto base della griglia (gp)</summary>
    public const uint GpBasePoint = GameVersion.Spanish ? 0x59Cu : GameVersion.French ? 0x588u : GameVersion.German ? 0x598u : GameVersion.English ? 0x5B4u : 0x5A4u;
    /// <summary>mondo della schermata del titolo (gp)</summary>
    public const uint GpTitleWorld = GameVersion.Spanish ? 0x630u : GameVersion.French ? 0x61Cu : GameVersion.German ? 0x62Cu : GameVersion.English ? 0x648u : 0x638u;
    /// <summary>variabile della griglia (candidata 0) (gp)</summary>
    public const uint GpGridSlot0 = GameVersion.Spanish ? 0x594u : GameVersion.French ? 0x580u : GameVersion.German ? 0x590u : GameVersion.English ? 0x5ACu : 0x59Cu;
    /// <summary>variabile della griglia (candidata 1) (gp)</summary>
    public const uint GpGridSlot1 = GameVersion.Spanish ? 0x5A4u : GameVersion.French ? 0x590u : GameVersion.German ? 0x5A0u : GameVersion.English ? 0x5BCu : 0x5ACu;
    /// <summary>variabile della griglia (candidata 2) (gp)</summary>
    public const uint GpGridSlot2 = GameVersion.Spanish ? 0x5A8u : GameVersion.French ? 0x594u : GameVersion.German ? 0x5A4u : GameVersion.English ? 0x5C0u : 0x5B0u;
    /// <summary>variabile della griglia (candidata 3) (gp)</summary>
    public const uint GpGridSlot3 = GameVersion.Spanish ? 0x5ACu : GameVersion.French ? 0x598u : GameVersion.German ? 0x5A8u : GameVersion.English ? 0x5C4u : 0x5B4u;
    /// <summary>variabile della griglia (candidata 4) (gp)</summary>
    public const uint GpGridSlot4 = GameVersion.Spanish ? 0x5B0u : GameVersion.French ? 0x59Cu : GameVersion.German ? 0x5ACu : GameVersion.English ? 0x5C8u : 0x5B8u;
    /// <summary>stato dei trucchi, u16 (mod Trucchi) (data)</summary>
    public const uint CheatState = GameVersion.Spanish ? 0x800EF11Au : GameVersion.French ? 0x800EF1DEu : GameVersion.German ? 0x800EF016u : GameVersion.English ? 0x800EEE76u : 0x800EF0A2u;
    /// <summary>tabella "DUST TO DUST", primo tasto Giu' (mod Trucchi) (data)</summary>
    public const uint CheatTableDust = GameVersion.Spanish ? 0x800CE09Cu : GameVersion.French ? 0x800CE0C4u : GameVersion.German ? 0x800CDFA8u : GameVersion.English ? 0x800CE01Cu : 0x800CE0B4u;
    /// <summary>tabella trucchi estesa, primo tasto Triangolo (mod Trucchi) (data)</summary>
    public const uint CheatTableExtended = GameVersion.Spanish ? 0x800CE0C4u : GameVersion.French ? 0x800CE0ECu : GameVersion.German ? 0x800CDFD0u : GameVersion.English ? 0x800CE044u : 0x800CE0DCu;
    /// <summary>LibCrypt: chiave a 16 bit (func_80065FC4 la usa per decifrare il Mausoleo) (data)</summary>
    public const uint LcKey = GameVersion.Spanish ? 0x800DB32Cu : GameVersion.French ? 0x800DB404u : GameVersion.German ? 0x800DB22Cu : GameVersion.English ? 0x800DB070u : 0x800DB2ACu;
    /// <summary>LibCrypt: numero di bit a 1 (data)</summary>
    public const uint LcCount = GameVersion.Spanish ? 0x800DB330u : GameVersion.French ? 0x800DB408u : GameVersion.German ? 0x800DB230u : GameVersion.English ? 0x800DB074u : 0x800DB2B0u;
    /// <summary>LibCrypt: puntatore alla voce corrente della tabella dei settori (data)</summary>
    public const uint LcTablePtr = GameVersion.Spanish ? 0x800DB3B4u : GameVersion.French ? 0x800DB48Cu : GameVersion.German ? 0x800DB2B4u : GameVersion.English ? 0x800DB0F8u : 0x800DB334u;
    /// <summary>LibCrypt: tabella dei settori (voci da 4 byte BCD: mm, ss, settore di partenza, fotogramma bersaglio) (data)</summary>
    public const uint LcTableBase = GameVersion.Spanish ? 0x800DB334u : GameVersion.French ? 0x800DB40Cu : GameVersion.German ? 0x800DB234u : GameVersion.English ? 0x800DB078u : 0x800DB2B4u;
    /// <summary>LibCrypt: controllo avviato dalla mappa (0/1/2), scritto da LANDMAP (data)</summary>
    public const uint LcState = GameVersion.Spanish ? 0x800EDA8Cu : GameVersion.French ? 0x800EDB50u : GameVersion.German ? 0x800ED984u : GameVersion.English ? 0x800ED7F0u : 0x800EDA14u;
    /// <summary>LibCrypt: jal 0x80065F8C in LANDMAP che avvia il controllo (code)</summary>
    public const uint LcLandmapCall = GameVersion.Spanish ? 0x80011E98u : GameVersion.French ? 0x80011E60u : GameVersion.German ? 0x80011E40u : GameVersion.English ? 0x80011E40u : 0x80011EA0u;
    /// <summary>tabella delle risorse di MEDRES.MWD (voci da 32 byte; la 0xD6 e' il Mausoleo cifrato) (data)</summary>
    public const uint MwdTable = GameVersion.Spanish ? 0x800CE5C0u : GameVersion.French ? 0x800CE5E8u : GameVersion.German ? 0x800CE4CCu : GameVersion.English ? 0x800CE540u : 0x800CE5D8u;
    /// <summary>valore di $gp in MEDIEVIL.EXE (wrapers/Game.cs) (data)</summary>
    public const uint GpBase = GameVersion.Spanish ? 0x800ED8C8u : GameVersion.French ? 0x800ED9A0u : GameVersion.German ? 0x800ED7C8u : GameVersion.English ? 0x800ED60Cu : 0x800ED848u;
    /// <summary>puntatore al giocatore (gp)</summary>
    public const uint GpPlayer = GameVersion.Spanish ? 0x550u : GameVersion.French ? 0x53Cu : GameVersion.German ? 0x54Cu : GameVersion.English ? 0x568u : 0x558u;
    /// <summary>radice della lista dei viewport (gp)</summary>
    public const uint GpViewportRoot = GameVersion.Spanish ? 0x7D8u : GameVersion.French ? 0x7C4u : GameVersion.German ? 0x7D4u : GameVersion.English ? 0x7F0u : 0x7E0u;
    /// <summary>dati della telecamera (gp)</summary>
    public const uint GpCamera = GameVersion.Spanish ? 0x458u : GameVersion.French ? 0x444u : GameVersion.German ? 0x454u : GameVersion.English ? 0x470u : 0x460u;
    /// <summary>entita' della telecamera (gp)</summary>
    public const uint GpCameraEntity = GameVersion.Spanish ? 0x45Cu : GameVersion.French ? 0x448u : GameVersion.German ? 0x458u : GameVersion.English ? 0x474u : 0x464u;
    /// <summary>plugin attivo della telecamera (gp)</summary>
    public const uint GpCameraPlugin = GameVersion.Spanish ? 0x46Cu : GameVersion.French ? 0x458u : GameVersion.German ? 0x468u : GameVersion.English ? 0x484u : 0x474u;
    /// <summary>lista delle texture (gp)</summary>
    public const uint GpTextureList = GameVersion.Spanish ? 0x7A8u : GameVersion.French ? 0x794u : GameVersion.German ? 0x7A4u : GameVersion.English ? 0x7C0u : 0x7B0u;
    /// <summary>dimensione del pool dei poligoni dinamici (gp)</summary>
    public const uint GpPolyPoolSize = GameVersion.Spanish ? 0x804u : GameVersion.French ? 0x7F0u : GameVersion.German ? 0x800u : GameVersion.English ? 0x81Cu : 0x80Cu;
    /// <summary>dimensione del buffer dei poligoni dinamici (gp)</summary>
    public const uint GpPolyBufferSize = GameVersion.Spanish ? 0x800u : GameVersion.French ? 0x7ECu : GameVersion.German ? 0x7FCu : GameVersion.English ? 0x818u : 0x808u;
    /// <summary>attributi delle texture (data)</summary>
    public const uint TextureAttributes = GameVersion.Spanish ? 0x800EDAA0u : GameVersion.French ? 0x800EDB64u : GameVersion.German ? 0x800ED998u : GameVersion.English ? 0x800ED7FCu : 0x800EDA28u;
    /// <summary>pool dei poligoni dinamici di MR (data)</summary>
    public const uint MrDynamicPools = GameVersion.Spanish ? 0x800EE0D0u : GameVersion.French ? 0x800EE194u : GameVersion.German ? 0x800EDFCCu : GameVersion.English ? 0x800EDE2Cu : 0x800EE058u;
    /// <summary>fine dei pool dei poligoni dinamici di MR (data)</summary>
    public const uint MrDynamicPoolEnds = GameVersion.Spanish ? 0x800EE0D8u : GameVersion.French ? 0x800EE19Cu : GameVersion.German ? 0x800EDFD4u : GameVersion.English ? 0x800EDE34u : 0x800EE060u;
    /// <summary>funzione MRAllocMem di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRAllocMem = GameVersion.Spanish ? 0x800A40B8u : GameVersion.French ? 0x800A4120u : GameVersion.German ? 0x800A4050u : GameVersion.English ? 0x800A40B0u : 0x800A40B8u;
    /// <summary>funzione MRFreeMem di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRFreeMem = GameVersion.Spanish ? 0x800A4330u : GameVersion.French ? 0x800A4398u : GameVersion.German ? 0x800A42C8u : GameVersion.English ? 0x800A4328u : 0x800A4330u;
    /// <summary>funzione MRInitialiseResources di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRInitialiseResources = GameVersion.Spanish ? 0x800A7EC8u : GameVersion.French ? 0x800A7F30u : GameVersion.German ? 0x800A7E60u : GameVersion.English ? 0x800A7EC0u : 0x800A7EC8u;
    /// <summary>funzione MRLoadResource di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRLoadResource = GameVersion.Spanish ? 0x800A80C8u : GameVersion.French ? 0x800A8130u : GameVersion.German ? 0x800A8060u : GameVersion.English ? 0x800A80C0u : 0x800A80C8u;
    /// <summary>funzione MRProcessResource di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRProcessResource = GameVersion.Spanish ? 0x800A845Cu : GameVersion.French ? 0x800A84C4u : GameVersion.German ? 0x800A83F4u : GameVersion.English ? 0x800A8454u : 0x800A845Cu;
    /// <summary>funzione MRUnloadResource di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRUnloadResource = GameVersion.Spanish ? 0x800A85E0u : GameVersion.French ? 0x800A8648u : GameVersion.German ? 0x800A8578u : GameVersion.English ? 0x800A85D8u : 0x800A85E0u;
    /// <summary>funzione PowerPackerDecrunch di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnPowerPackerDecrunch = GameVersion.Spanish ? 0x800BA550u : GameVersion.French ? 0x800BA5B8u : GameVersion.German ? 0x800BA4E8u : GameVersion.English ? 0x800BA548u : 0x800BA550u;
    /// <summary>funzione MRAllocateDynamicPolys di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRAllocateDynamicPolys = GameVersion.Spanish ? 0x800A483Cu : GameVersion.French ? 0x800A48A4u : GameVersion.German ? 0x800A47D4u : GameVersion.English ? 0x800A4834u : 0x800A483Cu;
    /// <summary>funzione MRFreeDynamicPolys di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRFreeDynamicPolys = GameVersion.Spanish ? 0x800A48ACu : GameVersion.French ? 0x800A4914u : GameVersion.German ? 0x800A4844u : GameVersion.English ? 0x800A48A4u : 0x800A48ACu;
    /// <summary>funzione MRRenderViewport di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRRenderViewport = GameVersion.Spanish ? 0x8009FDF4u : GameVersion.French ? 0x8009FE5Cu : GameVersion.German ? 0x8009FD8Cu : GameVersion.English ? 0x8009FDECu : 0x8009FDF4u;
    /// <summary>funzione MRKillViewport di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRKillViewport = GameVersion.Spanish ? 0x8009F370u : GameVersion.French ? 0x8009F3D8u : GameVersion.German ? 0x8009F308u : GameVersion.English ? 0x8009F368u : 0x8009F370u;
    /// <summary>funzione MRKillAllViewports di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRKillAllViewports = GameVersion.Spanish ? 0x8009BC78u : GameVersion.French ? 0x8009BCE0u : GameVersion.German ? 0x8009BC10u : GameVersion.English ? 0x8009BC70u : 0x8009BC78u;
    /// <summary>funzione MRUpdateViewportRenderMatrices di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnMRUpdateViewportRenderMatrices = GameVersion.Spanish ? 0x800A04FCu : GameVersion.French ? 0x800A0564u : GameVersion.German ? 0x800A0494u : GameVersion.English ? 0x800A04F4u : 0x800A04FCu;
    /// <summary>funzione CamUpdate di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnCamUpdate = GameVersion.Spanish ? 0x8002D274u : GameVersion.French ? 0x8002D2DCu : GameVersion.German ? 0x8002D20Cu : GameVersion.English ? 0x8002D24Cu : 0x8002D274u;
    /// <summary>funzione FadeUpdate di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnFadeUpdate = GameVersion.Spanish ? 0x8004EB60u : GameVersion.French ? 0x8004EBC8u : GameVersion.German ? 0x8004EAF8u : GameVersion.English ? 0x8004EB38u : 0x8004EB60u;
    /// <summary>funzione EntLibPointOccluded di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnEntLibPointOccluded = GameVersion.Spanish ? 0x800494C8u : GameVersion.French ? 0x80049530u : GameVersion.German ? 0x80049460u : GameVersion.English ? 0x800494A0u : 0x800494C8u;
    /// <summary>funzione LevelStartUp di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnLevelStartUp = GameVersion.Spanish ? 0x80065618u : GameVersion.French ? 0x80065680u : GameVersion.German ? 0x800655B0u : GameVersion.English ? 0x80065610u : 0x80065618u;
    /// <summary>funzione LevelShutDown di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnLevelShutDown = GameVersion.Spanish ? 0x800658BCu : GameVersion.French ? 0x80065924u : GameVersion.German ? 0x80065854u : GameVersion.English ? 0x800658B4u : 0x800658BCu;
    /// <summary>funzione FreeTerrShutDown di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnFreeTerrShutDown = GameVersion.Spanish ? 0x8005153Cu : GameVersion.French ? 0x800515A4u : GameVersion.German ? 0x800514D4u : GameVersion.English ? 0x80051514u : 0x8005153Cu;
    /// <summary>funzione TerrainFogShutdown di MEDIEVIL.EXE (wrapers/Functions.cs) (code)</summary>
    public const uint FnTerrainFogShutdown = GameVersion.Spanish ? 0x8007A140u : GameVersion.French ? 0x8007A1A8u : GameVersion.German ? 0x8007A0D8u : GameVersion.English ? 0x8007A138u : 0x8007A140u;
    /// <summary>lista degli oggetti attorno a Dan (DistantObjectsPatch) (data)</summary>
    public const uint ThingList = GameVersion.Spanish ? 0x800EEAC8u : GameVersion.French ? 0x800EEB8Cu : GameVersion.German ? 0x800EE9C4u : GameVersion.English ? 0x800EE824u : 0x800EEA50u;
    /// <summary>lista degli oggetti svegli, sentinella (DistantObjectsPatch) (data)</summary>
    public const uint ThingAwakeList = GameVersion.Spanish ? 0x800EEB60u : GameVersion.French ? 0x800EEC24u : GameVersion.German ? 0x800EEA5Cu : GameVersion.English ? 0x800EE8BCu : 0x800EEAE8u;
    /// <summary>punto di riferimento per la distanza degli oggetti (DistantObjectsPatch) (data)</summary>
    public const uint ThingRefPoint = GameVersion.Spanish ? 0x800EEBF8u : GameVersion.French ? 0x800EECBCu : GameVersion.German ? 0x800EEAF4u : GameVersion.English ? 0x800EE954u : 0x800EEB80u;
    /// <summary>puntatore alle soglie sveglia/disattivazione degli oggetti (DistantObjectsPatch) (data)</summary>
    public const uint ThingRangePtr = GameVersion.Spanish ? 0x800EEC0Cu : GameVersion.French ? 0x800EECD0u : GameVersion.German ? 0x800EEB08u : GameVersion.English ? 0x800EE968u : 0x800EEB94u;
    /// <summary>12 liste libere dell'allocatore del gioco, 0x14 byte l'una (HeapExpansion) (data)</summary>
    public const uint HeapFreeBins = GameVersion.Spanish ? 0x800F3B80u : GameVersion.French ? 0x800F3C44u : GameVersion.German ? 0x800F3A7Cu : GameVersion.English ? 0x800F38DCu : 0x800F3B08u;
    /// <summary>puntatore all'array dei poligoni del terreno (TerrainEdgeFade) (data)</summary>
    public const uint TerrainPolyArrayPtr = GameVersion.Spanish ? 0x800EDE54u : GameVersion.French ? 0x800EDF18u : GameVersion.German ? 0x800EDD50u : GameVersion.English ? 0x800EDBB0u : 0x800EDDDCu;
    /// <summary>puntatore all'array dei vertici del terreno (TerrainEdgeFade) (data)</summary>
    public const uint TerrainVertexArrayPtr = GameVersion.Spanish ? 0x800EDE58u : GameVersion.French ? 0x800EDF1Cu : GameVersion.German ? 0x800EDD54u : GameVersion.English ? 0x800EDBB4u : 0x800EDDE0u;
    /// <summary>tipo oggetto: runa sulla mano dei cancelli (DistantObjectsPatch); mappa solo "inferred": verificato a mano, struttura del tipo identica a parte i puntatori (spostati di -0x18 / +0x80 come il resto dei dati); FRA: +0x10, stessa verifica (24 parole su 24); DEU: -0x10C, ENG: -0x98, stessa verifica (24 parole su 24) (fixed)</summary>
    public const uint TypeRuneHand = GameVersion.Spanish ? 0x800D73A8u : GameVersion.French ? 0x800D73D0u : GameVersion.German ? 0x800D72B4u : GameVersion.English ? 0x800D7328u : 0x800D73C0u;
    /// <summary>tipo oggetto: casse, anche esplosive (DistantObjectsPatch); mappa solo "inferred": verificato a mano, struttura del tipo identica a parte i puntatori (spostati di -0x18 / +0x80 come il resto dei dati); FRA: +0x10, stessa verifica (24 parole su 24); DEU: -0x10C, ENG: -0x98, stessa verifica (24 parole su 24) (fixed)</summary>
    public const uint TypeChest = GameVersion.Spanish ? 0x800D5274u : GameVersion.French ? 0x800D529Cu : GameVersion.German ? 0x800D5180u : GameVersion.English ? 0x800D51F4u : 0x800D528Cu;
}

/// <summary>Immediati delle copie di funzioni ricompilate (CullPatch,
/// EntityFloatFunctions, LandMapPatch). Il nome e' l'istruzione italiana che
/// li produce (Overlay_Indirizzo) oppure, per gli indirizzi di codice,
/// Overlay_Code_IndirizzoItaliano.</summary>
public static class RAddr
{
    public const uint Game_80021DA4 = GameVersion.Spanish ? 0xF09Cu : GameVersion.French ? 0xF160u : GameVersion.German ? 0xEF98u : GameVersion.English ? 0xEDF8u : 0xF024u; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DA4, istr. FRA 80021E0C, istr. DEU 80021D3C, istr. ENG 80021D7C
    public const uint Game_80021DAC = GameVersion.Spanish ? 0xE070u : GameVersion.French ? 0xE134u : GameVersion.German ? 0xDF6Cu : GameVersion.English ? 0xDDCCu : 0xDFF8u; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DAC, istr. FRA 80021E14, istr. DEU 80021D44, istr. ENG 80021D84
    public const uint Game_80021DB0 = GameVersion.Spanish ? 0x1A94u : GameVersion.French ? 0x1B58u : GameVersion.German ? 0x1990u : GameVersion.English ? 0x17F0u : 0x1A1Cu; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DB0, istr. FRA 80021E18, istr. DEU 80021D48, istr. ENG 80021D88
    public const uint Game_80021DB4 = GameVersion.Spanish ? 0xDE58u : GameVersion.French ? 0xDF1Cu : GameVersion.German ? 0xDD54u : GameVersion.English ? 0xDBB4u : 0xDDE0u; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DB4, istr. FRA 80021E1C, istr. DEU 80021D4C, istr. ENG 80021D8C
    public const uint Game_80021DB8 = GameVersion.Spanish ? 0xDE54u : GameVersion.French ? 0xDF18u : GameVersion.German ? 0xDD50u : GameVersion.English ? 0xDBB0u : 0xDDDCu; // render/CullPatch.cs:func_80021D5C, istr. ESP 80021DB8, istr. FRA 80021E20, istr. DEU 80021D50, istr. ENG 80021D90
    public const uint Game_80024B08 = GameVersion.Spanish ? 0x0448u : GameVersion.French ? 0x0434u : GameVersion.German ? 0x0444u : GameVersion.English ? 0x0460u : 0x0450u; // render/CullPatch.cs:func_80024B00, istr. ESP 80024B08, istr. FRA 80024B70, istr. DEU 80024AA0, istr. ENG 80024AE0
    public const uint Game_80025120 = GameVersion.Spanish ? 0x0F38u : GameVersion.French ? 0x0E74u : GameVersion.German ? 0x103Cu : GameVersion.English ? 0x11DCu : 0x0FB0u; // render/CullPatch.cs:func_80024B00, istr. ESP 80025120, istr. FRA 80025188, istr. DEU 800250B8, istr. ENG 800250F8
    public const uint Game_80025294 = GameVersion.Spanish ? 0x05C8u : GameVersion.French ? 0x05B4u : GameVersion.German ? 0x05C4u : GameVersion.English ? 0x05E0u : 0x05D0u; // render/CullPatch.cs:func_80024B00, istr. ESP 80025294, istr. FRA 800252FC, istr. DEU 8002522C, istr. ENG 8002526C
    public const uint Game_80025298 = GameVersion.Spanish ? 0x0F44u : GameVersion.French ? 0x0E80u : GameVersion.German ? 0x1048u : GameVersion.English ? 0x11E8u : 0x0FBCu; // render/CullPatch.cs:func_80024B00, istr. ESP 80025298, istr. FRA 80025300, istr. DEU 80025230, istr. ENG 80025270
    public const uint Game_800252D8 = GameVersion.Spanish ? 0x0448u : GameVersion.French ? 0x0434u : GameVersion.German ? 0x0444u : GameVersion.English ? 0x0460u : 0x0450u; // render/CullPatch.cs:func_800252D0, istr. ESP 800252D8, istr. FRA 80025340, istr. DEU 80025270, istr. ENG 800252B0
    public const uint Game_80025690 = GameVersion.Spanish ? 0x0E14u : GameVersion.French ? 0x0D50u : GameVersion.German ? 0x0F18u : GameVersion.English ? 0x10B8u : 0x0E8Cu; // render/CullPatch.cs:func_800252D0, istr. ESP 80025690, istr. FRA 800256F8, istr. DEU 80025628, istr. ENG 80025668
    public const uint Game_800258B0 = GameVersion.Spanish ? 0x05C8u : GameVersion.French ? 0x05B4u : GameVersion.German ? 0x05C4u : GameVersion.English ? 0x05E0u : 0x05D0u; // render/CullPatch.cs:func_800252D0, istr. ESP 800258B0, istr. FRA 80025918, istr. DEU 80025848, istr. ENG 80025888
    public const uint Game_8009A6C0 = GameVersion.Spanish ? 0x2360u : GameVersion.French ? 0x229Cu : GameVersion.German ? 0x2464u : GameVersion.English ? 0x2604u : 0x23D8u; // render/CullPatch.cs:func_8009A694, istr. ESP 8009A6C0, istr. FRA 8009A728, istr. DEU 8009A658, istr. ENG 8009A6B8
    public const uint Game_800A10E8 = GameVersion.Spanish ? 0x1F28u : GameVersion.French ? 0x1E64u : GameVersion.German ? 0x202Cu : GameVersion.English ? 0x21CCu : 0x1FA0u; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A10E8, istr. FRA 800A1150, istr. DEU 800A1080, istr. ENG 800A10E0
    public const uint Game_800A13D8 = GameVersion.Spanish ? 0x2360u : GameVersion.French ? 0x229Cu : GameVersion.German ? 0x2464u : GameVersion.English ? 0x2604u : 0x23D8u; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A13D8, istr. FRA 800A1440, istr. DEU 800A1370, istr. ENG 800A13D0
    public const uint Game_800A1AA0 = GameVersion.Spanish ? 0x3B30u : GameVersion.French ? 0x3BF4u : GameVersion.German ? 0x3A2Cu : GameVersion.English ? 0x388Cu : 0x3AB8u; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A1AA0, istr. FRA 800A1B08, istr. DEU 800A1A38, istr. ENG 800A1A98
    public const uint Game_800A1D04 = GameVersion.Spanish ? 0x6DC4u : GameVersion.French ? 0x6DECu : GameVersion.German ? 0x6CD0u : GameVersion.English ? 0x6D44u : 0x6DDCu; // render/CullPatch.cs:func_800A10D8, istr. ESP 800A1D04, istr. FRA 800A1D6C, istr. DEU 800A1C9C, istr. ENG 800A1CFC
    public const uint Game_800B37A8 = GameVersion.Spanish ? 0x78C4u : GameVersion.French ? 0x78ECu : GameVersion.German ? 0x77D0u : GameVersion.English ? 0x7844u : 0x78DCu; // render/CullPatch.cs:func_800B3748, istr. ESP 800B37A8, istr. FRA 800B3810, istr. DEU 800B3740, istr. ENG 800B37A0
    public const uint Game_800B37FC = GameVersion.Spanish ? 0x07A8u : GameVersion.French ? 0x0794u : GameVersion.German ? 0x07A4u : GameVersion.English ? 0x07C0u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B37FC, istr. FRA 800B3864, istr. DEU 800B3794, istr. ENG 800B37F4
    public const uint Game_800B39B8 = GameVersion.Spanish ? 0x07A8u : GameVersion.French ? 0x0794u : GameVersion.German ? 0x07A4u : GameVersion.English ? 0x07C0u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B39B8, istr. FRA 800B3A20, istr. DEU 800B3950, istr. ENG 800B39B0
    public const uint Game_800B3BF8 = GameVersion.Spanish ? 0x07A8u : GameVersion.French ? 0x0794u : GameVersion.German ? 0x07A4u : GameVersion.English ? 0x07C0u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B3BF8, istr. FRA 800B3C60, istr. DEU 800B3B90, istr. ENG 800B3BF0
    public const uint Game_800B3DB4 = GameVersion.Spanish ? 0x07A8u : GameVersion.French ? 0x0794u : GameVersion.German ? 0x07A4u : GameVersion.English ? 0x07C0u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B3DB4, istr. FRA 800B3E1C, istr. DEU 800B3D4C, istr. ENG 800B3DAC
    public const uint Game_800B4064 = GameVersion.Spanish ? 0x7904u : GameVersion.French ? 0x792Cu : GameVersion.German ? 0x7810u : GameVersion.English ? 0x7884u : 0x791Cu; // render/CullPatch.cs:func_800B3748, istr. ESP 800B4064, istr. FRA 800B40CC, istr. DEU 800B3FFC, istr. ENG 800B405C
    public const uint Game_800B4160 = GameVersion.Spanish ? 0x07A8u : GameVersion.French ? 0x0794u : GameVersion.German ? 0x07A4u : GameVersion.English ? 0x07C0u : 0x07B0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B4160, istr. FRA 800B41C8, istr. DEU 800B40F8, istr. ENG 800B4158
    public const uint Game_800B425C = GameVersion.Spanish ? 0x1F28u : GameVersion.French ? 0x1E64u : GameVersion.German ? 0x202Cu : GameVersion.English ? 0x21CCu : 0x1FA0u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B425C, istr. FRA 800B42C4, istr. DEU 800B41F4, istr. ENG 800B4254
    public const uint Game_800B4410 = GameVersion.Spanish ? 0x2360u : GameVersion.French ? 0x229Cu : GameVersion.German ? 0x2464u : GameVersion.English ? 0x2604u : 0x23D8u; // render/CullPatch.cs:func_800B3748, istr. ESP 800B4410, istr. FRA 800B4478, istr. DEU 800B43A8, istr. ENG 800B4408
    public const uint Game_800B49D0 = GameVersion.Spanish ? 0x7944u : GameVersion.French ? 0x796Cu : GameVersion.German ? 0x7850u : GameVersion.English ? 0x78C4u : 0x795Cu; // render/CullPatch.cs:func_800B3748, istr. ESP 800B49D0, istr. FRA 800B4A38, istr. DEU 800B4968, istr. ENG 800B49C8
    public const uint Game_800B4E1C = GameVersion.Spanish ? 0x2C38u : GameVersion.French ? 0x2B60u : GameVersion.German ? 0x2D38u : GameVersion.English ? 0x2EF4u : 0x2CB8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E1C, istr. FRA 800B4E84, istr. DEU 800B4DB4, istr. ENG 800B4E14
    public const uint Game_800B4E24 = GameVersion.Spanish ? 0x2C24u : GameVersion.French ? 0x2B4Cu : GameVersion.German ? 0x2D24u : GameVersion.English ? 0x2EE0u : 0x2CA4u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E24, istr. FRA 800B4E8C, istr. DEU 800B4DBC, istr. ENG 800B4E1C
    public const uint Game_800B4E34 = GameVersion.Spanish ? 0x6D9Cu : GameVersion.French ? 0x6E60u : GameVersion.German ? 0x6C98u : GameVersion.English ? 0x6AF8u : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E34, istr. FRA 800B4E9C, istr. DEU 800B4DCC, istr. ENG 800B4E2C
    public const uint Game_800B4E44 = GameVersion.Spanish ? 0x2C70u : GameVersion.French ? 0x2B98u : GameVersion.German ? 0x2D70u : GameVersion.English ? 0x2F2Cu : 0x2CF0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E44, istr. FRA 800B4EAC, istr. DEU 800B4DDC, istr. ENG 800B4E3C
    public const uint Game_800B4E54 = GameVersion.Spanish ? 0x2C6Cu : GameVersion.French ? 0x2B94u : GameVersion.German ? 0x2D6Cu : GameVersion.English ? 0x2F28u : 0x2CECu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E54, istr. FRA 800B4EBC, istr. DEU 800B4DEC, istr. ENG 800B4E4C
    public const uint Game_800B4E64 = GameVersion.Spanish ? 0x2C68u : GameVersion.French ? 0x2B90u : GameVersion.German ? 0x2D68u : GameVersion.English ? 0x2F24u : 0x2CE8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E64, istr. FRA 800B4ECC, istr. DEU 800B4DFC, istr. ENG 800B4E5C
    public const uint Game_800B4E74 = GameVersion.Spanish ? 0x2C64u : GameVersion.French ? 0x2B8Cu : GameVersion.German ? 0x2D64u : GameVersion.English ? 0x2F20u : 0x2CE4u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E74, istr. FRA 800B4EDC, istr. DEU 800B4E0C, istr. ENG 800B4E6C
    public const uint Game_800B4E84 = GameVersion.Spanish ? 0x2C60u : GameVersion.French ? 0x2B88u : GameVersion.German ? 0x2D60u : GameVersion.English ? 0x2F1Cu : 0x2CE0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E84, istr. FRA 800B4EEC, istr. DEU 800B4E1C, istr. ENG 800B4E7C
    public const uint Game_800B4E94 = GameVersion.Spanish ? 0x2C5Cu : GameVersion.French ? 0x2B84u : GameVersion.German ? 0x2D5Cu : GameVersion.English ? 0x2F18u : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4E94, istr. FRA 800B4EFC, istr. DEU 800B4E2C, istr. ENG 800B4E8C
    public const uint Game_800B4EA8 = GameVersion.Spanish ? 0x2C3Cu : GameVersion.French ? 0x2B64u : GameVersion.German ? 0x2D3Cu : GameVersion.English ? 0x2EF8u : 0x2CBCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4EA8, istr. FRA 800B4F10, istr. DEU 800B4E40, istr. ENG 800B4EA0
    public const uint Game_800B4EB0 = GameVersion.Spanish ? 0x2C4Cu : GameVersion.French ? 0x2B74u : GameVersion.German ? 0x2D4Cu : GameVersion.English ? 0x2F08u : 0x2CCCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4EB0, istr. FRA 800B4F18, istr. DEU 800B4E48, istr. ENG 800B4EA8
    public const uint Game_800B4ECC = GameVersion.Spanish ? 0x6D54u : GameVersion.French ? 0x6E18u : GameVersion.German ? 0x6C50u : GameVersion.English ? 0x6AB0u : 0x6CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4ECC, istr. FRA 800B4F34, istr. DEU 800B4E64, istr. ENG 800B4EC4
    public const uint Game_800B4ED4 = GameVersion.Spanish ? 0x6D0Cu : GameVersion.French ? 0x6DD0u : GameVersion.German ? 0x6C08u : GameVersion.English ? 0x6A68u : 0x6C94u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4ED4, istr. FRA 800B4F3C, istr. DEU 800B4E6C, istr. ENG 800B4ECC
    public const uint Game_800B4F44 = GameVersion.Spanish ? 0x2C38u : GameVersion.French ? 0x2B60u : GameVersion.German ? 0x2D38u : GameVersion.English ? 0x2EF4u : 0x2CB8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4F44, istr. FRA 800B4FAC, istr. DEU 800B4EDC, istr. ENG 800B4F3C
    public const uint Game_800B4FD8 = GameVersion.Spanish ? 0x6D9Cu : GameVersion.French ? 0x6E60u : GameVersion.German ? 0x6C98u : GameVersion.English ? 0x6AF8u : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4FD8, istr. FRA 800B5040, istr. DEU 800B4F70, istr. ENG 800B4FD0
    public const uint Game_800B4FEC = GameVersion.Spanish ? 0x2C18u : GameVersion.French ? 0x2B40u : GameVersion.German ? 0x2D18u : GameVersion.English ? 0x2ED4u : 0x2C98u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4FEC, istr. FRA 800B5054, istr. DEU 800B4F84, istr. ENG 800B4FE4
    public const uint Game_800B4FFC = GameVersion.Spanish ? 0x2C30u : GameVersion.French ? 0x2B58u : GameVersion.German ? 0x2D30u : GameVersion.English ? 0x2EECu : 0x2CB0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B4FFC, istr. FRA 800B5064, istr. DEU 800B4F94, istr. ENG 800B4FF4
    public const uint Game_800B5040 = GameVersion.Spanish ? 0x35A8u : GameVersion.French ? 0x34D0u : GameVersion.German ? 0x36A8u : GameVersion.English ? 0x3864u : 0x3628u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5040, istr. FRA 800B50A8, istr. DEU 800B4FD8, istr. ENG 800B5038
    public const uint Game_800B5048 = GameVersion.Spanish ? 0x2C30u : GameVersion.French ? 0x2B58u : GameVersion.German ? 0x2D30u : GameVersion.English ? 0x2EECu : 0x2CB0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5048, istr. FRA 800B50B0, istr. DEU 800B4FE0, istr. ENG 800B5040
    public const uint Game_800B5050 = GameVersion.Spanish ? 0x2C2Cu : GameVersion.French ? 0x2B54u : GameVersion.German ? 0x2D2Cu : GameVersion.English ? 0x2EE8u : 0x2CACu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5050, istr. FRA 800B50B8, istr. DEU 800B4FE8, istr. ENG 800B5048
    public const uint Game_800B505C = GameVersion.Spanish ? 0x2C1Cu : GameVersion.French ? 0x2B44u : GameVersion.German ? 0x2D1Cu : GameVersion.English ? 0x2ED8u : 0x2C9Cu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B505C, istr. FRA 800B50C4, istr. DEU 800B4FF4, istr. ENG 800B5054
    public const uint Game_800B5068 = GameVersion.Spanish ? 0x2C30u : GameVersion.French ? 0x2B58u : GameVersion.German ? 0x2D30u : GameVersion.English ? 0x2EECu : 0x2CB0u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5068, istr. FRA 800B50D0, istr. DEU 800B5000, istr. ENG 800B5060
    public const uint Game_800B5278 = GameVersion.Spanish ? 0x2C28u : GameVersion.French ? 0x2B50u : GameVersion.German ? 0x2D28u : GameVersion.English ? 0x2EE4u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5278, istr. FRA 800B52E0, istr. DEU 800B5210, istr. ENG 800B5270
    public const uint Game_800B5298 = GameVersion.Spanish ? 0x2C28u : GameVersion.French ? 0x2B50u : GameVersion.German ? 0x2D28u : GameVersion.English ? 0x2EE4u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5298, istr. FRA 800B5300, istr. DEU 800B5230, istr. ENG 800B5290
    public const uint Game_800B5378 = GameVersion.Spanish ? 0x2C28u : GameVersion.French ? 0x2B50u : GameVersion.German ? 0x2D28u : GameVersion.English ? 0x2EE4u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5378, istr. FRA 800B53E0, istr. DEU 800B5310, istr. ENG 800B5370
    public const uint Game_800B539C = GameVersion.Spanish ? 0x2C28u : GameVersion.French ? 0x2B50u : GameVersion.German ? 0x2D28u : GameVersion.English ? 0x2EE4u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B539C, istr. FRA 800B5404, istr. DEU 800B5334, istr. ENG 800B5394
    public const uint Game_800B53B0 = GameVersion.Spanish ? 0x2C28u : GameVersion.French ? 0x2B50u : GameVersion.German ? 0x2D28u : GameVersion.English ? 0x2EE4u : 0x2CA8u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B53B0, istr. FRA 800B5418, istr. DEU 800B5348, istr. ENG 800B53A8
    public const uint Game_800B5410 = GameVersion.Spanish ? 0x6D9Cu : GameVersion.French ? 0x6E60u : GameVersion.German ? 0x6C98u : GameVersion.English ? 0x6AF8u : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5410, istr. FRA 800B5478, istr. DEU 800B53A8, istr. ENG 800B5408
    public const uint Game_800B5440 = GameVersion.Spanish ? 0x6D9Cu : GameVersion.French ? 0x6E60u : GameVersion.German ? 0x6C98u : GameVersion.English ? 0x6AF8u : 0x6D24u; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5440, istr. FRA 800B54A8, istr. DEU 800B53D8, istr. ENG 800B5438
    public const uint Game_800B5484 = GameVersion.Spanish ? 0x2C5Cu : GameVersion.French ? 0x2B84u : GameVersion.German ? 0x2D5Cu : GameVersion.English ? 0x2F18u : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5484, istr. FRA 800B54EC, istr. DEU 800B541C, istr. ENG 800B547C
    public const uint Game_800B5550 = GameVersion.Spanish ? 0x2C5Cu : GameVersion.French ? 0x2B84u : GameVersion.German ? 0x2D5Cu : GameVersion.English ? 0x2F18u : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5550, istr. FRA 800B55B8, istr. DEU 800B54E8, istr. ENG 800B5548
    public const uint Game_800B5648 = GameVersion.Spanish ? 0x2C5Cu : GameVersion.French ? 0x2B84u : GameVersion.German ? 0x2D5Cu : GameVersion.English ? 0x2F18u : 0x2CDCu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B5648, istr. FRA 800B56B0, istr. DEU 800B55E0, istr. ENG 800B5640
    public const uint Game_800B56B4 = GameVersion.Spanish ? 0x7984u : GameVersion.French ? 0x79ACu : GameVersion.German ? 0x7890u : GameVersion.English ? 0x7904u : 0x799Cu; // render/CullPatch.cs:func_800B4244, istr. ESP 800B56B4, istr. FRA 800B571C, istr. DEU 800B564C, istr. ENG 800B56AC
    public const uint Landmap_80012BC4 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : GameVersion.German ? 0x4E68u : GameVersion.English ? 0x4FB0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80012BBC, istr. ESP 80012BBC, istr. FRA 80012B84, istr. DEU 80012B64, istr. ENG 80012B64
    public const uint Landmap_80012BE4 = GameVersion.Spanish ? 0x4528u : GameVersion.French ? 0x44F0u : GameVersion.German ? 0x44D0u : GameVersion.English ? 0x4618u : 0x4530u; // widescreen/LandMapPatch.cs:func_80012BBC, istr. ESP 80012BDC, istr. FRA 80012BA4, istr. DEU 80012B84, istr. ENG 80012B84
    public const uint Landmap_80012F88 = GameVersion.Spanish ? 0x14D4u : GameVersion.French ? 0x15ACu : GameVersion.German ? 0x13D4u : GameVersion.English ? 0x1218u : 0x1454u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80012F80, istr. FRA 80012F48, istr. DEU 80012F28, istr. ENG 80012F28
    public const uint Landmap_80012FD8 = GameVersion.Spanish ? 0x4EB0u : GameVersion.French ? 0x4E78u : GameVersion.German ? 0x4E58u : GameVersion.English ? 0x4FA0u : 0x4EB8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80012FD0, istr. FRA 80012F98, istr. DEU 80012F78, istr. ENG 80012F78
    public const uint Landmap_80012FE4 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : GameVersion.German ? 0x4E68u : GameVersion.English ? 0x4FB0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80012FDC, istr. FRA 80012FA4, istr. DEU 80012F84, istr. ENG 80012F84
    public const uint Landmap_80013274 = GameVersion.Spanish ? 0x213Cu : GameVersion.French ? 0x2078u : GameVersion.German ? 0x2240u : GameVersion.English ? 0x23E0u : 0x21B4u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 8001326C, istr. FRA 80013234, istr. DEU 80013214, istr. ENG 80013214
    public const uint Landmap_80013284 = GameVersion.Spanish ? 0x4EB0u : GameVersion.French ? 0x4E78u : GameVersion.German ? 0x4E58u : GameVersion.English ? 0x4FA0u : 0x4EB8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 8001327C, istr. FRA 80013244, istr. DEU 80013224, istr. ENG 80013224
    public const uint Landmap_8001328C = GameVersion.Spanish ? 0x21B0u : GameVersion.French ? 0x20ECu : GameVersion.German ? 0x22B4u : GameVersion.English ? 0x2454u : 0x2228u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80013284, istr. FRA 8001324C, istr. DEU 8001322C, istr. ENG 8001322C
    public const uint Landmap_800132D0 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : GameVersion.German ? 0x4E68u : GameVersion.English ? 0x4FB0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 800132C8, istr. FRA 80013290, istr. DEU 80013270, istr. ENG 80013270
    public const uint Landmap_80013438 = GameVersion.Spanish ? 0x0F08u : GameVersion.French ? 0x0E44u : GameVersion.German ? 0x100Cu : GameVersion.English ? 0x11ACu : 0x0F80u; // widescreen/LandMapPatch.cs:func_80012F70, istr. ESP 80013430, istr. FRA 800133F8, istr. DEU 800133D8, istr. ENG 800133D8
    public const uint Landmap_80013C40 = GameVersion.Spanish ? 0x4EB0u : GameVersion.French ? 0x4E78u : GameVersion.German ? 0x4E58u : GameVersion.English ? 0x4FA0u : 0x4EB8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013C38, istr. FRA 80013C00, istr. DEU 80013BE0, istr. ENG 80013BE0
    public const uint Landmap_80013C78 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : GameVersion.German ? 0x4E68u : GameVersion.English ? 0x4FB0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013C70, istr. FRA 80013C38, istr. DEU 80013C18, istr. ENG 80013C18
    public const uint Landmap_80013C90 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : GameVersion.German ? 0x4E68u : GameVersion.English ? 0x4FB0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013C88, istr. FRA 80013C50, istr. DEU 80013C30, istr. ENG 80013C30
    public const uint Landmap_80013CE8 = GameVersion.Spanish ? 0x0F08u : GameVersion.French ? 0x0E44u : GameVersion.German ? 0x100Cu : GameVersion.English ? 0x11ACu : 0x0F80u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013CE0, istr. FRA 80013CA8, istr. DEU 80013C88, istr. ENG 80013C88
    public const uint Landmap_80013DD4 = GameVersion.Spanish ? 0x14D4u : GameVersion.French ? 0x15ACu : GameVersion.German ? 0x13D4u : GameVersion.English ? 0x1218u : 0x1454u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013DCC, istr. FRA 80013D94, istr. DEU 80013D74, istr. ENG 80013D74
    public const uint Landmap_80013E04 = GameVersion.Spanish ? 0x14D4u : GameVersion.French ? 0x15ACu : GameVersion.German ? 0x13D4u : GameVersion.English ? 0x1218u : 0x1454u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013DFC, istr. FRA 80013DC4, istr. DEU 80013DA4, istr. ENG 80013DA4
    public const uint Landmap_80013E58 = GameVersion.Spanish ? 0x14D4u : GameVersion.French ? 0x15ACu : GameVersion.German ? 0x13D4u : GameVersion.English ? 0x1218u : 0x1454u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013E50, istr. FRA 80013E18, istr. DEU 80013DF8, istr. ENG 80013DF8
    public const uint Landmap_80013EB0 = GameVersion.Spanish ? 0x4EC0u : GameVersion.French ? 0x4E88u : GameVersion.German ? 0x4E68u : GameVersion.English ? 0x4FB0u : 0x4EC8u; // widescreen/LandMapPatch.cs:func_80013C3C, istr. ESP 80013EA8, istr. FRA 80013E70, istr. DEU 80013E50, istr. ENG 80013E50
    public const uint Landmap_Code_80012FDC = GameVersion.Spanish ? 0x80012FD4u : GameVersion.French ? 0x80012F9Cu : GameVersion.German ? 0x80012F7Cu : GameVersion.English ? 0x80012F7Cu : 0x80012FDCu; // widescreen/LandMapPatch.cs:func_80013328_landmap
    public const uint Landmap_Code_80013328 = GameVersion.Spanish ? 0x80013320u : GameVersion.French ? 0x800132E8u : GameVersion.German ? 0x800132C8u : GameVersion.English ? 0x800132C8u : 0x80013328u; // widescreen/LandMapPatch.cs:func_80012FDC_landmap
    public const uint Landmap_Code_80013510 = GameVersion.Spanish ? 0x80013508u : GameVersion.French ? 0x800134D0u : GameVersion.German ? 0x800134B0u : GameVersion.English ? 0x800134B0u : 0x80013510u; // widescreen/LandMapPatch.cs:func_80012F70
    public const uint Landmap_Code_80013558 = GameVersion.Spanish ? 0x80013550u : GameVersion.French ? 0x80013518u : GameVersion.German ? 0x800134F8u : GameVersion.English ? 0x800134F8u : 0x80013558u; // widescreen/LandMapPatch.cs:func_80012F70
    public const uint Landmap_Code_800135A8 = GameVersion.Spanish ? 0x800135A0u : GameVersion.French ? 0x80013568u : GameVersion.German ? 0x80013548u : GameVersion.English ? 0x80013548u : 0x800135A8u; // widescreen/LandMapPatch.cs:func_80012F70
    public const uint Landmap_Code_80013778 = GameVersion.Spanish ? 0x80013770u : GameVersion.French ? 0x80013738u : GameVersion.German ? 0x80013718u : GameVersion.English ? 0x80013718u : 0x80013778u; // widescreen/LandMapPatch.cs:func_80012FDC_landmap
    public const uint Landmap_Code_80013798 = GameVersion.Spanish ? 0x80013790u : GameVersion.French ? 0x80013758u : GameVersion.German ? 0x80013738u : GameVersion.English ? 0x80013738u : 0x80013798u; // widescreen/LandMapPatch.cs:func_80012FDC_landmap
    public const uint Landmap_Code_80013D88 = GameVersion.Spanish ? 0x80013D80u : GameVersion.French ? 0x80013D48u : GameVersion.German ? 0x80013D28u : GameVersion.English ? 0x80013D28u : 0x80013D88u; // widescreen/LandMapPatch.cs:func_80013C8C
    public const uint Landmap_Code_80013D90 = GameVersion.Spanish ? 0x80013D88u : GameVersion.French ? 0x80013D50u : GameVersion.German ? 0x80013D30u : GameVersion.English ? 0x80013D30u : 0x80013D90u; // widescreen/LandMapPatch.cs:func_80013C8C
    public const uint Landmap_Code_80013D98 = GameVersion.Spanish ? 0x80013D90u : GameVersion.French ? 0x80013D58u : GameVersion.German ? 0x80013D38u : GameVersion.English ? 0x80013D38u : 0x80013D98u; // widescreen/LandMapPatch.cs:func_80013C3C
    public const uint Landmap_Code_80013DA0 = GameVersion.Spanish ? 0x80013D98u : GameVersion.French ? 0x80013D60u : GameVersion.German ? 0x80013D40u : GameVersion.English ? 0x80013D40u : 0x80013DA0u; // widescreen/LandMapPatch.cs:func_80013C3C
    public const uint Landmap_Code_80013DBC = GameVersion.Spanish ? 0x80013DB4u : GameVersion.French ? 0x80013D7Cu : GameVersion.German ? 0x80013D5Cu : GameVersion.English ? 0x80013D5Cu : 0x80013DBCu; // widescreen/LandMapPatch.cs:func_80013C3C
    public const uint Landmap_Code_80013FF0 = GameVersion.Spanish ? 0x80013FE8u : GameVersion.French ? 0x80013FB0u : GameVersion.German ? 0x80013F90u : GameVersion.English ? 0x80013F90u : 0x80013FF0u; // widescreen/LandMapPatch.cs:func_80013C8C
    public const uint Tl_8001097C = GameVersion.Spanish ? 0x2360u : GameVersion.French ? 0x229Cu : GameVersion.German ? 0x2464u : GameVersion.English ? 0x2604u : 0x23D8u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010984, istr. FRA 80010984, istr. DEU 8001097C, istr. ENG 80010984
    public const uint Tl_80010988 = GameVersion.Spanish ? 0x2360u : GameVersion.French ? 0x229Cu : GameVersion.German ? 0x2464u : GameVersion.English ? 0x2604u : 0x23D8u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010990, istr. FRA 80010990, istr. DEU 80010988, istr. ENG 80010990
    public const uint Tl_800109EC = GameVersion.Spanish ? 0x0F40u : GameVersion.French ? 0x0E7Cu : GameVersion.German ? 0x1044u : GameVersion.English ? 0x11E4u : 0x0FB8u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 800109F4, istr. FRA 800109F4, istr. DEU 800109EC, istr. ENG 800109F4
    public const uint Tl_80010A1C = GameVersion.Spanish ? 0x7ED8u : GameVersion.French ? 0x7ED8u : GameVersion.German ? 0x7EE0u : GameVersion.English ? 0x7ED8u : 0x7EE0u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010A24, istr. FRA 80010A24, istr. DEU 80010A1C, istr. ENG 80010A24
    public const uint Tl_80010A7C = GameVersion.Spanish ? 0x0EB4u : GameVersion.French ? 0x0DF0u : GameVersion.German ? 0x0FB8u : GameVersion.English ? 0x1158u : 0x0F2Cu; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010A84, istr. FRA 80010A84, istr. DEU 80010A7C, istr. ENG 80010A84
    public const uint Tl_80010C3C = GameVersion.Spanish ? 0x14D4u : GameVersion.French ? 0x15ACu : GameVersion.German ? 0x13D4u : GameVersion.English ? 0x1218u : 0x1454u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010C44, istr. FRA 80010C44, istr. DEU 80010C3C, istr. ENG 80010C44
    public const uint Tl_80010D90 = GameVersion.Spanish ? 0x0F08u : GameVersion.French ? 0x0E44u : GameVersion.German ? 0x100Cu : GameVersion.English ? 0x11ACu : 0x0F80u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010D98, istr. FRA 80010D98, istr. DEU 80010D90, istr. ENG 80010D98
    public const uint Tl_80010E10 = GameVersion.Spanish ? 0x0F08u : GameVersion.French ? 0x0E44u : GameVersion.German ? 0x100Cu : GameVersion.English ? 0x11ACu : 0x0F80u; // render/CullPatch.cs:func_80010800_tl, istr. ESP 80010E18, istr. FRA 80010E18, istr. DEU 80010E10, istr. ENG 80010E18
    public const uint Tl_Code_800104BC = GameVersion.Spanish ? 0x800104C4u : GameVersion.French ? 0x800104C4u : GameVersion.German ? 0x800104BCu : GameVersion.English ? 0x800104C4u : 0x800104BCu; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_800105B8 = GameVersion.Spanish ? 0x800105C0u : GameVersion.French ? 0x800105C0u : GameVersion.German ? 0x800105B8u : GameVersion.English ? 0x800105C0u : 0x800105B8u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_8001084C = GameVersion.Spanish ? 0x80010854u : GameVersion.French ? 0x80010854u : GameVersion.German ? 0x8001084Cu : GameVersion.English ? 0x80010854u : 0x8001084Cu; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_800108A0 = GameVersion.Spanish ? 0x800108A8u : GameVersion.French ? 0x800108A8u : GameVersion.German ? 0x800108A0u : GameVersion.English ? 0x800108A8u : 0x800108A0u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_80010A78 = GameVersion.Spanish ? 0x80010A80u : GameVersion.French ? 0x80010A80u : GameVersion.German ? 0x80010A78u : GameVersion.English ? 0x80010A80u : 0x80010A78u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_80010B14 = GameVersion.Spanish ? 0x80010B1Cu : GameVersion.French ? 0x80010B1Cu : GameVersion.German ? 0x80010B14u : GameVersion.English ? 0x80010B1Cu : 0x80010B14u; // render/CullPatch.cs:func_80010800_tl
    public const uint Tl_Code_80010F00 = GameVersion.Spanish ? 0x80010F08u : GameVersion.French ? 0x80010F08u : GameVersion.German ? 0x80010F00u : GameVersion.English ? 0x80010F08u : 0x80010F00u; // render/CullPatch.cs:func_80010800_tl
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
#elif LINGUA_DE
        => MediEvil_game.func_80021DCC(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_80021E0C(c, m);
#else
        => MediEvil_game.func_80021E34(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_80021E5C(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_80021EC4(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_80021DF4(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_80021E34(c, m);
#else
        => MediEvil_game.func_80021E5C(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_80022070(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800220D8(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_80022008(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_80022048(c, m);
#else
        => MediEvil_game.func_80022070(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_80022460(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800224C8(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800223F8(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_80022438(c, m);
#else
        => MediEvil_game.func_80022460(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_80022AF0(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_80022B58(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_80022A88(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_80022AC8(c, m);
#else
        => MediEvil_game.func_80022AF0(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800494C8(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_80049530(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_80049460(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800494A0(c, m);
#else
        => MediEvil_game.func_800494C8(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_8009A540(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_8009A5A8(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_8009A4D8(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_8009A538(c, m);
#else
        => MediEvil_game.func_8009A540(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_8009A694(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_8009A6FC(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_8009A62C(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_8009A68C(c, m);
#else
        => MediEvil_game.func_8009A694(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A2E90(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A2EF8(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800A2E28(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800A2E88(c, m);
#else
        => MediEvil_game.func_800A2E90(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4264(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A42CC(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800A41FC(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800A425C(c, m);
#else
        => MediEvil_game.func_800A4264(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4434(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A449C(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800A43CC(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800A442C(c, m);
#else
        => MediEvil_game.func_800A4434(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4464(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A44CC(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800A43FC(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800A445C(c, m);
#else
        => MediEvil_game.func_800A4464(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A48FC(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A4964(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800A4894(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800A48F4(c, m);
#else
        => MediEvil_game.func_800A48FC(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4D78(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A4DE0(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800A4D10(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800A4D70(c, m);
#else
        => MediEvil_game.func_800A4D78(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4E88(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A4EF0(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800A4E20(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800A4E80(c, m);
#else
        => MediEvil_game.func_800A4E88(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4EB8(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A4F20(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800A4E50(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800A4EB0(c, m);
#else
        => MediEvil_game.func_800A4EB8(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800A4FC8(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800A5030(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800A4F60(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800A4FC0(c, m);
#else
        => MediEvil_game.func_800A4FC8(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800ABA38(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800ABAA0(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800AB9D0(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800ABA30(c, m);
#else
        => MediEvil_game.func_800ABA38(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800AF490(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800AF4F8(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800AF428(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800AF488(c, m);
#else
        => MediEvil_game.func_800AF490(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B00E4(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B014C(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B007C(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B00DC(c, m);
#else
        => MediEvil_game.func_800B00E4(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B02BC(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B0324(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B0254(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B02B4(c, m);
#else
        => MediEvil_game.func_800B02BC(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B04D4(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B053C(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B046C(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B04CC(c, m);
#else
        => MediEvil_game.func_800B04D4(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B06D8(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B0740(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B0670(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B06D0(c, m);
#else
        => MediEvil_game.func_800B06D8(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B097C(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B09E4(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B0914(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B0974(c, m);
#else
        => MediEvil_game.func_800B097C(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B0B3C(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B0BA4(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B0AD4(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B0B34(c, m);
#else
        => MediEvil_game.func_800B0B3C(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B0D34(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B0D9C(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B0CCC(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B0D2C(c, m);
#else
        => MediEvil_game.func_800B0D34(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B0F20(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B0F88(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B0EB8(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B0F18(c, m);
#else
        => MediEvil_game.func_800B0F20(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B50BC(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B5124(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B5054(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B50B4(c, m);
#else
        => MediEvil_game.func_800B50BC(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B5A44(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B5AAC(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B59DC(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B5A3C(c, m);
#else
        => MediEvil_game.func_800B5A44(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B5E20(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B5E88(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B5DB8(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B5E18(c, m);
#else
        => MediEvil_game.func_800B5E20(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B62B4(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B631C(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B624C(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B62AC(c, m);
#else
        => MediEvil_game.func_800B62B4(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B67A0(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B6808(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B6738(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B6798(c, m);
#else
        => MediEvil_game.func_800B67A0(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B6EEC(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B6F54(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B6E84(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B6EE4(c, m);
#else
        => MediEvil_game.func_800B6EEC(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B72A0(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B7308(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B7238(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B7298(c, m);
#else
        => MediEvil_game.func_800B72A0(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B7738(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B77A0(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B76D0(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B7730(c, m);
#else
        => MediEvil_game.func_800B7738(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B7A80(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B7AE8(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B7A18(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B7A78(c, m);
#else
        => MediEvil_game.func_800B7A80(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B7E38(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B7EA0(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B7DD0(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B7E30(c, m);
#else
        => MediEvil_game.func_800B7E38(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B8304(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B836C(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B829C(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B82FC(c, m);
#else
        => MediEvil_game.func_800B8304(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B8A24(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B8A8C(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B89BC(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B8A1C(c, m);
#else
        => MediEvil_game.func_800B8A24(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800B90E8(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800B9150(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800B9080(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800B90E0(c, m);
#else
        => MediEvil_game.func_800B90E8(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800BFE04(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800BFE6C(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800BFD9C(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800BFDFC(c, m);
#else
        => MediEvil_game.func_800BFE04(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800C0028(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800C0090(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800BFFC0(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800C0020(c, m);
#else
        => MediEvil_game.func_800C0028(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800C00F4(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800C015C(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800C008C(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800C00EC(c, m);
#else
        => MediEvil_game.func_800C00F4(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800C0A64(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800C0ACC(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800C09FC(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800C0A5C(c, m);
#else
        => MediEvil_game.func_800C0A64(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800C0E0C(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800C0E74(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800C0DA4(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800C0E04(c, m);
#else
        => MediEvil_game.func_800C0E0C(c, m);
#endif
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Game_800C0F24(RecompOne.Runtime.Context.CpuContext c, RecompOne.Runtime.Memory.IMemory m)
#if LINGUA_FR
        => MediEvil_game.func_800C0F8C(c, m);
#elif LINGUA_DE
        => MediEvil_game.func_800C0EBC(c, m);
#elif LINGUA_EN
        => MediEvil_game.func_800C0F1C(c, m);
#else
        => MediEvil_game.func_800C0F24(c, m);
#endif
}
