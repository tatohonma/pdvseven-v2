namespace a7D.PDV.BLL.ValueObject
{
    public class SaldoClientes
    {
        public int IDCliente { get; set; }
        public string NomeCompleto { get; set; }
        public string Telefone { get; set; }
        public string Documento { get; set; }
        public decimal Saldo { get; set; }

        public override string ToString() => $"{IDCliente}: {NomeCompleto} => {Saldo}";
    }
}
