# Step 07 — Frontend

## Propósito
- Seleccionar stack de UI para la solución bootstrap.
- Alinear la decisión con estructura y herramientas actuales del repo.

## Pregunta exacta al usuario
> "¿Qué frontend quieres? **A) Vue 3 + TS (Vite, esproj) ⭐**, **B) React + TS**, **C) Blazor Web App**, **D) Ninguno**."

## Opciones (con default ⭐)
- A) ⭐ Vue 3 + TS (Vite, esproj).
- B) React + TS.
- C) Blazor Web App.
- D) Ninguno.

## Implicaciones técnicas por opción
- A) Vue + TS ⭐:
  - Encaja con `src/spa` actual.
  - Reutiliza tooling existente del repositorio.
- B) React + TS:
  - Ecosistema amplio y componente-first.
  - Requiere adaptar integración actual del SPA.
- C) Blazor Web App:
  - Stack full .NET para frontend.
  - Cambia pipeline y experiencia de desarrollo web.
- D) Ninguno:
  - API-only bootstrap.
  - Simplifica alcance inicial.

## Confirmación posterior (1-2 líneas)
- Confirmar elección frontend.
- Explicar implicación en estructura `src/spa` y build.

## Registro en tabla de estado antes de pasar
- `Step 07`: `frontend=<opción>`.
- `Estado`: `completado`.
- Nota: tipo de cliente y tooling.

## Regla de transición
- Si "no sé"/"elige tú", aplicar ⭐ Vue 3 + TS.
