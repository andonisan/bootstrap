# Step 09 — Resumen y confirmación

## Propósito
- Presentar el resumen final de decisiones antes de cualquier generación.
- Obtener confirmación humana explícita para ejecutar cambios.

## Acción principal
- Mostrar tabla final consolidada con Steps 00..08.
- Verificar que no hay campos pendientes.

## Pregunta exacta al usuario
> "Este es el resumen final. ¿Confirmas ejecución? Responde con: **sí**, **ajustar**, o **cancelar**."

## Respuestas válidas
- `sí`: avanzar a Step 10.
- `ajustar`: pedir un único dato adicional.
- `cancelar`: terminar sin cambios.

## Implicaciones técnicas por respuesta
- sí:
  - Se habilita la fase de ejecución.
  - Se empieza a reportar sub-pasos con `⏳/✅/❌`.
- ajustar:
  - No se genera nada todavía.
  - Se vuelve al step solicitado y luego se regresa al resumen.
- cancelar:
  - Fin de la sesión sin tocar código.
  - Se conserva el resumen para futura reanudación.

## Manejo de “ajustar”
- Pregunta exacta:
  > "Indícame el número de step que quieres revisar (01-08) y qué valor deseas cambiar."
- Reabrir solo ese step y mantener el resto fijo.

## Registro en tabla de estado antes de pasar
- `Step 09`: `confirmacion=si|ajustar|cancelar`.
- `Estado`: `completado` solo con `sí`.
- Si `ajustar`, marcar `Step 09` como `en revisión`.

## Regla crítica
- Sin un `sí` explícito del usuario, no se ejecuta Step 10.
