namespace a7D.PDV.EF.Models
{
    public class tbTamanhoPacote : IValueName
    {
        public int IDTamanhoPacote { get; set; }

        public string Nome { get; set; }

        public ValueName GetValueName() => new ValueName(IDTamanhoPacote, Nome);

        public void SetValueName(int value, string name)
        {
            IDTamanhoPacote = value;
            Nome = name;
        }
    }
}
