using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Model;

namespace a7D.PDV.Ativacao.API.Repository
{
    public class TipoPDVRepository : BaseRepository<PdvType>
    {
        public static List<PdvType> Lista;

        public TipoPDVRepository(ApplicationDbContext context) : base(context)
        { }

        public static void PreencheLista(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var pdv = new TipoPDVRepository(db);
            Lista = pdv.Set.ToList();
        }
    }
}