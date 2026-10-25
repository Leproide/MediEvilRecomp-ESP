#!/usr/bin/env python3
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvil PAL port tools - main entry point
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

"""Ports MediEvilRecomp's config/, funcmaps and C# patches from the USA
release (SCUS-942.27) to a PAL release, given the executables and overlays
extracted from both discs.

Usage:
  python3 port.py --repo MediEvilRecomp --usa usa_files --pal pal_files --out out

usa_files / pal_files layout (as extracted from the disc):
  MEDIEVIL.EXE
  OVERLAYS/*.BIN
  SYSTEM.CNF + boot executable   (optional, needed for funcmaps/main.json)
"""

import argparse
import copy
import json
import os
import re
import shutil
import sys
from collections import Counter

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

from addrmap import AddressMap, space_for          # noqa: E402
from csport import (DEF_RE, FUNCREF_RE, gp_offset_names, method_spans,  # noqa: E402
                    rename_labels, rewrite_body, rewrite_gp_offsets, rewrite_outside)
from matcher import ImageMatcher                   # noqa: E402
from mips import Image                             # noqa: E402

GP_REG = 28

# Prepended to every ported C# file. The original code belongs to the
# MediEvilRecomp authors (BlackLabelHQ); only the PAL address port is ours.
PORT_HEADER = (
    '// PAL port: addresses/constants rewritten for the PAL release by\n'
    '// medievil-pal-port (GPL-3.0-or-later), Author: https://github.com/Leproide\n'
    '// Original code: MediEvilRecomp by BlackLabelHQ - their license terms apply to it.\n\n')


class Report:
    def __init__(self):
        self.lines = []
        self.warnings = []

    def info(self, s):
        self.lines.append(s)

    def warn(self, s):
        self.warnings.append(s)


# ----------------------------------------------------------------------------
# input helpers

def find_gp(img):
    """Value loaded into $gp by 'lui gp,hi; addiu gp,gp,lo' in the image."""
    n = len(img.data) // 4
    w = img.words(img.base, n * 4)
    for i in range(n - 1):
        if w[i] >> 16 == 0x3C1C and w[i + 1] >> 16 == 0x279C:
            lo = w[i + 1] & 0xFFFF
            lo = lo - 0x10000 if lo & 0x8000 else lo
            return ((w[i] & 0xFFFF) << 16) + lo
    return None


def boot_exe(folder):
    """Path of the boot executable named in SYSTEM.CNF, or None."""
    cnf = os.path.join(folder, 'SYSTEM.CNF')
    if not os.path.exists(cnf):
        return None, None
    text = open(cnf, 'rb').read().decode('ascii', 'replace')
    m = re.search(r'BOOT\s*=\s*cdrom:\\?([^;\r\n]+)', text, re.I)
    if not m:
        return None, None
    name = m.group(1).strip().lstrip('\\')
    path = os.path.join(folder, name)
    if not os.path.exists(path):
        # tolerate extraction tools that drop the ';1' version or change case
        for f in os.listdir(folder):
            if f.upper().split(';')[0] == name.upper():
                path = os.path.join(folder, f)
                break
    serial = name.replace('_', '-').replace('.', '') if name else None
    if serial and len(serial) == 10:      # SCES-00311 -> SCES-003.11 style
        serial = serial[:8] + '.' + serial[8:]
    return (path if os.path.exists(path) else None), serial


def load_funcmap(path):
    d = json.load(open(path))
    return d, [(int(f['address'], 16), f['size']) for f in d['functions']]


# ----------------------------------------------------------------------------
# funcmap generation

def code_like(img, a, b):
    """True if [a, b) looks like code (contains a 'jr ra')."""
    for x in range(a, b, 4):
        if img.word(x) == 0x03E00008:
            return True
    return False


