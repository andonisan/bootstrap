using BuildingBlocks.DomainEvents;

namespace Contracts;

public sealed record TodoCreatedEvent(Guid TodoId) : IDomainEvent;
