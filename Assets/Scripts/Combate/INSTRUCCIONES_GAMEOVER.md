# ?? Sistema de GameOver - Guía Completa

## ? Qué se ha implementado

### Scripts nuevos:
1. **UICombateGameOver.cs** - Gestiona la pantalla de derrota
   - Se muestra cuando la vida del jugador llega a 0
   - Muestra información: título, mensaje, nivel, vida
   - Botones: Reintentar o ir a menú principal
   - Reproduce sonido de derrota
   - Pausa el juego (`Time.timeScale = 0`)

2. **UICombateVictoria.cs** - Gestiona la pantalla de victoria
   - Se muestra cuando se derrota al enemigo
   - Muestra información: título, mensaje, XP ganada, nivel
   - Botón: Continuar (vuelve al mapa)
   - Reproduce sonido de victoria
   - Pausa el juego (`Time.timeScale = 0`)

### Scripts modificados:
3. **GestorDeCombate.cs** - Correcciones
   - Eliminada línea duplicada `using UnityEngine;`
   - Añadido `Time.timeScale = 1f` en `VolverAlMapa()` por seguridad

---

## ??? Pasos de Configuración en Unity

### Paso 1: Preparar Canvas en escena de Combate

1. Abre la escena de **Combate** (la que carga cuando luchas contra enemigos)
2. En la Jerarquía, selecciona el **Canvas** de combate (debería existir)
3. Si no existe Canvas, crea uno: Botón derecho ? UI ? Canvas

### Paso 2: Crear Panel de GameOver

1. En el Canvas de combate, crea un Panel: Botón derecho ? UI ? Panel
   - Llámalo `PanelGameOver`
   - Escala: Stretch (llena todo el Canvas)
   - Color: Semitransparente negro (0, 0, 0, 200) para oscurecer fondo

2. Dentro del `PanelGameOver`, crea elementos UI:

   **a) Título (TextMeshProUGUI)**
   - Botón derecho en `PanelGameOver` ? TextMeshProUGUI - Text
   - Llámalo `TextoTitulo`
   - Posición Y: +150
   - Font Size: 60
   - Alineación: Centro
   - Color: Rojo (255, 0, 0)

   **b) Mensaje (TextMeshProUGUI)**
   - Crea otro TextMeshProUGUI, llámalo `TextoMensaje`
   - Posición Y: +50
   - Font Size: 30
   - Alineación: Centro
   - Color: Blanco

   **c) Info (TextMeshProUGUI)** - Opcional
   - Crea dos más: `TextoNivel` y `TextoVida`
   - Posición Y: -50
   - Font Size: 20
   - Color: Gris claro

   **d) Botón Reintentar**
   - Botón derecho en `PanelGameOver` ? Button
   - Llámalo `BotonReintentar`
   - Posición Y: -150, X: -150
   - Tamaño: 200x60
   - Texto: "Reintentar"
   - Color: Verde

   **e) Botón Menú Principal**
   - Crea otro Button, llámalo `BotonMenuPrincipal`
   - Posición Y: -150, X: +150
   - Tamaño: 200x60
   - Texto: "Menú Principal"
   - Color: Gris

### Paso 3: Crear Panel de Victoria

**Repite el Paso 2 pero:**
- Llámalo `PanelVictoria`
- Título color: Verde (0, 255, 0)
- Botón: "Continuar"
- Posición Y: -150

### Paso 4: Añadir componentes de script

1. En `PanelGameOver`, añade componente `UICombateGameOver`:
   - Selecciona `PanelGameOver`
   - Inspector ? Add Component ? `UICombateGameOver`
   - Asigna referencias:
     - **Panel GameOver**: Arrastra `PanelGameOver`
     - **Texto Título**: Arrastra `TextoTitulo`
     - **Texto Mensaje**: Arrastra `TextoMensaje`
     - **Texto Nivel**: Arrastra `TextoNivel`
     - **Texto Vida**: Arrastra `TextoVida`
     - **Botón Reintentar**: Arrastra `BotonReintentar`
     - **Botón Menu Principal**: Arrastra `BotonMenuPrincipal`
     - **Audio Source**: Arrastra un AudioSource (o crea uno nuevo)
     - **Sonido Derrota**: Asigna un AudioClip de derrota

