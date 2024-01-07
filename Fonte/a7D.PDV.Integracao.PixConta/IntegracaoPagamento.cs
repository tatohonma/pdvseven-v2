using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.DAL;
using a7D.PDV.Integracao.PixConta.Model;
using a7D.PDV.Model;
using System;
using System.Data;
using System.Data.SqlClient;

namespace a7D.PDV.Integracao.PixConta
{
    public partial class IntegraPixConta
    {
        private void VerificarPagamentosPendentes()
        {
            var pedidos = ListarPedidosFaturaPendente();

            foreach (var pedido in pedidos)
            {
                Int32 idPedido = Convert.ToInt32(((DataRow)pedido)["IDPedido"]);
                string guidIdentificacao = ((DataRow)pedido)["GUIDIdentificacao"].ToString();
                string fatura = ((DataRow)pedido)["fatura"].ToString();

                Model.InvoiceInformation invoice = APIInvoice.ConsultarStatus(fatura);
                decimal valorDecimal = (decimal)invoice.total_cents / 100;

                switch (invoice.status)
                {
                    case "paid":
                    case "paid_external":                        
                        RegistrarPagamento(idPedido, valorDecimal);
                        Tag.Alterar(guidIdentificacao, "FaturaPixConta_ContaCliente_Status", "pago");
                        break;
                    case "pending":
                        VerificarValidadeFatura(idPedido, invoice);
                        break;
                }
            }
        }

        private void VerificarValidadeFatura(int idPedido, InvoiceInformation invoice)
        {
            //Se pedido já finalizado
            //Se valor pendente do pedido diferente do valor da fatura

            //Chama API cancelando a fatura
            //Exclui TAG FaturaPixConta_ContaCliente_ID
            //Exclui TAG FaturaPixConta_ContaCliente_Status
        }

        private void RegistrarPagamento(int idPedido, decimal valor)
        {
            PedidoPagamentoInformation pedidoPagamento = new PedidoPagamentoInformation();

            pedidoPagamento.TipoPagamento = new TipoPagamentoInformation();
            pedidoPagamento.TipoPagamento.IDTipoPagamento = 6;

            pedidoPagamento.Pedido = new PedidoInformation();
            pedidoPagamento.Pedido.IDPedido = idPedido;

            pedidoPagamento.UsuarioPagamento = new UsuarioInformation();
            pedidoPagamento.UsuarioPagamento.IDUsuario = 1;

            pedidoPagamento.Valor = valor;
            pedidoPagamento.DataPagamento = DateTime.Now;

            pedidoPagamento.Excluido = false;

            pedidoPagamento.MeioPagamentoSAT = new MeioPagamentoSATInformation();
            pedidoPagamento.MeioPagamentoSAT.IDMeioPagamentoSAT = 1;

            pedidoPagamento.IDGateway = 5;

            CRUD.Adicionar(pedidoPagamento);

            //Verificar se pedido está 100% pago e Finalizar Pedido
        }


        private DataRowCollection ListarPedidosFaturaPendente()
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            String querySql = @"
            SELECT
                p.IDPedido,
            	p.GUIDIdentificacao,
	            status.Valor as status,
	            (SELECT Valor FROM tbTag fatura WHERE fatura.GUIDIdentificacao = p.GUIDIdentificacao AND fatura.Chave = 'FaturaPixConta_ContaCliente_ID') as fatura
            FROM
                tbPedido p
                LEFT JOIN tbTag status ON status.GUIDIdentificacao = p.GUIDIdentificacao AND status.Chave = 'FaturaPixConta_ContaCliente_Status'
            WHERE

                status.Valor = 'pendente'
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);

            da.Fill(ds);
            dt = ds.Tables[0];

            return dt.Rows;
        }
    }
}
