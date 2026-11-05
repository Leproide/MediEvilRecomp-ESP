#!/usr/bin/env bash
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvilRecomp ESP - primera compilación en Linux (versión PAL española, SCES-01495)
# Equivalente de windows_initial_build_es.bat. Usa config/es/MediEvil.json, genera el
# código en generated-es/ y compila con -p:Lingua=es (salida en bin-es/).
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
#
# Uso:  ./linux_initial_build_es.sh            (pregunta si ya está compilado)
#       ./linux_initial_build_es.sh --yes      (sin preguntas)

set -uo pipefail

# trabaja siempre desde la carpeta del repositorio, se lance desde donde se lance
cd "$(dirname "$(readlink -f "$0")")" || exit 1

DISC="disc"
RECOMPONE="RecompOne"
RECOMPILER="$RECOMPONE/RecompOne.Recompiler"
MEDIEVILJSON="config/es/MediEvil.json"
CDFIX="recompone-fixes/cd-whole-sector-mode.patch"
GBUFFIX="recompone-fixes/shader-gbuffer.patch"
LIGHTFIX="recompone-fixes/effect-lights.patch"
PICKFIX="recompone-fixes/material-picker.patch"
GAMEPROJ="RecompOne.MediEvil.csproj"
ASK=1
CREDITS=1

for arg in "$@"; do
    case "$arg" in
        --yes|-y) ASK=0 ;;
        nocredits) CREDITS=0 ;;
        *) echo "Argumento desconocido: $arg" >&2; exit 2 ;;
    esac
done

say()  { printf '\n\033[1;36m> %s\033[0m\n' "$1"; }
fail() { printf '\n\033[1;31mERROR: %s\033[0m\n' "$1" >&2; exit 1; }

ask() {
    # pregunta s/n, respuesta por defecto: no
    [ "$ASK" = 0 ] && return 0
    local r
    read -r -p "$1 [s/N] " r
    [[ "$r" =~ ^[sSyY]$ ]]
}

if [ "$CREDITS" = 1 ]; then
    echo "========="
    echo "Créditos:"
    echo "========="
    echo
    echo "Proyecto original MediEvilRecomp:"
    echo "  BlackLabelHQ: Derp Princess, flaffymg"
    echo
    echo "Port PAL italiano y español:"
    echo "  https://github.com/Leproide"
    echo
fi

if [ -d "generated-es" ]; then
    echo "Nota: parece que el proyecto ya está compilado."
    ask "¿Quieres repetir la primera compilación?" || { echo "Cancelado."; exit 1; }
fi

# ------------------------------------------------------------------ submódulos
say "Actualizo los submódulos..."
command -v git >/dev/null 2>&1 || fail "no se encuentra git. Instálalo con el gestor de paquetes de tu distribución."
if ! git submodule update --init --recursive >/dev/null 2>&1; then
    echo "No se han podido actualizar los submódulos. Sin ellos el proyecto no funciona."
    ask "¿Quieres continuar de todos modos? Normalmente no, salvo que ya los tuvieras." || { echo "Cancelado."; exit 1; }
    echo "Continúo de todos modos..."
fi

# ------------------------------------------------------------------ correcciones RecompOne
# en este orden: lector de CD en modo "sector entero", G-buffer y luces de los efectos para los
# shaders, selector de materiales; las que ya están aplicadas se saltan
say "Correcciones de RecompOne..."
for FIX in "$CDFIX" "$GBUFFIX" "$LIGHTFIX" "$PICKFIX"; do
    if git -C "$RECOMPONE" apply --reverse --check "../$FIX" >/dev/null 2>&1; then
        echo "$FIX ya está aplicada."
    else
        git -C "$RECOMPONE" apply "../$FIX" \
            || fail "no se puede aplicar $FIX. Comprueba que el submódulo RecompOne esté en la versión prevista."
        echo "$FIX aplicada."
    fi
done

# ------------------------------------------------------------------ .NET
say "Compruebo .NET 10..."
if ! command -v dotnet >/dev/null 2>&1 || ! dotnet --list-sdks 2>/dev/null | grep -q '^10\.'; then
    fail "hace falta el SDK de .NET 10 o superior: https://dotnet.microsoft.com/download/dotnet/10.0
En muchas distribuciones: paquete dotnet-sdk-10.0, o el script oficial dotnet-install.sh."
fi
echo "Encontrado."

say "Compilo RecompOne..."
dotnet build "$RECOMPONE/RecompOne.sln" || fail "la compilación de RecompOne ha fallado."
echo "Recompilador y runtime de RecompOne compilados."

