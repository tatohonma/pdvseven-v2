using a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    public class VendaItem
    {
        public VendaItem(VendaItem item, int add)
        {
            Quantidade = item.Quantidade + add;
            Produto = item.Produto;
            ValorUnitario = item.ValorUnitario;
            ValorModificacoes = item.ValorModificacoes;
            Modificacoes = item.Modificacoes;
            DescricaoModificacoes = item.DescricaoModificacoes;
        }

        public VendaItem(decimal qtd, Produto produto, decimal? preco = null)
        {
            Quantidade = qtd;
            Produto = produto;
            ValorUnitario = preco ?? produto.ValorUnitario.Value;
            Modificacoes = new List<Item>(); // modificações unitárias (código das modificações)
        }

        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorModificacoes { get; set; }
        public string DescricaoModificacoes { get; set; }
        public Produto Produto { get; set; }
        public Uri FotoURL => Produto.urlImagemThumb == null ? null : new Uri(Produto.urlImagemThumb);
        public string Descricao => Produto.Nome;
        public string Total => (Quantidade * (ValorUnitario + ValorModificacoes)).ToString("C");
        public Visibility AddVisibility
        {
            get
            {
                // Condições que não permitem adicionar mais itens
                if (Produto.IDTipoProduto == 50
                 || Produto.IDProduto == PdvServices.IDProduto_NovaComanda
                 || Produto.Categorias.Any(c => c.IDCategoria == PdvServices.IDCategoriaProduto_Credito))
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;

            }
        }

        public Visibility EditarVisibility => Produto.PaineisDeModificacao?.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        public List<Item> Modificacoes { get; set; }

        public override string ToString()
        {
            return $"{Quantidade} {ValorUnitario} {DescricaoModificacoes} {ValorModificacoes}";
        }
    }
}
