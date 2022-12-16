using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.Servico.Core;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace a7D.PDV.Integracao.iFood
{
    enum eStatusLoja
    {
        none, aberta, fechada
    }

    public partial class IntegraIFood : IntegracaoTask
    {
        public override string Nome => "Delivery iFood";

        ConfiguracoesIFood ConfigIFood;
        CaixaInformation CaixaIFood;
        PDVInformation PDVIFood;
        UsuarioInformation UsuarioIfood;
        TipoPagamentoInformation PagamentoIFood;
        TipoPagamentoInformation PagamentoDinheiro;
        TipoPagamentoInformation PagamentoCredito;
        TipoPagamentoInformation PagamentoDebito;
        TipoPagamentoInformation PagamentoRefeicao;
        TipoPagamentoInformation PagamentoOutros;
        TaxaEntregaInformation TaxaEntregaIFood;
        TipoDescontoInformation TipoDescontoIFood;
        ProdutoInformation TaxaAdicional;

        API.Order APIOrder;
        API.Merchant APIMerchant;
        String AccessToken;
        DateTime ExpiraEm;
        List<String> EventosRecebidos;

        public override void Executar()
        {
            if (!ValidarLicenca())
                return;

            if (!ValidarConfiguracoes())
                return;

            Iniciar(() => Loop());
        }

        public Boolean ValidarLicenca()
        {
            Disponivel = BLL.PDV.PossuiIFOOD();
            if (!Disponivel)
            {
                AddLog("Sem Licenças para iFood");
                return false;
            }

            AddLog("Licença para iFood OK!");
            return true;
        }

        public Boolean ValidarConfiguracoes()
        {
            Boolean configurado = true;

            ConfigIFood = new ConfiguracoesIFood();

            if (!ConfigIFood.IntegracaoIFood)
            {
                AddLog("Integração iFood desligada");
                return false;
            }

            if (ConfigIFood.CaixaPDV == 0
             || string.IsNullOrEmpty(ConfigIFood.ChaveUsuario)
             || string.IsNullOrEmpty(ConfigIFood.ClientId)
             || string.IsNullOrEmpty(ConfigIFood.ClientSecret)
             || string.IsNullOrEmpty(ConfigIFood.AuthorizationCodeVerifier)
             || string.IsNullOrEmpty(ConfigIFood.AuthorizationCode))
            {
                AddLog("Falta configurar o iFood no Configurador");
                return false;
            }

            var pdvs = BLL.PDV.Listar();
            PDVIFood = pdvs.FirstOrDefault(p => p.IDPDV == ConfigIFood.CaixaPDV && p.TipoPDV.Tipo == ETipoPDV.CAIXA);
            if (PDVIFood == null)
            {
                AddLog($"Caixa ID PDV: {ConfigIFood.CaixaPDV} inválido!");
                configurado = false;
            }

            var listaPagamentos = TipoPagamento.Listar().OrderByDescending(p => p.Ativo);

            PagamentoIFood = listaPagamentos.FirstOrDefault(p => p.IDGateway == (int)EGateway.iFood);
            if (PagamentoIFood == null)
            {
                PagamentoIFood = new TipoPagamentoInformation();
                PagamentoIFood.MeioPagamentoSAT = new MeioPagamentoSATInformation { IDMeioPagamentoSAT = 10 };

                PagamentoIFood.Nome = "iFood";
                PagamentoIFood.CodigoImpressoraFiscal = "iFood";
                PagamentoIFood.Ativo = true;
                PagamentoIFood.RegistrarValores = false;
                PagamentoIFood.IDGateway = (int)EGateway.iFood;

                CRUD.Adicionar(PagamentoIFood);
                AddLog("Pagamento com Gateway 'iFood' cadastrado!");
            }

            PagamentoRefeicao = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Refeicao));
            if (PagamentoRefeicao == null)
            {
                PagamentoRefeicao = new TipoPagamentoInformation();
                PagamentoRefeicao.Nome = "Vale Refeição";
                PagamentoRefeicao.CodigoImpressoraFiscal = "0";
                PagamentoRefeicao.RegistrarValores = true;
                PagamentoRefeicao.Ativo = true;
                PagamentoRefeicao.MeioPagamentoSAT = new MeioPagamentoSATInformation { IDMeioPagamentoSAT = 7 };

                CRUD.Adicionar(PagamentoRefeicao);
                AddLog("Pagamento 'Vale Refeição' cadastrado!");
            }

            TaxaEntregaIFood = TaxaEntrega.CarregarPorNome("iFood");
            if (TaxaEntregaIFood == null)
            {
                TaxaEntregaIFood = new TaxaEntregaInformation();
                TaxaEntregaIFood.Nome = "iFood";
                TaxaEntregaIFood.Ativo = true;
                TaxaEntregaIFood.Excluido = false;
                TaxaEntregaIFood.Valor = 0;

                CRUD.Adicionar(TaxaEntregaIFood);
                AddLog("Taxa de Entrega 'iFood' cadastrada!");
            }

            TipoDescontoIFood = (TipoDescontoInformation)CRUD.Carregar(new TipoDescontoInformation { Nome = "iFood" });
            if (TipoDescontoIFood.IDTipoDesconto == null)
            {
                TipoDescontoIFood = new TipoDescontoInformation();
                TipoDescontoIFood.Nome = "iFood";
                TipoDescontoIFood.Ativo = true;
                TipoDescontoIFood.Excluido = false;

                CRUD.Adicionar(TipoDescontoIFood);
                AddLog("Tipo Desconto 'iFood' adicionado!");
            }

            TaxaAdicional = (ProdutoInformation)CRUD.Carregar(new ProdutoInformation { Nome = "Taxa Adicional iFood" });
            if (TaxaAdicional.IDProduto == null)
            {
                TaxaAdicional = new ProdutoInformation();
                TaxaAdicional.Nome = "Taxa Adicional iFood";
                TaxaAdicional.TipoProduto = new TipoProdutoInformation { IDTipoProduto = 10 };
                TaxaAdicional.ValorUnitario = 0;
                TaxaAdicional.Ativo = true;
                TaxaAdicional.Disponibilidade = true;
                TaxaAdicional.Excluido = false;
                TaxaAdicional.DtAlteracaoDisponibilidade = DateTime.Now;
                TaxaAdicional.DtUltimaAlteracao = DateTime.Now;
                TaxaAdicional.ClassificacaoFiscal = new ClassificacaoFiscalInformation { IDClassificacaoFiscal = 2 };
                TaxaAdicional.Unidade = new UnidadeInformation { IDUnidade = 1 };
                TaxaAdicional.ControlarEstoque = false;
                TaxaAdicional.UtilizarBalanca = false;
                TaxaAdicional.AssistenteModificacoes = false;
                TaxaAdicional.GUIDIdentificacao = Guid.NewGuid().ToString();

                CRUD.Adicionar(TaxaAdicional);
                AddLog("Produto com o nome 'Taxa Adicional iFood' cadastrado!");
            }

            PagamentoDinheiro = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Dinheiro));
            if (PagamentoDinheiro == null)
            {
                AddLog("Não há um meio de pagamento 'Dinheiro' disponível cadastrado no Backoffice");
                configurado = false;
            }

            PagamentoDebito = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Debito));
            if (PagamentoDebito == null)
            {
                AddLog("Não há um meio de pagamento 'Cartão de Débito' disponível cadastrado no Backoffice");
                configurado = false;
            }

            PagamentoCredito = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Credito));
            if (PagamentoCredito == null)
            {
                AddLog("Não há um meio de pagamento 'Cartão de Credito' disponível cadastrado no Backoffice");
                configurado = false;
            }

            PagamentoOutros = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Outros));
            if (PagamentoOutros == null)
            {
                AddLog("Não há um meio de pagamento 'Outros' disponível cadastrado no Backoffice");
                configurado = false;
            }

            try
            {
                UsuarioIfood = Usuario.Autenticar(ConfigIFood.ChaveUsuario);
            }
            catch (ExceptionPDV ex)
            {
                configurado = false;
                AddLog(ex.Message);
            }
            catch (Exception ex)
            {
                configurado = false;
                AddLog("Erro ao carregar o usuário, pela chave informada");
                AddLog(ex.Message);
            }

            return configurado;
        }

        private void Loop()
        {
            try
            {
                EventosRecebidos = new List<string>();
                AddLog("Integração iFood: Ativada");

                while (Executando)
                {
                    if (AccessToken == null || AccessToken == "" || ExpiraEm < DateTime.Now)
                    {
                        AddLog(" -- Autenticando ----------------------------------------------------------------------------------------------");
                        if (!Autenticar())
                        {
                            Sleep(60);
                            continue;
                        }
                    }

                    APIOrder = new API.Order(AccessToken);
                    APIMerchant = new API.Merchant(AccessToken);

                    AddLog(" -- Verificando status da loja -------------------------------------------------------------------------------");
                    if (LojaAbertaSistema())
                    {
                        if (!LojaAbertaIfood())
                        {
                            Sleep(60);
                            continue;
                        }

                        AddLog(" -- Lendo eventos --------------------------------------------------------------------------------------------");
                        LerEventos();

                        AddLog(" -- Enviando confirmações ------------------------------------------------------------------------------------");
                        EnviaConfirmacao();

                        Sleep(25);
                    }
                    else
                    {
                        Sleep(60);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog("Erro na integração iFood: " + ex.Message);
                AddLog("Reinicie o Integrador para restabelecer essa integração...\r\nCaso não resolva, entre em contato com o suporte!!!");
                //if (!ex.Message.Contains("token expired") && !ex.Message.Contains("Invalid access"))
                //    throw new ExceptionPDV(CodigoErro.EE11, ex);
            }
        }

        private Boolean Autenticar()
        {
            API.OAuth apiOAuth = new API.OAuth();

            try
            {
                if (String.IsNullOrEmpty(ConfigIFood.RefreshToken))
                {
                    Model.OAuth.Token token = apiOAuth.Token("authorization_code", ConfigIFood.ClientId, ConfigIFood.ClientSecret, ConfigIFood.AuthorizationCode, ConfigIFood.AuthorizationCodeVerifier, "");

                    if (token.accessToken != "")
                    {
                        ConfigIFood.RefreshToken = token.refreshToken;
                        AtualizarRefreshToken(token.refreshToken);

                        AccessToken = token.accessToken;
                        ExpiraEm = DateTime.Now.AddSeconds(token.expiresIn * 0.8);

                        AddLog("Autorizado e novo Token gerado (authorization_code)!");
                        AddLog(AccessToken);
                        return true;
                    }
                    else
                    {
                        AddLog("Falha na autentição!");
                        return false;
                    }
                }
                else
                {
                    Model.OAuth.Token token = apiOAuth.Token("refresh_token", ConfigIFood.ClientId, ConfigIFood.ClientSecret, ConfigIFood.AuthorizationCode, ConfigIFood.AuthorizationCodeVerifier, ConfigIFood.RefreshToken);

                    if (token.accessToken != "")
                    {
                        ConfigIFood.RefreshToken = token.refreshToken;
                        AtualizarRefreshToken(token.refreshToken);

                        AccessToken = token.accessToken;
                        ExpiraEm = DateTime.Now.AddSeconds(token.expiresIn * 0.8);


                        AddLog("Autenticado e novo Token gerado (refresh_token)!");
                        AddLog(AccessToken);
                        return true;
                    }
                    else
                    {
                        AddLog("Falha na autentição!");
                        AccessToken = "";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog($"Erro na geração do Token: " + ex.Message);
                AccessToken = "";
                return false;
            }
        }

        private void AtualizarRefreshToken(string refreshToken)
        {
            ConfiguracaoBDInformation config = ConfiguracaoBD.BuscarConfiguracao("RefreshToken", 150);
            config.Valor = refreshToken;
            CRUD.Alterar(config);
        }

        public void LerEventos()
        {
            try
            {
                var eventos = APIOrder.EventsPolling();
                if (eventos == null)
                {
                    AddLog("Sem eventos!");
                    return;
                }

                AddLog(eventos.Length.ToString() + " encontrados...");

                foreach (var evento in eventos)
                {
                    AddLog("  > Id " + evento.id + " > OrderId " + evento.orderId + " > Code " + evento.code);
                    AddLog("  >> " + JsonConvert.SerializeObject(evento));

                    if (EventosRecebidos.Contains(evento.id))
                    {
                        AddLog("Evento duplicado...");
                        APIOrder.Acknowledgment(new Model.Order.Event[] { evento });
                    }
                    else
                    {
                        EventosRecebidos.Add(evento.id);
                    }

                    try
                    {
                        switch (evento.code)
                        {
                            case "PLC":
                                AddLog("  > Adicionar pedido > " + evento.orderId);
                                AdicionarPedido(evento);
                                break;

                            case "CON":
                                AddLog("  > Finalizar pedido > " + evento.orderId);
                                FinalizarPedido(evento);
                                break;

                            case "CAN":
                                AddLog("  > Confirmar cancelamento pedido > " + evento.orderId);
                                ConfirmarCancelamentoPedido(evento);
                                break;

                            case "CAR":
                                AddLog("  > Cancelamento confirmado pelo iFood > " + evento.orderId);
                                break;

                            case "CCR":
                                AddLog("  > Cancelar pedido > " + evento.orderId);
                                CancelarPedido(evento);
                                break;

                            case "CFM":
                                AddLog("  > Pedido confirmado pelo iFood > " + evento.orderId);
                                //ConfirmarPedido(evento);
                                break;

                            case "RDA":
                                AddLog("  > Motoboy disponível para essa entrega > " + evento.orderId);
                                break;

                            case "DSP":
                                AddLog("  > Despacho confirmado pelo iFood > " + evento.orderId);
                                break;

                            default:
                                AddLog("  > ATENÇÃO: Evento não tratado (" + evento.code + ") > " + evento.orderId);
                                break;
                        }

                        APIOrder.Acknowledgment(new Model.Order.Event[] { evento });
                    }
                    catch (Exception ex)
                    {
                        AddLog("  > Erro processando evento: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                AddLog("  > Erro lendo eventos: " + ex.Message);
            }
        }
    }
}