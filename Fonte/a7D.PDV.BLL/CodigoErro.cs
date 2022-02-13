using System;
using System.ComponentModel;

namespace a7D.PDV.BLL
{
    /// <summary>
    /// Todo erro tem que ter a descrição que é a mensagem que será exibida no messagebox!
    /// 'eN' Erro é quando para algo sem concluir
    /// 'wN' Warning é quando concluiu mas algo deu algum problema (exemplo fez o pedido mas não imprmiu)
    /// </summary>
    [Serializable]
    public enum CodigoErro
    {
        // Numeros gerados por: ErroCodes.Numeros
        // Inicialmente os códigos de erros fora gerados sem ordem, ou depois viu-se a necessidade de tentar agrupa-los

        // Licença =============
        [Description("A Licença do Software ainda não foi ativado ou ocorreu algum erro durante a verificação. \nEntre em contato com a sua revenda!")] F075,
        [Description("A Licença do Software expirou. \nEntre em contato com a sua revenda!")] F080,
        [Description("Chave de hardware não informada")] F07F,
        [Description("Chave de hardware está inativa")] F081,
        [Description("Chave de hardware está duplicada")] F08D,
        [Description("Erro ao validar a ativação")] E050,
        [Description("Chave de ativação inválida")] E060,
        [Description("Erro ao validar ativação")] E070,
        [Description("Erro ao validar sincronizar licenças")] E071,
        [Description("Sistema Bloqueado")] E077,
        [Description("Erro ao receber mensagem do ativador")] E079,

        // Erros ===============
        [Description("ERRO CAIXA: {0}")] E000,
        [Description("ERRO AO INICIALIZAR CAIXA:|{0}")] E001,
        [Description("ERRO AO INICIALIZAR CAIXA:|{0}")] E002,

        [Description("ERRO BACKOFFICE: {0}")] E010,
        [Description("ERRO AO INICIALIZAR BACKOFFICE:|{0}")] E011,
        [Description("ERRO AO CARREGAR BACKOFFICE:|{0}")] E012,
        [Description("ERRO NO BACKOFFICE:|{0}")] E013,

        [Description("ERRO TERMINAL: {0}")] E020,
        [Description("ERRO AO INICIALIZAR TERMINAL: {0}")] E021,
        [Description("ERRO AO CARREGAR TERMINAL: {0}")] E022,

        [Description("ERRO INTEGRADOR: {0}")] E030,
        [Description("URGENTE: O banco de dados SQL está muito grande")] E031,

        [Description("ERRO WS2: {0}")] E040,

        // Atualizador
        [Description("Erro no controle do processo")] EAA0,
        [Description("Erro no Atualizador Etapa 1")] EAA1,
        [Description("Erro no Atualizador Etapa 2")] EAA2,
        [Description("Erro no Atualizador Etapa 1")] EAA3,

        // Erros BLL 100-200
        [Description("ERRO: {0}")] E100,
        [Description("Não foi possível conectar ao servidor 'SQL Server'!")] E103,
        [Description("Verifique a data do computador.")] E105,
        [Description("Verifique a data do computador.")] E106,

        // Erros SAT
        [Description("ERRO SAT|{0}")] E500,
        [Description("Erro NO SAT|{0}")] E501,
        [Description("ERRO no meio de pagamento do SAT|{0}")] E502,
        [Description("ERRO AO ENVIAR SAT|{0}")] E503,
        [Description("ERRO AO IMPRIMIR SAT|{0}")] E504,
        [Description("ERRO AO REIMPRIMIR SAT|{0}")] E506,
        [Description("O SAT precisa estar isolado no IIS|{0}")] E507,
        [Description("Erro no WebServices do SAT|{0}")] E508,
        [Description("O número de pagamentos agrupados ultrapassa o limite aceito pelo SAT:|{0}")] E509,
        [Description("Erro ao preprar a CFE|{0}")] E510,
        [Description("Erro ao verificar envio anterior|{0}")] E511,
        [Description("Erro ao enviar venda|{0}")] E512,
        [Description("ERRO AO EMITIR SAT|{0}")] E513,
        [Description("ERRO AO INICIALIZAR NFCe")] E514,
        [Description("Taxas como serviço obrigatório para NFCe")] E515,
        [Description("Erro online da NFCe: {0}")] E516,
        [Description("Erro na emisão online da NFCe: {0}")] E517,
        [Description("Erro na assinatura digital: {0}")] E518,
        [Description("Erro na validação XDS: {0}")] E519,

