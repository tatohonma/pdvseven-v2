using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace a7D.PDV.AutoAtendimento.UI.Controles
{
    public partial class Produto1 : UserControl
    {
        public Produto1()
        {
            InitializeComponent();
        }

        public Produto1(string nome, decimal? valor, bool? disponivel, string imagem) : this()
        {
            if (imagem != null)
                Background = new ImageBrush(new BitmapImage(new Uri(imagem)));

            circulo.Fill = LayoutProdutoServices.CirculoFill;

            this.Nome.Text = nome;
            if (valor.HasValue && disponivel == true)
                this.Preco.Text = "R$ " + valor.Value.ToString("N2");
            else
            {
                this.Preco.Text = "indisponível";
                this.Preco.FontSize = 15;
            }
        }
    }
}
