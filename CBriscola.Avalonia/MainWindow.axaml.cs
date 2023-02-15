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

namespace CBriscola.Avalonia
{
    public partial class MainWindow : Window
    {
        private static Giocatore g, cpu, primo, secondo, temp;
        private static Mazzo m;
        private static Carta c, c1, briscola;
        private static Image cartaCpu = new Image();
        private static Image i, i1;
        private static bool avvisaTalloneFinito = true, briscolaDaPunti = false;
        private ResourceDictionary d;
        private ElaboratoreCarteBriscola e;
        private IAssetLoader assets;
        private Stream asset;
        private List<ListBoxItem> mazzi;
        public MainWindow()
        {
            this.InitializeComponent();
            d = this.FindResource(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName) as ResourceDictionary;
            if (d==null)
                d = this.FindResource("it") as ResourceDictionary;
            e = new ElaboratoreCarteBriscola(false);
            m = new Mazzo(e);
            Task<Opzioni> o1 = LeggiOpzioni(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            Opzioni o = o1.Result;
            assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            m.SetNome(o.nomeMazzo);

            Carta.Inizializza(m, 40, CartaHelperBriscola.GetIstanza(e), assets, d);
            if (o.nomeMazzo == "Napoletano")
            {
                asset = assets.Open(new Uri($"avares://{Assembly.GetEntryAssembly().GetName().Name}/resources/images/retro_carte_pc.png"));
                cartaCpu.Source = new Bitmap(asset);

            } 
            else
                cartaCpu.Source = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\wxBriscola\\Mazzi\\" + m.GetNome() + "\\retro carte pc.png");

            g = new Giocatore(new GiocatoreHelperUtente(), o.NomeUtente, 3);
            cpu = new Giocatore(new GiocatoreHelperCpu(ElaboratoreCarteBriscola.GetCartaBriscola()), o.NomeCpu, 3);
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
            List<String> path;
            mazzi = new List<ListBoxItem>();
            ListBoxItem item;
            String s1;
//            String dirs = "/usr/share/wxBriscola/Mazzi"
            String dirs = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\wxBriscola\\Mazzi";
            try
            {
                path = new List<String>(Directory.EnumerateDirectories(dirs));
            }
            catch (System.IO.DirectoryNotFoundException ex)
            {
                path = new List<string>();

            }
            if (!path.Contains("Napoletano"))
                path.Add("\\Napoletano");
            path.Sort();
            foreach (String s in path)
            {
                item = new ListBoxItem();
                s1 = s.Substring(s.LastIndexOf("\\") + 1);
                item.Content = s1;
                mazzi.Add(item);
            }
            lsmazzi.Items = mazzi;
            lbmazzi.Content =$"{d["Mazzo"]}";
        }

        private async Task<Opzioni> CaricaOpzioni()
        {
            Opzioni o;
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            o = await LeggiOpzioni(dir);
            return o;
        }
        private async Task<Opzioni> LeggiOpzioni(String folder)
        {
            Opzioni o;
            if (!Directory.Exists(folder + "\\CBriscola.Avalonia"))
               Directory.CreateDirectory(folder + "\\CBriscola.Avalonia");
            StreamReader file;
            try
            {
                file = new StreamReader(folder + "\\CBriscola.Avalonia\\opzioni.json");
            }
            catch (FileNotFoundException ex)
            {
                o = new Opzioni();
                o.NomeUtente = "numerone";
                o.NomeCpu = "Cpu";
                o.briscolaDaPunti = false;
                o.avvisaTalloneFinito = true;
                o.nomeMazzo = "Napoletano";
                SalvaOpzioni(folder, o);
                return o;
            }
            string s = file.ReadToEnd();
            file.Close();
            o = Newtonsoft.Json.JsonConvert.DeserializeObject<Opzioni>(s);
            return o;
        }

        private async void SalvaOpzioni(String folder, Opzioni o)
        {
            StreamWriter w = new StreamWriter(folder + "\\CBriscola.Avalonia\\opzioni.json");
            w.Write(Newtonsoft.Json.JsonConvert.SerializeObject(o));
            w.Close();
        }


        private void Gioca_Click(object sender, RoutedEventArgs e)
        {
            Informazioni.Content = "";
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
                        Informazioni.Content = d["TalloneFinito"] as string;
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
                        Informazioni.Content = $"{d["LaCPUHaGiocatoIl"]} {cpu.GetCartaGiocata().GetValore() + 1} {d["di"]} {d["Briscola"]}";
                    else if (cpu.GetCartaGiocata().GetPunteggio() > 0)
                        Informazioni.Content = $"{d["LaCPUHaGiocatoIl"]} {cpu.GetCartaGiocata().GetValore() + 1} {d["di"]} {cpu.GetCartaGiocata().GetSemeStr()}";
                }
            }
            else
            {
                String s;
                if (g.GetPunteggio() == cpu.GetPunteggio())
                    s = $"{d["PartitaPatta"]}";
                else
                {
                    if (g.GetPunteggio() > cpu.GetPunteggio())
                        s = $"{d["HaiVinto"]}";
                    else
                        s = $"{d["HaiPerso"]}";
                    s = $"{s} {d["per"]} {Math.Abs(g.GetPunteggio() - cpu.GetPunteggio())} {d["punti"]}";
                }
                fpRisultrato.Content = $"{d["PartitaFinita"]}. {s} {d["NuovaPartita"]}?";
                Applicazione.IsVisible = false;
                FinePartita.IsVisible = true;
                fpShare.IsEnabled = true;
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
            Info.IsVisible = false;
            Applicazione.IsVisible = false;
            GOpzioni.IsVisible = true;
            txtNomeUtente.Text = g.GetNome();
            txtCpu.Text = cpu.GetNome();
            cbCartaBriscola.IsChecked = briscolaDaPunti;
            cbAvvisaTallone.IsChecked = avvisaTalloneFinito;
        }

