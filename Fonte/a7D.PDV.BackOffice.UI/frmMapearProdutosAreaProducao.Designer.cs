namespace a7D.PDV.BackOffice.UI
{
    partial class frmMapearProdutosAreaProducao
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbbAreaImpressao = new System.Windows.Forms.ComboBox();
            this.ltbProdutosMapeados = new System.Windows.Forms.ListBox();
            this.btnRemoverTodos = new System.Windows.Forms.Button();
            this.btnRemover = new System.Windows.Forms.Button();
            this.btnAdicionarTodos = new System.Windows.Forms.Button();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.txtNomeProduto = new System.Windows.Forms.TextBox();
            this.ltbProdutos = new System.Windows.Forms.ListBox();
            this.cbbCategoria = new System.Windows.Forms.ComboBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(464, 22);
            this.label1.TabIndex = 8;
            this.label1.Text = "MAPEAR PRODUTOS PARA ÁREA DE PRODUÇÃO";
            // 
            // cbbAreaImpressao
            // 
            this.cbbAreaImpressao.DisplayMember = "Nome";
            this.cbbAreaImpressao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbbAreaImpressao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbAreaImpressao.Font = new System.Drawing.Font("Arial", 11.25F);
            this.cbbAreaImpressao.FormattingEnabled = true;
            this.cbbAreaImpressao.Location = new System.Drawing.Point(2, 2);
            this.cbbAreaImpressao.Margin = new System.Windows.Forms.Padding(2);
            this.cbbAreaImpressao.Name = "cbbAreaImpressao";
            this.cbbAreaImpressao.Size = new System.Drawing.Size(448, 25);
            this.cbbAreaImpressao.TabIndex = 0;
            this.cbbAreaImpressao.ValueMember = "IDAreaProducao";
            this.cbbAreaImpressao.SelectedIndexChanged += new System.EventHandler(this.cbbAreaImpressao_SelectedIndexChanged);
            // 
            // ltbProdutosMapeados
            // 
            this.ltbProdutosMapeados.DisplayMember = "Nome";
            this.ltbProdutosMapeados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ltbProdutosMapeados.Font = new System.Drawing.Font("Arial", 11.25F);
            this.ltbProdutosMapeados.FormattingEnabled = true;
            this.ltbProdutosMapeados.ItemHeight = 17;
            this.ltbProdutosMapeados.Location = new System.Drawing.Point(2, 35);
            this.ltbProdutosMapeados.Margin = new System.Windows.Forms.Padding(2);
            this.ltbProdutosMapeados.Name = "ltbProdutosMapeados";
            this.tableLayoutPanel1.SetRowSpan(this.ltbProdutosMapeados, 6);
            this.ltbProdutosMapeados.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ltbProdutosMapeados.Size = new System.Drawing.Size(448, 472);
            this.ltbProdutosMapeados.TabIndex = 1;
            this.ltbProdutosMapeados.ValueMember = "IDProduto";
            // 
            // btnRemoverTodos
            // 
            this.btnRemoverTodos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoverTodos.Enabled = false;
            this.btnRemoverTodos.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoverTodos.Location = new System.Drawing.Point(454, 351);
            this.btnRemoverTodos.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoverTodos.Name = "btnRemoverTodos";
            this.btnRemoverTodos.Size = new System.Drawing.Size(96, 75);
            this.btnRemoverTodos.TabIndex = 8;
            this.btnRemoverTodos.Text = ">>";
            this.btnRemoverTodos.UseVisualStyleBackColor = true;
            this.btnRemoverTodos.Click += new System.EventHandler(this.btnRemoverTodos_Click);
            // 
            // btnRemover
            // 
            this.btnRemover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemover.Enabled = false;
            this.btnRemover.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemover.Location = new System.Drawing.Point(454, 272);
            this.btnRemover.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemover.Name = "btnRemover";
            this.btnRemover.Size = new System.Drawing.Size(96, 75);
            this.btnRemover.TabIndex = 7;
            this.btnRemover.Text = ">";
            this.btnRemover.UseVisualStyleBackColor = true;
            this.btnRemover.Click += new System.EventHandler(this.btnRemover_Click);
            // 
            // btnAdicionarTodos
            // 
            this.btnAdicionarTodos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdicionarTodos.Enabled = false;
            this.btnAdicionarTodos.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionarTodos.Location = new System.Drawing.Point(454, 114);
            this.btnAdicionarTodos.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdicionarTodos.Name = "btnAdicionarTodos";
            this.btnAdicionarTodos.Size = new System.Drawing.Size(96, 75);
            this.btnAdicionarTodos.TabIndex = 6;
            this.btnAdicionarTodos.Text = "<<";
            this.btnAdicionarTodos.UseVisualStyleBackColor = true;
            this.btnAdicionarTodos.Click += new System.EventHandler(this.btnAdicionarTodos_Click);
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdicionar.Enabled = false;
            this.btnAdicionar.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionar.Location = new System.Drawing.Point(454, 35);
            this.btnAdicionar.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(96, 75);
            this.btnAdicionar.TabIndex = 5;
            this.btnAdicionar.Text = "<";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click);
            // 
            // txtNomeProduto
            // 
            this.txtNomeProduto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNomeProduto.Font = new System.Drawing.Font("Arial", 11.25F);
            this.txtNomeProduto.Location = new System.Drawing.Point(757, 2);
            this.txtNomeProduto.Margin = new System.Windows.Forms.Padding(2);
            this.txtNomeProduto.Name = "txtNomeProduto";
            this.txtNomeProduto.Size = new System.Drawing.Size(199, 25);
            this.txtNomeProduto.TabIndex = 3;
            // 
            // ltbProdutos
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ltbProdutos, 3);
            this.ltbProdutos.DisplayMember = "Nome";
            this.ltbProdutos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ltbProdutos.Font = new System.Drawing.Font("Arial", 11.25F);
            this.ltbProdutos.FormattingEnabled = true;
            this.ltbProdutos.ItemHeight = 17;
            this.ltbProdutos.Location = new System.Drawing.Point(554, 35);
            this.ltbProdutos.Margin = new System.Windows.Forms.Padding(2);
            this.ltbProdutos.Name = "ltbProdutos";
            this.tableLayoutPanel1.SetRowSpan(this.ltbProdutos, 6);
            this.ltbProdutos.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ltbProdutos.Size = new System.Drawing.Size(450, 472);
            this.ltbProdutos.TabIndex = 4;
            this.ltbProdutos.ValueMember = "IDProduto";
            // 
            // cbbCategoria
            // 
            this.cbbCategoria.DisplayMember = "Nome";
            this.cbbCategoria.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbCategoria.Font = new System.Drawing.Font("Arial", 11.25F);
            this.cbbCategoria.FormattingEnabled = true;
            this.cbbCategoria.Location = new System.Drawing.Point(554, 2);
            this.cbbCategoria.Margin = new System.Windows.Forms.Padding(2);
            this.cbbCategoria.Name = "cbbCategoria";
            this.cbbCategoria.Size = new System.Drawing.Size(199, 25);
            this.cbbCategoria.TabIndex = 2;
            this.cbbCategoria.ValueMember = "IDCategoriaProduto";
            this.cbbCategoria.SelectedIndexChanged += new System.EventHandler(this.cbbCategoria_SelectedIndexChanged);
            // 
            // btnBuscar
            // 
            this.btnBuscar.AutoSize = true;
            this.btnBuscar.BackgroundImage = global::a7D.PDV.BackOffice.UI.Properties.Resources.ic_search_black_24dp_1x;
            this.btnBuscar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBuscar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBuscar.Font = new System.Drawing.Font("Arial", 11.25F);
            this.btnBuscar.Location = new System.Drawing.Point(960, 2);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(2);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(44, 29);
            this.btnBuscar.TabIndex = 9;
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.5F));
            this.tableLayoutPanel1.Controls.Add(this.cbbAreaImpressao, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRemoverTodos, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.ltbProdutosMapeados, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRemover, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnBuscar, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAdicionarTodos, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbbCategoria, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAdicionar, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtNomeProduto, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.ltbProdutos, 2, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 31);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1006, 509);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // frmMapearProdutosAreaProducao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 550);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMapearProdutosAreaProducao";
            this.Text = "frmMapearProdutosAreaProducao";
            this.Load += new System.EventHandler(this.frmMapearProdutosAreaProducao_Load);
            this.VisibleChanged += new System.EventHandler(this.frmMapearProdutosAreaProducao_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbAreaImpressao;
        private System.Windows.Forms.ListBox ltbProdutosMapeados;
        private System.Windows.Forms.Button btnRemoverTodos;
        private System.Windows.Forms.Button btnRemover;
        private System.Windows.Forms.Button btnAdicionarTodos;
        private System.Windows.Forms.Button btnAdicionar;
        private System.Windows.Forms.TextBox txtNomeProduto;
        private System.Windows.Forms.ListBox ltbProdutos;
        private System.Windows.Forms.ComboBox cbbCategoria;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}