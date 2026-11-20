<!--
  Filmati a schermo intero - README
  Author: https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# Filmati a schermo intero

Mod opzionale. I filmati del gioco sono 320×176 dentro uno schermo PAL da 256 righe: normalmente si vedono piccoli, al centro del riquadro 4:3, con bande nere su tutti i lati. Con questa mod, durante i filmati si vede solo l'area del video, adattata alla finestra senza deformarla (formato circa 1,94:1). In 16:9 occupa tutta la larghezza.

MediEvil PAL italiano (SCES-01494), spagnolo (SCES-01495), francese (SCES-01492), tedesco (SCES-01493) e inglese (SCES-00311); NTSC-U americano (SCUS-94227, schermo di 240 righe), copiata dal launcher nel progetto USA (`usa/mods/`).

## Uso

1. Apri il pannello delle mod dal menu in alto e attiva **Filmati a schermo intero**.
2. Con l'ingranaggio puoi accendere o spegnere l'effetto anche durante un filmato. La scelta viene ricordata.

Finito il filmato, l'immagine del gioco torna quella normale.

## Dettagli tecnici

- Si attiva durante i due lettori di filmati: quello del gioco (`func_80079660` nella versione italiana, descrittore a `$gp+0x764`; nelle altre versioni indirizzo e descrittore cambiano, vedi il sorgente) e quello dell'eseguibile di avvio per l'introduzione (`func_801B0978`, descrittore a `$gp+0x48`).
- Solo per la durata di `PutDispEnv` imposta `disp.y`, `disp.h` e `screen.h` della DISPENV sulle righe del video, poi rimette i valori del gioco.
- Imposta il formato d'uscita a `4/3 × 256 / altezza del video` e lo ripristina a fine filmato.
- A fine filmato rimanda a `PutDispEnv` la DISPENV originale, così lo schermo non resta tagliato anche se il filmato viene saltato subito e il gioco non la reimposta.
- Con `MEDIEVIL_DEBUG` impostata, il log mostra inizio e fine dei filmati e ogni ritaglio (righe `[FilmatiSchermoIntero]`).

## Licenza

GPL-3.0-or-later.

## Autore

https://github.com/Leproide
