using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbEntradaSaida")]
    public class EntradaSaidaInformation
    {
        [CRUDParameterDAL(true, "IDEntradaSaida")]
        public int? IDEntradaSaida { get; set; }

        [CRUDParameterDAL(false, "GUID_Origem")]
        public string GUID_Origem { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        [CRUDParameterDAL(false, "Entrada")]
        public bool? Entrada { get; set; }

        [CRUDParameterDAL(false, "Quantidade")]
        public decimal? Quantidade { get; set; }

        [CRUDParameterDAL(false, "Data")]
        public DateTime? Data { get; set; }
    }
}
