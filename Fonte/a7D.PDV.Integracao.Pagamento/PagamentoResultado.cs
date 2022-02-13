namespace a7D.PDV.Integracao.Pagamento
{
    public class PagamentoResultado
    {
        public string Autorizacao { get; internal set; }
        public bool Confirmado { get; internal set; }
        public string Bandeira { get; internal set; }
        public string ContaRecebivel { get; internal set; }
        public string ViaEstabelecimento { get; internal set; }
        public bool Debito { get; internal set; }
        public string ViaCliente { get; internal set; }
        public string LocRef { get; internal set; }
        public string ExtRef { get; internal set; }
        public string VirtMerch { get; internal set; }
        public string MensagemOperador { get; internal set; }

        public override string ToString()
        {
            return $"{(Debito ? "DEBITO" : "CREDITO")}: {Bandeira} {ContaRecebivel} {Autorizacao}\r\n{ViaCliente}\r\n{ViaEstabelecimento}";
        }
    }
}