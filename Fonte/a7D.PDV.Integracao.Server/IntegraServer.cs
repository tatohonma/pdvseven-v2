using a7D.PDV.Ativacao.Shared.DTO;
using a7D.PDV.BLL;
using a7D.PDV.BLL.Entity;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Models;
using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Integracao.Servico.Core;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using a7D.PDV.Componentes.Services;
using Newtonsoft.Json.Linq;
using a7D.PDV.Shared.Atualizacao;

namespace a7D.PDV.Integracao.Server
{
    /// <summary>
    ///  Essa integração é responsável por tudo que for relacionado com o sistema: Backup, Mensagens, Atualização, UDP
    /// </summary>
    public class IntegraServer : IntegracaoTask, IShowInfo
    {
        public override string Nome => "Servidor PDV7";

        public const string okEtapa1 = "OK Etapa 1";
        public const string okEtapa3 = "OK Etapa 3";

        private volatile bool ProcessandoMensagem = false;
        private bool CheckBigData = false;

        public void ShowInfo(string info)
        {
            AddLog(info);
        }

        private DateTime dtLicSync = DateTime.MinValue;
        private UDPServer udp;

        public override void Executar()
        {
            Disponivel = false;
            Configurado = false;
            udp = new UDPServer
            {
                SQLServer = ConfigurationManager.AppSettings["SQLServer"],
                WS2Server = ConfigurationManager.AppSettings["WS2Server"],
            };
            udp.AddLog += AddLog;

            if (string.IsNullOrEmpty(udp.SQLServer) || string.IsNullOrEmpty(udp.WS2Server))
            {
                AddLog("Falta configurar o SQL e o WS2");
                return;
            }

            Configurado = true;
            Iniciar(() => Loop());
        }

        private void Loop()
        {
            try
            {
                if (udp.SQLServer == "." || udp.WS2Server.StartsWith("."))
                {
                    var ips = NetworUtil.GetAllIP(out string log);
                    AddLog("Identificando IP\r\n" + log);

                    if (udp.SQLServer == "." && ips.Length > 0)
                        udp.SQLServer = ips.FirstOrDefault(ip => ip.Contains(".77.")) ?? ips[0];

                    if (udp.WS2Server.StartsWith(".") && ips.Length > 0)
                        udp.WS2Server = (ips.FirstOrDefault(ip => ip.Contains(".77.")) ?? ips[0]) + ":7777";
                }

                AddLog($"SQL: {udp.SQLServer} WS2: {udp.WS2Server}");
                udp.StartListening();
                
                while (Executando)
                {
                    if (DateTime.Now.Subtract(dtLicSync).TotalHours > 1)
                    {
                        // A cada hora sincroniza as licenças
                        dtLicSync = DateTime.Now;
                        AddLog("Validando licenças");
                        using (var pdvServico = new Licencas())
                            pdvServico.Validar(TipoApp.SERVER);
                    }

                    if (!ProcessandoMensagem)
                        //Ativador.API.Mensagens(AC.Chave)
                        Ativador.API.SyncMensage(AC.Chave, EOrigemDestinoMensagem.Integrador, AC.Versao, "OK")
                            .ContinueWith(task => VerificaMensagens(task));

                    //if (CheckBigData)
                    //    BigData?.Sync();
                    //else
                    //{
                    //    CheckBigData = true;
                    //    if (BLL.PDV.PossuiBigData())
                    //    {
                    //        CheckBigData = true;
                    //        // Código 'secreto' para desabilitar o Sync
                    //        if (ConfiguracoesSistema.Valores.IntervaloBigData == -2606)
                    //        {
                    //            AddLog($"{BigDataServices.titulo} sincronismo desabilitado!!!");
                    //        }
                    //        else
                    //        {
                    //            BigData = new BigDataServices();
                    //            BigData.AddLog += AddLog;
                    //            BigData.IntervaloIntegracao = ConfiguracoesSistema.Valores.IntervaloBigData;
                    //            AddLog($"{BigDataServices.titulo} sincronimo a cada {BigData.IntervaloIntegracao} min");
                    //        }
                    //    }
                    //    else
                    //        BigData = null;
                    //}
                    
                    Sleep(60); // 1 Minuto
                }
            }
            catch (ExceptionPDV exPDV)
            {
                AddLog(exPDV);
                if (exPDV.CodigoErro == CodigoErro.E077)
                    RequestClose = true;
            }
            catch (Exception ex)
            {
                AddLog(ex);
            }
            finally
            {
                udp.Dispose();
            }
        }

