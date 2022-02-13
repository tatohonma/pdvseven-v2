using a7D.PDV.Ativacao.API.Context;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace a7D.PDV.Ativacao.API.Services
{
    public static class EmailServices
    {
        private static readonly string Url = "ativacoespdvseven.azurewebsites.net";

        private static string Assunto(this ETipoEmailAtivacao t, string cliente)
        {
            var assunto = string.Empty;
            switch (t)
            {
                case ETipoEmailAtivacao.AtivacaoOffline:
                    assunto += "[Ativação Offline] ";
                    break;
                case ETipoEmailAtivacao.LiberacaoTemporaria:
                    assunto += "[Liberação Temporária] ";
                    break;
            }

            return assunto + cliente;
        }

        private static string Assunto(this ETipoEmailUsuario t)
        {
            switch (t)
            {
                case ETipoEmailUsuario.EsqueciASenha:
                    return "Recupere sua senha";
                case ETipoEmailUsuario.NovoCadastro:
                    return "Complete seu cadastro";
                default:
                    return "Controle de licenças PDVSeven";
            }
        }

        private static string Corpo(this ETipoEmailUsuario t, Entities.Usuario usuario)
        {
            switch (t)
            {
                case ETipoEmailUsuario.EsqueciASenha:
                    return $@"<a href='http://{Url}/#/cadastro/{usuario.HashAlterarSenha}'>Clique aqui para redefinir sua senha</a>";
                case ETipoEmailUsuario.NovoCadastro:
                    return $@"<a href='http://{Url}/#/cadastro/{usuario.HashAlterarSenha}'>Clique aqui para terminar o cadastro</a>";
                default:
                    return string.Empty;
            }
        }

        private static string Corpo(this ETipoEmailAtivacao t, Entities.Ativacao ativacao, Entities.Usuario usuario)
        {
            switch (t)
            {
                case ETipoEmailAtivacao.AtivacaoOffline:
                    return $@"{usuario?.Nome} acaba de gerar uma chave de Ativação Offline para <a href='http://{Url}/#/ativacoes/edit/{ativacao.IDAtivacao}'>{ativacao.Cliente.Nome}</a>";
                case ETipoEmailAtivacao.LiberacaoTemporaria:
                    return $@"{usuario?.Nome} acaba de liberar o cliente <a href='http://{Url}/#/ativacoes/edit/{ativacao.IDAtivacao}'>{ativacao.Cliente.Nome}</a> por 3 dias úteis.";
                case ETipoEmailAtivacao.Duplicidade:
                    return $@"A Chave {ativacao?.ChaveAtivacao} está em duplicidade <a href='http://{Url}/#/ativacoes/edit/{ativacao.IDAtivacao}'>{ativacao.Cliente.Nome}</a>.";
                default:
                    return string.Empty;
            }
        }

        private static string Destinatarios(this ETipoEmailAtivacao t)
        {
            switch (t)
            {
                case ETipoEmailAtivacao.AtivacaoOffline:
                    return ConfigurationManager.AppSettings["DestinatariosOffline"];
                case ETipoEmailAtivacao.LiberacaoTemporaria:
                    return ConfigurationManager.AppSettings["DestinatariosTemporaria"];
                case ETipoEmailAtivacao.Duplicidade:
                    return ConfigurationManager.AppSettings["DestinatariosDuplicidade"];
                default:
                    return "fabio@pdvseven.com.br";
            }
        }

        public static void EnviarUsuario(ETipoEmailUsuario tipoEmail, Entities.Usuario usuario)
        {
            Enviar(usuario.Email, tipoEmail.Assunto(), tipoEmail.Corpo(usuario));
        }

        public static void EnviarAtivacao(ETipoEmailAtivacao tipoEmail, Entities.Ativacao ativacao, Entities.Usuario usuario)
        {
            Enviar(tipoEmail.Destinatarios(), tipoEmail.Assunto(ativacao.Cliente.Nome), tipoEmail.Corpo(ativacao, usuario));
        }

        internal static string EnviarErro(string errosDestinatarios, string chaveAtivacao, string aplicacao, string versao, int idPDV, string codigo, string erro, string stackTrace, string dados)
        {
            string cliente = "?";
            string pdv = "?";
            try
            {
                using (var db = new AtivacaoContext())
                {
                    var ativacao = db.Ativacoes.FirstOrDefault(a => a.ChaveAtivacao == chaveAtivacao);
                    if (ativacao != null)
                    {
                        cliente = ativacao.Cliente?.Nome ?? "???";
                        pdv = ativacao.PDVs.FirstOrDefault(p => p.IDPDV_instalacao == idPDV)?.Nome;
                    }
                }
            }
            catch (Exception ex)
            {
                cliente = "Não foi possivel obter o cliente: " + ex.Message;
            }

            string titulo = $"[ERRO PDV7] {versao} {erro}";
            string corpo = $@"ERRO: <b>{codigo}</b><br/>
ChaveAtivacao: {chaveAtivacao} <b>{cliente}</b><br/>
Versão: {versao}<br/>
Erro: {erro}<br/>
Aplicacao: {aplicacao}<br/>
IDPDV: {idPDV} {pdv}<br/>
StackTrace: <pre>{stackTrace}</pre><br/>
Dados: <pre>{dados}</pre>";
            return Enviar(errosDestinatarios, titulo, corpo);
        }

        internal static string EnviarTemplate(string destinatarios, string titulo, string templateHTML, ListDictionary ldReplacements, Attachment attach = null)
        {
            string body;
            if (File.Exists(templateHTML))
            {
                body = File.ReadAllText(templateHTML);
                foreach (string key in ldReplacements.Keys)
                    body = body.Replace(key, ldReplacements[key].ToString());
            }
            else
                return $"Erro arquivo template '{templateHTML}' não existe";

            return Enviar(destinatarios, titulo, body, attach);
        }

        public static string Enviar(string destinatarios, string titulo, string body, Attachment attach = null, bool html = true)
        {
            try
            {
                using (var email = new MailMessage())
                {
                    string from = ConfigurationManager.AppSettings["SMTPfrom"];
                    if (string.IsNullOrEmpty(from))
                        throw new Exception("SMTPfrom vazio");

                    titulo = titulo.Replace("\r", " ").Replace("\n", " ");
                    if (titulo.Length > 100)
                        titulo = titulo.Substring(0, 100) + "...";

                    email.From = new MailAddress(from);
                    email.Subject = titulo;
                    email.IsBodyHtml = html;
                    email.SubjectEncoding = Encoding.UTF8;
                    email.BodyEncoding = Encoding.UTF8;
                    email.Body = body;

                    if (string.IsNullOrEmpty(destinatarios))
                        throw new Exception("Destinatários não informado");

                    foreach (string destinatario in destinatarios.Split(';'))
                        email.To.Add(destinatario.Trim());

                    if (attach != null)
                        email.Attachments.Add(attach);

                    return Enviar(email);
                }
            }
            catch (Exception ex)
            {
                string erro = "";
                while (ex != null)
                {
                    erro += ex.Message + "\r\n" + ex.Message + "\r\n" + ex.StackTrace + "\r\n";
                    ex = ex.InnerException;
                }
                return erro;
            }
        }

        public static string Enviar(MailMessage email)
        {
            try
            {
                using (var client = new SmtpClient())
                    client.Send(email);

                return "OK";
            }
            catch (Exception ex)
            {
                string erro = "";
                while (ex != null)
                {
                    erro += ex.Message + "\r\n" + ex.Message + "\r\n" + ex.StackTrace + "\r\n";
                    ex = ex.InnerException;
                }
                return erro;
            }
        }
    }

    public enum ETipoEmailAtivacao
    {
        AtivacaoOffline,
        LiberacaoTemporaria,
        Duplicidade
    }

    public enum ETipoEmailUsuario
    {
        EsqueciASenha,
        NovoCadastro,
    }
}