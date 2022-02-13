namespace a7D.PDV.Configurador.UI
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnConfiguracoes = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.btnConfigurarConexaoDB = new System.Windows.Forms.Button();
            this.btnConfigurarLicenca = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnConfigurarSAT = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnConfigurarFechamento = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnLocalizarBalanca = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(859, 114);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btnConfiguracoes);
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(851, 81);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Configurações";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btnConfiguracoes
            // 
            this.btnConfiguracoes.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnConfiguracoes.Location = new System.Drawing.Point(4, 5);
            this.btnConfiguracoes.Margin = new System.Windows.Forms.Padding(2);
            this.btnConfiguracoes.Name = "btnConfiguracoes";
            this.btnConfiguracoes.Size = new System.Drawing.Size(156, 66);
            this.btnConfiguracoes.TabIndex = 0;
            this.btnConfiguracoes.Text = "Configurações Sistema";
            this.btnConfiguracoes.UseVisualStyleBackColor = true;
            this.btnConfiguracoes.Click += new System.EventHandler(this.btnConfiguracoes_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnLimpar);
            this.tabPage1.Controls.Add(this.btnConfigurarConexaoDB);
            this.tabPage1.Controls.Add(this.btnConfigurarLicenca);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(851, 81);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Banco de Dados";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnLimpar
            // 
            this.btnLimpar.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnLimpar.Location = new System.Drawing.Point(324, 5);
            this.btnLimpar.Margin = new System.Windows.Forms.Padding(2);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(156, 66);
            this.btnLimpar.TabIndex = 2;
            this.btnLimpar.Text = "Limpar Licença PDV";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // btnConfigurarConexaoDB
            // 
            this.btnConfigurarConexaoDB.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnConfigurarConexaoDB.Location = new System.Drawing.Point(4, 5);
            this.btnConfigurarConexaoDB.Margin = new System.Windows.Forms.Padding(2);
            this.btnConfigurarConexaoDB.Name = "btnConfigurarConexaoDB";
            this.btnConfigurarConexaoDB.Size = new System.Drawing.Size(156, 66);
            this.btnConfigurarConexaoDB.TabIndex = 0;
            this.btnConfigurarConexaoDB.Text = "Configurar Conexão DB";
            this.btnConfigurarConexaoDB.UseVisualStyleBackColor = true;
            this.btnConfigurarConexaoDB.Click += new System.EventHandler(this.btnConfigurarConexaoDB_Click);
            // 
            // btnConfigurarLicenca
            // 
            this.btnConfigurarLicenca.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnConfigurarLicenca.Location = new System.Drawing.Point(164, 5);
            this.btnConfigurarLicenca.Margin = new System.Windows.Forms.Padding(2);
            this.btnConfigurarLicenca.Name = "btnConfigurarLicenca";
            this.btnConfigurarLicenca.Size = new System.Drawing.Size(156, 66);
            this.btnConfigurarLicenca.TabIndex = 1;
            this.btnConfigurarLicenca.Text = "Configurar Licença";
            this.btnConfigurarLicenca.UseVisualStyleBackColor = true;
            this.btnConfigurarLicenca.Click += new System.EventHandler(this.btnConfigurarLicenca_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnConfigurarSAT);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(851, 81);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Fiscal";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnConfigurarSAT
            // 
            this.btnConfigurarSAT.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnConfigurarSAT.Location = new System.Drawing.Point(4, 5);
            this.btnConfigurarSAT.Margin = new System.Windows.Forms.Padding(2);
            this.btnConfigurarSAT.Name = "btnConfigurarSAT";
            this.btnConfigurarSAT.Size = new System.Drawing.Size(156, 66);
            this.btnConfigurarSAT.TabIndex = 0;
            this.btnConfigurarSAT.Text = "Configurar Fiscal";
            this.btnConfigurarSAT.UseVisualStyleBackColor = true;
            this.btnConfigurarSAT.Click += new System.EventHandler(this.btnConfigurarSAT_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btnConfigurarFechamento);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(851, 81);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Fechamento";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnConfigurarFechamento
            // 
            this.btnConfigurarFechamento.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnConfigurarFechamento.Location = new System.Drawing.Point(4, 5);
            this.btnConfigurarFechamento.Margin = new System.Windows.Forms.Padding(2);
            this.btnConfigurarFechamento.Name = "btnConfigurarFechamento";
            this.btnConfigurarFechamento.Size = new System.Drawing.Size(156, 66);
            this.btnConfigurarFechamento.TabIndex = 0;
            this.btnConfigurarFechamento.Text = "Configurar Fechamento";
            this.btnConfigurarFechamento.UseVisualStyleBackColor = true;
            this.btnConfigurarFechamento.Click += new System.EventHandler(this.btnConfigurarFechamento_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnLocalizarBalanca);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(851, 81);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Balança";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnLocalizarBalanca
            // 
            this.btnLocalizarBalanca.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnLocalizarBalanca.Location = new System.Drawing.Point(4, 5);
            this.btnLocalizarBalanca.Margin = new System.Windows.Forms.Padding(2);
            this.btnLocalizarBalanca.Name = "btnLocalizarBalanca";
            this.btnLocalizarBalanca.Size = new System.Drawing.Size(156, 66);
            this.btnLocalizarBalanca.TabIndex = 0;
            this.btnLocalizarBalanca.Text = "Localizar Balança";
            this.btnLocalizarBalanca.UseVisualStyleBackColor = true;
            this.btnLocalizarBalanca.Click += new System.EventHandler(this.btnLocalizarBalanca_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(859, 624);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F);
            this.IsMdiContainer = true;
            this.Name = "frmPrincipal";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurador PDV7";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnConfigurarConexaoDB;
        private System.Windows.Forms.Button btnConfigurarLicenca;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnConfigurarSAT;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnConfigurarFechamento;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnLocalizarBalanca;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button btnConfiguracoes;
        private System.Windows.Forms.Button btnLimpar;
    }
}