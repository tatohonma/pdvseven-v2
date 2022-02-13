using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbTipoMovimentacao")]
    public class TipoMovimentacaoInformation
    {
        public enum TipoMovimentacao
        {
            ENTRADA = 1,
            SAIDA = 2
        }

        [CRUDParameterDAL(true, "IDTipoMovimentacao")]
        public int? IDTipoMovimentacao { get; set; }

        [CRUDParameterDAL(false, "Tipo")]
        public string Tipo { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public string Descricao { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public bool Excluido { get; set; }

        public override int GetHashCode()
        {
            int i = 179;
            if (IDTipoMovimentacao.HasValue == false)
                return (i * 7) + base.GetHashCode();
            else
                return IDTipoMovimentacao.Value.GetHashCode() * i;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null)
                return false;

            var cObj = obj as TipoMovimentacaoInformation;

            if (cObj == null)
                return false;

            return GetHashCode() == cObj.GetHashCode();
        }
    }
}
