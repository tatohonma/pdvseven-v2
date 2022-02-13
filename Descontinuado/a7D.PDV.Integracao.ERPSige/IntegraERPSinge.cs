using a7D.PDV.BLL;
using System;
using System.Threading;

namespace a7D.PDV.Integracao.ERPSige
{
    public class IntegraERPSinge : a7D.PDV.Integracao.Servico.Core.IntegracaoTask
    {
        private DateTime dtPedidoInicio;
        private DateTime dtUltimoSync;

        public override void Executar()
        {
            Disponivel = BLL.PDV.PossuiERP();
            if (!Disponivel)
            {
                EventoMensagem?.Invoke("Sem Licenças para ERP");
                return;
            }

            var config = new ConfiguracoesERP();

            if (DateTime.TryParse(config.PedidoDataInicio, out DateTime dtStart))
                dtPedidoInicio = dtStart;
            else
            {
                EventoMensagem?.Invoke("Defina uma data inicial valida para sincronização");
                return;
            }

            if (DateTime.TryParse(config._ERPUltimoSincronismo, out DateTime dtUltimo))
                dtUltimoSync = dtUltimo;
            else
                dtUltimoSync = DateTime.MinValue;

            DTO.PedidoOrigemVenda = config.PedidoOrigemVenda;
            DTO.PedidoSemCliente = config.PedidoSemCliente;
            DTO.PedidoEmpresa = config.PedidoEmpresa;
            DTO.PedidoPlanoDeConta = config.PedidoPlanoDeConta;
            DTO.PedidoDeposito = config.PedidoDeposito;
            DTO.PedidoBanco = config.PedidoBanco;
            DTO.PedidoFormaPagamento = config.PedidoFormaPagamento;

            EventoMensagem?.Invoke("Iniciando Sincronismo com ERP");
            Iniciar(new Action(() => { Loop(); Executando = false; }));
        }

        private void Loop()
        {
            while (Executando)
            {
                Thread.Sleep(5000);

                var api = new APIERP();
                int n = 0;

                var dt = DateTime.Now; // Quando começou....

                // Subir os produtos
                //var produtos = Produto.Listar();
                //foreach (var produtoPDV in produtos)
                //{
                //    if (produtoPDV.DtUltimaAlteracao <dtUltimoSync)
                //        continue;

                //    // Le o produto como está, e altera as informações basicas
                //    var produtoERP = api.ProdutoCarregar(produtoPDV.IDProduto.Value);
                //    DTO.ProdutoFill(produtoPDV, ref produtoERP);
                //    var result = api.ProdutoSalvar(produtoERP);

                //    n++;
                //    string info = "";
                //    if (produtos.Count > 10) // mostra percentual quando a lista tem mais de 10 itens
                //        info = $"{(100 * n / produtos.Count).ToString("N1")}% ";

                //    EventoMensagem?.Invoke($"{info}{produtoPDV.IDProduto}-{produtoPDV.Nome} => {result}");
                //}

                //// Subir os clientes
                //var clientes = Cliente.Listar();
                //n = 0;
                //foreach (var clientePDV in clientes)
                //{
                //    if (clientePDV.DtInclusao < dtUltimoSync)
                //        continue;

                //    var clienteERP = DTO.Pessoa(clientePDV);
                //    var result = api.PessoaSalvar(clienteERP);

                //    n++;
                //    string info = "";
                //    if (clientes.Count > 10) // mostra percentual quando a lista tem mais de 10 itens
                //        info = $"{(100 * n / clientes.Count).ToString("N1")}% ";

                //    EventoMensagem?.Invoke($"{info}{clientePDV.NomeERP} => {result}");
                //}

                //ConfiguracaoBD.DefinirValorPadrao(EF.Enum.EConfig._ERPUltimoSincronismo, dt.ToString("dd/MM/yyyy HH:mm:ss"));

                // Subir as vendas
                var pedidos = Pedido.ListarNaoSincERP(dtPedidoInicio);
                n = 0;

                EventoMensagem?.Invoke("Enviando pedidos");
                foreach (var pedidoPDV in pedidos)
                {
                    try
                    {
                        pedidoPDV.ListaProduto = PedidoProduto.ListarPorPedido(pedidoPDV.IDPedido.Value);

                        if (pedidoPDV.ListaProduto.Count == 0)
                        {
                            pedidoPDV.SincERP = true;
                            Pedido.Salvar(pedidoPDV);
                            continue;
                        }

                        pedidoPDV.ListaPagamento = PedidoPagamento.ListarNaoCanceladoPorPedido(pedidoPDV.IDPedido.Value);

                        pedidoPDV.Caixa = Caixa.Carregar(pedidoPDV.Caixa.IDCaixa.Value);

                        if (pedidoPDV.Cliente != null)
                            pedidoPDV.Cliente = Cliente.Carregar(pedidoPDV.Cliente.IDCliente.Value);

                        var pedidoERP = DTO.Pedido(pedidoPDV);
                        var result = api.PedidoSalvarEFaturar(pedidoERP);
                        //var result = api.PedidoSalvar(pedidoERP);

                        n++;
                        string info = "";
                        if (pedidos.Count > 10) // mostra percentual quando a lista tem mais de 10 itens
                            info = $"{(100 * n / pedidos.Count).ToString("N1")}% ";

                        EventoMensagem?.Invoke($"{info}{pedidoERP.Codigo} {pedidoERP.OrigemVenda} {pedidoERP.ValorFinal.ToString("C")} => {result}");
                        if (result.Contains("COM SUCESSO"))
                        {
                            pedidoPDV.SincERP = true;
                            Pedido.Salvar(pedidoPDV);
                            LOG.Info(CodigoInfo.I010, $"{api.LastRequest}\r\n\r\n{api.LastResult}", "Pedido " + pedidoPDV.IDPedido);
                        }
                        else
                            throw new ExceptionPDV(CodigoErro.E5B0);
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("IDPedido", pedidoPDV.IDPedido);
                        ex.Data.Add("LastRequest", api.LastRequest);
                        ex.Data.Add("LastResult", api.LastResult);
                        LOG.Erro(ex);
                    }
                }
            }
        }
    }
}