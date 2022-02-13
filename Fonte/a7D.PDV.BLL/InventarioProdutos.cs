using a7D.Fmk.CRUD.DAL;
using a7D.PDV.DAL;
using a7D.PDV.Model;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class InventarioProdutos
    {
        public static List<InventarioProdutosInformation> Listar()
        {
            List<InventarioProdutosInformation> list = CRUD.Listar(new InventarioProdutosInformation()).Cast<InventarioProdutosInformation>().ToList();

            return list;
        }

        public static List<InventarioProdutosInformation> ListarPorInventario(InventarioInformation inventario)
        {
            List<InventarioProdutosInformation> lista = CRUD.Listar(new InventarioProdutosInformation
            {
                Inventario = new InventarioInformation { IDInventario = inventario.IDInventario.Value }
            }).Cast<InventarioProdutosInformation>().ToList();

            foreach (var obj in lista)
            {
                obj.Inventario = inventario;
                obj.Produto = Produto.Carregar(obj.Produto.IDProduto.Value);
                if (obj.Produto.Unidade != null && obj.Produto.Unidade.IDUnidade.HasValue)
                    obj.Produto.Unidade = Unidade.Carregar(obj.Produto.Unidade.IDUnidade.Value);
                obj.Unidade = Unidade.Carregar(obj.Unidade.IDUnidade.Value);
            }

            return lista;
        }

        public static List<InventarioProdutosInformation> ListarPorInventario(int idInventario)
        {
            return ListarPorInventario(Inventario.Carregar(idInventario));
        }

        public static List<InventarioProdutosInformation> UltimoInventarioProcessado(DateTime data)
        {
            int? idInventario = InventarioDAL.UltimoInventarioProcessado(data);

            if (idInventario.HasValue)
                return ListarPorInventario(idInventario.Value);

            return null;

        }


        public static InventarioProdutosInformation Carregar(int idInventarioProdutos)
        {
            InventarioProdutosInformation obj = new InventarioProdutosInformation { IDInventarioProdutos = idInventarioProdutos };
            return (InventarioProdutosInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(InventarioProdutosInformation obj)
        {
            switch (obj.StatusModel)
            {
                case StatusModel.Inalterado:
                case StatusModel.Novo:
                case StatusModel.Alterado:
                    if (obj.IDInventarioProdutos.HasValue && obj.IDInventarioProdutos.Value < 0)
                        obj.IDInventarioProdutos = null;
                    CRUD.Salvar(obj);
                    break;
                case StatusModel.Excluido:
                    CRUD.Excluir(obj);
                    break;
                default:
                    break;
            }
            CRUD.Salvar(obj);
        }

        public static void Adicionar(InventarioProdutosInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(InventarioProdutosInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(int idInventarioProdutos)
        {
            var obj = Carregar(idInventarioProdutos);
            var inventario = Inventario.Carregar(obj.Inventario.IDInventario.Value);
            if (inventario.Processado.Value)
                throw new InvalidOperationException("O inventário está processado");
            CRUD.Excluir(obj);
        }

        public static void Excluir(InventarioProdutosInformation inventarioProdutos)
        {
            if (inventarioProdutos.IDInventarioProdutos.HasValue)
                Excluir(inventarioProdutos.IDInventarioProdutos.Value);
        }
    }
}
