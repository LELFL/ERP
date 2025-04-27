using Events;
using Interfaces;
using Repositories;

namespace EventHandlers;

internal class RolePermissionsUpdatedEventEventHandler : INotificationHandler<RolePermissionsUpdatedEvent>
{
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _repository;

    public RolePermissionsUpdatedEventEventHandler(IIdentityService identityService, IUserRepository repository)
    {
        this._identityService = identityService;
        this._repository = repository;
    }
    public async Task Handle(RolePermissionsUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var query = _repository.AsNoTracking().Where(x => x.Roles.Any(y => y.Id == notification.RoleId)).Select(x => x.Id);
        var userIds = await _repository.ToListAsync(query);
        await _identityService.ClearCacheAsync([.. userIds]);
    }
}
