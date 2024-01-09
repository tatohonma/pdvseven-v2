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

                AddLog("Consultar fatura " + codigoFatura);
                Model.InvoiceInformation invoice = APIInvoice.ConsultarStatus(codigoFatura);

                switch (invoice.status)
                {
                    case "paid":
                    case "paid_external":
                        AddLog("Registrar pagamento pedido " + idPedido + " fatura " + codigoFatura);
                        RegistrarPagamento(idPedido, codigoFatura, valorFatura);

                        if (Pedido.TotalPago(idPedido))
                        {
                            AddLog("Fechar pedido " + idPedido);
                            FecharPedido(idPedido);
                        }
                        break;
                    case "pending":
                        AddLog("Verificar validade fatura pedido " + idPedido + " fatura " + codigoFatura);
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
                AddLog("Cancelar fatura pedido " + idPedido + " fatura " + codigoFatura);

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

            pedidoPagamento.Pedido = new PedidoInformation();
            pedidoPagamento.Pedido.IDPedido = idPedido;

            pedidoPagamento.TipoPagamento = TipoPagamentoPixConta;
            pedidoPagamento.UsuarioPagamento = UsuarioPixConta;
            pedidoPagamento.MeioPagamentoSAT = TipoPagamentoPixConta.MeioPagamentoSAT;

            pedidoPagamento.Valor = valor;
            pedidoPagamento.DataPagamento = DateTime.Now;
            pedidoPagamento.Excluido = false;

            pedidoPagamento.IDGateway = (int)EGateway.PixConta;

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
            Int32 idPDV = PDVPixConta.IDPDV.Value;
            Int32 idUsuario = UsuarioPixConta.IDUsuario.Value;

            PedidoInformation pedido = Pedido.CarregarCompleto(idPedido);
            CaixaInformation caixa = Caixa.UsaOuAbre(idPDV, idUsuario);

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
