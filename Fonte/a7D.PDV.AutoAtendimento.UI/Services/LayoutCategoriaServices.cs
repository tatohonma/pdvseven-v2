using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal static class LayoutCategoriaServices
    {
        internal static void FillCategoria(Page page, StackPanel sp, XmlNodeList childNodes, XmlNode nodeBotoes, XmlNode nodeBotaoSelecionada)
        {
            // Não tentar fazer junto
            // Primeiro verifica apenas a alteração da categoria PAI antes de renderizar!
            foreach (XmlNode node in childNodes)
            {
                if (node.NodeType != XmlNodeType.Element || node.Name != "Categoria")
                    continue;

                // mesma logica na mesma ordem abaixo
                if (!int.TryParse(node.Attributes["id"]?.InnerText, out int categoria) || categoria == 0)
                    return;

                if (LayoutServices.CategoriaSelecionadas != null
                 && LayoutServices.CategoriaSelecionadas.Length > 0
                 && LayoutServices.CategoriaSelecionadas[0] == categoria)
                    LayoutServices.CategoriaPai = categoria;
            }

            // Se foi redefinida a categoria pai, irá renderizar corretamente
            foreach (XmlNode node in childNodes)
            {
                if (node.NodeType != XmlNodeType.Element || node.Name != "Categoria")
                    continue;

                if (!int.TryParse(node.Attributes["id"]?.InnerText, out int categoria) || categoria == 0)
                    return;

                string categorias = null;
                if (categoria < 0)
                {
                    categorias = categoria.ToString();

                    foreach (XmlNode subNode in node.ChildNodes)
                    {
                        if (subNode.NodeType != XmlNodeType.Element || node.Name != "Categoria")
                            continue;

                        if (int.TryParse(subNode.Attributes["id"]?.InnerText, out int subcat) && subcat > 0)
                            categorias += "," + subcat;
                    }
                }
                else
                    categorias = categoria.ToString();

                var btn = LayoutServices.ConfiguraBotao(null, categorias, node, LayoutServices.CategoriaPai == categoria ? nodeBotaoSelecionada : nodeBotoes);
                if (page is INavPageClick iPageClick)
                    btn.Click += iPageClick.Buttom_Click;

                sp.Children.Add(btn);
            }
        }

        internal static void FillSubCategorias(Page page, StackPanel sp, XmlNodeList childNodes, XmlNode nodeBotoes, XmlNode nodeBotaoSelecionada)
        {
            foreach (XmlNode node in childNodes)
            {
                if (node.NodeType != XmlNodeType.Element || node.Name != "Categoria")
                    continue;

                if (!int.TryParse(node.Attributes["id"]?.InnerText, out int categoria) || categoria == 0)
                    return;

                bool selecionada = false;
                if (LayoutServices.CategoriaSelecionadas != null
                 && LayoutServices.CategoriaSelecionadas.Length == 1
                 && LayoutServices.CategoriaSelecionadas[0] == categoria)
                    selecionada = true;

                var btn = LayoutServices.ConfiguraBotao(null, categoria.ToString(), node, selecionada ? nodeBotaoSelecionada : nodeBotoes);
                if (page is INavPageClick inpc)
                    btn.Click += inpc.Buttom_Click;

                sp.Children.Add(btn);
            }
        }
    }
}