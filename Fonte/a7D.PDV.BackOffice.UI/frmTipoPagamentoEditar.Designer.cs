namespace a7D.PDV.BackOffice.UI
{
    partial class frmTipoPagamentoEditar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblImpressoraFiscal = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.lblNome = new System.Windows.Forms.Label();
            this.ckbAtivo = new System.Windows.Forms.CheckBox();
            this.cbbMeioPagamento = new System.Windows.Forms.ComboBox();
            this.lblMeioPagamento = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbbGateway = new System.Windows.Forms.ComboBox();
            this.ckbRegistrarValores = new System.Windows.Forms.CheckBox();
            this.tlpCampos = new System.Windows.Forms.TableLayoutPanel();
            this.lblRecebivel = new System.Windows.Forms.Label();
            this.cbbBandeira = new System.Windows.Forms.ComboBox();
            this.cbbRecebivel = new System.Windows.Forms.ComboBox();
            this.lblBandeira = new System.Windows.Forms.Label();
            this.tlpCampos.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelar.Location = new System.Drawing.Point(110, 395);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(92, 47);
            this.btnCancelar.TabIndex = 1;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSalvar.Location = new System.Drawing.Point(12, 395);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(92, 47);
            this.btnSalvar.TabIndex = 0;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // txtCodigo
            // 
            this.txtCodigo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCodigo.Location = new System.Drawing.Point(3, 130);
            this.txtCodigo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(306, 25);
            this.txtCodigo.TabIndex = 2;
            // 
            // lblImpressoraFiscal
            // 
            this.lblImpressoraFiscal.AutoSize = true;
            this.lblImpressoraFiscal.Location = new System.Drawing.Point(3, 110);
            this.lblImpressoraFiscal.Name = "lblImpressoraFiscal";
            this.lblImpressoraFiscal.Size = new System.Drawing.Size(185, 17);
            this.lblImpressoraFiscal.TabIndex = 12;
            this.lblImpressoraFiscal.Text = "Código (Impressora Fiscal)";
            // 
            // txtNome
            // 
            this.txtNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNome.Location = new System.Drawing.Point(3, 20);
            this.txtNome.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(306, 25);
            this.txtNome.TabIndex = 0;
            // 
            // lblNome
            // 
            this.lblNome.AutoSize = true;
            this.lblNome.Location = new System.Drawing.Point(3, 0);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(47, 17);
            this.lblNome.TabIndex = 10;
            this.lblNome.Text = "Nome";
            // 
            // ckbAtivo
            // 
            this.ckbAtivo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckbAtivo.AutoSize = true;
            this.ckbAtivo.Location = new System.Drawing.Point(3, 362);
            this.ckbAtivo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.ckbAtivo.Name = "ckbAtivo";
            this.ckbAtivo.Size = new System.Drawing.Size(239, 21);
            this.ckbAtivo.TabIndex = 5;
            this.ckbAtivo.Text = "Visível para pagamento no Caixa";
            this.ckbAtivo.UseVisualStyleBackColor = true;
            // 
            // cbbMeioPagamento
            // 
            this.cbbMeioPagamento.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbMeioPagamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbMeioPagamento.FormattingEnabled = true;
            this.cbbMeioPagamento.Location = new System.Drawing.Point(3, 185);
            this.cbbMeioPagamento.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cbbMeioPagamento.Name = "cbbMeioPagamento";
            this.cbbMeioPagamento.Size = new System.Drawing.Size(306, 25);
            this.cbbMeioPagamento.TabIndex = 3;
            this.cbbMeioPagamento.Visible = false;
            // 
            // lblMeioPagamento
            // 
            this.lblMeioPagamento.AutoSize = true;
            this.lblMeioPagamento.Location = new System.Drawing.Point(3, 165);
            this.lblMeioPagamento.Name = "lblMeioPagamento";
            this.lblMeioPagamento.Size = new System.Drawing.Size(137, 17);
            this.lblMeioPagamento.TabIndex = 18;
            this.lblMeioPagamento.Text = "Meio de Pagamento";
            this.lblMeioPagamento.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(201, 17);
            this.label4.TabIndex = 20;
            this.label4.Text = "Meio de Pagamento Integrado";
            // 
            // cbbGateway
            // 
            this.cbbGateway.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbGateway.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbGateway.FormattingEnabled = true;
            this.cbbGateway.Location = new System.Drawing.Point(3, 75);
            this.cbbGateway.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cbbGateway.Name = "cbbGateway";
            this.cbbGateway.Size = new System.Drawing.Size(306, 25);
            this.cbbGateway.TabIndex = 1;
            this.cbbGateway.SelectedIndexChanged += new System.EventHandler(this.cbbGateway_SelectedIndexChanged);
            // 
            // ckbRegistrarValores
            // 
            this.ckbRegistrarValores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ckbRegistrarValores.AutoSize = true;
            this.ckbRegistrarValores.Location = new System.Drawing.Point(3, 333);
            this.ckbRegistrarValores.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.ckbRegistrarValores.Name = "ckbRegistrarValores";
            this.ckbRegistrarValores.Size = new System.Drawing.Size(207, 21);
            this.ckbRegistrarValores.TabIndex = 4;
            this.ckbRegistrarValores.Text = "Visível na abertura do Caixa";
            this.ckbRegistrarValores.UseVisualStyleBackColor = true;
            // 
            // tlpCampos
            // 
            this.tlpCampos.AutoSize = true;
            this.tlpCampos.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpCampos.ColumnCount = 1;
            this.tlpCampos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCampos.Controls.Add(this.lblRecebivel, 0, 8);
            this.tlpCampos.Controls.Add(this.cbbBandeira, 0, 14);
            this.tlpCampos.Controls.Add(this.cbbRecebivel, 0, 12);
            this.tlpCampos.Controls.Add(this.txtNome, 0, 1);
            this.tlpCampos.Controls.Add(this.lblNome, 0, 0);
            this.tlpCampos.Controls.Add(this.label4, 0, 2);
            this.tlpCampos.Controls.Add(this.cbbGateway, 0, 3);
            this.tlpCampos.Controls.Add(this.cbbMeioPagamento, 0, 7);
            this.tlpCampos.Controls.Add(this.lblMeioPagamento, 0, 6);
            this.tlpCampos.Controls.Add(this.lblImpressoraFiscal, 0, 4);
            this.tlpCampos.Controls.Add(this.txtCodigo, 0, 5);
            this.tlpCampos.Controls.Add(this.ckbAtivo, 0, 16);
            this.tlpCampos.Controls.Add(this.ckbRegistrarValores, 0, 15);
            this.tlpCampos.Controls.Add(this.lblBandeira, 0, 13);
            this.tlpCampos.Location = new System.Drawing.Point(12, 12);
            this.tlpCampos.Name = "tlpCampos";
            this.tlpCampos.RowCount = 17;
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpCampos.Size = new System.Drawing.Size(312, 388);
            this.tlpCampos.TabIndex = 24;
            // 
            // lblRecebivel
            // 
            this.lblRecebivel.AutoSize = true;
            this.lblRecebivel.Location = new System.Drawing.Point(3, 220);
            this.lblRecebivel.Name = "lblRecebivel";
            this.lblRecebivel.Size = new System.Drawing.Size(135, 17);
            this.lblRecebivel.TabIndex = 27;
            this.lblRecebivel.Text = "Conta de Recebivel";
            this.lblRecebivel.Visible = false;
            // 
            // cbbBandeira
            // 
            this.cbbBandeira.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbBandeira.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbBandeira.FormattingEnabled = true;
            this.cbbBandeira.Location = new System.Drawing.Point(3, 295);
            this.cbbBandeira.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cbbBandeira.Name = "cbbBandeira";
            this.cbbBandeira.Size = new System.Drawing.Size(306, 25);
            this.cbbBandeira.TabIndex = 26;
            this.cbbBandeira.Visible = false;
            // 
            // cbbRecebivel
            // 
            this.cbbRecebivel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbRecebivel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbRecebivel.FormattingEnabled = true;
            this.cbbRecebivel.Location = new System.Drawing.Point(3, 240);
            this.cbbRecebivel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.cbbRecebivel.Name = "cbbRecebivel";
            this.cbbRecebivel.Size = new System.Drawing.Size(306, 25);
            this.cbbRecebivel.TabIndex = 25;
            this.cbbRecebivel.Visible = false;
            // 
            // lblBandeira
            // 
            this.lblBandeira.AutoSize = true;
            this.lblBandeira.Location = new System.Drawing.Point(3, 275);
            this.lblBandeira.Name = "lblBandeira";
            this.lblBandeira.Size = new System.Drawing.Size(66, 17);
            this.lblBandeira.TabIndex = 21;
            this.lblBandeira.Text = "Bandeira";
            this.lblBandeira.Visible = false;
            // 
            // frmTipoPagamentoEditar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 454);
            this.Controls.Add(this.tlpCampos);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmTipoPagamentoEditar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmTipoPagamentoEditar_Load);
            this.tlpCampos.ResumeLayout(false);
            this.tlpCampos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.TextBox txtCodigo;
        private System.Windows.Forms.Label lblImpressoraFiscal;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.CheckBox ckbAtivo;
        private System.Windows.Forms.ComboBox cbbMeioPagamento;
        private System.Windows.Forms.Label lblMeioPagamento;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbbGateway;
        private System.Windows.Forms.CheckBox ckbRegistrarValores;
        private System.Windows.Forms.TableLayoutPanel tlpCampos;
        private System.Windows.Forms.Label lblRecebivel;
        private System.Windows.Forms.ComboBox cbbBandeira;
        private System.Windows.Forms.ComboBox cbbRecebivel;
        private System.Windows.Forms.Label lblBandeira;
    }
}