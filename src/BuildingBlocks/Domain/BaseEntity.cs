using System.Text.Json.Serialization;
using BuildingBlocks.DomainEvents;

namespace BuildingBlocks.Domain;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.CreateVersion7();

    private readonly List<IDomainEvent> _domainEvents = [];

    [JsonIgnore]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void Raise(IDomainEvent eventItem) => _domainEvents.Add(eventItem);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
