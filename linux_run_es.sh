#!/usr/bin/env bash
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvilRecomp ESP - inicio en Linux (versión PAL española, SCES-01495)
# Equivalente de windows_run_es.bat: si falta la primera compilación, la hace.
# Con la opción --no-build arranca sin recompilar (como windows_run_no_build_es.bat).
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
# Ejemplo de log:  MEDIEVIL_DEBUG=libcrypt ./linux_run_es.sh > debug.txt 2>&1

set -uo pipefail
cd "$(dirname "$(readlink -f "$0")")" || exit 1

GAME=(dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=es)

if [ "${1:-}" = "--no-build" ]; then
    shift
    exec "${GAME[@]}" --no-build -- "$@"
fi

echo "Proyecto original MediEvilRecomp: BlackLabelHQ (Derp Princess, flaffymg)"
echo "Port PAL italiano y español: https://github.com/Leproide"
echo

if [ ! -d "generated-es" ]; then
    echo "La primera compilación todavía no se ha hecho: la hago ahora."
    ./linux_initial_build_es.sh nocredits --yes || { echo "La primera compilación ha fallado."; exit 1; }
fi

echo "> Inicio MediEvilRecomp ESP..."
exec "${GAME[@]}" -- "$@"