2. En `PanelVictoria`, añade componente `UICombateVictoria`:
   - Selecciona `PanelVictoria`
   - Inspector ? Add Component ? `UICombateVictoria`
   - Asigna referencias:
     - **Panel Victoria**: Arrastra `PanelVictoria`
     - **Texto Título**: Arrastra `TextoTitulo`
     - **Texto Mensaje**: Arrastra `TextoMensaje`
     - **Texto Exp Ganada**: Arrastra el TextMeshPro con XP
     - **Texto Nivel**: Arrastra `TextoNivel`
     - **Botón Continuar**: Arrastra el botón
     - **Audio Source**: Arrastra un AudioSource
     - **Sonido Victoria**: Asigna un AudioClip de victoria

---

## ?? Flujo de Funcionamiento

### Cuando pierdes:

```
Jugador.vida llega a 0
    ?
GestorDeCombate.RevisarGanador() ? if (!jugador.EstaVivo)
    ?
combateTerminado = true
    ?
Envía mensaje "Perdiste..."
    ?
UICombateGameOver.VerificarDerrota() escucha el mensaje
    ?
MostrarGameOver()
    ?
- Panel se activa
- Reproduce sonido
- Muestra textos
- Pausa Time.timeScale = 0
    ?
Usuario: Click en "Reintentar" o "Menú Principal"
    ?
Reintentar ? Recarga escena de combate
Menú Principal ? Carga escena MenuPrincipal
```

### Cuando ganas:

```
Enemigo.vida llega a 0
    ?
GestorDeCombate.RevisarGanador() ? if (!enemigo.EstaVivo)
    ?
combateTerminado = true
    ?
UICombateVictoria.VerificarVictoria() escucha el mensaje
    ?
MostrarVictoria()
    ?
- Panel se activa
- Reproduce sonido
- Muestra textos (XP ganada, nivel)
- Pausa Time.timeScale = 0
    ?
Usuario: Click en "Continuar"
    ?
Vuelve al mapa (descarga escena de combate)
```

---

## ?? Personalización recomendada

### En UICombateGameOver.cs (Inspector):

```
- Tiempo Muestra GameOver: 2 (segundos antes de mostrar)
- Sonido Derrota: [Elige un clip de audio tipo "game_over"]
```

### En UICombateVictoria.cs (Inspector):

```
- Sonido Victoria: [Elige un clip de audio tipo "victory"]
```

---

## ?? Troubleshooting

### "No aparece la pantalla de derrota"
- Verifica que el Panel está oculto en el Inspector (`Panel GameOver` ? unchecked)
- Verifica que `UICombateGameOver` está en el Canvas
- Revisa la Console para mensajes de error

### "El botón no funciona"
- Verifica que el botón está asignado en el Inspector
- Verifica que la escena se llama "MenuPrincipal" (debe coincidir con lo que está en el script)

### "El juego sigue funcionando cuando muestra GameOver"
- Verifica que `Time.timeScale = 0` en `MostrarGameOver()`
- Verifica que `Time.timeScale = 1f` en botones antes de cargar escenas

### "No se escucha el sonido"
- Asigna un AudioClip válido en Inspector
- Verifica que hay un AudioSource en la escena
- Verifica que el volumen global no está a 0

---

## ? Resultado final

? Cuando pierdes:
- Panel rojo con "¡PERDISTE!"
- Sonido de derrota
- Botones: Reintentar o Menú Principal
- Juego pausado

? Cuando ganas:
- Panel verde con "¡VICTORIA!"
- Muestra XP ganada
- Sonido de victoria
- Botón: Continuar
- Juego pausado

---

## ?? Testing

1. Abre escena de Combate
2. Haz Play
3. Pelea hasta perder (reduce vida del jugador manualmente si quieres probar rápido)
4. Verifica que aparece panel de derrota
5. Prueba botones
6. Repite para victoria

¡Listo! ??
