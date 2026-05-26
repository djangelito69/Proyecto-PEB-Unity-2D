# ?? RESUMEN RÁPIDO - Qué hacer en Unity

## Lo que ya está hecho (en los scripts):

? **UICombateGameOver.cs** - Script que muestra pantalla de derrota  
? **UICombateVictoria.cs** - Script que muestra pantalla de victoria  
? **GestorDeCombate.cs** - Modificado y listo  

---

## Lo que DEBES HACER en Unity (Paso a paso visual):

### 1?? ESCENA DE COMBATE - Estructura

Abre tu escena **"Combate"** y asegúrate de tener esto en la Jerarquía:

```
Canvas (Main)
??? Panel de Combate (actual UI del combate)
??? PanelGameOver (NUEVO - lo creamos ahora)
?   ??? TextoTitulo (TextMeshProUGUI)
?   ??? TextoMensaje (TextMeshProUGUI)
?   ??? TextoNivel (TextMeshProUGUI)
?   ??? TextoVida (TextMeshProUGUI)
?   ??? BotonReintentar (Button)
?   ??? BotonMenuPrincipal (Button)
??? PanelVictoria (NUEVO)
    ??? TextoTitulo (TextMeshProUGUI)
    ??? TextoMensaje (TextMeshProUGUI)
    ??? TextoExpGanada (TextMeshProUGUI)
    ??? TextoNivel (TextMeshProUGUI)
    ??? BotonContinuar (Button)
```

### 2?? CREAR PANELGAMEOVER

En el Canvas:
1. Botón derecho ? **UI ? Panel** 
2. Llama al panel: **`PanelGameOver`**
3. En Inspector:
   - Anchor Presets: Stretch
   - Color: Negro semitransparente (A=200)

### 3?? DENTRO DE PANELGAMEOVER - Crear textos y botones

**Crea dentro del panel (drag and drop como hijos):**

a) **TextMeshProUGUI** para título
   - Nombre: `TextoTitulo`
   - Tamaño: 60pt
   - Posición: Top, Y=0
   - Contenido: "¡PERDISTE!"
   - Color: Rojo

b) **TextMeshProUGUI** para mensaje
   - Nombre: `TextoMensaje`
   - Tamaño: 30pt
   - Contenido: "El enemigo te derrotó"

c) **TextMeshProUGUI** para nivel (opcional)
   - Nombre: `TextoNivel`
   - Tamaño: 20pt

d) **TextMeshProUGUI** para vida (opcional)
   - Nombre: `TextoVida`
   - Tamaño: 20pt

e) **Button** verde
   - Nombre: `BotonReintentar`
   - Posición: Abajo izquierda
   - Texto: "Reintentar"

f) **Button** gris
   - Nombre: `BotonMenuPrincipal`
   - Posición: Abajo derecha
   - Texto: "Menú Principal"

### 4?? REPETIR PARA PANELVICTORIA

Igual que arriba pero:
- Panel verde
- Título: "¡VICTORIA!"
- Solo 1 botón: "Continuar" (verde)
- Añade un TextMeshProUGUI para XP ganada: `TextoExpGanada`

### 5?? AÑADIR SCRIPT A PANELGAMEOVER

Selecciona `PanelGameOver` en la Jerarquía:

1. Inspector ? **Add Component**
2. Busca: `UICombateGameOver`
3. Selecciona el script
4. Se abrirán campos vacíos para asignar referencias

**Arrastra los elementos:**
- Panel GameOver: `PanelGameOver`
- Texto Título: `TextoTitulo` (dentro del panel)
- Texto Mensaje: `TextoMensaje`
- Texto Nivel: `TextoNivel`
- Texto Vida: `TextoVida`
- Botón Reintentar: `BotonReintentar`
- Botón Menu Principal: `BotonMenuPrincipal`
- Audio Source: Crea uno nuevo (Add Component ? AudioSource)
- Sonido Derrota: Arrastra un AudioClip

### 6?? AÑADIR SCRIPT A PANELVICTORIA

Selecciona `PanelVictoria` en la Jerarquía:

1. Inspector ? **Add Component**
2. Busca: `UICombateVictoria`
3. Selecciona el script

**Arrastra los elementos:**
- Panel Victoria: `PanelVictoria`
- Texto Título: `TextoTitulo` (del panel victoria)
- Texto Mensaje: `TextoMensaje`
- Texto Exp Ganada: `TextoExpGanada`
- Texto Nivel: `TextoNivel`
- Botón Continuar: `BotonContinuar`
- Audio Source: Arrastra un AudioSource o crea uno nuevo
- Sonido Victoria: Arrastra un AudioClip

### 7?? PROBAR

1. Abre escena de Combate
2. Play
3. Pelea contra enemigo
4. Pierde a propósito (reduce tu vida en Debug)
5. ? Debe aparecer panel rojo "¡PERDISTE!"
6. Click en botones ? funcionen
7. Click en Reintentar ? recarga el combate
8. Click en Menú Principal ? va a menú principal

¡Listo! ??

---

## ?? Nota importante sobre AudioSource

Si en el paso 5 no tienes un AudioSource:
1. En el Canvas (o en el GameObject con UICombateGameOver)
2. Add Component ? **AudioSource**
3. Deja los valores por defecto
4. No marquees "Play On Awake"

Igual para `UICombateVictoria`.

---

## ?? Colores recomendados

**Para PanelGameOver:**
- Color del panel: Negro (0, 0, 0) Alpha: 200
- Texto título: Rojo (255, 0, 0)
- Texto mensaje: Blanco (255, 255, 255)

**Para PanelVictoria:**
- Color del panel: Negro (0, 0, 0) Alpha: 200
- Texto título: Verde (0, 255, 0)
- Texto mensaje: Blanco

---

## ? Si todo está bien configurado:

Cuando pierdes:
- ?? Panel rojo oscuro llena pantalla
- "¡PERDISTE!" en grande rojo
- Mensaje personalizado
- 2 botones funcionales
- Sonido de derrota
- Juego pausado

Cuando ganas:
- ?? Panel verde oscuro llena pantalla
- "¡VICTORIA!" en grande verde
- Muestra XP ganada
- 1 botón para continuar
- Sonido de victoria
- Juego pausado

**¡Así de simple! ??**
