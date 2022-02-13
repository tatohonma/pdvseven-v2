using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BLL
{
    public static class TipoPagamento
    {
        public static List<TipoPagamentoInformation> Listar()
        {
            var listaObj = CRUD.Listar(new TipoPagamentoInformation());
            var lista = listaObj.Cast<TipoPagamentoInformation>().ToList();

            foreach (var item in lista.Where(t => t.MeioPagamentoSAT?.IDMeioPagamentoSAT != null))
                item.MeioPagamentoSAT = MeioPagamentoSAT.Carregar(item.MeioPagamentoSAT.IDMeioPagamentoSAT.Value);

            return lista;
        }

        public static List<TipoPagamentoInformation> ListarAtivos()
        {
            TipoPagamentoInformation obj = new TipoPagamentoInformation { Ativo = true };
            var lista = CRUD.Listar(obj).Cast<TipoPagamentoInformation>().ToList();

            foreach (var item in lista.Where(t => t.MeioPagamentoSAT?.IDMeioPagamentoSAT != null))
            {
                if (item.MeioPagamentoSAT != null)
                    item.MeioPagamentoSAT = MeioPagamentoSAT.Carregar(item.MeioPagamentoSAT.IDMeioPagamentoSAT.Value);

                if (item.ContaRecebivel != null)
                    item.ContaRecebivel = ContaRecebivel.Carregar(item.ContaRecebivel.IDContaRecebivel);

                if (item.Bandeira != null)
                    item.Bandeira = Bandeira.Carregar(item.Bandeira.IDBandeira);
            }

            return lista;
        }

        public static TipoPagamentoInformation Carregar(Int32 idTipoPagamento)
        {
            TipoPagamentoInformation obj = new TipoPagamentoInformation { IDTipoPagamento = idTipoPagamento };
            obj = (TipoPagamentoInformation)CRUD.Carregar(obj);

            if (obj?.MeioPagamentoSAT?.IDMeioPagamentoSAT != null)
            {
                obj.MeioPagamentoSAT = MeioPagamentoSAT.Carregar(obj.MeioPagamentoSAT.IDMeioPagamentoSAT.Value);
            }

            return obj;
        }

        public static TipoPagamentoInformation CarregarPorCodigo(int codigo)
        {
            return Listar().FirstOrDefault(p => p.Ativo == true && string.IsNullOrWhiteSpace(p.CodigoImpressoraFiscal) == false && Convert.ToInt32(p.CodigoImpressoraFiscal) == codigo);
        }

        public static TipoPagamentoInformation CarregarPorGateway(int gateway, bool ativoCaixa)
        {
            if (ativoCaixa)
                return ListarAtivos().FirstOrDefault(p => p.IDGateway == gateway);
            else
                return Listar().FirstOrDefault(p => p.IDGateway == gateway);
        }

        public static void Salvar(TipoPagamentoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Excluir(Int32 idTipoPagamento)
        {
            try
            {
                TipoPagamentoInformation obj = new TipoPagamentoInformation { IDTipoPagamento = idTipoPagamento };
                CRUD.Excluir(obj);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EF9A, ex);
            }
        }
    }
}
