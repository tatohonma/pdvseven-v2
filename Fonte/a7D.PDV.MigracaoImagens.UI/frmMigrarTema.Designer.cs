namespace a7D.PDV.MigracaoImagens.UI
{
    partial class frmMigrarTema
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
            this.lblProgresso = new System.Windows.Forms.Label();
            this.btnSelecionarPasta = new System.Windows.Forms.Button();
            this.txtCaminho = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.pbarProgresso = new System.Windows.Forms.ProgressBar();
            this.ddlTema = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ddlIdioma = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblProgresso
            // 
            this.lblProgresso.AutoSize = true;
            this.lblProgresso.Location = new System.Drawing.Point(303, 330);
            this.lblProgresso.Name = "lblProgresso";
            this.lblProgresso.Size = new System.Drawing.Size(136, 22);
            this.lblProgresso.TabIndex = 10;
            this.lblProgresso.Text = "0 de 0 (0,00%)";
            this.lblProgresso.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblProgresso.Visible = false;
            // 
            // btnSelecionarPasta
            // 
            this.btnSelecionarPasta.Location = new System.Drawing.Point(689, 117);
            this.btnSelecionarPasta.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSelecionarPasta.Name = "btnSelecionarPasta";
            this.btnSelecionarPasta.Size = new System.Drawing.Size(45, 32);
            this.btnSelecionarPasta.TabIndex = 7;
            this.btnSelecionarPasta.Text = "...";
            this.btnSelecionarPasta.UseVisualStyleBackColor = true;
            this.btnSelecionarPasta.Click += new System.EventHandler(this.btnSelecionarPasta_Click);
            // 
            // txtCaminho
            // 
            this.txtCaminho.Location = new System.Drawing.Point(26, 119);
            this.txtCaminho.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCaminho.Name = "txtCaminho";
            this.txtCaminho.Size = new System.Drawing.Size(653, 29);
            this.txtCaminho.TabIndex = 6;
            this.txtCaminho.TextChanged += new System.EventHandler(this.txtCaminho_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 90);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(386, 22);
            this.label1.TabIndex = 8;
            this.label1.Text = "Escolha o diretório com as imagens do tema";
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(631, 382);
            this.btnIniciar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(103, 32);
            this.btnIniciar.TabIndex = 9;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // pbarProgresso
            // 
            this.pbarProgresso.Location = new System.Drawing.Point(21, 293);
            this.pbarProgresso.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbarProgresso.Name = "pbarProgresso";
            this.pbarProgresso.Size = new System.Drawing.Size(714, 32);
            this.pbarProgresso.TabIndex = 5;
            this.pbarProgresso.Visible = false;
            // 
            // ddlTema
            // 
            this.ddlTema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTema.FormattingEnabled = true;
            this.ddlTema.Location = new System.Drawing.Point(26, 178);
            this.ddlTema.Name = "ddlTema";
            this.ddlTema.Size = new System.Drawing.Size(653, 30);
            this.ddlTema.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 153);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(361, 22);
            this.label2.TabIndex = 12;
            this.label2.Text = "Escolha de qual tema são essas imagens";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 211);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(355, 22);
            this.label3.TabIndex = 14;
            this.label3.Text = "Escolha qual é o idioma dessas imagens";
            // 
            // ddlIdioma
            // 
            this.ddlIdioma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlIdioma.FormattingEnabled = true;
            this.ddlIdioma.Location = new System.Drawing.Point(26, 236);
            this.ddlIdioma.Name = "ddlIdioma";
            this.ddlIdioma.Size = new System.Drawing.Size(653, 30);
            this.ddlIdioma.TabIndex = 13;
            // 
            // frmMigrarTema
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(747, 428);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ddlIdioma);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ddlTema);
            this.Controls.Add(this.lblProgresso);
            this.Controls.Add(this.btnSelecionarPasta);
            this.Controls.Add(this.txtCaminho);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.pbarProgresso);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.MaximizeBox = false;
            this.Name = "frmMigrarTema";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Migrar imagens tema";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmMigrarTema_Load);
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
        private System.Windows.Forms.ComboBox ddlTema;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddlIdioma;
    }
}