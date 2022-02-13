using a7D.PDV.EF.DAO;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace a7D.PDV.Integracao.WS2.Controllers
{
    public class ProdutosController : ApiController
    {
        private static List<PainelDeModificacao> cachePaineisModificacao = new List<PainelDeModificacao>();
        private static List<ProdutoPainelModificacaoInformation> cachePaineisModificacaoProduto = new List<ProdutoPainelModificacaoInformation>();
        private static DateTime cacheDateLoad = DateTime.MinValue;

        // TODO: A otimizar para não precisar fazer CACHE
        public static void ValidateCache()
        {
            // 2 Minutos de Cache
            if (DateTime.Now.Subtract(cacheDateLoad).TotalMinutes < BLL.ConfiguracoesSistema.TimeRefresh)
            {
                return;
            }

            var paineisModificacao = BLL.PainelModificacao.ListarCompleto();
            var newPaineisModificacaoProduto = BLL.ProdutoPainelModificacao.ListarCompleto();
            var newPaineisModificacao = paineisModificacao
                .Select(pm => new PainelDeModificacao(
                                    pm.IDPainelModificacao, pm.Nome,
                                    pm.Minimo, pm.Maximo, pm.Titulo,
                                    pm.ListaProduto.Select(pmp => new ProdutoPainelModificacao(pmp.Produto.IDProduto, pmp.Produto.ValorUnitario, pmp.Ordem))
                        .ToList()))
                .ToList();

            lock (cachePaineisModificacao)
            {
                cacheDateLoad = DateTime.Now;
                cachePaineisModificacaoProduto = newPaineisModificacaoProduto;
                cachePaineisModificacao = newPaineisModificacao;
            }
        }

        [HttpGet]
        public IHttpActionResult GetProdutos(
            [FromUri] string id = "0",
            [FromUri] string data = "0",
            [FromUri] string categoria = "0",
            [FromUri] string tipo = "0",
            [FromUri] string disponivel = "",
            [FromUri] string ativo = "1",
            [FromUri] string onlyids = "0",
            [FromUri] string top = "0",
            [FromUri] string limit = "0")
        {
            try
            {
                int nID = 0;
                if (id != "0" && !int.TryParse(id, out nID))
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, "ID inválido")));
                }

                DateTime? dtAlteracao = null;
                if (!string.IsNullOrEmpty(data) && data != "0" && long.TryParse(data, out long valData))
                {
                    dtAlteracao = DateTimeOffset.FromUnixTimeSeconds(valData).ToLocalTime().DateTime;
                }

                int idTipo = 0;
                if (tipo != "0" && !int.TryParse(tipo, out idTipo))
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, "Tipo inválido")));
                }

                int idCategoria = 0;
                if (categoria != "0" && !int.TryParse(categoria, out idCategoria))
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, "Categoria inválida")));
                }

                int qtdTop = 0;
                if (top != "0" && !int.TryParse(top, out qtdTop))
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, "TOP invalido")));
                }

                int qtdLimit = 0;
                if (limit != "0" && !int.TryParse(limit, out qtdLimit))
                {
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, "LIMIT invalido")));
                }

                Boolean? bDisponivel = null;
                if (!string.IsNullOrEmpty(disponivel) && disponivel.ToLower().Trim() != "all")
                {
                    bDisponivel = disponivel == "1";
                }

                Boolean? bAtivo = null;
                if (!string.IsNullOrEmpty(ativo) && ativo.ToLower().Trim() != "all")
                {
                    bAtivo = ativo == "1";
                }

                var produtos = ProdutosDAO.Listar(nID, dtAlteracao, idTipo, idCategoria, bDisponivel, bAtivo, qtdTop, qtdLimit);

                var categorias = EF.Repositorio.Listar<EF.Models.tbCategoriaProduto>();
                List<EF.Models.tbAreaImpressao> areas = null;

                if (onlyids == "0")
                {
                    if (qtdTop == 0)
                    {
                        ValidateCache(); // Cerca de 15 segundos em um i5!
                    }

                    areas = EF.Repositorio.Listar<EF.Models.tbAreaImpressao>();
                }

                var result = new List<Produto>();
                foreach (var produto in produtos)
                {
                    // Toda categoria deve existir!!!
                    var mProduto = new Produto(produto.IDProduto, produto.IDTipoProduto, produto.Nome, produto.Descricao, produto.ValorUnitario, produto.ValorUnitario2, produto.ValorUnitario3, produto.Ativo, produto.Excluido, produto.AssistenteModificacoes == true, produto.DtUltimaAlteracao, produto.Disponibilidade, produto.DtAlteracaoDisponibilidade);

                    if (produto.tbProdutoImagems.Any())
                    {
                        mProduto.urlImagemThumb = $"ImagensProdutos/{produto.IDProduto}_thumb.png";
                        mProduto.urlImagem = $"ImagensProdutos/{produto.IDProduto}.png";
                    }

                    if (onlyids == "1")
                    {
                        //Somente IDs

                        mProduto.Categorias = produto.tbProdutoCategoriaProdutoes
                            .Select(cp => new Categoria(cp.IDCategoriaProduto)).ToList();

                        mProduto.AreasDeImpressao = produto.tbMapAreaImpressaoProdutoes
                            .Select(ap => new AreaDeImpressao(ap.IDAreaImpressao)).ToList();

                        mProduto.PaineisDeModificacao = produto.tbProdutoPainelModificacaos
                            .Select(pm => new PainelDeModificacao(pm.IDPainelModificacao)).ToList();

                        if (mProduto.Disponibilidade == true)
                        {
                            mProduto.Disponibilidade = !mProduto.Categorias.Any(cp => categorias.First(c => c.IDCategoriaProduto == cp.IDCategoria).Disponibilidade == false);
                        }
                    }
                    else
                    {
                        // Dados completos

                        mProduto.Categorias = produto.tbProdutoCategoriaProdutoes
                            .Select(cp => DTO.ConverterBasico(categorias.First(c => c.IDCategoriaProduto == cp.IDCategoriaProduto))).ToList();

                        mProduto.AreasDeImpressao = produto.tbMapAreaImpressaoProdutoes
                            .Select(ap => DTO.Converter(areas.First(a => a.IDAreaImpressao == ap.IDAreaImpressao))).ToList();

                        mProduto.PaineisDeModificacao = cachePaineisModificacaoProduto.Where(pmp => pmp.Produto.IDProduto == produto.IDProduto)
                            .Select(pm => cachePaineisModificacao.FirstOrDefault(mpm => mpm.IDPainelModificacao == pm.PainelModificacao.IDPainelModificacao)).ToList();

                        if (mProduto.Disponibilidade == true)
                        {
                            mProduto.Disponibilidade = !mProduto.Categorias.Any(c => c.Disponibilidade == false);
                        }
                    }

                    result.Add(mProduto);
                }

                if (nID > 0)
                {
                    return Ok(result.FirstOrDefault());
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, ex)));
            }
        }

        [HttpPost]
        public IHttpActionResult PostProduto([FromBody] Produto produto)
        {
            try
            {
                var novoProduto = new ProdutoInformation
                {
                    TipoProduto = new TipoProdutoInformation { IDTipoProduto = produto.IDTipoProduto },
                    Nome = produto.Nome,
                    Descricao = produto.Descricao,
                    DtUltimaAlteracao = DateTime.Now,
                    ValorUnitario = produto.ValorUnitario,
                    Ativo = true,
                    Disponibilidade = true,
                    DtAlteracaoDisponibilidade = DateTime.Now,
                    ControlarEstoque = true,
                    UtilizarBalanca = false
                };
                BLL.Produto.SalvarCompleto(novoProduto);
                return Ok();
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, ex)));
            }
        }


        [HttpGet]
        [Route("api/Produtos/disponibilidade")]
        public IHttpActionResult GetDisponibilidade([FromUri] string data = "0")
        {
            DateTime dtDisponibilidade = new DateTime(2019, 1 ,1);
            if (!string.IsNullOrEmpty(data) && data != "0" && long.TryParse(data, out long valData))
            {
                dtDisponibilidade = DateTimeOffset.FromUnixTimeSeconds(valData).ToLocalTime().DateTime;
            }

            try
            {
                List<ProdutoInformation> lista = BLL.Produto.Listar(new ProdutoInformation() { Ativo = true, Excluido = false })
                    .Where(l => l.DtAlteracaoDisponibilidade >= dtDisponibilidade.AddMilliseconds(-1000))
                    .ToList();

                var produtosCategoriaProduto = BLL.ProdutoCategoriaProduto.ListarComCategoria();
                var result = new List<ProdutoDisponibilidade>();
                foreach (var produto in lista)
                {
                    var indisponivel = produtosCategoriaProduto.Any(pcp => pcp.Produto.IDProduto == produto.IDProduto && pcp.CategoriaProduto.Disponibilidade == false);
                    var prod = new ProdutoDisponibilidade()
                    {
                        IDProduto = produto.IDProduto,
                        Disponibilidade = indisponivel ? false : produto.Disponibilidade.Value,
                        DtAlteracaoDisponibilidade = produto.DtAlteracaoDisponibilidade.Value
                    };
                    result.Add(prod);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, new Erro(50, ex)));
            }
        }
    }
}
