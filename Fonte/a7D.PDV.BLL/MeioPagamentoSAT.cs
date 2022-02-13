using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class MeioPagamentoSAT
    {
        public static List<MeioPagamentoSATInformation> Listar()
        {
            return CRUD.Listar(new MeioPagamentoSATInformation()).Cast<MeioPagamentoSATInformation>().OrderBy(m => m.Codigo).ToList();
        }

        public static MeioPagamentoSATInformation Carregar(int IDMeioPagamentoSAT)
        {
            return CRUD.Carregar(new MeioPagamentoSATInformation { IDMeioPagamentoSAT = IDMeioPagamentoSAT }) as MeioPagamentoSATInformation;
        }

        public static MeioPagamentoSATInformation CarregarPorCodigo(string codigoPagamento)
        {
            return CRUD.Carregar(new MeioPagamentoSATInformation { Codigo = codigoPagamento }) as MeioPagamentoSATInformation;
        }
    }
}
