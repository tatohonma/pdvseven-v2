using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
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
        TaxaEntregaInformation TaxaEntregaDO;
        UsuarioInformation UsuarioDO;
        PDVInformation PDVDO;

        API.Orders APIOrders;
        API.Locations APILocations;

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
                AddLog("Falta configurar o acesso a API do Delivery Online no Configurador (username, password e device-name)");
                configurado = false;
            }

            TaxaEntregaDO = TaxaEntrega.CarregarPorNome("DeliveryOnline");
            if (TaxaEntregaDO == null)
            {
                AddLog("Não há uma 'Taxa de Entrega' com o nome 'DeliveryOnline' cadastrada no Backoffice");
                configurado = false;
            }

            var pdvs = BLL.PDV.Listar();
            PDVDO = pdvs.FirstOrDefault(p => p.IDPDV == ConfigDO.CaixaPDV && p.TipoPDV.Tipo == ETipoPDV.CAIXA);
            if (PDVDO == null)
            {
                AddLog($"Caixa ID PDV: {ConfigDO.CaixaPDV} inválido!");
                configurado = false;
            }

            try
            {
                UsuarioDO = Usuario.Autenticar(ConfigDO.ChaveUsuario);
            }
            catch (ExceptionPDV ex)
            {
                configurado = false;
                AddLog(ex.Message);
            }
            catch (Exception ex)
            {
                configurado = false;
                AddLog("Erro ao carregar o usuário pela chave informada ( " + ConfigDO.ChaveUsuario + ")");
                AddLog(ex.Message);
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

                    if (APIOrders == null)
                        APIOrders = new API.Orders(ConfigDO.Token);

                    if(APILocations == null)
                        APILocations = new API.Locations(ConfigDO.Token);

                    AddLog("Lendo pedidos...");
                    LerPedidos();

                    //APIMerchant = new API.Merchant(AccessToken);

                    //AddLog("Verificando status da loja");
                    //if (LojaAbertaSistema())
                    //{
                    //    if (!LojaAbertaDeliveryOnline())
                    //    {
                    //        Sleep(60);
                    //        continue;
                    //    }


                    //    if (APIOrders == null)
                    //        APIOrders = new API.Orders(ConfigDO.Token);

                    //    AddLog("Lendo pedidos...");
                    //    LerPedidos();


                    //    //    if (!LojaAbertaIfood())
                    //    //    {
                    //    //        Sleep(60);
                    //    //        continue;
                    //    //    }

                    //    //    AddLog("Lendo eventos...");
                    //    //    LerEventos();

                    //    //    AddLog("Enviando confirmações...");
                    //    //    EnviaConfirmacao();

                    //    //    Sleep(25);
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

        private void LerPedidos()
        {
            var pedidos = APIOrders.GetOrders();

            if (pedidos == null)
            {
                AddLog("Sem pedidos!");
                return;
            }

            foreach (var pedido in pedidos.data)
            {
                switch(pedido.attributes.status.status_id)
                {
                    case 2: //Pendente confirmação
                        //Verificar se já foi importado

                        //Adicionar no sistema
                        AdicionarPedido(pedido);

                        //Alterar status na API
                        break;
                }
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
                AtualizarToken("");

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