namespace a7D.PDV.EF.Enum
{
    public enum EConfig
    {
        _none = 0,
        ChaveAtivacao,
        IDAreaViaExpedicao, ImprimirViaExpedicao,

        // Caixa, Comanda, Terminal Tab, Terminal Windows, Autoatendimento
        OrdemImpressao, ImprimirCupomFiscal, TipoGerenciadorImpressao, ModeloImpressora,

        // Caixa, AutoAtendimento
        PagoChave, LarguraImpressaoWindows, MargemImpressaoWindows,

        // Autoatendimento
        ImpressaoLocal, SenhaSaida, ChaveUsuario,
        ExibirMouse, TimeoutInativo, TimeoutAlerta, VerificarDisponibilidade, MeioPagamento,
        GerarTicketPrePago, TituloTicketPrePago, ValidadeTicketPrePago, RodapeTicketPrePago,
        HabilitarIFood, AprovarIFood,
        FonteNomeImpressaoWindows,
        FonteTamanhoImpressaoWindows,
        ComandaCodigoHEX, GranitoIDPDV,

        // Campos ocultos
        _VersaoServidor,
        _ERPUltimoSincronismo,
        _StatusImpressora,
        _PedidoProdutoLocationSize,
        _PedidoPagamentoLocationSize,
        _ClientePesquisaLocationSize,
        _ClienteSaldoLocationSize,
        _PedidoSpliterSize,
    }
}
