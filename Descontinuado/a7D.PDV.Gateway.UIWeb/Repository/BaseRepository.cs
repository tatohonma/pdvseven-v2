using a7D.PDV.Gateway.UIWeb.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace a7D.PDV.Gateway.UIWeb.Repository
{
    public class BaseRepository<T> : IDisposable where T : class
    {
        protected GatewayContext _db;
        private DbSet<T> dbset;

        public BaseRepository(GatewayContext _db)
        {
            this._db = _db;
            dbset = _db.Set<T>();
        }

        public T ObterPorId(params object[] id)
        {
            return dbset.Find(id);
        }

        public IEnumerable<T> Listar()
        {
            return dbset.AsEnumerable();
        }

        public IQueryable<T> Buscar(Expression<Func<T, bool>> predicado)
        {
            return dbset.Where(predicado);
        }

        public T Adicionar(T obj)
        {
            return dbset.Add(obj);
        }

        public IEnumerable<T> AdicionarVarios(IEnumerable<T> objs)
        {
            return dbset.AddRange(objs);
        }

        public void Deletar(T obj)
        {
            dbset.Remove(obj);
        }

        public void DeletarVarios(IEnumerable<T> objs)
        {
            dbset.RemoveRange(objs);
        }

        public void SalvarAlteracoes()
        {
            _db.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _db.Dispose();
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