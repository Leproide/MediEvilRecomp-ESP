@echo off
REM MediEvilRecomp FRA - lancement rapide sans recompiler (utile pendant le travail sur les mods).
REM Base sur windows_run_no_build_es.bat (https://github.com/Leproide) et windows_run_no_build.bat
REM de BlackLabelHQ. Code original de MediEvilRecomp (BlackLabelHQ), adapte aux versions PAL.
REM MediEvilRecomp n'a pas de licence explicite : les droits sur ce code restent a leurs
REM auteurs. Ce fichier n'est PAS publie sous GPL-3.0 (voir la section License du README).
dotnet run --project RecompOne.MediEvil.csproj -p:Lingua=fr --no-build
