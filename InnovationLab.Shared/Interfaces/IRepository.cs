using System.Linq.Expressions;
using InnovationLab.Shared.Models;

namespace InnovationLab.Shared.Interfaces;

public interface IRepository<T> where T : BaseModel
{
    Task<bool> SaveChangesAsync();
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids, int skip, int take);
    Task<IEnumerable<T>> GetAsync(int skip, int take);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, int skip, int take);
    Task<IEnumerable<T>> QueryAsync(Func<IQueryable<T>, IQueryable<T>> query, int skip, int take);
    Task BatchAddAsync(IEnumerable<T> entities);
    Task AddAsync(T entity);
    void Update(T entity);
    void SoftDelete(T entity);
    void HardDelete(T entity);
}