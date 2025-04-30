using Dtos;
using ELF.Common.Expand;
using ELF.Common.Models;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class UnitGetListQueryHandler : IRequestHandler<UnitGetListQuery, AmisPagedOutput<UnitGetListOutput>>
{
    private readonly IRepository<Unit, long> _repository;

    public UnitGetListQueryHandler(IRepository<Unit, long> repository)
    {
        _repository = repository;
    }

    public async Task<AmisPagedOutput<UnitGetListOutput>> Handle(UnitGetListQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking();
        query = Filter(query);

        var totalCount = await _repository.CountAsync(query);

        var entities = new List<Unit>();

        if (totalCount > 0)
        {
            request.OrderBy = request.OrderBy ?? nameof(request.Name);
            query = query.AmisPaging(request);

            entities = await _repository.ToListAsync(query);
        }

        return new AmisPagedOutput<UnitGetListOutput>(
            totalCount,
            entities.Adapt<List<UnitGetListOutput>>()
        );
    }

    private IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        return query;
    }
}
