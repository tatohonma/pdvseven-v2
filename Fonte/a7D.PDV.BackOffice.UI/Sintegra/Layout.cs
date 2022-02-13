using Impactro.Layout;

// http://www.sintegra.gov.br/download.html
// https://www.devmedia.com.br/formato-dos-registros-do-sintegra/37226
// https://www.confaz.fazenda.gov.br/legislacao/convenios/1995/CV057_95_manualOrien2
namespace a7D.PDV.BackOffice.UI.Sintegra
{
    /// <summary>
    /// REGISTRO 10 Mestre do estabelecimento
    /// </summary>
    [RegLayout(@"^10400013\d{5}R", DateFormat8 ="yyyyMMdd")]
    public enum Reg10
    {
        [RegFormat(RegType.P9, 2, Default = "10")] // 1-2
        Tipo,

        [RegFormat(RegType.P9, 14)] // 3-16
        CNPJ,

        [RegFormat(RegType.PX, 14)] // 17-30
        IE,

        [RegFormat(RegType.PX, 35)] // 31-65
        nomeContribuinte,

        [RegFormat(RegType.PX, 30)] // 66-95
        municipio,

        [RegFormat(RegType.PX, 2)] // 96-97
        UF,

        [RegFormat(RegType.P9, 10)] // 98-107
        FAX,

        [RegFormat(RegType.PD, 8)] // 108-115
        dtInicial,

        [RegFormat(RegType.PD, 8)] // 116-123
        dtFinal,

        [RegFormat(RegType.PX, 1)]  // 124-124 (Código da identificação da estruturado arquivo magnético entregue, conforme tabela abaixo)
        codIDEstruturaMag,

        ////[RegFormat(RegType.PX, 1)]  // 124-124 (Código da identificação do convenio da estruturado arquivo magnético entregue, conforme tabela abaixo)
        //codIDConvenio,

        [RegFormat(RegType.PX, 1)]  // 125-125 
        codIDNaturezaOp,

        [RegFormat(RegType.PX, 1)]  // 126-126 
        codFinalidadeArquivMag,
    }

    /// <summary>
    /// REGISTRO 11 Dados Complementares do Informante
    /// </summary>
    [RegLayout(@"^10400013\d{5}R", DateFormat8 = "yyyyMMdd")]
    public enum Reg11
    {
        [RegFormat(RegType.P9, 2, Default = "11")] // 1-2
        Tipo,

        [RegFormat(RegType.PX, 34)] // 3-36
        Logradouro,

        [RegFormat(RegType.P9, 5)] // 37-41
        num,

        [RegFormat(RegType.PX, 22)] // 42-63
        complemento,

        [RegFormat(RegType.PX, 15)] // 64-78
        bairro,

        [RegFormat(RegType.P9, 8)] // 79-86
        CEP,

        [RegFormat(RegType.PX, 28)] // 87-114
        nomeContato,

        [RegFormat(RegType.P9, 12)] // 115-126
        telefone,
    }

    /// <summary>
    /// REGISTRO 50 Notas fiscais de compras e vendas
    /// </summary>
    [RegLayout(@"^10400013\d{5}R", DateFormat8 = "yyyyMMdd")]
    public enum Reg50
    {
        [RegFormat(RegType.P9, 2, Default = "50")] // 1-2
        Tipo,

        [RegFormat(RegType.P9, 14)] // 3-16
        CNPJ,

        [RegFormat(RegType.PX, 14)] // 17-30
        IE,

        [RegFormat(RegType.PD, 8)] // 31-38
        dtEmissao,

        [RegFormat(RegType.PX, 2)] // 39-40
        UF,

        [RegFormat(RegType.P9, 2)] // 41-42
        modelo,

        [RegFormat(RegType.PX, 3)] // 43-45
        serieNotaFiscal,

        [RegFormat(RegType.P9, 6)] // 46-51
        numNotaFiscal,

        [RegFormat(RegType.P9, 4)] // 52-55
        CFOP,

        [RegFormat(RegType.PX, 1)] // 56-56
        emitente,

        [RegFormat(RegType.PV, 13)] // 57-69
        valorTotal,

        [RegFormat(RegType.PV, 13)] // 70-82
        baseCalcICMS,

        [RegFormat(RegType.PV, 13)] // 83-95
        vlrICMS,

        [RegFormat(RegType.PV, 13)] // 96-108
        InsentaOuN,

        [RegFormat(RegType.PV, 13)] // 109-121
        outras,

        [RegFormat(RegType.PV, 4)] // 122-125
        aliquota,

        [RegFormat(RegType.PX, 1)] // 126-126
        situacao, // Emissão ou cancelamento

    }

    /// <summary>
    /// REGISTRO 53 Substituicao tributaria
    /// </summary>
    [RegLayout(@"^10400013\d{5}R", DateFormat8 = "ddMMyyyy")]
    public enum Reg53
    {
        [RegFormat(RegType.P9, 2, Default = "53")] // 1-2
        Tipo,

        [RegFormat(RegType.P9, 14)] // 3-16
        CNPJ,

        [RegFormat(RegType.PX, 14)] // 17-30
        IE,

        [RegFormat(RegType.PD, 8)] // 31-38
        dtEmissao,

        [RegFormat(RegType.PX, 2)] // 39-40
        UF,

        [RegFormat(RegType.P9, 2)] // 41-42
        modelo,

        [RegFormat(RegType.PX, 3)] // 43-45
        serie,

        [RegFormat(RegType.PX, 6)] // 46-51
        num,

        [RegFormat(RegType.P9, 4)] // 52-55
        CFOP,

        [RegFormat(RegType.PV, 1)] // 56-56
        emitente,

        [RegFormat(RegType.PV, 13)] // 57-69
        baseCalcICMS,

