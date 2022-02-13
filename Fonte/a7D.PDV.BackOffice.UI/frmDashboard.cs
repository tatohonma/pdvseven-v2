using mshtml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.IO;
using System.Threading;
using a7D.PDV.BLL;
using Newtonsoft.Json;
using a7D.PDV.BackOffice.UI.CEFAsyncBoundObjects;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmDashboard : Form
    {
        public ChromiumWebBrowser chromeBrowser { get; set; }
        private bool ScriptLoaded { get; set; }
        private CancellationTokenSource tokenSource;
        public Action<bool> AlterarAtivado { get; set; }

        Func<string, string> GetJsScope = (id) => $"angular.element('#{id}').scope().vm";

        public frmDashboard()
        {
            InitializeComponent();
            ScriptLoaded = false;
        }

        private void frmDashboard_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            IniciarDashboard();
        }

        public void IniciarDashboard()
        {
            AlterarAtivado?.Invoke(false);
            if (chromeBrowser == null)
            {
                chromeBrowser = new ChromiumWebBrowser($"file://{Environment.CurrentDirectory}/dashboard.html");
                chromeBrowser.RegisterAsyncJsObject("backoffice", new BackofficeAsyncObject());

                chromeBrowser.Size = Size;
                Controls.Add(chromeBrowser);
                chromeBrowser.Dock = DockStyle.Fill;
                tokenSource = new CancellationTokenSource();
                chromeBrowser.LoadingStateChanged += ChromeBrowser_LoadingStateChanged;
            }
            else
            {
                chromeBrowser.Load($"file://{Environment.CurrentDirectory}/dashboard.html");
                ScriptLoaded = false;
            }

        }

        private void ChromeBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading && !ScriptLoaded)
            {
                ScriptLoaded = true;
                AlterarAtivado?.Invoke(true);
                var token = tokenSource.Token;


                Task.Run(new Action(() =>
                {
                    try
                    {
                        var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                        var script = "var {0} = new Chart('{0}', {1});\n";


                        var js = new StringBuilder();
                        js.AppendLine("$(\".carregando\").remove();");
                        js.AppendLine("$(\".conteudo\").show();");

                        var dashboard = Dashboard.ObterDashboard(DateTime.Now.AddDays(-30));

                        var faturamentoDia = dashboard.FaturamentoPorDia;
                        js.AppendFormat(script, nameof(faturamentoDia), JsonConvert.SerializeObject(faturamentoDia, settings));

                        var faturamentoMensal = dashboard.FaturamentoMensal;
                        js.AppendFormat(script, nameof(faturamentoMensal), JsonConvert.SerializeObject(faturamentoMensal, settings));


                        var faturamentoCategoria = dashboard.FaturamentoPorCategoria;
                        js.AppendFormat(script, nameof(faturamentoCategoria), JsonConvert.SerializeObject(faturamentoCategoria, settings));

                        var faturamentoTipoPagamento = dashboard.FaturamentoPorTipoDePagamento;
                        js.AppendFormat(script, nameof(faturamentoTipoPagamento), JsonConvert.SerializeObject(faturamentoTipoPagamento, settings));

                        var faturamentoTipoPedido = dashboard.FaturamentoPorTipoPedido;
                        js.AppendFormat(script, nameof(faturamentoTipoPedido), JsonConvert.SerializeObject(faturamentoTipoPedido, settings));

                        var faturamentoDiaSemana = dashboard.FaturamentoPorDiaDaSemana;
                        js.AppendFormat(script, nameof(faturamentoDiaSemana), JsonConvert.SerializeObject(faturamentoDiaSemana, settings));

                        var motivosCancelamento = dashboard.MotivosCancelamento;
                        js.AppendFormat(script, nameof(motivosCancelamento), JsonConvert.SerializeObject(motivosCancelamento, settings));

                        var volumeGarcom = dashboard.VolumePorGarcom;
                        js.AppendFormat(script, nameof(volumeGarcom), JsonConvert.SerializeObject(volumeGarcom, settings));

                        chromeBrowser.GetMainFrame().ExecuteJavaScriptAsync(js.ToString());

                        var backofficeAsync = new BackofficeAsyncObject();
                        var _30dias = new DateTimeOffset(DateTime.Now.AddDays(-30)).ToUnixTimeMilliseconds();

                        InitBackground(token, "vendas", "atualizarDadosQuantidade", "Quantidade", _30dias, null, backofficeAsync.ProdutosVendidos);
                        InitBackground(token, "vendas", "atualizarDadosValor", "Valor", _30dias, null, backofficeAsync.ProdutosVendidos);
                        InitBackground(token, "clientes", "atualizarDados", _30dias, null, backofficeAsync.MelhoresClientes);
                        InitBackground(token, "fechamento", "atualizarDados", -1, backofficeAsync.ObterFechamento);
                        InitBackground(token, "resumo", "atualizarDados", _30dias, null, backofficeAsync.ObterResumo);
                    }
                    catch (OperationCanceledException)
                    {

                    }
                    catch (Exception ex)
                    {
                        if (ex.Source != "CefSharp")
                            throw ex;
                    }
                }), token);
            }
        }

        private void InitBackground(CancellationToken token, string scope, string func, string arg1, double arg2, double? arg3, Func<string, double, double?, string> local)
        {
            Task.Run(() =>
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    var ret = local(arg1, arg2, arg3);
                    token.ThrowIfCancellationRequested();
                    chromeBrowser.GetMainFrame().ExecuteJavaScriptAsync($"{GetJsScope(scope)}.{func}({ret});");
                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception)
                {
                    throw;
                }

            });
        }

        private void InitBackground(CancellationToken token, string scope, string func, double arg1, double? arg2, Func<double, double?, string> local)
        {
            Task.Run(() =>
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    var ret = local(arg1, arg2);
                    token.ThrowIfCancellationRequested();
                    chromeBrowser.GetMainFrame().ExecuteJavaScriptAsync($"{GetJsScope(scope)}.{func}({ret});");
                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception)
                {
                    throw;
                }

            });
        }

        private void InitBackground(CancellationToken token, string scope, string func, int arg, Func<int, string> local)
        {
            Task.Run(() =>
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    var ret = local(arg);
                    token.ThrowIfCancellationRequested();
                    chromeBrowser.GetMainFrame().ExecuteJavaScriptAsync($"{GetJsScope(scope)}.{func}({ret});");
                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception)
                {
                    throw;
                }

            });
        }

        private void frmDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            tokenSource?.Cancel();
        }
    }
}
