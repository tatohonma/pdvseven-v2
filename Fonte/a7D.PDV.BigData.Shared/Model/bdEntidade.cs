using System;
using System.ComponentModel.DataAnnotations;

namespace a7D.PDV.BigData.Shared.Model
{
    public class bdEntidade
    {
        [Required]
        public string Estabelecimento { get; set; }

        public string CNPJ { get; set; }

        public string RazaoSocial { get; set; }

        public string Versao { get; set; }

        public DateTime? UltimoSincronismo { get; set; } 
    }
}
