@echo off
REM ==================================================================================================
REM MediEvilRecomp DEU - start with debug log sin recompilar
REM Copyright (C) 2026
REM Author: https://github.com/Leproide
REM SPDX-License-Identifier: GPL-3.0-or-later
REM
REM This program is free software: you can redistribute it and/or modify it under the terms of the
REM GNU General Public License as published by the Free Software Foundation, either version 3 of the
REM License, or (at your option) any later version. This program is distributed WITHOUT ANY WARRANTY.
REM See <https://www.gnu.org/licenses/>.
REM
REM Starts windows_run_no_build_de.bat with MEDIEVIL_DEBUG=1 (debug lines of patches and mods) and saves
REM all the output to debug.txt, while showing it in the console.
REM Version francaise de windows_run_debug*_de.bat (Leproide).
REM ==================================================================================================
title MediEvilRecomp DEU (debug)
cd /d "%~dp0"

set MEDIEVIL_DEBUG=1
powershell -NoProfile -ExecutionPolicy Bypass -Command "& cmd /c windows_run_no_build_de.bat 2>&1 | Tee-Object -FilePath debug.txt"
