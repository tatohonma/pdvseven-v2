using System;
using System.Collections.Generic;
using a7D.PDV.Model;
using a7D.Fmk.CRUD.DAL;
using System.Linq;
using System.Text;

namespace a7D.PDV.BLL
{
    public class PedidoPagamento
    {
        public static PedidoPagamentoInformation Carregar(Int32 idPedidoPagamento)
        {
            PedidoPagamentoInformation obj = new PedidoPagamentoInformation { IDPedidoPagamento = idPedidoPagamento };
            obj = (PedidoPagamentoInformation)CRUD.Carregar(obj);

            if (obj?.IDPedidoPagamento != null)
                obj.Status = StatusModel.Inalterado;

            return obj;
        }

        public static void Salvar(PedidoPagamentoInformation obj, int idUsuario)
        {
            obj.UsuarioPagamento = new UsuarioInformation() { IDUsuario = idUsuario };
            obj.DataPagamento = DateTime.Now;
            if (obj.IDPedidoPagamento == null)
            {
                PedidoPagamento.Adicionar(obj);
            }
            else
            {
                PedidoPagamento.Alterar(obj);
            }
        }
        public static void Adicionar(PedidoPagamentoInformation obj)
        {
            CRUD.Adicionar(obj);
        }
        public static void Alterar(PedidoPagamentoInformation obj)
        {
            CRUD.Alterar(obj);
        }

        public static List<PedidoPagamentoInformation> ListarNaoCanceladoPorPedido(Int32 idPedido)
        {
            PedidoPagamentoInformation objFiltro = new PedidoPagamentoInformation();
            objFiltro.Pedido = new PedidoInformation { IDPedido = idPedido };
            objFiltro.Excluido = false;

            List<Object> listaObj = CRUD.Listar(objFiltro);
            List<PedidoPagamentoInformation> lista = listaObj.ConvertAll(new Converter<Object, PedidoPagamentoInformation>(PedidoPagamentoInformation.ConverterObjeto));

            foreach (var item in lista)
            {
                item.Status = StatusModel.Inalterado;
                item.TipoPagamento = TipoPagamento.Carregar(item.TipoPagamento.IDTipoPagamento.Value);

                if (item.MeioPagamentoSAT != null)
                    item.MeioPagamentoSAT = MeioPagamentoSAT.Carregar(item.MeioPagamentoSAT.IDMeioPagamentoSAT.Value);

                if (item.ContaRecebivel != null)
                    item.ContaRecebivel = ContaRecebivel.Carregar(item.ContaRecebivel.IDContaRecebivel);

                if (item.Bandeira != null)
                    item.Bandeira = Bandeira.Carregar(item.Bandeira.IDBandeira);
            }

            return lista;
        }

        //public static void ExcluirPorPedido(Int32 idPedido)
        //{
        //    PedidoPagamentoInformation objFiltro = new PedidoPagamentoInformation();
        //    objFiltro.Pedido = new PedidoInformation { IDPedido = idPedido };
        //    CRUD.Excluir(objFiltro);
        //}

        public static void CancelarPorPedido(Int32 idPedido, int idUsuario)
        {
            var objFiltro = new PedidoPagamentoInformation
            {
                Pedido = new PedidoInformation { IDPedido = idPedido }
            };
            var listaObj = CRUD.Listar(objFiltro);
            var lista = listaObj.ConvertAll(new Converter<Object, PedidoPagamentoInformation>(PedidoPagamentoInformation.ConverterObjeto));

            foreach (var item in lista)
            {
                item.Excluido = true;
                item.UsuarioCancelado = new UsuarioInformation() { IDUsuario = idUsuario };
                item.DataCancelado = DateTime.Now;
                CRUD.Salvar(item);
            }
        }

        public static void Cancelar(PedidoPagamentoInformation pagamento, int idUsuario)
        {
            pagamento.Excluido = true;
            pagamento.UsuarioCancelado = new UsuarioInformation() { IDUsuario = idUsuario };
            pagamento.DataCancelado = DateTime.Now;
            CRUD.Salvar(pagamento);
        }

        public class SatPagamento
        {
            public string Codigo;
            public decimal Valor;
        }

        public static List<SatPagamento> ListaSAT(PedidoInformation pedido)
        {
            var lista = new List<SatPagamento>();
            foreach (var item in pedido.ListaPagamento)
            {
                var mpItem = new SatPagamento
                {
                    Valor = item.Valor.Value
                };

                if (item.MeioPagamentoSAT != null)
                {
                    if (item.MeioPagamentoSAT.Codigo == null)
                        item.MeioPagamentoSAT = MeioPagamentoSAT.Carregar(item.MeioPagamentoSAT.IDMeioPagamentoSAT.Value);

                    if (item.MeioPagamentoSAT.Codigo == null)
                        throw new ExceptionPDV(CodigoErro.E502, item.IDPedidoPagamento?.ToString());

                    mpItem.Codigo = item.MeioPagamentoSAT.Codigo;
                }
                else if (item.TipoPagamento.MeioPagamentoSAT?.IDMeioPagamentoSAT == null)
                {
                    throw new ExceptionPDV(CodigoErro.E502, item.TipoPagamento.Nome);
                }
                else
                    mpItem.Codigo = item.TipoPagamento.MeioPagamentoSAT.Codigo;

                lista.Add(mpItem);
            }

            if (pedido.ListaPagamento.Count < 10)
                return lista;

            var listaAgrupada = new List<SatPagamento>();
            foreach (var item in lista)
            {
                var itemGrupo = listaAgrupada.FirstOrDefault(i => i.Codigo == item.Codigo);
                if (itemGrupo == null)
                    listaAgrupada.Add(item);
                else
                    itemGrupo.Valor += item.Valor;
            }

            if (listaAgrupada.Count > 10)
                throw new ExceptionPDV(CodigoErro.E509, "Pagamentos agrupados: " + listaAgrupada.Count);

            return listaAgrupada;
        }

        public static string TransferirPagamento(PedidoPagamentoInformation pagamento, PedidoInformation pedidoDestino, int idUsuario)
        {
            var pedidoPagamento = Carregar(pagamento.IDPedidoPagamento.Value);

            var log = $"IDPagamento: {pedidoPagamento.IDPedidoPagamento}";

            if (pedidoPagamento.Excluido == true)
                log += " EXCLUIDO";
            else
            {
                // Cria novo item principal
                var pedidoPagamentoDestino = new PedidoPagamentoInformation
                {
                    Pedido = pedidoDestino,
                    TipoPagamento = pedidoPagamento.TipoPagamento,
                    Valor = pedidoPagamento.Valor,
                    Autorizacao = pedidoPagamento.Autorizacao,
                    IDGateway = pedidoPagamento.IDGateway,
                    MeioPagamentoSAT = pedidoPagamento.MeioPagamentoSAT,
                    ContaRecebivel = pedidoPagamento.ContaRecebivel,
                    Bandeira = pedidoPagamento.Bandeira,
                    DataPagamento = pedidoPagamento.DataPagamento,
                    UsuarioPagamento = pedidoPagamento.UsuarioPagamento,
                    Excluido = false
                };

                PedidoPagamento.Salvar(pedidoPagamentoDestino, idUsuario);
                log += $" => {pedidoPagamentoDestino.IDPedidoPagamento} R$ {pedidoPagamentoDestino.Valor.Value.ToString("N2")}";

                Cancelar(pedidoPagamento, idUsuario);
            }

            return log.ToString();
        }
    }
}
