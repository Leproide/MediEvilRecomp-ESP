@echo off
REM MediEvilRecomp ENG - quick start without rebuilding (useful while working on mods).
REM Based on windows_run_no_build_es.bat (https://github.com/Leproide) and windows_run_no_build.bat
REM by BlackLabelHQ. Original MediEvilRecomp code (BlackLabelHQ), adapted to the PAL versions.
REM MediEvilRecomp has no explicit license: the rights to this code remain with its
REM authors. This file is NOT released under GPL-3.0 (see the License section of the README).
dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=en --no-build
