using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbPedido")]
    [Serializable]
    public class PedidoInformation
    {

        public PedidoInformation()
        {
            EnviarNfEmailCliente = false;
        }

        [CRUDParameterDAL(true, "IDPedido")]
        public Int32? IDPedido { get; set; }

        [CRUDParameterDAL(false, "IDCliente", "IDCliente")]
        public ClienteInformation Cliente { get; set; }

        [CRUDParameterDAL(false, "IDTipoPedido", "IDTipoPedido")]
        public TipoPedidoInformation TipoPedido { get; set; }

        [CRUDParameterDAL(false, "IDStatusPedido", "IDStatusPedido")]
        public StatusPedidoInformation StatusPedido { get; set; }

        [CRUDParameterDAL(false, "IDCaixa", "IDCaixa")]
        public CaixaInformation Caixa { get; set; }

        [CRUDParameterDAL(false, "IDTipoEntrada", "IDTipoEntrada")]
        public TipoEntradaInformation TipoEntrada { get; set; }

        [CRUDParameterDAL(false, "IDRetornoSAT_venda", "IDRetornoSAT")]
        public RetornoSATInformation RetornoSAT_venda { get; set; }

        [CRUDParameterDAL(false, "IDRetornoSAT_cancelamento", "IDRetornoSAT")]
        public RetornoSATInformation RetornoSAT_cancelamento { get; set; }

        // Origem do Pedido (Mensa, Comando ou IFOOD numero unico)
        [CRUDParameterDAL(false, "GUIDIdentificacao")]
        public String GUIDIdentificacao { get; set; }

        [CRUDParameterDAL(false, "NumeroCupom")]
        public String NumeroCupom { get; set; }

        [CRUDParameterDAL(false, "DocumentoCliente")]
        public String DocumentoCliente { get; set; }

        [CRUDParameterDAL(false, "NomeCliente")]
        public string NomeCliente { get; set; }

        [CRUDParameterDAL(false, "EmailCliente")]
        public string EmailCliente { get; set; }

        [CRUDParameterDAL(false, "DtPedido")]
        public DateTime? DtPedido { get; set; }

        [CRUDParameterDAL(false, "DtPedidoFechamento")]
        public DateTime? DtPedidoFechamento { get; set; }

        [CRUDParameterDAL(false, "SincERP")]
        public Boolean? SincERP { get; set; }

        [CRUDParameterDAL(false, "ValorServico")]
        public Decimal? ValorServico { get; set; }

        [CRUDParameterDAL(false, "ValorDesconto")]
        public Decimal? ValorDesconto { get; set; }

        [CRUDParameterDAL(false, "TaxaServicoPadrao")]
        public Decimal? TaxaServicoPadrao { get; set; }

        [CRUDParameterDAL(false, "ValorTotal")]
        public Decimal? ValorTotal { get; set; }

        // Campo auxiliar de controle, no IFOOD é o numero curto do pedido
        [CRUDParameterDAL(false, "GUIDAgrupamentoPedido")]
        public String GUIDAgrupamentoPedido { get; set; }

        [CRUDParameterDAL(false, "Observacoes")]
        public String Observacoes { get; set; }

        [CRUDParameterDAL(false, "ObservacaoCupom")]
        public String ObservacaoCupom { get; set; }

        [CRUDParameterDAL(false, "ValorConsumacaoMinima")]
        public Decimal? ValorConsumacaoMinima { get; set; }

        [CRUDParameterDAL(false, "ReferenciaLocalizacao")]
        public String ReferenciaLocalizacao { get; set; }

        [CRUDParameterDAL(false, "IDTipoDesconto", "IDTipoDesconto")]
        public TipoDescontoInformation TipoDesconto { get; set; }

        [CRUDParameterDAL(false, "GUIDMovimentacao")]
        public string GUIDMovimentacao { get; set; }

        [CRUDParameterDAL(false, "NumeroPessoas")]
        public int? NumeroPessoas { get; set; }

        [CRUDParameterDAL(false, "ValorEntrega")]
        public decimal? ValorEntrega { get; set; }

        [CRUDParameterDAL(false, "IDTaxaEntrega", "IDTaxaEntrega")]
        public TaxaEntregaInformation TaxaEntrega { get; set; }

        [CRUDParameterDAL(false, "DtEnvio")]
        public DateTime? DtEnvio { get; set; }

        [CRUDParameterDAL(false, "DtEntrega")]
        public DateTime? DtEntrega { get; set; }

        [CRUDParameterDAL(false, "IDEntregador", "IDEntregador")]
        public EntregadorInformation Entregador { get; set; }

        [CRUDParameterDAL(false, "AplicarDesconto")]
        public bool? AplicarDesconto { get; set; }

        [CRUDParameterDAL(false, "AplicarServico")]
        public bool? AplicarServico { get; set; }

        [CRUDParameterDAL(false, "IDUsuarioDesconto", "IDUsuario")]
        public UsuarioInformation UsuarioDesconto { get; set; }

        [CRUDParameterDAL(false, "IDUsuarioTaxaServico", "IDUsuario")]
        public UsuarioInformation UsuarioTaxaServico { get; set; }

        [CRUDParameterDAL(false, "TAG")]
        public String TAG { get; set; }

        [CRUDParameterDAL(false, "IDOrigemPedido", "IDOrigemPedido")]
        public OrigemPedidoInformation OrigemPedido { get; set; }

        [CRUDParameterDAL(false, "PermitirAlterar")]
        public bool? PermitirAlterar { get; set; }



        public string NumeroComanda { get; set; }
        public string NumeroMesa { get; set; }
        public bool EnviarNfEmailCliente { get; set; }
        public Decimal? ValorContaCliente { get; set; }
        public Decimal? ValorSaldoCliente { get; set; }

        public bool PodeEnviarEmailNfCliente
        {
            get
            {
                if (this.EnviarNfEmailCliente
                 && this.EmailCliente != null
                 && this.Cliente != null
                 && this.Cliente.IDCliente != null
                 && this.Cliente.IDCliente != 0
                 && !string.IsNullOrEmpty(this.Cliente.Email))
                        return true;
                    else
                        return false;
            }
        }

        public List<PedidoProdutoInformation> ListaProduto { get; set; }
        public List<PedidoPagamentoInformation> ListaPagamento { get; set; }

        public Int32 QtdItens
        {
            get
            {
                Int32 qtd = 0;

                if (ListaProduto != null && ListaProduto.Where(l => l.Cancelado == false).Count() > 0)
                    qtd = Convert.ToInt32(ListaProduto.Where(l => l.Cancelado == false).Sum(l => l.Quantidade).Value);

                return qtd;
            }
        }
        public Decimal ValorTotalProdutos
        {
            get
            {
                Decimal valorTotal = 0;
                if (ListaProduto != null)
                {
                    foreach (var item in ListaProduto.Where(l => l.Cancelado != true && l.Produto.IDProduto != ProdutoInformation.IDProdutoServico))
                    {
                        valorTotal += item.ValorTotal;

                        if (item.ListaModificacao != null)
                        {
                            foreach (var modificacao in item.ListaModificacao.Where(l => l.Cancelado != true))
                                valorTotal += Math.Truncate(item.Quantidade.Value * modificacao.ValorTotal * 100m) / 100m;
                        }
                    }
                }

                return valorTotal;
            }
        }

        //public decimal ValorTotalProdutosParaAcrescimo
        //{
        //    get
        //    {
        //        decimal valorTotal = 0;
        //        if (ListaProduto != null)
        //        {
        //            var excluidos = new List<int>() { 2, 3, 4 };
        //            foreach (var item in ListaProduto.Where(l => l.Cancelado != true && !excluidos.Contains(l.Produto.IDProduto.Value)))
        //            {
        //                valorTotal += item.ValorUnitario.Value * item.Quantidade.Value;
        //                if (item.ListaModificacao != null)
        //                {
        //                    foreach (var modificacao in item.ListaModificacao.Where(l => l.Cancelado != true))
        //                        valorTotal += modificacao.ValorUnitario.Value * modificacao.Quantidade.Value;
        //                }
        //            }
        //        }
        //        return valorTotal;
        //    }
        //}

        public Decimal ValorTotalProdutosServicos
        {
            get
            {
                Decimal servico = 0;
                PedidoProdutoInformation pedidoProduto = ListaProduto.FirstOrDefault(l => l.Produto.IDProduto == ProdutoInformation.IDProdutoServico);

                if (pedidoProduto != null)
                    servico = pedidoProduto.ValorUnitario.Value;

                return ValorTotalProdutos + servico;
            }
        }

        public Decimal ValorTotalTemp
        {
            get
            {
                if (ValorConsumacaoMinima > ValorTotalProdutos + ValorServicoTemp)
                    return ValorConsumacaoMinima.Value;
                else
                    return ValorTotalProdutos + ValorServicoTemp + (ValorEntrega.HasValue ? ValorEntrega.Value : 0)
                        - (ValorDesconto.HasValue ? ValorDesconto.Value : 0);
            }
        }
        public Decimal ValorServicoTemp
        {
            get
            {
                Decimal valorServico;

                if (AplicarServico == false)
                    return 0;

                if (TaxaServicoPadrao == null)
                    TaxaServicoPadrao = 0;

                var listaProduto = ListaProduto.Where(l => l.Cancelado != true
                    && l.Produto.IDProduto != ProdutoInformation.IDProdutoEntrada   // 2
                    && l.Produto.IDProduto != ProdutoInformation.IDProdutoEntracaCM // 3
                    && l.Produto.IDProduto != ProdutoInformation.IDProdutoServico); // 4

                Decimal valorTotal1 = PedidoProdutoInformation.SomaValorTotal(listaProduto);

                valorServico = valorTotal1 * TaxaServicoPadrao.Value / 100m;
                return Math.Truncate(valorServico * 100m) / 100m;
            }
        }
        public Decimal DiferencaConsumacaoMinima
        {
            get
            {
                Decimal valorTotal1 = 0;
                var produtos = new List<PedidoProdutoInformation>();

                if (ListaProduto != null)
                {
                    produtos.AddRange(ListaProduto);
                    produtos.AddRange(ListaProduto.Where(p => p.ListaModificacao != null).SelectMany(p => p.ListaModificacao));
                }


                // Consumação Mínima não deve incluir taxa de serviço!
                if (produtos?.Count > 0 
                 && produtos.Where(l => l.Cancelado != true
                             && l.Produto.IDProduto != ProdutoInformation.IDProdutoServico
                             && l.Produto.IDProduto != ProdutoInformation.IDProdutoEntracaCM)
                    .Count() > 0)
                {
                    valorTotal1 = produtos
                        .Where(l => l.Cancelado != true
                                 && l.Produto.IDProduto != ProdutoInformation.IDProdutoServico
                                 && l.Produto.IDProduto != ProdutoInformation.IDProdutoEntracaCM)
                        .Sum(l => l.ValorTotal);
                }

                if (valorTotal1 < ValorConsumacaoMinima)
                    return ValorConsumacaoMinima.Value - valorTotal1;
                else
                    return 0;
            }
        }

        public static PedidoInformation ConverterObjeto(Object obj)
        {
            return (PedidoInformation)obj;
        }

        public override string ToString()
        {
            return $"IDPedido: {IDPedido} Tipo: {TipoPedido?.ToString()} Status: {StatusPedido?.ToString()} Cliente {Cliente} GUIDIdentificacao: {GUIDIdentificacao}";
        }
    }
}
