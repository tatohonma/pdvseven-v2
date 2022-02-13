using a7D.PDV.Model;

namespace a7D.PDV.Fiscal
{
    public interface IEnviarCancelamento
    {
        RetornoSATInformation Enviar(out PedidoInformation pedido);
    }
}
