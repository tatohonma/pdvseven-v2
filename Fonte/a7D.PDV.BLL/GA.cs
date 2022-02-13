using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.BLL
{
    public enum EventType
    {
        pageview,
        screenview,
        @event,
        transaction,
        item,
        social,
        exception,
        timing
    }

    public static class GA
    {
        // https://developers.google.com/api-client-library/dotnet/apis/analytics/v3?hl=pt
        // https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide
        // https://pt.wikipedia.org/wiki/ISO_4217
        private static readonly HttpClient client = new HttpClient();
        private static readonly Uri endereco = new Uri("https://www.google-analytics.com/collect");
        private static readonly string TID = "UA-117918586-1";

        public static void Post(Form frm)
        {
            if (AC.Chave == null || AC.NomeAplicacao == null)
                return;

            var CD = new KeyValuePair<string, string>("cd", $"{frm.Name}"); // Screen name / content description.
            var DT = new KeyValuePair<string, string>("dt", $"{frm.Text}"); // Titulo
            var VP = new KeyValuePair<string, string>("vp", $"{frm.Width}x{frm.Height}"); // ViewPort Area
            var SR = new KeyValuePair<string, string>("sr", $"{Screen.PrimaryScreen.WorkingArea.Width}x{Screen.PrimaryScreen.WorkingArea.Height}"); // Tela

            // v, t, tid, av, an, ua, cid, uid
            PostAsync(EventType.screenview, AC.PDV, CD, DT, VP, SR);
        }

        public static void PostEvento(PDVInformation pdv, string categoria, string label = "", UsuarioInformation usuario = null)
        {
            if (AC.Chave == null || AC.NomeAplicacao == null)
                return;

            var EC = new KeyValuePair<string, string>("ec", categoria); // Event Category. Required (150 Bytes)
            var EA = new KeyValuePair<string, string>("ea", $"{AC.Cliente}: {AC.NomeAplicacao} {AC.Versao} - {pdv?.Nome ?? AC.PDV?.Nome} / {usuario?.Nome ?? AC.Usuario?.Nome}"); // Event Action. Required (até 500 bytes)
            var EL = new KeyValuePair<string, string>("el", label); // Rotulo (label) Opcional até 500

            // v, t, tid, av, an, ua, cid, uid
            PostAsync(EventType.@event, pdv ?? AC.PDV, EC, EA, EL);
        }

        public static void PostEvento(string categoria, string label = "", UsuarioInformation usuario = null)
        {
            PostEvento(AC.PDV, categoria, label, usuario);
        }

        public static void PostTransacao(PDVInformation pdv, int id, decimal valor)
        {
            if (AC.Chave == null || AC.NomeAplicacao == null || valor == 0)
                return;

            var CD = new KeyValuePair<string, string>("ta", $"{AC.Cliente}"); // CD
            var TI = new KeyValuePair<string, string>("ti", $"P{id}"); // Numero da transação
            var TR = new KeyValuePair<string, string>("tr", $"{valor.ToString("N2").Replace(",", ".")}");
            var CU = new KeyValuePair<string, string>("cu", $"BRL");

            // v, t, tid, av, an, ua, cid, uid
            PostAsync(EventType.transaction, pdv ?? AC.PDV, CD, TI, TR, CU);
        }

        internal static void PostErro(CodigoErro codigo, string erro)
        {
            if (AC.Chave == null || AC.NomeAplicacao == null)
                return;

            if (erro.Length > 150)
                erro = erro.Substring(0, 150);

            PostEvento(AC.PDV, "ERRO " + codigo.ToString(), erro);

            var EXD = new KeyValuePair<string, string>("exd", erro);
            var EXF = new KeyValuePair<string, string>("exf", erro.StartsWith("[E000]") ? "1" : "0");
            PostAsync(EventType.exception, AC.PDV, EXD, EXF);
        }

        private static void PostAsync(EventType et, PDVInformation pdv, params KeyValuePair<string, string>[] options)
        {
            try
            {
                //v, t, tid, av, an, ua, cid, uid
                var list = new List<KeyValuePair<string, string>>(new[] {
                    new KeyValuePair<string, string>("v", "1"),             // Versão
                    new KeyValuePair<string, string>("t", et.ToString()),   // Tipo do Evento
                    new KeyValuePair<string, string>("tid", TID),           // Chave do GA
                    new KeyValuePair<string, string>("av", AC.Versao),      // Versão da aplicação
                    new KeyValuePair<string, string>("an", AC.NomeAplicacao),   // Nome da aplicação: Caixa, BackOffice, WS2
                    new KeyValuePair<string, string>("ua", $"PDV7 ({AC.OS})"),  // Sistema Operacional
                    new KeyValuePair<string, string>("cid", $"{AC.Cliente}-{pdv?.Nome}"), // Cliente-PDV
                    new KeyValuePair<string, string>("uid", $"{AC.Usuario?.Nome ?? "-"}")}); // Usuário

                list.AddRange(options);

                Task.Run(() =>
                {
                    try
                    {
                        var request = new FormUrlEncodedContent(list.ToArray());
                        var result = client.PostAsync(endereco, request).Result;
                        var retorno = result.Content.ReadAsStringAsync().Result;
                    }
                    catch (Exception)
                    {
                    }
                });
            }
            catch (Exception)
            {
            }
        }
    }
}