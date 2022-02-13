using System;
using a7D.PDV.EF.Enum;

namespace a7D.PDV.EF.Models
{
    public class tbGateway : IValueName
    {
        public int IDGateway { get; set; }

        public string Nome { get; set; }

        public EGateway Tipo => (EGateway)IDGateway;

        public static tbGateway FromID(int iDGateway) => EF.Repositorio.Carregar<tbGateway>(g => g.IDGateway == iDGateway);

        public ValueName GetValueName() => new ValueName(IDGateway, Nome);

        public void SetValueName(int value, string name)
        {
            IDGateway = value;
            Nome = name;
        }
    }
}
