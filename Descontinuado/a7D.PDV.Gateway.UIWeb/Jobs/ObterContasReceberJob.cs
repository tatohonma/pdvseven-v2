using a7D.PDV.Gateway.UIWeb.Context;
using a7D.PDV.Gateway.UIWeb.Models;
using a7D.PDV.Gateway.UIWeb.Repository;
using Newtonsoft.Json.Linq;
using NLog;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace a7D.PDV.Gateway.UIWeb.Jobs
{
    [DisallowConcurrentExecution]
    public class ObterContasReceberJob : IJob
    {
        private static readonly Uri _pesquisarContasReceberUri = new Uri("https://api.tiny.com.br/api2/contas.receber.pesquisa.php");
        private static readonly Uri _obterContaReceberUri = new Uri("https://api.tiny.com.br/api2/conta.receber.obter.php");
        private static readonly Uri _pesquisarContatosUri = new Uri("https://api.tiny.com.br/api2/contatos.pesquisa.php");
        private static readonly Uri _obterContatoUri = new Uri("https://api.tiny.com.br/api2/contato.obter.php");
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Dictionary<string, Cliente> clientesCache;
        //private static readonly string _token = "b299bdd5034792760d695f96b3c9237f2fa53082"; // Tiny Teste
        private static readonly string _token = "1bc473367c23f8b988b46c83767ff1a77c75ddd7";
        public void Execute(IJobExecutionContext context)
        {
            clientesCache = new Dictionary<string, Cliente>();

            using (var repo = new ContaReceberRepository(new GatewayContext()))
            {
                logger.Info("Obtendo contas a receber do Tiny - INICIO");
                var contas = repo.Listar().ToList();
                logger.Info($"{contas.Count} contas a receber no banco de dados");

                var contasRecebidas = ObterContasReceber();
                logger.Info($"{contasRecebidas.Count} contas a receber foram recebidas do Tiny");

                foreach (var conta in contasRecebidas)
                {
                    try
                    {
                        logger.Debug($"Obtendo detalhes do cliente \"{conta.nome_cliente.Value}\"");
                        Cliente cliente = ObterCliente(conta.nome_cliente.Value);
                        if (cliente == null)
                            continue;

                        logger.Debug($"Obtendo detalhes da conta a receber com ID Tiny {conta.id.Value}");
                        var contaDetalhada = ObterDetalhesConta(conta);

                        logger.Debug("-".PadRight(10));
                        logger.Debug($"{contaDetalhada.ToString()}");
                        logger.Debug("-".PadRight(10));

                        var cr = contas.FirstOrDefault(c => c.IdBroker == conta.id.Value);
                        if (cr == null)
                        {
                            logger.Debug($"Conta NÃO EXISTE no banco de dados, adicionando...");
                            cr = repo.Adicionar(new ContaReceber { Pendente = true });
                        }
                        else
                        {
                            logger.Debug($"Conta EXISTE no banco de dados");
                            cr.Pendente = true;
                            contas.RemoveAt(contas.FindIndex(c => c.IdBroker == cr.IdBroker));
                        }

                        cr.Data = DateTime.ParseExact(conta.data_emissao.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        cr.Vencimento = DateTime.ParseExact(conta.data_vencimento.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        cr.IdCliente = cliente.IdCliente;
                        cr.Historico = conta.historico.Value;
                        cr.IdBroker = conta.id.Value;
                        cr.Saldo = Convert.ToDecimal(conta.saldo.Value, CultureInfo.InvariantCulture);
                        cr.Valor = Convert.ToDecimal(conta.valor.Value, CultureInfo.InvariantCulture);

                        if (contaDetalhada != null && contaDetalhada.categoria != null)
                        {
                            cr.Categoria = contaDetalhada.categoria.Value;
                        }
                        logger.Debug($"UPSERT:");
                        logger.Debug(cr.ToString());
                        repo.SalvarAlteracoes();
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Ocorreu um erro na execução do JOB:");
                        logger.Error(ex);
                        continue;
                    }
                }

                logger.Info($"Atualizando o status de {contas.Count} contas com \"Pendente\"=\"false\"");
                foreach (var c in contas)
                {
                    logger.Debug($"Conta a receber com ID {c.IdContaReceber}; \"Pendente\"=\"false\"");
                    c.Pendente = false;
                }
                try
                {
                    logger.Info("Salvando alterações...");
                    repo.SalvarAlteracoes();
                }
                catch (Exception ex)
                {
                    logger.Error("Ocorrou um erro ao salvar alterações");
                    logger.Error(ex);
                }
            }
            clientesCache = null;
            logger.Info("Obtendo contas a receber do Tiny - FIM");
        }

        private dynamic ObterDetalhesConta(dynamic conta)
        {
            using (var obterContaClient = new HttpClient())
            {
                obterContaClient.BaseAddress = _obterContaReceberUri;
                var informacoesPesquisa = InformacoesPadrao;
                informacoesPesquisa.Add("id", conta.id.Value);

                logger.Info($"Solicitando ao Tiny informações sobre a conta com id {conta.id.Value}");

                var response = obterContaClient.PostAsync(string.Empty, new FormUrlEncodedContent(informacoesPesquisa)).Result;
                if (response.IsSuccessStatusCode)
                {
                    dynamic resp = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    if (resp.retorno.status_processamento != 3)
                    {
                        if (resp.retorno.codigo_erro == "6")
                        {
                            logger.Info("API do Tiny sobrecarregada, aguardando 20s para tentar novamente...");
                            Thread.Sleep(20000);
                            return ObterDetalhesConta(conta);
                        }
                        else
                        {
                            logger.Error("Ocorreu um erro na solicitação");
                            logger.Error("-".PadRight(10));
                            logger.Error(resp.ToString());
                            logger.Error("-".PadRight(10));
                            return null;
                        }
                    }
                    return resp.retorno.conta;
                }
                else
                {
                    var errorResp = response.Content.ReadAsStringAsync().Result;
                    logger.Error("Ocorreu um erro ao se comunicar com o Tiny");
                    logger.Error("-".PadRight(10));
                    logger.Error(errorResp);
                    logger.Error("-".PadRight(10));
                }
                return null;
            }
        }

        private IList<dynamic> ObterContasReceber(string situacao = "aberto")
        {
            var contas = new List<dynamic>();
            var final = 1;
            var atual = 1;
            logger.Info($"Obtendo contas a receber do Tiny com situacao \"{situacao}\"");
            do
            {
                using (var pesquisarContasClient = new HttpClient())
                {
                    pesquisarContasClient.BaseAddress = _pesquisarContasReceberUri;
                    var informacoesPesquisa = InformacoesPadrao;
                    informacoesPesquisa.Add("data_fim_vencimento", DateTime.Now.AddDays(180).ToString("dd/MM/yyyy"));
                    informacoesPesquisa.Add("situacao", situacao);
                    informacoesPesquisa.Add("pagina", atual.ToString());
                    var response = pesquisarContasClient.PostAsync(string.Empty, new FormUrlEncodedContent(informacoesPesquisa)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic resp = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                        logger.Debug(resp.ToString());
                        if (resp.retorno.status_processamento != 3)
                        {
                            if (resp.retorno.codigo_erro == "6")
                            {
                                logger.Info("API do Tiny sobrecarregada, aguardando 20s para tentar novamente...");
                                Thread.Sleep(20000);
                                continue;
                            }
                            else if (resp.retorno.codigo_erro == "20")
                            {
                                return contas;
                            }
                            else
                            {
                                logger.Error("Ocorreu um erro na solicitação");
                                logger.Error("-".PadRight(10));
                                logger.Error(resp.ToString());
                                logger.Error("-".PadRight(10));
                                throw new Exception(resp.retorno.erros.erro);
                            }
                        }

                        foreach (var conta in resp.retorno.contas)
                            contas.Add(conta.conta);

                        final = resp.retorno.numero_paginas;
                        if (final > 1)
                            atual++;
                    }
                }
            } while (atual < final);
            if (situacao == "aberto")
                contas.AddRange(ObterContasReceber("parcial"));
            return contas;
        }

        private Cliente ObterCliente(string nome_cliente)
        {
            logger.Info($"Obtendo detalhes do Tiny detalhes do cliente \"{nome_cliente}\"");
            if (clientesCache.ContainsKey(nome_cliente))
            {
                logger.Info("Retornando do cache");
                return clientesCache[nome_cliente];
            }

            using (var pesquisarContatosClient = new HttpClient())
            using (var obterContatoClient = new HttpClient())
            using (var repo = new ClienteRepository(new GatewayContext()))
            {
                try
                {
                    pesquisarContatosClient.BaseAddress = _pesquisarContatosUri;
                    obterContatoClient.BaseAddress = _obterContatoUri;

                    var informacoesPesquisa = InformacoesPadrao;
                    informacoesPesquisa.Add("pesquisa", nome_cliente);
                    var response = pesquisarContatosClient.PostAsync(string.Empty, new FormUrlEncodedContent(informacoesPesquisa)).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        dynamic resp = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                        logger.Debug(resp.ToString());
                        if (resp.retorno.status_processamento != 3)
                        {
                            if (resp.retorno.codigo_erro == "6")
                            {
                                logger.Info("API do Tiny sobrecarregada, aguardando 20s para tentar novamente...");
                                Thread.Sleep(20000);
                                return ObterCliente(nome_cliente);
                            }
                            else
                            {
                                logger.Error("Ocorreu um erro na solicitação");
                                logger.Error("-".PadRight(10));
                                logger.Error(resp.ToString());
                                logger.Error("-".PadRight(10));
                                return null;
                            }
                        }
                        var contato = resp.retorno.contatos[0];
                        if (contato == null)
                        {
                            logger.Info($"Nenhum cliente com nome \"{nome_cliente}\" foi encontrado no Tiny");
                            return null;
                        }
                        contato = contato.contato;
                        var c = repo.EncontrarOuInserir(contato.id.Value as string, contato.nome.Value as string);
                        clientesCache.Add(nome_cliente, c);
                        return c;
                    }
                    else
                    {
                        var errorResp = response.Content.ReadAsStringAsync().Result;
                        logger.Error("Ocorreu um erro ao se comunicar com o Tiny");
                        logger.Error("-".PadRight(10));
                        logger.Error(errorResp);
                        logger.Error("-".PadRight(10));
                        return null;
                    }
                }
                finally
                {
                    repo.SalvarAlteracoes();
                }
            }

        }

        private Dictionary<string, string> InformacoesPadrao => new Dictionary<string, string>() { { "token", _token }, { "formato", "json" } };
    }
}