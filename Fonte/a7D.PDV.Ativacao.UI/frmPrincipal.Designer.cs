namespace a7D.PDV.Ativacao.UI
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
            this.btnGerarArquivoLicenca = new System.Windows.Forms.Button();
            this.btnGerarChaveAtivacao = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGerarArquivoLicenca
            // 
            this.btnGerarArquivoLicenca.Location = new System.Drawing.Point(13, 13);
            this.btnGerarArquivoLicenca.Name = "btnGerarArquivoLicenca";
            this.btnGerarArquivoLicenca.Size = new System.Drawing.Size(259, 23);
            this.btnGerarArquivoLicenca.TabIndex = 0;
            this.btnGerarArquivoLicenca.Text = "Gerar licença";
            this.btnGerarArquivoLicenca.UseVisualStyleBackColor = true;
            this.btnGerarArquivoLicenca.Click += new System.EventHandler(this.btnGerarArquivoLicenca_Click);
            // 
            // btnGerarChaveAtivacao
            // 
            this.btnGerarChaveAtivacao.Location = new System.Drawing.Point(13, 42);
            this.btnGerarChaveAtivacao.Name = "btnGerarChaveAtivacao";
            this.btnGerarChaveAtivacao.Size = new System.Drawing.Size(259, 23);
            this.btnGerarChaveAtivacao.TabIndex = 1;
            this.btnGerarChaveAtivacao.Text = "Gerar Chave Ativação";
            this.btnGerarChaveAtivacao.UseVisualStyleBackColor = true;
            this.btnGerarChaveAtivacao.Click += new System.EventHandler(this.btnGerarChaveAtivacao_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnGerarChaveAtivacao);
            this.Controls.Add(this.btnGerarArquivoLicenca);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmPrincipal";
            this.Text = "frmPrincipal";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGerarArquivoLicenca;
        private System.Windows.Forms.Button btnGerarChaveAtivacao;
    }
}