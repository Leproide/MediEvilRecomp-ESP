@echo off
REM ==================================================================================================
REM MediEvilRecomp ENG - first build (English PAL version, SCES-00311)
REM Based on windows_initial_build_es.bat (https://github.com/Leproide), itself based on
REM BlackLabelHQ's windows_initial_build.bat.
REM Original MediEvilRecomp code (BlackLabelHQ), adapted to the PAL versions.
REM MediEvilRecomp has no explicit license: the rights to this code remain with its
REM authors. This file is NOT released under GPL-3.0 (see the License section of the README).
REM
REM Uses config\en\MediEvil.json, generates the code in generated-en\ and builds with
REM -p:Lingua=en (output in bin-en\). The other versions are not touched.
REM ==================================================================================================
title MediEvilRecomp ENG - First build

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
set "GAMEPROJ=RecompOne.MediEvil.csproj"
REM ===================================================================================================

REM ============================================= Credits =============================================
if /I not "%~1"=="nocredits" (
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
)
REM ===================================================================================================

REM ============================== Already built? ===========================================
if exist "generated-en\" (
    echo.
    echo Note:
    choice /m "The project seems to be built already. Run the first build again?"
    if errorlevel 2 (
        echo Cancelled.
        echo.
        pause
        exit /b 1
    )
)
REM ===================================================================================================

echo ============================================================================================
echo Downloading the dependencies, building RecompOne and generating the code
echo from config\en\MediEvil.json
echo ============================================================================================

REM ========================================= Sous-modules =============================================
echo.
echo ^> Updating the submodules...
git submodule update --init --recursive >nul 2>&1

if errorlevel 1 (
    echo Could not update the submodules. The project does not work without them.
    echo.
    choice /m "Continue anyway? Usually not, unless you already have them."

    if errorlevel 2 (
        echo Cancelled.
        echo.
        pause
        exit /b 1
    )

    echo Continuing anyway...
)

echo.
REM ===================================================================================================

REM =================================== RecompOne fixes ======================================
REM Applies the fork's fixes to the RecompOne submodule, in this order: CD reader in
REM "whole sector" mode, G-buffer and effect lights for the shaders, material picker.
REM Those already applied are skipped.
echo ^> RecompOne fixes...
for %%P in ("%CDFIX%" "%GBUFFIX%" "%LIGHTFIX%" "%PICKFIX%") do (
    git -C "%RECOMPONE%" apply --reverse --check "..\%%~P" >nul 2>&1
    if errorlevel 1 (
        git -C "%RECOMPONE%" apply "..\%%~P"
        if errorlevel 1 (
            echo Cannot apply %%~P. Check that the RecompOne submodule is at the expected version.
            pause
            exit /b 1
        )
        echo %%~P applied.
    ) else (
        echo %%~P already applied.
    )
)
echo.
REM ===================================================================================================

REM ======================================== .NET 10 ou plus ==========================================
echo ^> Checking .NET 10...
dotnet --list-sdks | findstr /r "^10\." >nul

