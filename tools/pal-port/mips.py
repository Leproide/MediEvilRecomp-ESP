# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvil PAL port tools - MIPS R3000 helpers
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

"""Decoding / normalisation helpers for PS1 (MIPS I) code.

Normalisation removes everything that depends on *where* code and data
live (jal targets, lui/lo absolute address halves), so the same function
compiled into the USA and PAL images produces the same word sequence.
"""

import struct

# I-type opcodes whose 16-bit immediate can be the low half of an absolute address
LO_OPS = {0x08, 0x09, 0x0D,                    # addi, addiu, ori
          0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26,  # lb lh lwl lw lbu lhu lwr
          0x28, 0x29, 0x2A, 0x2B, 0x2E,        # sb sh swl sw swr
          0x32, 0x3A}                          # lwc2 swc2
STORE_OPS = {0x28, 0x29, 0x2A, 0x2B, 0x2E, 0x3A}
BRANCH_OPS = {0x01, 0x04, 0x05, 0x06, 0x07}


class Image:
    """A flat binary loaded at a fixed virtual base address."""

    def __init__(self, data: bytes, base: int):
        self.data = data
        self.base = base
        self.end = base + len(data)

    @classmethod
    def from_psx_exe(cls, path):
        raw = open(path, 'rb').read()
        if raw[:8] != b'PS-X EXE':
            raise ValueError(f'{path}: not a PS-X EXE')
        base = struct.unpack_from('<I', raw, 0x18)[0]
        return cls(raw[0x800:], base)

    @classmethod
    def from_raw(cls, path, base):
        return cls(open(path, 'rb').read(), base)

    def contains(self, addr):
        return self.base <= addr < self.end

    def word(self, addr):
        return struct.unpack_from('<I', self.data, addr - self.base)[0]

    def words(self, addr, size):
        off = addr - self.base
        n = size // 4
        return list(struct.unpack_from(f'<{n}I', self.data, off))


def sext16(v):
    return v - 0x10000 if v & 0x8000 else v


def written_reg(w):
    """Register written by instruction w (None if none / r0)."""
    op = w >> 26
    if op == 0:
        funct = w & 0x3F
        if funct in (0x08, 0x18, 0x19, 0x1A, 0x1B, 0x11, 0x13, 0x0C, 0x0D):
            return None  # jr, mult*, div*, mthi/lo, syscall, break
        rd = (w >> 11) & 31
        return rd or None
    if op == 0x03:
        return 31
    if op in BRANCH_OPS or op == 0x02 or op in STORE_OPS:
        return None
    if op in (0x10, 0x12):  # cop0/cop2 moves to GPR (mfc/cfc)
        rs = (w >> 21) & 31
        if rs in (0x00, 0x02):
            return ((w >> 16) & 31) or None
        return None
    if op in (0x32,):
        return None
    rt = (w >> 16) & 31
    return rt or None


GP = 28


def analyse(words, base, gp=None, ptr=None):
    """Normalise a function body and extract absolute address references.

    Returns (norm, refs) where
      norm : list of normalised words
      refs : list of (lo_index, hi_index, absolute_address)
    jal targets are returned as refs with hi_index = -1,
    $gp-relative accesses (when gp is given) with hi_index = -2,
    raw pointer words (jump tables, when ptr=(lo, hi) is given) with hi_index = -3.
    """
    norm = []
    refs = []
    hi = {}  # reg -> (index of lui, upper value)
    for i, w in enumerate(words):
        op = w >> 26
        rs = (w >> 21) & 31
        rt = (w >> 16) & 31
        n = w
        if ptr is not None and ptr[0] <= w < ptr[1] and not (w & 3):
            # embedded pointer (switch jump table, etc.)
            refs.append((i, -3, w))
            norm.append(0x80000000)
            continue
        if op in (0x02, 0x03):
            target = (((base + i * 4) & 0xF0000000) | ((w & 0x3FFFFFF) << 2))
            refs.append((i, -1, target))
            n = w & 0xFC000000
        elif op == 0x0F:  # lui
            n = w & 0xFFFF0000
            hi[rt] = (i, (w & 0xFFFF) << 16)
            norm.append(n)
            continue
        elif op in LO_OPS and rs in hi:
            hidx, hval = hi[rs]
            lo = (w & 0xFFFF) if op == 0x0D else sext16(w & 0xFFFF)
            refs.append((i, hidx, (hval + lo) & 0xFFFFFFFF))
            n = w & 0xFFFF0000
        elif op in LO_OPS and rs == GP and gp is not None:
            refs.append((i, -2, (gp + sext16(w & 0xFFFF)) & 0xFFFFFFFF))
            n = w & 0xFFFF0000
        norm.append(n)
        wr = written_reg(w)
        if wr is not None and wr in hi:
            del hi[wr]
    return norm, refs


def coarse(w):
    """Context-free normalisation (used only to index candidate positions)."""
    op = w >> 26
    if op in (0x02, 0x03):
        return w & 0xFC000000
    if op == 0x0F or op in LO_OPS:
        return w & 0xFFFF0000
    return w


def loose(w):
    """Structure-only normalisation: drops every immediate / shift amount.

    Two functions equal under this are the same code with different
    constants (PAL 50 Hz timing values, struct offsets, addresses).
    """
    op = w >> 26
    if op in (0x02, 0x03):
        return w & 0xFC000000
    if op == 0:
        return w & ~(0x1F << 6)
    if op in BRANCH_OPS:
        return w
    return w & 0xFFFF0000
