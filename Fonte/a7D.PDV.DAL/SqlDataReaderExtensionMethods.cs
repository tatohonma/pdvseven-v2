using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace a7D.PDV.DAL
{
    public static class SqlDataReaderExtensionMethods
    {
        public static T Def<T>(this SqlDataReader r, int ord)
        {
            var t = r.GetSqlValue(ord);
            if (t == DBNull.Value) return default(T);
            return ((INullable)t).IsNull ? default(T) : r.GetFieldValue<T>(ord);
        }

        public static T Def<T>(this SqlDataReader r, string columnName)
        {
            return Def<T>(r, r.GetOrdinal(columnName));
        }

        public static T? Val<T>(this SqlDataReader r, int ord) where T : struct
        {
            var t = r.GetSqlValue(ord);

            if (t == DBNull.Value) return default(T?);
            var nT = t as INullable;

            return nT?.IsNull == true ? default(T?) : r.GetFieldValue<T>(ord);
        }

        public static T? Val<T>(this SqlDataReader r, string columnName) where T : struct
        {
            return Val<T>(r, r.GetOrdinal(columnName));
        }

        public static T Ref<T>(this SqlDataReader r, int ord) where T : class
        {
            var t = r.GetSqlValue(ord);
            if (t == DBNull.Value) return null;
            return ((INullable)t).IsNull ? null : r.GetFieldValue<T>(ord);
        }
        public static T Ref<T>(this SqlDataReader r, string columnName) where T : class
        {
            return Ref<T>(r, r.GetOrdinal(columnName));
        }
    }
}
