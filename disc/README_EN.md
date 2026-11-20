<!--
  MediEvilRecomp ENG - instructions for the disc files
  After disc/LEEME.md by https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# Disc folder

To build and play you need **your own** original disc of **MediEvil, European English PAL version (SCES-00311)**. The project contains and distributes no game files, which are copyrighted: you have to dump them yourself from your original copy.

With the launcher (`MediEvilLauncher.exe`) this folder is not needed: you choose the disc (`.cue` or `.chd`) in any folder.

## Required format

The image must be **bin/cue** (MODE2/2352) and **clean**, identical to the original disc, without patches. This is the format of Redump dumps.

Copy these three files here, with exactly these names:

- `MediEvil (Europe) (Track 1).bin`
- `MediEvil (Europe) (Track 2).bin`
- `MediEvil (Europe).cue`

The `.cue` names the `.bin` files. If you rename the `.bin` files, update the `FILE` lines in the `.cue` too.

## LibCrypt

The English PAL disc is protected with **LibCrypt**: 16 sectors of the disc have a modified subcode, from which the game gets the key that opens the **Hilltop Mausoleum** level. The bin/cue format does not contain the subcode.

Nothing to do: the list of the protected sectors of SCES-00311 is already in `patches/PalLibCrypt.cs`. If this folder has a `MediEvil (Europe).sbi` (or `.lsd`) file, the same one emulators use, it is used instead of the built-in list. The log says which list was used (`[PalLibCrypt]`).

## Checks

- The size of each `.bin` must be an exact multiple of 2352 bytes.
- SHA-256 of the files of a correct disc:
  - `SYSTEM.CNF`: `4e97184f830a932da4b8cd1f2f0c91b9394850aa6c4d05afeec20dde0750a4a2`
  - `SCES_003.11`: `e6bf1bf90ee2f4aac170f426bb573e3dabea861c39b275da36539cb8dc2818d6`
  - `MEDIEVIL.EXE`: `770a4cdcb2375c0960820a110caebe375d6f6449b7908453753cf5f130dde093`
- Other versions do not work with the English configuration (`config/en/MediEvil.json`).

## License

GPL-3.0-or-later.
