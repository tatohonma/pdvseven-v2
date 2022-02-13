using System.Text;

namespace a7D.PDV.Integracao.Pagamento.GranitoTEF
{
    public class StringPdvResponse
    {
        public StringPdvResponse()
        {
            DadoColetado = "";
            Continuar = true;
        }

        public int Tipo { get; set; }

        public int Identificador { get; set; }

        public bool Continuar { get; set; }

        public string DadoColetado { get; set; }

        public string Parametro =>
            Tipo.ToString("0") +
            Identificador.ToString("000000") +
            (Continuar ? "0" : "1") +
            DadoColetado.Length.ToString("00") +
            DadoColetado;

        public static implicit operator StringBuilder(StringPdvResponse instancia)
        {
            return new StringBuilder(instancia.Parametro);
        }
    }
}