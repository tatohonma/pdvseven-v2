using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Integracao.API.Responses
{
    class ListarProdutosResponse
    {
        public Produto[] produtos { get; set; }

        public class Produto
        {
            public int idProduto { get; set; }
            public int idTipoProduto { get; set; }
            public string nome { get; set; }
            public string descricao { get; set; }
            public decimal valorUnitario { get; set; }
            public string textoAdicionar { get; set; }
            public Imagens imagens { get; set; }
            public string exibicaoPainelModificacao { get { return "lista"; } private set { } }
            public bool ativo { get; set; }
            public DateTime dtUltimaAlteracao { get; set; }
            public bool disponivel { get; set; }
            public DateTime dtAlteracaoDisponibilidade { get; set; }
            public Categoria[] categorias { get; set; }
            public PainelModificacao[] paineisModificacao { get; set; }
            public AreaImpressao[] areasImpressao { get; set; }
            public Traducao[] traducoes { get; set; }
        }

        public class Imagens
        {
            private int idProduto;
            private string _url;
            private string _urlThumb;

            public Imagens(int idProduto)
            {
                this.idProduto = idProduto;
                _url = $"/ImagensProdutos/{this.idProduto}.png";
                _urlThumb = $"/ImagensProdutos/{this.idProduto}_thumb.png";
            }

            public string url
            {
                get { return _url; }
                private set { }
            }

            public string urlThumb
            {
                get { return _urlThumb; }
                private set { }
            }
        }

        public class Categoria
        {
            public int idCategoria { get; set; }
            public int idCategoriaProduto { get; set; }
        }

        public class PainelModificacao
        {
            public int idPainelModificacao { get; set; }
            public int ordem { get; set; }
        }

        public class AreaImpressao
        {
            public int idAreaImpressao { get; set; }
        }

        public class Traducao
        {
            public string idioma { get; set; }
            public int id { get; set; }
            public string nome { get; set; }
            public string descricao { get; set; }
        }
    }
}