using a7D.PDV.AutoAtendimento.UI.Controles;
using a7D.PDV.AutoAtendimento.UI.Paginas;
using a7D.PDV.Integracao.API2.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal interface INavPageClick
    {
        void Buttom_Click(object sender, RoutedEventArgs e);
    }

    public static class LayoutServices
    {
        public static BitmapImage BackgroundImage1 { get; private set; }
        public static BitmapImage BackgroundImage2 { get; private set; }

        private static DependencyProperty PropertyCornerRadius = DependencyProperty.Register(nameof(Border.CornerRadius), typeof(CornerRadius), typeof(Button));

        private static SortedDictionary<string, BitmapImage> ImageList;
        private static XmlDocument xmlLayout;

        public static int CategoriaPai = 0;
        public static int[] CategoriaSelecionadas = null;
        public static bool SubCategoria = false;
        public static string idioma = "";

        public static void Load(ClienteWS ws, string fileXML, string idiomaPadrao)
        {
            TemaAPI temaAPI = null;

            xmlLayout = new XmlDocument();

            if (fileXML != null && fileXML.ToLower().EndsWith(".xml"))
                xmlLayout.Load(fileXML);
            else
            {
                temaAPI = ws.Tema();
                var tema = temaAPI.CarregarTema(PdvServices.PDVID);
                if (tema == null)
                    throw new Exception("Nenhum tema não encontrado");

                temaAPI.Tema = tema.Nome;
                xmlLayout.LoadXml(tema.TemaXML);
            }

            if (xmlLayout.DocumentElement.Attributes["versao"]?.InnerText != "1")
                throw new Exception("Versão de XML inválida");

            var imageListNode = xmlLayout.DocumentElement.SelectSingleNode("Imagens");
            if (imageListNode == null)
                throw new Exception("Não encontrado a Lista de Imagens no XML");

            string pathBase = imageListNode.Attributes["base"]?.InnerText;
            if (pathBase == null)
            {
                if (temaAPI == null)
                {
                    temaAPI = ws.Tema();
                    var tema = temaAPI.CarregarTema(PdvServices.PDVID);
                    if (tema == null)
                        throw new Exception("Nenhum tema não encontrado");

                    temaAPI.Tema = tema.Nome;
                }
                idioma = idiomaPadrao;
            }

            ImageList = new SortedDictionary<string, BitmapImage>();

            foreach (XmlNode node in imageListNode.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                if (ImageList.ContainsKey(node.Name))
                    throw new Exception("Imagem duplicada: " + node.Name);

                if (temaAPI == null)
                {
                    string cFile = node.InnerText;
                    if (!cFile.Contains("."))
                        cFile += ".png";

                    var fi = new FileInfo(pathBase + cFile);
                    if (!fi.Exists)
                        throw new FileNotFoundException($"Imagem não encontrada:\r\n{fi.FullName}");

                    ImageList.Add(node.Name, new BitmapImage(new Uri(fi.FullName)));
                }
                else
                {
                    var stream = temaAPI.Imagem(idioma, node.InnerText);
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream ?? throw new FileNotFoundException($"Imagem não encontrada({idioma}):\r\n{node.InnerText}");
                    image.EndInit();
                    ImageList.Add(node.Name, image);
                }
            }
        }

        internal static List<Button> BindButtons(Canvas canvas, string prefix, int selecionado)
        {
            var btns = new List<Button>();
            var page = (Page)canvas.Parent;
            string name = page.GetType().Name;
            if (name.EndsWith("Page"))
                name = name.Substring(0, name.Length - 4);

            var nodePage = xmlLayout.DocumentElement.SelectSingleNode(name);
            if (nodePage == null)
                return null;

            var nodeBotaoPadrao = nodePage.SelectSingleNode($"BotaoPadrao");
            var nodeBotaoSelecionado = nodePage.SelectSingleNode($"BotaoSelecionado");

            foreach (XmlNode node in nodePage.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;
                else if (node.Name.StartsWith(prefix))
                {
                    var btn = ConfiguraBotao(node.Name, null, node, btns.Count + 1 == selecionado ? nodeBotaoSelecionado : nodeBotaoPadrao);
                    canvas.Children.Add(btn);
                    if (page is INavPageClick iPageClick)
                        btn.Click += iPageClick.Buttom_Click;
                    btns.Add(btn);
                }
            }

            return btns;
        }

        internal static void Bind(Page page)
        {
            string name = page.GetType().Name;
            if (name.EndsWith("Page"))
                name = name.Substring(0, name.Length - 4);

            var nodePage = xmlLayout.DocumentElement.SelectSingleNode(name);
            if (nodePage == null)
                return;

            ConfigureElement(nodePage, page);

            foreach (XmlNode node in nodePage.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                if (node.Name == "Produto" && node.Attributes["control"] != null)
                    LayoutProdutoServices.Configure(node);

                else if (node.Name == "Credito")
                    LayoutCreditoServices.ConfigureNormal(node);

                else if (node.Name == "CreditoSelecionado")
                    LayoutCreditoServices.ConfigureSelecionado(node);

                else if (page.FindName(node.Name) is FrameworkElement fe)
                {
                    if (CondicaoValida(node, fe))
                    {
                        ConfigureElement(node, fe);

                        if (node.Name == "Categorias" && fe is StackPanel spCategoria)
                        {
                            var nodeCategorias = xmlLayout.DocumentElement.SelectSingleNode("Categorias");
                            if (nodeCategorias == null)
                                continue;

                            if (CategoriaSelecionadas == null && int.TryParse(nodeCategorias.Attributes["inicial"]?.InnerText, out int idInicial))
                                CategoriaSelecionadas = new int[] { CategoriaPai = idInicial };

                            var nodeBotao = xmlLayout.DocumentElement.SelectSingleNode($"Categorias/BotaoPadrao");
                            var nodeBotaoSelecionada = xmlLayout.DocumentElement.SelectSingleNode($"Categorias/BotaoPadraoSelecionada");

                            LayoutCategoriaServices.FillCategoria(page, spCategoria, nodeCategorias.ChildNodes, nodeBotao, nodeBotaoSelecionada);
                        }
                        else if (node.Name == "SubCategorias" && fe is StackPanel spSubCategoria)
                        {
                            var nodeCategoria = xmlLayout.DocumentElement.SelectSingleNode($"Categorias/Categoria[@id='{CategoriaPai}']");
                            SubCategoria = nodeCategoria != null && nodeCategoria.ChildNodes.Count > 0;
                            if (!SubCategoria)
                                continue;

                            var nodeBotaoSub = xmlLayout.DocumentElement.SelectSingleNode($"Categorias/BotaoPadraoSub");
                            var nodeBotaoSubSelecionada = xmlLayout.DocumentElement.SelectSingleNode($"Categorias/BotaoPadraoSubSelecionada");

                            LayoutCategoriaServices.FillSubCategorias(page, spSubCategoria, nodeCategoria.ChildNodes, nodeBotaoSub, nodeBotaoSubSelecionada);
                        }
                    }
                }
            }
        }

        internal static void Bind(Grid grid)
        {
            var nodePage = xmlLayout.DocumentElement.SelectSingleNode(grid.Name);
            if (nodePage == null)
                return;

            ConfigureElement(nodePage, grid);

            foreach (XmlNode node in nodePage.ChildNodes)
            {
                if (grid.FindName(node.Name) is FrameworkElement fe)
                {
                    if (CondicaoValida(node, fe))
                    {
                        ConfigureElement(node, fe);
                    }
                }
            }
        }

        internal static void Bind(TecladoBox teclado)
        {
            var nodePage = xmlLayout.DocumentElement.SelectSingleNode("Teclado");
            if (nodePage == null)
                return;

            var style = nodePage.Attributes["layout"]?.InnerText;
            if (int.TryParse(style, out int nstyle))
                teclado.LayoutStyle = nstyle;

            foreach (XmlNode node in nodePage.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                if (node.Name == "Digitos")
                {
                    ConfigureElement(node, teclado.teclado.btn0);
                    ConfigureElement(node, teclado.teclado.btn1);
                    ConfigureElement(node, teclado.teclado.btn2);
                    ConfigureElement(node, teclado.teclado.btn3);
                    ConfigureElement(node, teclado.teclado.btn4);
                    ConfigureElement(node, teclado.teclado.btn5);
                    ConfigureElement(node, teclado.teclado.btn6);
                    ConfigureElement(node, teclado.teclado.btn7);
                    ConfigureElement(node, teclado.teclado.btn8);
                    ConfigureElement(node, teclado.teclado.btn9);
                }
                else if (node.Name == "Enter")
                {
                    if (nstyle == 2)
                        ConfigureElement(node, teclado.btnEnter);
                    else
                        ConfigureElement(node, teclado.teclado.btnEnter);
                }
                else if (node.Name == "Cancel")
                {
                    if (nstyle == 2)
                        ConfigureElement(node, teclado.teclado.btnCancel);
                    else
                        ConfigureElement(node, teclado.btnCancel);
                }
                else if (node.Name == "Backspace")
                {
                    if (nstyle == 2)
                        ConfigureElement(node, teclado.teclado.btnBackspace);
                    else
                        ConfigureElement(node, teclado.btnBackspace);
                }
                else if (node.Name == "Texto")
                {
                    ConfigureElement(node, teclado.Texto);
                }
            }

            teclado.UpdateGrid();
        }

        internal static void Bind(TecladoAlfanumerico teclado)
        {
            if (xmlLayout == null)
                return;

            var nodeItens = xmlLayout.DocumentElement.SelectSingleNode("TecladoAlfanumerico");
            if (nodeItens == null)
                return;

            ConfigureElement(nodeItens, teclado);

            var fundo1 = nodeItens.Attributes["background1"]?.InnerText;
            if (fundo1 != null && fundo1.Contains(fundo1))
                teclado.BackgroundAlfabetico = new ImageBrush(ImageList[fundo1]);

            var fundo2 = nodeItens.Attributes["background2"]?.InnerText;
            if (fundo2 != null && fundo2.Contains(fundo2))
                teclado.BackgroundNumerico = new ImageBrush(ImageList[fundo2]);

            var teclas = nodeItens.SelectSingleNode("Teclas");

            foreach (XmlNode node in nodeItens.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                if (teclado.Find(node.Name) is Button btn)
                {
                    ConfigureElement(teclas, btn);
                    ConfigureElement(node, btn);
                }
            }
        }

        internal static void Bind(Window modal)
        {
            string name = modal.GetType().Name;
            if (name.EndsWith("Window"))
                name = name.Substring(0, name.Length - 6);

            var nodePage = xmlLayout.DocumentElement.SelectSingleNode(name);
            if (nodePage == null)
                return;

            foreach (XmlNode node in nodePage.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                if (node.Name.ToLower() == "background")
                {
                    var fundo = modal.Background = GetBrush(node.InnerText);
                    modal.Background = fundo ?? Brushes.Transparent;
                }
                else if (modal.FindName(node.Name) is FrameworkElement fe)
                {
                    ConfigureElement(node, fe);
                }
            }
        }

        private static bool CondicaoValida(XmlNode node, FrameworkElement fe)
        {
            string subcategoria = node.Attributes["subcategoria"]?.InnerText;
            if (subcategoria != null)
            {
                if (bool.TryParse(subcategoria, out bool subCat))
                    return SubCategoria == subCat;
            }

            string categoria = node.Attributes["categoria"]?.InnerText;
            if (categoria == null)
                return true;
            else
            {
                if (categoria == "0" && CategoriaSelecionadas == null)
                    return true;
                else if (categoria == "*" && CategoriaSelecionadas != null)
                    return true;
                else
                    return false;
            }
        }

        public static Brush GetBrush(string backgroud, FrameworkElement fe = null)
        {
            if (backgroud == null || backgroud.StartsWith("_"))
                return null;

            else if (ImageList.ContainsKey(backgroud))
            {
                var imagem = ImageList[backgroud];
                if (fe != null)
                {
                    if (double.IsNaN(fe.Width))
                        fe.Width = imagem.Width;
                    if (double.IsNaN(fe.Height))
                        fe.Height = imagem.Height;
                }
                return new ImageBrush(imagem);
            }
            else // if (backgroud.StartsWith("#"))
            {
                try
                {
                    var cor = System.Drawing.ColorTranslator.FromHtml(backgroud);
                    return new SolidColorBrush(Color.FromArgb(cor.A, cor.R, cor.G, cor.B));
                }
                catch (Exception ex)
                {
                    throw new Exception($"Não existe a imagem ou cor: {backgroud} ({fe?.Name})", ex);
                }
            }
        }

        public static void ConfigureElement(XmlNode node, FrameworkElement fe)
        {
            if (node == null)
                throw new Exception("Nó de referencia não definido");

            if (Boolean.TryParse(node.Attributes["visible"]?.InnerText, out bool visible))
                fe.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;

            if (fe.Visibility == Visibility.Collapsed)
                return;

            if (double.TryParse(node.Attributes["left"]?.InnerText, out double left))
                Canvas.SetLeft(fe, left);

            if (double.TryParse(node.Attributes["top"]?.InnerText, out double top))
                Canvas.SetTop(fe, top);

            if (double.TryParse(node.Attributes["width"]?.InnerText, out double width))
                fe.Width = width;

            if (double.TryParse(node.Attributes["height"]?.InnerText, out double height))
                fe.Height = height;

            var tag = node.Attributes["tag"]?.InnerText;
            if (tag != null)
                fe.Tag = tag;

            string margin = node.Attributes["margin"]?.InnerText;
            if (margin != null)
            {
                var m = margin.Split(',');
                if (m.Length == 1)
                {
                    if (double.TryParse(m[0], out double mAll))
                        fe.Margin = new Thickness(mAll);
                }
                else if (m.Length == 4)
                {
                    fe.Margin = new Thickness(int.Parse(m[0]), int.Parse(m[1]), int.Parse(m[2]), int.Parse(m[3]));
                }
            }

            if (fe is Panel pnl)
            {
                var fundo = GetBrush(node.Attributes["background"]?.InnerText, fe);
                if (fundo != null)
                    pnl.Background = fundo;

                if (pnl is StackPanel sp)
                {
                    var orientation = node.Attributes["orientation"]?.InnerText;
                    if (orientation != null)
                    {
                        if (orientation.ToLower() == "horizontal")
                            sp.Orientation = Orientation.Horizontal;
                        else if (orientation.ToLower() == "vertical")
                            sp.Orientation = Orientation.Vertical;
                    }
                }
            }
            else if (fe is Page pg)
            {
                var fundo1 = node.Attributes["background"]?.InnerText;
                if (fundo1 != null)
                {
                    if (ImageList.ContainsKey(fundo1))
                        BackgroundImage1 = ImageList[fundo1];

                    pg.Background = GetBrush(fundo1, fe);
                }

                var fundo2 = node.Attributes["background2"]?.InnerText;
                if (fundo2 != null && ImageList.ContainsKey(fundo2))
                {
                    BackgroundImage2 = ImageList[fundo2];
                }
                else
                    BackgroundImage2 = null;
            }

            if (fe is Control ctrl)
            {
                var fundo = GetBrush(node.Attributes["background"]?.InnerText, fe);
                if (fundo != null)
                    ctrl.Background = fundo;

                if (double.TryParse(node.Attributes["fontsize"]?.InnerText, out double fontsize))
                    ctrl.FontSize = fontsize;

                string fontfamily = node.Attributes["fontfamily"]?.InnerText;
                if (fontfamily != null)
                    ctrl.FontFamily = new FontFamily(fontfamily);

                var frente = GetBrush(node.Attributes["foreground"]?.InnerText);
                if (frente != null)
                    ctrl.Foreground = frente;

                string fontweight = node.Attributes["fontweight"]?.InnerText;
                if (fontweight != null)
                {
                    if (fontweight.ToLower() == "bold")
                        ctrl.FontWeight = FontWeights.Bold;
                }
            }

            if (fe is ContentControl cctrl)
            {
                string text = node.Attributes["text"]?.InnerText;
                if (text != null)
                    cctrl.Content = text;

                if (double.TryParse(node.Attributes["border"]?.InnerText, out double border))
                {
                    cctrl.BorderThickness = new Thickness(border);

                    var borderColor = GetBrush(node.Attributes["bordercolor"]?.InnerText, fe);
                    if (borderColor != null)
                        cctrl.BorderBrush = borderColor;

                    if (double.TryParse(node.Attributes["borderradius"]?.InnerText, out double radius))
                    {
                        cctrl.SetValue(PropertyCornerRadius, new CornerRadius(radius));
                        //(CornerRadius)((Setter)cctrl.Style.Setters[3]).Value
                    }
                }
            }

            if (fe is TextBlock ctxt)
            {
                string text = node.Attributes["text"]?.InnerText;
                if (text != null)
                    ctxt.Text = text;

                var fundo = GetBrush(node.Attributes["background"]?.InnerText, fe);
                if (fundo != null)
                    ctxt.Background = fundo;

                var frente = GetBrush(node.Attributes["foreground"]?.InnerText);
                if (frente != null)
                    ctxt.Foreground = frente;

                if (double.TryParse(node.Attributes["fontsize"]?.InnerText, out double fontsize))
                    ctxt.FontSize = fontsize;

                string fontfamily = node.Attributes["fontfamily"]?.InnerText;
                if (fontfamily != null)
                    ctxt.FontFamily = new FontFamily(fontfamily);

                string fontweight = node.Attributes["fontweight"]?.InnerText;
                if (fontweight != null)
                {
                    if (fontweight.ToLower() == "bold")
                        ctxt.FontWeight = FontWeights.Bold;
                }

                string textwrapping = node.Attributes["textwrapping"]?.InnerText;
                if (textwrapping != null)
                {
                    if (Enum.TryParse(textwrapping, true, out TextWrapping wrapResult))
                    {
                        ctxt.TextWrapping = wrapResult;
                    }
                }

                string textalignment = node.Attributes["textalignment"]?.InnerText;
                if (textalignment != null)
                {
                    if (Enum.TryParse(textalignment, true, out TextAlignment alignmentResult))
                    {
                        ctxt.TextAlignment = alignmentResult;
                    }
                }
            }
        }

        internal static Button ConfiguraBotao(string name, object tag, XmlNode nodeItem, XmlNode nodePadrao)
        {
            var btn = new Button()
            {
                Name = name,
                Tag = tag
            };

            if (nodePadrao != null)
                ConfigureElement(nodePadrao, btn);

            ConfigureElement(nodeItem, btn);

            var backgroundPadrao = nodePadrao?.Attributes["background"]?.InnerText;
            var background = nodeItem.Attributes["background"]?.InnerText;

            if (background != null)
            {
                if (backgroundPadrao != null && backgroundPadrao.StartsWith("_"))
                    background += backgroundPadrao;

                btn.Background = LayoutServices.GetBrush(background, btn);

            }

            return btn;
        }
    }
}