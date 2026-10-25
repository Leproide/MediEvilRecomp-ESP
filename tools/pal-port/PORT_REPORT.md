# MediEvilRecomp USA -> PAL port report

Author: https://github.com/Leproide  
License: GPL-3.0

MEDIEVIL.EXE  USA base 0x80021ca4 gp 0x800ed5d4  |  PAL base 0x80021d14 gp 0x800ed848

## Function matching

| image | functions | exact | aligned | fuzzy | missing |
|---|---|---|---|---|---|
| main | 338 | 288 | 36 | 14 | 0 |
| game | 1843 | 1657 | 152 | 34 | 0 |
| ac | 142 | 118 | 17 | 7 | 0 |
| ag | 98 | 78 | 13 | 7 | 0 |
| cc | 104 | 79 | 12 | 12 | 1 |
| ch | 66 | 53 | 5 | 8 | 0 |
| cr | 11 | 9 | 2 | 0 | 0 |
| credits | 15 | 11 | 3 | 0 | 1 |
| dc | 139 | 116 | 20 | 3 | 0 |
| ee | 170 | 145 | 21 | 4 | 0 |
| eh | 24 | 17 | 5 | 2 | 0 |
| gg | 27 | 20 | 1 | 6 | 0 |
| gs | 83 | 71 | 6 | 6 | 0 |
| gy1 | 45 | 33 | 4 | 8 | 0 |
| gy2 | 91 | 71 | 11 | 9 | 0 |
| hh | 97 | 93 | 4 | 0 | 0 |
| hr | 121 | 100 | 13 | 8 | 0 |
| ia | 51 | 39 | 3 | 9 | 0 |
| la | 69 | 58 | 3 | 8 | 0 |
| landmap | 34 | 22 | 7 | 4 | 1 |
| pd | 56 | 47 | 3 | 6 | 0 |
| pg | 70 | 54 | 11 | 5 | 0 |
| ps | 117 | 88 | 22 | 7 | 0 |
| sf | 135 | 107 | 16 | 12 | 0 |
| sv | 107 | 89 | 5 | 13 | 0 |
| td | 117 | 102 | 12 | 3 | 0 |
| tl | 98 | 86 | 10 | 2 | 0 |
| zl | 131 | 112 | 12 | 7 | 0 |

*exact*: same code, only addresses differ. *aligned*: same code, some constants differ (50 Hz timing, offsets). *fuzzy*: code changed in PAL.

## C# patch methods

| file | method | overlay | match | literals rewritten | unresolved |
|---|---|---|---|---|---|
| render/CullPatch.cs | func_80021CEC | game | exact | 5 | 0 |
| render/CullPatch.cs | func_80021DEC | game | exact | 0 | 0 |
| render/CullPatch.cs | func_80022000 | game | exact | 0 | 0 |
| render/CullPatch.cs | func_800223F0 | game | exact | 0 | 0 |
| render/CullPatch.cs | func_80024A90 | game | exact | 4 | 0 |
| render/CullPatch.cs | func_80025260 | game | exact | 3 | 0 |
| render/CullPatch.cs | func_80010808_tl | tl | exact | 7 | 0 |
| render/CullPatch.cs | func_8009A4C8 | game | exact | 0 | 0 |
| render/CullPatch.cs | func_8009A61C | game | exact | 1 | 0 |
| render/CullPatch.cs | func_800B36CC | game | exact | 10 | 0 |
| render/CullPatch.cs | func_800B3FAC | game | exact | 5 | 0 |
| render/CullPatch.cs | func_800B41C8 | game | exact | 43 | 0 |
| render/CullPatch.cs | func_800A105C | game | exact | 4 | 0 |
| render/EntityFloatFunctions.cs | func_800B0068 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B0240 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B0458 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B065C | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B0900 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B0AC0 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B0CB8 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B0EA4 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B59C8 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B5DA4 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B6238 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B6724 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B6E70 | game | aligned | 2 | 0 |
| render/EntityFloatFunctions.cs | func_800B7224 | game | aligned | 2 | 0 |
| render/EntityFloatFunctions.cs | func_800B76BC | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B7A04 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B7DBC | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B8288 | game | exact | 0 | 0 |
| render/EntityFloatFunctions.cs | func_800B89A8 | game | exact | 2 | 0 |
| render/EntityFloatFunctions.cs | func_800B906C | game | exact | 2 | 0 |
| widescreen/LandMapPatch.cs | func_80012B38 | landmap | aligned | 3 | 0 |
| widescreen/LandMapPatch.cs | func_80012EEC | landmap | aligned | 11 | 0 |
| widescreen/LandMapPatch.cs | func_80012F58_landmap | landmap | aligned | 6 | 0 |
| widescreen/LandMapPatch.cs | func_800132A4_landmap | landmap | aligned | 3 | 0 |
| widescreen/LandMapPatch.cs | func_80013BB8 | landmap | aligned | 9 | 0 |
| widescreen/LandMapPatch.cs | func_80013C08 | landmap | aligned | 3 | 0 |

