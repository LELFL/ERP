using Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities;

public abstract class BaseEntity<TKey> : IEntity<TKey>
{
    // 这可以轻松修改为 `BaseEntity<T>` 并将 `public T Id` 用于支持不同的键类型。
    // 为了简单起见，使用非泛型整数类型。
    public TKey Id { get; set; } = default!;

    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
