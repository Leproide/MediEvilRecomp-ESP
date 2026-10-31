<!--
  MediEvilRecomp ESP - README
  Author: https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# MediEvilRecomp ESP

Recompilación estática de **MediEvil para PlayStation, versión PAL española (SCES-01495)**, para jugarlo de forma nativa en ordenadores modernos, con panorámico y renderizado mejorado.

<img width="1920" height="1032" alt="MediEvil_PAL_ESP" src="https://github.com/user-attachments/assets/7906b56a-7c8b-47fd-9370-c24c3d62c195" />

Este repositorio parte de [MediEvilRecomp-ITA](https://github.com/Leproide/MediEvilRecomp-ITA), el port a la versión PAL italiana (SCES-01494), que a su vez es un **fork** de [MediEvilRecomp](https://github.com/BlackLabelHQ/MediEvilRecomp) de **BlackLabelHQ**, que recompila la versión USA. Todo el trabajo base (recompilación, parches de renderizado y panorámico, runtime RecompOne) es suyo.

Todas las correcciones PAL de la versión italiana se **portan** a la española recalculando las direcciones: la lógica no cambia y una sola copia del código sirve para las dos versiones.

> **Versión basada en MediEvilRecomp 1.0**, igual que MediEvilRecomp-ITA 1.1.3: cámara con la palanca derecha, interpolación mejorada, shaders slang, color de 24 bits, nuevo sistema de ajustes y correcciones de cierres inesperados. Incluye todas las correcciones PAL. La versión española no tiene Mod Hub (ver "Mod Hub"). Los ajustes de las versiones anteriores no se conservan.

> **Proyecto en beta.** Puede haber errores, incluso graves. Repórtalos en las [issues de este repositorio](https://github.com/Leproide/MediEvilRecomp-ESP/issues).

---

## Contribuciones

Este fork acepta también contribuciones escritas o asistidas por inteligencia artificial. Las issues y pull requests se valoran todas igual: lo que cuenta es que la contribución sea correcta, esté probada y bien explicada.

Los problemas de esta versión (PAL española) se comunican **aquí**, no en el repositorio original, que trabaja sobre la versión USA.

---

## Qué cambia respecto al original

| | Original | Este fork |
|---|---|---|
| Versión del juego | USA (SCUS-94227) | PAL española (SCES-01495) e italiana (SCES-01494) |
| Configuración y funcmaps | direcciones USA | ESP en `config/es/`, ITA en `config/` (arranque, EXE y 26 overlays) |
| Parches C# (`patches/`, `wrapers/`) | direcciones USA | direcciones, offsets `$gp`, constantes a 50 Hz y 256 líneas para PAL; las que cambian entre ITA y ESP están en `config/versions.json` |
| `patches/PalCdCallbacks.cs` | — | callbacks del CD alineados con RecompOne: mapa fluido tras la Sala de los Héroes y nivel que carga |
| `patches/PalMovies.cs` | — | los vídeos ya no se cortan tras un segundo (issue #5 del original) |
| `patches/widescreen/IrisFadePatch.cs` | — | fundido en iris hasta las esquinas en panorámico (issue #7 del original) |
| `patches/PalDisplay.cs` | — | 256 líneas PAL: vídeos centrados en vez de desplazados hacia abajo |
| `patches/PalLibCrypt.cs` | — | protección LibCrypt con un volcado bin/cue: se abre El Mausoleo de la Cima de la Colina |
| `patches/PalXaSpeech.cs` | — | los diálogos ya no se cierran solos cuando empieza una voz mientras suena la música |
| `patches/GameAddresses.cs`, `GameDisc.cs` | — | direcciones por versión y comprobación del disco español |
| `recompone-fixes/` | — | corrección del lector de CD de RecompOne (modo sector entero) |
| Scripts de compilación | solo Windows, nombres USA, en inglés | Windows y Linux (`*_es`), nombres `(Spain)`, en español, corrección aplicada automáticamente |
| Actualización automática | releases de BlackLabelHQ | releases de este fork |
| `tools/pal-port/`, `tools/ci/` | — | herramientas que han generado el port, con verificación, y compilación automática |

| | Española | Italiana |
|---|---|---|
| Configuración | `config/es/MediEvil.json` | `config/MediEvil.json` |
| Código generado | `generated-es/` | `generated/` |
| Compilación | `dotnet build -p:Lingua=es` | `dotnet build` |
| Salida | `bin-es/`, `obj-es/` | `bin/`, `obj/` |
| Scripts | `windows_*_es.bat`, `linux_*_es.sh` | en [MediEvilRecomp-ITA](https://github.com/Leproide/MediEvilRecomp-ITA) |
| Disco | `disc/MediEvil (Spain).cue` | `disc/MediEvil (Italy).cue` |

---

## Requisitos

- GPU compatible con OpenGL 2.1 como mínimo
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [OpenAL](https://www.openal.org/downloads/)
- [Git](https://git-scm.com/install/)
- Una copia **original** de MediEvil PAL España, de la que sacar un volcado **limpio** en formato bin/cue. Lee [`disc/LEEME.md`](disc/LEEME.md).

Este repositorio **no contiene archivos del juego** ni distribuye su código: todo se genera en tu ordenador a partir de tu disco.

---

## Compilación

### Windows (recomendado)

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
   - genera el código del juego en `generated-es\` a partir de tu disco;
   - compila el juego con `-p:Lingua=es`;
   - descarga el shader Dan's Curse V2 en `shaders\`.

Si no puedes compilarlo tú, descarga el paquete listo para Windows, Linux o macOS desde las [releases](https://github.com/Leproide/MediEvilRecomp-ESP/releases/latest).

### Linux

Hacen falta `git` y el [SDK de .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0) (en muchas distribuciones, el paquete `dotnet-sdk-10.0`).

1. Clona el repositorio y copia en `disc/` los archivos del disco con los mismos nombres de arriba. En Linux las mayúsculas y minúsculas cuentan.
2. Desde la carpeta del repositorio:
   ```
   chmod +x linux_initial_build_es.sh linux_run_es.sh
   ./linux_initial_build_es.sh
   ```
   El script hace los mismos pasos que el de Windows.

`run.sh` es el lanzador del proyecto original para una build de Linux ya lista (ejecutable `medievil` en la misma carpeta, como en los paquetes de las releases): no compila nada.

### A mano (Windows, Linux, macOS)

Desde la carpeta del repositorio:

```
git submodule update --init --recursive
git -C RecompOne apply ../recompone-fixes/cd-whole-sector-mode.patch
dotnet build RecompOne/RecompOne.sln
dotnet run --project RecompOne/RecompOne.Recompiler config/es/MediEvil.json
dotnet build RecompOne.MediEvil.csproj -p:Lingua=es
```

`git apply` solo hace falta la primera vez. Si la corrección ya está aplicada, da un error que puedes ignorar.

Para la versión italiana, en el mismo repositorio: `config/MediEvil.json` y los mismos comandos sin `-p:Lingua=es`, o los scripts sin `_es`.

---

## Ejecución

- **Windows:** `windows_run_es.bat`. Si el juego todavía no está compilado, la primera compilación arranca sola.
- **Inicio rápido sin recompilar:** `windows_run_no_build_es.bat`.
- **Linux:** `./linux_run_es.sh` (compila en el primer inicio), o `./linux_run_es.sh --no-build`.
- **Desde terminal:** `dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=es`.

Cuando el juego pida el disco, elige `disc\MediEvil (Spain).cue`. La versión española comprueba que el disco arranque `SCES_014.95`: si el ajuste apunta todavía a otro disco (por ejemplo al italiano, porque las dos versiones comparten `settings.json`), vuelve a pedirlo.

Después de actualizar `config/es/` hay que regenerar el código: borra la carpeta `generated-es` y vuelve a ejecutar `windows_initial_build_es.bat` (Linux: `./linux_initial_build_es.sh`).

---

## Debug

`patches/PalDebug.cs` activa los logs internos de RecompOne cuando está definida la variable `MEDIEVIL_DEBUG`. Los canales disponibles son `cd`, `irq`, `bios`, `sdk`, `vsync`, `gpu`, `dma`, `spu`, `mdec` o `all`. Con cualquier valor (por ejemplo `libcrypt`), `PalLibCrypt` escribe además el detalle del control LibCrypt (líneas `[PalLibCrypt]`). Si la variable no está definida, no cambia nada.

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

## LibCrypt: El Mausoleo de la Cima de la Colina

El disco PAL español está protegido con **LibCrypt**, igual que el italiano. Desde el mapa, el juego (código en 0x80065D20..0x80065FC0, iniciado en LANDMAP 0x80011E98) lee con GetlocP la posición de 16 sectores de la tabla en 0x800DB334. Ocho de ellos tienen en el disco original el subcódigo Q modificado, y de ellos sale una clave de 16 bits (0x800DB32C). Con esa clave `func_80065FC4` descifra el primer sector del Mausoleo en `MEDRES.MWD` (recurso 0xD6). Un volcado bin/cue no tiene subcódigo: la clave sale mal y el juego se queda en la pantalla del CD con el signo de exclamación.

`patches/PalLibCrypt.cs` (común a las dos versiones) hace que GetlocP devuelva lo que daría el lector real. Usa el archivo `.sbi`/`.lsd` junto al `.cue` si existe; si no, la lista incluida de SCES-01495. Comprobado sin ejecutar el juego con `tools/pal-port/libcrypt_check.py`:

| | Italiana (SCES-01494) | Española (SCES-01495) |
|---|---|---|
| Sectores protegidos (LBA) | 13955 … 44009 | 14499, 14749, 14906, 15162, 15478, 15881, 15951, 16017, 42521, 42663, 42862, 43204, 43484, 43904, 44009, 44162 |
| Clave | 0xAAA6 | **0x0E57** (8 bits a 1 en los primeros 16 controles) |
| Recurso 0xD6 de `MEDRES.MWD` | — | sector 0x1401, 152020 bytes comprimidos, 315748 descomprimidos |
| Resultado del descifrado | `PP20` | `PP20`; el tamaño al final del flujo (315748) coincide con el índice y se descomprime completo |

---

## Trucos

El juego tiene dos menús de trucos integrados, y en el recomp funcionan igual que en la consola. En pausa, mantén pulsado **L2** y pulsa:

- **Modo trucos**: Abajo, Arriba, Cuadrado, Triángulo, Triángulo, Círculo, Abajo, Arriba, Cuadrado, Triángulo
- **Modo trucos ampliado**: Triángulo, Círculo, Triángulo, Círculo, Círculo, Triángulo, Izquierda, Círculo, Arriba, Abajo, Derecha, Círculo, Izquierda, Izquierda, Triángulo, Derecha, Círculo, Izquierda, Izquierda, Triángulo, Círculo, Abajo, Círculo, Círculo, Derecha

La versión española usa las mismas secuencias que la italiana: las dos tablas de `MEDIEVIL.EXE` tienen el mismo contenido y solo cambia su posición. La mod incluida **Trucchi** los activa desde el panel de mods, sin combinaciones, y reconoce las dos versiones. Detalles en el [README de la mod](mods/Trucchi/README.md) (en italiano).

<img width="1920" height="1057" alt="Trucos_MediEvil_PAL_ESP" src="https://github.com/user-attachments/assets/e63afa4e-62b0-4c83-8b22-3eb064944a8c" />

<img width="1920" height="1080" alt="Trucos2_MediEvil_PAL_ESP" src="https://github.com/user-attachments/assets/02eb4635-7d14-45dd-bd4c-f63e5330c341" />

---

## Vídeos a pantalla completa

La mod opcional **Filmati a schermo intero** amplía los vídeos: en lugar de verlos pequeños en el centro del cuadro 4:3, muestra solo el área del vídeo y la adapta a la ventana sin deformarla. Se activa desde el panel de mods y funciona en las dos versiones. Detalles en el [README de la mod](mods/FilmatiSchermoIntero/README.md) (en italiano).

---

## Mod Hub

La versión española **no tiene un Mod Hub**: el menú **Mods → Explore Mod Hub** queda desactivado. El hub original no se usa, porque sus mods son para la versión USA, y el de la versión italiana ([MediEvilRecomp-ITA-ModHub](https://github.com/Leproide/MediEvilRecomp-ITA-ModHub)) está pensado para SCES-01494. En la build italiana `DefaultModHub` (en `Program.cs`) apunta al hub ITA; en la española está vacío.

Quien quiera probar un hub de todos modos puede definirlo en `settings.json`, sin recompilar:

```json
{
  "options": {
    "modhub.repository": "https://github.com/<usuario>/<repositorio>",
    "modhub.branch": "master"
  }
}
```

### Shader

El shader **[Dan's Curse V2](https://github.com/Leproide/DansCurseV2)** viene incluido en los paquetes de las releases y lo descargan los scripts de primera compilación en `shaders/dans-curse-v2-shader`, siempre en la última versión de su rama `main` (el mismo shader de la versión italiana). Para actualizarlo basta con volver a ejecutar el script de primera compilación. Se activa encendiendo la mod **Shaders** en el panel **Mods**, con "Better rendering" activo; los parámetros se ajustan en el menú **Shaders**. Recomendado: **Draw distance** a 3x en Settings → Display.

---

## Releases automáticas (GitHub Actions)

`.github/workflows/release.yml` compila la versión española para Windows (x64, ARM64), Linux x64 y macOS (Intel, Apple Silicon) y publica la release cuando se sube una etiqueta `v*` (por ejemplo `git tag v1.2 && git push origin v1.2`). Desde Actions → Release → Run workflow se obtienen solo los artefactos, sin release.

El workflow solo necesita los ejecutables del disco (unos 2 MB), tomados de un repositorio **privado**:
1. Crea un repositorio privado (por ejemplo `MediEvilRecomp-ESP-Disc`) con, en la raíz, `SYSTEM.CNF`, `SCES_014.95`, `MEDIEVIL.EXE` y la carpeta `OVERLAYS` extraídos del disco PAL español (sin `MEDRES.MWD`, que no hace falta). Tiene que seguir siendo privado.
2. Crea un token (Settings → Developer settings → Fine-grained tokens) con acceso de solo lectura (Contents: Read) a ese repositorio.
3. En este repositorio, Settings → Secrets and variables → Actions: secret `DISC_SOURCE_PAT` = el token. La variable `DISC_SOURCE_REPO` solo hace falta si el repositorio no es `Leproide/MediEvilRecomp-ESP-Disc`.

`tools/ci/make_disc.py` reconstruye con esos archivos una imagen mínima para el recompilador, y `tools/ci/fix_overlay_lba.py --ver esp` vuelve a poner en el código generado los sectores reales del disco español (sacados con `tools/ci/disc_lba.py`). Los paquetes no contienen datos del juego: cada jugador usa su propio disco. Las notas de la release son la primera sección de `changelog.md`.

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

Las 4399 funciones son *exact* o *aligned*: no hay ninguna función *fuzzy* ni ninguna sin pareja. Los datos de `MEDIEVIL.EXE` se desplazan por zonas: −0x18 (hasta ~0x800D8xxx), +0x80 (hasta ~0x800ED9FC) y +0x78 (el resto). Por eso los offsets `$gp` de las variables cercanas a `$gp` bajan 8 bytes.

### Tabla de direcciones por versión

Todas las direcciones que cambian entre versiones están en **`config/versions.json`**:

| Sección | Contenido |
|---|---|
| `hooks` | los 119 hooks de `config/*/MediEvil.json` (dirección ITA / ESP), incluidos los modos `precise` de MediEvilRecomp 1.0 |
| `entryPoints` | puntos de entrada declarados en `overlays[].functions` |
| `relocations` | arrays reubicados (`gapsPolySquaresCaptured`) |
| `constants` | direcciones, offsets `$gp` y funciones con nombre usados por los parches escritos a mano y por `wrapers/` |
| `literals` | inmediatos de las copias de funciones recompiladas (`CullPatch`, `LandMapPatch`), identificados por la instrucción italiana que los produce |

A partir de la tabla se generan **`patches/GameAddresses.cs`** (clases `GameVersion`, `Addr` y `RAddr`, con constantes `GameVersion.Spanish ? ESP : ITA` elegidas al compilar con `-p:Lingua=es`) y **`config/es/MediEvil.json`**. Los valores italianos son exactamente los que antes estaban escritos en el código, así que la versión italiana no cambia.

### Herramientas

```
py tools\pal-port\version_port.py check --ita CARPETA_ITA --dst CARPETA_ESP
py tools\pal-port\version_port.py gen-cs
py tools\pal-port\version_port.py gen-config
py tools\pal-port\version_port.py fill --ita CARPETA_ITA --dst CARPETA_ESP
py tools\pal-port\version_port.py convert --ita CARPETA_ITA --dst CARPETA_ESP --force
py tools\pal-port\libcrypt_check.py --ver esp --exe MEDIEVIL.EXE --mwd MEDRES.MWD
py tools\ci\disc_lba.py "disc\MediEvil (Spain) (Track 1).bin"
```

- `check` comprueba la tabla, las dos configuraciones y `GameAddresses.cs`. Con las carpetas de los archivos extraídos de los dos discos verifica además hooks, destinos de `jal` y de llamada, inmediatos y constantes contra los binarios.
- `gen-cs` / `gen-config` regeneran los archivos tras editar la tabla; `fill` calcula el valor español de las constantes nuevas.
- `convert` rehace la tabla a partir de un repositorio italiano actualizado (fuentes con los números escritos a mano).

Resultado actual de `check` sobre los dos discos:

```
ita: 119 hooks (113 en inicios de funcmap, 6 en puntos de entrada declarados); jal: 6109 + 6866, 0 fuera de inicio de función
esp: 119 hooks (113 en inicios de funcmap, 6 en puntos de entrada declarados); jal: 6109 + 6866, 0 fuera de inicio de función
106 literals y 65 constantes verificados
RESULT: OK
```

Los archivos extraídos de los discos están protegidos por derechos de autor: **no los subas nunca al repositorio**.

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
wrapers/                   acceso a las estructuras del juego (MediEvilRecomp 1.0)
recompone-fixes/           correcciones para el submódulo RecompOne
mods/Trucchi/              mod para activar los menús de trucos
mods/FilmatiSchermoIntero/ mod opcional para los vídeos a pantalla completa
disc/                      aquí van los archivos de tu disco (no incluidos)
tools/pal-port/            herramientas de port, informes y tabla por versión
tools/ci/                  imagen mínima y sectores reales para las releases automáticas
BACKUP_Dipendenze/         copia de reserva de RecompOne y ShaderMod
misc/                      actualización automática
assets/                    icono e idiomas de la interfaz
Directory.Build.props      carpetas de salida bin-es/obj-es con -p:Lingua=es
```

---

## Créditos

- **BlackLabelHQ** (Derp Princess, flaffymg): [MediEvilRecomp](https://github.com/BlackLabelHQ/MediEvilRecomp) y [RecompOne](https://github.com/BlackLabelHQ/RecompOne), es decir, todo el trabajo de recompilación en el que se basa este fork.
- El equipo de la decompilación de MediEvil, cuyas referencias usó el proyecto original.
- Port PAL italiano y español: https://github.com/Leproide

---

## License

Este repositorio contiene código con licencias distintas.

**Código de MediEvilRecomp (BlackLabelHQ): sin licencia.** MediEvilRecomp no tiene una licencia explícita, así que los derechos sobre su código siguen siendo de sus autores. Este fork **no** aplica la GPL a ese código, ni a las partes adaptadas a las versiones PAL. Son los archivos con la cabecera "Codice originale di MediEvilRecomp" o "Código original de MediEvilRecomp": `Program.cs`, `RecompOne.MediEvil.csproj`, `wrapers/`, `misc/AutoUpdater.cs`, `assets/`, los parches de `patches/render/` (salvo `TitleShaderPatch.cs`), `patches/widescreen/` (salvo `IrisFadePatch.cs`) y `patches/qol/`, los scripts `windows_*_es.bat`, `run.sh`, `.github/workflows/release.yml` y la configuración de `config/` (también `config/es/` y `config/versions.json`). Lo mismo vale para ShaderMod (`mods/ShaderMod`, `BACKUP_Dipendenze/ShaderMod`).

**RecompOne: licencia MIT**, Copyright (c) 2026 flaffy. El texto está en el archivo `LICENSE` del submódulo `RecompOne` (y en `BACKUP_Dipendenze/RecompOne`); los paquetes de las releases lo incluyen como `LICENSE-RecompOne-MIT.txt`.

**Código escrito para este fork: GPL-3.0-or-later.** Texto completo en [`tools/pal-port/LICENSE`](tools/pal-port/LICENSE). Comprende:
- `patches/PalDispatch.cs`, `PalDebug.cs`, `PalCdCallbacks.cs`, `PalMovies.cs`, `PalDisplay.cs`, `PalLibCrypt.cs`, `PalXaSpeech.cs`, `GameAddresses.cs`, `GameDisc.cs`;
- `patches/widescreen/IrisFadePatch.cs`, `patches/render/TitleShaderPatch.cs`, `patches/diag/`;
- las mods `mods/Trucchi/` y `mods/FilmatiSchermoIntero/`;
- `recompone-fixes/`;
- las herramientas de `tools/pal-port/` y `tools/ci/`;
- los scripts `linux_initial_build_es.sh`, `linux_run_es.sh` y `Directory.Build.props`;
- la documentación escrita para el fork (`disc/README.md`, `disc/LEEME.md`, los README de las mods y de las herramientas).

MediEvil es una marca de sus respectivos propietarios: este proyecto no está afiliado ni aprobado por Sony Interactive Entertainment.

---

## Author

https://github.com/Leproide
