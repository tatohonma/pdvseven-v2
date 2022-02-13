using MuxxLib.Enum_s;
using MuxxLib.Estruturas;
using MuxxLib.Interface;

namespace MuxxLib.Operações
{
    public class MenuDefault : ITipoDeDado
    {
        private ushort identificador;
        private string opcaoSelecionada;
        private string opcaoDefault;
       
        public MenuDefault(PW_GetData pW_GetData)
        {
            this.identificador = pW_GetData.wIdentificador;
            this.opcaoSelecionada = pW_GetData.stMenu.szTexto1;
            this.opcaoDefault = pW_GetData.stMenu.szValor1;
        }

        public short Operacao(ushort i, ref string message)
        {
            message = opcaoSelecionada;
            return (short)Interop.PW_iAddParam(identificador, opcaoDefault);
        }
    }
}
