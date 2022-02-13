using a7D.PDV.BLL;
using a7D.PDV.Integracao.Servico.Core.MyFinance;
using a7D.PDV.Integracao.Servico.Core.MyFinance.Models;
using a7D.PDV.Model;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using a7D.PDV.Integracao.API2.Model;

namespace a7D.PDV.Integracao.Servico.Core
{
    public class ServicoCore
    {
        ConfiguracoesServicoIntegracoes config; 

        public static ConfiguracoesServicoIntegracoes Configuracoes()
        {
            return new ConfiguracoesServicoIntegracoes();
        }
    }
}
