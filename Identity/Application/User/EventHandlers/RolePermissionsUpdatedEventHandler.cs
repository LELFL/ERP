using Events;
using Interfaces;

namespace EventHandlers;

internal class UserRolesUpdatedEventEventHandler : INotificationHandler<UserRolesUpdatedEvent>
{
    private readonly IIdentityService _identityService;

    public UserRolesUpdatedEventEventHandler(IIdentityService identityService)
    {
        this._identityService = identityService;
    }
    public async Task Handle(UserRolesUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _identityService.ClearCacheAsync(notification.UserId);
    }
}
