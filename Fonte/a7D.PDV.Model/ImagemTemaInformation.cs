using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbImagemTema")]
    [Serializable]
    public class ImagemTemaInformation
    {
        [CRUDParameterDAL(true, "IDImagemTema")]
        public int? IDImagemTema { get; set; }

        [CRUDParameterDAL(false, "IDTemaCardapio", "IDTemaCardapio")]
        public TemaCardapioInformation TemaCardapio { get; set; }

        [CRUDParameterDAL(false, "IDImagem", "IDImagem")]
        public ImagemInformation Imagem { get; set; }

        [CRUDParameterDAL(false, "IDIdioma", "IDIdioma")]
        public IdiomaInformation Idioma { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public string Descricao { get; set; }
    }
}

