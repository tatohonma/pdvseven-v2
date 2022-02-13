using System.Threading.Tasks;

namespace a7D.PDV.Iaago.Dialogo
{
    public abstract class IaagoAtribuicao
    {
        public IaagoAPI api { get; set; }

        public string[] atribuir { get; set; }

        public void ExecutarAtribuicoesER(IaagoVars userIaago)
        {
            api?.Call(userIaago).Wait();
            if (atribuir == null)
            {
                return;
            }

            Interpretador.ExecutarAtribuicoes(atribuir, (key, value) => userIaago.Add(key, value, true));
        }
    }
}
