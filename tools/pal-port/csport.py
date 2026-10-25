# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvil PAL port tools - C# patch translator
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

"""Rewrites the hand-maintained C# patches (copies of RecompOne output for
the USA build) so they target the PAL build.

For every `func_XXXXXXXX(CpuContext c, IMemory m)` method that mirrors a
recompiled function:
  1. the immediates the generator printed (hex literals and shift amounts)
     are aligned, with difflib, to the immediates of the USA instructions;
  2. each literal whose instruction differs in PAL is rewritten with the PAL
     value (addresses, $gp offsets, 50 Hz timing constants...);
Then, file-wide:
  3. code addresses (func_/L labels, `case 0x8...u`, return addresses) are
     translated through the code map;
  4. any other 0x80xxxxxx literal is translated through the data map and
     reported for review.
"""

import difflib
import re

from mips import BRANCH_OPS, LO_OPS, sext16

METHOD_RE = re.compile(
    r'(?:public|private|internal)?\s*static\s+\w+\s+(func_([0-9A-F]{8})(?:_\w+)?)\s*'
    r'\(\s*CpuContext\s+c\s*,\s*IMemory\s+m\s*\)')
HEX_RE = re.compile(r'0x([0-9A-Fa-f]+)(u?)')
SHIFT_RE = re.compile(r'(<<|>>)\s*(\d+)')
FUNCREF_RE = re.compile(r'\bMediEvil_(\w+?)\.func_([0-9A-F]{8})\b')
LABEL_RE = re.compile(r'\bL([0-9A-F]{8})\b')
DEF_RE = re.compile(r'\bfunc_([0-9A-F]{8})(_\w+)?\b')

LOAD_OPS = {0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26}
MEM_OPS = LOAD_OPS | {0x28, 0x29, 0x2A, 0x2B, 0x2E, 0x32, 0x3A}
# GTE commands the generator turns into named calls (no literal printed)
GTE_NAMED = {0x01, 0x06, 0x0C, 0x10, 0x11, 0x12, 0x13, 0x14, 0x16, 0x1B, 0x1C, 0x1E,
             0x20, 0x28, 0x29, 0x2A, 0x2D, 0x2E, 0x30, 0x3D, 0x3E, 0x3F}
DEC_RE = re.compile(r'<\s*(-?\d+)\s*\?')


def method_spans(text):
    """Yield (name, address, body_start, body_end) for every patch method."""
    for m in METHOD_RE.finditer(text):
        i = text.index('{', m.end())
        depth = 0
        j = i
        while j < len(text):
            ch = text[j]
            if ch == '{':
                depth += 1
            elif ch == '}':
                depth -= 1
                if depth == 0:
                    break
            j += 1
        yield m.group(1), int(m.group(2), 16), i, j + 1


def _u32(v):
    return v & 0xFFFFFFFF


def instr_literals(w):
    """Literals RecompOne's InstructionEmitter prints for instruction w.

    Returns a list of (kind, value, tag):
      kind 'x' hex literal, 's' decimal shift amount, 'd' decimal slti operand
      tag  'signed' (printed as +/- magnitude), 'plain' (printed as is)
    Mirrors RecompOne.Recompiler/CodeGen/InstructionEmitter.cs.
    """
    if w == 0:
        return []
    op = w >> 26
    rs = (w >> 21) & 31
    rt = (w >> 16) & 31
    imm = sext16(w & 0xFFFF)
    immu = w & 0xFFFF
    if op == 0:
        fn = w & 0x3F
        rd = (w >> 11) & 31
        if fn in (0x00, 0x02, 0x03) and rd:
            return [('s', (w >> 6) & 31, 'plain')]
        return []
    if op in (0x08, 0x09):
        if rt == 0:
            return []
        if rs == 0:
            return [('x', _u32(imm), 'plain')]
        return [('x', abs(imm), 'signed')]
    if op == 0x0A:
        return [('d', imm, 'plain')] if rt else []
    if op == 0x0B:
        return [('x', _u32(imm), 'plain')] if rt else []
    if op in (0x0C, 0x0E):
        return [('x', immu, 'plain')] if rt else []
    if op == 0x0D:
        return [('x', immu, 'plain')] if (rt and immu) else []
    if op == 0x0F:
        return [('x', immu << 16, 'plain')] if rt else []
    if op in MEM_OPS:
        if op in LOAD_OPS and rt == 0:
            return []
        if rs == 0:
            return [('x', _u32(imm), 'plain')]
        if imm == 0:
            return []
        return [('x', abs(imm), 'signed')]
    if op == 0x12 and (w >> 25) & 1 and (w & 0x3F) not in GTE_NAMED:
        return [('x', w, 'plain')]
    return []


