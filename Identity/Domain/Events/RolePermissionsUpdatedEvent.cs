namespace Events;

public class RolePermissionsUpdatedEvent : BaseEvent
{
    public RolePermissionsUpdatedEvent(long roleId)
    {
        RoleId = roleId;
    }

    public long RoleId  { get; }
}
