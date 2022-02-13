using System;

namespace a7D.PDV.Integracao.Servico.Core
{
    public delegate void OnMensagemListener(string info);
    public delegate void OnProgressListener(string msg, int total, int corrente, IntegracaoTask integracao);
    public delegate string onAddLogException(Exception ex, bool saveLog);

}
