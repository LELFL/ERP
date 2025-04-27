using Dtos;
using Events;
using Repositories;

namespace Commands;

/// <summary>
/// 分配角色权限
/// </summary>
public class RolePermissionsCommandHandler : IRequestHandler<RolePermissionsCommand>
{
    private readonly IRoleRepository _repository;

    public RolePermissionsCommandHandler(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var permissionIds = request.PermissionIds.Split(",");
        await _repository.PermissionAsync(request.Id, permissionIds);
        var role = _repository.Local.FirstOrDefault(x => x.Id == request.Id);
        role?.AddDomainEvent(new RolePermissionsUpdatedEvent(role.Id));
    }

}
