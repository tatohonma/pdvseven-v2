// frmInvalidarNotaFiscal.Designer.cs
using System.ComponentModel;

namespace a7D.PDV.Caixa.UI
{
    partial class frmInvalidarNotaFiscal
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.grpDados = new System.Windows.Forms.GroupBox();
            this.lblSerie = new System.Windows.Forms.Label();
            this.numSerie = new System.Windows.Forms.NumericUpDown();
            this.lblNumeroInicial = new System.Windows.Forms.Label();
            this.numNumeroInicial = new System.Windows.Forms.NumericUpDown();
            this.lblNumeroFinal = new System.Windows.Forms.Label();
            this.numNumeroFinal = new System.Windows.Forms.NumericUpDown();
            this.grpMotivo = new System.Windows.Forms.GroupBox();
            this.lblMotivo = new System.Windows.Forms.Label();
            this.txtMotivo = new System.Windows.Forms.TextBox();
            this.lblContador = new System.Windows.Forms.Label();
            this.btnInutilizar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.panelBotoes = new System.Windows.Forms.Panel();
            this.grpDados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSerie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumeroInicial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumeroFinal)).BeginInit();
            this.grpMotivo.SuspendLayout();
            this.panelBotoes.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(16, 14);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(287, 22);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Inutilização de Numeração";
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Font = new System.Drawing.Font("Arial", 10F);
            this.lblInfo.Location = new System.Drawing.Point(17, 41);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(766, 36);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "Informe a série e o intervalo (número inicial e final) das notas que deseja inutil" +
    "izar, e descreva o motivo.";
            // 
            // grpDados
            // 
            this.grpDados.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDados.Controls.Add(this.lblSerie);
            this.grpDados.Controls.Add(this.numSerie);
            this.grpDados.Controls.Add(this.lblNumeroInicial);
            this.grpDados.Controls.Add(this.numNumeroInicial);
            this.grpDados.Controls.Add(this.lblNumeroFinal);
            this.grpDados.Controls.Add(this.numNumeroFinal);
            this.grpDados.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpDados.Location = new System.Drawing.Point(20, 80);
            this.grpDados.Name = "grpDados";
            this.grpDados.Size = new System.Drawing.Size(763, 120);
            this.grpDados.TabIndex = 2;
            this.grpDados.TabStop = false;
            this.grpDados.Text = "Dados da inutilização";
            // 
            // lblSerie
            // 
            this.lblSerie.AutoSize = true;
            this.lblSerie.Font = new System.Drawing.Font("Arial", 10F);
            this.lblSerie.Location = new System.Drawing.Point(18, 35);
            this.lblSerie.Name = "lblSerie";
            this.lblSerie.Size = new System.Drawing.Size(40, 16);
            this.lblSerie.TabIndex = 0;
            this.lblSerie.Text = "Série";
            // 
            // numSerie
            // 
            this.numSerie.Font = new System.Drawing.Font("Arial", 10F);
            this.numSerie.Location = new System.Drawing.Point(21, 55);
            this.numSerie.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numSerie.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSerie.Name = "numSerie";
            this.numSerie.Size = new System.Drawing.Size(170, 23);
            this.numSerie.TabIndex = 1;
            this.numSerie.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblNumeroInicial
            // 
            this.lblNumeroInicial.AutoSize = true;
            this.lblNumeroInicial.Font = new System.Drawing.Font("Arial", 10F);
            this.lblNumeroInicial.Location = new System.Drawing.Point(224, 35);
            this.lblNumeroInicial.Name = "lblNumeroInicial";
            this.lblNumeroInicial.Size = new System.Drawing.Size(95, 16);
            this.lblNumeroInicial.TabIndex = 2;
            this.lblNumeroInicial.Text = "Número inicial";
            // 
            // numNumeroInicial
            // 
            this.numNumeroInicial.Font = new System.Drawing.Font("Arial", 10F);
            this.numNumeroInicial.Location = new System.Drawing.Point(227, 55);
            this.numNumeroInicial.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numNumeroInicial.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNumeroInicial.Name = "numNumeroInicial";
            this.numNumeroInicial.Size = new System.Drawing.Size(250, 23);
            this.numNumeroInicial.TabIndex = 3;
            this.numNumeroInicial.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNumeroInicial.ValueChanged += new System.EventHandler(this.numNumeroInicial_ValueChanged);
            // 
            // lblNumeroFinal
            // 
            this.lblNumeroFinal.AutoSize = true;
            this.lblNumeroFinal.Font = new System.Drawing.Font("Arial", 10F);
            this.lblNumeroFinal.Location = new System.Drawing.Point(506, 35);
            this.lblNumeroFinal.Name = "lblNumeroFinal";
            this.lblNumeroFinal.Size = new System.Drawing.Size(83, 16);
            this.lblNumeroFinal.TabIndex = 4;
            this.lblNumeroFinal.Text = "Número final";
            // 
            // numNumeroFinal
            // 
            this.numNumeroFinal.Font = new System.Drawing.Font("Arial", 10F);
            this.numNumeroFinal.Location = new System.Drawing.Point(509, 55);
            this.numNumeroFinal.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.numNumeroFinal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNumeroFinal.Name = "numNumeroFinal";
            this.numNumeroFinal.Size = new System.Drawing.Size(236, 23);
            this.numNumeroFinal.TabIndex = 5;
            this.numNumeroFinal.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNumeroFinal.ValueChanged += new System.EventHandler(this.numNumeroFinal_ValueChanged);
            // 
            // grpMotivo
            // 
            this.grpMotivo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMotivo.Controls.Add(this.lblContador);
            this.grpMotivo.Controls.Add(this.txtMotivo);
            this.grpMotivo.Controls.Add(this.lblMotivo);
            this.grpMotivo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.grpMotivo.Location = new System.Drawing.Point(20, 210);
            this.grpMotivo.Name = "grpMotivo";
            this.grpMotivo.Size = new System.Drawing.Size(763, 170);
            this.grpMotivo.TabIndex = 3;
            this.grpMotivo.TabStop = false;
            this.grpMotivo.Text = "Motivo";
            // 
            // lblMotivo
            // 
            this.lblMotivo.AutoSize = true;
            this.lblMotivo.Font = new System.Drawing.Font("Arial", 10F);
            this.lblMotivo.Location = new System.Drawing.Point(18, 28);
            this.lblMotivo.Name = "lblMotivo";
            this.lblMotivo.Size = new System.Drawing.Size(274, 16);
            this.lblMotivo.TabIndex = 0;
            this.lblMotivo.Text = "Descreva o motivo da inutilização (min. 15)";
            // 
            // txtMotivo
            // 
            this.txtMotivo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMotivo.Font = new System.Drawing.Font("Arial", 10F);
            this.txtMotivo.Location = new System.Drawing.Point(21, 50);
            this.txtMotivo.MaxLength = 255;
            this.txtMotivo.Multiline = true;
            this.txtMotivo.Name = "txtMotivo";
            this.txtMotivo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMotivo.Size = new System.Drawing.Size(724, 84);
            this.txtMotivo.TabIndex = 1;
            this.txtMotivo.TextChanged += new System.EventHandler(this.txtMotivo_TextChanged);
            // 
            // lblContador
            // 
            this.lblContador.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContador.Font = new System.Drawing.Font("Arial", 9F);
            this.lblContador.ForeColor = System.Drawing.Color.DimGray;
            this.lblContador.Location = new System.Drawing.Point(540, 137);
            this.lblContador.Name = "lblContador";
            this.lblContador.Size = new System.Drawing.Size(205, 20);
            this.lblContador.TabIndex = 2;
            this.lblContador.Text = "0/255";
            this.lblContador.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelBotoes
            // 
            this.panelBotoes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBotoes.Controls.Add(this.btnCancelar);
            this.panelBotoes.Controls.Add(this.btnInutilizar);
            this.panelBotoes.Location = new System.Drawing.Point(20, 388);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Size = new System.Drawing.Size(763, 52);
            this.panelBotoes.TabIndex = 4;
            // 
            // btnInutilizar
            // 
            this.btnInutilizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInutilizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnInutilizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInutilizar.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.btnInutilizar.ForeColor = System.Drawing.Color.White;
            this.btnInutilizar.Location = new System.Drawing.Point(549, 8);
            this.btnInutilizar.Name = "btnInutilizar";
            this.btnInutilizar.Size = new System.Drawing.Size(206, 36);
            this.btnInutilizar.TabIndex = 1;
            this.btnInutilizar.Text = "Inutilizar numeração";
            this.btnInutilizar.UseVisualStyleBackColor = false;
            this.btnInutilizar.Click += new System.EventHandler(this.btnInutilizar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.Location = new System.Drawing.Point(337, 8);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(206, 36);
            this.btnCancelar.TabIndex = 0;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // frmInvalidarNotaFiscal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelBotoes);
            this.Controls.Add(this.grpMotivo);
            this.Controls.Add(this.grpDados);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblTitulo);
            this.Font = new System.Drawing.Font("Arial", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInvalidarNotaFiscal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inutilizar Nota Fiscal (NFC-e/NF-e)";
            this.grpDados.ResumeLayout(false);
            this.grpDados.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSerie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumeroInicial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumeroFinal)).EndInit();
            this.grpMotivo.ResumeLayout(false);
            this.grpMotivo.PerformLayout();
            this.panelBotoes.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.GroupBox grpDados;
        private System.Windows.Forms.Label lblSerie;
        private System.Windows.Forms.NumericUpDown numSerie;
        private System.Windows.Forms.Label lblNumeroInicial;
        private System.Windows.Forms.NumericUpDown numNumeroInicial;
        private System.Windows.Forms.Label lblNumeroFinal;
        private System.Windows.Forms.NumericUpDown numNumeroFinal;
        private System.Windows.Forms.GroupBox grpMotivo;
        private System.Windows.Forms.Label lblMotivo;
        private System.Windows.Forms.TextBox txtMotivo;
        private System.Windows.Forms.Label lblContador;
        private System.Windows.Forms.Panel panelBotoes;
        private System.Windows.Forms.Button btnInutilizar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
