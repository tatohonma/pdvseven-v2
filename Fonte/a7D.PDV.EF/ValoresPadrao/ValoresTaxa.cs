using a7D.PDV.EF.Models;
using System.Collections.Generic;
using System.Linq;

namespace a7D.PDV.EF.ValoresPadrao
{
    internal class ValoresTaxaEntregas
    {
        internal static void Validar(pdv7Context context)
        {
            var taxasPadrao = new List<tbTaxaEntrega>
            {
                new tbTaxaEntrega() { Nome="iFood", Ativo=false, Excluido=false }
            };

            while (taxasPadrao.Count > 0)
            {
                var taxa = taxasPadrao[0];
                if (context.tbTaxaEntregas.FirstOrDefault(t => t.Nome == taxa.Nome) == null)
                {
                    context.tbTaxaEntregas.Add(taxa);
                    context.SaveChanges();
                }
                taxasPadrao.RemoveAt(0);
            }
        }
    }
}
