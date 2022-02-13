using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using a7D.PDV.BLL;

namespace a7D.PDV.Ativacao.UI
{
    public partial class frmGerarChaveAtivacao : Form
    {
        public frmGerarChaveAtivacao()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnGerarCodigoAtivacao_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            String chaveAtivacao;
            String codigoRevenda;
            String codigoAtivacao;
            Decimal codigoVerificadorNumerico = 0;
            String codigoVerificador;
            String codigo1;

            codigoRevenda = txtCodigoRevenda.Text;
            codigoAtivacao = rnd.Next(99999).ToString("00000");

            codigo1 = codigoRevenda + codigoAtivacao;

            for (int i = 0; i < codigo1.Length - 1; i++)
            {
                codigoVerificadorNumerico = codigoVerificadorNumerico + Convert.ToDecimal(codigo1[i].ToString()) + 1 / 2;
            }

            if (Convert.ToInt32(codigoVerificadorNumerico) <= 99)
                codigoVerificador = Convert.ToInt32(codigoVerificadorNumerico).ToString("00");
            else
                codigoVerificador = codigoVerificadorNumerico.ToString().Substring(0, 2);

            chaveAtivacao = codigoRevenda + "-" + codigoAtivacao + "-" + codigoVerificador;
            txtChaveAtivacao.Text = chaveAtivacao;
        }
    }
}
