using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Ativacao.API.Exceptions
{

    [Serializable]
    public class CadastroPendenteException : Exception
    {
        public CadastroPendenteException() { }
        public CadastroPendenteException(string message) : base(message) { }
        public CadastroPendenteException(string message, Exception inner) : base(message, inner) { }
        protected CadastroPendenteException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}