using a7D.PDV.AutoAtendimento.UI.Services;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace a7D.PDV.AutoAtendimento.UI.Controles
{
    public partial class Produto2 : UserControl
    {
        public Produto2()
        {
            InitializeComponent();
        }

        public Produto2(string nome, string descricao, decimal? valor, bool? disponivel, string imagem) : this()
        {
            if (imagem != null)
            {
                Foto.Source = new BitmapImage(new Uri(imagem));
            }

            this.Nome.Text = nome;

            this.Decricao.Text = descricao;

            if (valor.HasValue && disponivel == true)
            {
                if (valor.Value > 0)
                    this.Preco.Text = "R$ " + valor.Value.ToString("N2");
                else
                    this.Preco.Text = "";
            }
            else
            {
                this.Preco.Text = "indisponível";
                this.Preco.FontSize = 15;
            }
        }

        private void Item_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LayoutServices.Bind((Grid)sender);
        }
    }
}
