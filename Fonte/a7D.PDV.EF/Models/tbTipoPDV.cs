using a7D.PDV.EF;
using a7D.PDV.EF.Enum;
using a7D.PDV.EF.Models;
using System;
using System.Collections.Generic;

namespace a7D.PDV.Model
{
    [Serializable]
    public class TipoPDVInformation : IValueName
    {
        public TipoPDVInformation()
        {
            this.tbConfiguracaoBDs = new List<tbConfiguracaoBD>();
        }

        public Int32 IDTipoPDV { get; set; }

        public String Nome { get; set; }

        public ETipoPDV Tipo => (ETipoPDV)IDTipoPDV;

        public virtual ICollection<tbConfiguracaoBD> tbConfiguracaoBDs { get; set; }

        public ValueName GetValueName() => new ValueName(IDTipoPDV, Nome);

        public void SetValueName(int value, string name)
        {
            IDTipoPDV = value;
            Nome = name;
        }
    }
}
