using a7D.PDV.BLL;
using a7D.PDV.Integracao.Servico.Core;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace a7D.PDV.Integracao.DeliveryOnline
{
    public partial class IntegraDeliveryOnline : IntegracaoTask
    {
        public override string Nome => "Delivery Online";

        public override void Executar()
        {
            //if (!ValidarLicenca())
            //    return;

            //if (!ValidarConfiguracoes())
            //    return;

            Iniciar(() => Loop());
        }

        private void Loop()
        {
        }
    }
}