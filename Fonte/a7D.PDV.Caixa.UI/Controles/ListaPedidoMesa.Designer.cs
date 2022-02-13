namespace a7D.PDV.Caixa.UI.Controles
{
    partial class ListaPedidoMesa
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListaPedidoMesa));
            this.ltvMesas = new System.Windows.Forms.ListView();
            this.imageListaIconesMesa = new System.Windows.Forms.ImageList(this.components);
            this.txtMesa = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbFiltro = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ltvMesas
            // 
            this.ltvMesas.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.ltvMesas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ltvMesas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.ltvMesas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ltvMesas.HideSelection = false;
            this.ltvMesas.LargeImageList = this.imageListaIconesMesa;
            this.ltvMesas.Location = new System.Drawing.Point(0, 37);
            this.ltvMesas.Margin = new System.Windows.Forms.Padding(4);
            this.ltvMesas.MultiSelect = false;
            this.ltvMesas.Name = "ltvMesas";
            this.ltvMesas.Size = new System.Drawing.Size(525, 396);
            this.ltvMesas.TabIndex = 63;
            this.ltvMesas.UseCompatibleStateImageBehavior = false;
            this.ltvMesas.Click += new System.EventHandler(this.ltvMesas_Click);
            // 
            // imageListaIconesMesa
            // 
            this.imageListaIconesMesa.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListaIconesMesa.ImageStream")));
            this.imageListaIconesMesa.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListaIconesMesa.Images.SetKeyName(0, "icoVerde1.png");
            this.imageListaIconesMesa.Images.SetKeyName(1, "icoAmarelo1.png");
            this.imageListaIconesMesa.Images.SetKeyName(2, "icoVermelho1.png");
            this.imageListaIconesMesa.Images.SetKeyName(3, "icoVerde0.png");
            this.imageListaIconesMesa.Images.SetKeyName(4, "icoAmarelo0.png");
            this.imageListaIconesMesa.Images.SetKeyName(5, "icoVermelho0.png");
            // 
            // txtMesa
            // 
            this.txtMesa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtMesa.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMesa.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMesa.Location = new System.Drawing.Point(171, 0);
            this.txtMesa.Margin = new System.Windows.Forms.Padding(5);
            this.txtMesa.Name = "txtMesa";
            this.txtMesa.Size = new System.Drawing.Size(66, 28);
            this.txtMesa.TabIndex = 74;
            this.txtMesa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMesa_KeyDown);
            this.txtMesa.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMesa_KeyPress);
            this.txtMesa.Leave += new System.EventHandler(this.txtMesa_Leave);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 28);
            this.label4.TabIndex = 75;
            this.label4.Text = "Número da mesa";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbFiltro
            // 
            this.cbFiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbFiltro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbFiltro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbFiltro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFiltro.Font = new System.Drawing.Font("Arial", 12F);
            this.cbFiltro.FormattingEnabled = true;
            this.cbFiltro.Items.AddRange(new object[] {
            "Menor para o maior",
            "Maior para o menor",
            "Ocupadas primeiro",
            "Livres primeiro",
            "Solicitando conta primeiro"});
            this.cbFiltro.Location = new System.Drawing.Point(336, 2);
            this.cbFiltro.Name = "cbFiltro";
            this.cbFiltro.Size = new System.Drawing.Size(186, 26);
            this.cbFiltro.TabIndex = 76;
            this.cbFiltro.Visible = false;
            this.cbFiltro.SelectedIndexChanged += new System.EventHandler(this.cbFiltro_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(245, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 28);
            this.label1.TabIndex = 77;
            this.label1.Text = "Ordem";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Visible = false;
            // 
            // ListaPedidoMesa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbFiltro);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMesa);
            this.Controls.Add(this.ltvMesas);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ListaPedidoMesa";
            this.Size = new System.Drawing.Size(525, 433);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView ltvMesas;
        private System.Windows.Forms.ImageList imageListaIconesMesa;
        private System.Windows.Forms.TextBox txtMesa;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbFiltro;
        private System.Windows.Forms.Label label1;
    }
}
