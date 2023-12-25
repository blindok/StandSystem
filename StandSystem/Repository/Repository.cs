using Microsoft.EntityFrameworkCore;
using StandSystem.DataAccess;
using StandSystem.Repository.Interfaces;
using System.Linq.Expressions;

namespace StandSystem.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _db;
    internal readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }

    public IAsyncEnumerable<T> GetAll(string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet;

        foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query.Include(property);
        }

        return query.AsAsyncEnumerable();
    }

    public T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
        IQueryable<T> query = _dbSet.Where(filter);

        foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query.Include(property);
        }

        return query.FirstOrDefault();
    }

    public void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}
