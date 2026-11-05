# MediEvilRecomp ESP v1.1.15 (alineada con MediEvilRecomp-ITA 1.1.15)

- **Runa en la mano corregida de lejos**: las manos que sostienen las runas (tipo `800D73A8` en la versión española, `800D73C0` en la italiana) mostraban de lejos la runa ya colocada antes de ponerla. Ya no se precargan y de lejos solo se ven en el estado en que el juego las mostró por última vez de cerca (`patches/render/DistantObjectsPatch.cs`).
- **Cofres ya abiertos**: de lejos se veían cerrados (tipo `800D5274`, en la italiana `800D528C`); ahora se quedan como el juego los mostró por última vez de cerca.
- Los objetos lejanos se cargan al menos hasta donde se ve el terreno (`TerrainPatch.CurrentViewDistance`): en algunos niveles las manos vacías aparecían tarde.
- **Panel de diagnóstico de los objetos lejanos** (solo con `MEDIEVIL_DEBUG`, en Misc → Quality of life): lista los tipos de objetos del nivel y, al marcar uno, lo oculta al instante a cualquier distancia, sin reiniciar, para identificar objetos en pantalla y reportar fallos. Explicado en el README.
- **Nueva opción Map edge fade** (Misc → Quality of life, desactivada por defecto): el suelo cerca del vacío en los bordes del mapa se funde a negro según la distancia al borde, en vez de terminar en escalones (`patches/render/TerrainEdgeFade.cs`). Oscurece también objetos cercanos al borde y algunas zonas (por ejemplo el cementerio), por eso viene apagada.
- Shader Dan's Curse V2 actualizado aparte: nuevas opciones *Disable fog* y *Keep game darkness in fog*, y el vacío más allá de los bordes del mapa vuelve a ser negro en vez de gris. Se descarga la última versión al volver a ejecutar `windows_initial_build_es.bat`.
- Direcciones españolas nuevas en `config/versions.json`: tipos de la runa y de los cofres (0x800D73A8 / 0x800D5274) y punteros a los polígonos y vértices del terreno (0x800EDE54 / 0x800EDE58). Solo cambia código C#: basta con volver a compilar.
- Recordatorio: si usas trucos para saltarte partes de la historia, la Aldea Durmiente puede cerrarse. No es un fallo del recomp (ver la sección Trucos del README).

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.14 (alineada con MediEvilRecomp-ITA 1.1.14)

- **RecompOne actualizado a la versión de MediEvilRecomp 1.0.3** (`b18bec0`, antes `cc18281`). Correcciones incluidas:
  - packs de texturas: el área de VRAM dibujada se calculaba mal y algunas texturas sustitutas se asociaban a la textura equivocada;
  - packs de texturas: al entrar en un nivel que empieza con un vídeo las texturas sustitutas no se cargaban bien;
  - audio XA sustituido: reproducción y búsqueda más fiables, y el lector de CD ya no llena el búfer XA durante una búsqueda;
  - selector de archivos (disco, packs de texturas): en Windows se abre en otro hilo y ya no bloquea la ventana, en Linux con Wayland ya no se cierra;
  - pequeñas correcciones en el editor de packs de texturas y en la cámara de los shaders.
- **Nueva opción Disable camera collision** (Misc → Quality of life, de la 1.0.3 original): con la cámara libre, la cámara ya no se acerca a Dan cuando hay un muro o una valla en medio. Desactivada por defecto; las correcciones del fork a la cámara se mantienen.
- **Barra de menús oculta automáticamente: se queda la del fork.** RecompOne tiene ahora su propia versión (la barra aparece llevando el ratón a la parte de arriba de la pantalla) sobre el mismo ajuste; el fork usa un ajuste propio y deja apagado el de RecompOne. Quien tenía la opción activada la encuentra activada.
- La corrección del lector de CD del fork (`recompone-fixes/`) se aplica sin cambios al nuevo RecompOne.
- Después de actualizar: `git submodule update --init` (o vuelve a ejecutar `windows_initial_build_es.bat`) para bajar el nuevo RecompOne. El código generado no cambia.

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.13 (alineada con MediEvilRecomp-ITA 1.1.13)

