using a7D.PDV.Ativacao.API.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace a7D.PDV.Ativacao.API.Repository
{
    public abstract class BaseRepository<T> : IDisposable where T : class
    {

        private AtivacaoContext _context;
        protected DbSet<T> _set;

        public BaseRepository(AtivacaoContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public async Task<T> BuscarPorId(int id)
        {
            return await _set.FindAsync(id);
        }

        public async Task<int> SalvarMudancas()
        {
            return await _context.SaveChangesAsync();
        }

        protected T UnProxy(T proxyObject)
        {
            return _context.UnProxy(proxyObject);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}