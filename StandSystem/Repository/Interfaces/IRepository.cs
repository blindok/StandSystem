using System.Linq.Expressions;

namespace StandSystem.Repository.Interfaces;

public interface IRepository<T> where T : class
{
    IAsyncEnumerable<T> GetAll(string? includeProperties = null);
    T? GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}
