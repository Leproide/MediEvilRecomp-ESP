<!--
  medievil-pal-port - README
  Author: https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# Strumenti di porting USA → PAL

Questi script hanno generato `config/`, `config/funcmaps/` e `patches/` di questo fork a partire dal progetto USA originale ([BlackLabelHQ/MediEvilRecomp](https://github.com/BlackLabelHQ/MediEvilRecomp)). Usali quando l'originale pubblica nuove patch o modifiche alla configurazione e vuoi riportarle sulla versione italiana.

Servono solo Python 3.8 o superiore, senza pacchetti esterni.

## Cosa fanno

1. **Abbinamento delle funzioni** (eseguibile di boot, `MEDIEVIL.EXE` e i 26 overlay). Ogni funzione USA riceve una di queste classificazioni:
   - *exact*: codice identico, cambiano solo gli indirizzi;
   - *aligned*: stessa struttura, ma alcune costanti cambiano (timing a 50 Hz, 240→256 righe, offset);
   - *fuzzy*: codice modificato nella PAL, localizzato tra le funzioni vicine già abbinate.

   Le funzioni identiche tra loro, come stub e tabelle, vengono piazzate insieme per conservarne l'ordine.
2. **Mappa degli indirizzi** USA→PAL:
   - per il codice, istruzione per istruzione;
   - per i dati, dai riferimenti `lui`/`lo`, da quelli relativi a `$gp` e dalle tabelle di salto, a maggioranza di voti.
3. **Output generato**:
   - **`MediEvil.json`**: base, entry point, entry point interni usati dalle patch, patch, rilocazioni, `.cue` e seriale.
   - **Funcmap**: indirizzi e dimensioni PAL. I **nomi reali** delle funzioni (`VSync`, `CdRead`, …) restano invariati, perché RecompOne li sostituisce con le sue versioni HLE in base al nome.
   - **Patch C#**: i letterali vengono allineati alle istruzioni che li producono, seguendo il formato di `InstructionEmitter` di RecompOne, e riscritti con i valori PAL. Etichette, chiamate e indirizzi di ritorno vengono rimappati, e così le costanti dati e gli offset `$gp`.
   - **`PalDispatch.cs`**, **`PalDebug.cs`** e la correzione di RecompOne in `recompone-fixes/`.
   - **`PORT_REPORT.md`**: statistiche e tutto ciò che richiede un controllo.
4. **Verifica** (`verify.py`):
   - riallinea il C# originale al binario USA e quello portato al binario PAL: i punteggi devono coincidere;
   - controlla indirizzi, ordine e confini delle funzioni, che non devono risultare peggiori dell'originale.

## Uso

Estrai i file da **entrambi** i dischi, USA e PAL italiano pulito (7-Zip, PowerISO, `dumpsxiso`, …):

```
usa/                         pal/
  SYSTEM.CNF                   SYSTEM.CNF
  SCUS_942.27                  SCES_014.94
  MEDIEVIL.EXE                 MEDIEVIL.EXE
  OVERLAYS/*.BIN               OVERLAYS/*.BIN
```

Poi, partendo da un clone dell'originale **USA**:

```
git clone --recursive https://github.com/BlackLabelHQ/MediEvilRecomp originale-usa
python3 tools/pal-port/port.py   --repo originale-usa --usa usa --pal pal --out pal-out --cue "../disc/MediEvil (Italy).cue"
python3 tools/pal-port/verify.py --repo originale-usa --usa usa --pal pal --out pal-out
```

Copia `pal-out/config`, `pal-out/patches` e `pal-out/recompone-fixes` nel fork, rigenera il codice e prova il gioco. `verify.py` deve terminare con `RESULT: OK`.

I file estratti dai dischi sono protetti da copyright: **non caricarli mai nel repository**.

## Altre versioni PAL (ITA → ESP): `version_port.py`

La versione spagnola (SCES-01495) ha lo stesso codice dell'italiana con dati spostati, quindi non ha un set di patch separato: tutti gli indirizzi che cambiano sono in `config/versions.json` (hook, entry point, rilocazioni, costanti con nome, immediati delle copie di funzioni ricompilate). Da lì vengono generati `patches/GameAddresses.cs` (scelta in compilazione con `-p:Lingua=es`, simbolo `LINGUA_ES`) e `config/es/MediEvil.json`.

```
python3 tools/pal-port/version_port.py check --ita ita --dst esp
python3 tools/pal-port/version_port.py gen-cs
python3 tools/pal-port/version_port.py gen-config
python3 tools/pal-port/version_port.py convert --ita ita --dst esp --force
```

- `check` confronta tabella, configurazioni e `GameAddresses.cs`. Con le cartelle dei due dischi verifica anche:
  - hook e destinazioni `jal`, che devono essere inizi di funzione;
  - ogni immediato della tabella, confrontato con l'istruzione nei due binari;
  - ogni costante con nome, confrontata con la mappa ITA → ESP.
- `gen-cs` / `gen-config` rigenerano i file dopo una modifica alla tabella.
- `convert` è il passo unico che ha creato la tabella: esegue `port.py` con il progetto italiano come origine e sostituisce i letterali nel C# con riferimenti alla tabella. `PORT_REPORT_ES.md` è il suo report.

## LibCrypt: `libcrypt_check.py`

Simula il controllo LibCrypt del gioco (0x80065D20..0x80065FC0) con le posizioni che `patches/PalLibCrypt.cs` fa riportare a GetlocP, calcola la chiave e, con `MEDRES.MWD`, decifra il primo settore della risorsa 0xD6. Poi controlla "PP20", la dimensione in coda al flusso confrontata con l'indice e la decompressione completa (`pp20.py`).

```
python3 tools/pal-port/libcrypt_check.py --ver esp --exe MEDIEVIL.EXE [--sbi X.sbi] --mwd MEDRES.MWD
```

Risultati: ITA chiave 0xAAA6; ESP chiave 0x0E57, "PP20", 315748 byte come nell'indice.

## File

| File | Contenuto |
|---|---|
| `libcrypt_check.py`, `pp20.py` | verifica offline della correzione LibCrypt |
| `version_port.py` | tabella degli indirizzi per versione (ITA / ESP): conversione, generatori, verifica |
| `port.py` | punto d'ingresso: matching, mappa, generazione di config, funcmap e patch |
| `matcher.py` | abbinamento delle funzioni USA→PAL |
| `addrmap.py` | mappa degli indirizzi di codice e dati |
| `csport.py` | traduzione delle patch C# |
| `mips.py` | decodifica e normalizzazione MIPS R3000 |
| `verify.py` | verifica indipendente del port |
| `templates/` | `PalDebug.cs.template` e la correzione di RecompOne copiati nell'output |
| `PORT_REPORT.md` | report dell'ultimo port |

## Licenza

GPL-3.0-or-later. Il testo completo è in [`LICENSE`](LICENSE).

## Autore

https://github.com/Leproide
