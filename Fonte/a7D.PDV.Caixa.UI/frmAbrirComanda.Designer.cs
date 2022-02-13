namespace a7D.PDV.Caixa.UI
{
    partial class frmAbrirComanda
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbrirComanda));
            this.btnAbrirComanda = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbbEstado = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCodigoComanda = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNomeCompleto = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDataNascimento = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtRG = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtObservacao = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtCidade = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtBairro = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtComplemento = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtEnderecoNumero = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtEndereco = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDocumento1 = new System.Windows.Forms.TextBox();
            this.txtTelefone1Numero = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTelefone1DDD = new System.Windows.Forms.TextBox();
            this.lblTipoEntrada = new System.Windows.Forms.Label();
            this.cbbTipoEntrada = new System.Windows.Forms.ComboBox();
            this.txtCodigoEntrada = new System.Windows.Forms.TextBox();
            this.ckbBloqueado = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAbrirComanda
            // 
            this.btnAbrirComanda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnAbrirComanda.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAbrirComanda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirComanda.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbrirComanda.ForeColor = System.Drawing.Color.White;
            this.btnAbrirComanda.Location = new System.Drawing.Point(10, 557);
            this.btnAbrirComanda.Margin = new System.Windows.Forms.Padding(4);
            this.btnAbrirComanda.Name = "btnAbrirComanda";
            this.btnAbrirComanda.Size = new System.Drawing.Size(491, 59);
            this.btnAbrirComanda.TabIndex = 5;
            this.btnAbrirComanda.Text = "&ABRIR COMANDA";
            this.btnAbrirComanda.UseVisualStyleBackColor = false;
            this.btnAbrirComanda.Click += new System.EventHandler(this.btnAbrirComanda_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(424, 181);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 17);
            this.label1.TabIndex = 45;
            this.label1.Text = "UF";
            // 
            // cbbEstado
            // 
            this.cbbEstado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbbEstado.DisplayMember = "Sigla";
            this.cbbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbEstado.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbEstado.Font = new System.Drawing.Font("Arial", 14F);
            this.cbbEstado.FormattingEnabled = true;
            this.cbbEstado.ItemHeight = 22;
            this.cbbEstado.Location = new System.Drawing.Point(427, 199);
            this.cbbEstado.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.cbbEstado.Name = "cbbEstado";
            this.cbbEstado.Size = new System.Drawing.Size(57, 30);
            this.cbbEstado.TabIndex = 9;
            this.cbbEstado.ValueMember = "IDEstado";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 73);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 17);
            this.label4.TabIndex = 44;
            this.label4.Text = "Comanda";
            // 
            // txtCodigoComanda
            // 
            this.txtCodigoComanda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtCodigoComanda.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCodigoComanda.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoComanda.Location = new System.Drawing.Point(13, 93);
            this.txtCodigoComanda.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigoComanda.MaxLength = 16;
            this.txtCodigoComanda.Name = "txtCodigoComanda";
            this.txtCodigoComanda.Size = new System.Drawing.Size(211, 28);
            this.txtCodigoComanda.TabIndex = 0;
            this.txtCodigoComanda.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ApenasNumero_KeyPress);
            this.txtCodigoComanda.Leave += new System.EventHandler(this.txtCodigoComanda_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 17);
            this.label2.TabIndex = 47;
            this.label2.Text = "Nome do cliente";
            // 
            // txtNomeCompleto
            // 
            this.txtNomeCompleto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtNomeCompleto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNomeCompleto.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeCompleto.Location = new System.Drawing.Point(7, 41);
            this.txtNomeCompleto.Margin = new System.Windows.Forms.Padding(4);
            this.txtNomeCompleto.MaxLength = 100;
            this.txtNomeCompleto.Name = "txtNomeCompleto";
            this.txtNomeCompleto.Size = new System.Drawing.Size(302, 28);
            this.txtNomeCompleto.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDataNascimento);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtRG);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtEmail);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtObservacao);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtCidade);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtBairro);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtComplemento);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtEnderecoNumero);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtEndereco);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbbEstado);
            this.groupBox1.Controls.Add(this.txtDocumento1);
            this.groupBox1.Controls.Add(this.txtTelefone1Numero);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtTelefone1DDD);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtNomeCompleto);
            this.groupBox1.Location = new System.Drawing.Point(12, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(491, 384);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cliente";
            // 
            // txtDataNascimento
            // 
            this.txtDataNascimento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDataNascimento.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDataNascimento.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDataNascimento.Location = new System.Drawing.Point(347, 259);
            this.txtDataNascimento.Margin = new System.Windows.Forms.Padding(4);
            this.txtDataNascimento.MaxLength = 10;
            this.txtDataNascimento.Name = "txtDataNascimento";
            this.txtDataNascimento.Size = new System.Drawing.Size(137, 28);
            this.txtDataNascimento.TabIndex = 78;
            this.txtDataNascimento.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDataNascimento_KeyPress);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(347, 238);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(139, 17);
            this.label15.TabIndex = 79;
            this.label15.Text = "Data de nascimento";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(263, 73);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 17);
            this.label13.TabIndex = 77;
            this.label13.Text = "RG";
            // 
            // txtRG
            // 
            this.txtRG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtRG.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRG.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRG.Location = new System.Drawing.Point(266, 94);
            this.txtRG.Margin = new System.Windows.Forms.Padding(4);
            this.txtRG.MaxLength = 14;
            this.txtRG.Name = "txtRG";
            this.txtRG.Size = new System.Drawing.Size(213, 28);
            this.txtRG.TabIndex = 76;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 238);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 17);
            this.label12.TabIndex = 72;
            this.label12.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEmail.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmail.Location = new System.Drawing.Point(7, 259);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmail.MaxLength = 100;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(332, 28);
            this.txtEmail.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 298);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(96, 17);
            this.label14.TabIndex = 70;
            this.label14.Text = "Observações";
            // 
            // txtObservacao
            // 
            this.txtObservacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtObservacao.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtObservacao.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacao.Location = new System.Drawing.Point(7, 318);
            this.txtObservacao.Margin = new System.Windows.Forms.Padding(4);
            this.txtObservacao.MaxLength = 500;
            this.txtObservacao.Multiline = true;
            this.txtObservacao.Name = "txtObservacao";
            this.txtObservacao.Size = new System.Drawing.Size(477, 57);
            this.txtObservacao.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(239, 179);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 17);
            this.label11.TabIndex = 63;
            this.label11.Text = "Cidade";
            // 
            // txtCidade
            // 
            this.txtCidade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtCidade.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCidade.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCidade.Location = new System.Drawing.Point(239, 200);
            this.txtCidade.Margin = new System.Windows.Forms.Padding(4);
            this.txtCidade.MaxLength = 100;
            this.txtCidade.Name = "txtCidade";
            this.txtCidade.Size = new System.Drawing.Size(177, 28);
            this.txtCidade.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 179);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 17);
            this.label10.TabIndex = 61;
            this.label10.Text = "Bairro";
            // 
            // txtBairro
            // 
            this.txtBairro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtBairro.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBairro.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBairro.Location = new System.Drawing.Point(7, 200);
            this.txtBairro.Margin = new System.Windows.Forms.Padding(4);
            this.txtBairro.MaxLength = 100;
            this.txtBairro.Name = "txtBairro";
            this.txtBairro.Size = new System.Drawing.Size(224, 28);
            this.txtBairro.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(387, 126);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 17);
            this.label9.TabIndex = 59;
            this.label9.Text = "Complemento";
            // 
            // txtComplemento
            // 
            this.txtComplemento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtComplemento.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtComplemento.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtComplemento.Location = new System.Drawing.Point(387, 147);
            this.txtComplemento.Margin = new System.Windows.Forms.Padding(4);
            this.txtComplemento.MaxLength = 100;
            this.txtComplemento.Name = "txtComplemento";
            this.txtComplemento.Size = new System.Drawing.Size(97, 28);
            this.txtComplemento.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(317, 126);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 17);
            this.label8.TabIndex = 57;
            this.label8.Text = "Número";
            // 
            // txtEnderecoNumero
            // 
            this.txtEnderecoNumero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtEnderecoNumero.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEnderecoNumero.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEnderecoNumero.Location = new System.Drawing.Point(317, 147);
            this.txtEnderecoNumero.Margin = new System.Windows.Forms.Padding(4);
            this.txtEnderecoNumero.MaxLength = 100;
            this.txtEnderecoNumero.Name = "txtEnderecoNumero";
            this.txtEnderecoNumero.Size = new System.Drawing.Size(62, 28);
            this.txtEnderecoNumero.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 126);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 17);
            this.label7.TabIndex = 55;
            this.label7.Text = "Endereço";
            // 
            // txtEndereco
            // 
            this.txtEndereco.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtEndereco.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEndereco.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEndereco.Location = new System.Drawing.Point(7, 147);
            this.txtEndereco.Margin = new System.Windows.Forms.Padding(4);
            this.txtEndereco.MaxLength = 100;
            this.txtEndereco.Name = "txtEndereco";
            this.txtEndereco.Size = new System.Drawing.Size(302, 28);
            this.txtEndereco.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 73);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 17);
            this.label6.TabIndex = 53;
            this.label6.Text = "CPF/CNPJ";
            // 
            // txtDocumento1
            // 
            this.txtDocumento1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDocumento1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDocumento1.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumento1.Location = new System.Drawing.Point(7, 94);
            this.txtDocumento1.Margin = new System.Windows.Forms.Padding(4);
            this.txtDocumento1.MaxLength = 14;
            this.txtDocumento1.Name = "txtDocumento1";
            this.txtDocumento1.Size = new System.Drawing.Size(251, 28);
            this.txtDocumento1.TabIndex = 3;
            this.txtDocumento1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ApenasNumero_KeyPress);
            // 
            // txtTelefone1Numero
            // 
            this.txtTelefone1Numero.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtTelefone1Numero.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTelefone1Numero.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefone1Numero.Location = new System.Drawing.Point(361, 41);
            this.txtTelefone1Numero.Margin = new System.Windows.Forms.Padding(4);
            this.txtTelefone1Numero.MaxLength = 9;
            this.txtTelefone1Numero.Name = "txtTelefone1Numero";
            this.txtTelefone1Numero.Size = new System.Drawing.Size(123, 28);
            this.txtTelefone1Numero.TabIndex = 2;
            this.txtTelefone1Numero.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ApenasNumero_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(318, 21);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 17);
            this.label5.TabIndex = 49;
            this.label5.Text = "Telefone";
            // 
            // txtTelefone1DDD
            // 
            this.txtTelefone1DDD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtTelefone1DDD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTelefone1DDD.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTelefone1DDD.Location = new System.Drawing.Point(320, 41);
            this.txtTelefone1DDD.Margin = new System.Windows.Forms.Padding(4);
            this.txtTelefone1DDD.MaxLength = 2;
            this.txtTelefone1DDD.Name = "txtTelefone1DDD";
            this.txtTelefone1DDD.Size = new System.Drawing.Size(38, 28);
            this.txtTelefone1DDD.TabIndex = 1;
            this.txtTelefone1DDD.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ApenasNumero_KeyPress);
            // 
            // lblTipoEntrada
            // 
            this.lblTipoEntrada.AutoSize = true;
            this.lblTipoEntrada.Location = new System.Drawing.Point(232, 73);
            this.lblTipoEntrada.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTipoEntrada.Name = "lblTipoEntrada";
            this.lblTipoEntrada.Size = new System.Drawing.Size(88, 17);
            this.lblTipoEntrada.TabIndex = 72;
            this.lblTipoEntrada.Text = "Tipo entrada";
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
            this.cbbTipoEntrada.Location = new System.Drawing.Point(300, 92);
            this.cbbTipoEntrada.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.cbbTipoEntrada.Name = "cbbTipoEntrada";
            this.cbbTipoEntrada.Size = new System.Drawing.Size(203, 30);
            this.cbbTipoEntrada.TabIndex = 2;
            this.cbbTipoEntrada.ValueMember = "IDTipoEntrada";
            // 
            // txtCodigoEntrada
            // 
            this.txtCodigoEntrada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtCodigoEntrada.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCodigoEntrada.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoEntrada.Location = new System.Drawing.Point(235, 93);
            this.txtCodigoEntrada.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigoEntrada.Name = "txtCodigoEntrada";
            this.txtCodigoEntrada.Size = new System.Drawing.Size(62, 28);
            this.txtCodigoEntrada.TabIndex = 1;
            this.txtCodigoEntrada.TextChanged += new System.EventHandler(this.txtCodigoEntrada_TextChanged);
            // 
            // ckbBloqueado
            // 
            this.ckbBloqueado.BackColor = System.Drawing.Color.Red;
            this.ckbBloqueado.Location = new System.Drawing.Point(12, 518);
            this.ckbBloqueado.Name = "ckbBloqueado";
            this.ckbBloqueado.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ckbBloqueado.Size = new System.Drawing.Size(489, 32);
            this.ckbBloqueado.TabIndex = 4;
            this.ckbBloqueado.Text = "Bloquear cliente";
            this.ckbBloqueado.UseVisualStyleBackColor = false;
            this.ckbBloqueado.CheckedChanged += new System.EventHandler(this.ckbBloqueado_CheckedChanged);
            // 
            // frmAbrirComanda
            // 
            this.AcceptButton = this.btnAbrirComanda;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(514, 624);
            this.Controls.Add(this.ckbBloqueado);
            this.Controls.Add(this.txtCodigoEntrada);
            this.Controls.Add(this.lblTipoEntrada);
            this.Controls.Add(this.cbbTipoEntrada);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCodigoComanda);
            this.Controls.Add(this.btnAbrirComanda);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(514, 624);
            this.MinimumSize = new System.Drawing.Size(514, 624);
            this.Name = "frmAbrirComanda";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ABRIR COMANDA";
            this.Load += new System.EventHandler(this.frmAbrirComanda_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAbrirComanda;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbEstado;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCodigoComanda;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNomeCompleto;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtTelefone1Numero;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTelefone1DDD;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtObservacao;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCidade;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtBairro;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtComplemento;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtEnderecoNumero;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtEndereco;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDocumento1;
        private System.Windows.Forms.Label lblTipoEntrada;
        private System.Windows.Forms.ComboBox cbbTipoEntrada;
        private System.Windows.Forms.TextBox txtCodigoEntrada;
        private System.Windows.Forms.CheckBox ckbBloqueado;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtRG;
        private System.Windows.Forms.TextBox txtDataNascimento;
        private System.Windows.Forms.Label label15;
    }
}