using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbProdutoTraducao")]
    [Serializable]
    public class ProdutoTraducaoInformation
    {
        [CRUDParameterDAL(true, "IDProdutoTraducao")]
        public Int32? IDProdutoTraducao { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        [CRUDParameterDAL(false, "IDIdioma", "IDIdioma")]
        public IdiomaInformation Idioma { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public String Descricao { get; set; }

        public StatusModel StatusModel { get; set; }

        public static ProdutoTraducaoInformation ConverterObjeto(Object obj)
        {
            return (ProdutoTraducaoInformation)obj;
        }
    }
}
