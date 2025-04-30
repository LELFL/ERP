using Dtos;
using ELF.Common.Expand;
using ELF.Common.Models;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class SpecValueGetListQueryHandler : IRequestHandler<SpecValueGetListQuery, AmisPagedOutput<SpecValueGetListOutput>>
{
    private readonly IRepository<SpecValue, long> _repository;

    public SpecValueGetListQueryHandler(IRepository<SpecValue, long> repository)
    {
        _repository = repository;
    }

    public async Task<AmisPagedOutput<SpecValueGetListOutput>> Handle(SpecValueGetListQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking();
        query = Filter(query);

        var totalCount = await _repository.CountAsync(query);

        var entities = new List<SpecValue>();

        if (totalCount > 0)
        {
            request.OrderBy = request.OrderBy ?? nameof(request.Name);
            query = query.AmisPaging(request);

            entities = await _repository.ToListAsync(query);
        }

        return new AmisPagedOutput<SpecValueGetListOutput>(
            totalCount,
            entities.Adapt<List<SpecValueGetListOutput>>()
        );
    }

    private IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        return query;
    }
}