def literal_tokens(body):
    """(kind, value, start, end, text_digits, suffix) for each literal in a body."""
    toks = []
    for m in HEX_RE.finditer(body):
        toks.append(('x', int(m.group(1), 16), m.start(), m.end(), m.group(1), m.group(2)))
    for m in SHIFT_RE.finditer(body):
        toks.append(('s', int(m.group(2)), m.start(2), m.end(2), m.group(2), ''))
    for m in DEC_RE.finditer(body):
        toks.append(('d', int(m.group(1)), m.start(1), m.end(1), m.group(1), ''))
    toks.sort(key=lambda t: t[2])
    return toks


def fmt_hex(v, digits_like, suffix):
    """Hex literal keeping the original digit count (X4 / X8 formats)."""
    return '0x%0*X%s' % (len(digits_like), v, suffix)


LABELDEF_RE = re.compile(r'\bL([0-9A-F]{8})\s*:')


def _literal_edit(tok, lit, pv, uw, pw, body, report, ctx, k):
    """Edits turning one USA literal token into its PAL value."""
    if pv is None:
        report.warn(f'{ctx}: literal of instr {k:#010x} vanished in PAL, left as is')
        return []
    us = lit[2] == 'signed' and sext16(uw & 0xFFFF) < 0
    ps = lit[2] == 'signed' and sext16(pw & 0xFFFF) < 0
    if pv[1] == lit[1] and us == ps:
        return []
    start, end = tok[2], tok[3]
    if tok[0] in ('s', 'd'):
        return [(start, end, str(pv[1]))]
    edits = [(start, end, fmt_hex(pv[1], tok[4], tok[5]))]
    if us != ps:
        # flip the operator right before the literal ("+ 0x.." <-> "- 0x..")
        j = start - 1
        while j >= 0 and body[j] == ' ':
            j -= 1
        if j >= 0 and body[j] in '+-':
            edits.append((j, j + 1, '-' if ps else '+'))
        else:
            report.warn(f'{ctx}: sign change at instr {k:#010x} needs manual check')
    return edits


