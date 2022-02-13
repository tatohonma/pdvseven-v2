using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbLogTiny")]
    public class LogTinyInformation
    {
        [CRUDParameterDAL(true, "IDLogTiny")]
        public int? IDLogTiny { get; set; }

        [CRUDParameterDAL(false, "IniciadoEm")]
        public DateTime? IniciadoEm { get; set; }

        [CRUDParameterDAL(false, "FinalizadoEm")]
        public DateTime? FinalizadoEm { get; set; }
    }
}
