using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbConfiguracaoBD")]
    public class ConfiguracaoBDInformation
    {
        [CRUDParameterDAL(true, "IDConfiguracaoBD")]
        public int? IDConfiguracaoBD { get; set; }

        [CRUDParameterDAL(false, "IDTipoPDV", "IDTipoPDV")]
        public TipoPDVInformation TipoPDV { get; set; }

        [CRUDParameterDAL(false, "IDPDV", "IDPDV")]
        public PDVInformation PDV { get; set; }

        [CRUDParameterDAL(false, "Chave")]
        public string Chave { get; set; }

        [CRUDParameterDAL(false, "Valor")]
        public string Valor { get; set; }

        [CRUDParameterDAL(false, "ValoresAceitos")]
        public string ValoresAceitos { get; set; }

        [CRUDParameterDAL(false, "Obrigatorio")]
        public bool? Obrigatorio { get; set; }

        [CRUDParameterDAL(false, "Titulo")]
        public string Titulo { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
