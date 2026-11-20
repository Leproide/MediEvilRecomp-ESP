# MediEvilRecomp USA -> PAL port report

Author: https://github.com/Leproide  
License: GPL-3.0

MEDIEVIL.EXE  USA base 0x80021d14 gp 0x800ed848  |  PAL base 0x80021cac gp 0x800ed7c8

## Function matching

| image | functions | exact | aligned | fuzzy | missing |
|---|---|---|---|---|---|
| main | 338 | 338 | 0 | 0 | 0 |
| game | 1845 | 1744 | 100 | 1 | 0 |
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
| render\CullPatch.cs | func_80010800_tl | tl | exact | 0 | 7 |
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

- relocation gapsPolySquaresCaptured: 0x800eec88 -> 0x800eebfc (exact)

## Warnings (263)

- game: USA 0x80065dc0 ~ PAL 0x80065d58 weak match (similarity 0.42); rewritten in PAL
- patch Recompiled.PalCdCallbacks.SyncFromMemory @ game:0x80065dc0: function is fuzzy in PAL -> patch DISABLED (original PAL code runs instead)
- GameAddresses.cs:<consts>: 0x800eb6e0 -> 0x800eb660 (inferred); review
- GameAddresses.cs:<consts>: 0x800eb7b8 -> 0x800eb738 (inferred); review
- GameAddresses.cs:<consts>: 0x800eb6e4 -> 0x800eb664 (inferred); review
- GameAddresses.cs:<consts>: 0x800eb7bc -> 0x800eb73c (inferred); review
- GameAddresses.cs:<consts>: 0x800eb700 -> 0x800eb680 (inferred); review
- GameAddresses.cs:<consts>: 0x800eb7d8 -> 0x800eb758 (inferred); review
- GameAddresses.cs:<consts>: 0x800f1a28 -> 0x800f199c (inferred); review
- GameAddresses.cs:<consts>: 0x800f1aec -> 0x800f1a60 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef09c -> 0x800ef010 (inferred); review
- GameAddresses.cs:<consts>: 0x800f1a94 -> 0x800f1a08 (inferred); review
- GameAddresses.cs:<consts>: 0x800eece0 -> 0x800eec54 (inferred); review
- GameAddresses.cs:<consts>: 0x800eeda4 -> 0x800eed18 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef108 -> 0x800ef07c (inferred); review
- GameAddresses.cs:<consts>: 0x800ef1cc -> 0x800ef140 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef158 -> 0x800ef0cc (inferred); review
- GameAddresses.cs:<consts>: 0x800ef21c -> 0x800ef190 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef0c0 -> 0x800ef034 (inferred); review
- GameAddresses.cs:<consts>: 0x800ef11a -> 0x800ef08e (inferred); review
- GameAddresses.cs:<consts>: 0x800ef1de -> 0x800ef152 (inferred); review
- GameAddresses.cs:<consts>: 0x800ce09c -> 0x800cdf90 (inferred); review
- GameAddresses.cs:<consts>: 0x800ce0c4 -> 0x800cdfb8 (inferred); review
- GameAddresses.cs:<consts>: 0x800ce0c4 -> 0x800cdfb8 (inferred); review
- GameAddresses.cs:<consts>: 0x800ce0ec -> 0x800cdfe0 (inferred); review
- GameAddresses.cs:<consts>: 0x800db32c -> 0x800db2ac (inferred); review
- GameAddresses.cs:<consts>: 0x800db404 -> 0x800db384 (inferred); review
- GameAddresses.cs:<consts>: 0x800db330 -> 0x800db2b0 (inferred); review
- GameAddresses.cs:<consts>: 0x800db408 -> 0x800db388 (inferred); review
- GameAddresses.cs:<consts>: 0x800db3b4 -> 0x800db334 (inferred); review
- GameAddresses.cs:<consts>: 0x800db48c -> 0x800db40c (inferred); review
- GameAddresses.cs:<consts>: 0x800db40c -> 0x800db38c (inferred); review
- GameAddresses.cs:<consts>: 0x800eda8c -> 0x800eda00 (inferred); review
- GameAddresses.cs:<consts>: 0x800edb50 -> 0x800edac4 (inferred); review
- GameAddresses.cs:<consts>: 0x80011e98 -> 0x80011e98 (inferred); review
- GameAddresses.cs:<consts>: 0x80011e60 -> 0x80011e60 (inferred); review
- GameAddresses.cs:<consts>: 0x80011ea0 -> 0x80011ea0 (inferred); review
- GameAddresses.cs:<consts>: 0x800ce5e8 -> 0x800ce4dc (guess); review
- GameAddresses.cs:<consts>: 0x800ed8c8 -> 0x800ed848 (inferred); review
- GameAddresses.cs:<consts>: 0x800ed9a0 -> 0x800ed910 (guess); review
- GameAddresses.cs:<consts>: 0x800edaa0 -> 0x800eda14 (inferred); review
- GameAddresses.cs:<consts>: 0x800edb64 -> 0x800edad8 (inferred); review
- GameAddresses.cs:<consts>: 0x800ee0d0 -> 0x800ee044 (inferred); review
- GameAddresses.cs:<consts>: 0x800ee194 -> 0x800ee108 (inferred); review
- GameAddresses.cs:<consts>: 0x800ee0d8 -> 0x800ee04c (inferred); review
- GameAddresses.cs:<consts>: 0x800ee19c -> 0x800ee110 (inferred); review
- GameAddresses.cs:<consts>: 0x800eeac8 -> 0x800eea3c (inferred); review
- GameAddresses.cs:<consts>: 0x800eeb8c -> 0x800eeb00 (inferred); review
- GameAddresses.cs:<consts>: 0x800eeb60 -> 0x800eead4 (inferred); review
- GameAddresses.cs:<consts>: 0x800eebf8 -> 0x800eeb6c (inferred); review
- GameAddresses.cs:<consts>: 0x800eecbc -> 0x800eec30 (inferred); review
- GameAddresses.cs:<consts>: 0x800eec0c -> 0x800eeb80 (inferred); review
- GameAddresses.cs:<consts>: 0x800eecd0 -> 0x800eec44 (inferred); review
- GameAddresses.cs:<consts>: 0x800f3b80 -> 0x800f3af4 (inferred); review
- GameAddresses.cs:<consts>: 0x800f3c44 -> 0x800f3bb8 (inferred); review
- GameAddresses.cs:<consts>: 0x800d73a8 -> 0x800d729c (inferred); review
- GameAddresses.cs:<consts>: 0x800d73d0 -> 0x800d72c4 (inferred); review
- GameAddresses.cs:<consts>: 0x800d73c0 -> 0x800d72b4 (inferred); review
- GameAddresses.cs:<consts>: 0x800d5274 -> 0x800d5168 (inferred); review
- GameAddresses.cs:<consts>: 0x800d529c -> 0x800d5190 (inferred); review
- GameAddresses.cs:<consts>: 0x800d528c -> 0x800d5180 (inferred); review
- GameAddresses.cs:<consts>: 0x80012fd4 -> 0x80012fd4 (inferred); review
- GameAddresses.cs:<consts>: 0x80012f9c -> 0x80012f9c (inferred); review
- GameAddresses.cs:<consts>: 0x80012fdc -> 0x80012fdc (inferred); review
- GameAddresses.cs:<consts>: 0x80013320 -> 0x80013320 (inferred); review
- GameAddresses.cs:<consts>: 0x800132e8 -> 0x800132e8 (inferred); review
- GameAddresses.cs:<consts>: 0x80013328 -> 0x80013328 (inferred); review
- GameAddresses.cs:<consts>: 0x80013508 -> 0x80013508 (inferred); review
- GameAddresses.cs:<consts>: 0x800134d0 -> 0x800134d0 (inferred); review
- GameAddresses.cs:<consts>: 0x80013510 -> 0x80013510 (inferred); review
- GameAddresses.cs:<consts>: 0x80013550 -> 0x80013550 (inferred); review
- GameAddresses.cs:<consts>: 0x80013518 -> 0x80013518 (inferred); review
- GameAddresses.cs:<consts>: 0x80013558 -> 0x80013558 (inferred); review
- GameAddresses.cs:<consts>: 0x800135a0 -> 0x800135a0 (inferred); review
- GameAddresses.cs:<consts>: 0x80013568 -> 0x80013568 (inferred); review
- GameAddresses.cs:<consts>: 0x800135a8 -> 0x800135a8 (inferred); review
- GameAddresses.cs:<consts>: 0x80013770 -> 0x80013770 (inferred); review
- GameAddresses.cs:<consts>: 0x80013738 -> 0x80013738 (inferred); review
- GameAddresses.cs:<consts>: 0x80013778 -> 0x80013778 (inferred); review
- GameAddresses.cs:<consts>: 0x80013790 -> 0x80013790 (inferred); review
- GameAddresses.cs:<consts>: 0x80013758 -> 0x80013758 (inferred); review
- GameAddresses.cs:<consts>: 0x80013798 -> 0x80013798 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d80 -> 0x80013d80 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d48 -> 0x80013d48 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d88 -> 0x80013d88 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d88 -> 0x80013d88 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d50 -> 0x80013d50 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d90 -> 0x80013d90 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d90 -> 0x80013d90 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d58 -> 0x80013d58 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d98 -> 0x80013d98 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d98 -> 0x80013d98 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d60 -> 0x80013d60 (inferred); review
- GameAddresses.cs:<consts>: 0x80013da0 -> 0x80013da0 (inferred); review
- GameAddresses.cs:<consts>: 0x80013db4 -> 0x80013db4 (inferred); review
- GameAddresses.cs:<consts>: 0x80013d7c -> 0x80013d7c (inferred); review
- GameAddresses.cs:<consts>: 0x80013dbc -> 0x80013dbc (inferred); review
- GameAddresses.cs:<consts>: 0x80013fe8 -> 0x80013fe8 (inferred); review
- GameAddresses.cs:<consts>: 0x80013fb0 -> 0x80013fb0 (inferred); review
- GameAddresses.cs:<consts>: 0x80013ff0 -> 0x80013ff0 (inferred); review
- GameAddresses.cs:<consts>: 0x800104c4 -> 0x80010464 (guess); review
- GameAddresses.cs:<consts>: 0x800104c4 -> 0x80010464 (guess); review
- GameAddresses.cs:<consts>: 0x800104bc -> 0x8001045c (guess); review
- GameAddresses.cs:<consts>: 0x800105c0 -> 0x80010560 (guess); review
- GameAddresses.cs:<consts>: 0x800105c0 -> 0x80010560 (guess); review
- GameAddresses.cs:<consts>: 0x800105b8 -> 0x80010558 (guess); review
- GameAddresses.cs:<consts>: 0x80010854 -> 0x80010814 (guess); review
- GameAddresses.cs:<consts>: 0x80010854 -> 0x80010814 (guess); review
- GameAddresses.cs:<consts>: 0x8001084c -> 0x8001080c (guess); review
- GameAddresses.cs:<consts>: 0x800108a8 -> 0x80010868 (guess); review
- GameAddresses.cs:<consts>: 0x800108a8 -> 0x80010868 (guess); review
- GameAddresses.cs:<consts>: 0x800108a0 -> 0x80010860 (guess); review
- GameAddresses.cs:<consts>: 0x80010a80 -> 0x80010a40 (guess); review
- GameAddresses.cs:<consts>: 0x80010a80 -> 0x80010a40 (guess); review
- GameAddresses.cs:<consts>: 0x80010a78 -> 0x80010a38 (guess); review
- GameAddresses.cs:<consts>: 0x80010b1c -> 0x80010adc (guess); review
- GameAddresses.cs:<consts>: 0x80010b1c -> 0x80010adc (guess); review
- GameAddresses.cs:<consts>: 0x80010b14 -> 0x80010ad4 (guess); review
- GameAddresses.cs:<consts>: 0x80010f08 -> 0x80010ec8 (inferred); review
- GameAddresses.cs:<consts>: 0x80010f08 -> 0x80010ec8 (inferred); review
- GameAddresses.cs:<consts>: 0x80010f00 -> 0x80010ec0 (inferred); review
- HeapExpansion.cs:<consts>: 0x800f3b80 -> 0x800f3af4 (inferred); review
- PalCdCallbacks.cs:<consts>: 0x800eb6e0 -> 0x800eb660 (inferred); review
- PalCdCallbacks.cs:<consts>: 0x800eb6e4 -> 0x800eb664 (inferred); review
- PalLibCrypt.cs:<consts>: 0x80011ea0 -> 0x80011ea0 (inferred); review
- PalLibCrypt.cs:<consts>: 0x80011e98 -> 0x80011e98 (inferred); review
- PalLibCrypt.cs:<consts>: 0x800db32c -> 0x800db2ac (inferred); review
- PalMovies.cs:func_80079D90: literal 0x19b0 of instr 0x80079da4 (PAL 0x1924) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x760 of instr 0x80079ddc (PAL 0x754) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x764 of instr 0x80079de4 (PAL 0x758) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x768 of instr 0x80079df0 (PAL 0x75c) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x760 of instr 0x80079df8 (PAL 0x754) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19f0 of instr 0x80079e14 (PAL 0x1964) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x764 of instr 0x80079e1c (PAL 0x758) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19f0 of instr 0x80079e38 (PAL 0x1964) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19b0 of instr 0x80079e5c (PAL 0x1924) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19ec of instr 0x80079ecc (PAL 0x1960) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19b0 of instr 0x80079ed4 (PAL 0x1924) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x6c24 of instr 0x80079eec (PAL 0x6b98) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x6c24 of instr 0x80079f08 (PAL 0x6b98) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19e4 of instr 0x80079f10 (PAL 0x1958) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x19b0 of instr 0x80079ff4 (PAL 0x1924) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x248c of instr 0x8007a024 (PAL 0x2518) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x2488 of instr 0x8007a048 (PAL 0x2514) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x1a04 of instr 0x8007a050 (PAL 0x1978) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x76c of instr 0x8007a084 (PAL 0x760) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x1a1c of instr 0x8007a0c4 (PAL 0x1990) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x1a1c of instr 0x8007a108 (PAL 0x1990) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x1a1c of instr 0x8007a14c (PAL 0x1990) not found in the C#; check whether the patch hand-edited it
- PalMovies.cs:func_80079D90: literal 0x1a1c of instr 0x8007a160 (PAL 0x1990) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80021D5C: literal 0xf024 of instr 0x80021da4 (PAL 0xef98) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80021D5C: literal 0xdff8 of instr 0x80021dac (PAL 0xdf6c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80021D5C: literal 0x1a1c of instr 0x80021db0 (PAL 0x1990) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80021D5C: literal 0xdde0 of instr 0x80021db4 (PAL 0xdd54) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80021D5C: literal 0xdddc of instr 0x80021db8 (PAL 0xdd50) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80024B00: literal 0x450 of instr 0x80024b08 (PAL 0x444) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80024B00: literal 0xfb0 of instr 0x80025120 (PAL 0x103c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80024B00: literal 0x5d0 of instr 0x80025294 (PAL 0x5c4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80024B00: literal 0xfbc of instr 0x80025298 (PAL 0x1048) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800252D0: literal 0x450 of instr 0x800252d8 (PAL 0x444) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800252D0: literal 0xe8c of instr 0x80025690 (PAL 0xf18) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800252D0: literal 0x5d0 of instr 0x800258b0 (PAL 0x5c4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0x23d8 of instr 0x8001097c (PAL 0x2464) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0x23d8 of instr 0x80010988 (PAL 0x2464) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0xfb8 of instr 0x800109ec (PAL 0x1044) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0xf2c of instr 0x80010a7c (PAL 0xfb8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0x1454 of instr 0x80010c3c (PAL 0x13d4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0xf80 of instr 0x80010d90 (PAL 0x100c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_80010800_tl: literal 0xf80 of instr 0x80010e10 (PAL 0x100c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_8009A694: literal 0x23d8 of instr 0x8009a6c0 (PAL 0x2464) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x78dc of instr 0x800b37a8 (PAL 0x77d0) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x7b0 of instr 0x800b37fc (PAL 0x7a4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x7b0 of instr 0x800b39b8 (PAL 0x7a4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x7b0 of instr 0x800b3bf8 (PAL 0x7a4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x7b0 of instr 0x800b3db4 (PAL 0x7a4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x791c of instr 0x800b4064 (PAL 0x7810) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x7b0 of instr 0x800b4160 (PAL 0x7a4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x1fa0 of instr 0x800b425c (PAL 0x202c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x23d8 of instr 0x800b4410 (PAL 0x2464) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B3748: literal 0x795c of instr 0x800b49d0 (PAL 0x7850) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4028: literal 0x791c of instr 0x800b4064 (PAL 0x7810) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4028: literal 0x7b0 of instr 0x800b4160 (PAL 0x7a4) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4028: literal 0x1fa0 of instr 0x800b425c (PAL 0x202c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4028: literal 0x23d8 of instr 0x800b4410 (PAL 0x2464) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4028: literal 0x795c of instr 0x800b49d0 (PAL 0x7850) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x1fa0 of instr 0x800b425c (PAL 0x202c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x23d8 of instr 0x800b4410 (PAL 0x2464) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x795c of instr 0x800b49d0 (PAL 0x7850) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cb8 of instr 0x800b4e1c (PAL 0x2d38) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ca4 of instr 0x800b4e24 (PAL 0x2d24) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x6d24 of instr 0x800b4e34 (PAL 0x6c98) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cf0 of instr 0x800b4e44 (PAL 0x2d70) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cec of instr 0x800b4e54 (PAL 0x2d6c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ce8 of instr 0x800b4e64 (PAL 0x2d68) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ce4 of instr 0x800b4e74 (PAL 0x2d64) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ce0 of instr 0x800b4e84 (PAL 0x2d60) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cdc of instr 0x800b4e94 (PAL 0x2d5c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cbc of instr 0x800b4ea8 (PAL 0x2d3c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ccc of instr 0x800b4eb0 (PAL 0x2d4c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x6cdc of instr 0x800b4ecc (PAL 0x6c50) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x6c94 of instr 0x800b4ed4 (PAL 0x6c08) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cb8 of instr 0x800b4f44 (PAL 0x2d38) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x6d24 of instr 0x800b4fd8 (PAL 0x6c98) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2c98 of instr 0x800b4fec (PAL 0x2d18) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cb0 of instr 0x800b4ffc (PAL 0x2d30) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x3628 of instr 0x800b5040 (PAL 0x36a8) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cb0 of instr 0x800b5048 (PAL 0x2d30) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cac of instr 0x800b5050 (PAL 0x2d2c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2c9c of instr 0x800b505c (PAL 0x2d1c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cb0 of instr 0x800b5068 (PAL 0x2d30) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ca8 of instr 0x800b5278 (PAL 0x2d28) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ca8 of instr 0x800b5378 (PAL 0x2d28) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2ca8 of instr 0x800b539c (PAL 0x2d28) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x6d24 of instr 0x800b5440 (PAL 0x6c98) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cdc of instr 0x800b5484 (PAL 0x2d5c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cdc of instr 0x800b5550 (PAL 0x2d5c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800B4244: literal 0x2cdc of instr 0x800b5648 (PAL 0x2d5c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800A10D8: literal 0x1fa0 of instr 0x800a10e8 (PAL 0x202c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800A10D8: literal 0x23d8 of instr 0x800a13d8 (PAL 0x2464) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800A10D8: literal 0x3ab8 of instr 0x800a1aa0 (PAL 0x3a2c) not found in the C#; check whether the patch hand-edited it
- render\CullPatch.cs:func_800A10D8: literal 0x6ddc of instr 0x800a1d04 (PAL 0x6cd0) not found in the C#; check whether the patch hand-edited it
- render\DistantObjectsPatch.cs:<consts>: 0x800eeac8 -> 0x800eea3c (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800eeb60 -> 0x800eead4 (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800eebf8 -> 0x800eeb6c (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800eec0c -> 0x800eeb80 (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800d73c0 -> 0x800d72b4 (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800d73a8 -> 0x800d729c (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800d528c -> 0x800d5180 (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800d5274 -> 0x800d5168 (inferred); review
- render\DistantObjectsPatch.cs:<consts>: 0x800109f8 -> 0x800109b8 (guess); review
- render\DistantObjectsPatch.cs:<consts>: 0x800109f8 -> 0x800109b8 (guess); review
- render\ShaderDrawDistance.cs:<consts>: 0x800ef0c0 -> 0x800ef034 (inferred); review
- render\TerrainCapture.cs:<consts>: free-standing upper-half literal 0x80010000 left as is; review
- render\TerrainEdgeFade.cs:<consts>: 0x800ef108 -> 0x800ef07c (inferred); review
- render\TerrainEdgeFade.cs:<consts>: 0x800ef158 -> 0x800ef0cc (inferred); review
- render\TerrainEdgeFade.cs:<consts>: free-standing upper-half literal 0x80010000 left as is; review
- widescreen\FadeWidenPatch.cs:<consts>: 0x800d8368 -> 0x800d825c (inferred); review
- widescreen\LandMapPatch.cs:func_80012BBC: literal 0x4ec8 of instr 0x80012bc4 (PAL 0x4e68) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012BBC: literal 0x4530 of instr 0x80012be4 (PAL 0x44d0) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x1454 of instr 0x80012f88 (PAL 0x13d4) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x4ec8 of instr 0x80012fe4 (PAL 0x4e68) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x21b4 of instr 0x80013274 (PAL 0x2240) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x4eb8 of instr 0x80013284 (PAL 0x4e58) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x2228 of instr 0x8001328c (PAL 0x22b4) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0x4ec8 of instr 0x800132d0 (PAL 0x4e68) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012F70: literal 0xf80 of instr 0x80013438 (PAL 0x100c) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012FDC_landmap: literal 0x4ec8 of instr 0x80012fe4 (PAL 0x4e68) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012FDC_landmap: literal 0x21b4 of instr 0x80013274 (PAL 0x2240) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012FDC_landmap: literal 0x4eb8 of instr 0x80013284 (PAL 0x4e58) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012FDC_landmap: literal 0x2228 of instr 0x8001328c (PAL 0x22b4) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80012FDC_landmap: literal 0x4ec8 of instr 0x800132d0 (PAL 0x4e68) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013328_landmap: literal 0xf80 of instr 0x80013438 (PAL 0x100c) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x4eb8 of instr 0x80013c40 (PAL 0x4e58) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x4ec8 of instr 0x80013c78 (PAL 0x4e68) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x4ec8 of instr 0x80013c90 (PAL 0x4e68) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0xf80 of instr 0x80013ce8 (PAL 0x100c) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x1454 of instr 0x80013dd4 (PAL 0x13d4) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x1454 of instr 0x80013e04 (PAL 0x13d4) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x1454 of instr 0x80013e58 (PAL 0x13d4) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C3C: literal 0x4ec8 of instr 0x80013eb0 (PAL 0x4e68) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C8C: literal 0x4ec8 of instr 0x80013c90 (PAL 0x4e68) not found in the C#; check whether the patch hand-edited it
- widescreen\LandMapPatch.cs:func_80013C8C: literal 0xf80 of instr 0x80013ce8 (PAL 0x100c) not found in the C#; check whether the patch hand-edited it
- Game.cs:<consts>: 0x800ed8c8 -> 0x800ed848 (inferred); review

## Data map conflicts (0)

USA data addresses that mapped to more than one PAL address (majority vote used):

