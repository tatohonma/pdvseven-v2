using a7D.Fmk.CRUD.DAL;
using System;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbUnidade")]
    [Serializable]
    public class UnidadeInformation
    {
        [CRUDParameterDAL(true, "IDUnidade")]
        public int? IDUnidade { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        [CRUDParameterDAL(false, "Simbolo")]
        public string Simbolo { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public bool? Excluido { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var unidade = obj as UnidadeInformation;

            if (unidade == null)
                return false;

            return this == unidade;
        }

        public override int GetHashCode()
        {
            int prime = 31;
            if (IDUnidade.HasValue)
                return IDUnidade.Value.GetHashCode() * prime * base.GetHashCode();
            return base.GetHashCode();
        }

        public static bool operator ==(UnidadeInformation a, UnidadeInformation b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.IDUnidade.Value == b.IDUnidade.Value;
        }

        public static bool operator !=(UnidadeInformation a, UnidadeInformation b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"{IDUnidade ?? 0}: {Nome}";
        }
    }
}