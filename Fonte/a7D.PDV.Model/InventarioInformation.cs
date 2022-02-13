using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbInventario")]
    public class InventarioInformation
    {
        [CRUDParameterDAL(true, "IDInventario")]
        public int? IDInventario { get; set; }

        [CRUDParameterDAL(false, "GUID")]
        public string GUID { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public string Descricao { get; set; }

        [CRUDParameterDAL(false, "Data")]
        public DateTime? Data { get; set; }

        [CRUDParameterDAL(false, "Processado")]
        public bool? Processado { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public bool? Excluido { get; set; }

        public List<InventarioProdutosInformation> InventarioProdutos { get; set; }

    }
}
