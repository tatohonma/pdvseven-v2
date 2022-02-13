using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.BigData.WebAPI.Models
{
    [Table("ChannelUser")]
    public class ChannelUser
    {
        [Key, Required]
        public int IDChannelUser { get; set; }

        [Required, ForeignKey("Entidade")]
        public int IDEntidade { get; set; }

        [Required]
        public int IDUsuario { get; set; }

        [Required]
        public string ChannelId { get; set; }

        [Required]
        [StringLength(100)]
        public string FromId { get; set; }

        [Required]
        public DateTime LastLogin { get; set; }

        public virtual Entidade Entidade { get; set; }
    }
}