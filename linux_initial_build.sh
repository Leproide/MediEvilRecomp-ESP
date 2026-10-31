#!/usr/bin/env bash
# SPDX-License-Identifier: GPL-3.0-or-later
#
# MediEvilRecomp ITA - prima compilazione su Linux (versione PAL italiana, SCES-01494)
# Equivalente di windows_initial_build.bat.
# Copyright (C) 2026
# Author: https://github.com/Leproide
#
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <https://www.gnu.org/licenses/>.

set -uo pipefail

# lavora sempre dalla cartella del repository, ovunque venga lanciato lo script
cd "$(dirname "$(readlink -f "$0")")" || exit 1

DISC="disc"
RECOMPONE="RecompOne"
RECOMPILER="$RECOMPONE/RecompOne.Recompiler"
MEDIEVILJSON="config/MediEvil.json"
CDFIX="recompone-fixes/cd-whole-sector-mode.patch"

say()  { printf '\n\033[1;36m> %s\033[0m\n' "$1"; }
fail() { printf '\n\033[1;31mERRORE: %s\033[0m\n' "$1" >&2; exit 1; }

ask() {
    # domanda s/n, risposta predefinita: no
    local r
    read -r -p "$1 [s/N] " r
    [[ "$r" =~ ^[sSyY]$ ]]
}

if [ "${1:-}" != "nocredits" ]; then
    echo "========"
    echo "Crediti:"
    echo "========"
    echo
    echo "Progetto originale MediEvilRecomp:"
    echo "  BlackLabelHQ: Derp Princess, flaffymg"
    echo
    echo "Port PAL italiano:"
    echo "  https://github.com/Leproide"
    echo
fi

if [ -d "generated" ]; then
    echo "Nota: sembra che il progetto sia gia' stato compilato."
    ask "Vuoi davvero rieseguire la prima compilazione?" || { echo "Annullato."; exit 1; }
fi

# ------------------------------------------------------------------ sottomoduli
say "Aggiorno i sottomoduli..."
command -v git >/dev/null 2>&1 || fail "git non trovato. Installalo con il gestore pacchetti della tua distribuzione."
if ! git submodule update --init --recursive >/dev/null 2>&1; then
    echo "Aggiornamento dei sottomoduli fallito! Senza sottomoduli il progetto non funziona."
    ask "Vuoi continuare lo stesso? Di solito no, a meno che non li avessi gia'." || { echo "Annullato."; exit 1; }
    echo "Continuo comunque..."
fi

# ------------------------------------------------------------------ correzione CD
say "Correzione RecompOne per il boot PAL..."
if git -C "$RECOMPONE" apply --reverse --check "../$CDFIX" >/dev/null 2>&1; then
    echo "Correzione gia' presente."
else
    git -C "$RECOMPONE" apply "../$CDFIX" \
        || fail "impossibile applicare $CDFIX. Controlla che il sottomodulo RecompOne sia alla versione prevista."
    echo "Correzione applicata."
fi

# ------------------------------------------------------------------ .NET
say "Controllo .NET 10..."
if ! command -v dotnet >/dev/null 2>&1 || ! dotnet --list-sdks 2>/dev/null | grep -q '^10\.'; then
    fail "serve l'SDK di .NET 10 o superiore: https://dotnet.microsoft.com/download/dotnet/10.0
Su molte distribuzioni: pacchetto dotnet-sdk-10.0, oppure lo script ufficiale dotnet-install.sh."
fi
echo "Trovato."

say "Compilo RecompOne..."
dotnet build "$RECOMPONE/RecompOne.sln" || fail "compilazione di RecompOne fallita."
echo "Recompiler e runtime di RecompOne compilati."

# ------------------------------------------------------------------ disco
say "Controllo i file del disco..."
MISSING=0
for f in "MediEvil (Italy) (Track 1).bin" "MediEvil (Italy) (Track 2).bin" "MediEvil (Italy).cue"; do
    if [ ! -f "$DISC/$f" ]; then
        echo "Manca: $f"
        MISSING=1
    fi
done
# Linux distingue maiuscole e minuscole: avvisa se i nomi differiscono solo per quello
if [ "$MISSING" = "1" ]; then
    ls "$DISC" 2>/dev/null | grep -i "medievil" | sed 's/^/Trovato nella cartella: /'
    fail "file del disco mancanti o con il nome sbagliato (attenzione a maiuscole e minuscole). Leggi disc/README.md"
fi
echo "File del disco trovati."

# ------------------------------------------------------------------ configurazione
say "Controllo il file di configurazione..."
[ -f "$MEDIEVILJSON" ] || fail "file di configurazione mancante: $PWD/$MEDIEVILJSON"
echo "Configurazione trovata."

# ------------------------------------------------------------------ codice generato
say "Genero il codice C# ricompilato..."
dotnet run --project "$RECOMPILER" "$MEDIEVILJSON" || fail "generazione del codice C# fallita."

say "Compilo MediEvilRecomp..."
dotnet build "$RECOMPONE/RecompOne.sln" || fail "compilazione di MediEvilRecomp fallita."
echo "Compilazione completata."

# ------------------------------------------------------------------ shader Dan's Curse V2 (opzionale)
# Dan's Curse V2 (GPL-3.0, basato su Dan's Curse di flaffymg): ultima versione del ramo main.
# Il commit scaricato viene scritto nel marcatore del Mod Hub (voce dans-curse-v2-shader).
# Viene messo in shaders/ come se fosse installato dal Mod Hub. Se il download fallisce si continua.
DCDIR="shaders/dans-curse-v2-shader"
if [ ! -f "$DCDIR/dans-curse-v2.slangp" ]; then
    say "Scarico lo shader Dan's Curse V2..."
    rm -rf "$DCDIR"
    if git clone -q --depth 1 https://github.com/Leproide/DansCurseV2 "$DCDIR" >/dev/null 2>&1; then
        DCCOMMIT="$(git -C "$DCDIR" rev-parse HEAD)"
        rm -rf "$DCDIR/.git"
        printf '{"Id":"dans-curse-v2-shader","Type":"shader","Commit":"%s","Repository":"https://github.com/Leproide/DansCurseV2"}' "$DCCOMMIT" > "$DCDIR/.modhub"
        echo "Shader installato: si attiva accendendo la mod Shaders in Mods, con \"Better rendering\" acceso."
    else
        rm -rf "$DCDIR"
        echo "Download non riuscito: lo shader si puo' installare piu' tardi dal Mod Hub."
    fi
fi

echo
echo "Fatto! Usa ./linux_run.sh per giocare."
