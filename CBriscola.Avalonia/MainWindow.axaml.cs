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

namespace CBriscola.Avalonia
{
    public partial class MainWindow : Window
    {
        private static giocatore g, cpu, primo, secondo, temp;
        private static mazzo m;
        private static carta c, c1, briscola;
        private static Image cartaCpu = new Image();
        private static Image i, i1;
        private static bool avvisaTalloneFinito = true, briscolaDaPunti = false;
        private ResourceDictionary d;
        elaboratoreCarteBriscola e;
        public MainWindow()
        {
            this.InitializeComponent();
            d = this.FindResource(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName) as ResourceDictionary;
            if (d==null)
                d = this.FindResource("it") as ResourceDictionary;
            e = new elaboratoreCarteBriscola(false);
            m = new mazzo(e);

            IAssetLoader assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            Stream asset = assets.Open(new Uri($"avares://{Assembly.GetEntryAssembly().GetName().Name}/resources/images/retro_carte_pc.png"));

            m.setNome("Napoletano");
            cartaCpu.Source = new Bitmap(asset);

            carta.inizializza(40, cartaHelperBriscola.getIstanza(e), assets, d);

            g = new giocatore(new giocatoreHelperUtente(), "numerone", 3);
            cpu = new giocatore(new giocatoreHelperCpu(elaboratoreCarteBriscola.getCartaBriscola()), "Cpu", 3);

            primo = g;
            secondo = cpu;
            briscola = carta.getCarta(elaboratoreCarteBriscola.getCartaBriscola());
            Image[] img = new Image[3];
            for (UInt16 i = 0; i < 3; i++)
            {
                g.addCarta(m);
                cpu.addCarta(m);

            }
            NomeUtente.Content = g.getNome();
            NomeCpu.Content = cpu.getNome();
            Utente0.Source = g.getImmagine(0);
            Utente1.Source = g.getImmagine(1);
            Utente2.Source = g.getImmagine(2);
            Cpu0.Source = cartaCpu.Source;
            Cpu1.Source = cartaCpu.Source;
            Cpu2.Source = cartaCpu.Source;
            PuntiCpu.Content = $"{d["PuntiDiPrefisso"]} {cpu.getNome()} {d["PuntiDiSuffisso"]}: {cpu.getPunteggio()}";
            PuntiUtente.Content = $"{d["PuntiDiPrefisso"]} {g.getNome()} {d["PuntiDiSuffisso"]}: {g.getPunteggio()}";
            NelMazzoRimangono.Content = $"{d["NelMazzoRimangono"]} {m.getNumeroCarte()} {d["carte"]}";
            CartaBriscola.Content = $"{d["IlSemeDiBriscolaE"]}: {briscola.getSemeStr()}";
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
            Briscola.Source = briscola.getImmagine();
            btnGiocata.Content = $"{d["giocataVista"]}";
        }

        private void Gioca_Click(object sender, RoutedEventArgs e)
        {
            Informazioni.Content = "";
            c = primo.getCartaGiocata();
            c1 = secondo.getCartaGiocata();
            if ((c.CompareTo(c1) > 0 && c.stessoSeme(c1)) || (c1.stessoSeme(briscola) && !c.stessoSeme(briscola)))
            {
                temp = secondo;
                secondo = primo;
                primo = temp;
            }

            primo.aggiornaPunteggio(secondo);
            PuntiCpu.Content = $"{d["PuntiDiPrefisso"]} {cpu.getNome()} {d["PuntiDiSuffisso"]}: {cpu.getPunteggio()}";
            PuntiUtente.Content = $"{d["PuntiDiPrefisso"]} {g.getNome()} {d["PuntiDiSuffisso"]}: {g.getPunteggio()}";
            if (aggiungiCarte())
            {
                NelMazzoRimangono.Content = $"{d["NelMazzoRimangono"]} {m.getNumeroCarte()} {d["carte"]}";
                CartaBriscola.Content = $"{d["IlSemeDiBriscolaE"]}: {briscola.getSemeStr()}";
                if (Briscola.IsVisible && m.getNumeroCarte() == 0)
                {
                    NelMazzoRimangono.IsVisible = false;
                    Briscola.IsVisible = false;
                    if (avvisaTalloneFinito)
                        Informazioni.Content = d["TalloneFinito"] as string;
                }
                Utente0.Source = g.getImmagine(0);
                if (cpu.getNumeroCarte() > 1)
                    Utente1.Source = g.getImmagine(1);
                if (cpu.getNumeroCarte() > 2)
                    Utente2.Source = g.getImmagine(2);
                i.IsVisible = true;
                i1.IsVisible = true;
                Giocata0.IsVisible = false;
                Giocata1.IsVisible = false;
                if (cpu.getNumeroCarte() == 2)
                {
                    Utente2.IsVisible = false;
                    Cpu2.IsVisible = false;
                }
                if (cpu.getNumeroCarte() == 1)
                {
                    Utente1.IsVisible = false;
                    Cpu1.IsVisible = false;
                }
                if (primo == cpu)
                {
                    i1 = giocaCpu();
                    if (cpu.getCartaGiocata().stessoSeme(briscola))
                        Informazioni.Content = $"{d["LaCPUHaGiocatoIl"]} {cpu.getCartaGiocata().getValore() + 1} {d["di"]} {d["Briscola"]}";
                    else if (cpu.getCartaGiocata().getPunteggio() > 0)
                        Informazioni.Content = $"{d["LaCPUHaGiocatoIl"]} {cpu.getCartaGiocata().getValore() + 1} {d["di"]} {cpu.getCartaGiocata().getSemeStr()}";
                }
            }
            else
            {
                String s;
                if (g.getPunteggio() == cpu.getPunteggio())
                    s = $"{d["PartitaPatta"]}";
                else
                {
                    if (g.getPunteggio() > cpu.getPunteggio())
                        s = $"{d["HaiVinto"]}";
                    else
                        s = $"{d["HaiPerso"]}";
                    s = $"{s} {d["per"]} {Math.Abs(g.getPunteggio() - cpu.getPunteggio())} {d["punti"]}";
                }
                fpRisultrato.Content = $"{d["PartitaFinita"]}. {s} {d["NuovaPartita"]}?";
                Applicazione.IsVisible = false;
                FinePartita.IsVisible = true;
                fpShare.IsEnabled = true;
            }
            btnGiocata.IsVisible = false;
        }
        private Image giocaUtente(Image img)
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
            g.gioca(quale);
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
            txtNomeUtente.Text = g.getNome();
            txtCpu.Text = cpu.getNome();
            cbCartaBriscola.IsChecked = briscolaDaPunti;
            cbAvvisaTallone.IsChecked = avvisaTalloneFinito;
        }

