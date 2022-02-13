namespace a7D.PDV.Caixa.UI
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
            this.gbRetornarAoEstoque = new System.Windows.Forms.GroupBox();
            this.rbNao = new System.Windows.Forms.RadioButton();
            this.rbSim = new System.Windows.Forms.RadioButton();
            this.gbRetornarAoEstoque.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSalvar
            // 
            this.btnSalvar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnSalvar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(11, 347);
            this.btnSalvar.Margin = new System.Windows.Forms.Padding(2);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(378, 42);
            this.btnSalvar.TabIndex = 3;
            this.btnSalvar.Text = "CONFIRMAR";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 11.25F);
            this.label2.Location = new System.Drawing.Point(11, 156);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
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
            this.txtDescricao.Location = new System.Drawing.Point(12, 175);
            this.txtDescricao.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescricao.Multiline = true;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(377, 92);
            this.txtDescricao.TabIndex = 1;
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
            this.cbbMotivoCancelamento.Location = new System.Drawing.Point(14, 121);
            this.cbbMotivoCancelamento.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbbMotivoCancelamento.Name = "cbbMotivoCancelamento";
            this.cbbMotivoCancelamento.Size = new System.Drawing.Size(375, 30);
            this.cbbMotivoCancelamento.TabIndex = 0;
            this.cbbMotivoCancelamento.ValueMember = "IDMotivoCancelamento";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.label1.Location = new System.Drawing.Point(11, 99);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 45;
            this.label1.Text = "Motivo";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 11.25F);
            this.label4.Location = new System.Drawing.Point(11, 69);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 17);
            this.label4.TabIndex = 46;
            this.label4.Text = "Venda cancelada: ";
            // 
            // lblProduto
            // 
            this.lblProduto.AutoSize = true;
            this.lblProduto.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProduto.Location = new System.Drawing.Point(142, 69);
            this.lblProduto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProduto.Name = "lblProduto";
            this.lblProduto.Size = new System.Drawing.Size(32, 18);
            this.lblProduto.TabIndex = 47;
            this.lblProduto.Text = "xxx";
            // 
            // gbRetornarAoEstoque
            // 
            this.gbRetornarAoEstoque.Controls.Add(this.rbNao);
            this.gbRetornarAoEstoque.Controls.Add(this.rbSim);
            this.gbRetornarAoEstoque.Font = new System.Drawing.Font("Arial", 11.25F);
            this.gbRetornarAoEstoque.Location = new System.Drawing.Point(12, 272);
            this.gbRetornarAoEstoque.Name = "gbRetornarAoEstoque";
            this.gbRetornarAoEstoque.Size = new System.Drawing.Size(377, 70);
            this.gbRetornarAoEstoque.TabIndex = 49;
            this.gbRetornarAoEstoque.TabStop = false;
            this.gbRetornarAoEstoque.Text = "Retornar item ao estoque?";
            // 
            // rbNao
            // 
            this.rbNao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbNao.AutoSize = true;
            this.rbNao.Location = new System.Drawing.Point(314, 25);
            this.rbNao.Name = "rbNao";
            this.rbNao.Size = new System.Drawing.Size(57, 21);
            this.rbNao.TabIndex = 1;
            this.rbNao.Text = "NÃO";
            this.rbNao.UseVisualStyleBackColor = true;
            // 
            // rbSim
            // 
            this.rbSim.AutoSize = true;
            this.rbSim.Checked = true;
            this.rbSim.Location = new System.Drawing.Point(16, 25);
            this.rbSim.Name = "rbSim";
            this.rbSim.Size = new System.Drawing.Size(50, 21);
            this.rbSim.TabIndex = 0;
            this.rbSim.TabStop = true;
            this.rbSim.Text = "SIM";
            this.rbSim.UseVisualStyleBackColor = true;
            // 
            // frmPedidoProdutoCancelamento
            // 
            this.AcceptButton = this.btnSalvar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.gbRetornarAoEstoque);
            this.Controls.Add(this.lblProduto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbbMotivoCancelamento);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescricao);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 400);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "frmPedidoProdutoCancelamento";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CANCELAMENTO";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPedidoProdutoCancelamento_Load);
            this.gbRetornarAoEstoque.ResumeLayout(false);
            this.gbRetornarAoEstoque.PerformLayout();
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
        private System.Windows.Forms.GroupBox gbRetornarAoEstoque;
        private System.Windows.Forms.RadioButton rbNao;
        private System.Windows.Forms.RadioButton rbSim;
    }
}