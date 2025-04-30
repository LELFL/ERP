using Events;

namespace Entities;
public interface IEntity<TKey> : IEntity
{
    TKey Id { get; set; }
}

public interface IEntity
{
    IReadOnlyCollection<BaseEvent> DomainEvents { get; }

    void AddDomainEvent(BaseEvent domainEvent);

    void RemoveDomainEvent(BaseEvent domainEvent);

    void ClearDomainEvents();
}
