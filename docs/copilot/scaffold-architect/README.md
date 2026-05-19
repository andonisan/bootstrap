# Scaffold Architect

Skill conversacional de GitHub Copilot para guiar, paso a paso, la inicialización o extensión de una solución **.NET 9 + .NET Aspire** antes de generar código.

## Qué es

`scaffold-architect` es un chat mode que:
- pregunta una decisión técnica por turno,
- confirma cada respuesta y su impacto,
- mantiene una tabla de estado,
- no ejecuta cambios hasta confirmación explícita,
- y luego reporta avance con `⏳/✅/❌`.

## Cómo activarla

### En VS Code
1. Abre el panel de GitHub Copilot Chat.
2. Selecciona el chat mode `scaffold-architect`.
3. Inicia la conversación con tu objetivo de scaffold.

### Desde el coding agent (GitHub)
1. Crea/abre un issue y asígnalo a `@copilot`.
2. Indica que use el chat mode `scaffold-architect`.
3. Incluye contexto de alcance (crear o extender) y restricciones.

## Flujo de steps (ASCII)

```text
[00 Welcome]
    |
    v
[01 Architecture] -> [02 CQRS] -> [03 Persistence] -> [04 Cache]
    |
    v
[05 Messaging] -> [06 Auth] -> [07 Frontend] -> [08 Tests]
    |
    v
[09 Summary + confirmación explícita]
    | sí
    v
[10 Execution ⏳✅❌]
```

## Templates por step

| Step | Archivo |
|---|---|
| 00 | [step-00-welcome.md](./step-00-welcome.md) |
| 01 | [step-01-architecture.md](./step-01-architecture.md) |
| 02 | [step-02-cqrs.md](./step-02-cqrs.md) |
| 03 | [step-03-persistence.md](./step-03-persistence.md) |
| 04 | [step-04-cache.md](./step-04-cache.md) |
| 05 | [step-05-messaging.md](./step-05-messaging.md) |
| 06 | [step-06-auth.md](./step-06-auth.md) |
| 07 | [step-07-frontend.md](./step-07-frontend.md) |
| 08 | [step-08-tests.md](./step-08-tests.md) |
| 09 | [step-09-summary.md](./step-09-summary.md) |
| 10 | [step-10-execution.md](./step-10-execution.md) |
