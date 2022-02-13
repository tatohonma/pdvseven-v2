using a7D.Fmk.CRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbIdioma")]
    [Serializable]
    public class IdiomaInformation
    {
        [CRUDParameterDAL(true, "IDIdioma")]
        public Int32? IDIdioma { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "Codigo")]
        public String Codigo { get; set; }

        public static IdiomaInformation ConverterObjeto(Object obj)
        {
            return (IdiomaInformation)obj;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj as IdiomaInformation == null)
                return false;
            return (obj as IdiomaInformation).IDIdioma == IDIdioma;
        }

        public override int GetHashCode()
        {
            if(IDIdioma.HasValue == false)
                return base.GetHashCode();
            return IDIdioma.Value * 13 * IDIdioma.Value.GetHashCode();
        }
    }
}
