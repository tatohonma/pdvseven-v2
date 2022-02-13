namespace a7D.PDV.BackOffice.UI
{
    partial class frmCFE
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvPrincipal = new System.Windows.Forms.DataGridView();
            this.colIDPedido = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIDTipoSolicitacaoSAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIDRetornoSAT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDtPedidoFechamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChaveConsulta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentoCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValorTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmitir = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colExportar = new System.Windows.Forms.DataGridViewLinkColumn();
            this.colEnviar = new System.Windows.Forms.DataGridViewLinkColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFiltroDe = new System.Windows.Forms.DateTimePicker();
            this.txtBusca = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFiltroAte = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlCarregando = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.btnEnviarCFe = new System.Windows.Forms.Button();
            this.btnEmitirPendentes = new System.Windows.Forms.Button();
            this.btnExportar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrincipal)).BeginInit();
            this.pnlCarregando.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPrincipal
            // 
            this.dgvPrincipal.AllowUserToAddRows = false;
            this.dgvPrincipal.AllowUserToDeleteRows = false;
            this.dgvPrincipal.AllowUserToResizeColumns = false;
            this.dgvPrincipal.AllowUserToResizeRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(240)))));
            this.dgvPrincipal.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPrincipal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPrincipal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPrincipal.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.dgvPrincipal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvPrincipal.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvPrincipal.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvPrincipal.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrincipal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvPrincipal.ColumnHeadersHeight = 40;
            this.dgvPrincipal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIDPedido,
            this.colIDTipoSolicitacaoSAT,
            this.colIDRetornoSAT,
            this.colDtPedidoFechamento,
            this.colChaveConsulta,
            this.colDocumentoCliente,
            this.colValorTotal,
            this.colEmitir,
            this.colExportar,
            this.colEnviar});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPrincipal.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvPrincipal.EnableHeadersVisualStyles = false;
            this.dgvPrincipal.Location = new System.Drawing.Point(12, 88);
            this.dgvPrincipal.Name = "dgvPrincipal";
            this.dgvPrincipal.ReadOnly = true;
            this.dgvPrincipal.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrincipal.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvPrincipal.RowHeadersVisible = false;
            this.dgvPrincipal.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle10.Padding = new System.Windows.Forms.Padding(5);
            this.dgvPrincipal.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvPrincipal.RowTemplate.DividerHeight = 1;
            this.dgvPrincipal.RowTemplate.Height = 36;
            this.dgvPrincipal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvPrincipal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPrincipal.Size = new System.Drawing.Size(1000, 397);
            this.dgvPrincipal.TabIndex = 4;
            this.dgvPrincipal.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrincipal_CellContentClick);
            // 
            // colIDPedido
            // 
            this.colIDPedido.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colIDPedido.DataPropertyName = "IDPedido";
            this.colIDPedido.FillWeight = 119.7969F;
            this.colIDPedido.HeaderText = "ID Pedido";
            this.colIDPedido.Name = "colIDPedido";
            this.colIDPedido.ReadOnly = true;
            this.colIDPedido.Width = 104;
            // 
            // colIDTipoSolicitacaoSAT
            // 
            this.colIDTipoSolicitacaoSAT.DataPropertyName = "IDTipoSolicitacaoSAT";
            this.colIDTipoSolicitacaoSAT.HeaderText = "TipoSolicitacao";
            this.colIDTipoSolicitacaoSAT.Name = "colIDTipoSolicitacaoSAT";
            this.colIDTipoSolicitacaoSAT.ReadOnly = true;
            this.colIDTipoSolicitacaoSAT.Visible = false;
            // 
            // colIDRetornoSAT
            // 
            this.colIDRetornoSAT.DataPropertyName = "IDRetornoSAT";
            this.colIDRetornoSAT.HeaderText = "IDCfe";
            this.colIDRetornoSAT.Name = "colIDRetornoSAT";
            this.colIDRetornoSAT.ReadOnly = true;
            this.colIDRetornoSAT.Visible = false;
            // 
            // colDtPedidoFechamento
            // 
            this.colDtPedidoFechamento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDtPedidoFechamento.DataPropertyName = "DtPedidoFechamento";
            this.colDtPedidoFechamento.FillWeight = 119.7969F;
            this.colDtPedidoFechamento.HeaderText = "Data";
            this.colDtPedidoFechamento.Name = "colDtPedidoFechamento";
            this.colDtPedidoFechamento.ReadOnly = true;
            this.colDtPedidoFechamento.Width = 72;
            // 
            // colChaveConsulta
            // 
            this.colChaveConsulta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colChaveConsulta.DataPropertyName = "ChaveConsulta";
            this.colChaveConsulta.FillWeight = 30.56087F;
            this.colChaveConsulta.HeaderText = "CF-e";
            this.colChaveConsulta.Name = "colChaveConsulta";
            this.colChaveConsulta.ReadOnly = true;
            // 
            // colDocumentoCliente
            // 
            this.colDocumentoCliente.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDocumentoCliente.DataPropertyName = "DocumentoCliente";
            this.colDocumentoCliente.FillWeight = 50F;
            this.colDocumentoCliente.HeaderText = "CPF";
            this.colDocumentoCliente.Name = "colDocumentoCliente";
            this.colDocumentoCliente.ReadOnly = true;
            this.colDocumentoCliente.Width = 71;
            // 
            // colValorTotal
            // 
            this.colValorTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colValorTotal.DataPropertyName = "ValorTotal";
            this.colValorTotal.FillWeight = 71F;
            this.colValorTotal.HeaderText = "Valor Pedido";
            this.colValorTotal.Name = "colValorTotal";
            this.colValorTotal.ReadOnly = true;
            this.colValorTotal.Width = 122;
            // 
            // colEmitir
            // 
            this.colEmitir.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colEmitir.DataPropertyName = "Emitir";
            this.colEmitir.FillWeight = 50F;
            this.colEmitir.HeaderText = "";
            this.colEmitir.Name = "colEmitir";
            this.colEmitir.ReadOnly = true;
            this.colEmitir.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEmitir.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colEmitir.Width = 27;
            // 
            // colExportar
            // 
            this.colExportar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colExportar.DataPropertyName = "Exportar";
            this.colExportar.FillWeight = 50F;
            this.colExportar.HeaderText = "";
            this.colExportar.Name = "colExportar";
            this.colExportar.ReadOnly = true;
            this.colExportar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colExportar.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colExportar.Width = 27;
            // 
            // colEnviar
            // 
            this.colEnviar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colEnviar.DataPropertyName = "Enviar";
            this.colEnviar.FillWeight = 50F;
            this.colEnviar.HeaderText = "";
            this.colEnviar.Name = "colEnviar";
            this.colEnviar.ReadOnly = true;
            this.colEnviar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEnviar.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colEnviar.Width = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 26);
            this.label1.TabIndex = 19;
            this.label1.Text = "Histórico Fiscal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 17);
            this.label2.TabIndex = 20;
            this.label2.Text = "De:";
            // 
            // dtpFiltroDe
            // 
            this.dtpFiltroDe.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtpFiltroDe.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFiltroDe.Location = new System.Drawing.Point(55, 53);
            this.dtpFiltroDe.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            this.dtpFiltroDe.Name = "dtpFiltroDe";
            this.dtpFiltroDe.Size = new System.Drawing.Size(226, 25);
            this.dtpFiltroDe.TabIndex = 1;
            this.dtpFiltroDe.ValueChanged += new System.EventHandler(this.dtpDe_ValueChanged);
            // 
            // txtBusca
            // 
            this.txtBusca.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBusca.Location = new System.Drawing.Point(730, 53);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(282, 25);
            this.txtBusca.TabIndex = 3;
            this.txtBusca.TextChanged += new System.EventHandler(this.txtCfe_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(574, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 17);
            this.label3.TabIndex = 23;
            this.label3.Text = "Chave ou CPF/CNPJ:";
            // 
            // dtpFiltroAte
            // 
            this.dtpFiltroAte.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtpFiltroAte.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFiltroAte.Location = new System.Drawing.Point(334, 53);
            this.dtpFiltroAte.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            this.dtpFiltroAte.Name = "dtpFiltroAte";
            this.dtpFiltroAte.Size = new System.Drawing.Size(226, 25);
            this.dtpFiltroAte.TabIndex = 2;
            this.dtpFiltroAte.ValueChanged += new System.EventHandler(this.dtpAte_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(288, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 17);
            this.label4.TabIndex = 26;
            this.label4.Text = "Até";
            // 
            // pnlCarregando
            // 
            this.pnlCarregando.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlCarregando.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlCarregando.Controls.Add(this.progressBar1);
            this.pnlCarregando.Controls.Add(this.label5);
            this.pnlCarregando.Location = new System.Drawing.Point(12, 491);
            this.pnlCarregando.Name = "pnlCarregando";
            this.pnlCarregando.Size = new System.Drawing.Size(263, 47);
            this.pnlCarregando.TabIndex = 27;
            this.pnlCarregando.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(4, 10);
            this.progressBar1.MarqueeAnimationSpeed = 200;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(151, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(159, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Carregando...";
            // 
            // btnEnviarCFe
            // 
            this.btnEnviarCFe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnviarCFe.Font = new System.Drawing.Font("Arial", 9F);
            this.btnEnviarCFe.Image = global::a7D.PDV.BackOffice.UI.Properties.Resources.ic_email_black_24dp_1x;
            this.btnEnviarCFe.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEnviarCFe.Location = new System.Drawing.Point(583, 491);
            this.btnEnviarCFe.Name = "btnEnviarCFe";
            this.btnEnviarCFe.Padding = new System.Windows.Forms.Padding(5);
            this.btnEnviarCFe.Size = new System.Drawing.Size(141, 47);
            this.btnEnviarCFe.TabIndex = 29;
            this.btnEnviarCFe.Text = "Enviar por Email";
            this.btnEnviarCFe.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEnviarCFe.UseVisualStyleBackColor = true;
            this.btnEnviarCFe.Click += new System.EventHandler(this.btnEnviarCFe_Click);
            // 
            // btnEmitirPendentes
            // 
            this.btnEmitirPendentes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEmitirPendentes.Font = new System.Drawing.Font("Arial", 9F);
            this.btnEmitirPendentes.Image = global::a7D.PDV.BackOffice.UI.Properties.Resources.sat32px;
            this.btnEmitirPendentes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEmitirPendentes.Location = new System.Drawing.Point(730, 491);
            this.btnEmitirPendentes.Name = "btnEmitirPendentes";
            this.btnEmitirPendentes.Padding = new System.Windows.Forms.Padding(5);
            this.btnEmitirPendentes.Size = new System.Drawing.Size(155, 47);
            this.btnEmitirPendentes.TabIndex = 28;
            this.btnEmitirPendentes.Text = "Emitir Pendentes";
            this.btnEmitirPendentes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEmitirPendentes.UseVisualStyleBackColor = true;
            this.btnEmitirPendentes.Click += new System.EventHandler(this.btnReemitirTodos_Click);
            // 
            // btnExportar
            // 
            this.btnExportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportar.Font = new System.Drawing.Font("Arial", 9F);
            this.btnExportar.Image = global::a7D.PDV.BackOffice.UI.Properties.Resources.ic_file_download_black_24dp_1x;
            this.btnExportar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportar.Location = new System.Drawing.Point(891, 491);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Padding = new System.Windows.Forms.Padding(5);
            this.btnExportar.Size = new System.Drawing.Size(122, 47);
            this.btnExportar.TabIndex = 5;
            this.btnExportar.Text = "Exportar XML";
            this.btnExportar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportar.UseVisualStyleBackColor = true;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // frmCFE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 550);
            this.Controls.Add(this.btnEnviarCFe);
            this.Controls.Add(this.btnEmitirPendentes);
            this.Controls.Add(this.pnlCarregando);
            this.Controls.Add(this.dtpFiltroAte);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBusca);
            this.Controls.Add(this.dtpFiltroDe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvPrincipal);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmCFE";
            this.Text = "CF-e";
            this.Load += new System.EventHandler(this.frmCFE_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrincipal)).EndInit();
            this.pnlCarregando.ResumeLayout(false);
            this.pnlCarregando.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPrincipal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFiltroDe;
        private System.Windows.Forms.TextBox txtBusca;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.DateTimePicker dtpFiltroAte;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlCarregando;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnEmitirPendentes;
        private System.Windows.Forms.Button btnEnviarCFe;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIDPedido;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIDTipoSolicitacaoSAT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIDRetornoSAT;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDtPedidoFechamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colChaveConsulta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentoCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValorTotal;
        private System.Windows.Forms.DataGridViewLinkColumn colEmitir;
        private System.Windows.Forms.DataGridViewLinkColumn colExportar;
        private System.Windows.Forms.DataGridViewLinkColumn colEnviar;
    }
}