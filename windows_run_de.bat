@echo off
REM ==================================================================================================
REM MediEvilRecomp DEU - start (German PAL version, SCES-01493)
REM Based on windows_run_es.bat (https://github.com/Leproide) and BlackLabelHQ's windows_run.bat.
REM Original MediEvilRecomp code (BlackLabelHQ), adapted to the PAL versions.
REM MediEvilRecomp has no explicit license: the rights to this code remain with its
REM authors. This file is NOT released under GPL-3.0 (see the License section of the README).
REM ==================================================================================================
title MediEvilRecomp DEU

REM ============================================= Credits =============================================
	echo ========
	echo Credits:
	echo ========
	echo.
	echo Originalprojekt MediEvilRecomp:
	echo.
	echo	BlackLabelHQ:
	echo	- Derp Princess
	echo	- flaffymg
	echo.
	echo Italienischer und spanischer PAL-Port:
	echo	- https://github.com/Leproide
	echo.
	echo Franzoesische, deutsche und englische Version:
	echo	- https://github.com/JasonHababou
	echo.
REM ===================================================================================================

REM ========================================= Lance MediEvilRecomp ====================================
echo.
echo ^> MediEvilRecomp DEU wird gestartet...

if not exist "generated-de\" (
    echo Die erste Kompilierung wurde noch nicht durchgefuehrt: sie wird jetzt gestartet.
    echo.
    echo windows_initial_build_de.bat wird gestartet...
    echo.
    call windows_initial_build_de.bat nocredits

    REM Stop if the build failed
    if errorlevel 1 (
        echo Die erste Kompilierung ist fehlgeschlagen.
        pause
        exit /b 1
    )
)

dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=de

echo.
REM ===================================================================================================
pause
