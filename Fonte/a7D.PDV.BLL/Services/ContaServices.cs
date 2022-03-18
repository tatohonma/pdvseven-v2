using a7D.Fmk.CRUD.DAL;
using a7D.PDV.BLL.Extension;
using a7D.PDV.BLL.Utils;
using a7D.PDV.BLL.ValueObject;
using a7D.PDV.EF.Enum;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace a7D.PDV.BLL.Services
{
    public static class ContaServices
    {
        public static string NomeTaxaServico = "Serviço";

        public static void ImprimirConta(string nomeImpressoraWindows, PedidoInformation pedido)
        {
            var dados = PrepararDadosImpressao(pedido);
            ImpressoraWindows.Imprimir(nomeImpressoraWindows, (s, a) => DesenhaContaGerencial(a.Graphics, dados, ImpressoraWindows.Largura));
        }

        public static byte[] ImprimirImagemConta(PedidoInformation pedido, int width)
        {
            if (width < ImpressoraWindows.Largura)// Tamanho minimo e padrão
                width = ImpressoraWindows.Largura;

            var bmp = new Bitmap(width, 10000);
            Graphics g = Graphics.FromImage(bmp);
            var dados = PrepararDadosImpressao(pedido);
            int height = DesenhaContaGerencial(g, dados, width);
            return ImageUtil.ReduzEtransforma(bmp, height);
        }

        public static ImpressaoHelper PrepararDadosImpressao(PedidoInformation pedido, int colunas = Constantes.Colunas)
        {
            if (colunas < Constantes.Colunas)
                colunas = Constantes.Colunas;

            var tipoEntrada = string.Empty;
            if (pedido.TipoEntrada != null && pedido.TipoEntrada.IDTipoEntrada.HasValue)
            {
                tipoEntrada = $"({TipoEntrada.Carregar(pedido.TipoEntrada.IDTipoEntrada.Value).Nome})";
            }

            //string logoCliente = ConverterImagemParaBase64(Properties.Resources.PDV7, ImageFormat.Jpeg);

            List<string> listaProduto = new List<string>();
            List<ItemValor> listaProdutoValor = new List<ItemValor>();
            string dadosItem = string.Empty;
            string linha = string.Empty;
            int nItem = 0;

            List<PedidoProdutoInformation> listaPedidoProduto = new List<PedidoProdutoInformation>();

            bool exibirItensZerados = pedido.TipoPedido.TipoPedido == ETipoPedido.Delivery ? true : ConfiguracoesSistema.Valores.ExibirItensZeradosCaixaPreConta;

            foreach (var item in pedido.ListaProduto)
            {
                if (item.Cancelado != true && Produto.ServicoComoProduto(item.Produto))
                {
                    if (item.ValorUnitario > 0 || exibirItensZerados) // || item.ListaModificacao?.Count() > 0)
                    {
                        listaPedidoProduto.Add(item);
                    }

                    if (item.ListaModificacao?.Count() > 0)
                    {
                        foreach (var modificacao in item.ListaModificacao)
                        {
                            if (modificacao.Cancelado == false)
                            {
                                modificacao.NivelModificacao = 1;
                                if (modificacao.ValorUnitario > 0 || exibirItensZerados)
                                    listaPedidoProduto.Add(modificacao);
                            }
                        }
                    }
                }
            }

            var listaProdutoAgrupadoSemDesconto = from l in listaPedidoProduto
                                                  where l.ValorDesconto == 0 || l.ValorDesconto == null
                                                  group l by new { l.Produto.IDProduto, l.Produto.Nome, l.ValorUnitario, l.Notas, l.NivelModificacao } into g
                                                  // l.IDPedidoProduto, l.PedidoProdutoPai 
                                                  select new
                                                  {
                                                      IDProduto = g.Key.IDProduto,
                                                      NomeProduto = g.Key.Nome,
                                                      Valor = g.Key.ValorUnitario,
                                                      Quantidade = g.Sum(x => x.Quantidade),
                                                      Desconto = new Decimal?(0),
                                                      Notas = g.Key.Notas ?? "",
                                                      Nivel = g.Key.NivelModificacao
                                                  };

            var listaProdutoComDesconto = from l in listaPedidoProduto
                                          where l.ValorDesconto != 0 && l.ValorDesconto != null
                                          select new
                                          {
                                              IDProduto = l.Produto.IDProduto,
                                              NomeProduto = l.Produto.Nome,
                                              Valor = l.ValorUnitario + l.ValorDesconto,
                                              Quantidade = new Decimal?(1),
                                              Desconto = l.ValorDesconto,
                                              Notas = l.Notas ?? "",
                                              Nivel = l.NivelModificacao
                                          };


            var listaProdutosPedido = listaProdutoAgrupadoSemDesconto.ToList();
            listaProdutosPedido.AddRange(listaProdutoComDesconto);

            foreach (var pedidoProduto in listaProdutosPedido)
            {
                if (!(pedidoProduto.Valor > 0 || exibirItensZerados)) // mantendo a mesma logica !!! com negação
                    continue;

                string nomeProduto = pedidoProduto.IDProduto == ProdutoInformation.IDProdutoEntrada || pedidoProduto.IDProduto == ProdutoInformation.IDProdutoEntracaCM ? $"{pedidoProduto.NomeProduto} {tipoEntrada}" : pedidoProduto.NomeProduto;
                string numeroItem = "";
                if (pedidoProduto.Nivel == 0) // produto pai
                {
                    nItem++;
                    numeroItem = nItem.ToString("000");
                    nomeProduto = numeroItem + "|" + (new string(' ', pedidoProduto.Nivel)) + nomeProduto;
                }

                decimal quantidade = 1;
                if (pedidoProduto.Quantidade.HasValue)
                    quantidade = pedidoProduto.Quantidade.Value;

                decimal valorUnidade = 0;
                if (pedidoProduto.Valor.HasValue)
                    valorUnidade = pedidoProduto.Valor.Value;

                decimal valorTotal = Math.Truncate(quantidade * valorUnidade * 100m) / 100m;
                string valorProduto = valorTotal.ToString("N2");

                if (quantidade > 1)
                {
                    dadosItem = quantidade.ToString("#0.#").Replace(".", ",") + "x";
                    if (valorUnidade > 0)
                        dadosItem += valorUnidade.ToString("N2");
                }
                else
                    dadosItem = "";

                linha = LinhaItemValor(nomeProduto, dadosItem, valorTotal, colunas);
                listaProduto.Add(linha);

                var itemPrint = new ItemValor(nomeProduto, valorTotal, dadosItem, nivel: pedidoProduto.Nivel);
                //if (pedido.TipoPedido.IDTipoPedido==30)
                //    itemPrint.WordWrap = true;

                listaProdutoValor.Add(itemPrint);
                if (exibirItensZerados && !string.IsNullOrEmpty(pedidoProduto.Notas))
                {
                    itemPrint = new ItemValor("*" + pedidoProduto.Notas, nivel: 4, multLines: true);
                    listaProdutoValor.Add(itemPrint);
                }

                //printLine(0, 1); // pedidoProduto.IDPedidoProduto

                if ((pedidoProduto.Desconto ?? 0) > 0)
                {
                    linha = LinhaItemValor(null, $"    desconto no item {numeroItem}", pedidoProduto.Desconto.Value * -1, colunas);
                    listaProduto.Add(linha);
                    listaProdutoValor.Add(new ItemValor($"desconto no item {numeroItem}", pedidoProduto.Desconto.Value * -1, nivel: 4));
                }

            }

            var h = new ImpressaoHelper
            {
                NomeFantasia = ConfiguracoesSistema.Valores.NomeFantasia,
                DataEmissao = pedido.DtPedido?.ToString("dd/MM/yyyy HH:mm:ss"),
                IdPedido = pedido.IDPedido.Value.ToString(),
                ObservacaoCupom = pedido.ObservacaoCupom
            };
            h.plain.AppendLine(h.DataEmissao);

            pedido.TipoPedido = TipoPedido.Carregar(pedido.TipoPedido.IDTipoPedido.Value);

            switch ((ETipoPedido)pedido.TipoPedido.IDTipoPedido.Value)
            {
                case ETipoPedido.Mesa:
                    h.Identificacao = "MESA " + Mesa.CarregarPorGUID(pedido.GUIDIdentificacao).Numero.ToString();
                    h.plain.AppendLine(h.Identificacao);
                    break;
                case ETipoPedido.Comanda:
                    h.Identificacao = "COMANDA " + Comanda.CarregarPorGUID(pedido.GUIDIdentificacao).Numero.ToString();
                    h.plain.AppendLine(h.Identificacao);
                    break;
                case ETipoPedido.Delivery:
                    if(pedido.OrigemPedido != null && pedido.OrigemPedido.IDOrigemPedido == (int)EOrigemPedido.ifood)
                    {
                        TagInformation tagDisplayId = BLL.Tag.Carregar(pedido.GUIDIdentificacao, "ifood-displayId");

                        h.Identificacao = "IFOOD " + tagDisplayId.Valor;
                        h.plain.AppendLine("DELIVERY " + h.IdPedido + " IFOOD " + tagDisplayId.Valor);
                    }
                    else
                    {
                        h.Identificacao = "DELIVERY";
                        h.plain.AppendLine("DELIVERY " + h.IdPedido);
                    }

                    break;
                case ETipoPedido.Balcao:
                    //h.NumeroMesa = Comanda.CarregarPorGUID(pedido.GUIDIdentificacao).Numero.ToString();
                    h.Identificacao = "";
                    h.plain.AppendLine("PEDIDO " + h.IdPedido);
                    break;
            }

            h.plain.AppendLine("PEDIDO " + h.IdPedido);

            Cliente.Fill(pedido, h);

            h.Produtos = listaProduto.ToArray();
            h.ProdutosValores = listaProdutoValor.ToArray();

            h.plain.AppendLine();

            foreach (var pv in h.ProdutosValores)
                h.plain.AppendLine(pv.ToString());

            h.plain.AppendLine();

            TotaisFill(pedido, h);

            h.plain.AppendLine();
            h.plain.AppendLine("*************** PDVSeven ***************");
            h.plain.AppendLine("*  SISTEMAS PARA RESTAURANTES E BARES  *");
            h.plain.AppendLine("          www.pdvseven.com.br           ");

            return h;
        }

        public static void TotaisFill(PedidoInformation pedido, ImpressaoHelper h)
        {
            if (pedido.Entregador?.IDEntregador != null)
            {
                if (string.IsNullOrWhiteSpace(pedido.Entregador.Nome))
                    pedido.Entregador = Entregador.Carregar(pedido.Entregador.IDEntregador.Value);

                if (!string.IsNullOrWhiteSpace(pedido.Entregador.Nome))
                    h.Entregador = pedido.Entregador.Nome;
            }

            var valorTotalProdutosServicos = pedido.ValorTotalProdutosServicos + (pedido.ValorEntrega.HasValue ? pedido.ValorEntrega.Value : 0);
            var valorTotalProdutosItens = pedido.ValorTotalProdutos;

            if (pedido.ValorEntrega.HasValue)
                h.plain.AppendLine(new ItemValor("Taxa de Entrega: ", pedido.ValorEntrega.Value.ToString("#0.00")).ToString());

            h.NumeroPessoas = pedido.NumeroPessoas.HasValue ? pedido.NumeroPessoas.Value : 1;

            if (ConfiguracoesSistema.Valores.ServicoComoItem)
            {
                h.ValorTotal = valorTotalProdutosServicos.ToString("#0.00");
            }
            else
            {
                h.ValorTotal = valorTotalProdutosItens.ToString("#0.00");
                h.plain.AppendLine(new ItemValor("Valor Produtos: ", h.ValorTotal).ToString());
            }

            if (pedido.ValorEntrega.HasValue)
            {
                h.ValorEntrega = pedido.ValorEntrega.Value.ToString("#,##0.00");
                h.plain.AppendLine(new ItemValor("Valor Entrega: ", h.ValorEntrega).ToString());
            }

            var pagamentos = new List<ItemValor>();
            decimal totalJaPago = 0;
            foreach (var pagamento in pedido?.ListaPagamento.Where(p => p.Status != StatusModel.Excluido))
            {
                totalJaPago += pagamento.Valor.Value;
                pagamentos.Add(new ItemValor(pagamento.TipoPagamento.Nome, pagamento.Valor.Value));
            }

            if (totalJaPago > 0)
                h.plain.AppendLine(new ItemValor("Valor Pago: ", totalJaPago).ToString());

            h.PagamentosValores = pagamentos.ToArray();

            decimal? acrescimo = Pedido.IncluirAcrescimo(pedido.ListaProduto);

            if (acrescimo.HasValue && acrescimo.Value > 0m)
            {
                h.ValorAcrescimo = acrescimo.Value.ToString("#0.00");
                h.plain.AppendLine(new ItemValor(NomeTaxaServico + ":", h.ValorAcrescimo).ToString());
            }

            decimal totalApagar;
            if (pedido.ValorDesconto.HasValue && pedido.ValorDesconto > 0)
            {
                h.ValorDesconto = pedido.ValorDesconto.Value.ToString("#0.00");
                if (pedido.ValorContaCliente != null && pedido.ValorSaldoCliente != null)
                {
                    h.SaldoCliente = pedido.ValorSaldoCliente.Value.ToString("N2");
                    h.ContaCliente = pedido.ValorContaCliente.Value.ToString("N2");
                    h.plain.AppendLine(new ItemValor("Débito de Saldo do Cliente: ", h.ContaCliente).ToString());
                }
                else
                    h.plain.AppendLine(new ItemValor("Valor Desconto: ", h.ValorDesconto).ToString());

                totalApagar = valorTotalProdutosServicos - pedido.ValorDesconto.Value - totalJaPago;
            }
            else
                totalApagar = valorTotalProdutosServicos - totalJaPago;

            h.ValorTotalAPagar = totalApagar.ToString("#0.00");
            h.plain.AppendLine(new ItemValor("Total a Pagar: ", h.ValorTotalAPagar).ToString());

            if (!string.IsNullOrWhiteSpace(h.SaldoCliente))
                h.plain.AppendLine(new ItemValor("Saldo do Cliente: ", h.SaldoCliente).ToString());

            if (h.NumeroPessoas > 1)
            {
                var totalPessoa = Math.Round(totalApagar / h.NumeroPessoas, 2, MidpointRounding.AwayFromZero);
                h.plain.AppendLine(new ItemValor($"Total por pessoa ({h.NumeroPessoas} pessoas)".ToString(), totalPessoa).ToString());
            }

        }

        private static string LinhaItemValor(string item, string dados, decimal valor, int colunas)
        {
            string linha;
            string preco;

            if (dados == null) // item sem dados
            {
                dados = "";
            }
            else if (item == null) // desconto
            {
                item = "";
            }
            else
            {
                // produto  com dados
                if (string.IsNullOrEmpty(dados))
                    dados = "";
                else if (!string.IsNullOrEmpty(item))
                    dados = "|" + dados;
            }

            if (valor == 0)
                preco = new string(' ', 8);
            else
                preco = "  " + Convert.ToDecimal(valor.ToString("#0.00").Replace(".", ",")).ToString("#0.00");

            int maxLength = colunas - preco.Length;
            if ((item.Length + dados.Length + preco.Length) > maxLength)
                linha = item.PadRight(maxLength).Substring(0, maxLength - dados.Length) + dados;
            else
                linha = (item + dados).PadRight(maxLength).Substring(0, maxLength);

            var x = linha + preco;
            int n = x.Length;
            return linha + preco + "\n";
        }

        public static int DesenhaContaGerencial(Graphics g, ImpressaoHelper dados, int totalWidth)
        {
            ImpressoraWindows.ConfiguraFontes(g,
                out Font fTitulo,
                out Font fNormal,
                out Font fNormalB,
                out Font fPequena,
                out Font fPequenaB,
                totalWidth);

            int espaco = Constantes.Espacamento;
            Point p = new Point(0, 0);
            var img = ImageUtil.LogoPDV7_Horizontal_PB();

            p.X = totalWidth / 2 - img.Width / 2;
            g.DrawImage(img, p);
            p.Y += img.Height + espaco;

            if (!string.IsNullOrEmpty(dados.NomeFantasia))
                p.Y += g.DrawCenter(fTitulo, dados.NomeFantasia, p.Y, totalWidth);

            p.Y += g.DrawCenter(fNormalB, "DOCUMENTO NÃO FISCAL", p.Y, totalWidth);
            p.Y += g.DrawSeparador(p.Y, totalWidth);

            p.X = 0;
            p.Y += g.DrawText($"PEDIDO {dados.IdPedido}", dados.Identificacao, fNormalB, p.Y, totalWidth);

            if (!string.IsNullOrEmpty(dados.ObservacaoCupom))
                p.Y += g.DrawCenter(fNormalB, dados.ObservacaoCupom, p.Y, totalWidth);

            p.Y = g.ClienteDraw(dados, p.Y, fNormal, totalWidth);

            if (string.IsNullOrWhiteSpace(dados.Observacoes) == false)
                p.Y += g.DrawText(dados.Observacoes, null, fNormal, p.Y, totalWidth);

            if (!string.IsNullOrWhiteSpace(dados.Entregador))
                p.Y += g.DrawText($"ENTREGADOR: {dados.Entregador.ToUpper()}", null, fNormal, p.Y, totalWidth);

            p.Y += espaco;
            p.Y += g.DrawSeparador(p.Y, totalWidth);

            g.DrawString("VL ITEM", fNormalB, Brushes.Black, new Rectangle(0, p.Y, totalWidth, 20), new StringFormat() { Alignment = StringAlignment.Far });
            p.Y += g.DrawText("#|DESC/ITEM QTDxVL UNIT", null, fNormalB, p.Y, totalWidth);

            foreach (var iv in dados.ProdutosValores)
                p.Y += g.DrawItemValor(iv, fNormal, p.Y, totalWidth);

            p.Y += espaco;

            p.Y += g.DrawSeparador(p.Y, totalWidth);

            p.Y = g.DrawPagamentos(dados, p.Y, fNormal, fNormalB, totalWidth);

            if (string.IsNullOrWhiteSpace(dados.DataEmissao) == false)
            {
                p.X = 0;
                p.Y += espaco;
                g.DrawString($"Abertura pedido: {dados.DataEmissao}\nEmissão: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}", fNormal, Brushes.Black, new Rectangle(p, new Size(totalWidth, 40)), new StringFormat() { Alignment = StringAlignment.Center });
                p.Y += 40;
            }

            p.Y += g.DrawSeparador(p.Y, totalWidth);
            p.Y += g.DrawCenter(fNormal, "PDVSeven www.pdvseven.com.br", p.Y, totalWidth);

            return p.Y;
        }

        public static int DrawPagamentos(this Graphics g, ImpressaoHelper dados, int Y, Font fNormal, Font fNormalB, int totalWidth)
        {
            int espaco = Constantes.Espacamento;
            var valorTotalAPagar = Convert.ToDecimal(dados.ValorTotalAPagar);

            if (dados.ValorTotalAPagar != dados.ValorTotal)
            {
                Y += espaco + g.DrawItemValor(new ItemValor("Sub-Total:", dados.ValorTotal), fNormal, Y, totalWidth);
            }

            if (!string.IsNullOrWhiteSpace(dados.ValorEntrega))
                Y += espaco + g.DrawItemValor(new ItemValor("Taxa de Entrega:", dados.ValorEntrega), fNormal, Y, totalWidth);

            if (!string.IsNullOrWhiteSpace(dados.ValorAcrescimo))
                Y += espaco + g.DrawItemValor(new ItemValor(NomeTaxaServico + ":", dados.ValorAcrescimo), fNormal, Y, totalWidth);

            if (!string.IsNullOrWhiteSpace(dados.ContaCliente))
                Y += espaco + g.DrawItemValor(new ItemValor("Débito de Saldo do Cliente: ", dados.ContaCliente), fNormal, Y, totalWidth);
            else if (!string.IsNullOrWhiteSpace(dados.ValorDesconto))
                Y += espaco + g.DrawItemValor(new ItemValor("Desconto:", dados.ValorDesconto), fNormal, Y, totalWidth);

            decimal totalPago = 0;
            if (dados.PagamentosValores.Length > 0)
            {
                Y += espaco + g.DrawItemValor(new ItemValor("Pagamentos realizados:"), fNormal, Y, totalWidth);
                foreach (var pagamento in dados.PagamentosValores)
                {
                    pagamento.Nivel = 1;
                    totalPago += Convert.ToDecimal(pagamento.Valor);
                    Y += espaco + g.DrawItemValor(pagamento, fNormal, Y, totalWidth);
                }
            }

            if (valorTotalAPagar > 0)
                Y += espaco + g.DrawItemValor(new ItemValor("Total a Pagar:", dados.ValorTotalAPagar), fNormalB, Y, totalWidth);
            else if (valorTotalAPagar == 0)
                Y += espaco + g.DrawItemValor(new ItemValor("Total Pago:", totalPago.ToString("#0.00")), fNormalB, Y, totalWidth);
            else
                Y += espaco + g.DrawItemValor(new ItemValor("Troco:", (valorTotalAPagar * -1).ToString("#0.00")), fNormalB, Y, totalWidth);

            if (!string.IsNullOrWhiteSpace(dados.SaldoCliente))
                Y += espaco + g.DrawItemValor(new ItemValor("Saldo do Cliente: ", dados.SaldoCliente), fNormal, Y, totalWidth);

            if (valorTotalAPagar > 0 && dados.NumeroPessoas > 1)
            {
                var totalPessoa = Math.Round(valorTotalAPagar / dados.NumeroPessoas, 2, MidpointRounding.AwayFromZero);
                Y += espaco + g.DrawItemValor(new ItemValor($"Total por pessoa ({dados.NumeroPessoas} pessoas)", totalPessoa.ToString("N2")), fNormal, Y, totalWidth);
            }

            return Y;
        }
    }
}
