using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTemaCardapio")]
    [Serializable]
    public class TemaCardapioInformation
    {
        [CRUDParameterDAL(true, "IDTemaCardapio")]
        public Int32? IDTemaCardapio { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "Descricao")]
        public String Descricao { get; set; }

        [CRUDParameterDAL(false, "XML")]
        public String XML { get; set; }

        public static TemaCardapioInformation ConverterObjeto(Object obj)
        {
            return (TemaCardapioInformation)obj;
        }
    }
}
