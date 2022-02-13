using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesSistema : ConfiguracaoBD
    {
        // Singleton com expiração simples!
        private static object _lock = new object();
        private static DateTime _expirar = DateTime.Now;
        private static ConfiguracoesSistema _valores = null;
        public static int TimeRefresh = 15;

        public static ConfiguracoesSistema Valores
        {
            get
            {
                if (_valores == null || _expirar < DateTime.Now)
                {
                    lock (_lock)
                    {
                        _expirar = DateTime.Now.AddMinutes(TimeRefresh);
                        _valores = new ConfiguracoesSistema();
                    }
                }
                return _valores;
            }
        }

        public static void Recarregar()
        {
            _expirar = DateTime.Now;
        }

        private ConfiguracoesSistema() : base(null, 0)
        {
        }

        [Config("Nome Fantasia do Estabelecimento (Impressão de Conta)", Valor = "")]
        public string NomeFantasia { get; protected set; }

        [Config("Alíquota Padrão", Valor = "II", Obrigatorio = true)]
        public string AliquotaPadrao { get; protected set; }

        [Config("Comanda com Checkin", Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ComandaComCheckin { get; protected set; }

        [Config("Habilitar 'Comandas com Conta de Cliente'", Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ComandaComCredito { get; protected set; }

        [Config("Fechar comandas com 'Conta de Cliente' no Fechamento do Dia", Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool FecharComandasCreditoDia { get; protected set; }

        [Config("Dias de validade dos crédito das 'Contas de Cliente'", Valor = "180", Obrigatorio = true)]
        public int ValidadeCreditos { get; protected set; }

        [Config("Limite devedor na 'Conta de Cliente' (Limite Fiado)", Valor = "500", Obrigatorio = true)]
        public int LimiteFiado { get; protected set; }

        [Config("DDD Padrão", Valor = "11", Obrigatorio = true)]
        public int DDDPadrao { get; protected set; }

        [Config("Altura da Imagem do Produto", Valor = "800", Obrigatorio = true)]
        public string ImagemProdutoAltura { get; protected set; }

        [Config("Altura da Thumbnail da Imagem do Produto", Valor = "80", Obrigatorio = true)]
        public string ImagemProdutoAlturaThumb { get; protected set; }

        [Config("Largura da Imagem do Produto", Valor = "1280", Obrigatorio = true)]
        public string ImagemProdutoLargura { get; protected set; }

        [Config("Largura da Thumbnail da Imagem do Produto", Valor = "128", Obrigatorio = true)]
        public string ImagemProdutoLarguraThumb { get; protected set; }

        [Config("Limite de gastos para Comanda", Valor = "300", Obrigatorio = true)]
        public int LimiteComanda { get; protected set; }

        [Config("Mensagem do Cupom Fiscal", Valor = "Sistema PDV7  www.pdvseven.com.br", Obrigatorio = true)]
        public string MsgCupom { get; protected set; }

        [Config("Permitir Pedidos com Modificações Inválidas no Caixa", Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool PermitirPedidoModificacaoInvalido { get; protected set; }

        [Config("Integração Fiscal", Valor = "none", ValoresAceitos = "none:Gerencial|SAT:S@T|NFCe:NFCe", Obrigatorio = true)]
        public string Fiscal { get; protected set; }

        [Config("Usar Código Leitora em Hexadecimal na API (WS2)", Chave = EConfig.ComandaCodigoHEX, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ComandaCodigoHEX { get; protected set; }

        [Config("Solicitar senha de gerente para Desconto", Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool SolicitarSenhaDesconto { get; protected set; }

        [Config("Solicitar senha de gerente para Transferência", Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool SolicitarSenhaTransferencia { get; protected set; }

        [Config("Solicitar tipo de Desconto", Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool SolicitarTipoDesconto { get; protected set; }

        [Config("Solicitar tipo de Desconto no Desconto por Item", Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool SolicitarTipoDescontoItem { get; protected set; }

        [Config("Habilitar 'Produto para Viagem'", Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ProdutoViagem { get; protected set; }

        [Config("Taxa de Serviço Balcão %", Valor = "0", Obrigatorio = true)]
        public string TaxaServicoBalcao { get; protected set; }

        [Config("Taxa de Serviço Comanda %", Valor = "0", Obrigatorio = true)]
        public string TaxaServicoComanda { get; protected set; }

        [Config("Taxa de Serviço Delivery %", Valor = "0", Obrigatorio = true)]
        public string TaxaServicoEntrega { get; protected set; }

        [Config("Taxa de Serviço Mesa %", Valor = "10", Obrigatorio = true)]
        public string TaxaServicoMesa { get; protected set; }

        [Config("Validar Limites de Modificações no Caixa", Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ValidarPedidoModificacaoInvalido { get; protected set; }

        [Config("Mensagem do Ticket Pré-Pago (antigo)", Valor = null)]
        public string MsgTicketPrePago { get; protected set; }

        [Config("Ativar serviço como um item do pedido", Valor = "1", ValoresAceitos = "0|1")]
        public bool ServicoComoItem { get; protected set; }

        [Config("Exibir Painel de Modificação Avançado", Valor = "1", ValoresAceitos = "0|1")]
        public bool PainelModificacaoAvancado { get; protected set; }

        [Config("Exibir itens com valor zero no caixa e pre-conta", Valor = "0", ValoresAceitos = "0|1")]
        public bool ExibirItensZeradosCaixaPreConta { get; protected set; }

        [Config("Imprimir dados completos na ordem de produção", Valor = "1", ValoresAceitos = "0|1")]
        public bool DadosClienteCompletoOrdem { get; protected set; }

        [Config("Imprimir Via da conta para Motoboy no Delivery", Valor = "0", ValoresAceitos = "0|1")]
        public bool ImprimirViaMotoboy { get; protected set; }

        [Config("Imprimir Via de Controle no Delivery", Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ImprimirViaControleDelivery { get; protected set; }

        // Na clase comum do caixa há os possíveis valores: PEDIDO e CONTA
        [Config("Impressão da via de expedição no Delivery", Chave = EConfig.ImprimirViaExpedicao, Valor = "NAO", ValoresAceitos = "NAO:Não|NOVO:Imprimir na abertura do pedido (ou confirmação iFood)|ENTREGA:Imprimir na confirmação de entrega", Obrigatorio = true)]
        public string ImprimirViaExpedicao { get; protected set; }

        [Config("ID área de impressão para via de expedição delivery", Valor = "0")]
        public int IDAreaViaExpedicao { get; protected set; }

        [Config("Layout das vias de expedição", Valor = "TOTAL", ValoresAceitos = "TOTAL:Só imprime tudo a ser expedido|NOVOS:Imprime novos itens e o total|PEDIDO:Imprime separado por solicitação", Obrigatorio = true)]
        public string LayoutViaExpedicao { get; protected set; }

        [Config("Nas comanda referência é sempre mesa", Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ReferenciaMesa { get; set; }

        [Config("Cliente deve ter 'CPF' ou 'CNPJ'", Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ClienteCPFObrigatorio { get; protected set; }

        [Config("Cliente deve ter 'RG'", Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ClienteRGObrigatorio { get; protected set; }

        [Config("Cliente deve ter 'Data de Nascimento'", Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ClienteDataNascimentoObrigatorio { get; protected set; }

        [Config("Stone Code (TEF)", Valor = "")]
        public string StoneCode { get; protected set; }

        [Config("POS integrado com Fiscal", Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool IntegrarSATPOS { get; protected set; }

        [Config("Intervalo de integração BigData+Iaago (1 a 60 min)", Valor = "30", Obrigatorio = true)]
        public int IntervaloBigData { get; protected set; }
        [Config("GranitoCode (TEF)", Valor = "")]
        public string GranitoCode { get; protected set; }
        [Config("Granito CNPJ", Valor = "")]
        public string GranitoCNPJ { get; protected set; }
    }
}