        public static void ProcessaMensagens()
        {
            var msgs = MensagemServices.RetornaMensagens();
            if (!msgs.Any())
                return;

            MensagemServices.ExibeMensagens(msgs);

            // Envia mensagens sem resposta se houver
            int idInfo = (int)ETipoMensagem.Informacao;
            int idResposta = (int)ETipoMensagem.Resposta;
            var msgList = EF.Repositorio.Listar<tbMensagem>(m => m.IDTipo >= idInfo && m.IDTipo < idResposta && m.DataVisualizada.HasValue && m.IDRespostaProcesamento == 0);
            foreach (var msg in msgList)
            {
                try
                {
                    var msgRespota = new MensagemNova()
                    {
                        Chave = AC.Chave,
                        Tipo = ETipoMensagem.Resposta,
                        Origem = EOrigemDestinoMensagem.Integrador,
                        Destino = EOrigemDestinoMensagem.Ativador,
                        Texto = msg.ResultadoProcessamento + ": " + msg.Texto,
                        IdMensagemOrigem = msg.IDMensagem
                    };
                    msg.IDRespostaProcesamento = Ativador.API.Enviar(msgRespota).Result;
                }
                catch (Exception ex)
                {
                    msg.IDRespostaProcesamento = -1;
                    Logs.Erro(ex);
                }
                EF.Repositorio.Atualizar(msg);

                if (msg.Tipo == ETipoMensagem.Pergunta_Atualizar && msg.ResultadoProcessamento == "SIM")
                {
                    var msgUpdate = new tbMensagem()
                    {
                        Tipo = ETipoMensagem.Update,
                        Origem = EOrigemDestinoMensagem.Integrador,
                        Destino = EOrigemDestinoMensagem.Integrador,
                        Texto = msg.ResultadoProcessamento + ": " + msg.Texto,
                        Parametros = msg.Parametros,
                        IDMensagemOrigem = msg.IDMensagem
                    };
                    EF.Repositorio.Inserir(msgUpdate);
                }
            }
        }