def split_functions(img, a, b):
    """Split a PAL-only code region at 'jr ra' + delay slot boundaries."""
    starts = [a]
    for x in range(a, b - 8, 4):
        if img.word(x) == 0x03E00008 and x + 8 < b:
            starts.append(x + 8)
    starts.append(b)
    return [(s, e - s) for s, e in zip(starts, starts[1:]) if e > s]


def build_funcmap(name, usa_img, pal_img, matches, report):
    """PAL funcmap entries (address, size) + list of added PAL-only functions."""
    got = sorted((m.pal, m.pal_size) for m in matches if m.pal is not None)
    added = []
    usa_contig = {}
    for m1, m2 in zip(matches, matches[1:]):
        usa_contig[(m1.usa, m2.usa)] = (m1.usa + m1.size == m2.usa)
    ordered = [m for m in matches if m.pal is not None]
    for m1, m2 in zip(ordered, ordered[1:]):
        end = m1.pal + m1.pal_size
        if m2.pal <= end:
            continue
        usa_gap = m2.usa - (m1.usa + m1.size)
        pal_gap = m2.pal - end
        if pal_gap > usa_gap and code_like(pal_img, end, m2.pal):
            for s, sz in split_functions(pal_img, end, m2.pal):
                added.append((s, sz))
    got = sorted(set(got) | set(added))
    for s, sz in added:
        report.warn(f'{name}: PAL-only code at {s:#010x} (+{sz}) added to funcmap')
    return got, added


AUTO_NAME = re.compile(r'^func_[0-9A-Fa-f]{8}$')


def pal_names(template, matches):
    """PAL address -> name. Real symbol names from the USA map (SDK functions
    like VSync, CdRead, ResetGraph: RecompOne swaps those for its HLE versions by
    name) are kept; auto-generated func_<addr> names follow the new address."""
    names = {}
    for entry, m in zip(template['functions'], matches):
        if m.pal is None:
            continue
        n = entry.get('name', '')
        names[m.pal] = 'func_%08X' % m.pal if (not n or AUTO_NAME.match(n)) else n
    return names


def write_funcmap(path, template, entries, names=None):
    names = names or {}
    out = copy.deepcopy(template)
    out['functions'] = [{'address': '0x%08X' % a, 'name': names.get(a, 'func_%08X' % a), 'size': s}
                        for a, s in entries]
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, 'w', newline='\n') as f:
        json.dump(out, f, indent=2)
        f.write('\n')


# ----------------------------------------------------------------------------

def dispatch_prop(ov):
    return ov[0].upper() + ov[1:]


def write_dispatch(out_patches, overlays):
    """Helper giving patches address-based access to overlay functions."""
    lines = [
        '// SPDX-License-Identifier: GPL-3.0-or-later',
        '// PalDispatch - address based calls into recompiled overlays (PAL port)',
        '// Author: https://github.com/Leproide',
        '//',
        '// RecompOne appends "_<overlay>" to a function name when the same address',
        '// exists in more than one overlay, so the emitted name cannot be known before',
        '// recompiling. Patches call overlay code through the dispatch tables instead.',
        '',
        'using RecompOne.Runtime.Context;',
        'using RecompOne.Runtime.Memory;',
        '',
        'namespace Recompiled;',
        '',
        'internal static class PalDispatch',
        '{',
    ]
    for ov in overlays:
        cls = dispatch_prop(ov) + 'DispatchTable'
        lines.append('    public static readonly IReadOnlyDictionary<uint, Action<CpuContext, IMemory>> '
                     f'{dispatch_prop(ov)} = new {cls}().Functions;')
    lines += ['}', '']
    path = os.path.join(out_patches, 'PalDispatch.cs')
    with open(path, 'w', encoding='utf-8', newline='\n') as f:
        f.write('\n'.join(lines))


