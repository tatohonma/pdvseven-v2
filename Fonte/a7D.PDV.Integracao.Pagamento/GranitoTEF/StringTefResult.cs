using System.Text;

namespace a7D.PDV.Integracao.Pagamento.GranitoTEF
{
    public class StringTefResult
    {
        public StringTefResult()
        {
            Mensagem = "";
        }

        public StringTefResult(StringBuilder valor)
        {
            var value = valor.ToString();
            Tipo = int.Parse(value.Substring(0, 1));
            Identificador = int.Parse(value.Substring(1, 6));
            Mensagem = value.Substring(7).Trim();
        }

        public int Tipo { get; private set; }

        public int Identificador { get; private set; }

        public string Mensagem { get; private set; }
    }
}