using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Integracao.ERPSige.Model
{
    public class PedidoPagamento
    {
        public string FormaPagamento;
        public string Banco;
        public string DescricaoPagamento;
        public decimal ValorPagamento;
        public string BandeiraCartao;
        public string NumeroTerminal;
        public DateTime DataTransacao;
        public string CredenciadoraCartao;
        public string CredenciadoraCNPJ;
        public string CV_NSU;
        public int CondicaoPagamento;
        public int Parcelas;
        public int PeriodoParcelas;
        public decimal Adiantamento;
        public bool Quitar;

        public override string ToString()
            => $"{FormaPagamento} / {DescricaoPagamento} / {CredenciadoraCartao} / {BandeiraCartao} R$ {ValorPagamento.ToString("N2")}";
    }
}