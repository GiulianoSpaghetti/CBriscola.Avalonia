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
    public interface GiocatoreHelper
	{
		UInt16 Gioca(UInt16 i, Carta[] v, UInt16 numeroCarte);
		UInt16 Gioca(UInt16 i, Carta[] v, UInt16 numeroCarte, Carta c);
		void AggiornaPunteggio(ref UInt16 punteggio, Carta c, Carta c1);

    };
}