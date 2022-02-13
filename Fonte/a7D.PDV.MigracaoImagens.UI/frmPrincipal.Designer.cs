namespace a7D.PDV.MigracaoImagens.UI
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
            this.btnImagensTema = new System.Windows.Forms.Button();
            this.btnImagensProduto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImagensTema
            // 
            this.btnImagensTema.Location = new System.Drawing.Point(185, 12);
            this.btnImagensTema.Name = "btnImagensTema";
            this.btnImagensTema.Size = new System.Drawing.Size(167, 121);
            this.btnImagensTema.TabIndex = 1;
            this.btnImagensTema.Text = "Migrar Imagens Tema";
            this.btnImagensTema.UseVisualStyleBackColor = true;
            this.btnImagensTema.Click += new System.EventHandler(this.btnImagensTema_Click);
            // 
            // btnImagensProduto
            // 
            this.btnImagensProduto.Location = new System.Drawing.Point(12, 12);
            this.btnImagensProduto.Name = "btnImagensProduto";
            this.btnImagensProduto.Size = new System.Drawing.Size(167, 120);
            this.btnImagensProduto.TabIndex = 0;
            this.btnImagensProduto.Text = "Migrar Imagens Produto";
            this.btnImagensProduto.UseVisualStyleBackColor = true;
            this.btnImagensProduto.Click += new System.EventHandler(this.btnImagensProduto_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 144);
            this.Controls.Add(this.btnImagensTema);
            this.Controls.Add(this.btnImagensProduto);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPrincipal";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Migração de Imagens";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnImagensTema;
        private System.Windows.Forms.Button btnImagensProduto;
    }
}