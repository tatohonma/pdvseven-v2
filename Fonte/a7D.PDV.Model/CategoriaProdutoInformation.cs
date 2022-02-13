using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbCategoriaProduto")]
    [Serializable]
    public class CategoriaProdutoInformation
    {
        [CRUDParameterDAL(true, "IDCategoriaProduto")]
        public Int32? IDCategoriaProduto { get; set; }

        [CRUDParameterDAL(false, "CodigoERP")]
        public String CodigoERP { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public Boolean? Excluido { get; set; }

        [CRUDParameterDAL(false, "DtUltimaAlteracao")]
        public DateTime? DtUltimaAlteracao { get; set; }

        [CRUDParameterDAL(false, "Disponibilidade")]
        public Boolean? Disponibilidade { get; set; }

        [CRUDParameterDAL(false, "DtAlteracaoDisponibilidade")]
        public DateTime? DtAlteracaoDisponibilidade { get; set; }

        public static CategoriaProdutoInformation ConverterObjeto(Object obj)
        {
            return (CategoriaProdutoInformation)obj;
        }
    }
}