        private void OnOkFp_Click(object sender, RoutedEventArgs evt)
        {
            bool cartaBriscola = true;
            FinePartita.IsVisible = false;
            if (cbCartaBriscola.IsChecked == false)
                cartaBriscola = false;
            e = new elaboratoreCarteBriscola(cartaBriscola);
            m = new mazzo(e);
            briscola = carta.getCarta(elaboratoreCarteBriscola.getCartaBriscola());
            g = new giocatore(new giocatoreHelperUtente(), g.getNome(), 3);
            cpu = new giocatore(new giocatoreHelperCpu(elaboratoreCarteBriscola.getCartaBriscola()), cpu.getNome(), 3);
            for (UInt16 i = 0; i < 3; i++)
            {
                g.addCarta(m);
                cpu.addCarta(m);

            }
            Utente0.Source = g.getImmagine(0);
            Utente0.IsVisible = true;
            Utente1.Source = g.getImmagine(1);
            Utente1.IsVisible = true;
            Utente2.Source = g.getImmagine(2);
            Utente2.IsVisible = true;
            Cpu0.Source = cartaCpu.Source;
            Cpu0.IsVisible = true;
            Cpu1.Source = cartaCpu.Source;
            Cpu1.IsVisible = true;
            Cpu2.Source = cartaCpu.Source;
            Cpu2.IsVisible = true;
            Giocata0.IsVisible = false;
            Giocata1.IsVisible = false;
            PuntiCpu.Content = $"{d["PuntiDiPrefisso"]} {cpu.getNome()} {d["PuntiDiSuffisso"]}: {cpu.getPunteggio()}";
            PuntiUtente.Content = $"{d["PuntiDiPrefisso"]} {g.getNome()} {d["PuntiDiSuffisso"]}: {g.getPunteggio()}";
            NelMazzoRimangono.Content = $"{d["NelMazzoRimangono"]} {m.getNumeroCarte()} {d["carte"]}";
            NelMazzoRimangono.IsVisible = true;
            CartaBriscola.Content = $"{d["IlSemeDiBriscolaE"]}: {briscola.getSemeStr()}";
            CartaBriscola.IsVisible = true;
            Briscola.Source = briscola.getImmagine();
            Briscola.IsVisible = true;
            primo = g;
            secondo = cpu;
            Briscola.Source = briscola.getImmagine();
            Applicazione.IsVisible = true;
        }
        private void OnCancelFp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private Image giocaCpu()
        {
            UInt16 quale = 0;
            Image img1 = Cpu0;
            if (primo == cpu)
                cpu.gioca(0);
            else
                cpu.gioca(0, g);
            quale = cpu.getICartaGiocata();
            if (quale == 1)
                img1 = Cpu1;
            if (quale == 2)
                img1 = Cpu2;
            Giocata1.IsVisible = true;
            Giocata1.Source = cpu.getCartaGiocata().getImmagine();
            img1.IsVisible = false;
            return img1;
        }
        private static bool aggiungiCarte()
        {
            try
            {
                primo.addCarta(m);
                secondo.addCarta(m);
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
            i = giocaUtente(img);
            if (secondo == cpu)
                i1 = giocaCpu();
            btnGiocata.IsVisible= true;
        }
        public void OnOk_Click(Object source, RoutedEventArgs evt)
        {
            g.setNome(txtNomeUtente.Text);
            cpu.setNome(txtCpu.Text);
            if (cbCartaBriscola.IsChecked == false)
                briscolaDaPunti = false;
            else
                briscolaDaPunti = true;
            if (cbAvvisaTallone.IsChecked == false)
                avvisaTalloneFinito = false;
            else
                avvisaTalloneFinito = true;
            NomeUtente.Content = g.getNome();
            NomeCpu.Content = cpu.getNome();


            GOpzioni.IsVisible = false;
            Applicazione.IsVisible = true;

        }

        private void OnFPShare_Click(object sender, RoutedEventArgs e)
        {
            var psi = new ProcessStartInfo
            {
                FileName = $"https://twitter.com/intent/tweet?text=Con%20la%20CBriscola%20la%20partita%20{g.getNome()}%20contro%20{cpu.getNome()}%20%C3%A8%20finita%20{g.getPunteggio()}%20a%20{cpu.getPunteggio()}&url=https%3A%2F%2Fgithub.com%2Fnumerunix%2Fcbriscola.Avalonia",
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
