using System;

namespace a7D.PDV.Integracao.ERPSige.Model
{
    public class Pedido
    {
        public int Codigo;
        public string OrigemVenda;
        public string Deposito;
        public string StatusSistema;
        public string Status;
        public string Categoria;
        public DateTime Validade;
        public string Empresa;
        public string Cliente;
        public string Vendedor;
        public bool LancarComissaoVendedor;
        public string PlanoDeConta;
        public string FormaPagamento;
        public int NumeroParcelas;
        public string Transportadora;
        public DateTime DataEnvio;
        public bool Enviado;
        public decimal ValorFrete;
        public bool FreteContaEmitente;
        public decimal ValorSeguro;
        public string Descricao;
        public decimal OutrasDespesas;
        public decimal ValorFinal;
        public bool Finalizado;
        public bool Lancado;
        public string NumeroNFe;
        public string Municipio;
        public string CodigoMunicipio;
        public string Pais;
        public string CEP;
        public string UF;
        public string UFCodigo;
        public string Bairro;
        public string Logradouro;
        public string LogradouroNumero;
        public string LogradouroComplemento;
        public string Banco;
        public DateTime Data;
        public DateTime DataFaturamento;

        public PedidoItem[] Items;
        public PedidoPagamento[] Pagamentos;

        public override string ToString()
            => $"{Codigo} {DataFaturamento.ToString("dd/MM/yyyy HH:mm")} R$ {ValorFinal.ToString("N2")}";
    }
}