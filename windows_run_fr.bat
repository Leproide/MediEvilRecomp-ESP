@echo off
REM ==================================================================================================
REM MediEvilRecomp FRA - lancement (version PAL francaise, SCES-01492)
REM Base sur windows_run_es.bat (https://github.com/Leproide) et windows_run.bat de BlackLabelHQ.
REM Code original de MediEvilRecomp (BlackLabelHQ), adapte aux versions PAL.
REM MediEvilRecomp n'a pas de licence explicite : les droits sur ce code restent a leurs
REM auteurs. Ce fichier n'est PAS publie sous GPL-3.0 (voir la section License du README).
REM ==================================================================================================
title MediEvilRecomp FRA

REM ============================================= Credits =============================================
	echo ========
	echo Credits:
	echo ========
	echo.
	echo Projet original MediEvilRecomp:
	echo.
	echo	BlackLabelHQ:
	echo	- Derp Princess
	echo	- flaffymg
	echo.
	echo Port PAL italien et espagnol:
	echo	- https://github.com/Leproide
	echo.
	echo Version francaise:
	echo	- https://github.com/JasonHababou
	echo.
REM ===================================================================================================

REM ========================================= Lance MediEvilRecomp ====================================
echo.
echo ^> Lancement de MediEvilRecomp FRA...

if not exist "generated-fr\" (
    echo La premiere compilation n'a pas encore ete faite : je la lance maintenant.
    echo.
    echo Lancement de windows_initial_build_fr.bat...
    echo.
    call windows_initial_build_fr.bat nocredits

    REM S'arrete si la compilation a echoue
    if errorlevel 1 (
        echo La premiere compilation a echoue.
        pause
        exit /b 1
    )
)

dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=fr

echo.
REM ===================================================================================================
pause
