/*
 *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 0.1
 *
 *  Created by numerunix on 22/05/22.
 *  Copyright 2022 Some rights reserved.
 *
 */

using Avalonia.Controls;
using System;
using System.Resources;

namespace CBriscola.Avalonia
{
    class cartaHelperBriscola : cartaHelper
    {
        private ushort cartaBriscola;
        public cartaHelperBriscola(ushort briscola) { cartaBriscola = briscola; }
        private static cartaHelperBriscola istanza;
        public static cartaHelperBriscola getIstanza(elaboratoreCarteBriscola e)
        {
            if (istanza == null)
            {
                if (e == null)
                    throw new ArgumentNullException("Chiamata a cartaHelperBriscola::getIstanza con istanza==NULL e e==NULL");
                istanza = new cartaHelperBriscola(elaboratoreCarteBriscola.getCartaBriscola());
            }
            return istanza;
        }
        public ushort getSeme(ushort carta)
        {
            return (ushort)(carta / 10);
        }
        public ushort getValore(ushort carta)
        {
            return (ushort)(carta % 10);
        }
        public ushort getPunteggio(ushort carta)
        {
            ushort valore = 0;
            switch (carta % 10)
            {
                case 0: valore = 11; break;
                case 2: valore = 10; break;
                case 9: valore = 4; break;
                case 8: valore = 3; break;
                case 7: valore = 2; break;
            }
            return valore;
        }
        public string getSemeStr(ushort carta,ResourceDictionary d)
        {
            string s = "a";
            switch (carta / 10)
            {
                case 0: s = d["bastoni"] as string; break;
                case 1: s = d["coppe"] as string; break;
                case 2: s = d["denari"] as string; break;
                case 3: s = d["spade"] as string; break;
            }
            return s;
        }

        public ushort getNumero(ushort seme, ushort valore)
        {
            if (seme > 4 || valore > 9)
                throw new ArgumentException($"Chiamato cartaHelperBriscola::getNumero con seme={seme} e valore={valore}");
            return (ushort)(seme * 10 + valore);
        }

        public carta getCartaBriscola() { return carta.getCarta(cartaBriscola); }

        public int CompareTo(ushort carta, ushort carta1)
        {
            ushort punteggio = getPunteggio(carta),
                   punteggio1 = getPunteggio(carta1),
                   valore = getValore(carta),
                   valore1 = getValore(carta1),
                   semeBriscola = getSeme(cartaBriscola),
                   semeCarta = getSeme(carta),
                      semeCarta1 = getSeme(carta1);
            if (punteggio < punteggio1)
                return 1;
            else if (punteggio > punteggio1)
                return -1;
            else
            {
                if (valore < valore1 || semeCarta1 == semeBriscola && semeCarta != semeBriscola)
                    return 1;
                else if (valore > valore1 || semeCarta == semeBriscola && semeCarta1 != semeBriscola)
                    return -1;
                else
                    return 0;
            }
        }
    }
}