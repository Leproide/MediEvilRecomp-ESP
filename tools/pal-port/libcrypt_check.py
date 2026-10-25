#!/usr/bin/env python3
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvil PAL port tools - LibCrypt check simulator (ITA / ESP)
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

"""Offline check of the LibCrypt fix (patches/PalLibCrypt.cs).

Reproduces what the game does at 0x80065D20..0x80065FC0 with the sector
positions that PalLibCrypt.cs makes GetlocP report:
  - for every entry of the sector table (4 BCD bytes: mm, ss, start, target
    frame) the bit is 1 when the target sector is protected (GetlocP reports
    target+1), 0 otherwise (GetlocP reports the target);
  - key = (key << 1) | bit; after 16 entries the key is accepted only with
    exactly 8 bits set, otherwise the next 16 entries are read;
then, with MEDRES.MWD, decrypts the first sector of resource 0xD6 like
func_80065FC4 (XOR of every word with key<<16 + key) and checks that it is a
"PP20" stream whose trailing size matches the resource table and that it
decrunches to exactly that many bytes.

Addresses come from config/versions.json. The protected sectors come from a
.sbi/.lsd file or, by default, from the BuiltIn list in patches/PalLibCrypt.cs.

Usage (repository root):
  python3 tools/pal-port/libcrypt_check.py --ver esp --exe MEDIEVIL.EXE [--sbi X.sbi] [--mwd MEDRES.MWD]
The game files are only read; never commit them.
"""

import argparse
import json
import os
import re
import struct
import sys

EXE_BASE = 0x80021D14     # MEDIEVIL.EXE load address (same in ITA and ESP)
MAUSOLEUM = 0xD6          # resource decrypted by func_80065FC4 (called at 0x80065A78)


def bcd(b):
    return (b >> 4) * 10 + (b & 0x0F)


def msf(mm, ss, ff):
    return (bcd(mm) * 60 + bcd(ss)) * 75 + bcd(ff)


def parse_sbi(data):
    if data[:3] != b'SBI':
        raise ValueError('not a SBI file')
    out, o = set(), 4
    while o + 4 <= len(data):
        out.add(msf(data[o], data[o + 1], data[o + 2]) - 150)
        t = data[o + 3]
        o += 4 + (10 if t == 1 else 3)
    return out


def parse_lsd(data):
    return {msf(data[o], data[o + 1], data[o + 2]) - 150 for o in range(0, len(data) - 14, 15)}


def builtin(root, ver):
    """BuiltIn list of PalLibCrypt.cs for the version (BuiltInIta / BuiltInEsp)."""
    text = open(os.path.join(root, 'patches', 'PalLibCrypt.cs'), encoding='utf-8').read()
    name = 'BuiltInEsp' if ver == 'esp' else 'BuiltInIta'
    m = re.search(name + r'\s*=[^{]*\{([^}]*)\}', text)
    return {int(x) for x in re.findall(r'\d+', m.group(1))}


def simulate(table, protected, log):
    """Key computed by the game (0x80065E8C) with the positions PalLibCrypt reports."""
    key = count = idx = 0
    while True:
        mm, ss, _start, tf = table[idx * 4:idx * 4 + 4]
        target = msf(mm, ss, tf) - 150
        bit = 1 if target in protected else 0
        key = (key << 1) | bit
        count += bit
        idx += 1
        log(f'  entry {idx - 1:2}: {bytes(table[(idx - 1) * 4:idx * 4]).hex()}  LBA {target:6}  '
            f'GetlocP {target + bit:6}  bit {bit}')
        if idx == 16 and (key == 0 or count == 8):
            return key & 0xFFFF, count, idx
        if idx % 16 == 0 and idx != 16:
            return key & 0xFFFF, count, idx


def main():
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument('--repo', default='.', help='repository root')
    ap.add_argument('--ver', choices=('ita', 'esp'), required=True)
    ap.add_argument('--exe', required=True, help='MEDIEVIL.EXE of that version')
    ap.add_argument('--sbi', help='.sbi or .lsd file (default: BuiltIn list of PalLibCrypt.cs)')
    ap.add_argument('--mwd', help='MEDRES.MWD of that version (enables the PP20 check)')
    ap.add_argument('-q', action='store_true', help='do not list the entries')
    a = ap.parse_args()

    table_json = json.load(open(os.path.join(a.repo, 'config', 'versions.json'), encoding='utf-8'))
    c = {x['name']: int(x[a.ver], 16) for x in table_json['constants']}
    exe = open(a.exe, 'rb').read()
    if exe[:8] != b'PS-X EXE':
        sys.exit('not a PS-X EXE')
    body = exe[0x800:]

    def mem(addr, n):
        return body[addr - EXE_BASE:addr - EXE_BASE + n]

    if a.sbi:
        raw = open(a.sbi, 'rb').read()
        prot = parse_lsd(raw) if a.sbi.lower().endswith('.lsd') else parse_sbi(raw)
        src = os.path.basename(a.sbi)
    else:
        prot = builtin(a.repo, a.ver)
        src = 'BuiltIn ' + a.ver
    print(f'{a.ver}: {len(prot)} protected sectors ({src}): {sorted(prot)}')

    table = mem(c['LcTableBase'], 0x80)
    key, count, n = simulate(table, prot, (lambda s: None) if a.q else print)
    ok = n == 16 and count == 8
    print(f'key = 0x{key:04X}, bits set = {count} in {n} entries -> '
          + ('accepted in the first 16' if ok else 'NOT accepted in the first 16'))
    bad = not ok

    if a.mwd:
        mwd = open(a.mwd, 'rb').read()
        e = struct.unpack('<8I', mem(c['MwdTable'] + 32 * MAUSOLEUM, 32))
        sector, packed, info = e[3], e[6], e[7]
        print(f'resource 0x{MAUSOLEUM:02X}: flags {e[1]:#x}, sector {sector:#x}, packed {packed} bytes, '
              f'unpacked {info & 0xFFFFFF} bytes')
        blk = bytearray(mwd[sector * 2048:sector * 2048 + packed])
        mask = ((key << 16) + key) & 0xFFFFFFFF
        for i in range(0, 2048, 4):
            struct.pack_into('<I', blk, i, struct.unpack_from('<I', blk, i)[0] ^ mask)
        head_ok = blk[:4] == b'PP20'
        tail = int.from_bytes(blk[packed - 4:packed - 1], 'big')
        print(f'decrypted head: {bytes(blk[:8]).hex()} ({"PP20" if head_ok else "NOT PP20"}); '
              f'PP20 trailing size {tail}, index {info & 0xFFFFFF}')
        bad |= not head_ok or tail != (info & 0xFFFFFF)
        if head_ok:
            sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
            from pp20 import unpack
            try:
                out = unpack(bytes(blk))
                print(f'decrunched: {len(out)} bytes')
                bad |= len(out) != (info & 0xFFFFFF)
            except Exception as ex:
                print('decrunch failed:', ex)
                bad = True

    print('\nRESULT:', 'OK' if not bad else 'FAIL')
    return 1 if bad else 0


if __name__ == '__main__':
    sys.exit(main())