        // Estoque
        [Description("Ocorreu um erro na movimentação de estoque!")] E600,
        [Description("GUIDMovimentação vazio")] E601,
        [Description("Movimentação não pode ser nula")] E602,
        [Description("IDMovimentacao não pode ser nulo")] E603,
        [Description("Movimentação pode já foi processada")] A604,
        [Description("Movimentação precisa ter um tipo definido")] E605,
        [Description("Inventário com id {0} não encontrado")] E606,
        [Description("Inventário com id {0} foi excluído")] E607,
        [Description("Inventário com id {0} não está processado")] E608,
        [Description("Sem unidade de conversão")] E609,
        [Description("Erro no processamento da movimentação de estoque")] E60A,
        [Description("Não existe conversão cadastrada {0}")] E60B,
        [Description("Saída a vulsa")] E60C,
        [Description("Fator de conversão não pode ser '0' (zero) {0}")] E60D,
        [Description("Erro na movimentação dos produtos no estoque")] E60E,

        // Integrações
        [Description("Erro na integração ERP")] EE01,
        [Description("Erro na sincronização ERP")] EE02,
        [Description("Erro no pedido ERP")] EE03,
        [Description("Erro no produtos duplicados ERP")] EE04,
        [Description("Erro na integração IFOOD")] EE11,
        [Description("Erro na confirmação IFOOD")] EE12,
        [Description("Erro no pedido IFOOD")] EE13,
        [Description("Erro no pedido IFOOD (Cliente)")] EE14,
        [Description("Erro no pedido IFOOD (Itens)")] EE15,
        [Description("Erro no pedido IFOOD (Pagamento)")] EE16,
        [Description("Erro ao ler evento")] EE17,

        [Description("Erro no pedido LOGGI")] EE21,

        [Description("Erro na integração EASYCHOPP")] EE31,

        [Description("Erro no Integração de Serviços: {0}")] EEE1,

        // Balança
        [Description("Balança instável")] EC01,
        [Description("Balança em sobrepeso")] EC02,
        [Description("Ocorreu um erro de leitura na balança")] EC03,

        // ECF
        [Description("Erro no Fechamento da Conta")] EC10,
        [Description("Erro na geração do cupom")] EC11,
        [Description("Erro na identificação do cupom")] EC12,
        [Description("Erro na geração do ECF")] EC20,
        [Description("Erro na geração do ECF (abertura)")] EC21,
        [Description("Erro na geração do ECF (item)")] EC22,
        [Description("Erro na geração do ECF (subtotalizar)")] EC23,
        [Description("Erro na geração do ECF (pagamento)")] EC24,
        [Description("Erro na geração do ECF (fechar)")] EC25,

        // Controle dos produtos
        [Description("Produto não encontrado")] EA00,

        // Caixa
        [Description("ID da área de produção da via de expedição não foi configurada")] E801,
        [Description("Área de impressão não existe ou não é possível salvar")] E802,
        [Description("Erro ao abrir Produtos com Modificações")] E803,
        [Description("Erro ao transferir produtos")] E804,
        [Description("Não foi possível cancelar o pedido")] E805,
        [Description("Erro na abertura do caixa")] E806,
        [Description("Erro no fechamento do caixa")] E807,
        [Description("Não é possivel ter 'Comanda com Conta Cliente', sem um pagamento 'Conta Cliente' ativo")] E808,
        [Description("Erro ao carregar produtos, tente novamente!")] E809,
        [Description("Erro no Ajuste de Caixa/Sangria: {0}")] E810,

        // Erros em qualquer lugar
        [Description("Erro ao gerar relatório: {0}")] E120,
        [Description("Erro nos parametros do relatório: {0}")] E121,
        [Description("Objeto com as informações necessárias não informado")] E08E, // GET STONE
        [Description("Erro ao validar o usuário")] E055,
        [Description("A chave de acesso informada não pode ser usada")] E056,
        [Description("Há mais de um caixa aberto para este IDPDV")] E320,
        [Description("Erro ao enviar email das CFe")] ECFE,
        [Description("Erro na exportação da CFe")] ECFA,
        [Description("Sistema de ativação não encontrado")] E2BC,
        [Description("Número de licenças para este tipo de PDV excedeu!")] E110,
        [Description("ERRO NO CADASTRO DO PRODUTO EM MODO ASSISTENTE: {0}")] E220,
        [Description("Tipo entrada não fornecida (verifique o tipo de entrada padrão)")] EF78,
        [Description("O item não pode ser excluído pois existem registros relacionados!")] EF9A,
        [Description("Sem estado definido")] EC2D,
        [Description("Tipo inválido")] EC40,
        [Description("Erro durante agrupamento de comandas")] E143,
        [Description("Erro ao cancelar itens no delivery")] E230,
        [Description("Base de dados corrompida")] EB5B,


