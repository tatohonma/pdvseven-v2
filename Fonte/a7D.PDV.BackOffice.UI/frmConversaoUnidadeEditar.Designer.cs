namespace a7D.PDV.BackOffice.UI
{
    partial class frmConversaoUnidadeEditar
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
            this.cbbUnidadeDe = new System.Windows.Forms.ComboBox();
            this.cbbUnidadePara = new System.Windows.Forms.ComboBox();
            this.txtDivisao = new System.Windows.Forms.TextBox();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMultiplicacao = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cbbUnidadeDe
            // 
            this.cbbUnidadeDe.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbbUnidadeDe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbUnidadeDe.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbUnidadeDe.FormattingEnabled = true;
            this.cbbUnidadeDe.Location = new System.Drawing.Point(96, 73);
            this.cbbUnidadeDe.Name = "cbbUnidadeDe";
            this.cbbUnidadeDe.Size = new System.Drawing.Size(220, 29);
            this.cbbUnidadeDe.TabIndex = 1;
            // 
            // cbbUnidadePara
            // 
            this.cbbUnidadePara.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbbUnidadePara.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbUnidadePara.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbUnidadePara.FormattingEnabled = true;
            this.cbbUnidadePara.Location = new System.Drawing.Point(543, 73);
            this.cbbUnidadePara.Name = "cbbUnidadePara";
            this.cbbUnidadePara.Size = new System.Drawing.Size(220, 29);
            this.cbbUnidadePara.TabIndex = 3;
            // 
            // txtDivisao
            // 
            this.txtDivisao.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDivisao.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDivisao.Location = new System.Drawing.Point(446, 73);
            this.txtDivisao.Name = "txtDivisao";
            this.txtDivisao.Size = new System.Drawing.Size(91, 29);
            this.txtDivisao.TabIndex = 4;
            this.txtDivisao.Text = " ";
            this.txtDivisao.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDivisao_KeyPress);
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(17, 166);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(107, 55);
            this.btnSalvar.TabIndex = 8;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(130, 166);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(107, 55);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 22);
            this.label4.TabIndex = 10;
            this.label4.Text = "Um(a)";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(322, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 22);
            this.label5.TabIndex = 11;
            this.label5.Text = " é igual à ";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(769, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 22);
            this.label6.TabIndex = 12;
            this.label6.Text = "(s).";
            // 
            // txtMultiplicacao
            // 
            this.txtMultiplicacao.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMultiplicacao.Location = new System.Drawing.Point(446, 166);
            this.txtMultiplicacao.Name = "txtMultiplicacao";
            this.txtMultiplicacao.Size = new System.Drawing.Size(91, 29);
            this.txtMultiplicacao.TabIndex = 13;
            this.txtMultiplicacao.Text = " ";
            this.txtMultiplicacao.Visible = false;
            // 
            // frmConversaoUnidadeEditar
            // 
            this.AcceptButton = this.btnSalvar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(823, 233);
            this.Controls.Add(this.txtMultiplicacao);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.txtDivisao);
            this.Controls.Add(this.cbbUnidadePara);
            this.Controls.Add(this.cbbUnidadeDe);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConversaoUnidadeEditar";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Conversão de Unidade";
            this.Load += new System.EventHandler(this.frmConversaoUnidadeEditar_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbbUnidadeDe;
        private System.Windows.Forms.ComboBox cbbUnidadePara;
        private System.Windows.Forms.TextBox txtDivisao;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMultiplicacao;
    }
}