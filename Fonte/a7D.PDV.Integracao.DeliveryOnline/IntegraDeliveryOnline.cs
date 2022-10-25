using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.Integracao.Servico.Core;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace a7D.PDV.Integracao.DeliveryOnline
{
    enum eStatusLoja
    {
        none, aberta, fechada
    }

    public partial class IntegraDeliveryOnline : IntegracaoTask
    {
        public override string Nome => "Delivery Online";

        ConfiguracoesDeliveryOnline ConfigDO;

        public override void Executar()
        {
            //if (!ValidarLicenca())'
            //    return;

            if (!ValidarConfiguracoes())
                return;

            Iniciar(() => Loop());
        }

        private bool ValidarConfiguracoes()
        {
            Boolean configurado = true;

            ConfigDO = new ConfiguracoesDeliveryOnline();

            if (!ConfigDO.IntegracaoDeliveryOnline)
            {
                AddLog("Integração Delivery Online desligada no Configurador");
                return false;
            }

            if (string.IsNullOrEmpty(ConfigDO.Username)
             || string.IsNullOrEmpty(ConfigDO.Password)
             || string.IsNullOrEmpty(ConfigDO.DeviceName))
            {
                AddLog("Falta configurar o Delivery Online no Configurador (username, password e device-name)");
                return false;
            }

            return configurado;
        }

        private void Loop()
        {
            try
            {
                //EventosRecebidos = new List<string>();
                AddLog("Integração Delivery Online: Ativada");

                while (Executando)
                {
                    if (String.IsNullOrEmpty(ConfigDO.Token))
                    {
                        AddLog("Autenticando...");
                        if (!Autenticar())
                        {
                            Sleep(60);
                            continue;
                        }
                    }

                    Sleep(60);

                    //APIOrder = new API.Order(AccessToken);
                    //APIMerchant = new API.Merchant(AccessToken);

                    //AddLog("Verificando status da loja");
                    //if (LojaAbertaSistema())
                    //{
                    //    if (!LojaAbertaIfood())
                    //    {
                    //        Sleep(60);
                    //        continue;
                    //    }

                    //    AddLog("Lendo eventos...");
                    //    LerEventos();

                    //    AddLog("Enviando confirmações...");
                    //    EnviaConfirmacao();

                    //    Sleep(25);
                    //}
                    //else
                    //{
                    //    Sleep(60);
                    //    continue;
                    //}
                }
            }
            catch (Exception ex)
            {
                AddLog("Erro na integração Delivery Online: " + ex.Message);
                AddLog("Reinicie o Integrador para restabelecer essa integração...\r\nCaso não resolva, entre em contato com o suporte!!!");
                //if (!ex.Message.Contains("token expired") && !ex.Message.Contains("Invalid access"))
                //    throw new ExceptionPDV(CodigoErro.EE11, ex);
            }
        }
        private Boolean Autenticar()
        {
            API.Auth apiAuth = new API.Auth();

            try
            {
                Model.Token.Token token = apiAuth.Token(ConfigDO.Username, ConfigDO.Password, ConfigDO.DeviceName);

                if (!String.IsNullOrEmpty(token.token))
                {
                    ConfigDO.Token = token.token;
                    AtualizarToken(token.token);

                    AddLog("Autorizado e Token gerado!");
                    AddLog(ConfigDO.Token);
                    return true;
                }
                else
                {
                    AddLog("Falha na autentição!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                AddLog($"Erro na geração do Token: " + ex.Message);
                ConfigDO.Token = "";
                return false;
            }
        }
        private void AtualizarToken(string token)
        {
            ConfiguracaoBDInformation config = ConfiguracaoBD.BuscarConfiguracao("Token", 250);
            config.Valor = token;
            CRUD.Alterar(config);
        }
    }
}