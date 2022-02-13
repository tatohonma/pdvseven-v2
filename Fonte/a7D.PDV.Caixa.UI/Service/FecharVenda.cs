using a7D.PDV.BLL;
using a7D.PDV.BLL.Services;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Caixa.UI
{
    public interface IPedidoPagamentoForm
    {
        PedidoInformation PedidoAtual { get; }

        decimal ValorPendente { get; }

        void Fechar();

        void HabilitarParcial();
    }

    public class FecharVenda
    {
        private PedidoInformation Pedido1 { get; }
        private string Observacoes { get; }
        private AreaImpressaoInformation AreaImpressao { get; set; }
        private string ModeloImpressoraSAT { get; set; }
        private bool GerarNf { get; }

        private static int ImpressaoContaMudou = 0;

        public FecharVenda(PedidoInformation pedido) : this(pedido, true, string.Empty)
        {
        }

        public FecharVenda(PedidoInformation pedido, bool gerarNf, string observacoes)
        {
            Pedido1 = pedido;
            GerarNf = gerarNf;
            Observacoes = observacoes;
        }

        public FecharVenda ComAreaDeImpressao(AreaImpressaoInformation areaImpressao)
        {
            if (!string.IsNullOrWhiteSpace(ModeloImpressoraSAT))
                throw new ArgumentException("Não é possível especificar areaImpressao e modeloImpressoraSAT ao mesmo tempo", nameof(areaImpressao));
            AreaImpressao = areaImpressao;
            return this;
        }

        public FecharVenda NaImpressora(string modeloImpressoraSAT)
        {
            if (AreaImpressao != null)
                throw new ArgumentException("Não é possível especificar areaImpressao e modeloImpressoraSAT ao mesmo tempo", nameof(modeloImpressoraSAT));
            ModeloImpressoraSAT = modeloImpressoraSAT;
            return this;
        }

        public DialogResult Fechar(bool calcularTroco, bool imprimir, bool expedicao)
        {
            try
            {
                if (Pedido1.TipoPedido.TipoPedido == ETipoPedido.Delivery && ConfiguracoesSistema.Valores.ImprimirViaExpedicao == "ENTREGA")
                    imprimir = false;
                else if (imprimir != (ConfiguracoesCaixa.Valores.ImprimirCupomFiscal == "SIM"))
                {
                    if (++ImpressaoContaMudou >= 3)
                    {
                        string questao;
                        if (imprimir)
                            questao = "Deseja que a impresssão do cupom fique sempre ativa?";
                        else
                            questao = "Deseja que a impresssão do cupom fique sempre inativa?";

                        if (MessageBox.Show(questao, "Impressão do Cupom", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            ConfiguracaoBD.DefinirValorPadraoTipoPDV(EConfig.ImprimirCupomFiscal, ETipoPDV.CAIXA, AC.idPDV, imprimir ? "SIM" : "NAO");
                            ConfiguracoesCaixa.Recarregar();
                        }
                        ImpressaoContaMudou = 0;
                    }
                }
                else
                    ImpressaoContaMudou = 0;

                if (Pedido1.Cliente != null && !Pedido1.Cliente.IDCliente.HasValue)
                    Pedido1.Cliente = null;

                else if (Pedido1.Cliente == null
                    && Pedido1.EnviarNfEmailCliente
                    && !string.IsNullOrEmpty(Pedido1.EmailCliente))
                {
                    var novoCliente = new EF.Models.tbCliente
                    {
                        NomeCompleto = "e-mail: " + Pedido1.EmailCliente,
                        Email = Pedido1.EmailCliente,
                        Documento1 = Pedido1.DocumentoCliente,
                    };

                    var cliente = Cliente.BuscarCliente(Cliente.TipoCliente.EMAIL, Pedido1.EmailCliente, novoCliente);
                    if (cliente != null)
                    {
                        Pedido1.Cliente = Cliente.Carregar(cliente.IDCliente);
                    }
                }

                var total = Pedido1.ValorTotalProdutos - (Pedido1.ValorDesconto ?? 0) + Pedido1.ValorServico.Value + (Pedido1.ValorEntrega ?? 0);
                var valorPago = 0m;

                if (Pedido1.ListaPagamento?.Count > 0)
                    valorPago = Pedido1.ListaPagamento.Where(pp => pp.Status != StatusModel.Excluido).Sum(l => l.Valor.Value);

                var valorPendente = total - valorPago;
                bool emitirFiscal = Pedido1.ValorTotalProdutosServicos > 0 && (Pedido1.ValorDesconto ?? 0) < Pedido1.ValorTotalProdutosServicos;
                bool emitirCupon = Pedido1.ValorTotalProdutosServicos > 0 && (Pedido1.ValorDesconto ?? 0) == Pedido1.ValorTotalProdutosServicos;

                if (emitirFiscal && Pedido1.ListaPagamento.Any(p => p.IDGateway == (int)EGateway.ContaCliente && p.IDSaldoBaixa == null))
                {
                    // Se há pagamentos por conta cleintes, só gera SAT para o que não for FIADO (saldo positivo)
                    emitirFiscal = false;
                    emitirCupon = true;
                }
                else if (emitirFiscal && Pedido1.ListaProduto.Any(p => p.Produto.TipoProduto.IDTipoProduto == (int)ETipoProduto.Credito))
                {
                    // Se há compra de créditos não gera fiscal
                    emitirFiscal = false;
                    emitirCupon = true;
                }

                if (emitirFiscal)
                {
                    switch (ConfiguracoesCaixa.Valores.GerenciadorImpressao)
                    {
                        case ETipoGerenciadorImpressao.ImpressoraWindows: //Impressora Win
                            if (imprimir)
                            {
                                // Pedido1.NumeroCupom = frmPrincipal.Impressora1.GerarCupom(Pedido1, true);
                                ContaServices.ImprimirConta(ConfiguracoesCaixa.Valores.ModeloImpressora, Pedido1);
                            }
                            break;
                        case ETipoGerenciadorImpressao.ACBr: //ECF - ACbr
                        case ETipoGerenciadorImpressao.ECFBemafii: //ECF - Bemafii
                            Pedido1.NumeroCupom = frmPrincipal.Impressora1.GerarCupom(Pedido1, true);
                            break;
                        case ETipoGerenciadorImpressao.SAT: //SAT
                            if (Pedido1.ValorTotalTemp > 10000)
                            {
                                MessageBox.Show("Não é possivel gerar SAT com valor superior a R$ 10.000\r\n\r\nSugestão: Transfira alguns itens para outra mesa ou comanda e tente novamente.", "LIMITE SAT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return DialogResult.Cancel;
                            }
                            else if (!TentarFecharVenda(imprimir))
                                return DialogResult.Cancel;

                            break;
                    }
                }
                else if (emitirCupon)
                {
                    switch (ConfiguracoesCaixa.Valores.GerenciadorImpressao)
                    {
                        case ETipoGerenciadorImpressao.ImpressoraWindows:
                        case ETipoGerenciadorImpressao.SAT:
                            if (imprimir)
                                ContaServices.ImprimirConta(ConfiguracoesCaixa.Valores.ModeloImpressora, Pedido1);
                            break;
                        default:
                            Pedido1.NumeroCupom = frmPrincipal.Impressora1.GerarCupom(Pedido1, false);
                            break;
                    }
                }

                GerarTicket();

                if (expedicao
                 && OrdemProducaoServices.ImprimirViaExpedicao == "CONTA"
                 && OrdemProducaoServices.IDAreaViaExpedicao > 0
                 && Pedido1.StatusPedido.StatusPedido == EStatusPedido.Aberto
                 && Pedido1.ListaProduto.Any(p => p.Viagem == true))
                    OrdemProducaoServices.GerarViaExpedicao(Pedido1.IDPedido.Value, OrdemProducaoServices.IDAreaViaExpedicao);

                Pedido1.Observacoes = Observacoes;

                if (calcularTroco) // para gerar toco pode ser necessário refazer os valores em dinheiro
                    CalcularPagamentoDinheiro(Pedido1, valorPendente);

                Pedido.FecharVendaDB(Pedido1, frmPrincipal.Caixa1, AC.Usuario.IDUsuario.Value);

                if (Pedido1.StatusPedido.StatusPedido == EStatusPedido.Finalizado)
                    // Só libera quando o tipo de pedido é mesa ou comanda
                    LiberaMesaComanda(Pedido1.TipoPedido.TipoPedido, Pedido1.GUIDIdentificacao);

                EntradaSaida.Movimentar(Pedido1);

                if (ConfiguracoesCaixa.Valores.AbrirGaveta)
                    Impressora.ImpressoraHelper.AbrirGaveta(ConfiguracoesCaixa.Valores.ModeloImpressora);

                try
                {
                    if (Pedido1.PodeEnviarEmailNfCliente && Pedido1.RetornoSAT_venda != null)
                        new System.Threading.Tasks.Task(() => EnviarEmailNfeCliente()).Start();
                }
                catch (Exception)
                {
                }

                if (ConfiguracoesCaixa.Valores.GerenciadorImpressao == ETipoGerenciadorImpressao.SAT
                 && Pedido1.Cliente != null
                 && !frmPrincipal.ModoContingencia
                 && frmPrincipal.ContaCliente)
                 // A testar!!! Só emite SAT quando há liquidação onde é verificado se houve credito
                 // && Pedido1.ListaProduto.Any(p => p.Produto.TipoProduto.TipoProduto == ETipoProduto.Credito))
                {
                    // Se houve creditos/debitos tenta emitir SAT dos pedidos liquidados caso existam
                    EmiteDebitosSAT(Pedido1.Cliente.IDCliente.Value, imprimir);
                }

                return DialogResult.OK;
            }
            catch (Exception ex)
            {
                Logs.ErroBox(CodigoErro.EC10, ex);
                return DialogResult.None;
            }
        }

        private bool TentarFecharVenda(bool imprimir)
        {
            if (frmPrincipal.ModoContingencia)
            {
                if (imprimir)
                    //Pedido1.NumeroCupom = frmPrincipal.Impressora1.GerarCupom(Pedido1, true);
                    ContaServices.ImprimirConta(ConfiguracoesCaixa.Valores.ModeloImpressora, Pedido1);

                return true;
            }
            while (true)
            {
                frmAguardandoSAT frmSAT = null;
                var result = default(DialogResult);
                if (!string.IsNullOrWhiteSpace(ModeloImpressoraSAT))
                    frmSAT = new frmAguardandoSAT(Pedido1, GerarNf, AC.PDV.IDPDV.Value, AC.Usuario.IDUsuario.Value, ModeloImpressoraSAT);
                else
                    frmSAT = new frmAguardandoSAT(Pedido1, GerarNf, AC.PDV.IDPDV.Value, AC.Usuario.IDUsuario.Value, AreaImpressao);

                frmSAT.ImprimirCupon = imprimir;

                result = frmSAT.ShowDialog();
                if (result == DialogResult.No)
                {
                    ExceptionPDV exPDV = new ExceptionPDV(CodigoErro.E501, frmSAT.Exception);

                    var tentarNovamente = MessageBox.Show(exPDV.Message + "\n" +
          "\nTentar conectar novamente?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (tentarNovamente == DialogResult.Yes)
                    {
                        continue;
                    }
                    else
                    {
                        var modocontingencia = MessageBox.Show(
  "Deseja entrar em modo de contingência?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (modocontingencia == DialogResult.Yes)
                        {
                            var resultContingencia = MessageBox.Show(
                                              "SISTEMA EM MODO DE CONTINGÊNCIA.\n\n" +
                                                   "A partir de agora os cupons fiscais ficarão pendentes." +
                                                   " Para emissao dos cupons fiscais pendentes, acesse Backoffice (aba S@t).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            frmPrincipal.ModoContingencia = true;

                            if (imprimir)
                                ContaServices.ImprimirConta(ConfiguracoesCaixa.Valores.ModeloImpressora, Pedido1);
                        }
                        else
                            return false;
                    }
                }
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Ocorreu um erro ao imprimir, mas o cupom SAT FOI ENVIADO.\nVocê pode tentar reimprimir o cupom no menu \"SAT>Reimprimir SAT\"\n\nDetalhes do erro: " + frmSAT.Exception.Message, "Erro ao imprimir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                break;
            }
            return true;
        }

        internal static void CalcularPagamentoDinheiro(PedidoInformation pedido, decimal valorPendente)
        {
            var pagamentosDinheiroTodos = pedido.ListaPagamento.Where(l => l.TipoPagamento.IDTipoPagamento == 1).ToList();
            var valorDinheiro = pagamentosDinheiroTodos.Sum(l => l.Valor.Value);
            if ((valorDinheiro + valorPendente) > 0)
            {
                var pagamentoDinheiro = new PedidoPagamentoInformation
                {
                    Pedido = new PedidoInformation { IDPedido = pedido.IDPedido },
                    TipoPagamento = new TipoPagamentoInformation { IDTipoPagamento = 1 },
                    Valor = valorDinheiro + valorPendente,
                    Autorizacao = $"TOTAL",
                    Excluido = false
                };

                if (pagamentoDinheiro.Valor.Value != valorDinheiro)
                {
                    while (pagamentosDinheiroTodos.Count > 0)
                    {
                        var pagamento = pagamentosDinheiroTodos[0];
                        if (pagamento.IDPedidoPagamento.HasValue)
                            PedidoPagamento.Cancelar(pagamento, AC.Usuario.IDUsuario.Value);

                        pedido.ListaPagamento.Remove(pagamento);
                        pagamentosDinheiroTodos.RemoveAt(0);
                    }

                    pagamentoDinheiro.ConfiguraDinheiro();
                    PedidoPagamento.Salvar(pagamentoDinheiro, AC.Usuario.IDUsuario.Value);
                    pedido.ListaPagamento.Add(pagamentoDinheiro);
                }
            }
        }

        internal static void LiberaMesaComanda(ETipoPedido tipoPedido, string guidIdentificacao)
        {
            MesaInformation mesa;
            ComandaInformation comanda;
            switch (tipoPedido)
            {
                case ETipoPedido.Mesa:
                    mesa = Mesa.CarregarPorGUID(guidIdentificacao);
                    mesa.StatusMesa.IDStatusMesa = (int)EStatusMesa.Liberada;
                    Mesa.Salvar(mesa);
                    break;

                case ETipoPedido.Comanda:
                    comanda = Comanda.CarregarPorGUID(guidIdentificacao);
                    int? statusComanda = comanda.StatusComanda.IDStatusComanda;
                    comanda.StatusComanda = StatusComanda.Carregar(EStatusComanda.Liberada);
                    Comanda.Salvar(comanda);
                    break;
            }
        }

        private void GerarTicket()
        {
            if (ConfiguracoesCaixa.Valores.GerarTicketPrePago > 0
             && Pedido1.TipoPedido.TipoPedido == ETipoPedido.Balcao)
                frmPrincipal.Impressora1.GerarTicketPrePago(Pedido1);
        }

        private void EnviarEmailNfeCliente()
        {
            try
            {
                //http://apipdvseven.azurewebsites.net/wsUtil.asmx

                var confSat = new ConfiguracoesSAT();
                var wsUtil = new BLL.Ativacoes.wsUtil();

                wsUtil.EnviarNFEAoCliente(new BLL.Ativacoes.DadosNfeCliente
                {
                    CPFCNPJCliente = Pedido1.DocumentoCliente,
                    DataEmissao = DateTime.Now.ToShortDateString(),
                    ValorTotal = string.Format("{0:C}", Pedido1.ValorTotal.Value),
                    NomeFantasiaEmissor = confSat.infCFe_nome_fantasia,
                    Email = Pedido1.EmailCliente ?? Pedido1.Cliente.Email,
                    ChaveAcessoNF = Pedido1.RetornoSAT_venda.chaveConsulta.Remove(0, 3),
                    NumeroNota = Pedido1.RetornoSAT_venda.numeroSessao,
                    UF = confSat.InfCFe_UF,
                    XMLBase64 = Pedido1.RetornoSAT_venda.arquivoCFeSAT
                });
            }
            catch (Exception ex)
            {
                Logs.Erro(CodigoErro.A371, ex);
            }
        }

        public void EmiteDebitosSAT(int idCliente, bool imprimir)
        {
            // Pedidos liquidados sem SAT emitidos do Cliente Atual (fiados)
            string sql = @"SELECT DISTINCT p.IDPedido FROM tbPedidoPagamento pp
INNER JOIN tbPedido p ON pp.IDPedido = p.IDPedido
INNER JOIN tbSaldo s ON s.IDPedido = p.IDPedido
WHERE pp.IDGateway = 15
AND p.ValorTotal>0
AND p.IDStatusPedido=40
AND pp.IDSaldoBaixa > 0 
AND p.IDRetornoSAT_venda IS NULL 
AND s.IDCliente=" + idCliente;

            var pedidos = EF.Repositorio.Query<int>(sql);

            foreach (var id in pedidos)
            {
                if (frmPrincipal.ModoContingencia)
                    return;

                var pedido = Pedido.CarregarCompleto(id);
                try
                {
                    // Mesma logica do Caixa
                    if (pedido.ValorTotalProdutosServicos > 0 && (pedido.ValorDesconto ?? 0) < pedido.ValorTotalProdutosServicos)
                    {
                        var frmSAT = new frmAguardandoSAT(pedido, GerarNf, AC.PDV.IDPDV.Value, AC.Usuario.IDUsuario.Value, ModeloImpressoraSAT);
                        frmSAT.ImprimirCupon = imprimir;
                        var result = frmSAT.ShowDialog(); // Processa

                        // Se der erro para!
                        if (result != DialogResult.OK)
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logs.Erro(ex);
                }
            }
        }
    }
}
