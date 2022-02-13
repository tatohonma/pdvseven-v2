using System;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Pagamento
{
    public interface ITEF
    {
        string Bandeira { get; }
        string Adquirente { get; }
        bool Debito { get; }
        string Mensagem { get; }
        string Autorizacao { get; }
        string ViaEstabelecimento { get; }
        string ViaCliente { get; }
        Exception Erro { get; }
        decimal Valor { get; }
        bool PagamentoConfirmado { get; }
        bool PrecisaSelecionar { get; }
        bool Processando();
        void Cancelar();
        Task AguardaTransacao(StatusUpdateCallBack respostaTEF = null);
        void Estornar();
        void DefinirMetodoPagamento(MetodoPagamento metodo, int parcelas);
        string Log { get; }
    }

    public enum MetodoPagamento
    {
        Selecione = -1,
        Debito = 0,
        Credito = 1
    }
}