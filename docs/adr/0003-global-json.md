# 3. Uso de global.json para fijar versión del SDK

Date: 2025-05-25

## Status

Accepted

## Context

Queremos asegurar que todos los desarrolladores y entornos de CI/CD usen exactamente la misma versión del SDK de .NET para evitar inconsistencias.

## Decision

Usaremos un archivo `global.json` para especificar la versión exacta del SDK de .NET que se utilizará en el proyecto, configurando además `rollForward` en `latestPatch`.

```json
{
  "sdk": {
    "version": "9.0.100",
    "rollForward": "latestPatch"
  }
}
```

## Consequences

✅ Consistencia en entornos de desarrollo y CI/CD  
✅ Evita problemas del tipo "en mi máquina funciona"  
❌ Requiere actualizar manualmente al cambiar de versión mayor o menor del SDK