- **Objetos visibles desde lejos de nuevo activos por defecto** (Misc → Quality of life → Distant objects), con la precarga para todos los objetos como antes de la v1.1.12. El cierre en la Aldea Durmiente no lo causaban las modificaciones del fork (pasa también con los objetos lejanos desactivados): lo provoca saltarse una parte de la historia con los trucos. El nivel carga los modelos por grupos a medida que avanza la historia; si te saltas esa parte, el grupo 426 nunca se carga y, al entrar por la chimenea de la casa después de poner la cruz en la iglesia, el juego busca el modelo 429, que no está en memoria. Se explica en la sección Trucos del README. Si habías desactivado la opción, vuelve a activarla en Misc → Quality of life → Distant objects.
- Se mantienen los 2 MB de memoria extra y la protección por memoria casi agotada de la v1.1.12, y los muros destructibles que se mantienen despiertos.
- Solo cambia código C#: basta con volver a compilar (`windows_run_es.bat`), no hace falta regenerar el código.

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.12 (alineada con MediEvilRecomp-ITA 1.1.12)

- **Objetos visibles desde lejos desactivados por defecto** (Misc → Quality of life → Distant objects): con la opción activa la Aldea Durmiente puede cerrarse de forma inesperada, porque el juego inicializa un objeto del nivel cargado mucho antes de lo normal. Está en análisis; las correcciones siguen en el código y la opción se puede volver a activar bajo tu responsabilidad. Quien ya la había cambiado en los ajustes mantiene su elección.
- **Corregido el cierre por memoria agotada** (por ejemplo en los Campos de Espantapájaros con Draw distance alta): los objetos cargados desde lejos llenaban la memoria del juego (poco más de 100 KB libres) y el juego se cerraba en puntos distintos. Ahora el juego tiene 2 MB más de memoria: la memoria emulada pasa de 4 a 8 MB y el bloque se añade al gestor de memoria del juego al arrancar (`patches/HeapExpansion.cs`; listas libres en la dirección española 0x800F3B80). Los controles de direcciones de los parches usan toda la memoria.
- Precarga de los objetos lejanos limitada a los objetos de escena (puertas, verjas, decorados): los personajes del nivel ya no se despiertan durante un frame desde lejos y siguen invisibles de lejos como en el original.
- Protección: si la memoria libre baja aun así de 96 KB, los objetos lejanos se suspenden hasta que vuelve a superar los 256 KB (`patches/render/DistantObjectsPatch.cs`).
- Nuevos `windows_run_debug_es.bat` y `windows_run_debug_no_build_es.bat`: arrancan con `MEDIEVIL_DEBUG=1` y guardan el log en `debug.txt`.
- Después de actualizar hay que regenerar el código, porque cambia `config/es/MediEvil.json`: ejecuta `windows_initial_build_es.bat` y responde **S** a la pregunta de repetir la primera compilación (Linux: `./linux_initial_build_es.sh`).

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.11 (alineada con MediEvilRecomp-ITA 1.1.11)

- **Muros destructibles visibles desde lejos**: el juego solo los dibuja cuando están "despiertos", así que de lejos (Draw distance alta, shaders o cámara libre) desaparecían en cualquier estado y dejaban ver la sala de detrás. Ahora, dentro de la distancia ampliada, siguen despiertos y visibles; los golpes y la rotura funcionan como antes (`patches/render/DistantObjectsPatch.cs`, de la versión italiana; nueva dirección española de la lista de objetos despiertos 0x800EEB60 en `config/versions.json`).
- Diagnóstico de los objetos lejanos más completo con `MEDIEVIL_DEBUG`; el README explica cómo funciona la carga de los objetos lejanos y cómo encontrar el tipo de un objeto que desaparece.
- Solo cambia código C#: basta con volver a compilar (`windows_run_es.bat`), no hace falta regenerar el código.

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.10 (alineada con MediEvilRecomp-ITA 1.1.10)

- **Objetos visibles desde lejos**: ahora también las antorchas se dibujan desde lejos en lugar de desaparecer; de lejos están apagadas y se encienden al acercarse, como en el original (`patches/render/DistantObjectsPatch.cs`, de la versión italiana).
- Solo cambia código C#: basta con volver a compilar (`windows_run_es.bat`), no hace falta regenerar el código.

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.9 (alineada con MediEvilRecomp-ITA 1.1.9)

