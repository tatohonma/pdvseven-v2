using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{
    public class usuarioRequest
    {
        public string senha;

        public usuarioRequest()
        {
        }

        public usuarioRequest(string chave)
        {
            senha = chave;
        }
    }

    [DataContract]
    public class usuarioResult : ResultadoOuErro
    {
        [DataMember(EmitDefaultValue = false)]
        public int idUsuario;

        [DataMember(EmitDefaultValue = false)]
        public string nome;
    }
}
