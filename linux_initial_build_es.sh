#!/usr/bin/env bash
# ==================================================================================================
# MediEvilRecomp ESP - primera compilación en Linux (versión PAL española, SCES-01495)
# Equivalente de windows_initial_build_es.bat.
# Author: https://github.com/Leproide - GPL-3.0-or-later
#
# Usa config/es/MediEvil.json, genera el código en generated-es/ y compila con -p:Lingua=es
# (salida en bin-es/). La versión italiana (config/MediEvil.json, generated/, bin/) no se toca.
#
# Uso:  ./linux_initial_build_es.sh            (pregunta si ya está compilado)
#       ./linux_initial_build_es.sh --yes      (sin preguntas)
#       ./linux_initial_build_es.sh --release  (compila en Release)
# ==================================================================================================

set -euo pipefail

cd "$(dirname "$(readlink -f "$0")")"

DISC="disc"
CONFIG="config/es/MediEvil.json"
RECOMPONE="RecompOne"
RECOMPILER="$RECOMPONE/RecompOne.Recompiler"
CDFIX="recompone-fixes/cd-whole-sector-mode.patch"
GAMEPROJ="RecompOne.MediEvil.csproj"
CONFIGURATION="Debug"
ASK=1

for arg in "$@"; do
    case "$arg" in
        --yes|-y) ASK=0 ;;
        --release) CONFIGURATION="Release" ;;
        nocredits) ;;
        *) echo "Argumento desconocido: $arg" >&2; exit 2 ;;
    esac
done

say()  { printf '\033[1;36m> %s\033[0m\n' "$1"; }
die()  { printf '\033[1;31mERROR: %s\033[0m\n' "$1" >&2; exit 1; }
ask()  { # ask "pregunta" -> 0 si/1 no
    [ "$ASK" = 0 ] && return 0
    local r; read -r -p "$1 [s/N] " r; [[ "$r" =~ ^[sSyY]$ ]]
}

echo "========="
echo "Créditos:"
echo "========="
echo "Proyecto original MediEvilRecomp: BlackLabelHQ (Derp Princess, flaffymg)"
echo "Port PAL italiano y español: https://github.com/Leproide"
echo

# ------------------------------------------------------------------- ¿compilación ya hecha?
if [ -d generated-es ]; then
    ask "Parece que el proyecto ya está compilado. ¿Repetir la primera compilación?" || { echo "Cancelado."; exit 1; }
fi

# ------------------------------------------------------------------- herramientas necesarias
command -v git >/dev/null 2>&1 || die "falta git (por ejemplo: sudo apt install git)"
command -v dotnet >/dev/null 2>&1 || die "falta el SDK de .NET 10: https://dotnet.microsoft.com/download/dotnet/10.0"
dotnet --list-sdks | grep -q '^10\.' || die "hace falta el SDK de .NET 10 o superior: https://dotnet.microsoft.com/download/dotnet/10.0"

# ------------------------------------------------------------------- submódulos
say "Actualizo los submódulos..."
if ! git submodule update --init --recursive; then
    [ -f "$RECOMPONE/RecompOne.sln" ] || die "no se han podido descargar los submódulos y falta $RECOMPONE"
    echo "No se han podido actualizar los submódulos; uso los que ya hay."
fi

# ------------------------------------------------------------------- corrección de RecompOne (CD)
say "Corrección de RecompOne para el arranque PAL..."
if git -C "$RECOMPONE" apply --reverse --check "../$CDFIX" >/dev/null 2>&1; then
    echo "La corrección ya está aplicada."
else
    git -C "$RECOMPONE" apply "../$CDFIX" || die "no se puede aplicar $CDFIX: comprueba la versión del submódulo RecompOne"
    echo "Corrección aplicada."
fi

# ------------------------------------------------------------------- compila RecompOne
say "Compilo RecompOne..."
dotnet build "$RECOMPONE/RecompOne.sln" -c "$CONFIGURATION" || die "la compilación de RecompOne ha fallado"

# ------------------------------------------------------------------- archivos del disco
say "Compruebo los archivos del disco..."
missing=0
for f in "MediEvil (Spain) (Track 1).bin" "MediEvil (Spain) (Track 2).bin" "MediEvil (Spain).cue"; do
    if [ ! -f "$DISC/$f" ]; then echo "Falta: $DISC/$f"; missing=1; fi
done
[ "$missing" = 0 ] || die "faltan archivos del disco o tienen otro nombre (los nombres distinguen mayúsculas). Lee disc/LEEME.md"
[ -f "$CONFIG" ] || die "falta $CONFIG"

# ------------------------------------------------------------------- genera el código C#
say "Genero el código C# recompilado en generated-es/..."
dotnet run --project "$RECOMPILER" -c "$CONFIGURATION" -- "$CONFIG" || die "la generación del código C# ha fallado"

# ------------------------------------------------------------------- compila el juego
say "Compilo MediEvilRecomp ESP..."
dotnet build "$GAMEPROJ" -c "$CONFIGURATION" -p:Lingua=es || die "la compilación de MediEvilRecomp ESP ha fallado"

echo
echo "Listo. Usa ./linux_run_es.sh para jugar."
