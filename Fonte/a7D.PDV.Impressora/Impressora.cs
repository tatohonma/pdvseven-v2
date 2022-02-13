using a7D.PDV.BLL;
using a7D.PDV.BLL.Extension;
using a7D.PDV.BLL.Services;
using a7D.PDV.Model;
using ACBrFramework.ECF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Impressora
{
    public class Impressora
    {
        const int TamanhoItem = 14;

        #region Parametros e Construtor
        private Int32 TipoGerenciadorImpressao;
        private Boolean ModoFiscal;
        String ModeloImpressora;

        ACBrECF ImpressoraACBrECF;

        public Boolean ImpressoraAtiva;
        public Boolean ModoFiscalAtivo;

        private int Velocidade { get; set; }
        private string Porta { get; set; }

        public EstadoECF Estado
        {
            get
            {
                switch (TipoGerenciadorImpressao)
                {
                    case 0:
                    case 1:
                    case 4:
                        return EstadoECF.Livre;
                    case 2:
                        return ACBr_Estado();
                    //case 3:
                    //    return Bematech_Estado();
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public String NumCOO { get; set; }

        List<String> Cupom;

        #endregion

        #region Iniciar e Monitorar Impressora
        public Boolean Iniciar(int tipoGerenciadorImpressao, bool modoFiscal, string modeloImpressora, int velocidade, string porta, out String msgRetorno)
        {
            EstadoECF estadoECF;
            TipoGerenciadorImpressao = tipoGerenciadorImpressao;
            ModoFiscal = modoFiscal;
            ModeloImpressora = modeloImpressora;
            Porta = porta;
            Velocidade = velocidade;

            ImpressoraAtiva = false;
            ModoFiscalAtivo = false;

            Boolean verificar = true;
            Boolean ret;
            msgRetorno = "";

            if (TipoGerenciadorImpressao == 0)
            {
                ret = true;
            }
            else if (TipoGerenciadorImpressao == 1 || TipoGerenciadorImpressao == 4)
            {
                ImpressoraAtiva = true;
                ModoFiscalAtivo = true;
                ret = true;
            }
            else
            {
                ret = true;

                try
                {
                    AtivarImpressoraECF();

                    while (verificar)
                    {
                        estadoECF = this.Estado;
                        switch (estadoECF)
                        {
                            case EstadoECF.Desconhecido:
                                ImpressoraAtiva = true;
                                ModoFiscalAtivo = false;
                                ret = false;
                                msgRetorno = "ECF com erro desconhecido";
                                verificar = false;
                                break;

                            case EstadoECF.Bloqueada:
                                ImpressoraAtiva = true;
                                ModoFiscalAtivo = false;
                                ret = true;
                                msgRetorno = "ECF Bloqueada";
                                verificar = false;
                                break;

                            case EstadoECF.NaoInicializada:
                                ImpressoraAtiva = false;
                                ModoFiscalAtivo = false;
                                ret = false;
                                msgRetorno = "Não foi possível conectar na ECF";
                                verificar = false;
                                break;

                            case EstadoECF.NaoFiscal:
                                ImpressoraAtiva = true;
                                ModoFiscalAtivo = false;
                                msgRetorno = "Sistema em modo NÃO FISCAL";
                                verificar = false;
                                break;

                            case EstadoECF.Venda:
                            case EstadoECF.Pagamento:
                                ImpressoraAtiva = true;
                                ModoFiscalAtivo = true;

                                try { this.CupomFiscal_Cancelar(); }
                                catch { }

                                break;

                            case EstadoECF.Relatorio:
                                ImpressoraAtiva = true;
                                ModoFiscalAtivo = true;

                                try { this.CupomNaoFiscal_Cancelar(); }
                                catch { }

                                break;

                            case EstadoECF.RequerZ:
                                ImpressoraAtiva = true;
                                ModoFiscalAtivo = true;

                                this.ReducaoZ();
                                break;

                            case EstadoECF.RequerX:
                                ImpressoraAtiva = true;
                                ModoFiscalAtivo = true;

                                this.LeituraX();
                                break;

                            case EstadoECF.Livre:
                                ImpressoraAtiva = true;
                                ModoFiscalAtivo = true;

                                verificar = false;
                                break;
                        }
                    }
                }
                catch (Exception _e)
                {
                    ret = false;
                    msgRetorno = "Não foi possível conectar na impressora!\nDetalhes erro: " + _e.Message;
                }
            }

            return ret;
        }
        public Boolean VerificarStatus(out String msgRetorno)
        {
            Boolean ret = true;
            DialogResult dialogRetorno;
            msgRetorno = "";

            if (TipoGerenciadorImpressao != 0 && TipoGerenciadorImpressao != 1 && TipoGerenciadorImpressao != 4 && ImpressoraAtiva == true)
            {
                switch (this.Estado)
                {
                    case EstadoECF.Desconhecido:
                        ret = false;
                        msgRetorno = "ECF com erro desconhecido";
                        break;

                    case EstadoECF.Bloqueada:
                        ret = false;
                        msgRetorno = "ECF Bloqueada";
                        break;

                    case EstadoECF.NaoInicializada:
                        ret = false;
                        msgRetorno = "Não foi possível conectar na ECF";
                        break;

                    case EstadoECF.RequerZ:
                        if (DateTime.Now.Hour <= 1)
                        {
                            dialogRetorno = MessageBox.Show("Redução Z pendente. Deseja emitir agora?\n A Redução Z pode ser emitida até 01:00 e será emitida automaticamente após o horário limite.", "ATENÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dialogRetorno == DialogResult.Yes)
                                this.ReducaoZ();
                        }
                        else
                        {
                            this.ReducaoZ();
                        }
                        break;
                }

                //if (ImpressoraACBrECF.DataHora > DateTime.Now.AddHours(1) || ImpressoraACBrECF.DataHora < DateTime.Now.AddHours(-1))
                //{
                //    ret = false;
                //    msgRetorno = "Verificar a Data e Hora do computador e do ECF! Não é tolerado uma diferença maior que 1 hora.";
                //}
            }
            return ret;
        }

        private void AtivarImpressoraECF()
        {
            switch (TipoGerenciadorImpressao)
            {
                case 2:
                    ImpressoraACBrECF = new ACBrECF();
                    ImpressoraACBrECF.OnMsgPoucoPapel += ACBr_OnMsgPoucoPapel;

                    ModeloECF modeloECF;
                    Enum.TryParse<ModeloECF>(ModeloImpressora, true, out modeloECF);

                    ImpressoraACBrECF.Modelo = modeloECF;
                    ImpressoraACBrECF.Device.TimeOut = 1;
                    ImpressoraACBrECF.Device.Baud = Velocidade;
                    ImpressoraACBrECF.Device.Porta = Porta;

                    ImpressoraACBrECF.Ativar();
                    break;

                    //case 3:
                    //    Int32 iRetorno;
                    //    Int32 flags = 0;

                    //    iRetorno = BemaFI32.Bematech_FI_FlagsFiscais(ref flags);
                    //    if (iRetorno != 1)
                    //    {
                    //        BemaFI32.Analisa_iRetorno(iRetorno);
                    //    }
                    //    else if (iRetorno == 1 && flags == 8)
                    //    {
                    //        BemaFI32.Analisa_iRetorno(iRetorno);
                    //    }
                    //    break;
            }
        }
        private void DesativarImpressora()
        {
        }
        #endregion

        #region Motoboy

        public void GerarCupomMotoboy(PedidoInformation pedido)
        {
            if (ImpressoraAtiva == true)
            {
                var relatorio = ExpedicaoServices.ComprovanteEntrega(pedido);
                RelatorioGerencial(relatorio);
            }
            else if (TipoGerenciadorImpressao != 0 && ImpressoraAtiva == false)
            {
                MessageBox.Show("Não é possível gerar o Cupom! Sem conexão com a impressora.");
            }
        }

        #endregion

        #region Cupom
        public String GerarCupom(PedidoInformation pedido, Boolean fiscal)
        {
            String numeroCCO = "";
            if (ImpressoraAtiva == true)
            {
                try
                {
                    if (fiscal == true)
                        numeroCCO = GerarCupomFiscal(pedido);
                    else
                        numeroCCO = GerarNaoFiscal(pedido);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("fiscal", fiscal);
                    ex.Data.Add("pedido", pedido);
                    throw new ExceptionPDV(CodigoErro.EC11, ex);
                }
            }
            else if (TipoGerenciadorImpressao != 0 && ImpressoraAtiva == false)
            {
                MessageBox.Show("Não é possível gerar o Cupom! Sem conexão com a impressora.");
            }

            return numeroCCO;
        }
        #endregion

        #region Cupom Fiscal
        private String GerarCupomFiscal(PedidoInformation pedido)
        {
            EstadoECF estadoECF;
            String cliente = "";
            Int32 numeroItem = 1;
            String codigoProduto;
            Decimal valor;
            //Decimal descontoProduto;
            String codigoAliquota;
            String identificacao = "";
            String aliquotaPadrao = ConfiguracoesSistema.Valores.AliquotaPadrao;

            estadoECF = this.Estado;
            if (estadoECF == EstadoECF.Venda || estadoECF == EstadoECF.Pagamento)
            {
                this.CupomFiscal_Cancelar();
            }
            else if (estadoECF == EstadoECF.Relatorio)
            {
                this.CupomNaoFiscal_Cancelar();
            }

            estadoECF = this.Estado;
            if (estadoECF != EstadoECF.Livre)
            {
                throw new Exception("Não foi possível gerar a venda. Favor verificar se a impressora está conectada e tentar novamente!\nRetorno impressora: " + estadoECF.ToString());
            }

            if (pedido.ValorTotalProdutosServicos <= 0)
                return "";

            try
            {
                identificacao = "PEDIDO " + pedido.IDPedido.Value.ToString("00000") + "\n";
                identificacao += pedido.DtPedido.Value.ToString("dd/MM/yyyy HH:mm:ss") + " - " + DateTime.Now.ToString("HH:mm:ss") + "\n";

                switch (pedido.TipoPedido.IDTipoPedido)
                {
                    case 10:
                        identificacao += "MESA " + Mesa.CarregarPorGUID(pedido.GUIDIdentificacao).Numero;
                        break;
                    case 20:
                        identificacao += "COMANDA " + Comanda.CarregarPorGUID(pedido.GUIDIdentificacao).Numero;
                        if (pedido.Cliente != null)
                        {
                            pedido.Cliente = Cliente.Carregar(pedido.Cliente.IDCliente.Value);

                            identificacao += "\n";
                            identificacao += pedido.Cliente.NomeCompleto;
                        }
                        break;
                    case 30:
                        identificacao += "DELIVERY";

                        if (pedido.Cliente != null)
                        {
                            pedido.Cliente = Cliente.Carregar(pedido.Cliente.IDCliente.Value);

                            identificacao += "\n";
                            identificacao += QuebrarEmLinhas(38, pedido.Cliente.NomeCompleto) + "\n";
                            identificacao += QuebrarEmLinhas(38, pedido.Cliente.Endereco + ", " + pedido.Cliente.EnderecoNumero) + "\n";

                            if (!String.IsNullOrEmpty(pedido.Cliente.Complemento))
                                identificacao += QuebrarEmLinhas(38, pedido.Cliente.Complemento) + "\n";

                            identificacao += QuebrarEmLinhas(38, pedido.Cliente.Bairro + " - " + pedido.Cliente.Cidade) + "\n";

                            if (!String.IsNullOrEmpty(pedido.Cliente.EnderecoReferencia))
                                identificacao += QuebrarEmLinhas(38, pedido.Cliente.EnderecoReferencia) + "\n";

                            identificacao += "Tel:" + pedido.Cliente.Telefone1Numero;
                        }
                        break;
                    case 40:
                        identificacao += "BALCÃO";
                        break;
                }


            }
            catch (Exception ex)
            {
                ex.Data.Add("identificacao", identificacao);
                throw new ExceptionPDV(CodigoErro.EC12, ex);
            }

            try
            {
                List<PedidoProdutoInformation> listaProduto = pedido.ListaProduto;
                List<PedidoProdutoInformation> listaProdutoModificacao = new List<PedidoProdutoInformation>();
                List<PedidoProdutoInformation> listaProdutoTodos = new List<PedidoProdutoInformation>();

                foreach (var item in listaProduto)
                {
                    if (item.ListaModificacao != null)
                        listaProdutoModificacao.AddRange(item.ListaModificacao);
                }
                listaProdutoTodos.AddRange(listaProduto);
                listaProdutoTodos.AddRange(listaProdutoModificacao);

                var listaProdutoAgrupado =
                    from l in listaProdutoTodos
                    group l by new { l.Produto.IDProduto, l.Produto.Nome, l.CodigoAliquota, l.ValorUnitario, l.Cancelado } into g
                    select new
                    {
                        Codigo = g.Key.IDProduto,
                        Nome = g.Key.Nome,
                        CodigoAliquota = g.Key.CodigoAliquota,
                        ValorUnitario = g.Key.ValorUnitario,
                        Cancelado = g.Key.Cancelado,
                        Quantidade = g.Sum(x => x.Quantidade)
                    };

                this.CupomFiscal_Abrir(identificacao, pedido.DocumentoCliente, cliente);

                foreach (var item in listaProdutoAgrupado)
                {
                    codigoProduto = item.Codigo.ToString();
                    valor = item.ValorUnitario.Value;

                    codigoAliquota = item.CodigoAliquota;
                    if (String.IsNullOrEmpty(codigoAliquota))
                        codigoAliquota = aliquotaPadrao;

                    if (valor > 0)
                    {
                        this.CupomFiscal_RegistrarItem(numeroItem, numeroItem.ToString(), item.Nome, item.Quantidade.Value, valor, codigoAliquota, 0, item.Cancelado.Value);
                        numeroItem++;
                    }
                }

                decimal valorTaxaEntrega = pedido.TaxaEntrega?.Valor ?? 0;

                this.CupomFiscal_Subtotalizar(0, pedido.ValorDesconto, pedido.ValorTotalProdutosServicos, valorTaxaEntrega);

                foreach (var item in pedido.ListaPagamento)
                {
                    this.CupomFiscal_RegistrarPagamento(item.TipoPagamento.Nome, item.TipoPagamento.CodigoImpressoraFiscal, item.Valor.Value);
                }

                this.CupomFiscal_Fechar(pedido.ValorTotalProdutosServicos - pedido.ValorDesconto.Value + valorTaxaEntrega, pedido.ListaPagamento.Sum(l => l.Valor).Value);

                return NumCOO;
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EC80, ex);
            }
        }

        private void CupomFiscal_Abrir(String identificacao, String documento, String cliente)
        {
            try
            {
                switch (TipoGerenciadorImpressao)
                {
                    case 1:
                    case 4:
                        ImpressoraWin_CupomNaoFiscal_Abrir(identificacao);
                        break;
                    case 2:
                        ACBr_CupomFiscal_Abrir(identificacao, documento, cliente);
                        break;
                        //case 3:
                        //    Bematech_CupomFiscal_Abrir(identificacao, documento, cliente);
                        //    break;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("identificacao", identificacao);
                ex.Data.Add("documento", documento);
                ex.Data.Add("cliente", cliente);
                throw new ExceptionPDV(CodigoErro.EC21, ex);
            }

        }
        private void CupomFiscal_RegistrarItem(Int32 numeroItem, String codigo, String descricao, Decimal quantidade, Decimal valor, String codigoAliquota, Decimal desconto, Boolean cancelado)
        {
            try
            {
                switch (TipoGerenciadorImpressao)
                {
                    case 1:
                    case 4:
                        ImpressoraWin_CupomNaoFiscal_RegistrarItem(codigo, descricao, "", quantidade, valor, desconto, cancelado);
                        break;
                    case 2:
                        ACBr_CupomFiscal_RegistrarItem(numeroItem, codigo, descricao, quantidade, valor, codigoAliquota, desconto, cancelado);
                        break;
                        //case 3:
                        //    Bematech_CupomFiscal_RegistrarItem(numeroItem, codigo, descricao, quantidade, valor, codigoAliquota, desconto, cancelado);
                        //    break;
                }
            }
            catch (Exception ex)
            {
                ex.Data.Add("numeroItem", numeroItem);
                ex.Data.Add("codigo", codigo);
                ex.Data.Add("descricao", descricao);
                ex.Data.Add("quantidade", quantidade);
                ex.Data.Add("valor", valor);
                ex.Data.Add("codigoAliquota", codigoAliquota);
                ex.Data.Add("desconto", desconto);
                ex.Data.Add("cancelado", cancelado);
                throw new ExceptionPDV(CodigoErro.EC22, ex);
            }

        }
        private void CupomFiscal_Subtotalizar(Decimal? acrescimo, Decimal? desconto, Decimal? valorTotal, Decimal? taxaEntrega)
        {
            try
            {
                switch (TipoGerenciadorImpressao)
                {
                    case 1:
                    case 4:
                        ImpressoraWin_CupomNaoFiscal_Subtotalizar(desconto.Value, acrescimo.Value, valorTotal.Value, taxaEntrega.Value);
                        break;
                    case 2:
                        ACBr_CupomFiscal_Subtotalizar(desconto.Value, acrescimo.Value + taxaEntrega.Value);
                        break;
                        //case 3:
                        //    Bematech_CupomFiscal_Subtotalizar(acrescimo + taxaEntrega.Value, desconto);
                        //    break;
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EC23, ex);
            }

        }
        private void CupomFiscal_RegistrarPagamento(String nome, String codigoPagamento, Decimal valor)
        {
            try
            {
                switch (TipoGerenciadorImpressao)
                {
                    case 1:
                    case 4:
                        ImpressoraWin_CupomNaoFiscal_RegistrarPagamento(nome, valor);
                        break;
                    case 2:
                        ACBr_CupomFiscal_RegistrarPagamento(codigoPagamento, valor);
                        break;
                        //case 3:
                        //    Bematech_CupomFiscal_RegistrarPagamento(codigoPagamento, valor);
                        //    break;
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EC24, ex);
            }
        }

        private void CupomFiscal_Fechar(Decimal valorTotal, Decimal valorTotalPago)
        {
            try
            {
                switch (TipoGerenciadorImpressao)
                {
                    case 1:
                    case 4:
                        ImpressoraWin_CupomNaoFiscal_Fechar(valorTotal, valorTotalPago);
                        break;
                    case 2:
                        ACBr_CupomFiscal_Fechar();
                        break;
                        //case 3:
                        //    Bematech_CupomFiscal_Fechar();
                        //    break;
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EC25, ex);
            }
        }

        private void CupomFiscal_Cancelar()
        {
            switch (TipoGerenciadorImpressao)
            {
                case 1:
                    ImpressoraWin_CupomNaoFiscal_Cancelar();
                    break;
                case 2:
                    ACBr_CupomFiscal_Cancelar();
                    break;
                    //case 3:
                    //    Bematech_CupomFiscal_Cancelar();
                    //    break;
            }
        }
        #endregion

        #region Cupom Nao Fiscal
        private String GerarNaoFiscal(PedidoInformation pedido)
        {
            EstadoECF estadoECF;
            String numCOO = "";
            Int32 numeroItem = 0;
            String msgCupom = ConfiguracoesSistema.Valores.MsgCupom;
            String identificacao = "";

            estadoECF = this.Estado;
            if (estadoECF == EstadoECF.Venda || estadoECF == EstadoECF.Pagamento)
            {
                this.CupomFiscal_Cancelar();
            }
            else if (estadoECF == EstadoECF.Relatorio)
            {
                this.CupomNaoFiscal_Cancelar();
            }

            estadoECF = this.Estado;
            if (estadoECF != EstadoECF.Livre)
            {
                throw new Exception("Não foi possível gerar a venda. Favor verificar se a impressora está conectada e tentar novamente!\nRetorno impressora: " + estadoECF.ToString());
            }

            identificacao = "PEDIDO " + pedido.IDPedido.Value.ToString("00000") + "\n";
            identificacao += pedido.DtPedido.Value.ToString("dd/MM/yyyy HH:mm:ss") + " - " + DateTime.Now.ToString("HH:mm:ss") + "\n";

            switch (pedido.TipoPedido.IDTipoPedido)
            {
                case 10:
                    identificacao += "MESA " + Mesa.CarregarPorGUID(pedido.GUIDIdentificacao).Numero;
                    break;
                case 20:
                    identificacao += "COMANDA " + Comanda.CarregarPorGUID(pedido.GUIDIdentificacao).Numero;
                    if (pedido.Cliente != null)
                    {
                        pedido.Cliente = Cliente.Carregar(pedido.Cliente.IDCliente.Value);

                        identificacao += "\n";
                        identificacao += pedido.Cliente.NomeCompleto;
                    }
                    break;
                case 30:
                    identificacao += "DELIVERY";

                    string ifood = pedido.PedidoIFood;
                    if (!string.IsNullOrEmpty(ifood))
                        identificacao += " IFOOD " + ifood;

                    if (pedido.Cliente != null)
                    {
                        pedido.Cliente = Cliente.Carregar(pedido.Cliente.IDCliente.Value);

                        identificacao += "\n";
                        var nome = pedido.Cliente.NomeCompleto;

                        identificacao += QuebrarEmLinhas(38, nome) + "\n";

                        var enderecoCompleto = pedido.Cliente.Endereco + ", " + pedido.Cliente.EnderecoNumero;

                        identificacao += QuebrarEmLinhas(38, enderecoCompleto) + "\n";

                        if (!String.IsNullOrEmpty(pedido.Cliente.Complemento))
                            identificacao += QuebrarEmLinhas(38, pedido.Cliente.Complemento) + "\n";

                        identificacao += QuebrarEmLinhas(38, pedido.Cliente.Bairro + " - " + pedido.Cliente.Cidade) + "\n";

                        if (!String.IsNullOrEmpty(pedido.Cliente.EnderecoReferencia))
                            identificacao += QuebrarEmLinhas(38, pedido.Cliente.EnderecoReferencia) + "\n";

                        identificacao += "Tel:" + pedido.Cliente.Telefone1Numero;
                    }
                    break;
                case 40:
                    identificacao += "BALCÃO";
                    break;
            }

            this.CupomNaoFiscal_Abrir(identificacao);

            List<PedidoProdutoInformation> listaProduto = pedido.ListaProduto;
            List<PedidoProdutoInformation> listaProdutoModificacao = new List<PedidoProdutoInformation>();
            List<PedidoProdutoInformation> listaProdutoTodos = new List<PedidoProdutoInformation>();

            foreach (var item in listaProduto)
            {
                if (item.ListaModificacao != null)
                    listaProdutoModificacao.AddRange(item.ListaModificacao);
            }
            listaProdutoTodos.AddRange(listaProduto);
            listaProdutoTodos.AddRange(listaProdutoModificacao);

            var listaProdutoAgrupado =
                from l in listaProdutoTodos
                where Produto.ServicoComoProduto(l.Produto)
                group l by new { l.Produto.IDProduto, l.Produto.Nome, l.Produto.CodigoAliquota, l.ValorUnitario, l.Cancelado } into g
                select new
                {
                    Codigo = g.Key.IDProduto,
                    Nome = g.Key.Nome,
                    CodigoAliquota = g.Key.CodigoAliquota,
                    ValorUnitario = g.Key.ValorUnitario,
                    Cancelado = g.Key.Cancelado,
                    Quantidade = g.Sum(x => x.Quantidade)
                };

            Decimal valor;
            foreach (var item in listaProdutoAgrupado.Where(l => l.Cancelado == false).ToList())
            {
                valor = item.ValorUnitario.Value;

                if (valor > 0)
                {
                    this.CupomNaoFiscal_RegistrarItem("", item.Nome, "", item.Quantidade.Value, valor, 0, false);
                    numeroItem++;
                }
            }

            //Decimal valorServico;
            //if (pedido.ValorServico == null)
            //    valorServico = 0;
            //else
            //    valorServico = pedido.ValorServico.Value;

            Decimal valorDesconto;
            if (pedido.ValorDesconto == null)
                valorDesconto = 0;
            else
                valorDesconto = pedido.ValorDesconto.Value;

            //Decimal total = pedido.ValorTotalProdutos + pedido.ValorServicoTemp;
            Decimal total = pedido.ValorTotalProdutosServicos;

            this.CupomNaoFiscal_Fechar(valorDesconto, 0, total, pedido.TaxaEntrega == null ? 0 : pedido.TaxaEntrega.Valor.Value);
            numCOO = this.NumCOO;

            return numCOO;
        }

        private void CupomNaoFiscal_Abrir(String identificacao)
        {
            switch (TipoGerenciadorImpressao)
            {
                case 1:
                case 4:
                    ImpressoraWin_CupomNaoFiscal_Abrir(identificacao);
                    break;
                case 2:
                    ACBr_CupomNaoFiscal_Abrir(identificacao);
                    break;
                    //case 3:
                    //    Bematech_CupomNaoFiscal_Abrir(identificacao);
                    //    break;
            }
        }
        private void CupomNaoFiscal_RegistrarItem(String codigo, String descricao, String unidade, Decimal quantidade, Decimal valor, Decimal desconto, Boolean cancelado)
        {
            switch (TipoGerenciadorImpressao)
            {
                case 1:
                case 4:
                    ImpressoraWin_CupomNaoFiscal_RegistrarItem(codigo, descricao, unidade, quantidade, valor, desconto, cancelado);
                    break;
                case 2:
                    ACBr_CupomNaoFiscal_RegistrarItem(codigo, descricao, unidade, quantidade, valor, desconto, cancelado);
                    break;
                    //case 3:
                    //    Bematech_CupomNaoFiscal_RegistrarItem(codigo, descricao, unidade, quantidade, valor, desconto, cancelado);
                    //    break;
            }
        }
        private void CupomNaoFiscal_Fechar(Decimal valorDesconto, Decimal valorAcrescimo, Decimal valorTotal, Decimal taxaEntrega)
        {
            switch (TipoGerenciadorImpressao)
            {
                case 1:
                case 4:
                    ImpressoraWin_CupomNaoFiscal_Subtotalizar(valorDesconto, valorAcrescimo, valorTotal, taxaEntrega);
                    ImpressoraWin_CupomNaoFiscal_Fechar(valorTotal - valorDesconto + valorAcrescimo + taxaEntrega, 0);
                    break;
                case 2:
                    ACBr_CupomNaoFiscal_Fechar(valorDesconto, valorAcrescimo, valorTotal, taxaEntrega);
                    break;
                    //case 3:
                    //    Bematech_CupomNaoFiscal_Fechar(valorDesconto, valorAcrescimo, valorTotal, taxaEntrega);
                    //    break;
            }
        }
        private void CupomNaoFiscal_Cancelar()
        {
            switch (TipoGerenciadorImpressao)
            {
                case 1:
                case 4:
                    ImpressoraWin_CupomNaoFiscal_Cancelar();
                    break;
                case 2:
                    ACBr_CupomNaoFiscal_Cancelar();
                    break;
                    //case 3:
                    //    Bematech_CupomNaoFiscal_Cancelar();
                    //    break;
            }
        }
        #endregion

        #region Comandos Fiscais
        public void LeituraX()
        {
            switch (TipoGerenciadorImpressao)
            {
                case 1:
                    break;
                case 2:
                    ACBr_LeituraX();
                    break;
                    //case 3:
                    //    Bematech_LeituraX();
                    //    break;
            }
        }
        public void ReducaoZ()
        {
            switch (TipoGerenciadorImpressao)
            {
                case 1:
                    break;
                case 2:
                    ACBr_ReducaoZ();
                    break;
                    //case 3:
                    //    Bematech_ReducaoZ();
                    //    break;
            }
        }

        #endregion

        #region Relatorios Não Fiscais
        public void RelatorioGerencial(List<String> relatorio, int? tamanhoFonte = null)
        {
            switch (TipoGerenciadorImpressao)
            {
                case 1:
                case 4:
                    if (ImpressoraAtiva)
                        ImpressoraWindows.ImprimirTexto(ModeloImpressora, true, relatorio, tamanhoFonte);

                    break;
                case 2:
                    ABCr_RelatorioGerencial(relatorio);
                    break;
                    //case 3:
                    //    Bematech_RelatorioGerencial(relatorio);
                    //    break;
            }
        }
        #endregion

        #region ACBr
        private void ACBr_LeituraX()
        {
            ImpressoraACBrECF.LeituraX();
        }
        private void ACBr_ReducaoZ()
        {
            ImpressoraACBrECF.ReducaoZ();
        }

        private void ABCr_RelatorioGerencial(List<string> relatorio)
        {
            string relatorio2 = "";

            foreach (var item in relatorio)
                relatorio2 += "\n" + item;

            ImpressoraACBrECF.RelatorioGerencial(relatorio2.Split('\n'));
        }

        private EstadoECF ACBr_Estado()
        {
            return ImpressoraACBrECF.Estado;
        }

        private void ACBr_CupomFiscal_Abrir(string identificacao, string documento, string cliente)
        {
            ImpressoraACBrECF.AbreCupom(documento, cliente, "", false);
            NumCOO = ImpressoraACBrECF.NumCOO;
        }
        private void ACBr_CupomFiscal_RegistrarItem(Int32 numeroItem, String codigo, String descricao, Decimal quantidade, Decimal valor, String codigoAliquota, Decimal desconto, Boolean cancelado)
        {
            ImpressoraACBrECF.VendeItem(codigo, descricao, codigoAliquota, quantidade, valor);
            // ImpressoraACBrECF.VendeItemEx(codigo, descricao, codigoAliquota, quantidade, valor, ArredondaTrunca: 'T');

            if (desconto > 0)
                ImpressoraACBrECF.DescontoAcrescimoItemAnterior(valor, "desconto");

            if (cancelado == true)
                ImpressoraACBrECF.CancelaItemVendido(numeroItem);
        }
        private void ACBr_CupomFiscal_Subtotalizar(Decimal desconto, Decimal acrescimo)
        {
            if (acrescimo > 0)
                ImpressoraACBrECF.SubtotalizaCupom(acrescimo);
            else if (desconto > 0)
                ImpressoraACBrECF.SubtotalizaCupom(-desconto);
            else
                ImpressoraACBrECF.SubtotalizaCupom();
        }
        private void ACBr_CupomFiscal_RegistrarPagamento(string codigoPagamento, decimal valor)
        {
            ImpressoraACBrECF.EfetuaPagamento(codigoPagamento, valor);
        }
        private void ACBr_CupomFiscal_Fechar()
        {
            String msgCupom = ConfiguracoesSistema.Valores.MsgCupom;

            ImpressoraACBrECF.FechaCupom(msgCupom);
            NumCOO = ImpressoraACBrECF.NumCOO;
        }
        private void ACBr_CupomFiscal_Cancelar()
        {
            ImpressoraACBrECF.CancelaCupom();
        }

        private void ACBr_CupomNaoFiscal_Abrir(String identificacao)
        {
            NumCOO = ImpressoraACBrECF.NumCOO;
            Cupom = new List<String>();

            String linha;

            //46 caracteres
            linha = "******************* CONTA ********************\n";
            linha += "************ DOCUMENTO NAO FISCAL ************\n\n";
            Cupom.Add(linha);

            linha = identificacao + "\n";
            Cupom.Add(linha);

            linha = "ITEM".PadRight(TamanhoItem, ' ');
            linha += "QTD".PadLeft(3, ' ');
            linha += "VL UNIT".PadLeft(8, ' ');
            linha += "VL ITEM".PadLeft(8, ' ');

            Cupom.Add(linha);
        }
        private void ACBr_CupomNaoFiscal_RegistrarItem(String codigo, String descricao, String unidade, Decimal quantidade, Decimal valor, Decimal desconto, Boolean cancelado)
        {
            String linha;

            if (cancelado != true)
            {
                linha = descricao.PadRight(TamanhoItem, ' ').Substring(0, TamanhoItem);
                linha += quantidade.ToString("0").PadLeft(3, ' ');
                linha += ("$" + (valor - desconto).ToString("#,##0.00")).PadLeft(8, ' ');
                linha += ("$" + (quantidade * (valor - desconto)).ToString("#,##0.00")).PadLeft(8, ' ');

                //if (descricao.Length > TamanhoItem)
                //    linha += "\n   " + descricao.PadRight(50, ' ').Substring(TamanhoItem, 10);

                Cupom.Add(linha);
            }
        }
        private void ACBr_CupomNaoFiscal_Subtotalizar(Decimal desconto, Decimal acrescimo, Decimal valorTotal)
        {
            String linha = "\n";

            if (acrescimo > 0 || desconto > 0)
                linha += "Sub-total: R$ " + valorTotal.ToString("#,##0.00") + "\n";

            if (acrescimo > 0)
                linha += "Serviço: R$ " + acrescimo.ToString("#,##0.00") + "\n";

            if (desconto > 0)
                linha += "Desconto: R$ " + desconto.ToString("#,##0.00") + "\n";

            linha += "Valor total: R$ " + (valorTotal - desconto + acrescimo).ToString("#,##0.00") + "\n";

            Cupom.Add(linha);
        }
        private void ACBr_CupomNaoFiscal_Fechar()
        {
            ABCr_RelatorioGerencial(Cupom);
        }
        private void ACBr_CupomNaoFiscal_Fechar(Decimal valorDesconto, Decimal valorAcrescimo, Decimal valorTotal, Decimal taxaEntrega)
        {
            String linha = "\n";

            linha += "Subtotal: R$ " + valorTotal.ToString("#,##0.00") + "\n";

            if (valorAcrescimo > 0)
                linha += "Serviço: R$ " + valorAcrescimo.ToString("#,##0.00") + "\n";

            if (valorDesconto > 0)
                linha += "Desconto: R$ " + valorDesconto.ToString("#,##0.00") + "\n";

            if (taxaEntrega > 0)
                linha += "Taxa de entrega: R$ " + taxaEntrega.ToString("#,##0.00") + "\n";

            linha += "Valor total: R$ " + (valorTotal + taxaEntrega + valorAcrescimo - valorDesconto).ToString("#,##0.00") + "\n";
            Cupom.Add(linha);

            linha = "\n\n";
            linha += "*****************************************\n";
            linha += "***** PDVSeven  www.pdvseven.com.br *****\n";
            linha += "*****************************************\n";

            Cupom.Add(linha);

            ABCr_RelatorioGerencial(Cupom);
        }

        private void ACBr_CupomNaoFiscal_Cancelar()
        {
            ImpressoraACBrECF.FechaRelatorio();
        }

        private void ACBr_OnMsgPoucoPapel(object sender, EventArgs e)
        {
            //MessageBox.Show("Pouco papel!");
        }
        #endregion

        /*
        #region Bemafi
        private void Bematech_LeituraX()
        {
            BemaFI32.Bematech_FI_LeituraX();
        }
        private void Bematech_ReducaoZ()
        {
            BemaFI32.Bematech_FI_ReducaoZ("", "");
        }

        private void Bematech_RelatorioGerencial(List<string> relatorio)
        {
            BemaFI32.Bematech_FI_RelatorioGerencial("\n");

            foreach (var item in relatorio)
                BemaFI32.Bematech_FI_UsaComprovanteNaoFiscalVinculado(item);

            BemaFI32.Bematech_FI_FechaComprovanteNaoFiscalVinculado();
        }

        private EstadoECF Bematech_Estado()
        {
            EstadoECF estadoECF = EstadoECF.Desconhecido;

            Int32 iRetorno;
            Int32 ack = 0;
            Int32 st1 = 0;
            Int32 st2 = 0;

            String msg = "";
            String erros = "";
            string MSGCaption = "Atenção";
            MessageBoxIcon MSGIco = MessageBoxIcon.Information;

            iRetorno = BemaFI32.Bematech_FI_VerificaEstadoImpressora(ref ack, ref st1, ref st2);

            switch (iRetorno)
            {
                case 0:
                    msg = "Erro de Comunicação !";
                    MSGCaption = "Erro";
                    MSGIco = MessageBoxIcon.Error;
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case 1:
                    String statusRZ = new String(' ', 1);
                    iRetorno = BemaFI32.Bematech_FI_VerificaZPendente(ref statusRZ);
                    if (statusRZ == "1")
                    {
                        estadoECF = EstadoECF.RequerZ;
                    }
                    else
                    {
                        estadoECF = EstadoECF.Livre;
                    }
                    break;
                case -1:
                    msg = "Erro de Execução na Função. Verifique!";
                    MSGCaption = "Erro";
                    MSGIco = MessageBoxIcon.Error;
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -2:
                    msg = "Parâmetro Inválido !";
                    MSGCaption = "Erro";
                    MSGIco = MessageBoxIcon.Error;
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -3:
                    msg = "Alíquota não programada !";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -4:
                    msg = "Arquivo BemaFI32.INI não encontrado. Verifique!";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -5:
                    msg = "Erro ao Abrir a Porta de Comunicação";
                    MSGCaption = "Erro";
                    MSGIco = MessageBoxIcon.Error;
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -6:
                    msg = "Impressora Desligada ou Desconectada.";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -7:
                    msg = "Banco Não Cadastrado no Arquivo BemaFI32.ini";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -8:
                    msg = "Erro ao Criar ou Gravar no Arquivo Retorno.txt ou Status.txt.";
                    MSGCaption = "Erro";
                    MSGIco = MessageBoxIcon.Error;
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -18:
                    msg = "Não foi possível abrir arquivo INTPOS.001!";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -19:
                    msg = "Parâmetros diferentes!";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -20:
                    msg = "Transação cancelada pelo Operador!";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -21:
                    msg = "A Transação não foi aprovada!";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -22:
                    msg = "Não foi possível terminar a Impressão!";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -23:
                    msg = "Não foi possível terminar a Operação!";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -24:
                    msg = "Não foi possível terminal a Operação!";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -25:
                    msg = "Totalizador não fiscal não programado.";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -26:
                    msg = "Transação já Efetuada!";
                    estadoECF = EstadoECF.Desconhecido;
                    break;
                case -27:

                    #region Tratando o ST1
                    if (st1 >= 128)
                    {
                        st1 = st1 - 128;
                        erros += "BIT 7 - Fim de Papel" + '\x0D';
                        estadoECF = EstadoECF.Bloqueada;
                    }
                    if (st1 >= 64)
                    {
                        st1 = st1 - 64;
                        erros += "BIT 6 - Pouco Papel" + '\x0D';
                        estadoECF = EstadoECF.Livre;
                    }
                    if (st1 >= 32)
                    {
                        st1 = st1 - 32;
                        erros += "BIT 5 - Erro no Relógio" + '\x0D';
                        estadoECF = EstadoECF.Bloqueada;
                    }
                    if (st1 >= 16)
                    {
                        st1 = st1 - 16;
                        erros += "BIT 4 - Impressora em ERRO" + '\x0D';
                        estadoECF = EstadoECF.Desconhecido;
                    }
                    if (st1 >= 8)
                    {
                        st1 = st1 - 8;
                        erros += "BIT 3 - CMD não iniciado com ESC" + '\x0D';
                        estadoECF = EstadoECF.Desconhecido;
                    }
                    if (st1 >= 4)
                    {
                        st1 = st1 - 4;
                        erros += "BIT 2 - Comando Inexistente" + '\x0D';
                        estadoECF = EstadoECF.Desconhecido;
                    }
                    if (st1 >= 2)
                    {
                        st1 = st1 - 2;
                        erros += "BIT 1 - Cupom Aberto" + '\x0D';
                        estadoECF = EstadoECF.Venda;
                    }
                    if (st1 >= 1)
                    {
                        st1 = st1 - 1;
                        erros += "BIT 0 - Nº de Parâmetros Inválidos" + '\x0D';
                        estadoECF = EstadoECF.Desconhecido;
                    }
                    #endregion

                    #region Tratando o ST2
                    if (st2 >= 128)
                    {
                        st2 = st2 - 128;
                        erros += "BIT 7 - Tipo de Parâmetro Inválido" + '\x0D';
                        estadoECF = EstadoECF.Desconhecido;
                    }
                    if (st2 >= 64)
                    {
                        st2 = st2 - 64;
                        erros += "BIT 6 - Memória Fiscal Lotada" + '\x0D';
                        estadoECF = EstadoECF.Bloqueada;
                    }
                    if (st2 >= 32)
                    {
                        st2 = st2 - 32;
                        erros += "BIT 5 - CMOS não Volátil" + '\x0D';
                        estadoECF = EstadoECF.Bloqueada;
                    }
                    if (st2 >= 16)
                    {
                        st2 = st2 - 16;
                        erros += "BIT 4 - Alíquota Não Programada" + '\x0D';
                        estadoECF = EstadoECF.Bloqueada;
                    }
                    if (st2 >= 8)
                    {
                        st2 = st2 - 8;
                        erros += "BIT 3 - Alíquotas lotadas" + '\x0D';
                        estadoECF = EstadoECF.Bloqueada;
                    }
                    if (st2 >= 4)
                    {
                        st2 = st2 - 4;
                        erros += "BIT 2 - Cancelamento ñ Permitido" + '\x0D';
                        estadoECF = EstadoECF.Bloqueada;
                    }
                    if (st2 >= 2)
                    {
                        st2 = st2 - 2;
                        erros += "BIT 1 - CGC/IE não Programados" + '\x0D';
                        estadoECF = EstadoECF.Bloqueada;
                    }
                    if (st2 >= 1)
                    {
                        st2 = st2 - 1;
                        erros += "BIT 0 - Comando não Executado" + '\x0D';
                        estadoECF = EstadoECF.Desconhecido;
                    }

                    #endregion

                    break;
                case -28:
                    msg = "Não há Informações para serem Impressas!";
                    estadoECF = EstadoECF.Bloqueada;
                    break;
            }

            if (msg.Length != 0 && MSGIco == MessageBoxIcon.Error)
            {
                throw new Exception(msg);
            }
            else if (msg.Length != 0)
            {
                System.Windows.Forms.MessageBox.Show(msg, MSGCaption, MessageBoxButtons.OK, MSGIco);
            }

            return estadoECF;
        }

        private void Bematech_CupomFiscal_Abrir(string identificacao, string documento, string cliente)
        {
            if (documento != "")
            {
                BemaFI32.Bematech_FI_AbreCupom(documento);
            }
            else
            {
                BemaFI32.Bematech_FI_AbreCupom("");
            }

            String numeroCupom = new String(' ', 6);

            BemaFI32.Bematech_FI_NumeroCupom(ref numeroCupom);

            NumCOO = numeroCupom;
        }
        private void Bematech_CupomFiscal_RegistrarItem(Int32 numeroItem, String codigo, String descricao, Decimal quantidade, Decimal valor, String codigoAliquota, Decimal desconto, Boolean cancelado)
        {
            BemaFI32.Bematech_FI_VendeItem(codigo, descricao.PadRight(28, ' ').Substring(0, 28), codigoAliquota, "I", Convert.ToInt32(quantidade).ToString(), 2, valor.ToString("0.00"), "$", desconto.ToString("0.00"));

            if (cancelado == true)
            {
                BemaFI32.Bematech_FI_CancelaItemAnterior();
            }
        }
        private void Bematech_CupomFiscal_Subtotalizar(Decimal? acrescimo, Decimal? desconto)
        {
            if (acrescimo != null && desconto.Value > 0)
            {
                BemaFI32.Bematech_FI_IniciaFechamentoCupom("A", "$", acrescimo.Value.ToString("0.00"));
            }
            else if (desconto != null && desconto > 0)
            {
                BemaFI32.Bematech_FI_IniciaFechamentoCupom("D", "$", desconto.Value.ToString("0.00"));
            }
            else
            {
                BemaFI32.Bematech_FI_IniciaFechamentoCupom("A", "$", "0");
            }
        }
        private void Bematech_CupomFiscal_RegistrarPagamento(String codigoPagamento, Decimal valor)
        {
            BemaFI32.Bematech_FI_EfetuaFormaPagamento(codigoPagamento, valor.ToString("0.00"));
        }
        private void Bematech_CupomFiscal_Fechar()
        {
            String msgCupom = ConfiguracoesSistema.Valores.MsgCupom;
            BemaFI32.Bematech_FI_TerminaFechamentoCupom(msgCupom);
        }
        private void Bematech_CupomFiscal_Cancelar()
        {
            BemaFI32.Bematech_FI_CancelaCupom();
        }

        private void Bematech_CupomNaoFiscal_Abrir(String identificacao)
        {
            String linha;

            BemaFI32.Bematech_FI_RelatorioGerencial("");

            linha = "************** CONTA ***************\n";
            BemaFI32.Bematech_FI_UsaComprovanteNaoFiscalVinculado(linha);

            linha = identificacao + "\n";
            BemaFI32.Bematech_FI_UsaComprovanteNaoFiscalVinculado(linha);

            linha = "ITEM".PadRight(TamanhoItem, ' ');
            linha += "QTD".PadLeft(6, ' ');
            linha += "VL UNIT".PadLeft(9, ' ');
            linha += "VL ITEM".PadLeft(9, ' ');
            BemaFI32.Bematech_FI_UsaComprovanteNaoFiscalVinculado(linha);
        }
        private void Bematech_CupomNaoFiscal_RegistrarItem(String codigo, String descricao, String unidade, Decimal quantidade, Decimal valor, Decimal desconto, Boolean cancelado)
        {
            String linha;

            linha = descricao.PadRight(TamanhoItem, ' ').Substring(0, TamanhoItem);
            linha += quantidade.ToString("0.00").PadLeft(6, ' ');
            linha += ("$" + (valor - desconto).ToString("#,##0.00")).PadLeft(9, ' ');
            linha += ("$" + (quantidade * (valor - desconto)).ToString("#,##0.00")).PadLeft(9, ' ');

            if (descricao.Length > TamanhoItem)
                linha += "\n   " + descricao.PadRight(TamanhoItem, ' ').Substring(TamanhoItem);

            BemaFI32.Bematech_FI_RelatorioGerencial(linha + "\n");
        }
        private void Bematech_CupomNaoFiscal_Fechar(Decimal valorDesconto, Decimal valorAcrescimo, Decimal valorTotal, Decimal taxaEntrega)
        {
            String linha = "\n\n";

            if (taxaEntrega > 0)
                linha += "Taxa de entrega: R$ " + taxaEntrega.ToString("#,##0.00") + "\n";

            if (valorAcrescimo > 0)
            {
                linha += "Acrescimo: R$ " + valorAcrescimo.ToString("#,##0.00") + "\n";
                linha += "Valor total: R$ " + valorTotal.ToString("#,##0.00") + "\n";
                BemaFI32.Bematech_FI_RelatorioGerencial(linha + "\n");
            }
            else if (valorDesconto > 0)
            {
                linha += "Desconto: R$ " + valorDesconto.ToString("#,##0.00") + "\n";
                linha += "Valor total: R$ " + valorTotal.ToString("#,##0.00") + "\n";
                BemaFI32.Bematech_FI_RelatorioGerencial(linha + "\n");
            }
            else
            {
                linha += "Valor total: R$ " + valorTotal.ToString("#,##0.00") + "\n";
                BemaFI32.Bematech_FI_RelatorioGerencial(linha + "\n");
            }

            BemaFI32.Bematech_FI_FechaRelatorioGerencial();
        }
        private void Bematech_CupomNaoFiscal_Cancelar()
        {
            BemaFI32.Bematech_FI_FechaComprovanteNaoFiscalVinculado();
        }
        #endregion
        */

        #region Impressora Windows
        private void ImpressoraWin_CupomNaoFiscal_Abrir(String identificacao)
        {
            Cupom = new List<String>();

            String linha;

            //32 caracteres
            linha = "************* CONTA *************\n";
            linha += "***** DOCUMENTO NÃO FISCAL ******\n\n";

            linha += identificacao + "\n\n\n";

            linha += "ITEM".PadRight(TamanhoItem, ' ');
            linha += "QTD".PadLeft(3, ' ');
            linha += "VL UNIT".PadLeft(8, ' ');
            linha += "VL ITEM".PadLeft(8, ' ');

            Cupom.Add(linha);

            NumCOO = "";
        }
        private void ImpressoraWin_CupomNaoFiscal_RegistrarItem(String codigo, String descricao, String unidade, Decimal quantidade, Decimal valor, Decimal desconto, Boolean cancelado)
        {
            String linha;

            if (cancelado != true)
            {
                linha = descricao.PadRight(TamanhoItem, ' ').Substring(0, TamanhoItem);
                linha += quantidade.ToString("0").PadLeft(3, ' ');
                linha += ("$" + (valor - desconto).ToString("#,##0.00")).PadLeft(8, ' ');
                linha += ("$" + (quantidade * (valor - desconto)).ToString("#,##0.00")).PadLeft(8, ' ');

                //if (descricao.Length > 14)
                //    linha += "\n -" + descricao.PadRight(50, ' ').Substring(14, 10);

                Cupom.Add(linha);
            }
        }
        private void ImpressoraWin_CupomNaoFiscal_RegistrarPagamento(string codigoPagamento, decimal valor)
        {
            String linha = codigoPagamento + ": R$ " + valor.ToString("#,##0.00");
            Cupom.Add(linha);
        }
        private void ImpressoraWin_CupomNaoFiscal_Subtotalizar(Decimal valorDesconto, Decimal valorAcrescimo, Decimal valorTotal, Decimal taxaEntrega)
        {
            String linha = "\n";

            if (valorAcrescimo > 0 || valorDesconto > 0)
                linha += "Sub-total: R$ " + valorTotal.ToString("#,##0.00") + "\n";

            if (valorAcrescimo > 0)
                linha += "Serviço: R$ " + valorAcrescimo.ToString("#,##0.00") + "\n";

            if (valorDesconto > 0)
                linha += "Desconto: R$ " + valorDesconto.ToString("#,##0.00") + "\n";

            if (taxaEntrega > 0)
                linha += "Taxa de entrega: R$ " + taxaEntrega.ToString("#,##0.00") + "\n";

            linha += "Valor total: R$ " + (valorTotal - valorDesconto + valorAcrescimo + taxaEntrega).ToString("#,##0.00") + "\n";

            Cupom.Add(linha);
        }

        private void ImpressoraWin_CupomNaoFiscal_Fechar(Decimal valorTotal, Decimal valorTotalPago)
        {
            String linha = "\n";

            if (valorTotalPago > 0)
            {
                linha += "Total pago: R$ " + valorTotalPago.ToString("#,##0.00") + "\n";

                if (valorTotal != valorTotalPago)
                    linha += "Troco: R$ " + (valorTotalPago - valorTotal).ToString("#,##0.00") + "\n";
            }

            Cupom.Add(linha);

            linha = "\n*********************************\n";
            linha += "* PDVSeven  www.pdvseven.com.br *\n";
            linha += "*********************************\n";
            Cupom.Add(linha);

            ImpressoraWindows.ImprimirTexto(ModeloImpressora, false, Cupom);
        }

        private void ImpressoraWin_CupomNaoFiscal_Cancelar()
        {
            String conteudo = "\n\n**** CUPOM CANCELADO ****\n\n";
            Cupom.Add(conteudo);

            ImpressoraWindows.ImprimirTexto(ModeloImpressora, false, Cupom);

            Cupom = null;
        }

        #endregion

        private string QuebrarEmLinhas(int qtColunas, string texto)
        {
            return texto.QuebrarEmLinhas(qtColunas);
        }

        public void GerarTicketPrePago(PedidoInformation pedido)
        {
            int numeroItem = 1;
            foreach (var item in pedido.ListaProduto)
            {
                if (TipoGerenciadorImpressao == 1 || TipoGerenciadorImpressao == 4)
                {
                    var bmp = TiketServices.Gerar(pedido, numeroItem - 1);
                    ImpressoraWindows.ImprimirImagem(bmp, ModeloImpressora);
                }
                else
                {
                    var relatorio = TiketServices.TicketPrePago(pedido, item, numeroItem);
                    RelatorioGerencial(relatorio);
                }
                numeroItem++;
            }
        }
    }
}
