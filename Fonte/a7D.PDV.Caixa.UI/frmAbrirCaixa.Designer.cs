namespace a7D.PDV.Caixa.UI
{
    partial class frmAbrirCaixa
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbrirCaixa));
            this.dgvValoresRecebidos = new System.Windows.Forms.DataGridView();
            this.IDTipoPagamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAbrirCaixa = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvValoresRecebidos)).BeginInit();
            this.SuspendLayout();
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
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvValoresRecebidos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvValoresRecebidos.ColumnHeadersHeight = 40;
            this.dgvValoresRecebidos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IDTipoPagamento,
            this.Column1,
            this.Valor});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvValoresRecebidos.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvValoresRecebidos.EnableHeadersVisualStyles = false;
            this.dgvValoresRecebidos.Location = new System.Drawing.Point(12, 78);
            this.dgvValoresRecebidos.Name = "dgvValoresRecebidos";
            this.dgvValoresRecebidos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvValoresRecebidos.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvValoresRecebidos.RowHeadersVisible = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(5);
            this.dgvValoresRecebidos.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvValoresRecebidos.RowTemplate.DividerHeight = 1;
            this.dgvValoresRecebidos.RowTemplate.Height = 36;
            this.dgvValoresRecebidos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvValoresRecebidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvValoresRecebidos.Size = new System.Drawing.Size(490, 375);
            this.dgvValoresRecebidos.TabIndex = 0;
            // 
            // IDTipoPagamento
            // 
            this.IDTipoPagamento.DataPropertyName = "IDTipoPagamento";
            this.IDTipoPagamento.HeaderText = "IDTipoPagamento";
            this.IDTipoPagamento.Name = "IDTipoPagamento";
            this.IDTipoPagamento.Visible = false;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.DataPropertyName = "Nome";
            this.Column1.HeaderText = "Forma de Pagamento";
            this.Column1.Name = "Column1";
            this.Column1.Width = 300;
            // 
            // Valor
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.Valor.DefaultCellStyle = dataGridViewCellStyle3;
            this.Valor.HeaderText = "Valor (R$)";
            this.Valor.Name = "Valor";
            // 
            // btnAbrirCaixa
            // 
            this.btnAbrirCaixa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbrirCaixa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnAbrirCaixa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAbrirCaixa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirCaixa.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbrirCaixa.ForeColor = System.Drawing.Color.White;
            this.btnAbrirCaixa.Location = new System.Drawing.Point(12, 460);
            this.btnAbrirCaixa.Margin = new System.Windows.Forms.Padding(4);
            this.btnAbrirCaixa.Name = "btnAbrirCaixa";
            this.btnAbrirCaixa.Size = new System.Drawing.Size(491, 59);
            this.btnAbrirCaixa.TabIndex = 1;
            this.btnAbrirCaixa.Text = "ABRIR CAIXA";
            this.btnAbrirCaixa.UseVisualStyleBackColor = false;
            this.btnAbrirCaixa.Click += new System.EventHandler(this.btnAbrirCaixa_Click);
            // 
            // frmAbrirCaixa
            // 
            this.AcceptButton = this.btnAbrirCaixa;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(514, 532);
            this.Controls.Add(this.btnAbrirCaixa);
            this.Controls.Add(this.dgvValoresRecebidos);
            this.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbrirCaixa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ABRIR CAIXA";
            this.Load += new System.EventHandler(this.frmAbrirCaixa_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvValoresRecebidos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgvValoresRecebidos;
        private System.Windows.Forms.Button btnAbrirCaixa;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDTipoPagamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
    }
}