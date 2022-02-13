namespace a7D.PDV.Integracao.Pagamento.NTKTEF
{
    /// <summary>
    /// 6.2. Comandos existentes de Status, e resposta
    /// </summary>
    public enum NTKComandos
    {
        /// <summary>
        /// Verifica se o Pay&Go está ativo (Status)
        /// </summary>
        ATV,

        /// <summary>
        /// Realiza uma transação de Venda (Status e Resposta)
        /// </summary>
        CRT,

        /// <summary>
        /// Realiza uma transação Administrativa (Status e Resposta)
        /// </summary>
        ADM,

        /// <summary>
        /// Realiza uma transação de Cancelamento (Status e Resposta)
        /// </summary>
        CNC,

        /// <summary>
        /// Confirma a última transação realizada (Status)
        /// </summary>
        CNF,

        /// <summary>
        /// Desfaz a última transação realizada (Status)
        /// </summary>
        NCN,

        /// <summary>
        /// Captura um dado pessoal no PIN-pad (Status e Resposta)
        /// </summary>
        CDP
    }
}
