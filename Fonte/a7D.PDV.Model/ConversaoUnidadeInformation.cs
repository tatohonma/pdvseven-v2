using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbConversaoUnidade")]
    public class ConversaoUnidadeInformation
    {
        [CRUDParameterDAL(true, "IDConversaoUnidade")]
        public int? IDConversaoUnidade { get; set; }

        [CRUDParameterDAL(false, "IDUnidade_de", "IDUnidade")]
        public UnidadeInformation Unidade_de { get; set; }

        [CRUDParameterDAL(false, "IDUnidade_para", "IDUnidade")]
        public UnidadeInformation Unidade_para { get; set; }

        [CRUDParameterDAL(false, "Divisao")]
        public decimal? Divisao { get; set; }

        [CRUDParameterDAL(false, "Multiplicacao")]
        public decimal? Multiplicacao { get; set; }
    }
}
