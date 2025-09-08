using System.Collections.Specialized;
using System.Net.Mail;
using a7D.PDV.Ativacao.API.Enums;
using a7D.PDV.Ativacao.API.Model;

namespace a7D.PDV.Ativacao.API.Services.EmailService;

public interface IEmailService
{
    Task<string> EnviarUsuarioAsync(ETipoEmailUsuario tipoEmail, Usuario usuario);
    Task<string> EnviarAtivacaoAsync(ETipoEmailAtivacao tipoEmail, Activation ativacao, Usuario usuario);

    Task<string> EnviarErroAsync(
        string errosDestinatarios, string chaveAtivacao, string aplicacao,
        string versao, int idPdv, string codigo, string erro, string stackTrace, string dados);

    Task<string> EnviarTemplateAsync(
        string destinatarios, string titulo, string templateHtmlPath,
        NameValueCollection replacements, Attachment? attach = null);

    Task<string> EnviarAsync(string destinatarios, string titulo, string body, Attachment? attach = null, bool html = true);
    Task<string> EnviarAsync(MailMessage email);
}
