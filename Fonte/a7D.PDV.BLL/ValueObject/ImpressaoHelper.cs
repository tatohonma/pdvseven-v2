using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace a7D.PDV.BLL.ValueObject
{
    public class ImpressaoHelper
    {
        public StringBuilder plain = new StringBuilder();

        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string ClienteNome { get; set; }
        public string EnderecoCompleto { get; set; }
        public string EnderecoReferencia { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string CEP { get; set; }
        public string Observacoes { get; set; }
        public string ObservacaoCupom { get; set; }
        public string Telefone { get; set; }
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string NumeroSAT { get; set; }
        public string DataEmissao { get; set; }
        public string ChaveConsulta { get; set; }
        public string NumeroDocumento { get; set; }
        public string Entregador { get; set; }
        public string CodigoDeBarrasString { get; set; }
        public List<Bitmap> CodigoBarras { get; set; }
        public Bitmap QRCODE { get; set; }
        public string CPFCNPJCliente { get; set; }
        public string[] Produtos { get; set; }
        public AgrupadoProdutosValores[] ProdutosAgrupados { get; set; }
        public ItemValor[] ProdutosValores { get; set; }
        public string ValorTotal { get; set; }
        public string ValorDesconto { get; set; }
        public string SaldoCliente { get; set; }
        public string ContaCliente { get; set; }
        public string ValorTotalAPagar { get; set; }
        public string ValorTroco { get; set; }
        public string ImpostosTotais { get; set; }
        public string[] Pagamentos { get; set; }
        public ItemValor[] PagamentosValores { get; set; }
        public string Identificacao { get; set; }
        public bool Viagem { get; set; }
        public string ValorAcrescimo { get; set; }
        public string IdPedido { get; set; }
        public int NumeroPessoas { get; set; }
        public string ValorEntrega { get; set; }
    }

    public class AgrupadoProdutosValores
    {
        public string Titulo { get; set; }
        public ItemValor[] ProdutosValores { get; set; }
    }
    
}
