using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using System;
using System.Windows.Forms;

namespace a7D.PDV.Configurador.UI
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!frmConfigurarConexaoDB.Verifica())
                return;

            var autenticacao = new frmAutenticacao(true, true, false, false, false);
            var result = autenticacao.ShowDialog();
            if (result == DialogResult.OK)
                Application.Run(new frmPrincipal());
        }
    }
}
