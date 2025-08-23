using System.Linq.Expressions;
using InnovationLab.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace InnovationLab.Shared.Interfaces;

public interface IRepository<TDbContext, TModel>
    where TDbContext : DbContext
    where TModel : BaseModel
{
    Task<bool> SaveChangesAsync();
    Task<int> CountAsync(Expression<Func<TModel, bool>>? predicate = null);
    Task<TModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<TModel>> GetByIdsAsync(IEnumerable<Guid> ids, int skip, int take);
    Task<IEnumerable<TModel>> GetAsync(int skip, int take);
    Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, int skip, int take);
    Task<IEnumerable<TModel>> QueryAsync(Func<IQueryable<TModel>, IQueryable<TModel>> query, int skip, int take);
    Task BatchAddAsync(IEnumerable<TModel> entities);
    Task AddAsync(TModel entity);
    void Update(TModel entity);
    void SoftDelete(TModel entity);
    void HardDelete(TModel entity);
}