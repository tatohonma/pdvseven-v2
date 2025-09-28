using System.Collections.Specialized;
using System.Net;
using System.Net.Mail;
using System.Text;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Enums;
using a7D.PDV.Ativacao.API.Model;
using a7D.PDV.Ativacao.API.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace a7D.PDV.Ativacao.API.Services.EmailService;

public sealed class EmailService : IEmailService
{
    readonly ApplicationDbContext _db;
    readonly SmtpOptions _opt;
    readonly ILogger<EmailService> _logger;

    public EmailService(
        ApplicationDbContext db,
        IOptions<SmtpOptions> opt,
        ILogger<EmailService> logger)
    {
        _db = db;
        _opt = opt.Value;
        _logger = logger;
    }

    // --------- Public API

    public async Task<string> SendUserAsync(ETipoEmailUsuario emailType, AppUser user, IDictionary<string, string?>? data = null)
    {
        var subject = SubjectForUser(emailType);
        var body    = BodyForUser(emailType, user, data);

        return await SendAsync(user.Email!, subject, body);
    }

    public async Task<string> SendActivationAsync(ETipoEmailAtivacao emailType, Activation activation, AppUser user)
    {
        var recipients = RecipientsForActivation(emailType);
        var subject    = SubjectForActivation(emailType, activation.Client.Name);
        var body       = BodyForActivation(emailType, activation, user);

        return await SendAsync(recipients, subject, body);
    }

    public async Task<string> SendErrorAsync(
        string errorRecipients, string activationKey, string application,
        string version, int pdvId, string code, string error, string stackTrace, string payload)
    {
        string clientName = "?";
        string pdvName = "?";

        try
        {
            var activation = await _db.Activations
                .Include(a => a.Client)
                .Include(a => a.PDVs)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ActivationKey == activationKey);

            if (activation != null)
            {
                clientName = activation.Client?.Name ?? "???";
                pdvName = activation.PDVs.FirstOrDefault(p => p.InstallationPdvId == pdvId)?.Name ?? "?";
            }
        }
        catch (Exception ex)
        {
            clientName = $"Não foi possível obter o cliente: {ex.Message}";
            _logger.LogError(ex, "Falha ao consultar ativação para e-mail de erro");
        }

        var subject = $"[ERRO PDV7] {version} {error}".Trim();
        var body = $@"
            ERRO: <b>{HtmlEncode(code)}</b><br/>
            ChaveAtivacao: {HtmlEncode(activationKey)} <b>{HtmlEncode(clientName)}</b><br/>
            Versão: {HtmlEncode(version)}<br/>
            Erro: {HtmlEncode(error)}</br>
            Aplicação: {HtmlEncode(application)}<br/>
            IDPDV: {pdvId} {HtmlEncode(pdvName)}<br/>
            StackTrace: <pre>{HtmlEncode(stackTrace)}</pre><br/>
            Dados: <pre>{HtmlEncode(payload)}</pre>";

