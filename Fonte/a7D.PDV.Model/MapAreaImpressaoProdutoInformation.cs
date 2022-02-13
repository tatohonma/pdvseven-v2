using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbMapAreaImpressaoProduto")]
    [Serializable]
    public class MapAreaImpressaoProdutoInformation
    {

        public MapAreaImpressaoProdutoInformation()
        {
            Status = StatusModel.Inalterado;
        }

        [CRUDParameterDAL(true, "IDMapAreaImpressaoProduto")]
        public Int32? IDMapAreaImpressaoProduto { get; set; }

        [CRUDParameterDAL(false, "IDAreaImpressao", "IDAreaImpressao")]
        public AreaImpressaoInformation AreaImpressao { get; set; }

        [CRUDParameterDAL(false, "IDProduto", "IDProduto")]
        public ProdutoInformation Produto { get; set; }

        public StatusModel Status { get; set; }

        public static MapAreaImpressaoProdutoInformation ConverterObjeto(Object obj)
        {
            return (MapAreaImpressaoProdutoInformation)obj;
        }
    }
}
