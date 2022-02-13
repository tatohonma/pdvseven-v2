using a7D.PDV.BLL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace a7D.PDV.Integracao.API.Controllers
{
    public class PedidoController : ApiController
    {
        string Versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [HttpPost]
        public HttpResponseMessage ValidarMesa(int numero)
        {
            var status = HttpStatusCode.OK;
            string xmlRetorno;
            MesaInformation mesa;
            PedidoInformation pedido;

            try
            {
                mesa = Mesa.Validar(numero);
                pedido = BLL.Pedido.CarregarUltimoPedido(mesa.GUIDIdentificacao);

                xmlRetorno = "<validarMesa>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";

                if (pedido.IDPedido != null)
                {
                    xmlRetorno += "    <guidIdentificacao>" + mesa.GUIDIdentificacao + "</guidIdentificacao>";
                    xmlRetorno += "    <idTipoPedido>10</idTipoPedido>";
                    xmlRetorno += "    <idPedido>" + pedido.IDPedido.ToString() + "</idPedido>";
                    xmlRetorno += "    <referenciaLocalizacao>" + pedido.ReferenciaLocalizacao + "</referenciaLocalizacao>";

                    if (pedido.Cliente != null)
                        xmlRetorno += "    <cliente>" + pedido.Cliente.NomeCompleto + "</cliente>";
                    else
                        xmlRetorno += "    <cliente></cliente>";

                    xmlRetorno += "    <valorProdutos>" + pedido.ValorTotalProdutos.ToString() + "</valorProdutos>";
                    xmlRetorno += "    <valorServico>" + pedido.ValorServicoTemp.ToString() + "</valorServico>";

                    if (pedido.TipoEntrada != null)
                    {
                        xmlRetorno += "    <valorEntrada>0</valorEntrada>";
                        xmlRetorno += "    <valorConsumacaoMinima>" + pedido.TipoEntrada.ValorConsumacaoMinima.ToString() + "</valorConsumacaoMinima>";
                    }
                    else
                    {
                        xmlRetorno += "    <valorEntrada>0,00</valorEntrada>";
                        xmlRetorno += "    <valorConsumacaoMinima>0,00</valorConsumacaoMinima>";
                    }

                    xmlRetorno += "    <valorTotal>" + pedido.ValorTotalTemp.ToString() + "</valorTotal>";
                    xmlRetorno += "    <porcentagemServico>" + pedido.TaxaServicoPadrao.ToString() + "</porcentagemServico>";
                    xmlRetorno += "    <qtdItens></qtdItens>";
                }
                else
                {
                    xmlRetorno += "    <guidIdentificacao>" + mesa.GUIDIdentificacao + "</guidIdentificacao>";
                    xmlRetorno += "    <idTipoPedido>10</idTipoPedido>";
                    xmlRetorno += "    <idPedido>0</idPedido>";
                    xmlRetorno += "    <valorProdutos>0,00</valorProdutos>";
                    xmlRetorno += "    <valorServico>0,00</valorServico>";
                    xmlRetorno += "    <valorEntrada>0,00</valorEntrada>";
                    xmlRetorno += "    <valorConsumacaoMinima>0,00</valorConsumacaoMinima>";
                    xmlRetorno += "    <valorTotal>0,00</valorTotal>";
                    xmlRetorno += "    <porcentagemServico>0,00</porcentagemServico>";
                    xmlRetorno += "    <qtdItens>0</qtdItens>";
                }

                xmlRetorno += "  </retorno>";
                xmlRetorno += "</validarMesa>";
            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<validarMesa>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <codigoErro></codigoErro>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</validarMesa>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }

        [HttpPost]
        public HttpResponseMessage ValidarComanda(int numero)
        {
            var status = HttpStatusCode.OK;
            string xmlRetorno;

            ComandaInformation comanda;
            PedidoInformation pedido;

            try
            {
                comanda = Comanda.Validar(numero);
                pedido = BLL.Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);

                xmlRetorno = "<validarComanda>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <guidIdentificacao>" + comanda.GUIDIdentificacao + "</guidIdentificacao>";
                xmlRetorno += "    <idTipoPedido>20</idTipoPedido>";
                xmlRetorno += "    <idPedido>" + pedido.IDPedido.ToString() + "</idPedido>";
                xmlRetorno += "    <referenciaLocalizacao>" + pedido.ReferenciaLocalizacao + "</referenciaLocalizacao>";

                if (pedido.Cliente != null)
                    xmlRetorno += "    <cliente>" + pedido.Cliente.NomeCompleto + "</cliente>";
                else
                    xmlRetorno += "    <cliente></cliente>";

                xmlRetorno += "    <valorProdutos>" + pedido.ValorTotalProdutos.ToString() + "</valorProdutos>";
                xmlRetorno += "    <valorServico>" + pedido.ValorServicoTemp.ToString() + "</valorServico>";

                if (pedido.TipoEntrada != null)
                {
                    xmlRetorno += "    <valorEntrada>" + pedido.TipoEntrada.ValorEntrada.ToString() + "</valorEntrada>";
                    xmlRetorno += "    <valorConsumacaoMinima>" + pedido.TipoEntrada.ValorConsumacaoMinima.ToString() + "</valorConsumacaoMinima>";
                }
                else
                {
                    xmlRetorno += "    <valorEntrada>0,00</valorEntrada>";
                    xmlRetorno += "    <valorConsumacaoMinima>0,00</valorConsumacaoMinima>";
                }

                xmlRetorno += "    <valorTotal>" + pedido.ValorTotalTemp.ToString() + "</valorTotal>";
                xmlRetorno += "    <porcentagemServico>" + pedido.TaxaServicoPadrao.ToString() + "</porcentagemServico>";
                xmlRetorno += "    <qtdItens></qtdItens>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</validarComanda>";
            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<validarComanda>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <codigoErro></codigoErro>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</validarComanda>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }

        [HttpPost]
        public HttpResponseMessage AdicionarProdutos(HttpRequestMessage request)
        {
            var status = HttpStatusCode.OK;
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(request.Content.ReadAsStreamAsync().Result);

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SalvarXMLPedido"]) == true)
                xmlDoc.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/") + @"\xml\pedido_" + DateTime.Now.ToString("yyyyMMdd_hhmmssfff") + ".xml");
            //File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/") + @"\xml\pedido_" + DateTime.Now.ToString("yyyyMMdd_hhmmssfff") + ".xml", xmlSolicitacao);

            String xmlRetorno;

            List<PedidoProdutoInformation> listaPedidoProduto = new List<PedidoProdutoInformation>();
            List<PedidoProdutoInformation> listaProdutoImpressao1 = new List<PedidoProdutoInformation>();
            Dictionary<Int32, List<PedidoProdutoInformation>> listaProdutoImpressao2 = new Dictionary<int, List<PedidoProdutoInformation>>();

            try
            {

                XmlNode adicionarProdutos = xmlDoc["adicionarProdutos"];
                XmlNode listaProdutos = adicionarProdutos["listaProdutos"];
                XmlNode listaModificacoes;

                Int32 idTipoPedido = Convert.ToInt32(adicionarProdutos["idTipoPedido"].InnerText);
                Int32 numero = Convert.ToInt32(adicionarProdutos["numero"].InnerText);
                Int32 idUsuario = Convert.ToInt32(adicionarProdutos["idUsuario"].InnerText);
                Int32 idPDV = Convert.ToInt32(adicionarProdutos["idPDV"].InnerText);
                String guidControleDuplicidade = null;
                String guidIdentificacao;
                String referenciaLocalizacao = null;

                if (adicionarProdutos["guidPedido"] != null)
                    guidControleDuplicidade = Convert.ToString(adicionarProdutos["guidPedido"].InnerText);

                if (adicionarProdutos["referenciaLocalizacao"] != null)
                    referenciaLocalizacao = Convert.ToString(adicionarProdutos["referenciaLocalizacao"].InnerText);

                if (guidControleDuplicidade == null || PedidoProduto.PedidoDuplicado(guidControleDuplicidade) == false)
                {
                    switch (idTipoPedido)
                    {
                        case 10://Mesa
                            guidIdentificacao = Mesa.CarregarPorNumero(numero).GUIDIdentificacao;
                            break;
                        case 20://Comanda
                            guidIdentificacao = Comanda.CarregarPorNumero(numero).GUIDIdentificacao;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    //String guidIdentificacao = adicionarProdutos["guidIdentificacao"].InnerText;
                    //Int32 idPedido = Convert.ToInt32(adicionarProdutos["idPedido"].InnerText);

                    Int32 idProduto;
                    Int32 idProduto_modificacao;
                    Int32 qtd;
                    String notas;
                    ProdutoInformation objProduto;
                    ProdutoInformation objProdutoModificacao;
                    PedidoProdutoInformation pedidoProduto;
                    PedidoProdutoInformation pedidoProdutoModificacao;
                    Boolean ordemImpressao;
                    Int32 idAreaImpressao;

                    foreach (XmlNode produto in listaProdutos.SelectNodes("produto"))
                    {
                        idProduto = Convert.ToInt32(produto["idProduto"].InnerText);
                        qtd = Convert.ToInt32(produto["qtd"].InnerText);
                        notas = produto["notas"].InnerText;
                        decimal? valor = produto["valor"] != null ? Convert.ToDecimal(produto["valor"].InnerText) : (decimal?)null;
                        objProduto = BLL.Produto.Carregar(idProduto);
                        pedidoProduto = new PedidoProdutoInformation();
                        pedidoProduto.ListaModificacao = new List<PedidoProdutoInformation>();
                        pedidoProduto.GUIDControleDuplicidade = guidControleDuplicidade;
                        pedidoProduto.PDV = new PDVInformation { IDPDV = idPDV };
                        pedidoProduto.Usuario = new UsuarioInformation { IDUsuario = idUsuario };
                        pedidoProduto.Produto = objProduto;
                        pedidoProduto.Quantidade = qtd;
                        pedidoProduto.Notas = notas;
                        pedidoProduto.ValorUnitario = valor.HasValue ? valor : objProduto.ValorUnitario;
                        pedidoProduto.CodigoAliquota = objProduto.CodigoAliquota;
                        pedidoProduto.Cancelado = false;

                        listaModificacoes = produto["listaModificacoes"];
                        foreach (XmlNode modificacao in listaModificacoes)
                        {
                            idProduto_modificacao = Convert.ToInt32(modificacao.InnerText);
                            objProdutoModificacao = BLL.Produto.Carregar(idProduto_modificacao);

                            pedidoProdutoModificacao = new PedidoProdutoInformation();
                            pedidoProdutoModificacao.Produto = objProdutoModificacao;
                            pedidoProdutoModificacao.PDV = new PDVInformation { IDPDV = idPDV };
                            pedidoProdutoModificacao.Usuario = new UsuarioInformation { IDUsuario = idUsuario };
                            pedidoProdutoModificacao.Quantidade = 1;
                            pedidoProdutoModificacao.ValorUnitario = objProdutoModificacao.ValorUnitario;
                            pedidoProdutoModificacao.CodigoAliquota = objProdutoModificacao.CodigoAliquota;

                            pedidoProdutoModificacao.Cancelado = false;

                            pedidoProduto.ListaModificacao.Add(pedidoProdutoModificacao);
                        }

                        listaPedidoProduto.Add(pedidoProduto);

                        if (produto["idAreaImpressao"] != null && produto["idAreaImpressao"].InnerText != "")
                        {
                            idAreaImpressao = Convert.ToInt32(produto["idAreaImpressao"].InnerText);

                            if (listaProdutoImpressao2.ContainsKey(idAreaImpressao))
                            {
                                listaProdutoImpressao2[idAreaImpressao].Add(pedidoProduto);
                            }
                            else
                            {
                                List<PedidoProdutoInformation> list = new List<PedidoProdutoInformation>();
                                list.Add(pedidoProduto);
                                listaProdutoImpressao2.Add(idAreaImpressao, list);
                            }
                        }
                        else
                        {
                            listaProdutoImpressao1.Add(pedidoProduto);
                        }
                    }

                    BLL.Pedido.AdicionarProduto(idTipoPedido, guidIdentificacao, idUsuario, idPDV, listaPedidoProduto, referenciaLocalizacao);

                    ordemImpressao = Convert.ToBoolean(ConfigurationManager.AppSettings["OrdemImpressao"]);
                    if (ordemImpressao == true)
                    {
                        OrdemImpressao.GerarOrdemProducao(listaProdutoImpressao1);
                        OrdemImpressao.GerarOrdemProducao(listaProdutoImpressao2);
                    }
                }

                xmlRetorno = "<adicionarProdutos>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</adicionarProdutos>";
            }
            catch (Exception _e)
            {
                status = HttpStatusCode.InternalServerError;
                xmlRetorno = "<adicionarProdutos>";
                xmlRetorno += "  <versao>" + Versao + "</versao>";
                xmlRetorno += "  <status>0</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <codigoErro></codigoErro>";
                xmlRetorno += "    <descricaoErro>" + _e.Message + "</descricaoErro>";
                xmlRetorno += "    <descricaoDetalhadaErro>" + _e.ToString() + "</descricaoDetalhadaErro>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</adicionarProdutos>";
            }

            return new HttpResponseMessage()
            {
                StatusCode = status,
                Content = new StringContent(xmlRetorno, Encoding.UTF8, "application/xml")
            };
        }
    }
}
