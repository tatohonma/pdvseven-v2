using a7D.PDV.BLL.ValueObject;
using System.Drawing;

namespace a7D.PDV.Componentes
{
    public partial class frmSelecao : FormTouch
    {
        public int Selecionado { get; private set; }

        public static int Select(string title, string subTitle, SelectIDValor[] itens)
        {
            var frm = new frmSelecao(title, subTitle, itens);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                return frm.Selecionado;
            else
                return -1; // Cancelado
        }

        public frmSelecao()
        {
            InitializeComponent();
        }

        public frmSelecao(string title, string subTitle, SelectIDValor[] itens) : this()
        {
            Text = title;
            lblSubTitle.Text = subTitle;
            lbOpcoes.DataSource = itens;
            lbOpcoes.ClearSelected();
        }

        private void listBox1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
        {
            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Define the default color of the brush as black.
            e.Graphics.DrawString(lbOpcoes.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        private void lbOpcoes_SelectedValueChanged(object sender, System.EventArgs e)
        {
            btnConfirmar.Enabled = lbOpcoes.SelectedIndex != -1;
        }

        private void btnConfirmar_Click(object sender, System.EventArgs e)
        {
            if (lbOpcoes.SelectedIndex == -1)
                return;

            Selecionado = (lbOpcoes.Items[lbOpcoes.SelectedIndex] as SelectIDValor).ID;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
