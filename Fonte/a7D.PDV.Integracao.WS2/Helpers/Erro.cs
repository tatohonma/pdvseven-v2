using System;
using System.Runtime.Serialization;
using a7D.PDV.BLL;
using a7D.PDV.BLL.Utils;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class Erro : ErroResult
    {
        public Erro() : base()
        {
        }
     
        public Erro(int? Codigo = default(int?), string Mensagem = default(string)): base(Codigo,Mensagem)
        {
        }

        public Erro(CodigoErro codigo, Exception ex = null, string info = null)
        {
            ExceptionPDV exPDV;
            if (ex is ExceptionPDV)
                exPDV = ex as ExceptionPDV;
            else
                exPDV = new ExceptionPDV(codigo, ex, info);

            Codigo = int.Parse(codigo.ToString(), System.Globalization.NumberStyles.HexNumber);
            Mensagem = ExceptionHelper.InnerExceptionMessageLoop(exPDV);
        }

        public Erro(CodigoErro codigo, string info = null)
        {
            var exPDV = new ExceptionPDV(codigo, info);

            Codigo = int.Parse(codigo.ToString(), System.Globalization.NumberStyles.HexNumber);
            Mensagem = ExceptionHelper.InnerExceptionMessageLoop(exPDV);
        }

        public Erro(int codigo, Exception ex): base(codigo,ex)
        {
        }
    }
}
