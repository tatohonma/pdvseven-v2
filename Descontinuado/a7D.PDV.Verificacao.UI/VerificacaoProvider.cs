using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Verificacao.UI
{
    internal class VerificacaoProvider
    {
        internal static IEnumerable<IVerificacao> Verificacoes()
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   where t.IsDefined(typeof(VerificacaoAttribute), false)
                   select Activator.CreateInstance(t) as IVerificacao;
        }
    }
}
