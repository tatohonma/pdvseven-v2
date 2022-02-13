using System.ComponentModel;

namespace a7D.PDV.EF.Enum
{
    public enum EStatusPedido
    {
        Aberto = 10,
        Enviado = 20,
        Desconhecido = 30, // Existem alguns IF para este estado ???
        Finalizado = 40,
        Cancelado = 50,
        [Description("Não Confirmado")]
        NaoConfirmado = 60, // Quando inserido por aplicativos (iFood) e precisa de ação de operador
        [Description("Cancelamento Solicitado")]
        EmCancelamento = 70, // Quando o cliente de aplicativos (iFood) solicita cancelamento e operador precisa regristrar o cancelamento
    }
}
