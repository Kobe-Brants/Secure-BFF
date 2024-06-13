using System.Data.Common;
using System.Linq.Expressions;

namespace Core.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        IQueryable<T> Find(Expression<Func<T, bool>> expression);

        Task Add(T entity, CancellationToken cancellationToken);

        void Update(T entity);

        void Delete(T entity);

        Task Save(CancellationToken cancellationToken);

        IQueryable<T> ExecuteRawSql(string query, IEnumerable<DbParameter>? parameters = null);

        Task<int> ExecuteRawSqlAsync(string query, CancellationToken cancellationToken, IEnumerable<DbParameter>? parameters = null);
    }
}
