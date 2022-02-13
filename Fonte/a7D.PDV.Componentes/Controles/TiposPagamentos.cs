using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Componentes.Controles
{
    public delegate void PagamentoSelecionadoEventHandler(PedidoPagamentoInformation pagamento);

    public partial class TiposPagamentos : UserControl
    {
        public int botoesPorLinhaAtual = 0;
        public event PagamentoSelecionadoEventHandler PagamentoSelecionadoValor;
        private List<BotaoItem> itens;
        public event ObterValorEventHandler ObterValorPadrao;

        public TiposPagamentos()
        {
            InitializeComponent();
        }

        public void Fill()
        {
            var tipoPagamentos = EF.Repositorio.Listar<tbTipoPagamento>(t => t.Ativo == true);
            itens = tipoPagamentos.Select(i => new BotaoItem(i.IDTipoPagamento, i.Nome, true, i)).ToList();
            TiposPagamentos_Resize(null, null);
        }

        private void TiposPagamentos_Resize(object sender, EventArgs e)
        {
            if (DesignMode || itens == null)
                return;
            
            BotaoGrid.CriaBotoes(this, itens, btn_Select, ref botoesPorLinhaAtual, BotaoGrid.Azul, true);
        }

        private void btn_Select(object sender, EventArgs e)
        {
            var valor = ObterValorPadrao?.Invoke() ?? 0m;
            if (valor < 0) // Não permite selecionar valor negativo
                valor = 0;

            var btn = (Button)sender;
            var tipoPag = (tbTipoPagamento)btn.Tag;
            var pag = new PedidoPagamentoInformation()
            {
                TipoPagamento = BLL.TipoPagamento.Carregar(tipoPag.IDTipoPagamento),
                UsuarioPagamento = BLL.AC.Usuario,
                Excluido = false,
                DataPagamento = DateTime.Now,
            };

            pag.IDGateway = pag.TipoPagamento.IDGateway;

            if (pag.TipoPagamento.Gateway== EF.Enum.EGateway.ContaCliente)
            {
                pag.Valor = valor;
            }
            else
            {
                var frm = new frmTecladoValor(valor);
                frm.Text = "Valor para Pagamento";
                if (frm.ShowDialog() != DialogResult.OK || frm.Valor == 0)
                    return;

                pag.Valor = frm.Valor;
            }

            if (tipoPag.IDMeioPagamentoSAT > 0)
                pag.MeioPagamentoSAT = new MeioPagamentoSATInformation() { IDMeioPagamentoSAT = tipoPag.IDMeioPagamentoSAT };

            if (tipoPag.IDContaRecebivel > 0)
                pag.ContaRecebivel = new tbContaRecebivel() { IDContaRecebivel = tipoPag.IDContaRecebivel.Value };

            if (tipoPag.IDBandeira > 0)
                pag.Bandeira = new tbBandeira() { IDBandeira = tipoPag.IDBandeira.Value };

            PagamentoSelecionadoValor?.Invoke(pag);
        }
    }
}
