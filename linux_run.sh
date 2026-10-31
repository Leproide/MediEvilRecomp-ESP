#!/usr/bin/env bash
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvilRecomp ITA - avvio su Linux (versione PAL italiana, SCES-01494)
# Equivalente di windows_run.bat: se manca la prima compilazione la esegue.
# Con l'opzione --no-build avvia senza ricompilare (come windows_run_no_build.bat).
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

set -uo pipefail
cd "$(dirname "$(readlink -f "$0")")" || exit 1

if [ "${1:-}" = "--no-build" ]; then
    shift
    exec dotnet run --no-build -- "$@"
fi

echo "Progetto originale MediEvilRecomp: BlackLabelHQ (Derp Princess, flaffymg)"
echo "Port PAL italiano: https://github.com/Leproide"
echo

if [ ! -d "generated" ]; then
    echo "La prima compilazione non e' ancora stata fatta: la eseguo adesso."
    ./linux_initial_build.sh nocredits || { echo "Prima compilazione fallita."; exit 1; }
fi

echo "> Avvio MediEvilRecomp ITA..."
exec dotnet run -- "$@"
