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
    public class Mazzo
    {
        private UInt16[] carte;
        private UInt16 numeroCarte;
        private readonly ElaboratoreCarte elaboratore;
        private String nome;
        private void Mischia()
        {
            for (numeroCarte = 0; numeroCarte < 40; numeroCarte++)
                carte[numeroCarte] = elaboratore.GetCarta();
        }


        public Mazzo(ElaboratoreCarte e)
        {
            elaboratore = e;
            carte = new UInt16[40];
            Mischia();
        }
        public UInt16 GetNumeroCarte() { return numeroCarte; }
        public UInt16 GetCarta()
        {
            if (numeroCarte > 40)
                throw new IndexOutOfRangeException();
            UInt16 c = carte[--numeroCarte];
            return c;
        }

        public String GetNome() { return nome; }
        public void SetNome(String s) { nome = s; }
    };
}