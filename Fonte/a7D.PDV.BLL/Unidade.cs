using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;

namespace a7D.PDV.BLL
{
    public class Unidade
    {
        public static List<UnidadeInformation> Listar()
        {
            List<UnidadeInformation> list = CRUD.Listar(new UnidadeInformation()).Cast<UnidadeInformation>().ToList();

            return list;
        }

        public static UnidadeInformation Carregar(int idUnidade)
        {
            UnidadeInformation obj = new UnidadeInformation { IDUnidade = idUnidade };
            return (UnidadeInformation)CRUD.Carregar(obj);
        }

        public static UnidadeInformation CarregarPorProduto(int idProduto)
        {
            var produto = Produto.Carregar(idProduto);
            return Carregar(produto.Unidade.IDUnidade.Value);
        }

        public static void Salvar(UnidadeInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void Adicionar(UnidadeInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(UnidadeInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(int idUnidade)
        {
            var obj = Carregar(idUnidade);
            obj.Excluido = true;
            Salvar(obj);
        }

        public static void Excluir(UnidadeInformation unidade)
        {
            if (unidade.IDUnidade.HasValue)
                Excluir(unidade.IDUnidade.Value);
        }

        public static List<UnidadeInformation> ListarAtivos()
        {
            var obj = new UnidadeInformation
            {
                Excluido = false
            };

            return CRUD.Listar(obj).Cast<UnidadeInformation>().ToList();
        }

        public static bool ExisteSimbolo(int? idUnidade, string text)
        {
            return Listar().Any(u => u.Simbolo.ToLowerInvariant() == text.ToLowerInvariant() && u.IDUnidade != idUnidade);
        }
    }
}

