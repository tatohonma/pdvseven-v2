using a7D.PDV.Ativacao.API.Data;
using Microsoft.EntityFrameworkCore;

namespace a7D.PDV.Ativacao.API.Repository
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected DbSet<T> Set => _context.Set<T>();

        protected BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T?> GetAsync(int id, CancellationToken ct = default)
            => await Set.FindAsync(new object?[] { id }, ct);

        public virtual async Task<int> StoreAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);

   
        protected IQueryable<T> QueryNoTracking() => Set.AsNoTracking();
    }
}
