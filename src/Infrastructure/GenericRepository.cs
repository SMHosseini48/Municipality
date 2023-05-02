using System.Linq.Expressions;
using Application.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ResidueContext _context;
    private readonly DbSet<T> _db;

    public GenericRepository(ResidueContext context )
    {
        _context = context;
        _db = context.Set<T>();
    }

    public void Delete(T entity)
    {
        _db.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _db.RemoveRange(entities);
    }

    public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null )
    {
        IQueryable<T> query = _db;
        if (expression != null) query = query.Where(expression);

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        if (orderBy != null) query = orderBy(query);

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<T> GetOneByQueryAsync(Expression<Func<T, bool>> expression, List<string> includes = null)
    {
        IQueryable<T> query = _db;
        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);
        return await query.AsNoTracking().FirstOrDefaultAsync(expression);
    }

    public async Task InsertAsync(T entity)
    {
        await _db.AddAsync(entity);
    }

    public async Task InsertRangeAsync(IEnumerable<T> entities)
    {
        await _db.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        _db.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}