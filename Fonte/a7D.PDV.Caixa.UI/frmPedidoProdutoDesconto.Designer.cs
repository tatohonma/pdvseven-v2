namespace a7D.PDV.Caixa.UI
{
    partial class frmPedidoProdutoDesconto
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
            this.txtDescontoPercentual = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTipoDesconto = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProduto = new System.Windows.Forms.Label();
            this.lblPreco = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDescontoValor = new System.Windows.Forms.TextBox();
            this.lblValorFinal = new System.Windows.Forms.Label();
            this.lblNomeProduto = new System.Windows.Forms.Label();
            this.cbbTipoDesconto = new System.Windows.Forms.ComboBox();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDescontoPercentual
            // 
            this.txtDescontoPercentual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDescontoPercentual.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescontoPercentual.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtDescontoPercentual.Font = new System.Drawing.Font("Arial", 18F);
            this.txtDescontoPercentual.Location = new System.Drawing.Point(193, 58);
            this.txtDescontoPercentual.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescontoPercentual.Name = "txtDescontoPercentual";
            this.txtDescontoPercentual.Size = new System.Drawing.Size(132, 28);
            this.txtDescontoPercentual.TabIndex = 1;
            this.txtDescontoPercentual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDescontoPercentual.TextChanged += new System.EventHandler(this.txtDescontoPercentual_TextChanged);
            this.txtDescontoPercentual.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SomenteNumero_KeyPress);
            this.txtDescontoPercentual.Leave += new System.EventHandler(this.txtDescontoPercentual_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(97, 56);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 28);
            this.label4.TabIndex = 46;
            this.label4.Text = "Desconto %";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblTipoDesconto, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblProduto, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblPreco, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtDescontoPercentual, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtDescontoValor, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblValorFinal, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblNomeProduto, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbbTipoDesconto, 1, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 66);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(382, 171);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblTipoDesconto
            // 
            this.lblTipoDesconto.AutoSize = true;
            this.lblTipoDesconto.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblTipoDesconto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoDesconto.Location = new System.Drawing.Point(61, 140);
            this.lblTipoDesconto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTipoDesconto.Name = "lblTipoDesconto";
            this.lblTipoDesconto.Size = new System.Drawing.Size(126, 31);
            this.lblTipoDesconto.TabIndex = 53;
            this.lblTipoDesconto.Text = "Tipo de Desconto";
            this.lblTipoDesconto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTipoDesconto.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(110, 112);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 28);
            this.label2.TabIndex = 49;
            this.label2.Text = "Valor Final";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblProduto
            // 
            this.lblProduto.AutoSize = true;
            this.lblProduto.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblProduto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblProduto.Location = new System.Drawing.Point(92, 28);
            this.lblProduto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProduto.Name = "lblProduto";
            this.lblProduto.Size = new System.Drawing.Size(97, 28);
            this.lblProduto.TabIndex = 47;
            this.lblProduto.Text = "Valor Unitário";
            this.lblProduto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPreco
            // 
            this.lblPreco.AutoSize = true;
            this.lblPreco.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPreco.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Strikeout);
            this.lblPreco.Location = new System.Drawing.Point(193, 28);
            this.lblPreco.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPreco.Name = "lblPreco";
            this.lblPreco.Size = new System.Drawing.Size(142, 28);
            this.lblPreco.TabIndex = 48;
            this.lblPreco.Text = "R$ 99.999.99";
            this.lblPreco.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Right;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(91, 84);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 28);
            this.label3.TabIndex = 50;
            this.label3.Text = "Desconto R$";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDescontoValor
            // 
            this.txtDescontoValor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDescontoValor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescontoValor.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtDescontoValor.Font = new System.Drawing.Font("Arial", 18F);
            this.txtDescontoValor.Location = new System.Drawing.Point(193, 86);
            this.txtDescontoValor.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescontoValor.Name = "txtDescontoValor";
            this.txtDescontoValor.Size = new System.Drawing.Size(132, 28);
            this.txtDescontoValor.TabIndex = 2;
            this.txtDescontoValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDescontoValor.TextChanged += new System.EventHandler(this.txtDescontoValor_TextChanged);
            this.txtDescontoValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SomenteNumero_KeyPress);
            this.txtDescontoValor.Leave += new System.EventHandler(this.txtDescontoValor_Leave);
            // 
            // lblValorFinal
            // 
            this.lblValorFinal.AutoSize = true;
            this.lblValorFinal.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblValorFinal.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.lblValorFinal.Location = new System.Drawing.Point(193, 112);
            this.lblValorFinal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblValorFinal.Name = "lblValorFinal";
            this.lblValorFinal.Size = new System.Drawing.Size(142, 28);
            this.lblValorFinal.TabIndex = 52;
            this.lblValorFinal.Text = "R$ 99.999.99";
            this.lblValorFinal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNomeProduto
            // 
            this.lblNomeProduto.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblNomeProduto, 2);
            this.lblNomeProduto.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNomeProduto.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomeProduto.Location = new System.Drawing.Point(2, 0);
            this.lblNomeProduto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNomeProduto.Name = "lblNomeProduto";
            this.lblNomeProduto.Size = new System.Drawing.Size(378, 26);
            this.lblNomeProduto.TabIndex = 0;
            this.lblNomeProduto.Text = "Produto";
            this.lblNomeProduto.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cbbTipoDesconto
            // 
            this.cbbTipoDesconto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbbTipoDesconto.DisplayMember = "Nome";
            this.cbbTipoDesconto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbbTipoDesconto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTipoDesconto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbTipoDesconto.Font = new System.Drawing.Font("Arial", 14F);
            this.cbbTipoDesconto.FormattingEnabled = true;
            this.cbbTipoDesconto.Location = new System.Drawing.Point(193, 142);
            this.cbbTipoDesconto.Margin = new System.Windows.Forms.Padding(2);
            this.cbbTipoDesconto.Name = "cbbTipoDesconto";
            this.cbbTipoDesconto.Size = new System.Drawing.Size(187, 30);
            this.cbbTipoDesconto.TabIndex = 3;
            this.cbbTipoDesconto.ValueMember = "IDTipoDesconto";
            this.cbbTipoDesconto.Visible = false;
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
            this.btnConfirmar.Location = new System.Drawing.Point(9, 260);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(380, 48);
            this.btnConfirmar.TabIndex = 1;
            this.btnConfirmar.Text = "CONFIRMAR";
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // frmPedidoProdutoDesconto
            // 
            this.AcceptButton = this.btnConfirmar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(400, 320);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 320);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 320);
            this.Name = "frmPedidoProdutoDesconto";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DESCONTO";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPedidoProdutoDesconto_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtDescontoPercentual;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblProduto;
        private System.Windows.Forms.Label lblPreco;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDescontoValor;
        private System.Windows.Forms.Label lblValorFinal;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Label lblNomeProduto;
        private System.Windows.Forms.ComboBox cbbTipoDesconto;
        private System.Windows.Forms.Label lblTipoDesconto;
    }
}