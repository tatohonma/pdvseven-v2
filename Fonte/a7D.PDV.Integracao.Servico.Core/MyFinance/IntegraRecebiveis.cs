using a7D.PDV.BLL;
using a7D.PDV.BLL.Utils;
using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Integracao.Servico.Core.MyFinance.Models;
using a7D.PDV.Integracao.Servico.Core.PDVSeven;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace a7D.PDV.Integracao.Servico.Core.MyFinance
{
    public class IntegraRecebiveis : IntegracaoTask
    {
        private MyFinanceCore core;
        private MyFinanceAPI myFinanceApi;
        private int intervaloSyncMF;
        private PDVSevenCore _pDVSevenCore;
        private int countListaRecebiveis;

        ConfiguracaoBDInformation configEnviadoMF;
        ConfiguracaoBDInformation configIDPedidoMF;
        ConfiguracaoBDInformation configIDPagamentoMF;
        ConfiguracaoBDInformation configDtPedidoMF;

        public override void Executar()
        {
            var config = ServicoCore.Configuracoes();
            Disponivel = config.HabilitaMyFinance == "1";
            if (!Disponivel)
            {
                AddLog("MyFinance Inativo");
                return;
            }

            MyFinanceAPI.authToken = config.authTokenMyFinance;
            if (string.IsNullOrEmpty(MyFinanceAPI.authToken))
            {
                EventoMensagem?.Invoke("MyFinance sem Token de autenticação");
                return;
            }

            MyFinanceAPI.baseUrl = new Uri(new Uri(ConfigurationManager.AppSettings["APIMyFinance"].ToString()), "api");
            MyFinanceAPI.password = ConfigurationManager.AppSettings["PasswordMyFinance"].ToString();
            //idEntidade = ConfigurationManager.AppSettings["IDEntityMyFinance"].ToString();

            Iniciar(new Action(() =>
            {
                EventoMensagem?.Invoke("MyFinance Ativo");

                _pDVSevenCore = new PDVSevenCore();
                myFinanceApi = new MyFinanceAPI();
                core = new MyFinanceCore();

                var configuracoes = ConfiguracaoBD.ListarConfiguracoes();
                configEnviadoMF = configuracoes.Where(c => c.Chave == nameof(config.EnviadoMyFinance)).FirstOrDefault();
                configIDPedidoMF = configuracoes.Where(c => c.Chave == nameof(config.ultimoIDPedidoMyFinance)).FirstOrDefault();
                configIDPagamentoMF = configuracoes.Where(c => c.Chave == nameof(config.ultimoIDPagamentoMyFinance)).FirstOrDefault();
                configDtPedidoMF = configuracoes.Where(c => c.Chave == nameof(config.dtUltimaFechamentoPedidoMyFinance)).FirstOrDefault();

                var intervaloSetting = config.intervaloSyncMyFinance;
                intervaloSyncMF = Convert.ToInt32(intervaloSetting);

                SincPedidos();
            }));
        }

        private void SincPedidos()
        {
            while (Executando)
            {
                try
                {
                    if (MyFinanceAPI.idEntidade == 0)
                    {
                        EventoMensagem?.Invoke(FormatarMensagem("Obtendo Entidade..."));
                        var entidade = MyFinanceCore.ObterEntidadeID();
                        MyFinanceAPI.idEntidade = entidade.EntidadeID;
                        if (MyFinanceAPI.idEntidade == 0)
                        {
                            EventoMensagem?.Invoke(FormatarMensagem("Não foi possivel ober a identidade"));
                            return;
                        }
                        else
                            EventoMensagem?.Invoke(FormatarMensagem("Entidade: " + entidade.Nome));
                    }

                    EventoMensagem?.Invoke(FormatarMensagem("Sincronizando contas de recebíveis..."));
                    SincronizarContasRecebivel(EventoMensagem);
                    EventoProgress?.Invoke("", 0, 0, this);

                    SincronizarMF();
                    Thread.Sleep(intervaloSyncMF * 1000);
                }
                catch (Exception ex)
                {
                    EventoMensagem?.Invoke(FormatarMensagem(ExceptionHelper.InnerExceptionMessageLoop(ex)));
                    EventoProgress?.Invoke("", 0, 0, this);
                    Executando = false;
                }
            }
        }


        private void SincronizarMF()
        {



            var dtInicio = DateTimeOffset.Now.AddSeconds(-intervaloSyncMF).ToUnixTimeSeconds();

            var dtFim = DateTimeOffset.Now.ToUnixTimeSeconds();

            var pedidoConfig = CarregarEstado();
            var pedidoConfigPedido = pedidoConfig?.Pedido;
            if (pedidoConfigPedido != null && pedidoConfigPedido.DtPedidoFechamento != null && pedidoConfigPedido.DtPedidoFechamento != new DateTime(1980, 1, 1))
            {
                dtInicio = new DateTimeOffset(pedidoConfigPedido.DtPedidoFechamento.Value).ToUnixTimeSeconds();
                if (pedidoConfig.Enviado)
                    dtInicio = new DateTimeOffset(pedidoConfigPedido.DtPedidoFechamento.Value.AddSeconds(1)).ToUnixTimeSeconds();
            }
            else
            {
                dtInicio = new DateTimeOffset(pedidoConfigPedido.DtPedidoFechamento.Value).ToUnixTimeSeconds();
            }

            //retirar essa linha
            dtInicio = DateTimeOffset.Now.AddHours(-10).ToUnixTimeSeconds();

            EventoMensagem?.Invoke(FormatarMensagem("Obtendo pagamentos no PDV7..."));
            var listaPedidos = _pDVSevenCore.ObterVendasAPI2Async(dtInicio, dtFim, 10000).Result;

            if (listaPedidos.Count() == 0)
            {
                EventoMensagem?.Invoke(FormatarMensagem("Todos os pagamentos já foram enviados."));
            }
            else
            {
                EventoMensagem?.Invoke(FormatarMensagem("Enviando pagamentos ao MyFinance..."));

                var listaRecebiveis = core.MontarRecebiveis(listaPedidos.OrderBy(p => p.DtPedidoFechamento).ToList());
                countListaRecebiveis = listaRecebiveis.Count();

                EventoProgress?.Invoke("", countListaRecebiveis, 0, this);

                if (pedidoConfig != null && pedidoConfig.Enviado == false && pedidoConfig.Pedido != null)
                {
                    listaRecebiveis = RecebiveisAPartirDoRegistro(listaRecebiveis, new Recebivel { Pedido = pedidoConfig.Pedido });
                }
                EnviarRecebiveis(listaRecebiveis);
                EventoMensagem?.Invoke(FormatarMensagem("Pagamentos enviados com sucesso."));
            }

        }
        public void EnviarRecebiveis(List<Recebivel> listaRecebiveis)
        {
            //bool erroEnvio = false;
            //Recebivel ultimoRecebivel = null;
            foreach (var recebivel in listaRecebiveis)
            {

                GravarEstado(recebivel.Pedido, false);

                var resposta = myFinanceApi.PostRecebivel(recebivel).Result;

                if (!resposta.IsSuccessStatusCode)
                {
                    //throw new Exception("falha na conexao");

                    if ((int)resposta.StatusCode == 429)
                    {//too many requests                       
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        var respostaBody = resposta.Content.ReadAsStringAsync().Result;

                        throw new Exception($@"Erro ao enviar pagamento ao MyFinance. 
Codigo do erro: {(int)resposta.StatusCode} - {resposta.ReasonPhrase} - {respostaBody}
Pagamento {recebivel.Pedido.Pagamentos.FirstOrDefault().ID} 
Pedido {recebivel.Pedido.IDPedido}.");


                    }
                    //ultimoRecebivel = recebivel;
                    //erroEnvio = true;

                }
                else
                {
                    //EventoMensagem?.Invoke(FormatarMensagem($"Pagamento {recebivel.Pedido.Pagamentos[0].ID} enviado com sucesso."));
                    //EventoProgress?.Invoke("", countListaRecebiveis, ++countRecebiveisEnviados);
                    GravarEstado(recebivel.Pedido, true);
                }

            }
            //if (erroEnvio)
            //{
            //    listaRecebiveis = RecebiveisAPartirDoRegistro(listaRecebiveis, ultimoRecebivel);
            //    EnviarRecebiveis(listaRecebiveis);
            //}
        }

        private List<Recebivel> RecebiveisAPartirDoRegistro(List<Recebivel> lista, Recebivel ultimoRecebivel)
        {
            return lista.Where(r => r.Pedido.Pagamentos.FirstOrDefault().ID >= ultimoRecebivel.Pedido.Pagamentos.FirstOrDefault().ID).ToList();
        }

        private string FormatarMensagem(string s)
        {
            return $"{DateTime.Now.ToString("dd/MM/yy HH:mm:ss")} {s}";
        }

        public PedidoConfig CarregarEstado()
        {
            try
            {
                var config = ServicoCore.Configuracoes();
                var dtFechamento = config.dtUltimaFechamentoPedidoMyFinance;
                var data = new DateTime();
                DateTime.TryParse(dtFechamento, out data);
                if (data == new DateTime(0001, 1, 1))
                {
                    data = new DateTime(1980, 1, 1);
                }

                var idPedido = config.ultimoIDPedidoMyFinance;
                var idPagamento = config.ultimoIDPagamentoMyFinance;
                var enviado = config.EnviadoMyFinance;

                //if (string.IsNullOrEmpty(dtFechamento))
                //    return null;

                return new PedidoConfig
                {
                    Enviado = Convert.ToBoolean(Convert.ToInt16(enviado)),
                    Pedido = new API2.Model.Pedido
                    {

                        DtPedidoFechamento = data,
                        IDPedido = Convert.ToInt32(idPedido),
                        Pagamentos = new List<API2.Model.Pagamento> { { new API2.Model.Pagamento { ID = Convert.ToInt32(idPagamento) } } }
                    }
                };
            }
            catch (Exception ex)
            {

                var myEx = new Exception("Erro ao carregar estado do pedido na Integração de Recebiveis", ex);
                throw myEx;
            }
        }
        public void GravarEstado(API2.Model.Pedido pedido, bool enviado)
        {

            try
            {
                if (pedido != null)
                {
                    configEnviadoMF.Valor = (enviado ? 1 : 0).ToString();
                    configIDPedidoMF.Valor = pedido.IDPedido.ToString();
                    configIDPagamentoMF.Valor = pedido.Pagamentos[0].ID.Value.ToString();
                    configDtPedidoMF.Valor = pedido.DtPedidoFechamento.Value.ToString();
                    ConfiguracaoBD.Salvar(configEnviadoMF);
                    ConfiguracaoBD.Salvar(configIDPedidoMF);
                    ConfiguracaoBD.Salvar(configIDPagamentoMF);
                    ConfiguracaoBD.Salvar(configDtPedidoMF);
                }
            }
            catch (Exception ex)
            {
                var myEx = new Exception("Erro na gravação de estado do pedido da Integração de Recebiveis", ex);
                myEx.Data.Add("pedido", JsonConvert.SerializeObject(pedido));
                throw myEx;
            }
        }

        public void SincronizarContasRecebivel(OnMensagemListener EventoMensagem)
        {
            var tiposPagamentoPDV = PDVSevenCore.ListarTiposPagamento();
            var listaContasRecebivelOnline = MyFinanceCore.ListarContasRecebiveis();
            var listaContasAPI2 = PDVSevenCore.ListarContasRecebivel();

            foreach (var contaAPI2 in listaContasAPI2)
            {
                contaAPI2.Nome += "-PDV7";

                var contaMF = listaContasRecebivelOnline.FirstOrDefault(c => c.Name == contaAPI2.Nome && c.EntityID == MyFinanceAPI.idEntidade.ToString());
                if (string.IsNullOrEmpty(contaAPI2.CodigoIntegracao))
                {
                    if (contaMF == null)
                    {
                        EventoMensagem?.Invoke("Adicionando Conta de Recebivel: " + contaAPI2.Nome);
                        AdicionaContaDeRecebivel(tiposPagamentoPDV, contaAPI2);
                    }
                    else
                    {
                        EventoMensagem?.Invoke("Alterando Conta de Recebivel: " + contaAPI2.Nome);
                        contaAPI2.CodigoIntegracao = contaMF.ID;
                        PDVSevenCore.AlterarCodigoIntegracaoContaRecebivel(contaAPI2);
                        //throw new Exception("Já existe uma conta com esse nome");
                    }
                }
                else
                {
                    if (AlteraContaDeRecebivel(tiposPagamentoPDV, contaAPI2))
                        EventoMensagem?.Invoke("Definindo Conta de Recebivel: " + contaAPI2.Nome);
                }
            }
        }

        private static bool AlteraContaDeRecebivel(List<API2.Model.TipoPagamento> tiposPagamentoPDV, API2.Model.ContaRecebivel contaAPI2)
        {
            bool alterar = false;
            var contaRecebivelMyFinance = MyFinanceCore.ConsultarContaRecebivel(contaAPI2.CodigoIntegracao);
            if (contaRecebivelMyFinance == null)
            {
                contaAPI2.CodigoIntegracao = "";
                PDVSevenCore.AlterarCodigoIntegracaoContaRecebivel(contaAPI2);
                throw new Exception("Conta de Recebivel com cpodigo de integração inválido");
            }
            var tiposPagamentoPorContaRecebivelPDV = tiposPagamentoPDV.Where(t => t.ContaRecebivel != null && t.ContaRecebivel.IDContaRecebivel == contaAPI2.IDContaRecebivel);
            var contaRecebivelAux = new Models.ContaRecebivel();

            if (contaRecebivelMyFinance.ObterListaPaymentMethods().Count() < tiposPagamentoPorContaRecebivelPDV.Count())
            {
                foreach (var tipoPagamentoPDV in tiposPagamentoPorContaRecebivelPDV)
                {
                    foreach (PaymentMethodsMyFinance paymentMethod in contaRecebivelMyFinance.ObterListaPaymentMethods())
                    {
                        var metodo = MyFinanceCore.TraduzirMeioMetodoPagamento(tipoPagamentoPDV.MeioPagamento.IDMeioPagamento);
                        if (!metodo.Equals(paymentMethod))
                        {
                            contaRecebivelAux.AdicionarPaymentMethods(metodo);
                            alterar = true;
                        }
                    }
                }
            }

            foreach (var item in contaRecebivelAux.ObterListaPaymentMethods())
            {
                contaRecebivelMyFinance.AdicionarPaymentMethods(item);
            }

            if (alterar)
            {
                MyFinanceCore.AlterarContaRecebivel(contaRecebivelMyFinance);
            }

            return alterar;
        }

        private static void AdicionaContaDeRecebivel(List<API2.Model.TipoPagamento> tiposPagamentoPDV, API2.Model.ContaRecebivel contaAPI2)
        {
            var contaRevebivemMyFinance = new Models.ContaRecebivel
            {
                Name = contaAPI2.Nome,
                EntityID = MyFinanceAPI.idEntidade.ToString()
            };

            contaRevebivemMyFinance.SetProvider(contaAPI2);

            CompararMetodosDePagamento(tiposPagamentoPDV, contaAPI2, contaRevebivemMyFinance);

            var contaRecebivelMFRetornada = MyFinanceCore.EnviarContaDeRecebivel(contaRevebivemMyFinance);

            contaAPI2.CodigoIntegracao = contaRecebivelMFRetornada.ID;
            PDVSevenCore.AlterarCodigoIntegracaoContaRecebivel(contaAPI2);
        }

        private static void CompararMetodosDePagamento(List<API2.Model.TipoPagamento> tiposPagamento, API2.Model.ContaRecebivel contaAPI2, Models.ContaRecebivel contaRevebivemMyFinance)
        {
            var tiposPagamentoPorContaRecebivel = tiposPagamento.Where(t => t.ContaRecebivel != null && t.ContaRecebivel.IDContaRecebivel == contaAPI2.IDContaRecebivel);

            if (tiposPagamentoPorContaRecebivel.Count() == 0)
            {
                if (contaAPI2.IDContaRecebivel == (int)EContaRecebivel.Dinheiro)
                {
                    contaRevebivemMyFinance.AdicionarPaymentMethods(PaymentMethodsMyFinance.cash);
                }
                else
                {
                    contaRevebivemMyFinance.AdicionarPaymentMethods(PaymentMethodsMyFinance.credit);
                    contaRevebivemMyFinance.AdicionarPaymentMethods(PaymentMethodsMyFinance.debit);
                }
            }
            else
            {
                foreach (var _tipo in tiposPagamentoPorContaRecebivel)
                {
                    var metodoPagamento = MyFinanceCore.TraduzirMeioMetodoPagamento(_tipo.MeioPagamento.IDMeioPagamento);
                    contaRevebivemMyFinance.AdicionarPaymentMethods(metodoPagamento);
                }
            }
        }
    }
}
