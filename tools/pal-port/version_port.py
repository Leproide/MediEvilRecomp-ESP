#!/usr/bin/env python3
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvil PAL port tools - per-version address table (ITA <-> other PAL builds)
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

"""Ports the Italian PAL project (SCES-01494) to another PAL build that shares
its code (Spanish SCES-01495), keeping ONE set of C# patches.

Every address that differs between the builds lives in config/versions.json:

  hooks        config patch hooks           (ita / esp address)
  entryPoints  extra entry points           (overlays[].functions)
  relocations  relocated arrays
  constants    named addresses / $gp offsets used by the hand-written patches
  literals     immediates inside the recompiled-function copies
               (CullPatch, EntityFloatFunctions, LandMapPatch), keyed by the
               Italian instruction that produces them

From that table:
  gen-cs      writes patches/GameAddresses.cs (constants chosen at compile time
              with the LINGUA_ES symbol; the Italian values are the ones that
              were hard-coded before, so the Italian build is unchanged)
  gen-config  writes config/<ver>/MediEvil.json from the Italian config
  check       verifies table, configs, funcmaps and binaries

'convert' is the one-shot step that created the table: it runs port.py
ITA -> target, records every literal port.py would rewrite and replaces it in
the C# sources with a reference to the table.

Usage (from the repository root):
  python3 tools/pal-port/version_port.py convert --ita ITA_FILES --dst ESP_FILES
  python3 tools/pal-port/version_port.py gen-cs
  python3 tools/pal-port/version_port.py gen-config
  python3 tools/pal-port/version_port.py check [--ita ITA_FILES --dst ESP_FILES]

ITA_FILES / ESP_FILES: folders with SYSTEM.CNF, the boot executable,
MEDIEVIL.EXE and OVERLAYS/*.BIN extracted from each disc. Never commit them.
"""

import argparse
import copy
import json
import os
import re
import shutil
import sys
import tempfile

HERE = os.path.dirname(os.path.abspath(__file__))
sys.path.insert(0, HERE)

import csport                                                   # noqa: E402
from csport import instr_literals, method_spans, literal_tokens  # noqa: E402
from mips import Image                                          # noqa: E402

TABLE = os.path.join('config', 'versions.json')
CS_OUT = os.path.join('patches', 'GameAddresses.cs')
# C# files that are copies of recompiled functions: their literals go in the table
RECOMPILED_COPIES = ('render/CullPatch.cs', 'render/EntityFloatFunctions.cs',
                     'widescreen/LandMapPatch.cs')
TARGET = 'esp'


def hx(v, w=8):
    return '0x%0*X' % (w, v)


def ov_class(ov):
    return ov[0].upper() + ov[1:]


def load_table(root):
    return json.load(open(os.path.join(root, TABLE), encoding='utf-8'))


def save_json(path, data):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, 'w', encoding='utf-8', newline='\n') as f:
        json.dump(data, f, indent=2, ensure_ascii=False)
        f.write('\n')


def hook_addr(pt):
    """(key, address) of a config patch: key 'address' or 'function'."""
    if 'address' in pt:
        return 'address', int(pt['address'], 16)
    return 'function', int(pt['function'][5:], 16)


# ---------------------------------------------------------------------------
# images / matching (shared by convert and check)

def boot_image(folder):
    from port import boot_exe
    path, _ = boot_exe(folder)
    return Image.from_psx_exe(path) if path else None


def load_images(folder, cfg):
    imgs = {'game': Image.from_psx_exe(os.path.join(folder, 'MEDIEVIL.EXE'))}
    b = boot_image(folder)
    if b:
        imgs['main'] = b
    for ov in cfg['overlays']:
        if ov['name'] != 'game':
            imgs[ov['name']] = Image.from_raw(os.path.join(folder, *ov['file'].split('/')),
                                              int(ov['base'], 16))
    return imgs


