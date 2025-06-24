# 4. Central Package Management (CPM)

Date: 2025-05-25

## Status

Accepted

## Context

Queremos mantener coherencia en las versiones de paquetes NuGet en todos los proyectos y simplificar la gestión de dependencias.

## Decision

Usaremos Central Package Management (CPM) mediante el archivo `Directory.Packages.props` para gestionar centralizadamente las versiones de paquetes NuGet.

## Consequences

✅ Versiones consistentes en todos los proyectos  
✅ Simplificación en actualizaciones de dependencias  
✅ Menos conflictos de versiones  
❌ Requiere aprendizaje inicial del equipo sobre CPM
