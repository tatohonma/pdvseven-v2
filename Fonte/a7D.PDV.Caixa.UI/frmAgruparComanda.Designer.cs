namespace a7D.PDV.Caixa.UI
{
    partial class frmAgruparComanda
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.txtCodigoComanda = new System.Windows.Forms.TextBox();
            this.btnPagamento = new System.Windows.Forms.Button();
            this.dgvComandas = new System.Windows.Forms.DataGridView();
            this.remover = new System.Windows.Forms.DataGridViewImageColumn();
            this.Comanda = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtdItens = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValorTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GUIDIdentificacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblValorTotal = new System.Windows.Forms.Label();
            this.gbxValorPendente = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblConsumacao = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblProdutos = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComandas)).BeginInit();
            this.gbxValorPendente.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnAdicionar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAdicionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionar.ForeColor = System.Drawing.Color.White;
            this.btnAdicionar.Location = new System.Drawing.Point(149, 78);
            this.btnAdicionar.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(128, 28);
            this.btnAdicionar.TabIndex = 1;
            this.btnAdicionar.Text = "ADICIONAR";
            this.btnAdicionar.UseVisualStyleBackColor = false;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // txtCodigoComanda
            // 
            this.txtCodigoComanda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtCodigoComanda.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCodigoComanda.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoComanda.Location = new System.Drawing.Point(13, 78);
            this.txtCodigoComanda.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigoComanda.MaxLength = 16;
            this.txtCodigoComanda.Name = "txtCodigoComanda";
            this.txtCodigoComanda.Size = new System.Drawing.Size(128, 28);
            this.txtCodigoComanda.TabIndex = 0;
            this.txtCodigoComanda.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodigoComanda_KeyDown);
            this.txtCodigoComanda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCodigoComanda_KeyPress);
            // 
            // btnPagamento
            // 
            this.btnPagamento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPagamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnPagamento.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPagamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagamento.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPagamento.ForeColor = System.Drawing.Color.White;
            this.btnPagamento.Location = new System.Drawing.Point(773, 473);
            this.btnPagamento.Margin = new System.Windows.Forms.Padding(4);
            this.btnPagamento.Name = "btnPagamento";
            this.btnPagamento.Size = new System.Drawing.Size(192, 54);
            this.btnPagamento.TabIndex = 3;
            this.btnPagamento.Text = "&AGRUPAR";
            this.btnPagamento.UseVisualStyleBackColor = false;
            this.btnPagamento.Click += new System.EventHandler(this.btnPagamento_Click);
            // 
            // dgvComandas
            // 
            this.dgvComandas.AllowUserToAddRows = false;
            this.dgvComandas.AllowUserToDeleteRows = false;
            this.dgvComandas.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(240)))));
            this.dgvComandas.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvComandas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvComandas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvComandas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvComandas.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.dgvComandas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvComandas.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvComandas.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvComandas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvComandas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvComandas.ColumnHeadersHeight = 40;
            this.dgvComandas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.remover,
            this.Comanda,
            this.Cliente,
            this.QtdItens,
            this.ValorTotal,
            this.GUIDIdentificacao});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvComandas.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvComandas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvComandas.EnableHeadersVisualStyles = false;
            this.dgvComandas.Location = new System.Drawing.Point(13, 114);
            this.dgvComandas.Margin = new System.Windows.Forms.Padding(4);
            this.dgvComandas.Name = "dgvComandas";
            this.dgvComandas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvComandas.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvComandas.RowHeadersVisible = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(5);
            this.dgvComandas.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvComandas.RowTemplate.DividerHeight = 1;
            this.dgvComandas.RowTemplate.Height = 36;
            this.dgvComandas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvComandas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComandas.Size = new System.Drawing.Size(952, 351);
            this.dgvComandas.TabIndex = 2;
            this.dgvComandas.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComandas_CellContentClick);
            this.dgvComandas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComandas_CellDoubleClick);
            // 
            // remover
            // 
            this.remover.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.remover.HeaderText = "";
            this.remover.Image = global::a7D.PDV.Caixa.UI.Properties.Resources.btnExcluir1;
            this.remover.Name = "remover";
            this.remover.Width = 11;
            // 
            // Comanda
            // 
            this.Comanda.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Comanda.DefaultCellStyle = dataGridViewCellStyle3;
            this.Comanda.HeaderText = "Comanda";
            this.Comanda.Name = "Comanda";
            this.Comanda.Width = 105;
            // 
            // Cliente
            // 
            this.Cliente.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Cliente.HeaderText = "Cliente";
            this.Cliente.Name = "Cliente";
            // 
            // QtdItens
            // 
            this.QtdItens.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.QtdItens.DefaultCellStyle = dataGridViewCellStyle4;
            this.QtdItens.HeaderText = "Qtd Itens";
            this.QtdItens.Name = "QtdItens";
            // 
            // ValorTotal
            // 
            this.ValorTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Format = "#,##0.00";
            dataGridViewCellStyle5.NullValue = "0,00";
            this.ValorTotal.DefaultCellStyle = dataGridViewCellStyle5;
            this.ValorTotal.HeaderText = "Valor total (R$)";
            this.ValorTotal.Name = "ValorTotal";
            this.ValorTotal.Width = 137;
            // 
            // GUIDIdentificacao
            // 
            this.GUIDIdentificacao.HeaderText = "GUIDIdentificacao";
            this.GUIDIdentificacao.Name = "GUIDIdentificacao";
            this.GUIDIdentificacao.Visible = false;
            // 
            // lblValorTotal
            // 
            this.lblValorTotal.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblValorTotal.Location = new System.Drawing.Point(28, 22);
            this.lblValorTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValorTotal.Name = "lblValorTotal";
            this.lblValorTotal.Size = new System.Drawing.Size(117, 21);
            this.lblValorTotal.TabIndex = 5;
            this.lblValorTotal.Text = "0,00";
            this.lblValorTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxValorPendente
            // 
            this.gbxValorPendente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxValorPendente.Controls.Add(this.lblValorTotal);
            this.gbxValorPendente.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxValorPendente.Location = new System.Drawing.Point(612, 471);
            this.gbxValorPendente.Margin = new System.Windows.Forms.Padding(4);
            this.gbxValorPendente.Name = "gbxValorPendente";
            this.gbxValorPendente.Padding = new System.Windows.Forms.Padding(4);
            this.gbxValorPendente.Size = new System.Drawing.Size(153, 55);
            this.gbxValorPendente.TabIndex = 84;
            this.gbxValorPendente.TabStop = false;
            this.gbxValorPendente.Text = "Total";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.lblConsumacao);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 473);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(153, 55);
            this.groupBox1.TabIndex = 85;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Consum. Mín.";
            // 
            // lblConsumacao
            // 
            this.lblConsumacao.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConsumacao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblConsumacao.Location = new System.Drawing.Point(28, 22);
            this.lblConsumacao.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConsumacao.Name = "lblConsumacao";
            this.lblConsumacao.Size = new System.Drawing.Size(117, 21);
            this.lblConsumacao.TabIndex = 5;
            this.lblConsumacao.Text = "0,00";
            this.lblConsumacao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.lblProdutos);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(174, 473);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(153, 55);
            this.groupBox2.TabIndex = 85;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Total Produtos";
            // 
            // lblProdutos
            // 
            this.lblProdutos.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProdutos.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblProdutos.Location = new System.Drawing.Point(28, 22);
            this.lblProdutos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProdutos.Name = "lblProdutos";
            this.lblProdutos.Size = new System.Drawing.Size(117, 21);
            this.lblProdutos.TabIndex = 5;
            this.lblProdutos.Text = "0,00";
            this.lblProdutos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmAgruparComanda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(978, 540);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbxValorPendente);
            this.Controls.Add(this.dgvComandas);
            this.Controls.Add(this.btnPagamento);
            this.Controls.Add(this.btnAdicionar);
            this.Controls.Add(this.txtCodigoComanda);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAgruparComanda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AGRUPAR COMANDAS";
            this.Load += new System.EventHandler(this.frmAgruparComanda_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComandas)).EndInit();
            this.gbxValorPendente.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.TextBox txtCodigoComanda;
        private System.Windows.Forms.Button btnPagamento;
        private System.Windows.Forms.DataGridView dgvComandas;
        private System.Windows.Forms.Label lblValorTotal;
        private System.Windows.Forms.GroupBox gbxValorPendente;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblConsumacao;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblProdutos;
        private System.Windows.Forms.DataGridViewImageColumn remover;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comanda;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn QtdItens;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValorTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn GUIDIdentificacao;
    }
}