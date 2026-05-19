# Step 03 — Persistencia

## Propósito
- Seleccionar la estrategia de acceso a datos inicial.
- Ajustar recursos de Aspire y patrón de repositorio/modelado.

## Pregunta exacta al usuario
> "¿Qué persistencia quieres? **A) EF Core + Postgres (Aspire) ⭐**, **B) EF Core + SQL Server**, **C) Dapper + Postgres**, **D) Marten**, **E) MongoDB**, **F) Ninguna**."

## Opciones (con default ⭐)
- A) ⭐ EF Core + Postgres (Aspire).
- B) EF Core + SQL Server.
- C) Dapper + Postgres.
- D) Marten.
- E) MongoDB.
- F) Ninguna.

## Implicaciones técnicas por opción
- A) EF + Postgres ⭐:
  - Migrations y modelo relacional con integración Aspire.
  - Camino más equilibrado entre productividad y control.
- B) EF + SQL Server:
  - Similar a A, cambiando proveedor y recursos.
  - Ideal si hay estándar corporativo SQL Server.
- C) Dapper + Postgres:
  - Más control SQL y rendimiento fino.
  - Sin change tracking automático.
- D) Marten:
  - Document DB sobre Postgres con eventos opcionales.
  - Acelera patrones event-sourcing.
- E) MongoDB:
  - Modelo documental y flexibilidad de esquema.
  - Cambia estrategia de consistencia/consultas.
- F) Ninguna:
  - Scaffold sin data layer inicial.
  - Útil para prototipos temporales.

## Confirmación posterior (1-2 líneas)
- Confirmar persistencia seleccionada.
- Explicar el tipo de almacenamiento y tooling esperado.

## Registro en tabla de estado antes de pasar
- `Step 03`: `persistencia=<opción>`.
- `Estado`: `completado`.
- Nota: proveedor y estrategia de datos.

## Regla de transición
- Si "no sé"/"elige tú", usar ⭐ EF Core + Postgres (Aspire).
