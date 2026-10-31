@echo off
REM ==================================================================================================
REM MediEvilRecomp ESP - primera compilacion (version PAL espanola, SCES-01495)
REM Basado en windows_initial_build.bat de BlackLabelHQ.
REM Codigo original de MediEvilRecomp (BlackLabelHQ), adaptado a las versiones PAL.
REM MediEvilRecomp no tiene una licencia explicita: los derechos sobre este codigo siguen siendo
REM de sus autores. Este archivo NO se publica bajo GPL-3.0: la GPL del fork cubre solo los
REM archivos escritos desde cero para el port (ver la seccion License del README).
REM Cambios para el port PAL espanol: https://github.com/Leproide
REM
REM Usa config\es\MediEvil.json, genera el codigo en generated-es\ y compila con -p:Lingua=es
REM (salida en bin-es\). La version italiana (config\MediEvil.json, generated\, bin\) no se toca.
REM ==================================================================================================
title MediEvilRecomp ESP - Primera compilacion

REM =============================================== INIT ==============================================
set "DISC=disc"
set "CONFIG=config\es"
set "RECOMPONE=RecompOne"
set "RECOMPILER=%RECOMPONE%\RecompOne.Recompiler"
set "MISSING=0"
set "MEDIEVILJSON=%CONFIG%\MediEvil.json"
set "CDFIX=recompone-fixes\cd-whole-sector-mode.patch"
set "GAMEPROJ=RecompOne.MediEvil.csproj"
REM ===================================================================================================

REM ============================================= Creditos ============================================
if /I not "%~1"=="nocredits" (
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
)
REM ===================================================================================================

REM ============================== Compilacion ya hecha? ==============================================
if exist "generated-es\" (
    echo.
    echo Nota:
    choice /m "Parece que el proyecto ya esta compilado. Quieres repetir la primera compilacion?"
    if errorlevel 2 (
        echo Cancelado.
        echo.
        pause
        exit /b 1
    )
)
REM ===================================================================================================

echo ============================================================================================
echo Descargo las dependencias, compilo RecompOne y genero el codigo desde config\es\MediEvil.json
echo ============================================================================================

REM ========================================= Submodulos ==============================================
echo.
echo ^> Actualizo los submodulos...
git submodule update --init --recursive >nul 2>&1

if errorlevel 1 (
    echo No se han podido actualizar los submodulos. Sin ellos el proyecto no funciona.
    echo.
    choice /m "Quieres continuar de todos modos? Normalmente no, salvo que ya los tuvieras."

    if errorlevel 2 (
        echo Cancelado.
        echo.
        pause
        exit /b 1
    )

    echo Continuo de todos modos...
)

echo.
REM ===================================================================================================

REM =================================== Correccion RecompOne (CD) =====================================
REM Aplica al submodulo RecompOne la correccion del modo "sector entero" del lector de CD.
REM Si ya esta aplicada no hace nada.
echo ^> Correccion de RecompOne para el arranque PAL...
git -C "%RECOMPONE%" apply --reverse --check "..\%CDFIX%" >nul 2>&1
if errorlevel 1 (
    git -C "%RECOMPONE%" apply "..\%CDFIX%"
    if errorlevel 1 (
        echo No se puede aplicar %CDFIX%. Comprueba que el submodulo RecompOne este en la version prevista.
        pause
        exit /b 1
    )
    echo Correccion aplicada.
) else (
    echo La correccion ya esta aplicada.
)
echo.
REM ===================================================================================================

REM ======================================== .NET 10 o superior =======================================
echo ^> Compruebo .NET 10...
dotnet --list-sdks | findstr /r "^10\." >nul

