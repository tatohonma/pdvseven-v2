using a7D.PDV.BLL;
using a7D.PDV.BLL.Utils;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.Componentes;
using a7D.PDV.Fiscal.NFCe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmCFE : Form
    {
        private const string FORMATODATA = "dd/MM/yyyy HH:mm:ss";
        private DateTime dataDe;
        private DateTime dataAte;
        private readonly IFormatProvider _cultureInfo = new CultureInfo("pt-BR");
        private bool start = false;
        private PedidosCFe[] pedidos;
        private bool running = false;

        public frmCFE()
        {
            InitializeComponent();
        }

        private void frmCFE_Load(object sender, EventArgs e)
        {
            if (ConfiguracoesSistema.Valores.Fiscal != "SAT")
            {
                btnEmitirPendentes.TextAlign = ContentAlignment.MiddleCenter;
                btnEmitirPendentes.Image = null;
                NFeFacade.ConfigPathXSD(new FileInfo(GetType().Assembly.Location).Directory.FullName);
            }

            dgvPrincipal.AutoGenerateColumns = false;
            GA.Post(this);
            //var x = Image.FromFile(Path.GetPathRoot());
            //var imagem = (Image)(new Bitmap(MyImage, new Size(32, 32)));
            this.dgvPrincipal.DefaultCellStyle.Font = new Font("Arial", 9);

            var dtMesAnterior = DateTime.Now.AddMonths(-1);

            var dia1MesAnterior = new DateTime(dtMesAnterior.Year, dtMesAnterior.Month, 1, 0, 0, 0);
            var ultimoDiaMesAnterior = dia1MesAnterior.AddMonths(1).AddSeconds(-1);

            dataDe = dia1MesAnterior;
            dtpFiltroDe.Value = dataDe;

            dataAte = ultimoDiaMesAnterior;
            dtpFiltroAte.Value = dataAte;

            start = true;
            Filtrar();
        }

        private void dtpDe_ValueChanged(object sender, EventArgs e)
        {
            dataDe = dtpFiltroDe.Value;
            Filtrar();
        }

        private void dtpAte_ValueChanged(object sender, EventArgs e)
        {
            dataAte = dtpFiltroAte.Value;
            Filtrar();
        }

        private Array GerarListaGrid(string busca = "", bool somenteNaoEmitidos = false, bool somenteEmitidos = false)
        {
            var lista = pedidos.Select(r => new
            {
                r.IDPedido,
                r.IDRetornoSAT,
                r.IDTipoSolicitacaoSAT,
                DtPedidoFechamento = r.DtPedidoFechamento.ToString(FORMATODATA),
                r.ChaveConsulta,
                r.DocumentoCliente,
                r.ValorTotal,
                Emitir = string.IsNullOrEmpty(r.ChaveConsulta) && r.ValorTotal > 0 ? "Emitir" : "",
                Exportar = !string.IsNullOrEmpty(r.ChaveConsulta) ? "Exportar" : "",
                Enviar = !string.IsNullOrEmpty(r.ChaveConsulta) ? "Enviar" : ""
            });

            // Apenas um tipo de busca de cada vez!
            if (!string.IsNullOrEmpty(busca))
            {
                busca = busca.ToLower();
                lista = lista.Where(p => p.ChaveConsulta?.ToLower().Contains(busca) == true || p.DocumentoCliente?.Contains(busca) == true);
            }
            else if (somenteNaoEmitidos)
                lista = lista.Where(l => string.IsNullOrEmpty(l.ChaveConsulta));
            else if (somenteEmitidos)
                lista = lista.Where(l => !string.IsNullOrEmpty(l.ChaveConsulta));

            return lista.ToArray();
        }

        private async void Filtrar()
        {
            if (!start)
                return;
            else if (dataDe > dataAte)
                return;

            dgvPrincipal.Visible = false;
            pnlCarregando.Visible = true;
            dgvPrincipal.Refresh();

            await Task.Run(() => pedidos = Pedido.ListarFinalizados(dataDe, dataAte));

            dgvPrincipal.DataSource = GerarListaGrid();
            EstilizarGrid();
            pnlCarregando.Visible = false;
            dgvPrincipal.Visible = true;
        }

        private void EstilizarGrid()
        {
            foreach (DataGridViewRow linha in dgvPrincipal.Rows)
            {
                var cellEmitir = linha.Cells[dgvPrincipal.Columns["colEmitir"].Index];
                var cellExportar = linha.Cells[dgvPrincipal.Columns["colExportar"].Index];
                var cellEnviar = linha.Cells[dgvPrincipal.Columns["colEnviar"].Index];

                cellEmitir.Style.ForeColor = cellExportar.Style.ForeColor = cellEnviar.Style.ForeColor = Color.Blue;

            }
            dgvPrincipal.Refresh();
        }

        private void dgvPrincipal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPrincipal.Columns["colEmitir"].Index)
            {
                var cell = dgvPrincipal.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (!string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    var cellPedido = dgvPrincipal.Rows[e.RowIndex].Cells[dgvPrincipal.Columns["colIDPedido"].Index];

                    var idPedido = Convert.ToInt32(cellPedido.Value.ToString());
                    if (MessageBox.Show($"Deseja emitir os CF-e do pedido nº {idPedido}?", "Emissão Sat", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        EnviarParaSat(idPedido, false);
                        Filtrar();
                    }
                }
            }
            else if (e.ColumnIndex == dgvPrincipal.Columns["colEnviar"].Index)
            {
                var cell = dgvPrincipal.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (!string.IsNullOrEmpty(cell.Value.ToString()))
                {
                    var cellIDRetornoSAT = dgvPrincipal.Rows[e.RowIndex].Cells[dgvPrincipal.Columns["colIDRetornoSAT"].Index];
                    var idRetornoSAT = Convert.ToInt32(cellIDRetornoSAT.Value.ToString());
                    EnviarCFE(new List<int>() { idRetornoSAT });
                }
            }
            else if (e.ColumnIndex == dgvPrincipal.Columns["colExportar"].Index)
            {
                var cell = dgvPrincipal.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (!string.IsNullOrEmpty(cell.Value.ToString()))
                {

                    var cellIDRetornoSat = dgvPrincipal.Rows[e.RowIndex].Cells[dgvPrincipal.Columns["colIDRetornoSAT"].Index];
                    var cellIDPedido = dgvPrincipal.Rows[e.RowIndex].Cells[dgvPrincipal.Columns["colIDPedido"].Index];
                    var listaIDRetornoSat = new List<int>();
                    listaIDRetornoSat.Add(Convert.ToInt32(cellIDRetornoSat.Value));

                    if (MessageBox.Show($"Deseja exportar o CF-e do pedido nº {cellIDPedido.Value}?", "Exportar arquivo CF-e", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var selectedPath = ChooseFolderHelper();

                        if (string.IsNullOrWhiteSpace(selectedPath) == false)
                        {
                            if (Directory.Exists(selectedPath))
                            {
                                ExportarCFe(listaIDRetornoSat, selectedPath);

                                var abrirPasta = MessageBox.Show(string.Format("Exportado com sucesso para a pasta {0}\nAbrir agora?", selectedPath), "Sucesso!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (abrirPasta == DialogResult.Yes)
                                    Process.Start("explorer.exe", selectedPath);
                            }
                            else
                                MessageBox.Show("Diretório escolhido não existe!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show("Operação cancelada", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void ExportarCFe(List<int> idsRetornoSat, string selectedPath)
        {
            pnlCarregando.Visible = true;
            foreach (int numeroCfe in idsRetornoSat)
            {
                var xml = InformacaoXML.ObterXML(numeroCfe);
                xml.XML.Save(Path.Combine(selectedPath, xml.NomeArquivo));
            }
            pnlCarregando.Visible = false;

        }


        private string EnviarParaSat(int idPedido, bool lista)
        {
            string msg = "";

            var pedido = pedidos.Where(p => p.IDPedido == idPedido).FirstOrDefault();
            if ((pedido.ValorTotal ?? 0) == 0)
            {
                if (!lista)
                    MessageBox.Show("Não é possivel gerar uma nota fiscal com valor zero", "SAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return "";
            }
            else if ((pedido.ValorTotal ?? 0) > 10000)
            {
                if (!lista)
                    MessageBox.Show("Não é possivel gerar SAT com valor superior a R$ 10.000", "SAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Veja mais em https://www.confaz.fazenda.gov.br/legislacao/ajustes/2014/AJ_008_14

                return $"Pedido {pedido.IDPedido}: Acima do valor permitido de R$ 10.000,00\n";
            }

            frmAguardandoSAT frmSAT = null;
            var pedido2 = Pedido.CarregarCompleto(pedido.IDPedido);
            using (frmSAT = new frmAguardandoSAT(pedido2, true, AC.PDV.IDPDV.Value, AC.Usuario.IDUsuario.Value, "", $"Emitindo CF-e do pedido {pedido.IDPedido}"))
            {
                var result = frmSAT.ShowDialog();
                if (!lista)
                {
                    if (result == DialogResult.No)
                        MessageBox.Show(CodigoErro.E500.ToString() + "\n Erro no sat.\n" + frmSAT.Exception.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else if (result == DialogResult.Yes)
                        MessageBox.Show("Ocorreu um erro ao imprimir, mas o cupom SAT FOI ENVIADO.\nVocê pode tentar reimprimir o cupom no menu \"SAT>Reimprimir SAT\"\n\nDetalhes do erro: " + frmSAT.Exception.Message, "Erro ao imprimir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (result == DialogResult.OK)
                        MessageBox.Show("CF-e emitido com sucesso.", "", MessageBoxButtons.OK);
                }
                else
                {
                    if (result == DialogResult.No)
                        msg += $"Pedido {pedido.IDPedido}: {CodigoErro.E501.ToString()} {frmSAT.Exception.Message}\n";
                    else if (result == DialogResult.Yes)
                        msg += $"Pedido {pedido.IDPedido}: {frmSAT.Exception.Message}\n";
                }
            }

            return msg;
        }

        private void txtCfe_TextChanged(object sender, EventArgs e)
        {
            FiltrarPorBusca(txtBusca.Text);
        }

        private void FiltrarPorBusca(string busca)
        {

            dgvPrincipal.DataSource = GerarListaGrid(busca);
            EstilizarGrid();
            dgvPrincipal.Refresh();

        }


        private static string ChooseFolderHelper()
        {
            var result = new StringBuilder();
            var thread = new Thread(obj =>
            {
                var builder = (StringBuilder)obj;
                using (var dialog = new FolderBrowserDialog())
                {
                    dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                    dialog.ShowNewFolderButton = true;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        builder.Append(dialog.SelectedPath);
                    }
                }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(result);

            while (thread.IsAlive)
            {
                Thread.Sleep(100);
            }

            return result.ToString();
        }



        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                var indexColuna = dgvPrincipal.Columns["colIDRetornoSAT"].Index;
                var linhas = new List<DataGridViewRow>();


                foreach (DataGridViewRow linha in dgvPrincipal.Rows)
                {
                    var valueColIDsat = linha.Cells[indexColuna].Value?.ToString();
                    if (!string.IsNullOrEmpty(valueColIDsat))
                        linhas.Add(linha);
                }

                if (MessageBox.Show($"Deseja exportar o(s) CF-e(s)?\nTotal de arquivos: {linhas.Count}", "Exportar CF-e", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var selectedPath = ChooseFolderHelper();

                    if (string.IsNullOrWhiteSpace(selectedPath) == false)
                    {
                        if (Directory.Exists(selectedPath))
                        {
                            foreach (DataGridViewRow linha in linhas)
                            {
                                var cellIDRetornoSat = linha.Cells[indexColuna];
                                var listaIDRetornoSat = new List<int>();
                                listaIDRetornoSat.Add(Convert.ToInt32(cellIDRetornoSat.Value));
                                ExportarCFe(listaIDRetornoSat, selectedPath);
                            }
                            var abrirPasta = MessageBox.Show(string.Format("Exportado com sucesso para a pasta {0}\nAbrir agora?", selectedPath), "Sucesso!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (abrirPasta == DialogResult.Yes)
                                Process.Start("explorer.exe", selectedPath);
                        }
                        else
                            MessageBox.Show("Diretório escolhido não existe!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("Operação cancelada", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.ECFA, ex);
            }
        }

        private async void btnReemitirTodos_Click(object sender, EventArgs e)
        {
            btnEmitirPendentes.Visible = false;
            var lista = GerarListaGrid("", true);

            if (lista.Length == 0)
            {
                MessageBox.Show($"Não há CF-e pendentes nesta lista.", "Emitir CF-e pendentes", MessageBoxButtons.OK);
            }
            else if (MessageBox.Show($"Deseja emitir os CF-e pendentes?\nTotal de pedidos: {lista.Length}", "Emissão Sat", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                pnlCarregando.Visible = true;
                await Task.Run(new Action(() =>
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        var msg = "Operação concluída.\n\n";
                        var msgErros = "";
                        foreach (dynamic itemListaGrid in lista)
                        {
                            string retornoId = itemListaGrid.IDRetornoSAT;
                            if (string.IsNullOrEmpty(retornoId))
                                msgErros += EnviarParaSat(itemListaGrid.IDPedido, true);
                        }

                        if (!string.IsNullOrEmpty(msgErros))
                            msg += "Não foi possível emitir os seguintes pedidos:\n" + msgErros;

                        MessageBox.Show(msg, "Emissão de CF-e", MessageBoxButtons.OK);

                    }));
                }));

                pnlCarregando.Visible = false;
                Filtrar();
            }

            btnEmitirPendentes.Visible = true;
        }

        private void btnEnviarCFe_Click(object sender, EventArgs e)
        {
            try
            {
                var cfeEmitidos = GerarListaGrid("", false, true);

                if (MessageBox.Show($"Deseja enviar os CF-e agora?\nTotal de CF-e: {cfeEmitidos.Length}", "Emissão Sat", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                var idsRetornoSat = new List<int>();
                foreach (dynamic linha in cfeEmitidos)
                    idsRetornoSat.Add(Convert.ToInt32(linha.IDRetornoSAT));

                EnviarCFE(idsRetornoSat);
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.ECFE, ex);
            }
        }

        private async void EnviarCFE(List<int> idsRetornoSat)
        {
            if (running)
                return;

            running = true;

            try
            {
                btnEnviarCFe.Visible = false;

                var configSat = new ConfiguracoesSAT();
                var cnpj = configSat.InfCFe_emit_CNPJ;
                var emailPadrao = configSat.infCFe_email_destinatario_padrao;

                var formEscolherEmail = new frmEscolherEmail
                {
                    Destinatario = emailPadrao
                };

                if (formEscolherEmail.ShowDialog() != DialogResult.OK)
                    return;

                Exception innerError = null;
                using (var formEnviando = new frmDialogAguarde())
                {
                    formEnviando.TopMost = true;
                    formEnviando.Show();

                    await Task.Run(new Action(() =>
                    {
                        try
                        {
                            CFE.EnviarZipParaWS(idsRetornoSat.ToArray(),
                                $"cfes_cnpj_{cnpj}_{dataDe.ToString(FORMATODATA)}_{dataAte.ToString(FORMATODATA)}.zip", dataDe, dataAte,
                                formEscolherEmail.Destinatario);
                        }
                        catch (Exception ex)
                        {
                            innerError = ex;
                        }
                    }));

                    formEnviando.Close();
                }

                if (innerError != null)
                    Logs.ErroBox(CodigoErro.ECFE, innerError);
                else
                    MessageBox.Show($"Email enviado com sucesso.", "Envio de Email", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
            }
            finally
            {
                running = false;
                btnEnviarCFe.Visible = true;
            }
        }
    }
}