        return await SendAsync(errorRecipients, subject, body);
    }

    public async Task<string> SendTemplateAsync(
        string recipients, string subject, string templateHtmlPath,
        NameValueCollection replacements, Attachment? attachment = null)
    {
        if (!File.Exists(templateHtmlPath))
            return $"Erro: arquivo de template '{templateHtmlPath}' não existe";

        string body = await File.ReadAllTextAsync(templateHtmlPath);
        foreach (string key in replacements.Keys)
        {
            body = body.Replace(key, replacements[key]?.ToString() ?? string.Empty);
        }

        return await SendAsync(recipients, subject, body, attachment);
    }

    public async Task<string> SendAsync(string recipients, string subject, string body, Attachment? attachment = null, bool html = true)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_opt.From))
                throw new InvalidOperationException("SMTP.From não configurado");

            if (string.IsNullOrWhiteSpace(recipients))
                throw new ArgumentException("Destinatários não informados", nameof(recipients));

            subject = SanitizeSubject(subject, _opt.MaxSubjectLength);

            using var email = new MailMessage
            {
                From = new MailAddress(_opt.From),
                Subject = subject,
                IsBodyHtml = html,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                Body = body
            };

            foreach (var dest in SplitEmails(recipients))
                email.To.Add(dest);

            if (attachment != null)
                email.Attachments.Add(attachment);

            return await SendAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao preparar envio de e-mail");
            return FlattenError(ex);
        }
    }

    public async Task<string> SendAsync(MailMessage email)
    {
        try
        {
            using var client = BuildSmtpClient();
            await client.SendMailAsync(email);
            return "OK";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao enviar e-mail");
            return FlattenError(ex);
        }
    }

    // --------- Internals

    SmtpClient BuildSmtpClient()
    {
        var client = new SmtpClient(_opt.Host, _opt.Port)
        {
            EnableSsl = _opt.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        if (!string.IsNullOrWhiteSpace(_opt.User))
            client.Credentials = new NetworkCredential(_opt.User, _opt.Password);

        return client;
    }

    static IEnumerable<string> SplitEmails(string emails)
        => emails.Split(';', ',')
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x));

    static string SanitizeSubject(string subject, int maxLen)
    {
        subject ??= string.Empty;
        subject = subject.Replace("\r", " ").Replace("\n", " ").Trim();
        if (subject.Length > Math.Max(10, maxLen))
            subject = subject.Substring(0, maxLen) + "...";
        return subject;
    }

    static string HtmlEncode(string? s)
        => System.Net.WebUtility.HtmlEncode(s ?? string.Empty);

    string BaseHttpUrl() => $"http://{_opt.BaseUrl}".TrimEnd('/');

    // --------- Subjects/Bodies/Recipients

    string SubjectForActivation(ETipoEmailAtivacao type, string clientName)
    {
        var prefix = type switch
        {
            ETipoEmailAtivacao.AtivacaoOffline     => "[Ativação Offline] ",
            ETipoEmailAtivacao.LiberacaoTemporaria => "[Liberação Temporária] ",
            ETipoEmailAtivacao.Duplicidade         => "[Duplicidade] ",
            _ => string.Empty
        };
        return $"{prefix}{clientName}";
    }

    static string SubjectForUser(ETipoEmailUsuario type) => type switch
    {
        ETipoEmailUsuario.EsqueciASenha => "Recupere sua senha",
        ETipoEmailUsuario.NovoCadastro  => "Complete seu cadastro",
        _ => "Controle de licenças PDVSeven"
    };

    string BodyForUser(ETipoEmailUsuario type, AppUser user, IDictionary<string, string?>? data) => type switch
    {
        ETipoEmailUsuario.EsqueciASenha =>
            BuildLinkHtml("Clique aqui para redefinir sua senha",
                BuildRouteWithToken("/#/cadastro/redefinir-senha", data)),

        ETipoEmailUsuario.NovoCadastro =>
            BuildLinkHtml("Clique aqui para terminar o cadastro",
                BuildRouteWithToken("/#/cadastro/confirmar-email", data)),

        _ => string.Empty
    };

    string BodyForActivation(ETipoEmailAtivacao type, Activation activation, AppUser? user) => type switch
    {
        ETipoEmailAtivacao.AtivacaoOffline =>
            $@"{HtmlEncode(user?.Name)} gerou uma chave de Ativação Offline para
               <a href='{BaseHttpUrl()}/#/ativacoes/edit/{activation.Id}'>{HtmlEncode(activation.Client.Name)}</a>",

        ETipoEmailAtivacao.LiberacaoTemporaria =>
            $@"{HtmlEncode(user?.Name)} liberou temporariamente o cliente
               <a href='{BaseHttpUrl()}/#/ativacoes/edit/{activation.Id}'>{HtmlEncode(activation.Client.Name)}</a> por 3 dias úteis.",

        ETipoEmailAtivacao.Duplicidade =>
            $@"A Chave {HtmlEncode(activation?.ActivationKey)} está em duplicidade
               <a href='{BaseHttpUrl()}/#/ativacoes/edit/{activation.Id}'>{HtmlEncode(activation.Client.Name)}</a>.",

        _ => string.Empty
    };

    string RecipientsForActivation(ETipoEmailAtivacao type) => type switch
    {
        ETipoEmailAtivacao.AtivacaoOffline     => _opt.Recipients.Offline,
        ETipoEmailAtivacao.LiberacaoTemporaria => _opt.Recipients.Temporaria,
        ETipoEmailAtivacao.Duplicidade         => _opt.Recipients.Duplicidade,
        _ => string.Empty
    };

    static string FlattenError(Exception ex)
    {
        var sb = new StringBuilder();
        for (var e = ex; e != null; e = e.InnerException)
        {
            sb.AppendLine(e.Message);
            sb.AppendLine(e.StackTrace);
        }
        return sb.ToString();
    }

    // --------- Helpers for tokenized links

    string BuildRouteWithToken(string relativePath, IDictionary<string, string?>? data)
    {
        var baseUrl = BaseHttpUrl();
        if (data is null)
            return $"{baseUrl}{relativePath}";

        var userId = data.TryGetValue("UserId", out var uid) ? uid : null;
        var token  = data.TryGetValue("Token", out var tk) ? tk : null;

        var qs = new List<string>();
        if (!string.IsNullOrWhiteSpace(userId)) qs.Add($"userId={WebUtility.UrlEncode(userId)}");
        if (!string.IsNullOrWhiteSpace(token))  qs.Add($"token={WebUtility.UrlEncode(token)}");

        var query = qs.Count > 0 ? "?" + string.Join("&", qs) : string.Empty;
        return $"{baseUrl}{relativePath}{query}";
    }

    static string BuildLinkHtml(string text, string href)
        => $"<a href='{href}'>{HtmlEncode(text)}</a>";
}
