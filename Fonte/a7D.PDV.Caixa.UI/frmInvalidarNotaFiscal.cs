using System;
using System.Windows.Forms;
using a7D.PDV.BLL;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmInvalidarNotaFiscal : Form 
    {
        private const int MOTIVO_MIN = 15;

        private readonly int _idUsuario;
        private readonly int _idPDV;

        public int Serie => (int)numSerie.Value;
        public int NumeroInicial => (int)numNumeroInicial.Value;
        public int NumeroFinal => (int)numNumeroFinal.Value;
        public string Motivo => (txtMotivo.Text ?? string.Empty).Trim();

        public frmInvalidarNotaFiscal(int idUsuario, int idPDV)
        {
            _idUsuario = idUsuario;
            _idPDV = idPDV;

            InitializeComponent();
            AtualizaUI();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnInutilizar_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;

            var msg =
                "Confirmar inutilização?\n\n" +
                $"Série: {Serie}\n" +
                $"Número: {NumeroInicial} a {NumeroFinal}\n" +
                $"Motivo: {Motivo}";

            var ok = MessageBox.Show(msg, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ok != DialogResult.Yes)
                return;

            UseWaitCursor = true;
            btnInutilizar.Enabled = false;
            btnCancelar.Enabled = false;

            try
            {
                var retornoInut = FiscalServices
                    .Inutilizacao(Serie, NumeroInicial, NumeroFinal, Motivo, _idPDV, _idUsuario)
                    .Enviar();

                // RetornoSAT.Salvar(retornoInut);

                MessageBox.Show(
                    retornoInut.mensagem ?? "Inutilização concluída.",
                    retornoInut.EEEEE ?? "OK",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ExceptionPDV ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                btnInutilizar.Enabled = true;
                btnCancelar.Enabled = true;
            }
        }


        private bool Validar()
        {
            if (Serie <= 0)
            {
                MessageBox.Show("Informe uma série válida.", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                numSerie.Focus();
                return false;
            }

            if (NumeroInicial <= 0)
            {
                MessageBox.Show("Informe um número inicial válido.", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                numNumeroInicial.Focus();
                return false;
            }

            if (NumeroFinal <= 0)
            {
                MessageBox.Show("Informe um número final válido.", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                numNumeroFinal.Focus();
                return false;
            }

            if (NumeroFinal < NumeroInicial)
            {
                MessageBox.Show("O número final não pode ser menor que o número inicial.", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                numNumeroFinal.Focus();
                return false;
            }

            if (Motivo.Length < MOTIVO_MIN)
            {
                MessageBox.Show($"O motivo deve ter pelo menos {MOTIVO_MIN} caracteres.", "Validação",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMotivo.Focus();
                return false;
            }

            AtualizaUI();
            return true;
        }

        private void AtualizaUI()
        {
            if (numNumeroFinal.Value < numNumeroInicial.Value)
                numNumeroFinal.Value = numNumeroInicial.Value;

            lblContador.Text = $"{(txtMotivo.Text?.Length ?? 0)}/255";

            btnInutilizar.Enabled =
                Serie > 0 &&
                NumeroInicial > 0 &&
                NumeroFinal >= NumeroInicial &&
                Motivo.Length >= MOTIVO_MIN;
        }

        private void txtMotivo_TextChanged(object sender, EventArgs e) => AtualizaUI();
        private void numSerie_ValueChanged(object sender, EventArgs e) => AtualizaUI();
        private void numNumeroInicial_ValueChanged(object sender, EventArgs e) => AtualizaUI();
        private void numNumeroFinal_ValueChanged(object sender, EventArgs e) => AtualizaUI();
    }
}
