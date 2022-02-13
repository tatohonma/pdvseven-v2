using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using a7D.PDV.Iaago.Dialogo;
using a7D.PDV.Iaago.WebApp.Extender;
using a7D.PDV.Iaago.WebApp.Models;
using a7D.PDV.Iaago.WebApp.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

// https://adaptivecards.io/
// http://martink.me/articles/bot-framework-v4-with-luis
// https://github.com/Microsoft/BotFramework-Samples/tree/master/SDKV4-Samples
// https://github.com/martinkearn/Content/blob/master/Blogs/Bot%20Framework%20V4%20with%20Luis.md
// https://webchat.botframework.com/embed/DevBotPDV7?s=ns-witPQ1wI.cwA.kaI.IgHa6eSmI78zj7Q-M-mzFaZTY-BIlHLs4bVNX2yDHP8&userID=IaagoTeste
// https://github.com/Microsoft/BotFramework-WebChat/blob/master/README.md
namespace a7D.PDV.Iaago.WebApp.ChatBot
{
    public class IaagoBot : IBot, IIaagoBot
    {
        private readonly DialogSet _dialogs;
        private readonly ConversationState _conversationState;
        private readonly UserState _userState;
        private readonly IStatePropertyAccessor<DialogState> _dialogState;
        private readonly AzureBlobTranscriptStore _transcriptStore;
        private IStatePropertyAccessor<IaagoVars> _userProfile;
        private string _log;
        private bool requestUpdate;

        internal BotServices Services { get; private set; }

        internal ILogger Logger { get; private set; }

        public string Ambiente { get; private set; }

        public IaagoFluxo Fluxo { get; private set; }

        public IaagoVars UserIaago { get; private set; }

        public IaagoContext Context { get; private set; }

        public static bool ContextDisabled { get; private set; }

        public IaagoBot(BotServices services, UserState userState, ConversationState conversationState, IaagoContext context, AzureBlobTranscriptStore transcriptStore, ILoggerFactory loggerFactory)
        {
            _log = string.Empty;
            _transcriptStore = transcriptStore;

            Logger = loggerFactory.CreateLogger<IaagoBot>();

            Context = ContextDisabled ? null : context;

            Services = services ?? throw new ArgumentNullException(nameof(services));

            // Dialogo (é a tela)
            _conversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            _dialogState = _conversationState.CreateProperty<DialogState>(nameof(DialogState));
            _dialogs = new DialogSet(_dialogState);

            // Usuário (é quem usa a tela)
            _userState = userState ?? throw new ArgumentNullException(nameof(userState));
            _userProfile = _userState.CreateProperty<IaagoVars>(nameof(IaagoVars));

            // Peprara dialogos que podem ser iniciados
            _dialogs.Add(new IntencaoDialog(this));
        }

        internal async Task LoadAmbiente(ITurnContext turn, bool reload)
        {
            if (Services.FluxoServices.ContainsKey(Ambiente))
            {
                if (reload)
                {
                    lock (Services.FluxoServices)
                    {
                        Services.FluxoServices.Remove(Ambiente);
                    }
                }
                else
                {
                    Fluxo = Services.FluxoServices[Ambiente];
                    return;
                }
            }

            Fluxo = new IaagoFluxo();

            var di = new DirectoryInfo($@".\wwwroot\{Ambiente}\fluxo");
            if (!di.Exists)
            {
                di.Create();
                await turn.SendActivityAsync($"Novo ambiente '{Ambiente}' criado, carregue os arquvos .json!");
                return;
            }

            string erros;
            lock (Services.FluxoServices)
            {
                Services.FluxoServices.Add(Ambiente, Fluxo);
                erros = Fluxo.LoadDirectory(di.FullName);

                // Adiciona os 'Luises' do ambiente
                foreach (var luis in Fluxo.Luis)
                {
                    try
                    {
                        var app = new LuisApplication(
                            luis.appid,
                            luis.chave ?? "f8baa6ec30334ca3b2b9b56b5bd0be71",
                            luis.endpoint ?? "https://brazilsouth.api.cognitive.microsoft.com");

                        var luisIA = new LuisRecognizer(app);

                        lock (Services.LuisServices)
                        {
                            if (Services.LuisServices.ContainsKey(Ambiente + "-" + luis.nome))
                            {
                                Services.LuisServices.Remove(Ambiente + "-" + luis.nome);
                            }

                            Services.LuisServices.Add(Ambiente + "-" + luis.nome, luisIA);
                        }
                    }
                    catch (Exception ex)
                    {
                        erros += $"Luis '{luis}' ERRO: {ex.Message}\r\n";
                    }
                }
            }

            if (!string.IsNullOrEmpty(erros))
            {
                await turn.SendActivityAsync("Houve erros ao carregar meus arquivos de dialogo:\r\n" + erros);
                var ex = new Exception();
                ex.Data.Add("Erro ao carregar arquivos de fluxo de intenção", erros);
                await Email.EnviaErro(ex);
            }
        }

