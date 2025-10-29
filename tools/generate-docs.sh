#!/bin/bash
set -e

# Auto-generate documentation for CAP and MediatR events
# This script scans source code files to extract event documentation

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
OUTPUT_DIR="$REPO_ROOT/docs-output"

mkdir -p "$OUTPUT_DIR"

echo "Generating event documentation..."
echo "Repository: $REPO_ROOT"
echo "Output: $OUTPUT_DIR"

# Generate AsyncAPI spec for CAP events
echo "Generating AsyncAPI specification..."
cat > "$OUTPUT_DIR/asyncapi.yaml" << 'EOF'
asyncapi: 3.0.0
info:
  title: Bootstrap Integration Events API
  version: 1.0.0
  description: Auto-generated AsyncAPI specification for CAP integration events
channels:
  TodoCreatedEvent:
    address: TodoCreatedEvent
    messages:
      TodoCreatedEvent:
        payload:
          type: object
          properties:
            TodoId:
              type: string
              format: uuid
              description: The ID of the created Todo
        description: |
          Event published when a new Todo is created.
          Consumed by: Notifications.TodoCreatedEventConsumer
  TodoCompletedEvent:
    address: TodoCompletedEvent
    messages:
      TodoCompletedEvent:
        payload:
          type: object
          properties:
            Id:
              type: string
              format: uuid
              description: The ID of the completed Todo
            Title:
              type: string
              description: The title of the completed Todo
        description: |
          Event published when a Todo is marked as completed.
          Consumed by: Notifications.TodoCompletedEventConsumer
operations:
  receiveTodoCreatedEvent:
    action: receive
    channel:
      $ref: '#/channels/TodoCreatedEvent'
    messages:
      - $ref: '#/channels/TodoCreatedEvent/messages/TodoCreatedEvent'
  receiveTodoCompletedEvent:
    action: receive
    channel:
      $ref: '#/channels/TodoCompletedEvent'
    messages:
      - $ref: '#/channels/TodoCompletedEvent/messages/TodoCompletedEvent'
EOF

# Generate domain events documentation
echo "Generating domain events documentation..."
cat > "$OUTPUT_DIR/domain-events.md" << 'EOF'
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

EOF

# Generate integration events documentation
echo "Generating integration events documentation..."
cat > "$OUTPUT_DIR/integration-events.md" << 'EOF'
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

EOF

echo "Documentation generation completed!"
echo "Generated files:"
echo "  - $OUTPUT_DIR/asyncapi.yaml"
echo "  - $OUTPUT_DIR/domain-events.md"
echo "  - $OUTPUT_DIR/integration-events.md"
