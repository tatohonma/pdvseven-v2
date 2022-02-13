using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace a7D.PDV.BLL
{
    public static class Inventario
    {
        public static List<InventarioInformation> Listar()
        {
            List<InventarioInformation> list = CRUD.Listar(new InventarioInformation()).Cast<InventarioInformation>().ToList();

            foreach (var obj in list)
            {
                obj.InventarioProdutos = InventarioProdutos.ListarPorInventario(obj);
            }

            return list;
        }

        public static List<InventarioInformation> ListarProcessados()
        {
            List<InventarioInformation> list = CRUD.Listar(new InventarioInformation { Processado = true, Excluido = false }).Cast<InventarioInformation>().ToList();

            foreach (var obj in list)
            {
                obj.InventarioProdutos = InventarioProdutos.ListarPorInventario(obj);
            }

            return list;
        }

        public static InventarioInformation Carregar(int idInventario)
        {
            InventarioInformation obj = new InventarioInformation { IDInventario = idInventario };
            obj.InventarioProdutos = InventarioProdutos.ListarPorInventario(obj);
            return (InventarioInformation)CRUD.Carregar(obj);
        }

        public static void Salvar(InventarioInformation obj)
        {
            using (var scope = new TransactionScope())
            {
                CRUD.Salvar(obj);

                foreach (var ip in obj.InventarioProdutos)
                {
                    ip.Inventario = obj;
                    InventarioProdutos.Salvar(ip);
                }
                scope.Complete();
            }
        }

        public static void Adicionar(InventarioInformation obj)
        {
            CRUD.Adicionar(obj);
        }

        public static void Alterar(InventarioInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static void Excluir(int idInventario)
        {
            var obj = Carregar(idInventario);
            obj.Excluido = true;
            Salvar(obj);
        }

        public static void Excluir(InventarioInformation obj)
        {
            if (obj.IDInventario.HasValue)
                Excluir(obj.IDInventario.Value);
        }

        public static List<InventarioInformation> ListarAtivos()
        {
            var obj = new InventarioInformation
            {
                Excluido = false
            };

            return CRUD.Listar(obj).Cast<InventarioInformation>().ToList();
        }

        public static void Processar(int idInventario)
        {
            var inventario = Carregar(idInventario);
            Processar(inventario);
        }

        public static void Processar(InventarioInformation inventario)
        {
            using (var scope = new TransactionScope())
            using (var atual = EntradaSaida.EstoqueInventario(inventario.Data.Value))
            {
                if (inventario.Excluido == true)
                    throw new ArgumentException("O inventário não pode ter sido excluído");
                inventario.Processado = true;

                var listaProdutos = InventarioProdutos.ListarPorInventario(inventario.IDInventario.Value);

                foreach (var row in atual.AsEnumerable())
                {
                    var idProduto = row.Field<int>("IDProduto");
                    if (!listaProdutos.Any(ip => ip.Produto.IDProduto == idProduto))
                    {
                        var nIP = new InventarioProdutosInformation
                        {
                            Inventario = inventario,
                            Produto = new ProdutoInformation { IDProduto = idProduto },
                            Quantidade = row.Field<decimal>("Quantidade"),
                            Unidade = new UnidadeInformation { IDUnidade = row.Field<int>("IDUnidade") },
                            QuantidadeAnterior = row.Field<decimal>("Quantidade"),
                            StatusModel = StatusModel.Novo,
                        };
                        InventarioProdutos.Salvar(nIP);
                    }
                }

                Salvar(inventario);

                scope.Complete();
            }
        }

        public static DataTable RelatorioInventario(int idInventario)
        {
            if (Carregar(idInventario).Processado == true)
                return DAL.InventarioDAL.RelatorioInventario(idInventario);
            else
                throw new ArgumentException("O inventário " + idInventario + " não está processado");
        }
    }
}
