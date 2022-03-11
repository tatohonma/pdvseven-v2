using a7D.PDV.BLL;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using System;
using System.Linq;

namespace a7D.PDV.Integracao.iFood
{
    public static class DTO
    {
        public static string CriaPedido(PedidoIFood pedidoIFood, out int idPedido, PDVInformation vendaPDV, CaixaInformation ifoodCaixa, UsuarioInformation usuario, int pagamentoIFood, int pagamentoDinheiro, int pagamentoDebito, int pagamentoCredito, int pagamentoRefeicao, int pagamentoOutros, int taxaEntrega)
        {
            idPedido = 0;
            var pedidoPDV = IntegracaoPedido.CarregarPorIdentificacao("ifood#" + pedidoIFood.reference);
            if (pedidoPDV.IDPedido != null)
                return ""; //  $"Pedido {pedidoPDV.IDPedido} {pedidoPDV.GUIDAgrupamentoPedido} já inserido";

            //var ifoodCaixa = Caixa.UsaOuAbre(caixaPDV.IDPDV.Value, usuario.IDUsuario.Value, new int[] { pagamentoIFood, pagamentoDinheiro, pagamentoDebito, pagamentoCredito, pagamentoRefeicao, pagamentoOutros });
            pedidoPDV = IntegracaoPedido.NovoPedidoDelivery(ifoodCaixa);

            RelacionarCliente(pedidoPDV, pedidoIFood);

            pedidoPDV.TaxaEntrega = new TaxaEntregaInformation() { IDTaxaEntrega = taxaEntrega };
            pedidoPDV.DocumentoCliente = pedidoIFood.customer.taxPayerIdentificationNumber;
            pedidoPDV.GUIDIdentificacao = "ifood#" + pedidoIFood.reference;
            pedidoPDV.GUIDAgrupamentoPedido = "ifood#" + pedidoIFood.shortReference;
            pedidoPDV.ValorEntrega = pedidoIFood.deliveryFee;
            pedidoPDV.ValorTotal = pedidoIFood.totalPrice;
            pedidoPDV.StatusPedido.StatusPedido = EStatusPedido.NaoConfirmado;
            pedidoPDV.Observacoes = pedidoIFood.ToString();

            if ((pedidoIFood.customer?.ordersCountOnRestaurant ?? 0) == 0)
                pedidoPDV.ObservacaoCupom = "NOVO CLIENTE IFOOD";
            else if (pedidoIFood.customer?.ordersCountOnRestaurant > 0)
                pedidoPDV.ObservacaoCupom = "FIDELIDADE " + pedidoIFood.customer?.ordersCountOnRestaurant.ToString();

            IntegracaoPedido.Salvar(pedidoPDV);
            idPedido = pedidoPDV.IDPedido.Value;

            string log = $"OK Pedido {pedidoPDV.IDPedido} - {pedidoPDV.Cliente.IDCliente}: {pedidoPDV.Cliente.NomeCompleto}\r\n{pedidoPDV.Observacoes}";

            AdicionaPagamentos(pedidoPDV, pedidoIFood, usuario, pagamentoIFood, pagamentoDinheiro, pagamentoDebito, pagamentoCredito, pagamentoRefeicao, pagamentoOutros);
            log += AdicionaItens(pedidoPDV, pedidoIFood.items, null, vendaPDV, usuario);

            if (ConfiguracaoBD.ValorOuPadrao(EConfig.AprovarIFood, vendaPDV) == "1")
            {
                try
                {
                    pedidoPDV = IntegracaoPedido.CarregarCompleto(pedidoPDV.IDPedido.Value);
                    OrdemProducaoServices.GerarOrdemProducao(pedidoPDV.ListaProduto, false);
                    log += "\r\nConfirmado Automaticamente e Ordem de Produção Gerada";
                    pedidoPDV.StatusPedido.StatusPedido = EStatusPedido.Aberto;
                    IntegracaoPedido.Salvar(pedidoPDV);

                    if (ConfiguracoesSistema.Valores.ImprimirViaExpedicao == "NOVO") // IFOOD - Aprovação automatica
                        OrdemProducaoServices.GerarViaExpedicao(pedidoPDV.IDPedido.Value, ConfiguracoesSistema.Valores.IDAreaViaExpedicao);
                }
                catch (Exception ex)
                {
                    log += "\r\nErro ao gerar a Ordem de Produção";
                    Logs.Erro(ex);
                }
            }

            return log;
        }

