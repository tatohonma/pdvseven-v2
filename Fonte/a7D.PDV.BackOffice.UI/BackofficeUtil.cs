using System.IO;
using System.Windows.Forms;

namespace a7D.PDV.BackOffice.UI
{
    public static class BackofficeUtil
    {
        public static string NomeRelatorio(string nome)
        {
            var dir = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Relatorios");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return Path.Combine(dir, nome);
        }
    }
}
