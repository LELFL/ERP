using Dtos;
using ELF.Common.Expand;
using ELF.Common.Models;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class ProductSKUGetListQueryHandler : IRequestHandler<ProductSKUGetListQuery, AmisPagedOutput<ProductSKUGetListOutput>>
{
    private readonly IRepository<ProductSKU, long> _repository;

    public ProductSKUGetListQueryHandler(IRepository<ProductSKU, long> repository)
    {
        _repository = repository;
    }

    public async Task<AmisPagedOutput<ProductSKUGetListOutput>> Handle(ProductSKUGetListQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking();
        query = Filter(query);

        var totalCount = await _repository.CountAsync(query);

        var entities = new List<ProductSKU>();

        if (totalCount > 0)
        {
            request.OrderBy = request.OrderBy ?? nameof(request.Name);
            query = query.AmisPaging(request);

            entities = await _repository.ToListAsync(query);
        }

        return new AmisPagedOutput<ProductSKUGetListOutput>(
            totalCount,
            entities.Adapt<List<ProductSKUGetListOutput>>()
        );
    }

    private IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        return query;
    }
}
