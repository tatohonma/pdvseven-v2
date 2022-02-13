using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.Model
{
    [Serializable]
    public class ProdutoProducaoInformation
    {
        public Int32? IDProdutoProducao { get; set; }

        public ProdutoInformation Produto { get; set; }

        public string Quantidade { get; set; }

        public string Unidade { get; set; }

        public StatusModel StatusModel { get; set; }

        public static ProdutoProducaoInformation ConverterObjeto(Object obj)
        {
            return (ProdutoProducaoInformation)obj;
        }
    }
}
