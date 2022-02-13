using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.ERPSige.Model
{
    public class PedidoItem
    {
        public string Codigo;
        public string Unidade;
        public string Descricao;
        public decimal Quantidade;
        public decimal ValorUnitario;
        public decimal ValorFrete;
        public decimal DescontoUnitario;
        public decimal ValorTotal;
        public decimal PesoKG;
        public decimal Comprimento;
        public decimal Altura;
        public decimal Largura;
        public bool FreteGratis;
        public decimal ValorUnitarioFrete;
        public int PrazoEntregaFrete;
        public decimal ComissaoVendedor;
        //public ProdutoComposicao[] ;
        //public string Atributos;

        public override string ToString()
            => $"{Codigo} ({Quantidade}x{Unidade}) {Descricao} R$ {ValorTotal}";
    }

    //public class ProdutoComposicao
    //{
    //    public string Produto;
    //    public string Deposito;
    //    public decimal Consumo;
    //    public decimal ConsumoTotal;
    //    public string NumerosSerie;
    //}
}