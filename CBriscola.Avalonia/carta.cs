/*
 *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 0.1
 *
 *  Created by numerunix on 22/05/22.
 *  Copyright 2022 Some rights reserved.
 *
 */


using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace CBriscola.Avalonia
{
    class carta
    {
        private ushort seme,
                   valore,
                   punteggio;
        private string semeStr;
        private cartaHelperBriscola helper;
        private static carta[] carte = new carta[40];
        private Bitmap img;
        private carta(ushort n, cartaHelperBriscola h)
        {
            helper = h;
            seme = helper.getSeme(n);
            valore = helper.getValore(n);
            punteggio = helper.getPunteggio(n);
        }
        public static void inizializza(ushort n, cartaHelperBriscola h, IAssetLoader asset, ResourceDictionary d)
        {
            for (ushort i = 0; i < n; i++)
            {
                carte[i] = new carta(i, h);
            }
            CaricaImmagini(n, h, asset, d);
        }
        public static carta getCarta(ushort quale) { return carte[quale]; }
        public ushort getSeme() { return seme; }
        public ushort getValore() { return valore; }
        public ushort getPunteggio() { return punteggio; }
        public string getSemeStr() { return semeStr; }
        public bool stessoSeme(carta c1) { if (c1 == null) return false; else return seme == c1.getSeme(); }
        public int CompareTo(carta c1)
        {
            if (c1 == null)
                return 1;
            else
                return helper.CompareTo(helper.getNumero(getSeme(), getValore()), helper.getNumero(c1.getSeme(), c1.getValore()));
        }

        public override string ToString()
        {
            return $"{valore + 1} di {semeStr}{(stessoSeme(helper.getCartaBriscola()) ? "*" : " ")} ";
        }

        public static Bitmap getImmagine(ushort quale)
        {
            return carte[quale].img;
        }

        public Bitmap getImmagine()
        {
            return img;
        }

        public static void CaricaImmagini(ushort n, cartaHelperBriscola helper, IAssetLoader assets, ResourceDictionary d)
        {
            Stream asset;

            for (ushort i = 0; i < n; i++)
            {
                asset = assets.Open(new Uri($"avares://{Assembly.GetEntryAssembly().GetName().Name}/resources/images/" + i + ".png"));

                carte[i].img = new Bitmap(asset);
                carte[i].semeStr = helper.getSemeStr(i, d);
            }
        }
    }
}
