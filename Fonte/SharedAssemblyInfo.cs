using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyCompany("PDV7")]
[assembly: AssemblyCopyright("Copyright © PDVSeven 2014-2022")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("2.25.0.0")]

/* CheckList antes de gerar Release!
 *    Verificar se os WS2 (Autoatendimento, Saida, PainelMesaComanda) estão apontando para o "." que é para descoberta automática via UDP
 *    Verificar se os APP.Config dos programas .UI estão sem ConnectionString
 *    Verificar se no Web.Config do WS2 a ConnectionString está apontando para localhost
 *    Verificar se as variáveis de TESTE da compilação condicional não está ativa (autoatendimento, integração core)
 *    Sempre compilar como "Mixed Platforms", e cuidado com atualizações dos Nuget PAckages, principalmente do "cef"
 *    Usar o '_limpar.bat' antes de realizar compilação e publish dos WS
 *    Ao compilar o 'instalador_PDVSeven.nsi' descompactar a executável gerada e zipar para gerar o pacote do atualizador
 
Veja mais as anotações no RoadMap e resumo das funcionalidades
    * https://docs.google.com/document/d/1T0UqhKqQIKcYuXewtrOMA7A7-DB7zj9tJiJovSPlmY8
    * https://docs.google.com/document/d/1btwX4zvexmPNwrdQqIQ2QE3I8NxmImUHdaz5Os7Zr3A

BACKLOG RESUMIDO

    Stone POS
        Versão 1.1.0 com teste unitários em NodeJS com servidor Mockup
        Versão com venda de produto

    Android	
        Leitura de Balança via OTG no Terminal Android
        Chamar garçon pelo sistema de mensageria
        Usar WS2/JSON para: produtos, categorias, áreas de produção, pedidos, referencia
        Leitora QRCode

    Caixa
        Venda de credito integrado ao EasyChopp
        Nome Venda Balcão
        Categorias (cores e contranste, e status de ativo)
        Pesquisa CEP (WS BigData + Caixa)
        Novos campos no Cadastro de Clientes (Passaporte, Tipo de Documento, Limites)
        Opção de imprimir o que foi consumido das contas cliente

    Ativador
        Busca autocomplet no sistema de ativações para envio de mensagem

    Backoffice	
        Cadastrar TAG nos produtos
        Lista de clientes e Importação CSV no backoffice
        Faixas de desconto
        Novo cadastro de Relatórios Customizados
        Tipos de entrada (Taxa de serviço, Consumação, Limites Consumo)

    Autoatendimento
        Opção de retirada ou viagem na tela de resumo

Lista das últimas versões e FIX
===============================

    Definição de Versão: 2.Banco.Recurso.Fix
    Os 2 primeiros numeros por definir a estrutura de banco serão obrigatório ser igual entre todas aplicações em execução.
    Os nomes de versão incluindo o instalador será sempre só com os 3 primeiros digitospor padrão
    Será sempre exibido um alerta, não blocante nas aplicações de caixa e backoffice quando os 2 numeros finais não forem iguais.
    Os Branch do GIT no VSOnline deveram ser sempre o caracter 'V' os 3 números separados por underline, exemplo "V2_19_4" o digito do FIX não entram
    Os numeros de cada versão de cada aplicações Android seguirá sua numeração propria.
    O WS2 é responsável em bloquear versão antiga dos aplicativos, e fornecer o link do correto para download
    Especificar o tipo de cada alteração feita: Novo, Melhoria, Correção, TDD, Atualização

===============================================================================
===============================================================================

2.25.0.0 ======================================================================
    Novo: Pix-Conta
        Aba Pix-Conta no Configurador
        Aba Pix-Conta no Integrador
        Impressão Ticket Pix
    Melhoria: Alteração do nome do Gateway de Pagamento de PagSplit para PixConta

2.24.8.0 ======================================================================
    Melhoria: Criação do Gateway de pagamento PagSplit
    Melhoria: Importação de pedidos do Delivery On com pagamento online

2.24.7.1 ======================================================================
    Correção: No Delivery Online, tratamento do campo telefone

2.24.7.0 ======================================================================
    Melhoria: Melhorias na estrutura de projetos e referencias
    Melhoria: Atualização e Consolidação de versões de pacotes

2.24.6.1 ======================================================================
    Correção: Ajuste na importacao do meio de pagamento do Delivery On! pelo Range

2.24.6.0 ======================================================================
    Melhoria: Identificação dos pedidos do Delivery-On
    Melhoria: O sistema mantém o estado da lista de pedidos de Delivery
    Correção: No Delivery Online, tratamento do campo telefone

2.24.5.3 ======================================================================
    Correção: Gerava erro no Delivery Online quando o cliente enviava texto no CEP

2.24.5.2 ======================================================================
    Correção: Não estava importando o Bairro e CEP no Delivery Online
    Correção: Não estava importando os pedidos do iFood

2.24.5.1 ======================================================================
    Correção: Correção do cancelamento automático do cupom
        A identificação do cupom é feita usando o GUIDMovimentacao

2.24.5.0 ======================================================================
    Melhoria: Otimização tela de cancelamento de cupom fiscal
    Melhoria: Otimização monitor de pedidos
    Melhoria: Otimização fechamento de pedido
    Melhoria: Otimização transferencia de produtos
    Melhoria: Otimização adicionar produto

2.24.4.2 ======================================================================
    Correção: Correção dos cancelamentos automáticos do SAT nos pedidos iFood e DeliveryOnline

2.24.4.1 ======================================================================
    Correção: Importar endereço de entrega no Delivery Online
    Correção: Importar observacao do pedido no Delivery Online

2.24.4.0 ======================================================================
    Melhoria: Faz até 5 tentativas de reiniciar o processo de integração ao gerar um erro
    Melhoria: No cadastro de produtos, listar apenas os ingredientes para montar a receita

2.24.3.0 ======================================================================
    Correção: Selecionar ingredientes na receita não estava funcionando
    Correção: Forma de pagamento do iFood não estava relacionando com o IDMeioPagamentoSAT
    Melhoria: Reimpressão do fechamento da conta não fiscal
    Melhoria: Retirada do relatório "Taxa_Servico_Com_e_Sem_Desconto"
    Correção: Alterar taxa de serviço sem informar senha
    Melhoria: Permitir preencher todos os campos independente do gatway pagamento
    Melhoria: Ajuste das bordas no BackOffice
    Melhoria: Importar taxa de serviço no Delivery Online 
    
2.24.2.0 ======================================================================
    Correção: No fechamento, estava aparecendo produtos em aberto (IDCaixa não estava sendo gravado no fechamento pelo iFood)
    Correção: Produto não cadastrado não estava salvando o nome na observação
    Melhoria: Inclusao do campo Location_id para o Multi-lojas do Delivery Online

2.24.1.0 ======================================================================
    Melhoria: Listar Motivos Cancelamento do iFood

2.24.0.0 ======================================================================
    Novo: Integração com Delivery Online
    Melhoria: Ajustes na integração iFood para criar automaticamente Tipo Pagamento, Tipo Desconto, Taxa Entrega, Pagamento por VR
    Melhoria: Inclusão das TAGs em produtos
    Correção: Erro na autenticação do iFood que expirava
    Correção: Cancelamento de pedidos no Delivery sem integracao com App

2.23.0.7 ======================================================================
    Correção: Chamada da API de autenticação sem o HTTPS
    Correção: Relatório do Fechamento do dia
    Correção: Fechamento do dia sem movimentação

2.23.0.6 ======================================================================
    Melhoria: Inclusao da opção "Categorias Alcoolicas" no Configurador
    Correção: Ajuste do TEF Stone para a versão 3.9.4.4
        Bloqueio do uso de Vale Refeição para bebidas alcoolicas

2.23.0.5 ======================================================================
    Correção: Integração iFood
        Pagamento com dinheiro no iFood estava vindo zerado
        Cancelamento de pedido com o cupom sat acima do tempo
        Tratamentos de erros para evitar Exception

2.23.0.4 ======================================================================
    Correção: Integracao iFood com a nova API
        Abertura Loja
        Importação Pedido

2.22.0.0 ======================================================================
    Novo: Adicionado o tipo de pedido Retirada
        Incluir opção no configurador para Habilitar Pedido Retirada no Caixa
        Configurador > Caixa > Tipo de pedido Padrão: adicionado opção Retirada
        Configurador > Caixa: adicionado configuracao "Habilitar Tipo de Pedido Retirada"

2.21.1.11 ======================================================================
    Correção: o IDTipoPedido estava perdendo quando desconto dado no imprimir conta
    Correção: o valor do pedido, na tela de adicionar produto, aparecia errado quando tinha modificação com valor
    Melhoria: exibir o tipo de desconto no CheckBox "aplicar desconto" quando algum tipo selecionado


2.21.1.10 ======================================================================
    Migração GitHub

2.21.1.7
    Novo: Granito TEF
    Novo: autoatendimento fluxo para festa
    Novo: Teclado WPF com opção para formatação financeira

2.21.1.6
    Novo: GRANITO Novo método POS consulta usuário + registra crédito

2.21.1.5 A validar
    Correção: WS2 API Filtro de Produtos
    Correção: Cardápio 1.14.2 e Comanda 1.24.2 
    ? Emissão de SAT sempre que for possivel

2.21.1.4 (26/08/2019) Ferreira
    Correção: Cardápio 1.14.1 e Comanda 1.24.1 - Fechamento de Conta, e Referencia de localização

2.21.1.3 (04/08/2019) Ferreira
    Novo: Relaciona TAG aos clientes na Integração EasyChopp

2.21.1.0 (25/07/2019) Fabio ???



2.21.0.1 (15/07/2019)
    Novo: Cancelamento TEF Stone no Caixa e Homologação
    Novo: Cadastro e abertura de comanda dos clientes existentes
    Melhoria: Mais logs na inetegração EasyChopp e busca na integração pela comanda quando há!
    Melhoria: Autoatendimento
        + Removido páginas de crédito antigas incompativeis com conta cliente
        + Teste de regressão Fechamento de comanda
        + Teclado próprio para abertura de comanda
        + Layout dinamico para controles de Pedido e Produtos
        + Opção de Retirada ou viagem na tela de resumo

2.21.0.0 (11/06/2019)
    Alterações Estruturais no Banco de Dados
        + Tickets: Tabela de controle de geração e utilização de tickets
        + Integracoes: Tabela de integração e tipo de integracao

    Melhoria: Logs para TEF Stone, atualizado Nuget MicroTEF 3.8.6
    Novo: Entrada de modificações WS2 (Código, Quantidade, Valor, Notas) prevendo quantidade de modificações não unitárias para 1 nivel de modificações
    Novo: Aplicativo de validação de Tickets QRCode emitidos
    Novo: WS2 Emite, Consultar, Consome Tickets
    Novo: Controle de integração generico
    Novo: Autoatendimento Bodebrown
        + Modificações simples do produto via WS2 e validação min/max 
        + QRCode Emitir
        + EasyChoop no integrador e configurador: Consultar, cadastro, subir novos creditos 
        + Compra de Creditos e cadastro de cliente (teclado alfanumerico do windows)
        + Tela de confirmação
        + Tela de CPF na Nota

2.21 ======================================================================


2.20.4.5 (12/05/2019)
    Melhoria: Atualizador tenta apagar a pasta WWW_SAT quando percebe a versão anterior (as DLL do SAT estarão no backup da aplicação)
    Melhoria: Stone App 1.0.4 com numero de comanda ou mesa limitada a até 5 digitos (de acordo com a versão 2.20.3.4)
    Melhoria: Fechamento de caixa com totais de vendas com conta cliente e venda de creditos

2.20.4.4 (10/05/2019)
    Correção: Emissão Fiscal na Via de Expedição 
    Melhoria: Descontinuado "Área de Impressão SAT" (Via de Impressão faz a mesma função no Delivery)

2.20.4.3 (07/05/2019)
    Melhoria: Revisada rotina de impostos, validção e erro 
    Novo: Cadastro do CEST junto ao NCM no backoffice

2.20.4.2 (05/05/2019)
    Desativado emissão offline
    Inicio Sintegra

2.20.4.1 (04/05/2019) NFCe - Ambiente de Produção
    Novo: Para ambiente de homologação especificar '<add key="HomologacaoNFCe" value="true" />' no app.config ou web.config
    Novo: Reimpressão NFCe 
    Novo: Cancelamento NFCe
    Novo: Ajustes de compatibilidade Backoffice

2.20.4.0 NFCe - Versão de homologação
    Novo: WWW_SAT para emisão de SAT (Novo projeto WS Fiscal, NFCe a ser transferida nesse projeto)
    Novo: Configurações Fiscais para SAT ou NFCe
    Novo: Emisão NFCe Normal ou Offiline no Caixa e WS2
    Melhoria: Atualizado README.TXT com mais explicações da arquitetura, projetos e links
    Melhoria: Unificação e padronização da chamadas das rotinas do SAT 0.07, SAT 0.08, e NFCe
    Melhoria: Removido referencias do 'ReportServices' das rotinas de SAT, Caixa e Integrador
    Descontinuado: Layout 0.06 do SAT

2.20.3.11 (03/05/2019)
    Melhoria: No sync com o Cake exibe mais dados sobre quando é novo ou atualização e em produto se o item está ativo ou excluido
    Melhoria: No backoffice a exclusão de produto e usuário também deixa estes inativos

2.20.3.10 (30/04/2019)
    Correção: Impressão Fiscal na via de expedição
    Correção: Envia de campos ativo de produto no Cake

2.20.3.9 (22/04/2019)
    Melhoria: Permite fechamento com saldo negativo (fica devendo)
    Melhoria: Consulta de saldos melhorada

2.20.3.8 (21/04/2019)
    Melhoria: Tratamento de painel de modificações com produtos inesistentes
    Correção: Comanda 1.23.1, Cardápio 1.13.2 - Exibição dos valores de modificação
    Correção: No Backoffice, remoção de detecção de ENTER/ESC na tela de cadastro de comandas para a leitura de tag hexadecimal
    Correção: Cake produtos excluido sobem como produtos inativos
    Correção: Revisado Fluxo de Emissão Fiscal no Delivery
    Melhoria: Impressão unificada no Fechamento de Caixa Automaticos

2.20.3.7 (14/04/2019)
    Correção: Impressão de Notas do item na ordem de produção com multiplas linhas
    Melhoria: Ordenação alfabetica na tela de saldos de clientes

2.20.3.6 (06/04/2019)
    Novo: no WS2 é possível configurar o 'TraceLog' para gravar toda comunicação do WS2 (~\logs\{0:yyyyMMdd_hh}.log)

2.20.3.5 (29/03/2019)
    Novo: Configuração Código da Leitora em HEX/DEC para uso no caixa separadamente da WS2/API
    Melhoria: Forma de cadastro de TAG em HEX ou DEC independente de configuração no backoffice

2.20.3.4 (27/03/2019)
    Melhoria: Número mínimo de 6 dígitos para usar código em vez do número da comanda limitado a 5 digitos
    Correção: Retorno do arquivo 'Autenticacao.asmx' para o cardápio

2.20.3.3 (24/03/2019)
    Correção: Quantidade de modificações na ordem de impressão
    Melhoria: Validações no cadastro de comandas com códigos HEX/Decimal

2.20.3.2 (22/03/2019)
    Correção: Seleção do Tipo de Pedidos
    Correção: Copiando vampos de entrada e viagem na transferencia de produtos
    Correção: IFood, tratamento de pedido não existente, e eventos

2.20.3.1 (15/03/2019)
    Correção: Seleção de ordem de produção em Venda Balcão Touch
    Melhoria: Padronização layout do FormTouch em quase todas telas do Caixa

2.20.3.0 (14/03/2019)
    Novo: Botão de 'Adicionar Crédito' direto em comandas por credito
    Novo: Vai para a tela de 'Adicionar Crédito' automaticamente quando é aberta comanda e não há saldo
    Melhoria: Removido o botão de 'cancelar' da edição de valores (fechar cancela)
    Melhoria: Reposicionamentos dos menus de superior e inferior de ações para usar 100% da tela
    Melhoria: Spliter de redimencionamento horizontal na tela de pedidos
    Melhoria: Layout Touch na exibição de mensagens
    Melhoria: Ajuste de textos e layouts do status dos pedidos de mesa/comanda

2.20.2.3 (13/03/2019) e 2.20.2.2 
    Correção: Hexadecimal (MERGE 2.19.6.31)
    Correção: Taxa de serviço levada em consideração ao adicionar produto no caixa em comanda com conta cliente
    Correção: Correção no 'ValorDisponivel' da Api WS2 (EasyChop)
    Correção: Pedidos em comanda sem chekin (sem cliente)
    Correção: Terminal de Saída mostra o valor pendente corretamente quando há pagamento parcial
    Correção: Validação de versão do Terminal de Saída Windows/Android (só notifica necessiade de atualização no Temrinal de Saída Windows)
    Melhoria: Interrompe a emissão de SAT pendentes quando há erros na liquidação de conta cliente
    Melhoria: Inicio do "Shared Project" para o fluxo da auto-atualização do terminal de saída e autoatendimento
    Melhoria: Cadastro de email no pagamento normal e touch sem dados null no cadastro de clientes
    Melhoria: Sempre valida a existencia do cadastro por em CPF/CNPJ quando há essa dado como parametro para criação de novo registro

2.20.2.1 (04/03/2019)
    Novo: Comanda e Terminal de Saída compativel com Leitora RFID
    Correção: Cadastro de comanda no backoffice
    Correção: Resolvido travamento no cálculo de descontos no Pagamento Touch
    Correção: Layout estourado e eventos de remoção pagamentos na tela de Pagamento Touch
    Correção: Travamento na abertura ou fechamento do caixa quando há problemas na impressora
    Melhoria: Validação dos limites de desconto por valor ou percentual
    Melhoria: Abertura de comanda com consumação sem taxa de consumo
    Melhoria: Validação no posicionamento de telas quando monitores são desconectados
    Melhoria: Validando carga do tipo de pagamento de conta cliente e comanda com crédito
    Melhoria: Usado enumerador para o tipo de gerenciador de impressão

2.20.2.0 (01/03/2019)
    Novo: Cancelamento Ifood
    Novo: Bloqueio de edição dos itens do Ifood
    Novo: Teclado para inserir CPF/CNPJ no Duploclick em Pagamento Touch
    Novo: Nova bandeira 'Alelo'
    Novo: BigData subindo campo de custo do produto
    Correção: Quantidade de modificações no Ifood
    Melhoria: Gravando o tipo de entrada em PedidoProdutos
    Melhoria: Removido outros layout de impressão do SAT, mantido só 'imagem'

2.20.1.2 (26/02/2019) <= MERGE 2.19.7.2
    Novo: Comanda com Conta Cliente
    Novo: Conta cliente Fiscal, emite SAT quando liquida creditos
    Novo: Baixa de creditos no Cake
    Melhoria: Spliter na tela touch de pagamentos nas formas de pagamento
    Melhoria: Atualização e consolidação NugGet Packages

2.20.1.1 (19/02/2019)
    Correção: Correções no fechamento Touch com conta cliente
    Correção: Limpeza na Ordem de produção
    Correção: Consulta de conta quando não há creditso iniciais
    Melhoria: Controle de fluxo de emissão de conta

2.20.1.0 (19/02/2019)
    Novo: Conta Cliente
    Divida: Comanda con Conta Cliente e Liquidação de Creditos

2.20.0.0
    Alterações Estruturais no Banco de Dados (ver EF migrations)
        + Usuários: Campos de para direitos específicos no Caixa ou BackOffice
        + Tipos de Entrada: Campo para configurar Taxa de serviço e Limite de Comanda
        + Tipos de Desconto: Limites de valor, percentual de acordo com nivel de acesso para desconto
        + Ordem de Impressão: Campo para definir o tipo de ordem de produção
        + Pedidos Pagamentos: Criado 'IDGateway'
        + Pedidos Produtos: Campo Tipo de entreda para facilitar relacionamentos e relatórios
        + Saldos: Campo para relacionar debito por pagamento
        + Clientes: Identificador de cliente iFood e outro tipo de documento para identificação

    Home vazia no WS2 e documentação Swagger em http://localhost:7777/swagger
    Limpeza geral DAL, BLL, e rotinas descontinuadas


2.20 ======================================================================


2.19.7.2 (19/02/2019)
    Melhoria: Busca de EAN só quando há mais de 9 digitos 

2.19.7.1 (11/02/2019) <= MERGE 2.19.6.7
    Novo: Permite edição de Tema para Torneira
    Melhoria: Identifica tipos de PDVs cadastrado se é necessário habilitar edição de tema e cadastro de imagens em produtos

2.19.7.0 (04/02/2019)
    Novo: Torneira PDV7 - The Mellos

2.19.6.7 (11/02/2019)
    Correção: Impressão de com taxa de serviço personalizada no Integrador e WS2

2.19.6.6 (08/02/2019)
    Correção: Validação na busca por telefone com integração de IFOOD para dados sensível

2.19.6.5 (06/02/2019)
    Melhorias: Ifood com mascara de telefone para dados sensível (bloqueio por lei) https://developer.ifood.com.br/v1.0/page/altera%C3%A7%C3%A3o-no-envio-de-dados-sens%C3%ADveis

2.19.6.4 (06/02/2019)
    Melhorias: Validações, exibição e controle do token do Ifood

2.19.6.31 (08/03/2019)
    Correção: Faz leitura de HEXADECIMAL ou DECIMAL de acordo com a configuração

2.19.6.3 (31/01/2019)
    Novo: Tempo de integração do BigData+Iaago, e pause quando há erros!
    Melhorias: Validações do backoffice no cadastro de comandas
    Correção: Revisada ordenação, busca de comandas e Status de Comanda
    Correção: Ativador melhorado com rotinas de tratamento de erros igual ao do Bigdata

2.19.6.2 (30/01/2019)
    Melhoria: Filtro de produtos por código, só exibe produtos que tem código
    Melhoria: Foco no box do tipo de pagamento para tela não touch
    Melhoria: Nome da taxa de serviço impressão será o produto 'serviço' da tabela de produtos
    Melhoria: Posição e tamanho da tela está sendo gravado no banco e não mais nos arquivos user.config

2.19.6.1 (28/01/2019)
    Melhoria: Mensagem do status do BigData tem que começar com OK para continuar
    Correção: Abertura de comanda

2.19.6.0 (24/01/2019)
    Novo: Subindo Usuários, Clientes, Produtos, Pedidos para o BigData/Iaago

2.19.5.3 (22/01/2019)
    Melhoria: Imprimindo observações do cupom (fidelidade do Ifood) na Ordem de produção
    Melhoria: Padronização e mudança de alguns códigos de erros do sistema de ativações e validação
    Melhoria: Cancelamento do processo de atualização ao fechar janela de carregamento
    Novo: Cardapio 1.12.2 (obrigatorio)

2.19.5.2 (18/01/2019)
    Novo: Número da comanda ou tag para abrir a comanda ou buscar
    Novo: Backoffice fazer busca por numero ou tag e validar duplicidade de Comanda/TAG
    Novo: Numero da Versões obrigatórias editáveis no web.config
    Melhoria: Mostrando IDPDV no Backoffice e ordenando listagem por Tipo de PDV e Nome
    Melhoria: EasyChopp permite valor negativo em credito
    Melhoria: Otimização de performance quando há muitas comandas abertas (BUG!)

2.19.5.1 (16/01/2019) <= MERGE 2.19.4.7
    Novo: Caixa com seleção de área de produçãos e áreas padrão
    Novo: Obtem a versão mais recente do arquivo existente no diretório WS2/release para download de atualização necessário a partir da versão mínima
    Melhoria: SQLServerTypes atualizado (versão 14.0.1016.290)
    Melhoria: Subtotal só impreme quando é diferente do valor total
    Melhoria: Texto "Acréscimo" alterado para "Serviço" na Pré Conta e SAT
    Correção: Totalizador no sistema de créditos (Trucamento)
    
2.19.5.0 (15/12/2018) <= ORIGEM 2.19.4.3
    Novo: Seleção de Produto para Viagem no Caixa e Terminal Windows
    Novo: Comanda 1.22.0 (versão opcional)
    Melhoria: Terminal Windows totalmente com Layout Touch

2.19.4.7
    FIX CakeERP
    FIX EasyChoop
    
2.19.4.6 (09/01/2019)
    Melhoria: Listagem e validação do cadastro de numeros de comanda em HEX
    SQL-EF-AutomaticMigration: tbComanda.Codigo alterado para bigint

2.19.4.5 (09/01/2019)
    Novo: ValorDisponivel (Créditos ou Limite) api/comandas/{numero}/total
    Correção: Aceito valores em String HEX ou decimal API (/status /saldo /total)
    Melhoria: atualizando lib/swagger.json
    
2.19.4.4 (20/12/2018)
    Novo: Configuração para Habilitar/Desabilitar integração com SAT nos POS Integrados (FIX 2.19.3.21)
    Novo: Re-Remissão de SAT na tela de Reimpressão (FIX 2.19.3.21)
    Correção: NTK Pay&Go validação da Autenticação e Status (FIX 2.18.0.32)
    Melhoria: Revisão nos principais controles e formulários para suportar 125% DPI    

2.19.4.3 (13/12/2018)
    Correção: Usar áreas de impressão e área de impressão padrão na comanda 1.21.2 (versão obrigatória)
    Correção: Tratamento de loop de erros de nas áreas de impressão (Memoriza área de produção com erro por 1 minuto)

2.19.4.21 (10/12/2018)
    Novo: Fechamento Conta em Comanda com ChekIn e padronização APP Comanda 1.21.0 (versão obrigatória)
    Novo: Exibição do tamanho do banco de dados e monitoramento de arquivos grandes (4G)
    Melhoria: Cores e refresh na lista de pedidos com Checkin
    Melhoria: Guia inicial do integrador é o Servidor PDV7
    Correção: Alteração do nome do Caixa, requer Administrador
    Correção: Área de Conta Padrão funcionando se não for especificado um ID na configuração da comanda
    ATENÇÃO: quando o ultimo digito da versão é com 2 digitos é como se fosse um FIX.FIX

2.19.4.1 (07/12/2018)
    Correção: Exibição correta de itens excluidos pelo EF durante migração (Limpeza de Configrações)
    Correção: Relatório de Créditos
    Correção: Tamanho das fontes MonoSpace dos relatórios Gerenciais
    Melhoria: Envio da query do relatório quando há erro

2.19.4.0 (05/12/2018)
    Novo: Transferencia completa de produtos e pagamentos de mesa ou comanda
    Novo: Layout para telas Touch em Pedido, Pagamento, Autenticação e TEF Conta de Cliente
    Melhoria: Só apaga Backup com mais de 30 dias desde que haja no mínimo outros 10 backups anteriores
    Melhoria: Apaga configurações descontinuadas
    Em Desenvolvimento: Conta Cliente (forma de pagamento bloqueada)

2.19.3.3 (29/11/2018)
    Correção: IFOOD: changeFor, subItemsPrice
    Melhoria: Atualizador com Reboot se necessário
    Correção: Correção APP Comanda 1.20.2 - Correção da validação da Data de Nascimento no Checkin
    Melhoria: Só libera o menu principal do caixa apos carregar as configurações
    Melhoria: Usando compilador CS 7.3
    Melhoria: Identificação de erros de queda de internet

2.19.3.2 (24/11/2018)
    Novo: Monitoramento de clientes
    Novo: Bloqueio de versão incompatível no autoatendimento
    Corrigido: Segurança na obtenção do recebimento de mensagens

2.19.3.1 (23/11/2018)
    Novo: Gravando email no fechamento da conta (só envia nota se for marcado a opção, mas sempre grava o email)
    Melhoria: 'Solicitar Autenticação' nas comandas virá desligado
    Melhoria: (Padronizando) Configuração MargemImpressaoWindows e LarguraImpressaoWindows para configurar a área de impressão no Integrador, Caixa e Autoatendimento
    Melhoria: (Padronizando) Seleção de fonte e tamanho igual ao do 'Gerenciador de Impressão' (mantido o padrão Arial 10)
    Melhoria: (Padronizando) Migrado configurações de fontes do '.config' do integrador para o configurador 'Gerenciador de Impressao' 
    Melhoria: Ativações tem opção de ignorar algumas mensagens de erro pelo texto (ignorar mensagens de erros durante de desligamento do servidor e rede)
    Melhoria: Identificação do erro de diferença de horas entre servidor e comanda
    Melhoria: Impressão com fonte monospace para TEF
    Melhoria: Tratamento de erros nas configurações de via de expedição
    Melhoria: Identificando possíveis erros na abertura de paineis de modificações
    Melhoria: Carregamento dos dados do pedido antes do pagamento
    Melhoria: Informa quando solicitar conta pela comanda sem área de produção de conta configurada
    Melhoria: Rastreaento de erros do atualizador
    Correção: UDPServer não envia erros de conexão
    Correção: Impressão cortada por usar configuração conflitante

2.19.3.0 (14/11/2018)
    Novo: TEF Stone
    CherryPick 2.18.0.30
        Melhoria: Rastreamento para detectar possíveis erros na geração de conta delivery
        Correção: Agrupamento de itens no SAT

2.19.2.4 (08/11/2018)
    Novo: Instalador baixa e instala o SQL Server 2008 Express
    Melhoria: Cores e legenda do status de impressão
    Melhoria: Envio de emails do ativador pela conta do suporte
    Melhoria: Impressão gráfica no fechamento de conta gerencial não fiscal
    Correção: Texto 'DOCUMENTO NÃO FISCAL' em qualquer impressão de conta não fiscal
    Correção: Ordem de impressão perdia item quando dava erro
    Correção: Versão obrigatória do comanda 1.20.1 (problema ao abrir config)
    Correção: Tratamento correto da confirmção de cliente bloqueado na abertura de comanda com checkin

2.19.2.3 (06/11/2018)
    Novo: Mensagem prevendo link generico ou cobrança, e fechamento de mensagem se outro local responder
    Melhoria: Ignorando erros com as mensagens: "SHUTDOWN", "provider: TCP Provider", "Atingido o tempo limite da solicitação", "Tempo Limite de Execução Expirado"
    Correção: Finalização do Delivery não fechava a tela e imprimir conta desnecessáriamente quando é por via de expedição
    Correção: Validação de versão da comanda e terminal de saida

2.19.2.1 (02/11/2018)
    Novo: Relatório de impostos dos produtos
    Melhoria: Tratamento de erros no sincronismo do Iaago
    Correção: Ordem de impressão

2.19.2.0 (31/10/2018) Impressão de Expedição para Delivery
    Novo: Emissão da via de expedição, se estiver ativa, e mantendo compatibilidade
    Melhoria: Refatoração separando rotinas de Impressão, SAT, Imagens
    Melhoria: Revisão e padronização nas rotinas de ordem de produção, conta, e SAT
    Correção: Atualizador prevê erro do backup do update da mesma versão

2.19.1.1 (25/10/2018) 
    Novo: Pergunta para atualização: SIM, NÃO, ADIAR (1h)
    Novo: Gravação da informação extra do iFood (Bem Vindo, ou número de pedidos)

2.19.1.0 (23/10/2018) 
    Novo: Caixa funcionando como servidor de ordem de produção
    Novo: Status das impressoras no Integrador

2.19.0.2
    Novo: ETipoPDV EasyChopp, IAago
    Novo: Envio de produtos para o IAago a cada 4 horas

2.19.0.1
    Correção: FIX Campos removidos do EMenu
    Novo: Ativador envia atualização, informações e perguntas
    Melhoria: Exibição e controle de mensagens de respostas

2.19.0.0
    Alterações Estruturais no Banco de Dados (ver EF migrations 2_19)
        + Tabela de mensagens
        + Tabela para horarios do iFood
        + Padronização de casas decimais de quantidade (3 digitos)
        + Remoção de campos não usados: CodigoEmenu, SincEmenu, Integracao1/2, Quantidade, ValorAliquota
        + Remoção tabelas: tbAcao, tbIntegracaoSAT
        + Novos campos prevendo: Cores, Iaago, Caixa como servidor de Ordem de Impressão, CEST
    Novo: Notificações do status da atualização pelo sistema de mensagens


2.19 ======================================================================


2.18.0.30 (14/11/2018) 
    Melhoria: Rastreamento para detectar possíveis erros na geração de conta delivery
    Correção: Agrupamento de itens no SAT
    ATENÇÃO: Não fazer merge com versão 2.19!!!

2.18.0.29 (30/10/2018)
    Melhoria: Revalida foco no textbox do numero da comanda ou mesa a cada 1 segundo

2.18.0.28 (24/10/2018)
    Novo: No resumo do pedido IFOOD mostra a quantidade de pedidos ou NOVO quando for primeiro pedido do cliente
    Melhoria: Ocultado o administrador no cadastro de usuários
    Correção: Validação na Ordenação de modificações quando não há modificações
    Correção: Validação de valor no campo de desconto
    Correção: Validação do numero de Mesa/Comanda

2.18.0.27 (08/10/2018) PayGoWeb Certificado!
    Novo: Certificado do PayGoWeb
    Novo: Processo administratico do PayGoWeb no Autoatendimento
    Novo: Ativador envia comando de atualização e lista mensagens
    Melhoria: Integrador tentar ligar o serviço do SQL Server na primeira vez caso o SQL não tenha iniciado
    Melhoria: Rastreamento de erro em movimentação do estoque
    Melhoria: No integrador verifica necessidade de atualização antes de tudo
    Correção: PayGoWeb com timeout de 1h !!! ???
    Correção: Terminal de Saida 1.0.1 fechamento sem usar senha configurada
    Correção: Comanda 1.19.1 - Exibe valor dos itens confirmados com modificações e revisão das observações do item
    Correção: Cardápio 1.12.1 - Revisão das observações gerais do item

2.18.0.26 (05/10/2018) PayGoWeb Aguardando Certificado!
    Novo: Autoatendimento aceita o PayGoWeb
    Novo: Adicionada Bandeira "Outros"
    Novo: Limpando backups de Banco SQL com mais de 30 dias
    Novo: Relatório de formas de pagamento detalhado
    Novo: Parâmetro "adm" no Autoatendimento inicia processo administrativo no PayGoWeb
    Atualização: POS PTI 2.0.4.0 TEF PGWebLib 4.0.62.0
    Melhoria: Autoatendimento com impressão SAT e Conta melhorados com imagem do tamanho configurado
    Melhoria: Mensagem amigável no Caixa quando há pedido acima de R$ 10.000
    Melhoria: Padronização das chamadas das API do POS Integrado Stone, NTK, Autoatendimento
    Melhoria: Tratamento de produto inexistente no adicionar itens WS2
    Melhoria: Informação do IP servidor atual na SplahScreen
    Correção: Remoção da ordenação sem sentidos dos itens na tela de pagamentos
    Correção: Fluxo de atualização para não fazer backup das pastas de Update e UpdateOK adiciona o numero da versão para evitar exclusão
    Correção: Remoção do modal de processamento para PayGoWeb no Caixa
    Correção: Colocando o DDD padrão quando for inserido com 'Zero' pelo checkin
    Correção: Processo que fica travado do integrador
    Correção: Criação "Caixa Temporário" de limpeza no fechamento do dia só quando necessário para "Fechar Comandas com Credito", "Registrar Vendas com Credito", "Cancelar Balcao Pendentes", "Fecha Cancelados"
    Correção: Remoção do desconto no Pagamento Touch
    Correção: Desabilitando cache de imagens na impressão de conta sem parametro na Stone
    Correção: Na pré-conta o Sub-Total inclui o serviço como produto na conta
    Correção: Remoção de tela de detalhamento de pedido ainda não realizado no Terminal e Caixa Touch que limpava lista atual
    Correção: Produto com assistente de modificação avançado só permite ser adicionado individualmente

2.18.0.25 (28/09/2018)
    Novo: Revisão de envio de venda do SAT com 3 tentativas quando necessário
    Novo: Descontinuação da opção PERGUNTAR para imprimir cupon, e adicionado opção nas telas de pagamento normal e touch
    Melhoria: Exibição das mensagens de erro de conteudo de rejeição do SAT
    Correção: Validação de versão no APP Saida Windows

2.18.0.24 (28/09/2018)
    Correção: Volta do item normal do ECF via ACBR (sem configurar truncamento) e remoção das referencias do BEMAFI não usado a mais de 2 anos
    Correção: Tratamento de perda de conexão com SQL no integrador

2.18.0.23 (24/09/2018) APP DE SAIDA
    !!! Sempre usar os APK existentes na pasta "release" do WS2 (http://localhost:7777/release) !!! 
    !!! Ou usar os .BAT em modo de desenvolvedor mapeados existentes na mesma pasta C:\PDV7\WebServices\www2\release !!!
    Novo: Adicionada Bandeira "VR", "SODEXO", "TICKET"
    Melhoria: Ajustes e otimizações no WS2 para terminal de Saida Android
    Melhoria: Retorno da numeração das versões das aplicação Android de Comanda, Cardápio, Pesquisa 
    Correção: API da balança e revisão dos truncamentos

2.18.0.22 (16/09/2018) CheckIn na Comanda
    Novo: Permite abrir comanda com cadastro já existente pelo Tipo de Cliente, ou cadastrar novos
    Novo: Comanda 2.18.2 usando WS2: JSON/Retrofit

2.18.0.21 (10/09/2018)
    Novo: WS2 nova chamada 'api/comandas/{numero}/saldo' que retorna o limite disponivel para compra, ou Creditos pre pagos do cliente, e informações do cliente
    Novo: WS2 permite validar ou não limite de comanda / credito ao adicionar produtos
    Correção: Processo perdido no integrador
    Correção: Validação no limites de modificações no WS2/BackOffice
    Correção: no valor todal das modificações (Cardapio 2.18.1)
    Correção: Stone (WS2) Correção da identificação de Debito e Outras formas de pagamento
    Correção: Número do pedido no SAT e informações da Mesa/Comanda/Delivery

2.18.0.20 (07/09/2018) => REVERTIDO na 2.18.0.23
!   Melhoria: Numero da versão das aplicações Android 
!   Melhoria: Atualizada e padronizada versão da Comanda, Cardapio, Pesquisa para versão 2.18.0 igual aos softwares Windows

2.18.0.19 (01/09/2018)
    Novo: Gravando Bandeira desconhecida junto com autorização
    Novo: Tela de Pagamentos Touch com botão de Voltar
    Novo: Botão de Voltar em Produtos Touch
    Novo: Logando erros do Backoffice e Terminal Windows
    Melhoria: Informações do UDP e do Cliente SlashScreen
    Melhoria: Criado novos código de erros
    Melhoria: Processo do atualizador
    Correção: Padronização do Truncamento em todo o sistema
    Correção: SAT desagrupa itens quando necessário por causa do truncamento
    Correção: Scroll e Rezise da tela de Pagamentos Touch
    Correção: Identficação do telefone no IFood para cliente antigos com DDD errado

2.18.0.18 (30/08/2018) ATUALIZADOR
    Novo: SplashScreen com atualizador para o Caixa, BackOffice e Terminal Windows
    Novo: Atualização via mensagem para o integrador
    TDD: Teste unitário para resolver eventuais problemas de chave de hardware no CakeERP

2.18.0.17 (28/08/2018)
    Remoção da pasta Areas/HelpPage do WS2 pois não é usada para nada e tinha 20MB
    Tratamento de erros de travamento, verificação da DLL, testes unitários e isolamento do SAT
    Correção Autenticação de Gerente no BackOffice
    TesteUnitário para API de Comandas e Pedidos (EasyChopp)

2.18.0.16 (20/08/2018) 
    Correção BUG: Eventos desconto no pagamento touch
    Melhoria mensagens de erro Pedido não Existente
    Status de não configurado no integrador para serviços que presisam ser configurados

2.18.0.15
    Teste de melhoria no Sync do Cake ERP => Não Funcionou

2.18.0.14 (20/08/2018) GA e Licença
    Bloqueio de versão específica e revalidação a cada 30 minutos
    Eventos e Erros no GA Identificando o Cliente, Versão, PDV, Usuário sempre que possivel

2.18.0.13 (17/08/2018) Touch
    Corrigido dígito final do telefone no Delivery
    BackOffice permite gerente ter acesso a cadastros e cardapio apenas
    Revisado Scroll quadros Touch em Produtos, Categorias, Pagamento
    Parametros Touch para Tamanho da Fonte, dos botões, e tempo de Cache
    Autenticação Touch para Caixa Touch
    
2.18.0.12 (14/08/2018) CPF SAT
    Melhoria: No BackOffice mostra mensagem mais mais amigável quando SAT ultrapassa o valor maximo permitido de R$ 10.000 https://www.confaz.fazenda.gov.br/legislacao/ajustes/2014/AJ_008_14
    Correção e Validações com com roteiro dos casos de CPF/CNPJ no SAT para SAT e melhorias

2.18.0.11 (14/08/2018)
    Correção BUG: Listagem de Pagamentos Parciais no Pagamento Touch
    Correção BUG: CPF/CNPJ no SAT na tela de Pagamento Touch

2.18.0.10 (09/08/2018)
    Revisão dos tipos Tickets
    Revisão do registro de erro por versão mínima
    Impressão de referencia de endereço e nome fantasia

2.18.0.9 (07/08/2018)
    Correção Terminal Windows
    Correção na validação de Modificações e Exclusão de Produtos
    Ajustes na Homologação Stone (SAT, Conta, Metodo de pagamento)

2.18.0.8 (06/08/2018)
    Backup Diário do Banco SQL
    Melhoria de exibição em casos de Erro da Loggi
    Flag de configurações geral para as integrações: CackeERP, IFOOD, Loggi
    Atualização de Preco e disponibilidade do IFOOD
    Melhoria na validação das modificações no Terminal e Caixa (quando não é assistente de modificações)
    Alerta de Cliente Bloqueado para abrir comanda com checkin

2.18.0.7 (05/08/2018)
    Correção Delivery Loggi
    Melhorado a identificação do Tipo de pedido padrão
    Melhorada mensagem de delivery pendente
    Revalidação se há fechamento do dia pendente
    Senha de gerente para finaliar transferencia de produto (Configurável)
    Padronização do CaixaTouch para adição de Produtos e Pagamento 
    Melhorias em algumas querys com (NOLOCK)
    Correção do CaixaTouch sem dependenter das configurações do TerminalWindows
    Correção dos textos de confirmação de impressão de venda
    Correção da exportação de CFe para pasta

2.18.0.6 (04/08/2018)
    Correção Pagamento Touch
    Correção Busca CFe
    Correção Validação de Licença

2.18.0.5 (03/08/2018)
    Pagamento Touch

2.18.0.4 (02/08/2018)
    Nova opção no configurador para imprimir cupom fiscal
    Envio de email com cupon fiscal

2.18.0.3 (30/07/2018)
    Correção dos Status do Produto no IFOOD
    Homologando Stone

2.18.0.2 (26/07/2018)
    Correção CEP Loggi
    Totais do resumo diário
    CakeERP com iFood
    Corrigido priorizade do Gerenciador de impressão
    Correção de emisão de SAT pelo Backoffice
    Redução nos detalhes de mensagem de "Aviso" no integrador (erros de impressão)

2.18.0.1 (24/07/2018 Só Homologação)
    SAT (tratamento do erro de violação de memoria: <legacyCorruptedStateExceptionsPolicy> e [HandleProcessCorruptedStateExceptions])
    Novo Layout do SAT por "Imagem" para Caixa, Stone e Gerenciador de Impressão
    Reorgaizada Rotas do WS2
    CEP no Delivery
    Melhoria na detecção do limite de venda da comandas
    Correção de busca de Clientes
    Melhorias nos tratamentos internos de erros para IFOOD e LOGGI

2.18.0.0 (17/07/2018 Só Homologação)
    Loggi Preto (Configurações, EF, Caixa Delivery)


2.18 ======================================================================


2.17.17.5 (Só Homologação)
    CakeERP validação da chave de hardware
    Validação de cadastro contra caracteres especiais para produtos e clientes
    Limpeza de banco para telefones NULOS
    Limpeza padrão de erros antigos
    MyFinance descontinuado
    Configurador está no integrador, juntamente com um novo layout e opção de sair só para o adminsitrador principal
    Somente o integrador atualiza o banco, e os aplicativos validam o funcionamento correto com o banco
    SAT somente via WebServices
    Não requer mais ser adminsitrador no caixa

2.17.17.4
    Operações de gravação de cliente migrado totalmente para EF
    Melhoria na notificação de erros
    No caixa Produtos Touch não solicita preço quando é zero em modo de assistente
    Nova PGWebLib(v4.0.44.0) no autoatendimento
    Timeout de 1 minuto apos a inicialização das DLL de pagamento no autoatendimento

2.17.17.3
    Atualização NTK TEF DLL PGWebLib: v4.0.44.0
    Desconexão ao final das transações para envio de confirmação no POS Integrado NTK
    Tela de extrato de créditos no caixa
    Historico CFe melhorado (Buscas, Emissão, envio XML)
    Melhoria no tratamento de erros de impressão e estoque em vendas balcão

2.17.17.2
    Arquivos e logs do TEF NTK em C:\PayGo
    Consulta de creditos no autoatendimento
    Digitação de credito no teclado de valor
    Launcher Cardapio 1.10.3
    Removido sincronismo do status do produto no CakeERP

2.17.17.1
    Migrations !!! Novos campos na tabela tbComanda: Codigo e IDCliente
    Controle de comanda por número ou código
    Melhoria no sincronismo CakeERP: Clientes, Pedidos, SAT
    Revisado comparação de pedidos levando em conta só valor dos itens
    Edição de Modificação permite receitas

2.17.17.0
    Integração: POS Integrado Stone (CORS WS2)
    Integração: Cake ERP
    Notificação de erros por email, e nova tabela de erros po código na API:wsUtil

2.17.16.14
    Correção nos pagamentos do delivery
    Correção na leitura do código impresso na balança para caixa touch
    Otimização e padronização na leitura da ConnectionString
    
2.17.16.13
    Correção troco no delivery, e ajuste de exclusão de pagamento
    Padronização de fechamento de pagamento com gateways
    Cardápio por Comanda
    ! Remoção do botão de correção do estoque
    !!! EF: PROXY OFF !!!

2.17.16.12
    Correção Terminal Windows
    Correção de estoque para fechamentos via WS2 (POS, Autoatendimento, Saida)

2.17.16.11
    IFOOD com toque por audio "Telefone.wav", melhoria no status da loja, e na identificação do pedido geral
    Novo loho horizontal na pré-conta
    Bandeira ELO

2.17.16.10
    Correção da impressão de conta via ordem de impressão (melhoria na gestão da fila)
    Correção na listagem de produtos do autoatendimento (Listas DAL e WS2, e controle de disponibilidade)
    Correção de modal do autoatendimento
    Cliente UDP no Autoatendimento

 2.17.16.9
    Atualização NTK POS v4.0.45.3 - PTI: v2.0.3.0
    IFOOD: Impressão do código numero do Pedido IFOOD no via do motoboy, e o numero do documento no pedido

2.17.16.8
    Informações do servidor UDP no integrador
    Removido tarefa do WS2 o sincronismo de licenças
    Correção na limpeza do IFOOD de versões anteriores a 2.17.*
    Correção nos processos asincronos usados no WS2 (GA, Envio de Erros)

2.17.16.7
    Novo Layout Impresso de ordem de produção (menos papel, e mais legível)
    Refatoração dos componentes de impressão
    Serviço de UDP para detectar o servidor SQL e o endereço do WS2

2.17.16.6
    Cancelamento do ERP e desativando as opções, mas mantendo o código com carga de Pedidos, Clientes, e sincronizando Produtos
    Desativado PagSeven
    IFOOD homologado, melhoria de fluxo, ajustes finais
    Tela de delivery com pagamentos detalhados
    Identificação de bandeiras: AMEX, MASTERCARD D, MAESTROCP, VISA ELECTRO
    Instalador não permite erros na instalação (Evita Instalação Corrompida)
    Resisando e colocando mais rastreamento no GA

2.17.16.5
    Correção e abertura do caixa para Autoatendimento, iFood e POS integrado
    Revisado fluxo de abertura de comanda
    Revisado querys de relatórios

2.17.16.4
    Validação do valor total da comanda não considera os produtos cancelados
    Transferencia de produtos copia a data original da compra do item
    Verificação pelo tipo de pagamento e não pela primeira opção no pagamento para habilitar troco
    Validação do status de conta solicitada na transferencia de mesa
    Correção Fluxo TEF PAGO (loop de mensagem: remova o cartão, evento de cancelamento, log da comunicação)
    Melhoria do Cardapio Digital

2.17.16.3
    Agrupamento de comandas não permite comandas compagamento parciais, e foi adicionado LOG
    Melhorado descrição de erros no SAT
    Emissão de nota com CPF no backoffice
    Melhorado filtro no Histórico CFe
    No BackOffice, a lista de PDV preve tipos desconhecidos
    Validação de valores de preço e quantidades
    Pede senha para desconto de item de acordo com a configuração
    Novas imagens e layout na pesquisa de satisfação
    Correção de Cancelamento no Delivery
    Correção iFood: Tradução dos meios de pagamento para SAT
    Correção valor total de produtos do autoatendimento

2.17.16.2
    Logs de transações em banco de dados
    Limite de QTD e Valor de produto
    Google Analytics - Valor Pedidos e Erros
    Com creditos habilitados, a Entrada e Creditos vão sempre para venda balcão, junto com qualquer coisa a mais
    Corrigido Cancelamento iFood
    Removido tela de fechamento de conta, quando há opção de imprimir conta no fechamento do pedido
    tipo de entrega iFood (fica inativo, apenas para não exibir na lista)
    Configuração em qual caixa aparecerá notificaçação de ifood
    Configuração fechamento de comanda no fechamento do dia
    Tela de confirmação do pedido, com visualização do pedido original
    Remoção do projeto www (WS1)
    Não exibe todos produtos, ou os sem categoria, na tela de disponibilidade
    Pagina de Avaliação do estabeleciomento

2.17.16.1
    iFood
    Google Analytics

2.17.15.7
    Correção de compra de credito
    Correção de limpar licença

2.17.15.6
    Revisão Ativador 
    Revisão e erro específico para quando houver mais de 10 pagamentos agrupados no SAT
    Transferência de produtos e LOG
    Revisão de usabilidade no cadastro da taxa de entrega e entregador
    Correçãode limpar licenças no configurador
    Nova versão de comanda, corrigindo painel de modificações
    Revisão autoatendimento (tela de fim)
    Melhorado os tratamentos de erro no Histórico CFe, e não emite nota de pedidos Zerados
    Inicio de isolamento de contexto da DLL do SAT

2.17.15.5
    Título Crédito/Debito no relatório de créditos
    Autoatendimento: SUPRA - Checkin, Venda de Creditos, padronização de nomes
    Atualizado projetos de ativação
    Correção BUG: Pré-conta a taxa de entrega para ECF

2.17.15.4
    Revalidado leitura de balança no Caixa Normal/Touch, Terminal Windows
    Confirmação para fechar comanda no Autoatendimento: Axado, e tela de processamento
    No cadastro de painel de nodificação do backoffice, busca de modificação busca de acordo com o tipo
    Melhoria na identificação de erros para o POS Integrado
    Removida senhas Padrão "2010"

2.17.15.3
    Liberado Autoatendimento: Axado Bar
    Autoatendimento: Fechamento de comanda vazia, e revalidação do pedido validação 
    Configuração da margem e tamanho do ticket no autoatendimento e caixa
    Configuravel cor do circulo do autoatendimento
    Localizar Banlança adicionado
    
2.17.15.2
    Remoção do contexto generico do EF: Ativação, PDV, Gateway, ContaRecebivel
    Layout SAT para POS Integrado (Código de Barras e QRCode)
    Ajuste de área de impressão para o Autoatendimento (Padrão 10 de margem por 280 de largura)
    Logs no POS integrado

2.17.15.1
    Tickets Gráficos no Caixa igual ao autoatendimento e configurações de titulo, rodapé, validade
    Revisão e limpeza de algumas variáveis de Autenticação, PDV(EF)
    Padronização do número da versão nos programas auxiliares (MesaComanda, Terminal de Saída)
    Descrição mais detalhada do erro no programa de MesaComanda
    Configuração PDV na chave de configuração da PAGO: Caixa e Autoatendimento
    Uso de enumerador de configuração para ler de variável única
    Validação de data de nascimento acima do ano de 1900, até o ano atual e mascara na comanda
    OrdemImpressao individual para Autoatendimento, Comanda, Terminal TAB, Terminal Windows

2.17.15.0
    Liberado Autoatendimento (DOME)
    POS Integrado: PTI 2.0.2.0 QRCode e Código de Barras
    Correção BUG: Valida área de impressão para comanda/terminal tab
    Correção BUG: Leitura de algumas configurações antigas

2.17.14.1
    Não permite ativar assitente de modificações sem modificações no backoffice, e tratamento para produtos em modificações no caixa
    Atualiação dos relatórios para criação e edição
    Revisão e correção da criação inicial do banco
    Bloqueio de multipla execução dos programas
    Teste de operação local do SAT (sem WS)
    Não imprime produto como serviço na ECF quando configurado

2.17.14.0
    Limpeza de pedidos cancelados em fechamento do dia
    Cancelamento dos pedidos balcão em aberto
    Novo campo RG no cadastro de clientes
    Validação configurável para RG, CPF, e Data de Nascimento
    Data e Hora na lista de pedidos
    SuccessAudit log no processo de migração do banco com as alterações aplicadas
    Autoatendimento para Pedidos e pagamento de Comanda

2.17.13.2
    Opção de Pré-Conta na venda Venda Balcão
    Adicionada referencia a SqlServerTypes em todos projetos de UI para evitar referencia padrão incompatível
    STATUS do processo do SAT

2.17.13.1
    Web.config no WS do instalador !sempre substitui!
    POS Integrado, corrigido cancelamento de pagamento parcial
    Textos de rodapé para WS2 (conta e SAT)
    Nome do fabricante SAT no TXT do SAT via WS2

2.17.13.0
    Certificado e DLL TEF
    Ajuste foco PayGoWeb (Troca de arquivo)

2.17.12.8
    SAT no POS integrado
    Ajustes e finalização do fluxo POS
    Eventos de Log de Erro

2.17.12.7
    Impressão gerencial da via do motoboy para ecf
    mais detalhes no envio de erros
    Ajustes de formatação na ECF: #,##0.00 e dado da tafa de entregue

2.17.12.5
    Não valida o ultimo digito da versão minima, permitindo FIX rapidos sem ter que trocar tudo
    Corrigido problema eventual de dupla migração pelo SEED do EF
    TEF e POS grava os 2 códigos de autorização (AUTLOCREF+AUTHCODE)
    Melhoria no rastreamento de erros em impressora
    Validação com POS NTK Versão 4.0.42.0
    
2.17.12.4
    Homologação NKT: TEF e POS Integrado
    Correção no configurador (configurações individuais por PDV)
    Melhoria na identificação e tratamento de problemas com licença

2.17.12.3
    Correção WS2 para ler tema e imagens de produtos
    Correção dos ID de produtos na criação inicial
    Limpando web.config WS2
    Ajuste DataUTC em Pedidos(WS2) para Integração Donus

2.17.12.2
    Recibo Via do cliente Paygo Web (Troca de arquivo)

2.17.12.1
    Ajuste no integrador para não exibir messagebox, tudo aparecerá nos logs 
    Limpeza dos logs quando atingem 10K de texto
    Ajuste ContentType no WS2

2.17.12.0
    AutoAtendimento (iDTipoPDV, Configurações, XML) 
    Removido NLog de Ativações
    Removido NLog do SAT
    SaidaComanda
    BUG: Troco em Dinheiro gerando valores errados em PedidoPagamento
    Instalador com icones dos novos programas e registrando o integrador para ligar ao entrar no Windows (via chave de registro)

2.17.11.6
    Validação de alterações por Hash nas telas de pagamento e transferencia de produtos

2.17.11.5
    Validação para não deixar dar desconto quando não há valor a pagar

2.17.11.4
    Log para tentar identificar problema quando dá erro ao imprimir SAT

2.17.11.3
    Bloqueio de Transferência de produtos de mesa comanda quando há pagamentos parciais
    BUG: Instalador do Caixa em sem Terminal

2.17.11.2
    Revisada logica de desconto na tela de pagamento
    
2.17.11.1
    BUG: Erro ao ler configurações do caixa iDPDV

*/
