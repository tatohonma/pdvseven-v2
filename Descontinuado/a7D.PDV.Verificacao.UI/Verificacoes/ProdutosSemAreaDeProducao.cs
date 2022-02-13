using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Verificacao.UI.Verificacoes
{
    [Verificacao]
    class ProdutosSemAreaDeProducao : IVerificacao
    {
        public string Mensagem => "Existem produtos sem área de produção. Verique no backoffice.";

        public string Nome => "Produtos sem Área de Produto";

        public CategoriaVerificacao Categoria => CategoriaVerificacao.CADASTROS;

        public Nivel Nivel => Nivel.AVISO;

        public bool Invalido => _invalido;

        private bool _invalido;

        public ProdutosSemAreaDeProducao()
        {
            _invalido = BLL.Produto.ProdutosSemAreaProducao();
        }
    }
}
