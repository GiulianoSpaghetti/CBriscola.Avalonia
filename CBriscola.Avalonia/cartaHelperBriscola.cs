/*
 *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 0.1
 *
 *  Created by numerunix on 22/05/22.
 *  Copyright 2022 Some rights reserved.
 *
 */

using Avalonia.Controls;
using CBriscola.Avalonia;
using System;

namespace CBriscola {
	class cartaHelperBriscola : cartaHelper {
		private UInt16 cartaBriscola;
		public cartaHelperBriscola(UInt16 briscola) { cartaBriscola = briscola; }
		private static cartaHelperBriscola istanza;
		public static cartaHelperBriscola getIstanza(elaboratoreCarteBriscola e) {
			if (istanza == null) {
				if (e == null)
					throw new ArgumentNullException("Chiamata a cartaHelperBriscola::getIstanza con istanza==NULL e e==NULL");
				istanza = new cartaHelperBriscola(elaboratoreCarteBriscola.getCartaBriscola());
			}
			return istanza;
		}
		public UInt16 getSeme(UInt16 carta) {
			return (UInt16)(carta / 10);
		}
		public UInt16 getValore(UInt16 carta) {
			return (UInt16)(carta % 10);
		}
		public UInt16 getPunteggio(UInt16 carta) {
			UInt16 valore = 0;
			switch (carta % 10) {
				case 0: valore = 11; break;
				case 2: valore = 10; break;
				case 9: valore = 4; break;
				case 8: valore = 3; break;
				case 7: valore = 2; break;
			}
			return valore;
		}
		public string getSemeStr(UInt16 carta, MainWindow mw) {
			string s = "a";
            switch (carta / 10)
            {
                case 0: s = (String)mw.FindResource("bastoni") as String; break;
                case 1: s = (String)mw.FindResource("coppe") as String; break;
                case 2: s = (String)mw.FindResource("denari") as String; break;
                case 3: s = (String)mw.FindResource("spade") as String; break;
            }
            return s;
		}

		public UInt16 getNumero(UInt16 seme, UInt16 valore) {
			if (seme > 4 || valore > 9)
				throw new ArgumentException($"Chiamato cartaHelperBriscola::getNumero con seme={seme} e valore={valore}");
			return (UInt16)(seme * 10 + valore);
		}

		public carta getCartaBriscola() { return carta.getCarta(cartaBriscola); }

		public int CompareTo(UInt16 carta, UInt16 carta1) {
			UInt16 punteggio = getPunteggio(carta),
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