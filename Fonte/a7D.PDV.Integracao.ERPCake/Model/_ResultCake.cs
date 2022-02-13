using a7D.PDV.Integracao.Servico.Core;
using Newtonsoft.Json;

namespace a7D.PDV.Integracao.ERPCake.Model
{
    public class ResultCake<T> : apiError where T : ModelCake
    {
        public string registry;

        public T Result
            => registry == null ? 
                null : JsonConvert.DeserializeObject<T>(registry);
    }
}
