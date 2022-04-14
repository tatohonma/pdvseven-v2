using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;
using System;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbOrigemPedido")]
    [Serializable]
    public class OrigemPedidoInformation
    {
        [CRUDParameterDAL(true, "IDOrigemPedido")]
        public Int32? IDOrigemPedido { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }
    }
}
