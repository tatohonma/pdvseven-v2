using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbTipoSolicitacaoSAT")]
    [Serializable]
    public class TipoSolicitacaoSATInformation
    {
        [CRUDParameterDAL(true, "IDTipoSolicitacaoSAT")]
        public int? IDTipoSolicitacaoSAT { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        public ETipoSolicitacaoSAT TipoSolicitacaoSAT => (IDTipoSolicitacaoSAT.HasValue ? (ETipoSolicitacaoSAT)IDTipoSolicitacaoSAT.Value : ETipoSolicitacaoSAT.SEM_TIPO);

        public static TipoSolicitacaoSATInformation ConverterObjeto(Object obj)
        {
            return (TipoSolicitacaoSATInformation)obj;
        }
    }

    public enum ETipoSolicitacaoSAT
    {
        SEM_TIPO,
        ENVIAR_DADOS_VENDA = 1,
        CANCELAR_VENDA = 2
    }
}
