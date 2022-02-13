namespace a7D.PDV.BLL.ValueObject
{
    public class ItemValor
    {
        public const int ValorWidthPadrao = 70; // numero de pixels para o valor

        public string Item { get; set; }
        public string Valor { get; set; }
        public string Dados { get; set; }
        public int Nivel { get; set; }
        public bool MultiLines { get; set; }
        public int ValorWidth { get; set; }

        private ItemValor(int nivel, bool multLines, int valorWidth)
        {
            this.Nivel = nivel;
            this.MultiLines = multLines;
            this.ValorWidth = valorWidth;
        }

        public ItemValor(string item, string valor = null, string dados = null, int nivel = 0, bool multLines = false, int valorWidth = ValorWidthPadrao)
            : this(nivel, multLines, valorWidth)
        {
            this.Item = item;
            this.Valor = valor;
            this.Dados = dados;
        }

        public ItemValor(string item, decimal valor, string dados = null, int nivel = 0, bool multLines = false, int valorWidth = ValorWidthPadrao)
            : this(item, valor == 0 ? "" : valor.ToString("N2"), dados, nivel, multLines, valorWidth)
        {
        }

        public override string ToString()
        {
            int max;
            string v;
            if (Valor == null)
            {
                max = 40;
                v = "";
            }
            else
            {
                max = 32;
                v = Valor.PadLeft(8);
            }

            if (!string.IsNullOrEmpty(Dados))
            {
                Dados = " " + Dados;
                max -= Dados.Length;
            }

            string i= new string(' ', Nivel) + Item;
            if (i.Length > max)
                i = i.Substring(0, max);
            else
                i = i.PadRight(max);
            
            return $"{i}{Dados}{v}";
        }
    }
}
