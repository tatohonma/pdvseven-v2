using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public static class TipoProduto
    {
        //private static TipoProdutoInformation _item;

        public static List<TipoProdutoInformation> Listar()
        {
            List<Object> listObj = CRUD.Listar(new TipoProdutoInformation());
            List<TipoProdutoInformation> list = listObj.ConvertAll(new Converter<Object, TipoProdutoInformation>(TipoProdutoInformation.ConverterObjeto));

            return list;
        }

        public static TipoProdutoInformation Carregar(Int32 idTipoProduto)
        {
            TipoProdutoInformation obj = new TipoProdutoInformation { IDTipoProduto = idTipoProduto };
            return (TipoProdutoInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(TipoProdutoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Adicionar(TipoProdutoInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(TipoProdutoInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(Int32 idTipoProduto)
        {
            TipoProdutoInformation obj = new TipoProdutoInformation { IDTipoProduto = idTipoProduto };
            CRUD.Excluir(obj);
        }
    }
}
