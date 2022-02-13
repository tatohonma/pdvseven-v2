using a7D.Fmk.CRUD.DAL;
using System;

namespace a7D.PDV.Model
{
    [Serializable]
    [CRUDClassDAL("tbStatusProcessamentoSAT")]
    public class StatusProcessamentoSATInformation
    {
        [CRUDParameterDAL(true, "IDStatusProcessamentoSAT")]
        public int? IDStatusProcessamentoSAT { get; set; }

        [CRUDParameterDAL(false, "Nome")]
        public string Nome { get; set; }

        public EStatusProcessamentoSAT Status => (IDStatusProcessamentoSAT.HasValue ? (EStatusProcessamentoSAT)IDStatusProcessamentoSAT.Value : EStatusProcessamentoSAT.SEM_STATUS);

    }

    public enum EStatusProcessamentoSAT
    {
        SEM_STATUS = -1,
        NAO_INICIADO = 10,
        PROCESSANDO = 20,
        SUCESSO = 30,
        ABORTADO = 40,
        ERRO = 50
    }
}