using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbPainelModificacaoProduto")]
    [Serializable]
    public class PainelModificacaoProdutoInformation
    {
        [CRUDParameterDAL(true, "IDPainelModificacaoProduto")]
        public Int32? IDPainelModificacaoProduto { get; set; }

        [CRUDParameterDAL(false, "IDPainelModificacao", "IDPainelModificacao")]
        public PainelModificacaoInformation PainelModificacao { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        [CRUDParameterDAL(false, "Ordem")]
        public Int32? Ordem { get; set; }

        public StatusModel StatusModel { get; set; }

        public static PainelModificacaoProdutoInformation ConverterObjeto(Object obj)
        {
            return (PainelModificacaoProdutoInformation)obj;
        }
    }
}
