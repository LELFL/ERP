using Dtos;
using Repositories;

namespace Queries;

/// <summary>
/// ��ȡ��ɫȨ��
/// </summary>
public class RoleGetPermissionsQueryHandler : IRequestHandler<RoleGetPermissionsQuery, string>
{

    private readonly IRoleRepository _repository;

    public RoleGetPermissionsQueryHandler(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> Handle(RoleGetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking().Where(x => x.Id == request.Id).SelectMany(x => x.Permissions).Select(x => x.Id);
        var permissionIds = await _repository.ToListAsync(query);
        return string.Join(",", permissionIds);
    }

}
