namespace a7D.PDV.Terminal.UI
{
    partial class frmPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.pnlPrincipal = new System.Windows.Forms.Panel();
            this.txtEan = new System.Windows.Forms.TextBox();
            this.pedidoProduto1 = new a7D.PDV.Componentes.Controles.PedidoProduto();
            this.produtos1 = new a7D.PDV.Componentes.Controles.Produtos();
            this.categorias1 = new a7D.PDV.Componentes.Controles.Categorias();
            this.pnlPrincipal.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPrincipal
            // 
            this.pnlPrincipal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPrincipal.Controls.Add(this.txtEan);
            this.pnlPrincipal.Controls.Add(this.pedidoProduto1);
            this.pnlPrincipal.Controls.Add(this.produtos1);
            this.pnlPrincipal.Controls.Add(this.categorias1);
            this.pnlPrincipal.Location = new System.Drawing.Point(5, 77);
            this.pnlPrincipal.Name = "pnlPrincipal";
            this.pnlPrincipal.Size = new System.Drawing.Size(786, 420);
            this.pnlPrincipal.TabIndex = 0;
            // 
            // txtEan
            // 
            this.txtEan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtEan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEan.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEan.Location = new System.Drawing.Point(353, 6);
            this.txtEan.Name = "txtEan";
            this.txtEan.Size = new System.Drawing.Size(433, 28);
            this.txtEan.TabIndex = 0;
            this.txtEan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEan_KeyDown);
            this.txtEan.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtEan_MouseDown);
            // 
            // pedidoProduto1
            // 
            this.pedidoProduto1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pedidoProduto1.BackColor = System.Drawing.Color.White;
            this.pedidoProduto1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pedidoProduto1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pedidoProduto1.Location = new System.Drawing.Point(353, 41);
            this.pedidoProduto1.Margin = new System.Windows.Forms.Padding(4);
            this.pedidoProduto1.Name = "pedidoProduto1";
            this.pedidoProduto1.OcultaResumoPedido = false;
            this.pedidoProduto1.Size = new System.Drawing.Size(433, 375);
            this.pedidoProduto1.TabIndex = 3;
            this.pedidoProduto1.TabStop = false;
            // 
            // produtos1
            // 
            this.produtos1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.produtos1.AutoScroll = true;
            this.produtos1.AutoScrollMinSize = new System.Drawing.Size(15, 1);
            this.produtos1.BackColor = System.Drawing.Color.White;
            this.produtos1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.produtos1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.produtos1.ListarCreditos = false;
            this.produtos1.Location = new System.Drawing.Point(0, 51);
            this.produtos1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.produtos1.Name = "produtos1";
            this.produtos1.Size = new System.Drawing.Size(345, 365);
            this.produtos1.TabIndex = 2;
            this.produtos1.TabStop = false;
            // 
            // categorias1
            // 
            this.categorias1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categorias1.AutoScroll = true;
            this.categorias1.AutoScrollMinSize = new System.Drawing.Size(30, 0);
            this.categorias1.BackColor = System.Drawing.Color.White;
            this.categorias1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.categorias1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categorias1.ListarCreditos = false;
            this.categorias1.Location = new System.Drawing.Point(0, 0);
            this.categorias1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.categorias1.Name = "categorias1";
            this.categorias1.Size = new System.Drawing.Size(345, 47);
            this.categorias1.TabIndex = 1;
            this.categorias1.TabStop = false;
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.pnlPrincipal);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PEDIDOS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.Shown += new System.EventHandler(this.frmPrincipal_Shown);
            this.Enter += new System.EventHandler(this.frmPrincipal_Enter);
            this.pnlPrincipal.ResumeLayout(false);
            this.pnlPrincipal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlPrincipal;
        private a7D.PDV.Componentes.Controles.Categorias categorias1;
        private a7D.PDV.Componentes.Controles.Produtos produtos1;
        private a7D.PDV.Componentes.Controles.PedidoProduto pedidoProduto1;
        protected internal System.Windows.Forms.TextBox txtEan;
    }
}