        [Description("Erro ao incluir produtos na mesa ou comanda")] EB0C,
        [Description("Pedido na situação de 'Enviado' e não pode mais receber produtos")] EC80,
        [Description("Erro ao ler registro!")] EBB0,
        [Description("Erro ao busca registro!")] EBB2,
        [Description("Erro ao alterar registro!")] EBB5,
        [Description("Erro ao ler configurações!")] ECCO,
        [Description("Erro no registro de PDV")] EB80,
        [Description("Erro ao imprimir conta no delivery")] E300,
        [Description("Configurações dos meios de pagamento está inválida")] EESA,
        [Description("Não há forma de pagamento com gateway 'Stone POS Integrado' configurado!")] EESB,
        [Description("Não há forma de pagamento com gateway 'Granito POS Integrado' configurado!")] EESD,
        [Description("Não há forma de pagamento com gateway 'NTK POS Integrado' configurado!")] EESC,
        [Description("Erro no fechamento diário das comandas")] ECFD,
        [Description("Não há um produto de crédito ativo compatível com a consumação mínima")] ECCD,
        [Description("Valor do pagamento não confere com soma dos valores dos itens.")] EEA0,
        [Description("Tamanho do anexo ultrapassa o limite permitido de 20MB")] EAFE,
        [Description("Erro ao incluir pedido")] E880,
        [Description("Erro ao efetivar pagamento")] E900,

        // Warnings ============
        [Description("Ignorar: {0}")] AAAA,
        [Description("ERRO NA WS2: {0}")] A002,
        [Description("Erro ao imprimir: {0}")] A300,
        [Description("Erro ao imprimir na área de produção: {0}")] A310,
        [Description("O pedido foi realizado, mas não foi possível gerar a Ordem de Produção|{0}")] A200,
        [Description("Chave de acesso incorreta!")] A000,
        [Description("Informe a chave de acesso")] A005,
        [Description("Usuário desativado!")] A010,
        [Description("Usuário sem permissão para executar função!")] A011,
        [Description("Comanda por crédito sem cliente selecionado")] AA10,
        [Description("Erro na identificação do usuário!")] A012,
        [Description("Comanda Inválida!")] AC0A,
        [Description("Erro na Comanda!")] AC0B,
        [Description("Sem créditos suficientes.")] AA20,
        [Description("O pedido excede o limite da comanda:")] AA30,
        [Description("Não é possível comprar créditos junto com produtos")] AE31,
        [Description("Não é possível comprar créditos sem um Cliente")] AE32,
        [Description("Não é possível comprar de créditos com Conta Cliente")] AE33,
        [Description("Só é permitido o pagamento total por Conta Cliente")] AE34,
        [Description("A referência deve ser um número de Mesa.")] AC3A,
        [Description("Mesa não cadastrada.|Verifique o número da mesa informada!")] A301,
        [Description("Mesa aguardando a conta.|Não é possível fazer pedido!")] A302,
        [Description("Mesa reservada.\nNão é possível fazer pedido!")] A303,
        [Description("Mesa/Comanda sem pedido!")] A304,
        [Description("!!! ATENÇÃO !!!||As informações originais foram alteradas||Abra novamente a tela tente novamente")] A900,
        [Description("Ocorreu um erro enviar o email da NFe para o cliente!")] A371,
        [Description("Erro no UPD Server!")] AA00,

        // APP (Erros na comanda, cardapio, saída, autoatendimento) 
        [Description("Versão do aplicativo não informada!")] C000,
        [Description("TipoPDV não informado!")] C101,
        [Description("Favor verificar a hora do Servidor e da Aplicação!")] C102,
        [Description("Versão do aplicativo incompatível!")] C103,

        // Códigos de erro não registrados mas usados como controle em outra aplicação
        [Description("Comanda com checkin fechada!")] B101,

        // Não tem como logar ou registrar isso
        [Description("Insira a licença apenas pelo integrador")] A101,
        [Description("Houve um problema ao validar a licença")] A6F4,
        [Description("O banco de dados não esta pronto, use o integrador")] A6F5,
        [Description("O banco de dados não está na versão correta, use o integrador")] A6F6,
        [Description("Um dos componentes está na versão errada")] A6F7,
        [Description("A versão do banco de dados é incompatível")] A6F8,
        [Description("Aplicação não está na mesma versão do servidor")] A6F9
    }
}
