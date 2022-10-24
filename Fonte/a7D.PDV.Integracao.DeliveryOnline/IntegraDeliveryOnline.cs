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
            //if (!ValidarLicenca())
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

            return configurado;
        }

        private void Loop()
        {
            try
            {
                //EventosRecebidos = new List<string>();
                AddLog("Integração iFood: Ativada");

                while (Executando)
                {
                    //if (AccessToken == null || AccessToken == "" || ExpiraEm < DateTime.Now)
                    //{
                    //    AddLog("Autenticando...");
                    //    if (!Autenticar())
                    //    {
                    //        Sleep(60);
                    //        continue;
                    //    }
                    //}

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
                AddLog("Erro na integração DeliveryOnline: " + ex.Message);
                AddLog("Reinicie o Integrador para restabelecer essa integração...\r\nCaso não resolva, entre em contato com o suporte!!!");
                //if (!ex.Message.Contains("token expired") && !ex.Message.Contains("Invalid access"))
                //    throw new ExceptionPDV(CodigoErro.EE11, ex);
            }
        }
    }
}