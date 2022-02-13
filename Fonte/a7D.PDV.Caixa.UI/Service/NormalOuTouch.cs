using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public static class NormalOuTouch
    {
        public static void AdicionaProdutos(ETipoPedido idTipoPedido_selecionado, String guidIdentificacao_selecionado)
        {
            if (ConfiguracoesCaixa.Valores.CaixaTouch)
            {
                using (var frm = new frmPedidoProdutosTouch(idTipoPedido_selecionado, guidIdentificacao_selecionado, AC.Usuario.IDUsuario.Value, AC.PDV.IDPDV.Value))
                {
                    frm.UsarCodigoBarras = ConfiguracoesCaixa.Valores.CodigoBarras;
                    frm.ShowDialog();
                }
            }
            else
            {
                using (var form = new frmPedidoProdutos(idTipoPedido_selecionado, guidIdentificacao_selecionado))
                    form.ShowDialog();
            }
        }

        public static DialogResult FechaPagamento(int idPedido, bool parcial = false)
        {
            Form frm;
            if (ConfiguracoesCaixa.Valores.CaixaTouch)
                frm = new frmPedidoPagamentoTouch(idPedido);
            else
                frm = new frmPedidoPagamento(idPedido);

            if (parcial)
                ((IPedidoPagamentoForm)frm).HabilitarParcial();

            return frm.ShowDialog();
        }

        public static DialogResult Autenticacao(Boolean alterarUsuarioAutenticado, Boolean gerente, Boolean caixa, Boolean garcom, out UsuarioInformation usuario, Boolean admin = false)
        {
            if (ConfiguracoesCaixa.Valores.CaixaTouch)
            {
                var frm = new frmAutenticacaoTouch(alterarUsuarioAutenticado, admin, gerente, caixa, garcom);
                if (frm.ShowDialog() == DialogResult.OK)
                    usuario = frm.Usuario1;
                else
                    usuario = null;

                return frm.DialogResult;
            }
            else
            {
                var frm = new frmAutenticacao(alterarUsuarioAutenticado, admin, gerente, caixa, garcom);
                if (frm.ShowDialog() == DialogResult.OK)
                    usuario = frm.Usuario1;
                else
                    usuario = null;

                return frm.DialogResult;
            }
        }
    }
}