def build_maps(root, cfg, src, dst):
    """Function matches and address map ITA -> target (same algorithm as port.py)."""
    from matcher import ImageMatcher
    from addrmap import AddressMap
    from port import find_gp, load_funcmap
    si, di = load_images(src, cfg), load_images(dst, cfg)
    sgp, dgp = find_gp(si['game']), find_gp(di['game'])
    amap = AddressMap()
    matches = {}
    maps = {o['name']: o['funcMap'] for o in cfg['overlays']}
    maps['main'] = cfg['funcMap']
    for name, fmap in maps.items():
        _, funcs = load_funcmap(os.path.join(root, 'config', fmap))
        mt = ImageMatcher(name, si[name], di[name], sgp, dgp)
        res = mt.match(funcs)
        amap.feed(name, mt, res)
        matches[name] = res
    amap.space('global').add_code(sgp, dgp)
    return si, di, sgp, dgp, amap, matches


def lookup(amap, ov, v):
    return amap.lookup(v, 'game' if ov == 'main' else ov)


# ---------------------------------------------------------------------------
# convert

def cmd_convert(a):
    root = os.path.abspath(a.repo)
    cfg = json.load(open(os.path.join(root, 'config', 'MediEvil.json')))
    if os.path.exists(os.path.join(root, TABLE)) and not a.force:
        sys.exit(f'{TABLE} already exists (use --force to rebuild it from untouched sources)')

    # 1. run port.py ITA -> target, recording every literal it rewrites
    captured = []          # (ctx, kind, data)
    orig_lit, orig_addr = csport._literal_edit, csport.address_edit

    def lit_hook(tok, lit, pv, uw, pw, body, report, ctx, k):
        e = orig_lit(tok, lit, pv, uw, pw, body, report, ctx, k)
        if e:
            captured.append((ctx, 'lit', dict(tok=tok, lit=lit, pv=pv, uw=uw, pw=pw, k=k, edits=e)))
        return e

    def addr_hook(tok, tr, report, ctx):
        e = orig_addr(tok, tr, report, ctx)
        if e:
            captured.append((ctx, 'addr', dict(tok=tok, edits=e)))
        return e

    csport._literal_edit, csport.address_edit = lit_hook, addr_hook
    import port
    tmp = tempfile.mkdtemp(prefix='version-port-')
    argv = sys.argv
    sys.argv = ['port.py', '--repo', root, '--usa', a.ita, '--pal', a.dst, '--out', tmp,
                '--cue', a.cue]
    try:
        port.main()
    finally:
        sys.argv = argv
        csport._literal_edit, csport.address_edit = orig_lit, orig_addr

    # 2. hook / entry point / relocation table from the two configs
    tcfg = json.load(open(os.path.join(tmp, 'config', 'MediEvil.json')))
    if len(tcfg['patches']) != len(cfg['patches']):
        sys.exit('port.py disabled some patches: the builds differ too much for a shared table')
    hooks = []
    for sp, tp in zip(cfg['patches'], tcfg['patches']):
        k, sa = hook_addr(sp)
        _, ta = hook_addr(tp)
        hooks.append({'overlay': sp.get('overlay', 'main'), 'target': sp['target'], 'mode': sp['mode'],
                      'key': k, 'ita': '%08X' % sa, TARGET: '%08X' % ta})
    entries = []
    for so, to in zip(cfg['overlays'], tcfg['overlays']):
        sf = so.get('functions', [])
        tf = to.get('functions', [])
        for x, y in zip(sf, tf[:len(sf)]):
            entries.append({'overlay': so['name'], 'ita': '%08X' % int(x['address'], 16),
                            TARGET: '%08X' % int(y['address'], 16)})
        for y in tf[len(sf):]:
            sys.exit(f'target config needs extra entry point {y} in {so["name"]}: not supported')
    relocs = [{'overlay': s['overlay'], 'name': s['name'], 'ita': s['address'], TARGET: t['address']}
              for s, t in zip(cfg.get('relocations', []), tcfg.get('relocations', []))]

    # 3. literals of the recompiled-function copies -> table references
    si, di, sgp, dgp, amap, matches = build_maps(root, cfg, a.ita, a.dst)
    literals = {}
    per_file = {}
    for ctx, kind, d in captured:
        rel, meth = ctx.split(':', 1)
        rel = rel.replace(os.sep, '/')
        if rel not in RECOMPILED_COPIES or meth.startswith('<'):
            continue          # hand-written code: named constants (see 'constants')
        ov = next(h['overlay'] for h in hooks if h['target'].endswith('.' + meth))
        tok = d['tok']
        if tok[5] != 'u':
            sys.exit(f'{ctx}: literal {tok[4]} has no u suffix, cannot move it to a uint table')
        if kind == 'lit':
            if len(d['edits']) != 1:
                sys.exit(f'{ctx}: sign flip at {d["k"]:#x}, not supported by the table')
            k = d['k']
            pa, q = lookup(amap, ov, k)
            if q != 'exact':
                sys.exit(f'{ctx}: instruction {k:#x} has no exact counterpart')
            lits = instr_literals(d['uw'])
            idx = lits.index(d['lit'])
            name = '%s_%08X' % (ov_class(ov), k) + ('' if idx == 0 else '_%d' % idx)
            entry = {'name': name, 'overlay': ov, 'instr': {'ita': '%08X' % k, TARGET: '%08X' % pa},
                     'index': idx, 'ita': hx(d['lit'][1], 4), TARGET: hx(d['pv'][1], 4),
                     'where': ctx}
        else:
            v = tok[1]
            nv = int(d['edits'][0][2][2:].rstrip('u'), 16)
            name = '%s_Code_%08X' % (ov_class(ov), v)
            entry = {'name': name, 'overlay': ov, 'kind': 'code', 'ita': hx(v), TARGET: hx(nv),
                     'where': ctx}
        old = literals.get(name)
        if old and (old['ita'], old[TARGET]) != (entry['ita'], entry[TARGET]):
            sys.exit(f'{name}: conflicting values {old} / {entry}')
        literals.setdefault(name, entry)
        per_file.setdefault(rel, []).append((meth, tok[2], tok[3], name))

    # apply: replace each captured token with RAddr.<name>
    for rel, edits in per_file.items():
        path = os.path.join(root, 'patches', *rel.split('/'))
        text = open(path, encoding='utf-8').read()
        spans = {n: bs for n, _, bs, _ in method_spans(text)}
        abs_edits = sorted({(spans[m] + s, spans[m] + e, n) for m, s, e, n in edits}, reverse=True)
        for s, e, n in abs_edits:
            text = text[:s] + 'RAddr.' + n + text[e:]
        with open(path, 'w', encoding='utf-8', newline='') as f:
            f.write(text)
        print(f'{rel}: {len(abs_edits)} literal(s) moved to the table')

    # 4. target config folder (funcmaps from port.py, MediEvil.json from the table)
    dst_cfg = os.path.join(root, 'config', a.name)
    if os.path.isdir(os.path.join(dst_cfg, 'funcmaps')):
        shutil.rmtree(os.path.join(dst_cfg, 'funcmaps'))
    shutil.copytree(os.path.join(tmp, 'config', 'funcmaps'), os.path.join(dst_cfg, 'funcmaps'))
    shutil.copy(os.path.join(tmp, 'PORT_REPORT.md'), os.path.join(root, 'tools', 'pal-port',
                                                                   'PORT_REPORT_%s.md' % a.name.upper()))

    old = load_table(root) if os.path.exists(os.path.join(root, TABLE)) else {}
    table = {
        '_comment': 'Per-version address table. Edit here, then run version_port.py gen-cs / '
                    'gen-config / check. Author: https://github.com/Leproide - GPL-3.0-or-later',
        'versions': old.get('versions', {
            'ita': {'serial': 'SCES-01494', 'gameId': 'SCES-014.94', 'config': 'config/MediEvil.json',
                    'generated': 'generated', 'cue': '../disc/MediEvil (Italy).cue'},
            TARGET: {'serial': 'SCES-01495', 'gameId': a.game_id, 'config': f'config/{a.name}/MediEvil.json',
                     'generated': f'generated-{a.name}', 'cue': a.cue, 'define': 'LINGUA_ES'}}),
        'gp': {'ita': hx(sgp), TARGET: hx(dgp)},
        'hooks': hooks,
        'entryPoints': entries,
        'relocations': relocs,
        'constants': old.get('constants', []),
        'literals': sorted(literals.values(), key=lambda x: x['name']),
    }
    fill_constants(table, amap, sgp, dgp)
    save_json(os.path.join(root, TABLE), table)
    write_config(root, table, cfg)
    write_cs(root, table)
    shutil.rmtree(tmp, ignore_errors=True)
    print(f'{TABLE}: {len(hooks)} hooks, {len(entries)} entry points, '
          f'{len(table["constants"])} constants, {len(table["literals"])} literals')


