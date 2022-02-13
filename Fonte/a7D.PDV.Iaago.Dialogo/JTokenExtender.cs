using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace a7D.PDV.Iaago.Dialogo
{
    public static class JTokenExtender
    {
        public static DictionaryObject FromJTokenToDictionary(this JToken result, bool verifyEmpty = false)
        {
            var dic = new DictionaryObject();

            if (result is JObject jObj)
            {
                foreach (var item in jObj)
                {
                    if (item.Value.Type == JTokenType.Object)
                    {
                        dic.Add(item.Key.ToLower(), item.Value.FromJTokenToDictionary());
                    }
                    else if (item.Value.Type == JTokenType.Array)
                    {
                        dic.Add(item.Key.ToLower(), item.Value.FromJTokenToDictionary()?["itens"]);
                    }
                    else
                    {
                        var value = (JValue)item.Value;
                        dic.Add(item.Key.ToLower(), value.Value);
                    }
                }
                if (verifyEmpty && dic.Values.FirstOrDefault(v => v != null) == null)
                {
                    dic = null;
                }
            }
            else if (result is JArray jArray)
            {
                var itens = new List<object>();
                foreach (var item in jArray)
                {
                    if (item.Type == JTokenType.Object
                     || item.Type == JTokenType.Array)
                    {
                        itens.Add(item.FromJTokenToDictionary());
                    }
                    else
                    {
                        var value = (JValue)item;
                        itens.Add(value.Value);
                    }
                }

                if (verifyEmpty && itens.Count == 0)
                {
                    return null;
                }
                else if (verifyEmpty && itens.Count == 1
                      && itens[0] is DictionaryObject dic2
                      && dic2.Values.FirstOrDefault(v => v != null) == null)
                {
                    return null;
                }

                dic.Add("itens", itens);
            }
            return dic;
        }
    }
}
