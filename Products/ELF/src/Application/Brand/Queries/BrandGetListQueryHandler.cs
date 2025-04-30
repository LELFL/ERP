using Dtos;
using ELF.Common.Expand;
using ELF.Common.Models;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class BrandGetListQueryHandler : IRequestHandler<BrandGetListQuery, AmisPagedOutput<BrandGetListOutput>>
{
    private readonly IRepository<Brand, long> _repository;

    public BrandGetListQueryHandler(IRepository<Brand, long> repository)
    {
        _repository = repository;
    }

    public async Task<AmisPagedOutput<BrandGetListOutput>> Handle(BrandGetListQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking();
        query = Filter(query);

        var totalCount = await _repository.CountAsync(query);

        var entities = new List<Brand>();

        if (totalCount > 0)
        {
            request.OrderBy = request.OrderBy ?? nameof(request.Name);
            query = query.AmisPaging(request);

            entities = await _repository.ToListAsync(query);
        }

        return new AmisPagedOutput<BrandGetListOutput>(
            totalCount,
            entities.Adapt<List<BrandGetListOutput>>()
        );
    }

    private IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        return query;
    }
}
