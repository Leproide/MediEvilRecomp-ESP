@echo off
REM ==================================================================================================
REM MediEvilRecomp FRA - lancement avec journal de debug
REM Copyright (C) 2026
REM Author: https://github.com/Leproide
REM SPDX-License-Identifier: GPL-3.0-or-later
REM
REM This program is free software: you can redistribute it and/or modify it under the terms of the
REM GNU General Public License as published by the Free Software Foundation, either version 3 of the
REM License, or (at your option) any later version. This program is distributed WITHOUT ANY WARRANTY.
REM See <https://www.gnu.org/licenses/>.
REM
REM Lance windows_run_fr.bat avec MEDIEVIL_DEBUG=1 (lignes de debug des patches et mods) et enregistre
REM toute la sortie dans debug.txt, tout en l'affichant dans la console.
REM Version francaise de windows_run_debug*_fr.bat (Leproide).
REM ==================================================================================================
title MediEvilRecomp FRA (debug)
cd /d "%~dp0"

set MEDIEVIL_DEBUG=1
powershell -NoProfile -ExecutionPolicy Bypass -Command "& cmd /c windows_run_fr.bat 2>&1 | Tee-Object -FilePath debug.txt"
