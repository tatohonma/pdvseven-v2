using System;

namespace a7D.PDV.Fiscal
{
    public interface IFiscalApiClient : IDisposable
    {
        string Enviar();

        string XMLJSON { get; set; }

        IFiscalApiClient VendaClient(string codigoDeAtivacao, ICFeVenda cfe, int numeroSessao);
        IFiscalApiClient CancelamentoClient(string codigoDeAtivacao, int numeroSessao, string chave, string dadosCancelamento);
        IFiscalApiClient ConsultaClient(string codigoDeAtivacao, int numeroSessao);
    }
}
