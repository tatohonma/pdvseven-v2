namespace a7D.PDV.Balanca.UI
{
    partial class frmEnviarPeso
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
            this.btnEnviar = new System.Windows.Forms.Button();
            this.txtPeso = new System.Windows.Forms.TextBox();
            this.cbbSerial = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnEnviar
            // 
            this.btnEnviar.Location = new System.Drawing.Point(12, 70);
            this.btnEnviar.Name = "btnEnviar";
            this.btnEnviar.Size = new System.Drawing.Size(258, 23);
            this.btnEnviar.TabIndex = 0;
            this.btnEnviar.Text = "Enviar Peso";
            this.btnEnviar.UseVisualStyleBackColor = true;
            this.btnEnviar.Click += new System.EventHandler(this.btnEnviar_Click);
            // 
            // txtPeso
            // 
            this.txtPeso.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPeso.Location = new System.Drawing.Point(12, 12);
            this.txtPeso.Name = "txtPeso";
            this.txtPeso.Size = new System.Drawing.Size(258, 22);
            this.txtPeso.TabIndex = 1;
            // 
            // cbbSerial
            // 
            this.cbbSerial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSerial.FormattingEnabled = true;
            this.cbbSerial.Location = new System.Drawing.Point(12, 40);
            this.cbbSerial.Name = "cbbSerial";
            this.cbbSerial.Size = new System.Drawing.Size(258, 24);
            this.cbbSerial.TabIndex = 2;
            // 
            // frmEnviarPeso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 103);
            this.Controls.Add(this.cbbSerial);
            this.Controls.Add(this.txtPeso);
            this.Controls.Add(this.btnEnviar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEnviarPeso";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmEnviarPeso_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEnviar;
        private System.Windows.Forms.TextBox txtPeso;
        private System.Windows.Forms.ComboBox cbbSerial;
    }
}