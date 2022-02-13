using a7D.Fmk.CRUD.DAL;
using System;

namespace a7D.PDV.Model
{

    [CRUDClassDAL("tbClassificacaoFiscal")]
    [Serializable]
    public class ClassificacaoFiscalInformation
    {
        [CRUDParameterDAL(true, "IDClassificacaoFiscal")]
        public int? IDClassificacaoFiscal { get; set; }

        [CRUDParameterDAL(false, "IDTipoTributacao", "IDTipoTributacao")]
        public TipoTributacaoInformation TipoTributacao { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public string Descricao { get; set; }

        [CRUDParameterDAL(false, "NCM")]
        public string NCM { get; set; }

        [CRUDParameterDAL(false, "CEST")]
        public string CEST { get; set; }

        [CRUDParameterDAL(false, "IOF")]
        public decimal? IOF { get; set; }

        [CRUDParameterDAL(false, "IPI")]
        public decimal? IPI { get; set; }

        [CRUDParameterDAL(false, "PISPASEP")]
        public decimal? PISPASEP { get; set; }

        [CRUDParameterDAL(false, "COFINS")]
        public decimal? COFINS { get; set; }

        [CRUDParameterDAL(false, "CIDE")]
        public decimal? CIDE { get; set; }

        [CRUDParameterDAL(false, "ICMS")]
        public decimal? ICMS { get; set; }

        [CRUDParameterDAL(false, "ISS")]
        public decimal? ISS { get; set; }


        public static ClassificacaoFiscalInformation ConverterObjeto(object obj)
        {
            return (ClassificacaoFiscalInformation)obj;
        }
    }
}