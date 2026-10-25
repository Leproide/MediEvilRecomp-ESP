@echo off
REM ==================================================================================================
REM MediEvilRecomp ITA - prima compilazione (versione PAL italiana, SCES-01494)
REM Basato su windows_initial_build.bat di BlackLabelHQ.
REM Modifiche per il port PAL: Author: https://github.com/Leproide - GPL-3.0-or-later
REM ==================================================================================================
title MediEvilRecomp ITA - Prima compilazione

REM =============================================== INIT ==============================================
set "DISC=disc"
set "CONFIG=config"
set "RECOMPONE=RecompOne"
set "RECOMPILER=%RECOMPONE%\RecompOne.Recompiler"
set "MISSING=0"
set "MEDIEVILJSON=%CONFIG%/MediEvil.json"
set "CDFIX=recompone-fixes\cd-whole-sector-mode.patch"
REM ===================================================================================================

REM ============================================= Crediti =============================================
if /I not "%~1"=="nocredits" (
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
)
REM ===================================================================================================

REM ============================== Compilazione gia' eseguita? =========================================
if exist "generated\" (
    echo.
    echo Nota:
    choice /m "Sembra che il progetto sia gia' stato compilato. Vuoi davvero rieseguire la prima compilazione?"
    if errorlevel 2 (
        echo Annullato.
        echo.
        pause
        exit /b 1
    )
)
REM ===================================================================================================

echo ============================================================================================
echo Scarico le dipendenze, compilo RecompOne e genero il codice da MediEvil.json
echo ============================================================================================

REM ======================================== Sottomoduli ==============================================
echo.
echo ^> Aggiorno i sottomoduli...
git submodule update --init --recursive >nul 2>&1

if errorlevel 1 (
    echo Aggiornamento dei sottomoduli fallito! Senza sottomoduli il progetto non funziona.
    echo.
    choice /m "Vuoi continuare lo stesso? Di solito no, a meno che non li avessi gia'."

    if errorlevel 2 (
        echo Annullato.
        echo.
        pause
        exit /b 1
    )

    echo Continuo comunque...
)

echo.
REM ===================================================================================================

REM =================================== Correzione RecompOne (CD) =====================================
REM Applica la correzione della modalita' "settore intero" del lettore CD al sottomodulo RecompOne.
REM Se e' gia' applicata non fa nulla.
echo ^> Correzione RecompOne per il boot PAL...
git -C "%RECOMPONE%" apply --reverse --check "..\%CDFIX%" >nul 2>&1
if errorlevel 1 (
    git -C "%RECOMPONE%" apply "..\%CDFIX%"
    if errorlevel 1 (
        echo Impossibile applicare %CDFIX%. Controlla che il sottomodulo RecompOne sia alla versione prevista.
        pause
        exit /b 1
    )
    echo Correzione applicata.
) else (
    echo Correzione gia' presente.
)
echo.
REM ===================================================================================================

REM ======================================== .NET 10 o superiore ======================================
echo ^> Controllo .NET 10...
dotnet --list-sdks | findstr /r "^10\." >nul

if errorlevel 1 (
    echo.
    echo ============================================================
    echo ERRORE: serve l'SDK di .NET 10 o superiore!
    echo Installalo prima di continuare:
    echo https://dotnet.microsoft.com/download/dotnet/10.0
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Trovato.
echo.
REM ===================================================================================================

REM ========================================= Compila RecompOne =======================================
echo.
echo ^> Compilo RecompOne...

dotnet build "%RECOMPONE%\RecompOne.sln"

if errorlevel 1 (
    echo.
    echo Compilazione di RecompOne fallita. Impossibile continuare.
    pause
    exit /b 1
)

echo Recompiler e runtime di RecompOne compilati.
echo.
REM ===================================================================================================

REM ======================================== Controllo file del disco =================================
echo.
echo ^> Controllo i file del disco...

if not exist "%DISC%\MediEvil (Italy) (Track 1).bin" (
    echo Manca: MediEvil ^(Italy^) ^(Track 1^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (Italy) (Track 2).bin" (
    echo Manca: MediEvil ^(Italy^) ^(Track 2^).bin
    set "MISSING=1"
)

if not exist "%DISC%\MediEvil (Italy).cue" (
    echo Manca: MediEvil ^(Italy^).cue
    set "MISSING=1"
)

if "%MISSING%"=="1" (
    echo.
    echo ============================================================
    echo ERRORE: file del disco mancanti o con il nome sbagliato.
    echo Leggi disc\README.md
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo File del disco trovati.
echo.
REM ===================================================================================================

REM ========================================== Controllo config =======================================
echo.
echo ^> Controllo il file di configurazione...

if not exist "%MEDIEVILJSON%" (
    echo.
    echo ============================================================
    echo ERRORE: file di configurazione mancante!
    echo Atteso:
    echo %CD%\config\MediEvil.json
    echo ============================================================
    echo.
    pause
    exit /b 1
)

echo Configurazione trovata.
echo.

REM ======================================== Genera il codice C# ======================================
echo.
echo ^> Genero il codice C# ricompilato...

dotnet run --project "%RECOMPILER%" "%MEDIEVILJSON%"

if errorlevel 1 (
    echo.
    echo Generazione del codice C# fallita.
    pause
    exit /b 1
)

echo.
REM ===================================================================================================

REM ========================================= Compila MediEvilRecomp ==================================
echo.
echo ^> Compilo MediEvilRecomp...

dotnet build "%RECOMPONE%\RecompOne.sln"

if errorlevel 1 (
    echo.
    echo Compilazione di MediEvilRecomp fallita.
    pause
    exit /b 1
)

echo Compilazione completata.
echo.
REM ===================================================================================================

REM ======================================== Come avviare il gioco ====================================
echo.
echo Fatto! Usa windows_run.bat per giocare.
echo.
REM ===================================================================================================

echo.
pause
