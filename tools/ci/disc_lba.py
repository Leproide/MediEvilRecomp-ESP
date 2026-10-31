#!/usr/bin/env python3
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvilRecomp - settori reali degli eseguibili sul disco (per fix_overlay_lba.py)
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

"""Legge il file system ISO 9660 di un "Track 1.bin" (MODE2/2352) e stampa la
tabella REAL_LBA per tools/ci/fix_overlay_lba.py: per MEDIEVIL.EXE il settore del
file + 1 (il recompiler salta l'intestazione da 2048 byte), per ogni overlay il
primo settore del file.

Basta anche solo l'inizio del .bin (le prime migliaia di settori, dove ci sono le
directory). Il disco viene solo letto.

Uso: disc_lba.py "<Track 1.bin>"
"""

import struct
import sys

RAW = 2352
DATA = 24  # MODE2 form 1: 12 sync + 4 header + 8 subheader


def sector(f, lba):
    f.seek(lba * RAW + DATA)
    data = f.read(2048)
    if len(data) < 2048:
        raise EOFError(f'settore {lba} oltre la fine del file')
    return data


def records(f, lba, size):
    """(nome, lba, dimensione, directory?) delle voci di una directory ISO 9660."""
    out = []
    for n in range((size + 2047) // 2048):
        d = sector(f, lba + n)
        o = 0
        while o < 2048 and d[o]:
            ln = d[o]
            ext = struct.unpack_from('<I', d, o + 2)[0]
            sz = struct.unpack_from('<I', d, o + 10)[0]
            flags = d[o + 25]
            nl = d[o + 32]
            name = d[o + 33:o + 33 + nl].decode('ascii', 'replace').split(';')[0]
            if name not in ('\x00', '\x01'):
                out.append((name.upper(), ext, sz, bool(flags & 2)))
            o += ln
    return out


def main():
    if len(sys.argv) != 2:
        sys.exit(__doc__)
    with open(sys.argv[1], 'rb') as f:
        pvd = sector(f, 16)
        if pvd[1:6] != b'CD001':
            sys.exit('descrittore ISO 9660 non trovato: serve un .bin MODE2/2352')
        root = pvd[156:156 + 34]
        root_lba, root_size = struct.unpack_from('<I', root, 2)[0], struct.unpack_from('<I', root, 10)[0]
        top = records(f, root_lba, root_size)
        lba = {}
        exe = next((r for r in top if r[0] == 'MEDIEVIL.EXE'), None)
        if not exe:
            sys.exit('MEDIEVIL.EXE non trovato')
        lba['game'] = exe[1] + 1
        ovd = next((r for r in top if r[0] == 'OVERLAYS' and r[3]), None)
        if not ovd:
            sys.exit('cartella OVERLAYS non trovata')
        for name, ext, _sz, isdir in records(f, ovd[1], ovd[2]):
            if not isdir and name.endswith('.BIN'):
                lba[name[:-4].lower()] = ext
    keys = ['game'] + sorted(k for k in lba if k != 'game')
    print('{')
    line = '    '
    for k in keys:
        item = f'"{k}": {lba[k]}, '
        if len(line) + len(item) > 96:
            print(line.rstrip())
            line = '    '
        line += item
    print(line.rstrip())
    print('}')


if __name__ == '__main__':
    main()
