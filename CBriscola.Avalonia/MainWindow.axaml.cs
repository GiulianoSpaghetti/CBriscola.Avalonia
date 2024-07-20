using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;
using Avalonia.Platform;
using System.Reflection;
using Avalonia.Media.Imaging;
using System.Globalization;
using DesktopNotifications.FreeDesktop;
using System.Runtime.InteropServices;
using INotificationManager = DesktopNotifications.INotificationManager;
using Notification= DesktopNotifications.Notification;
using org.altervista.numerone.framework;

namespace CBriscola.Avalonia
{
    public partial class MainWindow : Window
    {
        private static Giocatore g, cpu, primo, secondo, temp;
        private static Mazzo m;
        private static Carta c, c1, briscola;
        private static Image cartaCpu = new Image();
        private static Image i, i1;
        private static UInt16 puntiUtente=0, puntiCpu=0;
        private static UInt128 partite = 0;
        private static bool avvisaTalloneFinito = true, briscolaDaPunti = false, primaUtente = true, stessoSeme=false;
        private static GiocatoreHelperCpu helper;
        private static readonly string folder= System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CBriscola.Avalonia");

        private ResourceDictionary d;
        private ElaboratoreCarteBriscola e;
        private Stream asset;
        private INotificationManager notification;
        private Opzioni o;
        private static org.altervista.numerone.framework.briscola.CartaHelper cartaHelper;
        private static INotificationManager CreateManager()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return new FreeDesktopNotificationManager();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return new DesktopNotifications.Windows.WindowsNotificationManager(null);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return new DesktopNotifications.Apple.AppleNotificationManager();

