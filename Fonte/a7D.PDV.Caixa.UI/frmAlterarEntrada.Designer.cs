namespace a7D.PDV.Caixa.UI
{
    partial class frmAlterarEntrada
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
            this.txtCodigoComanda = new System.Windows.Forms.TextBox();
            this.txtTelefone1Numero = new System.Windows.Forms.TextBox();
            this.btnPesquisar = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTelefone1DDD = new System.Windows.Forms.TextBox();
            this.btnAlterarTipoEntrada = new System.Windows.Forms.Button();
            this.txtNomeCompleto = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbbTipoEntrada = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCodigoComanda
            // 
            this.txtCodigoComanda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtCodigoComanda.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCodigoComanda.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoComanda.Location = new System.Drawing.Point(14, 97);
            this.txtCodigoComanda.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigoComanda.Name = "txtCodigoComanda";
            this.txtCodigoComanda.Size = new System.Drawing.Size(322, 28);
            this.txtCodigoComanda.TabIndex = 0;
            this.txtCodigoComanda.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodigoComanda_KeyDown);
            // 
            // txtTelefone1Numero
            // 
            this.txtTelefone1Numero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtTelefone1Numero.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTelefone1Numero.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefone1Numero.Location = new System.Drawing.Point(357, 42);
            this.txtTelefone1Numero.Margin = new System.Windows.Forms.Padding(4);
            this.txtTelefone1Numero.Name = "txtTelefone1Numero";
            this.txtTelefone1Numero.ReadOnly = true;
            this.txtTelefone1Numero.Size = new System.Drawing.Size(123, 28);
            this.txtTelefone1Numero.TabIndex = 1;
            this.txtTelefone1Numero.TabStop = false;
            // 
            // btnPesquisar
            // 
            this.btnPesquisar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(131)))), ((int)(((byte)(159)))));
            this.btnPesquisar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPesquisar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPesquisar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPesquisar.ForeColor = System.Drawing.Color.White;
            this.btnPesquisar.Location = new System.Drawing.Point(347, 77);
            this.btnPesquisar.Name = "btnPesquisar";
            this.btnPesquisar.Size = new System.Drawing.Size(161, 49);
            this.btnPesquisar.TabIndex = 1;
            this.btnPesquisar.Text = "PESQUISAR";
            this.btnPesquisar.UseVisualStyleBackColor = false;
            this.btnPesquisar.Click += new System.EventHandler(this.btnPesquisar_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(314, 21);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 17);
            this.label5.TabIndex = 49;
            this.label5.Text = "Telefone";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 77);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 17);
            this.label4.TabIndex = 88;
            this.label4.Text = "Código da Comanda";
            // 
            // txtTelefone1DDD
            // 
            this.txtTelefone1DDD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtTelefone1DDD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTelefone1DDD.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefone1DDD.Location = new System.Drawing.Point(317, 42);
            this.txtTelefone1DDD.Margin = new System.Windows.Forms.Padding(4);
            this.txtTelefone1DDD.Name = "txtTelefone1DDD";
            this.txtTelefone1DDD.ReadOnly = true;
            this.txtTelefone1DDD.Size = new System.Drawing.Size(38, 28);
            this.txtTelefone1DDD.TabIndex = 1;
            this.txtTelefone1DDD.TabStop = false;
            // 
            // btnAlterarTipoEntrada
            // 
            this.btnAlterarTipoEntrada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnAlterarTipoEntrada.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAlterarTipoEntrada.Enabled = false;
            this.btnAlterarTipoEntrada.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlterarTipoEntrada.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAlterarTipoEntrada.ForeColor = System.Drawing.Color.White;
            this.btnAlterarTipoEntrada.Location = new System.Drawing.Point(13, 284);
            this.btnAlterarTipoEntrada.Margin = new System.Windows.Forms.Padding(4);
            this.btnAlterarTipoEntrada.Name = "btnAlterarTipoEntrada";
            this.btnAlterarTipoEntrada.Size = new System.Drawing.Size(494, 59);
            this.btnAlterarTipoEntrada.TabIndex = 3;
            this.btnAlterarTipoEntrada.Text = "CONFIRMAR";
            this.btnAlterarTipoEntrada.UseVisualStyleBackColor = false;
            this.btnAlterarTipoEntrada.Click += new System.EventHandler(this.btnAlterarTipoEntrada_Click);
            // 
            // txtNomeCompleto
            // 
            this.txtNomeCompleto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtNomeCompleto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNomeCompleto.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeCompleto.Location = new System.Drawing.Point(7, 42);
            this.txtNomeCompleto.Margin = new System.Windows.Forms.Padding(4);
            this.txtNomeCompleto.Name = "txtNomeCompleto";
            this.txtNomeCompleto.ReadOnly = true;
            this.txtNomeCompleto.Size = new System.Drawing.Size(302, 28);
            this.txtNomeCompleto.TabIndex = 0;
            this.txtNomeCompleto.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 17);
            this.label2.TabIndex = 47;
            this.label2.Text = "Nome";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtTelefone1Numero);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtTelefone1DDD);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtNomeCompleto);
            this.groupBox1.Location = new System.Drawing.Point(13, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(494, 82);
            this.groupBox1.TabIndex = 85;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cliente";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 223);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 17);
            this.label12.TabIndex = 91;
            this.label12.Text = "Tipo entrada";
            // 
            // cbbTipoEntrada
            // 
            this.cbbTipoEntrada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbbTipoEntrada.DisplayMember = "Nome";
            this.cbbTipoEntrada.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTipoEntrada.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbTipoEntrada.Font = new System.Drawing.Font("Arial", 14F);
            this.cbbTipoEntrada.FormattingEnabled = true;
            this.cbbTipoEntrada.ItemHeight = 22;
            this.cbbTipoEntrada.Location = new System.Drawing.Point(13, 242);
            this.cbbTipoEntrada.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.cbbTipoEntrada.Name = "cbbTipoEntrada";
            this.cbbTipoEntrada.Size = new System.Drawing.Size(494, 30);
            this.cbbTipoEntrada.TabIndex = 2;
            this.cbbTipoEntrada.ValueMember = "IDTipoEntrada";
            // 
            // frmAlterarEntrada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(520, 360);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cbbTipoEntrada);
            this.Controls.Add(this.txtCodigoComanda);
            this.Controls.Add(this.btnPesquisar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAlterarTipoEntrada);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(520, 360);
            this.MinimumSize = new System.Drawing.Size(520, 360);
            this.Name = "frmAlterarEntrada";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STATUS DA COMANDA";
            this.Load += new System.EventHandler(this.frmAlterarEntrada_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCodigoComanda;
        private System.Windows.Forms.TextBox txtTelefone1Numero;
        private System.Windows.Forms.Button btnPesquisar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTelefone1DDD;
        private System.Windows.Forms.Button btnAlterarTipoEntrada;
        private System.Windows.Forms.TextBox txtNomeCompleto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbbTipoEntrada;
    }
}