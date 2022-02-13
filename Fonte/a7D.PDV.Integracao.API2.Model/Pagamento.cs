using System;
using System.Runtime.Serialization;

namespace a7D.PDV.Integracao.API2.Model
{

    public partial class Pagamento 
    {
        public Pagamento(int? id = default(int?),
            TipoPagamento tipoPagamento = default(TipoPagamento), 
            decimal? valor = default(decimal?), 
            string referenciaPagamento = default(string),
            string autorizacao = default(string),
            string contaRecebivel = default(string),
            string bandeira = default(string),
            int? metodo = default(int?)
            )
        {
            this.ID = id;
            this.TipoPagamento = tipoPagamento;
            this.Valor = valor;
            this.ReferenciaPagamento = referenciaPagamento;
            this.Autorizacao = autorizacao;
            this.ContaRecebivel = contaRecebivel;
            this.Bandeira = bandeira;
            this.IDMetodo = metodo;
        }


        [DataMember(Name = "ID", EmitDefaultValue = false)]
        public int? ID { get; set; }

        [DataMember(Name = "TipoPagamento", EmitDefaultValue = false)]
        public TipoPagamento TipoPagamento { get; set; }

        [DataMember(Name = "Valor", EmitDefaultValue = false)]
        public decimal? Valor { get; set; }

        [DataMember(Name = "ReferenciaPagamento", EmitDefaultValue = false)]
        public string ReferenciaPagamento { get; set; }

        [DataMember(Name = "Autorizacao", EmitDefaultValue = false)]
        public string Autorizacao { get; set; }

        [DataMember(Name = "IDMetodo", EmitDefaultValue = false)]
        public int? IDMetodo { get; set; }

        [DataMember(Name = "ContaRecebivel", EmitDefaultValue = false)]
        public string ContaRecebivel { get; set; }

        [DataMember(Name = "Bandeira", EmitDefaultValue = false)]
        public string Bandeira { get; set; }

 



    }

}
