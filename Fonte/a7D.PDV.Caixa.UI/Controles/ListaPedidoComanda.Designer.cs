namespace a7D.PDV.Caixa.UI.Controles
{
    partial class ListaPedidoComanda
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvComandas = new System.Windows.Forms.DataGridView();
            this.Numero = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NomeExibicao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NomeCompleto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.guidIdentificacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IDStatusComanda = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bloqueado = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.txtPesquisarComanda = new System.Windows.Forms.TextBox();
            this.btnAbrirComanda = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.txtCodigoComanda = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComandas)).BeginInit();
            this.SuspendLayout();
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
            this.Numero,
            this.NomeExibicao,
            this.NomeCompleto,
            this.guidIdentificacao,
            this.IDStatusComanda,
            this.Bloqueado});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvComandas.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvComandas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvComandas.EnableHeadersVisualStyles = false;
            this.dgvComandas.Location = new System.Drawing.Point(0, 93);
            this.dgvComandas.Margin = new System.Windows.Forms.Padding(5);
            this.dgvComandas.MultiSelect = false;
            this.dgvComandas.Name = "dgvComandas";
            this.dgvComandas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvComandas.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvComandas.RowHeadersVisible = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(5);
            this.dgvComandas.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvComandas.RowTemplate.DividerHeight = 1;
            this.dgvComandas.RowTemplate.Height = 36;
            this.dgvComandas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvComandas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComandas.Size = new System.Drawing.Size(696, 342);
            this.dgvComandas.TabIndex = 3;
            this.dgvComandas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComandas_CellClick);
            this.dgvComandas.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComandas_CellDoubleClick);
            // 
            // Numero
            // 
            this.Numero.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Numero.DataPropertyName = "Numero";
            this.Numero.FillWeight = 50F;
            this.Numero.HeaderText = "Comanda";
            this.Numero.Name = "Numero";
            this.Numero.ReadOnly = true;
            // 
            // NomeExibicao
            // 
            this.NomeExibicao.DataPropertyName = "NomeCompleto";
            this.NomeExibicao.HeaderText = "Cliente";
            this.NomeExibicao.Name = "NomeExibicao";
            this.NomeExibicao.ReadOnly = true;
            // 
            // NomeCompleto
            // 
            this.NomeCompleto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NomeCompleto.DataPropertyName = "NomeCompleto";
            this.NomeCompleto.HeaderText = "NomeCompleto";
            this.NomeCompleto.Name = "NomeCompleto";
            this.NomeCompleto.ReadOnly = true;
            this.NomeCompleto.Visible = false;
            // 
            // guidIdentificacao
            // 
            this.guidIdentificacao.DataPropertyName = "guidIdentificacao";
            this.guidIdentificacao.HeaderText = "guidIdentificacao";
            this.guidIdentificacao.Name = "guidIdentificacao";
            this.guidIdentificacao.ReadOnly = true;
            this.guidIdentificacao.Visible = false;
            // 
            // IDStatusComanda
            // 
            this.IDStatusComanda.DataPropertyName = "IDStatusComanda";
            this.IDStatusComanda.HeaderText = "IDStatusComanda";
            this.IDStatusComanda.Name = "IDStatusComanda";
            this.IDStatusComanda.ReadOnly = true;
            this.IDStatusComanda.Visible = false;
            // 
            // Bloqueado
            // 
            this.Bloqueado.HeaderText = "Bloqueado";
            this.Bloqueado.Name = "Bloqueado";
            this.Bloqueado.ReadOnly = true;
            this.Bloqueado.Visible = false;
            // 
            // txtPesquisarComanda
            // 
            this.txtPesquisarComanda.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPesquisarComanda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtPesquisarComanda.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPesquisarComanda.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPesquisarComanda.Location = new System.Drawing.Point(476, 53);
            this.txtPesquisarComanda.Margin = new System.Windows.Forms.Padding(5);
            this.txtPesquisarComanda.Name = "txtPesquisarComanda";
            this.txtPesquisarComanda.Size = new System.Drawing.Size(220, 28);
            this.txtPesquisarComanda.TabIndex = 2;
            this.txtPesquisarComanda.TextChanged += new System.EventHandler(this.txtPesquisarComanda_TextChanged);
            // 
            // btnAbrirComanda
            // 
            this.btnAbrirComanda.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbrirComanda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnAbrirComanda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirComanda.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbrirComanda.ForeColor = System.Drawing.Color.White;
            this.btnAbrirComanda.Location = new System.Drawing.Point(0, 0);
            this.btnAbrirComanda.Margin = new System.Windows.Forms.Padding(5);
            this.btnAbrirComanda.Name = "btnAbrirComanda";
            this.btnAbrirComanda.Size = new System.Drawing.Size(696, 43);
            this.btnAbrirComanda.TabIndex = 0;
            this.btnAbrirComanda.Text = "&ABRIR COMANDA";
            this.btnAbrirComanda.UseVisualStyleBackColor = false;
            this.btnAbrirComanda.Click += new System.EventHandler(this.btnAbrirComanda_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn1.FillWeight = 40F;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::a7D.PDV.Caixa.UI.Properties.Resources.btnExcluir1;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // txtCodigoComanda
            // 
            this.txtCodigoComanda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtCodigoComanda.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCodigoComanda.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoComanda.Location = new System.Drawing.Point(169, 53);
            this.txtCodigoComanda.Margin = new System.Windows.Forms.Padding(5);
            this.txtCodigoComanda.MaxLength = 16;
            this.txtCodigoComanda.Name = "txtCodigoComanda";
            this.txtCodigoComanda.Size = new System.Drawing.Size(149, 28);
            this.txtCodigoComanda.TabIndex = 1;
            this.txtCodigoComanda.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodigoComanda_KeyDown);
            this.txtCodigoComanda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCodigoComanda_KeyPress);
            this.txtCodigoComanda.Leave += new System.EventHandler(this.txtCodigoComanda_Leave);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 53);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 28);
            this.label4.TabIndex = 76;
            this.label4.Text = "Número comanda";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(342, 53);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 28);
            this.label1.TabIndex = 77;
            this.label1.Text = "Nome cliente";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ListaPedidoComanda
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCodigoComanda);
            this.Controls.Add(this.btnAbrirComanda);
            this.Controls.Add(this.txtPesquisarComanda);
            this.Controls.Add(this.dgvComandas);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ListaPedidoComanda";
            this.Size = new System.Drawing.Size(696, 433);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComandas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvComandas;
        private System.Windows.Forms.TextBox txtPesquisarComanda;
        private System.Windows.Forms.Button btnAbrirComanda;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.TextBox txtCodigoComanda;
        private System.Windows.Forms.DataGridViewTextBoxColumn Numero;
        private System.Windows.Forms.DataGridViewTextBoxColumn NomeExibicao;
        private System.Windows.Forms.DataGridViewTextBoxColumn NomeCompleto;
        private System.Windows.Forms.DataGridViewTextBoxColumn guidIdentificacao;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDStatusComanda;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Bloqueado;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
    }
}
