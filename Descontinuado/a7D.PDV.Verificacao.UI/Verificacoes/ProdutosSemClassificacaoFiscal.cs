using a7D.PDV.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Verificacao.UI.Verificacoes
{
    [Verificacao]
    class ProdutosSemClassificacaoFiscal : IVerificacao
    {
        public string Mensagem => "Verifique no Backoffice os produtos sem Classificacação Fiscal";

        public string Nome => "Classificação Fiscal";

        public CategoriaVerificacao Categoria => CategoriaVerificacao.SAT;

        public Nivel Nivel => Nivel.ERRO;

        public bool Invalido => _invalido;

        private bool _invalido;

        public ProdutosSemClassificacaoFiscal()
        {
            _invalido = Produto.ListarAtivosDAL(new DateTime(2010, 1, 1)).Any(p => p.ClassificacaoFiscal == null);
        }
    }
}
