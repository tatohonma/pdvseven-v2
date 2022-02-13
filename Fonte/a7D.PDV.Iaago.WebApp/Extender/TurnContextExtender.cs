using System;
using System.IO;
using System.Threading.Tasks;
using a7D.PDV.Iaago.WebApp.ChatBot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;

namespace a7D.PDV.Iaago.WebApp.Extender
{
    internal static class TurnContextExtender
    {
        internal static async Task SendAsync(this ITurnContext context, string text, params object[] prms)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var resposta = string.Format(text, prms);
            await context.SendActivityAsync(resposta);
            await Textos.Write("<<< " + resposta);
        }

        internal static async Task SaveAttachments(this ITurnContext turn, string ambiente)
        {
            // https://www.robinosborne.co.uk/2016/12/05/receiving-files-sent-to-your-botframework-chatbot/
            // https://docs.microsoft.com/en-us/microsoftteams/platform/concepts/bots/bots-files
            foreach (var att in turn.Activity.Attachments)
            {
                await Textos.Write($"Attachments >>> {att.Name} ({att.Content})");
                var connector = new ConnectorClient(new Uri(turn.Activity.ServiceUrl));
                if (turn.Activity.ChannelId == "emulator")
                {
                    await turn.SendAsync("Desculpe não sei carregar arquivo do emulador");
                }
                else
                {
                    var attData = connector.HttpClient.GetByteArrayAsync(att.ContentUrl).Result;
                    string name = att.Name.ToLower();
                    string path;
                    if (name.EndsWith("card.json"))
                    {
                        path = "cards";
                    }
                    else if (name.EndsWith(".json"))
                    {
                        path = "fluxo";
                    }
                    else if (name.EndsWith(".png") || name.EndsWith(".jpg"))
                    {
                        path = "imagens";
                    }
                    else
                    {
                        await turn.SendAsync("Desculpe não reconheco o tipo de arquivo");
                        return;
                    }

                    var namefile = $@".\wwwroot\{ambiente}\{path}\{name}";
                    var fi = new FileInfo(namefile);
                    if (!fi.Directory.Exists)
                    {
                        fi.Directory.Create();
                    }

                    await File.WriteAllBytesAsync(fi.FullName, attData);
                    string file = $"[{att.Name}]({BotServices.Endpoint}/{ambiente}/{path}/{name})";
                    await turn.SendAsync(file);
                }
            }
        }
    }
}
