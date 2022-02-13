using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Integracao.Servico.UI
{
    class FontesUtil
    {
        public static void InstalaFontes(TextBox txt)
        {
            try
            {
                string path = Assembly.GetExecutingAssembly().Location;
                path = path.Substring(0, path.LastIndexOf(@"\"));

                var di = new DirectoryInfo(path);
                foreach (var fi in di.GetFiles("*.ttf"))
                {
                    txt.Text += "Favor instalar a fonte: " + fi.FullName + Environment.NewLine;
                    System.Diagnostics.Process.Start(fi.FullName);
                }

                txt.Text +=
                    Environment.NewLine +
                    "!!! ATENÇÃO !!!" +
                    Environment.NewLine +
                    "É preciso encerrar o programa para as novas fontes serem reconhecidas\r\nSe o problema persistir instale a fonte manualmente" +
                    Environment.NewLine + Environment.NewLine;
            }
            catch (Exception ex)
            {
                txt.Text += "\r\nERRO:" + ex.Message + Environment.NewLine + ex.StackTrace;
            }
        }
    }
}
