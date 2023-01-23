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
    class giocatoreHelperUtente : giocatoreHelper
    {
        public giocatoreHelperUtente()
        {
            ;
        }
        public ushort gioca(ushort i, carta[] v, ushort numeroCarte)
        {
            if (i < numeroCarte)
                return i;
            else
                throw new ArgumentException("");
        }
        public ushort gioca(ushort i, carta[] v, ushort numeroCarte, carta c)
        {
            return gioca(i, v, numeroCarte);
        }
        public void aggiornaPunteggio(ref ushort punteggioAttuale, carta c, carta c1)
        {
            punteggioAttuale = (ushort)(punteggioAttuale + c.getPunteggio() + c1.getPunteggio());
        }

    };
}