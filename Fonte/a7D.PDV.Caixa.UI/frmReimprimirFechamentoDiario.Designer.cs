namespace a7D.PDV.Caixa.UI
{
    partial class frmReimprimirFechamentoDiario
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
            this.btnReimprimir = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbFechamento = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnReimprimir
            // 
            this.btnReimprimir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnReimprimir.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnReimprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReimprimir.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReimprimir.ForeColor = System.Drawing.Color.White;
            this.btnReimprimir.Location = new System.Drawing.Point(11, 136);
            this.btnReimprimir.Margin = new System.Windows.Forms.Padding(4);
            this.btnReimprimir.Name = "btnReimprimir";
            this.btnReimprimir.Size = new System.Drawing.Size(491, 59);
            this.btnReimprimir.TabIndex = 94;
            this.btnReimprimir.Text = "REIMPRIMIR";
            this.btnReimprimir.UseVisualStyleBackColor = false;
            this.btnReimprimir.Click += new System.EventHandler(this.btnReimprimir_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 69);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 17);
            this.label2.TabIndex = 93;
            this.label2.Text = "Selecione o Fechamento";
            // 
            // cbbFechamento
            // 
            this.cbbFechamento.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.cbbFechamento.DisplayMember = "Nome";
            this.cbbFechamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbFechamento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbFechamento.Font = new System.Drawing.Font("Arial", 14F);
            this.cbbFechamento.FormattingEnabled = true;
            this.cbbFechamento.ItemHeight = 22;
            this.cbbFechamento.Location = new System.Drawing.Point(15, 94);
            this.cbbFechamento.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.cbbFechamento.Name = "cbbFechamento";
            this.cbbFechamento.Size = new System.Drawing.Size(487, 30);
            this.cbbFechamento.TabIndex = 92;
            this.cbbFechamento.ValueMember = "IDTipoDesconto";
            // 
            // frmReimprimirFechamentoDiario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(516, 212);
            this.Controls.Add(this.btnReimprimir);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbbFechamento);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(516, 212);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(516, 212);
            this.Name = "frmReimprimirFechamentoDiario";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "REIMPRIMIR FECHAMENTO";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmReimprimirFechamentoDiario_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnReimprimir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbFechamento;
    }
}