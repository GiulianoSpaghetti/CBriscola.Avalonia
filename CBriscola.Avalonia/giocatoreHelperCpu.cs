/*
  *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 0.1
 *
 *  Created by numerunix on 22/05/22.
 *  Copyright 2022 Some rights reserved.
 *
 */

using System;

namespace CBriscola.Avalonia
{
    class giocatoreHelperCpu : giocatoreHelper
    {
        private carta briscola;
        private ushort getBriscola(carta[] mano)
        {
            ushort i;
            for (i = 0; i < mano.Length; i++)
                if (briscola.stessoSeme(mano[i]))
                    break;
            return i;
        }
        public giocatoreHelperCpu(ushort b)
        {
            briscola = carta.getCarta(b);
        }
        private ushort getSoprataglio(carta[] mano, carta c, bool maggiore)
        {
            bool trovata = false;
            ushort i;
            if (maggiore)
            {
                for (i = (ushort)(mano.Length - 1); i > 0; i--)
                    if (c.stessoSeme(mano[i]) && c.CompareTo(mano[i]) > 0)
                    {
                        trovata = true;
                        break;
                    }
                    else if (c.stessoSeme(mano[i]) && mano[i].CompareTo(c) > 0)
                        break;
            }
            else
            {
                for (i = 0; i < mano.Length; i++)
                    if (c.stessoSeme(mano[i]) && c.CompareTo(mano[i]) > 0)
                    {
                        trovata = true;
                        break;
                    }
            }
            if (trovata)
                return i;
            else
                return (ushort)mano.Length;
        }
        public ushort gioca(ushort x, carta[] mano, ushort numeroCarte)
        {
            ushort i;
            for (i = (ushort)(numeroCarte - 1); i > 0; i--) ;
            if (mano[i].getPunteggio() > 4 || briscola.stessoSeme(mano[i]))
                i = 0;
            return i;

        }
        public ushort gioca(ushort x, carta[] mano, ushort numeroCarte, carta c)
        {
            ushort i = (ushort)elaboratoreCarteBriscola.r.Next(0, ushort.MaxValue);
            if (!briscola.stessoSeme(c))
            {
                if ((i = getSoprataglio(mano, c, true)) < numeroCarte)
                    return i;
                if (c.getPunteggio() > 0 && (i = getBriscola(mano)) < numeroCarte)
                {
                    if (c.getPunteggio() > 4)
                        return i;
                    if (mano[i].getPunteggio() > 0)
                        if (elaboratoreCarteBriscola.r.Next() % 10 < 5)
                            return i;
                }
            }
            else
            {
                if (elaboratoreCarteBriscola.r.Next() % 10 < 5 && (i = getSoprataglio(mano, c, false)) < numeroCarte)
                    return i;
            }
            i = 0;
            return i;
        }
        public void aggiornaPunteggio(ref ushort punteggioAttuale, carta c, carta c1) { punteggioAttuale = (ushort)(punteggioAttuale + c.getPunteggio() + c1.getPunteggio()); }
    }
}