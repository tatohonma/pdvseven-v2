namespace a7D.PDV.Caixa.UI.Controles
{
    partial class PedidoPagamentoFormas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.grpTouch = new System.Windows.Forms.GroupBox();
            this.tpList = new a7D.PDV.Componentes.Controles.TiposPagamentos();
            this.dgvPagamentos = new System.Windows.Forms.DataGridView();
            this.Remover = new System.Windows.Forms.DataGridViewImageColumn();
            this.TipoPagamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.split = new System.Windows.Forms.SplitContainer();
            this.grpTouch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagamentos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::a7D.PDV.Caixa.UI.Properties.Resources.btnExcluir1;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // grpTouch
            // 
            this.grpTouch.Controls.Add(this.tpList);
            this.grpTouch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTouch.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpTouch.Location = new System.Drawing.Point(0, 0);
            this.grpTouch.Margin = new System.Windows.Forms.Padding(4);
            this.grpTouch.Name = "grpTouch";
            this.grpTouch.Padding = new System.Windows.Forms.Padding(4);
            this.grpTouch.Size = new System.Drawing.Size(533, 200);
            this.grpTouch.TabIndex = 111;
            this.grpTouch.TabStop = false;
            this.grpTouch.Text = "Forma de Pagamento";
            // 
            // tpList
            // 
            this.tpList.AutoScroll = true;
            this.tpList.BackColor = System.Drawing.Color.Transparent;
            this.tpList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpList.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tpList.Location = new System.Drawing.Point(4, 20);
            this.tpList.Margin = new System.Windows.Forms.Padding(5);
            this.tpList.Name = "tpList";
            this.tpList.Size = new System.Drawing.Size(525, 176);
            this.tpList.TabIndex = 0;
            this.tpList.PagamentoSelecionadoValor += new a7D.PDV.Componentes.Controles.PagamentoSelecionadoEventHandler(this.tpList_PagamentoSelecionadoValor);
            // 
            // dgvPagamentos
            // 
            this.dgvPagamentos.AllowUserToAddRows = false;
            this.dgvPagamentos.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(239)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 11.25F);
            this.dgvPagamentos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPagamentos.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.dgvPagamentos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPagamentos.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvPagamentos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(193)))), ((int)(((byte)(207)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPagamentos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPagamentos.ColumnHeadersHeight = 40;
            this.dgvPagamentos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Remover,
            this.TipoPagamento,
            this.Valor});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPagamentos.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvPagamentos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPagamentos.EnableHeadersVisualStyles = false;
            this.dgvPagamentos.Location = new System.Drawing.Point(0, 0);
            this.dgvPagamentos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvPagamentos.MultiSelect = false;
            this.dgvPagamentos.Name = "dgvPagamentos";
            this.dgvPagamentos.ReadOnly = true;
            this.dgvPagamentos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPagamentos.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPagamentos.RowHeadersVisible = false;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 11.25F);
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(5);
            this.dgvPagamentos.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvPagamentos.RowTemplate.Height = 35;
            this.dgvPagamentos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvPagamentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPagamentos.Size = new System.Drawing.Size(533, 344);
            this.dgvPagamentos.TabIndex = 113;
            this.dgvPagamentos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPagamentos_CellClick);
            // 
            // Remover
            // 
            this.Remover.HeaderText = "";
            this.Remover.Image = global::a7D.PDV.Caixa.UI.Properties.Resources.btnExcluir1;
            this.Remover.Name = "Remover";
            this.Remover.ReadOnly = true;
            this.Remover.Width = 40;
            // 
            // TipoPagamento
            // 
            this.TipoPagamento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TipoPagamento.DataPropertyName = "TipoPagamento";
            this.TipoPagamento.HeaderText = "Forma Pagamento";
            this.TipoPagamento.Name = "TipoPagamento";
            this.TipoPagamento.ReadOnly = true;
            // 
            // Valor
            // 
            this.Valor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Valor.DataPropertyName = "Valor";
            dataGridViewCellStyle3.Format = "C2";
            dataGridViewCellStyle3.NullValue = null;
            this.Valor.DefaultCellStyle = dataGridViewCellStyle3;
            this.Valor.HeaderText = "Valor Pago";
            this.Valor.Name = "Valor";
            this.Valor.ReadOnly = true;
            this.Valor.Width = 99;
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.Location = new System.Drawing.Point(0, 0);
            this.split.Name = "split";
            this.split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.grpTouch);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.dgvPagamentos);
            this.split.Size = new System.Drawing.Size(533, 554);
            this.split.SplitterDistance = 200;
            this.split.SplitterWidth = 10;
            this.split.TabIndex = 114;
            this.split.TabStop = false;
            // 
            // PedidoPagamentoFormas
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.split);
            this.Name = "PedidoPagamentoFormas";
            this.Size = new System.Drawing.Size(533, 554);
            this.Load += new System.EventHandler(this.PedidoPagamentoFormas_Load);
            this.grpTouch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPagamentos)).EndInit();
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.GroupBox grpTouch;
        private Componentes.Controles.TiposPagamentos tpList;
        private System.Windows.Forms.DataGridView dgvPagamentos;
        private System.Windows.Forms.DataGridViewImageColumn Remover;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoPagamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
        private System.Windows.Forms.SplitContainer split;
    }
}
