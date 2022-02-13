namespace a7D.PDV.Caixa.UI
{
    partial class frmPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrincipal));
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblModoContingencia = new System.Windows.Forms.Label();
            this.btnFechar = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.comandasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bloquearLiberarComandaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.relatóriosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumoDoDiaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.produtosCanceladosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.reimprimirFechamentoDoDiaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fiscalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reduçãoZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leituraXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configuraçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alterarNomePDVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTEF = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuNTK = new System.Windows.Forms.ToolStripMenuItem();
            this.administrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStone = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelarOperaçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirGavetaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sobreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versãoSistemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contatoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerImpressora = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblModo = new System.Windows.Forms.Label();
            this.progressBarCarregando = new System.Windows.Forms.ProgressBar();
            this.lblCarregando = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblUsuarioAutenticado = new System.Windows.Forms.Label();
            this.lblVersao = new System.Windows.Forms.Label();
            this.lblPDV = new System.Windows.Forms.Label();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.pnlCaixaFechado = new System.Windows.Forms.Panel();
            this.btnFechamentoDia = new System.Windows.Forms.Button();
            this.btnAbrirCaixa = new System.Windows.Forms.Button();
            this.pnlCaixaAberto = new System.Windows.Forms.Panel();
            this.btnPedidos = new System.Windows.Forms.Button();
            this.btnAjusteCaixa = new System.Windows.Forms.Button();
            this.btnFecharCaixaTurno = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tmrGeral = new System.Windows.Forms.Timer(this.components);
            this.bwGeral = new System.ComponentModel.BackgroundWorker();
            this.panel3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlCaixaFechado.SuspendLayout();
            this.pnlCaixaAberto.SuspendLayout();
            this.SuspendLayout();
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
            this.panel3.TabIndex = 84;
            // 
            // lblModoContingencia
            // 
            this.lblModoContingencia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModoContingencia.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModoContingencia.ForeColor = System.Drawing.Color.Tomato;
            this.lblModoContingencia.Location = new System.Drawing.Point(71, 5);
            this.lblModoContingencia.Name = "lblModoContingencia";
            this.lblModoContingencia.Size = new System.Drawing.Size(882, 29);
            this.lblModoContingencia.TabIndex = 2;
            this.lblModoContingencia.Text = "EM MODO DE CONTINGÊNCIA";
            this.lblModoContingencia.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFechar
            // 
            this.btnFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFechar.BackgroundImage = global::a7D.PDV.Caixa.UI.Properties.Resources.btnFechar;
            this.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechar.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFechar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnFechar.Location = new System.Drawing.Point(997, 11);
            this.btnFechar.Margin = new System.Windows.Forms.Padding(0);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(18, 19);
            this.btnFechar.TabIndex = 0;
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Enabled = false;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.comandasToolStripMenuItem,
            this.relatóriosToolStripMenuItem,
            this.fiscalToolStripMenuItem,
            this.configuraçãoToolStripMenuItem,
            this.MenuTEF,
            this.abrirGavetaToolStripMenuItem,
            this.sairToolStripMenuItem,
            this.sobreToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1024, 33);
            this.menuStrip1.TabIndex = 85;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // comandasToolStripMenuItem
            // 
            this.comandasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bloquearLiberarComandaToolStripMenuItem,
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem});
            this.comandasToolStripMenuItem.Name = "comandasToolStripMenuItem";
            this.comandasToolStripMenuItem.Size = new System.Drawing.Size(76, 29);
            this.comandasToolStripMenuItem.Text = "Ações";
            // 
            // bloquearLiberarComandaToolStripMenuItem
            // 
            this.bloquearLiberarComandaToolStripMenuItem.Name = "bloquearLiberarComandaToolStripMenuItem";
            this.bloquearLiberarComandaToolStripMenuItem.Size = new System.Drawing.Size(397, 34);
            this.bloquearLiberarComandaToolStripMenuItem.Text = "Bloquear/Liberar comanda";
            this.bloquearLiberarComandaToolStripMenuItem.Click += new System.EventHandler(this.bloquearLiberarComandaToolStripMenuItem_Click);
            // 
            // alterarDisponibilidadeDeProdutosToolStripMenuItem
            // 
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem.Name = "alterarDisponibilidadeDeProdutosToolStripMenuItem";
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem.Size = new System.Drawing.Size(397, 34);
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem.Text = "Alterar disponibilidade de produtos";
            this.alterarDisponibilidadeDeProdutosToolStripMenuItem.Click += new System.EventHandler(this.alterarDisponibilidadeDeProdutosToolStripMenuItem_Click);
            // 
            // relatóriosToolStripMenuItem
            // 
            this.relatóriosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resumoDoDiaToolStripMenuItem,
            this.produtosCanceladosToolStripMenuItem,
            this.toolStripSeparator2,
            this.reimprimirFechamentoDoDiaToolStripMenuItem});
            this.relatóriosToolStripMenuItem.Name = "relatóriosToolStripMenuItem";
            this.relatóriosToolStripMenuItem.Size = new System.Drawing.Size(106, 29);
            this.relatóriosToolStripMenuItem.Text = "Relatórios";
            // 
            // resumoDoDiaToolStripMenuItem
            // 
            this.resumoDoDiaToolStripMenuItem.Name = "resumoDoDiaToolStripMenuItem";
            this.resumoDoDiaToolStripMenuItem.Size = new System.Drawing.Size(360, 34);
            this.resumoDoDiaToolStripMenuItem.Text = "Resumo do dia";
            this.resumoDoDiaToolStripMenuItem.Click += new System.EventHandler(this.resumoDoDiaToolStripMenuItem_Click);
            // 
            // produtosCanceladosToolStripMenuItem
            // 
            this.produtosCanceladosToolStripMenuItem.Name = "produtosCanceladosToolStripMenuItem";
            this.produtosCanceladosToolStripMenuItem.Size = new System.Drawing.Size(360, 34);
            this.produtosCanceladosToolStripMenuItem.Text = "Produtos Cancelados";
            this.produtosCanceladosToolStripMenuItem.Click += new System.EventHandler(this.produtosCanceladosToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(357, 6);
            // 
            // reimprimirFechamentoDoDiaToolStripMenuItem
            // 
            this.reimprimirFechamentoDoDiaToolStripMenuItem.Name = "reimprimirFechamentoDoDiaToolStripMenuItem";
            this.reimprimirFechamentoDoDiaToolStripMenuItem.Size = new System.Drawing.Size(360, 34);
            this.reimprimirFechamentoDoDiaToolStripMenuItem.Text = "Reimprimir Fechamento do Dia";
            this.reimprimirFechamentoDoDiaToolStripMenuItem.Click += new System.EventHandler(this.reimprimirFechamentoDoDiaToolStripMenuItem_Click);
            // 
            // fiscalToolStripMenuItem
            // 
            this.fiscalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reduçãoZToolStripMenuItem,
            this.leituraXToolStripMenuItem});
            this.fiscalToolStripMenuItem.Name = "fiscalToolStripMenuItem";
            this.fiscalToolStripMenuItem.Size = new System.Drawing.Size(70, 29);
            this.fiscalToolStripMenuItem.Text = "Fiscal";
            // 
            // reduçãoZToolStripMenuItem
            // 
            this.reduçãoZToolStripMenuItem.Name = "reduçãoZToolStripMenuItem";
            this.reduçãoZToolStripMenuItem.Size = new System.Drawing.Size(197, 34);
            this.reduçãoZToolStripMenuItem.Text = "Redução Z";
            this.reduçãoZToolStripMenuItem.Click += new System.EventHandler(this.reduçãoZToolStripMenuItem_Click);
            // 
            // leituraXToolStripMenuItem
            // 
            this.leituraXToolStripMenuItem.Name = "leituraXToolStripMenuItem";
            this.leituraXToolStripMenuItem.Size = new System.Drawing.Size(197, 34);
            this.leituraXToolStripMenuItem.Text = "Leitura X";
            this.leituraXToolStripMenuItem.Click += new System.EventHandler(this.leituraXToolStripMenuItem_Click);
            // 
            // configuraçãoToolStripMenuItem
            // 
            this.configuraçãoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alterarNomePDVToolStripMenuItem});
            this.configuraçãoToolStripMenuItem.Name = "configuraçãoToolStripMenuItem";
            this.configuraçãoToolStripMenuItem.Size = new System.Drawing.Size(134, 29);
            this.configuraçãoToolStripMenuItem.Text = "Configuração";
            // 
            // alterarNomePDVToolStripMenuItem
            // 
            this.alterarNomePDVToolStripMenuItem.Name = "alterarNomePDVToolStripMenuItem";
            this.alterarNomePDVToolStripMenuItem.Size = new System.Drawing.Size(256, 34);
            this.alterarNomePDVToolStripMenuItem.Text = "Alterar nome PDV";
            this.alterarNomePDVToolStripMenuItem.Click += new System.EventHandler(this.alterarNomePDVToolStripMenuItem_Click);
            // 
            // MenuTEF
            // 
            this.MenuTEF.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuNTK,
            this.MenuStone});
            this.MenuTEF.Name = "MenuTEF";
            this.MenuTEF.Size = new System.Drawing.Size(55, 29);
            this.MenuTEF.Text = "TEF";
            this.MenuTEF.Visible = false;
            // 
            // MenuNTK
            // 
            this.MenuNTK.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.administrarToolStripMenuItem});
            this.MenuNTK.Name = "MenuNTK";
            this.MenuNTK.Size = new System.Drawing.Size(270, 34);
            this.MenuNTK.Text = "Pay Go";
            this.MenuNTK.Visible = false;
            // 
            // administrarToolStripMenuItem
            // 
            this.administrarToolStripMenuItem.Name = "administrarToolStripMenuItem";
            this.administrarToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.administrarToolStripMenuItem.Text = "Administrar";
            this.administrarToolStripMenuItem.Click += new System.EventHandler(this.AdministrarToolStripMenuItem_Click);
            // 
            // MenuStone
            // 
            this.MenuStone.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cancelarOperaçãoToolStripMenuItem});
            this.MenuStone.Name = "MenuStone";
            this.MenuStone.Size = new System.Drawing.Size(270, 34);
            this.MenuStone.Text = "Stone";
            this.MenuStone.Visible = false;
            // 
            // cancelarOperaçãoToolStripMenuItem
            // 
            this.cancelarOperaçãoToolStripMenuItem.Name = "cancelarOperaçãoToolStripMenuItem";
            this.cancelarOperaçãoToolStripMenuItem.Size = new System.Drawing.Size(270, 34);
            this.cancelarOperaçãoToolStripMenuItem.Text = "Cancelar Transação";
            this.cancelarOperaçãoToolStripMenuItem.Click += new System.EventHandler(this.CancelarOperaçãoToolStripMenuItem_Click);
            // 
            // abrirGavetaToolStripMenuItem
            // 
            this.abrirGavetaToolStripMenuItem.Name = "abrirGavetaToolStripMenuItem";
            this.abrirGavetaToolStripMenuItem.Size = new System.Drawing.Size(126, 29);
            this.abrirGavetaToolStripMenuItem.Text = "Abrir &Gaveta";
            this.abrirGavetaToolStripMenuItem.Click += new System.EventHandler(this.abrirGavetaToolStripMenuItem_Click);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(57, 29);
            this.sairToolStripMenuItem.Text = "Sair";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // sobreToolStripMenuItem
            // 
            this.sobreToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versãoSistemaToolStripMenuItem,
            this.contatoToolStripMenuItem});
            this.sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            this.sobreToolStripMenuItem.Size = new System.Drawing.Size(75, 29);
            this.sobreToolStripMenuItem.Text = "Sobre";
            this.sobreToolStripMenuItem.Visible = false;
            // 
            // versãoSistemaToolStripMenuItem
            // 
            this.versãoSistemaToolStripMenuItem.Name = "versãoSistemaToolStripMenuItem";
            this.versãoSistemaToolStripMenuItem.Size = new System.Drawing.Size(234, 34);
            this.versãoSistemaToolStripMenuItem.Text = "Versão Sistema";
            // 
            // contatoToolStripMenuItem
            // 
            this.contatoToolStripMenuItem.Name = "contatoToolStripMenuItem";
            this.contatoToolStripMenuItem.Size = new System.Drawing.Size(234, 34);
            this.contatoToolStripMenuItem.Text = "Contato";
            // 
            // timerImpressora
            // 
            this.timerImpressora.Interval = 60000;
            this.timerImpressora.Tick += new System.EventHandler(this.timerImpressora_Tick);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel2.Controls.Add(this.lblModo);
            this.panel2.Location = new System.Drawing.Point(0, 118);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(498, 591);
            this.panel2.TabIndex = 88;
            // 
            // lblModo
            // 
            this.lblModo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModo.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModo.ForeColor = System.Drawing.Color.Gray;
            this.lblModo.Location = new System.Drawing.Point(3, 524);
            this.lblModo.Name = "lblModo";
            this.lblModo.Size = new System.Drawing.Size(492, 68);
            this.lblModo.TabIndex = 57;
            this.lblModo.Text = "-";
            this.lblModo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // progressBarCarregando
            // 
            this.progressBarCarregando.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarCarregando.Location = new System.Drawing.Point(9, 72);
            this.progressBarCarregando.Name = "progressBarCarregando";
            this.progressBarCarregando.Size = new System.Drawing.Size(485, 23);
            this.progressBarCarregando.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarCarregando.TabIndex = 59;
            this.progressBarCarregando.Visible = false;
            // 
            // lblCarregando
            // 
            this.lblCarregando.AutoSize = true;
            this.lblCarregando.Location = new System.Drawing.Point(6, 98);
            this.lblCarregando.Name = "lblCarregando";
            this.lblCarregando.Size = new System.Drawing.Size(151, 26);
            this.lblCarregando.TabIndex = 58;
            this.lblCarregando.Text = "Carregando...";
            this.lblCarregando.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackgroundImage = global::a7D.PDV.Caixa.UI.Properties.Resources.bg_Titulo;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.lblPDV);
            this.panel1.Controls.Add(this.lblMensagem);
            this.panel1.Controls.Add(this.pnlCaixaFechado);
            this.panel1.Controls.Add(this.pnlCaixaAberto);
            this.panel1.Location = new System.Drawing.Point(500, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(518, 638);
            this.panel1.TabIndex = 87;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblUsuarioAutenticado, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblVersao, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 527);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(501, 100);
            this.tableLayoutPanel1.TabIndex = 58;
            // 
            // lblUsuarioAutenticado
            // 
            this.lblUsuarioAutenticado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsuarioAutenticado.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuarioAutenticado.ForeColor = System.Drawing.Color.DimGray;
            this.lblUsuarioAutenticado.Location = new System.Drawing.Point(3, 0);
            this.lblUsuarioAutenticado.Name = "lblUsuarioAutenticado";
            this.lblUsuarioAutenticado.Size = new System.Drawing.Size(495, 50);
            this.lblUsuarioAutenticado.TabIndex = 46;
            this.lblUsuarioAutenticado.Text = "-";
            this.lblUsuarioAutenticado.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblVersao
            // 
            this.lblVersao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVersao.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersao.ForeColor = System.Drawing.Color.Gray;
            this.lblVersao.Location = new System.Drawing.Point(3, 50);
            this.lblVersao.Name = "lblVersao";
            this.lblVersao.Size = new System.Drawing.Size(495, 50);
            this.lblVersao.TabIndex = 47;
            this.lblVersao.Text = "-";
            this.lblVersao.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblPDV
            // 
            this.lblPDV.BackColor = System.Drawing.Color.Transparent;
            this.lblPDV.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPDV.ForeColor = System.Drawing.Color.White;
            this.lblPDV.Location = new System.Drawing.Point(3, 0);
            this.lblPDV.Name = "lblPDV";
            this.lblPDV.Size = new System.Drawing.Size(510, 61);
            this.lblPDV.TabIndex = 57;
            this.lblPDV.Text = "-";
            this.lblPDV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMensagem
            // 
            this.lblMensagem.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensagem.ForeColor = System.Drawing.Color.Gray;
            this.lblMensagem.Location = new System.Drawing.Point(3, 72);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(511, 166);
            this.lblMensagem.TabIndex = 50;
            this.lblMensagem.Text = "CAIXA FECHADO";
            this.lblMensagem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // pnlCaixaFechado
            // 
            this.pnlCaixaFechado.Controls.Add(this.btnFechamentoDia);
            this.pnlCaixaFechado.Controls.Add(this.btnAbrirCaixa);
            this.pnlCaixaFechado.Location = new System.Drawing.Point(114, 240);
            this.pnlCaixaFechado.Name = "pnlCaixaFechado";
            this.pnlCaixaFechado.Size = new System.Drawing.Size(280, 203);
            this.pnlCaixaFechado.TabIndex = 56;
            this.pnlCaixaFechado.Visible = false;
            // 
            // btnFechamentoDia
            // 
            this.btnFechamentoDia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFechamentoDia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnFechamentoDia.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFechamentoDia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFechamentoDia.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFechamentoDia.ForeColor = System.Drawing.Color.White;
            this.btnFechamentoDia.Location = new System.Drawing.Point(3, 50);
            this.btnFechamentoDia.Name = "btnFechamentoDia";
            this.btnFechamentoDia.Size = new System.Drawing.Size(272, 39);
            this.btnFechamentoDia.TabIndex = 3;
            this.btnFechamentoDia.Text = "FECHAMENTO DIA";
            this.btnFechamentoDia.UseVisualStyleBackColor = false;
            this.btnFechamentoDia.Click += new System.EventHandler(this.btnFechamentoDia_Click);
            // 
            // btnAbrirCaixa
            // 
            this.btnAbrirCaixa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbrirCaixa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnAbrirCaixa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAbrirCaixa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbrirCaixa.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbrirCaixa.ForeColor = System.Drawing.Color.White;
            this.btnAbrirCaixa.Location = new System.Drawing.Point(3, 5);
            this.btnAbrirCaixa.Name = "btnAbrirCaixa";
            this.btnAbrirCaixa.Size = new System.Drawing.Size(272, 39);
            this.btnAbrirCaixa.TabIndex = 0;
            this.btnAbrirCaixa.Text = "ABRIR CAIXA";
            this.btnAbrirCaixa.UseVisualStyleBackColor = false;
            this.btnAbrirCaixa.Click += new System.EventHandler(this.btnAbrirCaixa_Click);
            // 
            // pnlCaixaAberto
            // 
            this.pnlCaixaAberto.Controls.Add(this.btnPedidos);
            this.pnlCaixaAberto.Controls.Add(this.btnAjusteCaixa);
            this.pnlCaixaAberto.Controls.Add(this.btnFecharCaixaTurno);
            this.pnlCaixaAberto.Location = new System.Drawing.Point(114, 240);
            this.pnlCaixaAberto.Name = "pnlCaixaAberto";
            this.pnlCaixaAberto.Size = new System.Drawing.Size(280, 203);
            this.pnlCaixaAberto.TabIndex = 55;
            this.pnlCaixaAberto.Visible = false;
            // 
            // btnPedidos
            // 
            this.btnPedidos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPedidos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(131)))), ((int)(((byte)(159)))));
            this.btnPedidos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPedidos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPedidos.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPedidos.ForeColor = System.Drawing.Color.White;
            this.btnPedidos.Location = new System.Drawing.Point(3, 95);
            this.btnPedidos.Name = "btnPedidos";
            this.btnPedidos.Size = new System.Drawing.Size(272, 39);
            this.btnPedidos.TabIndex = 55;
            this.btnPedidos.Text = "MONITOR DE PEDIDOS";
            this.btnPedidos.UseVisualStyleBackColor = false;
            this.btnPedidos.Click += new System.EventHandler(this.btnPedidos_Click);
            // 
            // btnAjusteCaixa
            // 
            this.btnAjusteCaixa.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAjusteCaixa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(131)))), ((int)(((byte)(159)))));
            this.btnAjusteCaixa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAjusteCaixa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAjusteCaixa.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAjusteCaixa.ForeColor = System.Drawing.Color.White;
            this.btnAjusteCaixa.Location = new System.Drawing.Point(3, 50);
            this.btnAjusteCaixa.Name = "btnAjusteCaixa";
            this.btnAjusteCaixa.Size = new System.Drawing.Size(272, 39);
            this.btnAjusteCaixa.TabIndex = 55;
            this.btnAjusteCaixa.Text = "SANGRIA OU SUPRIMENTO";
            this.btnAjusteCaixa.UseVisualStyleBackColor = false;
            this.btnAjusteCaixa.Click += new System.EventHandler(this.btnAjusteCaixa_Click);
            // 
            // btnFecharCaixaTurno
            // 
            this.btnFecharCaixaTurno.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFecharCaixaTurno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnFecharCaixaTurno.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFecharCaixaTurno.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFecharCaixaTurno.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFecharCaixaTurno.ForeColor = System.Drawing.Color.White;
            this.btnFecharCaixaTurno.Location = new System.Drawing.Point(3, 5);
            this.btnFecharCaixaTurno.Name = "btnFecharCaixaTurno";
            this.btnFecharCaixaTurno.Size = new System.Drawing.Size(272, 39);
            this.btnFecharCaixaTurno.TabIndex = 55;
            this.btnFecharCaixaTurno.Text = "FECHAR CAIXA";
            this.btnFecharCaixaTurno.UseVisualStyleBackColor = false;
            this.btnFecharCaixaTurno.Click += new System.EventHandler(this.btnFecharCaixaTurno_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // tmrGeral
            // 
            this.tmrGeral.Interval = 3000;
            this.tmrGeral.Tick += new System.EventHandler(this.tmrGeral_Tick);
            // 
            // bwGeral
            // 
            this.bwGeral.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwGeral_DoWork);
            this.bwGeral.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwGeral_RunWorkerCompleted);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1024, 711);
            this.Controls.Add(this.progressBarCarregando);
            this.Controls.Add(this.lblCarregando);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDV";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            this.panel3.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlCaixaFechado.ResumeLayout(false);
            this.pnlCaixaAberto.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblUsuarioAutenticado;
        private System.Windows.Forms.Label lblVersao;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnFechar;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblMensagem;
        private System.Windows.Forms.Panel pnlCaixaAberto;
        private System.Windows.Forms.Button btnPedidos;
        private System.Windows.Forms.Button btnAjusteCaixa;
        private System.Windows.Forms.Button btnFecharCaixaTurno;
        private System.Windows.Forms.Panel pnlCaixaFechado;
        private System.Windows.Forms.Button btnAbrirCaixa;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem fiscalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem relatóriosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem comandasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bloquearLiberarComandaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alterarDisponibilidadeDeProdutosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reduçãoZToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leituraXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sobreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem versãoSistemaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contatoToolStripMenuItem;
        private System.Windows.Forms.Button btnFechamentoDia;
        private System.Windows.Forms.ToolStripMenuItem resumoDoDiaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem produtosCanceladosToolStripMenuItem;
        private System.Windows.Forms.Timer timerImpressora;
        private System.Windows.Forms.Label lblModo;
        private System.Windows.Forms.ToolStripMenuItem configuraçãoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alterarNomePDVToolStripMenuItem;
        private System.Windows.Forms.Label lblPDV;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem reimprimirFechamentoDoDiaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirGavetaToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblCarregando;
        private System.Windows.Forms.ProgressBar progressBarCarregando;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer tmrGeral;
        private System.Windows.Forms.Label lblModoContingencia;
        private System.ComponentModel.BackgroundWorker bwGeral;
        private System.Windows.Forms.ToolStripMenuItem MenuTEF;
        private System.Windows.Forms.ToolStripMenuItem MenuNTK;
        private System.Windows.Forms.ToolStripMenuItem administrarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MenuStone;
        private System.Windows.Forms.ToolStripMenuItem cancelarOperaçãoToolStripMenuItem;
    }
}