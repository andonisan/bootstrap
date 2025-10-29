# Integration Events Documentation

This document lists all integration events (CAP) in the system.

## Topics

### TodoCreatedEvent

**Subscribers:**

- `Notifications.TodoCreatedEventConsumer.ProcessAsync` (Message: `TodoCreatedEvent`)

**Message Flow:**

```mermaid
graph LR
    Topic[TodoCreatedEvent]
    Topic --> TodoCreatedEventConsumer[TodoCreatedEventConsumer]
```

---

### TodoCompletedEvent

**Subscribers:**

- `Notifications.TodoCompletedEventConsumer.ProcessAsync` (Message: `TodoCompletedEvent`)

**Message Flow:**

```mermaid
graph LR
    Topic[TodoCompletedEvent]
    Topic --> TodoCompletedEventConsumer[TodoCompletedEventConsumer]
```

---

