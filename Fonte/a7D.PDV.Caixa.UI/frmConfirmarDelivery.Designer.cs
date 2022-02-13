namespace a7D.PDV.Caixa.UI
{
    partial class frmConfirmarDelivery
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
            this.components = new System.ComponentModel.Container();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.btnVer = new System.Windows.Forms.RadioButton();
            this.btnRejeirar = new System.Windows.Forms.Button();
            this.lblResumo = new System.Windows.Forms.Label();
            this.txtPedido = new System.Windows.Forms.TextBox();
            this.tmrWait = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirmar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnConfirmar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnConfirmar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmar.ForeColor = System.Drawing.Color.White;
            this.btnConfirmar.Location = new System.Drawing.Point(433, 359);
            this.btnConfirmar.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(240, 63);
            this.btnConfirmar.TabIndex = 2;
            this.btnConfirmar.Text = "ACEITAR";
            this.btnConfirmar.UseVisualStyleBackColor = false;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // btnVer
            // 
            this.btnVer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVer.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnVer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnVer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVer.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVer.ForeColor = System.Drawing.Color.White;
            this.btnVer.Location = new System.Drawing.Point(258, 358);
            this.btnVer.Name = "btnVer";
            this.btnVer.Size = new System.Drawing.Size(168, 65);
            this.btnVer.TabIndex = 41;
            this.btnVer.Text = "VER PEDIDO";
            this.btnVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnVer.UseVisualStyleBackColor = false;
            this.btnVer.CheckedChanged += new System.EventHandler(this.btnVer_CheckedChanged);
            // 
            // btnRejeirar
            // 
            this.btnRejeirar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRejeirar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnRejeirar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRejeirar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRejeirar.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRejeirar.ForeColor = System.Drawing.Color.White;
            this.btnRejeirar.Location = new System.Drawing.Point(12, 358);
            this.btnRejeirar.Name = "btnRejeirar";
            this.btnRejeirar.Size = new System.Drawing.Size(240, 65);
            this.btnRejeirar.TabIndex = 50;
            this.btnRejeirar.Text = "REJEITAR";
            this.btnRejeirar.UseVisualStyleBackColor = false;
            this.btnRejeirar.Click += new System.EventHandler(this.btnRejeirar_Click);
            // 
            // lblResumo
            // 
            this.lblResumo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResumo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResumo.Location = new System.Drawing.Point(12, 74);
            this.lblResumo.Name = "lblResumo";
            this.lblResumo.Size = new System.Drawing.Size(644, 252);
            this.lblResumo.TabIndex = 78;
            this.lblResumo.Text = "Confirmar Pedido iFood #1234\r\nCliente: Fabio Ferreira\r\nEndereço: Rua Cajuru, São " +
    "Paulo - SP\r\nTotal em produtos: R$ 123,45";
            this.lblResumo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPedido
            // 
            this.txtPedido.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPedido.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPedido.Location = new System.Drawing.Point(12, 74);
            this.txtPedido.Multiline = true;
            this.txtPedido.Name = "txtPedido";
            this.txtPedido.ReadOnly = true;
            this.txtPedido.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtPedido.Size = new System.Drawing.Size(661, 278);
            this.txtPedido.TabIndex = 79;
            this.txtPedido.Visible = false;
            // 
            // tmrWait
            // 
            this.tmrWait.Enabled = true;
            this.tmrWait.Interval = 5000;
            this.tmrWait.Tick += new System.EventHandler(this.tmrWait_Tick);
            // 
            // frmConfirmarDelivery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(685, 435);
            this.Controls.Add(this.txtPedido);
            this.Controls.Add(this.lblResumo);
            this.Controls.Add(this.btnRejeirar);
            this.Controls.Add(this.btnVer);
            this.Controls.Add(this.btnConfirmar);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "frmConfirmarDelivery";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Novo pedido que precisa de aprovação";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConfirmarDelivery_FormClosing);
            this.Load += new System.EventHandler(this.frmConfirmarDelivery_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.RadioButton btnVer;
        private System.Windows.Forms.Button btnRejeirar;
        private System.Windows.Forms.Label lblResumo;
        private System.Windows.Forms.TextBox txtPedido;
        private System.Windows.Forms.Timer tmrWait;
    }
}