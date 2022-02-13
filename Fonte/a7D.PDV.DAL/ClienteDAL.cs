using a7D.PDV.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace a7D.PDV.DAL
{
    public static class ClienteDAL
    {
        public static List<ClienteInformation> ListarResumido(string nomeCompleto = null, int? telefone1Numero = null, string documento1 = null, string email = null)
        {
            List<ClienteInformation> list = new List<ClienteInformation>();
            ClienteInformation obj;

            SqlDataAdapter da;
            DataTable dt = new DataTable();

            string querySql = @"
                SELECT 
                     IDCliente
                    ,NomeCompleto
                    ,Telefone1Numero
                    ,Documento1 
                    ,Email
                FROM 
                    tbCliente (NOLOCK)
                WHERE
				        (@nome IS NULL OR NomeCompleto LIKE @nome)
                    AND (@documento IS NULL OR Documento1 LIKE @documento)
                    AND (@telefone IS NULL OR CAST(Telefone1Numero AS VARCHAR(10)) LIKE @telefone)
                    AND (@email IS NULL OR Email LIKE @email)
                ORDER BY 
                    NomeCompleto
            ";

            da = new SqlDataAdapter(querySql, DB.ConnectionString);

            da.SelectCommand.Parameters.Add("@nome", SqlDbType.NVarChar)
                .Value = nomeCompleto == null ? DBNull.Value : (object)("%" + nomeCompleto + "%");

            da.SelectCommand.Parameters.Add("@telefone", SqlDbType.NVarChar)
                .Value = telefone1Numero == null ? DBNull.Value : (object)(telefone1Numero.ToString() + "%");

            da.SelectCommand.Parameters.Add("@documento", SqlDbType.NVarChar)
                .Value = documento1 == null ? DBNull.Value : (object)(documento1 + "%");

            da.SelectCommand.Parameters.Add("@email", SqlDbType.NVarChar)
                .Value = email == null ? DBNull.Value : (object)("%" + email + "%");

            // Se não enviar 'DBNull.Value' o parametro não é criado!
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                obj = new ClienteInformation
                {
                    IDCliente = dr.Field<int>("IDCliente"),
                    NomeCompleto = dr.Field<string>("NomeCompleto"),
                    Telefone1Numero = dr.IsNull("Telefone1Numero") ? default(int?) : dr.Field<int>("Telefone1Numero"),
                    Documento1 = dr.Field<string>("Documento1"),
                    Email = dr.Field<string>("Email")
                };
                list.Add(obj);
            }

            return list;
        }

        public static List<ClienteInformation> BuscarCliente(string nome, string email, string documento, string telefone, int pagina, int qtd)
        {
            var query = @"select
	tbCliente.*,
    tbEstado.Nome as EstadoNome,
    tbEstado.Sigla
from tbCliente (nolock)
left join tbEstado (nolock) on tbEstado.IDEstado = tbCliente.IDEstado
where
	(@nome is null OR NomeCompleto like @nome) AND
	(@email is null OR Email like @email) AND
	(@documento is null OR Documento1 like @documento) AND
	(@telefone is null OR CAST(Telefone1Numero as varchar(10)) like @telefone)";

            using (var conn = new SqlConnection(DB.ConnectionString))
            {
                conn.Open();

                if (telefone != null && telefone.Length > 9)
                    telefone = telefone.Substring(telefone.Length - 9, 9);
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@nome", nome.ValorOuNulo());
                    cmd.Parameters.AddWithValue("@email", email.ValorOuNulo());
                    cmd.Parameters.AddWithValue("@documento", documento.ValorOuNulo());
                    cmd.Parameters.AddWithValue("@telefone", telefone.ValorOuNulo());

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return Enumerable.Empty<ClienteInformation>().ToList();

                        var list = new List<ClienteInformation>();
                        while (reader.Read())
                        {
                            list.Add(reader.ObterCliente());
                        }
                        return list;
                    }
                }
            }
        }

        private static ClienteInformation ObterCliente(this SqlDataReader r)
        {
            var cliente = new ClienteInformation
            {
                IDCliente = r.Def<int>("IDCliente"),
                NomeCompleto = r.Ref<string>("NomeCompleto"),
                Telefone1DDD = r.Val<int>("Telefone1DDD"),
                Telefone1Numero = r.Val<int>("Telefone1Numero"),
                Documento1 = r.Ref<string>("Documento1"),
                Observacao = r.Ref<string>("Observacao"),
                Endereco = r.Ref<string>("Endereco"),
                EnderecoNumero = r.Ref<string>("EnderecoNumero"),
                Complemento = r.Ref<string>("Complemento"),
                Bairro = r.Ref<string>("Bairro"),
                Cidade = r.Ref<string>("Cidade"),
                CEP = r.Val<int>("CEP"),
                EnderecoReferencia = r.Ref<string>("EnderecoReferencia"),
                DataNascimento = r.Val<DateTime>("DataNascimento"),
                DtInclusao = r.Val<DateTime>("DtInclusao"),
                Sexo = r.Ref<string>("Sexo"),
                Email = r.Ref<string>("Email")
            };

            if (r.Val<int>("IDEstado").HasValue)
            {
                cliente.Estado = new EstadoInformation
                {
                    IDEstado = r.Val<int>("IDEstado"),
                    Nome = r.Ref<string>("EstadoNome"),
                    Sigla = r.Ref<string>("Sigla")
                };
            }

            return cliente;
        }

        private static object ValorOuNulo(this string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                return DBNull.Value;
            return $"%{val}%";
        }
    }
}