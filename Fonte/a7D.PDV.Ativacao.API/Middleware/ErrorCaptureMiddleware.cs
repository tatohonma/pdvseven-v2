using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text;
using a7D.PDV.Ativacao.API.Options;
using a7D.PDV.Ativacao.API.Services.EmailService;

public sealed class ErrorCaptureMiddleware : IMiddleware
{
    readonly ILogger<ErrorCaptureMiddleware> _logger;
    readonly IEmailService _email;       
    readonly ErrorNotifyOptions _opt;

    public ErrorCaptureMiddleware(
        ILogger<ErrorCaptureMiddleware> logger,
        IEmailService email,
        IOptions<ErrorNotifyOptions> opt)
    {
        _logger = logger;
        _email = email;
        _opt = opt.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            context.Request.EnableBuffering();
            await next(context);
        }
        catch (Exception ex)
        {
            var url = $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}";
            string body = await ReadBodySafeAsync(context.Request, _opt.MaxBodyBytes);

            _logger.LogError(ex, "Unhandled exception em {url}. Body (trunc): {body}", url, body);

            if (_opt.SendEmailOnUnhandled && !string.IsNullOrWhiteSpace(_opt.Recipients))
            {
                var plain = BuildPlainText(ex, url, body);
                await _email.SendAsync(_opt.Recipients!, "ERRO Ativações", plain, html: false);
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Ocorreu um erro interno.");
        }
    }

    static async Task<string> ReadBodySafeAsync(HttpRequest req, int maxBytes, CancellationToken ct = default)
    {
        if (req.Body.CanSeek)
            req.Body.Seek(0, SeekOrigin.Begin);

        using var ms = new MemoryStream(capacity: Math.Min(maxBytes, 128 * 1024));
        var buffer = new byte[81920];
        var remaining = maxBytes;

        while (remaining > 0)
        {
            var toRead = Math.Min(buffer.Length, remaining);
            var n = await req.Body.ReadAsync(buffer.AsMemory(0, toRead), ct);
            if (n <= 0) break;

            await ms.WriteAsync(buffer.AsMemory(0, n), ct);
            remaining -= n;
        }

        var data = ms.ToArray();
        var text = Encoding.UTF8.GetString(data);

        if (req.Body.CanSeek)
            req.Body.Seek(0, SeekOrigin.Begin);

        return text;
    }

    static string BuildPlainText(Exception ex, string url, string body)
    {
        var sb = new StringBuilder();
        sb.AppendLine("ERRO:");
        sb.AppendLine(url);
        sb.AppendLine();
        var max = 5;
        for (var e = ex; e != null && --max > 0; e = e.InnerException)
        {
            sb.AppendLine(e.Message);
            sb.AppendLine(e.StackTrace);
            sb.AppendLine();
        }
        sb.AppendLine("Body:");
        sb.AppendLine(body);
        return sb.ToString();
    }
}
