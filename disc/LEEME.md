<!--
  MediEvilRecomp ESP - instrucciones para los archivos del disco
  Author: https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# Carpeta del disco

Para compilar y jugar necesitas **tu** disco original de **MediEvil, versión PAL española (SCES-01495)**. El proyecto no contiene ni distribuye los archivos del juego, que están protegidos por derechos de autor: tienes que sacarlos tú de tu copia original.

Las instrucciones para la versión italiana (SCES-01494) están en [`README.md`](README.md).

## Formato necesario

El volcado debe estar en formato **bin/cue** (MODE2/2352) **limpio**, es decir, idéntico al disco original y sin parches. Es el mismo formato que los volcados de Redump.

Copia aquí estos tres archivos, exactamente con estos nombres:

- `MediEvil (Spain) (Track 1).bin`
- `MediEvil (Spain) (Track 2).bin`
- `MediEvil (Spain).cue`

El archivo `.cue` llama a los `.bin` por su nombre. Si cambias el nombre de los `.bin`, actualiza también las líneas `FILE` dentro del `.cue`.

## Comprobaciones

- El tamaño de cada `.bin` tiene que ser un múltiplo exacto de 2352 bytes.
- SHA-256 de los archivos extraídos de un disco correcto:
  - `SYSTEM.CNF`: `43e67cd4c9fcfc34dbe8cc34f81e7d5f074b6635f410e37dd803927c02ab3a36`
  - `SCES_014.95`: `e6bf1bf90ee2f4aac170f426bb573e3dabea861c39b275da36539cb8dc2818d6`
  - `MEDIEVIL.EXE`: `70e73fb31cb7a4410d9ef139b3cc2d6186e743b019834113c4eff992a92c9703`
- Otras versiones no funcionan con la configuración española: la USA (SCUS-94227) usa el proyecto original, la italiana usa `config/MediEvil.json` y las demás versiones PAL tienen código diferente.
- No uses imágenes con parches anti-LibCrypt: el ejecutable de arranque está modificado y la recompilación se bloquea al iniciar.

## License

GPL-3.0-or-later.

## Author

https://github.com/Leproide
