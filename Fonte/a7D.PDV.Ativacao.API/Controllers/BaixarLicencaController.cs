using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Filters;
using a7D.PDV.Ativacao.Shared.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace a7D.PDV.Ativacao.API.Controllers
{
    [ApiAuth]
    public class BaixarLicencaController : ApiController
    {
        private AtivacaoContext db = new AtivacaoContext();

        [HttpGet]
        public HttpResponseMessage Index(int id)
        {
            var ativacao = db.Ativacoes.Find(id);
            db.Entry(ativacao).Collection(a => a.PDVs).Load();

            if (ativacao.PDVs.Count < 1)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var sb = new StringBuilder();

            sb.AppendLine(string.Format(
@"DECLARE @chaveAtivacao AS VARCHAR(50)
SET @chaveAtivacao = '{0}'

DELETE FROM tbConfiguracao WHERE chave='chaveAtivacao'
INSERT [dbo].[tbConfiguracao] ([Chave], [Valor]) VALUES (N'chaveAtivacao', @chaveAtivacao)

DELETE FROM [tbPDV]", ativacao.ChaveAtivacao));

            var proximoId = 0;
            foreach (var pdv in ativacao.PDVs)
                sb.AppendLine(pdv.Insert(++proximoId));

            var lic = CryptMD5.Criptografar(sb.ToString());

            var base64string = Convert.ToBase64String(Encoding.UTF8.GetBytes(CryptMD5.Criptografar(sb.ToString())));

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(base64string);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition.FileName = ativacao.Cliente.Nome.Replace(" ", string.Empty) + ".lic";
            return response;
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
