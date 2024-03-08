using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using a7D.PDV.Model;
using a7D.PDV.BLL;
using System.Xml.Linq;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;

namespace a7D.PDV.Integracao.WS
{
    /// <summary>
    /// Summary description for Pedido
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Pedido : System.Web.Services.WebService
    {
        private IFormatProvider _formatProvider = new CultureInfo("pt-BR");

        private HashSet<string> Exec
        {
            get
            {
                if (HttpContext.Current.Application.Get("ExecDictionary") == null)
                {
                    HttpContext.Current.Application.Set("ExecDictionary", new HashSet<string>());
                }
                return (HashSet<string>)HttpContext.Current.Application.Get("ExecDictionary");
            }
            set { }
        }

        private bool Executando(string guid)
        {
            return Exec.Contains(guid);
        }

        [WebMethod]
        public string ValidarMesa(Int32 numero)
        {
            String xmlRetorno;
            MesaInformation mesa;
            PedidoInformation pedido;

            try
            {
                mesa = Mesa.Validar(numero);
                pedido = BLL.Pedido.CarregarUltimoPedido(mesa.GUIDIdentificacao);

                xmlRetorno = "<validarMesa>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
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

                    xmlRetorno += "    <valorTotal>" + BLL.Pedido.ValorTotalOuCredito(pedido, out bool credito).ToString() + "</valorTotal>";
                    xmlRetorno += "    <totalECredito>" + (credito ? "1" : "0") + "</totalECredito>";
                    xmlRetorno += "    <porcentagemServico>" + pedido.TaxaServicoPadrao.ToString() + "</porcentagemServico>";
                    xmlRetorno += "    <qtdItens></qtdItens>";

                    if (pedido.NumeroPessoas.HasValue)
                    {
                        xmlRetorno += "    <quantidadePessoas>" + pedido.NumeroPessoas.Value.ToString() + "</quantidadePessoas>";
                    }

                    if (!string.IsNullOrWhiteSpace(pedido.DocumentoCliente))
                    {
                        xmlRetorno += "    <cpf>" + pedido.DocumentoCliente + "</cpf>";
                    }

                    if (pedido.Cliente != null)
                    {
                        xmlRetorno += "    <cliente>" + pedido.Cliente.NomeCompleto + "</cliente>";
                    }
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
            catch (Exception ex)
            {
                ex.Data.Add("numero", numero);
                xmlRetorno = "<validarMesa>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</validarMesa>";
            }

            return xmlRetorno;
        }

        [WebMethod]
        public string ValidarComanda(long numero)
        {
            String xmlRetorno;
            PedidoInformation pedido;

            try
            {
                var retorno = Comanda.ValidarComRetorno(numero);
                var comanda = retorno.Comanda;
                pedido = BLL.Pedido.CarregarUltimoPedido(comanda.GUIDIdentificacao);

                xmlRetorno = "<validarComanda>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "    <guidIdentificacao>" + comanda.GUIDIdentificacao + "</guidIdentificacao>";
                xmlRetorno += "    <idTipoPedido>20</idTipoPedido>";
                xmlRetorno += "    <numero>" + comanda.Numero + "</numero>";
                xmlRetorno += "    <idPedido>" + pedido.IDPedido.ToString() + "</idPedido>";
                xmlRetorno += "    <referenciaLocalizacao>" + pedido.ReferenciaLocalizacao + "</referenciaLocalizacao>";
                xmlRetorno += $"    <checkin>{(retorno.ComCheckin ? "1" : "0")}</checkin>";

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

                xmlRetorno += "    <valorTotal>" + BLL.Pedido.ValorTotalOuCredito(pedido, out bool credito).ToString() + "</valorTotal>";
                xmlRetorno += "    <totalECredito>" + (credito ? "1" : "0") + "</totalECredito>";
                xmlRetorno += "    <porcentagemServico>" + pedido.TaxaServicoPadrao.ToString() + "</porcentagemServico>";
                xmlRetorno += "    <qtdItens></qtdItens>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</validarComanda>";
            }
            catch (Exception ex)
            {
                ex.Data.Add("numero", numero);
                xmlRetorno = "<validarComanda>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</validarComanda>";
            }

            return xmlRetorno;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlSolicitacao">
        /// <adicionarProdutos>
        ///   <guidIdentificacao>??</guidIdentificacao>
        ///   <idTipoPedido>??</idTipoPedido>
        ///   <idPedido>??</idPedido>
        ///   <idUsuario>??</idUsuario>
        ///   <idPDV>??</idPDV>
        ///   <listaProdutos>
        ///     <produto>
        ///       <idProduto>??</idProduto>
        ///       <qtd>??</qtd>
        ///       <notas>??</notas>
        ///       <listaModificacoes>
        ///         <idProduto>??</idProduto>
        ///         <idProduto>??</idProduto>
        ///       </listaModificacoes>
        ///     </produto>
        ///   </listaProdutos>
        /// </adicionarProdutos>
        /// </param>
        /// <returns></returns>
        /// 

        private static object @lock = new object();

        [WebMethod]
        public string AdicionarProdutos(string xmlSolicitacao)
        {
            String xmlRetorno;

            try
            {
                List<PedidoProdutoInformation> listaPedidoProduto = new List<PedidoProdutoInformation>();
                List<PedidoProdutoInformation> listaProdutoImpressao1 = new List<PedidoProdutoInformation>();
                Dictionary<Int32, List<PedidoProdutoInformation>> listaProdutoImpressao2 = new Dictionary<int, List<PedidoProdutoInformation>>();


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlSolicitacao);

                XmlNode adicionarProdutos = xmlDoc["adicionarProdutos"];
                XmlNode listaProdutos = adicionarProdutos["listaProdutos"];
                XmlNode listaModificacoes;

                Int32 idTipoPedido = Convert.ToInt32(adicionarProdutos["idTipoPedido"].InnerText);
                long numero = Convert.ToInt64(adicionarProdutos["numero"].InnerText);
                Int32 idUsuario = Convert.ToInt32(adicionarProdutos["idUsuario"].InnerText);
                Int32 idPDV = Convert.ToInt32(adicionarProdutos["idPDV"].InnerText);
                String guidControleDuplicidade = null;
                String guidIdentificacao;
                String referenciaLocalizacao = null;

                var usuario = Usuario.Carregar(idUsuario);
                var pdv = BLL.PDV.Carregar(idPDV);

                if (adicionarProdutos["guidPedido"] != null)
                    guidControleDuplicidade = Convert.ToString(adicionarProdutos["guidPedido"].InnerText);

                if (adicionarProdutos["referenciaLocalizacao"] != null)
                    referenciaLocalizacao = Convert.ToString(adicionarProdutos["referenciaLocalizacao"].InnerText);

                var tipoPedido = (ETipoPedido)idTipoPedido;
                PedidoInformation pedido = null;

                if (string.IsNullOrWhiteSpace(guidControleDuplicidade) == false)
                {
                    try
                    {
                        while (Executando(guidControleDuplicidade))
                        {

                        }

                        lock (@lock)
                        {
                            Exec.Add(guidControleDuplicidade);
                        }

                        if (PedidoProduto.PedidoDuplicado(guidControleDuplicidade))
                        {
                            xmlRetorno = "<adicionarProdutos>";
                            xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                            xmlRetorno += "  <status>1</status>";
                            xmlRetorno += "  <retorno>";
                            xmlRetorno += "  </retorno>";
                            xmlRetorno += "</adicionarProdutos>";

                            return xmlRetorno;
                        }

                        try
                        {


                            switch (tipoPedido)
                            {
                                case ETipoPedido.Mesa:
                                    Mesa.Validar((int)numero);
                                    guidIdentificacao = Mesa.CarregarPorNumero((int)numero).GUIDIdentificacao;
                                    break;
                                case ETipoPedido.Comanda:
                                    Comanda.Validar(numero);
                                    guidIdentificacao = Comanda.CarregarPorNumeroOuCodigo(numero).GUIDIdentificacao;
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }

                            //String guidIdentificacao = adicionarProdutos["guidIdentificacao"].InnerText;
                            //Int32 idPedido = Convert.ToInt32(adicionarProdutos["idPedido"].InnerText);

                            Int32 idProduto;
                            Int32 qtd;
                            String notas;
                            Boolean viagem;
                            ProdutoInformation objProduto;
                            PedidoProdutoInformation pedidoProduto;
                            Int32 idAreaImpressao;

                            foreach (XmlNode produto in listaProdutos.SelectNodes("produto"))
                            {
                                idProduto = Convert.ToInt32(produto["idProduto"].InnerText);
                                qtd = Convert.ToInt32(produto["qtd"].InnerText);
                                notas = produto["notas"].InnerText;
                                decimal? valor = produto["valor"] != null ? decimal.Parse(produto["valor"].InnerText, _formatProvider) : (decimal?)null;
                                viagem = produto["viagem"] == null ? false : produto["viagem"].InnerText == "1";

                                objProduto = BLL.Produto.ObterProduto(idProduto);
                                pedidoProduto = new PedidoProdutoInformation()
                                {
                                    ListaModificacao = new List<PedidoProdutoInformation>(),
                                    GUIDControleDuplicidade = guidControleDuplicidade,
                                    PDV = pdv,
                                    Usuario = usuario,
                                    Produto = objProduto,
                                    Quantidade = qtd,
                                    Notas = notas,
                                    ValorUnitario = valor.HasValue ? valor : objProduto.ValorUnitario,
                                    CodigoAliquota = objProduto.CodigoAliquota,
                                    Viagem = viagem,
                                    Cancelado = false
                                };
                                listaModificacoes = produto["listaModificacoes"];
                                foreach (XmlNode modificacao in listaModificacoes)
                                {
                                    var pedidoProdutoModificacao = new PedidoProdutoInformation()
                                    {
                                        PDV = pdv,
                                        Usuario = usuario,
                                        Quantidade = 1,
                                        Cancelado = false
                                    };

                                    if (modificacao.ChildNodes.Count == 1)
                                    {
                                        // forma antiga (só id do produto modificado)
                                        pedidoProdutoModificacao.PedidoProdutoPai = pedidoProduto;
                                        pedidoProdutoModificacao.Produto = BLL.Produto.ObterProduto(Convert.ToInt32(modificacao.InnerText));
                                        pedidoProdutoModificacao.ValorUnitario = pedidoProdutoModificacao.Produto.ValorUnitario;
                                        pedidoProdutoModificacao.CodigoAliquota = pedidoProdutoModificacao.Produto.CodigoAliquota;
                                    }
                                    else
                                    {
                                        // forma de assistente com arvore de produtos modificacos
                                        pedidoProdutoModificacao.IDPedidoProduto_Original = pedidoProdutoModificacao.IDPedidoProduto = Int32.Parse(modificacao["idPedidoProduto"].InnerText);
                                        if (modificacao["idPedidoProdutoPai"] != null)
                                        {
                                            pedidoProdutoModificacao.PedidoProdutoPai = new PedidoProdutoInformation()
                                            {
                                                IDPedidoProduto_Original = Int32.Parse(modificacao["idPedidoProdutoPai"].InnerText)
                                            };
                                        }
                                        else
                                            pedidoProdutoModificacao.PedidoProdutoPai = pedidoProduto;

                                        pedidoProdutoModificacao.Produto = BLL.Produto.ObterProduto(Convert.ToInt32(modificacao["idProduto"].InnerText));
                                        pedidoProdutoModificacao.ValorUnitario = decimal.Parse(modificacao["valor"].InnerText, _formatProvider);
                                        pedidoProdutoModificacao.CodigoAliquota = pedidoProdutoModificacao.Produto.CodigoAliquota;
                                        pedidoProdutoModificacao.Notas = modificacao["observacao"].InnerText;
                                    }

                                    pedidoProduto.ListaModificacao.Add(pedidoProdutoModificacao);
                                }

                                //foreach (XmlNode nodeModificacao in listaModificacoes)
                                //{
                                //    var modificacaoIdProduto = nodeModificacao["idProduto"].InnerText;
                                //    var modificacaoIdProdutoPai = nodeModificacao["idProdutoPai"].InnerText;
                                //    var modificacaoObservacao = nodeModificacao["observacao"].InnerText;

                                //    idProduto_modificacao = Convert.ToInt32(modificacaoIdProduto);
                                //    objProdutoModificacao = BLL.Produto.Carregar(idProduto_modificacao);

                                //    pedidoProdutoModificacao = new PedidoProdutoInformation()
                                //    {
                                //        Notas = modificacaoObservacao,
                                //        Produto = objProdutoModificacao,
                                //        PedidoProdutoPai = new PedidoProdutoInformation
                                //        {
                                //            Produto = new ProdutoInformation
                                //            {
                                //                IDProduto = Convert.ToInt32(modificacaoIdProdutoPai)
                                //            },
                                //            //Pedido = pedidoProduto.Pedido

                                //        },
                                //        PDV = new PDVInformation { IDPDV = idPDV },
                                //        Usuario = new UsuarioInformation { IDUsuario = idUsuario },
                                //        Quantidade = 1,
                                //        ValorUnitario = objProdutoModificacao.ValorUnitario,
                                //        CodigoAliquota = objProdutoModificacao.CodigoAliquota,
                                //        //pedidoProdutoModificacao.PainelModificacao = new PainelModificacaoInformation { IDPainelModificacao = Convert.ToInt32(modificacao["idPainelModificacao"].InnerText) };

                                //        Cancelado = false
                                //    };
                                //    pedidoProduto.ListaModificacao.Add(pedidoProdutoModificacao);
                                //}

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
                                        List<PedidoProdutoInformation> list = new List<PedidoProdutoInformation>()
                                        {
                                            pedidoProduto
                                        };
                                        listaProdutoImpressao2.Add(idAreaImpressao, list);
                                    }
                                }
                                else
                                {
                                    listaProdutoImpressao1.Add(pedidoProduto);
                                }
                            }

                            pedido = BLL.Pedido.AdicionarProduto(tipoPedido, guidIdentificacao, idUsuario, idPDV, listaPedidoProduto, referenciaLocalizacao);

                        }
                        catch (Exception ex)
                        {
                            throw new ExceptionPDV(CodigoErro.EB0C, ex);
                        }

                        var ordemImpressao = ConfiguracaoBD.ValorOuPadrao(EConfig.OrdemImpressao, pdv);
                        if (ordemImpressao == "1")
                        {
                            try
                            {
                                OrdemProducaoServices.GerarOrdemProducao(listaProdutoImpressao1);
                                OrdemProducaoServices.GerarOrdemProducao(listaProdutoImpressao2);
                            }
                            catch (Exception ex)
                            {
                                throw new ExceptionPDV(CodigoErro.A200, ex);
                            }
                        }

                        var imprimirViaExpedicao = ConfiguracaoBD.ValorOuPadrao(EConfig.ImprimirViaExpedicao, pdv);
                        if (imprimirViaExpedicao == "PEDIDO" && pedido != null)
                        {
                            var idAreaViaExpedicao = Int32.Parse(ConfiguracaoBD.ValorOuPadrao(EConfig.IDAreaViaExpedicao, pdv));
                            OrdemProducaoServices.GerarViaExpedicao(pedido.IDPedido.Value, idAreaViaExpedicao);
                        }

                    }
                    catch (ExceptionPDV ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("guidControleDuplicidade", guidControleDuplicidade);
                        Logs.Erro(ex);
                    }
                    finally
                    {
                        Exec.Remove(guidControleDuplicidade);
                    }
                }

                GA.PostEvento(pdv, "Pedido: " + tipoPedido, usuario: usuario);

                xmlRetorno = "<adicionarProdutos>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</adicionarProdutos>";
            }
            catch (Exception ex)
            {
                ex.Data.Add("xmlSolicitacao", xmlSolicitacao);
                xmlRetorno = "<adicionarProdutos>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</adicionarProdutos>";
            }

            return xmlRetorno;
        }

