using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using a7D.PDV.Ativacao.Shared.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using static a7D.PDV.BLL.PDV;

namespace a7D.PDV.BLL.Entity
{
    public class Licencas : IDisposable
    {
        private static readonly TimeZoneInfo _brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        private static readonly CultureInfo _cultureInfo = new CultureInfo("pt-BR");
        private const string _urlAtivacao = "http://apipdvseven.azurewebsites.net/";
        private ConfiguracoesAtivacao config = new ConfiguracoesAtivacao();
        public static bool AlertaVersao = false;

        public Licencas()
        {
            AC.RegitraLicenca(config.ChaveAtivacao);
        }

        public string ChaveAtivacao => config.ChaveAtivacao;

        public void Validar(TipoApp tipoApp)
        {
            VerificaDataCorreta();

            Exception exSync = null;
            try
            {
                string[] assemble = System.Reflection.Assembly.GetCallingAssembly().FullName.Split(',');
                string versao = assemble[1].Split('=')[1];

                Sincronizar(tipoApp, versao, tipoApp == TipoApp.SERVER);

                if (tipoApp == TipoApp.SERVER)
                    ConfiguracaoBD.DefinirValorPadrao(EConfig._VersaoServidor, versao);
                else if (!AlertaVersao && !string.IsNullOrEmpty(config._VersaoServidor) && config._VersaoServidor != versao)
                {
                    AlertaVersao = true;
                    throw new ExceptionPDV(CodigoErro.A6F9);
                }
            }
            catch (Exception ex)
            {
                exSync = ex;
            }

            if (exSync is ExceptionPDV exPDV && exPDV.CodigoErro == CodigoErro.E077)
                throw exSync;

            config = new ConfiguracoesAtivacao();
            if (string.IsNullOrWhiteSpace(config.DtValidade))
                throw exSync ?? new ExceptionPDV(CodigoErro.F075);

            if (!string.IsNullOrEmpty(config.DtValidade))
            {
                var validade = Convert.ToDateTime(CryptMD5.Descriptografar(config.DtValidade));
                if (validade < DateTime.Now)
                    throw exSync ?? new ExceptionPDV(CodigoErro.F080);
            }

            if (tipoApp != TipoApp.SERVER && exSync != null)
                Logs.ErroBox(CodigoErro.A6F4, exSync);
        }

        private void VerificaDataCorreta()
        {
            var dtMax = DateTime.Now.AddMinutes(62); // Tolerancia de 2 minutos prevendo horario de de verão

            // Para evitar que o cliente volte a data do computador, burlando a licença
            if (!string.IsNullOrEmpty(config.DtUltimaVerificacao))
            {
                var dtUltimaVerificacaoStr = CryptMD5.Descriptografar(config.DtUltimaVerificacao);
                var dtUltimaVerificacao = Convert.ToDateTime(dtUltimaVerificacaoStr, _cultureInfo);
                if (dtUltimaVerificacao >= dtMax)
                    throw new ExceptionPDV(CodigoErro.E105);
            }
        }

        public void VerificaDataPedidos()
        {
            var dtMax = DateTime.Now.AddMinutes(62); // Tolerancia de 2 minutos prevendo horario de de verão

            // Se tiver qualquer outro pedido com data superior algo errado pode ter ocorrido: Windows reconfigurou o horario incorretamente por exemplo
            var qtd = EF.Repositorio.Contar<EF.Models.tbPedido>(p => p.DtPedido > dtMax);
            if (qtd > 0)
                Logs.ErroBox(new ExceptionPDV(CodigoErro.E106));
        }

