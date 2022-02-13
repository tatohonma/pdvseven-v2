using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

namespace a7D.PDV.Ativacao.API.Services
{
    public static class UtilServices
    {
        private static readonly string ErrosDEV = ConfigurationManager.AppSettings["ErrosDEV"];

        public static Exception ObterDadosErro(this ApiController api, Exception ex)
        {
            try
            {
                var body = api.GetRawPostData().Result;
                ex.Data.Add("url", api.Request.RequestUri);
                ex.Data.Add("json", body);
            }
            catch (Exception ex2)
            {
                ex.Data.Add("ex2", ex2.Message);
            }
            return ConcaternarErro(ex);
        }

        public static async Task<string> GetRawPostData(this ApiController api)
        {
            using (var contentStream = await api.Request.Content.ReadAsStreamAsync())
            {
                contentStream.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(contentStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public static Exception ConcaternarErro(Exception ex)
        {
            EnviaErro(ex);

            string erro = "ERRO! ";
            Exception inner = ex;
            int max = 5; // para evitar loop em alguns casos muito específico
            while (inner != null && --max > 0)
            {
                erro += inner.Message + " \r\n";
                inner = inner.InnerException;
            }

            return new Exception(erro, ex);
        }

        public static void EnviaErro(Exception ex)
        {
            try
            {
                string erro = "ERRO: ";
                int max = 5; // para evitar loop em alguns casos muito específico
                while (ex != null && --max > 0)
                {
                    erro += ex.Message + "\r\n";
                    foreach (var key in ex.Data.Keys)
                    {
                        erro += $"\t{key}: {ex.Data[key]}\r\n";
                    }

                    erro += "\r\n" + ex.StackTrace + "\r\n\r\n";
                    ex = ex.InnerException;
                }

                EmailServices.Enviar(ErrosDEV, "ERRO Ativações", erro, html: false);
            }
            catch
            {
            }
        }
    }
}