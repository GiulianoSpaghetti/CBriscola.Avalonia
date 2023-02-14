/*
  *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 1.1.3
 *
 *  Created by Giulio Sorrentino (numerone) on 29/01/23.
 *  Copyright 2023 Some rights reserved.
 *
 */

using System;
namespace org.altervista.numerone.framework

{
    public class ElaboratoreCarteBriscola : ElaboratoreCarte
	{
		private const UInt16 numeroCarte = 40;
		private bool[] doppione;
		private static UInt16 CartaBriscola;
		private bool inizio,
				 briscolaDaPunti;
		public static Random r = new Random();
		public ElaboratoreCarteBriscola(bool punti = true)
		{
			inizio = true;
			briscolaDaPunti = punti;
			doppione = new bool[40];
			for (int i = 0; i < 40; i++)
				doppione[i] = false;
		}
		public UInt16 GetCarta()
		{
			UInt16 fine = (UInt16)(r.Next(0, 39) % numeroCarte),
			Carta = (UInt16)((fine + 1) % numeroCarte);
			while (doppione[Carta] && Carta != fine)
				Carta = (UInt16)((Carta + 1) % numeroCarte);
			if (doppione[Carta])
				throw new ArgumentException("Chiamato elaboratoreCarteItaliane::getCarta() quando non ci sono più carte da elaborare");
			else
			{
				if (inizio)
				{
					UInt16 valore = (UInt16)(Carta % 10);
					if (!briscolaDaPunti && (valore == 0 || valore == 2 || valore > 6))
					{
						Carta = (UInt16)(Carta - valore + 1);
					}
					if (!briscolaDaPunti)
						Carta = CartaHelperBriscola.GetIstanza(this).GetNumero(CartaHelperBriscola.GetIstanza(this).GetSeme(Carta), 1);
					CartaBriscola = Carta;
					inizio = false;
				}
				doppione[Carta] = true;
				return Carta;
			}
		}

		public static UInt16 GetCartaBriscola() { return CartaBriscola; }
	}
}