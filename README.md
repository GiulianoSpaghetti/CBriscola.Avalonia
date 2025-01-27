:it: Made in Italy. Il primo prodotto serio pubblicato in avalonia ed internazionalizzato.

Questo gioco dimostra che la teoria dei giochi è vera: l'algorimo brevettato funziona su tutti i giochi di carte senza piatto.

![Napoli-Logo](https://github.com/user-attachments/assets/8163c808-62d3-40d3-bce3-0957e57bc26a)
![made in parco grifeo](https://github.com/user-attachments/assets/fadbf046-aeae-4f11-bda4-eb332c701d56)

## ATTENZIONE
Questo prodotto viene dichiarato dismesso e sostituito dalla cbriscola.material.

## CBriscola.Avalonia
Quello che avete davanti non è il gioco della briscola come si intende oggi, perché oggi tutti i simulatori di briscola dicono "hai preso l'asso, bravo" e finisce lì. Quello che avete davanti è un simulatore equo e professionale, con punteggio aggiornato in tempo reale, in modo da poter decidere se "rischiare" o meno coscientemente, scritto in avalonia.
Dal momento che avalonia ha i timer che vengono blacklistati, c'è il pulsante per continuare a giocare.

## Dedica
Voglio dedicare l'intero progetto non a Francesca la barista, ma alla vecchia sory, ossia numerona, quella gentil donzella in quel di rivisondoli che mi ha aiutato quando ero una povera anima in pena ed ora mi rende sia felice che "felice"..
Perché io adesso sono anche "felice", non conto di diventarlo, ma lo sono.

## Per compilare

Bisogna scaricarsi da nuget il package CardFramework.avalonia

## Come funziona
Per festeggiare, vi spiego come funziona il mio algoritmo brevettato:
i punti in totale sono 120, ossia 4 assi che valgono 11 punti ciascuno, 4 3 che valgono 10 punti ciascuno, 4 10 che valgono 4 punti ciascuno, 4 9 che valgono 3 punti ciascuno, 4 8 che valgono 2 punti ciascuno.
Dal momento che la matematica non è una opinione:
4x11+4x10=84.
4x4+4x3+4x2=16+12+8=36

84+36=120 punti totali

120/2 = 60, servono 61 punti per vincere

basandosi solo sui carichi si rischia di perdere, perché

84-61=23, bisogna prenderli quasi tutti e lasciare solo 23 punti di carichi

60-36=24, prendendo tutte le altre carte bastano solo 3 carichi per vincere.

Per cui non metto i livelli, ma vi lascio imparare la teoria delle carte a lungo, da me inventata a 18 anni, con la wxbriscola, che mi ha portato l'amore di Francesca.                                                                                                                                              
## Internazionalizzazione
Aprire il file MainWindows.axaml, all'interno del tag MainWindow.Resources ci sono qulli che vengono chiamati dizionari.
BIsogna copiare un dizioario ed aggiungrlo alla fine dei dizionari, chiamarlo con la denominazione internazionale a due carattri ella lingua (it per italiano, pt per portoghese, es per spagnolo e via dicendo) e bisogna tradurre tutto qullo che è il contenuto del tag x:string, non il parametro.

Infine compilare.

## Dove recuperare i mazzi aggiuntivi

I mazzi aggiuntivi sono quelli della wxbriscola, si possono scaricare sulle relative home page dei progetti, per windows e linux.

## Bug noti

Se si usa un mazzo non completo all'avvio del programma, verrà caricato il mazzo napoletano e l'avviso non è garantito che esca.


## Donazione

http://numerone.altervista.org/donazioni.php
