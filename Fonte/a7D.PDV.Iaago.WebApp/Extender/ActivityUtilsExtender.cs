using System.Collections.Generic;
using System.IO;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace a7D.PDV.Iaago.WebApp.Extender
{
    internal static class ActivityUtilsExtender
    {
        internal static Activity CreateResponse(this Activity activity, Attachment attachment)
        {
            var response = activity.CreateReply();
            response.Attachments = new List<Attachment>() { attachment };
            return response;
        }

        internal static Attachment CreateAdaptiveCardAttachment(string ambiente, string file)
        {
            var adaptiveCard = File.ReadAllText($@".\wwwroot\{ambiente}\cards\" + file);
            return new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCard),
            };
        }
    }
}
