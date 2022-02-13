namespace a7D.PDV.Ativacao.UI
{
    partial class frmGerarChaveAtivacao
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
            this.txtCodigoRevenda = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGerarCodigoAtivacao = new System.Windows.Forms.Button();
            this.txtChaveAtivacao = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCodigoRevenda
            // 
            this.txtCodigoRevenda.Location = new System.Drawing.Point(6, 42);
            this.txtCodigoRevenda.MaxLength = 4;
            this.txtCodigoRevenda.Name = "txtCodigoRevenda";
            this.txtCodigoRevenda.Size = new System.Drawing.Size(82, 20);
            this.txtCodigoRevenda.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Código revenda";
            // 
            // btnGerarCodigoAtivacao
            // 
            this.btnGerarCodigoAtivacao.Location = new System.Drawing.Point(94, 40);
            this.btnGerarCodigoAtivacao.Name = "btnGerarCodigoAtivacao";
            this.btnGerarCodigoAtivacao.Size = new System.Drawing.Size(112, 23);
            this.btnGerarCodigoAtivacao.TabIndex = 5;
            this.btnGerarCodigoAtivacao.Text = "Gerar";
            this.btnGerarCodigoAtivacao.UseVisualStyleBackColor = true;
            this.btnGerarCodigoAtivacao.Click += new System.EventHandler(this.btnGerarCodigoAtivacao_Click);
            // 
            // txtChaveAtivacao
            // 
            this.txtChaveAtivacao.Location = new System.Drawing.Point(6, 69);
            this.txtChaveAtivacao.Name = "txtChaveAtivacao";
            this.txtChaveAtivacao.Size = new System.Drawing.Size(200, 20);
            this.txtChaveAtivacao.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtChaveAtivacao);
            this.groupBox1.Controls.Add(this.txtCodigoRevenda);
            this.groupBox1.Controls.Add(this.btnGerarCodigoAtivacao);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gerar Chave Ativação";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 126);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtCodigoRevenda;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGerarCodigoAtivacao;
        private System.Windows.Forms.TextBox txtChaveAtivacao;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

