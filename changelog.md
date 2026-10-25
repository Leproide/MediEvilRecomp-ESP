# MediEvilRecomp ITA - dialoghi XA e filmati a schermo intero

## Correzioni
- Il primo discorso del gargoyle nella cripta si chiudeva da solo se il filmato iniziale veniva saltato subito: un marcatore di fine traccia della musica veniva preso per la fine del discorso. Nuovo `patches/PalXaSpeech.cs`, agganciato in `config/MediEvil.json`.
- Mod **Filmati a schermo intero** 1.1: saltando un filmato dopo qualche secondo, il gioco poteva restare con l'immagine tagliata. Ora lo stato del lettore di avvio e quello del gioco sono separati.
- Dopo l'aggiornamento serve rigenerare il codice: cancella la cartella `generated` e rilancia `windows_initial_build.bat`, una volta completato lancia `windows_run.bat`

Autore: https://github.com/Leproide

---

# MediEvilRecomp ITA - filmati a schermo intero

## Novità
- Nuova mod opzionale **Filmati a schermo intero** (`mods/FilmatiSchermoIntero/`): durante i filmati mostra solo l'area del video (320×176) e la adatta alla finestra senza deformarla. Si attiva dal pannello delle mod e non richiede di ricompilare.

Autore: https://github.com/Leproide

---

# MediEvilRecomp ITA - filmati e dissolvenze

## Correzioni
- Filmati saltati dopo un secondo o due (issue #5 del progetto originale): il lettore dei filmati considerava finito il filmato dopo circa 4 milioni di tentativi a vuoto, che su PC durano una frazione di secondo. Ora l'attesa è misurata in tempo reale (2 secondi). Nuovo `patches/PalMovies.cs`.
- Dissolvenza a iride che in widescreen non copriva gli angoli (issue #7 del progetto originale): il bordo esterno della dissolvenza viene allargato in proporzione al margine widescreen. Nuovo `patches/widescreen/IrisFadePatch.cs`.
- Filmati spostati in basso: RecompOne usava 240 righe anche in PAL quando il gioco non indica l'altezza dello schermo. Ora sono 256. Nuovo `patches/PalDisplay.cs`.
- Dopo l'aggiornamento serve rigenerare il codice: cancella la cartella `generated` e rilancia `windows_initial_build.bat`.

Autore: https://github.com/Leproide

---

# MediEvilRecomp ITA - correzione mappa dopo la Sala degli Eroi

## Correzioni
- La mappa dopo la Sala degli Eroi andava a circa 1 fotogramma al secondo e i comandi non rispondevano; scegliendo il livello il gioco poi si bloccava.
- Causa 1: lo `StUnSetRing` HLE di RecompOne non azzera i callback del CD come quello originale, quindi dopo un filmato restava attivo `StCdInterrupt`.
- Causa 2: il gestore della musica XA del gioco scrive i callback direttamente nelle variabili di libcd (`0x800EB660`/`0x800EB664`) e RecompOne non le vedeva.
- Nuovo `patches/PalCdCallbacks.cs`, agganciato in `config/MediEvil.json`: azzera i callback dopo `StUnSetRing` e tiene allineate le variabili del gioco con quelle di RecompOne.
- Dopo l'aggiornamento serve rigenerare il codice: cancella la cartella `generated` e rilancia `windows_initial_build.bat`.

Autore: https://github.com/Leproide

---

# MediEvilRecomp ITA - port PAL italiano (SCES-01494)

Prima versione del fork per MediEvil PAL italiano.

## Novità
- Configurazione e funcmap tradotte per la versione PAL italiana: boot `SCES_014.94`, `MEDIEVIL.EXE` e i 26 overlay.
- Patch di rendering e widescreen (`CullPatch`, `EntityFloatFunctions`, `TerrainPatch`, `TerrainCapture`, `LandMapPatch`) portate sugli indirizzi PAL, con i valori a 50 Hz e la mappa a 256 righe.
- Conservati i nomi delle funzioni SDK, così RecompOne usa le sue implementazioni HLE di VSync, CdRead e simili.
- Nuovo `PalDispatch.cs`: le chiamate agli overlay passano per indirizzo e non dipendono dai suffissi dei nomi.
- Nuovo `PalDebug.cs`: log di debug attivabili con `MEDIEVIL_DEBUG`.
- Correzione di RecompOne per la modalità settore intero del lettore CD (`recompone-fixes/`), applicata in automatico dagli script.
- Script di compilazione in italiano, con i nomi dei file `(Italy)`.
- Aggiornamento automatico collegato alle release di questo fork.
- Strumenti di porting e di verifica in `tools/pal-port/`.
- Nuova mod **Trucchi (MediEvil ITA)** (`mods/Trucchi/`): attiva la Modalità Trucchi o la Modalità Trucchi Estesa dal pannello delle mod.

Autore del port: https://github.com/Leproide

---

# 

v0.2b* is not a massive update, just something that i wanted to fix way before the first release but havent had time! the rendering now should be fairly decent looking!

thanks for the support on the initial release! i hope you guys enjoy it!

# Changes
- ditched pgxp renderer in favor of a ""native"" rendering patch to use float, depth buffer and correct textures (the power of recomp patches!)
- fixed overworld map not rendering properly on 16:9
- fixed an issue with the clear buffer not covering 16:9
- added a "better rendering" option to improve the terrain/entity rendering and camera, interpolation is also improved
- added automatic updater
- included shadermod

# Dependencies

You're required to install [dotnet 10 runtime](https://dotnet.microsoft.com/pt-br/download/dotnet/10.0) to run the recomp, otherwise the game will not open, ideally also make sure to install [OpenAL](https://www.openal.org/downloads/), as of the current version no further dependencies are required

you can find instructions downloading dotnet 10 for linux systems that dont have it on their packages [here](https://wiki.archlinux.org/title/.NET), or you can run "run.sh" wich will install dotent if not available and execute sotn for you
