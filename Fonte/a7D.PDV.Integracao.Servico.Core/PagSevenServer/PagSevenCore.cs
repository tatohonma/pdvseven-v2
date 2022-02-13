using a7D.PDV.BLL;
using a7D.PDV.Integracao.Servico.Core.PagSevenServer.Models;
using a7D.PDV.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.PagSevenServer
{
    public class PagSevenCore
    {
        ConfiguracoesServicoIntegracoes config;
        HttpClient httpClient = new HttpClient();
        Uri baseUrl;
        ConfiguracaoBDInformation configHabilitaPagSeven;
        ConfiguracaoBDInformation configDtUltimaConsulta;


        public PagSevenCore()
        {
            baseUrl = new Uri(ConfigurationManager.AppSettings["EnderecoAPI2"].ToString());
            config = Configuracoes();
            if (config.HabilitaPagSeven == "1")
            {

                configDtUltimaConsulta = ConfiguracaoBD.ListarConfiguracoes().Where(c => c.Chave == nameof(config.dtUltimaConsultaPagSeven)).FirstOrDefault();
            }
        }

        public async Task<List<Consulta>> ObterConsultas(int idEstabelecimento, DateTime dataMinima)
        {
            var listaConsultas = new List<Consulta>();
            var pagSevenServerAPI = new PagSevenServerAPI();
            var response = await pagSevenServerAPI.GetConsultas(idEstabelecimento, dataMinima);
            var consultasString = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro na conexão com o PagSeven Server: {response.StatusCode} - {response.ReasonPhrase} - {consultasString}");
            }
            
            listaConsultas.AddRange(JsonConvert.DeserializeObject<List<Consulta>>(consultasString));
            return listaConsultas;
        }

        public async Task<List<Models.Pagamento>> ObterPagamentosPendentes(int idEstabelecimento, DateTime dataMinima)
        {
            var listaPagamentos = new List<Models.Pagamento>();
            var pagSevenServerAPI = new PagSevenServerAPI();
            var response = await pagSevenServerAPI.GetPagamentosPendentes(idEstabelecimento, dataMinima);
            var pagamentosRaw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro na conexão com o PagSeven Server: {response.StatusCode} - {response.ReasonPhrase} - {pagamentosRaw}");
            }
            listaPagamentos.AddRange(JsonConvert.DeserializeObject<List<Models.Pagamento>>(pagamentosRaw));
            return listaPagamentos;
        }

        public async Task<List<Models.Pagamento>> ObterPagamentosAutorizadosOperadora(int idEstabelecimento, DateTime dataMinima)
        {
            var listaPagamentos = new List<Models.Pagamento>();
            var pagSevenServerAPI = new PagSevenServerAPI();
            var response = await pagSevenServerAPI.GetPagamentosAutorizados(idEstabelecimento, dataMinima);
            var pagamentosRaw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro na conexão com o PagSeven Server: {response.StatusCode} - {response.ReasonPhrase} - {pagamentosRaw}");

            }
            listaPagamentos.AddRange(JsonConvert.DeserializeObject<List<Models.Pagamento>>(pagamentosRaw));
            return listaPagamentos;
        }

        public static void EnviarPedido(dynamic modelPedidoPag7)
        {
            HttpResponseMessage result = new PagSevenServerAPI().EnviarPedido(modelPedidoPag7);
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Não foi possível enviar o pedido {modelPedidoPag7.IDPedido} para o PagSeven: {result.StatusCode} - {result.ReasonPhrase} - {result.Content.ReadAsStringAsync().Result}");
            }
        }


        public static ConfiguracoesServicoIntegracoes Configuracoes()
        {
            return new ConfiguracoesServicoIntegracoes();
        }

        public static bool FecharPedido(int idEstabelecimento, int idPedido)
        {
            var result = new PagSevenServerAPI().FecharPedido(idEstabelecimento, idPedido);
            //return result.IsSuccessStatusCode;
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Não foi possível fechar o pedido {idPedido} no PagSeven: {result.StatusCode} {result.ReasonPhrase}");
            }
            return true;
        }
    }
}
