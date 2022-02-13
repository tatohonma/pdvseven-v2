using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using System;
using System.Runtime.Serialization;

namespace a7D.PDV.BLL
{
    [Serializable]
    [DataContract]
    public abstract class ConfiguracoesComumPedidos : ConfiguracaoBD
    {
        protected int IDPDV { get; set; }

        protected ConfiguracoesComumPedidos(int? idTipoPDV, int? idPDV) : base(idTipoPDV, idPDV)
        {
        }

        // Para compatibilidade com comanda/cardapio antigo via ASMX é necessário do [DataMember] e um setter publico (não pode ser protected)
        [DataMember]
        [Config("Gerar ordem de produção", ETipoPDV.TERMINAL_TAB, EConfig.OrdemImpressao, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Gerar ordem de produção", ETipoPDV.CARDAPIO_DIGITAL, EConfig.OrdemImpressao, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Gerar ordem de produção", ETipoPDV.COMANDA_ELETRONICA, EConfig.OrdemImpressao, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Gerar ordem de produção", ETipoPDV.CAIXA, EConfig.OrdemImpressao, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Gerar ordem de produção", ETipoPDV.TERMINAL_WIN, EConfig.OrdemImpressao, Valor = "1", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool OrdemImpressao { get; set; }

        [DataMember]
        [Config("Usar áreas de produção", ETipoPDV.TERMINAL_TAB, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Usar áreas de produção", ETipoPDV.CARDAPIO_DIGITAL, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Usar áreas de produção", ETipoPDV.COMANDA_ELETRONICA, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Usar áreas de produção", ETipoPDV.CAIXA, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        [Config("Usar áreas de produção", ETipoPDV.TERMINAL_WIN, Valor = "0", ValoresAceitos = "0|1", Obrigatorio = true)]
        public bool UsarAreas { get; set; }

        [DataMember]
        [Config("Áreas de produção padrão (ID1, ID2, IDn)", ETipoPDV.TERMINAL_TAB, Valor = "0", Obrigatorio = true)]
        [Config("Áreas de produção padrão (ID1, ID2, IDn)", ETipoPDV.CARDAPIO_DIGITAL, Valor = "0", Obrigatorio = true)]
        [Config("Áreas de produção padrão (ID1, ID2, IDn)", ETipoPDV.COMANDA_ELETRONICA, Valor = "0", Obrigatorio = true)]
        [Config("Áreas de produção padrão (ID1, ID2, IDn)", ETipoPDV.CAIXA, Valor = "0", Obrigatorio = true)]
        [Config("Áreas de produção padrão (ID1, ID2, IDn)", ETipoPDV.TERMINAL_WIN, Valor = "0", Obrigatorio = true)]
        public string AreasPadrao { get; set; }

        [DataMember]
        [Config("Impressão da via de expedição em Produto para Viagem", ETipoPDV.TERMINAL_TAB, EConfig.ImprimirViaExpedicao, Valor = "NAO", ValoresAceitos = "NAO:Não|PEDIDO:Imprimir a cada novo pedido|CONTA:Imprimir ao solicitar conta ou fechamento", Obrigatorio = true)]
        [Config("Impressão da via de expedição em Produto para Viagem", ETipoPDV.CARDAPIO_DIGITAL, EConfig.ImprimirViaExpedicao, Valor = "NAO", ValoresAceitos = "NAO:Não|PEDIDO:Imprimir a cada novo pedido|CONTA:Imprimir ao solicitar conta ou fechamento", Obrigatorio = true)]
        [Config("Impressão da via de expedição em Produto para Viagem", ETipoPDV.COMANDA_ELETRONICA, EConfig.ImprimirViaExpedicao, Valor = "NAO", ValoresAceitos = "NAO:Não|PEDIDO:Imprimir a cada novo pedido|CONTA:Imprimir ao solicitar conta ou fechamento", Obrigatorio = true)]
        [Config("Impressão da via de expedição em Produto para Viagem", ETipoPDV.CAIXA, EConfig.ImprimirViaExpedicao, Valor = "NAO", ValoresAceitos = "NAO:Não|PEDIDO:Imprimir a cada novo pedido|CONTA:Imprimir ao solicitar conta ou fechamento", Obrigatorio = true)]
        [Config("Impressão da via de expedição em Produto para Viagem", ETipoPDV.TERMINAL_WIN, EConfig.ImprimirViaExpedicao, Valor = "NAO", ValoresAceitos = "NAO:Não|PEDIDO:Imprimir a cada novo pedido|CONTA:Imprimir ao solicitar conta ou fechamento", Obrigatorio = true)]
        public string ImprimirViaExpedicao { get; set; }

        [DataMember]
        [Config("ID área de produção para via de expedição", ETipoPDV.TERMINAL_TAB, EConfig.IDAreaViaExpedicao, Valor = "0")]
        [Config("ID área de produção para via de expedição", ETipoPDV.CARDAPIO_DIGITAL, EConfig.IDAreaViaExpedicao, Valor = "0")]
        [Config("ID área de produção para via de expedição", ETipoPDV.COMANDA_ELETRONICA, EConfig.IDAreaViaExpedicao, Valor = "0")]
        [Config("ID área de produção para via de expedição", ETipoPDV.CAIXA, EConfig.IDAreaViaExpedicao, Valor = "0")]
        [Config("ID área de produção para via de expedição", ETipoPDV.TERMINAL_WIN, EConfig.IDAreaViaExpedicao, Valor = "0")]
        public int IDAreaViaExpedicao { get; set; }

    }
}
