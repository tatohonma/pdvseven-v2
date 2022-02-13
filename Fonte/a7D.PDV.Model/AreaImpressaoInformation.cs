using a7D.Fmk.CRUD.DAL;
using System;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbAreaImpressao")]
    [Serializable]
    public class AreaImpressaoInformation
    {
        [CRUDParameterDAL(true, "IDAreaImpressao")]
        public Int32? IDAreaImpressao { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "NomeImpressora")]
        public String NomeImpressora { get; set; } // Ou o nome do PDV + Impressora Local

        [CRUDParameterDAL(false, "IDPDV", "IDPDV")]
        public PDVInformation PDV { get; set; } // Impressora Windows, Caixa

        [CRUDParameterDAL(false, "IDTipoAreaImpressao", "IDTipoAreaImpressao")]
        public TipoAreaImpressaoInformation TipoAreaImpressao { get; set; }

        public static AreaImpressaoInformation ConverterObjeto(Object obj)
        {
            return (AreaImpressaoInformation)obj;
        }

        public override string ToString()
        {
            return $"{IDAreaImpressao}: {Nome}";
        }
    }
}
