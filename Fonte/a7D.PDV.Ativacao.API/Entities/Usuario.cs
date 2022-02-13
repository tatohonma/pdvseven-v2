using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace a7D.PDV.Ativacao.API.Entities
{

    [Table("tbUsuario")]
    [DataContract]
    public class Usuario
    {
        [Key]
        [DataMember]
        public int IDUsuario { get; set; }

        [DataMember]
        public string Nome { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Senha { get; set; }

        [DataMember]
        public bool Ativo { get; set; }

        [DataMember]
        public DateTime DtUltimaAlteracao { get; set; }

        [DataMember]
        public bool Excluido { get; set; }

        [DataMember]
        public string HashAlterarSenha { get; set; }

        [DataMember]
        public DateTime? DtSolicitacaoAlteracaoSenha { get; set; }

        [DataMember]
        public bool CadastroPendente { get; set; }

        [DataMember]
        public bool Adm { get; set; }
    }
}