def fill_constants(table, amap, sgp, dgp):
    """Target values of the named constants, from the address map (exact only)."""
    for c in table['constants']:
        if c.get(TARGET) and not c.get('auto', False):
            continue
        v = int(c['ita'], 16)
        if c['kind'] == 'data':
            nv, q = lookup(amap, c.get('overlay', 'game'), v)
        elif c['kind'] == 'gp':
            na, q = lookup(amap, 'game', (sgp + v) & 0xFFFFFFFF)
            nv = None if na is None else (na - dgp) & 0xFFFFFFFF
        elif c['kind'] == 'code':
            nv, q = lookup(amap, c['overlay'], v)
        else:
            continue  # 'fixed': value given by hand
        if q != 'exact':
            sys.exit(f'constant {c["name"]}: {c["ita"]} maps {q} to {nv}; set it by hand')
        c[TARGET] = hx(nv, len(c['ita']) - 2)


# ---------------------------------------------------------------------------
# generators

def build_config(table, cfg, ver):
    """config for version `ver` from the Italian config and the table."""
    v = table['versions'][ver]
    out = copy.deepcopy(cfg)
    if ver != 'ita':
        out['game']['id'] = v['gameId']
        out['game']['output'] = '../../' + v['generated']
        out['cue'] = v['cue']
    hooks = table['hooks']
    if len(hooks) != len(out['patches']):
        raise ValueError('hook table and Italian config have a different number of patches')
    for pt, h in zip(out['patches'], hooks):
        key, a = hook_addr(pt)
        if (h['overlay'], h['target'], h['mode'], key, '%08X' % a) != \
                (pt.get('overlay', 'main'), pt['target'], pt['mode'], h['key'], h['ita']):
            raise ValueError(f'hook table out of sync with config/MediEvil.json at {h}')
        if key == 'address':
            pt['address'] = h[ver]
        else:
            pt['function'] = 'func_' + h[ver]
    ep = {(e['overlay'], e['ita']): e[ver] for e in table['entryPoints']}
    for ov in out['overlays']:
        for f in ov.get('functions', []):
            na = ep[(ov['name'], '%08X' % int(f['address'], 16))]
            f['address'] = ('0x' if f['address'].lower().startswith('0x') else '') + na
            if 'name' in f:
                f['name'] = 'func_' + na
    # hook che esistono solo in questa versione: inseriti davanti a un hook della config italiana
    for grp in table.get('versionHooks', {}).get(ver, []):
        b = grp['before']
        pos = next((i for i, pt in enumerate(out['patches'])
                    if pt['target'] == b['target'] and pt.get('address') == h_addr(b, out, ver, table)), None)
        if pos is None:
            raise ValueError(f'versionHooks: anchor {b} not found in the {ver} config')
        out['patches'][pos:pos] = copy.deepcopy(grp['hooks'])
    rl = {(r['overlay'], r['name']): r[ver] for r in table['relocations']}
    for r in out.get('relocations', []):
        r['address'] = rl[(r['overlay'], r['name'])]
    return out


