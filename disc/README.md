<!--
  MediEvilRecomp ITA - istruzioni per i file del disco
  Author: https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# Cartella del disco

Per compilare e giocare ti serve il **tuo** disco originale di **MediEvil, versione PAL italiana (SCES-01494)**. Il progetto non contiene e non distribuisce i file del gioco, che sono protetti da copyright: devi ricavarli tu dalla tua copia originale.

## Formato richiesto

Il dump deve essere in formato **bin/cue** (MODE2/2352) **pulito**, cioè identico al disco originale e senza patch. È lo stesso formato dei dump Redump.

Metti qui questi tre file, con esattamente questi nomi:

- `MediEvil (Italy) (Track 1).bin`
- `MediEvil (Italy) (Track 2).bin`
- `MediEvil (Italy).cue`

Il file `.cue` richiama i `.bin` per nome. Se rinomini i `.bin`, aggiorna anche le righe `FILE` dentro il `.cue`.

## LibCrypt

Il disco PAL italiano è protetto con **LibCrypt**: 16 settori del disco hanno un sottocodice modificato, da cui il gioco ricava la chiave per aprire il livello **Il Mausoleo in Cima alla Collina**. Il formato bin/cue non contiene il sottocodice.

Non serve fare nulla: l'elenco dei settori protetti di SCES-01494 è già incluso in `patches/PalLibCrypt.cs`. Se nella cartella c'è un file `MediEvil (Italy).sbi` (o `.lsd`), lo stesso che usano gli emulatori, viene usato quello al posto dell'elenco incluso.

All'avvio il log indica quale elenco è stato usato (`[PalLibCrypt]`).

## Controlli

- Ogni `.bin` deve avere una dimensione multipla esatta di 2352 byte.
- Altre versioni non funzionano con questo fork: la USA (SCUS-94227) usa il progetto originale, mentre la inglese europea e le altre versioni PAL hanno codice diverso.
- Non usare immagini con patch anti-LibCrypt: l'eseguibile di boot è modificato e la ricompilazione si blocca all'avvio.

## Licenza

GPL-3.0-or-later.

## Autore

https://github.com/Leproide
