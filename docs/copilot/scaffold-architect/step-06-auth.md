# Step 06 — Auth

## Propósito
- Definir postura inicial de autenticación/autorización.
- Evitar bloquear el scaffold cuando auth completa aún no está decidida.

## Pregunta exacta al usuario
> "¿Qué autenticación quieres inicialmente? **A) Ninguna (placeholder) ⭐**, **B) JWT Bearer**, **C) Keycloak (Aspire)**, **D) Entra ID**."

## Opciones (con default ⭐)
- A) ⭐ Ninguna (placeholder).
- B) JWT Bearer.
- C) Keycloak (Aspire).
- D) Entra ID.

## Implicaciones técnicas por opción
- A) Placeholder ⭐:
  - Permite avanzar sin bloquear negocio inicial.
  - Requiere hardening posterior antes de producción.
- B) JWT Bearer:
  - Estándar API-first con tokens.
  - Necesita issuer/audience y políticas.
- C) Keycloak:
  - IdP self-hosted integrable con Aspire.
  - Mayor operación y configuración.
- D) Entra ID:
  - Integración enterprise en ecosistema Microsoft.
  - Dependencia de tenant/registro de apps.

## Confirmación posterior (1-2 líneas)
- Confirmar estrategia de auth.
- Explicar impacto en seguridad y tiempos de implementación.

## Registro en tabla de estado antes de pasar
- `Step 06`: `auth=<opción>`.
- `Estado`: `completado`.
- Nota: proveedor y nivel de seguridad inicial.

## Regla de transición
- Si "no sé"/"elige tú", aplicar ⭐ Ninguna (placeholder).
