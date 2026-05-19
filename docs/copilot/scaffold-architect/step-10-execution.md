# Step 10 — Ejecución guiada

## Propósito
- Ejecutar el scaffold confirmado de forma trazable.
- Reportar progreso en tiempo real por sub-paso.

## Formato obligatorio de reporte
- `⏳ <n>/15 <sub-paso> — en progreso`
- `✅ <n>/15 <sub-paso> — completado`
- `❌ <n>/15 <sub-paso> — error: <causa + siguiente acción>`

## Sub-pasos de generación (15)
1. **Archivos raíz**
   - Acción: crear/validar `global.json`, `Directory.Build.props`, `Directory.Packages.props`, `.editorconfig`.
2. **Solución (.sln)**
   - Acción: crear o actualizar `BootStrap.sln`.
   - Comando típico: `dotnet new sln -n BootStrap` (si no existe).
3. **Backend base**
   - Acción: crear/actualizar `Api`, `ServiceDefaults`, `BuildingBlocks`, `Contracts`.
4. **Arquitectura seleccionada**
   - Acción: aplicar estructura según Step 01 (capas/features/módulos).
5. **CQRS**
   - Acción: registrar handlers/pipeline según Step 02.
6. **Persistencia**
   - Acción: configurar proveedor y wiring según Step 03.
7. **Caché**
   - Acción: habilitar estrategia de caché según Step 04.
8. **Mensajería**
   - Acción: configurar publicación/consumo según Step 05.
9. **Auth**
   - Acción: aplicar baseline de seguridad según Step 06.
10. **Frontend**
    - Acción: configurar stack según Step 07 (p. ej. `src/spa` con esproj).
11. **Tests**
    - Acción: crear/ajustar proyectos de pruebas según Step 08.
12. **Endpoint demo + tests**
    - Acción: exponer endpoint mínimo y cubrirlo con tests elegidos.
13. **ADR inicial**
    - Acción: crear `docs/adr/0001-initial-architecture.md`.
14. **Validación técnica**
    - Comandos: `dotnet build BootStrap.sln` y `dotnet test BootStrap.sln`.
15. **Preparación de PR**
    - Acción: resumen final y propuesta de PR `chore: initial scaffold`.

## Reglas de ejecución
- Mantener salida breve y orientada a estado.
- Ante error, no ocultarlo: marcar `❌`, explicar causa y proponer corrección.
- Al final, mostrar checklist con 15 sub-pasos y su estado final.
