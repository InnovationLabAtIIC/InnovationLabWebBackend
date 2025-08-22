using System.Linq.Expressions;
using InnovationLab.Shared.Interfaces;
using InnovationLab.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace InnovationLab.Shared.Repositories;

public class Repository<TDbContext, TModel>(TDbContext dbContext) : IRepository<TDbContext, TModel>
    where TDbContext : DbContext
    where TModel : BaseModel
{
    private readonly TDbContext _dbContext = dbContext;
    private readonly DbSet<TModel> _dbSet = dbContext.Set<TModel>();

    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() != 0;
    }

    public async Task<int> CountAsync(Expression<Func<TModel, bool>>? predicate = null)
    {
        var count = predicate is null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(predicate);
        return count;
    }

    public async Task<TModel?> GetByIdAsync(Guid id)
    {
        var result = await _dbSet.FindAsync(id);
        return result;
    }

    public async Task<IEnumerable<TModel>> GetByIdsAsync(IEnumerable<Guid> ids, int skip, int take)
    {
        var result = await _dbSet.AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return result;
    }

    public async Task<IEnumerable<TModel>> GetAsync(int skip, int take)
    {
        var results = await _dbSet.AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return results;
    }

    public async Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate, int skip, int take)
    {
        var results = await _dbSet.AsNoTracking()
            .Where(predicate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return results;
    }

    public async Task<IEnumerable<TModel>> QueryAsync(Func<IQueryable<TModel>, IQueryable<TModel>> query, int skip, int take)
    {
        var results = await query(_dbSet.AsNoTracking())
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return results;
    }

    public async Task BatchAddAsync(IEnumerable<TModel> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task AddAsync(TModel entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(TModel entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void SoftDelete(TModel entity)
    {
        entity.DeletedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void HardDelete(TModel entity)
    {
        _dbSet.Remove(entity);
    }
}