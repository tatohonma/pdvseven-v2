namespace a7D.PDV.Configurador.UI
{
    partial class frmConsultarStatusOperacional
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
            this.txtDados = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtDados
            // 
            this.txtDados.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtDados.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDados.Location = new System.Drawing.Point(12, 12);
            this.txtDados.Multiline = true;
            this.txtDados.Name = "txtDados";
            this.txtDados.ReadOnly = true;
            this.txtDados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDados.Size = new System.Drawing.Size(890, 516);
            this.txtDados.TabIndex = 0;
            // 
            // frmConsultarStatusOperacional
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 540);
            this.Controls.Add(this.txtDados);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmConsultarStatusOperacional";
            this.Text = "Status Operacional SAT";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmConsultarStatusOperacional_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDados;
    }
}