namespace a7D.PDV.Caixa.UI
{
    partial class frmTipoDesconto
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbbTipoDesconto = new System.Windows.Forms.ComboBox();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 17);
            this.label1.TabIndex = 47;
            this.label1.Text = "Tipo";
            // 
            // cbbTipoDesconto
            // 
            this.cbbTipoDesconto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbbTipoDesconto.DisplayMember = "Nome";
            this.cbbTipoDesconto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTipoDesconto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbTipoDesconto.Font = new System.Drawing.Font("Arial", 14F);
            this.cbbTipoDesconto.FormattingEnabled = true;
            this.cbbTipoDesconto.ItemHeight = 22;
            this.cbbTipoDesconto.Location = new System.Drawing.Point(16, 93);
            this.cbbTipoDesconto.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.cbbTipoDesconto.Name = "cbbTipoDesconto";
            this.cbbTipoDesconto.Size = new System.Drawing.Size(494, 30);
            this.cbbTipoDesconto.TabIndex = 46;
            this.cbbTipoDesconto.ValueMember = "IDTipoDesconto";
            // 
            // btnSalvar
            // 
            this.btnSalvar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnSalvar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSalvar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalvar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(16, 135);
            this.btnSalvar.Margin = new System.Windows.Forms.Padding(4);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(494, 59);
            this.btnSalvar.TabIndex = 48;
            this.btnSalvar.Text = "CONFIRMAR";
            this.btnSalvar.UseVisualStyleBackColor = false;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // frmTipoDesconto
            // 
            this.AcceptButton = this.btnSalvar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(520, 220);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbbTipoDesconto);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(520, 220);
            this.MinimumSize = new System.Drawing.Size(520, 220);
            this.Name = "frmTipoDesconto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TIPO DE DESCONTO";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmTipoDesconto_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbTipoDesconto;
        private System.Windows.Forms.Button btnSalvar;
    }
}