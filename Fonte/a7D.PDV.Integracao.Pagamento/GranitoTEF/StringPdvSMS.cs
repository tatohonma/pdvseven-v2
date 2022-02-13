using System.Text;

namespace a7D.PDV.Integracao.Pagamento.GranitoTEF
{
    public class StringPdvSMS
    {
        public StringPdvSMS(int tipo, int identificador, string sms)
        {
            Tipo = tipo;
            Identificador = identificador;
            NumeroTelefone = sms;
        }

        public int Tipo { get; private set; }

        public int Identificador { get; private set; }

        public string NumeroTelefone { get; private set; }

        public string Parametro =>
            Tipo.ToString("0") +
            Identificador.ToString("000000") +
            "0" + // Continuar
            "1" + // Habilita SMS
            NumeroTelefone;

        public static implicit operator StringBuilder(StringPdvSMS instancia)
        {
            return new StringBuilder(instancia.Parametro);
        }
    }
}