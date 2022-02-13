using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    public enum EFluxo
    {
        Produto,
        ComandaPagamento,
        CheckInSemEntrada,
        CheckInEntrada,
        AdicionarCredito
    }

    internal partial class PedidoServices
    {
        PedidoAPI apiPedido;
        ClienteAPI apiCliente;
        ImpressaoAPI apiImpressao;
        TextBlock totalPedido, totalAPagar, saldoCredito;
        PagamentoServices pinpad;
        ObservableCollection<VendaItem> Itens;
        Button Confirmar;
        Pedido PedidoComanda;

        internal EFluxo TipoFluxo { get; private set; }
        internal int PedidoID { get; private set; }
        internal int PedidoItemEdit { get; private set; }
        internal Boolean PedidoItemEditCancel { get; private set; }
        internal int TipoPedido { get; private set; }
        internal int ComandaNumero { get; private set; }
        internal bool ComandaCarregada { get; private set; }
        internal int ComandaStatus { get; private set; }
        internal decimal TotalPedido { get; private set; } // ou entrada quando forcheckin
        internal decimal TotalAPagar { get; private set; }
        internal decimal TotalProdutos { get; private set; }

        // Processo de Checkin
        internal int ClienteID { get; private set; }
        internal string ClienteCPF { get; private set; }
        internal string ClienteNotaComCPF { get; private set; }
        internal string ClienteNome { get; private set; }
        internal decimal ClienteSaldo { get; private set; }
        internal VendaItem ItemEdit() => Itens[PedidoItemEdit];

        // TODO: Novos campos para o novo fluxo de comanda
        public string Comanda_LeitoraTAG { get; set; }
        public string Comanda_LeitoraTIPO { get; set; }
        public int Comanda_Numero { get; set; }
        public int Comanda_IDCliente { get; set; }
        public string Comanda_ClienteDocumento { get; set; }
        public string Comanda_ClienteNome { get; internal set; }
        public string Comanda_ClienteTelefone { get; internal set; }

        internal void ItemCancelar()
        {
            if (!PedidoItemEditCancel)
                Itens.RemoveAt(PedidoItemEdit);

            PedidoItemEditCancel = false;
        }

        internal PedidoServices(ClienteWS ws, PagamentoServices tefObj)
        {
            PedidoID = 0;
            TipoPedido = 40; // Venda Balcão
            apiPedido = ws.Pedido();
            apiCliente = ws.Cliente();
            apiImpressao = ws.Impressao();
            pinpad = tefObj;
            ComandaCarregada = false;
            Itens = new ObservableCollection<VendaItem>();
            TipoFluxo = EFluxo.Produto;
            ClienteID = 0;
            ClienteNome = "";
            ClienteNotaComCPF = null;
            ComandaNumero = 0;
            Comanda_LeitoraTAG = "";
            Comanda_IDCliente = 0;
            Comanda_Numero = 0;
            Comanda_ClienteNome = "";
            Comanda_ClienteTelefone = "";
        }

        internal void SetFluxo(EFluxo tipo)
        {
            TipoFluxo = tipo;
        }

        internal void Bind(ItemsControl pedidoLista)
        {
            pedidoLista.ItemsSource = Itens;
        }

        internal void Bind(Button confirmar)
        {
            Confirmar = confirmar;
            AtualizaTotal();
        }

        internal void Bind(TextBlock total, TextBlock apagar, TextBlock saldo)
        {
            totalPedido = total;
            totalAPagar = apagar;
            saldoCredito = saldo;
            AtualizaTotal();
        }

        internal void ClienteNotaCPF(string cpf)
        {
            ClienteNotaComCPF = cpf;
        }

        internal void ClienteDefine(int nIDCliente, string cNome, int nComanda)
        {
            ClienteID = nIDCliente;
            ClienteNome = cNome;
            ComandaNumero = nComanda;
        }

        internal void AtualizaTotal()
        {
            if (TipoFluxo == EFluxo.Produto)
            {
                TotalProdutos = 0;
                foreach (var item in Itens)
                {
                    item.ValorModificacoes = 0;
                    item.DescricaoModificacoes = item.Produto.Descricao;
                    if (item.Modificacoes.Count > 0)
                    {
                        item.DescricaoModificacoes = "";
                        foreach (var modificacao in item.Modificacoes)
                        {
                            var pModificacao = App.Produtos.LoadModificacao(modificacao.IDProduto.Value);
                            item.DescricaoModificacoes += pModificacao.Nome + ", ";
                            item.ValorModificacoes += pModificacao.ValorUnitario.Value;
                        }
                        item.DescricaoModificacoes = item.DescricaoModificacoes.Substring(0, item.DescricaoModificacoes.Length - 2);
                    }
                    TotalProdutos += item.Quantidade * (item.ValorUnitario + item.ValorModificacoes);
                }

                TotalAPagar = TotalProdutos;

                if (totalPedido != null)
                    totalPedido.Text = String.Format("Total: R$ {0:N2}", TotalAPagar);

                if (Confirmar != null)
                    Confirmar.Visibility = TotalAPagar > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (TipoFluxo == EFluxo.ComandaPagamento)
            {
                totalPedido.Text = String.Format("Total: R$ {0:N2}", TotalPedido);
                if (totalAPagar != null)
                {
                    if (TotalPedido != TotalAPagar)
                        totalAPagar.Text = String.Format("A Pagar: R$ {0:N2}", TotalAPagar);
                    else
                        totalAPagar.Text = "";
                }
            }
            else if (TipoFluxo == EFluxo.CheckInEntrada)
            {
                if (totalPedido != null)
                    totalPedido.Text = String.Format("Total a pagar: R$ {0:N2}", TotalAPagar);

                if (saldoCredito != null)
                    saldoCredito.Text = String.Format("{0:N2}", ClienteSaldo);

                if (Confirmar != null)
                    Confirmar.Visibility = TotalAPagar > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (TipoFluxo == EFluxo.AdicionarCredito)
            {
                if (totalPedido != null)
                    totalPedido.Text = String.Format("Total a pagar: R$ {0:N2}", TotalAPagar);

                if (saldoCredito != null)
                    saldoCredito.Text = String.Format("{0:N2}", ClienteSaldo);

                if (Confirmar != null)
                    Confirmar.Visibility = TotalAPagar > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        internal void ImprimirConta()
        {
            var conta = apiImpressao.ImagemConta(PedidoID, ImpressoraServices.Width);
            App.Impressora.ImprimirImagem(conta);
        }

        internal void ImprimirExtratoCreditos()
        {
            var itens = apiPedido.ComandaExtrato(ComandaNumero, true);
            if (itens == null)
                return;

            var sb = new StringBuilder();
            sb.AppendLine("Autoatendimento: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            sb.AppendLine("Comanda: " + ComandaNumero);
            foreach (var item in itens)
                sb.AppendLine($"{item.Data?.ToString("HH:mm") ?? "     "} {item.Produto.PadRight(20).Substring(0, 20)} {item.Valor.ToString("N2").PadLeft(7)} {item.Saldo.ToString("N2").PadLeft(7)}");

            App.Impressora.ImprimirTexto(sb.ToString());
        }

        internal void DefinirCredito(Produto produto)
        {
            Itens.Clear();
            Itens.Add(new VendaItem(1, produto));
            TotalAPagar = TotalPedido + TotalProdutos; // Entrada + Creditos
            AtualizaTotal();
        }

        internal bool Contem(int idProduto)
        {
            return Itens.Any(i => i.Produto.IDProduto == idProduto);
        }

        internal bool Adicionar(Produto produto)
        {
            if (produto.Disponibilidade != true)
                return false;

            PedidoItemEditCancel = false;

            for (int i = 0; i < Itens.Count; i++)
            {
                if (Itens[i].Produto.IDProduto == produto.IDProduto && Itens[i].Modificacoes.Count == 0)
                {
                    Itens[i] = new VendaItem(Itens[i].Quantidade + 1, produto);
                    PedidoItemEdit = i;
                    AtualizaTotal();
                    return true;
                }
            }

            Itens.Add(new VendaItem(1, produto));
            PedidoItemEdit = Itens.Count - 1;
            AtualizaTotal();
            return true;
        }

        internal Comanda ConsultaComanda(string numero)
        {
            return apiPedido.ComandaStatus(numero, true);
        }

        internal Comanda ComandaInfo(string numero, string tipo)
        {
            return apiPedido.ComandaInfo(numero, tipo, true);
        }

        internal Cliente ConsultaCliente(string numeroDocumento)
        {
            var cliente = apiCliente.ListaCliente(documento: numeroDocumento);
            if (cliente.Count >= 1)
                return cliente[0];
            else
                return null;
        }

        internal async Task<bool> CarregarComandaAsync(int numero)
        {
            ComandaStatus = 0;
            ComandaNumero = 0;
            ClienteSaldo = 0;
            await ModalSimNaoWindow.ShowAsync("Aguarde, validando comanda...", () =>
            {
                var ComandaCheckIn = apiPedido.ComandaStatus(numero, true);
                if (ComandaCheckIn == null)
                    return "Comanda não existe";

                else if (ComandaCheckIn.status == 10)
                {
                    ComandaNumero = numero;
                    ComandaStatus = 10; // OK: Liberada = 10
                    return null;
                }
                else if (ComandaCheckIn.status == 20)
                {
                    PedidoComanda = apiPedido.ComandaItens(numero, true);
                    if (PedidoComanda == null || PedidoComanda.Itens == null)
                        return "Erro ao ler o pedido da comanda";

                    if (PdvServices.ComandaComCredito)
                    {
                        if (PedidoComanda.Cliente?.IDCliente > 0)
                            CarregaSaldoCreditos(PedidoComanda.Cliente.IDCliente.Value);

                        TipoFluxo = EFluxo.AdicionarCredito;
                    }
                    else
                        TipoFluxo = EFluxo.ComandaPagamento;

                    ComandaStatus = 20; // OK: Em Uso
                    ComandaNumero = numero;
                    PreenchePedido();
                    return null;
                }
                else if (ComandaCheckIn.status == 30)
                    return "Comanda cancelada";
                else if (ComandaCheckIn.status == 40)
                    return "Comanda perdida";
                else // 0
                    return "Erro ao obter dados da comanda";

            });

            return ComandaStatus > 0;
        }

        internal void CarregaSaldoCreditos(int idCliente)
        {
            ClienteSaldo = 0;
            var resultado = apiCliente.SaldoCliente(idCliente);
            if (resultado?.Mensagem == "OK" && resultado.Valor > 0)
                ClienteSaldo = resultado.Valor.Value;
        }

        internal ExtratoItens[] CarregaExtratoCreditos()
        {
            return apiPedido.ComandaExtrato(ComandaNumero);
        }

        internal ResultadoOuErro InserirCliente(string cpf, string nome, string telefone)
        {
            int? telefoneDDD = null;
            int? telefoneNumero = null;
            telefone = telefone.SoNumeros();
            if (telefone.Length > 9)
            {
                telefoneDDD = int.Parse(telefone.Substring(0, 2));
                telefoneNumero = int.Parse(telefone.Substring(2));
            }
            return apiCliente.InserirCliente(new Cliente()
            {
                Documento1 = cpf,
                NomeCompleto = nome,
                Telefone1DDD = telefoneDDD,
                Telefone1Numero = telefoneNumero
            });
        }

        internal string AbrirComanda(int idCliente, string cNome, int nComanda)
        {
            return ModalSimNaoWindow.Show($"Aguarde, abrindo comanda {nComanda}...", () =>
            {
                try
                {
                    var abrir = new ComandaAbrir()
                    {
                        Comanda = nComanda,
                        ClienteID = idCliente,
                        PDVID = PdvServices.PDVID,
                        UsuarioID = PdvServices.UsuarioID,
                        Validar = false
                    };

                    var result = apiPedido.ComandaAbrir(abrir);

                    if (result.Mensagem == "OK")
                        ClienteDefine(result.Id.Value, cNome, nComanda);

                    return result?.Mensagem;
                }
                catch (Exception ex)
                {
                    EventLogServices.Error(ex);
                    return "ERRO: " + ex.Message;
                }
            });
        }

        internal async Task<bool> AbrirComandaAsync(string cpf)
        {
            bool retorno = false;
            TotalPedido = TotalAPagar = 0m;
            TipoFluxo = EFluxo.CheckInEntrada;
            await ModalSimNaoWindow.ShowAsync("Aguarde, buscando seu cadastro...", () =>
            {
                try
                {
                    var clientes = apiCliente.ListaCliente(documento: cpf);
                    if (clientes == null || clientes.Count == 0)
                        return "CPF não encontrado\nDirija-se ao caixa";

                    else if (clientes.Count > 1)
                        return "Há mais de um CPF cadastrado\nDirija-se ao caixa";

                    var entrada = apiPedido.ComandaEntrada(true);
                    if (entrada == null)
                        return "Não há tipo de entrada padrão configurada";

                    else if (entrada.Mensagem != "OK")
                        return entrada.Mensagem;

                    else if (entrada.ValorConsumacaoMinima > 0)
                        return "Não é possível ter consumação mínima no autoatendimento";

                    var abrir = new ComandaAbrir()
                    {
                        Comanda = ComandaNumero,
                        ClienteID = clientes[0].IDCliente.Value,
                        PDVID = PdvServices.PDVID,
                        UsuarioID = PdvServices.UsuarioID,
                        Validar = entrada.ValorEntrada > 0 // Não abre a comanda quando tem entrada, só valida para ver se pode ser aberta
                    };

                    var resultado = apiPedido.ComandaAbrir(abrir);
                    if (resultado?.Mensagem == "OK")
                    {
                        if (PdvServices.ComandaComCredito)
                            CarregaSaldoCreditos(clientes[0].IDCliente.Value);

                        TotalPedido = TotalAPagar = entrada.ValorEntrada;

                        if (abrir.Validar)
                        {
                            ClienteNome = clientes[0].NomeCompleto;
                            ClienteID = clientes[0].IDCliente.Value;
                            ClienteCPF = cpf;
                            TipoPedido = 20;
                            retorno = true;
                            return null;
                        }

                        PedidoComanda = apiPedido.ComandaItens(ComandaNumero, true);
                        if (PedidoComanda == null || PedidoComanda.Itens == null)
                            return "Pedido não foi criado corretamente";
                        else
                        {
                            if (entrada.ValorEntrada == 0)
                                TipoFluxo = EFluxo.CheckInSemEntrada; // Muda o tipo de fluxo!
                            else
                                TipoFluxo = EFluxo.CheckInEntrada;

                            retorno = true;
                            return "Comanda aberta com sucesso";
                        }
                    }
                    else
                        return resultado?.Mensagem ?? "Comanda não existe";
                }
                catch (Exception ex)
                {
                    EventLogServices.Error(ex);
                    return "ERRO: " + ex.Message;
                }
            });

            return retorno;
        }

        internal async Task<bool> CarregarComandaPedidoAsync(int numero)
        {
            PedidoComanda = null;
            string mensagem = ComandaNumero == numero ? "Validando Pedido..." : "Carregando Pedido...";
            await ModalSimNaoWindow.ShowAsync(mensagem, () =>
            {
                PedidoComanda = apiPedido.ComandaItens(numero, true);
                if (PedidoComanda == null || PedidoComanda.Itens == null)
                    return "Pedido não existe";

                return null;
            });

            if (PedidoComanda == null || PedidoComanda.Itens == null)
                return false;

            ComandaNumero = numero;

            if (!PdvServices.ComandaComCredito)
                TipoFluxo = EFluxo.ComandaPagamento;

            PreenchePedido();

            return true;
        }

        private void PreenchePedido()
        {
            Itens.Clear();
            ComandaCarregada = true;
            PedidoID = PedidoComanda.IDPedido.Value;
            TipoPedido = 20; // Comanda
            TotalPedido = PedidoComanda.ValorTotal.Value;
            var pago = PedidoComanda.Pagamentos.Sum(p => p.Valor.Value);
            TotalAPagar = TotalPedido - pago;

            ClienteID = PedidoComanda.Cliente?.IDCliente ?? 0;
            ClienteNome = PedidoComanda.Cliente?.NomeCompleto;
            ClienteCPF = PedidoComanda.Cliente?.Documento1;

            foreach (var item in PedidoComanda.Itens.Where(i => i.Preco > 0))
            {
                var existente = Itens.FirstOrDefault(i => i.Produto.IDProduto == item.IDProduto.Value);
                if (existente == null)
                {
                    var prod = App.Produtos.LoadProduto(item.IDProduto.Value);
                    var vi = new VendaItem(item.Qtd.Value, prod, item.Preco);

                    Itens.Add(vi);
                }
                else
                    existente.Quantidade += item.Qtd.Value;
            }
        }

        internal bool Edit(object sender)
        {
            var item = (sender as Button).DataContext as VendaItem;
            PedidoItemEdit = Itens.IndexOf(item);
            PedidoItemEditCancel = true;
            return PedidoItemEdit != -1;
        }

        internal void Remover(object sender)
        {
            var item = (sender as Button).DataContext as VendaItem;
            int index = Itens.IndexOf(item);
            if (index != -1)
            {
                Itens.RemoveAt(index);
                AtualizaTotal();
            }
        }

        internal void Alterar(object sender, int qtd)
        {
            var item = (sender as Button).DataContext as VendaItem;
            var index = Itens.IndexOf(item);
            if (index == -1)
                return;

            if (Itens[index].Quantidade + qtd <= 0)
                Itens.RemoveAt(index);
            else
                Itens[index] = new VendaItem(item, qtd);

            var temCreditos = Itens.Any(p => p.Produto.Categorias.Any(c => c.IDCategoria == PdvServices.IDCategoriaProduto_Credito));

            if (PdvServices.IDProduto_NovaComanda > 0) // Tem valor para compra de nova comanda
            {
                if (App.Pedido.Comanda_IDCliente == 0 // Comanda não relacionada a cliente
                 && (!Contem(PdvServices.IDProduto_NovaComanda) // Precisa compra ou sem credito apaga!
                  || !temCreditos))
                {
                    index = 0;
                    while (index < Itens.Count)
                    {
                        if (Itens[index].Produto.IDProduto == PdvServices.IDProduto_NovaComanda)
                            Itens.RemoveAt(index);
                        else if (Itens[index].Produto.Categorias.Any(c => c.IDCategoria == PdvServices.IDCategoriaProduto_Credito))
                            Itens.RemoveAt(index);
                        else
                            index++;
                    }
                }
            }

            if (!temCreditos)
                App.Pedido.Comanda_LeitoraTAG = ""; // Esquece a comanda!


            AtualizaTotal();
        }

        internal void Cancelar()
        {
            PedidoID = 0;
            TipoPedido = 40; // Balcão
            Itens.Clear();
            AtualizaTotal();
        }

        internal async Task FecharComandaSemValorAsync()
        {
            await ModalSimNaoWindow.ShowAsync("Fechando Comanda...", () =>
            {
                var fechamento = new FechamentoPedido(PedidoID, PdvServices.PDVID, PdvServices.ChaveUsuario, false);
                var result = apiPedido.Fechar(fechamento);
                if (result.Mensagem != "OK")
                    return $"Erro ao fechar pedido {PedidoID} retornou: {result.Mensagem}";

                return null;
            });
        }
    }
}