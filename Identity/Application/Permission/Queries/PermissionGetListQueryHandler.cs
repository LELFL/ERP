using System.Linq.Dynamic.Core;
using Dtos;
using ELF.Common.Models;
using ELF.Interfaces.Permissions;
using Mapster;

namespace Queries;

public class PermissionGetListQueryHandler : IRequestHandler<PermissionGetListQuery, AmisPagedOutput<PermissionGetListOutput>>
{
    private readonly IPermissionRepository _repository;

    public PermissionGetListQueryHandler(IPermissionRepository repository)
    {
        _repository = repository;
    }

    public async Task<AmisPagedOutput<PermissionGetListOutput>> Handle(PermissionGetListQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetTreeListAsync();
        entities = entities.Where(x => x.ParentId == null).ToList();
        return new AmisPagedOutput<PermissionGetListOutput>(
            entities.Count,
            entities.Adapt<List<PermissionGetListOutput>>()
        );
    }

    private IQueryable<TEntity> Filter<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        return query;
    }
}
