using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Verificacao.UI.Verificacoes
{
    [Verificacao]
    class ProdutosSemCategoria : IVerificacao
    {
        public bool Invalido => _invalido;

        private bool _invalido;

        public string Mensagem => "Existem produtos sem categoria. Verifique no backoffice.";

        public string Nome => "Produtos Sem Categoria";

        public CategoriaVerificacao Categoria => CategoriaVerificacao.CARDAPIO;

        public Nivel Nivel => Nivel.AVISO;

        public ProdutosSemCategoria()
        {
            _invalido = BLL.Produto.ProdutosSemCategoriaProduto();
        }
    }
}
