using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using a7D.PDV.Iaago.WebApp.Extender;
using a7D.PDV.Iaago.WebApp.Services;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace a7D.PDV.Iaago.WebApp.ChatBot
{
    internal static class ActivityMessageComandoExtender
    {
        internal static async Task<string> MessageComandoAsync(this IaagoBot bot, DialogContext dc, CancellationToken cancel)
        {
            var userIaago = bot.UserIaago;
            string cmd = dc.Context.Activity.Text?.Trim().ToLower();

            if (cmd.Contains("#fluxo"))
            {
                var i = cmd.IndexOf("#fluxo ");
                if (i >= 0)
                {
                    userIaago.Intencao = cmd.Substring(i + 7).Split(' ')[0];
                    userIaago.IgnorarMensagemIntencao = null;
                    await dc.BeginDialogAsync(nameof(IntencaoDialog));
                    return null;
                }
                else
                {
                    string vars = "Intenções de fluxo previstas:\r\n";
                    foreach (var nome in bot.Fluxo.NomeIntencoes)
                    {
                        vars += $"* {nome}\r\n";
                    }

                    return vars + "\r\nexpecifique o nome da intenção '#fluxo intenção'";
                }
            }
            else if (cmd.StartsWith("#ambient"))
            {
                var prms = cmd.Split(" ");
                if (prms.Length == 2)
                {
                    userIaago.Add(BotServices.AmbienteDefaultKey, prms[1], false);
                    return $"Ambiente '{prms[1]}' selecionado";
                }
                else
                {
                    return "Parametros invalido";
                }
            }
            else if (cmd.Contains("#clear"))
            {
                userIaago.Clear();
                userIaago.Add("_ChannelId", dc.Context.Activity.ChannelId, false);
                userIaago.Add("_fromId", dc.Context.Activity.From.Id, false);
                userIaago.Add("count", 0, false);

                return Textos.Logout;
            }
            else if (cmd.Contains("#reload"))
            {
                await bot.LoadAmbiente(dc.Context, true);
                return Textos.Reload;
            }
            else if (cmd.Contains("#var"))
            {
                string vars = "Variáveis Iaago\r\n";
                foreach (var key in userIaago.Keys)
                {
                    vars += $"* {key}: {userIaago[key]}\r\n";
                }

                return vars;
            }
            else if (cmd.StartsWith("#history"))
            {
                var i = cmd.IndexOf("#history ");
                if (i >= 0)
                {
                    var convid = cmd.Substring(i + 9).Split(' ')[0];
                    if (await bot.LoadHistoryByConversationId(dc.Context, convid, cancel))
                    {
                        return null;
                    }
                    else
                    {
                        return "Não há histórico para " + convid;
                    }
                }
                else if (await bot.LoadHistory(dc.Context, cancel))
                {
                    return null;
                }
                else
                {
                    return "Não há histórico";
                }
            }
            else if (cmd.Contains("#luis "))
            {
                int i = cmd.IndexOf("#luis ");
                cmd = dc.Context.Activity.Text?.Trim(); // Volta o texto original
                string[] prms = cmd.Substring(i + 6).Split(' ');
                if (prms.Length >= 2 && prms[1].Length == 36)
                {
                    if (bot.Services.LuisServices.ContainsKey(bot.Ambiente + "-" + prms[0]))
                    {
                        bot.Services.LuisServices.Remove(bot.Ambiente + "-" + prms[0]);
                    }

                    LuisApplication app;
                    if (prms.Length == 4)
                    {
                        app = new LuisApplication(prms[1], prms[2], prms[3]);
                    }
                    else if (prms.Length == 3)
                    {
                        app = new LuisApplication(prms[1], prms[2], "https://brazilsouth.api.cognitive.microsoft.com");
                    }
                    else
                    {
                        app = new LuisApplication(prms[1], "f8baa6ec30334ca3b2b9b56b5bd0be71", "https://brazilsouth.api.cognitive.microsoft.com");
                    }

                    bot.Services.LuisServices.Add(bot.Ambiente + "-" + prms[0], new LuisRecognizer(app));
                    return $"Novo Luis **'{prms[0]}'** adicionado";
                }
                else if (prms.Length == 1)
                {
                    if (bot.Services.LuisServices.ContainsKey(bot.Ambiente + "-" + prms[0]))
                    {
                        bot.Services.LuisServices.Remove(bot.Ambiente + "-" + prms[0]);
                        return $"Luis **'{prms[0]}'** removido";
                    }
                    else
                    {
                        return $"Não existe Luis **'{prms[0]}'**";
                    }
                }
                else
                {
                    return "Informe #luis nome appid";
                }
            }
            else if (cmd.Contains("#erro"))
            {
                throw new Exception(Textos.ErroProposital);
            }
            else if (cmd.Contains("#hora"))
            {
                return "Iaago ChatBot: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
            else if (cmd.Contains("#json"))
            {
                var dir = new DirectoryInfo($@".\wwwroot\{bot.Ambiente}\fluxo");
                string lista = "**lista de json**";
                foreach (var fi in dir.GetFiles("*.json"))
                {
                    lista += $"\r\n* [{fi.Name}]({BotServices.Endpoint}/{bot.Ambiente}/fluxo/{fi.Name})";
                }

                return lista;
            }
            else if (cmd.Contains("#image"))
            {
                var dir = new DirectoryInfo($@".\wwwroot\{bot.Ambiente}\imagens");
                string lista = "**lista de imagens**";
                foreach (var fi in dir.GetFiles("*.*"))
                {
                    lista += $"\r\n* [{fi.Name}]({BotServices.Endpoint}/{bot.Ambiente}/imagens/{fi.Name})";
                }

                return lista;
            }
            else if (cmd.Contains("#card"))
            {
                var dir = new DirectoryInfo($@".\wwwroot\{bot.Ambiente}\cards");
                string lista = "**lista de imagens**";
                foreach (var fi in dir.GetFiles("*.*"))
                {
                    lista += $"\r\n* [{fi.Name}]({BotServices.Endpoint}/{bot.Ambiente}/cards/{fi.Name})";
                }

                return lista;
            }
            else if (cmd.Contains("#debug"))
            {
                userIaago.Debug = !userIaago.Debug;
                return userIaago.Debug ? "Informação de debug ligado!" : "Informação de debug desligado";
            }

            else if (cmd.Contains("#chat"))
            {
                return $"[{Textos.FileChat}]({BotServices.Endpoint}/{Textos.FileChat})";
            }
            else if (cmd.Contains("#form"))
            {
                var welcomeCard = ActivityUtilsExtender.CreateAdaptiveCardAttachment(bot.Ambiente, "formCard.json");
                var response = dc.Context.Activity.CreateResponse(welcomeCard);
                await dc.Context.SendActivityAsync(response);
                return null;
            }
            else if (cmd.Contains("#bemv"))
            {
                var welcomeCard = ActivityUtilsExtender.CreateAdaptiveCardAttachment(bot.Ambiente, "welcomeCard.json");
                var response = dc.Context.Activity.CreateResponse(welcomeCard);
                await dc.Context.SendActivityAsync(response);
                return null;
            }
            else if (cmd.Contains("#suge"))
            {
                var response = dc.Context.Activity.CreateReply("OK, segue uma sugestões de comandos");
                response.SuggestedActions = new SuggestedActions()
                {
                    Actions = new List<CardAction>()
                    {
                        new CardAction() { Title = "Ola", Type = ActionTypes.ImBack, Value = "Blue" },
                        new CardAction() { Title = "quantas agua tenho", Type = ActionTypes.ImBack, Value = "Red" },
                        new CardAction() { Title = "faturamento de ontem", Type = ActionTypes.ImBack, Value = "Green" },
                    },
                };
                await dc.Context.SendActivityAsync(response);
                return null;
            }
            else if (cmd.Contains("#list"))
            {
                var dir = new DirectoryInfo(@".\wwwroot");
                string lista = "**lista de chats**";
                foreach (var fi in dir.GetFiles("*.txt"))
                {
                    lista += $"\r\n* [{fi.Name}]({BotServices.Endpoint}/{fi.Name})";
                }

                return lista;
            }
            else if (cmd.Contains("#link"))
            {
                return $"[Teste Link](teste.htm)";
            }

            else if (cmd.Contains("#ajuda"))
            {
                // https://blogs.msdn.microsoft.com/jamiedalton/2016/08/22/bot-framework-markdown-support-by-channel/
                string comandos = "**#ajuda**, **#vars**, **#history**, **#ambiente (nome)**, **#fluxo (nome)**, **#reload**, **#card**, **#image**, **#json**, **#hora**, **#clear**, **#info**, **#debug**";
                return $"Eu ainda estou nascendo então tenho uns comandinhos de testes das funcionalidades isoladas:\n{comandos}";
            }
            else
            {
                return "#"; // Continuar...
            }
        }
    }
}
