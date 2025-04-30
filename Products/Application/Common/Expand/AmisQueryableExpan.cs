using ELF.Common.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ELF.Common.Expand;
internal static class AmisQueryableExpan
{
    public static IQueryable<TEntity> AmisPaging<TEntity>(this IQueryable<TEntity> query, object input)
    {
        //Try to sort query if available
        if (input is AmisGetListInputBase amisInput)
        {
            if (!string.IsNullOrWhiteSpace(amisInput.Sorting))
            {
                query = query.OrderBy(amisInput.Sorting!);
            }

            if (amisInput.Skip > 0)
                query = (IQueryable<TEntity>)query.Skip(amisInput.Skip.Value);
            if (amisInput.Take > 0)
                query = (IQueryable<TEntity>)query.Take(amisInput.Take.Value);
        }

        //No sorting
        return query;
    }

    public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> query, bool condition, Expression<Func<TEntity, bool>> predicate)
    {
        if (condition)
        {
            return query.Where(predicate);
        }

        return query;
    }

    /// <summary>
    /// WhereNotNull
    /// </summary>
    public static IQueryable<TEntity> WhereNotNull<TEntity>(this IQueryable<TEntity> query, [NotNull] object? condition, Expression<Func<TEntity, bool>> predicate)
    {
        if (condition is not null)
        {
            return query.Where(predicate);
        }

#pragma warning disable CS8777 // 退出时，参数必须具有非 null 值。
        return query;
#pragma warning restore CS8777 // 退出时，参数必须具有非 null 值。
    }
}
