using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbMovimentacao")]
    public class MovimentacaoInformation
    {
        [CRUDParameterDAL(true, "IDMovimentacao")]
        public int? IDMovimentacao { get; set; }

        [CRUDParameterDAL(false, "IDTipoMovimentacao", "IDTipoMovimentacao")]
        public TipoMovimentacaoInformation TipoMovimentacao { get; set; }

        [CRUDParameterDAL(false, "IDFornecedor", "IDCliente")]
        public ClienteInformation Fornecedor { get; set; }

        [CRUDParameterDAL(false, "GUID")]
        public string GUID { get; set; }

        [CRUDParameterDAL(false, "DataMovimentacao")]
        public DateTime? DataMovimentacao { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public string Descricao { get; set; }

        [CRUDParameterDAL(false, "NumeroPedido")]
        public string NumeroPedido { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public bool? Excluido { get; set; }

        [CRUDParameterDAL(false, "Reversa")]
        public bool? Reversa { get; set; }

        [CRUDParameterDAL(false, "IDMovimentacao_reversa", "IDMovimentacao")]
        public MovimentacaoInformation MovimentacaoReversa { get; set; }

        [CRUDParameterDAL(false, "Processado")]
        public bool? Processado { get; set; }

        public List<MovimentacaoProdutosInformation> MovimentacaoProdutos { get; set; }

    }
}
