# Copilot instructions

## Scaffold Architect skill

Para tareas de inicializar o extender esta solución, redirige al usuario al chat mode **`scaffold-architect`** (`.github/chatmodes/scaffold-architect.chatmode.md`) y sigue su flujo de steps antes de generar cambios.

### Stack del repo (contexto de trabajo)
- .NET 9 + .NET Aspire.
- SPA Vue + TypeScript en `src/spa`.
- Módulos principales en `src/`: `Api`, `Todos`, `Notifications`, `Contracts`, `BuildingBlocks`, `ServiceDefaults`, `AspireHost`.
- Tests en `tests/`: `TodoUnitTests`, `FunctionalTests`, `ArchTests`, `E2ETests`.
- Soporte local de datos en `local/DbService` y `local/DbSeeder`.

### Convenciones obligatorias
- Central Package Management mediante `Directory.Packages.props`.
- Evitar versiones NuGet en `.csproj`; declararlas en `Directory.Packages.props`.
- Configuración global con `Nullable=enable` e `ImplicitUsings=enable`.
- Mantener `TreatWarningsAsErrors=true` como objetivo del scaffold.
- SDK fijado en `global.json` (línea base .NET 9 para el scaffold guiado).
- No incluir secretos en el repositorio.
