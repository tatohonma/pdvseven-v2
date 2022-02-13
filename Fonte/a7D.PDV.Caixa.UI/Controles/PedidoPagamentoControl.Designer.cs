namespace a7D.PDV.Caixa.UI.Controles
{
    partial class PedidoPagamentoControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ckbAplicarServico = new System.Windows.Forms.CheckBox();
            this.txtAcrescimoReais = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAcrescimoPorcentagem = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckbAplicaDesconto = new System.Windows.Forms.CheckBox();
            this.txtDescontoReais = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDescontoPorcentagem = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblValorPago = new System.Windows.Forms.Label();
            this.gbxValorPendente = new System.Windows.Forms.GroupBox();
            this.lblValorPendente = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.gbxValorPendente.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblSubTotal);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 10F);
            this.groupBox1.Location = new System.Drawing.Point(3, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(210, 66);
            this.groupBox1.TabIndex = 104;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Produtos";
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSubTotal.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblSubTotal.Location = new System.Drawing.Point(3, 25);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(204, 39);
            this.lblSubTotal.TabIndex = 1;
            this.lblSubTotal.Text = "0,00";
            this.lblSubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.ckbAplicarServico);
            this.groupBox3.Controls.Add(this.txtAcrescimoReais);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtAcrescimoPorcentagem);
            this.groupBox3.Font = new System.Drawing.Font("Arial", 10F);
            this.groupBox3.Location = new System.Drawing.Point(3, 71);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(210, 74);
            this.groupBox3.TabIndex = 101;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Taxa de serviço";
            // 
            // ckbAplicarServico
            // 
            this.ckbAplicarServico.AutoSize = true;
            this.ckbAplicarServico.Checked = true;
            this.ckbAplicarServico.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbAplicarServico.Font = new System.Drawing.Font("Arial", 7.8F);
            this.ckbAplicarServico.Location = new System.Drawing.Point(5, 20);
            this.ckbAplicarServico.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ckbAplicarServico.Name = "ckbAplicarServico";
            this.ckbAplicarServico.Size = new System.Drawing.Size(192, 22);
            this.ckbAplicarServico.TabIndex = 0;
            this.ckbAplicarServico.Text = "Aplicar taxa de serviço";
            this.ckbAplicarServico.UseVisualStyleBackColor = true;
            this.ckbAplicarServico.Click += new System.EventHandler(this.CkbAplicarServico_Click);
            // 
            // txtAcrescimoReais
            // 
            this.txtAcrescimoReais.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAcrescimoReais.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtAcrescimoReais.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAcrescimoReais.Enabled = false;
            this.txtAcrescimoReais.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAcrescimoReais.Location = new System.Drawing.Point(139, 43);
            this.txtAcrescimoReais.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAcrescimoReais.Name = "txtAcrescimoReais";
            this.txtAcrescimoReais.Size = new System.Drawing.Size(65, 30);
            this.txtAcrescimoReais.TabIndex = 2;
            this.txtAcrescimoReais.Text = "0,00";
            this.txtAcrescimoReais.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 10F);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label10.Location = new System.Drawing.Point(8, 47);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 23);
            this.label10.TabIndex = 86;
            this.label10.Text = "%";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 10F);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label9.Location = new System.Drawing.Point(104, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 23);
            this.label9.TabIndex = 88;
            this.label9.Text = "R$";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAcrescimoPorcentagem
            // 
            this.txtAcrescimoPorcentagem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtAcrescimoPorcentagem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAcrescimoPorcentagem.Enabled = false;
            this.txtAcrescimoPorcentagem.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAcrescimoPorcentagem.Location = new System.Drawing.Point(37, 43);
            this.txtAcrescimoPorcentagem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAcrescimoPorcentagem.Name = "txtAcrescimoPorcentagem";
            this.txtAcrescimoPorcentagem.Size = new System.Drawing.Size(65, 30);
            this.txtAcrescimoPorcentagem.TabIndex = 1;
            this.txtAcrescimoPorcentagem.Text = "0,00";
            this.txtAcrescimoPorcentagem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.ckbAplicaDesconto);
            this.groupBox2.Controls.Add(this.txtDescontoReais);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtDescontoPorcentagem);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 10F);
            this.groupBox2.Location = new System.Drawing.Point(3, 150);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(210, 78);
            this.groupBox2.TabIndex = 102;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Desconto";
            // 
            // ckbAplicaDesconto
            // 
            this.ckbAplicaDesconto.AutoSize = true;
            this.ckbAplicaDesconto.Font = new System.Drawing.Font("Arial", 7.8F);
            this.ckbAplicaDesconto.Location = new System.Drawing.Point(5, 20);
            this.ckbAplicaDesconto.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ckbAplicaDesconto.Name = "ckbAplicaDesconto";
            this.ckbAplicaDesconto.Size = new System.Drawing.Size(151, 22);
            this.ckbAplicaDesconto.TabIndex = 0;
            this.ckbAplicaDesconto.Text = "Aplicar desconto";
            this.ckbAplicaDesconto.UseVisualStyleBackColor = true;
            this.ckbAplicaDesconto.Click += new System.EventHandler(this.ckbAplicaDesconto_Click);
            // 
            // txtDescontoReais
            // 
            this.txtDescontoReais.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescontoReais.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDescontoReais.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescontoReais.Enabled = false;
            this.txtDescontoReais.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescontoReais.Location = new System.Drawing.Point(139, 43);
            this.txtDescontoReais.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDescontoReais.Name = "txtDescontoReais";
            this.txtDescontoReais.Size = new System.Drawing.Size(65, 30);
            this.txtDescontoReais.TabIndex = 2;
            this.txtDescontoReais.Text = "0,00";
            this.txtDescontoReais.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDescontoReais.TextChanged += new System.EventHandler(this.txtDescontoReais_TextChanged);
            this.txtDescontoReais.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SomenteNumero_KeyPress);
            this.txtDescontoReais.Leave += new System.EventHandler(this.txtDescontoReais_Leave);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 10F);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(8, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 23);
            this.label7.TabIndex = 86;
            this.label7.Text = "%";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 10F);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(104, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 23);
            this.label8.TabIndex = 88;
            this.label8.Text = "R$";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDescontoPorcentagem
            // 
            this.txtDescontoPorcentagem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtDescontoPorcentagem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescontoPorcentagem.Enabled = false;
            this.txtDescontoPorcentagem.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescontoPorcentagem.Location = new System.Drawing.Point(37, 43);
            this.txtDescontoPorcentagem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDescontoPorcentagem.Name = "txtDescontoPorcentagem";
            this.txtDescontoPorcentagem.Size = new System.Drawing.Size(60, 30);
            this.txtDescontoPorcentagem.TabIndex = 1;
            this.txtDescontoPorcentagem.Text = "0,00";
            this.txtDescontoPorcentagem.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDescontoPorcentagem.TextChanged += new System.EventHandler(this.txtDescontoPorcentagem_TextChanged);
            this.txtDescontoPorcentagem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SomenteNumero_KeyPress);
            this.txtDescontoPorcentagem.Leave += new System.EventHandler(this.txtDescontoPercentual_Leave);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.lblTotal);
            this.groupBox4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(3, 231);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox4.Size = new System.Drawing.Size(210, 69);
            this.groupBox4.TabIndex = 105;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Valor Total";
            // 
            // lblTotal
            // 
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotal.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTotal.Location = new System.Drawing.Point(3, 28);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(204, 39);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "0,00";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.lblValorPago);
            this.groupBox5.Font = new System.Drawing.Font("Arial", 10F);
            this.groupBox5.Location = new System.Drawing.Point(3, 305);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox5.Size = new System.Drawing.Size(210, 71);
            this.groupBox5.TabIndex = 106;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Valor Pago";
            // 
            // lblValorPago
            // 
            this.lblValorPago.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValorPago.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorPago.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblValorPago.Location = new System.Drawing.Point(3, 25);
            this.lblValorPago.Name = "lblValorPago";
            this.lblValorPago.Size = new System.Drawing.Size(204, 44);
            this.lblValorPago.TabIndex = 0;
            this.lblValorPago.Text = "0,00";
            this.lblValorPago.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gbxValorPendente
            // 
            this.gbxValorPendente.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxValorPendente.Controls.Add(this.lblValorPendente);
            this.gbxValorPendente.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbxValorPendente.Location = new System.Drawing.Point(3, 382);
            this.gbxValorPendente.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbxValorPendente.Name = "gbxValorPendente";
            this.gbxValorPendente.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbxValorPendente.Size = new System.Drawing.Size(210, 71);
            this.gbxValorPendente.TabIndex = 107;
            this.gbxValorPendente.TabStop = false;
            this.gbxValorPendente.Text = "A Pagar / Troco";
            // 
            // lblValorPendente
            // 
            this.lblValorPendente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblValorPendente.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorPendente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblValorPendente.Location = new System.Drawing.Point(3, 28);
            this.lblValorPendente.Name = "lblValorPendente";
            this.lblValorPendente.Size = new System.Drawing.Size(204, 41);
            this.lblValorPendente.TabIndex = 5;
            this.lblValorPendente.Text = "0,00";
            this.lblValorPendente.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PedidoPagamentoControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.gbxValorPendente);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(215, 455);
            this.Name = "PedidoPagamentoControl";
            this.Size = new System.Drawing.Size(215, 455);
            this.Load += new System.EventHandler(this.PedidoPagamento_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.gbxValorPendente.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSubTotal;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox ckbAplicarServico;
        private System.Windows.Forms.TextBox txtAcrescimoReais;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAcrescimoPorcentagem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ckbAplicaDesconto;
        private System.Windows.Forms.TextBox txtDescontoReais;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDescontoPorcentagem;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblValorPago;
        private System.Windows.Forms.GroupBox gbxValorPendente;
        private System.Windows.Forms.Label lblValorPendente;
    }
}