        [RegFormat(RegType.PV, 13)] // 85-95
        despesasAcessorias,

        [RegFormat(RegType.PX, 1)] // 96-96
        situacao,

        [RegFormat(RegType.PX, 1)] // 96-96
        situacaoCancelamento,

        [RegFormat(RegType.PX, 1)] // 97-97
        codigoAntecipacao,

        [RegFormat(RegType.PX, 30)] // 97-126
        brancos,

        [RegFormat(RegType.PX, 29)] // 98-126
        white,
    }

    /// <summary>
    /// REGISTRO 54 itens das Notas Fiscais
    /// </summary>
    [RegLayout(@"^10400013\d{5}R", DateFormat8 = "ddMMyyyy")]
    public enum Reg54
    {
        [RegFormat(RegType.P9, 2, Default = "54")] // 1-2
        Tipo,

        [RegFormat(RegType.P9, 14)] // 3-16
        CNPJ,

        [RegFormat(RegType.P9, 2)] // 17-18
        modelo,

        [RegFormat(RegType.PX, 3)] // 19-21
        serieNota,

        [RegFormat(RegType.P9, 6)] // 22-27
        numNota,

        [RegFormat(RegType.P9, 4)] // 28-31
        CFOP,

        [RegFormat(RegType.PX, 3)] // 32-34
        CST,

        [RegFormat(RegType.P9, 3)] // 35-37
        numItem,

        [RegFormat(RegType.PX, 14)] // 38-51
        codigoProduto,

        [RegFormat(RegType.P9, 11)] // 52-62
        qtdProduto,

        [RegFormat(RegType.PV, 12)] // 63-74
        vlrProduto,

        [RegFormat(RegType.PV, 12)] // 75-86
        vlrDesc,

        [RegFormat(RegType.PV, 12)] // 87-98
        baseCalcICMS,

        [RegFormat(RegType.P9, 12)] // 99-110
        baseCalcICMSSubstituicao,

        [RegFormat(RegType.P9, 12)] // 111-122
        valorIPI,

        [RegFormat(RegType.P9, 4)] // 123-126
        aliquotaICMS,
    }

    /// <summary>
    /// REGISTRO 61 - Documentos fiscais não emitidos por equipamento emissor de cupom fiscal
    /// </summary>
    [RegLayout(@"^10400013\d{5}R", DateFormat8 = "ddMMyyyy")]
    public enum Reg61
    {
        [RegFormat(RegType.P9, 2, Default = "61")] // 1-2
        Tipo,

        [RegFormat(RegType.PX, 14)] // 3-16
        branco,

        [RegFormat(RegType.PX, 14)] // 17-30
        brancos,

        [RegFormat(RegType.PD, 8)] // 31-38
        dtEmissao,

        [RegFormat(RegType.P9, 2)] // 39-40
        modelo,

        [RegFormat(RegType.PX, 3)] // 41-43
        serie,

        [RegFormat(RegType.PX, 2)] // 44-45
        subserie,

        [RegFormat(RegType.P9, 6)] // 46-51
        numInicial,

        [RegFormat(RegType.P9, 6)] // 52-57
        numFinal,

        [RegFormat(RegType.P9, 13)] // 58-70
        vlrTotal,

        [RegFormat(RegType.P9, 13)] // 71-83
        baseCalcICMS,

        [RegFormat(RegType.PV, 12)] // 84-95
        vlrICMS,

        [RegFormat(RegType.PV, 13)] // 96-108
        insentaouN,

        [RegFormat(RegType.PV, 13)] // 109-121
        outras,

        [RegFormat(RegType.PV, 4)] // 122-125
        aliquota,

        [RegFormat(RegType.PX, 1)] // 126-126
        white,
    }

    /// <summary>
    /// REGISTRO 61R - Resumo Mensal por Item : Registro de mercadoria/produto ou serviço comercializados através de nota fiscal de venda a consumidor não emitida por ECF
    /// </summary>
    [RegLayout(@"^10400013\d{5}R", DateFormat8 = "ddMMyyyy")]
    public enum Reg61R
    {
        [RegFormat(RegType.P9, 2, Default = "61")] // 1-2
        Tipo,

        [RegFormat(RegType.PX, 1, Default = "R")] // 3-3
        mestreAnaliticResumo,

        [RegFormat(RegType.P9, 6)] // 4-9
        mesAnoEmissao,

        [RegFormat(RegType.PX, 14)] // 10-23
        codProduto,

        [RegFormat(RegType.P9, 13)] // 24-36
        qtd,

        [RegFormat(RegType.PV, 16)] // 37-52
        vlrBrutoProduto,

        [RegFormat(RegType.PV, 16)] // 53-68
        baseCalcICMS,

        [RegFormat(RegType.P9, 4)] // 69 -9
        aliquotaProduto,

        [RegFormat(RegType.PX, 54)] // 73-126
        branco,
    }

    /// <summary>
    /// REGISTRO 90 - Totalização do arquivo
    /// </summary>
    [RegLayout(@"^10400013\d{5}R", DateFormat8 = "ddMMyyyy")]
    public enum Reg90
    {
        [RegFormat(RegType.P9, 2, Default = "90")] // 1-2
        Tipo,

        [RegFormat(RegType.P9, 14)] // 3-16
        CNPJ,

        [RegFormat(RegType.PX, 14)] // 17-30
        IE,

        [RegFormat(RegType.P9, 2)] // 31-32
        tipoTotalizado,

        [RegFormat(RegType.P9, 8)] // 33-40
        totalRegistro,

        [RegFormat(RegType.P9, 8)] // 41-49
        totalGeral,

        [RegFormat(RegType.P9, 1)] // 126-126
        numeroRegistro90,
    }
}
