using Dtos;
using Repositories;

namespace Commands;

/// <summary>
/// 处理用户的分配角色权限命令。
/// </summary>
public class UserRolesCommandHandler : IRequestHandler<UserRolesCommand>
{
    private readonly IUserRepository _repository;

    public UserRolesCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UserRolesCommand request, CancellationToken cancellationToken)
    {
        var permissionIds = request.RoleIds.Split(",");
        await _repository.RoleAsync(request.Id, permissionIds);
    }

}
