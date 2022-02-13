using a7D.PDV.BLL;
using a7D.PDV.Integracao.Pagamento.NTKPos;
using a7D.PDV.Integracao.Servico.Core;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using static a7D.PDV.DAL.DashboardDAL;

namespace a7D.PDV.Integracao.NTKServer
{
    public class IntegraNTK : IntegracaoTask
    {
        public static bool ModoFiscal = false;
        public static string BasePath = null;

        public override int Revalidar => 3;

        public override string Nome => "NTK POS Integrado";

        public override void Executar()
        {
            Configurado = false;
            Disponivel = BLL.PDV.PossuiPOSNTK();
            if (!Disponivel)
            {
                AddLog("Sem PDV com Licenças POS NTK");
                return;
            }

            AddLog("Servidor do POS Integrado NTK ");

            var tipoPagamento = BLL.TipoPagamento.Listar().FirstOrDefault(tp => tp.Gateway == EF.Enum.EGateway.NTKPOS);
            if (tipoPagamento == null)
            {
                AddLog("Não há Tipo de Pagamento com POS Integrado NTK");
                return;
            }

            ModoFiscal = ConfiguracoesSistema.Valores.Fiscal == "SAT";

            var fi = new FileInfo(Assembly.GetEntryAssembly().Location);
            var di = new DirectoryInfo(Path.Combine(fi.Directory.FullName, "POS"));
            if (!di.Exists)
                di.Create();

            BasePath = fi.Directory.FullName;

            AddLog("LOG: " + di.FullName);

            AddLog("Iniciando: " + tipoPagamento.Nome + (ModoFiscal ? " (MODO FISCAL SAT)" : " (GERENCIAL)"));

            string porta = ConfigurationManager.AppSettings["NTKPorta"].ToString();
            if (!Int16.TryParse(porta, out Int16 nPorta))
            {
                AddLog($"Porta '{porta}' incorreta");
                return;
            }

            Configurado = true;
            var server = ServerNTKPos<FluxoPos>.Start(nPorta, onEventNTKPos, onErro, false);
            Iniciar(() => server.MonitorLoop());
        }

        public void onEventNTKPos(TerminalInformation terminal, string status)
        {
            if (terminal != null)
                status = $"{terminal.terminalId} {terminal.pdvName}: {status}";

            AddLog(status);
        }

        public void onErro(TerminalInformation terminal, Exception ex)
        {
            AddLog(terminal?.terminalId + ": " + ex.Message);

            ex.Data.Add("terminalId", terminal?.terminalId);
            ex.Data.Add("pdvID", terminal?.pdvID);
            ex.Data.Add("pdvUserID", terminal?.pdvUserID);

            AddLog(ex);
        }
    }
}