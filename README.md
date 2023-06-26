# CBriscola.Avalonia
Lo so che vi siete scocciati, però la Cbriscola torna completamente libera e multipiattaforma con l'interfaccia in Avalonia, il pezzotto dell'XAML. Una chicca per appassionati.

# Dedica
Voglio dedicare l'intero progetto non a Francesca la barista, ma a Francesca, quella gentil donzella in quel di rivisondoli che mi ha aiutato quando ero una povera anima in pena ed ora mi rende sia felice che "felice"..


# Come funziona
Per festeggiare, vi spiego come funziona il mio algoritmo brevettato:
i punti in totale sono 120, ossia 4 assi che valgono 11 punti ciascuno, 4 3 che valgono 10 punti ciascuno, 4 10 che valgono 4 punti ciascuno, 4 9 che valgono 3 punti ciascuno, 4 8 che valgono 2 punti ciascuno.
Dal momento che la matematica non è una opinione:
4*11+4*10=84.
4*4+4*3+4*2=16+12+8=36

84+36=120 punti totali

120/2 = 60, servono 61 punti per vincere

basandosi solo sui carichi si rischia di perdere, perché

84-61=23, bisogna prenderli quasi tutti e lasciare solo 23 punti di carichi

60-36=24, prendendo tutte le altre carte bastano solo 3 carichi per vincere.

Per cui non metto i livelli, ma vi lascio imparare la teoria delle carte a lungo, da me inventata a 18 anni, con la wxbriscola, che mi ha portato l'amore di Francesca.

# Video
https://www.twitch.tv/videos/1856276989
https://youtu.be/Zcbq0DwYcW8
                                                                                                                                                  
# Internazionalizzazione
Aprire il file MainWindows.axaml, all'interno del tag MainWindow.Resources ci sono qulli che vengono chiamati dizionari.
BIsogna copiare un dizioario ed aggiungrlo alla fine dei dizionari, chiamarlo con la denominazione internazionale a due carattri ella lingua (it per italiano, pt per portoghese, es per spagnolo e via dicendo) e bisogna tradurre tutto qullo che è il contenuto del tag x:string, non il parametro.

Infine compilare.

# Dove recuperare i mazzi aggiuntivi

I mazzi aggiuntivi sono quelli della wxbriscola, si possono scaricare sulle relative home page dei progetti, per windows e linux.
Tenete presente che il mazzo fable 3 ha un bug: bisogna rinominare le immagini da 0 a 9 aggiungendo un 2 e bisogna togliere il 2 nelle immagini da 20 a 29.


# Bibliografia
https://stackoverflow.com/questions/68684968/close-a-window-in-avalonia-gui

https://docs.avaloniaui.net/docs/controls/image

https://docs.avaloniaui.net/docs/styling/resources

https://github.com/AvaloniaUI/Avalonia/issues/5442

https://stackoverflow.com/questions/44243167/how-to-define-a-separate-set-of-styles-for-each-platform-os-in-avalonia

https://github.com/AvaloniaUI/Avalonia/issues/54411
