using System.Data.Common;
using System.Linq.Expressions;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories
{
    public class GenericRepository<T, TContext> : IGenericRepository<T> where T : class where TContext : DbContext
    {
        public readonly TContext Context;

        public GenericRepository(TContext context)
        {
            Context = context;
        }

        public async Task Add(T entity, CancellationToken cancellationToken)
        {
            await Context.AddAsync(entity, cancellationToken);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression);
        }

        public IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            await Context.SaveChangesAsync(cancellationToken);
        }

        public void Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public IQueryable<T> ExecuteRawSql(string query, IEnumerable<DbParameter>? parameters = null)
        {
            if (parameters != null && parameters.Any())
            {
                return Context.Set<T>().FromSqlRaw(query, parameters);
            }

            return Context.Set<T>().FromSqlRaw(query);
        }

        public async Task<int> ExecuteRawSqlAsync(string query, CancellationToken cancellationToken, IEnumerable<DbParameter>? parameters = null)
        {
            if (parameters != null && parameters.Any())
            {
                return await Context.Database.ExecuteSqlRawAsync(query, parameters, cancellationToken);
            }

            return await Context.Database.ExecuteSqlRawAsync(query, cancellationToken);
        }
    }
}
