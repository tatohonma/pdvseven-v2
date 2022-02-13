using System;
using System.Linq;
using a7D.PDV.Integracao.ERPCake.Model;
using a7D.PDV.Integracao.Servico.Core;

namespace a7D.PDV.Integracao.ERPCake
{
    public class APIERPCake : APIJson
    {
        public APIERPCake(string token) : base("https://app.cakeerp.com/api/")
        {
            client.DefaultRequestHeaders.Add("X-cake-token", token);
            jsonWriter.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        }

        public T[] All<T>(int offset = 0, int limit = 100, string where = null) where T : ModelCake
            => Get<ListCake<T>>($"{typeof(T).Name.ToLower()}/all?offset={offset}&limit={limit}&{where}")
                .Itens;

        public T Save<T>(T obj) where T : ModelCake
            => Send<ResultCake<T>>($"{typeof(T).Name.ToLower()}", obj)
                .Result;

        public T GetByID<T>(int id) where T : ModelCake, new()
            => Get<ResultCake<T>>($"{typeof(T).Name.ToLower()}?id={id}").Result;

        public T GetFirst<T>(string where) where T : ModelCake, new()
            => Get<ListCake<T>>($"{typeof(T).Name.ToLower()}/all?{where}&limit=1")
                .Itens.FirstOrDefault();

        public void Delete<T>(T obj) where T : ModelCake, new()
            => Send<ResultCake<T>>($"{typeof(T).Name.ToLower()}?id={obj.id}", new T() { id = obj.id.Value }, "DELETE");

        // http://app.cakeerp.com/api_docs/outros_servicos.html
        public Sales_Order BillOrder(int id)
            => Send<ResultCake<Sales_Order>>("bill_order", new { sales_order_id = id }, "POST")
                .Result;
    }
}