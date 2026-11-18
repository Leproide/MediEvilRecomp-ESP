# MediEvilRecomp USA -> PAL port report

Author: https://github.com/Leproide  
License: GPL-3.0

MEDIEVIL.EXE  USA base 0x80021d14 gp 0x800ed848  |  PAL base 0x80021cec gp 0x800ed60c

## Function matching

| image | functions | exact | aligned | fuzzy | missing |
|---|---|---|---|---|---|
| main | 338 | 338 | 0 | 0 | 0 |
| game | 1845 | 1744 | 99 | 2 | 0 |
| ac | 142 | 139 | 3 | 0 | 0 |
| ag | 98 | 97 | 1 | 0 | 0 |
| cc | 104 | 101 | 3 | 0 | 0 |
| ch | 66 | 65 | 1 | 0 | 0 |
| cr | 11 | 10 | 1 | 0 | 0 |
| credits | 14 | 12 | 2 | 0 | 0 |
| dc | 139 | 135 | 4 | 0 | 0 |
| ee | 170 | 167 | 3 | 0 | 0 |
| eh | 24 | 21 | 3 | 0 | 0 |
| gg | 27 | 26 | 1 | 0 | 0 |
| gs | 83 | 81 | 2 | 0 | 0 |
| gy1 | 45 | 45 | 0 | 0 | 0 |
| gy2 | 91 | 90 | 1 | 0 | 0 |
| hh | 97 | 96 | 1 | 0 | 0 |
| hr | 121 | 117 | 4 | 0 | 0 |
| ia | 51 | 50 | 1 | 0 | 0 |
| la | 69 | 68 | 1 | 0 | 0 |
| landmap | 33 | 27 | 6 | 0 | 0 |
| pd | 56 | 55 | 1 | 0 | 0 |
| pg | 70 | 66 | 4 | 0 | 0 |
| ps | 117 | 113 | 4 | 0 | 0 |
| sf | 135 | 134 | 1 | 0 | 0 |
| sv | 107 | 104 | 3 | 0 | 0 |
| td | 117 | 116 | 1 | 0 | 0 |
| tl | 98 | 94 | 4 | 0 | 0 |
| zl | 131 | 130 | 1 | 0 | 0 |

*exact*: same code, only addresses differ. *aligned*: same code, some constants differ (50 Hz timing, offsets). *fuzzy*: code changed in PAL.

## C# patch methods

| file | method | overlay | match | literals rewritten | unresolved |
|---|---|---|---|---|---|
| PalMovies.cs | func_80079D90 | game | exact | 0 | 23 |
| render\CullPatch.cs | func_80021D5C | game | exact | 0 | 5 |
| render\CullPatch.cs | func_80021E5C | game | exact | 0 | 0 |
| render\CullPatch.cs | func_80022070 | game | exact | 0 | 0 |
| render\CullPatch.cs | func_80022460 | game | exact | 0 | 0 |
| render\CullPatch.cs | func_80024B00 | game | exact | 0 | 4 |
| render\CullPatch.cs | func_800252D0 | game | exact | 0 | 3 |
| render\CullPatch.cs | func_80010800_tl | tl | exact | 0 | 8 |
| render\CullPatch.cs | func_8009A540 | game | exact | 0 | 0 |
| render\CullPatch.cs | func_8009A694 | game | exact | 0 | 1 |
| render\CullPatch.cs | func_800B3748 | game | exact | 0 | 10 |
| render\CullPatch.cs | func_800B4028 | game | exact | 0 | 5 |
| render\CullPatch.cs | func_800B4244 | game | exact | 7 | 32 |
| render\CullPatch.cs | func_800A10D8 | game | exact | 0 | 4 |
| widescreen\LandMapPatch.cs | func_80012BBC | landmap | exact | 0 | 2 |
| widescreen\LandMapPatch.cs | func_80012F70 | landmap | exact | 0 | 7 |
| widescreen\LandMapPatch.cs | func_80012FDC_landmap | landmap | exact | 0 | 5 |
| widescreen\LandMapPatch.cs | func_80013328_landmap | landmap | exact | 0 | 1 |
| widescreen\LandMapPatch.cs | func_80013C3C | landmap | exact | 0 | 8 |
| widescreen\LandMapPatch.cs | func_80013C8C | landmap | exact | 0 | 2 |

