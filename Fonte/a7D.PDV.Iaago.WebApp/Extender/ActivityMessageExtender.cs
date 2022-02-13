using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using a7D.PDV.Iaago.Dialogo;
using a7D.PDV.Iaago.WebApp.ChatBot;
using a7D.PDV.Iaago.WebApp.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace a7D.PDV.Iaago.WebApp.Extender
{
    internal static class ActivityMessageExtender
    {
        internal static IaagoVars NewLogin(this IaagoBot bot, Activity activity)
        {
            var novo = new IaagoVars();
            novo.Add("_ChannelId", activity.ChannelId, false);
            novo.Add("_fromId", activity.From.Id, false);
            novo.Add("_count", 0, false);
            novo.Add(BotServices.AmbienteDefaultKey, BotServices.AmbienteDefaultValue, false);

            try
            {
                if (bot.Context == null)
                {
                    novo.Add("_userId", 0, false);
                    return novo;
                }

                var user = LoadOrInsert(bot.Context, activity);
                novo.Add("_userId", user.ID, false);
                var variaveis = bot.Context.Variaveis
                    .Where(v => v.IDUsuario == user.ID)
                    .OrderBy(v => v.DataGravacao);

                foreach (var variavel in variaveis)
                {
                    novo.Add(variavel.Chave, variavel.Valor, true, variavel.DataGravacao);
                }
            }
            catch (Exception ex)
            {
                novo.Add("_userId", -1, false);
                novo.Add("_userErro", ex.Message, false);
            }

            return novo;
        }

        private static Usuario LoadOrInsert(IaagoContext context, Activity activity)
        {
            var user = context.Usuarios
                    .FirstOrDefault(u => u.ChannelId == activity.ChannelId
                                 && u.FromId == activity.From.Id);
            if (user == null)
            {
                user = new Usuario()
                {
                    ChannelId = activity.ChannelId,
                    FromId = activity.From.Id,
                    Name = activity.From.Name ?? "?",
                    DataCriacao = DateTime.Now,
                    UltimoAcesso = DateTime.Now,
                };
                context.Usuarios.Add(user);
            }
            else
            {
                user.UltimoAcesso = DateTime.Now;
            }

            context.SaveChanges();
            return user;
        }

        internal static async Task<string> MessageRenponseAsync(this IaagoBot bot, DialogContext dc, CancellationToken cancel)
        {
            // Obtem a pergunta (texto) se não exister pode ser um arquivo enviado
            string text = dc.Context.Activity.Text?.Trim();

        retry:
            var userIaago = bot.UserIaago;

            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            else if (text.Contains("#"))
            {
                var results = await bot.MessageComandoAsync(dc, cancel);
                if (results != "#")
                {
                    return results;
                }
            }

            // Verifica se usuário enviou um comando de cancelamento ou pedindo ajuda
            var interrupted = await dc.IsTurnInterruptedAsync(text);
            if (interrupted || cancel.IsCancellationRequested)
            {
                return null;
            }

            // Continue o processamento do dialogo
            var dr = await dc.ContinueDialogAsync();
            bot.AddInfo("DialogStatus: " + dr.Status);

            switch (dr.Status)
            {
                case DialogTurnStatus.Empty:

                    // Se não estiver em algum dialogo busca intenções ou comandos
                    MyLuisResult ir = await bot.BuscaIntencaoLuiz(dc.Context, cancel);
                    if (ir == null || string.IsNullOrEmpty(ir.Intencao))
                    {
                        return "Não entendi sua pergunta\ndigite **ajuda** que posso sugerir algo";
                    }
                    else if (ir.Intencao.Contains("ERRO ") == true)
                    {
                        return "Ops, o Luis está comproblemas:\r\n" + ir.Intencao;
                    }
                    else if (bot.Fluxo.TemIntencao(ir.Intencao))
                    {
                        userIaago.Intencao = ir.Intencao;
                        userIaago.IgnorarMensagemIntencao = null;
                        userIaago.RetornoIntencao = null;
                        object valor = Interpretador.SimpleVar(ir.Valor);
                        object valor2 = null;

                        if (ir.Entidade == "datetime")
                        {
                            if (valor is DateTime dt2)
                            {
                                if (ir.Valor.Contains("XXXX-"))
                                {
                                    valor2 = dt2.AddMonths(1);
                                }
                                else
                                {
                                    valor2 = dt2.AddDays(1);
                                }
                            }
                            else if (valor is int ano)
                            {
                                valor = new DateTime(ano, 1, 1);
                                valor2 = new DateTime(ano + 1, 1, 1);
                            }
                        }
                        else if (valor is string str)
                        {
                            // Remove plurais
                            if (str.Length > 3 && str.EndsWith("s"))
                            {
                                valor = str.Substring(0, str.Length - 1);
                            }
                        }

                        var intencao = new DictionaryObject
                        {
                            { "luis", ir.Key },
                            { "score", ir.Score },
                            { "valor", valor },
                            { "valor2", valor2 },
                        };

                        userIaago.Add("@intencao", intencao, false);

                        await dc.BeginDialogAsync(nameof(IntencaoDialog));
                        return null;
                    }
                    else
                    {
                        string info = "Ainda não tenho dialogo para a intenção: " + ir.Intencao;
                        if (ir.Entidade != null)
                        {
                            info += $" Entidade: {ir.Valor}";
                        }

                        return info;
                    }

                case DialogTurnStatus.Waiting:
                    // dc.Context.Responded
                    return null;

                case DialogTurnStatus.Complete:
                    await dc.EndDialogAsync();
                    text = userIaago["texto_anterior"]?.ToString();
                    if (!string.IsNullOrEmpty(text))
                    {
                        userIaago.Remove("texto_anterior");
                        goto retry; // Não é bonito, mas por enquanto fica assim...
                    }

                    return null;

                default:
                    await dc.CancelAllDialogsAsync();
                    return null;
            }
        }

        internal static async Task<string> MessageQnAAsync(this IaagoBot bot, DialogContext dc, CancellationToken cancel)
        {
            var response = await bot.Services.QnAServices["xxxx"].GetAnswersAsync(dc.Context);
            return null;
        }
    }
}
