# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvil PAL port tools - USA -> PAL address map
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

"""Address translation built from matched functions.

Two kinds of address spaces exist:
  * 'global'      : main EXE code/data/bss, boot EXE, everything outside the
                    overlay window
  * 'ov:<name>'   : the overlay window (0x80010000 .. game base); each overlay
                    has its own map because they all load at the same place
"""

import bisect
import difflib
from collections import Counter, defaultdict

from mips import analyse, loose

OVERLAY_WINDOW = (0x80010000, 0x80021CA4)
GLOBAL_IMAGES = ('game', 'main')


def space_for(addr, overlay):
    """Address space an address belongs to, seen from code of `overlay`."""
    if overlay not in GLOBAL_IMAGES and OVERLAY_WINDOW[0] <= addr < OVERLAY_WINDOW[1]:
        return 'ov:' + overlay
    return 'global'


class Space:
    def __init__(self, name):
        self.name = name
        self.votes = defaultdict(Counter)   # usa -> Counter(pal)
        self.code = {}                      # usa instruction -> pal instruction
        self._sorted = None

    def add_code(self, usa, pal):
        self.code[usa] = pal
        self._sorted = None

    def add_ref(self, usa, pal, weight=1):
        self.votes[usa][pal] += weight
        self._sorted = None

    def conflicts(self):
        out = []
        for u, c in self.votes.items():
            if len(c) > 1:
                out.append((u, c.most_common()))
        return out

    def _anchors(self):
        if self._sorted is None:
            m = {u: c.most_common(1)[0][0] for u, c in self.votes.items()}
            m.update(self.code)  # instruction-level mapping wins
            keys = sorted(m)
            self._sorted = (keys, [m[k] for k in keys], m)
        return self._sorted

    def lookup(self, usa):
        """Return (pal, quality). quality: 'exact' | 'inferred' | 'guess' | None."""
        keys, vals, m = self._anchors()
        if usa in m:
            return m[usa], 'exact'
        i = bisect.bisect_left(keys, usa)
        below = (keys[i - 1], vals[i - 1]) if i > 0 else None
        above = (keys[i], vals[i]) if i < len(keys) else None
        if below and above:
            d1 = below[1] - below[0]
            d2 = above[1] - above[0]
            if d1 == d2:
                return usa + d1, 'inferred'
            # closest anchor wins, flagged
            near = below if usa - below[0] <= above[0] - usa else above
            return usa + (near[1] - near[0]), 'guess'
        near = below or above
        if near:
            return usa + (near[1] - near[0]), 'guess'
        return None, None


class AddressMap:
    def __init__(self):
        self.spaces = {}

    def space(self, name):
        if name not in self.spaces:
            self.spaces[name] = Space(name)
        return self.spaces[name]

    def lookup(self, usa, overlay):
        return self.space(space_for(usa, overlay)).lookup(usa)

    # ------------------------------------------------------------------
    def feed(self, overlay, matcher, matches):
        """Populate the map from the matches of one image."""
        u, p = matcher.u, matcher.p
        own = self.space('global' if overlay in GLOBAL_IMAGES else 'ov:' + overlay)
        for fm in matches:
            if fm.pal is None:
                continue
            if fm.kind in ('exact', 'aligned'):
                for k in range(0, fm.size, 4):
                    own.add_code(fm.usa + k, fm.pal + k)
                if len(fm.usa_refs) == len(fm.pal_refs):
                    for (ui, _, ua), (pi, _, pa) in zip(fm.usa_refs, fm.pal_refs):
                        if ui == pi:
                            self.space(space_for(ua, overlay)).add_ref(ua, pa, 3)
                continue
            # fuzzy: instruction alignment through difflib on loose words
            un, ur = analyse(u.words(fm.usa, fm.size), fm.usa, matcher.ugp, matcher.uptr)
            pn, pr = analyse(p.words(fm.pal, fm.pal_size), fm.pal, matcher.pgp, matcher.pptr)
            sm = difflib.SequenceMatcher(None, [loose(w) for w in un],
                                         [loose(w) for w in pn], autojunk=False)
            imap = {}
            for b in sm.get_matching_blocks():
                for t in range(b.size):
                    imap[b.a + t] = b.b + t
            own.add_code(fm.usa, fm.pal)  # function entry always maps
            for a_i, b_i in imap.items():
                own.add_code(fm.usa + a_i * 4, fm.pal + b_i * 4)
            pref = {pi: pa for pi, _, pa in pr}
            for ui, _, ua in ur:
                if ui in imap and imap[ui] in pref:
                    self.space(space_for(ua, overlay)).add_ref(ua, pref[imap[ui]], 1)
