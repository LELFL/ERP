using System.Linq.Expressions;
using ELF.Infrastructure.Data;
using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ELF.Data;
public class EFCoreRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, new()
{
    protected readonly ApplicationDbContext _db;

    public EFCoreRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public ICollection<TEntity> Local => _db.Set<TEntity>().Local.ToList();

    #region Add/AddRange

    public TEntity Add(object input)
    {
        var entity = new TEntity();
        _db.Set<TEntity>().Add(entity);
        _db.Entry(entity).CurrentValues.SetValues(input);
        return entity;
    }

    public TEntity Add(TEntity entity)
    {
        _db.Set<TEntity>().Add(entity);
        return entity;
    }

    public async Task<TEntity> AddAsync(object input)
    {
        var entity = new TEntity();
        await _db.Set<TEntity>().AddAsync(entity);
        _db.Entry(entity).CurrentValues.SetValues(input);
        return entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _db.Set<TEntity>().AddAsync(entity);
        return entity;
    } 

    public TEntity[] AddRange(params object[] inputs)
    {
        TEntity[] entities = new TEntity[inputs.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            entities[i] = Add(inputs[i]);
        }
        return entities;
    }

    public TEntity[] AddRange(params TEntity[] entities)
    {
        _db.Set<TEntity>().AddRange(entities);
        return entities;
    }

    public async Task<TEntity[]> AddRangeAsync(params object[] inputs)
    {
        TEntity[] entities = new TEntity[inputs.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            entities[i] = await AddAsync(inputs[i]);
        }
        return entities;
    }
    #endregion

    public async Task<bool> AnyAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate is null)
            return await query.AnyAsync(cancellationToken);
        else
            return await query.AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<TEntity> AsNoTracking()
    {
        return _db.Set<TEntity>().AsNoTracking();
    }

    public IQueryable<TEntity> AsQueryable()
    {
        return _db.Set<TEntity>().AsQueryable();
    }

    public async Task<int> CountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate is null)
            return await query.CountAsync(cancellationToken);
        else
            return await query.CountAsync(predicate, cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate is null)
            return await query.FirstOrDefaultAsync(cancellationToken);
        else
            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(_db.Set<TEntity>().AsQueryable(), predicate, cancellationToken);
    }

    public TEntity? Get(TKey id)
    {
        return _db.Set<TEntity>().Find(id);
    }

    public async Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await _db.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    public TEntity Remove(TKey id)
    {
        var entity = Get(id);
        if (entity is not null)
            _db.Set<TEntity>().Remove(entity);
        else
            throw new InvalidOperationException($"Entity with id {id} not found");

        return entity;
    }

    public TEntity Remove(TEntity entity)
    {
        _db.Set<TEntity>().Remove(entity);
        return entity;
    }

    public async Task<TEntity> RemoveAsync(TKey id)
    {
        var entity = await GetAsync(id);
        if (entity is not null)
            _db.Set<TEntity>().Remove(entity);
        else
            throw new InvalidOperationException($"Entity with id {id} not found");

        return entity;
    }

    public TEntity[] RemoveRange(params TEntity[] entities)
    {
        _db.Set<TEntity>().RemoveRange(entities);
        return entities;
    }

    public int SaveChanges()
    {
        return _db.SaveChanges();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate is null)
            return await query.SingleOrDefaultAsync(cancellationToken);
        else
            return await query.SingleOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return await SingleOrDefaultAsync(_db.Set<TEntity>().AsQueryable(), predicate, cancellationToken);
    }

    public async Task<List<T>> ToListAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        if (predicate is null)
            return await query.ToListAsync(cancellationToken);
        else
            return await query.Where(predicate).ToListAsync(cancellationToken);
    }

    public TEntity Update(TKey id, object input)
    {
        var entity = Get(id);
        if (entity is null) throw new InvalidOperationException($"Entity with id {id} not found");
        _db.Entry(entity).CurrentValues.SetValues(input);
        return entity;
    }

    public TEntity Update(TEntity entity)
    {
        _db.Set<TEntity>().Update(entity);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TKey id, object input)
    {
        var entity = await GetAsync(id);
        if (entity is null) throw new InvalidOperationException($"Entity with id {id} not found");
        _db.Entry(entity).CurrentValues.SetValues(input);

        return entity;
    }

    public TEntity[] UpdateRange(params TEntity[] entities)
    {
        _db.Set<TEntity>().UpdateRange(entities);
        return entities;
    }
}
