# Domain Events Documentation

This document lists all domain events (MediatR) in the system and their handlers.

## Events

### TodoCompletedEvent

**Namespace:** `Contracts`

**Properties:**

- `Id`: Guid
- `Title`: String

**Handlers:**

- `Todos.Application.Features.Todo.Events.TodoCompletedEventHandler`

**Event Flow:**

```mermaid
graph LR
    Event[TodoCompletedEvent]
    Event --> TodoCompletedEventHandler[TodoCompletedEventHandler]
```

---

### TodoCreatedEvent

**Namespace:** `Contracts`

**Properties:**

- `TodoId`: Guid

**Handlers:**

- `Todos.Application.Features.Todo.Events.TodoCreatedEventHandler`

**Event Flow:**

```mermaid
graph LR
    Event[TodoCreatedEvent]
    Event --> TodoCreatedEventHandler[TodoCreatedEventHandler]
```

---

