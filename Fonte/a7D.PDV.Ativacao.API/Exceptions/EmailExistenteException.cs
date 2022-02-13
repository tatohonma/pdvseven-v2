using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace a7D.PDV.Ativacao.API.Exceptions
{

    [Serializable]
    public class EmailExistenteException : Exception
    {
        public EmailExistenteException() { }
        public EmailExistenteException(string message) : base(message) { }
        public EmailExistenteException(string message, Exception inner) : base(message, inner) { }
        protected EmailExistenteException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}