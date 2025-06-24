# 2. Data Access Strategy

Date: 2025-05-25

## Status

Accepted

## Context

Necesitamos una estrategia clara para el acceso a datos que equilibre simplicidad, rendimiento y flexibilidad según el contexto.

## Decision

Usaremos un enfoque híbrido basado en el caso de uso:

- 🔧 **EF Core**: Comandos (CRUD), migrations
- ⚡ **Dapper**: Queries complejas, reportes  
- 🚀 **SQL crudo**: Performance crítica

## Consequences

✅ Performance optimizada según contexto  
❌ Múltiples abstracciones a mantener
