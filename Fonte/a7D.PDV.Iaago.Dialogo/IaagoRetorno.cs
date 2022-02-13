using System.Collections.Generic;

namespace a7D.PDV.Iaago.Dialogo
{
    public class IaagoRetorno : IaagoAtribuicao
    {
        public string condicao { get; set; }

        public string intencao { get; set; }

        public object mensagem { get; set; }

        public string resultado { get; set; }

        public string card { get; set; }

        public int? aguardar { get; set; }

        public bool? ignorarMensagemIntencao { get; set; }

        public bool? retorno { get; set; }

        public string ExibirMensagem(IaagoVars userIaago)
        {
            string final = userIaago.PreencheMensagem(mensagem);

            if (!string.IsNullOrEmpty(resultado) && userIaago["@api"] is DictionaryObject api && api.ContainsKey("itens"))
            {
                var itens = (List<object>)api["itens"];
                foreach (var item in itens)
                {
                    if (item is DictionaryObject campos)
                    {
                        string linha = Interpretador.FormataMensagem("\r\n" + resultado, key => campos.ContainsKey(key.Substring(1)) ? campos[key.Substring(1)] : string.Empty);
                        final += linha;
                    }
                }
            }

            return final;
        }

        public bool ValidaCondicaoER(IaagoVars userIaago)
        {
            return Interpretador.ValidaCondicao(condicao, userIaago.Text, key => userIaago.InterpretaValor(key));
        }
    }

}
