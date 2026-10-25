@echo off
REM ==================================================================================================
REM MediEvilRecomp ESP - paquetes para la release (Windows x64 y Linux x64)
REM Compila desde Windows los dos paquetes que espera la actualizacion automatica:
REM   publish-es\MediEvilRecomp-ESP-<version>-windows-x64.zip
REM   publish-es\MediEvilRecomp-ESP-<version>-linux-x64.tar.gz   (con run.sh)
REM Hace falta haber ejecutado antes windows_initial_build_es.bat (carpeta generated-es\).
REM Author: https://github.com/Leproide - GPL-3.0-or-later
REM
REM Uso:  windows_publish_es.bat v1.1
REM ==================================================================================================
setlocal
title MediEvilRecomp ESP - Paquetes de release

set "VERSION=%~1"
if "%VERSION%"=="" set "VERSION=dev"
set "OUT=publish-es"
set "NAME=MediEvilRecomp-ESP-%VERSION%"

if not exist "generated-es\" (
    echo Falta generated-es\: ejecuta antes windows_initial_build_es.bat
    pause
    exit /b 1
)

if exist "%OUT%\" rmdir /s /q "%OUT%"
mkdir "%OUT%"

call :publish win-x64 WinExe || goto :error
call :publish linux-x64 Exe || goto :error

REM ---------------------------------------------------------------- Windows: zip
powershell -NoProfile -Command "Compress-Archive -Path '%OUT%\win-x64\*' -DestinationPath '%OUT%\%NAME%-windows-x64.zip' -Force" || goto :error

REM ---------------------------------------------------------------- Linux: tar.gz con run.sh
copy /Y run.sh "%OUT%\linux-x64\run.sh" >nul
tar -czf "%OUT%\%NAME%-linux-x64.tar.gz" -C "%OUT%\linux-x64" . || goto :error

echo.
echo Paquetes listos en %OUT%\:
dir /b "%OUT%\*.zip" "%OUT%\*.tar.gz"
echo.
echo Subelos a la release de GitHub con estos nombres: la actualizacion automatica busca
echo los archivos que terminan en windows-x64.zip y linux-x64.tar.gz.
pause
exit /b 0

:publish
REM %1 = runtime, %2 = OutputType
echo.
echo ^> Compilo %1...
dotnet publish RecompOne.MediEvil.csproj -c Release -p:Lingua=es -r %1 --self-contained false ^
    -p:PublishSingleFile=false -p:OutputType=%2 -p:UseRidGraph=true ^
    -p:InformationalVersion=%VERSION% -p:NoWarn=0162 -o "%OUT%\%1" || exit /b 1
REM mods incluidas en el paquete (sin cache ni archivos git)
robocopy mods "%OUT%\%1\mods" /E /XD .git .cache /XF .git* >nul
if errorlevel 8 exit /b 1
exit /b 0

:error
echo.
echo Error al crear los paquetes.
pause
exit /b 1
