using BuildingBlocks.DomainEvents;

namespace Contracts;

public record TodoCompletedEvent(Guid Id, string Title) : IDomainEvent;
