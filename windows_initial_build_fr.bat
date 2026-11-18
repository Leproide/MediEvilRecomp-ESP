@echo off
REM ==================================================================================================
REM MediEvilRecomp FRA - premiere compilation (version PAL francaise, SCES-01492)
REM Base sur windows_initial_build_es.bat (https://github.com/Leproide), lui-meme base sur
REM windows_initial_build.bat de BlackLabelHQ.
REM Code original de MediEvilRecomp (BlackLabelHQ), adapte aux versions PAL.
REM MediEvilRecomp n'a pas de licence explicite : les droits sur ce code restent a leurs
REM auteurs. Ce fichier n'est PAS publie sous GPL-3.0 (voir la section License du README).
REM
REM Utilise config\fr\MediEvil.json, genere le code dans generated-fr\ et compile avec
REM -p:Lingua=fr (sortie dans bin-fr\). Les versions italienne et espagnole ne sont pas touchees.
REM ==================================================================================================
title MediEvilRecomp FRA - Premiere compilation

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
)
REM ===================================================================================================

REM ============================== Compilation deja faite ? ===========================================
if exist "generated-fr\" (
    echo.
    echo Note:
    choice /m "Le projet semble deja compile. Refaire la premiere compilation ?"
    if errorlevel 2 (
        echo Annule.
        echo.
        pause
        exit /b 1
    )
)
REM ===================================================================================================

echo ============================================================================================
echo Telechargement des dependances, compilation de RecompOne et generation du code
echo depuis config\fr\MediEvil.json
echo ============================================================================================

REM ========================================= Sous-modules =============================================
echo.
echo ^> Mise a jour des sous-modules...
git submodule update --init --recursive >nul 2>&1

if errorlevel 1 (
    echo Impossible de mettre a jour les sous-modules. Sans eux le projet ne fonctionne pas.
    echo.
    choice /m "Continuer quand meme ? Normalement non, sauf s'ils sont deja la."

    if errorlevel 2 (
        echo Annule.
        echo.
        pause
        exit /b 1
    )

    echo On continue quand meme...
)

echo.
REM ===================================================================================================

REM =================================== Corrections de RecompOne ======================================
REM Applique au sous-module RecompOne les corrections du fork, dans cet ordre : lecteur CD en mode
REM "secteur entier", G-buffer et lumieres des effets pour les shaders, selecteur de materiaux.
REM Celles deja appliquees sont ignorees.
echo ^> Corrections de RecompOne...
for %%P in ("%CDFIX%" "%GBUFFIX%" "%LIGHTFIX%" "%PICKFIX%") do (
    git -C "%RECOMPONE%" apply --reverse --check "..\%%~P" >nul 2>&1
    if errorlevel 1 (
        git -C "%RECOMPONE%" apply "..\%%~P"
        if errorlevel 1 (
            echo Impossible d'appliquer %%~P. Verifiez que le sous-module RecompOne est a la version prevue.
            pause
            exit /b 1
        )
        echo %%~P appliquee.
    ) else (
        echo %%~P deja appliquee.
    )
)
echo.
REM ===================================================================================================

REM ======================================== .NET 10 ou plus ==========================================
echo ^> Verification de .NET 10...
dotnet --list-sdks | findstr /r "^10\." >nul

