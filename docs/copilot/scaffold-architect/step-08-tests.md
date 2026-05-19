# Step 08 — Tests (multi-select)

## Propósito
- Definir baseline de calidad y cobertura desde el scaffold.
- Elegir qué capas de pruebas se generan inicialmente.

## Referencias del repo actual
- `tests/TodoUnitTests`
- `tests/FunctionalTests`
- `tests/ArchTests`
- `tests/E2ETests`

## Pregunta exacta al usuario
> "¿Qué tipos de tests quieres incluir? (multi-selección) **1) Unit (xUnit) ⭐**, **2) Functional (WebApplicationFactory) ⭐**, **3) Arch (NetArchTest) ⭐**, **4) E2E (Playwright)**."

## Opciones (con default ⭐)
- 1) ⭐ Unit (xUnit).
- 2) ⭐ Functional (WebApplicationFactory).
- 3) ⭐ Arch (NetArchTest).
- 4) E2E (Playwright).

## Implicaciones técnicas por opción
- 1) Unit:
  - Feedback rápido sobre lógica de dominio.
  - Bajo coste de mantenimiento.
- 2) Functional:
  - Valida contratos HTTP y wiring real.
  - Requiere host de pruebas y fixtures.
- 3) Arch:
  - Protege reglas de dependencia/arquitectura.
  - Evita erosión estructural en el tiempo.
- 4) E2E:
  - Cubre flujos completos UI/API.
  - Mayor coste y tiempos más largos.

## Confirmación posterior (1-2 líneas)
- Confirmar conjunto elegido (lista explícita).
- Explicar cobertura obtenida y coste esperado.

## Registro en tabla de estado antes de pasar
- `Step 08`: `tests=[...]`.
- `Estado`: `completado`.
- Nota: proyectos de tests a crear/ajustar.

## Regla de transición
- Si "no sé"/"elige tú", aplicar ⭐ `1+2+3` (sin E2E por defecto).
