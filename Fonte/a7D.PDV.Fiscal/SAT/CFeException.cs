using System;
using System.Runtime.Serialization;

namespace a7D.PDV.Fiscal.SAT
{
    [Serializable]
    public class CFeException : NotImplementedException
    {
        public CFeException() : base("Versão da CFe configurada não está implementada") { }
        protected CFeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
