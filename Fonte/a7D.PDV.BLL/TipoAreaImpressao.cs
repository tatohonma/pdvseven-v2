using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class TipoAreaImpressao
    {
        public static TipoAreaImpressaoInformation Carregar(int idTipoAreaImpressao)
        {
            return CRUD.Carregar(new TipoAreaImpressaoInformation { IDTipoAreaImpressao = idTipoAreaImpressao }) as TipoAreaImpressaoInformation;
        }

        public static List<TipoAreaImpressaoInformation> Listar()
        {
            return CRUD.Listar(new TipoAreaImpressaoInformation()).Cast<TipoAreaImpressaoInformation>().ToList();
        }
    }
}
