namespace a7D.PDV.Caixa.UI
{
    partial class frmAjusteCaixa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAjusteCaixa));
            this.txtValor = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdbSangria = new System.Windows.Forms.RadioButton();
            this.rdbSuprimento = new System.Windows.Forms.RadioButton();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAjustar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtValor
            // 
            this.txtValor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtValor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtValor.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValor.Location = new System.Drawing.Point(208, 109);
            this.txtValor.Margin = new System.Windows.Forms.Padding(4);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(132, 22);
            this.txtValor.TabIndex = 2;
            this.txtValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValor_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(204, 86);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Valor (R$)";
            // 
            // rdbSangria
            // 
            this.rdbSangria.AutoSize = true;
            this.rdbSangria.Checked = true;
            this.rdbSangria.Location = new System.Drawing.Point(16, 84);
            this.rdbSangria.Margin = new System.Windows.Forms.Padding(4);
            this.rdbSangria.Name = "rdbSangria";
            this.rdbSangria.Size = new System.Drawing.Size(76, 21);
            this.rdbSangria.TabIndex = 0;
            this.rdbSangria.TabStop = true;
            this.rdbSangria.Text = "Sangria";
            this.rdbSangria.UseVisualStyleBackColor = true;
            // 
            // rdbSuprimento
            // 
            this.rdbSuprimento.AutoSize = true;
            this.rdbSuprimento.Location = new System.Drawing.Point(16, 114);
            this.rdbSuprimento.Margin = new System.Windows.Forms.Padding(4);
            this.rdbSuprimento.Name = "rdbSuprimento";
            this.rdbSuprimento.Size = new System.Drawing.Size(101, 21);
            this.rdbSuprimento.TabIndex = 1;
            this.rdbSuprimento.Text = "Suprimento";
            this.rdbSuprimento.UseVisualStyleBackColor = true;
            // 
            // txtDescricao
            // 
            this.txtDescricao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDescricao.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescricao.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescricao.Location = new System.Drawing.Point(16, 172);
            this.txtDescricao.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescricao.Multiline = true;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(487, 98);
            this.txtDescricao.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 151);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Descrição";
            // 
            // btnAjustar
            // 
            this.btnAjustar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnAjustar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAjustar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAjustar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAjustar.ForeColor = System.Drawing.Color.White;
            this.btnAjustar.Location = new System.Drawing.Point(13, 278);
            this.btnAjustar.Margin = new System.Windows.Forms.Padding(4);
            this.btnAjustar.Name = "btnAjustar";
            this.btnAjustar.Size = new System.Drawing.Size(491, 59);
            this.btnAjustar.TabIndex = 4;
            this.btnAjustar.Text = "CONFIRMAR";
            this.btnAjustar.UseVisualStyleBackColor = false;
            this.btnAjustar.Click += new System.EventHandler(this.btnSangria_Click);
            // 
            // frmAjusteCaixa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(520, 350);
            this.Controls.Add(this.btnAjustar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescricao);
            this.Controls.Add(this.rdbSuprimento);
            this.Controls.Add(this.rdbSangria);
            this.Controls.Add(this.txtValor);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(520, 350);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(520, 350);
            this.Name = "frmAjusteCaixa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AJUSTE DE CAIXA";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdbSangria;
        private System.Windows.Forms.RadioButton rdbSuprimento;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAjustar;
    }
}