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

        API.Order APIOrder;
        API.Merchant APIMerchant;
        String AccessToken;
        DateTime ExpiraEm;

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
                AddLog("Não há um meio de pagamento com Gateway 'iFood' cadastrado no Backoffice");
                configurado = false;
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

            PagamentoRefeicao = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Refeicao));
            if (PagamentoRefeicao == null)
            {
                AddLog("Não há um meio de pagamento 'Vale Refeição' disponível cadastrado no Backoffice");
                configurado = false;
            }

            PagamentoOutros = listaPagamentos.FirstOrDefault(p => p.MeioPagamentoSAT != null && p.MeioPagamentoSAT.IDMeioPagamentoSAT.Value == (int)(EMetodoPagamento.Outros));
            if (PagamentoOutros == null)
            {
                AddLog("Não há um meio de pagamento 'Outros' disponível cadastrado no Backoffice");
                configurado = false;
            }

            TaxaEntregaIFood = TaxaEntrega.CarregarPorNome("iFood");
            if (TaxaEntregaIFood == null)
            {
                AddLog("Não há uma 'Taxa de Entrega' com o nome 'iFood' cadastrada no Backoffice");
                configurado = false;
            }

            TipoDescontoIFood = (TipoDescontoInformation)CRUD.Carregar(new TipoDescontoInformation { Nome = "iFood" });
            if (TipoDescontoIFood.IDTipoDesconto == null)
            {
                AddLog("Não há um 'Tipo de Desconto' com o nome 'iFood' cadastrada no Backoffice");
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
                AddLog("Integração iFood: Ativada");

                while (Executando)
                {
                    if (AccessToken == null || AccessToken == "" || ExpiraEm < DateTime.Now)
                    {
                        AddLog("Autenticando...");
                        if (!Autenticar())
                            continue;
                    }

                    APIOrder = new API.Order(AccessToken);
                    APIMerchant = new API.Merchant(AccessToken);

                    AddLog("Verificando status da loja");
                    if (LojaAbertaSistema())
                    {
                        if (!LojaAbertaIfood())
                            continue;

                        AddLog("Lendo eventos...");
                        LerEventos();

                        AddLog("Enviando confirmações...");
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
                        AccessToken = token.accessToken;
                        ConfigIFood.RefreshToken = token.refreshToken;
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
            ConfiguracaoBDInformation config = ConfiguracaoBD.BuscarConfiguracao("RefreshToken");
            config.Valor = refreshToken;
            CRUD.Alterar(config);
        }
    }
}