if errorlevel 1 (
    echo.
    echo ============================================================
    echo ERROR: hace falta el SDK de .NET 10 o superior.
    echo Instalalo antes de continuar:
    echo https://dotnet.microsoft.com/download/dotnet/10.0
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Encontrado.
echo.
REM ===================================================================================================

REM ========================================= Compila RecompOne =======================================
echo.
echo ^> Compilo RecompOne...

dotnet build "%RECOMPONE%\RecompOne.sln"

if errorlevel 1 (
    echo.
    echo La compilacion de RecompOne ha fallado. No se puede continuar.
    pause
    exit /b 1
)

echo Recompilador y runtime de RecompOne compilados.
echo.
REM ===================================================================================================

REM ======================================= Archivos del disco ========================================
echo.
echo ^> Compruebo los archivos del disco...

if not exist "%DISC%\MediEvil (Spain) (Track 1).bin" (
    echo Falta: MediEvil ^(Spain^) ^(Track 1^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (Spain) (Track 2).bin" (
    echo Falta: MediEvil ^(Spain^) ^(Track 2^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (Spain).cue" (
    echo Falta: MediEvil ^(Spain^).cue
    set "MISSING=1"
)

if "%MISSING%"=="1" (
    echo.
    echo ============================================================
    echo ERROR: faltan archivos del disco o tienen otro nombre.
    echo Lee disc\LEEME.md
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Archivos del disco encontrados.
echo.
REM ===================================================================================================

REM ========================================== Configuracion ==========================================
echo.
echo ^> Compruebo el archivo de configuracion...

if not exist "%MEDIEVILJSON%" (
    echo.
    echo ============================================================
    echo ERROR: falta el archivo de configuracion.
    echo Esperado:
    echo %CD%\config\es\MediEvil.json
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Configuracion encontrada.
echo.

REM ======================================== Genera el codigo C# ======================================
echo.
echo ^> Genero el codigo C# recompilado en generated-es\...

dotnet run --project "%RECOMPILER%" "%MEDIEVILJSON%"

if errorlevel 1 (
    echo.
    echo La generacion del codigo C# ha fallado.
    pause
    exit /b 1
)

echo.
REM ===================================================================================================

REM ===================================== Compila MediEvilRecomp ESP ==================================
echo.
echo ^> Compilo MediEvilRecomp ESP...

dotnet build "%GAMEPROJ%" -p:Lingua=es

if errorlevel 1 (
    echo.
    echo La compilacion de MediEvilRecomp ESP ha fallado.
    pause
    exit /b 1
)

echo Compilacion terminada.
echo.

REM ================================ Shader Dan's Curse V2 (opcional) =================================
REM Dan's Curse V2: ultima version de la rama main, en shaders\ como si estuviera instalado desde
REM el Mod Hub (el commit descargado se escribe en el marcador .modhub). Comun a ITA y ESP: el
REM shader no depende de la version del juego. Si la descarga falla, se continua.
set "DCDIR=shaders\dans-curse-v2-shader"
if not exist "%DCDIR%\dans-curse-v2.slangp" (
    echo ^> Descargo el shader Dan's Curse V2...
    if exist "%DCDIR%" rmdir /s /q "%DCDIR%"
    git clone -q --depth 1 https://github.com/Leproide/DansCurseV2 "%DCDIR%" >nul 2>&1
    if exist "%DCDIR%\.git" (
        for /f "delims=" %%c in ('git -C "%DCDIR%" rev-parse HEAD') do >"%DCDIR%\.modhub" echo {"Id":"dans-curse-v2-shader","Type":"shader","Commit":"%%c","Repository":"https://github.com/Leproide/DansCurseV2"}
        attrib -r -h "%DCDIR%\.git\*" /s /d >nul 2>&1
        rmdir /s /q "%DCDIR%\.git"
        echo Shader instalado: se activa encendiendo la mod Shaders en Mods, con "Better rendering" activo.
    ) else (
        echo No se ha podido descargar: vuelve a ejecutar este script mas tarde para instalar el shader.
    )
)
echo.
REM ===================================================================================================

REM ======================================== Como iniciar el juego ====================================
echo.
echo Listo. Usa windows_run_es.bat para jugar.
echo.
REM ===================================================================================================

echo.
pause
