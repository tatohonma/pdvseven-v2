namespace a7D.PDV.MigracaoImagens.UI
{
    partial class frmMigrarProdutos
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
            this.fbdDiretorio = new System.Windows.Forms.FolderBrowserDialog();
            this.pbarProgresso = new System.Windows.Forms.ProgressBar();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCaminho = new System.Windows.Forms.TextBox();
            this.btnSelecionarPasta = new System.Windows.Forms.Button();
            this.lblProgresso = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbarProgresso
            // 
            this.pbarProgresso.Location = new System.Drawing.Point(21, 206);
            this.pbarProgresso.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbarProgresso.Name = "pbarProgresso";
            this.pbarProgresso.Size = new System.Drawing.Size(714, 32);
            this.pbarProgresso.TabIndex = 0;
            this.pbarProgresso.Visible = false;
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(631, 295);
            this.btnIniciar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(103, 32);
            this.btnIniciar.TabIndex = 3;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 90);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(429, 22);
            this.label1.TabIndex = 2;
            this.label1.Text = "Escolha o diretório com as imagens dos produtos\r\n";
            // 
            // txtCaminho
            // 
            this.txtCaminho.Location = new System.Drawing.Point(26, 119);
            this.txtCaminho.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCaminho.Name = "txtCaminho";
            this.txtCaminho.Size = new System.Drawing.Size(653, 29);
            this.txtCaminho.TabIndex = 1;
            // 
            // btnSelecionarPasta
            // 
            this.btnSelecionarPasta.Location = new System.Drawing.Point(689, 117);
            this.btnSelecionarPasta.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSelecionarPasta.Name = "btnSelecionarPasta";
            this.btnSelecionarPasta.Size = new System.Drawing.Size(45, 32);
            this.btnSelecionarPasta.TabIndex = 2;
            this.btnSelecionarPasta.Text = "...";
            this.btnSelecionarPasta.UseVisualStyleBackColor = true;
            this.btnSelecionarPasta.Click += new System.EventHandler(this.btnSelecionarPasta_Click);
            // 
            // lblProgresso
            // 
            this.lblProgresso.AutoSize = true;
            this.lblProgresso.Location = new System.Drawing.Point(303, 243);
            this.lblProgresso.Name = "lblProgresso";
            this.lblProgresso.Size = new System.Drawing.Size(136, 22);
            this.lblProgresso.TabIndex = 4;
            this.lblProgresso.Text = "0 de 0 (0,00%)";
            this.lblProgresso.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProgresso.Visible = false;
            // 
            // frmMigrarProdutos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 341);
            this.Controls.Add(this.lblProgresso);
            this.Controls.Add(this.btnSelecionarPasta);
            this.Controls.Add(this.txtCaminho);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.pbarProgresso);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "frmMigrarProdutos";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Migrar Imagens produtos";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmMigrar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog fbdDiretorio;
        private System.Windows.Forms.ProgressBar pbarProgresso;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCaminho;
        private System.Windows.Forms.Button btnSelecionarPasta;
        private System.Windows.Forms.Label lblProgresso;
    }
}

