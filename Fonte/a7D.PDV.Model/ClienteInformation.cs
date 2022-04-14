using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using a7D.Fmk.CRUD.DAL;
using a7D.PDV.EF;

namespace a7D.PDV.Model
{
    [CRUDClassDAL("tbCliente")]
    [Serializable]
    public class ClienteInformation : IERP
    {
        [CRUDParameterDAL(true, "IDCliente")]
        public Int32? IDCliente { get; set; }

        [CRUDParameterDAL(false, "CodigoERP")]
        public string CodigoERP { get; set; }

        [CRUDParameterDAL(false, "NomeCompleto")]
        public String NomeCompleto { get; set; }

        [CRUDParameterDAL(false, "Telefone1DDD")]
        public Int32? Telefone1DDD { get; set; }

        [CRUDParameterDAL(false, "Telefone1Numero")]
        public Int32? Telefone1Numero { get; set; }

        [CRUDParameterDAL(false, "Telefone2DDD")]
        public Int32? Telefone2DDD { get; set; }

        [CRUDParameterDAL(false, "Telefone2Numero")]
        public Int32? Telefone2Numero { get; set; }

        [CRUDParameterDAL(false, "Documento1")]
        public String Documento1 { get; set; }

        [CRUDParameterDAL(false, "RG")]
        public String RG { get; set; }

        [CRUDParameterDAL(false, "Limite")]
        public Decimal? Limite { get; set; }

        [CRUDParameterDAL(false, "Credito")]
        public Decimal? Credito { get; set; }

        [CRUDParameterDAL(false, "Bloqueado")]
        public Boolean? Bloqueado { get; set; }

        [CRUDParameterDAL(false, "Observacao")]
        public String Observacao { get; set; }

        [CRUDParameterDAL(false, "Endereco")]
        public String Endereco { get; set; }

        [CRUDParameterDAL(false, "EnderecoNumero")]
        public String EnderecoNumero { get; set; }

        [CRUDParameterDAL(false, "Complemento")]
        public String Complemento { get; set; }

        [CRUDParameterDAL(false, "Bairro")]
        public String Bairro { get; set; }

        [CRUDParameterDAL(false, "Cidade")]
        public String Cidade { get; set; }

        [CRUDParameterDAL(false, "IDEstado", "IDEstado")]
        public EstadoInformation Estado { get; set; }

        [CRUDParameterDAL(false, "CEP")]
        public Int32? CEP { get; set; }

        [CRUDParameterDAL(false, "EnderecoReferencia")]
        public String EnderecoReferencia { get; set; }

        [CRUDParameterDAL(false, "Sexo")]
        public String Sexo { get; set; }

        [CRUDParameterDAL(false, "DataNascimento")]
        public DateTime? DataNascimento { get; set; }

        [CRUDParameterDAL(false, "DtInclusao")]
        public DateTime? DtInclusao { get; set; }

        [CRUDParameterDAL(false, "Email")]
        public string Email { get; set; }

        [CRUDParameterDAL(false, "GUIDIdentificacao")]
        public string GUIDIdentificacao { get; set; }

        public String Telefone1
        {
            get
            {
                String telefone = "";
                if (Telefone1DDD != null)
                    telefone += "(" + Telefone1DDD.ToString() + ") ";

                if (Telefone1Numero != null)
                    telefone += Telefone1Numero.ToString();

                return telefone;
            }
        }

        public String EnderecoCompleto
            => $"{Endereco}, {EnderecoNumero} - {Complemento}\n{Bairro} - {Cidade}\n{Estado?.Sigla} CEP: {CEP}";

        public static ClienteInformation ConverterObjeto(Object obj)
        {
            return (ClienteInformation)obj;
        }

        public override string ToString() => $"{IDCliente}: {NomeCompleto}";
    }
}
