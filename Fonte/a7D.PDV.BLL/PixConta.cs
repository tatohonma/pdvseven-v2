using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL.Utils;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class PixConta
    {
        public static async Task<string> GerarFaturaAsync(PedidoInformation pedido)
        {
            string token = ConfiguracoesPixConta.BuscarConfiguracao("Token_IUGU").Valor;
            //string token = "ODk3OTA0MDM1NUYwODhENEM5RkU0MDJDRTBBNDJERDQ3ODE3NUQ5QzcxNEJENThDMDBGNDA3MTVCNERDMkY3MDo="; // Substitua pelo seu token Bearer
            string apiUrl = "https://api.iugu.com/v1/invoices";
            decimal valorPendente = Pedido.ValorPendente(pedido.IDPedido.Value);

            FaturaPixContaInformation faturaPixConta = new FaturaPixContaInformation();
            faturaPixConta.Pedido = new PedidoInformation { IDPedido = pedido.IDPedido };
            faturaPixConta.Status = "pendente";

            FaturaPixContaInformation faturaPixContaGerada = (FaturaPixContaInformation)CRUD.Carregar(faturaPixConta);

            if (faturaPixContaGerada != null && faturaPixContaGerada.IDFaturaPixConta != null && faturaPixContaGerada.Status == "pendente")
            {
                if (faturaPixContaGerada.Valor == valorPendente)
                {
                    return faturaPixContaGerada.ChavePix;
                }
                else
                {
                    faturaPixContaGerada.Status = "cancelar";
                    faturaPixContaGerada.DtUltimaAlteracao = DateTime.Now;

                    CRUD.Alterar(faturaPixContaGerada);
                }
            }
            string resultado = await ApiClient.PostJsonAsync(apiUrl, CriarJson(valorPendente), token);
            FaturaIuguInformation faturaIugu = JsonConvert.DeserializeObject<FaturaIuguInformation>(resultado);

            faturaPixConta.CodigoFatura = faturaIugu.id;
            faturaPixConta.ChavePix = faturaIugu.pix.qrcode_text;
            faturaPixConta.Status = "pendente";
            faturaPixConta.Valor = valorPendente;
            faturaPixConta.DtInclusao = DateTime.Now;
            faturaPixConta.DtUltimaAlteracao = DateTime.Now;
            CRUD.Adicionar(faturaPixConta);

            return faturaIugu.pix.qrcode_text;
        }

        private static string CriarJson(decimal valorPendente)
        {
            string json = @"
                {
                    ""ensure_workday_due_date"": false,
                    ""payable_with"": [""pix""],
                    ""items"": [
                        {
                            ""description"": ""Pedido PDV7"",
                            ""quantity"": 1,
                            ""price_cents"": " + (valorPendente * 100).ToString("F0") + @"
                        }
                    ],
                    ""email"": ""suporte@pdvseven.com.br"",
                    ""due_date"": """ + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + @"""
                }";

            return json;
        }

        public class FaturaIuguInformation
        {
            public string id { get; set; }
            public Pix pix { get; set; }

            public class Pix
            {
                public string qrcode_text { get; set; }
            }
        }
    }
}