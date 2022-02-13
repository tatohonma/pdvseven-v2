using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Controllers;
using a7D.PDV.Ativacao.API.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace a7D.PDV.Ativacao.API
{
    public class clientes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sw = new Stopwatch();
            sw.Start();

            context.Response.ContentType = "text/plain";

            string cmd = context.Request["cmd"]?.ToLower() ?? "";
            if (cmd == "clear")
            {
                context.Response.Write($"Clientes: Zerados");
                ClientesService.clientes.Clear();
                return;
            }
            else if (cmd == "erro")
            {
                context.Response.Write($"Clientes: Erros Zerados");
                ClientesService.clientes.ForEach(c => c.ClearErro());
                return;
            }

            context.Response.Write($"Clientes: {ClientesService.clientes.Count} desde {ClientesService.dtStart.ToString("dd/MM/yyyy HH:mm:ss")}\r\n\r\n");
            using (var db = new AtivacaoContext())
            {
                List<Entities.Ativacao> outros = null;
                if (cmd == "all")
                {
                    outros = db.Ativacoes
                        .Where(a => a.Ativo && a.DtUltimaVerificacao.HasValue)
                        .OrderByDescending(a => a.DtUltimaVerificacao)
                        .ToList();
                }

                foreach (var cliente in ClientesService.clientes.OrderByDescending(c => c.data))
                {
                    var ativacao = db.Ativacoes.FirstOrDefault(a => a.ChaveAtivacao == cliente.chave);
                    if (ativacao == null)
                        context.Response.Write($"{cliente.data.ToString("dd/MM/yyyy HH:mm:ss")} {cliente.chave} {cliente.versao}\t{cliente.status}\t{(cliente.erros > 0 ? cliente.erros.ToString() : "")}\t???\r\n");
                    else
                    {
                        if (outros != null)
                        {
                            int i = outros.FindIndex(a => a.IDAtivacao == ativacao.IDAtivacao);
                            if (i >= 0)
                                outros.RemoveAt(i);
                        }

                        string versao = cliente.versao;
                        string status = cliente.status ?? "";

                        if (string.IsNullOrEmpty(versao))
                            versao = AchaVersao(ativacao);

                        context.Response.Write($"{cliente.data.ToString("dd/MM/yyyy HH:mm:ss")} {cliente.chave} - {versao}\t{status}\t{(cliente.erros > 0 ? cliente.erros.ToString() : "")}\t{ativacao?.Cliente?.Nome ?? "?"}\r\n");
                    }
                }

                context.Response.Write($"{sw.ElapsedMilliseconds}ms\r\n\r\n");
                sw.Restart();

                if (outros != null)
                {
                    context.Response.Write($"Outros: {outros.Count}\r\n\r\n");

                    foreach (var ativacao in outros)
                    {
                        string versao = AchaVersao(ativacao);
                        context.Response.Write($"{ativacao.DtUltimaVerificacao.Value.ToString("dd/MM/yyyy HH:mm:ss")} {ativacao.ChaveAtivacao} - {versao}\t{ativacao?.Cliente?.Nome ?? "?"}\r\n");
                    }

                    context.Response.Write($"{sw.ElapsedMilliseconds}ms\r\n\r\n");
                }
            }
        }

        public string AchaVersao(Entities.Ativacao ativacao)
        {
            // Caixa mais recente
            var pdv = ativacao.PDVs
                .Where(p => (p.IDTipoPDV == 1 || p.IDTipoPDV == 10)
                    && !string.IsNullOrEmpty(p.Versao))
                .Select(p => new Version(p.Versao))
                .OrderByDescending(v => v)
                .FirstOrDefault();

            return (pdv?.ToString() ?? "?") + "?";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}