        private static void RelacionarCliente(PedidoInformation pedidoPDV, PedidoIFood pedidoIFood)
        {
            try
            {
                string telefone = ObtemDDDTelefone(pedidoIFood.customer.phone, out int telefone1DDD, out int telefone1Numero);
                string iFoodID = $"IFood #{pedidoIFood.customer.id} ";
                string notas = $"Cliente " + iFoodID +
                    "\r\nTelefone: " + pedidoIFood.customer.phone;

                var clienteNovo = new tbCliente()
                {
                    NomeCompleto = pedidoIFood.customer.name,
                    Documento1 = pedidoIFood.customer.taxPayerIdentificationNumber,
                    Email = pedidoIFood.customer.email,
                    Telefone1DDD = telefone1DDD,
                    Telefone1Numero = telefone1Numero,
                    Endereco = pedidoIFood.deliveryAddress.streetName,
                    EnderecoNumero = pedidoIFood.deliveryAddress.streetNumber,
                    Complemento = pedidoIFood.deliveryAddress.complement,
                    EnderecoReferencia = pedidoIFood.deliveryAddress.reference,
                    Cidade = pedidoIFood.deliveryAddress.city,
                    Bairro = pedidoIFood.deliveryAddress.neighborhood,
                    IDEstado = Estado.Listar().FirstOrDefault(e => e.Sigla == pedidoIFood.deliveryAddress.state)?.IDEstado ?? 25,
                    CEP = int.Parse(pedidoIFood.deliveryAddress.postalCode),
                    Observacao = notas,
                    Telefone2Numero = 0,
                };

                tbCliente cliente;

                if (!string.IsNullOrEmpty(pedidoIFood.customer.taxPayerIdentificationNumber))
                    cliente = Cliente.BuscarCliente(Cliente.TipoCliente.CPFCNPJ, pedidoIFood.customer.taxPayerIdentificationNumber, clienteNovo, pedidoIFood.customer.id);
                else
                    cliente = Cliente.BuscarCliente(Cliente.TipoCliente.TELEFONE, telefone, clienteNovo, pedidoIFood.customer.id);

                //EventoMensagem.Invoke($"{cliente.IDCliente}: {cliente.NomeCompleto} ({cliente.Telefone1})");

                // Atualiza o endereço do cliente
                if (cliente.Endereco != clienteNovo.Endereco
                 || cliente.EnderecoNumero != clienteNovo.EnderecoNumero
                 || cliente.Complemento != clienteNovo.EnderecoCompleto
                 || cliente.EnderecoReferencia != clienteNovo.EnderecoReferencia
                 || cliente.Cidade != clienteNovo.Cidade
                 || cliente.Bairro != clienteNovo.Bairro
                 || cliente.IDEstado != clienteNovo.IDEstado
                 || cliente.CEP != clienteNovo.CEP
                 || cliente.Observacao?.Contains(iFoodID) == false)
                {
                    Logs.Info(CodigoInfo.I006,
                        "Cliente " + cliente.IDCliente + " atualizado pelo iFood em " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") +
                        " dados originais: " + cliente.EnderecoCompleto);

                    cliente.Endereco = clienteNovo.Endereco;
                    cliente.EnderecoNumero = clienteNovo.EnderecoNumero;
                    cliente.Complemento = clienteNovo.Complemento;
                    cliente.EnderecoReferencia = clienteNovo.EnderecoReferencia;
                    cliente.Cidade = clienteNovo.Cidade;
                    cliente.IDEstado = clienteNovo.IDEstado;
                    cliente.Bairro = clienteNovo.Bairro;
                    cliente.CEP = clienteNovo.CEP;
                    cliente.Observacao = cliente.Observacao ?? "\r\n" + iFoodID;
                    EF.Repositorio.Atualizar(cliente);
                }

                pedidoPDV.Cliente = Cliente.Carregar(cliente.IDCliente);
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EE14, ex);
            }
        }

