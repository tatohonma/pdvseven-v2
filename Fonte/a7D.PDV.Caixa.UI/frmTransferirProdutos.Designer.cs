namespace a7D.PDV.Caixa.UI
{
    partial class frmTransferirProdutos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTransferirProdutos));
            this.label1 = new System.Windows.Forms.Label();
            this.lblIdentificacao = new System.Windows.Forms.Label();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.rdbTotal = new System.Windows.Forms.RadioButton();
            this.rdbParcial = new System.Windows.Forms.RadioButton();
            this.dgvItens = new System.Windows.Forms.DataGridView();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menos = new System.Windows.Forms.DataGridViewButtonColumn();
            this.qtdTransferir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mais = new System.Windows.Forms.DataGridViewButtonColumn();
            this.IDPedidoProduto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnTransferir = new System.Windows.Forms.Button();
            this.cbbTipoPedido = new System.Windows.Forms.ComboBox();
            this.txtDestino = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 17);
            this.label1.TabIndex = 36;
            this.label1.Text = "para";
            // 
            // lblIdentificacao
            // 
            this.lblIdentificacao.AutoSize = true;
            this.lblIdentificacao.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdentificacao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblIdentificacao.Location = new System.Drawing.Point(13, 74);
            this.lblIdentificacao.Name = "lblIdentificacao";
            this.lblIdentificacao.Size = new System.Drawing.Size(21, 29);
            this.lblIdentificacao.TabIndex = 39;
            this.lblIdentificacao.Text = "-";
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::a7D.PDV.Caixa.UI.Properties.Resources.btnExcluir1;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // rdbTotal
            // 
            this.rdbTotal.AutoSize = true;
            this.rdbTotal.Location = new System.Drawing.Point(13, 174);
            this.rdbTotal.Name = "rdbTotal";
            this.rdbTotal.Size = new System.Drawing.Size(56, 21);
            this.rdbTotal.TabIndex = 2;
            this.rdbTotal.Text = "Total";
            this.rdbTotal.UseVisualStyleBackColor = true;
            this.rdbTotal.CheckedChanged += new System.EventHandler(this.rdbTotal_CheckedChanged);
            // 
            // rdbParcial
            // 
            this.rdbParcial.AutoSize = true;
            this.rdbParcial.Checked = true;
            this.rdbParcial.Location = new System.Drawing.Point(78, 174);
            this.rdbParcial.Name = "rdbParcial";
            this.rdbParcial.Size = new System.Drawing.Size(71, 21);
            this.rdbParcial.TabIndex = 3;
            this.rdbParcial.TabStop = true;
            this.rdbParcial.Text = "Parcial";
            this.rdbParcial.UseVisualStyleBackColor = true;
            this.rdbParcial.CheckedChanged += new System.EventHandler(this.rdbParcial_CheckedChanged);
            // 
            // dgvItens
            // 
            this.dgvItens.AllowUserToAddRows = false;
            this.dgvItens.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(240)))));
            this.dgvItens.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItens.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvItens.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.dgvItens.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItens.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvItens.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItens.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvItens.ColumnHeadersHeight = 40;
            this.dgvItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.descricao,
            this.qtd,
            this.menos,
            this.qtdTransferir,
            this.mais,
            this.IDPedidoProduto});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItens.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItens.EnableHeadersVisualStyles = false;
            this.dgvItens.Location = new System.Drawing.Point(13, 230);
            this.dgvItens.Margin = new System.Windows.Forms.Padding(4);
            this.dgvItens.MultiSelect = false;
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItens.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvItens.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(5);
            this.dgvItens.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvItens.RowTemplate.DividerHeight = 1;
            this.dgvItens.RowTemplate.Height = 36;
            this.dgvItens.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvItens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItens.Size = new System.Drawing.Size(948, 279);
            this.dgvItens.TabIndex = 4;
            this.dgvItens.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItens_CellContentClick);
            // 
            // descricao
            // 
            this.descricao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descricao.DataPropertyName = "nome";
            this.descricao.HeaderText = "Descrição";
            this.descricao.Name = "descricao";
            // 
            // qtd
            // 
            this.qtd.DataPropertyName = "quantidade";
            this.qtd.HeaderText = "Qtd. total";
            this.qtd.Name = "qtd";
            this.qtd.Width = 90;
            // 
            // menos
            // 
            this.menos.HeaderText = "";
            this.menos.Name = "menos";
            this.menos.Text = "-";
            this.menos.Width = 50;
            // 
            // qtdTransferir
            // 
            this.qtdTransferir.DataPropertyName = "quantidade";
            this.qtdTransferir.HeaderText = "Qtd. transferir";
            this.qtdTransferir.Name = "qtdTransferir";
            this.qtdTransferir.Width = 120;
            // 
            // mais
            // 
            this.mais.HeaderText = "";
            this.mais.Name = "mais";
            this.mais.Text = "+";
            this.mais.Width = 50;
            // 
            // IDPedidoProduto
            // 
            this.IDPedidoProduto.DataPropertyName = "IDPedidoProduto";
            this.IDPedidoProduto.HeaderText = "IDPedidoProduto";
            this.IDPedidoProduto.Name = "IDPedidoProduto";
            this.IDPedidoProduto.Visible = false;
            this.IDPedidoProduto.Width = 40;
            // 
            // btnTransferir
            // 
            this.btnTransferir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransferir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnTransferir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransferir.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransferir.ForeColor = System.Drawing.Color.White;
            this.btnTransferir.Location = new System.Drawing.Point(841, 517);
            this.btnTransferir.Margin = new System.Windows.Forms.Padding(4);
            this.btnTransferir.Name = "btnTransferir";
            this.btnTransferir.Size = new System.Drawing.Size(120, 47);
            this.btnTransferir.TabIndex = 5;
            this.btnTransferir.Text = "Transferir";
            this.btnTransferir.UseVisualStyleBackColor = false;
            this.btnTransferir.Click += new System.EventHandler(this.btnTransferir_Click);
            // 
            // cbbTipoPedido
            // 
            this.cbbTipoPedido.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbbTipoPedido.DisplayMember = "Nome";
            this.cbbTipoPedido.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTipoPedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbTipoPedido.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbTipoPedido.FormattingEnabled = true;
            this.cbbTipoPedido.Location = new System.Drawing.Point(12, 141);
            this.cbbTipoPedido.Name = "cbbTipoPedido";
            this.cbbTipoPedido.Size = new System.Drawing.Size(108, 27);
            this.cbbTipoPedido.TabIndex = 0;
            this.cbbTipoPedido.ValueMember = "IDTipoPedido";
            // 
            // txtDestino
            // 
            this.txtDestino.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDestino.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDestino.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDestino.Location = new System.Drawing.Point(127, 140);
            this.txtDestino.Margin = new System.Windows.Forms.Padding(4);
            this.txtDestino.MaxLength = 16;
            this.txtDestino.Name = "txtDestino";
            this.txtDestino.Size = new System.Drawing.Size(142, 28);
            this.txtDestino.TabIndex = 1;
            this.txtDestino.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ApenasNumero_KeyPress);
            this.txtDestino.Leave += new System.EventHandler(this.txtDestino_Leave);
            // 
            // frmTransferirProdutos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(974, 577);
            this.Controls.Add(this.txtDestino);
            this.Controls.Add(this.cbbTipoPedido);
            this.Controls.Add(this.btnTransferir);
            this.Controls.Add(this.dgvItens);
            this.Controls.Add(this.rdbParcial);
            this.Controls.Add(this.rdbTotal);
            this.Controls.Add(this.lblIdentificacao);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTransferirProdutos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TRANSFERIR ITEM";
            this.Load += new System.EventHandler(this.frmTransferirProdutos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblIdentificacao;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.RadioButton rdbTotal;
        private System.Windows.Forms.RadioButton rdbParcial;
        private System.Windows.Forms.DataGridView dgvItens;
        private System.Windows.Forms.Button btnTransferir;
        private System.Windows.Forms.ComboBox cbbTipoPedido;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtd;
        private System.Windows.Forms.DataGridViewButtonColumn menos;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtdTransferir;
        private System.Windows.Forms.DataGridViewButtonColumn mais;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDPedidoProduto;
        private System.Windows.Forms.TextBox txtDestino;
    }
}