namespace a7D.PDV.BackOffice.UI
{
    partial class frmAreaImpressaoEditar
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.cbbImpressora = new System.Windows.Forms.ComboBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSAT = new System.Windows.Forms.RadioButton();
            this.rbProducao = new System.Windows.Forms.RadioButton();
            this.rbContaPadrao = new System.Windows.Forms.RadioButton();
            this.rbConta = new System.Windows.Forms.RadioButton();
            this.rbImpressora = new System.Windows.Forms.RadioButton();
            this.rbCaixa = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbbCaixa = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nome da área de produção";
            // 
            // txtNome
            // 
            this.txtNome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNome.Location = new System.Drawing.Point(15, 29);
            this.txtNome.MaxLength = 50;
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(273, 25);
            this.txtNome.TabIndex = 0;
            // 
            // cbbImpressora
            // 
            this.cbbImpressora.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbImpressora.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbImpressora.FormattingEnabled = true;
            this.cbbImpressora.Location = new System.Drawing.Point(11, 78);
            this.cbbImpressora.Name = "cbbImpressora";
            this.cbbImpressora.Size = new System.Drawing.Size(256, 25);
            this.cbbImpressora.TabIndex = 1;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancelar.Location = new System.Drawing.Point(110, 335);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(92, 47);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSalvar.Location = new System.Drawing.Point(12, 335);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(92, 47);
            this.btnSalvar.TabIndex = 2;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rbSAT);
            this.groupBox1.Controls.Add(this.rbProducao);
            this.groupBox1.Controls.Add(this.rbContaPadrao);
            this.groupBox1.Controls.Add(this.rbConta);
            this.groupBox1.Location = new System.Drawing.Point(15, 185);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 134);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tipo de conteúdo a ser impresso";
            // 
            // rbSAT
            // 
            this.rbSAT.AutoSize = true;
            this.rbSAT.Location = new System.Drawing.Point(6, 105);
            this.rbSAT.Name = "rbSAT";
            this.rbSAT.Size = new System.Drawing.Size(60, 21);
            this.rbSAT.TabIndex = 3;
            this.rbSAT.Text = "S@T";
            this.rbSAT.UseVisualStyleBackColor = true;
            this.rbSAT.CheckedChanged += new System.EventHandler(this.rbSAT_CheckedChanged);
            // 
            // rbProducao
            // 
            this.rbProducao.AutoSize = true;
            this.rbProducao.Checked = true;
            this.rbProducao.Location = new System.Drawing.Point(6, 24);
            this.rbProducao.Name = "rbProducao";
            this.rbProducao.Size = new System.Drawing.Size(89, 21);
            this.rbProducao.TabIndex = 2;
            this.rbProducao.TabStop = true;
            this.rbProducao.Text = "Produção";
            this.rbProducao.UseVisualStyleBackColor = true;
            // 
            // rbContaPadrao
            // 
            this.rbContaPadrao.AutoSize = true;
            this.rbContaPadrao.Location = new System.Drawing.Point(6, 78);
            this.rbContaPadrao.Name = "rbContaPadrao";
            this.rbContaPadrao.Size = new System.Drawing.Size(114, 21);
            this.rbContaPadrao.TabIndex = 1;
            this.rbContaPadrao.Text = "Conta padrão";
            this.rbContaPadrao.UseVisualStyleBackColor = true;
            // 
            // rbConta
            // 
            this.rbConta.AutoSize = true;
            this.rbConta.Location = new System.Drawing.Point(6, 51);
            this.rbConta.Name = "rbConta";
            this.rbConta.Size = new System.Drawing.Size(65, 21);
            this.rbConta.TabIndex = 0;
            this.rbConta.Text = "Conta";
            this.rbConta.UseVisualStyleBackColor = true;
            // 
            // rbImpressora
            // 
            this.rbImpressora.AutoSize = true;
            this.rbImpressora.Checked = true;
            this.rbImpressora.Location = new System.Drawing.Point(11, 24);
            this.rbImpressora.Name = "rbImpressora";
            this.rbImpressora.Size = new System.Drawing.Size(178, 21);
            this.rbImpressora.TabIndex = 4;
            this.rbImpressora.TabStop = true;
            this.rbImpressora.Text = "Impressora no Servidor";
            this.rbImpressora.UseVisualStyleBackColor = true;
            this.rbImpressora.CheckedChanged += new System.EventHandler(this.rbImpressoraOuCaixa_CheckedChanged);
            // 
            // rbCaixa
            // 
            this.rbCaixa.AutoSize = true;
            this.rbCaixa.Location = new System.Drawing.Point(11, 51);
            this.rbCaixa.Name = "rbCaixa";
            this.rbCaixa.Size = new System.Drawing.Size(161, 21);
            this.rbCaixa.TabIndex = 5;
            this.rbCaixa.Text = "Impressora do Caixa";
            this.rbCaixa.UseVisualStyleBackColor = true;
            this.rbCaixa.CheckedChanged += new System.EventHandler(this.rbImpressoraOuCaixa_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cbbCaixa);
            this.groupBox2.Controls.Add(this.rbCaixa);
            this.groupBox2.Controls.Add(this.rbImpressora);
            this.groupBox2.Controls.Add(this.cbbImpressora);
            this.groupBox2.Location = new System.Drawing.Point(15, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(273, 119);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Local da impressão";
            // 
            // cbbCaixa
            // 
            this.cbbCaixa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbCaixa.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbbCaixa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbCaixa.FormattingEnabled = true;
            this.cbbCaixa.Location = new System.Drawing.Point(11, 78);
            this.cbbCaixa.Name = "cbbCaixa";
            this.cbbCaixa.Size = new System.Drawing.Size(256, 26);
            this.cbbCaixa.TabIndex = 6;
            this.cbbCaixa.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbbCaixa_DrawItem);
            this.cbbCaixa.SelectedIndexChanged += new System.EventHandler(this.cbbCaixa_SelectedIndexChanged);
            // 
            // frmAreaImpressaoEditar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 389);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmAreaImpressaoEditar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmAreaImpressaoEditar_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.ComboBox cbbImpressora;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbProducao;
        private System.Windows.Forms.RadioButton rbContaPadrao;
        private System.Windows.Forms.RadioButton rbConta;
        private System.Windows.Forms.RadioButton rbSAT;
        private System.Windows.Forms.RadioButton rbImpressora;
        private System.Windows.Forms.RadioButton rbCaixa;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbbCaixa;
    }
}