//using a7D.Fmk.CRUD.DAL;
//using a7D.PDV.BLL;
//using a7D.PDV.EF.Enum;
//using a7D.PDV.Integracao.Servico.Core;
//using a7D.PDV.Model;
//using Newtonsoft.Json;
using a7D.PDV.Integracao.Servico.Core;
using System;
using a7D.PDV.BLL;

namespace a7D.PDV.Integracao.PixConta
{
    public partial class IntegraPixConta : IntegracaoTask
    {
        public override string Nome => "Pix-Conta";
        ConfiguracoesPixConta ConfigPixConta;
        API.Invoice APIInvoice;

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

            ConfigPixConta = new ConfiguracoesPixConta();

            if(!ConfigPixConta.ContaCliente)
            {
                AddLog("Integração Pix-Conta Conta-Cliente desligada no Configurador");
                return false;
            }

            if (string.IsNullOrEmpty(ConfigPixConta.Token_IUGU))
            {
                AddLog("Falta configurar o Token IUGU no Configurador");
                return false;
            }

            return configurado;
        }

        private void Loop()
        {
            try
            {
                AddLog("Integração Pix-Conta: Ativada");

                while (Executando)
                {
                    if (APIInvoice == null)
                        APIInvoice = new API.Invoice(ConfigPixConta.Token_IUGU);

                    AddLog("Verificar Pagamentos");
                    VerificarPagamentosPendentes();

                    CancelarFaturas();

                    Sleep(30);
                }
            }
            catch (Exception ex)
            {
                AddLog("Erro na integração Pix-Conta: " + ex.Message);
                AddLog("Reinicie o Integrador para restabelecer essa integração...\r\nCaso não resolva, entre em contato com o suporte!!!");

                AddLog("Detalhes: \n" + ex.ToString());
            }
        }
    }
}