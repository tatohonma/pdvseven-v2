namespace a7D.PDV.BackOffice.UI
{
    partial class frmInventarioEditar
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
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.txtProdutos = new System.Windows.Forms.TextBox();
            this.btnTodosProdutos = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdicionarTodos = new System.Windows.Forms.Button();
            this.dgvPrincipal = new System.Windows.Forms.DataGridView();
            this.btRemover = new System.Windows.Forms.DataGridViewImageColumn();
            this.IDInventarioProdutos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IDProduto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NovaUnidade = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.NovaQuantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtpData = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrincipal)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(146, 632);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(118, 53);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(22, 632);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(118, 53);
            this.btnSalvar.TabIndex = 3;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // txtProdutos
            // 
            this.txtProdutos.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtProdutos.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtProdutos.Location = new System.Drawing.Point(6, 28);
            this.txtProdutos.MaxLength = 300;
            this.txtProdutos.Name = "txtProdutos";
            this.txtProdutos.Size = new System.Drawing.Size(667, 29);
            this.txtProdutos.TabIndex = 1;
            this.txtProdutos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProdutos_KeyDown);
            this.txtProdutos.Leave += new System.EventHandler(this.txtProdutos_Leave);
            // 
            // btnTodosProdutos
            // 
            this.btnTodosProdutos.Location = new System.Drawing.Point(679, 28);
            this.btnTodosProdutos.Name = "btnTodosProdutos";
            this.btnTodosProdutos.Size = new System.Drawing.Size(186, 29);
            this.btnTodosProdutos.TabIndex = 2;
            this.btnTodosProdutos.Text = "Pesquisar...";
            this.btnTodosProdutos.UseVisualStyleBackColor = true;
            this.btnTodosProdutos.Click += new System.EventHandler(this.btnTodosProdutos_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAdicionarTodos);
            this.groupBox1.Controls.Add(this.btnTodosProdutos);
            this.groupBox1.Controls.Add(this.dgvPrincipal);
            this.groupBox1.Controls.Add(this.txtProdutos);
            this.groupBox1.Location = new System.Drawing.Point(16, 126);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1063, 500);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Produtos";
            // 
            // btnAdicionarTodos
            // 
            this.btnAdicionarTodos.Location = new System.Drawing.Point(871, 28);
            this.btnAdicionarTodos.Name = "btnAdicionarTodos";
            this.btnAdicionarTodos.Size = new System.Drawing.Size(186, 29);
            this.btnAdicionarTodos.TabIndex = 3;
            this.btnAdicionarTodos.Text = "Adicionar todos";
            this.btnAdicionarTodos.UseVisualStyleBackColor = true;
            this.btnAdicionarTodos.Click += new System.EventHandler(this.btnAdicionarTodos_Click);
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
            this.btRemover,
            this.IDInventarioProdutos,
            this.IDProduto,
            this.Nome,
            this.Unidade,
            this.Quantidade,
            this.NovaUnidade,
            this.NovaQuantidade});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPrincipal.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvPrincipal.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvPrincipal.EnableHeadersVisualStyles = false;
            this.dgvPrincipal.Location = new System.Drawing.Point(6, 63);
            this.dgvPrincipal.Name = "dgvPrincipal";
            this.dgvPrincipal.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrincipal.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvPrincipal.RowHeadersVisible = false;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle10.Padding = new System.Windows.Forms.Padding(5);
            this.dgvPrincipal.RowsDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvPrincipal.RowTemplate.DividerHeight = 1;
            this.dgvPrincipal.RowTemplate.Height = 36;
            this.dgvPrincipal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvPrincipal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvPrincipal.Size = new System.Drawing.Size(1051, 431);
            this.dgvPrincipal.TabIndex = 0;
            this.dgvPrincipal.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrincipal_CellContentClick);
            this.dgvPrincipal.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrincipal_CellEndEdit);
            this.dgvPrincipal.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrincipal_CellLeave);
            // 
            // btRemover
            // 
            this.btRemover.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btRemover.HeaderText = "";
            this.btRemover.Image = global::a7D.PDV.BackOffice.UI.Properties.Resources.btnExcluir1;
            this.btRemover.Name = "btRemover";
            this.btRemover.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btRemover.Width = 11;
            // 
            // IDInventarioProdutos
            // 
            this.IDInventarioProdutos.DataPropertyName = "IDInventarioProdutos";
            this.IDInventarioProdutos.HeaderText = "IDInventarioProdutos";
            this.IDInventarioProdutos.Name = "IDInventarioProdutos";
            this.IDInventarioProdutos.ReadOnly = true;
            this.IDInventarioProdutos.Visible = false;
            // 
            // IDProduto
            // 
            this.IDProduto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.IDProduto.DataPropertyName = "IDProduto";
            this.IDProduto.HeaderText = "IDProduto";
            this.IDProduto.Name = "IDProduto";
            this.IDProduto.Visible = false;
            // 
            // Nome
            // 
            this.Nome.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Nome.DataPropertyName = "Nome";
            this.Nome.HeaderText = "Produto";
            this.Nome.Name = "Nome";
            this.Nome.ReadOnly = true;
            // 
            // Unidade
            // 
            this.Unidade.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Unidade.DataPropertyName = "Unidade";
            this.Unidade.HeaderText = "Unidade";
            this.Unidade.Name = "Unidade";
            this.Unidade.ReadOnly = true;
            this.Unidade.Visible = false;
            this.Unidade.Width = 119;
            // 
            // Quantidade
            // 
            this.Quantidade.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Quantidade.DataPropertyName = "Quantidade";
            this.Quantidade.HeaderText = "Quantidade Estoque";
            this.Quantidade.Name = "Quantidade";
            this.Quantidade.ReadOnly = true;
            this.Quantidade.Visible = false;
            this.Quantidade.Width = 222;
            // 
            // NovaUnidade
            // 
            this.NovaUnidade.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.NovaUnidade.DataPropertyName = "NovaUnidade";
            this.NovaUnidade.HeaderText = "Nova Unidade";
            this.NovaUnidade.Name = "NovaUnidade";
            this.NovaUnidade.Width = 142;
            // 
            // NovaQuantidade
            // 
            this.NovaQuantidade.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.NovaQuantidade.DataPropertyName = "NovaQuantidade";
            this.NovaQuantidade.HeaderText = "Nova Quantidade";
            this.NovaQuantidade.Name = "NovaQuantidade";
            this.NovaQuantidade.Width = 192;
            // 
            // dtpData
            // 
            this.dtpData.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dtpData.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpData.Location = new System.Drawing.Point(16, 91);
            this.dtpData.Name = "dtpData";
            this.dtpData.Size = new System.Drawing.Size(375, 29);
            this.dtpData.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 22);
            this.label4.TabIndex = 19;
            this.label4.Text = "Data do Inventário";
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(16, 34);
            this.txtDescricao.MaxLength = 100;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(718, 29);
            this.txtDescricao.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 22);
            this.label1.TabIndex = 13;
            this.label1.Text = "Descrição";
            // 
            // frmInventarioEditar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1091, 701);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dtpData);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDescricao);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmInventarioEditar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Contagem de Estoque";
            this.Load += new System.EventHandler(this.frmInventarioEditar_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrincipal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.TextBox txtProdutos;
        private System.Windows.Forms.Button btnTodosProdutos;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvPrincipal;
        private System.Windows.Forms.DateTimePicker dtpData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewImageColumn btRemover;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDInventarioProdutos;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDProduto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nome;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantidade;
        private System.Windows.Forms.DataGridViewComboBoxColumn NovaUnidade;
        private System.Windows.Forms.DataGridViewTextBoxColumn NovaQuantidade;
        private System.Windows.Forms.Button btnAdicionarTodos;
    }
}