namespace a7D.PDV.Caixa.UI
{
    partial class frmDisponibilidadeProduto
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
            this.tcProdutoCategoria = new System.Windows.Forms.TabControl();
            this.tbProdutos = new System.Windows.Forms.TabPage();
            this.cbbCategoria = new System.Windows.Forms.ComboBox();
            this.pnlProdutos = new System.Windows.Forms.Panel();
            this.tpCategorias = new System.Windows.Forms.TabPage();
            this.pnCategorias = new System.Windows.Forms.Panel();
            this.tcProdutoCategoria.SuspendLayout();
            this.tbProdutos.SuspendLayout();
            this.tpCategorias.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcProdutoCategoria
            // 
            this.tcProdutoCategoria.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcProdutoCategoria.Controls.Add(this.tbProdutos);
            this.tcProdutoCategoria.Controls.Add(this.tpCategorias);
            this.tcProdutoCategoria.Font = new System.Drawing.Font("Arial", 11.25F);
            this.tcProdutoCategoria.HotTrack = true;
            this.tcProdutoCategoria.Location = new System.Drawing.Point(9, 63);
            this.tcProdutoCategoria.Margin = new System.Windows.Forms.Padding(2);
            this.tcProdutoCategoria.Name = "tcProdutoCategoria";
            this.tcProdutoCategoria.SelectedIndex = 0;
            this.tcProdutoCategoria.Size = new System.Drawing.Size(780, 426);
            this.tcProdutoCategoria.TabIndex = 81;
            this.tcProdutoCategoria.SelectedIndexChanged += new System.EventHandler(this.tcProdutoCategoria_SelectedIndexChanged);
            // 
            // tbProdutos
            // 
            this.tbProdutos.Controls.Add(this.cbbCategoria);
            this.tbProdutos.Controls.Add(this.pnlProdutos);
            this.tbProdutos.Location = new System.Drawing.Point(4, 26);
            this.tbProdutos.Margin = new System.Windows.Forms.Padding(2);
            this.tbProdutos.Name = "tbProdutos";
            this.tbProdutos.Padding = new System.Windows.Forms.Padding(2);
            this.tbProdutos.Size = new System.Drawing.Size(772, 396);
            this.tbProdutos.TabIndex = 0;
            this.tbProdutos.Text = "Produtos";
            this.tbProdutos.UseVisualStyleBackColor = true;
            // 
            // cbbCategoria
            // 
            this.cbbCategoria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbCategoria.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbbCategoria.DisplayMember = "Nome";
            this.cbbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbCategoria.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbCategoria.Font = new System.Drawing.Font("Arial", 14F);
            this.cbbCategoria.FormattingEnabled = true;
            this.cbbCategoria.ItemHeight = 22;
            this.cbbCategoria.Location = new System.Drawing.Point(6, 7);
            this.cbbCategoria.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbbCategoria.Name = "cbbCategoria";
            this.cbbCategoria.Size = new System.Drawing.Size(760, 30);
            this.cbbCategoria.TabIndex = 81;
            this.cbbCategoria.ValueMember = "IDCategoriaProduto";
            this.cbbCategoria.SelectedIndexChanged += new System.EventHandler(this.cbbCategoria_SelectedIndexChanged);
            // 
            // pnlProdutos
            // 
            this.pnlProdutos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProdutos.AutoScroll = true;
            this.pnlProdutos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProdutos.Location = new System.Drawing.Point(6, 41);
            this.pnlProdutos.Margin = new System.Windows.Forms.Padding(2);
            this.pnlProdutos.Name = "pnlProdutos";
            this.pnlProdutos.Size = new System.Drawing.Size(760, 351);
            this.pnlProdutos.TabIndex = 82;
            // 
            // tpCategorias
            // 
            this.tpCategorias.Controls.Add(this.pnCategorias);
            this.tpCategorias.Location = new System.Drawing.Point(4, 26);
            this.tpCategorias.Margin = new System.Windows.Forms.Padding(2);
            this.tpCategorias.Name = "tpCategorias";
            this.tpCategorias.Padding = new System.Windows.Forms.Padding(2);
            this.tpCategorias.Size = new System.Drawing.Size(777, 416);
            this.tpCategorias.TabIndex = 1;
            this.tpCategorias.Text = "Categorias";
            this.tpCategorias.UseVisualStyleBackColor = true;
            // 
            // pnCategorias
            // 
            this.pnCategorias.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnCategorias.AutoScroll = true;
            this.pnCategorias.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnCategorias.Location = new System.Drawing.Point(4, 4);
            this.pnCategorias.Margin = new System.Windows.Forms.Padding(2);
            this.pnCategorias.Name = "pnCategorias";
            this.pnCategorias.Size = new System.Drawing.Size(769, 408);
            this.pnCategorias.TabIndex = 83;
            // 
            // frmDisponibilidadeProduto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.tcProdutoCategoria);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmDisponibilidadeProduto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DISPONIBILIDADE DE ITEM";
            this.Load += new System.EventHandler(this.frmDisponibilidadeProduto_Load);
            this.tcProdutoCategoria.ResumeLayout(false);
            this.tbProdutos.ResumeLayout(false);
            this.tpCategorias.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tcProdutoCategoria;
        private System.Windows.Forms.TabPage tbProdutos;
        private System.Windows.Forms.ComboBox cbbCategoria;
        private System.Windows.Forms.Panel pnlProdutos;
        private System.Windows.Forms.TabPage tpCategorias;
        private System.Windows.Forms.Panel pnCategorias;
    }
}