using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace a7D.PDV.Teste.UI
{
    public partial class frmTestSystem : Form
    {
        CultureInfo ptBR = CultureInfo.GetCultureInfo("pt-BR");
        CultureInfo enUS = CultureInfo.GetCultureInfo("en-US");

        public frmTestSystem()
        {
            InitializeComponent();
        }

        private void btnRunTest_Click(object sender, EventArgs e)
        {
            txtOut.Text =
                "CurrentCulture: " + CultureInfo.CurrentCulture.Name + ": " + CultureInfo.CurrentCulture.DisplayName + "\r\n" +
                "CurrentUICulture: " + CultureInfo.CurrentUICulture.Name + ": " + CultureInfo.CurrentUICulture.DisplayName + "\r\n" + 
                "\r\n";

            txtOut.Text += TestDecimal(123);
            txtOut.Text += TestDecimal(1234567.3456m);
            txtOut.Text += TestDecimal(0.345m);

        }

        private string TestDecimal(decimal valor)
        {
            string result = "Decimal: " + valor.ToString() + "\r\n";
            try
            {
                result += "\t N2: " + valor.ToString("N2") + "\r\n";
                result += "\t 0.00: " + valor.ToString("0.00") + "\r\n";

                result += "\t TryParse (default): " + (Decimal.TryParse(valor.ToString(), out decimal v2) ? "OK" : "ERRO");
                result += " = " + v2.ToString() + (valor == v2 ? " IGUAL" : " DIFERENTE") + "\r\n";

                result += "\t TryParse (pt-BR): " + (Decimal.TryParse(valor.ToString(), NumberStyles.Any, ptBR, out decimal v3) ? "OK" : "ERRO");
                result += " = " + v3.ToString() + (valor == v3 ? " IGUAL" : " DIFERENTE") + "\r\n";

                result += "\t TryParse (en-US): " + (Decimal.TryParse(valor.ToString(), NumberStyles.Any, enUS, out decimal v4) ? "OK" : "ERRO");
                result += " = " + v4.ToString() + (valor == v4 ? " IGUAL" : " DIFERENTE") + "\r\n";

            }
            catch (Exception ex)
            {
                result += ex.Message;
            }
            return result + "\r\n";
        }
    }
}