- **Objetos visibles desde lejos**: también con **Draw distance** a 1x (útil con los shaders, donde la niebla deja ver lejos), puertas, verjas y decorados se cargan y se dibujan desde lejos en lugar de aparecer solo cerca de Dan (por ejemplo, las hojas de la verja del cementerio, que de lejos parecían abiertas o faltaban). Vale con cualquier cámara. Enemigos, trampas y eventos se activan a la distancia original; de lejos los objetos quedan quietos y las antorchas, como en el original, solo aparecen al acercarse. Activo por defecto, se desactiva en **Misc → Quality of life → Distant objects** (`patches/render/DistantObjectsPatch.cs`, de la versión italiana, con las direcciones españolas 0x800EEAC8 / 0x800EEBF8 / 0x800EEC0C en `config/versions.json`).
- Después de actualizar hay que regenerar el código, porque cambia `config/es/MediEvil.json`: ejecuta `windows_initial_build_es.bat` y responde **S** a la pregunta de repetir la primera compilación (Linux: `./linux_initial_build_es.sh`).

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.8 (alineada con MediEvilRecomp-ITA 1.1.8)

- El cursor del ratón desaparece tras 2,5 segundos sin moverlo (también a pantalla completa y jugando con el mando) y vuelve a aparecer al moverlo. Sigue visible con menús o ventanas abiertos y sobre la barra de menús.
- **System → Autohide Menu Bar** ahora funciona (antes era una opción desactivada): la barra de menús se oculta tras 3 segundos sin mover el ratón y vuelve a aparecer al moverlo. F1 sigue mostrándola u ocultándola a mano (`misc/InterfaceAutoHide.cs`, de la versión italiana; respuesta a la issue 17 de BlackLabelHQ/MediEvilRecomp).

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.7 (alineada con MediEvilRecomp-ITA 1.1.7)

- Cámara libre con el stick derecho **desactivada por defecto**: se juega con la cámara original. Se vuelve a activar en **Misc → Quality of life → Right stick camera** (en español: **Varios → Calidad de vida → Cámara con el stick derecho**). Quien ya la había cambiado en los ajustes mantiene su elección.
- **R3** (pulsar el stick derecho) activa y desactiva la cámara libre en cualquier momento, con un aviso en pantalla; la elección se guarda (`patches/qol/CameraToggle.cs`).
- Corregida la distancia de dibujado pasada a los shaders (`patches/render/ShaderDrawDistance.cs`): la versión anterior se actualizaba con el evento VSync, que en MediEvil no llega, así que Dan's Curse V2 nunca la recibía. Ahora se actualiza antes de cada frame de los shaders (dirección española 0x800EF0C0).

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.6 (alineada con MediEvilRecomp-ITA 1.1.6)

- La distancia de dibujado del juego se pasa a los shaders (`patches/render/ShaderDrawDistance.cs`, de la versión italiana, con la dirección española 0x800EF0C0): Dan's Curse V2 la usa para fundir a negro el límite de dibujado, como en el original, sin que la niebla se acerque delante de los muros. Sin esta patch el shader funciona igual, pero estima la distancia por su cuenta y es menos estable.
- Shader Dan's Curse V2 siempre en la última versión de la rama `main` (ahora el commit 0a54879: límite de dibujado y vacío bajo el horizonte oscuros en lugar de grises, distancia de dibujado tomada del juego, bloom predeterminado a 0.70). Como la versión española no tiene Mod Hub, `windows_initial_build_es.bat` y `linux_initial_build_es.sh` lo actualizan cada vez que se ejecutan; los paquetes de las releases incluyen la última versión.
- Para actualizar desde la v1.1.5 basta con volver a ejecutar `windows_initial_build_es.bat` (Linux: `./linux_initial_build_es.sh`): no hace falta borrar `generated-es`.

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.5 (alineada con MediEvilRecomp-ITA 1.1.5)

- **Sistema → Salir** ya no bloquea el juego. Antes salía sin detener el audio y, a pantalla completa, el proceso podía quedarse bloqueado en el controlador de vídeo, hasta el punto de que ni el Administrador de tareas lo terminaba (con la X de la ventana no pasaba). Ahora Salir vuelve primero a ventana si el juego está a pantalla completa y después lo cierra como la X; por seguridad, si el proceso sigue activo después de unos segundos, se termina (`patches/PalQuitFix.cs`, el mismo de la versión italiana).

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP v1.1.3 - base MediEvilRecomp 1.0 (alineada con MediEvilRecomp-ITA 1.1.3)

