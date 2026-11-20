// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - interface texts (English, French, Italian, Spanish, German). The language
// is the one chosen in the launcher, or else the Windows display language; English when it is
// none of these.

using System.Globalization;

namespace MediEvilLauncher;

public static class T
{
    /// <summary>Languages of the launcher: code and name in that language.</summary>
    public static readonly (string Code, string Name)[] Languages =
    {
        ("en", "English"), ("fr", "Français"), ("it", "Italiano"), ("es", "Español"), ("de", "Deutsch"),
    };

    /// <summary>The language used: the one saved in launcher.json, else the Windows one.</summary>
    public static readonly string Lang = Pick(LauncherSettings.Load().Language)
        ?? Pick(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName) ?? "en";

    static string? Pick(string? code) =>
        Array.Exists(Languages, l => l.Code == code) ? code : null;

    static readonly Dictionary<string, string[]> Texts = new()
    {
        //                         en, fr, it, es, de
        ["title"] = new[] { "MediEvil Recomp", "MediEvil Recomp", "MediEvil Recomp", "MediEvil Recomp", "MediEvil Recomp" },
        ["disc"] = new[] { "Game disc (.cue, .chd or .vcd):", "Disque du jeu (.cue, .chd ou .vcd) :", "Disco del gioco (.cue, .chd o .vcd):", "Disco del juego (.cue, .chd o .vcd):", "Spiel-Disc (.cue, .chd oder .vcd):" },
        ["browse"] = new[] { "Browse...", "Parcourir...", "Sfoglia...", "Examinar...", "Durchsuchen..." },
        ["play"] = new[] { "Play", "Jouer", "Gioca", "Jugar", "Spielen" },
        ["build"] = new[] { "Build", "Compiler", "Compila", "Compilar", "Kompilieren" },
        ["rebuild"] = new[] { "Rebuild", "Recompiler", "Ricompila", "Recompilar", "Neu kompilieren" },
        ["update"] = new[] { "Update", "Mettre à jour", "Aggiorna", "Actualizar", "Aktualisieren" },
        ["log"] = new[] { "Show log", "Afficher le journal", "Mostra il log", "Mostrar el registro", "Protokoll anzeigen" },
        ["pick"] = new[] { "Choose your MediEvil disc image.", "Choisissez l'image de votre disque MediEvil.", "Scegli l'immagine del tuo disco di MediEvil.", "Elige la imagen de tu disco de MediEvil.", "Wähle das Image deiner MediEvil-Disc." },
        ["unreadable"] = new[] { "This file cannot be read as a PlayStation disc.", "Ce fichier ne peut pas être lu comme un disque PlayStation.", "Questo file non si legge come un disco PlayStation.", "Este archivo no se puede leer como un disco de PlayStation.", "Diese Datei lässt sich nicht als PlayStation-Disc lesen." },
        ["unknown"] = new[] { "Not a supported MediEvil disc (boot: {0}). Supported: Italy SCES-01494, Spain SCES-01495, France SCES-01492, Germany SCES-01493, Europe (English) SCES-00311, USA SCUS-94227.", "Disque MediEvil non pris en charge (démarrage : {0}). Pris en charge : Italie SCES-01494, Espagne SCES-01495, France SCES-01492, Allemagne SCES-01493, Europe (anglais) SCES-00311, USA SCUS-94227.", "Disco di MediEvil non supportato (avvio: {0}). Supportati: Italia SCES-01494, Spagna SCES-01495, Francia SCES-01492, Germania SCES-01493, Europa (inglese) SCES-00311, USA SCUS-94227.", "Disco de MediEvil no compatible (arranque: {0}). Compatibles: Italia SCES-01494, España SCES-01495, Francia SCES-01492, Alemania SCES-01493, Europa (inglés) SCES-00311, EE. UU. SCUS-94227.", "Keine unterstützte MediEvil-Disc (Start: {0}). Unterstützt: Italien SCES-01494, Spanien SCES-01495, Frankreich SCES-01492, Deutschland SCES-01493, Europa (Englisch) SCES-00311, USA SCUS-94227." },
        ["ver.ita"] = new[] { "MediEvil (Italy) - SCES-01494", "MediEvil (Italie) - SCES-01494", "MediEvil (Italia) - SCES-01494", "MediEvil (Italia) - SCES-01494", "MediEvil (Italien) - SCES-01494" },
        ["ver.esp"] = new[] { "MediEvil (Spain) - SCES-01495", "MediEvil (Espagne) - SCES-01495", "MediEvil (Spagna) - SCES-01495", "MediEvil (España) - SCES-01495", "MediEvil (Spanien) - SCES-01495" },
        ["ver.fra"] = new[] { "MediEvil (France) - SCES-01492", "MediEvil (France) - SCES-01492", "MediEvil (Francia) - SCES-01492", "MediEvil (Francia) - SCES-01492", "MediEvil (Frankreich) - SCES-01492" },
        ["ver.deu"] = new[] { "MediEvil (Germany) - SCES-01493", "MediEvil (Allemagne) - SCES-01493", "MediEvil (Germania) - SCES-01493", "MediEvil (Alemania) - SCES-01493", "MediEvil (Deutschland) - SCES-01493" },
        ["ver.eng"] = new[] { "MediEvil (Europe, English) - SCES-00311", "MediEvil (Europe, anglais) - SCES-00311", "MediEvil (Europa, inglese) - SCES-00311", "MediEvil (Europa, inglés) - SCES-00311", "MediEvil (Europa, Englisch) - SCES-00311" },
        ["ver.usa"] = new[] { "MediEvil (USA) - SCUS-94227 (original BlackLabelHQ project)", "MediEvil (USA) - SCUS-94227 (projet original BlackLabelHQ)", "MediEvil (USA) - SCUS-94227 (progetto originale BlackLabelHQ)", "MediEvil (EE. UU.) - SCUS-94227 (proyecto original BlackLabelHQ)", "MediEvil (USA) - SCUS-94227 (Originalprojekt von BlackLabelHQ)" },
        ["state.ready"] = new[] { "Ready to play.", "Prêt à jouer.", "Pronto per giocare.", "Listo para jugar.", "Bereit zum Spielen." },
        ["state.notbuilt"] = new[] { "Not built yet: the first build takes a few minutes (it generates the game code from your disc).", "Pas encore compilé : la première compilation prend quelques minutes (elle génère le code du jeu depuis votre disque).", "Non ancora compilato: la prima compilazione richiede qualche minuto (genera il codice del gioco dal tuo disco).", "Aún no compilado: la primera compilación tarda unos minutos (genera el código del juego desde tu disco).", "Noch nicht kompiliert: Das erste Kompilieren dauert einige Minuten (der Spielcode wird aus deiner Disc erzeugt)." },
        ["state.stale"] = new[] { "The project changed since the last build: rebuild before playing.", "Le projet a changé depuis la dernière compilation : recompilez avant de jouer.", "Il progetto e' cambiato dall'ultima compilazione: ricompila prima di giocare.", "El proyecto ha cambiado desde la última compilación: recompila antes de jugar.", "Das Projekt hat sich seit dem letzten Kompilieren geändert: vor dem Spielen neu kompilieren." },
        ["state.nousa"] = new[] { "The USA project is not downloaded yet: Build downloads it from BlackLabelHQ.", "Le projet USA n'est pas encore téléchargé : Compiler le télécharge depuis BlackLabelHQ.", "Il progetto USA non e' ancora scaricato: Compila lo scarica da BlackLabelHQ.", "El proyecto USA aún no está descargado: Compilar lo descarga de BlackLabelHQ.", "Das USA-Projekt ist noch nicht heruntergeladen: Kompilieren lädt es von BlackLabelHQ herunter." },
        ["step.tools"] = new[] { "Preparing the build tools (first time only: .NET SDK and Git, downloaded into the launcher's folder)", "Préparation des outils (première fois seulement : SDK .NET et Git, téléchargés dans le dossier du launcher)", "Preparazione degli strumenti (solo la prima volta: SDK .NET e Git, scaricati nella cartella del launcher)", "Preparación de las herramientas (solo la primera vez: SDK de .NET y Git, descargados en la carpeta del launcher)", "Build-Werkzeuge vorbereiten (nur beim ersten Mal: .NET SDK und Git, in den Ordner des Launchers heruntergeladen)" },
        ["need.project"] = new[] { "The PAL project was not found next to the launcher. Download it now into:\n{0}", "Le projet PAL est introuvable à côté du launcher. Le télécharger maintenant dans :\n{0}", "Il progetto PAL non si trova accanto al launcher. Scaricarlo ora in:\n{0}", "No se encuentra el proyecto PAL junto al launcher. ¿Descargarlo ahora en:\n{0}?", "Das PAL-Projekt wurde neben dem Launcher nicht gefunden. Jetzt herunterladen nach:\n{0}" },
        ["step.clone"] = new[] { "Downloading the project", "Téléchargement du projet", "Download del progetto", "Descarga del proyecto", "Projekt wird heruntergeladen" },
        ["step.submodules"] = new[] { "Updating submodules", "Mise à jour des sous-modules", "Aggiornamento dei sottomoduli", "Actualización de los submódulos", "Submodule werden aktualisiert" },
        ["step.fixes"] = new[] { "Applying the RecompOne fixes", "Application des corrections de RecompOne", "Correzioni di RecompOne", "Correcciones de RecompOne", "RecompOne-Korrekturen werden angewendet" },
        ["step.recompone"] = new[] { "Building RecompOne", "Compilation de RecompOne", "Compilazione di RecompOne", "Compilación de RecompOne", "RecompOne wird kompiliert" },
        ["step.generate"] = new[] { "Generating the game code from your disc", "Génération du code du jeu depuis votre disque", "Generazione del codice del gioco dal tuo disco", "Generación del código del juego desde tu disco", "Spielcode wird aus deiner Disc erzeugt" },
        ["step.game"] = new[] { "Building the game", "Compilation du jeu", "Compilazione del gioco", "Compilación del juego", "Spiel wird kompiliert" },
        ["step.shaders"] = new[] { "Downloading the shaders", "Téléchargement des shaders", "Download degli shader", "Descarga de los shaders", "Shader werden heruntergeladen" },
        ["step.pull"] = new[] { "Downloading the updates", "Téléchargement des mises à jour", "Download degli aggiornamenti", "Descarga de las actualizaciones", "Updates werden heruntergeladen" },
        ["done.build"] = new[] { "Build finished.", "Compilation terminée.", "Compilazione terminata.", "Compilación terminada.", "Kompilieren abgeschlossen." },
        ["done.update"] = new[] { "Up to date. Rebuild to play the new version.", "À jour. Recompilez pour jouer à la nouvelle version.", "Aggiornato. Ricompila per giocare la nuova versione.", "Actualizado. Recompila para jugar la nueva versión.", "Auf dem neuesten Stand. Neu kompilieren, um die neue Version zu spielen." },
        ["failed"] = new[] { "Failed: {0}. See the log.", "Échec : {0}. Voir le journal.", "Errore: {0}. Vedi il log.", "Error: {0}. Mira el registro.", "Fehlgeschlagen: {0}. Siehe Protokoll." },
        ["running"] = new[] { "The game is running.", "Le jeu est en cours.", "Il gioco e' in esecuzione.", "El juego está en marcha.", "Das Spiel läuft." },
        ["noshaders"] = new[] { "Shaders could not be downloaded (optional): the game works without them.", "Les shaders n'ont pas pu être téléchargés (facultatif) : le jeu fonctionne sans.", "Shader non scaricati (facoltativi): il gioco funziona anche senza.", "No se han podido descargar los shaders (opcionales): el juego funciona sin ellos.", "Die Shader konnten nicht heruntergeladen werden (optional): Das Spiel funktioniert auch ohne sie." },
        ["settings"] = new[] { "Graphics", "Réglages", "Grafica", "Gráficos", "Grafik" },
        ["saves"] = new[] { "Saves", "Sauvegardes", "Salvataggi", "Partidas", "Spielstände" },
        ["shortcut"] = new[] { "Desktop shortcut", "Raccourci bureau", "Collegamento", "Acceso directo", "Desktop-Verknüpfung" },
        ["shortcut.done"] = new[] { "Shortcut created on the desktop: {0}", "Raccourci créé sur le bureau : {0}", "Collegamento creato sul desktop: {0}", "Acceso directo creado en el escritorio: {0}", "Verknüpfung auf dem Desktop erstellt: {0}" },
        ["converting"] = new[] { "Converting the disc image (once)", "Conversion de l'image du disque (une seule fois)", "Conversione dell'immagine del disco (una volta sola)", "Conversión de la imagen del disco (una sola vez)", "Disc-Image wird umgewandelt (einmalig)" },
        ["variant.pt"] = new[] { "Portuguese translation", "traduction portugaise", "traduzione portoghese", "traducción portuguesa", "portugiesische Übersetzung" },
        ["updates.available"] = new[] { "{0} update(s) available: click Update.", "{0} mise(s) à jour disponible(s) : cliquez sur Mettre à jour.", "{0} aggiornamento/i disponibile/i: clicca Aggiorna.", "{0} actualización(es) disponible(s): pulsa Actualizar.", "{0} Update(s) verfügbar: auf Aktualisieren klicken." },
        ["backup.done"] = new[] { "Saves backed up to {0}", "Sauvegardes copiées dans {0}", "Salvataggi copiati in {0}", "Partidas copiadas en {0}", "Spielstände gesichert in {0}" },
        ["saves.open"] = new[] { "Open the saves folder", "Ouvrir le dossier des sauvegardes", "Apri la cartella dei salvataggi", "Abrir la carpeta de partidas", "Spielstand-Ordner öffnen" },
        ["saves.backup"] = new[] { "Back up the saves now", "Copier les sauvegardes maintenant", "Copia i salvataggi adesso", "Copiar las partidas ahora", "Spielstände jetzt sichern" },
        ["saves.restore"] = new[] { "Restore a backup", "Restaurer une copie", "Ripristina una copia", "Restaurar una copia", "Sicherung wiederherstellen" },
        ["saves.none"] = new[] { "(no backup yet)", "(aucune copie)", "(nessuna copia)", "(ninguna copia)", "(noch keine Sicherung)" },
        ["saves.backups"] = new[] { "Open the backups folder", "Ouvrir le dossier des copies", "Apri la cartella delle copie", "Abrir la carpeta de copias", "Sicherungsordner öffnen" },
        ["saves.nothing"] = new[] { "No saves to back up yet.", "Aucune sauvegarde à copier pour l'instant.", "Ancora nessun salvataggio da copiare.", "Todavía no hay partidas que copiar.", "Noch keine Spielstände zum Sichern." },
        ["saves.confirm"] = new[] { "Put back the memory cards of {0}? The current ones are copied first.", "Remettre les cartes mémoire du {0} ? Les actuelles sont copiées avant.", "Rimettere le memory card del {0}? Quelle attuali vengono copiate prima.", "¿Volver a poner las tarjetas de memoria del {0}? Las actuales se copian antes.", "Memory Cards vom {0} zurückspielen? Die aktuellen werden vorher gesichert." },
        ["saves.restored"] = new[] { "Memory cards of {0} restored.", "Cartes mémoire du {0} restaurées.", "Memory card del {0} ripristinate.", "Tarjetas de memoria del {0} restauradas.", "Memory Cards vom {0} wiederhergestellt." },
        ["backup.short"] = new[] { "(saves backed up)", "(sauvegardes copiées)", "(salvataggi copiati)", "(partidas copiadas)", "(Spielstände gesichert)" },
        ["settings.title"] = new[] { "Graphics settings", "Réglages graphiques", "Impostazioni grafiche", "Ajustes gráficos", "Grafikeinstellungen" },
        ["settings.scale"] = new[] { "Internal resolution", "Résolution interne", "Risoluzione interna", "Resolución interna", "Interne Auflösung" },
        ["settings.fps"] = new[] { "Frame rate", "Images par seconde", "Fotogrammi al secondo", "Imágenes por segundo", "Bildrate" },
        ["settings.fps.native"] = new[] { "Original", "D'origine", "Originale", "Original", "Original" },
        ["settings.fullscreen"] = new[] { "Fullscreen", "Plein écran", "Schermo intero", "Pantalla completa", "Vollbild" },
        ["settings.vsync"] = new[] { "Vertical sync (VSync)", "Synchronisation verticale (VSync)", "Sincronizzazione verticale (VSync)", "Sincronización vertical (VSync)", "Vertikale Synchronisation (VSync)" },
        ["settings.widescreen"] = new[] { "Widescreen", "Écran large", "Panoramico", "Panorámico", "Breitbild" },
        ["settings.note"] = new[] { "The game's own settings, applied at the next start. Everything else stays in the in-game menu.", "Les réglages du jeu lui-même, appliqués au prochain lancement. Le reste se règle dans le menu du jeu.", "Le impostazioni del gioco stesso, applicate al prossimo avvio. Il resto si regola nel menu del gioco.", "Los ajustes del propio juego, aplicados en el próximo arranque. El resto se ajusta en el menú del juego.", "Die Einstellungen des Spiels selbst, beim nächsten Start übernommen. Alles andere stellst du im Menü des Spiels ein." },
        ["cancel"] = new[] { "Cancel", "Annuler", "Annulla", "Cancelar", "Abbrechen" },
        ["nogamedata"] = new[] { "No game data is included: the game code is generated on this computer from your own disc.", "Aucune donnée du jeu n'est incluse : le code du jeu est généré sur cet ordinateur depuis votre propre disque.", "Nessun dato del gioco e' incluso: il codice viene generato su questo computer dal tuo disco.", "No se incluye ningún dato del juego: el código se genera en este ordenador desde tu propio disco.", "Es sind keine Spieldaten enthalten: Der Spielcode wird auf diesem Computer aus deiner eigenen Disc erzeugt." },
        ["language"] = new[] { "Language", "Langue", "Lingua", "Idioma", "Sprache" },
        ["language.auto"] = new[] { "Automatic (Windows)", "Automatique (Windows)", "Automatica (Windows)", "Automático (Windows)", "Automatisch (Windows)" },
    };

    static int Index => Array.FindIndex(Languages, l => l.Code == Lang);

    public static string S(string key) => Texts.TryGetValue(key, out var v) ? v[Index] : key;

    public static string F(string key, params object[] args) => string.Format(S(key), args);
}
