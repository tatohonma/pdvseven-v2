using a7D.PDV.Ativacao.Utils;
using a7D.PDV.Ativacao.Utils.ValueObject;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace a7D.PDV.Ativacao.WS
{
    /// <summary>
    /// Summary description for CFeService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CFeService : System.Web.Services.WebService
    {


        [WebMethod]
        public bool EnviarCFE(Byte[] docbinaryarray, string docname, string nomeFantasia, string cnpj, string dataDe, string dataAte, string destinatario)
        {

            
            var ms = new MemoryStream();

            //var appData = Server.MapPath("~/ArquivosRecebidos");
            //string strdocPath;
            //strdocPath = Path.Combine(appData, Path.GetFileName(docname));
            //FileStream objfilestream = new FileStream(strdocPath, FileMode.Create, FileAccess.ReadWrite);
            //objfilestream.Write(docbinaryarray, 0, docbinaryarray.Length);
            //objfilestream.Close();

            ms.Write(docbinaryarray, 0, docbinaryarray.Length);
            ms.Flush();


            var infoRemetente = new InformacoesRemetente
            {
                Remetente = ConfigurationManager.AppSettings["SMTPRemetente"].ToString(),
                Senha = ConfigurationManager.AppSettings["SMTPSenha"].ToString(),
                Usuario = ConfigurationManager.AppSettings["SMTPUsuario"].ToString(),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPEnableSsl"].ToString()),
                Host = ConfigurationManager.AppSettings["SMTPHost"].ToString(),
                Porta = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPorta"].ToString()),
                UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTPUseDefaultCredentials"].ToString())
            };

            ms.Position = 0;
            System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Zip);
            System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(ms, docname);
            attach.ContentDisposition.FileName = docname;

            var enderecoRoot = $"http://{Context.Request.Url.Authority}";


            var templateHTML = @"~\TemplatesHtml\emailCfe.html";
            ListDictionary ldReplacements = new ListDictionary();
            ldReplacements.Add("<%NOMEFANTASIA%>", nomeFantasia);
            ldReplacements.Add("<%CNPJ%>", cnpj);
            ldReplacements.Add("<%DATADE%>", dataDe);
            ldReplacements.Add("<%DATAATE%>", dataAte);
            

            Email.EnviarEmailHtml(infoRemetente, $"XML CF-e {nomeFantasia} {cnpj}", destinatario, templateHTML, ldReplacements, attach);

            ms.Close();

            return true;
        }
    }
}
