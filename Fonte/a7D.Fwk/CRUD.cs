using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace a7D.Fmk.CRUD.DAL
{
    public static class CRUD
    {
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    return ConfigurationManager.ConnectionStrings["connectionString"]?.ConnectionString;
                }
                return ConfigurationManager.ConnectionStrings[_connectionString]?.ConnectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public static List<object> Listar(object obj)
        {
            var _type = obj.GetType();

            var list = new List<Object>();

            var querySql = QueryCarregar(obj);

            using (var da = new SqlDataAdapter(querySql, ConnectionString))
            {

                foreach (var _info in _type.GetProperties())
                {
                    var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);
                    if (attParameter == null)
                        continue;

                    var coluna = _info.ObterColuna();

                    object valor = null;

                    if (attParameter.Fk == null)
                    {
                        valor = _info.GetValue(obj, null);
                    }
                    else
                    {
                        //Se o objeto for null, nao recuperar o valor
                        if (_info.GetValue(obj, null) != null)
                        {
                            valor = _info.GetValue(obj, null).GetType().GetProperty(attParameter.Fk).GetValue(_info.GetValue(obj, null), null);
                        }
                    }

                    da.SelectCommand.Parameters.AddWithValue($"@{coluna}", (valor ?? DBNull.Value));
                }
                using (var ds = new DataSet())
                {
                    da.Fill(ds);
                    var dt = ds.Tables[0];

                    foreach (DataRow dr in dt.Rows)
                    {
                        var registro = _type.InvokeMember(_type.Name, BindingFlags.CreateInstance, null, null, null);

                        foreach (var _info in _type.GetProperties())
                        {
                            var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);
                            if (attParameter == null)
                                continue;

                            var coluna = _info.ObterColuna();

                            if (dr[coluna] == DBNull.Value)
                            {
                                //SetValue null
                                _info.SetValue(registro, null, null);
                            }
                            else
                            {
                                if (attParameter.Fk == null)
                                {
                                    //SetValue
                                    _info.SetValue(registro, dr[coluna], null);
                                }
                                else
                                {
                                    //Chama construtor
                                    _info.SetValue(registro, _info.PropertyType.InvokeMember(_info.PropertyType.Name, BindingFlags.CreateInstance, null, null, null), null);

                                    //SetValue
                                    _info.GetValue(registro, null).GetType().GetProperty(attParameter.Fk).SetValue(_info.GetValue(registro, null), dr[coluna], null);
                                }
                            }
                        }

                        list.Add(registro);
                    }
                }
            }
            return list;
        }

        public static List<T> ListarGenerico<T>(T obj) where T : class, new()
        {
            return Enumerar(obj).ToList();
        }

        public static object Carregar(object obj)
        {
            var _type = obj.GetType();

            var querySql = QueryCarregar(obj);

            using (var da = new SqlDataAdapter(querySql, ConnectionString))
            {

                foreach (var _info in _type.GetProperties())
                {
                    var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);
                    var coluna = _info.ObterColuna();

                    object valor = null;

                    if (attParameter.Fk == null)
                    {
                        valor = _info.GetValue(obj, null);
                    }
                    else
                    {
                        //Se o objeto for null, nao recuperar o valor
                        if (_info.GetValue(obj, null) != null)
                        {
                            valor = _info.GetValue(obj, null).GetType().GetProperty(attParameter.Fk).GetValue(_info.GetValue(obj, null), null);
                        }
                    }

                    da.SelectCommand.Parameters.AddWithValue($"@{coluna}", (valor ?? DBNull.Value));
                }
                using (var ds = new DataSet())
                {
                    da.Fill(ds);
                    var dt = ds.Tables[0];

                    if (dt.Rows.Count == 1)
                    {
                        var dr = dt.Rows[0];

                        foreach (var _info in _type.GetProperties())
                        {
                            var coluna = _info.ObterColuna();
                            var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);

                            if (dr[coluna] == DBNull.Value)
                            {
                                //SetValue null
                                _info.SetValue(obj, null, null);
                            }
                            else
                            {
                                if (attParameter.Fk == null)
                                {
                                    //SetValue
                                    _info.SetValue(obj, dr[coluna], null);
                                }
                                else
                                {
                                    //Chama construtor
                                    _info.SetValue(obj, _info.PropertyType.InvokeMember(_info.PropertyType.Name, BindingFlags.CreateInstance, null, null, null), null);

                                    //SetValue
                                    _info.GetValue(obj, null).GetType().GetProperty(attParameter.Fk).SetValue(_info.GetValue(obj, null), dr[coluna], null);
                                }
                            }
                        }
                    }
                    else if (dt.Rows.Count > 1)
                    {
                        throw new Exception("O metodo \"Carregar\" não pode retornar mais de 1 resultado");
                    }
                }
            }

            return obj;
        }

        public static T Carregar<T>(T obj) where T : class, new()
        {
            var lista = Enumerar(obj);

            if (lista.Count() == 0)
            {
                return default(T);
            }

            if (lista.Count() == 1)
            {
                return lista.First();
            }

            throw new Exception("O metodo \"Carregar\" não pode retornar mais de 1 resultado");
        }

        public static T Carregar<T>(int id) where T : class, new()
        {
            var obj = new T();

            var type = typeof(T);
            PropertyInfo propPk = ObterPK(type);

            if (propPk != null)
            {
                propPk.SetValue(obj, id);
                return Carregar(obj);
            }
            return null;
        }

        private static PropertyInfo ObterPK(Type type)
        {
            var props = type.GetProperties();

            var propPk = props.FirstOrDefault(p =>
            {
                var attr = p.GetCustomAttribute<CRUDParameterDALAttribute>(false);
                return attr?.Pk == true;
            });
            return propPk;
        }

        private static string QueryCarregar(object obj)
        {
            var query = new StringBuilder();
            var queryCondicao = new StringBuilder();

            var _type = obj.GetType();

            foreach (var _info in _type.GetProperties())
            {
                var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);
                if (attParameter == null)
                    continue;
                var coluna = _info.ObterColuna();

                queryCondicao.Append($"AND ({coluna}=@{coluna} OR @{coluna} IS NULL) ");
            }
            query.AppendLine("SELECT * FROM ");
            query.AppendLine(_type.ObterTabela());
            query.AppendLine("WHERE");
            query.AppendLine(queryCondicao.ToString().Substring(3));

            return query.ToString();
        }

        public static void Salvar(object obj)
        {
            var _type = obj.GetType();
            var propPk = ObterPK(_type);

            if (propPk == null)
                return;

            if (propPk.GetValue(obj, null) == null)
                Adicionar(obj);
            else
                Alterar(obj);
        }

        public static void Adicionar(object obj)
        {
            var _type = obj.GetType();

            using (var cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();

                var querySql = QueryAdicionar(obj);

                using (var cmd = cnn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = querySql;

                    foreach (var _info in _type.GetProperties())
                    {
                        var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);
                        if (attParameter == null)
                            continue;
                        var coluna = _info.ObterColuna();

                        if (attParameter.Pk != true)
                        {
                            if (attParameter.Fk == null)
                            {
                                cmd.Parameters.AddWithValue($"@{coluna}", _info.GetValue(obj, null));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue($"@{coluna}",
                                    _info.PropertyType.GetProperty(attParameter.Fk).GetValue(_info.GetValue(obj, null), null)
                                );
                            }
                        }
                    }

                    var pk = Convert.ToInt32(cmd.ExecuteScalar());

                    var propPk = ObterPK(_type);

                    propPk?.SetValue(obj, pk);
                }
            }
        }
        private static string QueryAdicionar(object obj)
        {
            var query = new StringBuilder();
            var queryColunas = new StringBuilder();
            var queryValores = new StringBuilder();

            var _type = obj.GetType();

            var attClass = _type.GetCustomAttribute<CRUDClassDALAttribute>(false);

            foreach (var _info in _type.GetProperties())
            {
                var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);
                if (attParameter == null)
                    continue;
                var coluna = _info.ObterColuna();

                if (attParameter.Pk != true)
                {
                    queryColunas.Append(", ").Append(coluna);
                    queryValores.Append(", @").Append(coluna);
                }
            }

            query.AppendLine("INSERT INTO");
            query.AppendLine(_type.ObterTabela());
            query.AppendLine($"({queryColunas.ToString().Substring(1)})");
            query.AppendLine(" VALUES ");
            query.AppendLine($"({queryValores.ToString().Substring(1)})");

            return query.ToString();
        }

        public static void Alterar(object obj)
        {
            var _type = obj.GetType();

            using (var cnn = new SqlConnection(ConnectionString))
            {

                cnn.Open();

                var querySql = QueryAlterar(obj);

                using (var cmd = new SqlCommand(querySql, cnn))
                {
                    cmd.CommandType = CommandType.Text;

                    foreach (var _info in _type.GetProperties())
                    {
                        var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);
                        if (attParameter == null)
                            continue;
                        var coluna = _info.ObterColuna();

                        if (attParameter.Fk == null)
                        {
                            cmd.Parameters.AddWithValue($"@{coluna}", _info.GetValue(obj, null));
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue($"@{coluna}",
                                _info.GetValue(obj, null).GetType()
                                .GetProperty(attParameter.Fk).GetValue(_info.GetValue(obj, null), null)
                            );
                        }
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string QueryAlterar(object obj)
        {
            var query = new StringBuilder();
            var queryColunas = new StringBuilder();
            var queryCondicao = new StringBuilder();

            var _type = obj.GetType();

            var attClass = _type.GetCustomAttribute<CRUDClassDALAttribute>(false);

            foreach (var _info in _type.GetProperties())
            {
                var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);

                if (attParameter == null)
                    continue;

                var coluna = _info.ObterColuna();

                if (attParameter.Pk == false)
                {
                    queryColunas.Append($", {coluna}=@{coluna}");
                }
                else
                {
                    queryCondicao.Append($"{coluna}=@{coluna}");
                }
            }

            query.AppendLine("UPDATE");
            query.AppendLine(_type.ObterTabela());
            query.AppendLine("SET");
            query.AppendLine(queryColunas.ToString().Substring(1));
            query.AppendLine("WHERE ");
            query.AppendLine(queryCondicao.ToString());

            return query.ToString();
        }

        public static void Excluir(object obj)
        {
            var _type = obj.GetType();

            using (var cnn = new SqlConnection(ConnectionString))
            {
                cnn.Open();

                var ListaExcluir = Listar(obj);

                var querySql = QueryExcluir(obj);

                foreach (var item in ListaExcluir)
                {
                    using (var cmd = new SqlCommand(querySql, cnn))
                    {
                        cmd.CommandType = CommandType.Text;

                        var propPk = ObterPK(_type);

                        if (propPk != null)
                        {
                            var coluna = propPk.ObterColuna();

                            cmd.Parameters.Add(new SqlParameter($"@{coluna}", propPk.GetValue(item, null)));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static string QueryExcluir(object obj)
        {
            var query = new StringBuilder();

            var _type = obj.GetType();

            var attClass = _type.GetCustomAttribute<CRUDClassDALAttribute>(false);

            var propPk = ObterPK(_type);
            var coluna = propPk.ObterColuna();

            var queryCondicao = $"{coluna}=@{coluna}";

            query.AppendLine("DELETE FROM");
            query.AppendLine(attClass.Tabela);
            query.AppendLine("WHERE ");
            query.AppendLine(queryCondicao);

            return query.ToString();
        }

        private static string ObterTabela(this Type type)
        {
            var attr = type.GetCustomAttribute<CRUDClassDALAttribute>(false);
            if (attr == null)
                throw new Exception($"Classe {type.GetTypeInfo().Name} não tem o atributo {nameof(CRUDClassDALAttribute)}");
            var nome = type.GetTypeInfo().Name;
            if (nome.EndsWith("Information"))
                nome = nome.Substring(0, nome.LastIndexOf("Information"));
            return string.IsNullOrWhiteSpace(attr.Tabela) ? attr.Tabela : $"tb{nome}";
        }

        private static string ObterColuna(this MemberInfo info)
        {
            var attParameter = info.GetCustomAttribute<CRUDParameterDALAttribute>(false);
            if (attParameter == null) return null;
            return string.IsNullOrWhiteSpace(attParameter.Coluna) ? info.Name : attParameter.Coluna;
        }

        public static IEnumerable<T> Enumerar<T>(T filtro) where T : class, new()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    var type = typeof(T);
                    cmd.CommandText = QueryCarregar(filtro);
                    foreach (var _info in type.GetProperties())
                    {
                        var coluna = _info.ObterColuna();

                        if (string.IsNullOrWhiteSpace(coluna))
                            continue;

                        var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);
                        if (attParameter == null)
                            continue;

                        object valor = null;

                        if (attParameter.Fk == null)
                        {
                            valor = _info.GetValue(filtro, null);
                        }
                        else
                        {
                            //Se o objeto for null, nao recuperar o valor
                            if (_info.GetValue(filtro, null) != null)
                            {
                                valor = _info.GetValue(filtro, null).GetType().GetProperty(attParameter.Fk).GetValue(_info.GetValue(filtro, null), null);
                            }
                        }
                        cmd.Parameters.AddWithValue("@" + coluna, (valor ?? DBNull.Value));
                    }
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T registro = new T();

                            foreach (var _info in type.GetProperties())
                            {
                                var coluna = _info.ObterColuna();
                                var attParameter = _info.GetCustomAttribute<CRUDParameterDALAttribute>(false);

                                if (attParameter == null)
                                    continue;

                                var valor = reader.GetValue(reader.GetOrdinal(coluna));

                                if (valor == null || valor == DBNull.Value)
                                {
                                    //SetValue null
                                    _info.SetValue(registro, null);
                                }
                                else
                                {
                                    if (attParameter.Fk == null)
                                    {
                                        //SetValue
                                        _info.SetValue(registro, valor, null);
                                    }
                                    else
                                    {
                                        //Chama construtor
                                        _info.SetValue(registro, _info.PropertyType.InvokeMember(_info.PropertyType.Name, BindingFlags.CreateInstance, null, null, null), null);

                                        //SetValue
                                        _info.GetValue(registro, null).GetType().GetProperty(attParameter.Fk).SetValue(_info.GetValue(registro, null), valor, null);
                                    }
                                }
                            }
                            yield return registro;
                        }
                    }
                }
            }
        }
    }
}
