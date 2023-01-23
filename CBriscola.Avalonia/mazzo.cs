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
    class mazzo
    {
        private ushort[] carte;
        private ushort numeroCarte;
        private elaboratoreCarte elaboratore;
        private string nome;
        private void mischia()
        {
            for (numeroCarte = 0; numeroCarte < 40; numeroCarte++)
                carte[numeroCarte] = elaboratore.getCarta();
        }

        public mazzo(elaboratoreCarte e)
        {
            elaboratore = e;
            carte = new ushort[40];
            nome = "Napoletano";
            mischia();
        }
        public ushort getNumeroCarte() { return numeroCarte; }
        public ushort getCarta()
        {
            if (numeroCarte > 40)
                throw new IndexOutOfRangeException();
            ushort c = carte[--numeroCarte];
            return c;
        }

        public string getNome() { return nome; }
        public void setNome(string s) { nome = s; }
    };
}