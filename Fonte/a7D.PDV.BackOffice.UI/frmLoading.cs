using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public partial class frmLoading : Form
    {
        private Action action;

        public frmLoading(Action action)
        {
            this.action = action;
            InitializeComponent();
        }

        private void frmLoading_Load(object sender, EventArgs e)
        {
        }

        private void frmLoading_Shown(object sender, EventArgs e)
        {
            action();
            Close();
        }
    }
}
