<!--
  Trucchi (MediEvil ITA) - README
  Author: https://github.com/Leproide
  License: GPL-3.0-or-later
-->

# Trucchi (MediEvil ITA)

Mod che attiva i **menu trucchi integrati nel gioco** senza inserire le combinazioni di tasti. Funziona con **MediEvil PAL italiano (SCES-01494)** e **spagnolo (SCES-01495)**: su altre versioni non tocca nulla.

## Uso

1. Avvia il gioco.
2. Apri il pannello delle mod dal menu in alto, attiva **Trucchi (MediEvil ITA)** e scegli una modalità:
   - **Disattivati**
   - **Modalità Trucchi**: completa livello, invulnerabilità, calice;
   - **Modalità Trucchi Estesa**: in più ricomincia livello, filmati, test audio, telecamera, super trucco, energia, oggetti, monete e debug.
3. Entra in un livello e metti in pausa (Start): le voci dei trucchi sono nel menu di pausa.

La scelta viene ricordata e resta attiva anche quando cambi livello.

## Combinazioni originali

Per attivarle a mano: in pausa, tieni premuto **L2** e premi:

- **Modalità Trucchi**: Giù, Su, Quadrato, Triangolo, Triangolo, Cerchio, Giù, Su, Quadrato, Triangolo
- **Modalità Trucchi Estesa**: Triangolo, Cerchio, Triangolo, Cerchio, Cerchio, Triangolo, Sinistra, Cerchio, Su, Giù, Destra, Cerchio, Sinistra, Sinistra, Triangolo, Destra, Cerchio, Sinistra, Sinistra, Triangolo, Cerchio, Giù, Cerchio, Cerchio, Destra

Queste sequenze sono ricavate dalle tabelle dentro `MEDIEVIL.EXE` della versione italiana. Alcune wiki riportano per la prima una sequenza di 8 tasti, che è sbagliata.

## Dettagli tecnici

- La routine `func_80022C2C` gira a ogni frame e confronta i tasti premuti con due tabelle: `0x800CE0B4` (10 tasti) e `0x800CE0DC` (25 tasti).
- Lo stato dei trucchi è un halfword a `0x800EF0A2`: `0` nessuno, `1` Modalità Trucchi, `3` Modalità Trucchi Estesa.
- Versione spagnola (SCES-01495): stessa routine `func_80022C2C`, tabelle a `0x800CE09C` e `0x800CE0C4`, stato a `0x800EF11A`.
- La mod si aggancia dopo `func_80022C2C` e scrive lo stato scelto. Prima riconosce la versione controllando dove si trovano le due tabelle.

## Licenza

GPL-3.0-or-later.

## Autore

https://github.com/Leproide
