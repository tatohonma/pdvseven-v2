using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public partial class tbTipoAreaImpressao : IValueName
    {
        public tbTipoAreaImpressao()
        {
            this.tbAreaImpressaos = new List<tbAreaImpressao>();
        }

        public int IDTipoAreaImpressao { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<tbAreaImpressao> tbAreaImpressaos { get; set; }

        public ValueName GetValueName() => new ValueName(IDTipoAreaImpressao, Nome);

        public void SetValueName(int value, string name)
        {
            IDTipoAreaImpressao = value;
            Nome = name;
        }
    }
}
