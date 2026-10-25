# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvil PAL port tools - USA -> PAL function matcher
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

"""Matches every function of a USA image to its counterpart in the PAL image.

Strategy
  1. exact   : normalised bodies identical (same length, same instructions,
               only absolute addresses differ)
  2. aligned : same length and same instruction structure, but some
               immediates differ (PAL 50 Hz timing constants, struct offsets).
               Addresses still map 1:1, instruction by instruction.
  3. fuzzy   : body changed in PAL; located with difflib against a window
               around the predicted address, start/size taken from the
               aligned region. These need manual review.
"""

import difflib
from collections import defaultdict

from mips import analyse, coarse, loose

KEY = 4


class FuncMatch:
    __slots__ = ('name', 'usa', 'size', 'pal', 'pal_size', 'kind', 'ratio',
                 'usa_refs', 'pal_refs', 'cands')

    def __init__(self, usa, size):
        self.usa, self.size = usa, size
        self.pal = None
        self.pal_size = None
        self.kind = 'missing'
        self.ratio = 0.0
        self.usa_refs = self.pal_refs = None
        self.cands = 0


class ImageMatcher:
    def __init__(self, name, usa_img, pal_img, usa_gp=None, pal_gp=None):
        self.name = name
        self.ugp, self.pgp = usa_gp, pal_gp
        self.u = usa_img
        self.p = pal_img
        self.uptr = (usa_img.base, usa_img.end)
        self.pptr = (pal_img.base, pal_img.end)
        n = len(pal_img.data) // 4
        self.p_words = pal_img.words(pal_img.base, n * 4)
        self.p_coarse = [coarse(w) for w in self.p_words]
        self.index = defaultdict(list)
        for i in range(n - KEY + 1):
            self.index[tuple(self.p_coarse[i:i + KEY])].append(pal_img.base + i * 4)

    def _candidates(self, ucoarse):
        if len(ucoarse) >= KEY:
            return self.index.get(tuple(ucoarse[:KEY]), [])
        # very short function: linear scan
        k = len(ucoarse)
        out = []
        for i in range(len(self.p_coarse) - k + 1):
            if self.p_coarse[i:i + k] == ucoarse:
                out.append(self.p.base + i * 4)
        return out

    def match(self, funcs):
        """funcs: list of (address, size) from the USA funcmap."""
        res = [FuncMatch(a, s) for a, s in funcs]
        # pass 1: every exact / aligned candidate of every function
        cand = []
        for fm in res:
            uw = self.u.words(fm.usa, fm.size)
            unorm, urefs = analyse(uw, fm.usa, self.ugp, self.uptr)
            fm.usa_refs = urefs
            uloose = self._loose(unorm)
            ok, near = [], []
            for c in self._candidates([coarse(w) for w in uw]):
                if not self.p.contains(c + fm.size - 4):
                    continue
                pn, pr = analyse(self.p.words(c, fm.size), c, self.pgp, self.pptr)
                if pn == unorm:
                    ok.append((c, pr))
                elif self._loose(pn) == uloose:
                    near.append((c, pr))
            cand.append((ok, near))
            fm.cands = len(ok) or len(near)

        def assign(fm, pick, kind, claimed):
            fm.pal, fm.pal_refs = pick
            fm.kind = kind
            fm.pal_size = fm.size
            fm.ratio = 1.0
            claimed.add(fm.pal)

        # pass 2: unique matches become anchors
        claimed = set()
        for fm, (ok, near) in zip(res, cand):
            if len(ok) == 1:
                assign(fm, ok[0], 'exact', claimed)
            elif not ok and len(near) == 1:
                assign(fm, near[0], 'aligned', claimed)
        # drop anchors that break the USA order (spurious unique hits)
        anchors = [i for i, fm in enumerate(res) if fm.pal is not None]
        keep = self._longest_increasing(anchors, res)
        for i in anchors:
            if i not in keep:
                res[i].pal = None
                res[i].kind = 'missing'
                claimed.discard(res[i].pal)
        claimed = {res[i].pal for i in keep}

        # pass 2b: runs of USA-contiguous ambiguous functions (stub tables, data
        # pairs) are placed together, so their internal order is preserved
        n = len(res)
        i = 0
        while i < n:
            if res[i].pal is not None or not (cand[i][0] or cand[i][1]):
                i += 1
                continue
            j = i
            while (j + 1 < n and res[j + 1].pal is None and (cand[j + 1][0] or cand[j + 1][1])
                   and res[j].usa + res[j].size == res[j + 1].usa):
                j += 1
            if j > i:
                self._place_run(res, cand, i, j, claimed, assign)
            i = j + 1

        # pass 3: ambiguous functions, constrained between their anchors
        for i, (fm, (ok, near)) in enumerate(zip(res, cand)):
            if fm.pal is not None:
                continue
            prev = next((res[j] for j in range(i - 1, -1, -1) if res[j].pal is not None), None)
            nxt = next((res[j] for j in range(i + 1, n) if res[j].pal is not None), None)
            lo = prev.pal + prev.pal_size if prev else self.p.base
            hi = nxt.pal if nxt else self.p.end
            # predict from the closest anchor (in USA distance): stubs right before an
            # anchor follow its delta, stubs right after one follow the previous delta
            ref = min((x for x in (prev, nxt) if x is not None),
                      key=lambda x: abs(x.usa - fm.usa), default=None)
            pred = fm.usa + ((ref.pal - ref.usa) if ref else 0)

            def inside(lst):
                # candidates between the anchors, nearest to the prediction first;
                # lo moves forward as we assign, so identical stubs stay in sequence
                return sorted((x for x in lst if lo <= x[0] and x[0] + fm.size <= hi
                               and x[0] not in claimed), key=lambda x: abs(x[0] - pred))
            e_in, a_in = inside(ok), inside(near)
            if e_in:
                assign(fm, e_in[0], 'exact', claimed)
            elif a_in:
                assign(fm, a_in[0], 'aligned', claimed)
            else:
                # nothing fits between the anchors: nearest to the prediction, flagged later
                rest = [(x, 'exact') for x in ok if x[0] not in claimed] + \
                       [(x, 'aligned') for x in near if x[0] not in claimed]
                # keep PAL order whenever possible
                rest = [t for t in rest if t[0][0] >= lo] or rest
                rest.sort(key=lambda t: abs(t[0][0] - pred))
                if rest and abs(rest[0][0][0] - pred) <= 0x2000:
                    assign(fm, rest[0][0], rest[0][1], claimed)
        self._fuzzy(res)
        return res

    def _place_run(self, res, cand, i, j, claimed, assign):
        """Place res[i..j] (USA-contiguous) at one PAL offset where every member matches."""
        n = len(res)
        prev = next((res[k] for k in range(i - 1, -1, -1) if res[k].pal is not None), None)
        nxt = next((res[k] for k in range(j + 1, n) if res[k].pal is not None), None)
        lo = prev.pal + prev.pal_size if prev else self.p.base
        hi = nxt.pal if nxt else self.p.end
        ref = min((x for x in (prev, nxt) if x is not None),
                  key=lambda x: min(abs(x.usa - res[i].usa), abs(x.usa - res[j].usa)), default=None)
        delta_pred = (ref.pal - ref.usa) if ref else 0
        base = res[i].usa
        per = []
        for k in range(i, j + 1):
            ok, near = cand[k]
            per.append({c - (res[k].usa - base): (c, pr, 'exact') for c, pr in ok})
            for c, pr in near:
                per[-1].setdefault(c - (res[k].usa - base), (c, pr, 'aligned'))
        common = set(per[0])
        for d in per[1:]:
            common &= set(d)
        span = res[j].usa + res[j].size - base
        common = [s0 for s0 in common
                  if not any(d[s0][0] in claimed for d in per)]
        inside = [s0 for s0 in common if lo <= s0 and s0 + span <= hi]
        pool = inside or common
        if not pool:
            return
        s0 = min(pool, key=lambda x: abs(x - (base + delta_pred)))
        for k, d in zip(range(i, j + 1), per):
            c, pr, kind = d[s0]
            assign(res[k], (c, pr), kind, claimed)

    @staticmethod
    def _longest_increasing(idx, res):
        """Indices (subset of idx) whose PAL addresses increase with USA order."""
        if not idx:
            return set()
        vals = [res[i].pal for i in idx]
        import bisect
        tails, tails_i, prev = [], [], [-1] * len(vals)
        for k, v in enumerate(vals):
            p = bisect.bisect_left(tails, v)
            if p == len(tails):
                tails.append(v)
                tails_i.append(k)
            else:
                tails[p] = v
                tails_i[p] = k
            prev[k] = tails_i[p - 1] if p > 0 else -1
        out = set()
        k = tails_i[-1]
        while k != -1:
            out.add(idx[k])
            k = prev[k]
        return out

    @staticmethod
    def _loose(norm):
        return [loose(w) for w in norm]

    def _map_index(self, blocks, a_idx):
        """Translate an index of sequence A to sequence B through matching blocks."""
        best = None
        for b in blocks:
            if b.a <= a_idx < b.a + b.size:
                return b.b + (a_idx - b.a)
            if b.a > a_idx and best is None:
                best = b.b - (b.a - a_idx)  # first block after: keep its offset
        if best is None and blocks:
            last = blocks[-1]
            best = last.b + (a_idx - last.a)
        return best

    def _snap(self, addr, lo, hi, radius=16):
        """Move addr to the closest real function boundary in the PAL image
        (instruction right after a 'jr ra' + delay slot), if one is near."""
        best = None
        for d in range(0, radius + 1):
            for c in (addr - d * 4, addr + d * 4):
                if lo <= c < hi and self.p.contains(c - 8) and self.p.word(c - 8) == 0x03E00008:
                    best = c
                    break
            if best is not None:
                return best
        return addr

    def _fuzzy(self, res):
        """Locate changed functions in the PAL gap left between matched ones."""
        good = ('exact', 'aligned')
        i = 0
        n = len(res)
        while i < n:
            if res[i].kind in good:
                i += 1
                continue
            j = i
            while j < n and res[j].kind not in good:
                j += 1
            run = res[i:j]
            prev = res[i - 1] if i > 0 else None
            nxt = res[j] if j < n else None
            i = j
            u_lo, u_hi = run[0].usa, run[-1].usa + run[-1].size
            span = u_hi - u_lo
            prev_contig = prev is not None and prev.usa + prev.size == u_lo
            next_contig = nxt is not None and nxt.usa == u_hi
            # PAL search window
            if prev:
                lo = prev.pal + prev.pal_size + (u_lo - (prev.usa + prev.size))
            else:
                lo = max(self.p.base, u_lo + ((nxt.pal - nxt.usa) if nxt else 0) - span)
            if nxt:
                hi = nxt.pal - (nxt.usa - u_hi)
            else:
                hi = min(self.p.end, lo + 2 * span + 256)
            if not prev:
                pass
            lo = max(lo, self.p.base)
            hi = min(hi, self.p.end)
            if hi <= lo:
                continue
            un, _ = analyse(self.u.words(u_lo, span), u_lo, self.ugp, self.uptr)
            pn, _ = analyse(self.p.words(lo, hi - lo), lo, self.pgp, self.pptr)
            sm = difflib.SequenceMatcher(None, un, pn, autojunk=False)
            blocks = [b for b in sm.get_matching_blocks() if b.size]
            if not blocks:
                continue
            starts = []
            for k, fm in enumerate(run):
                if k == 0 and prev_contig:
                    starts.append(lo)
                    continue
                m = self._map_index(blocks, (fm.usa - u_lo) // 4)
                starts.append(self._snap(lo + m * 4, lo, hi) if m is not None else None)
            if next_contig:
                end = hi
            else:
                last = blocks[-1]
                end = lo + (last.b + last.size + (len(un) - (last.a + last.size))) * 4
                end = min(max(end, lo), hi)
            for k, fm in enumerate(run):
                s0 = starts[k]
                e0 = next((x for x in starts[k + 1:] if x is not None), end)
                if s0 is None or e0 <= s0:
                    continue
                fm.pal, fm.pal_size = s0, e0 - s0
                fn, fm.pal_refs = analyse(self.p.words(s0, fm.pal_size), s0, self.pgp, self.pptr)
                uf, _ = analyse(self.u.words(fm.usa, fm.size), fm.usa, self.ugp, self.uptr)
                fm.ratio = difflib.SequenceMatcher(None, uf, fn, autojunk=False).ratio()
                fm.kind = 'fuzzy'
