using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Integracao.Servico.Core.PagSevenServer;
using a7D.PDV.Integracao.Servico.Core.PagSevenServer.Models;
using a7D.PDV.Integracao.Servico.Core.PdvSeven;
using a7D.PDV.Integracao.Servico.Core.PDVSeven;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.PagSevenServer
{
    public class IntegraPagSeven : IntegracaoTask
    {
        private PagSevenCore pagSevenCore;
        private PagSevenServerAPI pagSevenApi;
        private int intervaloSyncPag7;

        public override void Executar()
        {
          
            var config = ServicoCore.Configuracoes();
            Disponivel = config.HabilitaPagSeven == "1";
            if (!Disponivel)
            {
                EventoMensagem?.Invoke("PagSeven Inativo");
                return;
            }
            pagSevenCore = new PagSevenCore();
            pagSevenApi = new PagSevenServerAPI();
            var intervaloSetting = config.IntervaloSyncConsultasPagSeven;
            intervaloSyncPag7 = Convert.ToInt32(intervaloSetting);



            Iniciar(new Action(() =>
            {
                Sincronizar();
            }));

        }

        public void Sincronizar()
        {
            EventoMensagem?.Invoke("PagSeven Ativo");
            while (Executando)
            {
                Thread.Sleep(intervaloSyncPag7 * 1000);
                SincronizarPagamentosPendentes();
                SincronizarPagamentosAutorizados();
                SincronizarConsultasPagSeven();
                SincronizarPedidosAbertos();
                SincronizarPedidosFechados();
            }

        }

        private void SincronizarPagamentosPendentes()
        {
            try
            {
                EventoMensagem?.Invoke("Obtendo pagamentos pendentes...");
                var listaPagamentos = pagSevenCore.ObterPagamentosPendentes(Convert.ToInt32(BLL.ConfiguracoesSistema.Valores.IDEstabelecimentoPDV7), DateTime.Now.AddSeconds(-intervaloSyncPag7)).Result;

                if (listaPagamentos.Count() == 0)
                {

                }
                else
                {
                    var Pdv7Api = new PDVSevenAPI();

                    foreach (var pagamento in listaPagamentos)
                    {
                        EventoMensagem?.Invoke($"Pagamento pendente {pagamento.IDPagamento}");
                        var response = Pdv7Api.GetPedido(pagamento.IDPedido.Value).Result;
                        if (!response.IsSuccessStatusCode)
                        {

                            pagamento.IDStatusIntegracao = 30;
                            pagamento.RespostaIntegracao = JsonConvert.SerializeObject(new RespostaIntegracaoErro { Message = "Erro ao obter pedido. " + response.ReasonPhrase });

                        }
                        else
                        {
                            var pedidoRaw = response.Content.ReadAsStringAsync().Result;
                            var pedido = JsonConvert.DeserializeObject<API2.Model.Pedido>(pedidoRaw);
                            if (pedido.ValorTotal == pagamento.Valor)
                            {
                                pagamento.IDStatusIntegracao = 10;
                            }
                            else if (pedido.ValorTotal >= pagamento.Valor)
                            {
                                pagamento.IDStatusIntegracao = 30;
                                pagamento.RespostaIntegracao = JsonConvert.SerializeObject(new RespostaIntegracaoErro { Message = "Valor do pagamento menor que o esperado" });
                            }
                            else if (pedido.ValorTotal <= pagamento.Valor)
                            {
                                pagamento.IDStatusIntegracao = 30;
                                pagamento.RespostaIntegracao = JsonConvert.SerializeObject(new RespostaIntegracaoErro { Message = "Valor do pagamento maior que o esperado" });
                            }
                        }


                        var responseALterarPagamento = pagSevenApi.AlterarStatusIntegracaoPagamento(pagamento).Result;
                        if (responseALterarPagamento.IsSuccessStatusCode)
                        {


                        }
                        else
                        {


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SincronizarPagamentosAutorizados()
        {
            try
            {
                EventoMensagem?.Invoke("Obtendo pagamentos autorizados...");
                var listaPagamentos = pagSevenCore.ObterPagamentosAutorizadosOperadora(Convert.ToInt32(BLL.ConfiguracoesSistema.Valores.IDEstabelecimentoPDV7), DateTime.Now.AddSeconds(-intervaloSyncPag7 - 10)).Result;

                if (listaPagamentos.Count() == 0)
                {

                }
                else
                {
                    var Pdv7Api = new PDVSevenAPI();

                    foreach (var pagamento in listaPagamentos)
                    {
                        EventoMensagem?.Invoke($"Pagamento autorizado {pagamento.IDPagamento}");
                        var objInserirPagamento = MontarPagamento(pagamento);
                        var response = Pdv7Api.AddPagamento(objInserirPagamento).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorMessage = response.Content.ReadAsStringAsync().Result;
                            pagamento.IDStatusIntegracao = 30;
                            pagamento.RespostaIntegracao = JsonConvert.SerializeObject(new RespostaIntegracaoErro { Message = "Erro ao adicionar pagamento. " + errorMessage });

                        }
                        else
                        {
                            var fechamento = new FechamentoPedido();
                            fechamento.ChaveAcesso = ConfigurationManager.AppSettings["ChaveAcessoPagSeven"].ToString();
                            fechamento.IDPdv = Convert.ToInt32(ConfigurationManager.AppSettings["IDPDVPagSeven"].ToString());
                            var responseFecharPedido = Pdv7Api.FecharPedido(pagamento.IDPedido.Value.ToString(), fechamento).Result;
                            if (!responseFecharPedido.IsSuccessStatusCode)
                            {
                                var errorMessage = response.Content.ReadAsStringAsync().Result;
                                pagamento.IDStatusIntegracao = 30;
                                pagamento.RespostaIntegracao = JsonConvert.SerializeObject(new RespostaIntegracaoErro { Message = "Erro ao fechar pedido. " + errorMessage });

                            }
                            else
                            {
                                EventoMensagem?.Invoke($"Pagamento {pagamento.IDPagamento} fechado no caixa");
                                pagamento.IDStatusIntegracao = 20;
                            }

                        }

                        var responseALterarPagamento = pagSevenApi.AlterarStatusIntegracaoPagamento(pagamento).Result;
                        if (responseALterarPagamento.IsSuccessStatusCode)
                        {
                        }
                        else
                        {
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private InsercaoPagamento MontarPagamento(Models.Pagamento pagamento)
        {


            var pagamentoApi2 = new API2.Model.Pagamento();
            pagamentoApi2.TipoPagamento = new API2.Model.TipoPagamento { IDTipoPagamento = 2 };
            pagamentoApi2.Valor = pagamento.Valor;

            var listaPagamentos = new List<API2.Model.Pagamento>();

            listaPagamentos.Add(pagamentoApi2);

            var pdv7Api = new PDVSevenAPI();
            var responsePedido = pdv7Api.GetPedido(pagamento.IDPedido.Value).Result;
            if (responsePedido.IsSuccessStatusCode)
            {
                var pedidoRaw = responsePedido.Content.ReadAsStringAsync().Result;
                var pedido = JsonConvert.DeserializeObject<API2.Model.Pedido>(pedidoRaw);

                var objInserirPagamento = new InsercaoPagamento(
                    pagamento.GUIDPagamento,
                    pedido.IDTipoPedido,
                    Convert.ToInt32(pedido.NumeroComanda),
                    Convert.ToInt32(ConfigurationManager.AppSettings["IDUsuarioPagSeven"].ToString()),
                    Convert.ToInt32(ConfigurationManager.AppSettings["IDPDVPagSeven"].ToString()),
                    listaPagamentos
                );

                return objInserirPagamento;
            }
            else
            {
                throw new Exception("Pedido nao encontrado");
            }


        }
        private void SincronizarConsultasPagSeven()
        {
            try
            {
                var dataMinima = DateTime.Now;

                //var configDataMinima = PagSevenCore.Configuracoes().dtUltimaConsultaPagSeven;
                //if (!string.IsNullOrEmpty(configDataMinima))
                //{
                //    dataMinima = DateTime.Parse(configDataMinima);

                //}

                EventoMensagem?.Invoke($"Obtendo consultas...");
                var listaConsultas = pagSevenCore.ObterConsultas(Convert.ToInt32(BLL.ConfiguracoesSistema.Valores.IDEstabelecimentoPDV7), dataMinima.AddSeconds(-intervaloSyncPag7)).Result;

                if (listaConsultas.Count() == 0)
                {

                }
                else
                {
                    var Pdv7Api = new PDVSevenAPI();
                    foreach (var consulta in listaConsultas)
                    {
                        EventoMensagem?.Invoke($"Consulta {consulta.IDPedido}");
                        var response = Pdv7Api.GetPedido(consulta.IDPedido.Value).Result;
                        if (!response.IsSuccessStatusCode)
                        {

                        }
                        else
                        {

                            var pedidoRaw = response.Content.ReadAsStringAsync().Result;
                            var pedido = JsonConvert.DeserializeObject<API2.Model.Pedido>(pedidoRaw);

                            var responseAlterarConsulta = pagSevenApi.AlterarRespostaConsulta(pedido, consulta.IDConsulta).Result;
                            if (responseAlterarConsulta.IsSuccessStatusCode)
                            {

                            }
                            else
                            {

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SincronizarPedidosAbertos()
        {

            var pedidos = PDVSevenCore.PedidosAbertos(intervaloSyncPag7, ETipoPedido.Comanda);
            EventoMensagem?.Invoke($"Pedidos abertos encontrados: { pedidos.Count }");

            if (pedidos.Count > 0)
            {
                foreach (var pedido in pedidos)
                {
                    dynamic modelPedidoPag7 = new ExpandoObject();
                    modelPedidoPag7.IDPedido = pedido.IDPedido;
                    modelPedidoPag7.IDEstabelecimento = Convert.ToInt32(BLL.ConfiguracoesSistema.Valores.IDEstabelecimentoPDV7);
                    modelPedidoPag7.TelefoneCliente =
                    pedido.Cliente.Telefone1DDD.ToString() + pedido.Cliente.Telefone1Numero.ToString();
                    modelPedidoPag7.IDComandaEstabelecimento = pedido.NumeroComanda;

                    PagSevenCore.EnviarPedido(modelPedidoPag7);
                }
            }


        }

        private void SincronizarPedidosFechados()
        {
            var dataInicio = DateTimeOffset.Now.AddDays(-1).ToUnixTimeSeconds();

            var dtUltimoPedidoFechado = new BLL.ConfiguracoesServicoIntegracoes().dtFechamentoUltimoPedidoFechadoPag7;
            if (!string.IsNullOrEmpty(dtUltimoPedidoFechado))
            {
                var data = DateTime.Parse(dtUltimoPedidoFechado).AddSeconds(1);
                dataInicio = new DateTimeOffset(data).ToUnixTimeSeconds();
            } 
            var dataFim = DateTimeOffset.Now.ToUnixTimeSeconds();

            var pedidos = new PDVSevenCore().ObterVendasAPI2Async(dataInicio, dataFim, 10000).Result;
            EventoMensagem?.Invoke($"Pedidos fechados encontrados: { pedidos.Count }");
            foreach (var pedido in pedidos.OrderBy(p => p.DtPedidoFechamento))
            {

                var enviado = PagSevenCore.FecharPedido(Convert.ToInt32(BLL.ConfiguracoesSistema.Valores.IDEstabelecimentoPDV7), pedido.IDPedido.Value);
                if (enviado)
                {
                    GravarEstado(pedido);
                }
            }
        }

        private void GravarEstado(API2.Model.Pedido pedido)
        {
            try
            {
                if (pedido != null)
                {
                    var configuracoes = ConfiguracaoBD.ListarConfiguracoes();
                    var configDtUltimoPedidoFechado = configuracoes.Where(c => c.Chave == nameof(ConfiguracoesServicoIntegracoes.dtFechamentoUltimoPedidoFechadoPag7)).FirstOrDefault();
                    configDtUltimoPedidoFechado.Valor = pedido.DtPedidoFechamento.Value.ToString("dd/MM/yyyy HH:mm:ss");
                    ConfiguracaoBD.Salvar(configDtUltimoPedidoFechado);
                }
            }
            catch (Exception ex)
            {
                var myEx = new Exception($"Erro na gravação de estado do pedido {pedido.IDPedido}", ex);
                myEx.Data.Add("pedido", JsonConvert.SerializeObject(pedido));
                throw myEx;
            }
        }

    }
}
