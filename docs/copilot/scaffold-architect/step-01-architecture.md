# Step 01 — Arquitectura

## Propósito
- Elegir el estilo arquitectónico base para organizar módulos y dependencias.
- Dejar claro cómo afectará al scaffold posterior.

## Pregunta exacta al usuario
> "¿Qué arquitectura quieres como base? **A) Clean**, **B) Vertical Slice ⭐**, **C) Hexagonal**, **D) Modular Monolith**."

## Opciones (con default ⭐)
- A) Clean Architecture.
- B) ⭐ Vertical Slice.
- C) Hexagonal.
- D) Modular Monolith.

## Implicaciones técnicas por opción
- A) Clean:
  - Separación estricta por capas (Application/Domain/Infrastructure).
  - Mayor disciplina de dependencias y más proyectos.
- B) Vertical Slice ⭐:
  - Organización por feature (commands/queries/endpoints juntos).
  - Iteración rápida y buen encaje con Minimal APIs.
- C) Hexagonal:
  - Dominio aislado con puertos/adaptadores.
  - Excelente testabilidad, más abstracciones iniciales.
- D) Modular Monolith:
  - Módulos con fronteras internas claras en un despliegue único.
  - Facilita evolución a servicios si se necesita.

## Confirmación posterior (1-2 líneas)
- Repetir arquitectura elegida.
- Explicar impacto en estructura de carpetas/proyectos.

## Registro en tabla de estado antes de pasar
- `Step 01`: `arquitectura=<opción>`.
- `Estado`: `completado`.
- Nota: patrón de organización principal.

## Regla de transición
- Si respuesta ambigua, pedir aclaración con una sola pregunta.
- Si "no sé"/"elige tú", usar ⭐ Vertical Slice.
