using System.Collections.Specialized;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using a7D.PDV.Ativacao.Utils.ValueObject;
using System;

namespace a7D.PDV.Ativacao.Utils
{
    public static class Email
    {
        public static void EnviarEmail(InformacoesRemetente informacoesRemetente, string assunto, string mensagem, string destinatario, 
            Attachment anexo = null, string ccopia = null)
        {
            var email = new MailMessage();

            try
            {
                email.To.Add(destinatario);
                if (ccopia != null)
                    email.CC.Add(ccopia);

                email.Priority = MailPriority.High;
                email.IsBodyHtml = false;
                email.Subject = assunto;
                email.Body = mensagem;
                email.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
                email.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

                if (anexo != null)
                    email.Attachments.Add(anexo);

                DispararEmail(informacoesRemetente, email);
            }
            finally
            {
                email.Dispose();
            }
        }

        public static void EnviarEmailHtml(InformacoesRemetente informacoesRemetente, string assunto, string destinatario, string templateHtml, ListDictionary ldReplacements, Attachment anexo = null, string ccopia = null)
        {
            MailMessage email = null;

            try
            {
                var emailDestinatario = destinatario;
                var mailDefinition = new MailDefinition
                {
                    BodyFileName = templateHtml,
                    From = "fake@fake.com"
                };
                
                
               
                email = mailDefinition.CreateMailMessage("fake@fake.com", ldReplacements, new Control());
                email.Priority = MailPriority.High;
                email.IsBodyHtml = true;
                email.Subject = assunto;
                email.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
                email.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

                email.To.Clear();

                foreach (var address in destinatario.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    email.To.Add(address);
                }

                if (anexo != null)
                    email.Attachments.Add(anexo);

                if (!string.IsNullOrEmpty(ccopia))
                    email.CC.Add(ccopia);

                DispararEmail(informacoesRemetente, email);
            }
            finally
            {
                email?.Dispose();
            }
        }

        private static void DispararEmail(InformacoesRemetente informacoesRemetente, MailMessage email)
        {
            try
            {
                var emailRemetente = informacoesRemetente.Remetente;
                var smtpHost = informacoesRemetente.Host;
                var smtpPorta = informacoesRemetente.Porta;
                var smtpUsuario = informacoesRemetente.Usuario;
                var smtpSenha = informacoesRemetente.Senha;
                var smtpUseDefaultCredentials = informacoesRemetente.UseDefaultCredentials;
                var smtpEnableSsl = informacoesRemetente.EnableSsl;

                email.From = new MailAddress(emailRemetente);
                var smtp = new SmtpClient
                {
                    UseDefaultCredentials = smtpUseDefaultCredentials,
                    Credentials = new NetworkCredential(smtpUsuario, smtpSenha),
                    Host = smtpHost,
                    Port = smtpPorta,
                    EnableSsl = smtpEnableSsl
                };

                smtp.Send(email);
            }
            finally
            {
                email.Dispose();
            }
        }
    }
}
