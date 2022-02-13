using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public class RelatorioFechamento
    {
        private const string _relatorio = "RelatorioFechamento-";

        public static readonly string ResumoCaixa = _relatorio + "ResumoCaixa";
        public static readonly string ResumoTipoPagamento = _relatorio + "ResumoTipoPagamento";
        public static readonly string ResumoCreditoPagamento = _relatorio + "ResumoCreditoPagamento";
        public static readonly string ProdutosVendidos = _relatorio + "ProdutosVendidos";
        public static readonly string ProdutosCancelados = _relatorio + "ProdutosCancelados";
        public static readonly string ProdutosCanceladosAberto = _relatorio + "ProdutosCanceladosAberto";
        public static readonly string ProdutosAbertos = _relatorio + "ProdutosAbertos";
        public static readonly string PedidosDescontoResumo = _relatorio + "PedidosDescontoResumo";
        public static readonly string PedidosDescontoDetalhe = _relatorio + "PedidosDescontoDetalhe";

        private static Func<string, bool> Ligado = (chave) => { return ConfiguracaoBD.BuscarConfiguracao(chave)?.Valor == "1"; };
        private static Action<string, string> Alterar = (chave, valor) => { ConfiguracaoBD.AlterarConfiguracaoSistema(chave, valor); };

        public static bool ResumoCaixaAtivado { get { return Ligado(ResumoCaixa); } set { Alterar(ResumoCaixa, value ? "1" : "0"); } }
        public static bool ResumoTipoPagamentoAtivado { get { return Ligado(ResumoTipoPagamento); } set { Alterar(ResumoTipoPagamento, value ? "1" : "0"); } }
        public static bool ResumoCreditoPagamentoAtivado { get { return Ligado(ResumoCreditoPagamento); } set { Alterar(ResumoCreditoPagamento, value ? "1" : "0"); } }
        public static bool ProdutosVendidosAtivado { get { return Ligado(ProdutosVendidos); } set { Alterar(ProdutosVendidos, value ? "1" : "0"); } }
        public static bool ProdutosCanceladosAtivado { get { return Ligado(ProdutosCancelados); } set { Alterar(ProdutosCancelados, value ? "1" : "0"); } }
        public static bool ProdutosCanceladosAbertoAtivado { get { return Ligado(ProdutosCanceladosAberto); } set { Alterar(ProdutosCanceladosAberto, value ? "1" : "0"); } }
        public static bool ProdutosAbertosAtivado { get { return Ligado(ProdutosAbertos); } set { Alterar(ProdutosAbertos, value ? "1" : "0"); } }
        public static bool PedidosDescontoResumoAtivado { get { return Ligado(PedidosDescontoResumo); } set { Alterar(PedidosDescontoResumo, value ? "1" : "0"); } }
        public static bool PedidosDescontoDetalheLigado { get { return Ligado(PedidosDescontoDetalhe); } set { Alterar(PedidosDescontoDetalhe, value ? "1" : "0"); } }
    }
}
