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
using System.Text;

namespace a7D.PDV.BLL.Services
{
    public static class ExpedicaoServices
    {
        public static List<string> ComprovanteEntrega(PedidoInformation pedido)
        {
            var relatorio = new List<string>();
            relatorio.Add("*** COMPROVANTE DE ENTREGA ***");
            relatorio.Add(string.Empty.PadLeft(Constantes.Colunas, '-'));
            relatorio.Add($"DATA {pedido.DtPedido.Value.ToString("dd/MM/yyyy HH:mm:ss")}");

            if (pedido.OrigemPedido != null && pedido.OrigemPedido.IDOrigemPedido == (int)EOrigemPedido.ifood)
            {
                TagInformation tagDisplay = Tag.Carregar(pedido.GUIDIdentificacao, "ifood-displayId");

                relatorio.Add($"PEDIDO N {pedido.IDPedido.Value} IFOOD {tagDisplay.Valor}");
            }
            else
            {
                relatorio.Add($"PEDIDO N {pedido.IDPedido.Value}");
            }

            relatorio.Add($"ENTREGADOR {pedido.Entregador?.Nome}");
            relatorio.Add($"TAXA DE ENTREGA {pedido.TaxaEntrega?.Nome}");
            relatorio.Add($"VALOR DA ENTREGA {pedido.ValorEntrega?.ToString("R$ #,##0.00")}");
            relatorio.Add(string.Empty.PadLeft(Constantes.Colunas, '-'));

            return relatorio;
        }

        public static ImpressaoHelper PrepararVia(PedidoInformation pedido, int colunas = Constantes.Colunas)
        {
            var h = new ImpressaoHelper
            {
                NomeFantasia = ConfiguracoesSistema.Valores.NomeFantasia,
                DataEmissao = pedido.DtPedido?.ToString("dd/MM/yyyy HH:mm:ss"),
                IdPedido = pedido.IDPedido.Value.ToString(),
                ObservacaoCupom = pedido.ObservacaoCupom
            };

            if (pedido.TipoPedido.TipoPedido == ETipoPedido.Delivery)
            {
                var agrupado = pedido.ListaProduto
                    .OrderBy(p => p.PedidoProdutoPai)
                    .ToArray();

                h.ProdutosValores = FillProdutos(agrupado);
            }
            else
            {
                h.Viagem = true;
                var viagem = pedido
                    .ListaProduto
                    .Where(p => p.Viagem == true)
                    .OrderBy(p => p.DtInclusao)
                    .ToArray();

                if (viagem.Length == 0)
                    return null;

                // Agrupa itens em grupos de pedidos distintos (10 segundos)
                int sequencia = 1;
                for (int i = 0; i < viagem.Length; i++)
                {
                    if (i > 0 && viagem[i].DtInclusao.Value.Subtract(viagem[i - 1].DtInclusao.Value).TotalSeconds > 10)
                        sequencia++;

                    viagem[i].sequenciaPedido = sequencia;
                }

                if (ConfiguracoesSistema.Valores.LayoutViaExpedicao == "TOTAL")
                {
                    var agrupado = viagem
                        .OrderBy(p => p.PedidoProdutoPai)
                        .ToArray();

                    h.ProdutosValores = FillProdutos(agrupado);
                }
                else
                {
                    AgrupadoProdutosValores[] agrupado;
                    if (ConfiguracoesSistema.Valores.LayoutViaExpedicao == "NOVOS" && sequencia > 1)
                    {
                        agrupado = new AgrupadoProdutosValores[]{
                            new AgrupadoProdutosValores()
                            {
                                Titulo = "NOVOS ITENS",
                                 ProdutosValores= FillProdutos(viagem
                                    .Where(p => p.sequenciaPedido == sequencia)
                                    .OrderBy(p => p.PedidoProdutoPai)
                                    .ToArray())
                            },
                            new AgrupadoProdutosValores()
                            {
                                Titulo= "TODOS ITENS",
                                ProdutosValores=FillProdutos(viagem
                                    .OrderBy(p => p.PedidoProdutoPai)
                                    .ToArray())
                            }
                        };
                    }
                    else // PEDIDO
                    {
                        agrupado = viagem.GroupBy(p => p.sequenciaPedido,
                           (k, v) => new AgrupadoProdutosValores()
                           {
                               Titulo = v.First().DtInclusao.Value.ToString("dd/MM HH:mm"),
                               ProdutosValores = FillProdutos(v.OrderBy(p => p.PedidoProdutoPai).ToArray())
                           })
                           .ToArray();
                    }

                    if (agrupado.Length == 1)
                        h.ProdutosValores = agrupado[0].ProdutosValores;
                    else
                        h.ProdutosAgrupados = agrupado;
                }
            }

            if (pedido.TipoPedido.TipoPedido == ETipoPedido.Mesa)
            {
                h.plain.AppendLine("EXPEDIÇÃO");
                h.Identificacao = "EXPEDIÇÃO MESA " + Mesa.CarregarPorGUID(pedido.GUIDIdentificacao).Numero.ToString();
                h.plain.AppendLine(h.Identificacao);
            }
            else if (pedido.TipoPedido.TipoPedido == ETipoPedido.Comanda)
            {
                h.plain.AppendLine("EXPEDIÇÃO");
                h.Identificacao = "EXPEDIÇÃO COMANDA " + Comanda.CarregarPorGUID(pedido.GUIDIdentificacao).Numero.ToString();
                h.plain.AppendLine(h.Identificacao);
            }
            else if (pedido.TipoPedido.TipoPedido == ETipoPedido.Balcao)
            {
                h.plain.AppendLine("EXPEDIÇÃO");
                h.Identificacao = "EXPEDIÇÃO BALCÃO";
                h.plain.AppendLine(h.Identificacao);
            }
            else if (pedido.TipoPedido.TipoPedido == ETipoPedido.Delivery)
            {
                if (pedido.OrigemPedido != null && pedido.OrigemPedido.IDOrigemPedido == (int)EOrigemPedido.ifood)
                {
                    TagInformation tagDisplayId = Tag.Carregar(pedido.GUIDIdentificacao, "ifood-displayId");

                    h.Identificacao = "IFOOD " + tagDisplayId.Valor;
                    h.plain.AppendLine("DELIVERY " + h.IdPedido + " IFOOD " + tagDisplayId.Valor);
                }
                else
                {
                    h.Identificacao = "DELIVERY";
                    h.plain.AppendLine("DELIVERY " + h.IdPedido);
                }
            }

            Cliente.Fill(pedido, h);

            ContaServices.TotaisFill(pedido, h);

            return h;
        }

        private static ItemValor[] FillProdutos(PedidoProdutoInformation[] agrupado)
        {
            int lastHash = 0;
            ImpressaoOrdemProducao lastProd = null;
            var pv = new List<ItemValor>();
            for (var i = 0; i < agrupado.Count(); i++)
            {
                var pedProd = new ImpressaoOrdemProducao(null, agrupado[i]);
                var itens = FillProdutos(pedProd);
                var hash = GetHash(itens);
                if (lastProd != null && itens.Count == 1 && hash == lastHash) // Itens simples
                {
                    pv.RemoveAt(pv.Count - 1); // Remove anterior
                    lastProd.Quantidade += pedProd.Quantidade;
                    itens = FillProdutos(lastProd);
                }
                else
                {
                    lastProd = pedProd;
                    lastHash = hash;
                }
                pv.AddRange(itens);
            }
            return pv.ToArray();
        }

        private static List<ItemValor> FillProdutos(ImpressaoOrdemProducao pp)
        {
            string qtd;
            var pv = new List<ItemValor>();
            if (pp.Quantidade == (int)pp.Quantidade)
                qtd = ((int)pp.Quantidade).ToString();
            else
                qtd = pp.Quantidade.ToString();

            if (pp.Nivel == 0)
                qtd += " x ";
            else if (qtd != "1")
                qtd = "> " + qtd + " x ";
            else
                qtd = "> ";

            pv.Add(new ItemValor(qtd + pp.Produto.Nome, nivel: pp.Nivel));
            if (!string.IsNullOrEmpty(pp.Notas))
                pv.Add(new ItemValor(pp.Notas, nivel: pp.Nivel + 3, multLines: true));

            if (pp.Modificacoes?.Count > 0)
            {
                foreach (var pm in pp.Modificacoes)
                    pv.AddRange(FillProdutos(pm));
            }

            return pv;
        }

        private static int GetHash(List<ItemValor> itens)
        {
            string result = "";
            foreach (var i in itens)
                result += i.ToString();

            return result.GetHashCode();
        }

        private static int DesenhaVia(Graphics g, ImpressaoHelper dados, int totalWidth)
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

            p.Y += g.DrawSeparador(p.Y, totalWidth);

            p.X = 0;
            p.Y += g.DrawText($"PEDIDO {dados.IdPedido}", dados.Identificacao, fNormalB, p.Y, totalWidth);

            if (!string.IsNullOrEmpty(dados.ObservacaoCupom))
                p.Y += g.DrawCenter(fNormalB, dados.ObservacaoCupom, p.Y, totalWidth);

            p.Y = g.ClienteDraw(dados, p.Y, fNormal, totalWidth);

            if (!string.IsNullOrWhiteSpace(dados.Entregador))
            {
                p.Y += espaco;
                p.Y += g.DrawText($"ENTREGADOR: {dados.Entregador.ToUpper()}", null, fNormal, p.Y, totalWidth);
            }

            p.Y += espaco;
            p.Y += g.DrawSeparador(p.Y, totalWidth);

            if (dados.ProdutosAgrupados != null)
            {
                foreach (var pg in dados.ProdutosAgrupados)
                {
                    p.Y += g.DrawCenter(fNormalB, pg.Titulo, p.Y, totalWidth);
                    foreach (var pv in pg.ProdutosValores)
                        p.Y += g.DrawItemValor(pv, fNormal, p.Y, totalWidth);

                    p.Y += espaco;
                    p.Y += g.DrawSeparador(p.Y, totalWidth);
                }
            }
            else if (dados.ProdutosValores != null)
            {
                foreach (var pv in dados.ProdutosValores)
                    p.Y += g.DrawItemValor(pv, fNormal, p.Y, totalWidth);

                p.Y += espaco;
                p.Y += g.DrawSeparador(p.Y, totalWidth);
            }

            if (!dados.Viagem)
                p.Y = g.DrawPagamentos(dados, p.Y, fNormal, fNormalB, totalWidth);

            g.DrawString($"Abertura pedido: {dados.DataEmissao}\nEmissão: {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}", fNormal, Brushes.Black, new Rectangle(p, new Size(totalWidth, 40)), new StringFormat() { Alignment = StringAlignment.Center });
            p.Y += 40;
            p.Y += g.DrawSeparador(p.Y, totalWidth);
            p.Y += g.DrawCenter(fNormal, "PDVSeven www.pdvseven.com.br", p.Y, totalWidth);

            return p.Y;
        }

        public static byte[] ImagemVia(PedidoInformation pedido, int width)
        {
            if (width < ImpressoraWindows.Largura)// Tamanho minimo e padrão
                width = ImpressoraWindows.Largura;

            var bmp = new Bitmap(width, 10000);
            Graphics g = Graphics.FromImage(bmp);
            var dados = PrepararVia(pedido);
            int height = DesenhaVia(g, dados, width);
            return ImageUtil.ReduzEtransforma(bmp, height);
        }

        public static bool ImprimirVia(string nomeImpressora, PedidoInformation pedido)
        {
            var dados = PrepararVia(pedido);
            if (dados == null)
                return false;

            ImpressoraWindows.Imprimir(nomeImpressora,
                (s, a) => DesenhaVia(a.Graphics, dados, ImpressoraWindows.Largura), true);

            return true;
        }
    }
}