if errorlevel 1 (
    echo.
    echo ============================================================
    echo ERREUR : il faut le SDK .NET 10 ou plus recent.
    echo Installez-le avant de continuer :
    echo https://dotnet.microsoft.com/download/dotnet/10.0
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Trouve.
echo.
REM ===================================================================================================

REM ======================================= Compile RecompOne =========================================
echo.
echo ^> Compilation de RecompOne...

dotnet build "%RECOMPONE%\RecompOne.sln"

if errorlevel 1 (
    echo.
    echo La compilation de RecompOne a echoue. Impossible de continuer.
    pause
    exit /b 1
)

echo Recompilateur et runtime de RecompOne compiles.
echo.
REM ===================================================================================================

REM ======================================= Fichiers du disque ========================================
echo.
echo ^> Verification des fichiers du disque...

if not exist "%DISC%\MediEvil (France) (Track 1).bin" (
    echo Manquant : MediEvil ^(France^) ^(Track 1^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (France) (Track 2).bin" (
    echo Manquant : MediEvil ^(France^) ^(Track 2^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (France).cue" (
    echo Manquant : MediEvil ^(France^).cue
    set "MISSING=1"
)

if "%MISSING%"=="1" (
    echo.
    echo ============================================================
    echo ERREUR : des fichiers du disque manquent ou ont un autre nom.
    echo Lisez disc\LISEZMOI.md
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Fichiers du disque trouves.
echo.
REM ===================================================================================================

REM ========================================== Configuration ==========================================
echo.
echo ^> Verification du fichier de configuration...

if not exist "%MEDIEVILJSON%" (
    echo.
    echo ============================================================
    echo ERREUR : le fichier de configuration manque.
    echo Attendu :
    echo %CD%\config\fr\MediEvil.json
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Configuration trouvee.
echo.

REM ======================================== Genere le code C# ========================================
echo.
echo ^> Generation du code C# recompile dans generated-fr\...

dotnet run --project "%RECOMPILER%" "%MEDIEVILJSON%"

if errorlevel 1 (
    echo.
    echo La generation du code C# a echoue.
    pause
    exit /b 1
)

echo.
REM ===================================================================================================

REM ===================================== Compile MediEvilRecomp FRA ==================================
echo.
echo ^> Compilation de MediEvilRecomp FRA...

dotnet build "%GAMEPROJ%" -p:Lingua=fr

if errorlevel 1 (
    echo.
    echo La compilation de MediEvilRecomp FRA a echoue.
    pause
    exit /b 1
)

echo Compilation terminee.
echo.

REM ================================ Shader Dan's Curse V2 (optionnel) ================================
REM Dan's Curse V2 : toujours la derniere version de la branche main (git clone --depth 1), dans
REM shaders\dans-curse-v2-shader. Il est mis a jour a chaque execution de ce script : telecharge dans
REM un dossier temporaire, il ne remplace l'ancien que si le telechargement reussit. Commun a toutes
REM les versions : le shader ne depend pas de la version du jeu.
set "DCDIR=shaders\dans-curse-v2-shader"
set "DCNEW=shaders\dans-curse-v2-shader.new"
echo ^> Telechargement de la derniere version du shader Dan's Curse V2...
if not exist shaders mkdir shaders
if exist "%DCNEW%" rmdir /s /q "%DCNEW%"
git clone -q --depth 1 https://github.com/Leproide/DansCurseV2 "%DCNEW%" >nul 2>&1
if exist "%DCNEW%\dans-curse-v2.slangp" (
    for /f "delims=" %%c in ('git -C "%DCNEW%" rev-parse --short HEAD') do echo Dan's Curse V2: commit %%c
    attrib -r -h "%DCNEW%\.git\*" /s /d >nul 2>&1
    rmdir /s /q "%DCNEW%\.git"
    if exist "%DCDIR%" rmdir /s /q "%DCDIR%"
    move "%DCNEW%" "%DCDIR%" >nul
    echo Shader installe : il s'active avec le mod Shaders dans Mods, avec "Better rendering" actif.
) else (
    if exist "%DCNEW%" rmdir /s /q "%DCNEW%"
    if exist "%DCDIR%\dans-curse-v2.slangp" (
        echo Telechargement impossible : le shader deja installe est conserve.
    ) else (
        echo Telechargement impossible : relancez ce script plus tard pour installer le shader.
    )
)
echo.
REM ===================================================================================================

REM ================================ Shader Gallowmere (par defaut) ===================================
REM Gallowmere (GPL-3.0) : shader par defaut du fork, toujours la derniere version (git clone
REM --depth 1), dans shaders\gallowmere-shader. Mis a jour comme Dan's Curse V2. Le jeu le
REM selectionne au premier lancement si aucun autre shader n'a ete choisi.
set "GMDIR=shaders\gallowmere-shader"
set "GMNEW=shaders\gallowmere-shader.new"
echo ^> Telechargement de la derniere version du shader Gallowmere...
if not exist shaders mkdir shaders
if exist "%GMNEW%" rmdir /s /q "%GMNEW%"
git clone -q --depth 1 https://github.com/Leproide/gallowmere-shader "%GMNEW%" >nul 2>&1
if exist "%GMNEW%\gallowmere.slangp" (
    for /f "delims=" %%c in ('git -C "%GMNEW%" rev-parse --short HEAD') do echo Gallowmere: commit %%c
    attrib -r -h "%GMNEW%\.git\*" /s /d >nul 2>&1
    rmdir /s /q "%GMNEW%\.git"
    if exist "%GMDIR%" rmdir /s /q "%GMDIR%"
    move "%GMNEW%" "%GMDIR%" >nul
    echo Shader Gallowmere installe : il est selectionne au premier lancement.
) else (
    if exist "%GMNEW%" rmdir /s /q "%GMNEW%"
    if exist "%GMDIR%\gallowmere.slangp" (
        echo Telechargement impossible : le shader deja installe est conserve.
    ) else (
        echo Telechargement impossible : relancez ce script plus tard pour installer le shader.
    )
)
echo.
REM ===================================================================================================

REM ======================================= Comment lancer le jeu =====================================
echo.
echo Termine. Utilisez windows_run_fr.bat pour jouer.
echo.
REM ===================================================================================================

echo.
pause
