#!/usr/bin/env python3
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvilRecomp ITA - immagine disco minima per la compilazione automatica (CI)
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

"""Crea un'immagine ISO 9660 (MODE1/2048) con i soli file che servono al
recompiler: SYSTEM.CNF, eseguibile di avvio, MEDIEVIL.EXE e OVERLAYS/*.BIN.
Serve alla compilazione automatica su GitHub, che non ha il disco completo:
il codice viene generato solo dagli eseguibili, la musica e i filmati non servono.

Uso: make_disc.py <cartella con i file estratti> <percorso del .cue da creare>
"""

import os
import sys

import pycdlib


def main():
    if len(sys.argv) != 3:
        sys.exit(__doc__)
    src, cue = sys.argv[1], sys.argv[2]
    out_dir = os.path.dirname(os.path.abspath(cue))
    os.makedirs(out_dir, exist_ok=True)
    bin_name = os.path.splitext(os.path.basename(cue))[0] + ' (Track 1).bin'

    iso = pycdlib.PyCdlib()
    iso.new(interchange_level=1, sys_ident='PLAYSTATION', vol_ident='MEDIEVIL')
    added = 0
    for name in sorted(os.listdir(src)):
        path = os.path.join(src, name)
        if os.path.isfile(path) and not name.startswith('.') and name.upper() == name:
            iso.add_file(path, '/' + name + ';1')
            added += 1
    ov = os.path.join(src, 'OVERLAYS')
    if os.path.isdir(ov):
        iso.add_directory('/OVERLAYS')
        for name in sorted(os.listdir(ov)):
            if name.upper().endswith('.BIN'):
                iso.add_file(os.path.join(ov, name), '/OVERLAYS/' + name.upper() + ';1')
                added += 1
    if added == 0:
        sys.exit(f'nessun file del gioco trovato in {src}')
    iso.write(os.path.join(out_dir, bin_name))
    iso.close()

    with open(cue, 'w', newline='\n') as f:
        f.write(f'FILE "{bin_name}" BINARY\n  TRACK 01 MODE1/2048\n    INDEX 01 00:00:00\n')
    print(f'{added} file -> {cue}')


if __name__ == '__main__':
    main()
