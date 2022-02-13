namespace a7D.PDV.EF.Models
{
    public class tbBandeira : IValueName
    {
        public int IDBandeira { get; set; }

        public string Nome { get; set; }

        public ValueName GetValueName() => new ValueName(IDBandeira, Nome);

        public void SetValueName(int value, string name)
        {
            IDBandeira = value;
            Nome = name;
        }
    }
}
