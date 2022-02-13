using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbUsuario")]
    [Serializable]
    public class UsuarioInformation : IERP
    {
        [CRUDParameterDAL(true, "IDUsuario")]
        public Int32? IDUsuario { get; set; }

        [CRUDParameterDAL(false, "CodigoERP")]
        public String CodigoERP { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public String Nome { get; set; }

        [CRUDParameterDAL(false, "Login")]
        public String Login { get; set; }

        [CRUDParameterDAL(false, "Senha")]
        public String Senha { get; set; }

        [CRUDParameterDAL(false, "PermissaoAdm")]
        public Boolean? PermissaoAdm { get; set; }

        [CRUDParameterDAL(false, "PermissaoCaixa")]
        public Boolean? PermissaoCaixa { get; set; }

        [CRUDParameterDAL(false, "PermissaoGarcom")]
        public Boolean? PermissaoGarcom { get; set; }

        [CRUDParameterDAL(false, "PermissaoGerente")]
        public Boolean? PermissaoGerente { get; set; }

        [CRUDParameterDAL(false, "Ativo")]
        public Boolean? Ativo { get; set; }

        [CRUDParameterDAL(false, "DtUltimaAlteracao")]
        public DateTime? DtUltimaAlteracao { get; set; }

        [CRUDParameterDAL(false, "Excluido")]
        public Boolean? Excluido { get; set; }

        public static UsuarioInformation ConverterObjeto(Object obj)
        {
            return (UsuarioInformation)obj;
        }

        public bool RequerAlteracaoERP(DateTime dtSync) => DtUltimaAlteracao > dtSync;
    }
}
