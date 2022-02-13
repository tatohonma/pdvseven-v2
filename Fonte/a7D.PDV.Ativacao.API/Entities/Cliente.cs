using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace a7D.PDV.Ativacao.API.Entities
{
    [Table("tbCliente")]
    [DataContract]
    public class Cliente
    {
        [Key]
        [DataMember]
        public int IDCliente { get; set; }

        [Display(Name = "Revenda")]
        [Required]
        [DataMember]
        public int IDRevenda { get; set; }

        [DataMember]
        public virtual Revenda Revenda { get; set; }

        [Display(Name = "Cliente")]
        [DataMember]
        [Required]
        public string Nome { get; set; }

        [DataMember]
        public string RazaoSocial { get; set; }

        [DataMember]
        public decimal? CNPJCPF { get; set; }

        [DataMember]
        public string Logradouro { get; set; }

        [DataMember]
        public string Numero { get; set; }

        [DataMember]
        public string Complemento { get; set; }

        [DataMember]
        public string Cidade { get; set; }

        [DataMember]
        public string UF { get; set; }

        [DataMember]
        public string Telefone { get; set; }
        [DataMember]
        public string IDTiny { get; set; }
    }
}