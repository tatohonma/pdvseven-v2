using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;
using System.Transactions;
using a7D.PDV.DAL;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;

namespace a7D.PDV.BLL
{
    public static class Produto
    {
        public const int MaxQTD = 99;

        public static bool QTDValido(decimal valor)
        {
            return valor >= 0 && valor <= MaxQTD;
        }

        public static bool ValorValido(decimal valor)
        {
            return valor >= 0 && valor < 1000000;
        }

        public static ProdutoInformation CarregarCompleto(int idProduto)
        {
            ProdutoInformation obj = new ProdutoInformation { IDProduto = idProduto };
            obj = (ProdutoInformation)CRUD.Carregar(obj);

            if (obj.Unidade != null && obj.Unidade.IDUnidade.HasValue)
                obj.Unidade = Unidade.Carregar(obj.Unidade.IDUnidade.Value);

            obj.ListaPainelModificacao = ProdutoPainelModificacao.Listar(idProduto);
            obj.ListaProdutoCategoria = ProdutoCategoriaProduto.ListarPorProdutoCompleto(idProduto);
            obj.ListaProdutoTraducao = ProdutoTraducao.ListarPorProduto(idProduto);
            obj.ListaProdutoReceita = ProdutoReceita.ListarPorProduto(idProduto);
            //obj.ClassificacaoFiscal = ClassificacaoFiscal.Carregar

            return obj;
        }

        public static ProdutoInformation Carregar(int idProduto)
        {
            ProdutoInformation obj = new ProdutoInformation { IDProduto = idProduto };
            obj = (ProdutoInformation)CRUD.Carregar(obj);

            return obj;
        }

        public static ProdutoInformation ObterProduto(int idProduto)
        {
            var produto = Carregar(idProduto);

            if (string.IsNullOrWhiteSpace(produto.Nome))
                throw new ExceptionPDV(BLL.CodigoErro.EA00, $"idProduto: {idProduto}");

            return produto;
        }

        public static bool ServicoComoProduto(ProdutoInformation produto)
        {
            if (produto.IDProduto != ProdutoInformation.IDProdutoServico)
                return true;
            else if (ConfiguracoesSistema.Valores.ServicoComoItem)
                return true;

            return false;
        }

        public static void SalvarDisponibilidade(int idProduto, Boolean disponibilidade)
        {
            ProdutoInformation obj = new ProdutoInformation { IDProduto = idProduto };
            obj = (ProdutoInformation)CRUD.Carregar(obj);

            obj.Disponibilidade = disponibilidade;
            obj.DtAlteracaoDisponibilidade = DateTime.Now;

            Salvar(obj);
        }

        public static void Salvar(ProdutoInformation obj)
        {
            CRUD.Salvar(obj);
        }

