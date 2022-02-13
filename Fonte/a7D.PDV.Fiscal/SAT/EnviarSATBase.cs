using a7D.PDV.BLL;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using System;

namespace a7D.PDV.Fiscal
{
    public abstract class EnviarSATBase
    {
        internal string ObterCodigoDeAtivacao()
        {
            var codigo = FiscalServices.ConfigSAT.InfCFe_codigoAtivacao;
            if (string.IsNullOrWhiteSpace(codigo))
                throw new Exception("Código de ativação não definido");
            return codigo;
        }

        internal static decimal CalcularImpostos(PedidoInformation pedido)
        {
            if (pedido == null || pedido?.ListaProduto?.Count < 1)
                return 0m;

            var IOF = 0m;
            var IPI = 0m;
            var PISPASEP = 0m;
            var COFINS = 0m;
            var CIDE = 0m;
            var ICMS = 0m;
            var ISS = 0m;

            if (pedido.ListaProduto == null)
                pedido = Pedido.CarregarCompleto(pedido.IDPedido.Value);

            foreach (var pedidoProduto in pedido.ListaProduto)
            {
                var valorItem = pedidoProduto.ValorTotal;

                if (pedidoProduto.Produto.ClassificacaoFiscal == null)
                    pedidoProduto.Produto = Produto.Carregar(pedidoProduto.Produto.IDProduto.Value);
                var classificacaoFiscal = ClassificacaoFiscal.Carregar(pedidoProduto.Produto.ClassificacaoFiscal.IDClassificacaoFiscal.Value);
                var tipoTributacao = TipoTributacao.Carregar(classificacaoFiscal.TipoTributacao.IDTipoTributacao.Value);

                IPI += valorItem * ValorImposto(classificacaoFiscal.IPI);

                IOF += valorItem * ValorImposto(classificacaoFiscal.IOF);

                PISPASEP += valorItem * ValorImposto(classificacaoFiscal.PISPASEP);

                COFINS += valorItem * ValorImposto(classificacaoFiscal.COFINS);

                ICMS += valorItem * ValorImposto(classificacaoFiscal.ICMS);

                ISS += valorItem * ValorImposto(classificacaoFiscal.ISS);

                CIDE += valorItem * ValorImposto(classificacaoFiscal.CIDE);
            }

            return IOF + IPI + PISPASEP + COFINS + CIDE + ICMS + ISS;
        }

        internal static decimal CalcularImpostos(int idProduto, decimal valor, decimal qtd)
        {
            var produto = Produto.Carregar(idProduto);
            if (produto.IDProduto.HasValue == false)
                return 0m;

            var classificacaoFiscal = ClassificacaoFiscal.Carregar(produto.ClassificacaoFiscal.IDClassificacaoFiscal.Value);
            if (classificacaoFiscal?.IDClassificacaoFiscal.HasValue == false)
                return 0m;

            var tipoTributacao = TipoTributacao.Carregar(classificacaoFiscal.TipoTributacao.IDTipoTributacao.Value);

            var valorItem = valor * qtd;

            var IOF = 0m;
            var IPI = 0m;
            var PISPASEP = 0m;
            var COFINS = 0m;
            var CIDE = 0m;
            var ICMS = 0m;
            var ISS = 0m;

            IPI += valorItem * ValorImposto(classificacaoFiscal.IPI);

            IOF += valorItem * ValorImposto(classificacaoFiscal.IOF);

            PISPASEP += valorItem * ValorImposto(classificacaoFiscal.PISPASEP);

            COFINS += valorItem * ValorImposto(classificacaoFiscal.COFINS);

            ICMS += valorItem * ValorImposto(classificacaoFiscal.ICMS);

            ISS += valorItem * ValorImposto(classificacaoFiscal.ISS);

            CIDE += valorItem * ValorImposto(classificacaoFiscal.CIDE);

            return IOF + IPI + PISPASEP + COFINS + CIDE + ICMS + ISS;
        }

        private static Func<decimal?, decimal> ValorImposto = (imposto) =>
        {
            if (imposto.HasValue)
                return imposto.Value / 100;
            return 0m;
        };
    }
}
