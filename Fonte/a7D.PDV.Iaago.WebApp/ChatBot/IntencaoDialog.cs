using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using a7D.PDV.Iaago.Dialogo;
using a7D.PDV.Iaago.WebApp.Extender;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace a7D.PDV.Iaago.WebApp.ChatBot
{
    // https://docs.microsoft.com/pt-br/azure/bot-service/bot-builder-dialog-manage-conversation-flow?view=azure-bot-service-4.0&tabs=csharp
    // https://docs.microsoft.com/pt-br/azure/bot-service/bot-builder-dialog-manage-complex-conversation-flow?view=azure-bot-service-4.0&tabs=csharp
    public class IntencaoDialog : ComponentDialog
    {
        // Prompts IDs names
        private static string waterfallFluxo;
        private static string promptText;

        private IIaagoBot Bot;

        public IntencaoDialog(IIaagoBot bot)
            : base(nameof(IntencaoDialog))
        {
            Bot = bot ?? throw new Exception("IaagoBot não atribuido");

            // Add control flow dialogs
            var waterfallSteps = new WaterfallStep[]
            {
                PromptForTextStepAsync,
                EndPromptStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(waterfallFluxo), waterfallSteps));
            AddDialog(new TextPrompt(nameof(promptText)));
        }

        private async Task<DialogTurnResult> PromptForTextStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancel)
        {
            var userIaago = Bot.UserIaago ?? throw new Exception("UserIaago não atribuida");
            var intencaoAlvo = Bot.Fluxo.ExecutaIntencao(userIaago);

            if (intencaoAlvo == null)
            {
                await stepContext.Context.SendAsync("OPS! Não entendi a intenção '{0}'", userIaago.Intencao);
                return await stepContext.EndDialogAsync();
            }

            if (!string.IsNullOrEmpty(intencaoAlvo.card))
            {
                var response = stepContext.Context.Activity.CreateReply();
                response.Attachments = new List<Attachment>() {
                    ActivityUtilsExtender.CreateAdaptiveCardAttachment(Bot.Ambiente, intencaoAlvo.card),
                };
                await stepContext.Context.SendActivityAsync(response);
            }

            var mensagem = intencaoAlvo.ExibirMensagem(userIaago);

            var suggested = ConvertSuggestedActions(intencaoAlvo.sugestao);

            if (intencaoAlvo.acaoResposta == null || intencaoAlvo.acaoResposta.Length == 0)
            {
                if (suggested != null)
                {
                    var reply = stepContext.Context.Activity.CreateReply(mensagem);
                    reply.SuggestedActions = suggested;
                    await stepContext.Context.SendActivityAsync(reply);
                }
                else
                {
                    await stepContext.Context.SendAsync(mensagem);
                }

                return await stepContext.EndDialogAsync();
            }
            else if (suggested == null && intencaoAlvo.acaoResposta.Any(a => a.tipoResposta == "bool"))
            {
                suggested = new SuggestedActions()
                {
                    Actions = new List<CardAction>()
                    {
                        new CardAction() { Title = "Sim", Type = ActionTypes.ImBack, Value = "sim" },
                        new CardAction() { Title = "Não", Type = ActionTypes.ImBack, Value = "não" },
                    },
                };
            }

            // Precisa de interação do usuário!
            if (intencaoAlvo.acaoResposta.Any(a => a.tipoResposta != null))
            {
                var opts = new PromptOptions
                {
                    Prompt = new Activity
                    {
                        Type = ActivityTypes.Message,
                        Text = mensagem,
                        SuggestedActions = suggested,
                    },
                };
                return await stepContext.PromptAsync(nameof(promptText), opts);
            }

            if (!string.IsNullOrEmpty(mensagem))
            {
                await stepContext.Context.SendAsync(mensagem);
            }

            // Em caso de fluxos sem respostas avança automático (automatico)
            // Caso haja sugestões na intenção essa será usada como comum a todas respostas
            return await stepContext.NextAsync();
        }

        private async Task<DialogTurnResult> EndPromptStepAsync(
            WaterfallStepContext stepContext,
            CancellationToken cancel)
        {
            var userIaago = Bot.UserIaago ?? throw new Exception("UserIaago não atribuida");
            if (stepContext.Result is string text && text != null)
            {
                userIaago.Text = text;
            }

            var intencaoAlvo = Bot.Fluxo.BuscaIntencao(userIaago);
            var ret = intencaoAlvo.BuscaRetorno(userIaago);
            if (ret != null)
            {
                var suggested = ConvertSuggestedActions(intencaoAlvo.sugestao);

                var mensagem = ret.ExibirMensagem(userIaago);

                if (suggested != null)
                {
                    var reply = stepContext.Context.Activity.CreateReply(mensagem);
                    reply.SuggestedActions = suggested;
                    await stepContext.Context.SendActivityAsync(reply);
                }
                else if (!string.IsNullOrEmpty(mensagem))
                {
                    await stepContext.Context.SendAsync(mensagem);
                }

                if (ret.aguardar != null)
                {
                    await Task.Delay(ret.aguardar.Value * 1000);
                }
            }

            if (string.IsNullOrEmpty(userIaago.Intencao))
            {
                return await stepContext.EndDialogAsync();
            }
            else
            {
                // Cuidado: Loop!
                return await stepContext.ReplaceDialogAsync(nameof(waterfallFluxo));
            }
        }

        private static SuggestedActions ConvertSuggestedActions(IaagoSugestao[] sugestao)
        {
            if (sugestao == null)
            {
                return null;
            }

            return new SuggestedActions()
            {
                Actions = sugestao.Select(s => new CardAction()
                {
                    Type = ActionTypes.ImBack,
                    Title = s.titulo,
                    Value = s.texto,
                }).ToList(),
            };
        }
    }
}
