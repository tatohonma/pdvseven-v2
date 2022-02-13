namespace a7D.PDV.BLL.ValueObject
{
    public class InformacoesRemetente
    {
        public string Remetente { get; set; }
        public string Host { get; set; }
        public int Porta { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool EnableSsl { get; set; }
    }
}
