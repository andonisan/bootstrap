# Step 04 — Caché

## Propósito
- Definir la estrategia de caché inicial para lecturas y respuestas.
- Balancear simplicidad, coste y escalabilidad.

## Pregunta exacta al usuario
> "¿Qué estrategia de caché quieres? **A) Ninguna**, **B) IMemoryCache**, **C) HybridCache (.NET 9) ⭐**, **D) Redis (Aspire)**, **E) Output cache**."

## Opciones (con default ⭐)
- A) Ninguna.
- B) IMemoryCache.
- C) ⭐ HybridCache (.NET 9).
- D) Redis (Aspire).
- E) Output cache.

## Implicaciones técnicas por opción
- A) Ninguna:
  - Menor complejidad inicial.
  - Puede penalizar lecturas frecuentes.
- B) IMemoryCache:
  - Muy simple para instancia única.
  - No compartida entre nodos.
- C) HybridCache ⭐:
  - Patrón recomendado moderno en .NET 9.
  - Permite evolucionar a distribuida sin reescritura grande.
- D) Redis (Aspire):
  - Caché distribuida real y escalable.
  - Requiere infraestructura adicional.
- E) Output cache:
  - Óptima para respuestas HTTP cacheables.
  - Menos útil para datos internos no HTTP.

## Confirmación posterior (1-2 líneas)
- Confirmar opción de caché.
- Explicar impacto en rendimiento/infraestructura.

## Registro en tabla de estado antes de pasar
- `Step 04`: `cache=<opción>`.
- `Estado`: `completado`.
- Nota: alcance (in-memory/distribuida/output).

## Regla de transición
- Si "no sé"/"elige tú", aplicar ⭐ HybridCache.
