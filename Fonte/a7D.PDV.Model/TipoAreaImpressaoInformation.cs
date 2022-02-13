using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTipoAreaImpressao")]
    public class TipoAreaImpressaoInformation
    {
        [CRUDParameterDAL(true, "IDTipoAreaImpressao")]
        public int? IDTipoAreaImpressao { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        public ETipoAreaImpressao TipoAreaImpressao
        {
            get { return (ETipoAreaImpressao)IDTipoAreaImpressao; }
            set { IDTipoAreaImpressao = (int)value; }
        }
    }
}
