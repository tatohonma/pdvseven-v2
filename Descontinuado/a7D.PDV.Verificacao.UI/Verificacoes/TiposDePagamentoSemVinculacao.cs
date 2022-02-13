using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a7D.PDV.BLL;

namespace a7D.PDV.Verificacao.UI.Verificacoes
{
    [Verificacao]
    class TiposDePagamentoSemVinculacao : IVerificacao
    {
        public string Mensagem => "Verifique os tipos de pagamento no Backoffice";

        public string Nome => "Tipos de pagamento S@T";

        public CategoriaVerificacao Categoria => CategoriaVerificacao.SAT;

        public Nivel Nivel => Nivel.ERRO;

        public ConfiguracoesSistema ConfiguracoesSistema { get; }

        public bool Invalido => _invalido;

        private bool _invalido;

        public TiposDePagamentoSemVinculacao()
        {
            ConfiguracoesSistema = ConfiguracoesSistema.Valores;
            var possuiSAT = ConfiguracoesSistema.PossuiSAT;
            _invalido = possuiSAT & TipoPagamento.ListarAtivos().Any(t => t.MeioPagamentoSAT == null);
        }

    }
}
