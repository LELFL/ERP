using Dtos;
using Repositories;

namespace Commands;

/// <summary>
/// �����ɫȨ��
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
    }

}
