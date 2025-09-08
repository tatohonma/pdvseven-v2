namespace a7D.PDV.Ativacao.API.Options;

public class ErrorNotifyOptions
{
    public bool SendEmailOnUnhandled { get; set; } = false;
    public string? Recipients { get; set; }
    public int MaxBodyBytes { get; set; } = 64_000;
}
