using a7D.PDV.Integracao.API2.Client;
using a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace a7D.PDV.AutoAtendimento.UI.Services
{
    internal class ProdutoServices
    {
        string pathTemp;

        ProdutoAPI api;
        List<Produto> produtos;
        List<Produto> modificacoes;
        List<Produto> creditos;

        public Produto LoadProduto(int idProduto)
            => produtos.FirstOrDefault(p => p.IDProduto == idProduto);

        public Produto LoadModificacao(int idProduto)
            => modificacoes.FirstOrDefault(p => p.IDProduto == idProduto);

        private void ResolveProdutoImage(Produto prod)
        {
            try
            {
                string prodImage_tumb = $"{prod.IDProduto}_tumb.png";
                string prodImage = $"{prod.IDProduto}.png";

                var fi_tumb = new FileInfo(pathTemp + prodImage_tumb);
                var fi = new FileInfo(pathTemp + prodImage);

                if (prod.urlImagem == null)
                {
                    if (fi.Exists)
                        fi.Delete();

                    if (fi_tumb.Exists)
                        fi_tumb.Delete();

                    return;
                }

                if (!fi.Exists || !fi_tumb.Exists || prod.DtUltimaAlteracao.HasValue && fi.CreationTime < prod.DtUltimaAlteracao)
                {
                    fi_tumb.Delete();
                    fi.Delete();

                    using (var sr = api.StreamProduto(prod.urlImagemThumb))
                    {
                        using (var sw = fi_tumb.Create())
                        {
                            sr.CopyTo(sw);
                            sw.Close();
                        }
                        sr.Close();
                    }

                    using (var sr = api.StreamProduto(prod.urlImagem))
                    {
                        using (var sw = fi.Create())
                        {
                            sr.CopyTo(sw);
                            sw.Close();
                        }
                        sr.Close();
                    }
                }

                prod.urlImagemThumb = fi_tumb.FullName;
                prod.urlImagem = fi.FullName;
            }
            catch (Exception)
            {
                prod.urlImagemThumb = null;
                prod.urlImagem = null;
            }
        }

        private DateTime nextLastSync;

        internal ProdutoServices(ClienteWS ws)
        {
            var di = new DirectoryInfo("TEMP");
            if (!di.Exists)
                di.Create();

            pathTemp = di.FullName + @"\";
            api = ws.Produto();
            nextLastSync = DateTime.Now;
        }

        internal async Task<bool> SyncProdutos()
        {
            if (produtos != null && DateTime.Now < nextLastSync)
                return true;

            try
            {
                App.StatusBar = "Atualizando produtos";
                await Task.Factory.StartNew(() =>
                {
                    produtos = api.ListaProdutos();
                    modificacoes = api.ListaProdutos(disponivel: true, tipo: 20);
                    produtos.ForEach(p => ResolveProdutoImage(p));
                });
                if (produtos.Count() == 0)
                {
                    produtos = null;
                    throw new Exception("Não há produtos disponíveis");
                }
                App.StatusBar = "Atualizado as " + DateTime.Now.ToString("HH:mm:ss");
            }
            catch (Exception ex)
            {
                EventLogServices.Error(ex);
                App.StatusBar = "Não foi possível consultar a disponibilidade dos produtos: " + ex.Message;
                return false;
            }
            nextLastSync = DateTime.Now.AddMinutes(PdvServices.VerificarDisponibilidade);
            return true;
        }

        internal List<Produto> TodosProdutos()
        {
            return produtos;
        }

        internal async Task<List<Produto>> PacoteCreditos()
        {
            if (creditos != null && DateTime.Now < nextLastSync)
                return creditos;

            try
            {
                creditos = null;
                App.StatusBar = "Carregando pacotes de créditos";
                await Task.Delay(App.waitStep);
                return await Task.Factory.StartNew(() => creditos = api.ListaProdutos(disponivel: true, tipo: 50));
            }
            catch (Exception ex)
            {
                EventLogServices.Error(ex);
                App.StatusBar = "Não foi possível consultar a disponibilidade dos produtos: " + ex.Message;
                return null;
            }
            finally
            {
                nextLastSync = DateTime.Now.AddMinutes(PdvServices.VerificarDisponibilidade);
                App.StatusBar = "";
            }
        }

        internal void FillProdutos(Panel panel, RoutedEventHandler click)
        {
            if (LayoutServices.CategoriaSelecionadas == null)
                return;

            var prod = produtos.Where(p => p.Categorias.Exists(c => LayoutServices.CategoriaSelecionadas.Contains(c.IDCategoria.Value)));
            LayoutProdutoServices.Fill(panel, click, prod.OrderBy(p => p.Nome).ToList());
        }

        internal void FillCredito(List<Produto> creditos, StackPanel panel, RoutedEventHandler click, Produto selecionado)
        {
            LayoutCreditoServices.Fill(panel, click, creditos, selecionado);
        }
    }
}