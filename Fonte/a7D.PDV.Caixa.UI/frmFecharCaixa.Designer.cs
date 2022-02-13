namespace a7D.PDV.Caixa.UI
{
    partial class frmFecharCaixa
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFecharCaixa));
            this.btnFecharCaixa = new System.Windows.Forms.Button();
            this.dgvValoresRecebidos = new System.Windows.Forms.DataGridView();
            this.IDCaixaValorRegistro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValorAbertura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValorFechamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValoresRecebidos)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFecharCaixa
            // 
            this.btnFecharCaixa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFecharCaixa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnFecharCaixa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFecharCaixa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFecharCaixa.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFecharCaixa.ForeColor = System.Drawing.Color.White;
            this.btnFecharCaixa.Location = new System.Drawing.Point(12, 467);
            this.btnFecharCaixa.Margin = new System.Windows.Forms.Padding(4);
            this.btnFecharCaixa.Name = "btnFecharCaixa";
            this.btnFecharCaixa.Size = new System.Drawing.Size(491, 59);
            this.btnFecharCaixa.TabIndex = 1;
            this.btnFecharCaixa.Text = "FECHAR CAIXA";
            this.btnFecharCaixa.UseVisualStyleBackColor = false;
            this.btnFecharCaixa.Click += new System.EventHandler(this.btnFecharCaixa_Click);
            // 
            // dgvValoresRecebidos
            // 
            this.dgvValoresRecebidos.AllowUserToAddRows = false;
            this.dgvValoresRecebidos.AllowUserToDeleteRows = false;
            this.dgvValoresRecebidos.AllowUserToResizeColumns = false;
            this.dgvValoresRecebidos.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(240)))));
            this.dgvValoresRecebidos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvValoresRecebidos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvValoresRecebidos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvValoresRecebidos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.dgvValoresRecebidos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvValoresRecebidos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvValoresRecebidos.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvValoresRecebidos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvValoresRecebidos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvValoresRecebidos.ColumnHeadersHeight = 40;
            this.dgvValoresRecebidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvValoresRecebidos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IDCaixaValorRegistro,
            this.Column1,
            this.ValorAbertura,
            this.ValorFechamento});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvValoresRecebidos.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvValoresRecebidos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvValoresRecebidos.EnableHeadersVisualStyles = false;
            this.dgvValoresRecebidos.Location = new System.Drawing.Point(12, 78);
            this.dgvValoresRecebidos.MultiSelect = false;
            this.dgvValoresRecebidos.Name = "dgvValoresRecebidos";
            this.dgvValoresRecebidos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvValoresRecebidos.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvValoresRecebidos.RowHeadersVisible = false;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(5);
            this.dgvValoresRecebidos.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvValoresRecebidos.RowTemplate.DividerHeight = 1;
            this.dgvValoresRecebidos.RowTemplate.Height = 36;
            this.dgvValoresRecebidos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvValoresRecebidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvValoresRecebidos.Size = new System.Drawing.Size(491, 379);
            this.dgvValoresRecebidos.TabIndex = 0;
            this.dgvValoresRecebidos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvValoresRecebidos_CellClick);
            this.dgvValoresRecebidos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvValoresRecebidos_CellContentClick);
            // 
            // IDCaixaValorRegistro
            // 
            this.IDCaixaValorRegistro.DataPropertyName = "IDCaixaValorRegistro";
            this.IDCaixaValorRegistro.HeaderText = "IDTipoPagamento";
            this.IDCaixaValorRegistro.Name = "IDCaixaValorRegistro";
            this.IDCaixaValorRegistro.Visible = false;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.DataPropertyName = "Nome";
            this.Column1.FillWeight = 56.54663F;
            this.Column1.HeaderText = "Forma de Pagamento";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // ValorAbertura
            // 
            this.ValorAbertura.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ValorAbertura.DataPropertyName = "ValorAbertura";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Format = "C2";
            dataGridViewCellStyle3.NullValue = null;
            this.ValorAbertura.DefaultCellStyle = dataGridViewCellStyle3;
            this.ValorAbertura.HeaderText = "Abertura (R$)";
            this.ValorAbertura.Name = "ValorAbertura";
            this.ValorAbertura.ReadOnly = true;
            this.ValorAbertura.Width = 120;
            // 
            // ValorFechamento
            // 
            this.ValorFechamento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle4.Format = "C2";
            this.ValorFechamento.DefaultCellStyle = dataGridViewCellStyle4;
            this.ValorFechamento.HeaderText = "Fechamento (R$)";
            this.ValorFechamento.Name = "ValorFechamento";
            this.ValorFechamento.Width = 140;
            // 
            // frmFecharCaixa
            // 
            this.AcceptButton = this.btnFecharCaixa;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(515, 535);
            this.Controls.Add(this.btnFecharCaixa);
            this.Controls.Add(this.dgvValoresRecebidos);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFecharCaixa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FECHAR CAIXA";
            this.Load += new System.EventHandler(this.frmFecharCaixaTurno_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvValoresRecebidos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFecharCaixa;
        private System.Windows.Forms.DataGridView dgvValoresRecebidos;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDCaixaValorRegistro;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValorAbertura;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValorFechamento;
    }
}