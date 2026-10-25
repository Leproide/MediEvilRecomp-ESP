@echo off
REM ==================================================================================================
REM MediEvilRecomp ITA - avvio (versione PAL italiana, SCES-01494)
REM Basato su windows_run.bat di BlackLabelHQ.
REM Modifiche per il port PAL: Author: https://github.com/Leproide - GPL-3.0-or-later
REM ==================================================================================================
title MediEvilRecomp ITA

REM ============================================= Crediti =============================================
	echo ========
	echo Crediti:
	echo ========
	echo.
	echo Progetto originale MediEvilRecomp:
	echo.
	echo	BlackLabelHQ:
	echo	- Derp Princess
	echo	- flaffymg
	echo.
	echo Port PAL italiano:
	echo	- https://github.com/Leproide
	echo.
REM ===================================================================================================

REM ========================================= Avvia MediEvilRecomp ====================================
echo.
echo ^> Avvio MediEvilRecomp ITA...

if not exist "generated\" (
    echo La prima compilazione non e' ancora stata fatta: la eseguo adesso.
    echo.
    echo Avvio windows_initial_build.bat...
    echo.
    call windows_initial_build.bat nocredits

    REM Interrompe se la compilazione e' fallita
    if errorlevel 1 (
        echo Prima compilazione fallita.
        pause
        exit /b 1
    )
)

dotnet run

echo.
REM ===================================================================================================
pause
