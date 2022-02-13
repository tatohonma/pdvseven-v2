namespace a7D.PDV.Model.DTO
{
    public struct InformacoesCancelamento
    {
        public int IDUsuario { get; set; }
        public int IDMotivoCancelamento { get; set; }
        public string Observacoes { get; set; }
        public bool RetornarAoEstoque { get; set; }
    }
}
