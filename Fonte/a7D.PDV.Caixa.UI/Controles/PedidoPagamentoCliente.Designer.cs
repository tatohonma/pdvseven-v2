namespace a7D.PDV.Caixa.UI.Controles
{
    partial class PedidoPagamentoCliente
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PedidoPagamentoCliente));
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtNomeCliente = new System.Windows.Forms.TextBox();
            this.btnSelecionarCliente = new System.Windows.Forms.Button();
            this.lblDocumento = new System.Windows.Forms.Label();
            this.txtDocumentoCliente = new System.Windows.Forms.TextBox();
            this.ckbEnviarEmail = new System.Windows.Forms.CheckBox();
            this.ckbNF = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTeclado = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtEmail
            // 
            this.txtEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(3, 7);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtEmail.MaxLength = 50;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(227, 19);
            this.txtEmail.TabIndex = 3;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Arial", 10F);
            this.lblEmail.Location = new System.Drawing.Point(7, 104);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(42, 16);
            this.lblEmail.TabIndex = 97;
            this.lblEmail.Text = "Email";
            // 
            // txtNomeCliente
            // 
            this.txtNomeCliente.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNomeCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtNomeCliente.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNomeCliente.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeCliente.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtNomeCliente.Location = new System.Drawing.Point(7, 22);
            this.txtNomeCliente.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNomeCliente.Name = "txtNomeCliente";
            this.txtNomeCliente.ReadOnly = true;
            this.txtNomeCliente.Size = new System.Drawing.Size(233, 28);
            this.txtNomeCliente.TabIndex = 93;
            this.txtNomeCliente.TabStop = false;
            this.txtNomeCliente.DoubleClick += new System.EventHandler(this.btnSelecionarCliente_Click);
            // 
            // btnSelecionarCliente
            // 
            this.btnSelecionarCliente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelecionarCliente.AutoSize = true;
            this.btnSelecionarCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(131)))), ((int)(((byte)(159)))));
            this.btnSelecionarCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelecionarCliente.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.btnSelecionarCliente.ForeColor = System.Drawing.Color.White;
            this.btnSelecionarCliente.Location = new System.Drawing.Point(257, 22);
            this.btnSelecionarCliente.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelecionarCliente.Name = "btnSelecionarCliente";
            this.btnSelecionarCliente.Size = new System.Drawing.Size(129, 37);
            this.btnSelecionarCliente.TabIndex = 0;
            this.btnSelecionarCliente.Text = "&SELECIONAR";
            this.btnSelecionarCliente.UseVisualStyleBackColor = false;
            this.btnSelecionarCliente.Click += new System.EventHandler(this.btnSelecionarCliente_Click);
            // 
            // lblDocumento
            // 
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Font = new System.Drawing.Font("Arial", 10F);
            this.lblDocumento.Location = new System.Drawing.Point(3, 53);
            this.lblDocumento.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Size = new System.Drawing.Size(74, 16);
            this.lblDocumento.TabIndex = 94;
            this.lblDocumento.Text = "CPF/CNPJ";
            // 
            // txtDocumentoCliente
            // 
            this.txtDocumentoCliente.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocumentoCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDocumentoCliente.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDocumentoCliente.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentoCliente.Location = new System.Drawing.Point(7, 73);
            this.txtDocumentoCliente.MaxLength = 20;
            this.txtDocumentoCliente.Name = "txtDocumentoCliente";
            this.txtDocumentoCliente.Size = new System.Drawing.Size(191, 28);
            this.txtDocumentoCliente.TabIndex = 1;
            this.txtDocumentoCliente.DoubleClick += new System.EventHandler(this.txtDocumentoCliente_DoubleClick);
            this.txtDocumentoCliente.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDocumentoCliente_KeyPress);
            this.txtDocumentoCliente.Leave += new System.EventHandler(this.txtDocumentoCliente_Leave);
            // 
            // ckbEnviarEmail
            // 
            this.ckbEnviarEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbEnviarEmail.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckbEnviarEmail.Location = new System.Drawing.Point(257, 130);
            this.ckbEnviarEmail.Margin = new System.Windows.Forms.Padding(4);
            this.ckbEnviarEmail.Name = "ckbEnviarEmail";
            this.ckbEnviarEmail.Size = new System.Drawing.Size(129, 23);
            this.ckbEnviarEmail.TabIndex = 4;
            this.ckbEnviarEmail.Text = "Email";
            this.ckbEnviarEmail.UseVisualStyleBackColor = true;
            this.ckbEnviarEmail.CheckedChanged += new System.EventHandler(this.ckbEnviarEmail_CheckedChanged);
            // 
            // ckbNF
            // 
            this.ckbNF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbNF.Checked = true;
            this.ckbNF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbNF.Font = new System.Drawing.Font("Arial", 9F);
            this.ckbNF.Location = new System.Drawing.Point(257, 82);
            this.ckbNF.Margin = new System.Windows.Forms.Padding(3, 10, 3, 2);
            this.ckbNF.Name = "ckbNF";
            this.ckbNF.Size = new System.Drawing.Size(129, 21);
            this.ckbNF.TabIndex = 2;
            this.ckbNF.Text = "CPF na nota";
            this.ckbNF.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Location = new System.Drawing.Point(7, 124);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(233, 34);
            this.panel1.TabIndex = 99;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTeclado);
            this.groupBox1.Controls.Add(this.ckbEnviarEmail);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.ckbNF);
            this.groupBox1.Controls.Add(this.btnSelecionarCliente);
            this.groupBox1.Controls.Add(this.txtDocumentoCliente);
            this.groupBox1.Controls.Add(this.lblDocumento);
            this.groupBox1.Controls.Add(this.lblEmail);
            this.groupBox1.Controls.Add(this.txtNomeCliente);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(393, 166);
            this.groupBox1.TabIndex = 100;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cliente";
            // 
            // btnTeclado
            // 
            this.btnTeclado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTeclado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnTeclado.Font = new System.Drawing.Font("Arial", 18F);
            this.btnTeclado.ForeColor = System.Drawing.Color.Transparent;
            this.btnTeclado.Image = ((System.Drawing.Image)(resources.GetObject("btnTeclado.Image")));
            this.btnTeclado.Location = new System.Drawing.Point(200, 73);
            this.btnTeclado.Name = "btnTeclado";
            this.btnTeclado.Size = new System.Drawing.Size(40, 28);
            this.btnTeclado.TabIndex = 100;
            this.btnTeclado.TabStop = false;
            this.btnTeclado.UseVisualStyleBackColor = false;
            this.btnTeclado.Click += new System.EventHandler(this.btnTeclado_Click);
            // 
            // PedidoPagamentoCliente
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.Name = "PedidoPagamentoCliente";
            this.Size = new System.Drawing.Size(393, 166);
            this.Load += new System.EventHandler(this.PedidoPagamentoCliente_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtNomeCliente;
        private System.Windows.Forms.Button btnSelecionarCliente;
        private System.Windows.Forms.Label lblDocumento;
        private System.Windows.Forms.TextBox txtDocumentoCliente;
        private System.Windows.Forms.CheckBox ckbEnviarEmail;
        private System.Windows.Forms.CheckBox ckbNF;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnTeclado;
    }
}
