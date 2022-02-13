using a7D.PDV.BigData.Shared.ValueObject;
using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.Servico.Core;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;

namespace a7D.PDV.Integracao.Server.BigData
{
    public class BigDataServices
    {
        internal const string titulo = "BigData+Iaago:";

        public event OnMensagemListener AddLog;

        private static IBigDataAPI API;
        private static DateTime IntegradorCheckSync;
        private static bdAlteracaoInfo SincronismoInfo;
        private static DateTime lastServerSync;

        private int intervaloSync;

        public int IntervaloIntegracao
        {
            get => intervaloSync;
            set
            {
                if (value > 60)
                    intervaloSync = 60;
                else if (value < 0)
                    intervaloSync = 1;
                else
                    intervaloSync = value;
            }
        }

        static BigDataServices()
        {
            IntegradorCheckSync = lastServerSync = DateTime.MinValue;

            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://pdvsevenbd.azurewebsites.net"),
                //BaseAddress = new Uri("http://localhost:50560"),
            };
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var settings = new RefitSettings
            {
                JsonSerializerSettings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            };

            API = RestService.For<IBigDataAPI>(client, settings);
        }

        public void Sync()
        {
            if (IntegradorCheckSync > DateTime.Now.AddMinutes(-intervaloSync))
                return;

            try
            {
                var dtStart = DateTime.Now;

                SincronismoInfo = API.StatusSync(AC.Chave).Result;
                if (SincronismoInfo == null)
                {
                    AddLog($"{titulo} Sem resposta do BigData");
                    return;
                }
                else if (SincronismoInfo.Mensagem != "OK")
                {
                    AddLog($"{titulo} {SincronismoInfo.Mensagem ?? "???"}");
                    if (SincronismoInfo.Mensagem?.StartsWith("OK") != true)
                        return;
                }
                else if (SincronismoInfo.UltimoSincronismo == null)
                    AddLog($"{titulo} Sync Inicial ");
                else if (lastServerSync != SincronismoInfo.UltimoSincronismo.Value)
                {
                    lastServerSync = SincronismoInfo.UltimoSincronismo.Value;
                    AddLog($"{titulo} Sync " + SincronismoInfo.UltimoSincronismo.Value.ToString("dd/MM/yyyy HH:mm:ss"));
                }

                int u = UsuariosSync();

                int p = ProdutosSync();

                int e = EstoqueSync();

                int c = ClientesSync();

                int t = TipoPagamentosSync();

                int v = PedidosSync();

                if (u + p + e + c + t + v > 0)
                {
                    var result = API.EndSync(AC.Chave, dtStart).Result;
                    AddLog($"{titulo} Sync fim");
                }

                IntegradorCheckSync = DateTime.Now;
            }
            catch (Exception ex)
            {
                // Quando houver erro de sincronimo para por 4 horas!
                IntegradorCheckSync = DateTime.Now.AddHours(4);
                throw new Exception($"{titulo} ERRO", ex);
            }
        }

        private int ProdutosSync()
        {
            var idTipoServico = (int)ETipoProduto.Servico;
            var idTipoCredito = (int)ETipoProduto.Credito;

            var produtos = EF.Repositorio.Listar<tbProduto>(
                p => p.Ativo == true
                 && (p.DtUltimaAlteracao > SincronismoInfo.Produto || !SincronismoInfo.Produto.HasValue)
                 && p.IDTipoProduto != idTipoServico
                 && p.IDTipoProduto != idTipoCredito)
                .OrderBy(p => p.DtUltimaAlteracao)
                .Select(p => p.ToBigData())
                .ToArray();

            if (produtos.Length == 0) return 0;

            var result = API.UploadProduto(AC.Chave, produtos).Result;
            AddLog($"{titulo} Produtos atualizados {produtos.Length - result} novos {result}");

            return 0;
        }

        private int EstoqueSync()
        {
            var estoqueAtual = EntradaSaida.EstoqueInventario(DateTime.Now, null, false);

            if (estoqueAtual.Rows.Count == 0) return 0;

            var estoque = new List<bdEstoque>();
            foreach (DataRow row in estoqueAtual.Rows)
            {
                estoque.Add(new bdEstoque()
                {
                    IDProduto = row.Field<int>("IDProduto"),
                    EstoqueAtual = row.Field<decimal>("Quantidade")
                });
            }

            var result = API.UploadEstoque(AC.Chave, estoque.ToArray()).Result;
            if (result > 0)
                AddLog($"{titulo} Estoque atualizados " + result);

            return result;

        }

        private int ClientesSync()
        {
            var clientes = EF.Repositorio.Listar<tbCliente>(
                c => c.DtInclusao > SincronismoInfo.Cliente || !SincronismoInfo.Cliente.HasValue)
                .OrderBy(p => p.DtInclusao)
                .Select(p => p.ToBigData());

            if (!clientes.Any()) return 0;

            int result = 0;
            int qtd = clientes.Count();
            int novos;
            for (int i = 0; i < qtd; i += 100)
            {
                var up = clientes
                    .Skip(i)
                    .Take(100)
                    .ToArray();

                novos = API.UploadCliente(AC.Chave, up).Result;
                result += novos;
                AddLog($"{titulo} Cliente atualizados {i + up.Length}/{qtd} novos {novos}");
            }

            return result;
        }

        private int TipoPagamentosSync()
        {
            var tipoPagamentos = EF.Repositorio.Listar<tbTipoPagamento>(
                tp => tp.IDTipoPagamento > SincronismoInfo.TipoPagamento || !SincronismoInfo.TipoPagamento.HasValue)
                .OrderBy(p => p.IDTipoPagamento)
                .Select(p => p.ToBigData())
                .ToArray();

            if (!tipoPagamentos.Any()) return 0;


            var result = API.UploadTipoPagamento(AC.Chave, tipoPagamentos).Result;
            AddLog($"{titulo} Tipos de Pagamento atualizados {tipoPagamentos.Length - result} novos {result}");

            return result;
        }

        private int UsuariosSync()
        {
            var usuarios = EF.Repositorio.Listar<tbUsuario>(
                u => u.DtUltimaAlteracao > SincronismoInfo.Usuario || !SincronismoInfo.Usuario.HasValue)
                .OrderBy(p => p.DtUltimaAlteracao)
                .Select(p => p.ToBigData())
                .ToArray();

            if (!usuarios.Any()) return 0;

            var result = API.UploadUsuario(AC.Chave, usuarios).Result;
            AddLog($"{titulo} Usuários atualizados {usuarios.Length - result} novos {result}");

            return result;
        }

        private int PedidosSync()
        {
            var pedidos = EF.Repositorio.ListarConfig<tbPedido>(
                tb => tb.Include(nameof(tbPedido.tbPedidoProdutoes))
                        .Include(nameof(tbPedido.tbPedidoPagamentoes)),
                p => p.IDStatusPedido == 40
                 && (p.DtPedidoFechamento > SincronismoInfo.Pedido || !SincronismoInfo.Pedido.HasValue))
                .OrderBy(p => p.DtPedidoFechamento)
                .Select(p => p.ToBigData());

            if (!pedidos.Any()) return 0;

            int result = 0;
            int qtd = pedidos.Count();
            for (int i = 0; i < qtd; i += 50)
            {
                var up = pedidos
                    .Skip(i)
                    .Take(50)
                    .ToArray();

                result += API.UploadPedido(AC.Chave, up).Result;
                AddLog($"{titulo} Pedido {i + result}/{qtd}");
            }

            return result;
        }
    }
}