- relocation gapsPolySquaresCaptured: 0x800eec88 -> 0x800eea5c (exact)

## Warnings (310)

- game: USA 0x80065dc0 ~ PAL 0x80065db8 weak match (similarity 0.42); rewritten in PAL
- landmap: PAL-only code at 0x8001448c (+328) added to funcmap
- patch Recompiled.PalCdCallbacks.SyncFromMemory @ game:0x80065dc0: function is fuzzy in PAL -> patch DISABLED (original PAL code runs instead)
- GameAddresses.cs:<consts>: 0x800eb6e0 -> 0x800eb4a4 (inferred); review
- GameAddresses.cs:<consts>: 0x800eb7b8 -> 0x800eb57c (inferred); review
- GameAddresses.cs:<consts>: 0x800eb5e0 -> 0x800eb3a4 (inferred); review
- GameAddresses.cs:<consts>: 0x800eb6e4 -> 0x800eb4a8 (inferred); review
- GameAddresses.cs:<consts>: 0x800eb7bc -> 0x800eb580 (inferred); review
- GameAddresses.cs:<consts>: 0x800eb5e4 -> 0x800eb3a8 (inferred); review
- GameAddresses.cs:<consts>: 0x800eb700 -> 0x800eb4c4 (inferred); review
- GameAddresses.cs:<consts>: 0x800eb7d8 -> 0x800eb59c (inferred); review
- GameAddresses.cs:<consts>: 0x800eb600 -> 0x800eb3c4 (inferred); review
- GameAddresses.cs:<consts>: 0x800f1a28 -> 0x800f17fc (inferred); review
- GameAddresses.cs:<consts>: 0x800f1aec -> 0x800f18c0 (inferred); review
- GameAddresses.cs:<consts>: 0x800f1924 -> 0x800f16f8 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef09c -> 0x800eee70 (inferred); review
- GameAddresses.cs:<consts>: 0x800eef98 -> 0x800eed6c (inferred); review
- GameAddresses.cs:<consts>: 0x800f1a94 -> 0x800f1868 (inferred); review
- GameAddresses.cs:<consts>: 0x800eece0 -> 0x800eeab4 (inferred); review
- GameAddresses.cs:<consts>: 0x800eeda4 -> 0x800eeb78 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef108 -> 0x800eeedc (inferred); review
- GameAddresses.cs:<consts>: 0x800ef1cc -> 0x800eefa0 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef004 -> 0x800eedd8 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef158 -> 0x800eef2c (inferred); review
- GameAddresses.cs:<consts>: 0x800ef21c -> 0x800eeff0 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef054 -> 0x800eee28 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef0c0 -> 0x800eee94 (inferred); review
- GameAddresses.cs:<consts>: 0x800eefbc -> 0x800eed90 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef11a -> 0x800eeeee (inferred); review
- GameAddresses.cs:<consts>: 0x800ef1de -> 0x800eefb2 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef016 -> 0x800eedea (inferred); review
- GameAddresses.cs:<consts>: 0x800ce09c -> 0x800ce004 (inferred); review
- GameAddresses.cs:<consts>: 0x800ce0c4 -> 0x800ce02c (inferred); review
- GameAddresses.cs:<consts>: 0x800cdfa8 -> 0x800cdf10 (inferred); review
- GameAddresses.cs:<consts>: 0x800ce0c4 -> 0x800ce02c (inferred); review
- GameAddresses.cs:<consts>: 0x800ce0ec -> 0x800ce054 (inferred); review
- GameAddresses.cs:<consts>: 0x800cdfd0 -> 0x800cdf38 (inferred); review
- GameAddresses.cs:<consts>: 0x800db32c -> 0x800db0f0 (inferred); review
- GameAddresses.cs:<consts>: 0x800db404 -> 0x800db1c8 (inferred); review
- GameAddresses.cs:<consts>: 0x800db22c -> 0x800daff0 (inferred); review
- GameAddresses.cs:<consts>: 0x800db330 -> 0x800db0f4 (inferred); review
- GameAddresses.cs:<consts>: 0x800db408 -> 0x800db1cc (inferred); review
- GameAddresses.cs:<consts>: 0x800db230 -> 0x800daff4 (inferred); review
- GameAddresses.cs:<consts>: 0x800db3b4 -> 0x800db178 (inferred); review
- GameAddresses.cs:<consts>: 0x800db48c -> 0x800db250 (inferred); review
- GameAddresses.cs:<consts>: 0x800db40c -> 0x800db1d0 (inferred); review
- GameAddresses.cs:<consts>: 0x800db234 -> 0x800daff8 (inferred); review
- GameAddresses.cs:<consts>: 0x800eda8c -> 0x800ed860 (inferred); review
- GameAddresses.cs:<consts>: 0x800edb50 -> 0x800ed924 (inferred); review
- GameAddresses.cs:<consts>: 0x800ed984 -> 0x800ed748 (guess); review
- GameAddresses.cs:<consts>: 0x80011e98 -> 0x80011ea0 (inferred); review
- GameAddresses.cs:<consts>: 0x80011e60 -> 0x80011e68 (inferred); review
- GameAddresses.cs:<consts>: 0x80011e40 -> 0x80011e48 (inferred); review
- GameAddresses.cs:<consts>: 0x80011ea0 -> 0x80011ea8 (inferred); review
- GameAddresses.cs:<consts>: 0x800ce5e8 -> 0x800ce550 (guess); review
- GameAddresses.cs:<consts>: 0x800ce4cc -> 0x800ce434 (inferred); review
- GameAddresses.cs:<consts>: 0x800ed8c8 -> 0x800ed68c (inferred); review
- GameAddresses.cs:<consts>: 0x800ed9a0 -> 0x800ed784 (guess); review
- GameAddresses.cs:<consts>: 0x800ed7c8 -> 0x800ed58c (inferred); review
- GameAddresses.cs:<consts>: 0x800edaa0 -> 0x800ed874 (inferred); review
- GameAddresses.cs:<consts>: 0x800edb64 -> 0x800ed938 (inferred); review
- GameAddresses.cs:<consts>: 0x800ed998 -> 0x800ed77c (guess); review
- GameAddresses.cs:<consts>: 0x800ee0d0 -> 0x800edea4 (inferred); review
- GameAddresses.cs:<consts>: 0x800ee194 -> 0x800edf68 (inferred); review
- GameAddresses.cs:<consts>: 0x800ee0d8 -> 0x800edeac (inferred); review
- GameAddresses.cs:<consts>: 0x800ee19c -> 0x800edf70 (inferred); review
- GameAddresses.cs:<consts>: 0x800eeac8 -> 0x800ee89c (inferred); review
- GameAddresses.cs:<consts>: 0x800eeb8c -> 0x800ee960 (inferred); review
- GameAddresses.cs:<consts>: 0x800ee9c4 -> 0x800ee798 (inferred); review
- GameAddresses.cs:<consts>: 0x800eeb60 -> 0x800ee934 (inferred); review
- GameAddresses.cs:<consts>: 0x800eea5c -> 0x800ee830 (inferred); review
- GameAddresses.cs:<consts>: 0x800eebf8 -> 0x800ee9cc (inferred); review
- GameAddresses.cs:<consts>: 0x800eecbc -> 0x800eea90 (inferred); review
- GameAddresses.cs:<consts>: 0x800eeaf4 -> 0x800ee8c8 (inferred); review
- GameAddresses.cs:<consts>: 0x800eec0c -> 0x800ee9e0 (inferred); review
- GameAddresses.cs:<consts>: 0x800eecd0 -> 0x800eeaa4 (inferred); review
- GameAddresses.cs:<consts>: 0x800eeb08 -> 0x800ee8dc (inferred); review
- GameAddresses.cs:<consts>: 0x800f3b80 -> 0x800f3954 (inferred); review
- GameAddresses.cs:<consts>: 0x800f3c44 -> 0x800f3a18 (inferred); review
- GameAddresses.cs:<consts>: 0x800f3a7c -> 0x800f3850 (inferred); review
- GameAddresses.cs:<consts>: 0x800d73a8 -> 0x800d7310 (inferred); review
- GameAddresses.cs:<consts>: 0x800d73d0 -> 0x800d7338 (inferred); review
- GameAddresses.cs:<consts>: 0x800d72b4 -> 0x800d721c (inferred); review
- GameAddresses.cs:<consts>: 0x800d73c0 -> 0x800d7328 (inferred); review
- GameAddresses.cs:<consts>: 0x800d5274 -> 0x800d51dc (inferred); review
- GameAddresses.cs:<consts>: 0x800d529c -> 0x800d5204 (inferred); review
- GameAddresses.cs:<consts>: 0x800d528c -> 0x800d51f4 (inferred); review
- GameAddresses.cs:<consts>: 0x80012fd4 -> 0x80012fd4 (inferred); review
- GameAddresses.cs:<consts>: 0x80012f9c -> 0x80012f9c (inferred); review
- GameAddresses.cs:<consts>: 0x80012f7c -> 0x80012f7c (inferred); review
- GameAddresses.cs:<consts>: 0x80012fdc -> 0x80012fdc (inferred); review
- GameAddresses.cs:<consts>: 0x80013320 -> 0x80013320 (inferred); review
- GameAddresses.cs:<consts>: 0x800132e8 -> 0x800132e8 (inferred); review
- GameAddresses.cs:<consts>: 0x800132c8 -> 0x800132c8 (inferred); review
- GameAddresses.cs:<consts>: 0x80013328 -> 0x80013328 (inferred); review
- GameAddresses.cs:<consts>: 0x80013508 -> 0x80013508 (inferred); review
- GameAddresses.cs:<consts>: 0x800134d0 -> 0x800134d0 (inferred); review
- GameAddresses.cs:<consts>: 0x800134b0 -> 0x800134b0 (inferred); review
- GameAddresses.cs:<consts>: 0x80013510 -> 0x80013510 (inferred); review
- GameAddresses.cs:<consts>: 0x80013550 -> 0x80013550 (inferred); review
- GameAddresses.cs:<consts>: 0x80013518 -> 0x80013518 (inferred); review
- GameAddresses.cs:<consts>: 0x800134f8 -> 0x800134f8 (inferred); review
- GameAddresses.cs:<consts>: 0x80013558 -> 0x80013558 (inferred); review
- GameAddresses.cs:<consts>: 0x800135a0 -> 0x800135a0 (inferred); review
- GameAddresses.cs:<consts>: 0x80013568 -> 0x80013568 (inferred); review
- GameAddresses.cs:<consts>: 0x80013548 -> 0x80013548 (inferred); review
- GameAddresses.cs:<consts>: 0x800135a8 -> 0x800135a8 (inferred); review
- GameAddresses.cs:<consts>: 0x80013770 -> 0x80013770 (inferred); review
- GameAddresses.cs:<consts>: 0x80013738 -> 0x80013738 (inferred); review
- GameAddresses.cs:<consts>: 0x80013718 -> 0x80013718 (inferred); review
- GameAddresses.cs:<consts>: 0x80013778 -> 0x80013778 (inferred); review
- GameAddresses.cs:<consts>: 0x80013790 -> 0x80013790 (inferred); review
- GameAddresses.cs:<consts>: 0x80013758 -> 0x80013758 (inferred); review
- GameAddresses.cs:<consts>: 0x80013738 -> 0x80013738 (inferred); review
- GameAddresses.cs:<consts>: 0x80013798 -> 0x80013798 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d80 -> 0x80013d80 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d48 -> 0x80013d48 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d28 -> 0x80013d28 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d88 -> 0x80013d88 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d88 -> 0x80013d88 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d50 -> 0x80013d50 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d30 -> 0x80013d30 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d90 -> 0x80013d90 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d90 -> 0x80013d90 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d58 -> 0x80013d58 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d38 -> 0x80013d38 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d98 -> 0x80013d98 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d98 -> 0x80013d98 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d60 -> 0x80013d60 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d40 -> 0x80013d40 (inferred); review
- GameAddresses.cs:<consts>: 0x80013da0 -> 0x80013da0 (inferred); review
- GameAddresses.cs:<consts>: 0x80013db4 -> 0x80013db4 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d7c -> 0x80013d7c (inferred); review
- GameAddresses.cs:<consts>: 0x80013d5c -> 0x80013d5c (inferred); review
- GameAddresses.cs:<consts>: 0x80013dbc -> 0x80013dbc (inferred); review
- GameAddresses.cs:<consts>: 0x80013fe8 -> 0x80013fe8 (inferred); review
- GameAddresses.cs:<consts>: 0x80013fb0 -> 0x80013fb0 (inferred); review
- GameAddresses.cs:<consts>: 0x80013f90 -> 0x80013f90 (inferred); review
- GameAddresses.cs:<consts>: 0x80013ff0 -> 0x80013ff0 (inferred); review
- GameAddresses.cs:<consts>: 0x800104c4 -> 0x80010464 (guess); review
- GameAddresses.cs:<consts>: 0x800104c4 -> 0x80010464 (guess); review
- GameAddresses.cs:<consts>: 0x800104bc -> 0x8001045c (guess); review
- GameAddresses.cs:<consts>: 0x800104bc -> 0x8001045c (guess); review
- GameAddresses.cs:<consts>: 0x800105c0 -> 0x80010560 (guess); review
- GameAddresses.cs:<consts>: 0x800105c0 -> 0x80010560 (guess); review
- GameAddresses.cs:<consts>: 0x800105b8 -> 0x80010558 (guess); review
- GameAddresses.cs:<consts>: 0x800105b8 -> 0x80010558 (guess); review
- GameAddresses.cs:<consts>: 0x80010854 -> 0x8001082c (guess); review
- GameAddresses.cs:<consts>: 0x80010854 -> 0x8001082c (guess); review
- GameAddresses.cs:<consts>: 0x8001084c -> 0x80010824 (guess); review
- GameAddresses.cs:<consts>: 0x8001084c -> 0x80010824 (guess); review
- GameAddresses.cs:<consts>: 0x800108a8 -> 0x80010880 (guess); review
- GameAddresses.cs:<consts>: 0x800108a8 -> 0x80010880 (guess); review
- GameAddresses.cs:<consts>: 0x800108a0 -> 0x80010878 (guess); review
- GameAddresses.cs:<consts>: 0x800108a0 -> 0x80010878 (guess); review
- GameAddresses.cs:<consts>: 0x80010a80 -> 0x80010a58 (guess); review
- GameAddresses.cs:<consts>: 0x80010a80 -> 0x80010a58 (guess); review
- GameAddresses.cs:<consts>: 0x80010a78 -> 0x80010a50 (guess); review
- GameAddresses.cs:<consts>: 0x80010a78 -> 0x80010a50 (guess); review
- GameAddresses.cs:<consts>: 0x80010b1c -> 0x80010af4 (guess); review
- GameAddresses.cs:<consts>: 0x80010b1c -> 0x80010af4 (guess); review
- GameAddresses.cs:<consts>: 0x80010b14 -> 0x80010aec (guess); review
- GameAddresses.cs:<consts>: 0x80010b14 -> 0x80010aec (guess); review
- GameAddresses.cs:<consts>: 0x80010f08 -> 0x80010ee0 (inferred); review
- GameAddresses.cs:<consts>: 0x80010f08 -> 0x80010ee0 (inferred); review
- GameAddresses.cs:<consts>: 0x80010f00 -> 0x80010ed8 (inferred); review
- GameAddresses.cs:<consts>: 0x80010f00 -> 0x80010ed8 (inferred); review
- HeapExpansion.cs:<consts>: 0x800f3b80 -> 0x800f3954 (inferred); review
- PalCdCallbacks.cs:<consts>: 0x800eb6e0 -> 0x800eb4a4 (inferred); review
- PalCdCallbacks.cs:<consts>: 0x800eb6e4 -> 0x800eb4a8 (inferred); review
- PalLibCrypt.cs:<consts>: 0x80011ea0 -> 0x80011ea8 (inferred); review
- PalLibCrypt.cs:<consts>: 0x80011e98 -> 0x80011ea0 (inferred); review
- PalLibCrypt.cs:<consts>: 0x800db32c -> 0x800db0f0 (inferred); review
- PalMovies.cs:func_80079D90: literal 0x19b0 of instr 0x80079da4 (PAL 0x1784) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x760 of instr 0x80079ddc (PAL 0x770) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x764 of instr 0x80079de4 (PAL 0x774) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x768 of instr 0x80079df0 (PAL 0x778) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x760 of instr 0x80079df8 (PAL 0x770) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19f0 of instr 0x80079e14 (PAL 0x17c4) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x764 of instr 0x80079e1c (PAL 0x774) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19f0 of instr 0x80079e38 (PAL 0x17c4) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19b0 of instr 0x80079e5c (PAL 0x1784) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19ec of instr 0x80079ecc (PAL 0x17c0) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19b0 of instr 0x80079ed4 (PAL 0x1784) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x6c24 of instr 0x80079eec (PAL 0x69f8) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x6c24 of instr 0x80079f08 (PAL 0x69f8) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19e4 of instr 0x80079f10 (PAL 0x17b8) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19b0 of instr 0x80079ff4 (PAL 0x1784) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x248c of instr 0x8007a024 (PAL 0x26b8) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x2488 of instr 0x8007a048 (PAL 0x26b4) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x1a04 of instr 0x8007a050 (PAL 0x17d8) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x76c of instr 0x8007a084 (PAL 0x77c) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x1a1c of instr 0x8007a0c4 (PAL 0x17f0) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x1a1c of instr 0x8007a108 (PAL 0x17f0) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x1a1c of instr 0x8007a14c (PAL 0x17f0) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x1a1c of instr 0x8007a160 (PAL 0x17f0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80021D5C: literal 0xf024 of instr 0x80021da4 (PAL 0xedf8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80021D5C: literal 0xdff8 of instr 0x80021dac (PAL 0xddcc) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80021D5C: literal 0x1a1c of instr 0x80021db0 (PAL 0x17f0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80021D5C: literal 0xdde0 of instr 0x80021db4 (PAL 0xdbb4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80021D5C: literal 0xdddc of instr 0x80021db8 (PAL 0xdbb0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80024B00: literal 0x450 of instr 0x80024b08 (PAL 0x460) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80024B00: literal 0xfb0 of instr 0x80025120 (PAL 0x11dc) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80024B00: literal 0x5d0 of instr 0x80025294 (PAL 0x5e0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80024B00: literal 0xfbc of instr 0x80025298 (PAL 0x11e8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800252D0: literal 0x450 of instr 0x800252d8 (PAL 0x460) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800252D0: literal 0xe8c of instr 0x80025690 (PAL 0x10b8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800252D0: literal 0x5d0 of instr 0x800258b0 (PAL 0x5e0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0x23d8 of instr 0x8001097c (PAL 0x2604) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0x23d8 of instr 0x80010988 (PAL 0x2604) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0xfb8 of instr 0x800109ec (PAL 0x11e4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0x7ee0 of instr 0x80010a1c (PAL 0x7ed8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0xf2c of instr 0x80010a7c (PAL 0x1158) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0x1454 of instr 0x80010c3c (PAL 0x1218) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0xf80 of instr 0x80010d90 (PAL 0x11ac) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0xf80 of instr 0x80010e10 (PAL 0x11ac) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_8009A694: literal 0x23d8 of instr 0x8009a6c0 (PAL 0x2604) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x78dc of instr 0x800b37a8 (PAL 0x7844) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x7b0 of instr 0x800b37fc (PAL 0x7c0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x7b0 of instr 0x800b39b8 (PAL 0x7c0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x7b0 of instr 0x800b3bf8 (PAL 0x7c0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x7b0 of instr 0x800b3db4 (PAL 0x7c0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x791c of instr 0x800b4064 (PAL 0x7884) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x7b0 of instr 0x800b4160 (PAL 0x7c0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x1fa0 of instr 0x800b425c (PAL 0x21cc) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x23d8 of instr 0x800b4410 (PAL 0x2604) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x795c of instr 0x800b49d0 (PAL 0x78c4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4028: literal 0x791c of instr 0x800b4064 (PAL 0x7884) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4028: literal 0x7b0 of instr 0x800b4160 (PAL 0x7c0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4028: literal 0x1fa0 of instr 0x800b425c (PAL 0x21cc) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4028: literal 0x23d8 of instr 0x800b4410 (PAL 0x2604) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4028: literal 0x795c of instr 0x800b49d0 (PAL 0x78c4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x1fa0 of instr 0x800b425c (PAL 0x21cc) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x23d8 of instr 0x800b4410 (PAL 0x2604) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x795c of instr 0x800b49d0 (PAL 0x78c4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cb8 of instr 0x800b4e1c (PAL 0x2ef4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ca4 of instr 0x800b4e24 (PAL 0x2ee0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x6d24 of instr 0x800b4e34 (PAL 0x6af8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cf0 of instr 0x800b4e44 (PAL 0x2f2c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cec of instr 0x800b4e54 (PAL 0x2f28) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ce8 of instr 0x800b4e64 (PAL 0x2f24) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ce4 of instr 0x800b4e74 (PAL 0x2f20) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ce0 of instr 0x800b4e84 (PAL 0x2f1c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cdc of instr 0x800b4e94 (PAL 0x2f18) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cbc of instr 0x800b4ea8 (PAL 0x2ef8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ccc of instr 0x800b4eb0 (PAL 0x2f08) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x6cdc of instr 0x800b4ecc (PAL 0x6ab0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x6c94 of instr 0x800b4ed4 (PAL 0x6a68) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cb8 of instr 0x800b4f44 (PAL 0x2ef4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x6d24 of instr 0x800b4fd8 (PAL 0x6af8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2c98 of instr 0x800b4fec (PAL 0x2ed4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cb0 of instr 0x800b4ffc (PAL 0x2eec) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x3628 of instr 0x800b5040 (PAL 0x3864) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cb0 of instr 0x800b5048 (PAL 0x2eec) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cac of instr 0x800b5050 (PAL 0x2ee8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2c9c of instr 0x800b505c (PAL 0x2ed8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cb0 of instr 0x800b5068 (PAL 0x2eec) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ca8 of instr 0x800b5278 (PAL 0x2ee4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ca8 of instr 0x800b5378 (PAL 0x2ee4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ca8 of instr 0x800b539c (PAL 0x2ee4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x6d24 of instr 0x800b5440 (PAL 0x6af8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cdc of instr 0x800b5484 (PAL 0x2f18) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cdc of instr 0x800b5550 (PAL 0x2f18) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cdc of instr 0x800b5648 (PAL 0x2f18) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800A10D8: literal 0x1fa0 of instr 0x800a10e8 (PAL 0x21cc) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800A10D8: literal 0x23d8 of instr 0x800a13d8 (PAL 0x2604) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800A10D8: literal 0x3ab8 of instr 0x800a1aa0 (PAL 0x388c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800A10D8: literal 0x6ddc of instr 0x800a1d04 (PAL 0x6d44) not found in the C#; check whether the patch hand-edited it
- render\DistantObjectsPatch.cs:<consts>: 0x800eeac8 -> 0x800ee89c (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800eeb60 -> 0x800ee934 (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800eebf8 -> 0x800ee9cc (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800eec0c -> 0x800ee9e0 (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800d73c0 -> 0x800d7328 (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800d73a8 -> 0x800d7310 (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800d528c -> 0x800d51f4 (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800d5274 -> 0x800d51dc (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800109f8 -> 0x800109d0 (guess); review
- render\DistantObjectsPatch.cs:<consts>: 0x800109f8 -> 0x800109d0 (guess); review
- render\ShaderDrawDistance.cs:<consts>: 0x800ef0c0 -> 0x800eee94 (inferred); review
- render\TerrainCapture.cs:<consts>: free-standing upper-half literal 0x80010000 left as is; review
- render\TerrainEdgeFade.cs:<consts>: 0x800ef108 -> 0x800eeedc (inferred); review
- render\TerrainEdgeFade.cs:<consts>: 0x800ef158 -> 0x800eef2c (inferred); review
- render\TerrainEdgeFade.cs:<consts>: free-standing upper-half literal 0x80010000 left as is; review
- widescreen\FadeWidenPatch.cs:<consts>: 0x800d8368 -> 0x800d82d0 (inferred); review
- widescreen\LandMapPatch.cs:func_80012BBC: literal 0x4ec8 of instr 0x80012bc4 (PAL 0x4fb0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012BBC: literal 0x4530 of instr 0x80012be4 (PAL 0x4618) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x1454 of instr 0x80012f88 (PAL 0x1218) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x4ec8 of instr 0x80012fe4 (PAL 0x4fb0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x21b4 of instr 0x80013274 (PAL 0x23e0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x4eb8 of instr 0x80013284 (PAL 0x4fa0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x2228 of instr 0x8001328c (PAL 0x2454) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x4ec8 of instr 0x800132d0 (PAL 0x4fb0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0xf80 of instr 0x80013438 (PAL 0x11ac) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012FDC_landmap: literal 0x4ec8 of instr 0x80012fe4 (PAL 0x4fb0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012FDC_landmap: literal 0x21b4 of instr 0x80013274 (PAL 0x23e0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012FDC_landmap: literal 0x4eb8 of instr 0x80013284 (PAL 0x4fa0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012FDC_landmap: literal 0x2228 of instr 0x8001328c (PAL 0x2454) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012FDC_landmap: literal 0x4ec8 of instr 0x800132d0 (PAL 0x4fb0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013328_landmap: literal 0xf80 of instr 0x80013438 (PAL 0x11ac) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x4eb8 of instr 0x80013c40 (PAL 0x4fa0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x4ec8 of instr 0x80013c78 (PAL 0x4fb0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x4ec8 of instr 0x80013c90 (PAL 0x4fb0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0xf80 of instr 0x80013ce8 (PAL 0x11ac) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x1454 of instr 0x80013dd4 (PAL 0x1218) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x1454 of instr 0x80013e04 (PAL 0x1218) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x1454 of instr 0x80013e58 (PAL 0x1218) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x4ec8 of instr 0x80013eb0 (PAL 0x4fb0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C8C: literal 0x4ec8 of instr 0x80013c90 (PAL 0x4fb0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C8C: literal 0xf80 of instr 0x80013ce8 (PAL 0x11ac) not found in the C#; check whether the patch hand-edited it
- Game.cs:<consts>: 0x800ed8c8 -> 0x800ed68c (inferred); review

## Data map conflicts (0)

USA data addresses that mapped to more than one PAL address (majority vote used):

