using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class TaxaEntrega
    {
        public static List<TaxaEntregaInformation> Listar()
        {
            return CRUD.Listar(new TaxaEntregaInformation { Excluido = false }).Cast<TaxaEntregaInformation>().ToList();
        }

        public static List<TaxaEntregaInformation> ListarAtivos()
        {
            return Listar().Where(t => t.Ativo == true).ToList();
        }

        public static TaxaEntregaInformation Carregar(int idTaxaEntrega)
        {
            return CRUD.Carregar(new TaxaEntregaInformation { IDTaxaEntrega = idTaxaEntrega }) as TaxaEntregaInformation;
        }

        public static TaxaEntregaInformation CarregarPorNome(string nome)
        {
            return CRUD.Carregar(new TaxaEntregaInformation { Nome = nome }) as TaxaEntregaInformation;
        }

        public static void Salvar(TaxaEntregaInformation taxaEntrega)
        {
            CRUD.Salvar(taxaEntrega);
        }

        public static void Excluir(int idTaxaEntrega)
        {
            Excluir(Carregar(idTaxaEntrega));
        }

        public static void Excluir(TaxaEntregaInformation taxaEntrega)
        {
            if (taxaEntrega == null)
                return;

            taxaEntrega.Excluido = true;
            Salvar(taxaEntrega);
        }
    }
}
