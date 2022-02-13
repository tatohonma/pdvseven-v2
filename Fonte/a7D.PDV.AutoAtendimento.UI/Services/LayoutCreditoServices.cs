using a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal static class LayoutCreditoServices
    {
        static XmlNode nodeLayoutNormal;
        static XmlNode nodeLayoutSelecionado;

        internal static void ConfigureNormal(XmlNode node)
        {
            nodeLayoutNormal = node.Clone();
        }

        internal static void ConfigureSelecionado(XmlNode node)
        {
            nodeLayoutSelecionado = node.Clone();
        }

        private static Button CreateButton(Produto prod, bool selecionado)
        {
            var tb = new TextBlock()
            {
                Text = prod.ValorUnitario > 0 ? prod.ValorUnitario.Value.ToString("C") : "Outro\r\nValor"
            };
            tb.TextAlignment = TextAlignment.Center;

            var btn = new Button()
            {
                Tag = prod,
                Content = tb,
            };

            if (selecionado)
                LayoutServices.ConfigureElement(nodeLayoutSelecionado, btn);
            else
                LayoutServices.ConfigureElement(nodeLayoutNormal, btn);

            return btn;
        }

        internal static void Fill(Panel panel, RoutedEventHandler clickHandler, List<Produto> produtos, Produto selecionado)
        {
            if (produtos == null)
                return;

            if (nodeLayoutNormal == null)
                throw new Exception("Nó <Credito> não definido");

            if (nodeLayoutSelecionado == null)
                throw new Exception("Nó <CreditoSelecionado> não definido");

            panel.Children.Clear();
            foreach (var prod in produtos)
            {
                var btn = CreateButton(prod, prod.IDProduto == selecionado?.IDProduto); //, ProdutoImage(prod.IDProduto.Value));
                btn.Click += clickHandler;
                panel.Children.Add(btn);
            }
        }
    }
}