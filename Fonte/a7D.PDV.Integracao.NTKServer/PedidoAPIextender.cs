using a7D.PDV.EF.Enum;
using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Integracao.API2.Model;
using a7D.PDV.Integracao.Pagamento.NTKPos;
using System;
using System.Collections.Generic;

namespace a7D.PDV.Integracao.NTKServer
{
    public static class PedidoAPIextender
    {
        internal static  ResultadoOuErro Pagar(this PedidoAPI api,  TerminalInformation terminal)
        {
            var pagamentos = new List<API2.Model.Pagamento>();
            try
            {
                int idMetodo = (int)EMetodoPagamento.Credito; // padrão
                if (terminal.pagamento.cardType == 2)
                    idMetodo = (int)EMetodoPagamento.Debito;
                else if( terminal.pagamento.cardType == 3)
                    idMetodo = (int)EMetodoPagamento.Loja; // Fidelidade
                else if (terminal.pagamento.cardType == 4)
                    idMetodo = (int)EMetodoPagamento.Presente;

                pagamentos.Add(new API2.Model.Pagamento()
                {
                    TipoPagamento = new TipoPagamento() { IDGateway = (int)EGateway.NTKPOS },
                    Valor = (decimal)terminal.valorTEF,
                    Autorizacao = terminal.pagamento.autorizacao,
                    ContaRecebivel = terminal.pagamento.adquirente,
                    Bandeira = terminal.pagamento.cardName,
                    IDMetodo = idMetodo,
                });

                var data = new InsercaoPagamento(
                    Guid.NewGuid().ToString(),
                    terminal.tipo, terminal.numero,
                    terminal.pdvUserID, terminal.pdvID,
                    pagamentos);

                return api.AdicionaPagamento(data);
            }
            catch (Exception ex)
            {
                return new ResultadoOuErro((int)BLL.CodigoErro.E100, ex.Message);
            }
        }
   }
}
