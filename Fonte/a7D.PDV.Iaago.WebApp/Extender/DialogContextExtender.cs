using System;
using System.Linq;
using System.Threading.Tasks;
using a7D.PDV.Iaago.WebApp.ChatBot;
using Microsoft.Bot.Builder.Dialogs;

namespace a7D.PDV.Iaago.WebApp.Extender
{
    internal static class DialogContextExtender
    {
        private static readonly string[] sair = { "cancel", "cancela", "cancelar", "sair", "abortar", "abandonar", "parar" };

        internal static async Task<bool> IsTurnInterruptedAsync(this DialogContext dc, string text)
        {
            // Se contiver qualquer um dos comandos de "sair"
            text = text.ToLower();
            if (sair.Contains(text))
            {
                if (dc.ActiveDialog != null)
                {
                    await dc.CancelAllDialogsAsync();
                    await dc.Context.SendAsync(Textos.DialogoCancelado);
                }
                else
                {
                    await dc.Context.SendAsync(Textos.DialogoNaoExiste);
                }

                return true;
            }

            return false;
        }
    }
}
