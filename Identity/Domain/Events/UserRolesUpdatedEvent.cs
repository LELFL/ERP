namespace Events;

public class UserRolesUpdatedEvent : BaseEvent
{
    public UserRolesUpdatedEvent(long userId)
    {
        UserId = userId;
    }

    public long UserId { get; }
}
