using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace org.altervista.numerone.framework;

public class GiocatoreHelperCpu2 : GiocatoreHelperCpu
{
    public GiocatoreHelperCpu2(ushort b) : base(b)
    {
    }

    public override ushort GetLivello()
    {
        return 3;
    }

    public override ushort Gioca(ushort x, Carta[] mano, ushort numeroCarte, Carta c)
    {
        UInt16 i = (UInt16)ElaboratoreCarteBriscola.r.Next(0, UInt16.MaxValue);
        if (!briscola.StessoSeme(c))
        {
            if ((i = getSoprataglio(mano, c, true)) < numeroCarte)
                return i;
            if (c.GetPunteggio() > 0 && (i = GetBriscola(mano)) < numeroCarte)
            {
                if (c.GetPunteggio() > 4)
                    return i;
                if (mano[i].GetPunteggio() > 0)
                    if (ElaboratoreCarteBriscola.r.Next() % 10 < 5)
                        return i;
            }
        }
        else
        {
            if (ElaboratoreCarteBriscola.r.Next() % 10 < 5 && (i = getSoprataglio(mano, c, false)) < numeroCarte)
                return i;
        }
        i = 0;
        return i;
    }
}
