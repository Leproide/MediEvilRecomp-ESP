@echo off
REM ==================================================================================================
REM MediEvilRecomp ESP - inicio (version PAL espanola, SCES-01495)
REM Basado en windows_run.bat de BlackLabelHQ y en el port PAL italiano.
REM Author: https://github.com/Leproide - GPL-3.0-or-later
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
