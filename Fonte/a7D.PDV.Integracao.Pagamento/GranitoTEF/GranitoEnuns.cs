namespace a7D.PDV.Integracao.Pagamento.GranitoTEF
{
    public enum PG_Retorno
    {
        /// <summary>
        /// Método finalizado com sucesso.
        /// </summary>
        END = 0,

        /// <summary>
        /// Transação finalizada e não espera novas solicitação com dados para transação atual. O pdv deve realizar nova chamada para finalização e obter retorno 0.
        /// </summary>
        TRN_END = 1,

        /// <summary>
        /// Transação aprovada. Os cupons são disponibilizados nas variáveis comp_estab e comp_cliente.
        /// </summary>
        TRN_OK = 2,

        /// <summary>
        /// Transação não aprovada. Verificar variáveis cod_resposta e desc_resposta. O pdv deve realizar nova chamada para finalização e obter retorno 0.
        /// </summary>
        TRN_NOK = 3,

        /// <summary>
        /// Operação em processamento. Deve-se avaliar a resposta e realizar nova chamada do método.
        /// </summary>
        PROCESSING = 4,
    }

    public enum PG_Rede
    {
        PAGO_GP = 7,
        PAGO_CIELO = 8
    }

    public enum PG_Bandeiras
    {
        VISA = 1,
        VISAELECTRON = 2,
        MASTERCARD = 3,
        MAESTRO = 4,
        ELO_CREDITO = 5,
        ELO_DEBITO = 6
    }

    public enum PG_TipoOperacao
    {
        VendaPresencial = 1,
        VendaDigitada = 3
    }

    public enum PG_TipoTransacao
    {
        Credito = 1,
        Debito = 2
    }

    public enum PG_TipoForma
    {
        AVista = 1,
        Parcelado = 2
    }

    public enum PG_TipoModalidade
    {
        AVista = 0,
        SemJuros = 1,
        ComJuros = 2
    }
}
