using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTaxaEntrega")]
    public class TaxaEntregaInformation
    {
        [CRUDParameterDAL(true, "IDTaxaEntrega")]
        public int? IDTaxaEntrega { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        [CRUDParameterDAL(false, "Valor")]
        public decimal? Valor { get; set; }

        [CRUDParameterDAL(false, "Ativo")]
        public bool? Ativo { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public bool? Excluido { get; set; }

        [CRUDParameterDAL(false, "IDTamanhoPacote")]
        public int? IDTamanhoPacote { get; set; }
    }
}
