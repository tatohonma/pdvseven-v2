using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbProdutoPainelModificacao")]
    [Serializable]
    public class ProdutoPainelModificacaoInformation
    {
        [CRUDParameterDAL(true, "IDProdutoPainelModificacao")]
        public Int32? IDProdutoPainelModificacao { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        [CRUDParameterDAL(false, "IDPainelModificacao", "IDPainelModificacao")]
        public PainelModificacaoInformation PainelModificacao { get; set; }

        [CRUDParameterDAL(false, "Ordem")]
        public Int32? Ordem { get; set; }

        public StatusModel StatusModel { get; set; }

        public int IDProdutoPai;

        public static ProdutoPainelModificacaoInformation ConverterObjeto(Object obj)
        {
            return (ProdutoPainelModificacaoInformation)obj;
        }

        public override string ToString()
        {
            return $"{IDProdutoPai}> {PainelModificacao} - {Produto}";
        }
    }
}
