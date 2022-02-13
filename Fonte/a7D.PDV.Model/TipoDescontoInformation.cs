using a7D.Fmk.CRUD.DAL;
using System;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbTipoDesconto")]
    public class TipoDescontoInformation
    {
        [CRUDParameterDAL(true, "IDTipoDesconto")]
        public int? IDTipoDesconto { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public string Descricao { get; set; }

        [CRUDParameterDAL(false, "Ativo")]
        public bool? Ativo { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public bool Excluido { get; set; }
    }
}