using System.Threading;
using System.Threading.Tasks;
using a7D.PDV.Iaago.WebApp.ChatBot;
using Microsoft.Bot.Builder;

namespace a7D.PDV.Iaago.WebApp.Extender
{
    internal static class ActivityConversationUpdateExtender
    {
        internal static async Task<bool> BemVindo(this IaagoBot bot, ITurnContext turn, CancellationToken cancel)
        {
            // Obtem a atividade do contexto apenas para facilitar
            var activity = turn.Activity;

            if (activity.MembersAdded == null)
            {
                return false;
            }

            // Iterate over all new members added to the conversation.
            foreach (var member in activity.MembersAdded)
            {
                if (cancel.IsCancellationRequested)
                {
                    return false;
                }

                bot.AddInfo($"MembersAdded: {activity.ChannelId} {member.Id} - {member.Name} - {member.Role}");
                if (member.Id != activity.Recipient.Id)
                {
                    //// Greet anyone that was not the target (recipient) of this message.
                    //// To learn more about Adaptive Cards, see https://aka.ms/msbot-adaptivecards for more details.
                    // var welcomeCard = ActivityUtilsExtender.CreateAdaptiveCardAttachment(bot.Ambiente, "welcomeCard.json");
                    // var response = activity.CreateResponse(welcomeCard);
                    // await turn.SendActivityAsync(response);
                    return true;
                }
            }

            return false;
        }
    }
}
