using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbEntregador")]
    public class EntregadorInformation
    {
        [CRUDParameterDAL(true, "IDEntregador")]
        public int? IDEntregador { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        [CRUDParameterDAL(false, "Ativo")]
        public bool? Ativo { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public bool? Excluido { get; set; }

        [CRUDParameterDAL(false, "IDGateway")]
        public int? IDGateway { get; set; }
    }
}
