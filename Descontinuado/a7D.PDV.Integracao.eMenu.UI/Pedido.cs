using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using a7D.PDV.BLL;
using a7D.PDV.Model;
using System.Xml;
using System.Windows.Forms;
using System.Configuration;

namespace a7D.PDV.Integracao.eMenu.UI
{
    public class Pedido
    {
        #region Solicitacao Serviço eMenu > PDV7
        public void ImportarSolicitacaoServico()
        {
            Boolean processado;
            String caminhoXMLConta = Configuracao.RetornarValor("sincEMENU_CaminhoXMLSolicitacaoEmenu");
            String caminhoXMLProcessado = Configuracao.RetornarValor("sincEMENU_CaminhoXMLSolicitacaoEmenuProcessado");
            FileInfo file;

            String[] arrArquivos = Directory.GetFiles(caminhoXMLConta);

            foreach (var item in arrArquivos)
            {
                processado = false;
                file = new FileInfo(item);

                try
                {
                    processado = ProcessarSolicitacaoServico(item);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (processado == true && caminhoXMLProcessado != null && caminhoXMLProcessado != "")
                        File.Move(item, caminhoXMLProcessado + file.Name);
                }
            }
        }
        private Boolean ProcessarSolicitacaoServico(String arquivoXml)
        {
            Boolean processado = false;
            PedidoInformation pedido = new PedidoInformation();
            MesaInformation mesa;
            Int32 numeroMesa;

            XmlDocument oXML = new XmlDocument();
            XmlNode service = default(XmlNode);

            String tipoServico;

            //carrega o arquivo XML
            oXML.Load(arquivoXml);

            service = oXML.SelectSingleNode("TREEROOT").SelectSingleNode("SERVICE");

            if (service != null)
            {
                tipoServico = service.SelectSingleNode("SERVICE_TYPE").InnerText;

                numeroMesa = Convert.ToInt32(service.SelectSingleNode("TABLE_NO").InnerText);
                mesa = Mesa.CarregarPorNumero(numeroMesa);

                pedido = BLL.Pedido.CarregarUltimoPedido(mesa.GUIDIdentificacao);
                if (pedido.IDPedido == null || pedido.StatusPedido.StatusPedido == EnumStatusPedido.Desconhecido || pedido.StatusPedido.StatusPedido == EnumStatusPedido.Finalizado)
                    pedido = BLL.Pedido.NovoPedidoMesa(mesa.GUIDIdentificacao);

                if (tipoServico == "BILL SERVICE")
                {
                    BLL.Mesa.AlterarStatus(mesa.GUIDIdentificacao, 30);
                }

                processado = true;
            }

            return processado;
        }
        #endregion

