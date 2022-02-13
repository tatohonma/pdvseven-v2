using a7D.PDV.Model;

namespace a7D.PDV.Configurador.UI
{
    public class PDVExibicao
    {
        public PDVExibicao()
        {
        }

        public PDVExibicao(PDVInformation pdv)
        {
            IDPDV = pdv.IDPDV.Value;
            Nome = pdv.Nome;
        }

        public int IDPDV { get; set; }
        public string Nome { get; set; }
        public string Exibicao
        {
            get
            {
                if (IDPDV > 0)
                    return $"({IDPDV}) {Nome}";
                else
                    return Nome;
            }
        }
    }
}
