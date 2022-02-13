using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Balanca.UI
{
    public partial class frmEnviarPeso : Form
    {
        public frmEnviarPeso()
        {
            InitializeComponent();
        }

        private void frmEnviarPeso_Load(object sender, EventArgs e)
        {
            cbbSerial.DataSource = SerialPort.GetPortNames();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                var port = new SerialPort(cbbSerial.SelectedValue as string, 9600, Parity.None, 8, StopBits.One);
                port.WriteTimeout = 400;
                port.ReadTimeout = 400;
                port.Open();
                var comand = ((char)2).ToString() + txtPeso.Text.PadLeft(6, '0') + ((char)3).ToString();
                port.WriteLine(comand);
                port.DataReceived += Port_DataReceived;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string existing = sp.ReadExisting();
            if (!string.IsNullOrWhiteSpace(existing))
            {
                MessageBox.Show(existing);
            }
            sp.Close();
        }
    }
}
