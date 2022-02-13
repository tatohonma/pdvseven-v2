using a7D.PDV.Ativacao.Shared.DTO;
using a7D.PDV.EF.Models;
using System.Collections.Generic;

namespace a7D.PDV.Componentes.Services
{
    public static class MensagemServices
    {
        public static IEnumerable<tbMensagem> RetornaMensagens()
        {
            int idInfo = (int)ETipoMensagem.Informacao;
            int idResposta = (int)ETipoMensagem.Resposta;
            return EF.Repositorio.Listar<tbMensagem>(m => m.IDTipo >= idInfo && m.IDTipo < idResposta && (!m.DataVisualizada.HasValue || m.IDRespostaProcesamento == -2));
        }

        public static void ExibeMensagens(IEnumerable<tbMensagem> msgList)
        {
            foreach (var msg in msgList)
                frmMensagem.Show(msg);
        }
    }
}
