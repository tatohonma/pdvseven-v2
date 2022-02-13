using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.LimparLicencas.UI
{
    static class Program
    {
        static Mutex mutex = new Mutex(false, "pdv7-limparlicencas-insntancia");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(2), false))
            {
                MessageBox.Show("O LimparLicencas já está sendo executado!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmLimparLicencas());
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }
}
