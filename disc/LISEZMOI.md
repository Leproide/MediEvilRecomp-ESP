<!--
  MediEvilRecomp FRA - instructions pour les fichiers du disque
  D'apres disc/LEEME.md de https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# Dossier du disque

Pour compiler et jouer, il vous faut **votre** disque original de **MediEvil, version PAL française (SCES-01492)**. Le projet ne contient et ne distribue aucun fichier du jeu, qui sont protégés par le droit d'auteur : vous devez les extraire vous-même de votre copie originale.

Les instructions pour la version italienne (SCES-01494) sont dans [`README.md`](README.md), celles pour la version espagnole (SCES-01495) dans [`LEEME.md`](LEEME.md).

## Format nécessaire

L'image doit être au format **bin/cue** (MODE2/2352) **propre**, c'est-à-dire identique au disque original et sans patch. C'est le format des images Redump.

Copiez ici ces trois fichiers, avec exactement ces noms :

- `MediEvil (France) (Track 1).bin`
- `MediEvil (France) (Track 2).bin`
- `MediEvil (France).cue`

Le fichier `.cue` désigne les `.bin` par leur nom. Si vous renommez les `.bin`, modifiez aussi les lignes `FILE` du `.cue`.

Une image `.chd` peut être convertie avec `chdman extractcd` (version récente, option `--splitbin` pour obtenir une piste par fichier).

## LibCrypt

Le disque PAL français est protégé par **LibCrypt** : 16 secteurs du disque ont un sous-code modifié, d'où le jeu tire la clé qui ouvre le niveau du mausolée (**Hilltop Mausoleum** dans la version anglaise). Le format bin/cue ne contient pas le sous-code.

Il n'y a rien à faire : la liste des secteurs protégés de SCES-01492 est déjà dans `patches/PalLibCrypt.cs`. Si ce dossier contient un fichier `MediEvil (France).sbi` (ou `.lsd`), le même que celui des émulateurs, il est utilisé à la place de la liste intégrée.

Au démarrage, le journal indique quelle liste a été utilisée (`[PalLibCrypt]`).

## Vérifications

- La taille de chaque `.bin` doit être un multiple exact de 2352 octets.
- SHA-256 des fichiers extraits d'un disque correct :
  - `SYSTEM.CNF` : `2324a4100c789757e01c06d2f884669ea204f46613e3cd26438733580e07ff3e`
  - `SCES_014.92` : `e6bf1bf90ee2f4aac170f426bb573e3dabea861c39b275da36539cb8dc2818d6`
  - `MEDIEVIL.EXE` : `281a26aebf8f901ff5d94340f6f8950047acd93fa2867fb775a5ea5f5284db61`
- Les autres versions ne fonctionnent pas avec la configuration française : la version USA (SCUS-94227) utilise le projet original, l'italienne `config/MediEvil.json` et l'espagnole `config/es/MediEvil.json`.
- N'utilisez pas d'images patchées contre LibCrypt : l'exécutable de démarrage est modifié et la recompilation se bloque au lancement.

## License

GPL-3.0-or-later.