def main():
    ap = argparse.ArgumentParser(description=__doc__,
                                 formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument('--repo', required=True, help='MediEvilRecomp checkout (reads config/, patches/)')
    ap.add_argument('--usa', required=True, help='files extracted from the USA disc')
    ap.add_argument('--pal', required=True, help='files extracted from the PAL disc')
    ap.add_argument('--out', required=True, help='output folder (config/, patches/, PORT_REPORT.md)')
    ap.add_argument('--cue', default='../disc/MediEvil (Italy).cue',
                    help='cue path written in MediEvil.json (relative to config/)')
    ap.add_argument('--id', default=None, help='PAL serial (default: from SYSTEM.CNF)')
    args = ap.parse_args()

    rep = Report()
    cfg_path = os.path.join(args.repo, 'config', 'MediEvil.json')
    cfg = json.load(open(cfg_path))
    cfg_dir = os.path.dirname(cfg_path)

    usa_exe = Image.from_psx_exe(os.path.join(args.usa, 'MEDIEVIL.EXE'))
    pal_exe = Image.from_psx_exe(os.path.join(args.pal, 'MEDIEVIL.EXE'))
    ugp, pgp = find_gp(usa_exe), find_gp(pal_exe)
    rep.info(f'MEDIEVIL.EXE  USA base {usa_exe.base:#010x} gp {ugp:#010x}  |  '
             f'PAL base {pal_exe.base:#010x} gp {pgp:#010x}')

    amap = AddressMap()
    results = {}     # overlay name -> (matcher, matches, funcmap template, usa img, pal img)

    # main (boot) executable -------------------------------------------------
    ub, _ = boot_exe(args.usa)
    pb, pal_serial = boot_exe(args.pal)
    jobs = []
    if cfg.get('funcMap'):
        if ub and pb:
            jobs.append(('main', Image.from_psx_exe(ub), Image.from_psx_exe(pb), cfg['funcMap']))
        else:
            rep.warn('boot executable (SYSTEM.CNF + SCUS/SCES file) missing for USA or PAL: '
                     'funcmaps/main.json copied UNCHANGED - it will be wrong for PAL')
    for ov in cfg['overlays']:
        if ov['name'] == 'game':
            jobs.append(('game', usa_exe, pal_exe, ov['funcMap']))
        else:
            base = int(ov['base'], 16)
            up = os.path.join(args.usa, *ov['file'].split('/'))
            pp = os.path.join(args.pal, *ov['file'].split('/'))
            jobs.append((ov['name'], Image.from_raw(up, base), Image.from_raw(pp, base), ov['funcMap']))

    for name, ui, pi, fmap in jobs:
        tmpl, funcs = load_funcmap(os.path.join(cfg_dir, fmap))
        mt = ImageMatcher(name, ui, pi, ugp, pgp)
        res = mt.match(funcs)
        amap.feed(name, mt, res)
        results[name] = (mt, res, tmpl, ui, pi, fmap)
        k = Counter(m.kind for m in res)
        rep.info(f'| {name} | {len(res)} | {k["exact"]} | {k["aligned"]} | {k["fuzzy"]} | {k["missing"]} |')

    # $gp itself maps
    amap.space('global').add_code(ugp, pgp)

    def tr_for(ov):
        return lambda v: amap.lookup(v, 'game' if ov == 'main' else ov)

    # funcmaps ---------------------------------------------------------------
    out_cfg_dir = os.path.join(args.out, 'config')
    os.makedirs(os.path.join(out_cfg_dir, 'funcmaps'), exist_ok=True)
    for name, (mt, res, tmpl, ui, pi, fmap) in results.items():
        entries, _ = build_funcmap(name, ui, pi, res, rep)
        write_funcmap(os.path.join(out_cfg_dir, fmap), tmpl, entries, pal_names(tmpl, res))
        for m in res:
            if m.kind == 'missing':
                rep.warn(f'{name}: USA function {m.usa:#010x} (+{m.size}) has no PAL counterpart; dropped')
            elif m.kind == 'fuzzy' and m.ratio < 0.5:
                rep.warn(f'{name}: USA {m.usa:#010x} ~ PAL {m.pal:#010x} weak match '
                         f'(similarity {m.ratio:.2f}); rewritten in PAL')
    if cfg.get('funcMap') and 'main' not in results:
        shutil.copy(os.path.join(cfg_dir, cfg['funcMap']), os.path.join(out_cfg_dir, cfg['funcMap']))

    def func_of(ov, addr):
        """FuncMatch containing USA address addr in overlay ov."""
        for m in results[ov][1]:
            if m.usa <= addr < m.usa + m.size:
                return m
        return None

    # MediEvil.json ----------------------------------------------------------
    pc = copy.deepcopy(cfg)
    pc['cue'] = args.cue
    pc['game']['id'] = args.id or pal_serial or 'PAL'
    for ov in pc['overlays']:
        tr = tr_for(ov['name'])
        if ov['name'] == 'game':
            ov['base'] = '0x%08X' % pal_exe.base
        for f in ov.get('functions', []):
            a = int(f['address'], 16)
            na, q = tr(a)
            if q != 'exact':
                rep.warn(f'config {ov["name"]} extra function {a:#010x} -> {na:#010x} ({q})')
            f['address'] = ('0x%08X' if f['address'].lower().startswith('0x') else '%08X') % na
            if 'name' in f:
                f['name'] = 'func_%08X' % na

    # patch targets: (overlay, USA address) and method renames
    rename = {}          # old method name -> new method name
    disabled = []
    method_info = {}     # method name -> (overlay, usa address)
    new_patches = []
    for pt in pc['patches']:
        ov = pt.get('overlay', 'main')
        a = int(pt['address'], 16) if 'address' in pt else int(pt['function'][5:], 16)
        fm = func_of(ov, a)
        na, q = tr_for(ov)(a)
        kind = fm.kind if fm else 'missing'
        tgt_cls, tgt_meth = pt['target'].rsplit('.', 1)
        if kind not in ('exact', 'aligned') or q != 'exact':
            disabled.append((pt, kind))
            rep.warn(f'patch {pt["target"]} @ {ov}:{a:#010x}: function is {kind} in PAL -> '
                     'patch DISABLED (original PAL code runs instead)')
            continue
        m = DEF_RE.fullmatch(tgt_meth)
        if m and int(m.group(1), 16) == a:
            new_name = 'func_%08X%s' % (na, m.group(2) or '')
            rename[tgt_meth] = new_name
            method_info[tgt_meth] = (ov, a)
            pt['target'] = tgt_cls + '.' + new_name
        if 'address' in pt:
            pt['address'] = '%08X' % na
        else:
            pt['function'] = 'func_%08X' % na
        new_patches.append(pt)
    pc['patches'] = new_patches

    for rl in pc.get('relocations', []):
        a = int(rl['address'], 16)
        na, q = tr_for(rl['overlay'])(a)
        rep.info(f'relocation {rl["name"]}: {a:#010x} -> {na:#010x} ({q})')
        if q != 'exact':
            rep.warn(f'relocation {rl["name"]} start {a:#010x} -> {na:#010x} is {q}: '
                     'verify the array start and its size (0x' + rl['size'][2:] + ')')
        rl['address'] = '%08X' % na

    with open(os.path.join(out_cfg_dir, 'MediEvil.json'), 'w', newline='\n') as f:
        json.dump(pc, f, indent=2)
        f.write('\n')

    # C# patches -------------------------------------------------------------
    def gp_tr(off):
        na, q = amap.lookup((ugp + off) & 0xFFFFFFFF, 'game')
        return (None, None) if na is None else ((na - pgp) & 0xFFFFFFFF, q)

    def words_at(ov):
        ui, pi = results[ov][3], results[ov][4]
        tr = tr_for(ov)

        def get(addr):
            if not ui.contains(addr):
                return None, None
            pa, q = tr(addr)
            pw = pi.word(pa) if (q == 'exact' and pi.contains(pa)) else None
            return ui.word(addr), pw
        return get

    src_patches = os.path.join(args.repo, 'patches')
    out_patches = os.path.join(args.out, 'patches')
    disabled_methods = {p['target'].rsplit('.', 1)[1] for p, _ in disabled}
    needed = {}          # overlay -> PAL entry points the C# patches call
    used_dispatch = set()
    for root, _, files in os.walk(src_patches):
        for fn in sorted(files):
            if not fn.endswith('.cs'):
                continue
            src = os.path.join(root, fn)
            rel = os.path.relpath(src, src_patches)
            text = open(src, encoding='utf-8').read()
            spans = list(method_spans(text))
            pieces = []
            last = 0
            for mname, maddr, bs, be in spans:
                pieces.append(('out', text[last:bs]))
                body = text[bs:be]
                ctx = f'{rel}:{mname}'
                if mname in method_info and mname not in disabled_methods:
                    ov, a = method_info[mname]
                    fm = func_of(ov, a)
                    tr = tr_for(ov)
                    body, nch, nmiss = rewrite_body(body, a, words_at(ov), rep, ctx, tr)
                    body = rename_labels(body, tr, rep, ctx)
                    rep.info(f'| {rel} | {mname} | {ov} | {fm.kind} | {nch} | {nmiss} |')
                elif mname in disabled_methods:
                    body = ('{\n        // PAL PORT: disabled, the PAL function differs from USA; '
                            'see PORT_REPORT.md\n        throw new System.NotSupportedException();\n    }'
                            + '\n    /* original USA body:\n' + body.replace('*/', '* /') + '\n    */')
                else:
                    rep.warn(f'{ctx}: method not referenced by config patches; literals '
                             'translated with the data map only')
                    body = rewrite_outside(body, tr_for('game'), rep, ctx)
                pieces.append(('body', body))
                last = be
            pieces.append(('out', text[last:]))
            gp_names, gp_lists = gp_offset_names(text)

            def fix_out(t):
                t = rewrite_gp_offsets(t, gp_names, gp_lists, gp_tr, rep, f'{rel}:<gp>')
                return rewrite_outside(t, tr_for('game'), rep, f'{rel}:<consts>')
            text = ''.join(fix_out(t) if kind == 'out' else t for kind, t in pieces)

            # calls into recompiled code: MediEvil_<ov>.func_X
            text = FUNCREF_RE.sub(lambda m: '\0FREF%s.%s\0' % (m.group(1), m.group(2)), text)

            # patch method definitions / cross-references
            def ren(m):
                full = m.group(0)
                return rename.get(full, full)
            text = DEF_RE.sub(ren, text)

            def fref(m):
                ov, v = m.group(1), int(m.group(2), 16)
                nv, q = tr_for(ov)(v)
                if nv is None or q != 'exact':
                    rep.warn(f'{rel}: call MediEvil_{ov}.func_{v:08X} -> {nv} ({q}); review')
                    nv = nv or v
                needed.setdefault(ov, set()).add(nv)
                if ov in ('game', 'main'):
                    return 'MediEvil_%s.func_%08X' % (ov, nv)
                # overlay functions may get an '_<overlay>' suffix from RecompOne when the
                # address exists in several overlays: call through the dispatch table instead
                used_dispatch.add(ov)
                return 'PalDispatch.%s[0x%08Xu]' % (dispatch_prop(ov), nv)
            text = re.sub(r'\0FREF(\w+?)\.([0-9A-F]{8})\0', fref, text)

            text = PORT_HEADER + text
            dst = os.path.join(out_patches, rel)
            os.makedirs(os.path.dirname(dst), exist_ok=True)
            with open(dst, 'w', encoding='utf-8', newline='') as f:
                f.write(text)

    for pt in pc['patches']:
        if 'address' in pt:
            needed.setdefault(pt.get('overlay', 'main'), set()).add(int(pt['address'], 16))
    added_entries = 0
    for ov in pc['overlays']:
        fmap = json.load(open(os.path.join(out_cfg_dir, ov['funcMap'])))['functions']
        starts = {int(f['address'], 16) for f in fmap}
        have = {int(f['address'], 16) for f in ov.get('functions', [])}
        for a in sorted(needed.get(ov['name'], ())):
            if a in starts or a in have:
                continue
            # entry point inside a function: RecompOne only found these by luck in the
            # USA build (escape/cross-image scans), so declare them explicitly
            ov.setdefault('functions', []).append({'address': '0x%08X' % a, 'name': 'func_%08X' % a})
            added_entries += 1
    rep.info(f'declared {added_entries} inner entry point(s) used by patches in MediEvil.json')
    with open(os.path.join(out_cfg_dir, 'MediEvil.json'), 'w', newline='\n') as f:
        json.dump(pc, f, indent=2)
        f.write('\n')
    if used_dispatch:
        write_dispatch(out_patches, sorted(used_dispatch))
    # opt-in runtime tracing (MEDIEVIL_DEBUG=cd,irq,...)
    tdir = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'templates')
    tpl = os.path.join(tdir, 'PalDebug.cs.template')
    if os.path.exists(tpl):
        shutil.copy(tpl, os.path.join(out_patches, 'PalDebug.cs'))
    # RecompOne runtime fixes needed by the PAL build (applied with git apply)
    fix = os.path.join(tdir, 'cd-whole-sector-mode.patch')
    if os.path.exists(fix):
        os.makedirs(os.path.join(args.out, 'recompone-fixes'), exist_ok=True)
        shutil.copy(fix, os.path.join(args.out, 'recompone-fixes', os.path.basename(fix)))

    # data map conflicts (diagnostics) --------------------------------------
    conf = amap.space('global').conflicts()

    # report ------------------------------------------------------------------
    write_report(args, rep, conf, results)
    print(f'done: {len(rep.warnings)} warnings, see {os.path.join(args.out, "PORT_REPORT.md")}')


