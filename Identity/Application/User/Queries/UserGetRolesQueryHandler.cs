using Dtos;
using Repositories;

namespace Queries;

/// <summary>
/// ��ȡ��ɫȨ��
/// </summary>
public class UserGetRolesQueryHandler : IRequestHandler<UserGetRolesQuery, string>
{

    private readonly IUserRepository _repository;

    public UserGetRolesQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(UserGetRolesQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking().Where(x => x.Id == request.Id).SelectMany(x => x.Roles).Select(x => x.Id);
        var permissionIds = await _repository.ToListAsync(query);
        return string.Join(",", permissionIds);
    }

}
