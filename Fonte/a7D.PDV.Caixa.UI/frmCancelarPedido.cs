using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL;
using a7D.PDV.Componentes;
using a7D.PDV.EF.Enum;
using a7D.PDV.Fiscal.Services;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public partial class frmCancelarPedido : FormTouch
    {

        private PedidoInformation Pedido1 { get; set; }
        private string GuidIdentificacao { get; set; }
        private int IDUsuario { get; set; }
        private EOrigemPedido OrigemPedido;
        private static readonly string _formatoData = "yyyyMMddHHmmss";
        private static readonly IFormatProvider _cultureInfo = new CultureInfo("pt-BR");

        public frmCancelarPedido(int idUsuario, string guidIdentificacao) : this()
        {
            GuidIdentificacao = guidIdentificacao;
            IDUsuario = idUsuario;
        }

        public frmCancelarPedido(int idUsuario, string guidIdentificacao, EOrigemPedido origemPedido) : this()
        {
            GuidIdentificacao = guidIdentificacao;
            IDUsuario = idUsuario;
            OrigemPedido = origemPedido;
        }

        private frmCancelarPedido()
        {
            InitializeComponent();
        }

        private void frmCancelarPedido_Load(object sender, EventArgs e)
        {
            GA.Post(this);
            Pedido1 = Pedido.CarregarUltimoPedido(GuidIdentificacao);
            PopularMotivosCancelamento();
            PopularListaProdutos();
        }

        private void PopularListaProdutos()
        {
            dgvPrincipal.DataSource = Pedido1
                .ListaProduto
                .Where(pp => pp.Cancelado == false)
                .Select(pp => new CancelarPedidoVM(pp))
                .OrderBy(p => p.Nome)
                .ToArray();
        }

        private void PopularMotivosCancelamento()
        {
            var listaMotivo = MotivoCancelamento.Listar();
            listaMotivo.Insert(0, new MotivoCancelamentoInformation());
            cbbMotivoCancelamento.DataSource = listaMotivo;
        }

        private class CancelarPedidoVM
        {
            public CancelarPedidoVM(PedidoProdutoInformation pedidoProduto)
            {
                IDPedidoProduto = pedidoProduto.IDPedidoProduto.Value;
                Quantidade = pedidoProduto.Quantidade.Value;
                Nome = pedidoProduto.Produto.Nome;
            }

            public int IDPedidoProduto { get; set; }
            public decimal Quantidade { get; set; }
            public string Nome { get; set; }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            var motivo = (MotivoCancelamentoInformation)cbbMotivoCancelamento.SelectedItem;
            if (!motivo.IDMotivoCancelamento.HasValue)
            {
                MessageBox.Show("Selecione um motivo de cancelamento", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Enabled = false;
                Cursor = Cursors.WaitCursor;
                Refresh();
                if (ConfiguracoesCaixa.Valores.GerenciadorImpressao == ETipoGerenciadorImpressao.SAT
                 && Pedido1.RetornoSAT_venda?.IDRetornoSAT != null)
                {
                    Pedido1.RetornoSAT_venda = RetornoSAT.Carregar(Pedido1.RetornoSAT_venda.IDRetornoSAT.Value);
                    var horaConsultaUTC3 = DateTime.UtcNow.AddMinutes(-10);
                    if (DateTime.ParseExact(Pedido1.RetornoSAT_venda.timeStamp, _formatoData, _cultureInfo).ToUniversalTime() > horaConsultaUTC3)
                    {
                        if (Pedido1.RetornoSAT_cancelamento?.IDRetornoSAT == null)
                        {
                            var retornoCancSat = FiscalServices.Cancelamento(Pedido1.RetornoSAT_venda, AC.PDV.IDPDV.Value, IDUsuario).Enviar(out PedidoInformation pedido);

                            if (pedido != null)
                                Pedido.Salvar(pedido);

                            if (retornoCancSat.EEEEE == "07000" || retornoCancSat.EEEEE == "07007")
                            {
                                Pedido1.RetornoSAT_venda.RetornoSATCancelamento = retornoCancSat;
                                RetornoSAT.Salvar(Pedido1.RetornoSAT_venda);
                                CancelarPedido(motivo, true);
                                DialogResult = DialogResult.OK;
                                Close();
                                return;
                            }
                            else
                            {
                                MessageBox.Show($"Não foi possível cancelar o SAT!\n{retornoCancSat.mensagem}", retornoCancSat.EEEEE, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        CancelarPedido(motivo, false);
                        DialogResult = DialogResult.OK;
                        Close();
                        return;
                    }
                }
                CancelarPedido(motivo, true);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.E805, ex);
                DialogResult = DialogResult.Cancel;
            }
            finally
            {
                Enabled = true;
                Cursor = Cursors.Default;
                Refresh();
            }
        }

        private void CancelarPedido(MotivoCancelamentoInformation motivo, bool satCancelado)
        {
            if (Pedido1.OrigemPedido.IDOrigemPedido == (int)EOrigemPedido.ifood)
            {
                SalvarCodigoCancelamentoIfood(motivo);
            }

            var statusPedido = (EStatusPedido)Pedido1.StatusPedido.IDStatusPedido.Value;
            var listCancelados = new List<PedidoProdutoInformation>();

            foreach (DataGridViewRow row in dgvPrincipal.Rows)
            {
                var observacoesCancelamento = row.Cells[nameof(colObservacoes)].Value as string;
                var idPedidoProduto = Convert.ToInt32(row.Cells[nameof(colIDPedidoProduto)].Value);
                var retornarAoEstoque = Convert.ToBoolean(row.Cells[nameof(colRetornarAoEstoque)].Value);

                if (statusPedido == EStatusPedido.Aberto)
                {
                    var pedidoProduto = Pedido1.ListaProduto.First(pp => pp.IDPedidoProduto == idPedidoProduto);
                    listCancelados.Add(pedidoProduto);
                }

                Pedido.CancelarProduto(AC.PDV.IDPDV.Value, IDUsuario, idPedidoProduto, motivo.IDMotivoCancelamento.Value, observacoesCancelamento, retornarAoEstoque);

                frmPedidoProdutoCancelamento.ImprimirComprovanteCancelamento(idPedidoProduto);
            }

            if (statusPedido == EStatusPedido.Aberto)
            {
                OrdemProducaoServices.GerarOrdemProducao(listCancelados, pedidoCancelado: true);
            }

            if (Pedido1.OrigemPedido.IDOrigemPedido == (int)EOrigemPedido.ifood)
            {
                if (Pedido1.StatusPedido.StatusPedido == EStatusPedido.EmCancelamento)
                {
                    OrdemProducaoServices.GerarOrdemProducao(listCancelados, pedidoCancelado: true);
                }
                else
                {
                    TagInformation tagStatus = new TagInformation();
                    tagStatus.GUIDIdentificacao = Pedido1.GUIDIdentificacao;
                    tagStatus.Chave = "ifood-status";
                    CRUD.Carregar(tagStatus);

                    tagStatus.Valor = "requestCancellation";
                    CRUD.Alterar(tagStatus);
                }
            }

            Pedido.AlterarStatus(Pedido1.IDPedido.Value, EStatusPedido.Cancelado);

            //frmPedidoPagamento.AlterarStatus((ETipoPedido)Pedido1.TipoPedido.IDTipoPedido.Value, GuidIdentificacao);
            FecharVenda.LiberaMesaComanda(Pedido1.TipoPedido.TipoPedido, GuidIdentificacao);
            PedidoPagamento.CancelarPorPedido(Pedido1.IDPedido.Value, IDUsuario);

            if (satCancelado)
            {
                MessageBox.Show("Pedido cancelado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Não foi possível cancelar o SAT porque o cupom foi emitido há mais de 1 hora.\nPedido cancelado no sistema", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //if (Pedido1.GUIDIdentificacao?.StartsWith("ifood#") == true)
            //    MessageBox.Show("O IFOOD não cancela pedidos confirmados\nEntre em contato com o cliente informando o cancelamento", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SalvarCodigoCancelamentoIfood(MotivoCancelamentoInformation motivo)
        {
            Dictionary<string, string> motivos = new Dictionary<string, string>();
            motivos.Add("", "");
            motivos.Add("501", "PROBLEMAS DE SISTEMA");
            motivos.Add("502", "PEDIDO EM DUPLICIDADE");
            motivos.Add("503", "ITEM INDISPONÍVEL");
            motivos.Add("504", "RESTAURANTE SEM MOTOBOY");
            motivos.Add("505", "CARDÁPIO DESATUALIZADO");
            motivos.Add("506", "PEDIDO FORA DA ÁREA DE ENTREGA");
            motivos.Add("507", "CLIENTE GOLPISTA / TROTE");
            motivos.Add("508", "FORA DO HORÁRIO DO DELIVERY");
            motivos.Add("509", "DIFICULDADES INTERNAS DO RESTAURANTE");
            motivos.Add("511", "ÁREA DE RISCO");
            motivos.Add("512", "RESTAURANTE ABRIRÁ MAIS TARDE");
            motivos.Add("513", "RESTAURANTE FECHOU MAIS CEDO");

            string cancellationCode = motivos.Where(v => v.Value == motivo.Nome).FirstOrDefault().Key;

            if (cancellationCode == null)
                cancellationCode = "509";

            TagInformation tag = new TagInformation();
            tag.GUIDIdentificacao = Pedido1.GUIDIdentificacao;
            tag.Chave = "ifood-cancellationCode";
            tag.Valor = cancellationCode;
            tag.DtInclusao = DateTime.Now;
            CRUD.Adicionar(tag);
        }
    }
}
