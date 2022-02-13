// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License

using System;
using System.Collections.Generic;
using a7D.PDV.Iaago.Dialogo;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Configuration;
using Microsoft.Bot.Schema;

namespace a7D.PDV.Iaago.WebApp.ChatBot
{
    /// <summary>
    /// Represents references to external services.
    /// For example, LUIS services are kept here as a singleton.  This external service is configured
    /// using the <see cref="BotConfiguration"/> class.
    /// </summary>
    /// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.1"/>
    /// <seealso cref="https://www.luis.ai/home"/>
    public class BotServices
    {
        public const string AmbienteDefaultKey = "ambiente";
        public const string AmbienteDefaultValue = "default";

        public readonly Dictionary<string, LuisRecognizer> LuisServices = new Dictionary<string, LuisRecognizer>();

        public readonly Dictionary<string, QnAMaker> QnAServices = new Dictionary<string, QnAMaker>();

        public readonly Dictionary<string, IaagoFluxo> FluxoServices = new Dictionary<string, IaagoFluxo>();

        public static string Endpoint { get; internal set; }

        public static int CountInstances { get; internal set; }

        public BotServices(BotConfiguration botConfiguration)
        {
            foreach (var service in botConfiguration.Services)
            {
                switch (service.Type)
                {
                    case ServiceTypes.Luis:
                        var luis = (LuisService)service;
                        if (luis == null)
                        {
                            throw new InvalidOperationException("The LUIS service is not configured correctly in your '.bot' file.");
                        }

                        var endpoint = (luis.Region?.StartsWith("https://") ?? false) ? luis.Region : luis.GetEndpoint();
                        var app = new LuisApplication(luis.AppId, luis.AuthoringKey, endpoint);
                        var recognizer = new LuisRecognizer(app);
                        this.LuisServices.Add(luis.Name, recognizer);
                        break;

                    case ServiceTypes.QnA:

                        // Create a QnA Maker that is initialized and suitable for passing
                        // into the IBot-derived class (QnABot).
                        var qna = (QnAMakerService)service;
                        if (qna == null)
                        {
                            throw new InvalidOperationException("The QnA service is not configured correctly in your '.bot' file.");
                        }

                        if (string.IsNullOrWhiteSpace(qna.KbId))
                        {
                            throw new InvalidOperationException("The QnA KnowledgeBaseId ('kbId') is required to run this sample. Please update your '.bot' file.");
                        }

                        if (string.IsNullOrWhiteSpace(qna.EndpointKey))
                        {
                            throw new InvalidOperationException("The QnA EndpointKey ('endpointKey') is required to run this sample. Please update your '.bot' file.");
                        }

                        if (string.IsNullOrWhiteSpace(qna.Hostname))
                        {
                            throw new InvalidOperationException("The QnA Host ('hostname') is required to run this sample. Please update your '.bot' file.");
                        }

                        var qnaEndpoint = new QnAMakerEndpoint()
                        {
                            KnowledgeBaseId = qna.KbId,
                            EndpointKey = qna.EndpointKey,
                            Host = qna.Hostname,
                        };

                        var qnaMaker = new QnAMaker(qnaEndpoint);
                        QnAServices.Add(qna.Name, qnaMaker);
                        break;
                }
            }
        }
    }
}
