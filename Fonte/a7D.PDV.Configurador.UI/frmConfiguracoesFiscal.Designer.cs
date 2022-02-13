namespace a7D.PDV.Configurador.UI
{
    partial class frmConfiguracoesFiscal
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
            this.tabPageNFCe = new System.Windows.Forms.TabPage();
            this.tableNFCe = new System.Windows.Forms.TableLayoutPanel();
            this.tabPageSATcfg = new System.Windows.Forms.TabPage();
            this.tableSat = new System.Windows.Forms.TableLayoutPanel();
            this.tabPageSATac = new System.Windows.Forms.TabPage();
            this.btnConfigurar = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtinfCFe_ide_signAC = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageNFCe.SuspendLayout();
            this.tabPageSATcfg.SuspendLayout();
            this.tabPageSATac.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageNFCe);
            this.tabControl1.Controls.Add(this.tabPageSATcfg);
            this.tabControl1.Controls.Add(this.tabPageSATac);
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.tabControl1.Location = new System.Drawing.Point(5, 40);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(684, 449);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageNFCe
            // 
            this.tabPageNFCe.AutoScroll = true;
            this.tabPageNFCe.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageNFCe.Controls.Add(this.tableNFCe);
            this.tabPageNFCe.Location = new System.Drawing.Point(4, 26);
            this.tabPageNFCe.Name = "tabPageNFCe";
            this.tabPageNFCe.Size = new System.Drawing.Size(676, 419);
            this.tabPageNFCe.TabIndex = 4;
            this.tabPageNFCe.Text = "Configurações NFCe";
            // 
            // tableNFCe
            // 
            this.tableNFCe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableNFCe.AutoSize = true;
            this.tableNFCe.ColumnCount = 1;
            this.tableNFCe.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableNFCe.Location = new System.Drawing.Point(6, 6);
            this.tableNFCe.Name = "tableNFCe";
            this.tableNFCe.RowCount = 1;
            this.tableNFCe.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableNFCe.Size = new System.Drawing.Size(664, 407);
            this.tableNFCe.TabIndex = 1;
            // 
            // tabPageSATcfg
            // 
            this.tabPageSATcfg.AutoScroll = true;
            this.tabPageSATcfg.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSATcfg.Controls.Add(this.tableSat);
            this.tabPageSATcfg.Location = new System.Drawing.Point(4, 29);
            this.tabPageSATcfg.Name = "tabPageSATcfg";
            this.tabPageSATcfg.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSATcfg.Size = new System.Drawing.Size(676, 416);
            this.tabPageSATcfg.TabIndex = 3;
            this.tabPageSATcfg.Text = "Configurações do S@T";
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
            this.tableSat.Size = new System.Drawing.Size(664, 407);
            this.tableSat.TabIndex = 0;
            // 
            // tabPageSATac
            // 
            this.tabPageSATac.Controls.Add(this.btnConfigurar);
            this.tabPageSATac.Controls.Add(this.label12);
            this.tabPageSATac.Controls.Add(this.txtinfCFe_ide_signAC);
            this.tabPageSATac.Location = new System.Drawing.Point(4, 26);
            this.tabPageSATac.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageSATac.Name = "tabPageSATac";
            this.tabPageSATac.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageSATac.Size = new System.Drawing.Size(676, 419);
            this.tabPageSATac.TabIndex = 1;
            this.tabPageSATac.Text = "Assinatura AC S@T";
            // 
            // btnConfigurar
            // 
            this.btnConfigurar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfigurar.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnConfigurar.Location = new System.Drawing.Point(529, 365);
            this.btnConfigurar.Margin = new System.Windows.Forms.Padding(2);
            this.btnConfigurar.Name = "btnConfigurar";
            this.btnConfigurar.Size = new System.Drawing.Size(143, 44);
            this.btnConfigurar.TabIndex = 2;
            this.btnConfigurar.Text = "Configurar";
            this.btnConfigurar.UseVisualStyleBackColor = true;
            this.btnConfigurar.Click += new System.EventHandler(this.btnConfigurar_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.label12.Location = new System.Drawing.Point(4, 2);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(100, 20);
            this.label12.TabIndex = 1;
            this.label12.Text = "Assinatura AC";
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
            this.txtinfCFe_ide_signAC.Size = new System.Drawing.Size(668, 332);
            this.txtinfCFe_ide_signAC.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(8, 7);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(193, 25);
            this.label15.TabIndex = 3;
            this.label15.Text = "Configurações Fiscal";
            // 
            // frmConfiguracoesFiscal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmConfiguracoesFiscal";
            this.Text = "Configurações SAT";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmConfiguracoesSAT_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageNFCe.ResumeLayout(false);
            this.tabPageNFCe.PerformLayout();
            this.tabPageSATcfg.ResumeLayout(false);
            this.tabPageSATcfg.PerformLayout();
            this.tabPageSATac.ResumeLayout(false);
            this.tabPageSATac.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSATac;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtinfCFe_ide_signAC;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TabPage tabPageSATcfg;
        private System.Windows.Forms.TableLayoutPanel tableSat;
        private System.Windows.Forms.Button btnConfigurar;
        private System.Windows.Forms.TabPage tabPageNFCe;
        private System.Windows.Forms.TableLayoutPanel tableNFCe;
    }
}