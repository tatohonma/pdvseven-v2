namespace a7D.PDV.Integracao.Pagamento.NTKPos
{
    public interface ITerminalClient
    {
        void Loop();

        void Create(TerminalEvent onEvent, RegErro onErro);

        TerminalInformation Terminal { get; set; }
        bool Valid { get; }
    }
}
