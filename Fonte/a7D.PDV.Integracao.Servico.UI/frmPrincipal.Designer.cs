namespace a7D.PDV.Integracao.Servico.UI
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Caixa"}, "CaixaIcon.png", System.Drawing.Color.DarkRed, System.Drawing.SystemColors.ActiveBorder, null);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Impressora", "ImpressoraIcon.png");
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pbServer = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pbDeliveryOnline = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.pbIFood = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pbImpressao = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pbPagamentoIntegrado = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPOS = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.txtStatusPOS = new System.Windows.Forms.TextBox();
            this.tabServicos = new System.Windows.Forms.TabControl();
            this.tabServidor = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStatusServer = new System.Windows.Forms.TextBox();
            this.tabImpressao = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.txtStatusImpressao = new System.Windows.Forms.TextBox();
            this.tabIFood = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.txtIFood = new System.Windows.Forms.TextBox();
            this.tabDeliveryOnline = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.txtStatusDeliveryOnline = new System.Windows.Forms.TextBox();
            this.ntf = new System.Windows.Forms.NotifyIcon(this.components);
            this.mnu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSair = new System.Windows.Forms.ToolStripMenuItem();
            this.tmr = new System.Windows.Forms.Timer(this.components);
            this.st = new System.Windows.Forms.StatusStrip();
            this.stCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.stStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lvStatus = new System.Windows.Forms.ListView();
            this.imgs = new System.Windows.Forms.ImageList(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabPixConta = new System.Windows.Forms.TabPage();
            this.pbPixConta = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtStatusPixConta = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeliveryOnline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIFood)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImpressao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPagamentoIntegrado)).BeginInit();
            this.tabPOS.SuspendLayout();
            this.tabServicos.SuspendLayout();
            this.tabServidor.SuspendLayout();
            this.tabImpressao.SuspendLayout();
            this.tabIFood.SuspendLayout();
            this.tabDeliveryOnline.SuspendLayout();
            this.mnu.SuspendLayout();
            this.st.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPixConta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPixConta)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pbPixConta);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.pbServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pbDeliveryOnline);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.pbIFood);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.pbImpressao);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.pbPagamentoIntegrado);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 190);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serviços";
            // 
            // pbServer
            // 
            this.pbServer.BackColor = System.Drawing.Color.Silver;
            this.pbServer.Location = new System.Drawing.Point(9, 27);
            this.pbServer.Name = "pbServer";
            this.pbServer.Size = new System.Drawing.Size(23, 20);
            this.pbServer.TabIndex = 15;
            this.pbServer.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 14;
            this.label1.Text = "Servidor PDV7";
            // 
            // pbDeliveryOnline
            // 
            this.pbDeliveryOnline.BackColor = System.Drawing.Color.Silver;
            this.pbDeliveryOnline.Location = new System.Drawing.Point(9, 130);
            this.pbDeliveryOnline.Name = "pbDeliveryOnline";
            this.pbDeliveryOnline.Size = new System.Drawing.Size(23, 20);
            this.pbDeliveryOnline.TabIndex = 13;
            this.pbDeliveryOnline.TabStop = false;
            this.pbDeliveryOnline.Click += new System.EventHandler(this.Status_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(38, 130);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(104, 17);
            this.label11.TabIndex = 12;
            this.label11.Text = "Delivery Online";
            // 
            // pbIFood
            // 
            this.pbIFood.BackColor = System.Drawing.Color.Silver;
            this.pbIFood.Location = new System.Drawing.Point(9, 104);
            this.pbIFood.Name = "pbIFood";
            this.pbIFood.Size = new System.Drawing.Size(23, 20);
            this.pbIFood.TabIndex = 11;
            this.pbIFood.TabStop = false;
            this.pbIFood.Click += new System.EventHandler(this.Status_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(38, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 17);
            this.label9.TabIndex = 10;
            this.label9.Text = "iFood";
            // 
            // pbImpressao
            // 
            this.pbImpressao.BackColor = System.Drawing.Color.Silver;
            this.pbImpressao.Location = new System.Drawing.Point(9, 52);
            this.pbImpressao.Name = "pbImpressao";
            this.pbImpressao.Size = new System.Drawing.Size(23, 20);
            this.pbImpressao.TabIndex = 7;
            this.pbImpressao.TabStop = false;
            this.pbImpressao.Click += new System.EventHandler(this.Status_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Ordem de Impressão";
            // 
            // pbPagamentoIntegrado
            // 
            this.pbPagamentoIntegrado.BackColor = System.Drawing.Color.Silver;
            this.pbPagamentoIntegrado.Location = new System.Drawing.Point(9, 78);
            this.pbPagamentoIntegrado.Name = "pbPagamentoIntegrado";
            this.pbPagamentoIntegrado.Size = new System.Drawing.Size(23, 20);
            this.pbPagamentoIntegrado.TabIndex = 5;
            this.pbPagamentoIntegrado.TabStop = false;
            this.pbPagamentoIntegrado.Click += new System.EventHandler(this.Status_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Pagamento Integrado";
            // 
            // tabPOS
            // 
            this.tabPOS.Controls.Add(this.label6);
            this.tabPOS.Controls.Add(this.txtStatusPOS);
            this.tabPOS.Location = new System.Drawing.Point(4, 22);
            this.tabPOS.Name = "tabPOS";
            this.tabPOS.Size = new System.Drawing.Size(627, 373);
            this.tabPOS.TabIndex = 2;
            this.tabPOS.Text = "Pagamento Integrado";
            this.tabPOS.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Log:";
            // 
            // txtStatusPOS
            // 
            this.txtStatusPOS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatusPOS.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.txtStatusPOS.Location = new System.Drawing.Point(10, 27);
            this.txtStatusPOS.Multiline = true;
            this.txtStatusPOS.Name = "txtStatusPOS";
            this.txtStatusPOS.ReadOnly = true;
            this.txtStatusPOS.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatusPOS.Size = new System.Drawing.Size(607, 336);
            this.txtStatusPOS.TabIndex = 4;
            // 
            // tabServicos
            // 
            this.tabServicos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabServicos.Controls.Add(this.tabServidor);
            this.tabServicos.Controls.Add(this.tabImpressao);
            this.tabServicos.Controls.Add(this.tabPOS);
            this.tabServicos.Controls.Add(this.tabIFood);
            this.tabServicos.Controls.Add(this.tabDeliveryOnline);
            this.tabServicos.Controls.Add(this.tabPixConta);
            this.tabServicos.Location = new System.Drawing.Point(256, 12);
            this.tabServicos.Multiline = true;
            this.tabServicos.Name = "tabServicos";
            this.tabServicos.SelectedIndex = 0;
            this.tabServicos.Size = new System.Drawing.Size(635, 399);
            this.tabServicos.TabIndex = 2;
            // 
            // tabServidor
            // 
            this.tabServidor.Controls.Add(this.label2);
            this.tabServidor.Controls.Add(this.txtStatusServer);
            this.tabServidor.Location = new System.Drawing.Point(4, 22);
            this.tabServidor.Name = "tabServidor";
            this.tabServidor.Padding = new System.Windows.Forms.Padding(3);
            this.tabServidor.Size = new System.Drawing.Size(627, 373);
            this.tabServidor.TabIndex = 7;
            this.tabServidor.Text = "Servidor PDV7";
            this.tabServidor.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Log:";
            // 
            // txtStatusServer
            // 
            this.txtStatusServer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatusServer.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.txtStatusServer.Location = new System.Drawing.Point(10, 27);
            this.txtStatusServer.Multiline = true;
            this.txtStatusServer.Name = "txtStatusServer";
            this.txtStatusServer.ReadOnly = true;
            this.txtStatusServer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatusServer.Size = new System.Drawing.Size(607, 352);
            this.txtStatusServer.TabIndex = 6;
            // 
            // tabImpressao
            // 
            this.tabImpressao.Controls.Add(this.label5);
            this.tabImpressao.Controls.Add(this.txtStatusImpressao);
            this.tabImpressao.Location = new System.Drawing.Point(4, 22);
            this.tabImpressao.Name = "tabImpressao";
            this.tabImpressao.Padding = new System.Windows.Forms.Padding(3);
            this.tabImpressao.Size = new System.Drawing.Size(627, 373);
            this.tabImpressao.TabIndex = 3;
            this.tabImpressao.Text = "Ordem de Impressão";
            this.tabImpressao.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Log:";
            // 
            // txtStatusImpressao
            // 
            this.txtStatusImpressao.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatusImpressao.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.txtStatusImpressao.Location = new System.Drawing.Point(10, 27);
            this.txtStatusImpressao.Multiline = true;
            this.txtStatusImpressao.Name = "txtStatusImpressao";
            this.txtStatusImpressao.ReadOnly = true;
            this.txtStatusImpressao.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatusImpressao.Size = new System.Drawing.Size(607, 336);
            this.txtStatusImpressao.TabIndex = 4;
            // 
            // tabIFood
            // 
            this.tabIFood.Controls.Add(this.label10);
            this.tabIFood.Controls.Add(this.txtIFood);
            this.tabIFood.Location = new System.Drawing.Point(4, 22);
            this.tabIFood.Name = "tabIFood";
            this.tabIFood.Padding = new System.Windows.Forms.Padding(3);
            this.tabIFood.Size = new System.Drawing.Size(627, 373);
            this.tabIFood.TabIndex = 5;
            this.tabIFood.Text = "iFood";
            this.tabIFood.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Log:";
            // 
            // txtIFood
            // 
            this.txtIFood.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIFood.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.txtIFood.Location = new System.Drawing.Point(10, 27);
            this.txtIFood.Multiline = true;
            this.txtIFood.Name = "txtIFood";
            this.txtIFood.ReadOnly = true;
            this.txtIFood.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtIFood.Size = new System.Drawing.Size(607, 336);
            this.txtIFood.TabIndex = 8;
            // 
            // tabDeliveryOnline
            // 
            this.tabDeliveryOnline.Controls.Add(this.label12);
            this.tabDeliveryOnline.Controls.Add(this.txtStatusDeliveryOnline);
            this.tabDeliveryOnline.Location = new System.Drawing.Point(4, 22);
            this.tabDeliveryOnline.Name = "tabDeliveryOnline";
            this.tabDeliveryOnline.Padding = new System.Windows.Forms.Padding(3);
            this.tabDeliveryOnline.Size = new System.Drawing.Size(627, 373);
            this.tabDeliveryOnline.TabIndex = 6;
            this.tabDeliveryOnline.Text = "Delivery Online";
            this.tabDeliveryOnline.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(28, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Log:";
            // 
            // txtStatusDeliveryOnline
            // 
            this.txtStatusDeliveryOnline.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatusDeliveryOnline.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusDeliveryOnline.Location = new System.Drawing.Point(10, 27);
            this.txtStatusDeliveryOnline.Multiline = true;
            this.txtStatusDeliveryOnline.Name = "txtStatusDeliveryOnline";
            this.txtStatusDeliveryOnline.ReadOnly = true;
            this.txtStatusDeliveryOnline.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatusDeliveryOnline.Size = new System.Drawing.Size(607, 336);
            this.txtStatusDeliveryOnline.TabIndex = 8;
            // 
            // ntf
            // 
            this.ntf.ContextMenuStrip = this.mnu;
            this.ntf.Icon = ((System.Drawing.Icon)(resources.GetObject("ntf.Icon")));
            this.ntf.Text = "Integrador";
            this.ntf.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ntf_MouseDoubleClick);
            // 
            // mnu
            // 
            this.mnu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.mnu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuConfig,
            this.mnuSair});
            this.mnu.Name = "mnu";
            this.mnu.Size = new System.Drawing.Size(152, 48);
            // 
            // mnuConfig
            // 
            this.mnuConfig.Name = "mnuConfig";
            this.mnuConfig.Size = new System.Drawing.Size(151, 22);
            this.mnuConfig.Text = "Configurações";
            this.mnuConfig.Click += new System.EventHandler(this.mnuConfig_Click);
            // 
            // mnuSair
            // 
            this.mnuSair.Name = "mnuSair";
            this.mnuSair.Size = new System.Drawing.Size(151, 22);
            this.mnuSair.Text = "Sair";
            this.mnuSair.Click += new System.EventHandler(this.mnuSair_Click);
            // 
            // tmr
            // 
            this.tmr.Interval = 2000;
            this.tmr.Tick += new System.EventHandler(this.tmr_Tick);
            // 
            // st
            // 
            this.st.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.st.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stCount,
            this.stStatus});
            this.st.Location = new System.Drawing.Point(0, 418);
            this.st.Name = "st";
            this.st.Size = new System.Drawing.Size(903, 22);
            this.st.TabIndex = 8;
            this.st.Text = "st";
            // 
            // stCount
            // 
            this.stCount.Margin = new System.Windows.Forms.Padding(5, 3, 5, 2);
            this.stCount.Name = "stCount";
            this.stCount.Size = new System.Drawing.Size(13, 17);
            this.stCount.Text = "0";
            // 
            // stStatus
            // 
            this.stStatus.Name = "stStatus";
            this.stStatus.Size = new System.Drawing.Size(78, 17);
            this.stStatus.Text = "Carregando...";
            // 
            // lvStatus
            // 
            this.lvStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lvStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lvStatus.FullRowSelect = true;
            this.lvStatus.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            this.lvStatus.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.lvStatus.LabelWrap = false;
            this.lvStatus.Location = new System.Drawing.Point(6, 22);
            this.lvStatus.MultiSelect = false;
            this.lvStatus.Name = "lvStatus";
            this.lvStatus.ShowItemToolTips = true;
            this.lvStatus.Size = new System.Drawing.Size(225, 175);
            this.lvStatus.SmallImageList = this.imgs;
            this.lvStatus.TabIndex = 16;
            this.lvStatus.TileSize = new System.Drawing.Size(168, 30);
            this.lvStatus.UseCompatibleStateImageBehavior = false;
            this.lvStatus.View = System.Windows.Forms.View.List;
            // 
            // imgs
            // 
            this.imgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgs.ImageStream")));
            this.imgs.TransparentColor = System.Drawing.Color.Transparent;
            this.imgs.Images.SetKeyName(0, "ImpressoraIcon.png");
            this.imgs.Images.SetKeyName(1, "CaixaIcon.png");
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.lvStatus);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.groupBox2.Location = new System.Drawing.Point(12, 208);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 203);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Áreas de Impressão";
            // 
            // tabPixConta
            // 
            this.tabPixConta.Controls.Add(this.label8);
            this.tabPixConta.Controls.Add(this.txtStatusPixConta);
            this.tabPixConta.Location = new System.Drawing.Point(4, 22);
            this.tabPixConta.Name = "tabPixConta";
            this.tabPixConta.Padding = new System.Windows.Forms.Padding(3);
            this.tabPixConta.Size = new System.Drawing.Size(627, 373);
            this.tabPixConta.TabIndex = 8;
            this.tabPixConta.Text = "Pix-Conta";
            this.tabPixConta.UseVisualStyleBackColor = true;
            // 
            // pbPixConta
            // 
            this.pbPixConta.BackColor = System.Drawing.Color.Silver;
            this.pbPixConta.Location = new System.Drawing.Point(9, 157);
            this.pbPixConta.Name = "pbPixConta";
            this.pbPixConta.Size = new System.Drawing.Size(23, 20);
            this.pbPixConta.TabIndex = 17;
            this.pbPixConta.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(38, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Pix-Conta";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Log:";
            // 
            // txtStatusPixConta
            // 
            this.txtStatusPixConta.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStatusPixConta.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatusPixConta.Location = new System.Drawing.Point(11, 27);
            this.txtStatusPixConta.Multiline = true;
            this.txtStatusPixConta.Name = "txtStatusPixConta";
            this.txtStatusPixConta.ReadOnly = true;
            this.txtStatusPixConta.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatusPixConta.Size = new System.Drawing.Size(607, 336);
            this.txtStatusPixConta.TabIndex = 10;
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 440);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.st);
            this.Controls.Add(this.tabServicos);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(748, 394);
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PDVSeven Integrador";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipal2_FormClosing);
            this.Load += new System.EventHandler(this.frmPrincipal2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeliveryOnline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIFood)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImpressao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPagamentoIntegrado)).EndInit();
            this.tabPOS.ResumeLayout(false);
            this.tabPOS.PerformLayout();
            this.tabServicos.ResumeLayout(false);
            this.tabServidor.ResumeLayout(false);
            this.tabServidor.PerformLayout();
            this.tabImpressao.ResumeLayout(false);
            this.tabImpressao.PerformLayout();
            this.tabIFood.ResumeLayout(false);
            this.tabIFood.PerformLayout();
            this.tabDeliveryOnline.ResumeLayout(false);
            this.tabDeliveryOnline.PerformLayout();
            this.mnu.ResumeLayout(false);
            this.st.ResumeLayout(false);
            this.st.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabPixConta.ResumeLayout(false);
            this.tabPixConta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPixConta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbPagamentoIntegrado;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPOS;
        private System.Windows.Forms.TabControl tabServicos;
        private System.Windows.Forms.NotifyIcon ntf;
        private System.Windows.Forms.PictureBox pbImpressao;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabImpressao;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtStatusPOS;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtStatusImpressao;
        private System.Windows.Forms.PictureBox pbIFood;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabIFood;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtIFood;
        private System.Windows.Forms.PictureBox pbDeliveryOnline;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tabDeliveryOnline;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtStatusDeliveryOnline;
        private System.Windows.Forms.ContextMenuStrip mnu;
        private System.Windows.Forms.ToolStripMenuItem mnuConfig;
        private System.Windows.Forms.ToolStripMenuItem mnuSair;
        private System.Windows.Forms.Timer tmr;
        private System.Windows.Forms.StatusStrip st;
        private System.Windows.Forms.ToolStripStatusLabel stCount;
        private System.Windows.Forms.ToolStripStatusLabel stStatus;
        private System.Windows.Forms.PictureBox pbServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabServidor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStatusServer;
        private System.Windows.Forms.ListView lvStatus;
        private System.Windows.Forms.ImageList imgs;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pbPixConta;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPixConta;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtStatusPixConta;
    }
}