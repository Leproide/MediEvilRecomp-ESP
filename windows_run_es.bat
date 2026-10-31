@echo off
REM ==================================================================================================
REM MediEvilRecomp ESP - inicio (version PAL espanola, SCES-01495)
REM Basado en windows_run.bat de BlackLabelHQ.
REM Codigo original de MediEvilRecomp (BlackLabelHQ), adaptado a las versiones PAL.
REM MediEvilRecomp no tiene una licencia explicita: los derechos sobre este codigo siguen siendo
REM de sus autores. Este archivo NO se publica bajo GPL-3.0: la GPL del fork cubre solo los
REM archivos escritos desde cero para el port (ver la seccion License del README).
REM Cambios para el port PAL espanol: https://github.com/Leproide
REM ==================================================================================================
title MediEvilRecomp ESP

REM ============================================= Creditos ============================================
	echo =========
	echo Creditos:
	echo =========
	echo.
	echo Proyecto original MediEvilRecomp:
	echo.
	echo	BlackLabelHQ:
	echo	- Derp Princess
	echo	- flaffymg
	echo.
	echo Port PAL italiano y espanol:
	echo	- https://github.com/Leproide
	echo.
REM ===================================================================================================

REM ========================================= Inicia MediEvilRecomp ===================================
echo.
echo ^> Inicio MediEvilRecomp ESP...

if not exist "generated-es\" (
    echo La primera compilacion todavia no se ha hecho: la hago ahora.
    echo.
    echo Inicio windows_initial_build_es.bat...
    echo.
    call windows_initial_build_es.bat nocredits

    REM Se detiene si la compilacion ha fallado
    if errorlevel 1 (
        echo La primera compilacion ha fallado.
        pause
        exit /b 1
    )
)

dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=es

echo.
REM ===================================================================================================
pause
