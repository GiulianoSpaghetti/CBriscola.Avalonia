using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;
using System.Runtime.CompilerServices;
using Avalonia.Platform;
using Avalonia;
using System.Reflection;
using Avalonia.Media.Imaging;
using System.Globalization;
using org.altervista.numerone.framework;
using static System.Net.WebRequestMethods;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Shapes;

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
        private static bool avvisaTalloneFinito = true, briscolaDaPunti = false, primaUtente = true;
        private static GiocatoreHelperCpu helper;
        private ResourceDictionary d;
        private ElaboratoreCarteBriscola e;
        private Stream asset;
        private WindowNotificationManager notification;
        private Opzioni o;
        public MainWindow()
        {
            this.InitializeComponent();
            notification = new WindowNotificationManager(this) { Position = NotificationPosition.BottomRight };
            d = this.FindResource(CultureInfo.CurrentCulture.TwoLetterISOLanguageName) as ResourceDictionary;
            if (d==null)
                d = this.FindResource("it") as ResourceDictionary;
            e = new ElaboratoreCarteBriscola(false);
            m = new Mazzo(e);
            o = LeggiOpzioni(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            m.SetNome(o.nomeMazzo);

            Carta.Inizializza(m, 40, CartaHelperBriscola.GetIstanza(e), d);
            if (o.nomeMazzo == "Napoletano")
            {
                asset = AssetLoader.Open(new Uri($"avares://{Assembly.GetEntryAssembly().GetName().Name}/Assets/retro_carte_pc.png"));
                cartaCpu.Source = new Bitmap(asset);

            }
            else
                try
                {  
                        cartaCpu.Source = new Bitmap($"{App.path}{App.separator}Mazzi{App.separator}{m.GetNome()}{App.separator}retro carte pc.png");
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
            briscolaDaPunti = o.briscolaDaPunti;
            avvisaTalloneFinito = o.avvisaTalloneFinito;
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

        }

        private Opzioni CaricaOpzioni()
        {
            Opzioni o;
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            o = LeggiOpzioni(dir);
            return o;
        }
        private Opzioni LeggiOpzioni(String folder)
        {
            Opzioni o;
                if (!Directory.Exists($"{folder}{App.separator}CBriscola.Avalonia"))
               Directory.CreateDirectory($"{folder}{App.separator}CBriscola.Avalonia");
            StreamReader file;
            try
            {
                file = new StreamReader($"{folder}{App.separator}CBriscola.Avalonia{App.separator}opzioni.json");
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
                SalvaOpzioni(folder, o);
                return o;
            }
            string s = file.ReadToEnd();
            file.Close();
            o = Newtonsoft.Json.JsonConvert.DeserializeObject<Opzioni>(s);
            return o;
        }

        private void SalvaOpzioni(String folder, Opzioni o)
        {
            StreamWriter w = new StreamWriter($"{folder}{App.separator}CBriscola.Avalonia{App.separator}opzioni.json");
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
                        notification.Show(new Notification($"{d["TalloneFinitoIntestazione"]}", d["TalloneFinito"] as string));
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
                        notification.Show(new Notification($"{d["GiocataCarta"]}", $"{d["LaCPUHaGiocatoIl"]} {cpu.GetCartaGiocata().GetValore() + 1} {d["di"]} {d["Briscola"]}"));
                    else if (cpu.GetCartaGiocata().GetPunteggio() > 0)
                        notification.Show(new Notification($"{d["GiocataCarta"]}", $"{d["LaCPUHaGiocatoIl"]} {cpu.GetCartaGiocata().GetValore() + 1} {d["di"]} {cpu.GetCartaGiocata().GetSemeStr()}"));
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
            Giocata0.IsVisible = true;
            Giocata0.Source = img1.Source;
            img1.IsVisible = false;
            g.Gioca(quale);
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
            List<ListBoxItem> mazzi;
            List<String> path;
            cbLivello.SelectedIndex = helper.GetLivello() - 1;
            ListBoxItem item;
            String s1 = "";
            string dirs = $"{App.path}{App.separator}Mazzi";

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
                    path[i] = path[i].Substring(path[i].LastIndexOf(App.separator) + 1);

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

        private void NuovaPartita()
        {
            if (o.livello != helper.GetLivello()) {
                notification.Show(new Notification($"{d["LivelloCambiato"]}", $"{d["PartitaRiavviata"]}"));
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
            NuovaPartita();
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
                cpu.Gioca(0, g);
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
            i = GiocaUtente(img);
            if (secondo == cpu)
                i1 = GiocaCpu();
            btnGiocata.IsVisible= true;
        }
        public void OnOk_Click(Object source, RoutedEventArgs evt)
        {
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
            NomeUtente.Content = g.GetNome();
            NomeCpu.Content = cpu.GetNome();
            ListBoxItem i = (ListBoxItem) lsmazzi.SelectedItem;
            if (i != null)
            {
                m.SetNome((string)i.Content);
                Carta.CaricaImmagini(m, 40, CartaHelperBriscola.GetIstanza(e), d);
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
                        cartaCpu.Source = new Bitmap($"{App.path}{App.separator}Mazzi{App.separator}{m.GetNome()}{App.separator}retro carte pc.png");
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
            SalvaOpzioni(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), o);
            GOpzioni.IsVisible = false;
            Applicazione.IsVisible = true;
            if (o.livello != helper.GetLivello())
                NuovaPartita();

        }

        private void OnFPShare_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = $"https://twitter.com/intent/tweet?text=Con%20la%20CBriscola%20la%20partita%20numero%20{partite}%20{g.GetNome()}%20contro%20{cpu.GetNome()}%20%C3%A8%20finita%20{puntiUtente}%20a%20{puntiCpu}%20col%20mazzo%20{m.GetNome()}%20su%20sistema%20operativo%20{App.SistemaOperativo}&url=https%3A%2F%2Fgithub.com%2Fnumerunix%2Fcbriscola.Avalonia",
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
