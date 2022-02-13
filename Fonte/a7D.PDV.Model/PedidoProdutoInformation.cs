using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbPedidoProduto")]
    [Serializable]
    public class PedidoProdutoInformation : ICloneable
    {
        public PedidoProdutoInformation()
        {
            Status = StatusModel.Novo;
        }

        [CRUDParameterDAL(true, "IDPedidoProduto")]
        public Int32? IDPedidoProduto { get; set; }
        public Int32? IDPedidoProduto_Original;

        [CRUDParameterDAL(false, "IDPedido", "IDPedido")]
        public PedidoInformation Pedido { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        [CRUDParameterDAL(false, "IDPedidoProduto_pai", "IDPedidoProduto")]
        public PedidoProdutoInformation PedidoProdutoPai { get; set; }

        [CRUDParameterDAL(false, "IDPDV", "IDPDV")]
        public PDVInformation PDV { get; set; }

        [CRUDParameterDAL(false, "IDUsuario", "IDUsuario")]
        public UsuarioInformation Usuario { get; set; }

        [CRUDParameterDAL(false, "Quantidade")]
        public Decimal? Quantidade { get; set; }

        [CRUDParameterDAL(false, "ValorUnitario")]
        public Decimal? ValorUnitario { get; set; }

        [CRUDParameterDAL(false, "CodigoAliquota")]
        public String CodigoAliquota { get; set; }

        [CRUDParameterDAL(false, "Viagem")]
        public bool? Viagem { get; set; }

        [CRUDParameterDAL(false, "IDTipoEntrada")]
        public int? IDTipoEntrada { get; set; }

        [CRUDParameterDAL(false, "Notas")]
        public String Notas { get; set; }

        [CRUDParameterDAL(false, "Cancelado")]
        public Boolean? Cancelado { get; set; }

        [CRUDParameterDAL(false, "DtInclusao")]
        public DateTime? DtInclusao { get; set; }

        [CRUDParameterDAL(false, "DtAlteracao")]
        public DateTime? DtAlteracao { get; set; }

        [CRUDParameterDAL(false, "IDPDV_cancelamento", "IDPDV")]
        public PDVInformation PDVCancelamento { get; set; }

        [CRUDParameterDAL(false, "IDUsuario_cancelamento", "IDUsuario")]
        public UsuarioInformation UsuarioCancelamento { get; set; }

        [CRUDParameterDAL(false, "IDMotivoCancelamento", "IDMotivoCancelamento")]
        public MotivoCancelamentoInformation MotivoCancelamento { get; set; }

        [CRUDParameterDAL(false, "ObservacoesCancelamento")]
        public String ObservacoesCancelamento { get; set; }

        [CRUDParameterDAL(false, "DtCancelamento")]
        public DateTime? DtCancelamento { get; set; }

        [CRUDParameterDAL(false, "GUIDControleDuplicidade")]
        public String GUIDControleDuplicidade { get; set; }

        [CRUDParameterDAL(false, "RetornarAoEstoque")]
        public Boolean? RetornarAoEstoque { get; set; }

        public List<PedidoProdutoInformation> ListaModificacao;

        [CRUDParameterDAL(false, "IDPainelModificacao", "IDPainelModificacao")]
        public PainelModificacaoInformation PainelModificacao { get; set; }

        [CRUDParameterDAL(false, "ValorDesconto")]
        public decimal? ValorDesconto { get; set; }

        [CRUDParameterDAL(false, "IDUsuarioDesconto", "IDUsuario")]
        public UsuarioInformation UsuarioDesconto { get; set; }

        [CRUDParameterDAL(false, "IDTipoDesconto", "IDTipoDesconto")]
        public TipoDescontoInformation TipoDesconto { get; set; }

        public int NivelModificacao { get; set; }

        public int idAreaProducao { get; set; }
        public int sequenciaPedido { get; set; }

        public StatusModel Status { get; set; }

        public static PedidoProdutoInformation ConverterObjeto(Object obj)
        {
            return (PedidoProdutoInformation)obj;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"IDPedidoProduto: {IDPedidoProduto} => {Quantidade} x {IDPedidoProduto_Original} / Pedido: {Pedido?.IDPedido} / PedidoProdutoPai: {PedidoProdutoPai?.IDPedidoProduto} {(PainelModificacao == null ? "" : ("/ PainelModificacao: " + PainelModificacao))} / Produto: {Produto} {(Viagem == true ? "(Viagem)" : "")} => AREA: {idAreaProducao}";
        }

        public decimal ValorTotal => Math.Truncate(Quantidade.Value * ValorUnitario.Value * 100m) / 100m;

        private static IFormatProvider _providerPTBR = new CultureInfo("pt-BR");

        public string ValorTotalString => ValorTotal.ToString("N2", _providerPTBR);

        public string ValorUnitarioString => ValorUnitario.Value.ToString("N2", _providerPTBR);

        public static decimal SomaValorTotal(IEnumerable<PedidoProdutoInformation> lista)
        {
            var valor = 0m;
            foreach (var item in lista.Where(pp => pp.Cancelado == false && pp.Status != StatusModel.Excluido))
            {
                valor += item.ValorTotal;
                if (item.ListaModificacao?.Count > 0)
                    valor += Math.Truncate(item.Quantidade.Value * SomaValorTotal(item.ListaModificacao) * 100m) / 100m;
            }
            return valor;
        }

        public override int GetHashCode()
        {
            var i = (IDPedidoProduto ?? IDPedidoProduto_Original ?? 1) * 499;
            i ^= (Produto.IDProduto ?? 29) * 997;
            i ^= (int)(Quantidade ?? 1) * 31;
            if (Produto?.TipoProduto?.IDTipoProduto == (int)ETipoProduto.Item)
            {
                int valor = (int)(ValorUnitario ?? 1);
                if (valor >= 1)
                    return i ^ valor;
            }
            return i;
        }
    }
}
