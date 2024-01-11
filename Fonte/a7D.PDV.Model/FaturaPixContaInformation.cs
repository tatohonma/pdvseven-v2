using System;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbFaturaPixConta")]
    [Serializable]
    public class FaturaPixContaInformation
    {
        [CRUDParameterDAL(true, "IDFaturaPixConta")]
        public int? IDFaturaPixConta { get; set; }

        [CRUDParameterDAL(false, "IDPedido", "IDPedido")]
        public PedidoInformation Pedido { get; set; }

        [CRUDParameterDAL(false, "CodigoFatura")]
        public string CodigoFatura { get; set; }

        [CRUDParameterDAL(false, "ChavePix")]
        public string ChavePix { get; set; }

        //Valores: pendente, cancelar, cancelado, pago
        [CRUDParameterDAL(false, "Status")]
        public string Status { get; set; }

        [CRUDParameterDAL(false, "Valor")]
        public decimal? Valor { get; set; }

        [CRUDParameterDAL(false, "DtInclusao")]
        public DateTime? DtInclusao { get; set; }

        [CRUDParameterDAL(false, "DtUltimaAlteracao")]
        public DateTime? DtUltimaAlteracao { get; set; }

        public static FaturaPixContaInformation ConverterObjeto(object obj)
        {
            return (FaturaPixContaInformation)obj;
        }
    }
}