        public static void SalvarCompleto(ProdutoInformation obj)
        {
            try
            {
                if (string.IsNullOrEmpty(obj.GUIDIdentificacao))
                    obj.GUIDIdentificacao = Guid.NewGuid().ToString();

                obj.DtUltimaAlteracao = DateTime.Now;
                obj.Excluido = false;
                Salvar(obj);

                ProdutoPainelModificacao.ExcluirPorProduto(obj.IDProduto.Value);

                if (obj.ListaPainelModificacao != null)
                {
                    foreach (var item in obj.ListaPainelModificacao)
                    {
                        item.IDProdutoPainelModificacao = null;
                        item.Produto = new ProdutoInformation { IDProduto = obj.IDProduto };

                        ProdutoPainelModificacao.Salvar(item);
                    }
                }

                if (obj.ListaProdutoCategoria != null)
                {
                    ProdutoCategoriaProduto.SalvarLista(obj.ListaProdutoCategoria);
                }

                if (obj.ListaProdutoTraducao != null)
                    ProdutoTraducao.SalvarLista(obj.ListaProdutoTraducao);

                if (obj.ListaProdutoReceita != null)
                    ProdutoReceita.Salvar(obj.ListaProdutoReceita);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static List<ProdutoInformation> ListarCompletoSemCategoria()
        {
            List<int> lista = ProdutoDAL.ListarSemCategoria();
            List<ProdutoInformation> retorno = new List<ProdutoInformation>();

            foreach (var idProduto in lista)
            {
                retorno.Add(CarregarCompleto(idProduto));
            }

            return retorno;
        }

        public static int ContarSemCategoria(bool listarCreditos)
        {
            return ProdutoDAL.ContarSemCategoria(listarCreditos);
        }

        public static List<ProdutoInformation> Listar(ProdutoInformation objFiltro)
        {
            List<object> listaObj = CRUD.Listar(objFiltro);
            List<ProdutoInformation> lista = listaObj.Cast<ProdutoInformation>().ToList();
            return lista;
        }

        public static List<ProdutoInformation> ListarCompleto()
        {
            List<ProdutoInformation> listaProdutos = Listar(new ProdutoInformation() { Ativo = true, Excluido = false });

            foreach (var produto in listaProdutos)
            {
                //if (produto.CategoriaProduto != null && produto.CategoriaProduto.IDCategoriaProduto.HasValue)
                //    produto.CategoriaProduto = CategoriaProduto.Carregar(produto.CategoriaProduto.IDCategoriaProduto.Value);

                if (produto.TipoProduto != null && produto.TipoProduto.IDTipoProduto.HasValue)
                    produto.TipoProduto = TipoProduto.Carregar(produto.TipoProduto.IDTipoProduto.Value);

                if (produto.Unidade != null && produto.Unidade.IDUnidade.HasValue)
                    produto.Unidade = Unidade.Carregar(produto.Unidade.IDUnidade.Value);
            }


            return listaProdutos;
        }

        public static List<ProdutoInformation> ListarApenasModificacao()
        {
            ProdutoInformation objFiltro = new ProdutoInformation();
            objFiltro.Excluido = false;
            objFiltro.TipoProduto = TipoProduto.Carregar((int)ETipoProduto.Modificacao);

            List<object> listaObj = CRUD.Listar(objFiltro);
            List<ProdutoInformation> lista = listaObj.Cast<ProdutoInformation>().ToList();

            return lista;
        }

        public static List<ProdutoInformation> ListarNaoExcluidosDAL()
        {
            if (ConfiguracoesSistema.Valores.ServicoComoItem)
                return ProdutoDAL.ListarNaoExcluidos().Where(p => p.IDProduto > 3).ToList();
            else
                return ProdutoDAL.ListarNaoExcluidos().Where(p => p.IDProduto > 4).ToList();
        }

        public static List<ProdutoInformation> ListarExcluidosDAL(DateTime data)
        {
            return ProdutoDAL.ListarExcluidos(data);
        }

        public static void Excluir(int idProduto)
        {
            //using (TransactionScope s = new TransactionScope())
            //{
            ProdutoInformation obj = Carregar(idProduto);

            if (obj.TipoProduto.IDTipoProduto == 30)
                throw new Exception("Esse produto/serviço não pode ser excluído!");

            try
            {
                obj.Excluido = true;
                obj.Ativo = false;
                obj.Disponibilidade = false;
                obj.DtUltimaAlteracao = DateTime.Now;
                obj.DtAlteracaoDisponibilidade = DateTime.Now;
                Salvar(obj);

                // CRUD.Excluir(new ProdutoCategoriaProdutoInformation { Produto = obj });
                CRUD.Excluir(new MapAreaImpressaoProdutoInformation { Produto = obj });
                CRUD.Excluir(new ProdutoPainelModificacaoInformation { Produto = obj });
                CRUD.Excluir(new PainelModificacaoProdutoInformation { Produto = obj });

            }
            catch (Exception)
            {
                throw new Exception("O item foi apenas desativado! Não pode ser excluído pois existem registros relacionados a ele");
            }
        }

        public static List<AreaImpressaoInformation> ListarAreaImpressao(int idProduto)
        {
            return ProdutoDAL.ListarAreaImpressao(idProduto);
        }

        public static bool ExisteMovimentacao(int idProduto)
        {
            return EntradaSaidaDAL.ExisteMovimentacao(idProduto);
        }

        public static bool ExisteMovimentacao(this ProdutoInformation produto)
        {
            if (!produto.IDProduto.HasValue)
                return false;
            else if (produto.IDProduto.Value <= 0)
                return false;
            else
                return ExisteMovimentacao(produto.IDProduto.Value);
        }

        public static bool UtilizaBalanca(int idProduto)
        {
            return ProdutoDAL.UtilizaBalanca(idProduto);
        }

        public static ProdutoInformation BuscarPorEan(string ean)
        {
            var idProduto = ProdutoDAL.BuscarPorEAN(ean);
            if (idProduto.HasValue)
                return Carregar(idProduto.Value);
            return null;
        }

        public static bool ExistemProdutosComEstoqueControlado()
        {
            return ProdutoDAL.ExistemProdutosComEstoqueControlado();
        }

        public static ProdutoInformation BuscarPorCodigoIntegracao(string codigo) //int tipo,
        {
            var idProduto = ProdutoDAL.BuscarPorCodigoIntegracao(codigo); // tipo, 
            if (idProduto.HasValue)
                return CarregarCompleto(idProduto.Value);
            return null;
        }

        public static List<ProdutoInformation> ListarPorData(DateTime data, bool ativos, bool? alteradoOuDisponibilidade = null)
        {
            return ProdutoDAL.ListarPorData(data, ativos, alteradoOuDisponibilidade: alteradoOuDisponibilidade);
        }

        /// <summary>
        /// O Código do produto é comporto por 1 digito + código interno + peso
        /// </summary>
        /// <remarks>
        /// Código de Teste: T10
        /// </remarks>
        public static ProdutoInformation ObterProdutoEtiqueta(string ean, out decimal? quantidade, int digitosCodigo, bool peso)
        {
            var result = default(ProdutoInformation);
            quantidade = null;

            if (ean?.Length == 13)
            {
                var codigo = Convert.ToInt32(ean.Substring(1).Substring(0, digitosCodigo));
                var produto = Produto.BuscarPorCodigoIntegracao(codigo.ToString()); // 0, 
                if (produto != null)
                {
                    result = produto;
                    if (peso)
                    {
                        quantidade = Convert.ToDecimal(ean.Substring(11 - digitosCodigo, 5)) / 1000;
                    }
                    else
                    {
                        var preco = Convert.ToDecimal(ean.Substring(11 - digitosCodigo, 5)) / 100;
                        quantidade = Math.Round(preco / produto.ValorUnitario.Value, 3);
                    }
                }
            }
            return result;
        }

        //public static bool ProdutosSemAreaProducao()
        //{
        //    return ProdutoDAL.ProdutosSemAreaProducao();
        //}

        //public static bool ProdutosSemCategoriaProduto(bool listarCreditos)
        //{
        //    return ProdutoDAL.ProdutosSemCategoriaProduto(listarCreditos);
        //}

        public static List<ProdutoInformation> ListarPorCategorias(int[] ids, bool listarCreditos)
        {
            return ProdutoDAL.ListarPorCategorias(ids, listarCreditos);
        }

        public static List<ProdutoInformation> ListarSemCategorias(bool listarCreditos)
        {
            return ProdutoDAL.ListarSemCategorias(listarCreditos);
        }

        public static List<int> ProdutosIndisponivelPorCategoria()
        {
            using (var pdv = new pdv7Context())
            {
                return pdv.Database.SqlQuery<int>(@"SELECT DISTINCT IDProduto 
FROM tbProdutoCategoriaProduto p
INNER JOIN tbCategoriaProduto c ON c.IDCategoriaProduto = p.IDCategoriaProduto
WHERE c.Disponibilidade = 0;").ToList();
            }
        }
    }
}
