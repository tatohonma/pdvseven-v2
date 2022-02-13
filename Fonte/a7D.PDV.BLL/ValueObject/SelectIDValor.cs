namespace a7D.PDV.BLL.ValueObject
{
    public delegate int onSelectHandler(string subTitle, SelectIDValor[] opcoes);

    public class SelectIDValor
    {
        public static event onSelectHandler onSelect;

        public static int Select(string sb, SelectIDValor[] opcoes) 
            => onSelect(sb, opcoes);

        public int ID { get; set; }
        public string Texto { get; set; }

        public SelectIDValor()
        {
        }

        public SelectIDValor(int iDAreaImpressao, string nome)
        {
            this.ID = iDAreaImpressao;
            this.Texto = nome;
        }
        
        public override string ToString() 
            => Texto;
    }
}
