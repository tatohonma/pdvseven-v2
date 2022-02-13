using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.Servico.Core.PagSevenServer.Models
{
    public class Pagamento
    {
        
        public Int32 IDPagamento { get; set; }
        public Int32? IDPedido { get; set; }
        public Int32? IDEstabelecimento { get; set; }
        public Int32? IDStatusIntegracao { get; set; }
        public Int32? IDStatusPagamento { get; set; }
        public string GUIDPagamento { get; set; }
        public Decimal? Valor { get; set; }

        public string CpfUsuario { get; set; }
        public DateTime? DataAlteracaoStatusPagamento { get; set; }
        public DateTime? DataAlteracaoStatusIntegracao { get; set; }
        public string RespostaIntegracao { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
