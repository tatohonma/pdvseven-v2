using a7D.PDV.Integracao.API2.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace a7D.PDV.Integracao.API2.Client
{
    public class ProdutoAPI
    {
        private ClienteWS api;

        internal ProdutoAPI(ClienteWS ws)
            => api = ws;

        public List<Produto> ListaProdutos(int idCategoria = 0, int tipo = 0, bool? disponivel = null, bool? ativo=null)
            => api.Get<List<Produto>>("api/produtos?categoria=" + idCategoria + 
                "&tipo=" + tipo +
                "&disponivel=" + (disponivel.HasValue ? (disponivel == true ? "1" : "0") : "all") +
                "&ativo=" + (ativo.HasValue ? (ativo == true ? "1" : "0") : "all"));

        public Produto Carregar(int idProduto)
            => api.Get<Produto>("api/produtos/" + idProduto);

        public Uri UriProduto(int idProduto)
            => api.Query($"produtos/{idProduto}/imagem");

        public Uri UrlImagem(Produto produto)
            => produto.urlImagem == null ? null : api.Query(produto.urlImagem);

        public Uri UrlImagemThumb(Produto produto)
            => produto.urlImagemThumb == null ? null : api.Query(produto.urlImagemThumb);

        public Image ImageProduto(string imagem)
           => imagem == null ? null : Image.FromStream(StreamProduto(imagem));

        public Stream StreamProduto(string imagem)
           => imagem == null ? null : api.GetBytes(imagem);

    }
}