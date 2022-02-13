using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbPainelModificacaoOperacao")]
    public class PainelModificacaoOperacaoInformation
    {
        [CRUDParameterDAL(true, "IDPainelModificacaoOperacao")]
        public int? IDPainelModificacaoOperacao { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + IDPainelModificacaoOperacao.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var c = obj as PainelModificacaoOperacaoInformation;
            if (c == null)
                return false;
            return IDPainelModificacaoOperacao == c.IDPainelModificacaoOperacao;
        }
    }
}
