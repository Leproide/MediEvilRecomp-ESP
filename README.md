<!--
  MediEvilRecomp ESP - README
  Author: https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# MediEvilRecomp ESP

Recompilación estática de **MediEvil para PlayStation, versión PAL española (SCES-01495)**, para jugarlo de forma nativa en ordenadores modernos, con panorámico y renderizado mejorado.

<img width="1920" height="1032" alt="immagine" src="https://github.com/user-attachments/assets/7906b56a-7c8b-47fd-9370-c24c3d62c195" />

Este repositorio parte de [MediEvilRecomp-ITA](https://github.com/Leproide/MediEvilRecomp-ITA), el port a la versión PAL italiana (SCES-01494), que a su vez es un **fork** de [MediEvilRecomp](https://github.com/BlackLabelHQ/MediEvilRecomp) de **BlackLabelHQ** (versión USA). Todo el trabajo base (recompilación, parches de renderizado y panorámico, runtime RecompOne) es suyo.

Todas las correcciones PAL ya hechas para la versión italiana (callbacks del CD, reproducción de vídeos, fundido en iris, pantalla PAL de 256 líneas, voces XA) se **portan** a la versión española recalculando las direcciones. La lógica de las correcciones no cambia, y una sola copia del código sirve para las dos versiones.

> **Proyecto en beta.** Puede haber errores, incluso graves. Repórtalos en las issues de este repositorio.

> La versión italiana del proyecto está en [MediEvilRecomp-ITA](https://github.com/Leproide/MediEvilRecomp-ITA).

---

## Qué contiene

| | Italiana (por defecto) | Española |
|---|---|---|
| Juego | PAL Italia, SCES-01494 | PAL España, SCES-01495 |
| Configuración del recompilador | `config/MediEvil.json` | `config/es/MediEvil.json` |
| Funcmaps | `config/funcmaps/` | `config/es/funcmaps/` |
| Código generado | `generated/` | `generated-es/` |
| Compilación | `dotnet build` | `dotnet build -p:Lingua=es` |
| Salida | `bin/`, `obj/` | `bin-es/`, `obj-es/` |
| Scripts de Windows | `windows_*.bat` | `windows_*_es.bat` |
| Scripts de Linux | — | `linux_*_es.sh` |
| Disco | `disc/MediEvil (Italy).cue` | `disc/MediEvil (Spain).cue` |

La configuración, el código generado y la compilación por defecto de la versión italiana no cambian: los valores italianos de la tabla de direcciones son exactamente los que antes estaban escritos en el código.

---

## Requisitos

- GPU compatible con OpenGL 2.1 como mínimo
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [OpenAL](https://www.openal.org/downloads/)
- [Git](https://git-scm.com/install/)
- Una copia **original** de MediEvil PAL España, de la que sacar un volcado **limpio** en formato bin/cue. Lee [`disc/LEEME.md`](disc/LEEME.md).
- Opcional, solo para las herramientas de port: Python 3.8 o superior, sin paquetes externos.

Este repositorio **no contiene archivos del juego** ni distribuye su código: todo se genera en tu ordenador a partir de tu disco.

---

## Compilación (versión española)

### Windows: script automático

1. Clona el repositorio con los submódulos:
   ```
   git clone --recursive https://github.com/Leproide/MediEvilRecomp-ESP
   cd MediEvilRecomp-ESP
   ```
2. Copia en `disc\` los archivos de tu disco con estos nombres:
   - `MediEvil (Spain) (Track 1).bin`
   - `MediEvil (Spain) (Track 2).bin`
   - `MediEvil (Spain).cue`
3. Ejecuta **`windows_initial_build_es.bat`**. El script:
   - actualiza los submódulos;
   - aplica a RecompOne la corrección de `recompone-fixes\`;
   - compila RecompOne;
   - genera el código del juego en `generated-es\` a partir de `config\es\MediEvil.json`;
   - compila el juego con `-p:Lingua=es`.

### Windows: comandos a mano

Desde la carpeta del repositorio (cmd o PowerShell):

```
git submodule update --init --recursive
git -C RecompOne apply ../recompone-fixes/cd-whole-sector-mode.patch
dotnet build RecompOne/RecompOne.sln
dotnet run --project RecompOne/RecompOne.Recompiler config/es/MediEvil.json
dotnet build RecompOne.MediEvil.csproj -p:Lingua=es
dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=es
```

`git apply` solo hace falta la primera vez. Si la corrección ya está aplicada, da un error que puedes ignorar. Los mismos comandos sirven en Linux y macOS.

### Linux

Requisitos: [.NET 10 SDK](https://learn.microsoft.com/dotnet/core/install/linux), `git`, controladores OpenGL y, para el selector de archivos del disco, GTK 3 (en Debian/Ubuntu: `sudo apt install git libgtk-3-0`). OpenAL y SDL vienen incluidos con el juego.

1. Clona el repositorio y copia en `disc/` los archivos del disco, con los mismos nombres de arriba (en Linux se distinguen mayúsculas y minúsculas).
2. Compila y juega:
   ```
   chmod +x linux_*.sh
   ./linux_initial_build_es.sh
   ./linux_run_es.sh
   ```

`./linux_run_es.sh` lanza solo la primera compilación si hace falta. `./linux_run_es.sh --no-build` arranca sin recompilar.

### Paquetes para la release (Windows y Linux)

Una vez generado el código (`generated-es/`), `windows_publish_es.bat v1.1` crea en `publish-es/` los dos paquetes que espera la actualización automática:

- `MediEvilRecomp-ESP-v1.1-windows-x64.zip`
- `MediEvilRecomp-ESP-v1.1-linux-x64.tar.gz`, con `run.sh`, que instala el runtime de .NET 10 si falta.

En Linux, el paquete se usa así:

```
mkdir -p MediEvilRecomp-ESP
tar -xzf MediEvilRecomp-ESP-v1.1-linux-x64.tar.gz -C MediEvilRecomp-ESP
cd MediEvilRecomp-ESP && bash run.sh
```

El paquete no contiene archivos del juego: la primera vez el juego pide el `.cue` de tu disco.

Para la versión italiana, en el mismo repositorio: `dotnet run --project RecompOne/RecompOne.Recompiler config/MediEvil.json` y luego `dotnet run`, sin `-p:Lingua`. También puedes usar los scripts `windows_*.bat` sin `_es`.

---

## Ejecución

- **Windows:** `windows_run_es.bat`. Si el juego todavía no está compilado, la primera compilación arranca sola.
- **Linux:** `./linux_run_es.sh` (o `bash run.sh` en el paquete de la release).
- **Inicio rápido sin recompilar:** `windows_run_no_build_es.bat`.
- **Desde terminal:** `dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=es`.

Cuando el juego pida el disco, elige `disc\MediEvil (Spain).cue`.

Las dos versiones comparten `settings.json` si se ejecutan desde la misma carpeta. La versión española comprueba que el disco elegido arranque `SCES_014.95`. Si no es así, por ejemplo porque el ajuste apunta todavía al disco italiano, vuelve a pedir el disco.

Después de cambiar `config/es/` hay que regenerar el código: borra `generated-es` y vuelve a ejecutar `windows_initial_build_es.bat`.

---

## Cómo se ha hecho el port ITA → ESP

Las dos versiones se han comparado instrucción por instrucción con las herramientas de `tools/pal-port/`, usando el proyecto italiano como origen. Resultado (detalle en [`tools/pal-port/PORT_REPORT_ES.md`](tools/pal-port/PORT_REPORT_ES.md)):

| Imagen | Funciones | Código | Notas |
|---|---|---|---|
| `SCES_014.95` (arranque) | 338 | idéntico | el archivo es igual byte a byte que `SCES_014.94` |
| `MEDIEVIL.EXE` | 1845 | mismas direcciones | 98 funciones *aligned*: cambian direcciones de datos y una constante de pista XA; `$gp` 0x800ED848 → 0x800ED8C8 |
| 23 overlays | 2071 | mismas direcciones | cambian solo los datos que siguen al código |
| `TL.BIN` | 98 | +0x8 | textos españoles más largos al principio del archivo |
| `LANDMAP.BIN` | 33 | −0x8 | |
| `CREDITS.BIN` | 14 | −0x38 | |

Las 4399 funciones son *exact* o *aligned*: no hay ninguna función *fuzzy* ni ninguna sin pareja. En las funciones *aligned* casi todos los inmediatos que cambian son direcciones de datos. Las únicas constantes distintas son dos números de pista XA (`MEDIEVIL.EXE` 0x80065930 y `TL.BIN` 0x8001174C), y ninguna está en las funciones parcheadas o enganchadas. Los datos de `MEDIEVIL.EXE` se desplazan por zonas: −0x18 (hasta ~0x800D8xxx), +0x80 (hasta ~0x800ED9FC) y +0x78 (el resto). Por eso los offsets `$gp` de las variables cercanas a `$gp` bajan 8 bytes.

### Tabla de direcciones por versión

Todas las direcciones que cambian entre versiones están en **`config/versions.json`**:

| Sección | Contenido |
|---|---|
| `hooks` | los 67 hooks de `config/*/MediEvil.json` (dirección ITA / ESP) |
| `entryPoints` | puntos de entrada declarados en `overlays[].functions` |
| `relocations` | arrays reubicados (`gapsPolySquaresCaptured`) |
| `constants` | direcciones y offsets `$gp` con nombre, usados por los parches escritos a mano (`PalCdCallbacks`, `PalLibCrypt`, `PalMovies`, `TerrainPatch`, `TerrainCapture`, mod Trucchi) |
| `versionHooks` | hooks que existen solo en una versión (los de `PalLibCrypt` en la española; la configuración italiana no cambia) |
| `literals` | inmediatos de las copias de funciones recompiladas (`CullPatch`, `EntityFloatFunctions`, `LandMapPatch`), identificados por la instrucción italiana que los produce |

A partir de la tabla se generan:

- **`patches/GameAddresses.cs`**: clases `GameVersion`, `Addr` y `RAddr` con constantes `GameVersion.Spanish ? ESP : ITA`. La versión se elige al compilar: `-p:Lingua=es` define el símbolo `LINGUA_ES`. Los parches usan `Addr.CdCbSync`, `RAddr.Game_80021DA4`, etc. en lugar de números.
- **`config/es/MediEvil.json`**: la configuración española, derivada de la italiana. Los nombres de los métodos de los parches siguen siendo los italianos (por ejemplo `LandMapPatch.func_80012BBC` se engancha en ESP en 0x80012BB4).

La tabla también recoge el título de la ventana y el repositorio de las actualizaciones automáticas (`Leproide/MediEvilRecomp-ESP` para la versión española). Las mods **Trucchi** y **Filmati a schermo intero** reconocen la versión en tiempo de ejecución por la posición de las tablas de trucos, que son datos constantes del ejecutable.

### Herramientas

`tools/pal-port/version_port.py` (Python 3, sin dependencias):

```
py tools\pal-port\version_port.py check --ita CARPETA_ITA --dst CARPETA_ESP
py tools\pal-port\version_port.py gen-cs
py tools\pal-port\version_port.py gen-config
py tools\pal-port\version_port.py convert --ita CARPETA_ITA --dst CARPETA_ESP --force
```

- `check` comprueba la tabla, las dos configuraciones y `GameAddresses.cs`. Si se le pasan las carpetas con los archivos extraídos de los dos discos (`SYSTEM.CNF`, ejecutable de arranque, `MEDIEVIL.EXE`, `OVERLAYS/`), además verifica:
  - que cada hook y cada destino de `jal` sea el inicio de una función en las funcmaps de las dos versiones;
  - que cada valor de `literals` coincida con el inmediato de la instrucción en los dos binarios;
  - que cada constante con nombre corresponda en el mapa de direcciones ITA → ESP;
  - que los destinos de llamada de los parches (`PalDispatch`, `Dispatcher.Call`) se comporten igual en las dos versiones.
- `gen-cs` / `gen-config` regeneran `GameAddresses.cs` y `config/es/MediEvil.json` después de editar la tabla.
- `fill` calcula a partir de los dos discos el valor español de las constantes nuevas que solo tienen el valor italiano.
- `convert` es el paso único que creó la tabla. Necesita los fuentes C# originales con los números escritos a mano, así que solo sirve para rehacer el port desde el repositorio italiano.

Resultado de `check` sobre los dos discos:

```
ita: 67 hooks (61 en inicios de funcmap, 6 en puntos de entrada declarados); jal: 6109 en su imagen + 6866 hacia game/main, 0 fuera de inicio de función
esp: 72 hooks (66 en inicios de funcmap, 6 en puntos de entrada declarados); jal: 6109 en su imagen + 6866 hacia game/main, 0 fuera de inicio de función
114 literals y 33 constantes verificados
RESULT: OK
```

Los 6 hooks en "puntos de entrada declarados" son los mismos que en la versión italiana: puntos de entrada internos (por ejemplo `func_80021E5C`) declarados en `overlays[].functions` de la configuración. Hay una sola llamada de los parches que no apunta a un punto de entrada (`Dispatcher.Call` a 0x80013D88 ITA / 0x80013D80 ESP en `LandMapPatch`). También pasa en la versión italiana: RecompOne la resuelve en tiempo de ejecución (`Dispatcher.Tolerant`), así que el comportamiento es el mismo.

Los archivos extraídos de los discos están protegidos por derechos de autor: **no los subas nunca al repositorio**.

---

## LibCrypt: El Mausoleo de la Cima de la Colina

El disco PAL español está protegido con **LibCrypt**, igual que el italiano. Desde el mapa, el juego (código en 0x80065D20..0x80065FC0, iniciado en LANDMAP 0x80011E98) lee con GetlocP la posición de 16 sectores de la tabla en 0x800DB334. Ocho de ellos tienen en el disco original el subcódigo Q modificado, y de ellos sale una clave de 16 bits (0x800DB32C). Con esa clave `func_80065FC4` descifra el primer sector del Mausoleo en `MEDRES.MWD` (recurso 0xD6). Un volcado bin/cue no tiene subcódigo: la clave sale mal y el juego se queda en la pantalla del CD con el signo de exclamación.

`patches/PalLibCrypt.cs` (compartido con la versión italiana, direcciones en `config/versions.json`) hace que GetlocP devuelva lo que daría el lector real. Usa el archivo `.sbi`/`.lsd` junto al `.cue` si existe; si no, la lista incluida de SCES-01495. Comprobado sin ejecutar el juego con `tools/pal-port/libcrypt_check.py`:

| | Italiana (SCES-01494) | Española (SCES-01495) |
|---|---|---|
| Sectores protegidos (LBA) | 13955 … 44009 | 14499, 14749, 14906, 15162, 15478, 15881, 15951, 16017, 42521, 42663, 42862, 43204, 43484, 43904, 44009, 44162 |
| Clave | 0xAAA6 | **0x0E57** (8 bits a 1 en los primeros 16 controles) |
| Recurso 0xD6 de `MEDRES.MWD` | — | sector 0x1401, 152020 bytes comprimidos, 315748 descomprimidos |
| Resultado del descifrado | `PP20` | `PP20`; el tamaño al final del flujo (315748) coincide con el índice y se descomprime completo |

La clave italiana no sirve para el disco español: los 16 sectores protegidos y los datos cifrados son distintos, aunque el código y la tabla de sectores sean iguales.

```
py tools\pal-port\libcrypt_check.py --ver esp --exe MEDIEVIL.EXE --mwd MEDRES.MWD
py tools\pal-port\libcrypt_check.py --ver esp --exe MEDIEVIL.EXE --sbi "disc\MediEvil (Spain).sbi" --mwd MEDRES.MWD
```

---

## Debug

`patches/PalDebug.cs` activa los logs internos de RecompOne cuando está definida la variable `MEDIEVIL_DEBUG`. Los canales disponibles son `cd`, `irq`, `bios`, `sdk`, `vsync`, `gpu`, `dma`, `spu`, `mdec` o `all`. Si la variable no está definida, no cambia nada.

Con cualquier valor de `MEDIEVIL_DEBUG` (por ejemplo `libcrypt`), `PalLibCrypt` escribe además el detalle del control LibCrypt (líneas `[PalLibCrypt]`).

Símbolo del sistema (cmd):
```
set MEDIEVIL_DEBUG=cd,irq
dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=es > debug.txt 2>&1
```

PowerShell:
```
$env:MEDIEVIL_DEBUG = "cd,irq"
dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=es > debug.txt 2>&1
```

Adjunta `debug.txt` a las issues cuando informes de un bloqueo o un cierre inesperado.

---

## Trucos

El juego tiene dos menús de trucos integrados, y en el recomp funcionan igual que en la consola. En pausa, mantén pulsado **L2** y pulsa:

- **Modo trucos**: Abajo, Arriba, Cuadrado, Triángulo, Triángulo, Círculo, Abajo, Arriba, Cuadrado, Triángulo
- **Modo trucos ampliado**: Triángulo, Círculo, Triángulo, Círculo, Círculo, Triángulo, Izquierda, Círculo, Arriba, Abajo, Derecha, Círculo, Izquierda, Izquierda, Triángulo, Derecha, Círculo, Izquierda, Izquierda, Triángulo, Círculo, Abajo, Círculo, Círculo, Derecha

La versión española usa las mismas secuencias que la italiana: las dos tablas de `MEDIEVIL.EXE` tienen el mismo contenido y solo cambia su posición. La mod incluida **Trucchi** los activa desde el panel de mods, sin combinaciones, y funciona en las dos versiones. Detalles en el [README de la mod](mods/Trucchi/README.md) (en italiano).

<img width="1920" height="1057" alt="immagine" src="https://github.com/user-attachments/assets/e63afa4e-62b0-4c83-8b22-3eb064944a8c" />

<img width="1920" height="1080" alt="immagine" src="https://github.com/user-attachments/assets/02eb4635-7d14-45dd-bd4c-f63e5330c341" />

---

## Vídeos a pantalla completa

La mod opcional **Filmati a schermo intero** amplía los vídeos: muestra solo el área del vídeo y la adapta a la ventana sin deformarla. Se activa desde el panel de mods y funciona en las dos versiones. Detalles en el [README de la mod](mods/FilmatiSchermoIntero/README.md) (en italiano).

---

## Estructura

```
config/MediEvil.json       configuración del recompilador, versión italiana
config/funcmaps/           funcmaps italianas
config/es/                 configuración y funcmaps españolas
config/versions.json       tabla de direcciones por versión (ITA / ESP)
patches/                   parches C# de renderizado, panorámico y soporte PAL
patches/GameAddresses.cs   tabla de direcciones en C# (generada desde versions.json)
patches/GameDisc.cs        comprobación del disco en la versión española
recompone-fixes/           correcciones para el submódulo RecompOne
mods/Trucchi/              mod para activar los menús de trucos
mods/FilmatiSchermoIntero/ mod opcional para los vídeos a pantalla completa
disc/                      aquí van los archivos de tu disco (no incluidos)
tools/pal-port/            herramientas de port, informes y tabla por versión
BACKUP_Dipendenze/         copia de reserva de RecompOne y ShaderMod
misc/                      actualización automática
assets/                    icono e idiomas de la interfaz
Directory.Build.props      carpetas de salida bin-es/obj-es con -p:Lingua=es
```

---

## Créditos

- **BlackLabelHQ** (Derp Princess, flaffymg): [MediEvilRecomp](https://github.com/BlackLabelHQ/MediEvilRecomp) y [RecompOne](https://github.com/BlackLabelHQ/RecompOne), es decir, todo el trabajo de recompilación en el que se basa este proyecto.
- El equipo de la decompilación de MediEvil, cuyas referencias usó el proyecto original.
- Port PAL italiano y español: https://github.com/Leproide

---

## License

Los cambios introducidos por este fork (port PAL italiano y español, `config/es/`, `config/versions.json`, herramientas de `tools/pal-port/`, scripts, `PalDispatch.cs`, `PalDebug.cs`, `GameAddresses.cs`, `GameDisc.cs`, `recompone-fixes/`, mods `mods/Trucchi/` y `mods/FilmatiSchermoIntero/`, parches PAL de `patches/`) se publican bajo **GPL-3.0-or-later**. El texto completo está en [`tools/pal-port/LICENSE`](tools/pal-port/LICENSE).

El código original de MediEvilRecomp y RecompOne pertenece a BlackLabelHQ y sigue sujeto a sus términos. MediEvil es una marca de sus respectivos propietarios: este proyecto no está afiliado ni aprobado por Sony Interactive Entertainment.

---

## Author

https://github.com/Leproide
