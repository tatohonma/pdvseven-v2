using a7D.PDV.BLL;
using a7D.PDV.Integracao.API.ExtensionMethods;
using a7D.PDV.Integracao.API.Responses;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace a7D.PDV.Integracao.API.Controllers
{
    public class ProdutosJsonController : ApiController
    {
        private string Versao => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [HttpGet]
        public IHttpActionResult ListarCategorias([FromUri]string ultimaVerificacao)
        {
            var response = new ListarCategoriasResponse();
            response.multi = true;
            var listaCategorias = new List<ListarCategoriasResponse.Categoria>();
            var dtUltimaVerificacao = ultimaVerificacao.ConverterData();

            List<CategoriaProdutoInformation> lista = CategoriaProduto.Listar().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).OrderBy(l => l.IDCategoriaProduto).ToList();
            List<CategoriaProdutoInformation> listaExcluidos = CategoriaProduto.ListarExcluidos().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).OrderBy(l => l.IDCategoriaProduto).ToList();

            lista.ForEach(categoria =>
            {
                listaCategorias.Add(new ListarCategoriasResponse.Categoria
                {
                    idCategoriaProduto = categoria.IDCategoriaProduto.Value,
                    nome = categoria.Nome,
                    ativo = true,
                    dtUltimaAlteracao = categoria.DtUltimaAlteracao.Value
                });
            });

            listaExcluidos.ForEach(categoria =>
            {
                listaCategorias.Add(new ListarCategoriasResponse.Categoria
                {
                    idCategoriaProduto = categoria.IDCategoriaProduto.Value,
                    nome = categoria.Nome,
                    ativo = false,
                    dtUltimaAlteracao = categoria.DtUltimaAlteracao.Value
                });
            });

            response.categorias = listaCategorias.ToArray();

            response.sucesso = true;
            response.versao = Versao;

            return ResponseMessage(new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpGet]
        public IHttpActionResult ListarProdutos([FromUri]string ultimaVerificacao)
        {
            var response = new ListarProdutosResponse();
            var produtos = new List<ListarProdutosResponse.Produto>();

            var dtUltimaVerificacao = ultimaVerificacao.ConverterData();

            List<ProdutoInformation> lista = BLL.Produto.ListarAtivosDAL(dtUltimaVerificacao).OrderBy(l => l.Nome).ToList();
            List<ProdutoInformation> listaExcluidos = BLL.Produto.ListarExcluidosDAL(dtUltimaVerificacao).OrderBy(l => l.Nome).ToList();

            var paineis = ProdutoPainelModificacao.Listar();
            var produtoCategoriaProduto = ProdutoCategoriaProduto.Listar();
            var traducoes = ProdutoTraducao.ListarIdiomaCompleto();

            foreach (var item in lista)
            {
                item.ListaPainelModificacao = paineis.Where(l => l.Produto.IDProduto == item.IDProduto).ToList();
            }

            lista.ForEach(produto =>
            {
                var rProduto = new ListarProdutosResponse.Produto
                {
                    idProduto = produto.IDProduto.Value,
                    idTipoProduto = produto.TipoProduto.IDTipoProduto.Value,
                    nome = produto.Nome,
                    descricao = produto.Descricao,
                    valorUnitario = produto.ValorUnitario.Value,
                    textoAdicionar = string.Empty,
                    ativo = true,
                    imagens = ProdutoImagem.Existe(produto.IDProduto.Value) ? new ListarProdutosResponse.Imagens(produto.IDProduto.Value) : null,
                    dtUltimaAlteracao = produto.DtUltimaAlteracao.Value,
                    disponivel = produto.Disponibilidade.Value,
                    dtAlteracaoDisponibilidade = produto.DtAlteracaoDisponibilidade.Value
                };

                var listaCategorias = new List<ListarProdutosResponse.Categoria>();

                var categorias = produtoCategoriaProduto.Where(pcp => pcp.Produto.IDProduto.Value == produto.IDProduto.Value).ToList();
                categorias.ForEach(pcp =>
                {
                    listaCategorias.Add(new ListarProdutosResponse.Categoria
                    {
                        idCategoria = pcp.IDProdutoCategoriaProduto.Value,
                        idCategoriaProduto = pcp.CategoriaProduto.IDCategoriaProduto.Value
                    });
                });

                rProduto.categorias = listaCategorias.ToArray();

                var listaModificacoes = new List<ListarProdutosResponse.PainelModificacao>();
                int ordem = 1;
                produto.ListaPainelModificacao.ForEach(painelModificacao =>
                {
                    listaModificacoes.Add(new ListarProdutosResponse.PainelModificacao
                    {
                        idPainelModificacao = painelModificacao.PainelModificacao.IDPainelModificacao.Value,
                        ordem = ordem++
                    });
                });

                rProduto.paineisModificacao = listaModificacoes.ToArray();

                var listaAreasImpressao = new List<ListarProdutosResponse.AreaImpressao>();

                Produto.ListarAreaImpressao(produto.IDProduto.Value).ForEach(areaImpressao =>
                {
                    listaAreasImpressao.Add(new ListarProdutosResponse.AreaImpressao
                    {
                        idAreaImpressao = areaImpressao.IDAreaImpressao.Value
                    });
                });

                rProduto.areasImpressao = listaAreasImpressao.ToArray();

                var listaTraducoes = new List<ListarProdutosResponse.Traducao>();

                var tproduto = traducoes.Where(pt => pt.Produto.IDProduto.Value == produto.IDProduto.Value).ToList();
                var gtraducoes = traducoes.GroupBy(t => t.Idioma, (key, value) => new { Idioma = key, Traducoes = value.ToList() });

                foreach (var gt in gtraducoes)
                {
                    foreach (var item in gt.Traducoes)
                    {
                        listaTraducoes.Add(new ListarProdutosResponse.Traducao
                        {
                            idioma = gt.Idioma.Codigo,
                            id = item.IDProdutoTraducao.Value,
                            nome = item.Nome,
                            descricao = item.Descricao
                        });
                    }
                }

                rProduto.traducoes = listaTraducoes.ToArray();

                produtos.Add(rProduto);
            });

            listaExcluidos.ForEach(produto =>
            {
                var rProduto = new ListarProdutosResponse.Produto
                {
                    idProduto = produto.IDProduto.Value,
                    idTipoProduto = produto.TipoProduto.IDTipoProduto.Value,
                    nome = produto.Nome,
                    descricao = produto.Descricao,
                    valorUnitario = 0m,
                    textoAdicionar = string.Empty,
                    ativo = false,
                    dtUltimaAlteracao = produto.DtUltimaAlteracao.Value,
                    disponivel = produto.Disponibilidade.Value,
                    dtAlteracaoDisponibilidade = produto.DtAlteracaoDisponibilidade.Value
                };

                produtos.Add(rProduto);
            });

            response.produtos = produtos.ToArray();

            return ResponseMessage(new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                StatusCode = HttpStatusCode.OK
            });

        }

        [HttpGet]
        public IHttpActionResult ListarPaineisModificacao([FromUri]string ultimaVerificacao)
        {
            var response = new ListarPaineisModificacaoResponse();
            var listaPaineis = new List<ListarPaineisModificacaoResponse.PainelModificacao>();
            var dtUltimaVerificacao = ultimaVerificacao.ConverterData();

            List<PainelModificacaoInformation> lista = PainelModificacao.ListarCompleto().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();
            List<PainelModificacaoInformation> listaExcluidos = PainelModificacao.ListarExcluidos().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();


            lista.ForEach(painelModificacao =>
            {
                var rPainelModificacao = new ListarPaineisModificacaoResponse.PainelModificacao
                {
                    idPainelModificacao = painelModificacao.IDPainelModificacao.Value,
                    titulo = painelModificacao.Titulo,
                    min = painelModificacao.Minimo,
                    max = painelModificacao.Maximo,
                    ativo = true,
                    dtUltimaAlteracao = painelModificacao.DtUltimaAlteracao.Value
                };

                var listaProdutos = new List<ListarPaineisModificacaoResponse.ProdutoModificacao>();
                var ordem = 1;

                painelModificacao.ListaProduto.ForEach(produto =>
                {
                    listaProdutos.Add(new ListarPaineisModificacaoResponse.ProdutoModificacao
                    {
                        ordem = ordem++,
                        idProduto = produto.Produto.IDProduto.Value
                    });
                });

                rPainelModificacao.produtos = listaProdutos.ToArray();

                listaPaineis.Add(rPainelModificacao);
            });

            listaExcluidos.ForEach(painelModificacao =>
            {
                listaPaineis.Add(new ListarPaineisModificacaoResponse.PainelModificacao
                {
                    idPainelModificacao = painelModificacao.IDPainelModificacao.Value,
                    ativo = false,
                    dtUltimaAlteracao = painelModificacao.DtUltimaAlteracao.Value
                });
            });

            response.paineisModificacao = listaPaineis.ToArray();
            response.sucesso = true;
            response.versao = Versao;

            return ResponseMessage(new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                StatusCode = HttpStatusCode.OK
            });

        }

        [HttpGet]
        public IHttpActionResult ListarDisponibilidadeProdutos([FromUri]string ultimaVerificacao)
        {
            var response = new ListarDisponibilidadeResponse();
            var listaProdutos = new List<ListarDisponibilidadeResponse.Produto>();

            var dtUltimaVerificacao = ultimaVerificacao.ConverterData();

            Produto
                .Listar()
                .Where(l => l.DtAlteracaoDisponibilidade >= dtUltimaVerificacao.AddMinutes(-2))
                .ToList()
                .ForEach(produto =>
                {
                    listaProdutos.Add(new ListarDisponibilidadeResponse.Produto
                    {
                        idProduto = produto.IDProduto.Value,
                        disponibilidade = produto.Disponibilidade.Value,
                        dtAlteracaoDisponibilidade = produto.DtAlteracaoDisponibilidade.Value
                    });
                });


            response.produtos = listaProdutos.ToArray();
            response.sucesso = true;
            response.versao = Versao;

            return ResponseMessage(new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                StatusCode = HttpStatusCode.OK
            });
        }

        [HttpGet]
        public IHttpActionResult ListarProdutosCategoriaProduto([FromUri]string ultimaVerificacao)
        {
            var response = new ListarProdutosCategoriaProduto();
            var listaProdutos = new List<ListarProdutosCategoriaProduto.ProdutoCategoriaProduto>();

            var dtUltimaVerificacao = ultimaVerificacao.ConverterData();

            var lista = ProdutoCategoriaProduto.ListarCompleto();
            lista = lista.Where(pcp => pcp.Produto.DtUltimaAlteracao > dtUltimaVerificacao).ToList();
            lista = lista
                        .OrderBy(pcp => pcp.Produto.IDProduto.Value)
                        .ThenBy(pcp => pcp.CategoriaProduto.IDCategoriaProduto.Value)
                        .ToList();
            var listaIdProdutos = lista.Select(pcp => pcp.Produto.IDProduto.Value).Distinct().ToArray();
            lista = lista.Where(pcp => pcp.CategoriaProduto.Excluido == false).ToList();

            response.produtos = listaIdProdutos;

            lista
                .Where(pcp => pcp.Produto.Excluido == false)
                .ToList()
                .ForEach(pcp =>
                {
                    listaProdutos.Add(new ListarProdutosCategoriaProduto.ProdutoCategoriaProduto
                    {
                        idProdutoCategoriaProduto = pcp.IDProdutoCategoriaProduto.Value,
                        idProduto = pcp.Produto.IDProduto.Value,
                        idCategoriaProduto = pcp.CategoriaProduto.IDCategoriaProduto.Value
                    });
                });

            response.produtosCategoriaProduto = listaProdutos.ToArray();
            response.sucesso = true;
            response.versao = Versao;

            return ResponseMessage(new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
