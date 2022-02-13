using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbStatusPedido")]
    [Serializable]
    public class StatusPedidoInformation
    {
        [CRUDParameterDAL(true, "IDStatusPedido")]
        public Int32? IDStatusPedido { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        public EStatusPedido StatusPedido
        {
            get { return (EStatusPedido)IDStatusPedido; }
            set { IDStatusPedido = (int)value; }
        }

        public static StatusPedidoInformation ConverterObjeto(Object obj)
        {
            return (StatusPedidoInformation)obj;
        }

        public override string ToString()
        {
            return $"{IDStatusPedido} {StatusPedido.ToString()}";
        }
    }

    
}
