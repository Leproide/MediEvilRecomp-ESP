@echo off
REM ==================================================================================================
REM MediEvilRecomp ESP - inicio con log de debug sin recompilar
REM Copyright (C) 2026
REM Author: https://github.com/Leproide
REM SPDX-License-Identifier: GPL-3.0-or-later
REM
REM This program is free software: you can redistribute it and/or modify it under the terms of the
REM GNU General Public License as published by the Free Software Foundation, either version 3 of the
REM License, or (at your option) any later version. This program is distributed WITHOUT ANY WARRANTY.
REM See <https://www.gnu.org/licenses/>.
REM
REM Inicia windows_run_no_build_es.bat con MEDIEVIL_DEBUG=1 (lineas de debug de parches y mods en el log) y guarda
REM toda la salida tambien en debug.txt, mostrandola en la consola.
REM Version espanola de windows_run_debug*.bat de MediEvilRecomp-ITA.
REM ==================================================================================================
title MediEvilRecomp ESP (debug)
cd /d "%~dp0"

set MEDIEVIL_DEBUG=1
powershell -NoProfile -ExecutionPolicy Bypass -Command "& cmd /c windows_run_no_build_es.bat 2>&1 | Tee-Object -FilePath debug.txt"
