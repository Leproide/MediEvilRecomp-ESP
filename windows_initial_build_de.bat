@echo off
REM ==================================================================================================
REM MediEvilRecomp DEU - first build (German PAL version, SCES-01493)
REM Based on windows_initial_build_es.bat (https://github.com/Leproide), itself based on
REM BlackLabelHQ's windows_initial_build.bat.
REM Original MediEvilRecomp code (BlackLabelHQ), adapted to the PAL versions.
REM MediEvilRecomp has no explicit license: the rights to this code remain with its
REM authors. This file is NOT released under GPL-3.0 (see the License section of the README).
REM
REM Uses config\de\MediEvil.json, generates the code in generated-de\ and builds with
REM -p:Lingua=de (output in bin-de\). The other versions are not touched.
REM ==================================================================================================
title MediEvilRecomp DEU - Erste Kompilierung

REM =============================================== INIT ==============================================
set "DISC=disc"
set "CONFIG=config\fr"
set "RECOMPONE=RecompOne"
set "RECOMPILER=%RECOMPONE%\RecompOne.Recompiler"
set "MISSING=0"
set "MEDIEVILJSON=%CONFIG%\MediEvil.json"
set "CDFIX=recompone-fixes\cd-whole-sector-mode.patch"
set "GBUFFIX=recompone-fixes\shader-gbuffer.patch"
set "LIGHTFIX=recompone-fixes\effect-lights.patch"
set "PICKFIX=recompone-fixes\material-picker.patch"
set "MODFIX=recompone-fixes\mod-names.patch"
set "GAMEPROJ=RecompOne.MediEvil.csproj"
REM ===================================================================================================

REM ============================================= Credits =============================================
if /I not "%~1"=="nocredits" (
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
)
REM ===================================================================================================

REM ============================== Already built? ===========================================
if exist "generated-de\" (
    echo.
    echo Hinweis:
    choice /m "Das Projekt scheint bereits kompiliert zu sein. Erste Kompilierung wiederholen?"
    if errorlevel 2 (
        echo Abgebrochen.
        echo.
        pause
        exit /b 1
    )
)
REM ===================================================================================================

echo ============================================================================================
echo Abhaengigkeiten herunterladen, RecompOne kompilieren und den Code erzeugen
echo aus config\de\MediEvil.json
echo ============================================================================================

REM ========================================= Sous-modules =============================================
echo.
echo ^> Submodule werden aktualisiert...
git submodule update --init --recursive >nul 2>&1

if errorlevel 1 (
    echo Die Submodule konnten nicht aktualisiert werden. Ohne sie funktioniert das Projekt nicht.
    echo.
    choice /m "Trotzdem fortfahren? Normalerweise nicht, ausser sie sind schon vorhanden."

    if errorlevel 2 (
        echo Abgebrochen.
        echo.
        pause
        exit /b 1
    )

    echo Es wird trotzdem fortgefahren...
)

echo.
REM ===================================================================================================

REM =================================== RecompOne fixes ======================================
REM Applies the fork's fixes to the RecompOne submodule, in this order: CD reader in
REM "whole sector" mode, G-buffer and effect lights for the shaders, material picker, translated mod names.
REM Those already applied are skipped.
echo ^> Korrekturen fuer RecompOne...
for %%P in ("%CDFIX%" "%GBUFFIX%" "%LIGHTFIX%" "%PICKFIX%" "%MODFIX%") do (
    git -C "%RECOMPONE%" apply --reverse --check "..\%%~P" >nul 2>&1
    if errorlevel 1 (
        git -C "%RECOMPONE%" apply "..\%%~P"
        if errorlevel 1 (
            echo %%~P kann nicht angewendet werden. Pruefen Sie, ob das Submodul RecompOne die vorgesehene Version hat.
            pause
            exit /b 1
        )
        echo %%~P angewendet.
    ) else (
        echo %%~P bereits angewendet.
    )
)
echo.
REM ===================================================================================================

REM ======================================== .NET 10 ou plus ==========================================
echo ^> .NET 10 wird geprueft...
dotnet --list-sdks | findstr /r "^10\." >nul