## Novedades
- Toda la base de MediEvilRecomp-ITA 1.1.3 portada a la versión española: MediEvilRecomp 1.0 y su RecompOne (cámara con la palanca derecha, interpolación mejorada, shaders slang, color de 24 bits, nuevo sistema de ajustes, correcciones de cierres inesperados), shader Dan's Curse V2 incluido (el mismo de la versión italiana). La versión española no tiene Mod Hub: el menú queda desactivado.
- Todas las correcciones de la versión italiana: cámara libre sin temblor, pantalla cortada tras los vídeos, modo de vídeo PAL forzado, iris con los shaders, LibCrypt.
- Direcciones nuevas en `config/versions.json` (119 hooks, 106 inmediatos, 65 constantes, también para `wrapers/`), todas comprobadas contra los dos discos con `tools/pal-port/version_port.py check`.
- Releases automáticas con GitHub Actions para Windows, Linux y macOS (`.github/workflows/release.yml`, repositorio privado con los ejecutables del disco español).
- Licencias como en la versión italiana: la GPL-3.0 cubre solo el código escrito para el fork; el código de MediEvilRecomp queda sin licencia (derechos de sus autores) y RecompOne es MIT. Ver la sección License del README.
- Los ajustes de las versiones anteriores no se conservan (nuevo sistema de MediEvilRecomp 1.0).
- Quitados de este repositorio los scripts de la versión italiana (`windows_initial_build.bat`, `windows_run.bat`, `windows_run_no_build.bat`, `linux_initial_build.sh`, `linux_run.sh`): aquí solo quedan los `_es`. La versión italiana se compila desde MediEvilRecomp-ITA.
- Después de actualizar hay que regenerar el código: borra la carpeta `generated-es` y vuelve a ejecutar `windows_initial_build_es.bat` (Linux: `./linux_initial_build_es.sh`).

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP - Linux

## Novedades
- Scripts para Linux: `linux_initial_build_es.sh` (primera compilación) y `linux_run_es.sh` (inicio), equivalentes a los de Windows.
- `windows_publish_es.bat <versión>` creaba los paquetes de la release para Windows y Linux. En la v1.1.3 lo sustituyen las releases automáticas de GitHub Actions.
- `Directory.Build.props` usa `/` en las rutas de salida (`bin-es/`, `obj-es/`) para que funcionen igual en Linux.

Autor: https://github.com/Leproide

---

# MediEvilRecomp ESP - LibCrypt (El Mausoleo de la Cima de la Colina)

## Correcciones
- El nivel **El Mausoleo de la Cima de la Colina** no se abría en la versión española y el juego se quedaba en la pantalla del CD con el signo de exclamación. Es la misma protección LibCrypt corregida en la versión italiana, pero el disco SCES-01495 tiene otros 16 sectores protegidos y otra clave (0x0E57 en lugar de 0xAAA6).
- `patches/PalLibCrypt.cs` ahora es común a las dos versiones: direcciones en `config/versions.json` (`LcKey`, `LcCount`, `LcTablePtr`, `LcTableBase`), lista de sectores de SCES-01494 y de SCES-01495, y `GameId` según la versión compilada. Si junto al `.cue` hay un `.sbi`/`.lsd`, se usa ese.
- Hooks de `PalLibCrypt` añadidos solo a `config/es/MediEvil.json` (sección `versionHooks` de `config/versions.json`); la configuración italiana no cambia.
- El detalle del control (`[PalLibCrypt]`) se escribe solo con `MEDIEVIL_DEBUG` definida.
- Nuevas herramientas: `tools/pal-port/libcrypt_check.py` (simula el control y comprueba el descifrado en `MEDRES.MWD`) y `version_port.py fill`.
- Después de actualizar hay que regenerar el código: borra la carpeta `generated-es` y vuelve a ejecutar `windows_initial_build_es.bat`; al terminar, ejecuta `windows_run_es.bat`.

Autor: https://github.com/Leproide

---

# Historial de MediEvilRecomp ITA (en italiano)

Las secciones siguientes vienen del repositorio italiano: describen las correcciones que esta versión ha heredado.

---

# MediEvilRecomp ITA v1.1.3

