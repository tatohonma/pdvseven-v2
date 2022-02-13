using a7D.PDV.BLL;
using a7D.PDV.Integracao.Servico.MyFinance;
using a7D.PDV.Integracao.Servico.MyFinance.Models;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace a7D.PDV.Integracao.Servico
{
    public partial class Service1 : ServiceBase
    {
        Timer timer1;
        HttpClient httpClient = new HttpClient();
        Uri baseUrl = new Uri("http://localhost:7788/api/");
        List<WS2.Models.Pedido> listaPedidos = new List<WS2.Models.Pedido>();
        string filePath = @"c:\pdv7\servicoMyFinance.txt";




        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Init();
        }

        protected override void OnStop()
        {

        }

        private void timer1_Tick(object sender)
        {
            

            var dtInicio = DateTimeOffset.Now.AddDays(-10).ToUnixTimeSeconds();
            var dtFim = DateTimeOffset.Now.ToUnixTimeSeconds();

            var pagamentosArquivo = LerArquivo();
            if (pagamentosArquivo != null)
            {
                dtInicio = new DateTimeOffset(pagamentosArquivo.DtPedidoFechamento.Value).ToUnixTimeSeconds(); 
            }

            ObterVendasAPI2Async(dtInicio, dtFim,1000);
        }

        public void Init()
        {
            
            var hoje = DateTime.Now;
            hoje = new DateTime(hoje.Year, hoje.Month, hoje.Day, 23, 59, 59);
            //var intervalo = (Int64) new TimeSpan(24, 0, 0).TotalMilliseconds;

            var intervalo = (Int64)new TimeSpan(1, 0, 30).TotalMilliseconds;
            //var inicio = (Int64)(hoje - DateTime.Now).TotalMilliseconds;
            var inicio = (Int64)(DateTime.Now.AddSeconds(3) - DateTime.Now).TotalMilliseconds;

            timer1 = new Timer(new TimerCallback(timer1_Tick), null, inicio, intervalo);
        }

        private void music()
        {
            var dur = 500;
            var c = 1046;
            var d = 1174;
            var e = 1318;
            var f = 1396;
            var g = 1597;
            Console.Beep(e, dur);
            Console.Beep(e, dur);
            Console.Beep(f, dur);
            Console.Beep(g, dur);
            Console.Beep(g, dur);
            Console.Beep(f, dur);
            Console.Beep(e, dur);
            Console.Beep(d, dur);
            Console.Beep(c, dur);
            //for (int i = 1000; i < 10000; i=i+20)
            //{
            //    Console.Beep(i, 100);
            //}
        }

        private async void ObterVendasAPI2Async(long dtInicio, long dtFim, int qtRegistros)
        {
            int pagina = 1;
            int countTotalPesquisa = 0;
            var response = await GetPedidosFechados(dtInicio, dtFim, pagina, qtRegistros);
            var headerTotalPesquisa = response.Headers.GetValues("countTotalPesquisa").FirstOrDefault();
            var pedidosString = await response.Content.ReadAsStringAsync();
            listaPedidos.AddRange(JsonConvert.DeserializeObject<List<WS2.Models.Pedido>>(pedidosString));


            if (!string.IsNullOrEmpty(headerTotalPesquisa))
            {
                countTotalPesquisa = Convert.ToInt32(headerTotalPesquisa);

                while (qtRegistros * pagina < countTotalPesquisa)
                {
                    pagina++;
                    response = await GetPedidosFechados(dtInicio, dtFim, pagina, qtRegistros);
                    var responseString = await response.Content.ReadAsStringAsync();
                    listaPedidos.AddRange(JsonConvert.DeserializeObject<List<WS2.Models.Pedido>>(responseString));
                }
            }
            
            EnviarRecebiveis(MontarRecebiveis(listaPedidos.OrderBy(p => p.DtPedidoFechamento).ToList()));
        }

        private async Task<HttpResponseMessage> GetPedidosFechados(long dtInicio, long dtFim, int pagina, int qtRegistros)
        {
            var endereco = new Uri(baseUrl, "pedidos");
            endereco = endereco
                .AddQuery("dtFechamentoMin", $"{dtInicio}")
                .AddQuery("dtFechamentoMax", $"{dtFim}")
                .AddQuery("pagina", $"{pagina}")
                .AddQuery("qtRegistros", $"{qtRegistros}");

            return await httpClient.GetAsync(endereco);
        }

        private void EnviarRecebiveis(List<Recebivel> listaRecebiveis)
        {
            bool erroEnvio = false;
            Recebivel ultimoRecebivel = null;
            foreach (var recebivel in listaRecebiveis)
            {
                EscreverArquivo(recebivel.Pedido);
                var resposta = MyFinanceAPI.PostRecebivel(recebivel).Result;
                if (!resposta.IsSuccessStatusCode)
                {
                    Thread.Sleep(2000);
                    if ((int)resposta.StatusCode == 429)//too many requests
                        Thread.Sleep(30000);
                    ultimoRecebivel = recebivel;
                    erroEnvio = true;
                    break;
                }
            }
            if (erroEnvio)
            {
                listaRecebiveis = listaRecebiveis
                    .Where(r => r.Pedido.Pagamentos.FirstOrDefault().ID >= ultimoRecebivel.Pedido.Pagamentos.FirstOrDefault().ID).ToList();
                EnviarRecebiveis(listaRecebiveis);
            }
        }

        private List<Recebivel> MontarRecebiveis(List<WS2.Models.Pedido> pedidos)
        {
            List<Recebivel> listaRecebiveis = new List<Recebivel>();
            foreach (var pedido in pedidos)
            {
                foreach (var pagamento in pedido.Pagamentos)
                {
                    var pedidoFlat = new WS2.Models.Pedido
                    {
                        IDPedido = pedido.IDPedido,
                        DtPedidoFechamento = pedido.DtPedidoFechamento,
                        Pagamentos = new List<WS2.Models.Pagamento> { { pagamento } }
                    };

                    var recebivel = new Recebivel(pedidoFlat);
                    recebivel.Description = $"Pedido {pedido.IDPedido} - Pagamento {pagamento.ID}";
                    recebivel.NominalAmount = pagamento.Valor.Value;
                    recebivel.OcurredAt = pedido.DtPedidoFechamento.Value;
                    listaRecebiveis.Add(recebivel);
                }   
            }
            return listaRecebiveis;
        }

        public void EscreverArquivo(WS2.Models.Pedido pedido)
        {
            try
            {
                StreamWriter vWriter = null;

                if (File.Exists(filePath))
                    vWriter = new StreamWriter(filePath, false);
                else
                    vWriter = new StreamWriter(filePath, true);

                vWriter.WriteLine($"DtFechamentoPedido      {pedido.DtPedidoFechamento.Value}");
                vWriter.WriteLine($"IDPedido                {pedido.IDPedido}");
                vWriter.WriteLine($"IDPagamento             {pedido.Pagamentos[0].ID}");
                vWriter.Flush();
                vWriter.Close();
            }
            catch (Exception)
            {

            }

        }

        //retorna ultimo pagamento q tentou ser enviado ao MyFinance
        public WS2.Models.Pedido LerArquivo()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    using(var writer = new StreamWriter(filePath))
                    {
                        writer.Flush();
                        writer.Close();
                    }
                }

                int indexLine = 24;
                var file = new StreamReader(filePath).ReadToEnd();
                var lines = file.Split(new char[] { '\n' });
                if (lines.Count() < 1)
                {
                    return null;
                }

                return new WS2.Models.Pedido
                {
                    DtPedidoFechamento = DateTime.Parse(lines[0].Substring(indexLine)),
                    IDPedido = Convert.ToInt32(lines[1].Substring(indexLine)),
                    Pagamentos = new List<WS2.Models.Pagamento> { { new WS2.Models.Pagamento { ID = Convert.ToInt32(lines[2].Substring(indexLine)) } } }
                };

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
