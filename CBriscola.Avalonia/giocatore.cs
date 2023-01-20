/*
 *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 0.1
 *
 *  Created by numerunix on 22/05/22.
 *  Copyright 2022 Some rights reserved.
 *
 */

using Avalonia.Media.Imaging;
using System;

namespace CBriscola {
	class giocatore
	{
		private string nome;
		private carta[] mano;
		private bool ordinaMano;
		private UInt16 numeroCarte;
		private UInt16 iCarta;
		private UInt16 iCartaGiocata;
		private UInt16 punteggio;
		private giocatoreHelper helper;
		public enum CARTA_GIOCATA { NESSUNA_CARTA_GIOCATA = UInt16.MaxValue };
		public giocatore(giocatoreHelper h, string n, UInt16 carte, bool ordina = true)
		{
			ordinaMano = ordina;
			numeroCarte = carte;
			iCartaGiocata = (UInt16)(CARTA_GIOCATA.NESSUNA_CARTA_GIOCATA);
			punteggio = 0;
			helper = h;
			nome = n;
			mano = new carta[3];
			iCarta = 0;
		}
		public string getNome() { return nome; }
		public void setNome(string n) { nome = n; }
		public bool getFlagOrdina() { return ordinaMano; }
		public void setFlagOrdina(bool ordina) { ordinaMano = ordina; }
		public void addCarta(mazzo m)
		{
			UInt16 i = 0;
			UInt16 j = 0;
			carta c;
			carta temp;
			if (iCarta == numeroCarte && iCartaGiocata == (UInt16)CARTA_GIOCATA.NESSUNA_CARTA_GIOCATA)
				throw new ArgumentException($"Chiamato giocatore::setCarta con mano.size()==numeroCarte=={numeroCarte}");
			if (iCartaGiocata != (UInt16)CARTA_GIOCATA.NESSUNA_CARTA_GIOCATA)
			{
				for (i = iCartaGiocata; i < numeroCarte - 1; i++)
					mano[i] = mano[i + 1];
				mano[i] = null;
				iCartaGiocata = (UInt16)CARTA_GIOCATA.NESSUNA_CARTA_GIOCATA;
				mano[iCarta - 1] = sostituisciCartaGiocata(m);
				for (i = (UInt16)(iCarta - 2); i < UInt16.MaxValue && iCarta > 1 && mano[i].CompareTo(mano[i + 1]) < 0; i--)
				{
					temp = mano[i];
					mano[i] = mano[i+1];
					mano[i+1] = temp;
				}
				return;
			}
			ordina(m);


		}

		private void ordina(mazzo m)
		{
			UInt16 i = 0;
			UInt16 j = 0;
			carta c = sostituisciCartaGiocata(m);
			for (i = 0; i < iCarta && mano[i] != null && c.CompareTo(mano[i]) < 0; i++) ;
			for (j = (UInt16)(numeroCarte - 1); j > i; j--)
				mano[j] = mano[j - 1];
			mano[i] = c;
			iCarta++;
		}
		private carta sostituisciCartaGiocata(mazzo m)
		{
			carta c;
			try
			{
				c = carta.getCarta(m.getCarta());
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
		public carta getCartaGiocata()
		{
			return mano[iCartaGiocata];
		}
		public UInt16 getPunteggio() { return punteggio; }
		public void gioca(UInt16 i)
		{
			iCartaGiocata = helper.gioca(i, mano, numeroCarte);
		}
		public void gioca(UInt16 i, giocatore g1)
		{
			iCartaGiocata = helper.gioca(i, mano, numeroCarte, g1.getCartaGiocata());
		}
		public void aggiornaPunteggio(giocatore g)
		{
			helper.aggiornaPunteggio(ref punteggio, getCartaGiocata(), g.getCartaGiocata());
		}

		public Bitmap getImmagine(UInt16 quale)
		{
			return mano[quale].getImmagine();
		}

		public UInt16 getICartaGiocata()
		{
			return iCartaGiocata;
		}

		public UInt16 getNumeroCarte()
		{
			return numeroCarte;
		}
	}

}