- Shader Dan's Curse V2 aggiornato: fiamme e magie illuminano davvero (riflessi, calore, tremolio del fuoco, ombre delle sorgenti di luce), onde dell'acqua piu' alte, foschia che parte piu' lontano dal personaggio.
- I pacchetti delle release e gli script di compilazione includono sempre l'ultima versione di Dan's Curse V2 (prima era fissata a un commit).

---

# MediEvilRecomp ITA v1.1.2

- Telecamera libera (levetta destra): niente piu' tremolio. Quando il gioco non ricalcolava la telecamera in un frame, la rotazione veniva applicata due volte; vicino a muri e recinzioni la telecamera entrava e usciva a ogni frame. Ora riparte sempre dai valori del gioco e si avvicina e si allontana dagli ostacoli in modo graduale.

---

# MediEvilRecomp ITA v1.1.1

- Shader **Dan's Curse V2** incluso nei pacchetti: basato su Dan's Curse di flaffymg, con nebbia e illuminazione riscritte (nebbia lineare e lontana, niente figure squadrate al limite di disegno, foschia a terra con banchi in movimento, ombre senza rumore). Si attiva accendendo la mod **Shaders** dal pannello **Mods**. Consigliato: **Draw distance** a 3x in Settings → Display.

---

# MediEvilRecomp ITA v1.1 - basato su MediEvilRecomp 1.0

Port PAL della versione 1.0 di MediEvilRecomp (BlackLabelHQ, commit 6383fe8) con il suo RecompOne (cc18281).

## Dalla 1.0 del progetto originale
- Telecamera con la levetta destra (`patches/qol/`). In PAL la velocita' e' calcolata su 25 fotogrammi al secondo.
- Interpolazione migliorata, acqua per gli shader, shader slang (ShaderMod aggiornata), colore a 24 bit, opzione per il dithering, editor di asset pack.
- Funzioni ricompilate in modalita' `"precise"` al posto delle vecchie patch in virgola mobile.
- Crash all'uscita da un livello e con distanza visiva alta, sfarfallio a inizio dissolvenza.
- Dissolvenze in widescreen (`FadeWidenPatch.cs`): in PAL esclude l'iride, che resta a `IrisFadePatch.cs` con il foro rotondo.
- Nuovo sistema di impostazioni: le impostazioni precedenti non vengono riprese.

## Port PAL
- Immagine tagliata in basso dopo lo skip dei filmati: il runtime 1.0 poteva trovarsi in modo video NTSC e mostrare 226 righe invece di 256. `PalDisplay.cs` rimette il modo PAL prima di ogni PutDispEnv.
- Mod Hub: ramo `master` del repository ITA (prima cercava `main`, da qui l'errore 404).
- Colore a 24 bit e "Better rendering" attivi di default.
- Release automatiche con GitHub Actions (`.github/workflows/release.yml`, `tools/ci/make_disc.py`): Windows, Linux e macOS.
- `tools/pal-port` aggiornato: gestisce le voci `"precise"` e la cartella `wrapers/`.
- Offset rispetto a `$gp` in `wrapers/Game.cs` e `wrapers/Mr.cs` tradotti a mano, tutti verificati come corrispondenze esatte.
- `WaterMaterialPatch.EndLevel` agganciata a mano a `0x800658BC`: in PAL la funzione di fine livello e' diversa nella parte finale, quindi lo strumento la scartava.
- Mantenute tutte le correzioni PAL del fork: callback CD, LibCrypt (Mausoleo), filmati, 256 righe, dialoghi XA, iride.
- Mod Trucchi e Filmati a schermo intero passate al nuovo sistema di impostazioni e attive di default.
- Shader Dan's Curse V2 (GPL-3.0, basato su Dan's Curse di flaffymg, con nebbia e illuminazione riscritte) incluso nei pacchetti delle release e scaricato dagli script di prima compilazione (Windows e Linux), alla versione fissata, in `shaders/dans-curse-v2-shader`, gia' registrato come installato dal Mod Hub.
- Shader: "Native resolution input" disattivato di default (con l'opzione attiva Dan's Curse diventa pixeloso). Resta attivabile da Shaders, per esempio per gli shader CRT.
- Dan's Curse: nebbia piu' leggera solo nel menu principale (`patches/render/TitleShaderPatch.cs`), dove la scena e' inquadrata da lontano e finiva quasi tutta nella nebbia.
- Mod **Filmati a schermo intero** 1.2: saltando molto in fretta il filmato iniziale lo schermo restava tagliato alle righe del video (dialogo del gargoyle e gioco). A fine filmato la mod rimanda la DISPENV originale. Log con `MEDIEVIL_DEBUG`.
- Iride con uno shader attivo: l'anello sfumato (sottrattivo) diventa nero opaco, perche' gli shader non sanno rappresentarlo e mostravano luci e nebbia sopra il nero.
- Mod Hub della versione PAL: https://github.com/Leproide/MediEvilRecomp-ITA-ModHub (`DefaultModHub` in `Program.cs`, sostituibile con `modhub.repository` e `modhub.branch` in `settings.json`). L'hub originale non e' usato: le sue mod sono per la versione USA.
- Aggiornate le copie di riserva in `BACKUP_Dipendenze`.

