using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbProdutoImposto")]
    [Serializable]
    public class ProdutoImpostoInformation
    {
        [CRUDParameterDAL(true, "IDProdutoImposto")]
        public Int32? IDProdutoImposto { get; set; }

        [CRUDParameterDAL(false, "IDCategoriaImposto", "IDCategoriaImposto")]
        public CategoriaImpostoInformation CategoriaImposto { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "cEAN")]
        public String cEAN { get; set; }

        [CRUDParameterDAL(false, "NCM")]
        public String NCM { get; set; }

        [CRUDParameterDAL(false, "CFOP")]
        public String CFOP { get; set; }

        [CRUDParameterDAL(false, "uCom")]
        public String uCom { get; set; }

        public static ProdutoImpostoInformation ConverterObjeto(Object obj)
        {
            return (ProdutoImpostoInformation)obj;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
