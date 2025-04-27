using Dtos;
using ELF.Common.Expand;
using ELF.Common.Models;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class RoleGetListQueryHandler : IRequestHandler<RoleGetListQuery, AmisPagedOutput<RoleGetListOutput>>
{
    private readonly IRepository<Role, long> _repository;

    public RoleGetListQueryHandler(IRepository<Role, long> repository)
    {
        _repository = repository;
    }

    public async Task<AmisPagedOutput<RoleGetListOutput>> Handle(RoleGetListQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking();
        query = Filter(query);

        var totalCount = await _repository.CountAsync(query);

        var entities = new List<Role>();

        if (totalCount > 0)
        {
            request.OrderBy = request.OrderBy ?? nameof(request.Name);
            query = query.AmisPaging(request);

            entities = await _repository.ToListAsync(query);
        }

        return new AmisPagedOutput<RoleGetListOutput>(
            totalCount,
            entities.Adapt<List<RoleGetListOutput>>()
        );
    }

    private IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        return query;
    }
}
