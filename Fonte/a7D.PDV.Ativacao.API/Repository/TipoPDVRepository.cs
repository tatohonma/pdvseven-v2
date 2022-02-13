using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace a7D.PDV.Ativacao.API.Repository
{
    public class TipoPDVRepository : BaseRepository<TipoPDV>
    {
        public static List<TipoPDV> Lista;

        public TipoPDVRepository(AtivacaoContext context) : base(context)
        {
        }

        public static void PreencheLista()
        {
            using (var db = new AtivacaoContext())
            {
                var pdv = new TipoPDVRepository(db);
                Lista = pdv._set.ToList();
            }
        }
    }
}