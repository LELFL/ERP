using Dtos;
using ELF.Common.Expand;
using ELF.Common.Models;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class SpecGetListQueryHandler : IRequestHandler<SpecGetListQuery, AmisPagedOutput<SpecGetListOutput>>
{
    private readonly IRepository<Spec, long> _repository;

    public SpecGetListQueryHandler(IRepository<Spec, long> repository)
    {
        _repository = repository;
    }

    public async Task<AmisPagedOutput<SpecGetListOutput>> Handle(SpecGetListQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking();
        query = Filter(query);

        var totalCount = await _repository.CountAsync(query);

        var entities = new List<Spec>();

        if (totalCount > 0)
        {
            request.OrderBy = request.OrderBy ?? nameof(request.Name);
            query = query.AmisPaging(request);

            entities = await _repository.ToListAsync(query);
        }

        return new AmisPagedOutput<SpecGetListOutput>(
            totalCount,
            entities.Adapt<List<SpecGetListOutput>>()
        );
    }

    private IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        return query;
    }
}
