namespace a7D.PDV.VisualizadorLicencas.UI
{
    partial class frmLicencas
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
            this.gdvLicencas = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gdvLicencas)).BeginInit();
            this.SuspendLayout();
            // 
            // gdvLicencas
            // 
            this.gdvLicencas.AllowUserToAddRows = false;
            this.gdvLicencas.AllowUserToDeleteRows = false;
            this.gdvLicencas.AllowUserToResizeColumns = false;
            this.gdvLicencas.AllowUserToResizeRows = false;
            this.gdvLicencas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gdvLicencas.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.gdvLicencas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.gdvLicencas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdvLicencas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdvLicencas.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gdvLicencas.Location = new System.Drawing.Point(0, 0);
            this.gdvLicencas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gdvLicencas.MultiSelect = false;
            this.gdvLicencas.Name = "gdvLicencas";
            this.gdvLicencas.RowHeadersVisible = false;
            this.gdvLicencas.RowTemplate.Height = 24;
            this.gdvLicencas.Size = new System.Drawing.Size(875, 475);
            this.gdvLicencas.TabIndex = 0;
            // 
            // frmLicencas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 475);
            this.Controls.Add(this.gdvLicencas);
            this.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimizeBox = false;
            this.Name = "frmLicencas";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmLicencas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gdvLicencas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gdvLicencas;
    }
}