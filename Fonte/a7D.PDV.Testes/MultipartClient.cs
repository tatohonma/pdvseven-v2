using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace a7D.PDV.Testes
{
    class MultipartClient
    {
        public async Task Test()
        {
            using (var client = new HttpClient())
            using (var form = new MultipartFormDataContent()) // caso não funcione, tem um construtor dessa classe que recebe uma string para o boundary
            using (var dados = new StringContent(new JavaScriptSerializer().Serialize(new { nome = "Alberson" }), Encoding.UTF8, "application/json")) // campo de texto com json
            using (var img = new MemoryStream()) // esse stream vai vir da request de upload
            using (var imagem = new ByteArrayContent(img.ToArray()))
            {
                form.Add(dados);
                form.Add(imagem);
                var resopnse = await client.PostAsync("url", form);
            }
        }
    }
}
