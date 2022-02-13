using Newtonsoft.Json;
using System;
using System.Globalization;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    public abstract class ModelCake
    {
        public int? id; // Identificador do registro (nulo para inserção)

        protected DateTime? GetDate(string data)
            => data == null ? null : (DateTime?)DateTime.ParseExact(data, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

        protected string SetDate(DateTime? data)
            => data?.ToString("yyyy-MM-dd");

        public virtual bool RequerAlteracaoPDV(DateTime dtSync, out int id)
        {
            id = 0;
            return false;
        }

        public virtual void SetCode(string code)
        {
        }
    }
}