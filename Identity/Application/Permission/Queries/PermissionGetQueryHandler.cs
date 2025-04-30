using Dtos;
using Entities;
using Interfaces;
using Mapster;

namespace Queries;

public class PermissionGetQueryHandler : IRequestHandler<PermissionQuery, PermissionDto>
{
    private readonly IRepository<Permission, long> _repository;

    public PermissionGetQueryHandler(IRepository<Permission, long> repository)
    {
        _repository = repository;
    }

    public async Task<PermissionDto> Handle(PermissionQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetAsync(request.Id);
        return entity.Adapt<PermissionDto>();
    }
}