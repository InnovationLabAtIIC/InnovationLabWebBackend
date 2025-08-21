using System.Linq.Expressions;
using InnovationLab.Shared.Interfaces;
using InnovationLab.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace InnovationLab.Shared.Repositories;

public class Repository<T>(DbContext dbContext) : IRepository<T> where T : BaseModel
{
    private readonly DbContext _dbContext = dbContext;
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() != 0;
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        var count = predicate is null
            ? await _dbSet.CountAsync()
            : await _dbSet.CountAsync(predicate);
        return count;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var result = await _dbSet.FindAsync(id);
        return result;
    }

    public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids, int skip, int take)
    {
        var result = await _dbSet.AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return result;
    }

    public async Task<IEnumerable<T>> GetAsync(int skip, int take)
    {
        var results = await _dbSet.AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return results;
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, int skip, int take)
    {
        var results = await _dbSet.AsNoTracking()
            .Where(predicate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return results;
    }

    public async Task<IEnumerable<T>> QueryAsync(Func<IQueryable<T>, IQueryable<T>> query, int skip, int take)
    {
        var results = await query(_dbSet.AsNoTracking())
            .Skip(skip)
            .Take(take)
            .ToListAsync();
        return results;
    }

    public async Task BatchAddAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void SoftDelete(T entity)
    {
        entity.DeletedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void HardDelete(T entity)
    {
        _dbSet.Remove(entity);
    }
}