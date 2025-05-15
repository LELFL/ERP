using System.Linq.Expressions;

namespace Interfaces;

/// <summary>
/// 通用仓储接口
/// </summary>
public interface IRepository<TEntity, TKey> where TEntity : class, new()
{
    ICollection<TEntity> Local { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    #region Add/AddRange
    TEntity Add(object input);
    TEntity Add(TEntity entity);
    Task<TEntity> AddAsync(object input);
    Task<TEntity> AddAsync(TEntity entity);

    TEntity[] AddRange(params object[] inputs);
    TEntity[] AddRange(params TEntity[] entities);
    Task<TEntity[]> AddRangeAsync(params object[] inputs);
    #endregion

    #region Update/UpdateRange
    TEntity Update(TKey id, object entity);
    TEntity Update(TEntity entity);
    Task<TEntity> UpdateAsync(TKey id, object entity);
    TEntity[] UpdateRange(params TEntity[] entities);
    #endregion

    #region Remove/RemoveRange
    /// <summary>
    /// 根据主键移除实体
    /// </summary>
    /// <param name="id"> 要移除的主键 </param>
    TEntity Remove(TKey id);
    /// <summary>
    /// 根据主键移除实体
    /// </summary>
    /// <param name="id"> 要移除的主键 </param>
    Task<TEntity> RemoveAsync(TKey id);
    /// <summary>
    /// 移除实体
    /// </summary>
    /// <param name="entity"> 要移除的实体 </param>
    TEntity Remove(TEntity entity);

    /// <summary>
    /// 移除多个实体
    /// </summary>
    /// <param name="entities"> 要移除的实体集合 </param>
    TEntity[] RemoveRange(params TEntity[] entities);
    #endregion

    #region Query

    #region Get
    TEntity? Get(TKey id);
    Task<TEntity?> GetAsync(TKey id, CancellationToken cancellationToken = default);
    #endregion

    IQueryable<TEntity> AsQueryable();
    IQueryable<TEntity> AsNoTracking();

    Task<List<T>> ToListAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    Task<T[]> ToArrayAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<int> CountAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<bool> AnyAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
    #endregion
}