Da provare: filmati (saltati e non), mappa e Mausoleo, gargoyle della cripta, dissolvenze in widescreen, distanza visiva al massimo e uscita dai livelli, telecamera, Asylum, Scarecrow Fields, Pumpkin Gorge e Zarok.

Autore del port: https://github.com/Leproide

---

# MediEvilRecomp ITA - script per Linux

## Novità
- Nuovi `linux_initial_build.sh` e `linux_run.sh`: stessi passaggi degli script Windows (sottomoduli, correzione di RecompOne, controllo di .NET 10 e dei file del disco, generazione e compilazione). `./linux_run.sh --no-build` avvia senza ricompilare.
- `.gitattributes`: gli script `.sh` mantengono la fine riga LF anche se il repository viene clonato su Windows.

Autore: https://github.com/Leproide

---

# MediEvilRecomp ITA - Mausoleo e LibCrypt

## Correzioni
- Il livello **Il Mausoleo in Cima alla Collina** non si apriva e il gioco restava sulla schermata con il CD e il punto esclamativo. Il disco PAL è protetto con LibCrypt: dalla mappa il gioco controlla 16 settori del disco e, da quelli con il sottocodice modificato, ricava la chiave che decifra l'inizio dei dati del Mausoleo in `MEDRES.MWD`. Un dump bin/cue non ha il sottocodice, quindi la chiave risultava 0 e i dati restavano cifrati.
- Nuovo `patches/PalLibCrypt.cs`: sui settori protetti fa riportare a GetlocP la posizione del settore precedente, come fa il lettore reale con un sottocodice non valido. L'elenco dei settori di SCES-01494 è incluso; se in `disc` c'è un file `.sbi` o `.lsd` viene usato quello. Agganciato in `config/MediEvil.json`.
- Corretti i commenti di `PalCdCallbacks.cs`: la macchina a stati a 0x80065D20 è il controllo LibCrypt, non la musica XA.
- Dopo l'aggiornamento serve rigenerare il codice: cancella la cartella `generated` e rilancia `windows_initial_build.bat`, una volta completato lancia `windows_run.bat`

Autore: https://github.com/Leproide

---

# MediEvilRecomp ITA - dialoghi XA e filmati a schermo intero

## Correzioni
- Il primo discorso del gargoyle nella cripta si chiudeva da solo se il filmato iniziale veniva saltato subito: un marcatore di fine traccia della musica veniva preso per la fine del discorso. Nuovo `patches/PalXaSpeech.cs`, agganciato in `config/MediEvil.json`.
- Mod **Filmati a schermo intero** 1.1: saltando un filmato dopo qualche secondo, il gioco poteva restare con l'immagine tagliata. Ora lo stato del lettore di avvio e quello del gioco sono separati.
- Dopo l'aggiornamento serve rigenerare il codice: cancella la cartella `generated` e rilancia `windows_initial_build.bat`, una volta completato lancia `windows_run.bat`

Autore: https://github.com/Leproide

---

# MediEvilRecomp ITA - filmati a schermo intero

## Novità
- Nuova mod opzionale **Filmati a schermo intero** (`mods/FilmatiSchermoIntero/`): durante i filmati mostra solo l'area del video (320×176) e la adatta alla finestra senza deformarla. Si attiva dal pannello delle mod e non richiede di ricompilare.

Autore: https://github.com/Leproide

---

# MediEvilRecomp ITA - filmati e dissolvenze

