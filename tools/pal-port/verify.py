#!/usr/bin/env python3
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvil PAL port tools - port verifier
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

"""Independent check of a port.

For every patch method, the C# literals are aligned against the
instructions of the binary they claim to mirror:
  original C#  <->  USA binary
  ported C#    <->  PAL binary
A correct port keeps the same number of matching literals and leaves no
USA-only value behind. Also checks that every config patch / funcmap
address points at the start of a PAL instruction stream that exists.

Usage: python3 verify.py --repo REPO --out OUT --usa USA --pal PAL
"""

import argparse
import difflib
import json
import os
import sys

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

from csport import LABELDEF_RE, instr_literals, literal_tokens, method_spans  # noqa: E402
from mips import Image                                                      # noqa: E402
from port import boot_exe                                                   # noqa: E402


def images(folder, cfg):
    imgs = {'game': Image.from_psx_exe(os.path.join(folder, 'MEDIEVIL.EXE'))}
    boot, _ = boot_exe(folder)
    if boot:
        imgs['main'] = Image.from_psx_exe(boot)
    for ov in cfg['overlays']:
        if ov['name'] != 'game':
            imgs[ov['name']] = Image.from_raw(os.path.join(folder, *ov['file'].split('/')),
                                              int(ov['base'], 16))
    return imgs


def score(body, start, img):
    """(matched literals, total instruction literals) for one method body."""
    labels = [(m.start(), int(m.group(1), 16)) for m in LABELDEF_RE.finditer(body)]
    cuts = [(0, start)] + labels
    hit = tot = 0
    for n, (pos, addr) in enumerate(cuts):
        end = cuts[n + 1][0] if n + 1 < len(cuts) else len(body)
        nxt = cuts[n + 1][1] if n + 1 < len(cuts) else None
        lim = min(nxt if (nxt and nxt > addr) else addr + 0x800, addr + 0x800)
        exp = []
        k = addr
        while k < lim and img.contains(k):
            exp += [(lit[0], lit[1]) for lit in instr_literals(img.word(k))]
            k += 4
        toks = [(t[0], t[1]) for t in literal_tokens(body[pos:end])]
        sm = difflib.SequenceMatcher(None, exp, toks, autojunk=False)
        blocks = [b for b in sm.get_matching_blocks() if b.size]
        hit += sum(b.size for b in blocks)
        tot += (blocks[-1].a + blocks[-1].size) if blocks else 0
    return hit, tot


def boundary_quality(cfg_dir, cfg, imgs):
    """Per image: (functions, jal targets that are not funcmap starts,
    starts that do not look like a function boundary, bodies without an end)."""
    maps = {o['name']: o['funcMap'] for o in cfg['overlays']}
    if cfg.get('funcMap') and 'main' in imgs:
        maps['main'] = cfg['funcMap']
    out = {}
    for name, fmap in maps.items():
        im = imgs[name]
        fm = [(int(f['address'], 16), f['size'])
              for f in json.load(open(os.path.join(cfg_dir, fmap)))['functions']]
        starts = {a for a, _ in fm}
        lo, hi = min(starts), max(a + s for a, s in fm)
        targets = set()
        for a, sz in fm:
            for k in range(a, a + sz, 4):
                w = im.word(k)
                if w >> 26 == 3:
                    t = 0x80000000 | ((w & 0x3FFFFFF) << 2)
                    if lo <= t < hi:
                        targets.add(t)
        bad_start = sum(1 for a, _ in fm
                        if not ((im.contains(a - 8) and im.word(a - 8) == 0x03E00008)
                                or im.word(a) >> 16 == 0x27BD or a in targets or a == lo))
        no_end = sum(1 for a, sz in fm
                     if not any(w == 0x03E00008 or w >> 26 == 2 or (w >> 26 == 0 and w & 0x3F == 8)
                                for w in im.words(a, sz)[-3:]))
        out[name] = (len(fm), len(targets - starts), bad_start, no_end)
    return out


