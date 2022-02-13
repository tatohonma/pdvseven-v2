using a7D.PDV.Integracao.Servico.Core;
using Newtonsoft.Json;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    public class ListCake<T> : apiError where T : ModelCake
    {
        public string list;

        public T[] Itens
            => JsonConvert.DeserializeObject<T[]>(list);
    }
}
