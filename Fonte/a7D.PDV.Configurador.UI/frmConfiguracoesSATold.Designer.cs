namespace a7D.PDV.Configurador.UI
{
    partial class frmConfiguracoesSATold
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tableSat = new System.Windows.Forms.TableLayoutPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.txtinfCFe_ide_signAC = new System.Windows.Forms.TextBox();
            this.tabConfiguracoesSAT = new System.Windows.Forms.TabPage();
            this.btnAssociar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConsultarStatus = new System.Windows.Forms.Button();
            this.btnDesbloquear = new System.Windows.Forms.Button();
            this.btnTesteFimAFim = new System.Windows.Forms.Button();
            this.btnExtrairLogs = new System.Windows.Forms.Button();
            this.btnAssociarAssinatura = new System.Windows.Forms.Button();
            this.btnAtivarSAT = new System.Windows.Forms.Button();
            this.btnConsultarSAT = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.cbbMarcaSAT = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnConfigurar = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabConfiguracoesSAT.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabConfiguracoesSAT);
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl1.Location = new System.Drawing.Point(5, 40);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(731, 549);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.AutoScroll = true;
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.tableSat);
            this.tabPage4.Location = new System.Drawing.Point(4, 32);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(723, 513);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Configurações do Aplicativo";
            // 
            // tableSat
            // 
            this.tableSat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableSat.AutoSize = true;
            this.tableSat.ColumnCount = 1;
            this.tableSat.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableSat.Location = new System.Drawing.Point(6, 6);
            this.tableSat.Name = "tableSat";
            this.tableSat.RowCount = 1;
            this.tableSat.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableSat.Size = new System.Drawing.Size(711, 453);
            this.tableSat.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnConfigurar);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.txtinfCFe_ide_signAC);
            this.tabPage2.Location = new System.Drawing.Point(4, 32);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(723, 513);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Assinatura AC";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.label12.Location = new System.Drawing.Point(4, 2);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 25);
            this.label12.TabIndex = 1;
            this.label12.Text = "Assinatura";
            // 
            // txtinfCFe_ide_signAC
            // 
            this.txtinfCFe_ide_signAC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtinfCFe_ide_signAC.Font = new System.Drawing.Font("Consolas", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtinfCFe_ide_signAC.Location = new System.Drawing.Point(4, 29);
            this.txtinfCFe_ide_signAC.Margin = new System.Windows.Forms.Padding(2);
            this.txtinfCFe_ide_signAC.MaxLength = 344;
            this.txtinfCFe_ide_signAC.Multiline = true;
            this.txtinfCFe_ide_signAC.Name = "txtinfCFe_ide_signAC";
            this.txtinfCFe_ide_signAC.Size = new System.Drawing.Size(717, 432);
            this.txtinfCFe_ide_signAC.TabIndex = 0;
            // 
            // tabConfiguracoesSAT
            // 
            this.tabConfiguracoesSAT.Controls.Add(this.btnAssociar);
            this.tabConfiguracoesSAT.Controls.Add(this.groupBox1);
            this.tabConfiguracoesSAT.Controls.Add(this.label13);
            this.tabConfiguracoesSAT.Controls.Add(this.cbbMarcaSAT);
            this.tabConfiguracoesSAT.Location = new System.Drawing.Point(4, 32);
            this.tabConfiguracoesSAT.Margin = new System.Windows.Forms.Padding(2);
            this.tabConfiguracoesSAT.Name = "tabConfiguracoesSAT";
            this.tabConfiguracoesSAT.Padding = new System.Windows.Forms.Padding(2);
            this.tabConfiguracoesSAT.Size = new System.Drawing.Size(723, 465);
            this.tabConfiguracoesSAT.TabIndex = 2;
            this.tabConfiguracoesSAT.Text = "Configurações do SAT";
            // 
            // btnAssociar
            // 
            this.btnAssociar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAssociar.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnAssociar.Location = new System.Drawing.Point(604, 28);
            this.btnAssociar.Margin = new System.Windows.Forms.Padding(2);
            this.btnAssociar.Name = "btnAssociar";
            this.btnAssociar.Size = new System.Drawing.Size(115, 34);
            this.btnAssociar.TabIndex = 8;
            this.btnAssociar.Text = "Associar DLL";
            this.btnAssociar.UseVisualStyleBackColor = true;
            this.btnAssociar.Click += new System.EventHandler(this.btnAssociar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConsultarStatus);
            this.groupBox1.Controls.Add(this.btnDesbloquear);
            this.groupBox1.Controls.Add(this.btnTesteFimAFim);
            this.groupBox1.Controls.Add(this.btnExtrairLogs);
            this.groupBox1.Controls.Add(this.btnAssociarAssinatura);
            this.groupBox1.Controls.Add(this.btnAtivarSAT);
            this.groupBox1.Controls.Add(this.btnConsultarSAT);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.groupBox1.Location = new System.Drawing.Point(9, 85);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(710, 172);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SAT";
            // 
            // btnConsultarStatus
            // 
            this.btnConsultarStatus.Enabled = false;
            this.btnConsultarStatus.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnConsultarStatus.Location = new System.Drawing.Point(160, 29);
            this.btnConsultarStatus.Margin = new System.Windows.Forms.Padding(2);
            this.btnConsultarStatus.Name = "btnConsultarStatus";
            this.btnConsultarStatus.Size = new System.Drawing.Size(182, 40);
            this.btnConsultarStatus.TabIndex = 13;
            this.btnConsultarStatus.Text = "Status Operacional";
            this.btnConsultarStatus.UseVisualStyleBackColor = true;
            this.btnConsultarStatus.Click += new System.EventHandler(this.btnConsultarStatus_Click);
            // 
            // btnDesbloquear
            // 
            this.btnDesbloquear.Enabled = false;
            this.btnDesbloquear.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnDesbloquear.Location = new System.Drawing.Point(160, 73);
            this.btnDesbloquear.Margin = new System.Windows.Forms.Padding(2);
            this.btnDesbloquear.Name = "btnDesbloquear";
            this.btnDesbloquear.Size = new System.Drawing.Size(182, 40);
            this.btnDesbloquear.TabIndex = 12;
            this.btnDesbloquear.Text = "Desbloquear SAT";
            this.btnDesbloquear.UseVisualStyleBackColor = true;
            this.btnDesbloquear.Click += new System.EventHandler(this.btnDesbloquear_Click);
            // 
            // btnTesteFimAFim
            // 
            this.btnTesteFimAFim.Enabled = false;
            this.btnTesteFimAFim.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnTesteFimAFim.Location = new System.Drawing.Point(346, 29);
            this.btnTesteFimAFim.Margin = new System.Windows.Forms.Padding(2);
            this.btnTesteFimAFim.Name = "btnTesteFimAFim";
            this.btnTesteFimAFim.Size = new System.Drawing.Size(182, 40);
            this.btnTesteFimAFim.TabIndex = 11;
            this.btnTesteFimAFim.Text = "Teste Fim-a-Fim";
            this.btnTesteFimAFim.UseVisualStyleBackColor = true;
            this.btnTesteFimAFim.Click += new System.EventHandler(this.btnTesteFimAFim_Click);
            // 
            // btnExtrairLogs
            // 
            this.btnExtrairLogs.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnExtrairLogs.Location = new System.Drawing.Point(4, 117);
            this.btnExtrairLogs.Margin = new System.Windows.Forms.Padding(2);
            this.btnExtrairLogs.Name = "btnExtrairLogs";
            this.btnExtrairLogs.Size = new System.Drawing.Size(152, 40);
            this.btnExtrairLogs.TabIndex = 10;
            this.btnExtrairLogs.Text = "Extrair Logs";
            this.btnExtrairLogs.UseVisualStyleBackColor = true;
            this.btnExtrairLogs.Click += new System.EventHandler(this.btnExtrairLogs_Click);
            // 
            // btnAssociarAssinatura
            // 
            this.btnAssociarAssinatura.Enabled = false;
            this.btnAssociarAssinatura.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnAssociarAssinatura.Location = new System.Drawing.Point(160, 117);
            this.btnAssociarAssinatura.Margin = new System.Windows.Forms.Padding(2);
            this.btnAssociarAssinatura.Name = "btnAssociarAssinatura";
            this.btnAssociarAssinatura.Size = new System.Drawing.Size(217, 40);
            this.btnAssociarAssinatura.TabIndex = 9;
            this.btnAssociarAssinatura.Text = "Associar Assinatura";
            this.btnAssociarAssinatura.UseVisualStyleBackColor = true;
            this.btnAssociarAssinatura.Click += new System.EventHandler(this.btnAssociarAssinatura_Click);
            // 
            // btnAtivarSAT
            // 
            this.btnAtivarSAT.Enabled = false;
            this.btnAtivarSAT.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnAtivarSAT.Location = new System.Drawing.Point(4, 73);
            this.btnAtivarSAT.Margin = new System.Windows.Forms.Padding(2);
            this.btnAtivarSAT.Name = "btnAtivarSAT";
            this.btnAtivarSAT.Size = new System.Drawing.Size(152, 40);
            this.btnAtivarSAT.TabIndex = 8;
            this.btnAtivarSAT.Text = "Ativar SAT";
            this.btnAtivarSAT.UseVisualStyleBackColor = true;
            this.btnAtivarSAT.Click += new System.EventHandler(this.btnAtivarSAT_Click);
            // 
            // btnConsultarSAT
            // 
            this.btnConsultarSAT.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnConsultarSAT.Location = new System.Drawing.Point(4, 29);
            this.btnConsultarSAT.Margin = new System.Windows.Forms.Padding(2);
            this.btnConsultarSAT.Name = "btnConsultarSAT";
            this.btnConsultarSAT.Size = new System.Drawing.Size(152, 40);
            this.btnConsultarSAT.TabIndex = 7;
            this.btnConsultarSAT.Text = "Consultar SAT";
            this.btnConsultarSAT.UseVisualStyleBackColor = true;
            this.btnConsultarSAT.Click += new System.EventHandler(this.btnConsultarSAT_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.label13.Location = new System.Drawing.Point(4, 2);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 25);
            this.label13.TabIndex = 1;
            this.label13.Text = "Marca";
            // 
            // cbbMarcaSAT
            // 
            this.cbbMarcaSAT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbMarcaSAT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbMarcaSAT.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cbbMarcaSAT.FormattingEnabled = true;
            this.cbbMarcaSAT.Location = new System.Drawing.Point(4, 29);
            this.cbbMarcaSAT.Margin = new System.Windows.Forms.Padding(2);
            this.cbbMarcaSAT.Name = "cbbMarcaSAT";
            this.cbbMarcaSAT.Size = new System.Drawing.Size(596, 33);
            this.cbbMarcaSAT.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(8, 7);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(237, 32);
            this.label15.TabIndex = 3;
            this.label15.Text = "Configurações S@T";
            // 
            // btnConfigurar
            // 
            this.btnConfigurar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigurar.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnConfigurar.Location = new System.Drawing.Point(576, 465);
            this.btnConfigurar.Margin = new System.Windows.Forms.Padding(2);
            this.btnConfigurar.Name = "btnConfigurar";
            this.btnConfigurar.Size = new System.Drawing.Size(143, 44);
            this.btnConfigurar.TabIndex = 2;
            this.btnConfigurar.Text = "Configurar";
            this.btnConfigurar.UseVisualStyleBackColor = true;
            this.btnConfigurar.Click += new System.EventHandler(this.btnConfigurar_Click);
            // 
            // frmConfiguracoesSAT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 600);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmConfiguracoesSAT";
            this.Text = "Configurações SAT";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmConfiguracoesSAT_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabConfiguracoesSAT.ResumeLayout(false);
            this.tabConfiguracoesSAT.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtinfCFe_ide_signAC;
        private System.Windows.Forms.TabPage tabConfiguracoesSAT;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbbMarcaSAT;
        private System.Windows.Forms.Button btnAssociar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnConsultarSAT;
        private System.Windows.Forms.Button btnAtivarSAT;
        private System.Windows.Forms.Button btnAssociarAssinatura;
        private System.Windows.Forms.Button btnExtrairLogs;
        private System.Windows.Forms.Button btnTesteFimAFim;
        private System.Windows.Forms.Button btnDesbloquear;
        private System.Windows.Forms.Button btnConsultarStatus;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TableLayoutPanel tableSat;
        private System.Windows.Forms.Button btnConfigurar;
    }
}