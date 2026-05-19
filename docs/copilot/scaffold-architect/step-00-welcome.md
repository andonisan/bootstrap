# Step 00 — Bienvenida y contexto

## Propósito
- Dar la bienvenida y alinear objetivo del usuario.
- Detectar si el repositorio está vacío o ya tiene solución.
- Definir modo inicial: crear desde cero o extender.

## Detección de contexto para este repo
- En `andonisan/bootstrap` **ya existe** una solución .NET + Aspire.
- Debes proponer por defecto ⭐ **modo Extender**.
- Si el usuario pide reconstrucción total, confirmar impacto antes de seguir.

## Pregunta exacta al usuario
> "He detectado que este repo ya tiene una solución (.NET + Aspire con módulos y tests). ¿Quieres que trabajemos en modo **A) Extender lo existente ⭐** o **B) Crear/re-bootstrappear desde cero**?"

## Opciones (con default ⭐)
- A) ⭐ Extender lo existente.
- B) Crear/re-bootstrappear desde cero.

## Implicaciones técnicas por opción
- A) Extender:
  - Mantiene estructura actual y minimiza ruptura.
  - Prioriza cambios incrementales y compatibilidad.
- B) Re-bootstrappear:
  - Puede requerir reestructurar solución/proyectos.
  - Mayor impacto en historial, referencias y tests existentes.

## Confirmación posterior (1-2 líneas)
- Confirmar elección del usuario.
- Explicar efecto inmediato en alcance y riesgo.

## Registro en tabla de estado antes de pasar
- `Step 00`: `modo=extender|rebootstrap`.
- `Estado`: `completado`.
- Añadir nota de contexto: `repo_existente=true`.

## Regla de transición
- Solo avanzar a Step 01 cuando el usuario confirme que está listo para continuar.
- Si responde "no sé"/"elige tú", aplicar ⭐ Extender.
- Si responde "cancelar", finalizar sin cambios y conservar resumen de contexto.
