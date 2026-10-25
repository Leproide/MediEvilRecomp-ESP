<!--
  Filmati a schermo intero - README
  Author: https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# Filmati a schermo intero

Mod opzionale. I filmati del gioco sono 320×176 dentro uno schermo PAL da 256 righe: normalmente si vedono piccoli, al centro del riquadro 4:3, con bande nere su tutti i lati. Con questa mod, durante i filmati si vede solo l'area del video, adattata alla finestra senza deformarla (formato circa 1,94:1). In 16:9 occupa tutta la larghezza.

MediEvil PAL italiano (SCES-01494) e spagnolo (SCES-01495).

## Uso

1. Apri il pannello delle mod dal menu in alto e attiva **Filmati a schermo intero**.
2. Con l'ingranaggio puoi accendere o spegnere l'effetto anche durante un filmato. La scelta viene ricordata.

Finito il filmato, l'immagine del gioco torna quella normale.

## Dettagli tecnici

- Si attiva durante i due lettori di filmati: quello del gioco (`func_80079660`, descrittore a `$gp+0x764`) e quello dell'eseguibile di avvio per l'introduzione (`func_801B0978`, descrittore a `$gp+0x48`).
- Solo per la durata di `PutDispEnv` imposta `disp.y`, `disp.h` e `screen.h` della DISPENV sulle righe del video, poi rimette i valori del gioco.
- Imposta il formato d'uscita a `4/3 × 256 / altezza del video` e lo ripristina a fine filmato.

## Licenza

GPL-3.0-or-later.

## Autore

https://github.com/Leproide
