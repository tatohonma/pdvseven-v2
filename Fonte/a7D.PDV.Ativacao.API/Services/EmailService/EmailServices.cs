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

    // --------- API pública

    public async Task<string> EnviarUsuarioAsync(ETipoEmailUsuario tipoEmail, Usuario usuario)
    {
        var assunto = AssuntoUsuario(tipoEmail);
        var corpo   = CorpoUsuario(tipoEmail, usuario);

        return await EnviarAsync(usuario.Email, assunto, corpo);
    }

    public async Task<string> EnviarAtivacaoAsync(ETipoEmailAtivacao tipoEmail, Activation ativacao, Usuario usuario)
    {
        var destinatarios = DestinatariosAtivacao(tipoEmail);
        var assunto = AssuntoAtivacao(tipoEmail, ativacao.Client.Name);
        var corpo   = CorpoAtivacao(tipoEmail, ativacao, usuario);

        return await EnviarAsync(destinatarios, assunto, corpo);
    }

    public async Task<string> EnviarErroAsync(
        string errosDestinatarios, string chaveAtivacao, string aplicacao,
        string versao, int idPDV, string codigo, string erro, string stackTrace, string dados)
    {
        string cliente = "?";
        string pdv = "?";

        try
        {
            var ativacao = await _db.Activations
                .Include(a => a.Client)
                .Include(a => a.PDVs)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.ActivationKey == chaveAtivacao);

            if (ativacao != null)
            {
                cliente = ativacao.Client?.Name ?? "???";
                pdv = ativacao.PDVs.FirstOrDefault(p => p.InstallationPdvId == idPDV)?.Name ?? "?";
            }
        }
        catch (Exception ex)
        {
            cliente = $"Não foi possível obter o cliente: {ex.Message}";
            _logger.LogError(ex, "Falha ao consultar ativação para email de erro");
        }

        var titulo = $"[ERRO PDV7] {versao} {erro}".Trim();
        var body = $@"
                    ERRO: <b>{HtmlEncode(codigo)}</b><br/>
                    ChaveAtivacao: {HtmlEncode(chaveAtivacao)} <b>{HtmlEncode(cliente)}</b><br/>
                    Versão: {HtmlEncode(versao)}<br/>
                    Erro: {HtmlEncode(erro)}</br>
                    Aplicacao: {HtmlEncode(aplicacao)}<br/>
                    IDPDV: {idPDV} {HtmlEncode(pdv)}<br/>
                    StackTrace: <pre>{HtmlEncode(stackTrace)}</pre><br/>
                    Dados: <pre>{HtmlEncode(dados)}</pre>";

        return await EnviarAsync(errosDestinatarios, titulo, body);
    }

    public async Task<string> EnviarTemplateAsync(
        string destinatarios, string titulo, string templateHtmlPath,
        NameValueCollection replacements, Attachment? attach = null)
    {
        if (!File.Exists(templateHtmlPath))
            return $"Erro arquivo template '{templateHtmlPath}' não existe";

        string body = await File.ReadAllTextAsync(templateHtmlPath);
        foreach (string key in replacements.Keys)
        {
            body = body.Replace(key, replacements[key]?.ToString() ?? string.Empty);
        }

        return await EnviarAsync(destinatarios, titulo, body, attach);
    }

    public async Task<string> EnviarAsync(string destinatarios, string titulo, string body, Attachment? attach = null, bool html = true)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_opt.From))
                throw new InvalidOperationException("SMTP.From não configurado");

            if (string.IsNullOrWhiteSpace(destinatarios))
                throw new ArgumentException("Destinatários não informados", nameof(destinatarios));

            titulo = SanitizeSubject(titulo, _opt.MaxSubjectLength);

            using var email = new MailMessage
            {
                From = new MailAddress(_opt.From),
                Subject = titulo,
                IsBodyHtml = html,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                Body = body
            };

            foreach (var dest in SplitEmails(destinatarios))
                email.To.Add(dest);

            if (attach != null)
                email.Attachments.Add(attach);

            return await EnviarAsync(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao preparar envio de e-mail");
            return FlattenError(ex);
        }
    }

    public async Task<string> EnviarAsync(MailMessage email)
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
        if (subject.Length > Math.Max(10, maxLen)) // evita truncar demais se config vier baixa
            subject = subject.Substring(0, maxLen) + "...";
        return subject;
    }

    static string HtmlEncode(string? s)
        => System.Net.WebUtility.HtmlEncode(s ?? string.Empty);

    string BaseUrlHttp() => $"http://{_opt.BaseUrl}".TrimEnd('/');
    // se quiser forçar https: $"https://{_opt.BaseUrl}"

    // --------- Montagem de Assuntos/Corpos/Destinatários

    private string AssuntoAtivacao(ETipoEmailAtivacao tipo, string cliente)
    {
        var prefixo = tipo switch
        {
            ETipoEmailAtivacao.AtivacaoOffline   => "[Ativação Offline] ",
            ETipoEmailAtivacao.LiberacaoTemporaria => "[Liberação Temporária] ",
            ETipoEmailAtivacao.Duplicidade       => "[Duplicidade] ",
            _ => string.Empty
        };
        return $"{prefixo}{cliente}";
    }

    static string AssuntoUsuario(ETipoEmailUsuario tipo) => tipo switch
    {
        ETipoEmailUsuario.EsqueciASenha => "Recupere sua senha",
        ETipoEmailUsuario.NovoCadastro  => "Complete seu cadastro",
        _ => "Controle de licenças PDVSeven"
    };

    string CorpoUsuario(ETipoEmailUsuario tipo, Usuario usuario) => tipo switch
    {
        ETipoEmailUsuario.EsqueciASenha =>
            $@"<a href='{BaseUrlHttp()}/#/cadastro/{usuario.HashAlterarSenha}'>Clique aqui para redefinir sua senha</a>",
        ETipoEmailUsuario.NovoCadastro =>
            $@"<a href='{BaseUrlHttp()}/#/cadastro/{usuario.HashAlterarSenha}'>Clique aqui para terminar o cadastro</a>",
        _ => string.Empty
    };

    string CorpoAtivacao(ETipoEmailAtivacao tipo, Activation ativacao, Usuario? usuario) => tipo switch
    {
        ETipoEmailAtivacao.AtivacaoOffline =>
            $@"{HtmlEncode(usuario?.Nome)} acaba de gerar uma chave de Ativação Offline para <a href='{BaseUrlHttp()}/#/ativacoes/edit/{ativacao.Id}'>{HtmlEncode(ativacao.Client.Name)}</a>",
        ETipoEmailAtivacao.LiberacaoTemporaria =>
            $@"{HtmlEncode(usuario?.Nome)} acaba de liberar o cliente <a href='{BaseUrlHttp()}/#/ativacoes/edit/{ativacao.Id}'>{HtmlEncode(ativacao.Client.Name)}</a> por 3 dias úteis.",
        ETipoEmailAtivacao.Duplicidade =>
            $@"A Chave {HtmlEncode(ativacao?.ActivationKey)} está em duplicidade <a href='{BaseUrlHttp()}/#/ativacoes/edit/{ativacao.Id}'>{HtmlEncode(ativacao.Client.Name)}</a>.",
        _ => string.Empty
    };

    string DestinatariosAtivacao(ETipoEmailAtivacao tipo) => tipo switch
    {
        ETipoEmailAtivacao.AtivacaoOffline    => _opt.Recipients.Offline,
        ETipoEmailAtivacao.LiberacaoTemporaria => _opt.Recipients.Temporaria,
        ETipoEmailAtivacao.Duplicidade        => _opt.Recipients.Duplicidade,
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
}