        [WebMethod]
        public string SolicitarFechamentoConta(String xmlSolicitacao)
        {
            String xmlRetorno;

            try
            {
                PedidoInformation pedido;
                String guidIdentificacao;

                List<PedidoProdutoInformation> listaPedidoProduto = new List<PedidoProdutoInformation>();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlSolicitacao);

                XmlNode solicitarFechamentoConta = xmlDoc["solicitarFechamentoConta"];
                //XmlNode listaProdutos = adicionarProdutos["listaProdutos"];
                //XmlNode listaModificacoes;

                ETipoPedido tipoPedido = (ETipoPedido)Convert.ToInt32(solicitarFechamentoConta["idTipoPedido"].InnerText);
                long numero = Convert.ToInt64(solicitarFechamentoConta["numero"].InnerText);
                int? numeroPessoas = null;
                string cpf = null;
                string cliente = null;
                int tipoCliente = -1;
                bool info;

                var infoMesaElement = solicitarFechamentoConta["infoMesa"];

                info = infoMesaElement?.InnerText == "1";

                if (info)
                {
                    var quantidadeElement = solicitarFechamentoConta["quantidadePessoas"];
                    numeroPessoas = Convert.ToInt32(quantidadeElement?.InnerText);

                    var cpfElement = solicitarFechamentoConta["cpf"];
                    cpf = cpfElement?.InnerText;

                    var clienteElement = solicitarFechamentoConta["cliente"];
                    cliente = clienteElement?.InnerText;

                    var tipoClienteElement = solicitarFechamentoConta["tipoCliente"];
                    tipoCliente = Convert.ToInt32(tipoClienteElement?.InnerText);
                }

                var idUsuario = Convert.ToInt32(solicitarFechamentoConta["idUsuario"].InnerText);
                var idPDV = Convert.ToInt32(solicitarFechamentoConta["idPDV"].InnerText);

                if (tipoPedido == ETipoPedido.Mesa)
                    guidIdentificacao = Mesa.CarregarPorNumero((int)numero).GUIDIdentificacao;
                else
                    guidIdentificacao = Comanda.CarregarPorNumeroOuCodigo(numero).GUIDIdentificacao;

                pedido = BLL.Pedido.CarregarUltimoPedido(guidIdentificacao);
                if (pedido.IDPedido == null || pedido.StatusPedido.StatusPedido != EStatusPedido.Aberto)
                {
                    if (tipoPedido == ETipoPedido.Mesa)
                        pedido = BLL.Pedido.NovoPedidoMesa(guidIdentificacao);
                    else
                        pedido = BLL.Pedido.NovoPedidoComanda(guidIdentificacao, null, null);
                }

                if (tipoPedido == ETipoPedido.Mesa)
                    BLL.Mesa.AlterarStatus(guidIdentificacao, EStatusMesa.ContaSolicitada);
                else
                    BLL.Comanda.AlterarStatus(guidIdentificacao, EStatusComanda.ContaSolicitada);

                if (info)
                {
                    pedido.NumeroPessoas = numeroPessoas;

                    if (!string.IsNullOrEmpty(cpf)
                    && (ValidacaoDocumento.IsCpf(cpf) || ValidacaoDocumento.IsCnpj(cpf)))
                        pedido.DocumentoCliente = cpf;

                    if (pedido.Cliente?.IDCliente == null)
                    {
                        try
                        {
                            var clienteEF = Cliente.BuscarCliente((BLL.Cliente.TipoCliente)tipoCliente, cliente);
                            if (clienteEF != null)
                                pedido.Cliente = Cliente.Carregar(clienteEF.IDCliente);
                        }
                        catch (Exception ex)
                        {
                            ex.Data.Add("cliente", cliente);
                            ex.Data.Add("TipoCliente", tipoCliente);
                            Logs.Erro(ex);
                        }
                    }
                    BLL.Pedido.Salvar(pedido);
                }

                if (pedido.ListaProduto.Any(p => p.Viagem == true))
                {
                    string cIDAreaViaExpedicao = ConfiguracaoBD.ValorOuPadrao(EConfig.IDAreaViaExpedicao, idPDV);
                    if (!string.IsNullOrEmpty(cIDAreaViaExpedicao)
                     && int.TryParse(cIDAreaViaExpedicao, out int IDAreaViaExpedicao)
                     && IDAreaViaExpedicao > 0)
                        OrdemProducaoServices.GerarViaExpedicao(pedido.IDPedido.Value, IDAreaViaExpedicao);
                }

                var impressaoContaElement = solicitarFechamentoConta["impressaoConta"];
                if (impressaoContaElement != null)
                {
                    var idAreaImpressao = Convert.ToInt32(impressaoContaElement.InnerText);
                    if (idAreaImpressao > 0)
                        OrdemProducaoServices.GerarConta(pedido.IDPedido.Value, idUsuario, idPDV, idAreaImpressao);
                    else
                        throw new Exception("Conta fechada! mas não pode ser impressa por não possuir uma área de impressão configurada");
                }
                else
                {
                    int idPadrao = (int)ETipoAreaImpressao.ContaPadrao;
                    var areaContaPadrao = EF.Repositorio.Carregar<tbAreaImpressao>(i => i.IDTipoAreaImpressao == idPadrao);
                    if (areaContaPadrao != null)
                        OrdemProducaoServices.GerarConta(pedido.IDPedido.Value, idUsuario, idPDV, areaContaPadrao.IDAreaImpressao);
                }

                xmlRetorno = "<solicitarFechamentoConta>";
                xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                xmlRetorno += "  <status>1</status>";
                xmlRetorno += "  <retorno>";
                xmlRetorno += "  </retorno>";
                xmlRetorno += "</solicitarFechamentoConta>";
            }
            catch (Exception ex)
            {
                ex.Data.Add("xmlSolicitacao", xmlSolicitacao);
                xmlRetorno = "<solicitarFechamentoConta>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</solicitarFechamentoConta>";
            }

