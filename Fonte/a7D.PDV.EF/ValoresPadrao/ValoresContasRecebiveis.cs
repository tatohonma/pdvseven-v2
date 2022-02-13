using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace a7D.PDV.EF.ValoresPadrao
{
    internal class ValoresContasRecebiveis
    {
        internal static void Validar(pdv7Context context)
        {
            var contasRecebiveis = ValueName.Convert<tbContaRecebivel>(typeof(EContaRecebivel)).ToList();
            var i = 0;

            var contasRecebiveisExistentes = context.ContaRecebiveis.ToList();
            while (i < contasRecebiveis.Count)
            {
                if (contasRecebiveisExistentes.FirstOrDefault(c => c.IDContaRecebivel == contasRecebiveis[i].IDContaRecebivel) == null)
                    i++;
                else
                    contasRecebiveis.RemoveAt(i);
            }

            if (contasRecebiveis.Count > 0)
                context.ContaRecebiveis.AddOrUpdate(c => c.IDContaRecebivel, contasRecebiveis.ToArray());
        }
    }
}
