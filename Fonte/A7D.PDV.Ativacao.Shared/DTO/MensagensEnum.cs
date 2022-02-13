namespace a7D.PDV.Ativacao.Shared.DTO
{
    public enum EOrigemDestinoMensagem
    {
        Ativador = 1,
        Integrador = 2,
        Caixa = 3,
        Comanda = 4,
        Cardapio = 5,

        IA = 10
    }

    public enum ETipoMensagem
    {
        Update = 10,        // Iniciar Atualização 
        Update_Erro,        // Houve algum erro descrito no texto da mensagem
        Update_SIM,         // Cliente autorizou 
        Update_ServerStart, // Processo de atualizador iniciado no servidor (integrador)
        Update_ServerOK,    // Integrador e WS atualizados, Migration
        Update_ClienteOK,   // Todos PDV atualizados

        Informacao = 30,
        Abrir_Link = 31,    // Botão ABRIR
        Pergunta_SIMNAO = 32,
        Pergunta_Texto = 33,
        Pergunta_Atualizar = 34, // Pergunda se pode iniciar a atualização
        Cobranca_Link = 35, // Botão PAGAR
        Resposta = 40, // SIM, NÃO, OK, ou resposta digitada

        Config = 70
    }
}