def rewrite_body(body, start, words_at, report, ctx, tr):
    """Apply instruction-aligned literal rewrites to one method body.

    start    : USA address of the first instruction the method mirrors
    words_at : f(usa_addr) -> (usa_word, pal_word or None)
    The body is cut at its 'Lxxxxxxxx:' labels; each piece is aligned with
    the instructions that start at that label, so duplicated delay slots and
    code appended after the function end stay in sync.
    """
    labels = [(m.start(), int(m.group(1), 16)) for m in LABELDEF_RE.finditer(body)]
    cuts = [(0, start)] + labels
    edits = []
    changed = missed = 0
    for n, (pos, addr) in enumerate(cuts):
        seg_end = cuts[n + 1][0] if n + 1 < len(cuts) else len(body)
        nxt_addr = cuts[n + 1][1] if n + 1 < len(cuts) else None
        lim = nxt_addr if (nxt_addr and nxt_addr > addr) else addr + 0x800
        lim = min(lim, addr + 0x800)
        exp = []
        k = addr
        while k < lim:
            uw, pw = words_at(k)
            if uw is None:
                break
            ul = instr_literals(uw)
            pl = instr_literals(pw) if pw is not None else []
            for i, lit in enumerate(ul):
                exp.append((lit, pl[i] if (pw is not None and i < len(pl)) else None, k, uw, pw))
            k += 4
        seg = body[pos:seg_end]
        toks = [(t[0], t[1], t[2] + pos, t[3] + pos, t[4], t[5]) for t in literal_tokens(seg)]
        sm = difflib.SequenceMatcher(None, [(e[0][0], e[0][1]) for e in exp],
                                     [(t[0], t[1]) for t in toks], autojunk=False)
        consumed = set()
        used_exp = set()
        repl = {}      # (kind, usa value) -> set of PAL values seen in this segment
        for blk in sm.get_matching_blocks():
            for t in range(blk.size):
                lit, pv, k, uw, pw = exp[blk.a + t]
                tok = toks[blk.b + t]
                consumed.add(blk.b + t)
                used_exp.add(blk.a + t)
                if pw is None:
                    if (uw >> 26) == 0x0F or is_ram_addr(lit[1]):
                        report.warn(f'{ctx}: instr {k:#010x} has no exact PAL counterpart')
                    continue
                if uw == pw:
                    repl.setdefault((lit[0], lit[1]), set()).add(lit[1])
                    continue
                e = _literal_edit(tok, lit, pv, uw, pw, body, report, ctx, k)
                if e:
                    changed += 1
                edits.extend(e)
                repl.setdefault((lit[0], lit[1]), set()).add(pv[1] if pv else lit[1])
        # duplicated delay slots: same literal, same instruction
        for i, tok in enumerate(toks):
            if i in consumed:
                continue
            key = (tok[0], tok[1])
            dup = [e for j, e in enumerate(exp) if (e[0][0], e[0][1]) == key and e[4] is not None]
            if not dup:
                continue
            outs = {(e[1][1] if e[1] else None, e[3] == e[4]) for e in dup}
            if len(outs) == 1:
                lit, pv, k, uw, pw = dup[0]
                consumed.add(i)
                if uw != pw:
                    e = _literal_edit(tok, lit, pv, uw, pw, body, report, ctx, k)
                    if e:
                        changed += 1
                    edits.extend(e)
            else:
                report.warn(f'{ctx}: ambiguous duplicate literal {tok[1]:#x}; review')
                consumed.add(i)
        handled = {(toks[i][0], toks[i][1]) for i in consumed}
        last_used = max(used_exp) if used_exp else -1
        for j, (lit, pv, k, uw, pw) in enumerate(exp):
            if j > last_used:
                break  # past the code this C# piece mirrors
            if (j not in used_exp and pw is not None and uw != pw
                    and (lit[0], lit[1]) not in handled
                    and (pv is None or pv[1] != lit[1])):
                missed += 1
                report.warn(f'{ctx}: literal {lit[1]:#x} of instr {k:#010x} (PAL '
                            f'{pv[1] if pv else None:#x}) not found in the C#; '
                            'check whether the patch hand-edited it')
        # literals not produced by an instruction: absolute code/data addresses
        for i, tok in enumerate(toks):
            if i in consumed or tok[0] != 'x':
                continue
            edits.extend(address_edit(tok, tr, report, ctx))
    edits.sort(key=lambda e: e[0], reverse=True)
    out = body
    for s0, e0, r in edits:
        out = out[:s0] + r + out[e0:]
    return out, changed, missed


def is_ram_addr(v):
    """KSEG0 address inside the original 2 MB of PS1 RAM (extension RAM and
    scratchpad stay untouched)."""
    return 0x80010000 <= v < 0x80200000


def address_edit(tok, tr, report, ctx):
    v = tok[1]
    if not is_ram_addr(v):
        return []
    if v & 0xFFFF == 0:
        report.warn(f'{ctx}: free-standing upper-half literal {v:#010x} left as is; review')
        return []
    nv, q = tr(v)
    if nv is None:
        report.warn(f'{ctx}: no mapping for {v:#010x}')
        return []
    if q != 'exact':
        report.warn(f'{ctx}: {v:#010x} -> {nv:#010x} ({q}); review')
    if nv == v:
        return []
    return [(tok[2], tok[3], fmt_hex(nv, tok[4], tok[5]))]


