namespace a7D.PDV.Ativacao.API.Options;

public class SmtpOptions
{
    public string Host { get; set; } = default!;
    public int Port { get; set; } = 25;
    public bool EnableSsl { get; set; } = true;
    public string From { get; set; } = default!;
    public string? User { get; set; }
    public string? Password { get; set; }

    public string BaseUrl { get; set; } = default!;

    public RecipientsOptions Recipients { get; set; } = new();
    public int MaxSubjectLength { get; set; } = 100;

    public sealed class RecipientsOptions
    {
        public string Offline { get; set; } = string.Empty;
        public string Temporaria { get; set; } = string.Empty;
        public string Duplicidade { get; set; } = string.Empty;
    }
}
