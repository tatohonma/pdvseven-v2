namespace a7D.PDV.Caixa.UI
{
    partial class frmPedidos
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPedidos));
            this.tmrPedidos = new System.Windows.Forms.Timer(this.components);
            this.btnAdicionarProduto = new System.Windows.Forms.Button();
            this.btnPagamento = new System.Windows.Forms.Button();
            this.btnImprimirConta = new System.Windows.Forms.Button();
            this.btnTransferirProdutos = new System.Windows.Forms.Button();
            this.dgvItens = new System.Windows.Forms.DataGridView();
            this.descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rdbMesas = new System.Windows.Forms.RadioButton();
            this.rdbComandas = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblInformacaoAdicional = new System.Windows.Forms.Label();
            this.lblTaxaEntrega = new System.Windows.Forms.Label();
            this.lblTaxaEntregaTexto = new System.Windows.Forms.Label();
            this.lblValorConsumacaoMinima = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblValorServico = new System.Windows.Forms.Label();
            this.lblValorProdutos = new System.Windows.Forms.Label();
            this.lblValorTotal = new System.Windows.Forms.Label();
            this.lblIdentificacao = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblModoContingencia = new System.Windows.Forms.Label();
            this.btnFechar = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mesasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelarSolicitaçãoDeContaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pagamentoParcialMesaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geralToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.comandasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alterarTipoDeEntradaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bloquearLiberarComandaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.juntarComandasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelarSolicitaçãoDeContaToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pagamentoParcialComandaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informarPerdaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.clientesMenuCadastro = new System.Windows.Forms.ToolStripMenuItem();
            this.clientesMenuSaldos = new System.Windows.Forms.ToolStripMenuItem();
            this.fiscalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reimprimirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cancelarPedidoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirGavetaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.integracaoMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.iFoodMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.iFoodHabilitado = new System.Windows.Forms.ToolStripMenuItem();
            this.iFoodAprovacao = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTituloForm = new System.Windows.Forms.Label();
            this.rdbEntrega = new System.Windows.Forms.RadioButton();
            this.tmrDelivery = new System.Windows.Forms.Timer(this.components);
            this.tmrStatus = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdicionarCreditos = new System.Windows.Forms.Button();
            this.spContainer = new System.Windows.Forms.SplitContainer();
            this.listaPedidoEntrega1 = new a7D.PDV.Caixa.UI.Controles.ListaPedidoEntrega();
            this.listaPedidoMesa1 = new a7D.PDV.Caixa.UI.Controles.ListaPedidoMesa();
            this.listaPedidoComandaSemCheckin1 = new a7D.PDV.Caixa.UI.Controles.ListaPedidoComandaSemCheckin();
            this.listaPedidoComanda1 = new a7D.PDV.Caixa.UI.Controles.ListaPedidoComanda();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tbMenuTop = new System.Windows.Forms.TableLayoutPanel();
            this.rdbRetirada = new System.Windows.Forms.RadioButton();
            this.btnBalcao = new System.Windows.Forms.Button();
            this.menuTEFcancelarTransacao = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spContainer)).BeginInit();
            this.spContainer.Panel1.SuspendLayout();
            this.spContainer.Panel2.SuspendLayout();
            this.spContainer.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tbMenuTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrPedidos
            // 
            this.tmrPedidos.Enabled = true;
            this.tmrPedidos.Interval = 10000;
            this.tmrPedidos.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnAdicionarProduto
            // 
            this.btnAdicionarProduto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnAdicionarProduto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdicionarProduto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionarProduto.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionarProduto.ForeColor = System.Drawing.Color.White;
            this.btnAdicionarProduto.Location = new System.Drawing.Point(3, 3);
            this.btnAdicionarProduto.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnAdicionarProduto.Name = "btnAdicionarProduto";
            this.btnAdicionarProduto.Size = new System.Drawing.Size(220, 74);
            this.btnAdicionarProduto.TabIndex = 1;
            this.btnAdicionarProduto.Text = "ADICIONAR\r\n&PRODUTO";
            this.btnAdicionarProduto.UseVisualStyleBackColor = false;
            this.btnAdicionarProduto.Click += new System.EventHandler(this.btnAdicionarProduto_Click);
            // 
            // btnPagamento
            // 
            this.btnPagamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnPagamento.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPagamento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPagamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPagamento.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPagamento.ForeColor = System.Drawing.Color.White;
            this.btnPagamento.Location = new System.Drawing.Point(895, 3);
            this.btnPagamento.Name = "btnPagamento";
            this.btnPagamento.Size = new System.Drawing.Size(126, 74);
            this.btnPagamento.TabIndex = 4;
            this.btnPagamento.Text = "&FECHAR CONTA";
            this.btnPagamento.UseVisualStyleBackColor = false;
            this.btnPagamento.Click += new System.EventHandler(this.btnPagamento_Click);
            // 
            // btnImprimirConta
            // 
            this.btnImprimirConta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(131)))), ((int)(((byte)(159)))));
            this.btnImprimirConta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImprimirConta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimirConta.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimirConta.ForeColor = System.Drawing.Color.White;
            this.btnImprimirConta.Location = new System.Drawing.Point(672, 3);
            this.btnImprimirConta.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnImprimirConta.Name = "btnImprimirConta";
            this.btnImprimirConta.Size = new System.Drawing.Size(220, 74);
            this.btnImprimirConta.TabIndex = 3;
            this.btnImprimirConta.Text = "&IMPRIMIR\r\nCONTA";
            this.btnImprimirConta.UseVisualStyleBackColor = false;
            this.btnImprimirConta.Click += new System.EventHandler(this.btnImprimirConta_Click);
            // 
            // btnTransferirProdutos
            // 
            this.btnTransferirProdutos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(131)))), ((int)(((byte)(159)))));
            this.btnTransferirProdutos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTransferirProdutos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransferirProdutos.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTransferirProdutos.ForeColor = System.Drawing.Color.White;
            this.btnTransferirProdutos.Location = new System.Drawing.Point(449, 3);
            this.btnTransferirProdutos.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnTransferirProdutos.Name = "btnTransferirProdutos";
            this.btnTransferirProdutos.Size = new System.Drawing.Size(220, 74);
            this.btnTransferirProdutos.TabIndex = 2;
            this.btnTransferirProdutos.Text = "&TRANSFERIR\r\nPRODUTO";
            this.btnTransferirProdutos.UseVisualStyleBackColor = false;
            this.btnTransferirProdutos.Click += new System.EventHandler(this.btnTransferirProdutos_Click);
            // 
            // dgvItens
            // 
            this.dgvItens.AllowUserToAddRows = false;
            this.dgvItens.AllowUserToDeleteRows = false;
            this.dgvItens.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(240)))));
            this.dgvItens.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItens.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvItens.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvItens.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.dgvItens.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvItens.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvItens.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvItens.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItens.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvItens.ColumnHeadersHeight = 40;
            this.dgvItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.descricao,
            this.qtd});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItens.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItens.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItens.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvItens.EnableHeadersVisualStyles = false;
            this.dgvItens.Location = new System.Drawing.Point(0, 120);
            this.dgvItens.Margin = new System.Windows.Forms.Padding(0);
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItens.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvItens.RowHeadersVisible = false;
            this.dgvItens.RowHeadersWidth = 62;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(231)))), ((int)(((byte)(234)))));
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(5);
            this.dgvItens.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvItens.RowTemplate.DividerHeight = 1;
            this.dgvItens.RowTemplate.Height = 36;
            this.dgvItens.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvItens.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItens.Size = new System.Drawing.Size(449, 108);
            this.dgvItens.TabIndex = 5;
            this.dgvItens.DoubleClick += new System.EventHandler(this.dgvItens_DoubleClick);
            // 
            // descricao
            // 
            this.descricao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descricao.DataPropertyName = "nome";
            this.descricao.HeaderText = "Descrição";
            this.descricao.MinimumWidth = 8;
            this.descricao.Name = "descricao";
            // 
            // qtd
            // 
            this.qtd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.qtd.DataPropertyName = "quantidade";
            this.qtd.FillWeight = 50F;
            this.qtd.HeaderText = "Qtd.";
            this.qtd.MinimumWidth = 8;
            this.qtd.Name = "qtd";
            this.qtd.Width = 50;
            // 
            // rdbMesas
            // 
            this.rdbMesas.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdbMesas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rdbMesas.Checked = true;
            this.rdbMesas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdbMesas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdbMesas.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbMesas.ForeColor = System.Drawing.Color.White;
            this.rdbMesas.Location = new System.Drawing.Point(3, 3);
            this.rdbMesas.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.rdbMesas.Name = "rdbMesas";
            this.rdbMesas.Size = new System.Drawing.Size(200, 44);
            this.rdbMesas.TabIndex = 0;
            this.rdbMesas.TabStop = true;
            this.rdbMesas.Text = "&MESAS";
            this.rdbMesas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbMesas.UseVisualStyleBackColor = false;
            this.rdbMesas.Visible = false;
            this.rdbMesas.CheckedChanged += new System.EventHandler(this.rdbMesas_CheckedChanged);
            // 
            // rdbComandas
            // 
            this.rdbComandas.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdbComandas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rdbComandas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdbComandas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdbComandas.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbComandas.ForeColor = System.Drawing.Color.White;
            this.rdbComandas.Location = new System.Drawing.Point(206, 3);
            this.rdbComandas.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.rdbComandas.Name = "rdbComandas";
            this.rdbComandas.Size = new System.Drawing.Size(200, 44);
            this.rdbComandas.TabIndex = 1;
            this.rdbComandas.Text = "&COMANDAS";
            this.rdbComandas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbComandas.UseVisualStyleBackColor = false;
            this.rdbComandas.Visible = false;
            this.rdbComandas.CheckedChanged += new System.EventHandler(this.rdbComandas_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.panel1.Controls.Add(this.lblInformacaoAdicional);
            this.panel1.Controls.Add(this.lblTaxaEntrega);
            this.panel1.Controls.Add(this.lblTaxaEntregaTexto);
            this.panel1.Controls.Add(this.lblValorConsumacaoMinima);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.lblValorServico);
            this.panel1.Controls.Add(this.lblValorProdutos);
            this.panel1.Controls.Add(this.lblValorTotal);
            this.panel1.Controls.Add(this.lblIdentificacao);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.lblCliente);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(449, 114);
            this.panel1.TabIndex = 82;
            // 
            // lblInformacaoAdicional
            // 
            this.lblInformacaoAdicional.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInformacaoAdicional.Font = new System.Drawing.Font("Arial", 12F);
            this.lblInformacaoAdicional.Location = new System.Drawing.Point(230, 33);
            this.lblInformacaoAdicional.Name = "lblInformacaoAdicional";
            this.lblInformacaoAdicional.Size = new System.Drawing.Size(216, 26);
            this.lblInformacaoAdicional.TabIndex = 83;
            this.lblInformacaoAdicional.Text = "R$ 0,00";
            this.lblInformacaoAdicional.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaxaEntrega
            // 
            this.lblTaxaEntrega.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTaxaEntrega.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTaxaEntrega.Location = new System.Drawing.Point(358, 81);
            this.lblTaxaEntrega.Name = "lblTaxaEntrega";
            this.lblTaxaEntrega.Size = new System.Drawing.Size(88, 22);
            this.lblTaxaEntrega.TabIndex = 82;
            this.lblTaxaEntrega.Text = "R$ 0,00";
            this.lblTaxaEntrega.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTaxaEntregaTexto
            // 
            this.lblTaxaEntregaTexto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTaxaEntregaTexto.Location = new System.Drawing.Point(203, 81);
            this.lblTaxaEntregaTexto.Name = "lblTaxaEntregaTexto";
            this.lblTaxaEntregaTexto.Size = new System.Drawing.Size(165, 22);
            this.lblTaxaEntregaTexto.TabIndex = 81;
            this.lblTaxaEntregaTexto.Text = "Taxa de entrega:";
            this.lblTaxaEntregaTexto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblValorConsumacaoMinima
            // 
            this.lblValorConsumacaoMinima.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblValorConsumacaoMinima.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblValorConsumacaoMinima.Location = new System.Drawing.Point(358, 59);
            this.lblValorConsumacaoMinima.Name = "lblValorConsumacaoMinima";
            this.lblValorConsumacaoMinima.Size = new System.Drawing.Size(88, 22);
            this.lblValorConsumacaoMinima.TabIndex = 80;
            this.lblValorConsumacaoMinima.Text = "R$ 0,00";
            this.lblValorConsumacaoMinima.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(198, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(170, 22);
            this.label7.TabIndex = 77;
            this.label7.Text = "Consumação Min.:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblValorServico
            // 
            this.lblValorServico.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblValorServico.Location = new System.Drawing.Point(90, 81);
            this.lblValorServico.Name = "lblValorServico";
            this.lblValorServico.Size = new System.Drawing.Size(134, 22);
            this.lblValorServico.TabIndex = 76;
            this.lblValorServico.Text = "R$ 0,00 (10%)";
            this.lblValorServico.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblValorProdutos
            // 
            this.lblValorProdutos.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblValorProdutos.Location = new System.Drawing.Point(90, 59);
            this.lblValorProdutos.Name = "lblValorProdutos";
            this.lblValorProdutos.Size = new System.Drawing.Size(134, 22);
            this.lblValorProdutos.TabIndex = 75;
            this.lblValorProdutos.Text = "R$ 0,00";
            this.lblValorProdutos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblValorTotal
            // 
            this.lblValorTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblValorTotal.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblValorTotal.Location = new System.Drawing.Point(221, 4);
            this.lblValorTotal.Name = "lblValorTotal";
            this.lblValorTotal.Size = new System.Drawing.Size(225, 34);
            this.lblValorTotal.TabIndex = 74;
            this.lblValorTotal.Text = "R$ 0,00";
            this.lblValorTotal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblIdentificacao
            // 
            this.lblIdentificacao.AutoSize = true;
            this.lblIdentificacao.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdentificacao.Location = new System.Drawing.Point(3, 4);
            this.lblIdentificacao.Name = "lblIdentificacao";
            this.lblIdentificacao.Size = new System.Drawing.Size(77, 24);
            this.lblIdentificacao.TabIndex = 70;
            this.lblIdentificacao.Text = "MESA:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 22);
            this.label5.TabIndex = 73;
            this.label5.Text = "Produtos:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Location = new System.Drawing.Point(4, 33);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(53, 17);
            this.lblCliente.TabIndex = 71;
            this.lblCliente.Text = "Cliente";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(17, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 22);
            this.label4.TabIndex = 72;
            this.label4.Text = "Serviço:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Black;
            this.panel3.Controls.Add(this.lblModoContingencia);
            this.panel3.Controls.Add(this.btnFechar);
            this.panel3.Location = new System.Drawing.Point(0, 27);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1024, 38);
            this.panel3.TabIndex = 85;
            // 
            // lblModoContingencia
            // 
            this.lblModoContingencia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModoContingencia.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModoContingencia.ForeColor = System.Drawing.Color.Tomato;
            this.lblModoContingencia.Location = new System.Drawing.Point(68, 6);
            this.lblModoContingencia.Name = "lblModoContingencia";
            this.lblModoContingencia.Size = new System.Drawing.Size(882, 29);
            this.lblModoContingencia.TabIndex = 1;
            this.lblModoContingencia.Text = "EM MODO DE CONTINGÊNCIA";
            this.lblModoContingencia.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFechar
            // 
            this.btnFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFechar.BackgroundImage = global::a7D.PDV.Caixa.UI.Properties.Resources.btnFechar;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFechar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnFechar.Location = new System.Drawing.Point(996, 10);
            this.btnFechar.Margin = new System.Windows.Forms.Padding(0);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(18, 19);
            this.btnFechar.TabIndex = 0;
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mesasToolStripMenuItem,
            this.geralToolStripMenuItem,
            this.comandasToolStripMenuItem,
            this.informarPerdaToolStripMenuItem,
            this.clientesMenu,
            this.fiscalMenuItem,
            this.abrirGavetaToolStripMenuItem,
            this.integracaoMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1024, 24);
            this.menuStrip1.TabIndex = 86;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mesasToolStripMenuItem
            // 
            this.mesasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cancelarSolicitaçãoDeContaToolStripMenuItem,
            this.pagamentoParcialMesaToolStripMenuItem});
            this.mesasToolStripMenuItem.Name = "mesasToolStripMenuItem";
            this.mesasToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.mesasToolStripMenuItem.Text = "M&esas";
            // 
            // cancelarSolicitaçãoDeContaToolStripMenuItem
            // 
            this.cancelarSolicitaçãoDeContaToolStripMenuItem.Name = "cancelarSolicitaçãoDeContaToolStripMenuItem";
            this.cancelarSolicitaçãoDeContaToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.cancelarSolicitaçãoDeContaToolStripMenuItem.Text = "Cancelar solicitação de conta";
            this.cancelarSolicitaçãoDeContaToolStripMenuItem.Click += new System.EventHandler(this.cancelarSolicitaçãoDeContaToolStripMenuItem_Click);
            // 
            // pagamentoParcialMesaToolStripMenuItem
            // 
            this.pagamentoParcialMesaToolStripMenuItem.Name = "pagamentoParcialMesaToolStripMenuItem";
            this.pagamentoParcialMesaToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.pagamentoParcialMesaToolStripMenuItem.Text = "Pagamento Parcial";
            this.pagamentoParcialMesaToolStripMenuItem.Click += new System.EventHandler(this.btnPagamento_Click);
            // 
            // geralToolStripMenuItem
            // 
            this.geralToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem});
            this.geralToolStripMenuItem.Name = "geralToolStripMenuItem";
            this.geralToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.geralToolStripMenuItem.Text = "Geral";
            this.geralToolStripMenuItem.Visible = false;
            // 
            // alterarDisponibilidadeDeProdutosToolStripMenuItem
            // 
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem.Name = "alterarDisponibilidadeDeProdutosToolStripMenuItem";
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem.Text = "Alterar disponibilidade de produtos";
            // 
            // comandasToolStripMenuItem
            // 
            this.comandasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alterarTipoDeEntradaToolStripMenuItem,
            this.bloquearLiberarComandaToolStripMenuItem,
            this.juntarComandasToolStripMenuItem,
            this.cancelarSolicitaçãoDeContaToolStripMenuItem1,
            this.pagamentoParcialComandaToolStripMenuItem});
            this.comandasToolStripMenuItem.Name = "comandasToolStripMenuItem";
            this.comandasToolStripMenuItem.Size = new System.Drawing.Size(76, 20);
            this.comandasToolStripMenuItem.Text = "C&omandas";
            // 
            // alterarTipoDeEntradaToolStripMenuItem
            // 
            this.alterarTipoDeEntradaToolStripMenuItem.Name = "alterarTipoDeEntradaToolStripMenuItem";
            this.alterarTipoDeEntradaToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.alterarTipoDeEntradaToolStripMenuItem.Text = "Alterar tipo de entrada";
            this.alterarTipoDeEntradaToolStripMenuItem.Click += new System.EventHandler(this.alterarTipoDeEntradaToolStripMenuItem_Click);
            // 
            // bloquearLiberarComandaToolStripMenuItem
            // 
            this.bloquearLiberarComandaToolStripMenuItem.Name = "bloquearLiberarComandaToolStripMenuItem";
            this.bloquearLiberarComandaToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.bloquearLiberarComandaToolStripMenuItem.Text = "Bloquear/Liberar";
            this.bloquearLiberarComandaToolStripMenuItem.Click += new System.EventHandler(this.bloquearLiberarComandaToolStripMenuItem_Click);
            // 
            // juntarComandasToolStripMenuItem
            // 
            this.juntarComandasToolStripMenuItem.Name = "juntarComandasToolStripMenuItem";
            this.juntarComandasToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.juntarComandasToolStripMenuItem.Text = "Agrupar";
            this.juntarComandasToolStripMenuItem.Click += new System.EventHandler(this.juntarComandasToolStripMenuItem_Click);
            // 
            // cancelarSolicitaçãoDeContaToolStripMenuItem1
            // 
            this.cancelarSolicitaçãoDeContaToolStripMenuItem1.Name = "cancelarSolicitaçãoDeContaToolStripMenuItem1";
            this.cancelarSolicitaçãoDeContaToolStripMenuItem1.Size = new System.Drawing.Size(231, 22);
            this.cancelarSolicitaçãoDeContaToolStripMenuItem1.Text = "Cancelar Solicitação de Conta";
            this.cancelarSolicitaçãoDeContaToolStripMenuItem1.Click += new System.EventHandler(this.cancelarSolicitaçãoDeContaToolStripMenuItem1_Click);
            // 
            // pagamentoParcialComandaToolStripMenuItem
            // 
            this.pagamentoParcialComandaToolStripMenuItem.Name = "pagamentoParcialComandaToolStripMenuItem";
            this.pagamentoParcialComandaToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.pagamentoParcialComandaToolStripMenuItem.Text = "Pagamento Parcial";
            this.pagamentoParcialComandaToolStripMenuItem.Click += new System.EventHandler(this.btnPagamento_Click);
            // 
            // informarPerdaToolStripMenuItem
            // 
            this.informarPerdaToolStripMenuItem.Name = "informarPerdaToolStripMenuItem";
            this.informarPerdaToolStripMenuItem.Size = new System.Drawing.Size(85, 20);
            this.informarPerdaToolStripMenuItem.Text = "Saída &Avulsa";
            this.informarPerdaToolStripMenuItem.Visible = false;
            this.informarPerdaToolStripMenuItem.Click += new System.EventHandler(this.informarPerdaToolStripMenuItem_Click);
            // 
            // clientesMenu
            // 
            this.clientesMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientesMenuCadastro,
            this.clientesMenuSaldos});
            this.clientesMenu.Name = "clientesMenu";
            this.clientesMenu.Size = new System.Drawing.Size(61, 20);
            this.clientesMenu.Text = "Clientes";
            this.clientesMenu.Click += new System.EventHandler(this.clientesMenu_Click);
            // 
            // clientesMenuCadastro
            // 
            this.clientesMenuCadastro.Name = "clientesMenuCadastro";
            this.clientesMenuCadastro.Size = new System.Drawing.Size(182, 22);
            this.clientesMenuCadastro.Text = "Cadastro de Clientes";
            this.clientesMenuCadastro.Click += new System.EventHandler(this.clientesMenuCadastro_Click);
            // 
            // clientesMenuSaldos
            // 
            this.clientesMenuSaldos.Name = "clientesMenuSaldos";
            this.clientesMenuSaldos.Size = new System.Drawing.Size(182, 22);
            this.clientesMenuSaldos.Text = "Saldo dos clientes";
            this.clientesMenuSaldos.Click += new System.EventHandler(this.clientesMenuSaldos_Click);
            // 
            // fiscalMenuItem
            // 
            this.fiscalMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reimprimirToolStripMenuItem,
            this.toolStripSeparator1,
            this.cancelarPedidoToolStripMenuItem});
            this.fiscalMenuItem.Name = "fiscalMenuItem";
            this.fiscalMenuItem.Size = new System.Drawing.Size(78, 20);
            this.fiscalMenuItem.Text = "S&AT / NFCe";
            this.fiscalMenuItem.Visible = false;
            // 
            // reimprimirToolStripMenuItem
            // 
            this.reimprimirToolStripMenuItem.Name = "reimprimirToolStripMenuItem";
            this.reimprimirToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.reimprimirToolStripMenuItem.Text = "Reimprimir Cupom";
            this.reimprimirToolStripMenuItem.Click += new System.EventHandler(this.reimprimirToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(173, 6);
            // 
            // cancelarPedidoToolStripMenuItem
            // 
            this.cancelarPedidoToolStripMenuItem.Name = "cancelarPedidoToolStripMenuItem";
            this.cancelarPedidoToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.cancelarPedidoToolStripMenuItem.Text = "Cancelar Cupom";
            this.cancelarPedidoToolStripMenuItem.Click += new System.EventHandler(this.cancelarPedidoToolStripMenuItem_Click);
            // 
            // abrirGavetaToolStripMenuItem
            // 
            this.abrirGavetaToolStripMenuItem.Name = "abrirGavetaToolStripMenuItem";
            this.abrirGavetaToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.abrirGavetaToolStripMenuItem.Text = "Abrir &Gaveta";
            this.abrirGavetaToolStripMenuItem.Click += new System.EventHandler(this.abrirGavetaToolStripMenuItem_Click);
            // 
            // integracaoMenu
            // 
            this.integracaoMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iFoodMenu});
            this.integracaoMenu.Name = "integracaoMenu";
            this.integracaoMenu.Size = new System.Drawing.Size(80, 20);
            this.integracaoMenu.Text = "Integrações";
            // 
            // iFoodMenu
            // 
            this.iFoodMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iFoodHabilitado,
            this.iFoodAprovacao});
            this.iFoodMenu.Name = "iFoodMenu";
            this.iFoodMenu.Size = new System.Drawing.Size(104, 22);
            this.iFoodMenu.Text = "iFood";
            // 
            // iFoodHabilitado
            // 
            this.iFoodHabilitado.Name = "iFoodHabilitado";
            this.iFoodHabilitado.Size = new System.Drawing.Size(196, 22);
            this.iFoodHabilitado.Text = "Habilitado";
            this.iFoodHabilitado.Click += new System.EventHandler(this.iFoodHabilitado_Click);
            // 
            // iFoodAprovacao
            // 
            this.iFoodAprovacao.Name = "iFoodAprovacao";
            this.iFoodAprovacao.Size = new System.Drawing.Size(196, 22);
            this.iFoodAprovacao.Text = "Aprovação Automatica";
            this.iFoodAprovacao.Click += new System.EventHandler(this.iFoodAprovacao_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewImageColumn1.FillWeight = 40F;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::a7D.PDV.Caixa.UI.Properties.Resources.btnExcluir1;
            this.dataGridViewImageColumn1.MinimumWidth = 8;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackgroundImage = global::a7D.PDV.Caixa.UI.Properties.Resources.bg_Titulo2;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.lblTituloForm);
            this.panel2.Location = new System.Drawing.Point(0, 71);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1024, 71);
            this.panel2.TabIndex = 76;
            // 
            // lblTituloForm
            // 
            this.lblTituloForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTituloForm.BackColor = System.Drawing.Color.Transparent;
            this.lblTituloForm.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloForm.ForeColor = System.Drawing.Color.White;
            this.lblTituloForm.Location = new System.Drawing.Point(0, 19);
            this.lblTituloForm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTituloForm.Name = "lblTituloForm";
            this.lblTituloForm.Size = new System.Drawing.Size(1024, 29);
            this.lblTituloForm.TabIndex = 39;
            this.lblTituloForm.Text = "PEDIDOS";
            this.lblTituloForm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rdbEntrega
            // 
            this.rdbEntrega.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdbEntrega.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rdbEntrega.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdbEntrega.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdbEntrega.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbEntrega.ForeColor = System.Drawing.Color.White;
            this.rdbEntrega.Location = new System.Drawing.Point(409, 3);
            this.rdbEntrega.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.rdbEntrega.Name = "rdbEntrega";
            this.rdbEntrega.Size = new System.Drawing.Size(200, 44);
            this.rdbEntrega.TabIndex = 2;
            this.rdbEntrega.Text = "&ENTREGA";
            this.rdbEntrega.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbEntrega.UseVisualStyleBackColor = false;
            this.rdbEntrega.Visible = false;
            this.rdbEntrega.CheckedChanged += new System.EventHandler(this.rdbEntrega_CheckedChanged);
            // 
            // tmrDelivery
            // 
            this.tmrDelivery.Interval = 5000;
            this.tmrDelivery.Tick += new System.EventHandler(this.tmrDelivery_Tick);
            // 
            // tmrStatus
            // 
            this.tmrStatus.Enabled = true;
            this.tmrStatus.Interval = 500;
            this.tmrStatus.Tick += new System.EventHandler(this.tmrStatus_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnAdicionarCreditos, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnImprimirConta, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnTransferirProdutos, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAdicionarProduto, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnPagamento, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 420);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1024, 80);
            this.tableLayoutPanel1.TabIndex = 92;
            // 
            // btnAdicionarCreditos
            // 
            this.btnAdicionarCreditos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnAdicionarCreditos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdicionarCreditos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionarCreditos.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionarCreditos.ForeColor = System.Drawing.Color.White;
            this.btnAdicionarCreditos.Location = new System.Drawing.Point(226, 3);
            this.btnAdicionarCreditos.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnAdicionarCreditos.Name = "btnAdicionarCreditos";
            this.btnAdicionarCreditos.Size = new System.Drawing.Size(220, 74);
            this.btnAdicionarCreditos.TabIndex = 5;
            this.btnAdicionarCreditos.Text = "ADICIONAR\r\n&CREDITO";
            this.btnAdicionarCreditos.UseVisualStyleBackColor = false;
            this.btnAdicionarCreditos.Visible = false;
            this.btnAdicionarCreditos.Click += new System.EventHandler(this.btnAdicionarCreditos_Click);
            // 
            // spContainer
            // 
            this.spContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spContainer.Location = new System.Drawing.Point(5, 196);
            this.spContainer.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.spContainer.Name = "spContainer";
            // 
            // spContainer.Panel1
            // 
            this.spContainer.Panel1.Controls.Add(this.listaPedidoEntrega1);
            this.spContainer.Panel1.Controls.Add(this.listaPedidoMesa1);
            this.spContainer.Panel1.Controls.Add(this.listaPedidoComandaSemCheckin1);
            this.spContainer.Panel1.Controls.Add(this.listaPedidoComanda1);
            // 
            // spContainer.Panel2
            // 
            this.spContainer.Panel2.Controls.Add(this.tableLayoutPanel3);
            this.spContainer.Size = new System.Drawing.Size(1014, 221);
            this.spContainer.SplitterDistance = 557;
            this.spContainer.SplitterWidth = 8;
            this.spContainer.TabIndex = 40;
            this.spContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spContainer_SplitterMoved);
            // 
            // listaPedidoEntrega1
            // 
            this.listaPedidoEntrega1.BackColor = System.Drawing.Color.White;
            this.listaPedidoEntrega1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listaPedidoEntrega1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listaPedidoEntrega1.Location = new System.Drawing.Point(0, 0);
            this.listaPedidoEntrega1.Margin = new System.Windows.Forms.Padding(4);
            this.listaPedidoEntrega1.Name = "listaPedidoEntrega1";
            this.listaPedidoEntrega1.Size = new System.Drawing.Size(557, 221);
            this.listaPedidoEntrega1.TabIndex = 88;
            // 
            // listaPedidoMesa1
            // 
            this.listaPedidoMesa1.BackColor = System.Drawing.Color.White;
            this.listaPedidoMesa1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listaPedidoMesa1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listaPedidoMesa1.Location = new System.Drawing.Point(0, 0);
            this.listaPedidoMesa1.Margin = new System.Windows.Forms.Padding(4);
            this.listaPedidoMesa1.Name = "listaPedidoMesa1";
            this.listaPedidoMesa1.Size = new System.Drawing.Size(557, 221);
            this.listaPedidoMesa1.TabIndex = 1;
            // 
            // listaPedidoComandaSemCheckin1
            // 
            this.listaPedidoComandaSemCheckin1.BackColor = System.Drawing.Color.White;
            this.listaPedidoComandaSemCheckin1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listaPedidoComandaSemCheckin1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.listaPedidoComandaSemCheckin1.GUIDIdentificacao_selecionado = null;
            this.listaPedidoComandaSemCheckin1.Location = new System.Drawing.Point(0, 0);
            this.listaPedidoComandaSemCheckin1.Margin = new System.Windows.Forms.Padding(4);
            this.listaPedidoComandaSemCheckin1.Name = "listaPedidoComandaSemCheckin1";
            this.listaPedidoComandaSemCheckin1.NumeroComanda = 0;
            this.listaPedidoComandaSemCheckin1.Size = new System.Drawing.Size(557, 221);
            this.listaPedidoComandaSemCheckin1.TabIndex = 91;
            this.listaPedidoComandaSemCheckin1.Visible = false;
            // 
            // listaPedidoComanda1
            // 
            this.listaPedidoComanda1.BackColor = System.Drawing.Color.White;
            this.listaPedidoComanda1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listaPedidoComanda1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listaPedidoComanda1.Location = new System.Drawing.Point(0, 0);
            this.listaPedidoComanda1.Margin = new System.Windows.Forms.Padding(5);
            this.listaPedidoComanda1.Name = "listaPedidoComanda1";
            this.listaPedidoComanda1.Size = new System.Drawing.Size(557, 221);
            this.listaPedidoComanda1.TabIndex = 89;
            this.listaPedidoComanda1.Visible = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.dgvItens, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(449, 221);
            this.tableLayoutPanel3.TabIndex = 83;
            // 
            // tbMenuTop
            // 
            this.tbMenuTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMenuTop.ColumnCount = 5;
            this.tbMenuTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbMenuTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbMenuTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbMenuTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbMenuTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbMenuTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tbMenuTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tbMenuTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tbMenuTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tbMenuTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tbMenuTop.Controls.Add(this.rdbMesas, 0, 0);
            this.tbMenuTop.Controls.Add(this.rdbComandas, 1, 0);
            this.tbMenuTop.Controls.Add(this.rdbEntrega, 2, 0);
            this.tbMenuTop.Controls.Add(this.rdbRetirada, 3, 0);
            this.tbMenuTop.Controls.Add(this.btnBalcao, 4, 0);
            this.tbMenuTop.Location = new System.Drawing.Point(0, 143);
            this.tbMenuTop.Name = "tbMenuTop";
            this.tbMenuTop.RowCount = 1;
            this.tbMenuTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbMenuTop.Size = new System.Drawing.Size(1021, 50);
            this.tbMenuTop.TabIndex = 40;
            // 
            // rdbRetirada
            // 
            this.rdbRetirada.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdbRetirada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rdbRetirada.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdbRetirada.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdbRetirada.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbRetirada.ForeColor = System.Drawing.Color.White;
            this.rdbRetirada.Location = new System.Drawing.Point(612, 3);
            this.rdbRetirada.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.rdbRetirada.Name = "rdbRetirada";
            this.rdbRetirada.Size = new System.Drawing.Size(200, 44);
            this.rdbRetirada.TabIndex = 3;
            this.rdbRetirada.Text = "&RETIRADA";
            this.rdbRetirada.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbRetirada.UseVisualStyleBackColor = false;
            this.rdbRetirada.Visible = false;
            this.rdbRetirada.CheckedChanged += new System.EventHandler(this.rdbRetirada_CheckedChanged);
            // 
            // btnBalcao
            // 
            this.btnBalcao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnBalcao.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnBalcao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBalcao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBalcao.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBalcao.ForeColor = System.Drawing.Color.White;
            this.btnBalcao.Location = new System.Drawing.Point(815, 3);
            this.btnBalcao.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.btnBalcao.Name = "btnBalcao";
            this.btnBalcao.Size = new System.Drawing.Size(206, 44);
            this.btnBalcao.TabIndex = 4;
            this.btnBalcao.Text = "&BALCÃO";
            this.btnBalcao.UseVisualStyleBackColor = false;
            this.btnBalcao.Visible = false;
            this.btnBalcao.Click += new System.EventHandler(this.btnBalcao_Click);
            // 
            // menuTEFcancelarTransacao
            // 
            this.menuTEFcancelarTransacao.Name = "menuTEFcancelarTransacao";
            this.menuTEFcancelarTransacao.Size = new System.Drawing.Size(32, 19);
            // 
            // frmPedidos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1024, 500);
            this.Controls.Add(this.tbMenuTop);
            this.Controls.Add(this.spContainer);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmPedidos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDV";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPedidos_FormClosing);
            this.Load += new System.EventHandler(this.frmPedidos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.spContainer.Panel1.ResumeLayout(false);
            this.spContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spContainer)).EndInit();
            this.spContainer.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tbMenuTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrPedidos;
        private System.Windows.Forms.Button btnAdicionarProduto;
        private System.Windows.Forms.Button btnImprimirConta;
        private System.Windows.Forms.Button btnPagamento;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Button btnTransferirProdutos;
        private System.Windows.Forms.DataGridView dgvItens;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTituloForm;
        private System.Windows.Forms.RadioButton rdbComandas;
        private System.Windows.Forms.RadioButton rdbMesas;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblValorServico;
        private System.Windows.Forms.Label lblValorProdutos;
        private System.Windows.Forms.Label lblValorTotal;
        private System.Windows.Forms.Label lblIdentificacao;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.Label lblValorConsumacaoMinima;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem comandasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alterarTipoDeEntradaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bloquearLiberarComandaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem juntarComandasToolStripMenuItem;
        private System.Windows.Forms.RadioButton rdbEntrega;
        private System.Windows.Forms.ToolStripMenuItem geralToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alterarDisponibilidadeDeProdutosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mesasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelarSolicitaçãoDeContaToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn descricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn qtd;
        private System.Windows.Forms.ToolStripMenuItem fiscalMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelarPedidoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem informarPerdaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reimprimirToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label lblTaxaEntrega;
        private System.Windows.Forms.Label lblTaxaEntregaTexto;
        private System.Windows.Forms.ToolStripMenuItem abrirGavetaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelarSolicitaçãoDeContaToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pagamentoParcialMesaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pagamentoParcialComandaToolStripMenuItem;
        private System.Windows.Forms.Label lblModoContingencia;
        private System.Windows.Forms.ToolStripMenuItem integracaoMenu;
        private System.Windows.Forms.ToolStripMenuItem iFoodMenu;
        private System.Windows.Forms.ToolStripMenuItem iFoodHabilitado;
        private System.Windows.Forms.ToolStripMenuItem iFoodAprovacao;
        private System.Windows.Forms.Timer tmrDelivery;
        private System.Windows.Forms.Timer tmrStatus;
        private System.Windows.Forms.ToolStripMenuItem clientesMenu;
        private System.Windows.Forms.ToolStripMenuItem clientesMenuCadastro;
        private System.Windows.Forms.ToolStripMenuItem clientesMenuSaldos;
        private System.Windows.Forms.Label lblInformacaoAdicional;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnAdicionarCreditos;
        private System.Windows.Forms.SplitContainer spContainer;
        private Controles.ListaPedidoComandaSemCheckin listaPedidoComandaSemCheckin1;
        private Controles.ListaPedidoComanda listaPedidoComanda1;
        private Controles.ListaPedidoEntrega listaPedidoEntrega1;
        private Controles.ListaPedidoMesa listaPedidoMesa1;
        private System.Windows.Forms.TableLayoutPanel tbMenuTop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ToolStripMenuItem menuTEFcancelarTransacao;
        private System.Windows.Forms.Button btnBalcao;
        private System.Windows.Forms.RadioButton rdbRetirada;
    }
}