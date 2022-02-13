using a7D.PDV.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace a7D.PDV.EF
{
    public static class Repositorio
    {
        static pdv7Context pdvFast = null;

        public static List<TEntity> Listar<TEntity>(Expression<Func<TEntity, bool>> expression = null) where TEntity : class, new()
        {
            using (var pdv7 = new pdv7Context())
            {
                var dbSet = pdv7.DbSet<TEntity>();
                if (expression == null)
                    return dbSet.ToList();
                else
                    return dbSet.Where(expression).ToList();
            }
        }

        public static TEntity[] ListarConfig<TEntity>(Func<DbQuery<TEntity>, DbQuery<TEntity>> config, 
            Expression<Func<TEntity, bool>> expression) where TEntity : class, new()
        {
            if (pdvFast == null)
                pdvFast = new pdv7Context(true);

            lock (pdvFast)
            {
                var dbSet = config.Invoke(pdvFast.DbSet<TEntity>());
                var result = dbSet.Where(expression);
                //var sql = result.ToString();
                return result.ToArray();
            }
        }

        public static TEntity CarregarConfig<TEntity>(Func<DbQuery<TEntity>, DbQuery<TEntity>> config, 
            Expression<Func<TEntity, bool>> expression) where TEntity : class, new()
        {
            if (pdvFast == null)
                pdvFast = new pdv7Context(true);

            lock (pdvFast)
            {
                var dbSet = config.Invoke(pdvFast.DbSet<TEntity>());
                var result = dbSet.Where(expression);
                //var sql = result.ToString();
                return result.FirstOrDefault();
            }
        }

        public static TEntity[] ListarFast<TEntity>(Expression<Func<TEntity, bool>> expression = null) where TEntity : class, new()
        {
            //using (new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            //using (var pdv7 = new pdv7Context(true))
            if (pdvFast == null)
                pdvFast = new pdv7Context(true);

            lock (pdvFast)
            {
                //pdv7.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                var dbSet = pdvFast.DbSet<TEntity>().AsNoTracking();
                //var sql = dbSet.Sql;
                if (expression == null)
                    return dbSet.ToArray();
                else
                    return dbSet.Where(expression).ToArray();
            }
        }

        public static TEntity[] Query<TEntity>(string sql, params object[] parameters)
        {
            using (var pdv = new pdv7Context())
                return pdv.Database.SqlQuery<TEntity>(sql, parameters).ToArray();
        }

        public static decimal? Somar<TEntity>(Expression<Func<TEntity, decimal?>> selector, Expression<Func<TEntity, bool>> expression = null) where TEntity : class, new()
        {
            using (var pdv7 = new pdv7Context())
            {
                var dbSet = pdv7.DbSet<TEntity>();
                if (expression == null)
                    return dbSet.Sum(selector);
                else
                    return dbSet.Where(expression).Sum(selector);
            }
        }

        public static int Contar<TEntity>(Expression<Func<TEntity, bool>> expression = null) where TEntity : class, new()
        {
            using (var pdv7 = new pdv7Context())
            {
                var dbSet = pdv7.DbSet<TEntity>();
                if (expression == null)
                    return dbSet.Count();
                else
                    return dbSet.Where(expression).Count();
            }
        }

        public static TEntity Carregar<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, new()
        {
            using (var pdv7 = new pdv7Context())
            {
                var dbSet = pdv7.DbSet<TEntity>();
                return dbSet.Where(expression).FirstOrDefault();
            }
        }

        public static TEntity Carregar<TEntity, TKey>(Expression<Func<TEntity, bool>> expression,
                                                      Expression<Func<TEntity, TKey>> ordem,
                                                      bool desc = false) where TEntity : class, new()
        {
            using (var pdv7 = new pdv7Context())
            {
                var dbSet = pdv7.DbSet<TEntity>();
                if (desc)
                    return dbSet.Where(expression).OrderByDescending(ordem).FirstOrDefault();
                else
                    return dbSet.Where(expression).OrderBy(ordem).FirstOrDefault();
            }
        }

        public static void Inserir<TEntity>(TEntity dado) where TEntity : class, new()
        {

            using (var pdv7 = new pdv7Context())
            {
                pdv7.DbSet<TEntity>().Add(dado);
                pdv7.SaveChanges();
            }
        }

        public static void Atualizar<TEntity>(TEntity dado) where TEntity : class, new()
        {
            using (var pdv7 = new pdv7Context())
            {
                if (pdv7.Entry(dado).State == EntityState.Detached)
                    pdv7.DbSet<TEntity>().Attach(dado);

                pdv7.Entry(dado).State = EntityState.Modified;
                pdv7.SaveChanges();
            }
        }

        public static void AtualizarLista<TEntity>(IEnumerable<TEntity> dados) where TEntity : class, new()
        {
            using (var pdv7 = new pdv7Context())
            {
                var dbSet = pdv7.DbSet<TEntity>();
                foreach (var dado in dados)
                {
                    if (pdv7.Entry(dado).State == EntityState.Detached)
                        dbSet.Attach(dado);

                    pdv7.Entry(dado).State = EntityState.Modified;

                }
                pdv7.SaveChanges();
            }
        }

        public static void Excluir<TEntity>(TEntity dado) where TEntity : class, new()
        {
            using (var pdv7 = new pdv7Context())
            {
                if (pdv7.Entry(dado).State == EntityState.Detached)
                    pdv7.DbSet<TEntity>().Attach(dado);

                pdv7.Entry(dado).State = EntityState.Deleted;
                pdv7.SaveChanges();
            }
        }

        public static int Execute(string sql, params object[] parameters)
        {
            using (var pdv7 = new pdv7Context())
            {
                if (sql.Contains("BACKUP"))
                    pdv7.Database.CommandTimeout = 5 * 60;

                return pdv7.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, sql, parameters);
            }
        }

        public static T ExecuteScalar<T>(string sql, params object[] parameters)
        {
            var query = Query<T>(sql, parameters);
            if (query.Length > 0)
                return query[0];
            else
                return default(T);
        }
    }
}