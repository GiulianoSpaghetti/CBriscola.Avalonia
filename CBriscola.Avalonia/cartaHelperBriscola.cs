/*
  *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 1.1.3
 *
 *  Created by Giulio Sorrentino (numerone) on 29/01/23.
 *  Copyright 2023 Some rights reserved.
 *
 */

using Avalonia.Controls;
using System;
using System.Windows;

namespace org.altervista.numerone.framework
{
    public class CartaHelperBriscola : CartaHelper {
		private readonly UInt16 CartaBriscola;
		public CartaHelperBriscola(UInt16 briscola) { CartaBriscola = briscola; }
		private static CartaHelperBriscola istanza;
		public static CartaHelperBriscola GetIstanza(ElaboratoreCarteBriscola e) {
			if (istanza == null) {
				if (e == null)
					throw new ArgumentNullException("Chiamata a CartaHelperBriscola::getIstanza con istanza==NULL e e==NULL");
				istanza = new CartaHelperBriscola(ElaboratoreCarteBriscola.GetCartaBriscola());
			}
			return istanza;
		}
		public UInt16 GetSeme(UInt16 Carta) {
			return (UInt16)(Carta / 10);
		}
		public UInt16 GetValore(UInt16 Carta) {
			return (UInt16)(Carta % 10);
		}
		public UInt16 GetPunteggio(UInt16 Carta) {
			UInt16 valore = 0;
			switch (Carta % 10) {
				case 0: valore = 11; break;
				case 2: valore = 10; break;
				case 9: valore = 4; break;
				case 8: valore = 3; break;
				case 7: valore = 2; break;
			}
			return valore;
		}
        public string GetSemeStr(UInt16 carta, String mazzo, Avalonia.Controls.ResourceDictionary d)
        {
            string s = "a";
            if (mazzo == "Bergamasco" || mazzo == "Bolognese" || mazzo == "Bresciano" || mazzo == "Napoletano" || mazzo == "Romagnolo" || mazzo == "Sardo" || mazzo == "Siciliano" || mazzo == "Trientino" || mazzo == "Trevigiano" || mazzo == "Trentino" || mazzo == "Triestino")
                switch (carta / 10)
                {
                    case 0: s = d["bastoni"] as String; break;
                    case 1: s = d["coppe"] as String; break;
                    case 2: s = d["denari"] as String; break;
                    case 3: s = d["spade"] as String; break;
                }
            else
                switch (carta / 10)
                {
                    case 0: s = d["Fiori"] as String; break;
                    case 1: s = d["Quadri"] as String; break;
                    case 2: s = d["Cuori"] as String; break;
                    case 3: s = d["Picche"] as String; break;
                }
            return s;
        }


        public UInt16 GetNumero(UInt16 seme, UInt16 valore) {
			if (seme > 4 || valore > 9)
				throw new ArgumentException($"Chiamato CartaHelperBriscola::getNumero con seme={seme} e valore={valore}");
			return (UInt16)(seme * 10 + valore);
		}

		public Carta GetCartaBriscola() { return Carta.GetCarta(CartaBriscola); }

		public int CompareTo(UInt16 Carta, UInt16 Carta1) {
			UInt16 punteggio = GetPunteggio(Carta),
				   punteggio1 = GetPunteggio(Carta1),
				   valore = GetValore(Carta),
				   valore1 = GetValore(Carta1),
				   semeBriscola = GetSeme(CartaBriscola),
				   semeCarta = GetSeme(Carta),
					  semeCarta1 = GetSeme(Carta1);
			if (punteggio < punteggio1)
				return 1;
			else if (punteggio > punteggio1)
				return -1;
			else {
				if (valore < valore1 || (semeCarta1 == semeBriscola && semeCarta != semeBriscola))
					return 1;
				else if (valore > valore1 || (semeCarta == semeBriscola && semeCarta1 != semeBriscola))
					return -1;
				else
					return 0;
			}
		}
	}
}