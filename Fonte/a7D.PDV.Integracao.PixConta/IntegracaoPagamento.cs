using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.DAL;
using a7D.PDV.EF.Enum;
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
                string codigoFatura = ((DataRow)pedido)["CodigoFatura"].ToString();
                decimal valorFatura = Convert.ToDecimal(((DataRow)pedido)["Valor"]);

                Model.InvoiceInformation invoice = APIInvoice.ConsultarStatus(codigoFatura);

                switch (invoice.status)
                {
                    case "paid":
                    case "paid_external":
                        RegistrarPagamento(idPedido, codigoFatura, valorFatura);

                        if (Pedido.TotalPago(idPedido))
                            FecharPedido(idPedido);

                        break;
                    case "pending":
                        VerificarValidadeFatura(idPedido, codigoFatura, valorFatura);
                        break;
                }
            }
        }

        private void CancelarFaturas()
        {
            var pedidos = ListarPedidosFaturaCancelamento();

            foreach (var pedido in pedidos)
            {
                string codigoFatura = ((DataRow)pedido)["CodigoFatura"].ToString();

                APIInvoice.CancelarFatura(codigoFatura);

                FaturaPixContaInformation faturaPixConta = new FaturaPixContaInformation();
                faturaPixConta.CodigoFatura = codigoFatura;

                faturaPixConta = (FaturaPixContaInformation)CRUD.Carregar(faturaPixConta);
                faturaPixConta.Status = "cancelada";
                faturaPixConta.DtUltimaAlteracao = DateTime.Now;

                CRUD.Alterar(faturaPixConta);
            }
        }

        private void VerificarValidadeFatura(int idPedido, string codigoFatura, decimal valorFatura)
        {
            //Se valor pendente do pedido diferente do valor da fatura
            if(Pedido.ValorPendente(idPedido) != valorFatura)
            {
                FaturaPixContaInformation faturaPixConta = new FaturaPixContaInformation();
                faturaPixConta.CodigoFatura = codigoFatura;

                faturaPixConta = (FaturaPixContaInformation)CRUD.Carregar(faturaPixConta);
                faturaPixConta.Status = "cancelar";
                faturaPixConta.DtUltimaAlteracao = DateTime.Now;

                CRUD.Alterar(faturaPixConta);
            }
        }

        private void RegistrarPagamento(int idPedido, string codigoFatura, decimal valor)
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


            FaturaPixContaInformation faturaPixConta = new FaturaPixContaInformation();
            faturaPixConta.CodigoFatura = codigoFatura;

            faturaPixConta = (FaturaPixContaInformation)CRUD.Carregar(faturaPixConta);
            faturaPixConta.Status = "pago";
            faturaPixConta.DtUltimaAlteracao = DateTime.Now;

            CRUD.Alterar(faturaPixConta);
        }

        private void FecharPedido(int idPedido)
        {
            PedidoInformation pedido = Pedido.CarregarCompleto(idPedido);
            CaixaInformation caixa = new CaixaInformation();
            caixa.IDCaixa = 1;

            Int32 idUsuario = 1;

            Pedido.FecharVendaDB(pedido, caixa, idUsuario);
            switch (pedido.TipoPedido.TipoPedido)
            {
                case ETipoPedido.Mesa:
                    var mesa = Mesa.CarregarPorGUID(pedido.GUIDIdentificacao);
                    Mesa.AlterarStatus(pedido.GUIDIdentificacao, EStatusMesa.Liberada);
                    break;
                case ETipoPedido.Comanda:
                    var comanda = Comanda.CarregarPorGUID(pedido.GUIDIdentificacao);
                    Comanda.AlterarStatus(pedido.GUIDIdentificacao, EStatusComanda.Liberada);
                    break;
            }
        }

        private DataRowCollection ListarPedidosFaturaPendente()
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();

            String querySql = "SELECT * FROM tbFaturaPixConta WHERE Status='pendente'";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.Fill(ds);

            return ds.Tables[0].Rows;
        }

        private DataRowCollection ListarPedidosFaturaCancelamento()
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();

            String querySql = "SELECT * FROM tbFaturaPixConta WHERE Status='cancelar'";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);
            da.Fill(ds);

            return ds.Tables[0].Rows;
        }
    }
}
