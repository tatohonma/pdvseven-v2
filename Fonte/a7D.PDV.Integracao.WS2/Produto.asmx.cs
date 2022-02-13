using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using System.Configuration;
using System.IO;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.Integracao.WS
{
    /// <summary>
    /// Summary description for Produto
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Produto : System.Web.Services.WebService
    {
        [WebMethod]
        public String ListarCategorias(DateTime dtUltimaVerificacao)
        {
            String xmlRetorno;

            try
            {
                List<CategoriaProdutoInformation> lista = CategoriaProduto.Listar().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).OrderBy(l => l.IDCategoriaProduto).ToList();
                List<CategoriaProdutoInformation> listaExcluidos = CategoriaProduto.ListarExcluidos().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).OrderBy(l => l.IDCategoriaProduto).ToList();

                xmlRetorno = "<listarCategorias>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <multi>1</multi>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <listaCategoria>";

                foreach (var item in lista)
                {
                    xmlRetorno += "      <categoria>";
                    xmlRetorno += "        <idCategoriaProduto>" + item.IDCategoriaProduto + "</idCategoriaProduto>";
                    xmlRetorno += "        <nome>" + item.Nome + "</nome>";

                    xmlRetorno += "        <ativo>1</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + item.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtUltimaAlteracao>";
                    xmlRetorno += "        <disponibilidade>" + Convert.ToInt32(item.Disponibilidade.Value) + "</disponibilidade>";
                    xmlRetorno += "        <dtAlteracaoDisponibilidade>" + item.DtAlteracaoDisponibilidade.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtAlteracaoDisponibilidade>";
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
            catch (Exception ex)
            {
                xmlRetorno = "<listarCategorias>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</listarCategorias>";
            }

            return xmlRetorno;
        }

        [WebMethod]
        public String ListarProdutos(DateTime dtUltimaVerificacao)
        {
            String xmlRetorno;

            try
            {
                Int32 i;
                Int32 ii;

                List<ProdutoInformation> lista = BLL.Produto.ListarPorData(dtUltimaVerificacao, true)
                    .OrderBy(l => l.Nome)
                    .Where(p => p.TipoProduto.IDTipoProduto != (int)ETipoProduto.Ingrediente)
                    .ToList();

                List<ProdutoInformation> listaExcluidos = BLL.Produto.ListarExcluidosDAL(dtUltimaVerificacao)
                    .OrderBy(l => l.Nome)
                    .ToList();

                var listaProdutoPainelModificacao = ProdutoPainelModificacao.Listar().OrderBy(p => p.Ordem);
                var listaProdutoCategoriaProduto = ProdutoCategoriaProduto.Listar();
                var listaTraducoes = ProdutoTraducao.ListarIdiomaCompleto();
                var listaImagem = ProdutoImagem.Listar();

                foreach (var item in lista)
                {
                    item.ListaPainelModificacao = listaProdutoPainelModificacao.Where(l => l.Produto.IDProduto == item.IDProduto).ToList();
                }

                xmlRetorno = "<listarProdutos>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <multi>1</multi>";
                xmlRetorno += "  <retorno>";

                xmlRetorno += "    <listaProduto>";
                i = 0;
                foreach (var produto in lista)
                {
                    i = i + 10;

                    xmlRetorno += "      <produto>";
                    xmlRetorno += "        <idProduto>" + produto.IDProduto + "</idProduto>";
                    xmlRetorno += "        <idTipoProduto>" + produto.TipoProduto.IDTipoProduto + "</idTipoProduto>";
                    xmlRetorno += "        <nome>" + produto.Nome + "</nome>";
                    xmlRetorno += "        <descricao>" + produto.Descricao + "</descricao>";
                    xmlRetorno += "        <valorUnitario>" + produto.ValorUnitario.Value.ToString("#0.00") + "</valorUnitario>";

                    if (produto.ValorUnitario2 == null)
                        xmlRetorno += "        <valorUnitario2>-1</valorUnitario2>";
                    else
                        xmlRetorno += "        <valorUnitario2>" + produto.ValorUnitario2.Value.ToString("#0.00") + "</valorUnitario2>";

                    if (produto.ValorUnitario3 == null)
                        xmlRetorno += "        <valorUnitario3>-1</valorUnitario3>";
                    else
                        xmlRetorno += "        <valorUnitario3>" + produto.ValorUnitario3.Value.ToString("#0.00") + "</valorUnitario3>";


                    xmlRetorno += "        <AssistenteModificacoes>" + (produto.AssistenteModificacoes == true ? 1 : 0) + "</AssistenteModificacoes>";

                    xmlRetorno += "        <textoAdicionar></textoAdicionar>";

                    if (listaImagem.Any(pi => pi.Produto.IDProduto == produto.IDProduto.Value))
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

                    xmlRetorno += "          <categoria>";
                    var categoria = listaProdutoCategoriaProduto.FirstOrDefault(pcp => pcp.Produto.IDProduto == produto.IDProduto.Value);
                    if (categoria != null)
                    {
                        xmlRetorno += "            <idCategoriaProduto>" + categoria.CategoriaProduto.IDCategoriaProduto + "</idCategoriaProduto>";
                    }
                    else
                    {
                        xmlRetorno += "            <idCategoriaProduto></idCategoriaProduto>";
                    }
                    xmlRetorno += "          </categoria>";

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
                    else
                    {
                        xmlRetorno += "            <idCategoriaProduto></idCategoriaProduto>";
                    }
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

                    List<AreaImpressaoInformation> listaAreaImpressao = BLL.Produto.ListarAreaImpressao(produto.IDProduto.Value);
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
                }

                foreach (var produto in listaExcluidos)
                {
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
                }

                xmlRetorno += "    </listaProduto>";

                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarProdutos>";

            }
            catch (Exception ex)
            {
                xmlRetorno = "<listarProdutos>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</listarProdutos>";
            }

            return xmlRetorno;
        }

        [WebMethod]
        public String ListarPaineisModificacoes(DateTime dtUltimaVerificacao)
        {
            String xmlRetorno;

            try
            {
                Int32 i;

                List<PainelModificacaoInformation> lista = PainelModificacao.ListarCompleto().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();
                List<PainelModificacaoInformation> listaExcluidos = PainelModificacao.ListarExcluidos().Where(l => l.DtUltimaAlteracao > dtUltimaVerificacao).ToList();

                xmlRetorno = "<listarPaineisModificacoes>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <listaPainelModificacao>";

                foreach (var painelModificacao in lista)
                {
                    xmlRetorno += "      <painelModificacao>";
                    xmlRetorno += "        <idPainelModificacao>" + painelModificacao.IDPainelModificacao.ToString() + "</idPainelModificacao>";
                    xmlRetorno += "        <titulo>" + painelModificacao.Titulo + "</titulo>";
                    xmlRetorno +=$"        <min>{painelModificacao.Minimo}</min>";
                    xmlRetorno +=$"        <max>{painelModificacao.Maximo ?? painelModificacao.Minimo}</max>";

                    if (painelModificacao.PainelModificacaoOperacao == null)
                        xmlRetorno += "        <idPainelModificacaoOperacao>1</idPainelModificacaoOperacao>";
                    else
                        xmlRetorno += "        <idPainelModificacaoOperacao>" + painelModificacao.PainelModificacaoOperacao.IDPainelModificacaoOperacao.Value.ToString() + "</idPainelModificacaoOperacao>";

                    if (painelModificacao.IDValorUtilizado == null)
                        xmlRetorno += "        <idValorUtilizado>1</idValorUtilizado>";
                    else
                        xmlRetorno += "        <idValorUtilizado>" + painelModificacao.IDValorUtilizado.ToString() + "</idValorUtilizado>";

                    if (painelModificacao.IgnorarValorItem == null || painelModificacao.IgnorarValorItem == false)
                        xmlRetorno += "        <IgnorarValorItem>1</IgnorarValorItem>";
                    else
                        xmlRetorno += "        <IgnorarValorItem>2</IgnorarValorItem>";

                    xmlRetorno += "        <idTipoItem>" + painelModificacao.IDTipoItem.ToString() + "</idTipoItem>";
                    xmlRetorno += "        <ativo>1</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + painelModificacao.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtUltimaAlteracao>";

                    if (painelModificacao.IDTipoItem == 3)
                    {
                        xmlRetorno += "        <listaCategoriaModificacao>";
                        foreach (var categoria in painelModificacao.ListaCategoria.OrderBy(l => l.Ordem))
                        {

                            xmlRetorno += "          <categoriaModificacao>";
                            xmlRetorno += "            <selecionado>0</selecionado>";
                            xmlRetorno += "            <ordem>" + categoria.Ordem.ToString() + "</ordem>";
                            xmlRetorno += "            <categoria>";
                            xmlRetorno += "              <idCategoria>" + categoria.Categoria.IDCategoriaProduto.ToString() + "</idCategoria>";
                            xmlRetorno += "            </categoria>";
                            xmlRetorno += "          </categoriaModificacao>";
                        }
                        xmlRetorno += "        </listaCategoriaModificacao>";
                    }
                    else
                    {
                        xmlRetorno += "        <listaProdutoModificacao>";
                        i = 0;
                        foreach (var produto in painelModificacao.ListaProduto.OrderBy(l => l.Ordem))
                        {
                            i = i + 10;

                            xmlRetorno += "          <produtoModificacao>";
                            xmlRetorno += "            <selecionado>0</selecionado>";
                            xmlRetorno += "            <ordem>" + i + "</ordem>";
                            xmlRetorno += "            <produto>";
                            xmlRetorno += "              <idProduto>" + produto.Produto.IDProduto.ToString() + "</idProduto>";
                            xmlRetorno += "            </produto>";
                            xmlRetorno += "          </produtoModificacao>";
                        }
                        xmlRetorno += "        </listaProdutoModificacao>";
                    }

                    xmlRetorno += "        <listaPaineisRelacionados>";
                    foreach (var painel in painelModificacao.PaineisRelacionados) //.PaineisRelacionados.OrderBy(l => l.Ordem))
                    {

                        xmlRetorno += "          <painelRelacionado>";
                        //xmlRetorno += "            <idPainelPai>" + painel.PainelModificacao1.IDPainelModificacao.ToString() + "</idPainelPai>";
                        xmlRetorno += "            <idPainelRelacionado>" + painel.PainelModificacao2.IDPainelModificacao.ToString() + "</idPainelRelacionado>";
                        //xmlRetorno += "            <ordem>" + painel.Ordem.ToString() + "</ordem>";
                        xmlRetorno += "          </painelRelacionado>";
                    }
                    xmlRetorno += "        </listaPaineisRelacionados>";




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
            catch (Exception ex)
            {
                xmlRetorno = "<listarPaineisModificacoes>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</listarPaineisModificacoes>";
            }
           

            return xmlRetorno;
        }

        [WebMethod]
        public string ListarDisponibilidadeCategorias(DateTime dtUltimaVerificacao)
        {
            var xmlRetorno = string.Empty;

            try
            {
                var lista = CategoriaProduto.Listar().Where(l => l.DtAlteracaoDisponibilidade >= dtUltimaVerificacao.AddMilliseconds(-500));

                xmlRetorno = "<listarDisponibilidadeCategorias>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";

                xmlRetorno += "    <listaCategorias>";

                foreach (var item in lista)
                {
                    xmlRetorno += "      <categoria>";
                    xmlRetorno += "        <idCategoriaProduto>" + item.IDCategoriaProduto + "</idCategoriaProduto>";
                    xmlRetorno += "        <nome>" + item.Nome + "</nome>";

                    xmlRetorno += "        <ativo>1</ativo>";
                    xmlRetorno += "        <dtUltimaAlteracao>" + item.DtUltimaAlteracao.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtUltimaAlteracao>";
                    xmlRetorno += "        <disponibilidade>" + Convert.ToInt32(item.Disponibilidade.Value) + "</disponibilidade>";
                    xmlRetorno += "        <dtAlteracaoDisponibilidade>" + item.DtAlteracaoDisponibilidade.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtAlteracaoDisponibilidade>";
                    xmlRetorno += "      </categoria>";
                }

                xmlRetorno += "    </listaCategorias>";

                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarDisponibilidadeCategorias>";
            }
            catch (Exception ex)
            {
                xmlRetorno = "<listarDisponibilidadeCategorias>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</listarDisponibilidadeCategorias>";
            }

            return xmlRetorno;
        }

        [WebMethod]
        public String ListarDisponibilidadeProdutos(DateTime dtUltimaVerificacao)
        {
            String xmlRetorno;

            try
            {
                List<ProdutoInformation> lista = BLL.Produto.Listar(new ProdutoInformation() { Ativo=true, Excluido=false } )
                    .Where(l => l.DtAlteracaoDisponibilidade >= dtUltimaVerificacao.AddMilliseconds(-1000))
                    .ToList();

                var produtosCategoriaProduto = ProdutoCategoriaProduto.ListarComCategoria();

                xmlRetorno = "<listarDisponibilidadeProdutos>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";

                xmlRetorno += "    <listaProduto>";
                foreach (var produto in lista)
                {
                    var indisponivel = produtosCategoriaProduto.Any(pcp => pcp.Produto.IDProduto == produto.IDProduto && pcp.CategoriaProduto.Disponibilidade == false);
                    xmlRetorno += "      <produto>";
                    xmlRetorno += "        <idProduto>" + produto.IDProduto + "</idProduto>";
                    xmlRetorno += "        <disponibilidade>" + Convert.ToInt32(indisponivel ? false : produto.Disponibilidade.Value) + "</disponibilidade>";
                    xmlRetorno += "        <dtAlteracaoDisponibilidade>" + produto.DtAlteracaoDisponibilidade.Value.ToString("yyyy-MM-dd HH:mm:ss") + "</dtAlteracaoDisponibilidade>";
                    xmlRetorno += "      </produto>";
                }
                xmlRetorno += "    </listaProduto>";

                xmlRetorno += "  </retorno>";
                xmlRetorno += "</listarDisponibilidadeProdutos>";
            }
            catch (Exception ex)
            {
                xmlRetorno = "<listarDisponibilidadeProdutos>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</listarDisponibilidadeProdutos>";
            }

            return xmlRetorno;
        }

        [WebMethod]
        public string ListarProdutosCategoriaProduto(DateTime dtUltimaVerificacao)
        {
            string xmlRetorno;
            try
            {
                var lista = ProdutoCategoriaProduto.ListarCompleto();
                lista = lista.Where(pcp => pcp.Produto != null).Where(pcp => pcp.Produto.DtUltimaAlteracao > dtUltimaVerificacao).ToList();
                lista = lista
                            .OrderBy(pcp => pcp.Produto.IDProduto.Value)
                            .ThenBy(pcp => pcp.CategoriaProduto.IDCategoriaProduto.Value)
                            .ToList();
                var listaIdProdutos = lista.Select(pcp => pcp.Produto.IDProduto.Value).Distinct().ToArray();
                lista = lista.Where(pcp => pcp.CategoriaProduto.Excluido == false).ToList();

                xmlRetorno = "<listarProdutosCategoriaProduto>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
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
            catch (Exception ex)
            {
                xmlRetorno = "<listarProdutosCategoriaProduto>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</listarProdutosCategoriaProduto>";
            }

            return xmlRetorno;
        }
    }
}