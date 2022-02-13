namespace a7D.PDV.LocalizadorBalanca.UI
{
    partial class frmLocalizarBalanca
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
            this.btnLocalizar = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbbProtocolo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnLocalizar
            // 
            this.btnLocalizar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLocalizar.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold);
            this.btnLocalizar.Location = new System.Drawing.Point(9, 46);
            this.btnLocalizar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLocalizar.Name = "btnLocalizar";
            this.btnLocalizar.Size = new System.Drawing.Size(260, 89);
            this.btnLocalizar.TabIndex = 0;
            this.btnLocalizar.Text = "Localizar Balança";
            this.btnLocalizar.UseVisualStyleBackColor = true;
            this.btnLocalizar.Click += new System.EventHandler(this.btnLocalizar_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Font = new System.Drawing.Font("Consolas", 19.8F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(9, 137);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(260, 119);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbbProtocolo
            // 
            this.cbbProtocolo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbbProtocolo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbProtocolo.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.cbbProtocolo.FormattingEnabled = true;
            this.cbbProtocolo.Items.AddRange(new object[] {
            "Toledo",
            "Filizola"});
            this.cbbProtocolo.Location = new System.Drawing.Point(9, 10);
            this.cbbProtocolo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbbProtocolo.Name = "cbbProtocolo";
            this.cbbProtocolo.Size = new System.Drawing.Size(261, 27);
            this.cbbProtocolo.TabIndex = 2;
            // 
            // frmLocalizarBalanca
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 263);
            this.Controls.Add(this.cbbProtocolo);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnLocalizar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLocalizarBalanca";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmLocalizarBalanca_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLocalizar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cbbProtocolo;
    }
}

