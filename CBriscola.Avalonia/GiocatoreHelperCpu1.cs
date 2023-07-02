using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace org.altervista.numerone.framework
{
    public class GiocatoreHelperCpu1 : GiocatoreHelperCpu
    {
        public GiocatoreHelperCpu1(ushort b) : base(b)
        {
        }

        public override ushort GetLivello()
        {
            return 2;
        }

        public override ushort Gioca(ushort x, Carta[] mano, ushort numeroCarte, Carta c)
        {
            UInt16 i = (UInt16)ElaboratoreCarteBriscola.r.Next(0, UInt16.MaxValue);
            if (!briscola.StessoSeme(c))
            {
                if ((i = getSoprataglio(mano, c, true)) < numeroCarte)
                    return i;
                else
                    for (i = 0; i < mano.Length - 1; i++)
                        if (briscola.StessoSeme(mano[i]))
                            return i;
            }
            i = 0;
            return i;
        }
    }
}