def h_addr(anchor, out, ver, table):
    """Address of an Italian-config hook (given with its ITA address) in version `ver`."""
    for h in table['hooks']:
        if h['target'] == anchor['target'] and h['ita'] == anchor['address']:
            return h[ver]
    return anchor['address']


def write_config(root, table, cfg):
    for ver, v in table['versions'].items():
        if ver == 'ita':
            continue
        save_json(os.path.join(root, v['config']), build_config(table, cfg, ver))


def cs_value(c, ver):
    return hx(int(c[ver], 16), len(c[ver]) - 2) + 'u'


def version_fields(ver, v):
    return [f'    public const string Code = "{ver}";',
            f'    public const string Serial = "{v["serial"]}";',
            f'    /// <summary>eseguibile di avvio indicato da SYSTEM.CNF</summary>',
            f'    public const string BootFile = "{v["bootFile"]}";',
            f'    /// <summary>titolo della finestra</summary>',
            f'    public const string Title = "{v["title"]}";',
            f'    /// <summary>repository GitHub da cui scaricare gli aggiornamenti</summary>',
            f'    public const string UpdateRepo = "{v["updateRepo"]}";',
            f'    public const bool Spanish = {"true" if ver == "esp" else "false"};']


def write_cs(root, table):
    vers = [v for v in table['versions'] if v != 'ita']
    L = ['// SPDX-License-Identifier: GPL-3.0-or-later',
         '//',
         '// MediEvilRecomp - tabella degli indirizzi per versione (ITA / ESP)',
         '// GENERATO da tools/pal-port/version_port.py gen-cs a partire da config/versions.json:',
         '// non modificare a mano, modifica la tabella e rigenera.',
         '// Author: https://github.com/Leproide',
         '//',
         '// This program is free software: you can redistribute it and/or modify',
         '// it under the terms of the GNU General Public License as published by',
         '// the Free Software Foundation, either version 3 of the License, or',
         '// (at your option) any later version. See <https://www.gnu.org/licenses/>.',
         '//',
         '// La versione si sceglie in compilazione (-p:Lingua=es definisce LINGUA_ES).',
         '// Senza simboli si compila la versione italiana con gli stessi valori di prima.',
         '',
         'namespace Recompiled;',
         '',
         '/// <summary>Versione del gioco per cui e\' compilato l\'eseguibile.</summary>',
         'public static class GameVersion',
         '{']
    first = True
    for ver in vers:
        v = table['versions'][ver]
        L.append(('#if ' if first else '#elif ') + v['define'])
        L += version_fields(ver, v)
        first = False
    L += ['#else'] + version_fields('ita', table['versions']['ita']) + ['#endif', '}', '']

    def pick(c):
        e = cs_value(c, 'esp')
        return f'GameVersion.Spanish ? {e} : {cs_value(c, "ita")}'

    L += ['/// <summary>Indirizzi e offset $gp usati dalle patch scritte a mano.</summary>',
          'public static class Addr', '{']
    for c in table['constants']:
        L.append(f'    /// <summary>{c["desc"]} ({c["kind"]})</summary>')
        L.append(f'    public const uint {c["name"]} = {pick(c)};')
    L += ['}', '',
          '/// <summary>Immediati delle copie di funzioni ricompilate (CullPatch,',
          '/// EntityFloatFunctions, LandMapPatch). Il nome e\' l\'istruzione italiana che',
          '/// li produce (Overlay_Indirizzo) oppure, per gli indirizzi di codice,',
          '/// Overlay_Code_IndirizzoItaliano.</summary>',
          'public static class RAddr', '{']
    for c in table['literals']:
        where = c['where']
        if 'instr' in c:
            where += ', istr. ESP ' + c['instr']['esp']
        L.append(f'    public const uint {c["name"]} = {pick(c)}; // {where}')
    L += ['}', '']
    with open(os.path.join(root, CS_OUT), 'w', encoding='utf-8', newline='\n') as f:
        f.write('\n'.join(L))


