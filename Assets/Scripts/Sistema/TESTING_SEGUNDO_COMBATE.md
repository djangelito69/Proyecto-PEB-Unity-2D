# ?? Testing - Verificar que funciona el segundo combate

## ? Checklist de Testing

### Primer combate:
- [ ] Abres el juego en tu escena principal (mapa)
- [ ] Te acercas a un enemigo
- [ ] Colisiona y carga escena de combate ?
- [ ] Peleas y **ganas**
- [ ] Panel de victoria aparece (verde, "¡VICTORIA!")
- [ ] Haces click en "Continuar"
- [ ] Vuelves al mapa

### Segundo combate (EL QUE FALLABA):
- [ ] Estás de vuelta en el mapa
- [ ] El sprite del jugador **NO está parpadeando** (inmunidad terminó)
- [ ] Te acercas a otro enemigo
- [ ] **Colisiona y carga escena de combate** ? **ESTO DEBE FUNCIONAR AHORA**
- [ ] Peleas y ganas de nuevo
- [ ] Panel de victoria aparece

### Verificación en Console:

Cuando vuelves del primer combate, deberías ver:

```
GestorCombateGlobal: Transición reestablecida. Listo para próximo combate.
GestorCombateGlobal: Escena de combate descargada. Reestableciendo transición automáticamente.
```

Cuando colisiona el segundo enemigo, deberías ver:

```
GestorCombateGlobal: Iniciando combate con [Nombre del enemigo]
```

---

## ?? Si aún falla:

**Síntoma:** "El segundo enemigo no inicia combate"

**Causas posibles:**

1. **Los scripts NO se actualizaron**
   - Verifica que los 4 archivos tienen los cambios:
     - `GestorCombateGlobal.cs`
     - `GestorDeCombate.cs`
     - `UICombateVictoria.cs`
     - `UICombateGameOver.cs`

2. **Unity no recompilando**
   - Guarda todos los archivos (Ctrl+S)
   - En Unity: Espera a que diga "Compilation Successful"
   - Si no aparece, haz Play y pausa (Ctrl+P) y Play de nuevo

3. **El flag no se limpia**
   - Abre Console (Window ? General ? Console)
   - Busca si ves "Transición reestablecida"
   - Si NO lo ves, los scripts no están correctamente asignados

4. **Problema de timing**
   - La escena tarda en descargar
   - Espera 2-3 segundos después de volver al mapa antes de colisionar
   - Verifica que ves el texto "Listo para próximo combate" en Console

---

## ?? Pasos para limpiar problemas:

### Opción 1: Recompilación forzada
1. En Unity ? Assets ? Reimport All
2. Espera a que termine
3. Play

### Opción 2: Limpieza de cache
1. Cierra Unity
2. Elimina carpeta `Library` (en la raíz del proyecto)
3. Abre de nuevo (Unity reconstruirá la carpeta)
4. Espera a que compile
5. Play

### Opción 3: Verificar código manualmente
1. Abre `GestorCombateGlobal.cs` en tu editor
2. Verifica que tiene `SceneManager.sceneUnloaded += OnSceneUnloaded;` en Awake()
3. Verifica que existe el método `OnSceneUnloaded(Scene scene)`
4. Guarda y vuelve a Unity

---

## ? Si funciona correctamente verás:

? Primer combate ? ganas ? vuelves
? Segundo combate ? se carga la escena
? Tercer combate ? se carga la escena
? Y así sucesivamente sin problemas

---

## ?? Console output esperado (victoria ? segundo combate)

```
[COMBATE] Jugador configurado: [Tu personaje]
[COMBATE] Enemigo: Goblin
¡Ganaste!
¡Ganaste 50 XP!
UICombateVictoria: Mostrando pantalla de victoria
UICombateVictoria: Volviendo al mapa
Time.timeScale = 1.0
UICombateVictoria: Flag de transición reestablecido
GestorCombateGlobal: Transición reestablecida. Listo para próximo combate.
GestorCombateGlobal: Escena de combate descargada. Reestableciendo transición automáticamente.

[Esperar 2-3 segundos]

[Acercarse a nuevo enemigo]

Enemigo: OnCollisionEnter2D detecta colisión
GestorCombateGlobal: Iniciando combate con Orco
GestorCombateGlobal: Inmunidad del jugador activada
PlayerImmunity: Inmunidad activada

? Escena de combate carga exitosamente
```

---

**¿Aún no funciona? Cuéntame qué ves en la Console y te ayudaré a depurar. ??**
