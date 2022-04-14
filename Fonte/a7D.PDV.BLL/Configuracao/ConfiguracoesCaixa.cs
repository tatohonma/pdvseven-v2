using a7D.PDV.EF;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.BLL
{
    public class ConfiguracoesCaixa : ConfiguracoesComumPedidos
    {
        public static ConfiguracoesCaixa Valores { get; private set; }

        public ConfiguracoesCaixa(int idPdv) :
            base((int)ETipoPDV.CAIXA, idPdv)
        {
            Valores = this;
        }

        public static void Recarregar()
        {
            new ConfiguracoesCaixa(AC.idPDV);
        }

        [Config("Abrir gaveta após cada venda", ETipoPDV.CAIXA, ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool AbrirGaveta { get; protected set; }

        [Config("Usar Código Leitora em Hexadecimal no Caixa", ETipoPDV.CAIXA, EConfig.ComandaCodigoHEX, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ComandaCodigoHEX { get; protected set; }

        [Config("Quantidade de Dígitos para o Código do produto", ETipoPDV.CAIXA, Valor = "4", Obrigatorio = true)]
        public int DigitosCodigo { get; protected set; }

        [Config("Utiliza Balança Etiquetadora", ETipoPDV.CAIXA, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool EtiquetaBalanca { get; protected set; }

        [Config("Leitura de código de barras", ETipoPDV.CAIXA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool CodigoBarras { get; protected set; }

        [Config("Tipo de Ticket Pré-Pago", ETipoPDV.CAIXA, Valor = "0", ValoresAceitos = "0:Nenhum|1:Padrão (antigo)|2:Últimos 2 dígitos do número do pedido|3:Ticket por Produto", Obrigatorio = true)]
        public int GerarTicketPrePago { get; protected set; }

        [Config("Margem de Impressão Windows", ETipoPDV.CAIXA, EConfig.MargemImpressaoWindows, Valor = "0", Obrigatorio = true)]
        public int MargemImpressaoWindows { get; protected set; }

        [Config("Largura de Impressão Windows", ETipoPDV.CAIXA, EConfig.LarguraImpressaoWindows, Valor = "280", Obrigatorio = true)]
        public int LarguraImpressaoWindows { get; protected set; }

        [Config("Nome da Fonte de Impressão Padrão", ETipoPDV.CAIXA, EConfig.FonteNomeImpressaoWindows, Valor = "Arial", Obrigatorio = true)]
        public string FonteNomeImpressaoWindows { get; protected set; }

        [Config("Tamanho da Fonte de Impressão Padrão", ETipoPDV.CAIXA, EConfig.FonteTamanhoImpressaoWindows, Valor = "10", Obrigatorio = true)]
        public int FonteTamanhoImpressaoWindows { get; protected set; }

        [Config("Título do Ticket Pré-Pago", ETipoPDV.CAIXA, Valor = "Ticket Pré-Pago")]
        public string TituloTicketPrePago { get; protected set; }

        [Config("Texto de validade do Ticket Pré-Pago", ETipoPDV.CAIXA, Valor = null)]
        public string ValidadeTicketPrePago { get; protected set; }

        [Config("Rodape do Ticket Pré-Pago", ETipoPDV.CAIXA, Valor = null)]
        public string RodapeTicketPrePago { get; protected set; }

        [Config("Manter posição selecionada na lista", ETipoPDV.CAIXA, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ManterPosicaoLista { get; protected set; }

        [Config("Nome da Impressora", ETipoPDV.CAIXA, EConfig.ModeloImpressora, Valor = "")]
        public string ModeloImpressora { get; protected set; }

        [Config("Modo Fiscal", ETipoPDV.CAIXA, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ModoFiscal { get; protected set; }

        [Config("Porta da Balança", ETipoPDV.CAIXA, Valor = "COM1", Obrigatorio = true)]
        public string PortaBalanca { get; protected set; }

        [Config("Porta da Impressora", ETipoPDV.CAIXA, Valor = "USB001", Obrigatorio = true)]
        public string PortaImpressora { get; protected set; }

        [Config("O que está sendo impresso na etiqueta", ETipoPDV.CAIXA, Valor = "peso", ValoresAceitos = "peso:Peso|preco:Preço", Obrigatorio = true)]
        public string PrecoPeso { get; protected set; }

        public bool BalancaPorPeso => string.Compare(PrecoPeso, "peso", true) == 0;

        [Config("Protocolo de comunicação da Balança", ETipoPDV.CAIXA, Valor = "TOLEDO", ValoresAceitos = "TOLEDO|FILIZOLA", Obrigatorio = true)]
        public string ProtocoloBalanca { get; protected set; }

        [Config("Tipo Gerenciador Impressão", ETipoPDV.CAIXA, EConfig.TipoGerenciadorImpressao, Valor = "0", ValoresAceitos = "0:Sem Impressora (Gerencial)|1:Impressora Windows (Gerencial)|2:ECF via ACBr|4:S@T ou NFCe em Impressora Windows", Obrigatorio = true)]
        public int TipoGerenciadorImpressao { get; protected set; }
        public ETipoGerenciadorImpressao GerenciadorImpressao => (ETipoGerenciadorImpressao)TipoGerenciadorImpressao;

        [Config("Tipo de pedido Padrão", ETipoPDV.CAIXA, Valor = "10", ValoresAceitos = "10:Mesa|20:Comanda|30:Delivery|40:Balcão|50:Retirada", Obrigatorio = true)]
        public int TipoPedidoPadrao { get; protected set; }

        [Config("Velocidade da Impressora", ETipoPDV.CAIXA, Valor = "115200", ValoresAceitos = "9600:Serial|115200:USB", Obrigatorio = true)]
        public int VelocidadeImpressora { get; protected set; }

        [Config("Habilitar Tipo de Pedido Balcão", ETipoPDV.CAIXA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool TipoPedidoBalcao { get; protected set; }

        [Config("Habilitar Tipo de Pedido Comanda", ETipoPDV.CAIXA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool TipoPedidoComanda { get; protected set; }

        [Config("Habilitar Tipo de Pedido Delivery", ETipoPDV.CAIXA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool TipoPedidoEntrega { get; protected set; }

        [Config("Habilitar Tipo de Pedido Mesa", ETipoPDV.CAIXA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool TipoPedidoMesa { get; protected set; }

        [Config("Habilitar Tipo de Pedido Retirada", ETipoPDV.CAIXA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool TipoPedidoRetirada { get; protected set; }

        [Config("Exibir confirmação de Pedido Delivery (ifood)", ETipoPDV.CAIXA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool ConfirmarDelivery { get; protected set; }

        [Config("Usar Caixa Touch", ETipoPDV.CAIXA, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool CaixaTouch { get; protected set; }

        [Config("Caixa Touch Parametros (Fonte, Largura, Altura, Tempo de Cache)", ETipoPDV.CAIXA, Valor = "11,100,50,1")]
        public string CaixaTouchParametros { get; protected set; }

        [Config("Imprimir pré conta na venda balcão", ETipoPDV.CAIXA, Valor = "NAO", ValoresAceitos = "SIM:Sim|NAO:Não|Perguntar:Perguntar", Obrigatorio = true)]
        public string PreContaVendaBalcao { get; protected set; }

        //[Config("Chave de Configuração TEF Pago", ETipoPDV.CAIXA, EConfig.PagoChave, Valor = "")]
        //public string PagoChave { get; protected set; }

        [Config("Notificar pedido Delivery Integrado (IFOOD)", ETipoPDV.CAIXA, Valor = "AUDIO", ValoresAceitos = "NAO:Não notificar|SIM:Notificar com tela|AUDIO:Notificar com tela e tocar áudio", Obrigatorio = true)]
        public string NotificarDelivery { get; protected set; }

        [Config("Imprimir comprovante no final da Venda (Impressora Windows ou SAT/NFCe)", ETipoPDV.CAIXA, Chave = EConfig.ImprimirCupomFiscal, Valor = "SIM", ValoresAceitos = "SIM:Sim|NAO:Não", Obrigatorio = true)]
        public string ImprimirCupomFiscal { get; protected set; }

        [Config("", ETipoPDV.CAIXA, Chave = EConfig._StatusImpressora)]
        public string _StatusImpressora { get; protected set; } // Data, Hora e da impressão quando não estiver OK

        [Config("", ETipoPDV.CAIXA, Chave = EConfig._PedidoProdutoLocationSize)]

        public string _PedidoProdutoLocationSize { get; protected set; }

        [Config("", ETipoPDV.CAIXA, Chave = EConfig._PedidoPagamentoLocationSize)]

        public string _PedidoPagamentoLocationSize { get; protected set; }

        [Config("", ETipoPDV.CAIXA, Chave = EConfig._ClientePesquisaLocationSize)]

        public string _ClientePesquisaLocationSize { get; protected set; }

        [Config("", ETipoPDV.CAIXA, Chave = EConfig._ClienteSaldoLocationSize)]

        public string _ClienteSaldoLocationSize { get; protected set; }

        [Config("", ETipoPDV.CAIXA, Valor = "750", Chave = EConfig._PedidoSpliterSize)]
        public int _PedidoSpliterSize { get; protected set; }

        [Config("", ETipoPDV.CAIXA, Valor = "001", Chave = EConfig.GranitoIDPDV)]
        public string GranitoIDPDV { get; protected set; }
    }
}