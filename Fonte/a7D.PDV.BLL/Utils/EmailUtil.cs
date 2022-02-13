namespace a7D.PDV.BLL.Utils
{
    public static class EmailUtil
    {
        //public static void EnviarEmail(InformacoesRemetente informacoesRemetente, string assunto, string mensagem, string destinatario, 
        //    Attachment anexo = null, string ccopia = null)
        //{
        //    var email = new MailMessage();

        //    try
        //    {
        //        email.To.Add(destinatario);
        //        if (ccopia != null)
        //            email.CC.Add(ccopia);

        //        email.Priority = MailPriority.High;
        //        email.IsBodyHtml = false;
        //        email.Subject = assunto;
        //        email.Body = mensagem;
        //        email.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
        //        email.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

        //        if (anexo != null)
        //            email.Attachments.Add(anexo);

        //        DispararEmail(informacoesRemetente, email);
        //    }
        //    finally
        //    {
        //        email.Dispose();
        //    }
        //}

        //public static void EnviarEmailHtml(InformacoesRemetente informacoesRemetente, string assunto, string destinatario, string templateHtml, ListDictionary ldReplacements, bool enviarParaHotel = false, Attachment anexo = null, string ccopia = null)
        //{
        //    MailMessage email = null;

        //    try
        //    {
        //        var emailDestinatario = destinatario;
        //        var mailDefinition = new MailDefinition
        //        {
        //            BodyFileName = templateHtml,
        //            From = "fake@fake.com"
        //        };

        //        email = mailDefinition.CreateMailMessage(emailDestinatario, ldReplacements, new Control());
        //        email.Priority = MailPriority.High;
        //        email.IsBodyHtml = true;
        //        email.Subject = assunto;
        //        email.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
        //        email.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");

        //        if (anexo != null)
        //            email.Attachments.Add(anexo);

        //        if (!string.IsNullOrEmpty(ccopia))
        //            email.CC.Add(ccopia);

        //        DispararEmail(informacoesRemetente, email);
        //    }
        //    finally
        //    {
        //        email?.Dispose();
        //    }
        //}

        //private static void DispararEmail(InformacoesRemetente informacoesRemetente, MailMessage email)
        //{
        //    try
        //    {
        //        var emailRemetente = informacoesRemetente.Remetente;
        //        var smtpHost = informacoesRemetente.Host;
        //        var smtpPorta = informacoesRemetente.Porta;
        //        var smtpUsuario = informacoesRemetente.Usuario;
        //        var smtpSenha = informacoesRemetente.Senha;
        //        var smtpUseDefaultCredentials = informacoesRemetente.UseDefaultCredentials;
        //        var smtpEnableSsl = informacoesRemetente.EnableSsl;

        //        email.From = new MailAddress(emailRemetente);
        //        var smtp = new SmtpClient
        //        {
        //            UseDefaultCredentials = smtpUseDefaultCredentials,
        //            Credentials = new NetworkCredential(smtpUsuario, smtpSenha),
        //            Host = smtpHost,
        //            Port = smtpPorta,
        //            EnableSsl = smtpEnableSsl
        //        };

        //        smtp.Send(email);
        //    }
        //    finally
        //    {
        //        email.Dispose();
        //    }
        //}

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