        public async Task OnTurnAsync(
            ITurnContext turn,
            CancellationToken cancel)
        {
            AddInfo($"Conversation.Id: {turn.Activity.Conversation.Id}");
            AddInfo($"Activity: {turn.Activity.Type} - {turn.Activity.ChannelId} - {turn.Activity.Action}");
            AddInfo($"From: {turn.Activity.From?.Id} - {turn.Activity.From?.Name} - {turn.Activity.From?.Role}");
            AddInfo($"Recipient: {turn.Activity.Recipient?.Id}: {turn.Activity.Recipient?.Name} - {turn.Activity.Recipient?.Role}");

            // Cria o contexto de dialogo
            DialogContext dc = null;
            string resposta = null;

            try
            {
                // Cria o usuário unicamente!!!
                UserIaago = await _userProfile.GetAsync(turn, () => this.NewLogin(turn.Activity));
                UserIaago.onAdd += UserIaago_onAdd;

                Ambiente = (string)UserIaago[BotServices.AmbienteDefaultKey];
                await LoadAmbiente(turn, false);

                if (turn.Activity.Attachments?.Count > 0)
                {
                    await turn.SaveAttachments(Ambiente);
                    return;
                }

                bool bemvindo = false;
                if (turn.Activity.Type != ActivityTypes.Message)
                {
                    if (turn.Activity.Name == "requestWelcomeDialog")
                    {
                        var conversationIdOld = (string)UserIaago["conversationIdOld"];
                        if (conversationIdOld != null)
                        {
                            await LoadHistoryByConversationId(turn, conversationIdOld, cancel);
                        }
                    }
                    else
                    {
                        // Ignorando qualquer evento diferente de mensagem, a menos que seja um novo membro na conversa
                        bemvindo = await this.BemVindo(turn, cancel);

                        if (!bemvindo)
                        {
                            await _conversationState.SaveChangesAsync(turn);
                            await _userState.SaveChangesAsync(turn);
                            return;
                        }
                        else
                        {
                            AddInfo($"(Evento: {turn.Activity.Type})");
                        }
                    }
                }

                if (bemvindo)
                {
                    UserIaago.Add("_count", 2, false);

                    var conversationIdOld = (string)UserIaago["conversationId"];
                    UserIaago.Add("conversationOld", conversationIdOld, false);
                    UserIaago.Add("conversationId", turn.Activity.Conversation.Id, false);
                }
                else
                {
                    UserIaago.Add("_count", "@_count+1", true);
                }

                dc = await _dialogs.CreateContextAsync(turn, cancel);

                if (turn.Activity.Value is JObject json)
                {
                    string dump = "Valores:";
                    var dic = new DictionaryObject();
                    foreach (var item in json)
                    {
                        dump += $"\n* {item.Key}: {item.Value}";
                        if (item.Key == "intencao")
                        {
                            UserIaago.Intencao = item.Value.ToString();
                        }
                        else
                        {
                            dic.Add(item.Key, Interpretador.SimpleVar(item.Value.ToString()));
                        }
                    }

                    UserIaago.Add("@form", dic, false);

                    if (!string.IsNullOrEmpty(UserIaago.Intencao))
                    {
                        UserIaago.RetornoAtribuir = null;
                        UserIaago.IgnorarMensagemIntencao = null;
                        await dc.BeginDialogAsync(nameof(IntencaoDialog));
                    }

                    AddInfo(dump);
                }
                else if (turn.Activity.Text != null)
                {
                    await Textos.Write($" >>> {turn.Activity.Text}");
                }

                resposta = await this.MessageRenponseAsync(dc, cancel);
            }
            catch (SqlException ex)
            {
                UserIaago.Intencao = null;
                await dc?.CancelAllDialogsAsync(cancel);

                if (ex.Message.Contains("Cannot open server", StringComparison.InvariantCultureIgnoreCase))
                {
                    ContextDisable();
                    throw new Exception("Banco de dados inacessivel, Verifique o Firewall do Azure SQL ou a ConnectonString", ex);
                }

                throw ex;
            }
            catch (Exception ex)
            {
                UserIaago.Intencao = null;
                await dc?.CancelAllDialogsAsync(cancel);

                throw ex;
            }
            finally
            {
                if (!cancel.IsCancellationRequested)
                {
                    if (UserIaago?.Debug == true && _log.Length > 0)
                    {
                        await turn.SendAsync($"[DEBUG {(cancel.IsCancellationRequested ? "CANCEL" : string.Empty)} {_log}]");
                    }

                    if (!string.IsNullOrEmpty(resposta))
                    {
                        await turn.SendAsync(resposta);
                    }

                    if (requestUpdate)
                    {
                        await Context.SaveChangesAsync();
                    }

                    try
                    {
                        // Sempre salva a situação dos objetos da conversa e do usuário em um local unico
                        await _userProfile.SetAsync(turn, UserIaago);
                        await _conversationState.SaveChangesAsync(turn);
                        await _userState.SaveChangesAsync(turn);
                    }
                    catch (Exception ex)
                    {
                        await Email.EnviaErro(new Exception("Erro ao finalizar", ex));
                    }
                }
            }
        }

