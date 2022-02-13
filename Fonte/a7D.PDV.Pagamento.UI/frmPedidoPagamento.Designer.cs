namespace a7D.PDV.Pagamento.UI
{
    partial class frmPedidoPagamento
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPedidoPagamento));
            this.label4 = new System.Windows.Forms.Label();
            this.txtDocumentoCliente = new System.Windows.Forms.TextBox();
            this.btnFecharVenda = new System.Windows.Forms.Button();
            this.dgvPagamentos = new System.Windows.Forms.DataGridView();
            this.Remover = new System.Windows.Forms.DataGridViewImageColumn();
            this.TipoPagamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAdicionarPagamento = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtValorPagamento = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbbFormaPagamento = new System.Windows.Forms.ComboBox();
            this.gbxValorPendente = new System.Windows.Forms.GroupBox();
            this.lblValorPendente = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblValorPago = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ckbAplicarServico = new System.Windows.Forms.CheckBox();
            this.txtAcrescimoReais = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAcrescimoPorcentagem = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblCodigoPedido = new System.Windows.Forms.Label();
            this.lblPedido = new System.Windows.Forms.Label();
            this.dgvItens = new System.Windows.Forms.DataGridView();
            this.btnSelecionarCliente = new System.Windows.Forms.Button();
            this.txtObservacoes = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtNomeCliente = new System.Windows.Forms.TextBox();
            this.ckbNFe = new System.Windows.Forms.CheckBox();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblIdentificacao = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtCodigoFormaPagamento = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckbAplicaDesconto = new System.Windows.Forms.CheckBox();
            this.txtDescontoReais = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDescontoPorcentagem = new System.Windows.Forms.TextBox();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valorUnitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagamentos)).BeginInit();
            this.gbxValorPendente.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 125);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 22);
            this.label4.TabIndex = 18;
            this.label4.Text = "CPF/CNPJ";
            // 
            // txtDocumentoCliente
            // 
            this.txtDocumentoCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDocumentoCliente.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDocumentoCliente.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentoCliente.Location = new System.Drawing.Point(13, 146);
            this.txtDocumentoCliente.Margin = new System.Windows.Forms.Padding(4);
            this.txtDocumentoCliente.Name = "txtDocumentoCliente";
            this.txtDocumentoCliente.Size = new System.Drawing.Size(175, 35);
            this.txtDocumentoCliente.TabIndex = 7;
            this.txtDocumentoCliente.Click += new System.EventHandler(this.txt_EnterInteiro);
            this.txtDocumentoCliente.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDocumentoCliente_KeyPress);
            this.txtDocumentoCliente.Leave += new System.EventHandler(this.txtDocumentoCliente_Leave);
            // 
            // btnFecharVenda
            // 
            this.btnFecharVenda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnFecharVenda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFecharVenda.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFecharVenda.ForeColor = System.Drawing.Color.White;
            this.btnFecharVenda.Location = new System.Drawing.Point(450, 584);
            this.btnFecharVenda.Margin = new System.Windows.Forms.Padding(4);
            this.btnFecharVenda.Name = "btnFecharVenda";
            this.btnFecharVenda.Size = new System.Drawing.Size(197, 52);
            this.btnFecharVenda.TabIndex = 4;
            this.btnFecharVenda.Text = "&FECHAR";
            this.btnFecharVenda.UseVisualStyleBackColor = false;
            this.btnFecharVenda.Click += new System.EventHandler(this.btnFecharVenda_Click);
            // 
            // dgvPagamentos
            // 
            this.dgvPagamentos.AllowUserToAddRows = false;
            this.dgvPagamentos.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(240)))));
            this.dgvPagamentos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPagamentos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.dgvPagamentos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPagamentos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvPagamentos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPagamentos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPagamentos.ColumnHeadersHeight = 40;
            this.dgvPagamentos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Remover,
            this.TipoPagamento,
            this.Valor});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPagamentos.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPagamentos.EnableHeadersVisualStyles = false;
            this.dgvPagamentos.Location = new System.Drawing.Point(15, 244);
            this.dgvPagamentos.MultiSelect = false;
            this.dgvPagamentos.Name = "dgvPagamentos";
            this.dgvPagamentos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPagamentos.RowHeadersVisible = false;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(5);
            this.dgvPagamentos.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPagamentos.RowTemplate.Height = 35;
            this.dgvPagamentos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvPagamentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPagamentos.Size = new System.Drawing.Size(423, 304);
            this.dgvPagamentos.TabIndex = 7;
            this.dgvPagamentos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPagamentos_CellClick);
            // 
            // Remover
            // 
            this.Remover.HeaderText = "";
            this.Remover.Image = global::a7D.PDV.Pagamento.UI.Properties.Resources.btnExcluir1;
            this.Remover.Name = "Remover";
            this.Remover.Width = 40;
            // 
            // TipoPagamento
            // 
            this.TipoPagamento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TipoPagamento.DataPropertyName = "TipoPagamento";
            this.TipoPagamento.HeaderText = "Forma Pagamento";
            this.TipoPagamento.Name = "TipoPagamento";
            // 
            // Valor
            // 
            this.Valor.DataPropertyName = "Valor";
            dataGridViewCellStyle3.Format = "C2";
            dataGridViewCellStyle3.NullValue = null;
            this.Valor.DefaultCellStyle = dataGridViewCellStyle3;
            this.Valor.HeaderText = "Valor Pago";
            this.Valor.Name = "Valor";
            // 
            // btnAdicionarPagamento
            // 
            this.btnAdicionarPagamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(131)))), ((int)(((byte)(159)))));
            this.btnAdicionarPagamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionarPagamento.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionarPagamento.ForeColor = System.Drawing.Color.White;
            this.btnAdicionarPagamento.Location = new System.Drawing.Point(381, 190);
            this.btnAdicionarPagamento.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdicionarPagamento.Name = "btnAdicionarPagamento";
            this.btnAdicionarPagamento.Size = new System.Drawing.Size(56, 47);
            this.btnAdicionarPagamento.TabIndex = 3;
            this.btnAdicionarPagamento.Text = "+";
            this.btnAdicionarPagamento.UseVisualStyleBackColor = false;
            this.btnAdicionarPagamento.Click += new System.EventHandler(this.btnAdicionarPagamento_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(279, 190);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 22);
            this.label2.TabIndex = 28;
            this.label2.Text = "Valor";
            // 
            // txtValorPagamento
            // 
            this.txtValorPagamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtValorPagamento.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtValorPagamento.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValorPagamento.Location = new System.Drawing.Point(282, 209);
            this.txtValorPagamento.Margin = new System.Windows.Forms.Padding(4);
            this.txtValorPagamento.Name = "txtValorPagamento";
            this.txtValorPagamento.Size = new System.Drawing.Size(94, 35);
            this.txtValorPagamento.TabIndex = 2;
            this.txtValorPagamento.Click += new System.EventHandler(this.txt_Enter);
            this.txtValorPagamento.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtValorPagamento_KeyDown);
            this.txtValorPagamento.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SomenteNumero_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 189);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 22);
            this.label1.TabIndex = 26;
            this.label1.Text = "Forma de pagamento";
            // 
            // cbbFormaPagamento
            // 
            this.cbbFormaPagamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbbFormaPagamento.DisplayMember = "Nome";
            this.cbbFormaPagamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbFormaPagamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbFormaPagamento.Font = new System.Drawing.Font("Arial", 14F);
            this.cbbFormaPagamento.FormattingEnabled = true;
            this.cbbFormaPagamento.ItemHeight = 26;
            this.cbbFormaPagamento.Location = new System.Drawing.Point(65, 208);
            this.cbbFormaPagamento.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.cbbFormaPagamento.Name = "cbbFormaPagamento";
            this.cbbFormaPagamento.Size = new System.Drawing.Size(206, 34);
            this.cbbFormaPagamento.TabIndex = 1;
            this.cbbFormaPagamento.ValueMember = "IDFormaPagamento";
            this.cbbFormaPagamento.SelectedIndexChanged += new System.EventHandler(this.cbbFormaPagamento_SelectedIndexChanged);
            // 
            // gbxValorPendente
            // 
            this.gbxValorPendente.Controls.Add(this.lblValorPendente);
            this.gbxValorPendente.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxValorPendente.Location = new System.Drawing.Point(450, 521);
            this.gbxValorPendente.Margin = new System.Windows.Forms.Padding(4);
            this.gbxValorPendente.Name = "gbxValorPendente";
            this.gbxValorPendente.Padding = new System.Windows.Forms.Padding(4);
            this.gbxValorPendente.Size = new System.Drawing.Size(196, 55);
            this.gbxValorPendente.TabIndex = 24;
            this.gbxValorPendente.TabStop = false;
            this.gbxValorPendente.Text = "A Pagar / Troco";
            // 
            // lblValorPendente
            // 
            this.lblValorPendente.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorPendente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblValorPendente.Location = new System.Drawing.Point(25, 22);
            this.lblValorPendente.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValorPendente.Name = "lblValorPendente";
            this.lblValorPendente.Size = new System.Drawing.Size(160, 21);
            this.lblValorPendente.TabIndex = 5;
            this.lblValorPendente.Text = "0,00";
            this.lblValorPendente.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblValorPago);
            this.groupBox5.Location = new System.Drawing.Point(450, 458);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(196, 55);
            this.groupBox5.TabIndex = 23;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Valor Pago";
            // 
            // lblValorPago
            // 
            this.lblValorPago.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorPago.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblValorPago.Location = new System.Drawing.Point(25, 22);
            this.lblValorPago.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValorPago.Name = "lblValorPago";
            this.lblValorPago.Size = new System.Drawing.Size(160, 21);
            this.lblValorPago.TabIndex = 0;
            this.lblValorPago.Text = "0,00";
            this.lblValorPago.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblTotal);
            this.groupBox4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(449, 395);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(196, 55);
            this.groupBox4.TabIndex = 22;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Valor Total";
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTotal.Location = new System.Drawing.Point(25, 22);
            this.lblTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(160, 21);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "0,00";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ckbAplicarServico);
            this.groupBox3.Controls.Add(this.txtAcrescimoReais);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtAcrescimoPorcentagem);
            this.groupBox3.Location = new System.Drawing.Point(449, 217);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(195, 81);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Taxa de serviço";
            // 
            // ckbAplicarServico
            // 
            this.ckbAplicarServico.AutoSize = true;
            this.ckbAplicarServico.Checked = true;
            this.ckbAplicarServico.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbAplicarServico.Location = new System.Drawing.Point(8, 26);
            this.ckbAplicarServico.Name = "ckbAplicarServico";
            this.ckbAplicarServico.Size = new System.Drawing.Size(222, 26);
            this.ckbAplicarServico.TabIndex = 89;
            this.ckbAplicarServico.Text = "Aplicar taxa de serviço";
            this.ckbAplicarServico.UseVisualStyleBackColor = true;
            this.ckbAplicarServico.Click += new System.EventHandler(this.ckbAplicarServico_Click);
            // 
            // txtAcrescimoReais
            // 
            this.txtAcrescimoReais.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtAcrescimoReais.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAcrescimoReais.Enabled = false;
            this.txtAcrescimoReais.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAcrescimoReais.Location = new System.Drawing.Point(130, 51);
            this.txtAcrescimoReais.Margin = new System.Windows.Forms.Padding(4);
            this.txtAcrescimoReais.Name = "txtAcrescimoReais";
            this.txtAcrescimoReais.Size = new System.Drawing.Size(55, 25);
            this.txtAcrescimoReais.TabIndex = 1;
            this.txtAcrescimoReais.Text = "0,00";
            this.txtAcrescimoReais.Click += new System.EventHandler(this.txt_EnterDecimal);
            this.txtAcrescimoReais.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SomenteNumero_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label10.Location = new System.Drawing.Point(3, 52);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 23);
            this.label10.TabIndex = 86;
            this.label10.Text = "%";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label9.Location = new System.Drawing.Point(102, 52);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 23);
            this.label9.TabIndex = 88;
            this.label9.Text = "R$";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAcrescimoPorcentagem
            // 
            this.txtAcrescimoPorcentagem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtAcrescimoPorcentagem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAcrescimoPorcentagem.Enabled = false;
            this.txtAcrescimoPorcentagem.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAcrescimoPorcentagem.Location = new System.Drawing.Point(31, 51);
            this.txtAcrescimoPorcentagem.Margin = new System.Windows.Forms.Padding(4);
            this.txtAcrescimoPorcentagem.Name = "txtAcrescimoPorcentagem";
            this.txtAcrescimoPorcentagem.Size = new System.Drawing.Size(55, 25);
            this.txtAcrescimoPorcentagem.TabIndex = 0;
            this.txtAcrescimoPorcentagem.Text = "0,00";
            this.txtAcrescimoPorcentagem.Click += new System.EventHandler(this.txt_EnterDecimal);
            this.txtAcrescimoPorcentagem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SomenteNumero_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSubTotal);
            this.groupBox1.Location = new System.Drawing.Point(449, 154);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(196, 55);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Produtos";
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblSubTotal.Location = new System.Drawing.Point(25, 22);
            this.lblSubTotal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(160, 21);
            this.lblSubTotal.TabIndex = 1;
            this.lblSubTotal.Text = "0,00";
            this.lblSubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCodigoPedido
            // 
            this.lblCodigoPedido.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCodigoPedido.AutoSize = true;
            this.lblCodigoPedido.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigoPedido.ForeColor = System.Drawing.Color.Gray;
            this.lblCodigoPedido.Location = new System.Drawing.Point(874, 103);
            this.lblCodigoPedido.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCodigoPedido.Name = "lblCodigoPedido";
            this.lblCodigoPedido.Size = new System.Drawing.Size(117, 35);
            this.lblCodigoPedido.TabIndex = 50;
            this.lblCodigoPedido.Text = "000000";
            this.lblCodigoPedido.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPedido
            // 
            this.lblPedido.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPedido.AutoSize = true;
            this.lblPedido.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPedido.ForeColor = System.Drawing.Color.Gray;
            this.lblPedido.Location = new System.Drawing.Point(763, 103);
            this.lblPedido.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPedido.Name = "lblPedido";
            this.lblPedido.Size = new System.Drawing.Size(140, 35);
            this.lblPedido.TabIndex = 49;
            this.lblPedido.Text = "PEDIDO:";
            this.lblPedido.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvItens
            // 
            this.dgvItens.AllowUserToAddRows = false;
            this.dgvItens.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(240)))));
            this.dgvItens.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvItens.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvItens.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.dgvItens.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItens.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvItens.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItens.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvItens.ColumnHeadersHeight = 40;
            this.dgvItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.descricao,
            this.valorUnitario});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItens.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvItens.EnableHeadersVisualStyles = false;
            this.dgvItens.Location = new System.Drawing.Point(654, 150);
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvItens.RowHeadersVisible = false;
            dataGridViewCellStyle9.Padding = new System.Windows.Forms.Padding(5);
            this.dgvItens.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvItens.RowTemplate.Height = 35;
            this.dgvItens.RowTemplate.ReadOnly = true;
            this.dgvItens.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItens.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvItens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItens.Size = new System.Drawing.Size(314, 486);
            this.dgvItens.TabIndex = 11;
            // 
            // btnSelecionarCliente
            // 
            this.btnSelecionarCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(131)))), ((int)(((byte)(159)))));
            this.btnSelecionarCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelecionarCliente.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelecionarCliente.ForeColor = System.Drawing.Color.White;
            this.btnSelecionarCliente.Location = new System.Drawing.Point(198, 93);
            this.btnSelecionarCliente.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelecionarCliente.Name = "btnSelecionarCliente";
            this.btnSelecionarCliente.Size = new System.Drawing.Size(239, 28);
            this.btnSelecionarCliente.TabIndex = 6;
            this.btnSelecionarCliente.Text = "SELECIONAR CLIENTE";
            this.btnSelecionarCliente.UseVisualStyleBackColor = false;
            this.btnSelecionarCliente.Click += new System.EventHandler(this.btnSelecionarCliente_Click);
            // 
            // txtObservacoes
            // 
            this.txtObservacoes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtObservacoes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtObservacoes.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacoes.Location = new System.Drawing.Point(13, 572);
            this.txtObservacoes.Margin = new System.Windows.Forms.Padding(4);
            this.txtObservacoes.Multiline = true;
            this.txtObservacoes.Name = "txtObservacoes";
            this.txtObservacoes.Size = new System.Drawing.Size(424, 64);
            this.txtObservacoes.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 551);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 22);
            this.label6.TabIndex = 82;
            this.label6.Text = "Observações";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(11, 73);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(69, 22);
            this.label11.TabIndex = 84;
            this.label11.Text = "Cliente";
            // 
            // txtNomeCliente
            // 
            this.txtNomeCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtNomeCliente.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNomeCliente.Enabled = false;
            this.txtNomeCliente.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomeCliente.Location = new System.Drawing.Point(13, 93);
            this.txtNomeCliente.Margin = new System.Windows.Forms.Padding(4);
            this.txtNomeCliente.Name = "txtNomeCliente";
            this.txtNomeCliente.Size = new System.Drawing.Size(175, 35);
            this.txtNomeCliente.TabIndex = 5;
            // 
            // ckbNFe
            // 
            this.ckbNFe.AutoSize = true;
            this.ckbNFe.Checked = true;
            this.ckbNFe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbNFe.Location = new System.Drawing.Point(198, 150);
            this.ckbNFe.Name = "ckbNFe";
            this.ckbNFe.Size = new System.Drawing.Size(121, 26);
            this.ckbNFe.TabIndex = 8;
            this.ckbNFe.Text = "Gerar NFe";
            this.ckbNFe.UseVisualStyleBackColor = true;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::a7D.PDV.Pagamento.UI.Properties.Resources.btnExcluir1;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::a7D.PDV.Pagamento.UI.Properties.Resources.bg_Titulo2;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(-20, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1013, 71);
            this.panel2.TabIndex = 78;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(423, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(205, 35);
            this.label3.TabIndex = 39;
            this.label3.Text = "PAGAMENTO";
            // 
            // lblIdentificacao
            // 
            this.lblIdentificacao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIdentificacao.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdentificacao.ForeColor = System.Drawing.Color.Gray;
            this.lblIdentificacao.Location = new System.Drawing.Point(646, 73);
            this.lblIdentificacao.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIdentificacao.Name = "lblIdentificacao";
            this.lblIdentificacao.Size = new System.Drawing.Size(319, 29);
            this.lblIdentificacao.TabIndex = 87;
            this.lblIdentificacao.Text = "MESA";
            this.lblIdentificacao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCodigoFormaPagamento
            // 
            this.txtCodigoFormaPagamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtCodigoFormaPagamento.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCodigoFormaPagamento.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoFormaPagamento.Location = new System.Drawing.Point(13, 209);
            this.txtCodigoFormaPagamento.Margin = new System.Windows.Forms.Padding(4);
            this.txtCodigoFormaPagamento.Name = "txtCodigoFormaPagamento";
            this.txtCodigoFormaPagamento.Size = new System.Drawing.Size(49, 35);
            this.txtCodigoFormaPagamento.TabIndex = 0;
            this.txtCodigoFormaPagamento.TextChanged += new System.EventHandler(this.txtCodigoFormaPagamento_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ckbAplicaDesconto);
            this.groupBox2.Controls.Add(this.txtDescontoReais);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtDescontoPorcentagem);
            this.groupBox2.Location = new System.Drawing.Point(449, 306);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(195, 81);
            this.groupBox2.TabIndex = 90;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Desconto";
            // 
            // ckbAplicaDesconto
            // 
            this.ckbAplicaDesconto.AutoSize = true;
            this.ckbAplicaDesconto.Location = new System.Drawing.Point(8, 26);
            this.ckbAplicaDesconto.Name = "ckbAplicaDesconto";
            this.ckbAplicaDesconto.Size = new System.Drawing.Size(174, 26);
            this.ckbAplicaDesconto.TabIndex = 89;
            this.ckbAplicaDesconto.Text = "Aplicar desconto";
            this.ckbAplicaDesconto.UseVisualStyleBackColor = true;
            this.ckbAplicaDesconto.Click += new System.EventHandler(this.ckbAplicaDesconto_Click);
            // 
            // txtDescontoReais
            // 
            this.txtDescontoReais.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDescontoReais.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescontoReais.Enabled = false;
            this.txtDescontoReais.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescontoReais.Location = new System.Drawing.Point(130, 51);
            this.txtDescontoReais.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescontoReais.Name = "txtDescontoReais";
            this.txtDescontoReais.Size = new System.Drawing.Size(55, 25);
            this.txtDescontoReais.TabIndex = 1;
            this.txtDescontoReais.Text = "0,00";
            this.txtDescontoReais.Click += new System.EventHandler(this.txt_EnterDecimal);
            this.txtDescontoReais.TextChanged += new System.EventHandler(this.txtDescontoReais_TextChanged);
            this.txtDescontoReais.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SomenteNumero_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(3, 52);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 23);
            this.label7.TabIndex = 86;
            this.label7.Text = "%";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(102, 52);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 23);
            this.label8.TabIndex = 88;
            this.label8.Text = "R$";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDescontoPorcentagem
            // 
            this.txtDescontoPorcentagem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDescontoPorcentagem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescontoPorcentagem.Enabled = false;
            this.txtDescontoPorcentagem.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescontoPorcentagem.Location = new System.Drawing.Point(31, 51);
            this.txtDescontoPorcentagem.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescontoPorcentagem.Name = "txtDescontoPorcentagem";
            this.txtDescontoPorcentagem.Size = new System.Drawing.Size(55, 25);
            this.txtDescontoPorcentagem.TabIndex = 0;
            this.txtDescontoPorcentagem.Text = "0,00";
            this.txtDescontoPorcentagem.Click += new System.EventHandler(this.txt_EnterDecimal);
            this.txtDescontoPorcentagem.TextChanged += new System.EventHandler(this.txtDescontoPorcentagem_TextChanged);
            this.txtDescontoPorcentagem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SomenteNumero_KeyPress);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.Visible = false;
            this.Column1.Width = 50;
            // 
            // descricao
            // 
            this.descricao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descricao.DataPropertyName = "nome";
            this.descricao.HeaderText = "Descrição";
            this.descricao.Name = "descricao";
            // 
            // valorUnitario
            // 
            this.valorUnitario.DataPropertyName = "valorUnitario";
            this.valorUnitario.HeaderText = "Valor";
            this.valorUnitario.Name = "valorUnitario";
            // 
            // frmPedidoPagamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(978, 649);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbbFormaPagamento);
            this.Controls.Add(this.txtCodigoFormaPagamento);
            this.Controls.Add(this.lblIdentificacao);
            this.Controls.Add(this.ckbNFe);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtNomeCliente);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtObservacoes);
            this.Controls.Add(this.btnSelecionarCliente);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dgvItens);
            this.Controls.Add(this.lblCodigoPedido);
            this.Controls.Add(this.lblPedido);
            this.Controls.Add(this.btnFecharVenda);
            this.Controls.Add(this.dgvPagamentos);
            this.Controls.Add(this.btnAdicionarPagamento);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtValorPagamento);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gbxValorPendente);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDocumentoCliente);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPedidoPagamento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Activated += new System.EventHandler(this.frmPedidoPagamento_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPedidoPagamento_FormClosed);
            this.Load += new System.EventHandler(this.frmPagamento_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagamentos)).EndInit();
            this.gbxValorPendente.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDocumentoCliente;
        private System.Windows.Forms.Button btnFecharVenda;
        private System.Windows.Forms.DataGridView dgvPagamentos;
        private System.Windows.Forms.Button btnAdicionarPagamento;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtValorPagamento;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbFormaPagamento;
        private System.Windows.Forms.GroupBox gbxValorPendente;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblValorPago;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblValorPendente;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.Label lblCodigoPedido;
        private System.Windows.Forms.Label lblPedido;
        private System.Windows.Forms.DataGridViewImageColumn Remover;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoPagamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
        private System.Windows.Forms.DataGridView dgvItens;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelecionarCliente;
        private System.Windows.Forms.TextBox txtAcrescimoReais;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAcrescimoPorcentagem;
        private System.Windows.Forms.TextBox txtObservacoes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtNomeCliente;
        private System.Windows.Forms.CheckBox ckbNFe;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Label lblIdentificacao;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtCodigoFormaPagamento;
        private System.Windows.Forms.CheckBox ckbAplicarServico;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ckbAplicaDesconto;
        private System.Windows.Forms.TextBox txtDescontoReais;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDescontoPorcentagem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn valorUnitario;
    }
}