def cmd_fill(a):
    """Compute the target values of constants that have none yet (needs both discs)."""
    root = a.repo
    table = load_table(root)
    cfg = json.load(open(os.path.join(root, 'config', 'MediEvil.json')))
    todo = [c['name'] for c in table['constants'] if not c.get(TARGET)]
    _, _, sgp, dgp, amap, _ = build_maps(root, cfg, a.ita, a.dst)
    for c in table['constants']:
        if c['name'] in todo:
            c['auto'] = True
    fill_constants(table, amap, sgp, dgp)
    for c in table['constants']:
        c.pop('auto', None)
    save_json(os.path.join(root, TABLE), table)
    print('filled:', ', '.join(f"{c['name']}={c[TARGET]}" for c in table['constants'] if c['name'] in todo) or 'nothing')


def cmd_gen_cs(a):
    write_cs(a.repo, load_table(a.repo))
    print('written', CS_OUT)


def cmd_gen_config(a):
    root = a.repo
    table = load_table(root)
    write_config(root, table, json.load(open(os.path.join(root, 'config', 'MediEvil.json'))))
    print('written', ', '.join(v['config'] for k, v in table['versions'].items() if k != 'ita'))


# ---------------------------------------------------------------------------
# check

class Checker:
    def __init__(self):
        self.bad = 0

    def ok(self, cond, msg):
        if not cond:
            self.bad += 1
            print('  FAIL', msg)
        return cond


