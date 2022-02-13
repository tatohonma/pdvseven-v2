using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Balanca
{
    public static class BalancaFactory
    {
        public static Interfaces.IBalanca ObterBalanca(ETipoBalanca tipo, string portaCOM)
        {
            switch (tipo)
            {
                case ETipoBalanca.TOLEDO:
                    return new Toledo() { Porta = portaCOM };
                case ETipoBalanca.FILIZOLA:
                    return new Filizola() { Porta = portaCOM };
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
