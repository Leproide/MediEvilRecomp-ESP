@echo off
REM ==================================================================================================
REM MediEvilRecomp ENG - start (English PAL version, SCES-00311)
REM Based on windows_run_es.bat (https://github.com/Leproide) and BlackLabelHQ's windows_run.bat.
REM Original MediEvilRecomp code (BlackLabelHQ), adapted to the PAL versions.
REM MediEvilRecomp has no explicit license: the rights to this code remain with its
REM authors. This file is NOT released under GPL-3.0 (see the License section of the README).
REM ==================================================================================================
title MediEvilRecomp ENG

REM ============================================= Credits =============================================
	echo ========
	echo Credits:
	echo ========
	echo.
	echo Original MediEvilRecomp project:
	echo.
	echo	BlackLabelHQ:
	echo	- Derp Princess
	echo	- flaffymg
	echo.
	echo Italian and Spanish PAL port:
	echo	- https://github.com/Leproide
	echo.
	echo French, German and English versions:
	echo	- https://github.com/JasonHababou
	echo.
REM ===================================================================================================

REM ========================================= Lance MediEvilRecomp ====================================
echo.
echo ^> Starting MediEvilRecomp ENG...

if not exist "generated-en\" (
    echo The first build has not been done yet: starting it now.
    echo.
    echo Starting windows_initial_build_en.bat...
    echo.
    call windows_initial_build_en.bat nocredits

    REM Stop if the build failed
    if errorlevel 1 (
        echo The first build failed.
        pause
        exit /b 1
    )
)

dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=en

echo.
REM ===================================================================================================
pause
