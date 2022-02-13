using a7D.PDV.AutoAtendimento.UI.Controles;
using a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal static class LayoutProdutoServices
    {
        static XmlNode nodeLayout;
        internal static int Width { get; private set; }
        internal static int Height { get; private set; }
        internal static int Margin { get; private set; }
        internal static Brush CirculoFill { get; private set; }

        internal static void Configure(XmlNode node)
        {
            nodeLayout = node.Clone();
            Width = int.Parse(node.Attributes["width"].InnerText);
            Height = int.Parse(node.Attributes["height"].InnerText);
            Margin = int.Parse(node.Attributes["margin"].InnerText);
            CirculoFill = LayoutServices.GetBrush(node.Attributes["circle"]?.InnerText ?? "#A0000000");
        }

        private static Button CreateButton(Produto prod)
        {
            var btn = new Button()
            {
                Tag = prod,
            };

            LayoutServices.ConfigureElement(nodeLayout, btn);

            string control = nodeLayout.Attributes["control"].InnerText;
            if (control == "Produto1")
            {
                btn.Content = new Produto1(prod.Nome, prod.ValorUnitario, prod.Disponibilidade, prod.urlImagemThumb)
                {
                    Width = btn.Width,
                    Height = btn.Height
                };
            }
            else if (control == "Produto2")
            {
                btn.Content = new Produto2(prod.Nome, prod.Descricao, prod.ValorUnitario, prod.Disponibilidade, prod.urlImagemThumb)
                {
                    Width = btn.Width,
                    Height = btn.Height
                };
            }

            return btn;
        }

        internal static void Fill(Panel panel, RoutedEventHandler clickHandler, List<Produto> produtos)
        {
            if (produtos == null)
                return;

            if (nodeLayout == null)
                throw new Exception("Nó <Produto> não definido");

            int x = 0;
            int y = 0;
            foreach (var prod in produtos)
            {
                var btn = CreateButton(prod); //, ProdutoImage(prod.IDProduto.Value));

                Canvas.SetLeft(btn, Margin + x * (Width + 2 * Margin));
                Canvas.SetTop(btn, Margin + y * (Height + 2 * Margin));
                btn.Click += clickHandler;

                panel.Children.Add(btn);

                x++;
                if ((x + 1) * Width > panel.ActualWidth)
                {
                    x = 0;
                    y++;
                }
            }
            panel.Height = (y + (x == 0 ? 0 : 1)) * (Height + Margin * 2);
        }
    }
}