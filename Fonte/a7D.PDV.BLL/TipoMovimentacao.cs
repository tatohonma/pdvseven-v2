using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class TipoMovimentacao
    {
        public static List<TipoMovimentacaoInformation> Listar()
        {
            List<TipoMovimentacaoInformation> list = CRUD.Listar(new TipoMovimentacaoInformation()).Cast<TipoMovimentacaoInformation>().ToList();

            return list;
        }

        public static TipoMovimentacaoInformation Carregar(int idTipoMovimentacao)
        {
            TipoMovimentacaoInformation obj = new TipoMovimentacaoInformation { IDTipoMovimentacao = idTipoMovimentacao };
            return (TipoMovimentacaoInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(TipoMovimentacaoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Adicionar(TipoMovimentacaoInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(TipoMovimentacaoInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(int idTipoMovimentacao)
        {
            var obj = Carregar(idTipoMovimentacao);
            obj.Excluido = true;
            Salvar(obj);
        }

        public static void Excluir(TipoMovimentacaoInformation tipoMovimentacao)
        {
            if (tipoMovimentacao.IDTipoMovimentacao.HasValue)
                Excluir(tipoMovimentacao.IDTipoMovimentacao.Value);
        }
    }
}