if errorlevel 1 (
    echo.
    echo ============================================================
    echo FEHLER: Das .NET 10 SDK oder neuer wird benoetigt.
    echo Installieren Sie es, bevor Sie fortfahren:
    echo https://dotnet.microsoft.com/download/dotnet/10.0
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Gefunden.
echo.
REM ===================================================================================================

REM ======================================= Compile RecompOne =========================================
echo.
echo ^> RecompOne wird kompiliert...

dotnet build "%RECOMPONE%\RecompOne.sln"

if errorlevel 1 (
    echo.
    echo Die Kompilierung von RecompOne ist fehlgeschlagen. Abbruch.
    pause
    exit /b 1
)

echo Recompiler und Runtime von RecompOne kompiliert.
echo.
REM ===================================================================================================

REM ======================================= Disc files ========================================
echo.
echo ^> Dateien der Disc werden geprueft...

if not exist "%DISC%\MediEvil (Germany) (Track 1).bin" (
    echo Fehlt: MediEvil ^(Germany^) ^(Track 1^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (Germany) (Track 2).bin" (
    echo Fehlt: MediEvil ^(Germany^) ^(Track 2^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (Germany).cue" (
    echo Fehlt: MediEvil ^(Germany^).cue
    set "MISSING=1"
)

if "%MISSING%"=="1" (
    echo.
    echo ============================================================
    echo FEHLER: Dateien der Disc fehlen oder haben einen anderen Namen.
    echo Lesen Sie disc\LIESMICH.md
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Dateien der Disc gefunden.
echo.
REM ===================================================================================================

REM ========================================== Configuration ==========================================
echo.
echo ^> Konfigurationsdatei wird geprueft...

if not exist "%MEDIEVILJSON%" (
    echo.
    echo ============================================================
    echo FEHLER: Die Konfigurationsdatei fehlt.
    echo Erwartet:
    echo %CD%\config\de\MediEvil.json
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Konfiguration gefunden.
echo.

REM ======================================== Generate the C# code ========================================
echo.
echo ^> Rekompilierter C#-Code wird in generated-de\ erzeugt...

dotnet run --project "%RECOMPILER%" "%MEDIEVILJSON%"

if errorlevel 1 (
    echo.
    echo Die Erzeugung des C#-Codes ist fehlgeschlagen.
    pause
    exit /b 1
)

echo.
REM ===================================================================================================

REM ===================================== Compile MediEvilRecomp DEU ==================================
echo.
echo ^> MediEvilRecomp DEU wird kompiliert...

dotnet build "%GAMEPROJ%" -p:Lingua=de

if errorlevel 1 (
    echo.
    echo Die Kompilierung von MediEvilRecomp DEU ist fehlgeschlagen.
    pause
    exit /b 1
)

echo Kompilierung abgeschlossen.
echo.

REM ================================ Shader Dan's Curse V2 (optionnel) ================================
REM Dan's Curse V2: always the latest version of the main branch (git clone --depth 1), in
REM shaders\dans-curse-v2-shader. It is updated every time this script runs: downloaded into
REM a temporary folder, it replaces the old one only if the download succeeds. Shared by all
REM versions: the shader does not depend on the game version.
set "DCDIR=shaders\dans-curse-v2-shader"
set "DCNEW=shaders\dans-curse-v2-shader.new"
echo ^> Neueste Version des Shaders Dan's Curse V2 wird heruntergeladen...
if not exist shaders mkdir shaders
if exist "%DCNEW%" rmdir /s /q "%DCNEW%"
git clone -q --depth 1 https://github.com/Leproide/DansCurseV2 "%DCNEW%" >nul 2>&1
if exist "%DCNEW%\dans-curse-v2.slangp" (
    for /f "delims=" %%c in ('git -C "%DCNEW%" rev-parse --short HEAD') do echo Dan's Curse V2: commit %%c
    attrib -r -h "%DCNEW%\.git\*" /s /d >nul 2>&1
    rmdir /s /q "%DCNEW%\.git"
    if exist "%DCDIR%" rmdir /s /q "%DCDIR%"
    move "%DCNEW%" "%DCDIR%" >nul
    echo Shader installiert: er wird mit dem Mod Shaders unter Mods aktiviert, bei aktivem "Better rendering".
) else (
    if exist "%DCNEW%" rmdir /s /q "%DCNEW%"
    if exist "%DCDIR%\dans-curse-v2.slangp" (
        echo Download nicht moeglich: der bereits installierte Shader bleibt erhalten.
    ) else (
        echo Download nicht moeglich: starten Sie dieses Skript spaeter erneut, um den Shader zu installieren.
    )
)
echo.
REM ===================================================================================================

REM ================================ Shader Gallowmere (par defaut) ===================================
REM Gallowmere (GPL-3.0): the fork's default shader, always the latest version (git clone
REM --depth 1), in shaders\gallowmere-shader. Updated like Dan's Curse V2. The game selects it
REM at the first start if no other shader was chosen.
set "GMDIR=shaders\gallowmere-shader"
set "GMNEW=shaders\gallowmere-shader.new"
echo ^> Neueste Version des Shaders Gallowmere wird heruntergeladen...
if not exist shaders mkdir shaders
if exist "%GMNEW%" rmdir /s /q "%GMNEW%"
git clone -q --depth 1 https://github.com/Leproide/gallowmere-shader "%GMNEW%" >nul 2>&1
if exist "%GMNEW%\gallowmere.slangp" (
    for /f "delims=" %%c in ('git -C "%GMNEW%" rev-parse --short HEAD') do echo Gallowmere: commit %%c
    attrib -r -h "%GMNEW%\.git\*" /s /d >nul 2>&1
    rmdir /s /q "%GMNEW%\.git"
    if exist "%GMDIR%" rmdir /s /q "%GMDIR%"
    move "%GMNEW%" "%GMDIR%" >nul
    echo Shader Gallowmere installiert: er wird beim ersten Start ausgewaehlt.
) else (
    if exist "%GMNEW%" rmdir /s /q "%GMNEW%"
    if exist "%GMDIR%\gallowmere.slangp" (
        echo Download nicht moeglich: der bereits installierte Shader bleibt erhalten.
    ) else (
        echo Download nicht moeglich: starten Sie dieses Skript spaeter erneut, um den Shader zu installieren.
    )
)
echo.
REM ===================================================================================================

REM ======================================= How to start the game  =====================================
echo.
echo Fertig. Spielen Sie mit windows_run_de.bat.
echo.
REM ===================================================================================================

echo.
pause
