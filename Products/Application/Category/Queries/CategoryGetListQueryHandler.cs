using Dtos;
using ELF.Common.Expand;
using ELF.Common.Models;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class CategoryGetListQueryHandler : IRequestHandler<CategoryGetListQuery, AmisPagedOutput<CategoryGetListOutput>>
{
    private readonly IRepository<Category, long> _repository;

    public CategoryGetListQueryHandler(IRepository<Category, long> repository)
    {
        _repository = repository;
    }

    public async Task<AmisPagedOutput<CategoryGetListOutput>> Handle(CategoryGetListQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking();
        query = Filter(query);

        var totalCount = await _repository.CountAsync(query);

        var entities = new List<Category>();

        if (totalCount > 0)
        {
            request.OrderBy = request.OrderBy ?? nameof(request.Name);
            query = query.AmisPaging(request);

            entities = await _repository.ToListAsync(query);
        }

        return new AmisPagedOutput<CategoryGetListOutput>(
            totalCount,
            entities.Adapt<List<CategoryGetListOutput>>()
        );
    }

    private IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        return query;
    }
}
