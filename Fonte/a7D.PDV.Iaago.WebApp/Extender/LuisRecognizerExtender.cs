using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using a7D.PDV.Iaago.Dialogo;
using a7D.PDV.Iaago.WebApp.ChatBot;
using a7D.PDV.Iaago.WebApp.Models;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace a7D.PDV.Iaago.WebApp.Extender
{
    public class MyLuisResult
    {
        public string Key { get; set; }

        public string Intencao { get; set; }

        public string Erro { get; set; }

        public double Score { get; set; }

        public string Entidade { get; set; }

        public string Valor { get; set; }

        public string Json { get; set; }
    }

    internal static class LuisRecognizerExtender
    {
        internal static async Task<MyLuisResult> BuscaIntencaoLuiz(this IaagoBot bot, ITurnContext turn, CancellationToken cancel)
        {
            string text = turn.Activity.Text?.Trim().ToLower();
            text = text
                .Replace("  ", " ")
                .Replace("?", string.Empty)
                .Replace("!", string.Empty);

            try
            {
                var pergunta = bot.Context?.Perguntas.FirstOrDefault(p => p.Texto == text);
                if (pergunta != null && pergunta.Score > 0.5)
                {
                    pergunta.UltimoUso = DateTime.Now;
                    pergunta.QTD++;

                    // Já como os valores de entidade as vezes são data ou numeros que veriam não pode-se ficar muito tempo com os dados em cache
                    if (pergunta.UltimaAtualizacao > DateTime.Now.AddMinutes(-2))
                    {
                        bot.Context.Perguntas.Update(pergunta);
                        bot.Context.SaveChanges();
                        bot.AddInfo($"(CACHE) LUIS **{pergunta.Luis}**: {pergunta.Intencao} = {pergunta.Score} {pergunta.Entidades()}");
                        return new MyLuisResult()
                        {
                            Intencao = pergunta.Intencao,
                            Entidade = pergunta.Entidade1,
                            Valor = pergunta.Valor1,
                        };
                    }
                }

                var luisResults = await bot.BuscaIntencaoSoLuiz(turn, cancel);

                if (pergunta == null)
                {
                    pergunta = new Pergunta()
                    {
                        Texto = text,
                        Criacao = DateTime.Now,
                        UltimoUso = DateTime.Now,
                    };
                }

                MyLuisResult lr;
                if (luisResults.Length == 0)
                {
                    lr = null;
                }
                else
                {
                    lr = luisResults.First();
                    pergunta.Luis = lr.Key;
                    pergunta.Score = lr.Score;
                    pergunta.Intencao = lr.Intencao;
                    pergunta.Entidade1 = lr.Entidade;
                    pergunta.Valor1 = lr.Valor;
                    pergunta.Retorno = lr.Json;

                    // Não responde!
                    if (lr.Score < 0.3)
                    {
                        lr = null;
                    }
                }

                if (bot.Context != null)
                {
                    // Salva tudo, inclusive o que não se sabe responder
                    pergunta.UltimaAtualizacao = pergunta.UltimoUso = DateTime.Now;
                    if (pergunta.ID == 0)
                    {
                        bot.Context.Perguntas.Add(pergunta);
                    }
                    else
                    {
                        bot.Context.Perguntas.Update(pergunta);
                    }

                    bot.Context.SaveChanges();
                }

                return lr;
            }
            catch (Exception ex)
            {
                ex.Data.Add("text", text);
                throw ex;
            }
        }

        private static async Task<MyLuisResult[]> BuscaIntencaoSoLuiz(this IaagoBot bot, ITurnContext turn, CancellationToken cancel)
        {
            var luizRequests = bot.Services.LuisServices.Keys.Where(k => k.StartsWith(bot.Ambiente + "-")).Select(l => bot.LuizIntencaoEntidade(l, turn, cancel));
            var luizResults = await Task.WhenAll(luizRequests);
            var listResults = luizResults.OrderByDescending(l => l.Score);
            foreach (var lr in listResults)
            {
                bot.AddInfo($"LUIS **{lr.Key}**: {lr.Intencao} = {lr.Score} ({lr.Entidade}={lr.Valor})");
            }

            return listResults
                .Where(l => l.Score > 0 && l.Intencao != "None")
                .ToArray();
        }

        private static async Task<MyLuisResult> LuizIntencaoEntidade(this IaagoBot bot, string luizKey, ITurnContext turn, CancellationToken cancel)
        {
            var lr = new MyLuisResult
            {
                Key = luizKey,
            };
            try
            {
                string resposta = string.Empty;
                var luizService = bot.Services.LuisServices[luizKey];
                var luizResults = await luizService.RecognizeAsync(turn, cancel);
                var (intent, score) = luizResults.GetTopScoringIntent();
                lr.Intencao = intent;
                lr.Score = score;
                lr.Json = luizResults.Entities.ToString(); // .ParseLuisForEntities();
                if (lr.Json.Length > 30)
                {
                    var results = luizResults.Entities.FromJTokenToDictionary();
                    if (results.Count == 2 && results.ContainsKey("datetime"))
                    {
                        lr.Entidade = "datetime";
                        var valores = (List<object>)results["datetime"];
                        var itens = (DictionaryObject)valores[0];
                        var valores2 = (List<object>)itens["timex"];
                        lr.Valor = valores2[0].ToString();
                    }
                    else if (results.Count >= 1 && results.ContainsKey("$instance")
                     && results["$instance"] is DictionaryObject instance1)
                    {
                        lr.Entidade = instance1.Keys.First();
                        var valores = (List<object>)instance1[lr.Entidade];
                        var itens = (DictionaryObject)valores[0];
                        lr.Valor = (string)itens["text"];
                    }
                }
            }
            catch (APIErrorException ex)
            {
                lr.Erro = ex.Body?.Message ?? ex.Message;
                bot.AddInfo($"LUIS **{lr.Key}** ERRO API: {lr.Erro}");
            }
            catch (Exception ex)
            {
                lr.Erro = ex.Message;
                bot.AddInfo($"LUIS **{lr.Key}** ERRO: {lr.Erro}");
            }

            return lr;
        }

        internal static string ParseLuisForEntities(this RecognizerResult recognizerResult)
        {
            string result = string.Empty;

            // recognizerResult.Entities returns type JObject.
            foreach (var entity in recognizerResult.Entities)
            {
                try
                {
                    // Parse JObject for a known entity types: Appointment, Meeting, and Schedule.
                    var tp = entity.GetType().Name;
                    var name = entity.Key; // {"$instance":
                    var value = entity.Value.ToString();
                    if (value.Contains(":"))
                    {
                        var entidadeFound = JObject.Parse(value);
                        // We will return info on the first entity found.
                        if (entidadeFound != null)
                        {
                            // use JsonConvert to convert entity.Value to a dynamic object.
                            var o = JsonConvert.DeserializeObject<InstanceData>(entity.Value.ToString());
                            foreach (dynamic x in o.Properties)
                            {
                                result += $"\r\n{x.Key}: {x.Value[0]["text"]}";
                            }
                        }
                    }
                    else
                    {
                        //var array = JArray.Parse(value);
                        //result += $"\r\n{name}: {array.First()}";
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }
    }
}
