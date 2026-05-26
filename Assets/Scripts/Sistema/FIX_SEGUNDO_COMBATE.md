# ?? FIX - Sistema de Combate: Segundo combate no iniciaba

## ?? Problema

Después de ganar el primer combate y volver al mapa, al colisionar con un enemigo nuevamente **no se iniciaba el combate**. La escena de combate no cargaba.

### Causa raíz

El flag `combateEnTransicion` en `GestorCombateGlobal` **no se reestablecía correctamente** porque:

1. `SceneManager.UnloadSceneAsync()` es **asincrónico** (se ejecuta después)
2. El flag se reestablecía antes de que la escena terminara de descargarse
3. Resultado: El flag quedaba en un estado inconsistente

## ? Soluciones implementadas

### 1. **Múltiples puntos de reestablecimiento**

Ahora el flag se reestablece en 3 lugares diferentes:

- **En `GestorDeCombate.VolverAlMapa()`** - Cuando ganas
- **En `UICombateVictoria.Continuar()`** - Botón Continuar
- **En `UICombateGameOver.Reintentar()`** - Botón Reintentar
- **En `UICombateGameOver.IrAlMenuPrincipal()`** - Botón Menú

Esto asegura que pase lo que pase, el flag se reestablece.

### 2. **Reestablecimiento automático mediante eventos de escena**

Añadido en `GestorCombateGlobal`:

```csharp
SceneManager.sceneUnloaded += OnSceneUnloaded;

private void OnSceneUnloaded(Scene scene)
{
    if (scene.name == "Combate")
    {
        ReestablecerTransicion();
    }
}
```

Cuando la escena de combate se **termina de descargar**, el sistema **automáticamente** reestablece el flag. Esta es una red de seguridad adicional.

### 3. **Mejorado orden de operaciones**

En todos los lugares se sigue este orden:

```csharp
1. Time.timeScale = 1f              // Reanudar juego
2. ReestablecerTransicion()         // Limpiar flag PRIMERO
3. SceneManager.UnloadSceneAsync()  // Descargar DESPUÉS
```

Así se asegura que el flag esté limpio ANTES de que se inicie una nueva solicitud de combate.

## ?? Cambios específicos

### GestorCombateGlobal.cs
- ? Añadido evento `sceneUnloaded`
- ? Método `OnSceneUnloaded()` para reestablecimiento automático
- ? Desuscripción en `OnDestroy()`

### GestorDeCombate.cs
- ? Reordenado: ReestablecerTransicion() ANTES de UnloadSceneAsync()
- ? Añadido logging para depuración

### UICombateVictoria.cs
- ? Reestablecimiento explícito en `Continuar()`

### UICombateGameOver.cs
- ? Reestablecimiento explícito en `Reintentar()`
- ? Reestablecimiento explícito en `IrAlMenuPrincipal()`

## ?? Testing

Verifica que funciona:

1. ? Pelea y **gana** contra un enemigo
2. ? Vuelve al mapa
3. ? El sprite del jugador **deja de parpadear** (inmunidad termina)
4. ? Colisiona con otro enemigo
5. ? **La escena de combate carga** (antes no lo hacía)
6. ? Pelea y gana de nuevo
7. ? Repite sin problemas

## ?? Flujo mejorado

```
PRIMER COMBATE
?? Ganas
?? UICombateVictoria.Continuar() ? ReestablecerTransicion()
?? SceneManager.UnloadSceneAsync("Combate")
?? Evento OnSceneUnloaded("Combate") ? ReestablecerTransicion() [RED DE SEGURIDAD]
?? Flag = false ?
?? Vuelves al mapa

SEGUNDO COMBATE
?? Colisionar con enemigo
?? Enemigo.OnCollisionEnter2D()
?? GestorCombateGlobal.IntentarIniciarCombate()
?? ¿combateEnTransicion? NO (porque lo limpiamos)
?? ? Iniciamos combate
?? Escena de combate carga ?
```

## ?? Debugging

Si aún no funciona, verifica la Console:

```
? "GestorCombateGlobal: Transición reestablecida"
? "Escena de combate descargada. Reestableciendo transición automáticamente"
? Si no ves estos mensajes, revisa que los scripts estén actualizados
```

## ? Robustez

El sistema ahora es **resistente a fallos**:

- ? Múltiples intentos de reestablecimiento
- ? Evento automático de escena
- ? Logging completo para depuración
- ? Sin estado inconsistente posible

---

**¡Ya debería funcionar correctamente! ??**
