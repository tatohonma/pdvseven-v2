using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.Pagamento.UI.Controles
{
    public partial class Teclado : UserControl
    {
        public delegate void TecladoClickEventHandler(object sender, System.EventArgs e, Int32 n);
        public event TecladoClickEventHandler TecladoClick;

        public delegate void ConfirmarEventHandler(object sender, System.EventArgs e);
        public event ConfirmarEventHandler Confirmar;

        public Teclado()
        {
            InitializeComponent();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            Confirmar(sender, e);
        }

        private void btnTeclado_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            TecladoClick(sender, e, Convert.ToInt32(btn.Text));
        }
    }
}
