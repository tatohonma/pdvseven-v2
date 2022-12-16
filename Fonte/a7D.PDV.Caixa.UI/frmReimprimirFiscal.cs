using a7D.PDV.BLL;
using a7D.PDV.BLL.Services;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmReimprimirFiscal : FormTouch
    {
        private DadosGridReimpressao[] lista;

        public frmReimprimirFiscal()
        {
            InitializeComponent();
        }

        private void frmReimprimirSAT_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            Todos();
        }

        private void Todos()
        {
            string sql = @"SELECT 
    p.IDPedido, 
    p.DtPedido DataPedido, 
    p.IDRetornoSAT_venda IDRetornoSAT, 
    p.DocumentoCliente,
    p.ValorTotal,
    CASE p.IDTipoPedido 
	    WHEN 10 THEN 'Mesa ' + CAST( m.Numero AS VARCHAR)
	    WHEN 20 THEN 'Comanda ' + CAST(c.Numero AS VARCHAR)
	    WHEN 30 THEN 'Delivery'
	    WHEN 40 THEN 'Balcão'
	    END Origem, 
    pdv.Nome pdvNome
FROM tbPedido p (nolock) 
INNER JOIN tbCaixa cx ON p.IDCaixa=cx.IDCaixa
INNER JOIN tbPDV pdv ON cx.IDPDV=pdv.IDPDV
LEFT JOIN tbMesa m ON p.GUIDIdentificacao=m.GUIDIdentificacao
LEFT JOIN tbComanda c ON p.GUIDIdentificacao=c.GUIDIdentificacao
WHERE p.IDStatusPedido=40 
    AND NOT p.DtPedidoFechamento IS NULL 
    AND p.ValorTotal>0
    AND cx.DtFechamento IS NULL
ORDER BY DataPedido";

            lista = EF.Repositorio.Query<DadosGridReimpressao>(sql);
            dgvPrincipal.DataSource = lista;
            dgvPrincipal.ClearSelection();
        }

        private void btnEmitir_Click(object sender, EventArgs e)
        {
            var emitir = lista.Where(p => p.Selecione && p.IDRetornoSAT is null);
            if (emitir.Count() == 0)
            {
                MessageBox.Show("Selecione pedidos não emitidos para emissão.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvPrincipal.UseWaitCursor = true;
            btnEmitir.Enabled = btnReimprimir.Enabled = false;
            try
            {
                foreach (var item in emitir)
                {
                    var pedido = Pedido.CarregarCompleto(item.IDPedido);
                    if (pedido.RetornoSAT_venda == null)
                    {
                        // Garante a não emissão em duplicidade da mesma nota!
                        var frmSAT = new frmAguardandoSAT(pedido, !string.IsNullOrEmpty(pedido.DocumentoCliente), AC.PDV.IDPDV.Value, AC.Usuario.IDUsuario.Value, ConfiguracoesCaixa.Valores.ModeloImpressora);
                        if (frmSAT.ShowDialog() != DialogResult.OK)
                        {
                            if (frmSAT.Exception != null)
                                throw frmSAT.Exception;

                            continue;
                        }
                        item.IDRetornoSAT = frmSAT.RetornoSat.IDRetornoSAT.Value;
                        frmPrincipal.ModoContingencia = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E513, ex);
            }
            finally
            {
                dgvPrincipal.Refresh();
                dgvPrincipal.ClearSelection();
                dgvPrincipal.UseWaitCursor = false;
                btnEmitir.Enabled = btnReimprimir.Enabled = true;
            }
        }

        private void btnReimprimir_Click(object sender, EventArgs e)
        {
            var imprimir = lista.Where(p => p.Selecione);

            dgvPrincipal.UseWaitCursor = true;
            btnEmitir.Enabled = btnReimprimir.Enabled = false;
            try
            {
                foreach (var item in imprimir)
                {
                    if(item.IDRetornoSAT > 0)
                    {
                        var retornoSAT = RetornoSAT.Carregar(item.IDRetornoSAT.Value);
                        if (!CupomSATService.ImprimirCupomVenda(retornoSAT.arquivoCFeSAT, item.IDPedido, ConfiguracoesCaixa.Valores.ModeloImpressora, out Exception mensagemErro))
                            throw mensagemErro;
                    }
                    else
                    {
                        PedidoInformation pedido = Pedido.CarregarCompleto(item.IDPedido);

                        switch (ConfiguracoesCaixa.Valores.GerenciadorImpressao)
                        {
                            case ETipoGerenciadorImpressao.ACBr:
                            case ETipoGerenciadorImpressao.ECFBemafii:
                                frmPrincipal.Impressora1.GerarCupom(pedido, false);
                                break;
                            case ETipoGerenciadorImpressao.ImpressoraWindows:
                            case ETipoGerenciadorImpressao.SAT:
                                ContaServices.ImprimirConta(ConfiguracoesCaixa.Valores.ModeloImpressora, pedido);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E506, ex);
            }
            finally
            {
                dgvPrincipal.ClearSelection();
                dgvPrincipal.UseWaitCursor = false;
                btnEmitir.Enabled = btnReimprimir.Enabled = true;
            }
        }

        private void dgvPrincipal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var dados = (DadosGridReimpressao)dgvPrincipal.Rows[e.RowIndex].DataBoundItem;
            dados.Selecione = !dados.Selecione;
            dgvPrincipal.Refresh();
        }
    }

    class DadosGridReimpressao
    {
        public bool Selecione { get; set; }
        public int IDPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public int? IDRetornoSAT { get; set; }
        public string DocumentoCliente { get; set; }
        public decimal ValorTotal { get; set; }
        public string Origem { get; set; }
        public string PDVNome { get; set; }
    }
}
