namespace a7D.PDV.Terminal.UI
{
    partial class frmIdentificacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIdentificacao));
            this.txtIdentificacao = new System.Windows.Forms.TextBox();
            this.rdbComandas = new System.Windows.Forms.RadioButton();
            this.rdbMesas = new System.Windows.Forms.RadioButton();
            this.btnApagar = new System.Windows.Forms.Button();
            this.teclado1 = new a7D.PDV.Componentes.Controles.Teclado();
            this.lblTipo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtIdentificacao
            // 
            this.txtIdentificacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtIdentificacao.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIdentificacao.Font = new System.Drawing.Font("Arial", 24F);
            this.txtIdentificacao.Location = new System.Drawing.Point(11, 185);
            this.txtIdentificacao.Name = "txtIdentificacao";
            this.txtIdentificacao.Size = new System.Drawing.Size(263, 37);
            this.txtIdentificacao.TabIndex = 1;
            this.txtIdentificacao.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIdentificacao_KeyDown);
            this.txtIdentificacao.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ApenasNumero_KeyPress);
            this.txtIdentificacao.Leave += new System.EventHandler(this.txtIdentificacao_Leave);
            // 
            // rdbComandas
            // 
            this.rdbComandas.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdbComandas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rdbComandas.Checked = true;
            this.rdbComandas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdbComandas.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbComandas.ForeColor = System.Drawing.Color.White;
            this.rdbComandas.Location = new System.Drawing.Point(11, 73);
            this.rdbComandas.Name = "rdbComandas";
            this.rdbComandas.Size = new System.Drawing.Size(323, 50);
            this.rdbComandas.TabIndex = 80;
            this.rdbComandas.TabStop = true;
            this.rdbComandas.Text = "COMANDAS";
            this.rdbComandas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbComandas.UseVisualStyleBackColor = false;
            this.rdbComandas.CheckedChanged += new System.EventHandler(this.rdbComandas_CheckedChanged);
            // 
            // rdbMesas
            // 
            this.rdbMesas.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdbMesas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.rdbMesas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdbMesas.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbMesas.ForeColor = System.Drawing.Color.White;
            this.rdbMesas.Location = new System.Drawing.Point(11, 129);
            this.rdbMesas.Name = "rdbMesas";
            this.rdbMesas.Size = new System.Drawing.Size(323, 50);
            this.rdbMesas.TabIndex = 79;
            this.rdbMesas.Text = "MESAS";
            this.rdbMesas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rdbMesas.UseVisualStyleBackColor = false;
            this.rdbMesas.CheckedChanged += new System.EventHandler(this.rdbMesas_CheckedChanged);
            // 
            // btnApagar
            // 
            this.btnApagar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnApagar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnApagar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApagar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApagar.ForeColor = System.Drawing.Color.White;
            this.btnApagar.Location = new System.Drawing.Point(280, 185);
            this.btnApagar.Name = "btnApagar";
            this.btnApagar.Size = new System.Drawing.Size(57, 37);
            this.btnApagar.TabIndex = 83;
            this.btnApagar.Text = "<";
            this.btnApagar.UseVisualStyleBackColor = false;
            this.btnApagar.Click += new System.EventHandler(this.btnApagar_Click);
            // 
            // teclado1
            // 
            this.teclado1.BackColor = System.Drawing.Color.Transparent;
            this.teclado1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.teclado1.Location = new System.Drawing.Point(11, 230);
            this.teclado1.Margin = new System.Windows.Forms.Padding(4);
            this.teclado1.Name = "teclado1";
            this.teclado1.Size = new System.Drawing.Size(326, 317);
            this.teclado1.TabIndex = 82;
            // 
            // lblTipo
            // 
            this.lblTipo.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipo.Location = new System.Drawing.Point(12, 73);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(322, 106);
            this.lblTipo.TabIndex = 84;
            this.lblTipo.Text = "Mesa / Comanda";
            this.lblTipo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTipo.Visible = false;
            // 
            // frmIdentificacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(350, 560);
            this.Controls.Add(this.btnApagar);
            this.Controls.Add(this.teclado1);
            this.Controls.Add(this.rdbComandas);
            this.Controls.Add(this.rdbMesas);
            this.Controls.Add(this.txtIdentificacao);
            this.Controls.Add(this.lblTipo);
            this.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(350, 560);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(350, 560);
            this.Name = "frmIdentificacao";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PEDIDO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmIdentificacao_FormClosing);
            this.Load += new System.EventHandler(this.frmIdentificacao_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIdentificacao;
        private System.Windows.Forms.RadioButton rdbComandas;
        private System.Windows.Forms.RadioButton rdbMesas;
        private a7D.PDV.Componentes.Controles.Teclado teclado1;
        private System.Windows.Forms.Button btnApagar;
        private System.Windows.Forms.Label lblTipo;
    }
}