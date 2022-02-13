using System;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    [DataContract]
    public class ErroResult
    {
        [DataMember(EmitDefaultValue = false)]
        public int? Codigo { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Mensagem { get; set; }

        public ErroResult()
        {
        }

        public ErroResult(string Mensagem )
        {
            this.Mensagem = Mensagem;
        }

        public ErroResult(int? Codigo = default(int?), string Mensagem = default(string))
        {
            this.Codigo = Codigo;
            this.Mensagem = Mensagem;
        }

        public ErroResult(int codigo, Exception ex)
        {
            Codigo = codigo;
            string msg = "";
            while (ex != null)
            {
                msg += ex.Message + " \r\n";
                ex = ex.InnerException;
            }
            Mensagem = msg;
        }
    }
}
