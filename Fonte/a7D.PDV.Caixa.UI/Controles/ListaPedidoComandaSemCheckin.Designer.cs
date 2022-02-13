namespace a7D.PDV.Caixa.UI.Controles
{
    partial class ListaPedidoComandaSemCheckin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListaPedidoComandaSemCheckin));
            this.label4 = new System.Windows.Forms.Label();
            this.txtComanda = new System.Windows.Forms.TextBox();
            this.lvComandas = new System.Windows.Forms.ListView();
            this.imageListIconesComanda = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(186, 29);
            this.label4.TabIndex = 77;
            this.label4.Text = "Número da comanda";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtComanda
            // 
            this.txtComanda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtComanda.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtComanda.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComanda.Location = new System.Drawing.Point(201, 0);
            this.txtComanda.Margin = new System.Windows.Forms.Padding(5);
            this.txtComanda.MaxLength = 16;
            this.txtComanda.Name = "txtComanda";
            this.txtComanda.Size = new System.Drawing.Size(133, 28);
            this.txtComanda.TabIndex = 0;
            this.txtComanda.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtComanda_KeyDown);
            this.txtComanda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtComanda_KeyPress);
            this.txtComanda.Leave += new System.EventHandler(this.txtComanda_Leave);
            // 
            // lvComandas
            // 
            this.lvComandas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvComandas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.lvComandas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvComandas.HideSelection = false;
            this.lvComandas.LargeImageList = this.imageListIconesComanda;
            this.lvComandas.Location = new System.Drawing.Point(0, 36);
            this.lvComandas.Name = "lvComandas";
            this.lvComandas.Size = new System.Drawing.Size(525, 397);
            this.lvComandas.TabIndex = 1;
            this.lvComandas.UseCompatibleStateImageBehavior = false;
            this.lvComandas.Click += new System.EventHandler(this.lvComandas_Click);
            // 
            // imageListIconesComanda
            // 
            this.imageListIconesComanda.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIconesComanda.ImageStream")));
            this.imageListIconesComanda.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListIconesComanda.Images.SetKeyName(0, "icoVerde1.png");
            this.imageListIconesComanda.Images.SetKeyName(1, "icoBloqueado1.png");
            this.imageListIconesComanda.Images.SetKeyName(2, "icoVermelho1.png");
            this.imageListIconesComanda.Images.SetKeyName(3, "icoVerde0.png");
            this.imageListIconesComanda.Images.SetKeyName(4, "icoBloqueado0.png");
            this.imageListIconesComanda.Images.SetKeyName(5, "icoVermelho0.png");
            this.imageListIconesComanda.Images.SetKeyName(6, "icoPerdido1.png");
            this.imageListIconesComanda.Images.SetKeyName(7, "icoPerdido0.png");
            this.imageListIconesComanda.Images.SetKeyName(8, "icoAmarelo1.png");
            this.imageListIconesComanda.Images.SetKeyName(9, "icoAmarelo0.png");
            // 
            // ListaPedidoComandaSemCheckin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lvComandas);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtComanda);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ListaPedidoComandaSemCheckin";
            this.Size = new System.Drawing.Size(525, 433);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtComanda;
        private System.Windows.Forms.ListView lvComandas;
        private System.Windows.Forms.ImageList imageListIconesComanda;
    }
}
