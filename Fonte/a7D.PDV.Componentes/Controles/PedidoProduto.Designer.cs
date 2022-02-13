namespace a7D.PDV.Componentes.Controles
{
    partial class PedidoProduto
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

        #region Component Designer generated code

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PedidoProduto));
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.dgvItensSelecionados = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Notas = new System.Windows.Forms.DataGridViewImageColumn();
            this.Editavel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IDPedidoProduto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Viagem = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnlTitulo = new System.Windows.Forms.Panel();
            this.lblValorServico = new System.Windows.Forms.Label();
            this.lblValorProdutos = new System.Windows.Forms.Label();
            this.lblValorTotal = new System.Windows.Forms.Label();
            this.lblIdentificacao = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlTitulo2 = new System.Windows.Forms.Panel();
            this.btnViagem = new System.Windows.Forms.Button();
            this.lblTotalNovos = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItensSelecionados)).BeginInit();
            this.pnlTitulo.SuspendLayout();
            this.pnlTitulo2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnConfirmar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmar.ForeColor = System.Drawing.Color.White;
            this.btnConfirmar.Location = new System.Drawing.Point(-1, 437);
            this.btnConfirmar.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(536, 64);
            this.btnConfirmar.TabIndex = 80;
            this.btnConfirmar.TabStop = false;
            this.btnConfirmar.Text = "CONFIRMAR";
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // dgvItensSelecionados
            // 
            this.dgvItensSelecionados.AllowUserToAddRows = false;
            this.dgvItensSelecionados.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(240)))));
            this.dgvItensSelecionados.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvItensSelecionados.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItensSelecionados.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.dgvItensSelecionados.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItensSelecionados.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvItensSelecionados.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItensSelecionados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvItensSelecionados.ColumnHeadersHeight = 40;
            this.dgvItensSelecionados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Notas,
            this.Editavel,
            this.dataGridViewTextBoxColumn1,
            this.qtd,
            this.dataGridViewTextBoxColumn2,
            this.valorTotal,
            this.IDPedidoProduto,
            this.Viagem});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItensSelecionados.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvItensSelecionados.EnableHeadersVisualStyles = false;
            this.dgvItensSelecionados.Location = new System.Drawing.Point(0, 111);
            this.dgvItensSelecionados.Margin = new System.Windows.Forms.Padding(4);
            this.dgvItensSelecionados.MultiSelect = false;
            this.dgvItensSelecionados.Name = "dgvItensSelecionados";
            this.dgvItensSelecionados.ReadOnly = true;
            this.dgvItensSelecionados.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItensSelecionados.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvItensSelecionados.RowHeadersVisible = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle10.Padding = new System.Windows.Forms.Padding(5);
            this.dgvItensSelecionados.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvItensSelecionados.RowTemplate.DividerHeight = 1;
            this.dgvItensSelecionados.RowTemplate.Height = 36;
            this.dgvItensSelecionados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvItensSelecionados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItensSelecionados.Size = new System.Drawing.Size(535, 318);
            this.dgvItensSelecionados.TabIndex = 79;
            this.dgvItensSelecionados.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItensSelecionados_CellClick);
            this.dgvItensSelecionados.DoubleClick += new System.EventHandler(this.dgvItensSelecionados_DoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Image = global::a7D.PDV.Componentes.Properties.Resources.btnExcluir1;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 50;
            // 
            // Notas
            // 
            this.Notas.HeaderText = "";
            this.Notas.Image = global::a7D.PDV.Componentes.Properties.Resources.ico_anotacao;
            this.Notas.Name = "Notas";
            this.Notas.ReadOnly = true;
            this.Notas.Width = 50;
            // 
            // Editavel
            // 
            this.Editavel.HeaderText = "Editavel";
            this.Editavel.Name = "Editavel";
            this.Editavel.ReadOnly = true;
            this.Editavel.Visible = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "nome";
            this.dataGridViewTextBoxColumn1.HeaderText = "Descrição";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // qtd
            // 
            this.qtd.DataPropertyName = "quantidade";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.qtd.DefaultCellStyle = dataGridViewCellStyle3;
            this.qtd.HeaderText = "Qtd.";
            this.qtd.Name = "qtd";
            this.qtd.ReadOnly = true;
            this.qtd.Width = 70;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "valorUnitario";
            this.dataGridViewTextBoxColumn2.HeaderText = "Valor Unit.";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // valorTotal
            // 
            this.valorTotal.DataPropertyName = "valorTotal";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.valorTotal.DefaultCellStyle = dataGridViewCellStyle4;
            this.valorTotal.HeaderText = "Valor Total";
            this.valorTotal.Name = "valorTotal";
            this.valorTotal.ReadOnly = true;
            // 
            // IDPedidoProduto
            // 
            this.IDPedidoProduto.DataPropertyName = "IDPedidoProduto";
            this.IDPedidoProduto.HeaderText = "IDPedidoProduto";
            this.IDPedidoProduto.Name = "IDPedidoProduto";
            this.IDPedidoProduto.ReadOnly = true;
            this.IDPedidoProduto.Visible = false;
            this.IDPedidoProduto.Width = 40;
            // 
            // Viagem
            // 
            this.Viagem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Viagem.HeaderText = "Viagem";
            this.Viagem.Name = "Viagem";
            this.Viagem.ReadOnly = true;
            this.Viagem.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Viagem.Width = 70;
            // 
            // pnlTitulo
            // 
            this.pnlTitulo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.pnlTitulo.Controls.Add(this.lblValorServico);
            this.pnlTitulo.Controls.Add(this.lblValorProdutos);
            this.pnlTitulo.Controls.Add(this.lblValorTotal);
            this.pnlTitulo.Controls.Add(this.lblIdentificacao);
            this.pnlTitulo.Controls.Add(this.label5);
            this.pnlTitulo.Controls.Add(this.lblCliente);
            this.pnlTitulo.Controls.Add(this.label4);
            this.pnlTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlTitulo.Location = new System.Drawing.Point(-1, -1);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(536, 110);
            this.pnlTitulo.TabIndex = 81;
            // 
            // lblValorServico
            // 
            this.lblValorServico.AutoSize = true;
            this.lblValorServico.Location = new System.Drawing.Point(101, 86);
            this.lblValorServico.Name = "lblValorServico";
            this.lblValorServico.Size = new System.Drawing.Size(123, 22);
            this.lblValorServico.TabIndex = 76;
            this.lblValorServico.Text = "R$ 0,00 (0%)";
            // 
            // lblValorProdutos
            // 
            this.lblValorProdutos.AutoSize = true;
            this.lblValorProdutos.Location = new System.Drawing.Point(101, 61);
            this.lblValorProdutos.Name = "lblValorProdutos";
            this.lblValorProdutos.Size = new System.Drawing.Size(151, 22);
            this.lblValorProdutos.TabIndex = 75;
            this.lblValorProdutos.Text = "R$ 0,00 (0 itens)";
            // 
            // lblValorTotal
            // 
            this.lblValorTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblValorTotal.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblValorTotal.Location = new System.Drawing.Point(388, 6);
            this.lblValorTotal.Name = "lblValorTotal";
            this.lblValorTotal.Size = new System.Drawing.Size(148, 32);
            this.lblValorTotal.TabIndex = 74;
            this.lblValorTotal.Text = "R$ 0,00";
            this.lblValorTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblIdentificacao
            // 
            this.lblIdentificacao.AutoSize = true;
            this.lblIdentificacao.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdentificacao.Location = new System.Drawing.Point(3, 6);
            this.lblIdentificacao.Name = "lblIdentificacao";
            this.lblIdentificacao.Size = new System.Drawing.Size(165, 32);
            this.lblIdentificacao.TabIndex = 70;
            this.lblIdentificacao.Text = "COMANDA:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 22);
            this.label5.TabIndex = 73;
            this.label5.Text = "Produtos:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(5, 35);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(69, 22);
            this.lblCliente.TabIndex = 71;
            this.lblCliente.Text = "Cliente";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 22);
            this.label4.TabIndex = 72;
            this.label4.Text = "Serviço:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlTitulo2
            // 
            this.pnlTitulo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTitulo2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.pnlTitulo2.Controls.Add(this.btnViagem);
            this.pnlTitulo2.Controls.Add(this.lblTotalNovos);
            this.pnlTitulo2.Controls.Add(this.label7);
            this.pnlTitulo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlTitulo2.Location = new System.Drawing.Point(303, 55);
            this.pnlTitulo2.Name = "pnlTitulo2";
            this.pnlTitulo2.Size = new System.Drawing.Size(230, 54);
            this.pnlTitulo2.TabIndex = 82;
            // 
            // btnViagem
            // 
            this.btnViagem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViagem.BackColor = System.Drawing.Color.White;
            this.btnViagem.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnViagem.BackgroundImage")));
            this.btnViagem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViagem.Location = new System.Drawing.Point(179, 3);
            this.btnViagem.Name = "btnViagem";
            this.btnViagem.Size = new System.Drawing.Size(48, 48);
            this.btnViagem.TabIndex = 78;
            this.btnViagem.UseVisualStyleBackColor = false;
            this.btnViagem.Visible = false;
            this.btnViagem.Click += new System.EventHandler(this.btnViagem_Click);
            // 
            // lblTotalNovos
            // 
            this.lblTotalNovos.AutoSize = true;
            this.lblTotalNovos.Location = new System.Drawing.Point(3, 30);
            this.lblTotalNovos.Name = "lblTotalNovos";
            this.lblTotalNovos.Size = new System.Drawing.Size(59, 17);
            this.lblTotalNovos.TabIndex = 75;
            this.lblTotalNovos.Text = "R$ 0,00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 17);
            this.label7.TabIndex = 73;
            this.label7.Text = "Produtos a confrmar:";
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::a7D.PDV.Componentes.Properties.Resources.btnExcluir1;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = global::a7D.PDV.Componentes.Properties.Resources.ico_anotacao;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Width = 50;
            // 
            // PedidoProduto
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pnlTitulo2);
            this.Controls.Add(this.pnlTitulo);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.dgvItensSelecionados);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PedidoProduto";
            this.Size = new System.Drawing.Size(534, 500);
            this.Load += new System.EventHandler(this.PedidoProduto_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItensSelecionados)).EndInit();
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.pnlTitulo2.ResumeLayout(false);
            this.pnlTitulo2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.DataGridView dgvItensSelecionados;
        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblValorTotal;
        private System.Windows.Forms.Label lblIdentificacao;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblValorServico;
        private System.Windows.Forms.Label lblValorProdutos;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewImageColumn Column1;
        private System.Windows.Forms.DataGridViewImageColumn Notas;
        private System.Windows.Forms.DataGridViewTextBoxColumn Editavel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtd;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDPedidoProduto;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Viagem;
        private System.Windows.Forms.Panel pnlTitulo2;
        private System.Windows.Forms.Button btnViagem;
        private System.Windows.Forms.Label lblTotalNovos;
        private System.Windows.Forms.Label label7;
    }
}
