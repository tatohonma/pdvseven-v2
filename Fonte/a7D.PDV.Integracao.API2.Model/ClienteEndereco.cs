using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace a7D.PDV.Integracao.API2.Model
{
    /// <summary>
    /// ClienteEndereco
    /// </summary>
    [DataContract]
    public partial class ClienteEndereco
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClienteEndereco" /> class.
        /// </summary>
        /// <param name="CEP">CEP do endereco deste cliente.</param>
        /// <param name="Endereco">Rua (logradouro) do endereco deste cliente.</param>
        /// <param name="Numero">Numero do endereco deste cliente.</param>
        /// <param name="Complemento">Complemento do endereco deste cliente.</param>
        /// <param name="Bairro">Bairro do endereco deste cliente.</param>
        /// <param name="Cidade">Cidade do endereco deste cliente.</param>
        /// <param name="Referencia">Referencia do endereco deste cliente.</param>
        /// <param name="UF">UF do estado do endereco deste cliente.</param>
        public ClienteEndereco(string CEP = default(string), string Endereco = default(string), string Numero = default(string), string Complemento = default(string), string Bairro = default(string), string Cidade = default(string), string Referencia = default(string), string UF = default(string))
        {
            this.CEP = CEP;
            this.Endereco = Endereco;
            this.Numero = Numero;
            this.Complemento = Complemento;
            this.Bairro = Bairro;
            this.Cidade = Cidade;
            this.Referencia = Referencia;
            this.UF = UF;
        }
        
        /// <summary>
        /// CEP do endereco deste cliente
        /// </summary>
        /// <value>CEP do endereco deste cliente</value>
        [DataMember(Name="CEP", EmitDefaultValue=false)]
        public string CEP { get; set; }
        /// <summary>
        /// Rua (logradouro) do endereco deste cliente
        /// </summary>
        /// <value>Rua (logradouro) do endereco deste cliente</value>
        [DataMember(Name="Endereco", EmitDefaultValue=false)]
        public string Endereco { get; set; }
        /// <summary>
        /// Numero do endereco deste cliente
        /// </summary>
        /// <value>Numero do endereco deste cliente</value>
        [DataMember(Name="Numero", EmitDefaultValue=false)]
        public string Numero { get; set; }
        /// <summary>
        /// Complemento do endereco deste cliente
        /// </summary>
        /// <value>Complemento do endereco deste cliente</value>
        [DataMember(Name="Complemento", EmitDefaultValue=false)]
        public string Complemento { get; set; }
        /// <summary>
        /// Bairro do endereco deste cliente
        /// </summary>
        /// <value>Bairro do endereco deste cliente</value>
        [DataMember(Name="Bairro", EmitDefaultValue=false)]
        public string Bairro { get; set; }
        /// <summary>
        /// Cidade do endereco deste cliente
        /// </summary>
        /// <value>Cidade do endereco deste cliente</value>
        [DataMember(Name="Cidade", EmitDefaultValue=false)]
        public string Cidade { get; set; }
        /// <summary>
        /// Referencia do endereco deste cliente
        /// </summary>
        /// <value>Referencia do endereco deste cliente</value>
        [DataMember(Name="Referencia", EmitDefaultValue=false)]
        public string Referencia { get; set; }
        /// <summary>
        /// UF do estado do endereco deste cliente
        /// </summary>
        /// <value>UF do estado do endereco deste cliente</value>
        [DataMember(Name="UF", EmitDefaultValue=false)]
        public string UF { get; set; }
    }
}