def patch_methods(cfg):
    out = {}
    for pt in cfg['patches']:
        cls, meth = pt['target'].rsplit('.', 1)
        if meth.startswith('func_') and pt['mode'] == 'replace':
            out[meth] = (pt.get('overlay', 'main'), int(pt['address'], 16))
    return out


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument('--repo', required=True)
    ap.add_argument('--out', required=True)
    ap.add_argument('--usa', required=True)
    ap.add_argument('--pal', required=True)
    a = ap.parse_args()
    ucfg = json.load(open(os.path.join(a.repo, 'config', 'MediEvil.json')))
    pcfg = json.load(open(os.path.join(a.out, 'config', 'MediEvil.json')))
    ui, pi = images(a.usa, ucfg), images(a.pal, pcfg)
    um, pm = patch_methods(ucfg), patch_methods(pcfg)
    bad = 0

    # 1. patch methods: literal agreement before vs after
    print('%-36s %-14s %-14s' % ('method (USA -> PAL)', 'USA C#/bin', 'PAL C#/bin'))
    for root, _, files in os.walk(os.path.join(a.repo, 'patches')):
        for fn in sorted(files):
            if not fn.endswith('.cs'):
                continue
            rel = os.path.relpath(os.path.join(root, fn), os.path.join(a.repo, 'patches'))
            utext = open(os.path.join(root, fn), encoding='utf-8').read()
            ptext = open(os.path.join(a.out, 'patches', rel), encoding='utf-8').read()
            uspans = [s for s in method_spans(utext) if s[0] in um]
            pspans = [s for s in method_spans(ptext) if s[0] in pm]
            for (un, _, us, ue), (pn, _, ps, pe) in zip(uspans, pspans):
                uo, ua = um[un]
                po, pa = pm[pn]
                uh, ut = score(utext[us:ue], ua, ui[uo])
                ph, pt = score(ptext[ps:pe], pa, pi[po])
                flag = '' if (uh - ut) == (ph - pt) and ph >= uh - 1 else '   <-- CHECK'
                bad += bool(flag)
                print('%-36s %5d/%-8d %5d/%-8d%s' % (f'{un} -> {pn}', uh, ut, ph, pt, flag))

    # 2. config addresses exist in the PAL images and land on instructions
    for pt in pcfg['patches']:
        addr = int(pt['address'], 16) if 'address' in pt else int(pt['function'][5:], 16)
        img = pi[pt.get('overlay', 'main')]
        if not img.contains(addr):
            print('patch address outside image:', pt)
            bad += 1
    for ov in pcfg['overlays']:
        fm = json.load(open(os.path.join(a.out, 'config', ov['funcMap'])))['functions']
        prev_end = 0
        for f in fm:
            s = int(f['address'], 16)
            if s < prev_end:
                print(f'{ov["name"]}: overlapping funcmap entry {f["address"]}')
                bad += 1
            prev_end = s + f['size']
            if not pi[ov['name']].contains(s):
                print(f'{ov["name"]}: funcmap entry outside image {f["address"]}')
                bad += 1
    # 3. funcmap boundary quality: PAL must not be worse than the USA original
    uq = boundary_quality(os.path.join(a.repo, 'config'), ucfg, ui)
    pq = boundary_quality(os.path.join(a.out, 'config'), pcfg, pi)
    print('\n%-8s %-26s %-26s' % ('image', 'USA n/jal-miss/start/end', 'PAL n/jal-miss/start/end'))
    for name in uq:
        if name not in pq:
            continue
        worse = any(pq[name][i] > uq[name][i] for i in (1, 2, 3))
        bad += worse
        print('%-8s %-26s %-26s%s' % (name, uq[name], pq[name], '   <-- CHECK' if worse else ''))
    print('\nRESULT:', 'OK' if not bad else f'{bad} issue(s)')
    return 1 if bad else 0


if __name__ == '__main__':
    sys.exit(main())
