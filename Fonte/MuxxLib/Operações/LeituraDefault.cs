using MuxxLib.Estruturas;
using MuxxLib.Interface;

namespace MuxxLib.Operações
{
    public class LeituraDefault : ITipoDeDado
    {
        private ushort identificador;
        private string opcaoDefault;
       
        public LeituraDefault(ushort wIdentificador, string valor)
        {
            this.identificador = wIdentificador;
            this.opcaoDefault = valor;
        }

        public short Operacao(ushort i, ref string message)
        {
            message = opcaoDefault;
            return (short)Interop.PW_iAddParam(identificador, opcaoDefault);
        }
    }
}
