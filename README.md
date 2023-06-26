# CBriscola.Avalonia
Lo so che vi siete scocciati, però la Cbriscola torna completamente libera e multipiattaforma con l'interfaccia in Avalonia, il pezzotto dell'XAML. Una chicca per appassionati.

# Screenshots
<img src="https://user-images.githubusercontent.com/49764967/218879403-0f3586ce-21b5-4765-ad62-d3b6b04b15ac.png" />
<img src="https://user-images.githubusercontent.com/49764967/218879409-295fff5c-8556-4c61-9f0c-8bfd7b7ad980.png" />
<img src="https://user-images.githubusercontent.com/49764967/218879412-15fcd966-26af-4eb9-9370-be739c444f3c.png" />
<img src="https://user-images.githubusercontent.com/49764967/218879414-3e3d4fa5-f378-46fc-a084-c062377b9769.png" />
<img src="https://user-images.githubusercontent.com/49764967/219301533-e73f8a4b-c285-4f9e-976d-8cbf5c3d88f6.png" />


# Installazione

# Su windows

https://youtu.be/auXFlDdNdaA

# Tramite repository
Seguite prima la guida all'indirizzo https://learn.microsoft.com/it-it/dotnet/core/install/linux-debian

Installate la nuova chiave del repository tramite il comando sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 52B68EEB

Poi inserite nel file /etc/apt/sources.list la riga deb http://numeronesoft.ddns.net/repos/apt/debian bullseye main

Infine fate apt update ed apt install cbriscola.avalonia
                                                                                                                                                  
# Bibliografia
https://stackoverflow.com/questions/68684968/close-a-window-in-avalonia-gui

https://docs.avaloniaui.net/docs/controls/image

https://docs.avaloniaui.net/docs/styling/resources

https://github.com/AvaloniaUI/Avalonia/issues/5442

https://stackoverflow.com/questions/44243167/how-to-define-a-separate-set-of-styles-for-each-platform-os-in-avalonia

https://github.com/AvaloniaUI/Avalonia/issues/54411

# Internazionalizzazione
Aprire il file MainWindows.axaml, all'interno del tag MainWindow.Resources ci sono qulli che vengono chiamati dizionari.
BIsogna copiare un dizioario ed aggiungrlo alla fine dei dizionari, chiamarlo con la denominazione internazionale a due carattri ella lingua (it per italiano, pt per portoghese, es per spagnolo e via dicendo) e bisogna traurre tutto qullo che è il contnuto del tag x:string, non il parametro.

Infine compilare.

# Dove recuperare i mazzi aggiuntivi

I mazzi aggiuntivi sono quelli della wxbriscola, si possono scaricare sulle relative home page dei progetti, per windows e linux.
Tenete presente che il mazzo fable 3 ha un bug: bisogna rinominare le immagini da 0 a 9 aggiungendo un 2 e bisogna togliere il 2 nelle immagini da 20 a 29.

# Dedica
Voglio dedicare l'intero progetto non a Francesca la barista, ma a Francesca, quella gentil donzella in quel di Rivisondoli che mi ha aiutato quando ero una povera anima in pena ed ora mi rende sia felice che "felice".

# Donazioni

[![paypal](https://www.paypalobjects.com/it_IT/IT/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=H4ZHTFRCETWXG)

Puoi donare anche tramite carta Hype a patto di avere il mio numero di cellulare nella rubrica. Sai dove lo puoi trovare? Sul mio curriculum.
Apri l'app Hype, fai il login, seleziona PAGAMENTI, INVIA DENARO, seleziona il mio numero nella rubrica, imposta l'importo, INSERISCI LA CAUSALE e segui le istruzioni a video.

