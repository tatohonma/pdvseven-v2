using System;
using System.Collections.Generic;

namespace a7D.PDV.EF.Models
{
    public class tbCliente : IERP, IERPSync
    {
        public tbCliente()
        {
            this.tbPedidos = new List<tbPedido>();
        }

        public int IDCliente { get; set; }
        public string CodigoERP { get; set; }
        public string NomeCompleto { get; set; }
        public int? Telefone1DDD { get; set; }
        public int? Telefone1Numero { get; set; }
        public int? Telefone2DDD { get; set; }
        public int? Telefone2Numero { get; set; }
        public string Documento1 { get; set; } // CPF
        public int? IDDocumento2Tipo { get; set; } // Tipo Documento 2 (Passaporte)
        public string Documento2 { get; set; } // Passaporte
        public string IDiFood { get; set; }
        public string RG { get; set; }
        public decimal? Limite { get; set; }
        public decimal? Credito { get; set; }
        public bool Bloqueado { get; set; }
        public string Observacao { get; set; }
        public string Endereco { get; set; }
        public string EnderecoNumero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public int? IDEstado { get; set; }
        public int? CEP { get; set; }
        public string EnderecoReferencia { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Sexo { get; set; }
        public DateTime DtInclusao { get; set; }
        public DateTime? DtAlteracao { get; set; }
        public string Email { get; set; }

        public virtual tbEstado tbEstado { get; set; }
        public virtual ICollection<tbPedido> tbPedidos { get; set; }
        public virtual ICollection<tbSaldo> Saldos { get; set; }

        public bool RequerAlteracaoERP(DateTime dtSync) => DtInclusao > dtSync;

        public int myID() => IDCliente;

        public String EnderecoCompleto
            => $"{Endereco}, {EnderecoNumero} {Complemento}\n{Bairro} - {Cidade}\nCEP: {CEP}";


        public override string ToString() => $"{IDCliente}: {NomeCompleto}";
    }
}