        public bool Sincronizar(TipoApp tipoApp, string versao, bool atualizar)
        {
            DateTime? validade = null;
            try
            {
                var jwt = JWT.Obter(tipoApp);
                validade = ReceberDadosServidor(_urlAtivacao, jwt, config.ChaveAtivacao, versao);
                if (atualizar)
                {
                    EnviarDadosServidor(_urlAtivacao, jwt);
                    ConfiguracaoBD.AlterarConfiguracaoSistema("dtUltimaVerificacao", CryptMD5.Criptografar(DateTime.Now.ToString(_cultureInfo)));
                }
            }
            catch (AggregateException ex)
            {
                if (!ex.InnerExceptions.Where(e => e.GetType() == typeof(HttpRequestException)).Any())
                    throw ex;

                return false;
            }
            catch (ExceptionPDV exPDV)
            {
                if (exPDV.CodigoErro == CodigoErro.E077)
                    throw exPDV;
                else if (exPDV.CodigoErro == CodigoErro.E2BC) // Não encontrado
                    return false;
                else
                    Logs.Erro(exPDV);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.E071, ex);
            }
            finally
            {
                if (validade.HasValue)
                    AlterarDataValidade(validade.Value);
            }
            return true;
        }

        private DateTime? ReceberDadosServidor(string endereco, string jwt, string chaveAtivacao, string versao)
        {
            DateTime? validade = null;
            if (string.IsNullOrWhiteSpace(chaveAtivacao))
                throw new ExceptionPDV(CodigoErro.E060);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(endereco);
                client.DefaultRequestHeaders.Add("x-auth-token", jwt);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("/api/validade/" + chaveAtivacao + "?versao=" + versao).Result;

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        throw new ExceptionPDV(CodigoErro.E2BC);

                    var jobject = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    {
                        string msg = jobject.Value<string>("Message");
                        if (msg.Contains("desativada"))
                            throw new ExceptionPDV(CodigoErro.E077, msg);
                        else
                            throw new ExceptionPDV(CodigoErro.E070, msg);
                    }
                }
                else
                {
                    var jsonDatas = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    validade = jsonDatas.Value<DateTime>("Validade");
                    validade = TimeZoneInfo.ConvertTimeFromUtc(validade.Value, _brasiliaTimeZone);
                }

