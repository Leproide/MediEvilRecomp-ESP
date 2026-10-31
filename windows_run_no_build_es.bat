@echo off
REM MediEvilRecomp ESP - inicio rapido sin recompilar (util mientras trabajas en las mods).
REM Basado en windows_run_no_build.bat de BlackLabelHQ.
REM Codigo original de MediEvilRecomp (BlackLabelHQ), adaptado a las versiones PAL.
REM MediEvilRecomp no tiene una licencia explicita: los derechos sobre este codigo siguen siendo
REM de sus autores. Este archivo NO se publica bajo GPL-3.0: la GPL del fork cubre solo los
REM archivos escritos desde cero para el port (ver la seccion License del README).
REM Cambios para el port PAL espanol: https://github.com/Leproide
dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=es --no-build
