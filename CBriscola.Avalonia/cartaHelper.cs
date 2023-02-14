/*
 *  This code is distribuited under GPL 3.0 or, at your opinion, any later version
 *  CBriscola 0.1
 *
 *  Created by numerunix on 22/05/22.
 *  Copyright 2022 Some rights reserved.
 *
 */

using Avalonia.Controls;
using System;
using System.Resources;

namespace CBriscola.Avalonia
{
    interface cartaHelper
    {
        ushort getSeme(ushort carta);
        ushort getValore(ushort carta);
        ushort getPunteggio(ushort carta);
        string getSemeStr(ushort carta, ResourceDictionary d);
        ushort getNumero(ushort seme, ushort valore);
    };
}