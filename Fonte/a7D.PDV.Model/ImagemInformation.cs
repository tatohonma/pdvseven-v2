using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbImagem")]
    public class ImagemInformation
    {
        [CRUDParameterDAL(true, "IDImagem")]
        public int? IDImagem { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        [CRUDParameterDAL(false, "Extensao")]
        public string Extensao { get; set; }

        [CRUDParameterDAL(false, "Altura")]
        public int? Altura { get; set; }

        [CRUDParameterDAL(false, "Largura")]
        public int? Largura { get; set; }

        [CRUDParameterDAL(false, "Tamanho")]
        public int? Tamanho { get; set; }

        [CRUDParameterDAL(false, "Dados")]
        public byte[] Dados { get; set; }
    }
}