def write_report(args, rep, conf, results):
    path = os.path.join(args.out, 'PORT_REPORT.md')
    L = []
    L.append('# MediEvilRecomp USA -> PAL port report\n')
    L.append('Author: https://github.com/Leproide  ')
    L.append('License: GPL-3.0\n')
    L.append(rep.lines[0] + '\n')
    L.append('## Function matching\n')
    L.append('| image | functions | exact | aligned | fuzzy | missing |')
    L.append('|---|---|---|---|---|---|')
    L.extend(x for x in rep.lines if x.startswith('| ') and x.count('|') == 7 and '.cs' not in x)
    L.append('\n*exact*: same code, only addresses differ. *aligned*: same code, some '
             'constants differ (50 Hz timing, offsets). *fuzzy*: code changed in PAL.\n')
    L.append('## C# patch methods\n')
    L.append('| file | method | overlay | match | literals rewritten | unresolved |')
    L.append('|---|---|---|---|---|---|')
    L.extend(x for x in rep.lines if x.startswith('| ') and x.count('|') == 7 and '.cs' in x)
    L.append('')
    L.extend('- ' + x for x in rep.lines if x.startswith('relocation'))
    L.append('\n## Warnings (%d)\n' % len(rep.warnings))
    L.extend('- ' + w for w in rep.warnings)
    L.append('\n## Data map conflicts (%d)\n' % len(conf))
    L.append('USA data addresses that mapped to more than one PAL address '
             '(majority vote used):\n')
    for u, c in sorted(conf)[:200]:
        L.append('- %#010x: %s' % (u, ', '.join('%#010x x%d' % (p, n) for p, n in c)))
    with open(path, 'w', newline='\n') as f:
        f.write('\n'.join(L) + '\n')


if __name__ == '__main__':
    main()
