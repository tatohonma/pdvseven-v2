using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace a7D.PDV.Integracao.Servico.Core.Impressao
{
    class FontesUtil
    {
        public static string InstalaFontes()
        {
            string result = "";
            try
            {
                string path = Assembly.GetExecutingAssembly().Location;
                path = path.Substring(0, path.LastIndexOf(@"\"));

                var di = new DirectoryInfo(path);
                foreach (var fi in di.GetFiles("*.ttf"))
                {
                    result += "Favor instalar a fonte: " + fi.FullName + Environment.NewLine;
                    System.Diagnostics.Process.Start(fi.FullName);
                }

                result +=
                    Environment.NewLine +
                    "!!! ATENÇÃO !!!" +
                    Environment.NewLine +
                    "É preciso encerrar o programa para as novas fontes serem reconhecidas\r\nSe o problema persistir instale a fonte manualmente" +
                    Environment.NewLine + Environment.NewLine;
            }
            catch (Exception ex)
            {
                result += "\r\nERRO:" + ex.Message + Environment.NewLine + ex.StackTrace;
            }
            return result;
        }
    }
}
