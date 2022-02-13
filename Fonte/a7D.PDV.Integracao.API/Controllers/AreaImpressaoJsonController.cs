using a7D.PDV.BLL;
using a7D.PDV.Integracao.API.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace a7D.PDV.Integracao.API.Controllers
{

    public class AreaImpressaoJsonController : ApiController
    {
        private string Versao => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        [HttpGet]
        public IHttpActionResult Listar()
        {
            var response = new AreaImpressaoListarResponse();
            var listaAreaImpressao = new List<AreaImpressaoListarResponse.AreaImpressao>();

            AreaImpressao
                .Listar()
                .OrderBy(l => l.Nome)
                .ToList()
                .ForEach(area =>
                {
                    listaAreaImpressao.Add(new AreaImpressaoListarResponse.AreaImpressao
                    {
                        idAreaImpressao = area.IDAreaImpressao.Value,
                        nome = area.Nome
                    });
                });

            response.areasImpressao = listaAreaImpressao.ToArray();

            response.sucesso = true;
            response.versao = Versao;

            return ResponseMessage(new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(response), Encoding.UTF8, "text/json"),
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
