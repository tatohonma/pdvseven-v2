namespace a7D.PDV.Caixa.UI
{
    partial class frmPedidoPagamentoTouch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPedidoPagamentoTouch));
            this.lblCodigoPedido = new System.Windows.Forms.Label();
            this.txtObservacoes = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblIdentificacao = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFecharVenda = new System.Windows.Forms.Button();
            this.ppProdutos = new a7D.PDV.Caixa.UI.Controles.PedidoPagamentoProdutos();
            this.ppFormas = new a7D.PDV.Caixa.UI.Controles.PedidoPagamentoFormas();
            this.ppCliente = new a7D.PDV.Caixa.UI.Controles.PedidoPagamentoCliente();
            this.ppControl = new a7D.PDV.Caixa.UI.Controles.PedidoPagamentoControl();
            this.chkImprimir = new System.Windows.Forms.CheckBox();
            this.chkExpedicao = new System.Windows.Forms.CheckBox();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCodigoPedido
            // 
            this.lblCodigoPedido.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblCodigoPedido, 2);
            this.lblCodigoPedido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCodigoPedido.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCodigoPedido.ForeColor = System.Drawing.Color.Gray;
            this.lblCodigoPedido.Location = new System.Drawing.Point(582, 50);
            this.lblCodigoPedido.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCodigoPedido.Name = "lblCodigoPedido";
            this.lblCodigoPedido.Size = new System.Drawing.Size(330, 50);
            this.lblCodigoPedido.TabIndex = 50;
            this.lblCodigoPedido.Text = "000000";
            this.lblCodigoPedido.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtObservacoes
            // 
            this.txtObservacoes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtObservacoes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.txtObservacoes, 4);
            this.txtObservacoes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtObservacoes.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtObservacoes.Location = new System.Drawing.Point(2, 508);
            this.txtObservacoes.Margin = new System.Windows.Forms.Padding(2);
            this.txtObservacoes.Multiline = true;
            this.txtObservacoes.Name = "txtObservacoes";
            this.txtObservacoes.Size = new System.Drawing.Size(346, 42);
            this.txtObservacoes.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label6, 4);
            this.label6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label6.Font = new System.Drawing.Font("Arial", 10F);
            this.label6.Location = new System.Drawing.Point(2, 490);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(346, 16);
            this.label6.TabIndex = 82;
            this.label6.Text = "Observações";
            // 
            // lblIdentificacao
            // 
            this.lblIdentificacao.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblIdentificacao, 2);
            this.lblIdentificacao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblIdentificacao.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdentificacao.ForeColor = System.Drawing.Color.Gray;
            this.lblIdentificacao.Location = new System.Drawing.Point(582, 0);
            this.lblIdentificacao.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblIdentificacao.Name = "lblIdentificacao";
            this.lblIdentificacao.Size = new System.Drawing.Size(330, 50);
            this.lblIdentificacao.TabIndex = 87;
            this.lblIdentificacao.Text = "MESA";
            this.lblIdentificacao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 127F));
            this.tableLayoutPanel1.Controls.Add(this.btnFecharVenda, 1, 13);
            this.tableLayoutPanel1.Controls.Add(this.txtObservacoes, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.lblIdentificacao, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCodigoPedido, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.ppProdutos, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.ppFormas, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.ppCliente, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ppControl, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkImprimir, 5, 13);
            this.tableLayoutPanel1.Controls.Add(this.chkExpedicao, 6, 13);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 70);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 14;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(914, 552);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnFecharVenda
            // 
            this.btnFecharVenda.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(198)))), ((int)(((byte)(63)))));
            this.btnFecharVenda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFecharVenda.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFecharVenda.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFecharVenda.ForeColor = System.Drawing.Color.White;
            this.btnFecharVenda.Location = new System.Drawing.Point(352, 508);
            this.btnFecharVenda.Margin = new System.Windows.Forms.Padding(2);
            this.btnFecharVenda.Name = "btnFecharVenda";
            this.btnFecharVenda.Size = new System.Drawing.Size(226, 42);
            this.btnFecharVenda.TabIndex = 91;
            this.btnFecharVenda.Text = "&FECHAR";
            this.btnFecharVenda.UseVisualStyleBackColor = false;
            this.btnFecharVenda.Click += new System.EventHandler(this.btnFecharVenda_Click);
            // 
            // ppProdutos
            // 
            this.ppProdutos.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.SetColumnSpan(this.ppProdutos, 2);
            this.ppProdutos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppProdutos.Location = new System.Drawing.Point(582, 102);
            this.ppProdutos.Margin = new System.Windows.Forms.Padding(2);
            this.ppProdutos.Name = "ppProdutos";
            this.tableLayoutPanel1.SetRowSpan(this.ppProdutos, 10);
            this.ppProdutos.Size = new System.Drawing.Size(330, 384);
            this.ppProdutos.TabIndex = 92;
            // 
            // ppFormas
            // 
            this.ppFormas.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.ppFormas, 4);
            this.ppFormas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppFormas.Location = new System.Drawing.Point(2, 171);
            this.ppFormas.Margin = new System.Windows.Forms.Padding(2);
            this.ppFormas.Name = "ppFormas";
            this.tableLayoutPanel1.SetRowSpan(this.ppFormas, 7);
            this.ppFormas.Size = new System.Drawing.Size(346, 315);
            this.ppFormas.SplitterDistance = 113;
            this.ppFormas.TabIndex = 93;
            this.ppFormas.PagamentosAlterados += new System.EventHandler(this.ppForma_PagamentosAlterados);
            // 
            // ppCliente
            // 
            this.ppCliente.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.ppCliente, 4);
            this.ppCliente.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppCliente.Location = new System.Drawing.Point(2, 2);
            this.ppCliente.Margin = new System.Windows.Forms.Padding(2);
            this.ppCliente.Name = "ppCliente";
            this.tableLayoutPanel1.SetRowSpan(this.ppCliente, 5);
            this.ppCliente.Size = new System.Drawing.Size(346, 165);
            this.ppCliente.TabIndex = 94;
            // 
            // ppControl
            // 
            this.ppControl.BackColor = System.Drawing.Color.Transparent;
            this.ppControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppControl.Location = new System.Drawing.Point(352, 102);
            this.ppControl.Margin = new System.Windows.Forms.Padding(2);
            this.ppControl.MinimumSize = new System.Drawing.Size(180, 370);
            this.ppControl.Name = "ppControl";
            this.tableLayoutPanel1.SetRowSpan(this.ppControl, 10);
            this.ppControl.Size = new System.Drawing.Size(226, 384);
            this.ppControl.TabIndex = 95;
            this.ppControl.ParametrosAlterados += new System.EventHandler(this.ppControl_ParametrosAlterados);
            // 
            // chkImprimir
            // 
            this.chkImprimir.AutoSize = true;
            this.chkImprimir.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkImprimir.Location = new System.Drawing.Point(660, 509);
            this.chkImprimir.Name = "chkImprimir";
            this.chkImprimir.Size = new System.Drawing.Size(124, 40);
            this.chkImprimir.TabIndex = 99;
            this.chkImprimir.Text = "Imprimir Cupom";
            this.chkImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkImprimir.UseVisualStyleBackColor = true;
            // 
            // chkExpedicao
            // 
            this.chkExpedicao.AutoSize = true;
            this.chkExpedicao.Dock = System.Windows.Forms.DockStyle.Right;
            this.chkExpedicao.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.chkExpedicao.Location = new System.Drawing.Point(820, 508);
            this.chkExpedicao.Margin = new System.Windows.Forms.Padding(2);
            this.chkExpedicao.Name = "chkExpedicao";
            this.chkExpedicao.Size = new System.Drawing.Size(92, 42);
            this.chkExpedicao.TabIndex = 100;
            this.chkExpedicao.Text = "Expedição";
            this.chkExpedicao.UseVisualStyleBackColor = true;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::a7D.PDV.Caixa.UI.Properties.Resources.btnExcluir1;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // frmPedidoPagamentoTouch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(950, 642);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(950, 590);
            this.Name = "frmPedidoPagamentoTouch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PAGAMENTO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPedidoPagamento_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmPedidoPagamento_FormClosed);
            this.Load += new System.EventHandler(this.frmPagamento_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblCodigoPedido;
        private System.Windows.Forms.TextBox txtObservacoes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.Label lblIdentificacao;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnFecharVenda;
        private Controles.PedidoPagamentoProdutos ppProdutos;
        private Controles.PedidoPagamentoFormas ppFormas;
        private Controles.PedidoPagamentoCliente ppCliente;
        private Controles.PedidoPagamentoControl ppControl;
        private System.Windows.Forms.CheckBox chkImprimir;
        private System.Windows.Forms.CheckBox chkExpedicao;
    }
}