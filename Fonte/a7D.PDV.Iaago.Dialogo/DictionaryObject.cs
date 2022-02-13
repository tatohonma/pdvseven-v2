using System.Collections.Generic;

namespace a7D.PDV.Iaago.Dialogo
{
    public class DictionaryObject : Dictionary<string, object>
    {
        public override string ToString() => $"Itens: {Count}";

        public object GetValueByKey(string key)
        {
            return ContainsKey(key) ? this[key] : null;
        }
    }
}