## Correzioni
- Filmati saltati dopo un secondo o due (issue #5 del progetto originale): il lettore dei filmati considerava finito il filmato dopo circa 4 milioni di tentativi a vuoto, che su PC durano una frazione di secondo. Ora l'attesa è misurata in tempo reale (2 secondi). Nuovo `patches/PalMovies.cs`.
- Dissolvenza a iride che in widescreen non copriva gli angoli (issue #7 del progetto originale): il bordo esterno della dissolvenza viene allargato in proporzione al margine widescreen. Nuovo `patches/widescreen/IrisFadePatch.cs`.
- Filmati spostati in basso: RecompOne usava 240 righe anche in PAL quando il gioco non indica l'altezza dello schermo. Ora sono 256. Nuovo `patches/PalDisplay.cs`.
- Dopo l'aggiornamento serve rigenerare il codice: cancella la cartella `generated` e rilancia `windows_initial_build.bat`.

Autore: https://github.com/Leproide

---

# MediEvilRecomp ITA - correzione mappa dopo la Sala degli Eroi

## Correzioni
- La mappa dopo la Sala degli Eroi andava a circa 1 fotogramma al secondo e i comandi non rispondevano; scegliendo il livello il gioco poi si bloccava.
- Causa 1: lo `StUnSetRing` HLE di RecompOne non azzera i callback del CD come quello originale, quindi dopo un filmato restava attivo `StCdInterrupt`.
- Causa 2: il gestore della musica XA del gioco scrive i callback direttamente nelle variabili di libcd (`0x800EB660`/`0x800EB664`) e RecompOne non le vedeva.
- Nuovo `patches/PalCdCallbacks.cs`, agganciato in `config/MediEvil.json`: azzera i callback dopo `StUnSetRing` e tiene allineate le variabili del gioco con quelle di RecompOne.
- Dopo l'aggiornamento serve rigenerare il codice: cancella la cartella `generated` e rilancia `windows_initial_build.bat`.

Autore: https://github.com/Leproide

---

# MediEvilRecomp ITA - port PAL italiano (SCES-01494)

Prima versione del fork per MediEvil PAL italiano.

## Novità
- Configurazione e funcmap tradotte per la versione PAL italiana: boot `SCES_014.94`, `MEDIEVIL.EXE` e i 26 overlay.
- Patch di rendering e widescreen (`CullPatch`, `EntityFloatFunctions`, `TerrainPatch`, `TerrainCapture`, `LandMapPatch`) portate sugli indirizzi PAL, con i valori a 50 Hz e la mappa a 256 righe.
- Conservati i nomi delle funzioni SDK, così RecompOne usa le sue implementazioni HLE di VSync, CdRead e simili.
- Nuovo `PalDispatch.cs`: le chiamate agli overlay passano per indirizzo e non dipendono dai suffissi dei nomi.
- Nuovo `PalDebug.cs`: log di debug attivabili con `MEDIEVIL_DEBUG`.
- Correzione di RecompOne per la modalità settore intero del lettore CD (`recompone-fixes/`), applicata in automatico dagli script.
- Script di compilazione in italiano, con i nomi dei file `(Italy)`.
- Aggiornamento automatico collegato alle release di questo fork.
- Strumenti di porting e di verifica in `tools/pal-port/`.
- Nuova mod **Trucchi (MediEvil ITA)** (`mods/Trucchi/`): attiva la Modalità Trucchi o la Modalità Trucchi Estesa dal pannello delle mod.

Autore del port: https://github.com/Leproide

---

# 

v0.2b* is not a massive update, just something that i wanted to fix way before the first release but havent had time! the rendering now should be fairly decent looking!

thanks for the support on the initial release! i hope you guys enjoy it!

# Changes
- ditched pgxp renderer in favor of a ""native"" rendering patch to use float, depth buffer and correct textures (the power of recomp patches!)
- fixed overworld map not rendering properly on 16:9
- fixed an issue with the clear buffer not covering 16:9
- added a "better rendering" option to improve the terrain/entity rendering and camera, interpolation is also improved
- added automatic updater
- included shadermod

# Dependencies

You're required to install [dotnet 10 runtime](https://dotnet.microsoft.com/pt-br/download/dotnet/10.0) to run the recomp, otherwise the game will not open, ideally also make sure to install [OpenAL](https://www.openal.org/downloads/), as of the current version no further dependencies are required

you can find instructions downloading dotnet 10 for linux systems that dont have it on their packages [here](https://wiki.archlinux.org/title/.NET), or you can run "run.sh" wich will install dotent if not available and execute sotn for you
