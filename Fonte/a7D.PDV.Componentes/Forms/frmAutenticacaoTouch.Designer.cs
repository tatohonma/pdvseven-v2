namespace a7D.PDV.Componentes
{
    partial class frmAutenticacaoTouch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAutenticacaoTouch));
            this.lblMensagem = new System.Windows.Forms.Label();
            this.txtIdentificacao = new System.Windows.Forms.TextBox();
            this.btnApagar = new System.Windows.Forms.Button();
            this.teclado1 = new a7D.PDV.Componentes.Controles.Teclado();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lblMensagem
            // 
            this.lblMensagem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensagem.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensagem.ForeColor = System.Drawing.Color.Red;
            this.lblMensagem.Location = new System.Drawing.Point(12, 76);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(322, 42);
            this.lblMensagem.TabIndex = 16;
            this.lblMensagem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtIdentificacao
            // 
            this.txtIdentificacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtIdentificacao.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIdentificacao.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIdentificacao.Location = new System.Drawing.Point(12, 121);
            this.txtIdentificacao.MaxLength = 10;
            this.txtIdentificacao.Name = "txtIdentificacao";
            this.txtIdentificacao.Size = new System.Drawing.Size(255, 37);
            this.txtIdentificacao.TabIndex = 15;
            this.txtIdentificacao.UseSystemPasswordChar = true;
            this.txtIdentificacao.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIdentificacao_KeyPress);
            // 
            // btnApagar
            // 
            this.btnApagar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnApagar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnApagar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApagar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApagar.ForeColor = System.Drawing.Color.White;
            this.btnApagar.Location = new System.Drawing.Point(274, 121);
            this.btnApagar.Name = "btnApagar";
            this.btnApagar.Size = new System.Drawing.Size(60, 46);
            this.btnApagar.TabIndex = 85;
            this.btnApagar.Text = "<";
            this.btnApagar.UseVisualStyleBackColor = false;
            this.btnApagar.Click += new System.EventHandler(this.btnApagar_Click);
            // 
            // teclado1
            // 
            this.teclado1.BackColor = System.Drawing.Color.Transparent;
            this.teclado1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teclado1.Location = new System.Drawing.Point(11, 171);
            this.teclado1.Margin = new System.Windows.Forms.Padding(4);
            this.teclado1.Name = "teclado1";
            this.teclado1.Size = new System.Drawing.Size(326, 317);
            this.teclado1.TabIndex = 83;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.Location = new System.Drawing.Point(12, 121);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(255, 46);
            this.panel1.TabIndex = 100;
            // 
            // frmAutenticacaoTouch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(350, 500);
            this.Controls.Add(this.btnApagar);
            this.Controls.Add(this.teclado1);
            this.Controls.Add(this.lblMensagem);
            this.Controls.Add(this.txtIdentificacao);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(350, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 500);
            this.Name = "frmAutenticacaoTouch";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CHAVE DE ACESSO";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAutenticacao_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblMensagem;
        private System.Windows.Forms.TextBox txtIdentificacao;
        private a7D.PDV.Componentes.Controles.Teclado teclado1;
        private System.Windows.Forms.Button btnApagar;
        private System.Windows.Forms.Panel panel1;
    }
}