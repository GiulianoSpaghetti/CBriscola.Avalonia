:it: Made in Italy. Il primo prodotto serio pubblicato in avalonia ed internazionalizzato.

## CBriscola.Avalonia
Lo so che vi siete scocciati, però la Cbriscola torna multipiattaforma con l'interfaccia in Avalonia (quindi non completamente libera), il pezzotto dell'XAML. Una chicca per appassionati.

## Dedica
Voglio dedicare l'intero progetto non a Francesca la barista, ma alla vecchia sory, ossia numerona, quella gentil donzella in quel di rivisondoli che mi ha aiutato quando ero una povera anima in pena ed ora mi rende sia felice che "felice"..
Perché io adesso sono anche "felice", non conto di diventarlo, ma lo sono.

## Video di presentazione

https://1drv.ms/u/s!ApmOB0x2yBN0kohpLGZYUm1EtGUDoA?e=pO2UEr (su windows)


https://1drv.ms/u/s!ApmOB0x2yBN0ktR6rFcvmC18tSWdDg?e=oj6fhu (su debian)

# Come installare

## Su Windows

[![winget](https://user-images.githubusercontent.com/49786146/159123313-3bdafdd3-5130-4b0d-9003-40618390943a.png)](https://marticliment.com/wingetui/share?pid=GiulioSorrentino.CBriscola.Avalonia&pname=CBriscola.Avalonia&psource=Winget:%20winget)

## Su GNU/linux
Seguite le istruzioni su http://numeronesoft.ddns.net:8080

Poi installate cbriscola.avalonia
ATTENZIONE:
Avalonia si basa su due librerie native libharfbuzzsharp e libskiasharp che non sono libere, quindi avalonia non è libero.

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


## Donazione

http://numerone.altervista.org/donazioni.php

## Bibliografia
https://stackoverflow.com/questions/68684968/close-a-window-in-avalonia-gui

https://docs.avaloniaui.net/docs/controls/image

https://docs.avaloniaui.net/docs/styling/resources

https://github.com/AvaloniaUI/Avalonia/issues/5442

https://stackoverflow.com/questions/44243167/how-to-define-a-separate-set-of-styles-for-each-platform-os-in-avalonia

https://github.com/AvaloniaUI/Avalonia/issues/54411
