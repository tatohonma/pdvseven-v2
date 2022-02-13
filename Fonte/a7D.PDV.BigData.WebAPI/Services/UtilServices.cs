using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace a7D.PDV.BigData.WebAPI.Services
{
    public static class UtilServices
    {
        public static async Task<ModelStateDictionary> BadRequestModel(this ApiController api)
        {
            var json = await api.GetRawPostData();
            api.ModelState.AddModelError("body_request", json);
            return api.ModelState;
        }
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
            Task.Run(()=>Iaago.Util.Email.EnviaErro(ex));

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
    }
}