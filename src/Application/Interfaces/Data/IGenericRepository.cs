using System.Linq.Expressions;

namespace Application.Interfaces.Data;

public interface IGenericRepository<T> where T : class
{
    Task<IList<T>> GetAllAsync(
        Expression<Func<T, bool>> expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        List<string> includes = null
    );

    Task<T> GetOneByQueryAsync(Expression<Func<T, bool>> expression, List<string> includes = null);

    Task InsertAsync(T entity);

    Task InsertRangeAsync(IEnumerable<T> entities);

    Task SaveChanges();

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);

    void Update(T entity);
}