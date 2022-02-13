namespace a7D.PDV.Terminal.UI
{
    partial class frmPedidoProdutoCancelamento
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
            this.btnSalvar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescricao = new System.Windows.Forms.TextBox();
            this.cbbMotivoCancelamento = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblProduto = new System.Windows.Forms.Label();
            this.ckbRetornar = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSalvar
            // 
            this.btnSalvar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnSalvar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(16, 314);
            this.btnSalvar.Margin = new System.Windows.Forms.Padding(4);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(491, 59);
            this.btnSalvar.TabIndex = 41;
            this.btnSalvar.Text = "CONFIRMAR";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 162);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 42;
            this.label2.Text = "Descrição";
            // 
            // txtDescricao
            // 
            this.txtDescricao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDescricao.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescricao.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescricao.Location = new System.Drawing.Point(16, 183);
            this.txtDescricao.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescricao.Multiline = true;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(487, 91);
            this.txtDescricao.TabIndex = 40;
            // 
            // cbbMotivoCancelamento
            // 
            this.cbbMotivoCancelamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbbMotivoCancelamento.DisplayMember = "Nome";
            this.cbbMotivoCancelamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbMotivoCancelamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbMotivoCancelamento.Font = new System.Drawing.Font("Arial", 14F);
            this.cbbMotivoCancelamento.FormattingEnabled = true;
            this.cbbMotivoCancelamento.ItemHeight = 22;
            this.cbbMotivoCancelamento.Location = new System.Drawing.Point(16, 123);
            this.cbbMotivoCancelamento.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.cbbMotivoCancelamento.Name = "cbbMotivoCancelamento";
            this.cbbMotivoCancelamento.Size = new System.Drawing.Size(487, 30);
            this.cbbMotivoCancelamento.TabIndex = 44;
            this.cbbMotivoCancelamento.ValueMember = "IDMotivoCancelamento";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 105);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 45;
            this.label1.Text = "Motivo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 77);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 17);
            this.label4.TabIndex = 46;
            this.label4.Text = "Venda cancelada: ";
            // 
            // lblProduto
            // 
            this.lblProduto.AutoSize = true;
            this.lblProduto.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProduto.Location = new System.Drawing.Point(137, 77);
            this.lblProduto.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProduto.Name = "lblProduto";
            this.lblProduto.Size = new System.Drawing.Size(32, 18);
            this.lblProduto.TabIndex = 47;
            this.lblProduto.Text = "xxx";
            // 
            // ckbRetornar
            // 
            this.ckbRetornar.AutoSize = true;
            this.ckbRetornar.Checked = true;
            this.ckbRetornar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbRetornar.Location = new System.Drawing.Point(16, 281);
            this.ckbRetornar.Name = "ckbRetornar";
            this.ckbRetornar.Size = new System.Drawing.Size(160, 21);
            this.ckbRetornar.TabIndex = 48;
            this.ckbRetornar.Text = "Retornar ao estoque";
            this.ckbRetornar.UseVisualStyleBackColor = true;
            this.ckbRetornar.Visible = false;
            // 
            // frmPedidoProdutoCancelamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(516, 384);
            this.Controls.Add(this.ckbRetornar);
            this.Controls.Add(this.lblProduto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbbMotivoCancelamento);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescricao);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmPedidoProdutoCancelamento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CANCELAMENTO";
            this.Load += new System.EventHandler(this.frmPedidoProdutoCancelamento_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDescricao;
        private System.Windows.Forms.ComboBox cbbMotivoCancelamento;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblProduto;
        private System.Windows.Forms.CheckBox ckbRetornar;
    }
}