def rewrite_outside(text, tr, report, ctx):
    """Translate address literals in code that is not a recompiled function."""
    edits = []
    for tok in literal_tokens(text):
        if tok[0] == 'x':
            edits.extend(address_edit(tok, tr, report, ctx))
    edits.sort(key=lambda e: e[0], reverse=True)
    for s, e, r in edits:
        text = text[:s] + r + text[e:]
    return text


def rename_labels(body, code_tr, report, ctx):
    def sub(m):
        v = int(m.group(1), 16)
        nv, q = code_tr(v)
        if nv is None:
            report.warn(f'{ctx}: label L{v:08X} not mapped')
            return m.group(0)
        if q != 'exact':
            report.warn(f'{ctx}: label L{v:08X} -> L{nv:08X} ({q})')
        return 'L%08X' % nv
    return LABEL_RE.sub(sub, body)


def rename_funcrefs(text, tr_for_overlay, report, ctx):
    def sub(m):
        ov, v = m.group(1), int(m.group(2), 16)
        nv, q = tr_for_overlay(ov)(v)
        if nv is None:
            report.warn(f'{ctx}: call MediEvil_{ov}.func_{v:08X} not mapped')
            return m.group(0)
        if q != 'exact':
            report.warn(f'{ctx}: call MediEvil_{ov}.func_{v:08X} -> {nv:08X} ({q})')
        return 'MediEvil_%s.func_%08X' % (ov, nv)
    return FUNCREF_RE.sub(sub, text)


GP_USE_RE = re.compile(r'\bGP\s*\+\s*(\w+)')
GP_LIT_RE = re.compile(r'\bGP\s*\+\s*0x([0-9A-Fa-f]+)(u?)')


def gp_offset_names(text):
    """Identifiers used as $gp offsets ('c.GP + NAME') and the lists they come from."""
    names = {n for n in GP_USE_RE.findall(text) if not n.startswith('0x')}
    lists = set()
    for var in list(names):
        for m in re.finditer(r'foreach\s*\(\s*var\s+' + re.escape(var) + r'\s+in\s+(\w+)\s*\)', text):
            lists.add(m.group(1))
    return names, lists


def rewrite_gp_offsets(text, names, lists, gp_tr, report, ctx):
    """Translate $gp-relative offsets held in constants, lists or direct literals.

    gp_tr(offset) -> (new_offset, quality)
    """
    edits = []

    def lit_edit(m_start, digits, suffix, what):
        v = int(digits, 16)
        nv, q = gp_tr(v)
        if nv is None:
            report.warn(f'{ctx}: $gp offset {what} {v:#x} not mapped')
            return
        if q != 'exact':
            report.warn(f'{ctx}: $gp offset {what} {v:#x} -> {nv:#x} ({q}); review')
        if nv != v:
            edits.append((m_start, m_start + 2 + len(digits) + len(suffix),
                          fmt_hex(nv, digits, suffix)))

    for name in names:
        for m in re.finditer(r'\b(?:const\s+uint|uint)\s+' + re.escape(name) +
                             r'\s*=\s*0x([0-9A-Fa-f]+)(u?)\s*;', text):
            lit_edit(m.start(1) - 2, m.group(1), m.group(2), name)
    for lst in lists:
        for m in re.finditer(r'\b' + re.escape(lst) + r'\s*=\s*[\[{]([^\]}]*)[\]}]', text):
            for h in re.finditer(r'0x([0-9A-Fa-f]+)(u?)', m.group(1)):
                lit_edit(m.start(1) + h.start(), h.group(1), h.group(2), lst + '[]')
    for m in GP_LIT_RE.finditer(text):
        lit_edit(m.start(1) - 2, m.group(1), m.group(2), 'literal')
    edits.sort(key=lambda e: e[0], reverse=True)
    for s0, e0, r in edits:
        text = text[:s0] + r + text[e0:]
    return text