            throw new PlatformNotSupportedException();
        }
        public MainWindow()
        {
            this.InitializeComponent();
            notification = CreateManager();
            notification.Initialize();
            d = this.FindResource(CultureInfo.CurrentCulture.TwoLetterISOLanguageName) as ResourceDictionary;
            if (d==null)
                d = this.FindResource("it") as ResourceDictionary;
            o = LeggiOpzioni();
            briscolaDaPunti = o.briscolaDaPunti;

            e = new ElaboratoreCarteBriscola(briscolaDaPunti);
            m = new Mazzo(e);
   
            m.SetNome(o.nomeMazzo);
            Carta.Inizializza(App.path, m, 40, cartaHelper=new org.altervista.numerone.framework.briscola.CartaHelper(ElaboratoreCarteBriscola.GetCartaBriscola()), d["bastoni"] as string, d["coppe"] as string, d["denari"] as string, d["spade"] as string, d["Fiori"] as string, d["Quadri"] as string, d["Cuori"] as string, d["Picche"] as string, "Cbriscola.Avalonia");

        if (o.nomeMazzo == "Napoletano")
            {
                asset = AssetLoader.Open(new Uri($"avares://{Assembly.GetEntryAssembly().GetName().Name}/Assets/retro_carte_pc.png"));
                cartaCpu.Source = new Bitmap(asset);

            }
            else
                try
                {
                    cartaCpu.Source = new Bitmap(System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(App.path, "Mazzi"), m.GetNome()), "retro carte pc.png"));
                }
                catch (Exception ex)
                {
                    asset = AssetLoader.Open(new Uri($"avares://{Assembly.GetEntryAssembly().GetName().Name}/Assets/retro_carte_pc.png"));
                    cartaCpu.Source = new Bitmap(asset);

                }
            g = new Giocatore(new GiocatoreHelperUtente(), o.NomeUtente, 3);
            switch (o.livello) {
                case 1: helper = new GiocatoreHelperCpu0(ElaboratoreCarteBriscola.GetCartaBriscola()); break;
                case 2: helper = new GiocatoreHelperCpu1(ElaboratoreCarteBriscola.GetCartaBriscola()); break;
                default: helper = new GiocatoreHelperCpu2(ElaboratoreCarteBriscola.GetCartaBriscola()); break;

            }
            cpu = new Giocatore(helper, o.NomeCpu, 3);
            avvisaTalloneFinito = o.avvisaTalloneFinito;
            stessoSeme = o.stessoSeme;
            primo = g;
            secondo = cpu;
            briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
            Image[] img = new Image[3];
            for (UInt16 i = 0; i < 3; i++)
            {
                g.AddCarta(m);
                cpu.AddCarta(m);

            }
            NomeUtente.Content = g.GetNome();
            NomeCpu.Content = cpu.GetNome();
            Utente0.Source = g.GetImmagine(0);
            Utente1.Source = g.GetImmagine(1);
            Utente2.Source = g.GetImmagine(2);
            Cpu0.Source = cartaCpu.Source;
            Cpu1.Source = cartaCpu.Source;
            Cpu2.Source = cartaCpu.Source;
            PuntiCpu.Content = $"{d["PuntiDiPrefisso"]} {cpu.GetNome()} {d["PuntiDiSuffisso"]}: {cpu.GetPunteggio()}";
            PuntiUtente.Content = $"{d["PuntiDiPrefisso"]} {g.GetNome()} {d["PuntiDiSuffisso"]}: {g.GetPunteggio()}";
            NelMazzoRimangono.Content = $"{d["NelMazzoRimangono"]} {m.GetNumeroCarte()} {d["carte"]}";
            CartaBriscola.Content = $"{d["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
            lbCartaBriscola.Content = $"{d["BriscolaDaPunti"]}";
            lbAvvisaTallone.Content = $"{d["AvvisaTallone"]}";
            opNomeUtente.Content = $"{d["NomeUtente"]}: ";
            opNomeCpu.Content = $"{d["NomeCpu"]}: ";
            InfoApplicazione.Content = $"{d["Applicazione"]}";
            OpzioniApplicazione.Content = $"{d["Applicazione"]}";
            OpzioniInformazioni.Content = $"{d["Informazioni"]}";
            AppInformazioni.Content = $"{d["Informazioni"]}";
            AppOpzioni.Content = $"{d["Opzioni"]}";
            fpOk.Content = $"{d["Si"]}";
            fpCancel.Content = $"{d["No"]}";
            fpShare.Content = $"{d["Condividi"]}";
            Briscola.Source = briscola.GetImmagine();
            btnGiocata.Content = $"{d["giocataVista"]}";
            lbmazzi.Content =$"{d["Mazzo"]}";
            lbLivello.Content = $"{d["Livello"]}";
            lbStessoSeme.Content =$"{d["VarianteStessoSeme"]}";
        }

        private Opzioni CaricaOpzioni()
        {
            Opzioni o;

            o = LeggiOpzioni();
            return o;
        }
        private Opzioni LeggiOpzioni()
        {
            Opzioni o;
                if (!System.IO.Path.Exists(folder))
               Directory.CreateDirectory(folder);
            StreamReader file=null;
            try
            {
                file = new StreamReader(System.IO.Path.Combine(folder, "opzioni.json"));
                try
                {
                    o = Newtonsoft.Json.JsonConvert.DeserializeObject<Opzioni>(file.ReadToEnd());
                }
                catch (Newtonsoft.Json.JsonReaderException ex)
                {
                    o = null;
                    file.Close();
                }
                if (o == null)
                    throw new FileNotFoundException();
            }
            catch (FileNotFoundException ex)
            {
                o = new Opzioni();
                o.NomeUtente = "numerone";
                o.NomeCpu = "numerona";
                o.briscolaDaPunti = false;
                o.avvisaTalloneFinito = true;
                o.nomeMazzo = "Napoletano";
                o.livello = 3;
                o.stessoSeme = false;
                SalvaOpzioni(o);
                return o;
            }
            file.Close();
            return o;
        }

        private void SalvaOpzioni(Opzioni o)
        {
            StreamWriter w = new StreamWriter($"{System.IO.Path.Combine(folder, "opzioni.json")}");
            w.Write(Newtonsoft.Json.JsonConvert.SerializeObject(o));
            w.Close();
        }


        private void Gioca_Click(object sender, RoutedEventArgs e)
        {
            c = primo.GetCartaGiocata();
            c1 = secondo.GetCartaGiocata();
            if ((c.CompareTo(c1) > 0 && c.StessoSeme(c1)) || (c1.StessoSeme(briscola) && !c.StessoSeme(briscola)))
            {
                temp = secondo;
                secondo = primo;
                primo = temp;
            }

            primo.AggiornaPunteggio(secondo);
            PuntiCpu.Content = $"{d["PuntiDiPrefisso"]} {cpu.GetNome()} {d["PuntiDiSuffisso"]}: {cpu.GetPunteggio()}";
            PuntiUtente.Content = $"{d["PuntiDiPrefisso"]} {g.GetNome()} {d["PuntiDiSuffisso"]}: {g.GetPunteggio()}";
            if (AggiungiCarte())
            {
                NelMazzoRimangono.Content = $"{d["NelMazzoRimangono"]} {m.GetNumeroCarte()} {d["carte"]}";
                CartaBriscola.Content = $"{d["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
                if (Briscola.IsVisible && m.GetNumeroCarte() == 0)
                {
                    NelMazzoRimangono.IsVisible = false;
                    Briscola.IsVisible = false;
                    if (avvisaTalloneFinito)
                    {
                        Notification not = new Notification
                        {
                            Title = $"{d["TalloneFinitoIntestazione"]}",
                            Body = $"{d["TalloneFinito"]}"
                        };
                        notification.ShowNotification(not);
                    }
                }
                Utente0.Source = g.GetImmagine(0);
                if (cpu.GetNumeroCarte() > 1)
                    Utente1.Source = g.GetImmagine(1);
                if (cpu.GetNumeroCarte() > 2)
                    Utente2.Source = g.GetImmagine(2);
                i.IsVisible = true;
                i1.IsVisible = true;
                Giocata0.IsVisible = false;
                Giocata1.IsVisible = false;
                if (cpu.GetNumeroCarte() == 2)
                {
                    Utente2.IsVisible = false;
                    Cpu2.IsVisible = false;
                }
                if (cpu.GetNumeroCarte() == 1)
                {
                    Utente1.IsVisible = false;
                    Cpu1.IsVisible = false;
                }
                    if (primo == cpu)
                    {
                        i1 = GiocaCpu();
                    if (cpu.GetCartaGiocata().StessoSeme(briscola))
                    {
                        Notification not = new Notification
                        {
                            Title = $"{d["GiocataCarta"]}",
                            Body = $"{d["LaCPUHaGiocatoIl"]} {cpu.GetCartaGiocata().GetValore() + 1} {d["di"]} {d["Briscola"]}"
                        };
                        notification.ShowNotification(not);

                    }
                    else if (cpu.GetCartaGiocata().GetPunteggio() > 0)
                    {
                        Notification not = new Notification
                        {
                            Title = $"{d["GiocataCarta"]}",
                            Body = $"{d["LaCPUHaGiocatoIl"]} {cpu.GetCartaGiocata().GetValore() + 1} {d["di"]} {cpu.GetCartaGiocata().GetSemeStr()}"
                        };
                        notification.ShowNotification(not);

                    }
                }
            }
            else
            {
                String s;
                puntiUtente+=g.GetPunteggio();
                puntiCpu += cpu.GetPunteggio();
                if (puntiUtente == puntiCpu)
                    s = $"{d["PartitaPatta"]}";
                else
                {
                    if (puntiUtente > puntiCpu)
                        s = $"{d["HaiVinto"]}";
                    else
                        s = $"{d["HaiPerso"]}";
                    s = $"{s} {d["per"]} {Math.Abs(puntiUtente - puntiCpu)} {d["punti"]}";
                }
                if (partite % 2 == 1) {
                    fpRisultrato.Content = $"{d["PartitaFinita"]}. {s}. {d["NuovaPartita"]}?";
                    fpShare.IsVisible=true;
                    fpShare.IsEnabled=true;
                } else {
                    fpRisultrato.Content = $"{d["PartitaFinita"]}. {s}. {d["EffettuaSecondaPartita"]}?";
                    fpShare.IsVisible = false;
                }
                Applicazione.IsVisible = false;
                FinePartita.IsVisible = true;
                fpShare.IsEnabled = helper.GetLivello()==3;
                partite++;
            }
            btnGiocata.IsVisible = false;
        }
        private Image GiocaUtente(Image img)
        {
            UInt16 quale = 0;
            Image img1 = Utente0;
            if (img == Utente1)
            {
                quale = 1;
                img1 = Utente1;
            }
            if (img == Utente2)
            {
                quale = 2;
                img1 = Utente2;
            }
            if (primo == g)
                g.Gioca(quale);
            else
                g.Gioca(quale, primo, stessoSeme);

            Giocata0.IsVisible = true;
            Giocata0.Source = img1.Source;
            img1.IsVisible = false;
            return img1;
        }

        private void OnInfo_Click(object sender, RoutedEventArgs e)
        {
            Applicazione.IsVisible = false;
            GOpzioni.IsVisible = false;
            Info.IsVisible = true;
        }

        private void OnApp_Click(object sender, RoutedEventArgs e)
        {
            GOpzioni.IsVisible = false;
            Info.IsVisible = false;
            Applicazione.IsVisible = true;
        }
        private void OnOpzioni_Click(object sender, RoutedEventArgs e)
        {
            lsmazzi.Items.Clear();
            Info.IsVisible = false;
            Applicazione.IsVisible = false;
            GOpzioni.IsVisible = true;
            txtNomeUtente.Text = g.GetNome();
            txtCpu.Text = cpu.GetNome();
            cbCartaBriscola.IsChecked = briscolaDaPunti;
            cbAvvisaTallone.IsChecked = avvisaTalloneFinito;
            cbStessoSeme.IsChecked = stessoSeme;
            List<ListBoxItem> mazzi;
            List<String> path;
            cbLivello.SelectedIndex = helper.GetLivello() - 1;
            ListBoxItem item;
            String s1 = "";
            string dirs = System.IO.Path.Combine(App.path,"Mazzi");

            try
            {
                path = new List<String>(Directory.EnumerateDirectories(dirs));
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                path = new List<string>();
            }
            for (UInt16 i=0; i<path.Count; i++)
            {
                    path[i] = path[i].Substring(path[i].LastIndexOf(System.IO.Path.DirectorySeparatorChar)+1);

            }
            if (!path.Contains("Napoletano"))
                    path.Add("Napoletano");
            path.Sort();
            foreach (String s in path)
            {
                item = new ListBoxItem();
                item.Content = s;
                lsmazzi.Items.Add(item);

            }
            

        }

        private void NuovaPartita(bool vecchioStessoSeme)
        {
            if (o.livello != helper.GetLivello()) {
                Notification not = new Notification
                {
                    Title = $"{d["LivelloCambiato"]}",
                    Body = $"{d["PartitaRiavviata"]}"
                };
                notification.ShowNotification(not);
                puntiCpu = puntiUtente = 0;
                partite = 0;
            }
            if (stessoSeme!=vecchioStessoSeme)
            {
                if (stessoSeme) {
                    Notification not = new Notification
                    {
                        Title = d["VarianteBussataTitolo"] as string,
                        Body = d["VarianteBussataTesto"] as string
                    };
                    notification.ShowNotification(not);
                }
                else
                {
                    Notification not = new Notification
                    {
                        Title = d["VarianteNormaleTitolo"] as string,
                        Body = d["VarianteNormaleTesto"] as string
                    };
                    notification.ShowNotification(not);

                }
                puntiCpu = puntiUtente = 0;
                partite = 0;
            }
            if (partite % 2 == 0)
                puntiUtente = puntiCpu = 0;
            bool cartaBriscola = true;
            if (cbCartaBriscola.IsChecked == false)
                cartaBriscola = false;
            e = new ElaboratoreCarteBriscola(cartaBriscola);
            m = new Mazzo(e);
            Carta.SetHelper(cartaHelper= new org.altervista.numerone.framework.briscola.CartaHelper(ElaboratoreCarteBriscola.GetCartaBriscola()));
            m.SetNome(o.nomeMazzo);
            briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
            g = new Giocatore(new GiocatoreHelperUtente(), g.GetNome(), 3);
            switch (o.livello)
            {
                case 1: helper = new GiocatoreHelperCpu0(ElaboratoreCarteBriscola.GetCartaBriscola()); break;
                case 2: helper = new GiocatoreHelperCpu1(ElaboratoreCarteBriscola.GetCartaBriscola()); break;
                default: helper = new GiocatoreHelperCpu2(ElaboratoreCarteBriscola.GetCartaBriscola()); break;

            }
            cpu = new Giocatore(helper, cpu.GetNome(), 3);
            g = new Giocatore(new GiocatoreHelperUtente(), g.GetNome(), 3);
            for (UInt16 i = 0; i < 3; i++)
            {
                g.AddCarta(m);
                cpu.AddCarta(m);

            }
            Utente0.Source = g.GetImmagine(0);
            Utente0.IsVisible = true;
            Utente1.Source = g.GetImmagine(1);
            Utente1.IsVisible = true;
            Utente2.Source = g.GetImmagine(2);
            Utente2.IsVisible = true;
            Cpu0.Source = cartaCpu.Source;
            Cpu0.IsVisible = true;
            Cpu1.Source = cartaCpu.Source;
            Cpu1.IsVisible = true;
            Cpu2.Source = cartaCpu.Source;
            Cpu2.IsVisible = true;
            Giocata0.IsVisible = false;
            Giocata1.IsVisible = false;
            PuntiCpu.Content = $"{d["PuntiDiPrefisso"]} {cpu.GetNome()} {d["PuntiDiSuffisso"]}: {cpu.GetPunteggio()}";
            PuntiUtente.Content = $"{d["PuntiDiPrefisso"]} {g.GetNome()} {d["PuntiDiSuffisso"]}: {g.GetPunteggio()}";
            NelMazzoRimangono.Content = $"{d["NelMazzoRimangono"]} {m.GetNumeroCarte()} {d["carte"]}";
            NelMazzoRimangono.IsVisible = true;
            CartaBriscola.Content = $"{d["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
            CartaBriscola.IsVisible = true;
            Briscola.Source = briscola.GetImmagine();
            Briscola.IsVisible = true;
            primaUtente = !primaUtente;
            btnGiocata.IsVisible=false;
            if (primaUtente)
            {
                primo = g;
                secondo = cpu;
            }
            else {
                primo = cpu;
                secondo = g;
                i1=GiocaCpu();
            }
            Briscola.Source = briscola.GetImmagine();

        }
        private void OnOkFp_Click(object sender, RoutedEventArgs evt)
        {
            FinePartita.IsVisible = false;
            NuovaPartita(stessoSeme);
            Applicazione.IsVisible = true;

        }
        private void OnCancelFp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private Image GiocaCpu()
        {
            UInt16 quale = 0;
            Image img1 = Cpu0;
            if (primo == cpu)
                cpu.Gioca(0);
            else
                cpu.Gioca(0, g, stessoSeme);
            quale = cpu.GetICartaGiocata();
            if (quale == 1)
                img1 = Cpu1;
            if (quale == 2)
                img1 = Cpu2;
            Giocata1.IsVisible = true;
            Giocata1.Source = cpu.GetCartaGiocata().GetImmagine();
            img1.IsVisible = false;
            return img1;
        }
        private static bool AggiungiCarte()
        {
            try
            {
                primo.AddCarta(m);
                secondo.AddCarta(m);
            }
            catch (IndexOutOfRangeException e)
            {
                return false;
            }
            return true;
        }

        private void Image_Tapped(object Sender, RoutedEventArgs arg)
        {
	    if (btnGiocata.IsVisible)
            	return;
            Image img = (Image) ((Button)Sender).Content;
            try
            {
                i = GiocaUtente(img);

            }
            catch (Exception ex)
            {
                Notification not = new Notification
                {
                    Title = d["MossaNonConsentitaTitolo"] as string,
                    Body = d["MossaNonConsentitaTesto"] as string
                };
                notification.ShowNotification(not);
                return;
            }
            if (secondo == cpu)
                i1 = GiocaCpu();
            btnGiocata.IsVisible= true;
        }
        public void OnOk_Click(Object source, RoutedEventArgs evt)
        {
            bool vecchioStessoSeme = o.stessoSeme;
            g.SetNome(txtNomeUtente.Text);
            cpu.SetNome(txtCpu.Text);
            if (cbCartaBriscola.IsChecked == false)
                briscolaDaPunti = false;
            else
                briscolaDaPunti = true;
            if (cbAvvisaTallone.IsChecked == false)
                avvisaTalloneFinito = false;
            else
                avvisaTalloneFinito = true;
            if (cbStessoSeme.IsChecked == false)
                stessoSeme = false;
            else
                stessoSeme = true;
            NomeUtente.Content = g.GetNome();
            NomeCpu.Content = cpu.GetNome();
            ListBoxItem i = (ListBoxItem) lsmazzi.SelectedItem;
            if (i != null)
            {
                m.SetNome((string)i.Content);
                Carta.CaricaImmagini(App.path, m, 40, d["bastoni"] as string, d["coppe"] as string, d["denari"] as string, d["spade"] as string, d["Fiori"] as string, d["Quadri"] as string, d["Cuori"] as string, d["Picche"] as string, "CBriscola.Avalonia");
                Utente0.Source = g.GetImmagine(0);
                Utente1.Source = g.GetImmagine(1);
                Utente2.Source = g.GetImmagine(2);
                try
                {
                    Giocata0.Source = g.GetCartaGiocata().GetImmagine();
                }
                catch (System.IndexOutOfRangeException ex) {; }
                try
                {
                    Giocata1.Source = cpu.GetCartaGiocata().GetImmagine();
                }
                catch (System.IndexOutOfRangeException ex) {; }

                briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
                Briscola.Source = briscola.GetImmagine();
                if (m.GetNome() != "Napoletano")
                    try
                    {
                        cartaCpu.Source = new Bitmap(System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(App.path,"Mazzi"),m.GetNome()),"retro carte pc.png"));
                    }
                    catch (System.IO.FileNotFoundException ex)
                    {
                        asset = AssetLoader.Open(new Uri($"avares://{Assembly.GetEntryAssembly().GetName().Name}/Assets/retro_carte_pc.png"));
                        cartaCpu.Source = new Bitmap(asset);

                    }
                else
                {
                    asset = AssetLoader.Open(new Uri($"avares://{Assembly.GetEntryAssembly().GetName().Name}/Assets/retro_carte_pc.png"));
                    cartaCpu.Source = new Bitmap(asset);
                }
                Cpu0.Source = cartaCpu.Source;
                Cpu1.Source = cartaCpu.Source;
                Cpu2.Source = cartaCpu.Source;
                CartaBriscola.Content = $"{d["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
            }
            o=new Opzioni();
            o.nomeMazzo= m.GetNome();
            o.NomeCpu = cpu.GetNome();
            o.NomeUtente=g.GetNome();
            o.briscolaDaPunti = briscolaDaPunti;
            o.avvisaTalloneFinito = avvisaTalloneFinito;
            o.livello = (UInt16) (cbLivello.SelectedIndex+1);
            o.stessoSeme = stessoSeme;
            GOpzioni.IsVisible = false;
            Applicazione.IsVisible = true;
            SalvaOpzioni(o);
            if (o.livello != helper.GetLivello() || stessoSeme != vecchioStessoSeme)
            {
                NuovaPartita(vecchioStessoSeme);
            }

        }

        private void OnFPShare_Click(object sender, RoutedEventArgs e)
        {
            string s="";
            if (stessoSeme)
                s = "bussata%20";

            var psi = new ProcessStartInfo
            {
                FileName = $"https://twitter.com/intent/tweet?text=Con%20la%20CBriscola.avalonia%20{s}la%20partita%20numero%20{partite}%20{g.GetNome()}%20contro%20{cpu.GetNome()}%20%C3%A8%20finita%20{puntiUtente}%20a%20{puntiCpu}%20col%20mazzo%20{m.GetNome()}%20su%20sistema%20operativo%20{App.SistemaOperativo}&url=https%3A%2F%2Fgithub.com%2Fnumerunix%2Fcbriscola.Avalonia",
                UseShellExecute = true
            };
            fpShare.IsEnabled = false;
            Process.Start(psi);
        }


        private void OnSito_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "https://github.com/numerunix/cbriscola.Avalonia",
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}