namespace a7D.PDV.Integracao.Servico.Core
{
    public interface IIntegracaoTask
    {
        //OnMensagemListener EventoMensagem { get; set; }
        OnProgressListener EventoProgress { get; set; }
        int Revalidar { get; }
        bool Executando { get; }
        bool Disponivel { get; }
        void Executar();
        void Parar();
    }
}