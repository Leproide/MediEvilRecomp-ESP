// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilRecomp - controllo del disco per le versioni diverse dall'italiana
// Copyright (C) 2026
// Author: https://github.com/Leproide
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

// Le build italiana e spagnola condividono settings.json (percorso del disco) quando
// girano dalla stessa cartella. Il codice ricompilato e' legato al disco da cui e'
// stato generato: avviare la build spagnola con il disco italiano (o viceversa)
// porta a blocchi o crash. Nella build spagnola il disco viene accettato solo se il
// suo SYSTEM.CNF avvia l'eseguibile atteso (SCES_014.95); altrimenti RecompOne
// mostra di nuovo la scelta del disco.
// La build italiana non registra nessun controllo: comportamento invariato.

using System.Text;
using RecompOne.Runtime.Cdrom;

namespace Recompiled;

public static class GameDisc
{
    public static void Register()
    {
        if (!GameVersion.Spanish) return;
        RecompOne.Runtime.Runtime.DiscValidator = Validate;
    }

    // null = disco valido, altrimenti il messaggio da mostrare
    static string? Validate(string path)
    {
        using var fs = DiscFs.Open(path);
        string cnf = Encoding.ASCII.GetString(fs.ReadFile("SYSTEM.CNF"));
        if (cnf.Contains(GameVersion.BootFile, StringComparison.OrdinalIgnoreCase)) return null;
        return $"Disco non valido: serve MediEvil {GameVersion.Serial} (SYSTEM.CNF deve avviare {GameVersion.BootFile}).";
    }
}
