using System;
using System.Linq;

namespace a7D.PDV.Iaago.Dialogo
{
    public class IaagoResposta : IaagoAtribuicao
    {
        public string tipoResposta { get; set; }

        public IaagoRetorno[] retorno { get; set; }

        internal IaagoRetorno Retorno(IaagoVars userIaago)
        {
            IaagoRetorno ret = null;
            if (retorno == null)
            {
                throw new Exception("Todo objeto de Resposta precisa conter pelo menos um elemento do array de Retorno");
            }

            if (tipoResposta == "bool")
            {
                string sn = Interpretador.SimNao(userIaago.Text);
                if (sn != null)
                {
                    ret = retorno.FirstOrDefault(c => c.condicao == sn);
                }
            }
            else if (tipoResposta == "datetime")
            {
                if (Interpretador.ObterDateTime(userIaago.Text, out DateTime dt1, out DateTime dt2)
                 && dt1 > DateTime.MinValue)
                {
                    userIaago.Add("@datetime", dt1, false);
                    userIaago.Add("@datetime2", dt2, false);
                    ret = retorno.FirstOrDefault(c => c.ValidaCondicaoER(userIaago));
                }
            }
            else
            {
                // "text" ou null (vazio)
                ret = retorno.FirstOrDefault(c => c.ValidaCondicaoER(userIaago));

                if (ret == null)
                {
                    // Se não achou uma condição procura uma default
                    ret = retorno.FirstOrDefault(c => c.condicao == null);
                }
            }

            return ret;
        }
    }
}