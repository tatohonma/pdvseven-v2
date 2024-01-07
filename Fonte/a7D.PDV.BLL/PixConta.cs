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

            string resultado = await ApiClient.PostJsonAsync(apiUrl, CriarJson(pedido), token);

            FaturaInformation fatura = JsonConvert.DeserializeObject<FaturaInformation>(resultado);

            Tag.Adicionar(pedido.GUIDMovimentacao, "FaturaPixConta_ContaCliente_ID", fatura.id);
            Tag.Adicionar(pedido.GUIDMovimentacao, "FaturaPixConta_ContaCliente_Status", "pendente");

            string qrcodeText = fatura.pix.qrcode_text;
            return qrcodeText;
        }

        private static string CriarJson(PedidoInformation pedido)
        {
            var valorTotalProdutosServicos = pedido.ValorTotalProdutosServicos + (pedido.ValorEntrega.HasValue ? pedido.ValorEntrega.Value : 0);
            decimal totalJaPago = 0;
            foreach (var pagamento in pedido?.ListaPagamento.Where(p => p.Status != StatusModel.Excluido))
                totalJaPago += pagamento.Valor.Value;

            decimal totalApagar;
            if (pedido.ValorDesconto.HasValue && pedido.ValorDesconto > 0)
                totalApagar = valorTotalProdutosServicos - pedido.ValorDesconto.Value - totalJaPago;
            else
                totalApagar = valorTotalProdutosServicos - totalJaPago;

            // Criar o JSON diretamente no código
            string json = @"
                {
                    ""ensure_workday_due_date"": false,
                    ""payable_with"": [""pix""],
                    ""items"": [
                        {
                            ""description"": ""Pedido PDV7"",
                            ""quantity"": 1,
                            ""price_cents"": " + (totalApagar * 100).ToString("F0") + @"
                        }
                    ],
                    ""email"": ""suporte@pdvseven.com.br"",
                    ""due_date"": """ + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + @"""
                }";

            return json;
        }

        public class FaturaInformation
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