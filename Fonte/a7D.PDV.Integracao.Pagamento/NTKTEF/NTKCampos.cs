namespace a7D.PDV.Integracao.Pagamento.NTKTEF
{
    /// <summary>
    /// 6.4 Campos previstos para cada comando?
    /// ‘n’ indica um campo contendo exclusivamente caracteres numéricos;
    /// ‘a’ indica um campo podendo conter qualquer caractere permitido, de acordo com “6.1. Formato genérico”;
    /// ‘a7’ indica um campo de exatamente 7 caracteres;
    /// ‘a..5’ indica um campo de 1 a 5 caracteres.
    /// </summary>
    public enum NTKCampos
    {
        /// <summary>
        /// a3 - Identifica o propósito do arquivo, conforme “6.2. Comandos existentes”.
        /// </summary>
        Comando = 0,

        /// <summary>
        /// n..10 - Número de controle gerado pela Automação Comercial, devendo o valor ser diferente para cada nova operação de TEF. É ecoado pelo Pay&Go nos arquivos de status e de resposta, e deve ser consistido pelo Automação Comercial.
        /// </summary>
        Identificacao = 1,

        /// <summary>
        /// n..12 - Número do documento fiscal ao qual a operação de TEF está vinculada. Caso seja usada uma Impressora Fiscal, o preenchimento deste campo é obrigatório para transações de venda.
        /// </summary>
        DocumentoFiscal = 2,

        /// <summary>
        /// n..12 - Valor total da operação, em centavos da moeda informada no campo 004-000, incluindo todas as taxas cobradas do Cliente (serviço, embarque, etc.). No arquivo de resposta para transações de venda, este campo indica o valor efetivamente debitado do Cliente e creditado para o Estabelecimento (desconsiderando taxas referentes ao uso da solução, descontadas pela Rede Adquirente).
        /// </summary>
        ValorTotal = 3,

        /// <summary>
        /// n1 - 0: Real 
        /// 1: Dólar americano 
        /// 2: Euro
        /// </summary>
        Moeda = 4,

        /// <summary>
        /// a1 - Forma de identificação do Cliente: 
        /// F: CPF 
        /// J: CNPJ 
        /// X: Outro identificador, gerenciado pelo estabelecimento.
        /// </summary>
        EntidadeCliente = 6,

        /// <summary>
        /// n..16 - Identificador do Cliente (somente números, sem formatação), de acordo com o campo 006-000, obrigatório caso este campo esteja presente. Caso informado pela Automação Comercial, este dado poderá ser utilizado pelo Pay&Go para transações que requeiram esta informação (por exemplo, fidelização), evitando assim uma dupla digitação na Automação Comercial e no Pay&Go.
        /// </summary>
        IdentificadorCliente = 7,

        /// <summary>
        /// a..3 - Indica o resultado final da operação de TEF: 0: operação bem-sucedida, ou transação aprovada; qualquer outro valor: erro na transação, ou transação negada.
        /// </summary>
        Status = 9,

        /// <summary>
        /// a..8 - Codinome da Rede Adquirente que processou a transação. Consultar o item 6.4.1 Redes adquirentes no Pay&Go. Para compatibilidade com versões futuras do produto (por exemplo, inclusão de novas redes), a Automação Comercial não deve consistir este campo, somente armazená-lo para consulta ou agrupamento de transações. Pelo mesmo motivo, é recomendado não preencher este campo no arquivo de solicitação para os comandos CRT e ADM. Caso seja preenchido, o Pay&Go processará a transação através da rede informada, sem apresentar o menu de seleção da Rede Adquirente.
        /// </summary>
        RedeAdquirente = 10,

        /// <summary>
        /// n2 - Venda ou cancelamento (de acordo com o campo 000-000): 
        /// 10: Cartão de crédito – à vista 
        /// 11: Cartão de crédito – parcelado pelo Estabelecimento 
        /// 12: Cartão de crédito – parcelado pelo Emissor 
        /// 20: Cartão de débito – à vista 
        /// 22: Cartão de débito – parcelado pelo Estabelecimento 
        /// 21: Cartão de débito – pré-datado 
        /// 24: Cartão de débito – pré-datado forçada 
        /// 40: CDC / débito parcelado pelo Emissor 
        /// 60: Voucher / PAT 
        /// 30: Outro tipo de cartão 
        /// 99: Não definido (a operação não foi concluída) 
        /// Operação administrativa: 
        /// 13: Pré-autorização com cartão de crédito 
        /// 41: Consulta CDC / débito parcelado pelo Emissor 
        /// 70: Consulta de cheque 
        /// 71: Garantia de cheque 
        /// 01: Fechamento / Finalização 
        /// </summary>
        TipoTransacao = 11,

        /// <summary>
        /// a..40 - Identificador único da transação, atribuído pela Rede Adquirente. Por motivo de compatibilidade com versões anteriores desta especificação, a Automação deve sinalizar através do campo 706-000 que suporta o tamanho de 40 caracteres para este campo. Caso contrário, se a Rede Adquirente retornar um valor com mais de 12 caracteres, somente os 12 últimos serão considerados, prejudicando o uso desta informação (tipicamente, para cancelamento).
        /// </summary>
        NSU = 12,

        /// <summary>
        /// a..6 - Código de autorização, atribuído pelo Emissor.
        /// </summary>
        CodigoAutorizacao = 13,

        /// <summary>
        /// n..2 - Quantidade de parcelas, para transações parceladas
        /// </summary>
        Parcelas = 18,

        /// <summary>
        /// n8 - Formato DDMMAAAA.
        /// </summary>
        DataComprovante = 22,

        /// <summary>
        /// n6 - Formato hhmmss
        /// </summary>
        HoraComprovante = 23,

        /// <summary>
        /// a..30 - Identificador único da transação gerado pelo Pay&Go.
        /// </summary>
        CodigoControle = 27,

        /// <summary>
        /// n..3 Quantidade de linhas da via única do comprovante.
        /// </summary>
        TamanhoViaUnica = 28,

        /// <summary>
        /// Linha da via única do comprovante, entre aspas duplas. xxx indica o número da linha, de 001 até a quantidade total de linhas indicada pelo campo 028-000.
        /// </summary>
        ViaUnica = 29,

        /// <summary>
        /// a..40 - Mensagem de texto que deve ser apresentada ao operador do sistema pela Automação Comercial.
        /// </summary>
        MensagemOperador = 30,

        /// <summary>
        /// a..12 - Nome do cartão ou do Emissor. O mesmo cartão pode ter nomes diferentes de acordo com a Rede Adquirente utilizada. Por motivo de compatibilidade, a Automação Comercial não deve consistir este campo, somente armazená-lo para consulta ou agrupamento de transações.
        /// </summary>
        NomeCartaoAdministradora = 40,

        /// <summary>
        /// Soma dos seguintes valores, identificando as funcionalidades suportadas pela Automação Comercial:
        /// 1: funcionalidade de troco (ver campo 708-000)
        /// 2: funcionalidade de desconto (ver campo 709-000)
        /// 4: valor fixo, sempre incluir
        /// 8: vias diferenciadas do comprovante para Cliente/Estabelecimento (campos 712-000 a 715-000)
        /// 16: cupom reduzido (campos 710-000 e 711-000)
        /// 32: funcionalidade de valor devido (ver campo 743-000)
        /// 64: funcionalidade de valor reajustado (ver campo 744-000)
        /// 128: suporta NSU com tamanho de até 40 caracteres (campos 012-000 e 025-000)
        /// Caso este campo não seja informado pela Automação Comercial (versões anteriores), considera-se que nenhuma das funcionalidades é suportada.
        /// Importante: na certificação da CIELO, é exigido que a Automação Comercial implemente a funcionalidade de desconto.
        /// </summary>
        CapacidadesAutomacao = 706,

        /// <summary>
        /// n..3 - Quantidade de linhas do cupom reduzido.
        /// </summary>
        TamanhoCupomReduzido = 710,

        /// <summary>
        /// a..40 Linha do cupom reduzido, entre aspas duplas. xxx indica o número da linha, de 001 até a quantidade total de linhas indicada pelo campo 710-000.
        /// </summary>
        CupomReduzido = 711,

        /// <summary>
        /// n..3 - Quantidade de linhas da via diferenciada do comprovante destinada ao Cliente.
        /// </summary>
        TamanhoViaCliente = 712,

        /// <summary>
        /// a..40 - Linha da via do Cliente, entre aspas duplas. xxx indica o número da linha, de 001 até a quantidade total de linhas indicada pelo campo 712-000.
        /// </summary>
        ViaClienteComprovante = 713,

        /// <summary>
        /// n..3 - Quantidade de linhas da via diferenciada do comprovante destinada ao Estabelecimento.
        /// </summary>
        TamanhoViaEstabelecimento = 714,

        /// <summary>
        /// a..40 - Linha da via do Estabelecimento, entre aspas duplas. xxx indica o número da linha, de 001 até a quantidade total de linhas indicada pelo campo 714-000.
        /// </summary>
        ViaEstabelecimentoComprovante = 715,

        /// <summary>
        /// a..40 - Razão social da empresa responsável pelo desenvolvimento da aplicação de Automação Comercial. Exemplo: KND SISTEMAS LTDA.
        /// </summary>
        EmpresaAutomacao = 716,

        /// <summary>
        /// n12 - Data/hora registrada no cupom fiscal, no formato AAMMDDhhmmss Caso seja usada uma Impressora Fiscal, o preenchimento deste campo é obrigatório para transações de venda.
        /// </summary>
        DataHoraFiscal = 717,

        /// <summary>
        /// a..50 Identificação do terminal.
        /// </summary>
        NumeroLogicoTerminal = 718,

        /// <summary>
        /// a..50 Identificação do estabelecimento.
        /// </summary>
        CodigoEstabelecimento = 719,

        /// <summary>
        /// n1 - Indica o status da confirmação da transação, para transações bem sucedidas (campo 009-000 = 0): 
        /// 1: transação não requer confirmação, ou já confirmada 
        /// 2: transação requer confirmação 
        /// Para manter compatibilidade com versões de especificação anteriores, caso este campo não esteja presente no arquivo de resposta, assumir que a transação requer confirmação se houver comprovantes a serem impressos.
        /// </summary>
        StatusConfirmacao = 729,

        /// <summary>
        /// n..12 - Valor correspondente à taxa de serviço cobrada adicionalmente aos produtos adquiridos, tipicamente no setor de alimentação (gorjeta), em centavos da moeda informada no campo 004-000. 
        /// Este valor é incluído no valor total informado no campo 003-000.
        /// </summary>
        TaxaServico = 727,

        /// <summary>
        /// Operação
        /// 1: venda (pagamento com cartão)
        /// Para manter compatibilidade com versões de especificação anteriores, caso este campo não esteja presente no arquivo de resposta, verificar o campo 011-000.
        /// </summary>
        Operacao = 730,

        /// <summary>
        /// Modalidade da transação com cartão:
        /// 0: qualquer / não definido (padrão)
        /// 1: crédito
        /// 2: débito
        /// 3: voucher
        /// Importante: para compatibilidade com evoluções futuras, caso a Automação Comercial capture esta informação antes de acionar o Pay&Go, sempre deve oferecer para o usuário uma opção “outro” que alimente este campo com o valor 0. Para manter compatibilidade com versões de especificação anteriores, caso este campo não esteja presente no arquivo de resposta, verificar o campo 011-000.
        /// </summary>
        TipoCartao = 731,

        /// <summary>
        /// Modalidade de financiamento da transação:
        /// 0: qualquer / não definido (padrão)
        /// 1: à vista
        /// 2: parcelado pelo Emissor
        /// 3: parcelado pelo Estabelecimento
        /// 4: pré-datado
        /// 5: pré-datado forçado
        /// Para manter compatibilidade com versões de especificação anteriores, caso este campo não esteja presente no arquivo de resposta, verificar os campos 011-000 e 017-000.
        /// </summary>
        TipoFinanciamento = 732,

        /// <summary>
        /// a..40 Nome da aplicação de Automação Comercial.
        /// </summary>
        NomeAutomacao = 735,

        /// <summary>
        /// a..20 Versão da aplicação de Automação Comercial, conforme nomenclatura utilizada pelo desenvolvedor.
        /// </summary>
        VersaoAutomacao = 736,

        ViasComprovante = 737,

        /// <summary>
        /// a..20 - Código obtido junto à NTK Solutions no início do processo de certificação da Automação Comercial.
        /// </summary>
        RegistroCertificacao = 738,

        /// <summary>
        /// a..40 - Nome do Cliente, extraído do cartão ou informado pelo emissor.
        /// </summary>
        NomeCliente = 741,

        /// <summary>
        /// a..40 Nome do produto enviado na transação pela rede adquirente: - Recarga de celular: TIMTURBO 7, CLARO R$10, etc. - Transações de venda: CREDITO, MASTERCARD DEBITO, etc.
        /// </summary>
        NomeProduto = 742,

        /// <summary>
        /// Fim
        /// </summary>
        RegistroFinalizador = 999

    }


}
