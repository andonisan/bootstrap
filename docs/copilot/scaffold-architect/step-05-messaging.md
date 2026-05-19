# Step 05 — Mensajería

## Propósito
- Elegir cómo publicar y procesar eventos/comandos entre componentes.
- Definir si el scaffold arranca con broker o solo in-process.

## Pregunta exacta al usuario
> "¿Qué enfoque de mensajería prefieres? **A) In-process con MediatR ⭐**, **B) MassTransit + RabbitMQ (Aspire)**, **C) MassTransit + Azure Service Bus**, **D) Outbox pattern**."

## Opciones (con default ⭐)
- A) ⭐ In-process con MediatR.
- B) MassTransit + RabbitMQ (Aspire).
- C) MassTransit + Azure Service Bus.
- D) Outbox pattern.

## Implicaciones técnicas por opción
- A) In-process ⭐:
  - Simplicidad y velocidad de arranque.
  - Sin desacoplo distribuido inicial.
- B) RabbitMQ:
  - Mensajería robusta local con Aspire.
  - Añade complejidad operativa moderada.
- C) Azure Service Bus:
  - Integración cloud enterprise.
  - Dependencia de entorno Azure y costes.
- D) Outbox pattern:
  - Mejora consistencia entre DB y eventos.
  - Suele combinarse con broker externo.

## Confirmación posterior (1-2 líneas)
- Confirmar elección de mensajería.
- Explicar implicación en entrega/consistencia de eventos.

## Registro en tabla de estado antes de pasar
- `Step 05`: `mensajeria=<opción>`.
- `Estado`: `completado`.
- Nota: in-process o distribuida.

## Regla de transición
- Si "no sé"/"elige tú", aplicar ⭐ In-process con MediatR.