        internal void ContextDisable()
        {
            Context = null;
            ContextDisabled = true;
        }

        internal void AddInfo(string info)
        {
            _log += $"\r\n* {info}";
        }

        internal async Task<bool> LoadHistory(ITurnContext turn, CancellationToken cancel)
        {
            return await LoadHistoryByConversationId(turn, turn.Activity.Conversation.Id, cancel);
        }

        internal async Task<bool> LoadHistoryByConversationId(ITurnContext turn, string conversationId, CancellationToken cancel)
        {
            int incrementId = 0;

            try
            {
                // WebChat and Emulator require modifying the activity.Id to display the same activity again within the same chat window
                var activity = turn.Activity;
                bool updateActivities = new[] { Channels.Webchat, Channels.Emulator, Channels.Directline, }.Contains(activity.ChannelId);
                if (updateActivities && activity.Id.Contains("|"))
                {
                    int.TryParse(activity.Id.Split('|')[1], out incrementId);
                }
                else if (!updateActivities)
                {
                    return false;
                }

                // Download the activities from the Transcript (blob store) and send them over to the channel when a request to upload history arrives.
                // This could be an event or a special message acctivity as above.
                var connectorClient = turn.TurnState.Get<ConnectorClient>(typeof(IConnectorClient).FullName);

                // Get all the message type activities from the Transcript.
                string continuationToken = null;
                var count = 0;

                do
                {
                    var pagedTranscript = await _transcriptStore.GetTranscriptActivitiesAsync(activity.ChannelId, conversationId, continuationToken);
                    var activities = pagedTranscript.Items
                        .Where(a => a.Type == ActivityTypes.Message)
                        .Select(ia => (Activity)ia)
                        .ToList();

                    //if (conversationId == activity.Conversation.Id)
                    {
                        foreach (var a in activities)
                        {
                            incrementId++;
                            a.Conversation.Id = activity.Conversation.Id;
                            a.Id = string.Concat(activity.Conversation.Id, "|", incrementId.ToString().PadLeft(7, '0'));
                            a.Timestamp = DateTimeOffset.UtcNow;
                            a.ChannelData = string.Empty; // WebChat uses ChannelData for id comparisons, so we clear it here
                        }
                    }

                    // DirectLine only allows the upload of at most 500 activities at a time. The limit of 1500 below is
                    // arbitrary and up to the Bot author to decide.
                    count += activities.Count();
                    if (activities.Count() > 500 || count > 1500)
                    {
                        throw new InvalidOperationException("Attempt to upload too many activities");
                    }

                    var transcript = new Transcript(activities);

                    await connectorClient.Conversations.SendConversationHistoryAsync(activity.Conversation.Id, transcript, cancellationToken: cancel);

                    continuationToken = pagedTranscript.ContinuationToken;
                }
                while (continuationToken != null && !cancel.IsCancellationRequested);

                return count > 0;
            }
            catch (Exception ex)
            {
                await turn.SendActivityAsync($"History #{incrementId} Load Erro:\r\n" + ex.Message + "\r\n" + ex.StackTrace);
                return false;
            }
        }

        private void UserIaago_onAdd(string chave, string valor)
        {
            if (Context == null || ContextDisabled)
            {
                return;
            }

            int userID = UserIaago.UserID();
            if (userID <= 0) // -1 indica erro de leitura!
            {
                return;
            }

            var variavel = new Models.Variaveis()
            {
                IDUsuario = userID,
                Chave = chave,
                Valor = valor,
                DataGravacao = DateTime.Now,
            };

            Context.Variaveis.Add(variavel);
            requestUpdate = true;
        }
    }
}