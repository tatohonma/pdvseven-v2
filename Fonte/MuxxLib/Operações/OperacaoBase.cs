using MuxxLib.Enum_s;
using MuxxLib.Interface;
using System.Text;

namespace MuxxLib.Operações
{
    public abstract class OperacaoBase : ITipoDeDado
    {

        #region Atributos

        private short _TempReturn;
        private PWRET _TempCode => (PWRET)_TempReturn;
        private StringBuilder _DisplayMessage;

        #endregion

        #region Metodos

        public OperacaoBase()
        {
            _TempReturn = 0;
            _DisplayMessage = new StringBuilder(100, 100);
        }

        public short Operacao(ushort i, ref string message)
        {
            return OperacaoPadrao(ref message);
        }

        private short OperacaoPadrao(ref string message)
        {
            this._TempReturn = Interop.PW_iPPEventLoop(_DisplayMessage, 100);
            if (_TempCode == PWRET.DISPLAY)
                message = this._DisplayMessage.ToString();
            
            return _TempReturn;
        }

        #endregion

    }
}