        #region Pedido eMenu > PDV7
        public void ImportarPedido()
        {
            Boolean processado;
            String caminhoXMLPedido = Configuracao.RetornarValor("sincEMENU_CaminhoXMLSolicitacaoEmenu");
            String caminhoXMLProcessado = Configuracao.RetornarValor("sincEMENU_CaminhoXMLSolicitacaoEmenuProcessado");
            FileInfo file;

            String[] arrArquivos = Directory.GetFiles(caminhoXMLPedido);

            foreach (var item in arrArquivos)
            {
                processado = false;
                file = new FileInfo(item);

                try
                {
                    processado = ProcessarPedido(item);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (processado == true && caminhoXMLProcessado != null && caminhoXMLProcessado != "")
                        File.Move(item, caminhoXMLProcessado + file.Name);
                }
            }
        }
        private Boolean ProcessarPedido(String arquivoXml)
        {
            Boolean processado = false;
            //PedidoInformation pedido;
            List<PedidoProdutoInformation> listaPedidoProduto = new List<PedidoProdutoInformation>();
            PedidoProdutoInformation pedidoProduto;
            PedidoProdutoInformation pedidoProdutoModificacao;
            ProdutoInformation produto;
            ProdutoInformation modificacao;
            MesaInformation mesa;

            XmlDocument oXML = new XmlDocument();
            XmlNode kot = default(XmlNode);

            Int32 idUsuario = Convert.ToInt32(Configuracao.RetornarValor("sincEMENU_IDUsuario"));
            Int32 idPDV = Convert.ToInt32(Configuracao.RetornarValor("sincEMENU_IDPDV"));
            String codigoProduto;
            String notas;
            Boolean ordemImpressao;

            //carrega o arquivo XML
            oXML.Load(arquivoXml);

            kot = oXML.SelectSingleNode("TREEROOT").SelectSingleNode("KOT");
            if (kot != null)
            {
                mesa = Mesa.CarregarPorNumero(Convert.ToInt32(kot.SelectSingleNode("TABLE_NO").InnerText));
                
                foreach (XmlNode node_kot_transaction in kot.SelectNodes("KOT_TRANSACTION"))
                {
                    codigoProduto = node_kot_transaction.SelectSingleNode("ITEM").SelectSingleNode("ITEM_ID").InnerText;
                    produto = BLL.Produto.CarregarPorCodigo(codigoProduto);

                    if (produto.IDProduto == null)
                    {
                        MessageBox.Show("Produto \"" + codigoProduto + "\" não cadastrado no PDV7!\nPedido mesa " + mesa.Numero, "Sinc E-Menu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (node_kot_transaction.SelectSingleNode("ITEM").SelectSingleNode("ITEM_NOTE") != null)
                            notas = node_kot_transaction.SelectSingleNode("ITEM").SelectSingleNode("ITEM_NOTE").InnerText;
                        else
                            notas = "";

                        pedidoProduto = new PedidoProdutoInformation();
                        pedidoProduto.Usuario = new UsuarioInformation { IDUsuario = idUsuario };
                        pedidoProduto.PDV = new PDVInformation { IDPDV = idPDV };

                        pedidoProduto.Produto = produto;
                        pedidoProduto.Quantidade = Convert.ToInt32(node_kot_transaction.SelectSingleNode("QTY").InnerText); ;
                        pedidoProduto.ValorUnitario = pedidoProduto.Produto.ValorUnitario;
                        pedidoProduto.Notas = notas;
                        pedidoProduto.Cancelado = false;

                        pedidoProduto.ListaModificacao = new List<PedidoProdutoInformation>();
                        foreach (XmlNode node_modifier_id in node_kot_transaction.SelectSingleNode("ITEM").SelectNodes("MODIFIER_ID"))
                        {
                            modificacao = BLL.Produto.CarregarPorCodigo(node_modifier_id.InnerText);

                            if (modificacao.IDProduto != null)
                            {
                                pedidoProdutoModificacao = new PedidoProdutoInformation();
                                pedidoProdutoModificacao.Produto = modificacao;
                                pedidoProdutoModificacao.PDV = new PDVInformation { IDPDV = idPDV };
                                pedidoProdutoModificacao.Usuario = new UsuarioInformation { IDUsuario = idUsuario };
                                pedidoProdutoModificacao.Quantidade = 1;
                                pedidoProdutoModificacao.ValorUnitario = modificacao.ValorUnitario;
                                pedidoProdutoModificacao.CodigoAliquota = modificacao.CodigoAliquota;
                                pedidoProdutoModificacao.Cancelado = false;

                                pedidoProduto.ListaModificacao.Add(pedidoProdutoModificacao);
                            }
                        }

                        listaPedidoProduto.Add(pedidoProduto);
                    }
                }

                BLL.Pedido.AdicionarProduto(10, mesa.GUIDIdentificacao, idUsuario, idPDV, listaPedidoProduto);

                ordemImpressao = Convert.ToBoolean(ConfigurationManager.AppSettings["OrdemImpressao"]);
                if (ordemImpressao == true && listaPedidoProduto.Count > 0)
                    OrdemImpressao.GerarOrdemProducao(listaPedidoProduto);

                processado = true;
            }

            return processado;
        }
        #endregion

        #region Finalizar pedido PDV7 > eMenu
        public void FinalizarPedidoEMENU()
        {
            List<PedidoInformation> listaPedido = BLL.Pedido.ListarNaoSincEmenu();
            String xml;

            String numero = "";
            MesaInformation mesa;

            foreach (var item in listaPedido)
            {
                if (item.TipoPedido.IDTipoPedido == 10)
                {
                    mesa = Mesa.Carregar(item.GUIDIdentificacao);
                    numero = mesa.Numero.ToString();
                }

                xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";
                xml += "<TREEROOT>\r\n";
                xml += "  <VERSION>\r\n";
                xml += "  </VERSION>\r\n";
                xml += "  <SERVICE>\r\n";
                xml += "    <ROOM_NO>pdv7</ROOM_NO>\r\n";
                xml += "    <TABLE_NO>" + numero + "</TABLE_NO>\r\n";
                xml += "    <SERVICE_TYPE>BILL SERVICE</SERVICE_TYPE>\r\n";
                xml += "    <SERVICE_TIME>" + item.DtPedidoFechamento.Value.ToString("MM-dd-yyyy HH:mm:ss") + "</SERVICE_TIME>\r\n";
                xml += "    <WAITER_ID>1</WAITER_ID>\r\n";
                xml += "    <ORDER_ID>0</ORDER_ID>\r\n";
                xml += "    <TIP>0</TIP>\r\n";
                xml += "  </SERVICE>\r\n";
                xml += "</TREEROOT>\r\n";

                String caminhoXMLItens = Configuracao.RetornarValor("sincEMENU_CaminhoXMLFinalizarPedido") + "\\fecharConta_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".xml";
                StreamWriter arquivoXML = new StreamWriter(caminhoXMLItens);
                arquivoXML.Write(xml);
                arquivoXML.Close();

                item.SincEmenu = true;
                BLL.Pedido.AlterarStatusSincEmenu(item.IDPedido.Value, true);
            }
        }
        #endregion
    }
}
