using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace a7D.PDV.AtivacaoOld.API.Entities
{
    [Table("tbPrioridade")]
    public class Prioridade
    {
        [Key]
        public int IDPrioridade { get; set; }
        public string Nome { get; set; }
    }
}