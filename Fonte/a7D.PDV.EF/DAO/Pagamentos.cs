using a7D.PDV.EF.DTO;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using System;

namespace a7D.PDV.EF.DAO
{
    public class Pagamentos
    {
        public static PagamentoTEF[] ListarTEFStone()
        {
            string sql = $@"SELECT IDPedidoPagamento, IDPedido, DataPagamento, Autorizacao, Valor 
                from tbPedidoPagamento 
                where IDGateway={(int)EGateway.StoneTEF} 
                and DataPagamento>=@p0
                and Excluido=0
                ORDER BY DataPagamento DESC";

            return Repositorio.Query<PagamentoTEF>(sql, DateTime.Now.AddMinutes(-30));
        }

        public static void Cancelar(int iDPedidoPagamento)
        {
            // TODO: Cancelar pedido, cupom fiscal, e se for pagamento parcial???
            // TODO: O pedido deve ser reaberto e o cupom fiscal cancelado

            var pg = Repositorio.Carregar<tbPedidoPagamento>(p => p.IDPedidoPagamento == iDPedidoPagamento);
            pg.Excluido = true;
            //pg.Autorizacao += "-cancelado";
            Repositorio.Atualizar(pg);
        }
    }
}
