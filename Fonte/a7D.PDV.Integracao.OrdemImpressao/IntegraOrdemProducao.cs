using a7D.PDV.BLL;
using a7D.PDV.BLL.Services;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Windows.Forms;

namespace a7D.PDV.Integracao.Servico.Core.Impressao
{
    public class IntegraOrdemProducao : IntegracaoTask
    {
        private PDVInformation PDVPadrao;

        private UsuarioInformation UsuarioPadrao;

        private ConfiguracoesGerenciadorImpressao configOI;

        public override int Revalidar => 2; // SEMPRE

        public override string Nome => "Ordem de Impressão";

        private static bool JaTentou = false;

        public override void Executar()
        {
            Disponivel = true;
            configOI = new ConfiguracoesGerenciadorImpressao();
            PDVPadrao = BLL.PDV.Listar().FirstOrDefault(p => (p.TipoPDV.Tipo == EF.Enum.ETipoPDV.CAIXA || p.TipoPDV.Tipo == EF.Enum.ETipoPDV.COMANDA_ELETRONICA));
            UsuarioPadrao = Usuario.Listar().FirstOrDefault(u => u.PermissaoAdm == true);
            Configurado = true;

            try
            {
                ImpressoraWindows.Largura = configOI.LarguraImpressaoWindows;
                ImpressoraWindows.Margem = configOI.MargemImpressaoWindows;
                ImpressoraWindows.FontePadrao = new Font(configOI.FonteNomeImpressaoWindows, configOI.FonteTamanhoImpressaoWindows);
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }

            if (!JaTentou && ImpressoraWindows.FontePadrao.Name != configOI.FonteNomeImpressaoWindows)
            {
                // Se o nome da fonte não for o mesmo é porque não achou e precisa instalar!
                JaTentou = true;
                AddLog(FontesUtil.InstalaFontes());
            }

            Iniciar(new Action(() => Loop()));
        }

        private void Loop()
        {
            AddLog("Ordem de Impressão Ativa");

            //// Apaga ordens de impressão anterior a 2 horas
            //EF.Repositorio.Execute("DELETE FROM tbOrdemImpressao WHERE DtOrdem<@data OR EnviadoFilaImpressao=1",
            //    new SqlParameter("@data", DateTime.Now.AddHours(-2)));

            while (Executando)
            {
                Componentes.Services.OrdemImpressaoServices.ImprimiPendentes(UsuarioPadrao, PDVPadrao, configOI, AddLog);
                Sleep(5);
            }
        }

        private static DateTime dtNextUpdate = DateTime.MinValue;

        public static void Update(ListView lvStatus)
        {
            try
            {
                if (dtNextUpdate > DateTime.Now)
                    return;

                dtNextUpdate = DateTime.Now.AddMinutes(1);
                var list = EF.Repositorio.Listar<tbAreaImpressao>();
                lvStatus.Clear();
                foreach (var item in list)
                {
                    var lvi = new ListViewItem($"{item.Nome} - {item.NomeImpressora}", item.IDPDV > 0 ? 1 : 0);
                    try
                    {
                        if (item.IDPDV > 0)
                        {
                            string status = ConfiguracaoBD.ValorOuPadrao(EF.Enum.EConfig._StatusImpressora, EF.Enum.ETipoPDV.CAIXA, item.IDPDV.Value);
                            if (status != null)
                            {
                                lvi.ToolTipText = status;
                                if (status.Length > 11 && DateTime.TryParseExact(status.Substring(0, 11), "dd/MM HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out DateTime dt))
                                {
                                    if (status.Contains(" ERRO: "))
                                    {
                                        lvi.ForeColor = Color.Red;
                                    }
                                    else if (dt < DateTime.Now.AddMinutes(-5))
                                    {
                                        lvi.ForeColor = Color.Red;
                                        lvi.ToolTipText += " (Caixa sem informação atual da impressora)";
                                    }
                                    else if (dt < DateTime.Now.AddMinutes(-2))
                                    {
                                        lvi.ForeColor = Color.OrangeRed;
                                        lvi.ToolTipText += " (Caixa sem informação da impressora a mais de 2 minutos)";
                                    }
                                    else
                                    {
                                        lvi.ForeColor = Color.Green;
                                        lvi.ToolTipText += " OK";
                                    }
                                }
                            }
                            else
                            {
                                lvi.ForeColor = Color.Red;
                                lvi.ToolTipText += " (Caixa não instalado)";
                            }
                        }
                        else
                        {
                            var localServer = new LocalPrintServer();
                            var queue = localServer.GetPrintQueue(item.NomeImpressora);
                            if (queue.QueueStatus == PrintQueueStatus.None)
                                lvi.ForeColor = Color.Green;
                            else
                            {
                                lvi.ForeColor = Color.Red;
                                lvi.ToolTipText = queue.QueueStatus.ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lvi.ToolTipText = ex.Message;
                        lvi.ForeColor = Color.Red;
                    }
                    lvStatus.Items.Add(lvi);
                }


            }
            catch (Exception)
            {
            }
        }
    }
}
