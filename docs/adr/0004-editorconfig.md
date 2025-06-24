# 4. Uso de .editorconfig para consistencia de estilo

Date: 2025-05-25

## Status

Accepted

## Context

Queremos mantener un estilo de código consistente en todo el proyecto, independientemente del IDE o editor utilizado por cada desarrollador.

## Decision

Usaremos un archivo `.editorconfig` para definir reglas básicas de estilo y formato del código fuente.

```editorconfig
dotnet_sort_system_directives_first = true
dotnet_style_qualification_for_field = false
```

## Consequences

✅ Estilo consistente en todo el proyecto  
✅ Menos discusiones sobre estilo en revisiones de código  
❌ Requiere consenso inicial sobre reglas de estilo
