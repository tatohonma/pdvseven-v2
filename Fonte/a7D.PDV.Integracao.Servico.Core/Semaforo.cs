namespace a7D.PDV.Integracao.Servico.Core
{
    public enum Semaforo : uint
    {
        Rodando = 0xFF00FF00,           // Verde
        Parado = 0xFFFF0000,            // Vermelho
        Desabilitado = 0xFFC0C0C0,      // Zinza
        Iniciado = 0xFFFF9900,          // Laranja
        NaoConfigurado = 0xFFFFFF00,    // Amarelo
    }
}
