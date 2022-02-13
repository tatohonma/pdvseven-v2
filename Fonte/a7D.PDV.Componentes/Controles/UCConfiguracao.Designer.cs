namespace a7D.PDV.Componentes.Controles
{
    partial class UCConfiguracao
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCConfiguracao));
            this.tbLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblChave = new System.Windows.Forms.Label();
            this.llAlterar = new System.Windows.Forms.LinkLabel();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.tbLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLayout
            // 
            this.tbLayout.ColumnCount = 3;
            this.tbLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.44444F));
            this.tbLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.44444F));
            this.tbLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tbLayout.Controls.Add(this.lblChave, 0, 0);
            this.tbLayout.Controls.Add(this.llAlterar, 2, 0);
            this.tbLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLayout.Location = new System.Drawing.Point(0, 0);
            this.tbLayout.Name = "tbLayout";
            this.tbLayout.RowCount = 1;
            this.tbLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbLayout.Size = new System.Drawing.Size(793, 40);
            this.tbLayout.TabIndex = 0;
            // 
            // lblChave
            // 
            this.lblChave.AutoSize = true;
            this.lblChave.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblChave.Location = new System.Drawing.Point(206, 0);
            this.lblChave.Name = "lblChave";
            this.lblChave.Size = new System.Drawing.Size(143, 40);
            this.lblChave.TabIndex = 0;
            this.lblChave.Text = "Configuração Dummy";
            this.lblChave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblChave.MouseEnter += new System.EventHandler(this.lblChave_MouseEnter);
            this.lblChave.MouseLeave += new System.EventHandler(this.lblChave_MouseLeave);
            // 
            // llAlterar
            // 
            this.llAlterar.AutoSize = true;
            this.llAlterar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llAlterar.Location = new System.Drawing.Point(707, 0);
            this.llAlterar.Name = "llAlterar";
            this.llAlterar.Size = new System.Drawing.Size(83, 40);
            this.llAlterar.TabIndex = 1;
            this.llAlterar.TabStop = true;
            this.llAlterar.Text = "Utilizar Padrão";
            this.llAlterar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.llAlterar.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "close-box-outline.png");
            this.imgList.Images.SetKeyName(1, "close-box-outline_red.png");
            // 
            // UCConfiguracao
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tbLayout);
            this.Name = "UCConfiguracao";
            this.Size = new System.Drawing.Size(793, 40);
            this.Load += new System.EventHandler(this.UCConfiguracao_Load);
            this.tbLayout.ResumeLayout(false);
            this.tbLayout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbLayout;
        private System.Windows.Forms.Label lblChave;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.LinkLabel llAlterar;
    }
}
