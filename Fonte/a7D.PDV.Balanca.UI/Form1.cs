using a7D.PDV.Balanca.Interfaces;
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
    public partial class Form1 : Form
    {
        private IBalanca Balanca;
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Balanca = BalancaFactory.ObterBalanca(Tipo.TOLEDO);
            //Cursor = Cursors.WaitCursor;
            if (string.IsNullOrWhiteSpace(txtUrl.Text))
                DadosLidos(Balanca.LerPesoAsync(cbPort.SelectedValue as string).Result);
            else
                DadosLidos(Balanca.LerPesoWs(txtUrl.Text));
            //Cursor = Cursors.Default;
        }

        private void DadosLidos(Dados dados)
        {
            string text = string.Empty;

            switch (dados.Status)
            {
                case Status.OK:
                    text = dados.Peso.ToString("#,##0.000 Kg");
                    break;
                case Status.INSTAVEL:
                    text = "Peso Instável";
                    break;
                case Status.SOBREPESO:
                    text = "Sobrepeso";
                    break;
                case Status.ERRO:
                    text = "Erro de Leitura";
                    break;
            }

            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    AtualizarPeso(text);
                }));
            }
            else
            {
                AtualizarPeso(text);
            }
        }

        private void AtualizarPeso(string text)
        {
            label2.Text = text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbPort.DataSource = SerialPort.GetPortNames();
            timer1.Interval = 500;
            timer1.Tick += Timer1_Tick;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Balanca = BalancaFactory.ObterBalanca(Tipo.TOLEDO);
            DadosLidos(Balanca.LerPesoAsync(cbPort.SelectedValue as string).Result);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new frmEnviarPeso().Show();
        }
    }
}
