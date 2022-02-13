using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace a7D.PDV.Integracao.API.Controllers
{
    public class ProdutosController : ApiController
    {
        string Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [HttpGet]
        public HttpResponseMessage ListarCategorias(DateTime dtUltimaVerificacao)
        {
            var status = HttpStatusCode.OK;
            string xmlRetorno;

            try
            {
                List<CategoriaProdutoInformation> lista = CategoriaProduto.Listar().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).OrderBy(l => l.IDCategoriaProduto).ToList();
                List<CategoriaProdutoInformation> listaExcluidos = CategoriaProduto.ListarExcluidos().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).OrderBy(l => l.IDCategoriaProduto).ToList();

                xmlRetorno = "<listarCategorias>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <multi>1</multi>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <listaCategoria>";

                foreach (var item in lista)
                {
                    xmlRetorno += "      <categoria>";
                    xmlRetorno += "        <idCategoriaProduto>" + item.IDCategoriaProduto + "</idCategoriaProduto>";
                    xmlRetorno += "        <nome>" + item.Nome + "</nome>";

                    xmlRetorno += "        <ativo>1</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + item.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtUltimaAlteracao>";
                    xmlRetorno += "      </categoria>";
                }

                foreach (var item in listaExcluidos)
                {
                    xmlRetorno += "      <categoria>";
                    xmlRetorno += "        <idCategoriaProduto>" + item.IDCategoriaProduto + "</idCategoriaProduto>";
                    xmlRetorno += "        <nome>" + item.Nome + "</nome>";

                    xmlRetorno += "        <ativo>0</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + item.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtUltimaAlteracao>";
                    xmlRetorno += "      </categoria>";
                }

                xmlRetorno += "    </listaCategoria>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarCategorias>";
            }
            catch (Exception _e)
            {
                xmlRetorno = "<listarCategorias>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarCategorias>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }

        [HttpGet]
        public HttpResponseMessage ListarProdutos(DateTime dtUltimaVerificacao)
        {
            var status = HttpStatusCode.OK;
            String xmlRetorno;
            Int32 i;
            Int32 ii;

            //dtUltimaVerificacao = new DateTime(dtUltimaVerificacao.Ticks - (dtUltimaVerificacao.Ticks % TimeSpan.TicksPerSecond), dtUltimaVerificacao.Kind);

            try
            {
                var ini = DateTime.Now;
                List<ProdutoInformation> lista = BLL.Produto.ListarAtivosDAL(dtUltimaVerificacao).OrderBy(l => l.Nome).ToList();
                System.Diagnostics.Debug.WriteLine("Produtos ativos: {0} milisegundos", (DateTime.Now - ini).Milliseconds);
                ini = DateTime.Now;
                List<ProdutoInformation> listaExcluidos = BLL.Produto.ListarExcluidosDAL(dtUltimaVerificacao).OrderBy(l => l.Nome).ToList();
                System.Diagnostics.Debug.WriteLine("Produtos excluídos: {0} milisegundos", (DateTime.Now - ini).Milliseconds);

                ini = DateTime.Now;
                var listaProdutoPainelModificacao = ProdutoPainelModificacao.Listar();
                System.Diagnostics.Debug.WriteLine("Painéis de modificação: {0} milisegundos", (DateTime.Now - ini).Milliseconds);
                ini = DateTime.Now;
                var listaProdutoCategoriaProduto = ProdutoCategoriaProduto.Listar();
                System.Diagnostics.Debug.WriteLine("Categorias: {0} milisegundos", (DateTime.Now - ini).Milliseconds);
                ini = DateTime.Now;
                var listaTraducoes = ProdutoTraducao.ListarIdiomaCompleto();
                System.Diagnostics.Debug.WriteLine("Idiomas: {0} milisegundos", (DateTime.Now - ini).Milliseconds);

                foreach (var item in lista)
                {
                    item.ListaPainelModificacao = listaProdutoPainelModificacao.Where(l => l.Produto.IDProduto == item.IDProduto).ToList();
                }

                xmlRetorno = "<listarProdutos>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                //xmlRetorno += "  <multi>1</multi>";
                xmlRetorno += "  <retorno>";

                xmlRetorno += "    <listaProduto>";
                i = 0;
                foreach (var produto in lista)
                {
                    ini = DateTime.Now;
                    i = i + 10;

                    xmlRetorno += "      <produto>";
                    xmlRetorno += "        <idProduto>" + produto.IDProduto + "</idProduto>";
                    xmlRetorno += "        <idTipoProduto>" + produto.TipoProduto.IDTipoProduto + "</idTipoProduto>";
                    xmlRetorno += "        <nome>" + produto.Nome + "</nome>";
                    xmlRetorno += "        <descricao>" + produto.Descricao + "</descricao>";
                    xmlRetorno += "        <valorUnitario>" + produto.ValorUnitario.Value.ToString("#0.00") + "</valorUnitario>";
                    xmlRetorno += "        <textoAdicionar></textoAdicionar>";

                    if (ProdutoImagem.Existe(produto.IDProduto.Value))
                    {
                        xmlRetorno += "        <urlImagemThumb>ImagensProdutos/" + produto.IDProduto.ToString() + "_thumb.png</urlImagemThumb>";
                        xmlRetorno += "        <urlImagem>ImagensProdutos/" + produto.IDProduto.ToString() + ".png</urlImagem>";
                    }
                    else
                    {
                        xmlRetorno += "        <urlImagemThumb></urlImagemThumb>";
                        xmlRetorno += "        <urlImagem></urlImagem>";
                    }

                    xmlRetorno += "        <exibicaoPainelModificacao>lista</exibicaoPainelModificacao>";
                    xmlRetorno += "        <ativo>" + Convert.ToInt32(produto.Ativo.Value) + "</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + produto.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss.fff") + "</dtUltimaAlteracao>";

                    xmlRetorno += "        <disponivel>" + Convert.ToInt32(produto.Disponibilidade.Value) + "</disponivel>";
                    xmlRetorno += "        <dtAlteracaoDisponibilidade>" + produto.DtAlteracaoDisponibilidade.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtAlteracaoDisponibilidade>";

                    xmlRetorno += "        <listaCategoria>";

                    //xmlRetorno += "          <categoria>";
                    //var categoria = CategoriaProduto.PrimeiraPorProduto(produto.IDProduto.Value);
                    //if (categoria != null)
                    //{
                    //    xmlRetorno += "            <idCategoriaProduto>" + categoria.IDCategoriaProduto + "</idCategoriaProduto>";
                    //}
                    //else
                    //{
                    //    xmlRetorno += "            <idCategoriaProduto></idCategoriaProduto>";
                    //}
                    //xmlRetorno += "          </categoria>";

                    //var categorias = ProdutoCategoriaProduto.ListarPorProduto(produto.IDProduto.Value);
                    var categorias = listaProdutoCategoriaProduto.Where(pcp => pcp.Produto.IDProduto.Value == produto.IDProduto.Value).ToList();

                    if (categorias != null && categorias.Count > 0)
                    {
                        foreach (var produtoCategoriaProduto in categorias)
                        {
                            xmlRetorno += "          <categoria id='" + produtoCategoriaProduto.IDProdutoCategoriaProduto.Value + "'>";
                            xmlRetorno += "            <idCategoriaProduto>" + produtoCategoriaProduto.CategoriaProduto.IDCategoriaProduto + "</idCategoriaProduto>";
                            xmlRetorno += "          </categoria>";
                        }
                    }
                    //else
                    //{
                    //    xmlRetorno += "            <idCategoriaProduto></idCategoriaProduto>";
                    //}
                    xmlRetorno += "        </listaCategoria>";
                    ii = 0;
                    xmlRetorno += "        <listaPainelModificacao>";
                    foreach (var painelModificacao in produto.ListaPainelModificacao)
                    {
                        ii = ii + 10;
                        xmlRetorno += "          <painelModificacao>";
                        xmlRetorno += "            <idPainelModificacao>" + painelModificacao.PainelModificacao.IDPainelModificacao + "</idPainelModificacao>";
                        xmlRetorno += "            <ordem>" + ii + "</ordem>";
                        xmlRetorno += "          </painelModificacao>";
                    }
                    xmlRetorno += "        </listaPainelModificacao>";

                    //ini = DateTime.Now;
                    List<AreaImpressaoInformation> listaAreaImpressao = BLL.Produto.ListarAreaImpressao(produto.IDProduto.Value);
                    //System.Diagnostics.Debug.WriteLine("Áreas de impressão: {0} segundos", (ini - DateTime.Now).Seconds);

                    xmlRetorno += "        <listaAreaImpressao>";
                    foreach (var areaImpressao in listaAreaImpressao)
                    {
                        xmlRetorno += "          <areaImpressao>";
                        xmlRetorno += "            <idAreaImpressao>" + areaImpressao.IDAreaImpressao + "</idAreaImpressao>";
                        xmlRetorno += "          </areaImpressao>";
                    }
                    xmlRetorno += "        </listaAreaImpressao>";

                    List<ProdutoTraducaoInformation> traducoes = listaTraducoes.Where(pt => pt.Produto.IDProduto.Value == produto.IDProduto.Value).ToList();

                    if (traducoes.Count > 0)
                    {
                        xmlRetorno += "        <traducoes>";
                        var gtraducoes = traducoes.GroupBy(t => t.Idioma, (key, value) => new { Idioma = key, Traducoes = value.ToList() });
                        foreach (var gt in gtraducoes)
                        {
                            foreach (var item in gt.Traducoes)
                            {
                                xmlRetorno += "        <traducao idioma='" + gt.Idioma.Codigo + "'>";
                                xmlRetorno += "          <id>" + item.IDProdutoTraducao.Value + "</id>";
                                xmlRetorno += "          <produtotrad>" + produto.IDProduto.Value + "</produtotrad>";
                                xmlRetorno += "          <nome>" + item.Nome + "</nome>";
                                xmlRetorno += "          <descricao>" + item.Descricao + "</descricao>";
                                xmlRetorno += "         </traducao>";
                            }
                        }
                        xmlRetorno += "        </traducoes>";
                    }

                    xmlRetorno += "      </produto>";
                    System.Diagnostics.Debug.WriteLine("Produto {0} em {1} milisegundos.", (i / 10), (DateTime.Now - ini).Milliseconds);
                }

                foreach (var produto in listaExcluidos)
                {
                    ini = DateTime.Now;
                    i = i + 10;

                    xmlRetorno += "      <produto>";
                    xmlRetorno += "        <idProduto>" + produto.IDProduto + "</idProduto>";
                    xmlRetorno += "        <idTipoProduto>" + produto.TipoProduto.IDTipoProduto + "</idTipoProduto>";
                    xmlRetorno += "        <nome>" + produto.Nome + "</nome>";
                    xmlRetorno += "        <descricao></descricao>";
                    xmlRetorno += "        <valorUnitario>0,00</valorUnitario>";
                    xmlRetorno += "        <textoAdicionar></textoAdicionar>";
                    xmlRetorno += "        <urlImagemThumb></urlImagemThumb>";
                    xmlRetorno += "        <urlImagem></urlImagem>";
                    xmlRetorno += "        <exibicaoPainelModificacao></exibicaoPainelModificacao>";
                    xmlRetorno += "        <ativo>0</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + produto.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtUltimaAlteracao>";
                    xmlRetorno += "        <disponivel>" + Convert.ToInt32(produto.Disponibilidade.Value) + "</disponivel>";
                    xmlRetorno += "        <dtAlteracaoDisponibilidade>" + produto.DtAlteracaoDisponibilidade.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtAlteracaoDisponibilidade>";

                    xmlRetorno += "      </produto>";
                    System.Diagnostics.Debug.WriteLine("Produto {0} em {1} milisegundos.", (i / 10), (DateTime.Now - ini).Milliseconds);
                }

                xmlRetorno += "    </listaProduto>";

                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarProdutos>";


            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<listarProdutos>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarProdutos>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }

        [HttpGet]
        public HttpResponseMessage ListarPaineisModificacao(DateTime dtUltimaVerificacao)
        {
            var status = HttpStatusCode.OK;
            String xmlRetorno;
            Int32 i;

            try
            {
                List<PainelModificacaoInformation> lista = PainelModificacao.ListarCompleto().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();
                List<PainelModificacaoInformation> listaExcluidos = PainelModificacao.ListarExcluidos().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();

                xmlRetorno = "<listarPaineisModificacoes>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <listaPainelModificacao>";

                foreach (var painelModificacao in lista)
                {
                    xmlRetorno += "      <painelModificacao>";
                    xmlRetorno += "        <idPainelModificacao>" + painelModificacao.IDPainelModificacao.ToString() + "</idPainelModificacao>";
                    xmlRetorno += "        <titulo>" + painelModificacao.Titulo + "</titulo>";
                    xmlRetorno += "        <min>" + painelModificacao.Minimo.ToString() + "</min>";
                    xmlRetorno += "        <max>" + painelModificacao.Maximo.ToString() + "</max>";

                    xmlRetorno += "        <ativo>1</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + painelModificacao.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtUltimaAlteracao>";

                    xmlRetorno += "        <listaProdutoModificacao>";

                    i = 0;
                    foreach (var produto in painelModificacao.ListaProduto)
                    {
                        i = i + 10;

                        xmlRetorno += "          <produtoModificacao>";
                        xmlRetorno += "            <selecionado>0</selecionado>";
                        xmlRetorno += "            <tratamentoValor>somar</tratamentoValor>";
                        xmlRetorno += "            <ordem>" + i + "</ordem>";
                        xmlRetorno += "            <produto>";
                        xmlRetorno += "              <idProduto>" + produto.Produto.IDProduto.ToString() + "</idProduto>";
                        xmlRetorno += "            </produto>";
                        xmlRetorno += "          </produtoModificacao>";
                    }

                    xmlRetorno += "        </listaProdutoModificacao>";
                    xmlRetorno += "      </painelModificacao>";
                }

                foreach (var painelModificacao in listaExcluidos)
                {
                    xmlRetorno += "      <painelModificacao>";
                    xmlRetorno += "        <idPainelModificacao>" + painelModificacao.IDPainelModificacao.ToString() + "</idPainelModificacao>";
                    xmlRetorno += "        <titulo></titulo>";
                    xmlRetorno += "        <min></min>";
                    xmlRetorno += "        <max></max>";

                    xmlRetorno += "        <ativo>0</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + painelModificacao.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtUltimaAlteracao>";
                    xmlRetorno += "      </painelModificacao>";
                }

                xmlRetorno += "    </listaPainelModificacao>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarPaineisModificacoes>";
            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<listarPaineisModificacoes>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarPaineisModificacoes>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }

        [HttpGet]
        public HttpResponseMessage ListarDisponibilidadeProdutos(DateTime dtUltimaVerificacao)
        {
            var status = HttpStatusCode.OK;
            String xmlRetorno;
            try
            {
                List<ProdutoInformation> lista = BLL.Produto.Listar().Where(l => l.DtAlteracaoDisponibilidade >= dtUltimaVerificacao.AddMinutes(-2)).ToList();

                xmlRetorno = "<listarDisponibilidadeProdutos>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";

                xmlRetorno += "    <listaProduto>";
                foreach (var produto in lista)
                {
                    xmlRetorno += "      <produto>";
                    xmlRetorno += "        <idProduto>" + produto.IDProduto + "</idProduto>";
                    xmlRetorno += "        <disponibilidade>" + Convert.ToInt32(produto.Disponibilidade.Value) + "</disponibilidade>";
                    xmlRetorno += "        <dtAlteracaoDisponibilidade>" + produto.DtAlteracaoDisponibilidade.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtAlteracaoDisponibilidade>";
                    xmlRetorno += "      </produto>";
                }
                xmlRetorno += "    </listaProduto>";

                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarDisponibilidadeProdutos>";
            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<listarDisponibilidadeProdutos>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarDisponibilidadeProdutos>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }

        [HttpGet]
        public HttpResponseMessage ListarProdutosCategoriaProduto(DateTime dtUltimaVerificacao)
        {
            var status = HttpStatusCode.OK;
            string xmlRetorno;
            try
            {
                var lista = ProdutoCategoriaProduto.ListarCompleto();
                lista = lista.Where(pcp => pcp.Produto.DtUltimaAlteracao > dtUltimaVerificacao).ToList();
                lista = lista
                            .OrderBy(pcp => pcp.Produto.IDProduto.Value)
                            .ThenBy(pcp => pcp.CategoriaProduto.IDCategoriaProduto.Value)
                            .ToList();
                var listaIdProdutos = lista.Select(pcp => pcp.Produto.IDProduto.Value).Distinct().ToArray();
                lista = lista.Where(pcp => pcp.CategoriaProduto.Excluido == false).ToList();

                xmlRetorno = "<listarProdutosCategoriaProduto>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "  <produtos>" + string.Join(",", listaIdProdutos) + "</produtos>";

                xmlRetorno += "    <listaProdutoCategoriaProduto>";

                foreach (var pcp in lista.Where(pcp => pcp.Produto.Excluido == false))
                {
                    xmlRetorno += "      <produtoCategoriaProduto>";
                    xmlRetorno += "         <idProdutoCategoriaProduto>" + pcp.IDProdutoCategoriaProduto.Value + "</idProdutoCategoriaProduto>";
                    xmlRetorno += "         <idProduto>" + pcp.Produto.IDProduto.Value + "</idProduto>";
                    xmlRetorno += "         <idCategoriaProduto>" + pcp.CategoriaProduto.IDCategoriaProduto.Value + "</idCategoriaProduto>";
                    xmlRetorno += "      </produtoCategoriaProduto>";
                }

                xmlRetorno += "    </listaProdutoCategoriaProduto>";


                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarProdutosCategoriaProduto>";
            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<listarProdutosCategoriaProduto>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarProdutosCategoriaProduto>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }
    }
}
