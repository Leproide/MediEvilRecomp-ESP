#!/usr/bin/env python3
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvilRecomp ITA - posizioni reali degli overlay per la compilazione automatica (CI)
# Copyright (C) 2026
# Author: https://github.com/Leproide
#
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <https://www.gnu.org/licenses/>.

"""Il runtime riconosce quale overlay il gioco sta caricando dal settore del disco
letto (LbaStart nel codice generato). L'immagine minima della CI (make_disc.py) ha
i file in posizioni diverse dal disco vero, quindi il recompiler scriverebbe settori
sbagliati e gli overlay non verrebbero mai attivati (crash all'avvio di un livello).
Questo script rimette in generated*/*.cs i settori del disco reale della versione:
PAL italiano (SCES-01494, "MediEvil (Italy) (Track 1).bin") o PAL spagnolo
(SCES-01495, "MediEvil (Spain) (Track 1).bin"), letti dal file system ISO del disco
(tools/ci/disc_lba.py).

Uso: fix_overlay_lba.py <cartella generated> [--ver ita|esp]
"""

import re
import sys
from pathlib import Path

# overlay -> primo settore sul disco reale (MEDIEVIL.EXE: settore del file + 1 per "skip": 2048)
REAL_LBA_ITA = {
    "game": 156163,
    "ac": 205212, "ag": 205238, "cc": 205257, "ch": 205277, "cr": 205290,
    "credits": 205294, "dc": 205300, "ee": 205328, "eh": 205358, "gg": 205365,
    "gs": 205372, "gy1": 205389, "gy2": 205399, "hh": 205420, "hr": 205445,
    "ia": 205470, "la": 205481, "landmap": 205497, "pd": 205507, "pg": 205519,
    "ps": 205538, "sf": 205564, "sv": 205591, "td": 205618, "tl": 205645,
    "zl": 205663,
}

PATTERN = re.compile(r"(public int LbaStart => )(-?\d+)(;)")


# PAL spagnolo (SCES-01495): stessi file, posizioni diverse sul disco
# Valori presi dalla generated-es compilata dal disco completo (LbaStart di ogni overlay).
# Gli overlay occupano i 486 settori tra 2AV.STR e MEDIEVIL.EXE (193090-193575); a
# differenza del disco italiano AG viene prima di AC. Ricavabili anche con
# tools/ci/disc_lba.py "<Track 1.bin>"
REAL_LBA_ESP = {
    "game": 193577, "ac": 193109, "ag": 193090, "cc": 193135, "ch": 193155, "cr": 193168,
    "credits": 193172, "dc": 193178, "ee": 193206, "eh": 193236, "gg": 193243,
    "gs": 193250, "gy1": 193267, "gy2": 193277, "hh": 193298, "hr": 193323, "ia": 193348,
    "la": 193359, "landmap": 193375, "pd": 193385, "pg": 193397, "ps": 193416,
    "sf": 193442, "sv": 193469, "td": 193496, "tl": 193523, "zl": 193541,
}

REAL_LBA = {"ita": REAL_LBA_ITA, "esp": REAL_LBA_ESP}


def main() -> int:
    args = sys.argv[1:]
    ver = "ita"
    if len(args) == 3 and args[1] == "--ver":
        ver = args[2]
        args = args[:1]
    if len(args) != 1 or ver not in REAL_LBA:
        print(__doc__)
        return 2
    table = REAL_LBA[ver]
    if table is None:
        print(f"ERRORE: settori del disco {ver} non ancora inseriti in {__file__}")
        return 1
    gen = Path(args[0])
    errors = 0
    for name, lba in table.items():
        path = gen / f"{name}.cs"
        if not path.is_file():
            print(f"ERRORE: manca {path}")
            errors += 1
            continue
        text = path.read_text(encoding="utf-8")
        found = PATTERN.findall(text)
        if len(found) != 1:
            print(f"ERRORE: {path}: trovate {len(found)} righe LbaStart invece di 1")
            errors += 1
            continue
        old = int(found[0][1])
        path.write_text(PATTERN.sub(lambda m: f"{m.group(1)}{lba}{m.group(3)}", text), encoding="utf-8")
        print(f"{name}: {old} -> {lba}")
    return 1 if errors else 0


if __name__ == "__main__":
    sys.exit(main())