if errorlevel 1 (
    echo.
    echo ============================================================
    echo ERROR: the .NET 10 SDK or newer is required.
    echo Install it before continuing:
    echo https://dotnet.microsoft.com/download/dotnet/10.0
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Found.
echo.
REM ===================================================================================================

REM ======================================= Compile RecompOne =========================================
echo.
echo ^> Building RecompOne...

dotnet build "%RECOMPONE%\RecompOne.sln"

if errorlevel 1 (
    echo.
    echo Building RecompOne failed. Cannot continue.
    pause
    exit /b 1
)

echo RecompOne recompiler and runtime built.
echo.
REM ===================================================================================================

REM ======================================= Disc files ========================================
echo.
echo ^> Checking the disc files...

if not exist "%DISC%\MediEvil (Europe) (Track 1).bin" (
    echo Missing: MediEvil ^(Europe^) ^(Track 1^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (Europe) (Track 2).bin" (
    echo Missing: MediEvil ^(Europe^) ^(Track 2^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (Europe).cue" (
    echo Missing: MediEvil ^(Europe^).cue
    set "MISSING=1"
)

if "%MISSING%"=="1" (
    echo.
    echo ============================================================
    echo ERROR: disc files are missing or have another name.
    echo Read disc\README_EN.md
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Disc files found.
echo.
REM ===================================================================================================

REM ========================================== Configuration ==========================================
echo.
echo ^> Checking the configuration file...

if not exist "%MEDIEVILJSON%" (
    echo.
    echo ============================================================
    echo ERROR: the configuration file is missing.
    echo Expected:
    echo %CD%\config\en\MediEvil.json
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Configuration found.
echo.

REM ======================================== Generate the C# code ========================================
echo.
echo ^> Generating the recompiled C# code in generated-en\...

dotnet run --project "%RECOMPILER%" "%MEDIEVILJSON%"

if errorlevel 1 (
    echo.
    echo Generating the C# code failed.
    pause
    exit /b 1
)

echo.
REM ===================================================================================================

REM ===================================== Compile MediEvilRecomp ENG ==================================
echo.
echo ^> Building MediEvilRecomp ENG...

dotnet build "%GAMEPROJ%" -p:Lingua=en

if errorlevel 1 (
    echo.
    echo Building MediEvilRecomp ENG failed.
    pause
    exit /b 1
)

echo Build finished.
echo.

REM ================================ Shader Dan's Curse V2 (optionnel) ================================
REM Dan's Curse V2: always the latest version of the main branch (git clone --depth 1), in
REM shaders\dans-curse-v2-shader. It is updated every time this script runs: downloaded into
REM a temporary folder, it replaces the old one only if the download succeeds. Shared by all
REM versions: the shader does not depend on the game version.
set "DCDIR=shaders\dans-curse-v2-shader"
set "DCNEW=shaders\dans-curse-v2-shader.new"
echo ^> Downloading the latest Dan's Curse V2 shader...
if not exist shaders mkdir shaders
if exist "%DCNEW%" rmdir /s /q "%DCNEW%"
git clone -q --depth 1 https://github.com/Leproide/DansCurseV2 "%DCNEW%" >nul 2>&1
if exist "%DCNEW%\dans-curse-v2.slangp" (
    for /f "delims=" %%c in ('git -C "%DCNEW%" rev-parse --short HEAD') do echo Dan's Curse V2: commit %%c
    attrib -r -h "%DCNEW%\.git\*" /s /d >nul 2>&1
    rmdir /s /q "%DCNEW%\.git"
    if exist "%DCDIR%" rmdir /s /q "%DCDIR%"
    move "%DCNEW%" "%DCDIR%" >nul
    echo Shader installed: turn it on with the Shaders mod in Mods, with "Better rendering" on.
) else (
    if exist "%DCNEW%" rmdir /s /q "%DCNEW%"
    if exist "%DCDIR%\dans-curse-v2.slangp" (
        echo Download failed: the shader already installed is kept.
    ) else (
        echo Download failed: run this script again later to install the shader.
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
echo ^> Downloading the latest Gallowmere shader...
if not exist shaders mkdir shaders
if exist "%GMNEW%" rmdir /s /q "%GMNEW%"
git clone -q --depth 1 https://github.com/Leproide/gallowmere-shader "%GMNEW%" >nul 2>&1
if exist "%GMNEW%\gallowmere.slangp" (
    for /f "delims=" %%c in ('git -C "%GMNEW%" rev-parse --short HEAD') do echo Gallowmere: commit %%c
    attrib -r -h "%GMNEW%\.git\*" /s /d >nul 2>&1
    rmdir /s /q "%GMNEW%\.git"
    if exist "%GMDIR%" rmdir /s /q "%GMDIR%"
    move "%GMNEW%" "%GMDIR%" >nul
    echo Gallowmere shader installed: it is selected at the first start.
) else (
    if exist "%GMNEW%" rmdir /s /q "%GMNEW%"
    if exist "%GMDIR%\gallowmere.slangp" (
        echo Download failed: the shader already installed is kept.
    ) else (
        echo Download failed: run this script again later to install the shader.
    )
)
echo.
REM ===================================================================================================

REM ======================================= How to start the game  =====================================
echo.
echo Done. Play with windows_run_en.bat.
echo.
REM ===================================================================================================

echo.
pause
