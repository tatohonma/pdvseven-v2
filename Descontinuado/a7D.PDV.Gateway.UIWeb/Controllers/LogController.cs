using a7D.PDV.Gateway.UIWeb.Filters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace a7D.PDV.Gateway.UIWeb.Controllers
{
    [JwtAuthorize]
    public class LogController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Last([FromUri] int linhas = 15, [FromUri] string tipo = "")
        {
            try
            {
                string padraoBusca = "*.log";
                if (!string.IsNullOrWhiteSpace(tipo))
                {
                    switch (tipo.ToLower())
                    {
                        case "debug":
                            padraoBusca = "*-debug.log";
                            break;
                        case "error":
                            padraoBusca = "*-error.log";
                            break;
                    }
                }

                var path = HttpContext.Current.Request.MapPath("../Logs");
                string strLastLog = ObterUltimoArquivo(path, padraoBusca);

                if (string.IsNullOrWhiteSpace(strLastLog))
                    return Ok(new { log = "Não há log." });

                return Ok(new { log = ObterLog(linhas, strLastLog) });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private static string ObterUltimoArquivo(string path, string padraoBusca = "*.log")
        {
            string strLastLog = string.Empty;
            DateTime lastLog = new DateTime(1970, 1, 1);
            foreach (var arquivo in Directory.EnumerateFiles(path, padraoBusca, SearchOption.TopDirectoryOnly))
            {
                var nome = new FileInfo(arquivo).Name;
                var rg = new Regex(padraoBusca.Replace("*", string.Empty));
                DateTime dt;
                if (!DateTime.TryParseExact(rg.Replace(nome, ""), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    continue;
                }
                if (dt.Date > lastLog.Date)
                {
                    lastLog = dt.Date;
                    strLastLog = arquivo;
                }
            }

            return strLastLog;
        }

        private static string ObterLog(int linhas, string arquivo)
        {
            string log;
            using (var fs = File.OpenRead(arquivo))
            {
                fs.Seek(0, SeekOrigin.End);

                int newLines = 0;
                while (newLines < linhas)
                {
                    fs.Seek(-1, SeekOrigin.Current);
                    newLines += fs.ReadByte() == 13 ? 1 : 0; // look for \r
                    fs.Seek(-1, SeekOrigin.Current);
                }

                byte[] data = new byte[fs.Length - fs.Position];
                fs.Read(data, 0, data.Length);
                log = Encoding.UTF8.GetString(data);
            }

            return log;
        }
    }
}
