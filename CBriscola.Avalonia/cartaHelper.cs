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
	public interface CartaHelper
	{
		UInt16 GetSeme(UInt16 Carta);
		UInt16 GetValore(UInt16 Carta);
		UInt16 GetPunteggio(UInt16 Carta);
		string GetSemeStr(UInt16 carta, String mazzo, Avalonia.Controls.ResourceDictionary d);
		UInt16 GetNumero(UInt16 seme, UInt16 valore);
	};
}