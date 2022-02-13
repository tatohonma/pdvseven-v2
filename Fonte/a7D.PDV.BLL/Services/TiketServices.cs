using a7D.PDV.Ativacao.Shared.Services;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace a7D.PDV.BLL.Services
{
    public static class TiketServices
    {
        static Font fntPedido = new Font("Arial Narrow", 150, FontStyle.Bold);
        static Font fntTitulo = new Font("Arial Narrow", 70, FontStyle.Bold);
        static Font fntNormal = new Font("Arial Narrow", 50);
        static Font fntModificacao = new Font("Arial Narrow", 42);
        static Font fntNotas = new Font("Arial Narrow", 40, FontStyle.Italic);
        static Font fntPequeno = new Font("Arial Narrow", 36);
        static Font fntPDV = new Font("Arial Narrow", 30);

        public static List<string> TicketPrePago(PedidoInformation pedido, PedidoProdutoInformation item, int numeroItem)
        {
            String titulo = ConfiguracoesSistema.Valores.MsgTicketPrePago;

            var relatorio = new List<string>();
            relatorio.Add(titulo);
            relatorio.Add("********* Ticket Pré-Pago *********");
            relatorio.Add("");

            relatorio.Add("PEDIDO " + pedido.IDPedido.Value.ToString("00000") + "." + numeroItem.ToString());
            relatorio.Add("Data: " + pedido.DtPedido.Value);
            relatorio.Add("");

            string nome = "";
            for (int i = 0; i <= item.Produto.Nome.Length / 45; i++)
            {
                if (item.Produto.Nome.Substring(i * 45).Length < 45)
                    nome += item.Produto.Nome.Substring(i * 45) + "\n";
                else
                    nome += item.Produto.Nome.Substring(i * 45, 45) + "\n";
            }

            relatorio.Add(nome);

            if (!String.IsNullOrEmpty(item.Notas))
                relatorio.Add("   " + item.Notas);

            if (item.ListaModificacao != null)
            {
                foreach (var modificacao in item.ListaModificacao)
                    relatorio.Add(" - " + modificacao.Produto.Nome);
            }

            relatorio.Add("");

            relatorio.Add("***********************************");
            relatorio.Add("** PDVSeven  www.pdvseven.com.br **");
            relatorio.Add("***********************************");

            return relatorio;
        }

        private static Bitmap TicketPadrao(PedidoInformation pedido, int item, PDVInformation pdv)
        {
            var bmp = new Bitmap(840, 1000);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            var rc = new Rectangle(0, 0, 840, 600);
            var center = new StringFormat() { Alignment = StringAlignment.Center };
            int y = 0;

            // (Titulo)
            string titulo = ConfiguracaoBD.ValorOuPadrao(EConfig.TituloTicketPrePago, pdv);
            if (!string.IsNullOrEmpty(titulo))
            {
                rc.Location = new Point(0, y);
                g.DrawString(titulo, fntTitulo, Brushes.Black, rc, center);
                y += (int)g.MeasureString(titulo, fntTitulo, rc.Width).Height;
                y += 20;
            }

            // (Pedido)
            string cPedido = "Pedido " + pedido.IDPedido.Value.ToString("000000") + "." + (item + 1).ToString();
            rc.Location = new Point(0, y);
            g.DrawString(cPedido, fntNormal, Brushes.Black, rc, center);
            y += (int)g.MeasureString(cPedido, fntNormal, rc.Width).Height;

            // (Produto)
            var pp = pedido.ListaProduto[item];
            DesenhaProdutoCompleto(g, ref y, 0, pp);

            y += 20;

            // (Terminal, Data e Hora)
            var pdvItem = pp.PDV; // PDV onde foi realizada a venda do item!
            var datahora = $"Terminal: {pdvItem.IDPDV}  Data: {pedido.DtPedido.Value.ToString("dd/MM/yyyy")}  Hora: {pedido.DtPedido.Value.ToString("HH:mm")}";
            rc.Location = new Point(0, y);
            g.DrawString(datahora, fntPequeno, Brushes.Black, rc, center);
            y += (int)g.MeasureString(datahora, fntPequeno, rc.Width).Height;
            y += 10;

            rc.Location = new Point(0, y);
            y += Copyrights(g, rc);

            return ReduzBitmap(bmp, y);
        }

        private static void DesenhaProdutoCompleto(Graphics g, ref int y, int nivel, PedidoProdutoInformation pp)
        {
            string produto;
            if (nivel == 0)
                produto = (int)(pp.Quantidade) + " " + pp.Produto.Nome;
            else
                produto = pp.Produto.Nome;

            var rc = new Rectangle(nivel * 50, y, 840, 600);
            g.DrawString(produto, nivel == 0 ? fntNormal : fntModificacao, Brushes.Black, rc);
            y += (int)g.MeasureString(produto, nivel == 0 ? fntNormal : fntModificacao, rc.Width).Height;

            if (!String.IsNullOrEmpty(pp.Notas))
            {
                string notas = pp.Notas;
                rc.Location = new Point(nivel * 50, y);
                g.DrawString(notas, fntNotas, Brushes.Black, rc);
                y += (int)g.MeasureString(notas, fntNotas, rc.Width).Height;
            }

            if (pp.ListaModificacao == null || pp.ListaModificacao.Count == 0)
                return;

            foreach (var prod in pp.ListaModificacao)
                DesenhaProdutoCompleto(g, ref y, nivel + 1, prod);
        }

        private static Bitmap TicketPedido2Digitos(PedidoInformation pedido, int item, PDVInformation pdv)
        {
            if (item > 0)
                return null;

            var bmp = new Bitmap(840, 550);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            var rc = new Rectangle(0, 0, 840, 550);
            var center = new StringFormat() { Alignment = StringAlignment.Center };
            int y = 0;

            // (Titulo Pedido)
            string pedidoTexto = "Pedido";
            rc.Location = new Point(0, y);
            g.DrawString(pedidoTexto, fntPequeno, Brushes.Black, rc, center);
            y += (int)g.MeasureString(pedidoTexto, fntPequeno, rc.Width).Height;

            // (Ultimos 2 digitos do pedido)
            string cPedido = pedido.IDPedido.Value.ToString("000000").Substring(4, 2);
            rc.Location = new Point(0, y);
            g.DrawString(cPedido, fntPedido, Brushes.Black, rc, center);
            y += (int)g.MeasureString(cPedido, fntPedido, rc.Width).Height;

            // (Terminal, Data e Hora)
            var pdvItem = pedido.ListaProduto[item].PDV; // PDV onde foi realizada a venda do item!
            var datahora = $"Terminal: {pdvItem.IDPDV}  Data: {pedido.DtPedido.Value.ToString("dd/MM/yyyy")}  Hora: {pedido.DtPedido.Value.ToString("HH:mm")}";
            rc.Location = new Point(0, y);
            g.DrawString(datahora, fntPequeno, Brushes.Black, rc, center);
            y += (int)g.MeasureString(datahora, fntPequeno, rc.Width).Height;
            y += 10;

            rc.Location = new Point(0, y);
            y += Copyrights(g, rc);

            return ReduzBitmap(bmp, y);
        }

        private static Bitmap TicketCompletoPorProduto(PedidoInformation pedido, int item, PDVInformation pdv, bool qrCode)
        {
            if (item >= pedido.ListaProduto.Count)
                return null;

            var bmp = new Bitmap(840, 1200);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            var rc = new Rectangle(0, 0, 840, 1000);
            var center = new StringFormat() { Alignment = StringAlignment.Center };
            int y = 0;

            // (Titulo)
            string titulo = BLL.ConfiguracaoBD.ValorOuPadrao(EConfig.TituloTicketPrePago, pdv);
            if (!string.IsNullOrEmpty(titulo))
            {
                rc.Location = new Point(0, y);
                g.DrawString(titulo, fntTitulo, Brushes.Black, rc, center);
                y += (int)g.MeasureString(titulo, fntTitulo, rc.Width).Height;
                y += 20;
            }

            // (Produto)
            var produto = pedido.ListaProduto[item];
            string produtoQtdNome = (int)(produto.Quantidade) + " " + pedido.ListaProduto[item].Produto.Nome;
            rc.Location = new Point(0, y);
            g.DrawString(produtoQtdNome, fntNormal, Brushes.Black, rc, center);
            y += (int)g.MeasureString(produtoQtdNome, fntNormal, rc.Width).Height;

            var ticket = EF.Repositorio.Carregar<tbTicket>(p => p.IDPedidoProduto == produto.IDPedidoProduto);
            if (ticket == null)
            {
                ticket = new tbTicket()
                {
                    IDPedidoProduto = produto.IDPedidoProduto.Value,
                };
                EF.Repositorio.Inserir(ticket);
            }

            decimal valorItem = produto.ValorTotal;
            string modificacoes = "";
            if (produto.ListaModificacao.Count > 0)
            {
                foreach (var mod in produto.ListaModificacao)
                {
                    modificacoes += (mod.Quantidade == 1 ? "" : (mod.Quantidade + " ")) + mod.Produto.Nome + ", ";
                    valorItem += Math.Truncate(produto.Quantidade.Value * mod.ValorTotal * 100m) / 100m;
                }
                modificacoes = modificacoes.Substring(0, modificacoes.Length - 2);

                rc.Location = new Point(0, y);
                g.DrawString(modificacoes, fntPequeno, Brushes.Black, rc, center);
                y += (int)g.MeasureString(modificacoes, fntNormal, rc.Width).Height;
            }

            // (Preco)
            string preco = "R$ " + valorItem.ToString("N2");
            rc.Location = new Point(0, y);
            g.DrawString(preco, fntNormal, Brushes.Black, rc, center);
            y += (int)g.MeasureString(preco, fntNormal, rc.Width).Height;
            y += 10;

            // (Imagem Código de Barras)
            string codigoBarras =
                pedido.IDPedido.Value.ToString("000000") +
                produto.IDPedidoProduto.Value.ToString("0000000");

            Bitmap img;
            if (qrCode)
            {
                var qrcodeEncoder = new QRCodeEncoder
                {
                    QRCodeScale = 8,
                    CharacterSet = "UTF-8",
                    QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L
                };
                var dados = // Só '\n' pois android irá fazer split com '\n'
                    codigoBarras + "\n" +
                    produtoQtdNome + "\n" +
                    modificacoes + "\n" +
                    PDVSecurity.CalculateMD5Hash(codigoBarras);

                img = qrcodeEncoder.Encode(dados);
            }
            else
                img = BarCodeServices.BarCodeImage(codigoBarras, 10, 70);

            g.DrawImage(img, (rc.Width - img.Width) / 2, y);
            y += img.Height + 20;

            if (!qrCode)
            {
                // (Texto do Código de Barras)
                string codigoEspacado = "";
                for (int n = 0; n < codigoBarras.Length; n++)
                    codigoEspacado += codigoBarras[n] + " ";
                rc.Location = new Point(0, y);
                g.DrawString(codigoEspacado, fntPequeno, Brushes.Black, rc, center);
                y += (int)g.MeasureString(codigoEspacado, fntPequeno, rc.Width).Height;
                y += 20;
            }

            // (Terminal)
            var pdvItem = pedido.ListaProduto[item].PDV; // PDV onde foi realizada a venda do item!
            var terminal = $"Terminal: {pdvItem.IDPDV} - {pdvItem.Nome}";
            if (pdv.TipoPDV.Tipo == ETipoPDV.CAIXA)
                terminal += "    Usuário: " + pedido.ListaProduto[item].Usuario.IDUsuario;
            rc.Location = new Point(0, y);
            g.DrawString(terminal, fntPequeno, Brushes.Black, rc, center);
            y += (int)g.MeasureString(terminal, fntPequeno, rc.Width).Height;

            // (Data e Hora)
            var datahora = $"Data: {pedido.DtPedido.Value.ToString("dd/MM/yyyy")}    Hora: {pedido.DtPedido.Value.ToString("HH:mm")}";
            rc.Location = new Point(0, y);
            g.DrawString(datahora, fntPequeno, Brushes.Black, rc, center);
            y += (int)g.MeasureString(datahora, fntPequeno, rc.Width).Height;

            // (Item/Total)
            var impressao = $"Impressão: {(item + 1)}/{pedido.ListaProduto.Count}";
            if (pedido.ListaPagamento.Count > 0)
                impressao += "    Pago: " + pedido.ListaPagamento[0].TipoPagamento?.Nome;
            rc.Location = new Point(0, y);
            g.DrawString(impressao, fntPequeno, Brushes.Black, rc, center);
            y += (int)g.MeasureString(impressao, fntPequeno, rc.Width).Height;
            y += 10;

            // (Validade)
            var validade = ConfiguracaoBD.ValorOuPadrao(EConfig.ValidadeTicketPrePago, pdv);
            if (!string.IsNullOrEmpty(validade))
            {
                rc.Location = new Point(0, y);
                g.DrawString(validade, fntPequeno, Brushes.Black, rc, center);
                y += (int)g.MeasureString(validade, fntPequeno, rc.Width).Height;
            }

            // (Mensagem)
            var mensagem = ConfiguracaoBD.ValorOuPadrao(EConfig.RodapeTicketPrePago, pdv);
            if (!string.IsNullOrEmpty(mensagem))
            {
                //"Proibido a venda de bebidas alcoolicas para menores de 18 anos";
                rc.Location = new Point(0, y);
                g.DrawString(mensagem, fntPequeno, Brushes.Black, rc, center);
                y += (int)g.MeasureString(mensagem, fntPequeno, rc.Width).Height;
                y += 10;
            }

            rc.Location = new Point(0, y);
            y += Copyrights(g, rc);

            return ReduzBitmap(bmp, y);
        }

        public static Bitmap Gerar(PedidoInformation pedido, int item, string tipo = null)
        {
            Bitmap bmp = null;
            if (pedido.ListaProduto.Count == 0)
                return null;

            var pdv = pedido.ListaProduto[0].PDV;
            if (pdv == null)
                return null;

            var pdvInfo = PDV.Carregar(pdv.IDPDV.Value);

            if (tipo == null)
                tipo = ConfiguracaoBD.ValorOuPadrao(EConfig.GerarTicketPrePago, pdvInfo);

            if (tipo == null)
                return null;

            if (tipo == "1")
                bmp = TicketPadrao(pedido, item, pdvInfo);
            else if (tipo == "2")
                bmp = TicketPedido2Digitos(pedido, item, pdvInfo);
            else if (tipo == "3")
                bmp = TicketCompletoPorProduto(pedido, item, pdvInfo, false);
            else if (tipo == "4")
                bmp = TicketCompletoPorProduto(pedido, item, pdvInfo, true);
            else
                bmp = null;

            return bmp;
        }

        private static int Copyrights(Graphics g, Rectangle rc)
        {
            // (Linha e Copyrights)
            g.DrawLine(Pens.Black, 0, rc.Top - 5, rc.Width, rc.Top - 5);

            var copyrights = "* PDVSeven - www.pdvseven.com.br *\nSistemas para Restaurantes e Bares";

            g.DrawString(copyrights, fntPDV, Brushes.Black, rc,
                new StringFormat() { Alignment = StringAlignment.Center });

            return (int)g.MeasureString(copyrights, fntPDV, rc.Width).Height + 5;
        }

        private static Bitmap ReduzBitmap(Bitmap bmp, int y)
        {
            var bmp2 = new Bitmap(bmp.Width, y);
            var g2 = Graphics.FromImage(bmp2);
            g2.DrawImage(bmp, 0, 0);
            g2.Flush();
            return bmp2;
        }
    }
}
