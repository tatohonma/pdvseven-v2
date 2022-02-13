using a7D.PDV.AutoAtendimento.UI.Services;
using a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace a7D.PDV.AutoAtendimento.UI.Paginas
{
    public partial class ComandaClienteCreditoPage : Page, INavPageClick
    {
        public static int Selecionado { get; internal set; }
        public static int IDCliente { get; internal set; }
        public static int ComandaNumero { get; internal set; }
        private Button lastBtn = null;
        private Produto produtoComanda = null;

        const string buttonNames = "Credito";

        public ComandaClienteCreditoPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Numero.Text = $"{App.Pedido.Comanda_ClienteNome}";
            if (App.Pedido.Comanda_Numero == 0 // É comanda nova
             && PdvServices.IDProduto_NovaComanda > 0
             && !App.Pedido.Contem(PdvServices.IDProduto_NovaComanda)) // E ainda não foi comprada
            {
                produtoComanda = App.Produtos.LoadProduto(PdvServices.IDProduto_NovaComanda);
            }

            CarregarCreditos();
        }

        public void Buttom_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            string id = btn.Name.Substring(buttonNames.Length);
            Selecionado = int.Parse(id);

            if (Selecionado == 4) // Outros valores
            {
                App.Navigate<ComandaClienteCreditoValorPage>();
            }
            else
            {
                App.Navigate<ComandaClienteCreditoPage>();
            }
        }

        private void CarregarCreditos()
        {
            var btns = LayoutServices.BindButtons(this.canvas, buttonNames, Selecionado);

            var listaProdutoCredito = App.Produtos
                .TodosProdutos()
                .Where(p => p.IDTipoProduto == PdvServices.IDCategoriaProduto_Credito)
                .OrderBy(p => p.ValorUnitario)
                .ToList();

            var listaProdutosCreditoComValor = listaProdutoCredito.Where(lpc => lpc.ValorUnitario > 0).ToList();
            int i;
            for (i = 0; i < btns.Count; i++)
            {
                if (i < listaProdutosCreditoComValor.Count)
                {
                    btns[i].Content = "R$ " + listaProdutosCreditoComValor[i].ValorUnitario;
                    btns[i].Tag = listaProdutosCreditoComValor[i].IDProduto;
                    if (Selecionado == i + 1)
                        lastBtn = btns[i];
                }
                else
                {
                    btns[i].Visibility = Visibility.Hidden;
                }
            }

            var produtoCreditoValorZero = listaProdutoCredito.FirstOrDefault(lpc => lpc.ValorUnitario == 0);
            if (produtoCreditoValorZero != null)
            {
                btns[--i].Content = "Outro valor";
                btns[i].Tag = produtoCreditoValorZero.IDProduto;
                btns[i].Visibility = Visibility.Visible;
            }

            if (produtoComanda != null)
                NovaComanda.Text = string.Format(NovaComanda.Text.ToString(), produtoComanda.ValorUnitario);
            else
                NovaComanda.Visibility = Visibility.Hidden;
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Selecionado = 0;
            App.Navigate<ComandaLeitoraPage>();
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (lastBtn == null)
            {
                ModalSimNaoWindow.Show("Selecione um valor de crédito");
                return;
            }

            if (!int.TryParse(lastBtn.Tag?.ToString(), out int idCredito))
            {
                ModalSimNaoWindow.Show("TAG inválido como código de credito");
                return;
            }

            var credito = App.Produtos.LoadProduto(idCredito);
            if (credito == null)
                ModalSimNaoWindow.Show("Código do crédito inválido na aplicação");
            else
            {
                Selecionado = 0;

                if (produtoComanda != null)
                    App.Pedido.Adicionar(produtoComanda);


                App.Pedido.Adicionar(credito);

                App.Navigate<VendaResumoPage>();
            }
        }

        private void Credito_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (LayoutServices.BackgroundImage2 == null)
                return;

            if (lastBtn != null)
                lastBtn.Background = Brushes.Transparent;

            lastBtn = btn;
            // https://social.msdn.microsoft.com/Forums/vstudio/en-US/6d2380ca-4eda-4247-8892-27260fbe5517/how-can-i-make-all-white-ffffffff-pixel-in-an-imageimagebrush-to-be-transparent?forum=wpf
            int pixelWidth = (int)btn.Width;
            int pixelHeight = (int)btn.Height;
            int Stride = pixelWidth * 4;
            var imgSource = LayoutServices.BackgroundImage2;
            byte[] pixels = new byte[pixelHeight * Stride];
            var rc = new Int32Rect((int)Canvas.GetLeft(btn), (int)Canvas.GetTop(btn), pixelWidth, pixelHeight);
            imgSource.CopyPixels(rc, pixels, Stride, 0);
            byte TransparentByte = byte.Parse("0");
            byte Byte255 = byte.Parse("255");
            int N = pixelWidth * pixelHeight;
            //Operate the pixels directly
            for (int i = 0; i < N; i++)
            {
                byte a = pixels[i * 4];
                byte b = pixels[i * 4 + 1];
                byte c = pixels[i * 4 + 2];
                byte d = pixels[i * 4 + 3];
                if (a == Byte255 && b == Byte255 && c == Byte255 && d == Byte255)
                {
                    pixels[i * 4] = TransparentByte;
                    pixels[i * 4 + 1] = TransparentByte;
                    pixels[i * 4 + 2] = TransparentByte;
                    pixels[i * 4 + 3] = TransparentByte;
                }
            }

            WriteableBitmap writeableBitmap = new WriteableBitmap(pixelWidth, pixelHeight, 96, 96, PixelFormats.Pbgra32, BitmapPalettes.Halftone256Transparent);
            writeableBitmap.WritePixels(new Int32Rect(0, 0, pixelWidth, pixelHeight), pixels, Stride, 0);
            btn.Background = new ImageBrush(writeableBitmap);

        }

        private void Teclado_onChange(object sender, TextChangedEventArgs e)
        {
            return;
        }
        }
    }
