/*
  *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 1.1.3
 *
 *  Created by Giulio Sorrentino (numerone) on 29/01/23.
 *  Copyright 2023 Some rights reserved.
 *
 */

using Avalonia.Media.Imaging;
using System;

namespace org.altervista.numerone.framework
{
  public  class Giocatore
	{
		private string nome;
		private Carta[] mano;
		private bool ordinaMano;
		private UInt16 numeroCarte, iCarta, iCartaGiocata, punteggio;
		private readonly UInt16 totaleCarte;
		private readonly GiocatoreHelper helper;
		public enum CARTA_GIOCATA { NESSUNA_CARTA_GIOCATA = UInt16.MaxValue };
		public Giocatore(GiocatoreHelper h, string n, UInt16 carte, bool ordina = true)
		{
			totaleCarte = carte;
			ordinaMano = ordina;
			numeroCarte = carte;
			iCartaGiocata = (UInt16)(CARTA_GIOCATA.NESSUNA_CARTA_GIOCATA);
			punteggio = 0;
			helper = h;
			nome = n;
			mano = new Carta[totaleCarte];
			iCarta = 0;
		}
		public string GetNome() { return nome; }
		public void SetNome(string n) { nome = n; }
		public bool GetFlagOrdina() { return ordinaMano; }
		public void SetFlagOrdina(bool ordina) { ordinaMano = ordina; }
		public void AddCarta(Mazzo m)
		{
			UInt16 i = 0;
			Carta temp;
			if (iCarta == numeroCarte && iCartaGiocata == (UInt16)CARTA_GIOCATA.NESSUNA_CARTA_GIOCATA)
				throw new ArgumentException($"Chiamato Giocatore::setCarta con mano.size()==numeroCarte=={numeroCarte}");
			if (iCartaGiocata != (UInt16)CARTA_GIOCATA.NESSUNA_CARTA_GIOCATA)
			{
				for (i = iCartaGiocata; i < numeroCarte - 1; i++)
					mano[i] = mano[i + 1];
				mano[i] = null;
				iCartaGiocata = (UInt16)CARTA_GIOCATA.NESSUNA_CARTA_GIOCATA;
				mano[iCarta - 1] = SostituisciCartaGiocata(m);
				for (i = (UInt16)(iCarta - 2); i < UInt16.MaxValue && iCarta > 1 && mano[i].CompareTo(mano[i + 1]) < 0; i--)
				{
					temp = mano[i];
					mano[i] = mano[i+1];
					mano[i+1] = temp;
				}
				return;
			}
			Ordina(m);


		}

		private void Ordina(Mazzo m)
		{
			UInt16 i = 0;
			UInt16 j = 0;
			Carta c = SostituisciCartaGiocata(m);
			for (i = 0; i < iCarta && mano[i] != null && c.CompareTo(mano[i]) < 0; i++) ;
			for (j = (UInt16)(numeroCarte - 1); j > i; j--)
				mano[j] = mano[j - 1];
			mano[i] = c;
			iCarta++;
		}
		private Carta SostituisciCartaGiocata(Mazzo m)
		{
			Carta c;
			try
			{
				c = Carta.GetCarta(m.GetCarta());
			}
			catch (IndexOutOfRangeException e)
			{
				numeroCarte--;
				iCarta--;
				if (numeroCarte == 0)
					throw e;
				return mano[numeroCarte];
			}
			return c;
		}
		public Carta GetCartaGiocata()
		{
			return mano[iCartaGiocata];
		}
		public UInt16 GetPunteggio() { return punteggio; }
		public void Gioca(UInt16 i)
		{
			iCartaGiocata = helper.Gioca(i, mano, numeroCarte);
		}
		public void Gioca(UInt16 i, Giocatore g1)
		{
			iCartaGiocata = helper.Gioca(i, mano, numeroCarte, g1.GetCartaGiocata());
		}
		public void AggiornaPunteggio(Giocatore g)
		{
			helper.AggiornaPunteggio(ref punteggio, GetCartaGiocata(), g.GetCartaGiocata());
		}

        public Bitmap GetImmagine(UInt16 quale)
        {
            return mano[quale].GetImmagine();
        }


        public UInt16 GetICartaGiocata()
		{
			return iCartaGiocata;
		}

		public UInt16 GetNumeroCarte()
		{
			return numeroCarte;
		}
	}

}