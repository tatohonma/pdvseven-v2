using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using a7D.PDV.Iaago.WebApp.ChatBot;
using a7D.PDV.Iaago.WebApp.Models;
using a7D.PDV.Iaago.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace a7D.PDV.Iaago.WebApp
{
    public class Startup
    {
        private ILoggerFactory _loggerFactory;
        private bool _isProduction = false;

        public IConfiguration Configuration { get; }

        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-2.1
        public Startup(IHostingEnvironment env)
        {
            _isProduction = env.IsProduction();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                var connection = Configuration.GetSection("connectionIaagoString")?.Value;
                services.AddDbContext<IaagoContext>(options => options.UseSqlServer(connection));

                var secretKey = Configuration.GetSection("botFileSecret")?.Value;
                var botFilePath = Configuration.GetSection("botFilePath")?.Value;
                if (!File.Exists(botFilePath))
                {
                    throw new FileNotFoundException($"The .bot configuration file was not found. botFilePath: {botFilePath}");
                }

                // Loads .bot configuration file and adds a singleton that your Bot can access through dependency injection.
                BotConfiguration botConfig = null;
                try
                {
                    botConfig = BotConfiguration.Load(botFilePath, secretKey);
                }
                catch
                {
                    var msg = @"Error reading bot file. Please ensure you have valid botFilePath and botFileSecret set for your environment.
    - You can find the botFilePath and botFileSecret in the Azure App Service application settings.
    - If you are running this bot locally, consider adding a appsettings.json file with botFilePath and botFileSecret.
    - See https://aka.ms/about-bot-file to learn more about .bot file its use and bot configuration.
    ";
                    throw new InvalidOperationException(msg);
                }

                services.AddSingleton(sp => botConfig ?? throw new InvalidOperationException($"The .bot configuration file could not be loaded. botFilePath: {botFilePath}"));

                // Create the connected services from .bot file.
                services.AddSingleton(sp => new BotServices(botConfig));

                // Retrieve current endpoint.
                var environment = _isProduction ? "production" : "development";
                var service = botConfig.Services.FirstOrDefault(s => s.Type == "endpoint" && s.Name == environment);
                if (service == null && _isProduction)
                {
                    // Attempt to load development environment
                    service = botConfig.Services.Where(s => s.Type == "endpoint" && s.Name == "development").FirstOrDefault();
                }

                if (service is EndpointService endpointService)
                {
                    BotServices.Endpoint = endpointService.Endpoint.Substring(0, endpointService.Endpoint.IndexOf("/api"));
                }
                else
                {
                    throw new InvalidOperationException($"The .bot file does not contain an endpoint with name '{environment}'.");
                }

                // Storage configuration name or ID from the .bot file.
                const string StorageConfigurationId = "Storage";
                var blobConfig = botConfig.FindServiceByNameOrId(StorageConfigurationId);
                if (!(blobConfig is BlobStorageService blobStorageConfig))
                {
                    throw new InvalidOperationException($"The .bot file does not contain an blob storage with name '{StorageConfigurationId}'.");
                }

                // Create and add conversation state.
                // IStorage memoryDataStore = new AzureBlobStorage(blobStorageConfig.ConnectionString, "conversation");
                IStorage memoryDataStore = new MemoryStorage();
                var conversationState = new ConversationState(memoryDataStore);
                services.AddSingleton(conversationState);

                // As variáveis virão do SQL
                // IStorage userDataStore = new AzureBlobStorage(blobStorageConfig.ConnectionString, "user");
                IStorage userDataStore = new MemoryStorage();
                var userState = new UserState(userDataStore);
                services.AddSingleton(userState);

                var blobHistoryStore = new AzureBlobTranscriptStore(blobStorageConfig.ConnectionString, "history");
                services.AddSingleton(blobHistoryStore);

                services.AddBot<IaagoBot>(options =>
                {
                    options.CredentialProvider = new SimpleCredentialProvider(endpointService.AppId, endpointService.AppPassword);
                    options.ChannelProvider = new ConfigurationChannelProvider(Configuration);

                    ILogger logger = _loggerFactory.CreateLogger<IaagoBot>();

                    options.OnTurnError = async (context, exception) =>
                    {
                        logger.LogError(exception, $"Exception caught : {exception}");
                        await context.SendActivityAsync(string.Format(Textos.Bug, exception.Message, _isProduction ? string.Empty : exception.StackTrace));
                        await Email.EnviaErro(exception);
                    };

                    // Enable the conversation transcript middleware.
                    var transcriptMiddleware = new TranscriptLoggerMiddleware(blobHistoryStore);
                    options.Middleware.Add(transcriptMiddleware);
                });
            }
            catch (Exception ex)
            {
                Task.WaitAll(Email.EnviaErro(ex));
                throw ex;
            }
        }

        public void Configure(IApplicationBuilder appl, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            appl.UseDefaultFiles()
                .UseStaticFiles()
                .UseBotFramework();

            // .UseDeveloperExceptionPage()
        }
    }
}
