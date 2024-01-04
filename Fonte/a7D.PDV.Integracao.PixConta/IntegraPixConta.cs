//using a7D.Fmk.CRUD.DAL;
//using a7D.PDV.BLL;
//using a7D.PDV.EF.Enum;
//using a7D.PDV.Integracao.Servico.Core;
//using a7D.PDV.Model;
//using Newtonsoft.Json;
using a7D.PDV.Integracao.Servico.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace a7D.PDV.Integracao.PixConta
{
    public partial class IntegraPixConta : IntegracaoTask
    {
        public override string Nome => "Pix-Conta";

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

            return configurado;
        }

        private void Loop()
        {
            try
            {
                AddLog("Integração Pix-Conta: Ativada");

                while (Executando)
                {
                    AddLog("Verificando Faturas");
                    Sleep(60);
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