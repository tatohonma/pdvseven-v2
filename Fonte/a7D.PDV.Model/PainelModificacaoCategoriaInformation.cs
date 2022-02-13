using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbPainelModificacaoCategoria")]
    [Serializable]
    public class PainelModificacaoCategoriaInformation
    {
        [CRUDParameterDAL(true, "IDPainelModificacaoCategoria")]
        public Int32? IDPainelModificacaoCategoria { get; set; }

        [CRUDParameterDAL(false, "IDPainelModificacao", "IDPainelModificacao")]
        public PainelModificacaoInformation PainelModificacao { get; set; }

        [CRUDParameterDAL(false, "IDCategoriaProduto", "IDCategoriaProduto")]
        public CategoriaProdutoInformation Categoria { get; set; }

        [CRUDParameterDAL(false, "Ordem")]
        public Int32? Ordem { get; set; }

        public StatusModel StatusModel { get; set; }

        //public List<ProdutoInformation> ListaProduto { get; set; }

        public static PainelModificacaoCategoriaInformation ConverterObjeto(Object obj)
        {
            return (PainelModificacaoCategoriaInformation)obj;
        }
    }
}
