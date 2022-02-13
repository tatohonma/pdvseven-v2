using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace a7D.PDV.Configurador.UI
{
    public partial class frmLogSATSemData : Form
    {
        public string Log { get; private set; }

        public frmLogSATSemData()
        {
            InitializeComponent();
        }

        public frmLogSATSemData(string log)
        {
            InitializeComponent();
            Log = log;
        }

        private void frmLogSATSemData_Load(object sender, EventArgs e)
        {
            txtLog.Text = Log.Replace("\n", Environment.NewLine);
        }
    }
}
