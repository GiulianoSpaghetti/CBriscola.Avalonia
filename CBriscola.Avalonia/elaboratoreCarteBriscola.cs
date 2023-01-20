/*
 *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 0.1
 *
 *  Created by numerunix on 22/05/22.
 *  Copyright 2022 Some rights reserved.
 *
 */

using System;
namespace CBriscola
{
	class elaboratoreCarteBriscola : elaboratoreCarte
	{
		private const UInt16 numeroCarte = 40;
		private bool[] doppione;
		private static UInt16 cartaBriscola;
		private bool inizio,
				 briscolaDaPunti;
		public static Random r = new Random();
		public elaboratoreCarteBriscola(bool punti = true)
		{
			inizio = true;
			briscolaDaPunti = punti;
			doppione = new bool[40];
			for (int i = 0; i < 40; i++)
				doppione[i] = false;
		}
		public UInt16 getCarta()
		{
			UInt16 fine = (UInt16)(r.Next(0, 39) % numeroCarte),
			carta = (UInt16)((fine + 1) % numeroCarte);
			while (doppione[carta] && carta != fine)
				carta = (UInt16)((carta + 1) % numeroCarte);
			if (doppione[carta])
				throw new ArgumentException("Chiamato elaboratoreCarteItaliane::getCarta() quando non ci sono più carte da elaborare");
			else
			{
				if (inizio)
				{
					UInt16 valore = (UInt16)(carta % 10);
					if (!briscolaDaPunti && (valore == 0 || valore == 2 || valore > 6))
					{
						carta = (UInt16)(carta - valore + 1);
					}
					if (!briscolaDaPunti)
						carta = cartaHelperBriscola.getIstanza(this).getNumero(cartaHelperBriscola.getIstanza(this).getSeme(carta), 1);
					cartaBriscola = carta;
					inizio = false;
				}
				doppione[carta] = true;
				return carta;
			}
		}

		public static UInt16 getCartaBriscola() { return cartaBriscola; }
	}
}