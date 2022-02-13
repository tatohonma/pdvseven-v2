namespace a7D.PDV.Iaago.Dialogo
{
    public class IaagoIntencoes : IaagoAtribuicao
    {
        public IaagoLuis[] luis { get; set; }

        public string condicao { get; set; }

        public string intencao { get; set; }

        public object mensagem { get; set; }

        public string card { get; set; }

        public IaagoResposta[] acaoResposta { get; set; }

        public IaagoSugestao[] sugestao { get; set; }

        public override string ToString() => intencao;

        public string ExibirMensagem(IaagoVars userIaago)
        {
            if (userIaago.IgnorarMensagemIntencao == true)
            {
                return string.Empty;
            }
            else
            {
                return userIaago.PreencheMensagem(mensagem);
            }
        }

        internal IaagoRetorno BuscaRetorno(IaagoVars userIaago)
        {
            IaagoRetorno ret = null;
            if (acaoResposta != null)
            {
                foreach (var acao in acaoResposta)
                {
                    acao.ExecutarAtribuicoesER(userIaago);
                    ret = acao.Retorno(userIaago);
                    if (ret != null)
                    {
                        ret.ExecutarAtribuicoesER(userIaago);
                        break;
                    }
                }
            }

            userIaago.IgnorarMensagemIntencao = ret?.ignorarMensagemIntencao;

            if (ret?.retorno == true && userIaago.RetornoAtribuir != null)
            {
                userIaago.Add(userIaago.RetornoAtribuir, "OK", false);
                userIaago.Intencao = ret?.retorno == true ? userIaago.RetornoIntencao : ret?.intencao;
                userIaago.RetornoIntencao = null;
            }
            else
            {
                userIaago.Intencao = ret?.intencao;
            }

            return ret;
        }
    }
}