            return xmlRetorno;
        }

        /// <summary>
        /// <pedido>
        ///    <idTipoPedido>0</idTipoPedido>
        ///    <numero>0</numero>
        /// </pedido>
        /// </summary>
        /// <param name="xmlSolicitacao"></param>
        /// <returns></returns>
        [WebMethod]
        public string RetornarPedido(String xmlSolicitacao)
        {
            String xmlRetorno;

            try
            {
                PedidoInformation pedido = new PedidoInformation();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlSolicitacao);

                XmlNode xmlPedido = xmlDoc["pedido"];

                int idTipoPedido = Convert.ToInt32(xmlPedido["idTipoPedido"].InnerText);
                long numero = Convert.ToInt64(xmlPedido["numero"].InnerText);
                String guidIdentificacao;

                switch (idTipoPedido)
                {
                    case 10://Mesa
                        guidIdentificacao = Mesa.CarregarPorNumero((int)numero).GUIDIdentificacao;
                        break;
                    case 20://Comanda
                        guidIdentificacao = Comanda.CarregarPorNumeroOuCodigo(numero).GUIDIdentificacao;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                pedido = BLL.Pedido.CarregarUltimoPedido(guidIdentificacao);
                if (pedido != null && pedido.IDPedido != null)
                {
                    xmlRetorno = "<pedido>";
                    xmlRetorno += "  <versao>" + Util.Versao + "</versao>";
                    xmlRetorno += "  <status>1</status>";
                    xmlRetorno += "  <retorno>";
                    xmlRetorno += "    <pedido>";
                    xmlRetorno += "      <idPedido>" + pedido.IDPedido.ToString() + "</idPedido>";

                    xmlRetorno += "      <listaProduto>";
                    foreach (PedidoProdutoInformation pedidoProduto in pedido.ListaProduto.Where(l => !l.Cancelado.Value).OrderBy(l => l.DtInclusao).ToList())
                    {
                        xmlRetorno += "        <produto>";
                        xmlRetorno += "          <idProduto>" + pedidoProduto.Produto.IDProduto.ToString() + "</idProduto>";
                        xmlRetorno += "          <valorUnitario>" + pedidoProduto.ValorUnitario.Value.ToString("#,##0.00", _formatProvider) + "</valorUnitario>";
                        xmlRetorno += "          <qtd>" + pedidoProduto.Quantidade.Value.ToString(_formatProvider) + "</qtd>";
                        xmlRetorno += "          <viagem>" + (pedidoProduto.Viagem == true ? "1" : "0") + "</viagem>";
                        xmlRetorno += "          <notas>" + pedidoProduto.Notas + "</notas>";

                        xmlRetorno += "          <listaModificacoes>";
                        foreach (var modificacao in pedidoProduto.ListaModificacao)
                        {
                            xmlRetorno += "            <idPainelModificacao>" + modificacao?.PainelModificacao?.IDPainelModificacao + "</idPainelModificacao>";
                            xmlRetorno += "            <idProduto>" + modificacao.Produto.IDProduto.ToString() + "</idProduto>";
                            xmlRetorno += "            <qtd>" + modificacao.Quantidade.ToString() + "</qtd>";
                            xmlRetorno += "            <valorUnitario>" + modificacao.ValorUnitario.Value.ToString("#,##0.00") + "</valorUnitario>";
                        }

                        xmlRetorno += "          </listaModificacoes>";
                        xmlRetorno += "        </produto>";
                    }
                    xmlRetorno += "      </listaProduto>";
                    xmlRetorno += "    </pedido>";
                    xmlRetorno += "  </retorno>";
                    xmlRetorno += "</pedido>";
                }
                else
                {
                    throw new ExceptionPDV(CodigoErro.A304);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("xmlSolicitacao", xmlSolicitacao);
                xmlRetorno = "<pedido>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</pedido>";
            }

            return xmlRetorno;
        }

        [WebMethod]
        public string AlterarReferenciaLocalizacao(string xmlSolicitacao)
        {
            string xmlRetorno = "";
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlSolicitacao);

                XmlNode node = xmlDoc["AlterarReferenciaLocalizacao"];
                var idTipoPedido = Convert.ToInt32(node["IDTipoPedido"].InnerText);
                var guidIdentificacao = node["GUIDIdentificacao"].InnerText;
                var referencia = node["ReferenciaLocalizacao"].InnerText;

                var pedido = BLL.Pedido.CarregarUltimoPedido(guidIdentificacao);

                if (!string.IsNullOrWhiteSpace(referencia) && pedido?.IDPedido.HasValue == true)
                {
                    if (ConfiguracoesSistema.Valores.ReferenciaMesa)
                    {
                        if (Int32.TryParse(referencia, out int mesa))
                            pedido.ReferenciaLocalizacao = mesa.ToString();
                        else
                            throw new ExceptionPDV(CodigoErro.AC3A);
                    }
                    else
                        pedido.ReferenciaLocalizacao = referencia;

                    BLL.Pedido.Salvar(pedido);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("xmlSolicitacao", xmlSolicitacao);
                xmlRetorno = "<alterarReferenciaLocalizacao>";
                xmlRetorno += Util.RetornaErro(ex);
                xmlRetorno += "</alterarReferenciaLocalizacao>";
            }

            return xmlRetorno;
        }
    }
}
