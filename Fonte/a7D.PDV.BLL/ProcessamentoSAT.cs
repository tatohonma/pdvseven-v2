using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF.Models;
using a7D.PDV.Model;

namespace a7D.PDV.BLL
{
    public static class ProcessamentoSAT
    {
        public static ProcessamentoSATInformation Carregar(int idProcessamentoSAT)
        {
            var obj = new ProcessamentoSATInformation { IDProcessamentoSAT = idProcessamentoSAT };
            return (ProcessamentoSATInformation)CRUD.Carregar(obj);
        }

        public static ProcessamentoSATInformation Carregar(string GUID, ETipoSolicitacaoSAT tipo)
        {
            var obj = new ProcessamentoSATInformation { GUID = GUID, IDTipoSolicitacaoSAT = (int)tipo };
            var result = (ProcessamentoSATInformation)CRUD.Carregar(obj);

            if (result.IDProcessamentoSAT.HasValue == false)
                return null;

            return result;
        }

        public static void Salvar(ProcessamentoSATInformation processamentoSAT)
        {
            CRUD.Salvar(processamentoSAT);
        }

        public static void AlterarStatus(int idProcessamentoSAT, EStatusProcessamentoSAT status)
        {
            EF.Repositorio.Execute($"UPDATE tbProcessamentoSAT SET IDStatusProcessamentoSAT={(int)status} WHERE idProcessamentoSAT={idProcessamentoSAT}");
        }
    }
}
