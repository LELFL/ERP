using Dtos;
using ELF.Common.Expand;
using ELF.Common.Models;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class ProductGetListQueryHandler : IRequestHandler<ProductGetListQuery, AmisPagedOutput<ProductGetListOutput>>
{
    private readonly IRepository<Product, long> _repository;

    public ProductGetListQueryHandler(IRepository<Product, long> repository)
    {
        _repository = repository;
    }

    public async Task<AmisPagedOutput<ProductGetListOutput>> Handle(ProductGetListQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking();
        query = Filter(query);

        var totalCount = await _repository.CountAsync(query);

        var entities = new List<Product>();

        if (totalCount > 0)
        {
            request.OrderBy = request.OrderBy ?? nameof(request.Name);
            query = query.AmisPaging(request);

            entities = await _repository.ToListAsync(query);
        }

        return new AmisPagedOutput<ProductGetListOutput>(
            totalCount,
            entities.Adapt<List<ProductGetListOutput>>()
        );
    }

    private IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        return query;
    }
}
