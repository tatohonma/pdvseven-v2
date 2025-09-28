using System.Collections.Specialized;
using System.Net.Mail;
using a7D.PDV.Ativacao.API.Enums;
using a7D.PDV.Ativacao.API.Model;

namespace a7D.PDV.Ativacao.API.Services.EmailService;

public interface IEmailService
{
    Task<string> SendUserAsync(ETipoEmailUsuario emailType, AppUser user, IDictionary<string, string?>? data = null);
    Task<string> SendActivationAsync(ETipoEmailAtivacao emailType, Activation activation, AppUser user);

    Task<string> SendErrorAsync(
        string errorRecipients, string activationKey, string application,
        string version, int pdvId, string code, string error, string stackTrace, string payload);

    Task<string> SendTemplateAsync(
        string recipients, string subject, string templateHtmlPath,
        NameValueCollection replacements, Attachment? attachment = null);

    Task<string> SendAsync(string recipients, string subject, string body, Attachment? attachment = null, bool html = true);
    Task<string> SendAsync(MailMessage email);
}