# ------------------------------------------------------------------ disco
say "Compruebo los archivos del disco..."
MISSING=0
for f in "MediEvil (Spain) (Track 1).bin" "MediEvil (Spain) (Track 2).bin" "MediEvil (Spain).cue"; do
    if [ ! -f "$DISC/$f" ]; then
        echo "Falta: $f"
        MISSING=1
    fi
done
# Linux distingue mayúsculas y minúsculas: avisa si los nombres solo se diferencian en eso
if [ "$MISSING" = "1" ]; then
    ls "$DISC" 2>/dev/null | grep -i "medievil" | sed 's/^/Encontrado en la carpeta: /'
    fail "faltan archivos del disco o tienen otro nombre (atención a mayúsculas y minúsculas). Lee disc/LEEME.md"
fi
echo "Archivos del disco encontrados."

# ------------------------------------------------------------------ configuración
say "Compruebo el archivo de configuración..."
[ -f "$MEDIEVILJSON" ] || fail "falta el archivo de configuración: $PWD/$MEDIEVILJSON"
echo "Configuración encontrada."

# ------------------------------------------------------------------ código generado
say "Genero el código C# recompilado en generated-es/..."
dotnet run --project "$RECOMPILER" "$MEDIEVILJSON" || fail "la generación del código C# ha fallado."

say "Compilo MediEvilRecomp ESP..."
dotnet build "$GAMEPROJ" -p:Lingua=es || fail "la compilación de MediEvilRecomp ESP ha fallado."
echo "Compilación terminada."

# ------------------------------------------------------------------ shader Dan's Curse V2 (opcional)
# Siempre la última versión de la rama main (git clone --depth 1, sin commit fijo), en
# shaders/dans-curse-v2-shader. La versión española no tiene Mod Hub, así que el shader se
# actualiza cada vez que se ejecuta este script: se descarga en una carpeta temporal y solo si
# la descarga sale bien sustituye a la anterior. Si falla, se continúa con el shader que ya estaba.
DCDIR="shaders/dans-curse-v2-shader"
DCNEW="shaders/dans-curse-v2-shader.new"
say "Descargo la última versión del shader Dan's Curse V2..."
mkdir -p shaders
rm -rf "$DCNEW"
if git clone -q --depth 1 https://github.com/Leproide/DansCurseV2 "$DCNEW" >/dev/null 2>&1 \
        && [ -f "$DCNEW/dans-curse-v2.slangp" ]; then
    echo "Dan's Curse V2: commit $(git -C "$DCNEW" rev-parse --short HEAD)"
    rm -rf "$DCNEW/.git" "$DCDIR"
    mv "$DCNEW" "$DCDIR"
    echo "Shader instalado: se activa encendiendo la mod Shaders en Mods, con \"Better rendering\" activo."
else
    rm -rf "$DCNEW"
    if [ -f "$DCDIR/dans-curse-v2.slangp" ]; then
        echo "No se ha podido descargar: se mantiene el shader ya instalado."
    else
        echo "No se ha podido descargar: vuelve a ejecutar este script más tarde para instalar el shader."
    fi
fi

# ------------------------------------------------------------------ shader Gallowmere (predeterminado)
# Gallowmere (GPL-3.0): shader predeterminado del fork, siempre la última versión de la rama principal
# (git clone --depth 1, sin commit fijo), en shaders/gallowmere-shader. Se actualiza cada vez que se
# ejecuta este script, como Dan's Curse V2; si la descarga falla se mantiene el que ya estaba. El juego
# lo selecciona en el primer arranque si no se ha elegido otro shader.
GMDIR="shaders/gallowmere-shader"
GMNEW="shaders/gallowmere-shader.new"
say "Descargo la última versión del shader Gallowmere..."
mkdir -p shaders
rm -rf "$GMNEW"
if git clone -q --depth 1 https://github.com/Leproide/gallowmere-shader "$GMNEW" >/dev/null 2>&1 \
        && [ -f "$GMNEW/gallowmere.slangp" ]; then
    echo "Gallowmere: commit $(git -C "$GMNEW" rev-parse --short HEAD)"
    rm -rf "$GMNEW/.git" "$GMDIR"
    mv "$GMNEW" "$GMDIR"
    echo "Shader Gallowmere instalado: se selecciona en el primer arranque."
else
    rm -rf "$GMNEW"
    if [ -f "$GMDIR/gallowmere.slangp" ]; then
        echo "No se ha podido descargar: se mantiene el shader ya instalado."
    else
        echo "No se ha podido descargar: vuelve a ejecutar este script más tarde para instalar el shader."
    fi
fi

echo
echo "Listo. Usa ./linux_run_es.sh para jugar."