def funcmap_starts(root, cfg_dir, fmap):
    d = json.load(open(os.path.join(root, cfg_dir, fmap)))['functions']
    return [(int(f['address'], 16), f['size']) for f in d]


def cmd_check(a):
    root = a.repo
    ck = Checker()
    table = load_table(root)
    cfg = json.load(open(os.path.join(root, 'config', 'MediEvil.json')))
    print('== table <-> configs')
    for ver, v in table['versions'].items():
        try:
            want = build_config(table, cfg, ver)
        except (ValueError, KeyError) as e:
            ck.ok(False, f'{ver}: {e}')
            continue
        have = json.load(open(os.path.join(root, v['config'])))
        ck.ok(want == have, f'{v["config"]} differs from the table (run gen-config)')
        print(f'  {ver}: {v["config"]} OK' if want == have else '')

    print('== C# table')
    gen = os.path.join(root, CS_OUT)
    before = open(gen, encoding='utf-8').read() if os.path.exists(gen) else ''
    tmp = tempfile.mkdtemp()
    os.makedirs(os.path.join(tmp, 'patches'))
    write_cs(tmp, table)
    now = open(os.path.join(tmp, CS_OUT), encoding='utf-8').read()
    shutil.rmtree(tmp)
    ck.ok(before == now, f'{CS_OUT} is not up to date (run gen-cs)')
    used = set()
    for dp, _, files in os.walk(os.path.join(root, 'patches')):
        for fn in files:
            if fn.endswith('.cs') and fn != 'GameAddresses.cs':
                t = open(os.path.join(dp, fn), encoding='utf-8').read()
                used |= set(re.findall(r'\b(?:R?Addr)\.(\w+)', t))
    names = {c['name'] for c in table['constants']} | {c['name'] for c in table['literals']}
    ck.ok(not (used - names), f'references without a table entry: {sorted(used - names)}')
    mods = {c['name'] for c in table['constants'] if c.get('usedBy')}
    unused = names - used - mods
    print(f'  {len(names)} entries, {len(used & names)} referenced by patches, {len(mods)} documented only (mods, notes)'
          + (f', unused: {sorted(unused)}' if unused else ''))

    if not (a.ita and a.dst):
        print('\n(binaries not given: hook/jal/literal checks skipped)')
        print('\nRESULT:', 'OK' if not ck.bad else f'{ck.bad} issue(s)')
        return 1 if ck.bad else 0

    si, di, sgp, dgp, amap, matches = build_maps(root, cfg, a.ita, a.dst)
    ck.ok(hx(sgp) == table['gp']['ita'] and hx(dgp) == table['gp'][TARGET], 'gp values in the table')
    vcfg = {'ita': ('config', cfg)}
    tv = table['versions'][TARGET]
    vcfg[TARGET] = (os.path.dirname(tv['config']), json.load(open(os.path.join(root, tv['config']))))
    imgs = {'ita': si, TARGET: di}

    print('== hooks and jal targets (function starts)')
    for ver, (cdir, c) in vcfg.items():
        maps = {o['name']: o['funcMap'] for o in c['overlays']}
        maps['main'] = c['funcMap']
        starts, inner = {}, {}
        for name, fmap in maps.items():
            starts[name] = {s for s, _ in funcmap_starts(root, cdir, fmap)}
        for o in c['overlays']:
            inner[o['name']] = {int(f['address'], 16) for f in o.get('functions', [])}
        inner['main'] = set()
        n_start = n_inner = 0
        for pt in c['patches']:
            ov = pt.get('overlay', 'main')
            _, adr = hook_addr(pt)
            if adr in starts[ov]:
                n_start += 1
            elif ck.ok(adr in inner[ov], f'{ver} hook {pt["target"]} @ {ov}:{adr:08X} is not a function start'):
                n_inner += 1
        # every jal: into its own image, or from an overlay into MEDIEVIL.EXE / boot executable
        ranges = {}
        for name, fmap in maps.items():
            fm = funcmap_starts(root, cdir, fmap)
            ranges[name] = (min(s for s, _ in fm), max(s + z for s, z in fm))
        jal_own = jal_cross = jal_miss = 0
        for name, fmap in maps.items():
            im = imgs[ver][name]
            for s, z in funcmap_starts(root, cdir, fmap):
                for k in range(s, s + z, 4):
                    w = im.word(k)
                    if w >> 26 != 3:
                        continue
                    t = 0x80000000 | ((w & 0x3FFFFFF) << 2)
                    dest = name if ranges[name][0] <= t < ranges[name][1] else next(
                        (g for g in ('game', 'main') if ranges[g][0] <= t < ranges[g][1]), None)
                    if dest is None:
                        continue      # outside every map (BIOS, data): not a recompiled call
                    if dest == name:
                        jal_own += 1
                    else:
                        jal_cross += 1
                    if t not in starts[dest] and t not in inner.get(dest, ()):
                        jal_miss += 1
                        ck.ok(False, f'{ver} {name}: jal at {k:08X} -> {dest}:{t:08X} not a function start')
        print(f'  {ver}: {len(c["patches"])} hooks ({n_start} at funcmap starts, {n_inner} at declared '
              f'entry points); jal: {jal_own} in own image + {jal_cross} into game/main, '
              f'{jal_miss} not at a function start')

    print('== calls made by the C# patches (PalDispatch / Dispatcher.Call / MediEvil_*.func_)')
    lits = {c['name']: c for c in table['literals']}
    calls = set()
    for dp, _, files in os.walk(os.path.join(root, 'patches')):
        for fn in files:
            if not fn.endswith('.cs'):
                continue
            t = open(os.path.join(dp, fn), encoding='utf-8').read()
            for m in re.finditer(r'PalDispatch\.(\w+)\[\s*RAddr\.(\w+)\s*\]', t):
                calls.add((m.group(1).lower(), 'lit', m.group(2)))
            for m in re.finditer(r'Dispatcher\.Call\(\s*c\s*,\s*m\s*,\s*RAddr\.(\w+)\s*\)', t):
                calls.add((lits[m.group(1)]['overlay'], 'lit', m.group(1)))
            for m in re.finditer(r'PalDispatch\.(\w+)\[\s*0x([0-9A-Fa-f]+)u\s*\]', t):
                calls.add((m.group(1).lower(), 'fixed', int(m.group(2), 16)))
            for m in re.finditer(r'Dispatcher\.Call\(\s*c\s*,\s*m\s*,\s*0x([0-9A-Fa-f]+)u\s*\)', t):
                calls.add((None, 'fixed', int(m.group(1), 16)))
            for m in re.finditer(r'MediEvil_(\w+?)\.func_([0-9A-F]{8})\b', t):
                calls.add((m.group(1), 'fixed', int(m.group(2), 16)))
    not_entry = {}
    for ver, (cdir, c) in vcfg.items():
        maps = {o['name']: o['funcMap'] for o in c['overlays']}
        maps['main'] = c['funcMap']
        entry = {n: {s for s, _ in funcmap_starts(root, cdir, f)} for n, f in maps.items()}
        for o in c['overlays']:
            entry[o['name']] |= {int(f['address'], 16) for f in o.get('functions', [])}
        miss = set()
        for call in calls:
            ov, kind, v = call
            adr = int(lits[v][ver], 16) if kind == 'lit' else v
            where = [ov] if ov else list(entry)
            if not any(adr in entry.get(x, ()) for x in where):
                miss.add(call)
                print(f'  {ver}: call target {ov}:{adr:08X} is not a function entry')
        not_entry[ver] = miss
        print(f'  {ver}: {len(calls)} call targets, {len(calls) - len(miss)} function entries')
    # the Spanish build must behave like the Italian one: same targets, same status
    ck.ok(not_entry[TARGET] == not_entry['ita'], 'call targets differ between the builds')
    if not_entry['ita']:
        print('  note: the targets above are not entries in the Italian build either (tail jumps that '
              'RecompOne resolves at run time, Dispatcher.Tolerant): behaviour unchanged')

    print('== hook targets: same code in both builds')
    for h in table['hooks']:
        ov = h['overlay']
        sa, ta = int(h['ita'], 16), int(h[TARGET], 16)
        m = next((x for x in matches[ov] if x.usa <= sa < x.usa + x.size), None)
        good = m is not None and m.kind in ('exact', 'aligned') and m.pal + (sa - m.usa) == ta
        ck.ok(good, f'hook {h["target"]} {ov}:{h["ita"]} -> {h[TARGET]} not matched')

    print('== literals (RAddr) against the instructions')
    for c in table['literals']:
        ov = c['overlay']
        if 'instr' in c:
            sk, tk = int(c['instr']['ita'], 16), int(c['instr'][TARGET], 16)
            sl = instr_literals(si[ov].word(sk))
            tl = instr_literals(di[ov].word(tk))
            i = c['index']
            ck.ok(i < len(sl) and sl[i][1] == int(c['ita'], 16), f'{c["name"]}: ITA instruction literal')
            ck.ok(i < len(tl) and tl[i][1] == int(c[TARGET], 16), f'{c["name"]}: {TARGET} instruction literal')
            pa, q = lookup(amap, ov, sk)
            ck.ok(q == 'exact' and pa == tk, f'{c["name"]}: instruction mapping')
        else:
            nv, q = lookup(amap, ov, int(c['ita'], 16))
            ck.ok(q == 'exact' and nv == int(c[TARGET], 16), f'{c["name"]}: code address mapping')
    print(f'  {len(table["literals"])} checked')

    print('== named constants (Addr) against the address map')
    for c in table['constants']:
        v, t = int(c['ita'], 16), int(c[TARGET], 16)
        if c['kind'] == 'data':
            nv, q = lookup(amap, c.get('overlay', 'game'), v)
        elif c['kind'] == 'gp':
            na, q = lookup(amap, 'game', (sgp + v) & 0xFFFFFFFF)
            nv = None if na is None else (na - dgp) & 0xFFFFFFFF
        elif c['kind'] == 'code':
            nv, q = lookup(amap, c['overlay'], v)
        else:
            nv, q = t, 'fixed'
        ck.ok(nv == t and q in ('exact', 'fixed'), f'{c["name"]}: {c["ita"]} -> table {c[TARGET]}, map {nv} ({q})')
        if c.get('expect'):
            # constant data that must be found at this address in both builds
            for ver, im in (('ita', si), (TARGET, di)):
                adr = int(c[ver], 16)
                ck.ok(im['game'].word(adr) == int(c['expect'], 16),
                      f'{c["name"]}: {ver} word at {adr:08X} is not {c["expect"]}')
    print(f'  {len(table["constants"])} checked')

    print('\nRESULT:', 'OK' if not ck.bad else f'{ck.bad} issue(s)')
    return 1 if ck.bad else 0


