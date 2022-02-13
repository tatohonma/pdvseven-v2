using System.Text;

namespace a7D.PDV.Integracao.Pagamento.GranitoTEF
{
    public class StringTefPdvInit
    {
        public StringTefPdvInit()
        {
            Operacao = PG_TipoOperacao.VendaPresencial;
            Transacao = PG_TipoTransacao.Credito;
            Forma = PG_TipoForma.AVista;
            Modalidade = PG_TipoModalidade.SemJuros;
            Parcelas = 1;
        }

        ///  <summary>
        ///  1 - Operação
        ///  1 (Venda presencial Magnética e EMV) 
        ///  3 (Venda digitada)
        /// </summary>
        public PG_TipoOperacao Operacao { get; set; }

        /// <summary>
        /// 2 a 7 - Identificador entre aplicações - 6 caracteres preenchidos com zeros a esquerda 
        /// </summary>
        public int Identificador { get; set; }

        /// <summary>
        /// 8 a 19 - Documento vinculado - 12 caracteres preenchidos com zeros a esquerda
        /// </summary>
        public long Documento { get; set; }

        /// <summary>
        /// 20 a 31 Valor - 12 caracteres com zeros a esquerda
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// 32 - Tipo de Transação
        /// 1 (Crédito)
        /// 2 (Débito)
        /// </summary>
        public PG_TipoTransacao Transacao { get; set; }

        /// <summary>
        /// 33 - Forma de Pagamento 
        /// 1 (À vista) 
        /// 2 (Parcelado)
        /// </summary>
        public PG_TipoForma Forma { get; set; }

        /// <summary>
        /// 34 - Modalidade de Parcelamento 
        /// 1 (Sem juros) 
        /// 2 (Com juros)
        /// </summary>
        public PG_TipoModalidade Modalidade { get; set; }

        /// <summary>
        ///  35 a 36 - Quantidade de Parcelas 
        /// </summary>
        public int Parcelas { get; set; }

        // --------000000000011111111122222222223333333
        // --------123456789012345678901234567890123456
        //var cmd = "1123456Comanda 123400000000500011";
        //var cmd = "112345612345678901200000000000411000";
        //var cmd = "112345612345678901200000000010121000";

        public string Parametro => 
            ((int)Operacao).ToString("0") + 
            Identificador.ToString("000000") +
            Documento.ToString("000000000000") +
            ((long)(Valor*100)).ToString("000000000000") +
            ((int)Transacao).ToString("0") +
            ((int)Forma).ToString("0") +
            ((int)Modalidade).ToString("0") +
            ((int)Parcelas).ToString("00");

        public static implicit operator StringBuilder(StringTefPdvInit instancia)
        {
            return new StringBuilder(instancia.Parametro);
        }
    }
}