using Dtos;
using Events;
using Repositories;

namespace Commands;

/// <summary>
/// �����û��ķ����ɫȨ�����
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
        var user = _repository.Local.FirstOrDefault(x => x.Id == request.Id);
        user?.AddDomainEvent(new UserRolesUpdatedEvent(user.Id));
    }

}