        private void OnOkFp_Click(object sender, RoutedEventArgs evt)
        {
            bool cartaBriscola = true;
            FinePartita.IsVisible = false;
            if (cbCartaBriscola.IsChecked == false)
                cartaBriscola = false;
            e = new ElaboratoreCarteBriscola(cartaBriscola);
            m = new Mazzo(e);
            briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
            g = new Giocatore(new GiocatoreHelperUtente(), g.GetNome(), 3);
            cpu = new Giocatore(new GiocatoreHelperCpu(ElaboratoreCarteBriscola.GetCartaBriscola()), cpu.GetNome(), 3);
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
            primo = g;
            secondo = cpu;
            Briscola.Source = briscola.GetImmagine();
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
            Image img = (Image)Sender;
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
                Carta.CaricaImmagini(m, 40, CartaHelperBriscola.GetIstanza(e), assets, d);
                Utente0.Source = g.GetImmagine(0);
                Utente1.Source = g.GetImmagine(1);
                Utente2.Source = g.GetImmagine(2);

                briscola = Carta.GetCarta(ElaboratoreCarteBriscola.GetCartaBriscola());
                Briscola.Source = briscola.GetImmagine();
                if (m.GetNome() != "Napoletano")
                    //cartaCpu.Source = new Bitmap("/usr/share/wxBriscola/Mazzi/" + m.GetNome + "/retro carte pc.png");
                    cartaCpu.Source = new Bitmap(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\wxBriscola\\Mazzi\\" + m.GetNome() + "\\retro carte pc.png");
                else
                {
                    asset = assets.Open(new Uri($"avares://{Assembly.GetEntryAssembly().GetName().Name}/resources/images/retro_carte_pc.png"));
                    cartaCpu.Source = new Bitmap(asset);
                }
                Cpu0.Source = cartaCpu.Source;
                Cpu1.Source = cartaCpu.Source;
                Cpu2.Source = cartaCpu.Source;
                CartaBriscola.Content = $"{d["IlSemeDiBriscolaE"]}: {briscola.GetSemeStr()}";
            }
            Opzioni o=new Opzioni();
            o.nomeMazzo= m.GetNome();
            o.NomeCpu = cpu.GetNome();
            o.NomeUtente=g.GetNome();
            o.briscolaDaPunti = briscolaDaPunti;
            o.avvisaTalloneFinito = avvisaTalloneFinito;
            SalvaOpzioni(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), o);
            GOpzioni.IsVisible = false;
            Applicazione.IsVisible = true;

        }

        private void OnFPShare_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = $"https://twitter.com/intent/tweet?text=Con%20la%20CBriscola%20la%20partita%20{g.GetNome()}%20contro%20{cpu.GetNome()}%20%C3%A8%20finita%20{g.GetPunteggio()}%20a%20{cpu.GetPunteggio()}&url=https%3A%2F%2Fgithub.com%2Fnumerunix%2Fcbriscola.Avalonia",
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
