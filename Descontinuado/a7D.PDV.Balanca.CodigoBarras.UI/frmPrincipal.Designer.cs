namespace a7D.PDV.Balanca.CodigoBarras.UI
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
            this.cbbTipo = new System.Windows.Forms.ComboBox();
            this.pbCodigoBarras = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbQuantidade = new System.Windows.Forms.RadioButton();
            this.rbPeso = new System.Windows.Forms.RadioButton();
            this.txtCodigoBarras = new System.Windows.Forms.TextBox();
            this.btnLer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbCodigoBarras)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbbTipo
            // 
            this.cbbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTipo.Font = new System.Drawing.Font("Segoe UI", 13.8F);
            this.cbbTipo.FormattingEnabled = true;
            this.cbbTipo.Items.AddRange(new object[] {
            "ET901",
            "ET902",
            "ET903",
            "ET904",
            "ET905",
            "ET906",
            "ET907",
            "ET908",
            "ET909",
            "ET910"});
            this.cbbTipo.Location = new System.Drawing.Point(12, 12);
            this.cbbTipo.Name = "cbbTipo";
            this.cbbTipo.Size = new System.Drawing.Size(489, 39);
            this.cbbTipo.TabIndex = 0;
            this.cbbTipo.SelectedIndexChanged += new System.EventHandler(this.cbbTipo_SelectedIndexChanged);
            // 
            // pbCodigoBarras
            // 
            this.pbCodigoBarras.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbCodigoBarras.Location = new System.Drawing.Point(11, 243);
            this.pbCodigoBarras.Name = "pbCodigoBarras";
            this.pbCodigoBarras.Size = new System.Drawing.Size(490, 402);
            this.pbCodigoBarras.TabIndex = 1;
            this.pbCodigoBarras.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbQuantidade);
            this.groupBox1.Controls.Add(this.rbPeso);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 13.8F);
            this.groupBox1.Location = new System.Drawing.Point(12, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(489, 75);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tipo";
            // 
            // rbQuantidade
            // 
            this.rbQuantidade.AutoSize = true;
            this.rbQuantidade.Font = new System.Drawing.Font("Segoe UI", 13.7F);
            this.rbQuantidade.Location = new System.Drawing.Point(233, 34);
            this.rbQuantidade.Name = "rbQuantidade";
            this.rbQuantidade.Size = new System.Drawing.Size(156, 35);
            this.rbQuantidade.TabIndex = 1;
            this.rbQuantidade.Text = "Quantidade";
            this.rbQuantidade.UseVisualStyleBackColor = true;
            this.rbQuantidade.CheckedChanged += new System.EventHandler(this.rbQuantidade_CheckedChanged);
            // 
            // rbPeso
            // 
            this.rbPeso.AutoSize = true;
            this.rbPeso.Checked = true;
            this.rbPeso.Font = new System.Drawing.Font("Segoe UI", 13.7F);
            this.rbPeso.Location = new System.Drawing.Point(116, 34);
            this.rbPeso.Name = "rbPeso";
            this.rbPeso.Size = new System.Drawing.Size(82, 35);
            this.rbPeso.TabIndex = 0;
            this.rbPeso.TabStop = true;
            this.rbPeso.Text = "Peso";
            this.rbPeso.UseVisualStyleBackColor = true;
            this.rbPeso.CheckedChanged += new System.EventHandler(this.rbPeso_CheckedChanged);
            // 
            // txtCodigoBarras
            // 
            this.txtCodigoBarras.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodigoBarras.Location = new System.Drawing.Point(12, 138);
            this.txtCodigoBarras.Name = "txtCodigoBarras";
            this.txtCodigoBarras.Size = new System.Drawing.Size(489, 38);
            this.txtCodigoBarras.TabIndex = 2;
            this.txtCodigoBarras.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnLer
            // 
            this.btnLer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLer.Font = new System.Drawing.Font("Segoe UI", 13.8F);
            this.btnLer.Location = new System.Drawing.Point(184, 182);
            this.btnLer.Name = "btnLer";
            this.btnLer.Size = new System.Drawing.Size(143, 55);
            this.btnLer.TabIndex = 3;
            this.btnLer.Text = "Ler";
            this.btnLer.UseVisualStyleBackColor = true;
            // 
            // frmPrincipal
            // 
            this.AcceptButton = this.btnLer;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 656);
            this.Controls.Add(this.btnLer);
            this.Controls.Add(this.txtCodigoBarras);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pbCodigoBarras);
            this.Controls.Add(this.cbbTipo);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPrincipal";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Codigo Barras";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbCodigoBarras)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbbTipo;
        private System.Windows.Forms.PictureBox pbCodigoBarras;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbQuantidade;
        private System.Windows.Forms.RadioButton rbPeso;
        private System.Windows.Forms.TextBox txtCodigoBarras;
        private System.Windows.Forms.Button btnLer;
    }
}

