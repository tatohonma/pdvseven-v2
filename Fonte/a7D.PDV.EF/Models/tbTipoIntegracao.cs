using a7D.PDV.EF.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace a7D.PDV.EF.Models
{
    [Table("tbTipoIntegracao")]
    public class tbTipoIntegracao : IValueName
    {
        public tbTipoIntegracao()
        {
            Integracoes = new List<tbIntegracao>();
        }

        [Key]
        [Required]
        public int IDTipoIntegracao { get; set; }

        [Required]
        public string Nome { get; set; }

        public virtual ICollection<tbIntegracao> Integracoes { get; set; }

        public ETipoIntegracao Tipo => (ETipoIntegracao)IDTipoIntegracao;

        public ValueName GetValueName() => new ValueName(IDTipoIntegracao, Nome);

        public void SetValueName(int value, string name)
        {
            IDTipoIntegracao = value;
            Nome = name;
        }
    }
}
