# 5. Estrategia de Testing

Date: 2025-05-25

## Status

Accepted

## Context

Queremos asegurar calidad y mantenibilidad mediante tests automatizados en diferentes niveles.

## Decision

Usaremos diferentes tipos de tests según la pirámide de testing:

- Unitarios con xUnit y Build pattern
- Funcionales con WebApplicationFactory
- Tests de arquitectura con ArchUnit.NET
- E2E con Playwright integrado con Aspire

## Consequences

✅ Alta cobertura y calidad del código  
✅ Detección temprana de errores  
❌ Mayor esfuerzo inicial en creación y mantenimiento de tests
