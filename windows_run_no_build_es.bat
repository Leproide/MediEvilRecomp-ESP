@echo off
REM MediEvilRecomp ESP - inicio rapido sin recompilar (util mientras trabajas en los mods).
REM Basado en windows_run_no_build.bat de BlackLabelHQ.
REM Author: https://github.com/Leproide - GPL-3.0-or-later
dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=es --no-build