        private static void AdicionaPagamentos(PedidoInformation pedidoPDV, PedidoIFood pedidoIFood, UsuarioInformation usuario,
            int pagamentoIFood, int pagamentoDinheiro, int pagamentoDebito, int pagamentoCredito, int pagamentoRefeicao, int pagamentoOutro)
        {
            try
            {
                foreach (var pag in pedidoIFood.payments) //.Where(p => p.prepaid || p.code == "DIN"))
                {
                    int meioPagamento = TraduzCodePag(pag.code);
                    int tipoPagamento;
                    tbContaRecebivel conta = null;
                    if (pag.prepaid)
                    {
                        tipoPagamento = pagamentoIFood; // O PAGAMENTO FOI FEITO ONLINE!
                        conta = ContaRecebivel.Carregar((int)EContaRecebivel.iFood);
                    }

                    else if (meioPagamento == (int)EMetodoPagamento.Dinheiro)
                        tipoPagamento = pagamentoDinheiro;

                    else if (meioPagamento == (int)EMetodoPagamento.Debito)
                        tipoPagamento = pagamentoDebito;

                    else if (meioPagamento == (int)EMetodoPagamento.Credito)
                        tipoPagamento = pagamentoCredito;

                    else if (meioPagamento == (int)EMetodoPagamento.Refeicao)
                        tipoPagamento = pagamentoRefeicao;

                    else
                        tipoPagamento = pagamentoOutro;

                    var bandeira = Bandeira.CarregarPorNome(pag.issuer);

                    PedidoPagamento.Adicionar(new PedidoPagamentoInformation()
                    {
                        Pedido = pedidoPDV,
                        TipoPagamento = new TipoPagamentoInformation() { IDTipoPagamento = tipoPagamento },
                        MeioPagamentoSAT = new MeioPagamentoSATInformation() { IDMeioPagamentoSAT = meioPagamento },
                        ContaRecebivel = conta,
                        IDGateway = (int)EGateway.iFood,
                        Bandeira = bandeira,
                        UsuarioPagamento = usuario,
                        Valor = pag.changeFor > 0 ? pag.changeFor.Value : pag.value,
                        Excluido = false,
                        DataPagamento = DateTime.Now,
                        Autorizacao = meioPagamento == (int)EMetodoPagamento.Dinheiro ? "" : pag.name
                    });
                }
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EE16, ex);
            }
        }

        private static int TraduzCodePag(string code)
        {
            // {"cards":[{"option":{"key":"CRÉDITO - AMERICAN EXPRESS (MÁQUINA)","value":"RAM"},"active":true},{"option":{"key":"CRÉDITO - BANRICOMPRAS (MÁQUINA)","value":"BANRC"},"active":true},{"option":{"key":"CRÉDITO - DINERS (MÁQUINA)","value":"DNREST"},"active":true},{"option":{"key":"CRÉDITO - ELO (MÁQUINA)","value":"REC"},"active":true},{"option":{"key":"CRÉDITO - GOODCARD (MÁQUINA)","value":"GOODC"},"active":true},{"option":{"key":"CRÉDITO - HIPERCARD (MÁQUINA)","value":"RHIP"},"active":true},{"option":{"key":"CRÉDITO - MASTERCARD (MÁQUINA)","value":"RDREST"},"active":true},{"option":{"key":"CRÉDITO - VERDECARD (MÁQUINA)","value":"VERDEC"},"active":true},{"option":{"key":"CRÉDITO - VISA (MÁQUINA)","value":"VSREST"},"active":true},{"option":{"key":"DÉBITO - BANRICOMPRAS (MÁQUINA)","value":"BANRD"},"active":true},{"option":{"key":"DÉBITO - ELO (MÁQUINA)","value":"RED"},"active":true},{"option":{"key":"DÉBITO - MASTERCARD (MÁQUINA)","value":"MEREST"},"active":true},{"option":{"key":"DÉBITO - VISA (MÁQUINA)","value":"VIREST"},"active":true}],"vouchers":[{"option":{"key":"VALE - ALELO REFEIÇÃO / VISA VALE (CARTÃO)","value":"VVREST"},"active":true},{"option":{"key":"VALE - COOPER CARD (CARTÃO)","value":"CPRCAR"},"active":true},{"option":{"key":"VALE - GREEN CARD (CARTÃO)","value":"GRNCAR"},"active":true},{"option":{"key":"VALE - GREEN CARD (PAPEL)","value":"GRNCPL"},"active":true},{"option":{"key":"VALE - REFEISUL (CARTÃO)","value":"RSELE"},"active":true},{"option":{"key":"VALE - SODEXO (CARTÃO)","value":"RSODEX"},"active":true},{"option":{"key":"VALE - TICKET RESTAURANTE (CARTÃO)","value":"TRE"},"active":true},{"option":{"key":"VALE - VALE CARD","value":"VALECA"},"active":true},{"option":{"key":"VALE - VEROCARD (CARTÃO)","value":"TVER"},"active":true},{"option":{"key":"VALE - VR SMART (CARTÃO)","value":"VR_SMA"},"active":true}],"others":[{"option":{"key":"CHEQUE","value":"CHE"},"active":true},{"option":{"key":"DINHEIRO","value":"DIN"},"active":true}]}
            switch (code)
            {
                case "DIN":
                    return (int)EMetodoPagamento.Dinheiro;
                case "RAM":
                case "BANRC":
                case "DNREST":
                case "REC":
                case "GOODC":
                case "RHIP":
                case "RDREST":
                case "VERDEC":
                case "VSREST":
                    return (int)EMetodoPagamento.Credito;
                case "BANRD":
                case "RED":
                case "MEREST":
                case "VIREST":
                    return (int)EMetodoPagamento.Debito;
                case "VVREST":
                case "CPRCAR":
                case "GRNCAR":
                case "GRNCPL":
                case "RSELE":
                case "TRE":
                case "RSODEX":
                case "VALECA":
                case "VR_SMA":
                case "TVER":
                    return (int)EMetodoPagamento.Refeicao;
            }
            return (int)EMetodoPagamento.Outros;
        }

        private static string AdicionaItens(PedidoInformation pedidoPDV, ItemIFood[] itensIFood, PedidoProdutoInformation itemPaiPDV, PDVInformation pdv, UsuarioInformation usuario)
        {
            try
            {
                string log = "";
                foreach (var itemIFood in itensIFood)
                {
                    string notaProduto = "";
                    if (!int.TryParse(itemIFood.externalCode, out int idPdod))
                    {
                        idPdod = 1;
                        notaProduto = $"(sem código: {itemIFood.name})";
                        log += $"\r\n{idPdod}: {notaProduto}";
                    }

                    var itemPDV = Produto.Carregar(idPdod);
                    if (itemPDV == null || itemPDV.Nome == null)
                    {
                        itemPDV = Produto.Carregar(1); // Lança como produto não cadastrado
                        notaProduto = $"(não existe: {itemIFood.name})";
                        log += $"\r\n{idPdod}: {notaProduto}";
                    }
                    else if (itemPDV.Excluido == true)
                    {
                        itemPDV = Produto.Carregar(1); // Lança como produto não cadastrado
                        notaProduto = $"(excluido: {itemIFood.name})";
                        log += $"\r\n{idPdod}: {notaProduto}";
                    }
                    else if (itemPDV.Ativo == false)
                    {
                        itemPDV = Produto.Carregar(1); // Lança como produto não cadastrado
                        notaProduto = $"(inativo: {itemIFood.name})";
                        log += $"\r\n{idPdod}: {notaProduto}";
                    }

                    //decimal qtd = itemIFood.quantity;
                    //if (itemPaiPDV != null)
                    //{
                    //    if (qtd != 1)
                    //    {
                    //        notaProduto = $"({qtd}X)";
                    //        //qtd = 1;
                    //    }
                    //}

                    var item = new PedidoProdutoInformation()
                    {
                        Pedido = pedidoPDV,
                        Produto = itemPDV,
                        PedidoProdutoPai = itemPaiPDV,
                        PDV = pdv,
                        Usuario = usuario,
                        Quantidade = itemIFood.quantity,
                        ValorUnitario = itemIFood.price,
                        CodigoAliquota = itemPDV.CodigoAliquota,
                        Notas = notaProduto + (itemIFood.observations ?? ""),
                        DtInclusao = DateTime.Now,
                        Cancelado = false,
                        RetornarAoEstoque = false
                    };

                    PedidoProduto.Adicionar(item);

                    if (itemIFood.subItems != null)
                        AdicionaItens(pedidoPDV, itemIFood.subItems, item, pdv, usuario);
                }
                return log;
            }
            catch (Exception ex)
            {
                throw new ExceptionPDV(CodigoErro.EE15, ex);
            }
        }

        public static string ObtemDDDTelefone(string telefone, out int telefone1DDD, out int telefone1Numero)
        {
            var telefones = telefone.Split('-');
            if (telefone.Length > 10 && (telefone.StartsWith("0800") || telefone.Contains("ID:")))
            {
                telefone1DDD = 0;
                telefone1Numero = 0;
                return "0";
            }
            else if (telefones.Length == 2)
            {
                // Limpa
                telefones[0] = telefones[0].Trim();
                telefones[1] = telefones[1].Trim();

                // Valida o numero para no maximo 9 digitos!
                if (telefones[1].Length > 9)
                {
                    int offset = telefones[1].Length - 9;
                    if (offset == 2 && telefones[0] == "0") // o DDD está no campo do telefone!
                        telefones[0] = telefones[1].Substring(0, 2);

                    // Só os ultimos digitos
                    telefones[1] = telefones[1].Substring(offset, 9);
                }

                // DDD para no maximo 2 digitos
                if (telefones[0].Length > 2)
                    telefones[0] = telefones[0].Substring(0, 2);

                int.TryParse(telefones[0], out telefone1DDD);
                int.TryParse(telefones[1], out telefone1Numero);

                return $"{telefone1DDD}{telefone1Numero}";
            }
            else
            {
                telefone1DDD = 1;
                telefone1Numero = 1;
                return "1";
                //throw new Exception("Não é possível identificar o cliente sem um telefone válido");
            }
        }
    }
}