- relocation gapsPolySquaresCaptured: 0x800eea24 -> 0x800eec88 (exact)

## Warnings (14)

- game: PAL-only code at 0x80065f8c (+56) added to funcmap
- game: PAL-only code at 0x80065fc4 (+56) added to funcmap
- game: USA 0x8002afd8 ~ PAL 0x8002b044 weak match (similarity 0.22); rewritten in PAL
- game: USA 0x8006e2b4 ~ PAL 0x8006e36c weak match (similarity 0.36); rewritten in PAL
- game: USA 0x8006e340 ~ PAL 0x8006e3d0 weak match (similarity 0.23); rewritten in PAL
- cc: PAL-only code at 0x8001052c (+76) added to funcmap
- cc: USA function 0x800103e4 (+108) has no PAL counterpart; dropped
- credits: USA function 0x8001289c (+8) has no PAL counterpart; dropped
- landmap: USA 0x800115ec ~ PAL 0x80011618 weak match (similarity 0.18); rewritten in PAL
- landmap: USA function 0x80014468 (+328) has no PAL counterpart; dropped
- sf: USA 0x800180f4 ~ PAL 0x800180dc weak match (similarity 0.02); rewritten in PAL
- zl: USA 0x80019ce0 ~ PAL 0x80019ce8 weak match (similarity 0.42); rewritten in PAL
- render/CullPatch.cs:<consts>: 0x800eedd0 -> 0x800ef034 (inferred); review
- render/TerrainCapture.cs:<consts>: free-standing upper-half literal 0x80010000 left as is; review

## Data map conflicts (16)

USA data addresses that mapped to more than one PAL address (majority vote used):

- 0x00018000: 0x00018000 x3, 0x0001cccc x3
- 0x00088000: 0x000a3333 x3, 0x000a3000 x3
- 0x00222222: 0x00222222 x3, 0x0028f5c2 x1
- 0x1b4e81b5: 0x10624dd3 x94, 0x1b4e81b5 x30
- 0x66666667: 0x66666667 x106, 0x2aaaaaab x1
- 0x8006d5c4: 0x8006d694 x11, 0x800a6a5c x1
- 0x800a69e0: 0x800a6a5c x31, 0x800a5218 x1
- 0x800ed5f0: 0x800ed860 x3, 0x800ed864 x3
- 0x801beff4: 0x801bec5c x2, 0x801bec14 x1
- 0x801bfd68: 0x801bf9b0 x2, 0x801bf80c x1
- 0x88888889: 0x88888889 x39, 0x51eb851f x37, 0x2aaaaaab x1, 0xaaaaaaab x1
- 0x91a2b3c5: 0x91a2b3c5 x3, 0x057619f1 x2
- 0x92492493: 0x92492493 x9, 0x2aaaaaab x3
- 0xb60b60b7: 0x1b4e81b5 x7, 0xdd67c8a7 x1
- 0xdd67c8a7: 0xdd67c8a7 x33, 0x84210843 x11
- 0xfff78000: 0xfff5cccd x3, 0xfff5d000 x3
