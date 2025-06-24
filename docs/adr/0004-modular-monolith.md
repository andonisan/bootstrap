# 4. Modular Monolith

Date: 2025-05-25

## Status

Accepted

## Context

Queremos evitar la complejidad innecesaria de microservicios, pero mantener una estructura modular que permita evolución y escalabilidad.

## Decision

Adoptamos una arquitectura Modular Monolith con módulos separados por carpetas (Application, Domain, Infrastructure, Web). Usaremos ArchUnit.NET para validar dependencias entre módulos.

## Consequences

✅ Simplicidad operativa y despliegue  
✅ Fácil refactorización y evolución  
❌ Posible acoplamiento accidental si no se vigilan dependencias  
