using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Terminal.UI
{
    class TerminalEventArgs : EventArgs
    {
        public DialogResult Result { get; set; }
        public int IDPedido { get; set; }
    }
}
