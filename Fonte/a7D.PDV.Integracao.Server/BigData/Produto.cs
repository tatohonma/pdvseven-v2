namespace a7D.PDV.Integracao.Server.IaagoBot
{
    public class Produto
    {
        public int IDProduto;
        public string Nome;
        public int IDProdutoGlobal;
        public string EAN;
        public decimal QTDatual;
        public decimal Valor;
        public decimal EstoqueMinimo;
        public decimal EstoqueIdeal;
        public int EstoqueFrequenciaValor; // Numero de dias, semenas ou meses
        public string EstoqueFrequenciaTipo; // Semanal, Mensal, Anual, ou dia da semana: SEG, TER, QUA, QUI, SEX, SAB, DOM

        public override string ToString()
        => $"{IDProduto}: {Nome} => {QTDatual}";
    }
}