def main():
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument('--repo', default='.', help='repository root (default: current folder)')
    sub = ap.add_subparsers(dest='cmd', required=True)
    p = sub.add_parser('convert')
    p.add_argument('--ita', required=True, help='files of the Italian disc')
    p.add_argument('--dst', required=True, help='files of the target disc')
    p.add_argument('--name', default='es', help='config sub-folder (config/<name>)')
    p.add_argument('--game-id', default='SCES-014.95')
    p.add_argument('--cue', default='../../disc/MediEvil (Spain).cue')
    p.add_argument('--force', action='store_true')
    p = sub.add_parser('fill')
    p.add_argument('--ita', required=True, help='files of the Italian disc')
    p.add_argument('--dst', required=True, help='files of the target disc')
    sub.add_parser('gen-cs')
    sub.add_parser('gen-config')
    p = sub.add_parser('check')
    p.add_argument('--ita', help='files of the Italian disc')
    p.add_argument('--dst', help='files of the target disc')
    a = ap.parse_args()
    a.repo = os.path.abspath(a.repo)
    return {'convert': cmd_convert, 'fill': cmd_fill, 'gen-cs': cmd_gen_cs, 'gen-config': cmd_gen_config,
            'check': cmd_check}[a.cmd](a) or 0


if __name__ == '__main__':
    sys.exit(main())