        private void VerificaMensagens(Task<MensagemRecebida[]> task)
        {
            if (ProcessandoMensagem)
                return;

            ProcessandoMensagem = true;
            try
            {
                var mensagens = task.Result;
                if (mensagens.Length > 3)
                    throw new Exception("Mensagens não autorizadas");

                foreach (var msg in mensagens)
                {
                    var msgLocal = new tbMensagem(msg);
                    EF.Repositorio.Inserir(msgLocal);
                    AddLog($"(Nova Mensagem) {msgLocal.IDMensagem}: {msgLocal.Tipo} - {msgLocal.Texto}");
                }

                VerificaAtualizacao();
            }
            catch (ThreadAbortException)
            {
                AddLog("Parando...");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is Refit.ApiException rex && rex.Content.StartsWith("{"))
                {
                    try
                    {
                        var json = JObject.Parse(rex.Content);
                        if (json.ContainsKey("Message") && json.ContainsKey("ExceptionMessage"))
                        {
                            string Message = json.Value<string>("Message");
                            string ExceptionMessage = json.Value<string>("ExceptionMessage");
                            string StackTrace = json.Value<string>("StackTrace");
                            var ex2 = new ExceptionPDV(CodigoErro.E079, ExceptionMessage);
                            ex2.Data.Add("Message", Message);
                            ex2.Data.Add("StackTrace", StackTrace);
                            AddLog(ex2);
                            return;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                AddLog(ex);
            }
            finally
            {
                ProcessandoMensagem = false;
            }
        }

        private void VerificaAtualizacao()
        {
            try
            {
                int idTipo = (int)ETipoMensagem.Update;
                var msgList = EF.Repositorio.Listar<tbMensagem>(m => m.IDTipo == idTipo && !m.DataProcessamento.HasValue);

                if (!msgList.Any())
                {
                    var msgUpdateOK = EF.Repositorio.Carregar<tbMensagem>(m => m.IDTipo == idTipo && m.ResultadoProcessamento.StartsWith(okEtapa1));
                    if (msgUpdateOK != null)
                    {
                        try
                        {
                            var msgOK = new MensagemNova()
                            {
                                Chave = AC.Chave,
                                Tipo = ETipoMensagem.Update_ServerOK,
                                Origem = EOrigemDestinoMensagem.Integrador,
                                Destino = EOrigemDestinoMensagem.Ativador,
                                Texto = "Atualização etapa 3 concluída - Versão: " + AC.Versao,
                                Parametros = pdv7Context.LastLog,
                                IdMensagemOrigem = msgUpdateOK.IDMensagemOrigem
                            };
                            msgUpdateOK.DataVisualizada = DateTime.Now; // Registra o final do processo
                            msgUpdateOK.ResultadoProcessamento = okEtapa3;
                            msgUpdateOK.IDRespostaProcesamento = Ativador.API.Enviar(msgOK).Result;
                        }
                        catch (Exception ex2)
                        {
                            msgUpdateOK.ResultadoProcessamento = okEtapa3 + ", mas não foi possivel enviar resultado";
                            Logs.Erro(ex2);
                        }
                        EF.Repositorio.Atualizar(msgUpdateOK);
                    }
                    else if (!string.IsNullOrEmpty(pdv7Context.LastLog)) // Apenas se ocorreu algum migration
                    {
                        try
                        {
                            var msgOK = new MensagemNova()
                            {
                                Chave = AC.Chave,
                                Tipo = ETipoMensagem.Update_ServerOK,
                                Origem = EOrigemDestinoMensagem.Integrador,
                                Destino = EOrigemDestinoMensagem.Ativador,
                                Texto = "Migração SQL realizada: " + AC.Versao,
                                Parametros = pdv7Context.LastLog
                            };
                            pdv7Context.ClearLastLog();

                            if (msgOK.Parametros?.Length > 10000)
                                msgOK.Parametros = msgOK.Parametros.Substring(0, 10000) + "...";

                            var id = Ativador.API.Enviar(msgOK).Result;
                            AddLog($"Mensagem {id} com log da migração enviada");
                        }
                        catch (Exception ex2)
                        {
                            Logs.Erro(ex2);
                        }
                    }

                    Atualizacao.VerificaUpdateZip(AddLog);
                    Backup.LimpaAntigos(AddLog);
                    Backup.Verifica(AddLog);
                    return;
                }

                var msgUpdate = msgList.Last();
                if (msgList.Count > 1)
                {
                    msgList.RemoveAt(msgList.Count - 1);
                    for (int i = 0; i < msgList.Count; i++)
                    {
                        msgList[i].DataProcessamento = DateTime.Now;
                        msgList[i].ResultadoProcessamento = "ignorado por duplicidade";
                    }
                    EF.Repositorio.AtualizarLista(msgList);
                }

                AddLog("(Processando Mensagem) " + msgUpdate.ToString());
                msgUpdate.DataProcessamento = DateTime.Now; // Registra o inicio do processo

                var msgResult = new MensagemNova()
                {
                    Chave = AC.Chave,
                    Origem = EOrigemDestinoMensagem.Integrador,
                    Destino = EOrigemDestinoMensagem.Ativador,
                    Texto = msgUpdate.ResultadoProcessamento,
                    IdMensagemOrigem = msgUpdate.IDMensagemOrigem
                };

                try
                {
                    if (string.IsNullOrEmpty(msgUpdate.Parametros))
                        throw new Exception("Parametro com o link não informado");

                    // O código a seguir precisa ser finalizado nos proximos 12 segundos...
                    try
                    {
                        Backup.Verifica(AddLog, true);
                        Atualizacao.ApagaUpdateZip(AddLog);
                        Componentes.Atualizacao.IniciaAtualizacao(this, msgUpdate.Parametros, true);
                        msgUpdate.ResultadoProcessamento = okEtapa1;

                        msgResult.Tipo = ETipoMensagem.Update_ServerStart;
                        msgResult.Texto = "Atualização etapa 1 concluída (Reiniciando Integrador)";
                        msgUpdate.IDRespostaProcesamento = Ativador.API.Enviar(msgResult).Result;
                    }
                    catch (Exception ex2)
                    {
                        msgUpdate.ResultadoProcessamento = okEtapa1 + ", mas não foi possivel enviar resultado";
                        Logs.Erro(ex2);
                    }

                    EF.Repositorio.Atualizar(msgUpdate);
                    RequestClose = true; // Isso deve parar a aplicação de forma normal
                    //Thread.Sleep(7000);
                    //AddLog("Forçando finalização...");
                    //Application.Exit();
                }
                catch (ThreadAbortException)
                {
                    AddLog("Finalizando...");
                }
                catch (Exception ex)
                {
                    msgUpdate.ResultadoProcessamento = ex.Message;
                    try
                    {
                        // Toda mensagem inserida pelo ativador 'vai e volta' então não deve ser inserida local
                        msgResult.Tipo = ETipoMensagem.Update_Erro;
                        msgResult.Texto = msgUpdate.ResultadoProcessamento;
                        msgResult.Parametros = ex.StackTrace;

                        msgUpdate.IDRespostaProcesamento = Ativador.API.Enviar(msgResult).Result;
                    }
                    catch (Exception ex2)
                    {
                        Logs.Erro(ex2);
                    }

                    EF.Repositorio.Atualizar(msgUpdate);

                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ProcessandoMensagem = false;
            }
        }
    }
}