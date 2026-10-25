# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvil PAL port tools - PowerPacker "PP20" decruncher (verification only)
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

"""Classic PowerPacker decrunch algorithm. Input: "PP20" + 4 efficiency bytes +
packed bits + 3-byte big-endian unpacked size + skip-bit count. Used by
libcrypt_check.py to prove that a decrypted stream is complete."""


def unpack(data):
    assert data[:4]==b'PP20'
    eff=data[4:8]; src=data[8:]
    outlen=int.from_bytes(data[-4:-1],'big'); skip=data[-1]
    buf=len(src)-4; state={'bb':0,'bl':0,'buf':buf}
    def rd(n):
        while state['bl']<n:
            state['buf']-=1
            if state['buf']<0: raise ValueError('underrun')
            state['bb']|=src[state['buf']]<<state['bl']; state['bl']+=8
        v=0; state['bl']-=n
        for _ in range(n):
            v=(v<<1)|(state['bb']&1); state['bb']>>=1
        return v
    out=bytearray(outlen); o=outlen
    rd(skip)
    while o>0:
        if rd(1)==0:
            todo=1
            while True:
                x=rd(2); todo+=x
                if x!=3: break
            while todo>0 and o>0:
                o-=1; out[o]=rd(8); todo-=1
            if o==0: break
        x=rd(2); offbits=eff[x]; todo=x+2
        if x==3:
            if rd(1)==0: offbits=7
            off=rd(offbits)
            while True:
                y=rd(3); todo+=y
                if y!=7: break
        else:
            off=rd(offbits)
        while todo>0 and o>0:
            out[o-1]=out[o+off]; o-=1; todo-=1
    return bytes(out)
