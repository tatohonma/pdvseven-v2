using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using a7D.PDV.BLL;

namespace a7D.PDV.Verificacao.UI.Verificacoes
{
    [Verificacao]
    class MotivosDeDesconto : IVerificacao
    {
        private ConfiguracoesSistema ConfiguracoesSistema;

        public string Mensagem => "Não existem motivos de desconto cadastrados";

        public string Nome => "Motivos de Desconto";

        public CategoriaVerificacao Categoria => CategoriaVerificacao.CADASTROS;

        public Nivel Nivel => Nivel.ERRO;

        public bool Invalido => _invalido;

        private bool _invalido;

        public MotivosDeDesconto()
        {
            ConfiguracoesSistema = ConfiguracoesSistema.Valores;
            var solicitarTipo = ConfiguracoesSistema.SolicitarTipoDesconto || ConfiguracoesSistema.SolicitarTipoDescontoItem;
            _invalido = solicitarTipo & !BLL.TipoDesconto.ListarAtivos().Any();
        }
    }
}
