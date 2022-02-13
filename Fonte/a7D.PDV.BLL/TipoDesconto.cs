using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class TipoDesconto
    {
        public static TipoDescontoInformation Carregar(int idTipoDesconto)
        {
            var obj = new TipoDescontoInformation { IDTipoDesconto = idTipoDesconto };
            return CRUD.Carregar(obj) as TipoDescontoInformation;
        }

        public static List<TipoDescontoInformation> Listar()
        {
            var obj = new TipoDescontoInformation();
            var lista =  CRUD.Listar(obj).Cast<TipoDescontoInformation>().ToList();

            if (lista.Count > 0)
                lista = lista.Where(l => l.Excluido == false).ToList();

            return lista;
        }

        public static List<TipoDescontoInformation> ListarAtivos()
        {
            return Listar().Where(td => td.Ativo == true).ToList();
        }

        public static void Salvar(TipoDescontoInformation obj)
        {
            if (obj.IDTipoDesconto.HasValue)
                CRUD.Alterar(obj);
            else
                CRUD.Adicionar(obj);
        }

        public static void Excluir(TipoDescontoInformation obj)
        {
            if(obj.IDTipoDesconto.HasValue)
            {
                obj.Excluido = true;
                Salvar(obj);
            }
        }

    }
}
