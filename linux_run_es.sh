#!/usr/bin/env bash
# ==================================================================================================
# MediEvilRecomp ESP - inicio en Linux (versión PAL española, SCES-01495)
# Equivalente de windows_run_es.bat. Si el juego no está compilado, lanza la primera compilación.
# Author: https://github.com/Leproide - GPL-3.0-or-later
#
# Uso:  ./linux_run_es.sh              compila si hace falta y arranca
#       ./linux_run_es.sh --no-build   arranca sin recompilar
#       MEDIEVIL_DEBUG=libcrypt ./linux_run_es.sh > debug.txt 2>&1
# ==================================================================================================

set -euo pipefail

cd "$(dirname "$(readlink -f "$0")")"

NOBUILD=()
[ "${1:-}" = "--no-build" ] && NOBUILD=(--no-build)

if [ ! -d generated-es ]; then
    echo "La primera compilación todavía no se ha hecho: la hago ahora."
    ./linux_initial_build_es.sh --yes
fi

exec dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=es ${NOBUILD[@]+"${NOBUILD[@]}"}
