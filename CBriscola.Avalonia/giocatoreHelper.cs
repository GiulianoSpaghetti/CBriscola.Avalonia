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
	interface giocatoreHelper
	{
		UInt16 gioca(UInt16 i, carta[] v, UInt16 numeroCarte);
		UInt16 gioca(UInt16 i, carta[] v, UInt16 numeroCarte, carta c);
		void aggiornaPunteggio(ref UInt16 punteggio, carta c, carta c1);

    };
}