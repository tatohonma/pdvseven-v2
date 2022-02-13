using System;
using System.Windows.Forms;

namespace a7D.PDV.Componentes.Controles
{
    public delegate void TecladoClickEventHandler(object sender, EventArgs e, Int32 n);

    public partial class Teclado : UserControl
    {
        public event TecladoClickEventHandler TecladoClick;
        public event EventHandler Confirmar;

        public Teclado()
        {
            InitializeComponent();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            Confirmar?.Invoke(sender, e);
        }

        private void btnTeclado_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (lEnter)
                btnConfirmar_Click(sender, e);
            else
                TecladoClick?.Invoke(sender, e, Convert.ToInt32(btn.Text));

            lEnter = false;
        }

        bool lEnter = false;

        private void btn1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
           if (e.KeyCode == Keys.Enter)
                lEnter = true;
            else
                lEnter = false;
        }
    }
}