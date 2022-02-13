namespace a7D.PDV.BackOffice.UI
{
    partial class frmUsuarioEditar
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
            this.txtSenha1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSenha2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ckbPermissaoAdm = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbPermissaoGarcom = new System.Windows.Forms.CheckBox();
            this.ckbPermissaoCaixa = new System.Windows.Forms.CheckBox();
            this.ckbPermissaoGerente = new System.Windows.Forms.CheckBox();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.ckbAtivo = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nome";
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(13, 34);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(432, 25);
            this.txtNome.TabIndex = 0;
            // 
            // txtSenha1
            // 
            this.txtSenha1.Location = new System.Drawing.Point(13, 90);
            this.txtSenha1.Name = "txtSenha1";
            this.txtSenha1.PasswordChar = '*';
            this.txtSenha1.Size = new System.Drawing.Size(213, 25);
            this.txtSenha1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Chave de Acesso";
            // 
            // txtSenha2
            // 
            this.txtSenha2.Location = new System.Drawing.Point(232, 90);
            this.txtSenha2.Name = "txtSenha2";
            this.txtSenha2.PasswordChar = '*';
            this.txtSenha2.Size = new System.Drawing.Size(213, 25);
            this.txtSenha2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(229, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Confirmar Chave de Acesso";
            // 
            // ckbPermissaoAdm
            // 
            this.ckbPermissaoAdm.AutoSize = true;
            this.ckbPermissaoAdm.Location = new System.Drawing.Point(20, 24);
            this.ckbPermissaoAdm.Name = "ckbPermissaoAdm";
            this.ckbPermissaoAdm.Size = new System.Drawing.Size(117, 21);
            this.ckbPermissaoAdm.TabIndex = 0;
            this.ckbPermissaoAdm.Text = "Administrador";
            this.ckbPermissaoAdm.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbPermissaoGarcom);
            this.groupBox1.Controls.Add(this.ckbPermissaoCaixa);
            this.groupBox1.Controls.Add(this.ckbPermissaoGerente);
            this.groupBox1.Controls.Add(this.ckbPermissaoAdm);
            this.groupBox1.Location = new System.Drawing.Point(15, 129);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(430, 61);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Permissão";
            // 
            // ckbPermissaoGarcom
            // 
            this.ckbPermissaoGarcom.AutoSize = true;
            this.ckbPermissaoGarcom.Location = new System.Drawing.Point(322, 24);
            this.ckbPermissaoGarcom.Name = "ckbPermissaoGarcom";
            this.ckbPermissaoGarcom.Size = new System.Drawing.Size(80, 21);
            this.ckbPermissaoGarcom.TabIndex = 3;
            this.ckbPermissaoGarcom.Text = "Garçom";
            this.ckbPermissaoGarcom.UseVisualStyleBackColor = true;
            // 
            // ckbPermissaoCaixa
            // 
            this.ckbPermissaoCaixa.AutoSize = true;
            this.ckbPermissaoCaixa.Location = new System.Drawing.Point(244, 24);
            this.ckbPermissaoCaixa.Name = "ckbPermissaoCaixa";
            this.ckbPermissaoCaixa.Size = new System.Drawing.Size(64, 21);
            this.ckbPermissaoCaixa.TabIndex = 2;
            this.ckbPermissaoCaixa.Text = "Caixa";
            this.ckbPermissaoCaixa.UseVisualStyleBackColor = true;
            // 
            // ckbPermissaoGerente
            // 
            this.ckbPermissaoGerente.AutoSize = true;
            this.ckbPermissaoGerente.Location = new System.Drawing.Point(151, 24);
            this.ckbPermissaoGerente.Name = "ckbPermissaoGerente";
            this.ckbPermissaoGerente.Size = new System.Drawing.Size(79, 21);
            this.ckbPermissaoGerente.TabIndex = 1;
            this.ckbPermissaoGerente.Text = "Gerente";
            this.ckbPermissaoGerente.UseVisualStyleBackColor = true;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(12, 231);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(92, 47);
            this.btnSalvar.TabIndex = 5;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(110, 231);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(92, 47);
            this.btnCancelar.TabIndex = 6;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // ckbAtivo
            // 
            this.ckbAtivo.AutoSize = true;
            this.ckbAtivo.Location = new System.Drawing.Point(15, 204);
            this.ckbAtivo.Name = "ckbAtivo";
            this.ckbAtivo.Size = new System.Drawing.Size(58, 21);
            this.ckbAtivo.TabIndex = 4;
            this.ckbAtivo.Text = "Ativo";
            this.ckbAtivo.UseVisualStyleBackColor = true;
            // 
            // frmUsuarioEditar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 292);
            this.Controls.Add(this.ckbAtivo);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSenha2);
            this.Controls.Add(this.txtSenha1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmUsuarioEditar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmUsuarioEditar_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.TextBox txtSenha1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSenha2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox ckbPermissaoAdm;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ckbPermissaoGarcom;
        private System.Windows.Forms.CheckBox ckbPermissaoCaixa;
        private System.Windows.Forms.CheckBox ckbPermissaoGerente;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.CheckBox ckbAtivo;
    }
}