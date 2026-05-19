# Step 02 — CQRS

## Propósito
- Definir cómo separar comandos y consultas.
- Elegir framework/patrón para handlers y pipeline.

## Pregunta exacta al usuario
> "Para CQRS, ¿prefieres **A) MediatR ⭐**, **B) Wolverine**, **C) Handlers propios**, o **D) No usar CQRS (services directos)**?"

## Opciones (con default ⭐)
- A) ⭐ MediatR.
- B) Wolverine.
- C) Handlers propios.
- D) No CQRS (services directos).

## Implicaciones técnicas por opción
- A) MediatR ⭐:
  - Estándar de facto para commands/queries en .NET.
  - Pipeline behaviors para validación/logging.
- B) Wolverine:
  - Unifica CQRS + mensajería avanzada.
  - Mayor curva y decisiones de infraestructura.
- C) Handlers propios:
  - Máximo control y mínima dependencia externa.
  - Más código boilerplate a mantener.
- D) Services directos:
  - Menos complejidad inicial.
  - Menor separación de responsabilidades.

## Confirmación posterior (1-2 líneas)
- Confirmar elección CQRS.
- Explicar implicación en estructura de casos de uso.

## Registro en tabla de estado antes de pasar
- `Step 02`: `cqrs=<opción>`.
- `Estado`: `completado`.
- Nota: uso o no de handlers/pipeline.

## Regla de transición
- Si "no sé"/"elige tú", aplicar ⭐ MediatR.
