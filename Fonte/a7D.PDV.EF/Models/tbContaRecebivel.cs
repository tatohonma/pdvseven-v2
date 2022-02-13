using System;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.EF.Models
{
    public class tbContaRecebivel : IValueName
    {
        public int IDContaRecebivel { get; set; }

        public string Nome { get; set; }

        public string CodigoIntegracao { get; set; }

        public ValueName GetValueName() => new ValueName(IDContaRecebivel, Nome);

        public void SetValueName(int value, string name)
        {
            IDContaRecebivel = value;
            Nome = name;
        }
    }
}
