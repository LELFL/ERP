using Dtos;
using Repositories;

namespace Queries;

/// <summary>
/// 获取用户权限列表
/// </summary>
public class UserGetPermissionsQueryHandler : IRequestHandler<UserGetPermissionsQuery, string[]>
{

    private readonly IUserRepository _repository;

    public UserGetPermissionsQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<string[]> Handle(UserGetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking().Where(x => x.Id == request.Id).SelectMany(x => x.Roles).SelectMany(x => x.Permissions).Select(x => x.Name).Distinct();
        var permissions = await _repository.ToArrayAsync(query);
        return permissions;
    }

}
