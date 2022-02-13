namespace a7D.PDV.Caixa.UI
{
    partial class frmPedidoProdutosTouch
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPedidoProdutosTouch));
            this.scCategoriaProdutos = new System.Windows.Forms.SplitContainer();
            this.categorias1 = new a7D.PDV.Componentes.Controles.Categorias();
            this.produtos1 = new a7D.PDV.Componentes.Controles.Produtos();
            this.txtEan = new System.Windows.Forms.TextBox();
            this.pedidoProduto1 = new a7D.PDV.Componentes.Controles.PedidoProduto();
            this.scItensPedido = new System.Windows.Forms.SplitContainer();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.scCategoriaProdutos)).BeginInit();
            this.scCategoriaProdutos.Panel1.SuspendLayout();
            this.scCategoriaProdutos.Panel2.SuspendLayout();
            this.scCategoriaProdutos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scItensPedido)).BeginInit();
            this.scItensPedido.Panel1.SuspendLayout();
            this.scItensPedido.Panel2.SuspendLayout();
            this.scItensPedido.SuspendLayout();
            this.SuspendLayout();
            // 
            // scCategoriaProdutos
            // 
            this.scCategoriaProdutos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scCategoriaProdutos.Location = new System.Drawing.Point(0, 0);
            this.scCategoriaProdutos.Margin = new System.Windows.Forms.Padding(2);
            this.scCategoriaProdutos.Name = "scCategoriaProdutos";
            this.scCategoriaProdutos.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scCategoriaProdutos.Panel1
            // 
            this.scCategoriaProdutos.Panel1.Controls.Add(this.categorias1);
            // 
            // scCategoriaProdutos.Panel2
            // 
            this.scCategoriaProdutos.Panel2.Controls.Add(this.produtos1);
            this.scCategoriaProdutos.Size = new System.Drawing.Size(348, 346);
            this.scCategoriaProdutos.SplitterDistance = 103;
            this.scCategoriaProdutos.SplitterWidth = 3;
            this.scCategoriaProdutos.TabIndex = 4;
            // 
            // categorias1
            // 
            this.categorias1.AutoScroll = true;
            this.categorias1.AutoScrollMinSize = new System.Drawing.Size(30, 0);
            this.categorias1.BackColor = System.Drawing.Color.White;
            this.categorias1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.categorias1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.categorias1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categorias1.ListarCreditos = false;
            this.categorias1.Location = new System.Drawing.Point(0, 0);
            this.categorias1.Margin = new System.Windows.Forms.Padding(4);
            this.categorias1.Name = "categorias1";
            this.categorias1.Size = new System.Drawing.Size(348, 103);
            this.categorias1.TabIndex = 1;
            this.categorias1.TabStop = false;
            // 
            // produtos1
            // 
            this.produtos1.AutoScroll = true;
            this.produtos1.AutoScrollMinSize = new System.Drawing.Size(15, 1);
            this.produtos1.BackColor = System.Drawing.Color.White;
            this.produtos1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.produtos1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.produtos1.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.produtos1.ListarCreditos = false;
            this.produtos1.Location = new System.Drawing.Point(0, 0);
            this.produtos1.Margin = new System.Windows.Forms.Padding(4);
            this.produtos1.Name = "produtos1";
            this.produtos1.Size = new System.Drawing.Size(348, 240);
            this.produtos1.TabIndex = 2;
            this.produtos1.TabStop = false;
            // 
            // txtEan
            // 
            this.txtEan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtEan.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEan.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEan.Location = new System.Drawing.Point(1, 4);
            this.txtEan.Margin = new System.Windows.Forms.Padding(2);
            this.txtEan.Name = "txtEan";
            this.txtEan.Size = new System.Drawing.Size(370, 28);
            this.txtEan.TabIndex = 0;
            this.txtEan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEan_KeyDown);
            this.txtEan.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtEan_MouseDown);
            // 
            // pedidoProduto1
            // 
            this.pedidoProduto1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pedidoProduto1.BackColor = System.Drawing.Color.White;
            this.pedidoProduto1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pedidoProduto1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pedidoProduto1.Location = new System.Drawing.Point(0, 35);
            this.pedidoProduto1.Margin = new System.Windows.Forms.Padding(4);
            this.pedidoProduto1.Name = "pedidoProduto1";
            this.pedidoProduto1.OcultaResumoPedido = false;
            this.pedidoProduto1.Size = new System.Drawing.Size(371, 311);
            this.pedidoProduto1.TabIndex = 3;
            this.pedidoProduto1.TabStop = false;
            // 
            // scItensPedido
            // 
            this.scItensPedido.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scItensPedido.Location = new System.Drawing.Point(4, 70);
            this.scItensPedido.Margin = new System.Windows.Forms.Padding(4);
            this.scItensPedido.Name = "scItensPedido";
            // 
            // scItensPedido.Panel1
            // 
            this.scItensPedido.Panel1.Controls.Add(this.scCategoriaProdutos);
            // 
            // scItensPedido.Panel2
            // 
            this.scItensPedido.Panel2.Controls.Add(this.txtEan);
            this.scItensPedido.Panel2.Controls.Add(this.pedidoProduto1);
            this.scItensPedido.Size = new System.Drawing.Size(723, 346);
            this.scItensPedido.SplitterDistance = 348;
            this.scItensPedido.TabIndex = 78;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // frmPedidoProdutosTouch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(732, 450);
            this.Controls.Add(this.scItensPedido);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "frmPedidoProdutosTouch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PEDIDOS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPedidoProdutosTouch_FormClosing);
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.Enter += new System.EventHandler(this.frmPrincipal_Enter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEan_KeyDown);
            this.scCategoriaProdutos.Panel1.ResumeLayout(false);
            this.scCategoriaProdutos.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scCategoriaProdutos)).EndInit();
            this.scCategoriaProdutos.ResumeLayout(false);
            this.scItensPedido.Panel1.ResumeLayout(false);
            this.scItensPedido.Panel2.ResumeLayout(false);
            this.scItensPedido.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scItensPedido)).EndInit();
            this.scItensPedido.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private a7D.PDV.Componentes.Controles.Categorias categorias1;
        private a7D.PDV.Componentes.Controles.Produtos produtos1;
        private a7D.PDV.Componentes.Controles.PedidoProduto pedidoProduto1;
        protected internal System.Windows.Forms.TextBox txtEan;
        private System.Windows.Forms.SplitContainer scCategoriaProdutos;
        private System.Windows.Forms.SplitContainer scItensPedido;
        private System.Windows.Forms.Timer timer1;
    }
}