                response = client.GetAsync("/api/licencas/" + chaveAtivacao).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var jobject = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    throw new ExceptionPDV(CodigoErro.F080, jobject.Value<string>("Message"));
                }
                else
                {
                    var jsonLicencas = response.Content.ReadAsStringAsync().Result;
                    var jsonO = JObject.Parse(jsonLicencas);
                    var jsonA = (JArray)jsonO.SelectToken("pdvs");
                    var jsonID = jsonO.Value<int>("id");
                    var jsonCliente = jsonO.Value<string>("cliente");
                    AC.RegitraAtivacao(jsonID, jsonCliente);
                    var listaAremover = EF.Repositorio.Listar<PDVInformation>().Select(p => p.IDPDV.Value).ToList();
                    foreach (var json in jsonA)
                    {
                        bool adicionar = false;
                        var idPdv = json.Value<int?>("IDPDV_instalacao");
                        if (!idPdv.HasValue)
                            continue;

                        var pdv = idPdv.HasValue ? PDV.Carregar(idPdv.Value) : null;
                        if (pdv == null)
                        {
                            adicionar = true;
                            pdv = new PDVInformation
                            {
                                IDPDV = idPdv,
                                Nome = json.Value<string>("Nome"),
                                TipoPDV = new TipoPDVInformation { IDTipoPDV = json.Value<int>("IDTipoPDV") }
                            };
                        }
                        else
                            listaAremover.Remove(pdv.IDPDV.Value); // tudo certo (não deve ser removido

                        pdv.TipoPDV = new TipoPDVInformation { IDTipoPDV = json.Value<int>("IDTipoPDV") };
                        var dataRecebida = json.Value<DateTime?>("DtUltimaAlteracao");
                        if (dataRecebida != null)
                        {
                            dataRecebida = TimeZoneInfo.ConvertTimeFromUtc(dataRecebida.Value, _brasiliaTimeZone);
                            if (pdv.UltimaAlteracao == null || pdv.UltimaAlteracao < dataRecebida)
                            {
                                pdv.UltimaAlteracao = dataRecebida;
                                pdv.Nome = json.Value<string>("Nome");
                            }
                        }

                        pdv.Ativo = json.Value<bool>("Ativo");
                        pdv.AtualizarDados();

                        if (adicionar)
                            EF.Repositorio.Inserir(pdv);
                        else
                            EF.Repositorio.Atualizar(pdv);

                        //pdv7Context.DB.tbPDVs.Add(pdv);
                    }
                    //pdv7Context.DB.SaveChanges();

                    // Tenta apaga os antigos
                    foreach (var idLicenca in listaAremover)
                    {
                        try
                        {
                            var pdvDel = EF.Repositorio.Carregar<PDVInformation>(p => p.IDPDV == idLicenca);
                            EF.Repositorio.Excluir(pdvDel);
                            //var pdvDel = pdv7Context.DB.tbPDVs.Find(idLicenca);
                            //pdv7Context.DB.tbPDVs.Remove(pdvDel);
                            //pdv7Context.DB.SaveChanges();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            return validade;
        }

        private void EnviarDadosServidor(string endereco, string jwt)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(endereco);
                client.DefaultRequestHeaders.Add("x-auth-token", jwt);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonEnvio();
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync("/api/licencas", content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        throw new ExceptionPDV(CodigoErro.E2BC);

                    dynamic jobject = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                    var ex = new ExceptionPDV(CodigoErro.E050, jobject?.Message?.Value as string);
                    ex.Data.Add("json", json);
                    throw ex;
                }
            }
        }

        public PDVInformation Carregar(ETipoPDV TipoPdv, string chaveHardware, string nome = null)
        {
            var listaTodos = PDV.Listar();
            var listaTipo = listaTodos.Where(p => p.TipoPDV.Tipo == TipoPdv).ToList();
            var pdvsHard = listaTipo.Where(p => p.ChaveHardware == chaveHardware);
            if (pdvsHard.Count() > 1)
                throw new ExceptionPDV(CodigoErro.F08D, chaveHardware);

            var pdv = pdvsHard.FirstOrDefault();
            if (pdv == null)
            {
                var listaTipoAtivo = listaTipo.Where(p => p.Ativo == true).ToList();
                pdv = listaTipoAtivo.Where(p => string.IsNullOrWhiteSpace(p.ChaveHardware)).OrderBy(p => p.Nome).FirstOrDefault();

                if (pdv == null)
                {
                    var ex = new ExceptionPDV(CodigoErro.E110, TipoPdv.ToString() + " H:" + chaveHardware);
                    ex.Data.Add(TipoPdv.ToString() + " inativas", listaTipo.Count(p => p.Ativo == false));
                    foreach (var p in listaTipoAtivo)
                        ex.Data.Add("IDPDV: " + p.IDPDV, p.Nome + " H: " + p.ChaveHardware);

                    throw ex;
                }
                else
                {
                    pdv.ChaveHardware = chaveHardware;
                }
            }
            else if (pdv.Ativo == false)
                throw new ExceptionPDV(CodigoErro.F081, chaveHardware);

            string[] assemble = System.Reflection.Assembly.GetCallingAssembly().FullName.Split(',');
            string exeDll = assemble[0];

            pdv.UltimoAcesso = DateTime.Now;
            pdv.Versao = assemble[1].Split('=')[1];

            if (!string.IsNullOrEmpty(nome))
                pdv.Nome = nome;

            Salvar(pdv);

            AC.RegitraPDV(exeDll, pdv.Versao, pdv);

            return pdv;
        }

        public string JsonEnvio()
        {
            var result = new JArray();
            var pdvs = BLL.PDV.Listar();
            foreach (var pdv in pdvs)
            {
                dynamic obj = new JObject();

                obj.IdPdv = pdv.IDPDV.Value;
                obj.IDTipoPDV = pdv.TipoPDV.IDTipoPDV;
                obj.IDPDV_instalacao = pdv.IDPDV.Value;
                obj.ChaveHardware = pdv.ChaveHardware;
                obj.DtUltimaAlteracao = pdv.UltimaAlteracao.HasValue ? pdv.UltimaAlteracao.Value.ToUniversalTime() : null as DateTime?;
                obj.Versao = pdv.Versao;
                obj.ChaveAtivacao = config.ChaveAtivacao;
                obj.Nome = pdv.Nome;
                obj.Ativo = pdv.Ativo.HasValue ? pdv.Ativo.Value : false;

                result.Add(obj);
            }

            return result.ToString();
        }

        public void Dispose()
        {
        }
    }
}
