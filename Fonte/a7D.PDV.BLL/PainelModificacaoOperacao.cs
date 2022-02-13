using a7D.Fmk.CRUD.DAL;
using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.BLL
{
    public static class PainelModificacaoOperacao
    {
        public static List<PainelModificacaoOperacaoInformation> Listar()
        {
            return CRUD.Listar(new PainelModificacaoOperacaoInformation()).Cast<PainelModificacaoOperacaoInformation>().ToList();
        }

        public static PainelModificacaoOperacaoInformation Carregar(int idPainelModificacaoOperacao)
        {
            return CRUD.Carregar(new PainelModificacaoOperacaoInformation { IDPainelModificacaoOperacao = idPainelModificacaoOperacao }) as PainelModificacaoOperacaoInformation;
        }
    }
}
