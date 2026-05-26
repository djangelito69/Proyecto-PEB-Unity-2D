# ?? Sistema de Inmunidad y Control de Combate - Guía de Instalación

## ? Qué se ha implementado

### 1. **GestorCombateGlobal.cs** (NUEVO - Crítico)
   - Singleton persistente que existe desde el inicio del juego
   - Previene cargas múltiples de escenas de combate
   - Controla que solo un combate se inicie a la vez
   - Métodos públicos:
     - `IntentarIniciarCombate()` - valida si puede iniciar combate
     - `ReestablecerTransicion()` - limpia el flag tras volver del combate

### 2. **GestorDeCombate.cs** (modificado)
   - Ahora se coordina con `GestorCombateGlobal`
   - Llama a `ReestablecerTransicion()` al volver al mapa
   - Mantiene la lógica de combate intacta

### 3. **PlayerImmunity.cs** (nuevo)
   - Gestiona la inmunidad temporal del jugador
   - Parpadeo visual en rojo durante 2 segundos
   - Enemigos no lo detectan mientras está inmune
   - Totalmente configurable desde Inspector

### 4. **Enemigo.cs** (modificado)
   - Verifica inmunidad del jugador antes de iniciar combate
   - Llama a `GestorCombateGlobal.IntentarIniciarCombate()`
   - Solo el primer enemigo inicia combate; otros son rechazados

### 5. **MovimientoPersonaje.cs** (modificado)
   - Integración con `PlayerImmunity`

---

## ??? Pasos de Configuración en Unity (IMPORTANTE)

### Paso 1: Añadir GestorCombateGlobal a la escena principal
**ESTO ES CRÍTICO - Sin esto, habrá error "Object reference not set"**

1. Abre tu escena principal (el mapa donde está el jugador y los enemigos)
2. Crea un **GameObject vacío** y llámalo "Managers" o "GlobalManagers"
3. En el Inspector ? **Add Component** ? `GestorCombateGlobal`
4. Guarda la escena
5. ? Listo - este script persistirá entre escenas

### Paso 2: Añadir PlayerImmunity al jugador

1. Selecciona el GameObject del jugador (tag "Player")
2. Inspector ? **Add Component** ? `PlayerImmunity`
3. Configura (opcional):
   - **Duración Inmunidad**: 2 segundos (ajustable)
   - **Color Inmunidad**: Rojo translúcido (1, 0.5, 0.5, 1)
   - **Velocidad Parpadeo**: 0.1 segundos

### Paso 3: Verificar que no hay errores
- Abre la escena del mapa
- En Play Mode, acércate a un enemigo
- Debería iniciar combate sin errores
- El sprite debería parpadear en rojo

---

## ?? Cómo funciona

### Flujo de colisión (sin errores):

```
Jugador choca con Enemigo A
    ?
Enemigo A: ¿GestorCombateGlobal existe? SÍ ?
    ?
Enemigo A: ¿Jugador inmune? NO
    ?
Enemigo A: Llama a GestorCombateGlobal.IntentarIniciarCombate()
    ?
GestorCombateGlobal: ¿Combate en transición? NO
    ?
GestorCombateGlobal: combateEnTransicion = true
    ?
GestorCombateGlobal: Carga escena "Combate"
    ?
GestorCombateGlobal: Activa PlayerImmunity
    ?
Enemigo A: enCombate = true
```

### Protección contra múltiples enemigos:

```
Jugador choca con Enemigo A ? combateEnTransicion = true
(casi simultáneamente)
Jugador choca con Enemigo B ? GestorCombateGlobal.IntentarIniciarCombate()
                             ? "Combate ya en transición" ? return false
                             ? Enemigo B ignorado ?
```

### Regreso del combate:

```
Combate termina (gana o pierde)
    ?
GestorDeCombate.VolverAlMapa()
    ?
GestorCombateGlobal.ReestablecerTransicion()
    ?
combateEnTransicion = false
    ?
Escena de combate se descarga
    ?
Sistema listo para próximo combate ?
```

---

## ?? Solución del error "Object reference not set to an instance of an object"

**Causa**: `GestorCombateGlobal` no existía en la escena  
**Solución**: Crear GameObject con `GestorCombateGlobal` en la escena principal

**Logs que verás si está correctamente configurado:**

```
GestorCombateGlobal: Inicializado como singleton persistente
GestorCombateGlobal: Iniciando combate con Goblin
GestorCombateGlobal: Inmunidad del jugador activada
PlayerImmunity: Inmunidad activada
[escena de combate carga]
[combate termina]
GestorDeCombate: Volviendo al mapa...
GestorCombateGlobal: Transición reestablecida. Listo para próximo combate.
```

---

## ?? Estados del jugador

| Estado | Descripción | Enemigos detectan | Combate posible |
|--------|-------------|------------------|-----------------|
| Normal | Libre | Sí | Sí |
| Inmune | Parpadeo rojo, 2s | NO ? | NO ? |
| En combate | Escena de combate activa | NO | NO |

---

## ?? Personalización

### Cambiar duración de inmunidad:
En `PlayerImmunity` (Inspector):
```
Duración Inmunidad = 3 (para 3 segundos)
```

### Cambiar color:
```
Color Inmunidad = RGB(0.8, 0.2, 0.2) para rojo más oscuro
```

### Cambiar velocidad de parpadeo:
```
Velocidad Parpadeo = 0.05 (parpadea más rápido)
```

---

## ? Ventajas del sistema

? **Robusto**: Previene race conditions  
? **Visual**: Parpadeo rojo indica inmunidad  
? **Escalable**: Funciona con N enemigos  
? **Limpio**: Código bien separado en responsabilidades  
? **Centralizado**: `GestorCombateGlobal` es la fuente única de verdad  
? **Configurable**: Todo desde Inspector  
? **Sin romper existente**: Combate sigue funcionando igual  

---

## ?? Requisitos finales

- ? Jugador con tag "Player"
- ? `SpriteRenderer` en jugador (o hijo)
- ? `GestorCombateGlobal` en escena principal (singleton persistente)
- ? `PlayerImmunity` en GameObject del jugador
- ? Enemigos con `Enemigo.cs` actualizado

---

## ?? Testing rápido

1. Abre escena principal (mapa)
2. Play Mode
3. Acércate a un enemigo
4. Debería:
   - Iniciar combate SIN errores
   - Sprite debería parpadear en rojo ~2 segundos
   - Otros enemigos cercanos NO inician combate
5. Termina combate (gana o pierde)
6. Vuelves al mapa
7. Puedes hacer combate de nuevo

¡Listo para usar! ??
