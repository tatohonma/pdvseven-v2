using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTipoProduto")]
    [Serializable]
    public class TipoProdutoInformation
    {
        [CRUDParameterDAL(true, "IDTipoProduto")]
        public Int32? IDTipoProduto { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        public ETipoProduto TipoProduto => (ETipoProduto)IDTipoProduto.Value;

        public static TipoProdutoInformation ConverterObjeto(Object obj)
        {
            return (TipoProdutoInformation)obj;
        }
    }
}
