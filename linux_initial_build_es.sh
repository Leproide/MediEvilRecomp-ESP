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

# ------------------------------------------------------------------ corrección CD
say "Corrección de RecompOne para el arranque PAL..."
if git -C "$RECOMPONE" apply --reverse --check "../$CDFIX" >/dev/null 2>&1; then
    echo "La corrección ya está aplicada."
else
    git -C "$RECOMPONE" apply "../$CDFIX" \
        || fail "no se puede aplicar $CDFIX. Comprueba que el submódulo RecompOne esté en la versión prevista."
    echo "Corrección aplicada."
fi

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
# Última versión de la rama main, en shaders/ como si estuviera instalado desde el Mod Hub
# (el commit descargado se escribe en el marcador .modhub). Si la descarga falla, se continúa.
DCDIR="shaders/dans-curse-v2-shader"
if [ ! -f "$DCDIR/dans-curse-v2.slangp" ]; then
    say "Descargo el shader Dan's Curse V2..."
    rm -rf "$DCDIR"
    if git clone -q --depth 1 https://github.com/Leproide/DansCurseV2 "$DCDIR" >/dev/null 2>&1; then
        DCCOMMIT="$(git -C "$DCDIR" rev-parse HEAD)"
        rm -rf "$DCDIR/.git"
        printf '{"Id":"dans-curse-v2-shader","Type":"shader","Commit":"%s","Repository":"https://github.com/Leproide/DansCurseV2"}' "$DCCOMMIT" > "$DCDIR/.modhub"
        echo "Shader instalado: se activa encendiendo la mod Shaders en Mods, con \"Better rendering\" activo."
    else
        rm -rf "$DCDIR"
        echo "No se ha podido descargar: vuelve a ejecutar este script más tarde para instalar el shader."
    fi
fi

echo
echo "Listo. Usa ./linux_run_es.sh para jugar."
