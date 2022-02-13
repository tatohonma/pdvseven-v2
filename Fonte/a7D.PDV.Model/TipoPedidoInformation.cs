using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTipoPedido")]
    [Serializable]
    public class TipoPedidoInformation
    {
        [CRUDParameterDAL(true, "IDTipoPedido")]
        public Int32? IDTipoPedido { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        public ETipoPedido TipoPedido => (ETipoPedido)IDTipoPedido;

        public static TipoPedidoInformation ConverterObjeto(Object obj)
        {
            return (TipoPedidoInformation)obj;
        }

        public override string ToString()
        {
            return $"{IDTipoPedido}";
        }
    }
}
