<!--
  MediEvilRecomp DEU - Anleitung fuer die Dateien der Disc
  Nach disc/LEEME.md von https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# Ordner der Disc

Zum Kompilieren und Spielen brauchen Sie **Ihre** Original-Disc von **MediEvil, deutsche PAL-Version (SCES-01493)**. Das Projekt enthält und verbreitet keine Dateien des Spiels, sie sind urheberrechtlich geschützt: Sie müssen sie selbst aus Ihrer Originalkopie erstellen.

Mit dem Launcher (`MediEvilLauncher.exe`) ist dieser Ordner nicht nötig: dort wählen Sie die Disc (`.cue` oder `.chd`) in einem beliebigen Ordner aus.

## Benötigtes Format

Das Abbild muss im Format **bin/cue** (MODE2/2352) und **unverändert** sein, also identisch mit der Original-Disc und ohne Patches. Das ist das Format der Redump-Abbilder.

Kopieren Sie diese drei Dateien mit genau diesen Namen hierher:

- `MediEvil (Germany) (Track 1).bin`
- `MediEvil (Germany) (Track 2).bin`
- `MediEvil (Germany).cue`

Die `.cue`-Datei nennt die `.bin`-Dateien beim Namen. Wenn Sie die `.bin`-Dateien umbenennen, ändern Sie auch die `FILE`-Zeilen in der `.cue`.

## LibCrypt

Die deutsche PAL-Disc ist mit **LibCrypt** geschützt: 16 Sektoren der Disc haben einen veränderten Subcode, aus dem das Spiel den Schlüssel für das Mausoleums-Level (**Hilltop Mausoleum**) gewinnt. Das bin/cue-Format enthält den Subcode nicht.

Es ist nichts zu tun: die Liste der geschützten Sektoren von SCES-01493 steht bereits in `patches/PalLibCrypt.cs`. Liegt in diesem Ordner eine Datei `MediEvil (Germany).sbi` (oder `.lsd`), dieselbe wie bei Emulatoren, wird sie statt der eingebauten Liste verwendet.

## Prüfungen

- Die Größe jeder `.bin` muss ein genaues Vielfaches von 2352 Byte sein.
- SHA-256 der Dateien einer korrekten Disc:
  - `SYSTEM.CNF`: `d8aa77b472f441d2558937fd1c1e55704a3d1ab83b12994130634a92c76ac528`
  - `SCES_014.93`: `e6bf1bf90ee2f4aac170f426bb573e3dabea861c39b275da36539cb8dc2818d6`
  - `MEDIEVIL.EXE`: `a4177d74b460f12dbb271cbfb34a52bf7833fb49126b97558084384b64dd0cbe`
- Andere Versionen funktionieren nicht mit der deutschen Konfiguration (`config/de/MediEvil.json`).

## License

GPL-3.0-or-later.
