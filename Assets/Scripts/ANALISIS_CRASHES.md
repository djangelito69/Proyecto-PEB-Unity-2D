# ?? PROBLEMAS IDENTIFICADOS - Crashes después de 3-4 combates

## ?? Bugs encontrados

### 1. **UICombate.cs - Listeners acumulándose (CRÍTICO)**
**Líneas 37-38:**
```csharp
botonAtaqueBasico.onClick.AddListener(OnBasico);
botonAtaqueEspecial.onClick.AddListener(OnEspecial);
```

**Problema:** 
- Se añaden listeners pero NUNCA se removem
- Cada vez que carga la escena de combate, se añaden más listeners
- Después de 3-4 combates: 3-4 listeners ejecutándose cada click
- Causa: Memory leak + comportamiento impredecible
- Resultado: Crash por memoria o comportamiento extraño

**Solución:** Remover listeners en OnDestroy()

---

### 2. **UICombate.cs - Corrutinas sin limpieza (PlayerImmunity)**
**En PlayerImmunity.cs línea 52:**
```csharp
corrutinaPistolaje = StartCoroutine(ParpadearDuranteInmunidad());
```

**Problema:**
- Si el jugador recibe inmunidad múltiples veces, se acumulan corrutinas
- No hay StopCoroutine() si se activa nuevamente
- Línea 57 también inicia corrutina pero sin guardar referencia

**Solución:** StopCoroutine antes de iniciar una nueva

---

### 3. **UICombateGameOver/Victoria - Listeners no removidos**
**En ambos scripts, Start():**
```csharp
botonReintentar.onClick.AddListener(Reintentar);
botonMenuPrincipal.onClick.AddListener(IrAlMenuPrincipal);
```

**Problema:**
- Se añaden listeners pero no se removem
- OnDestroy() solo remueve el evento OnMensajeCombate, no los botones

**Solución:** Remover listeners onClick en OnDestroy()

---

### 4. **GestorDeCombate.cs - Invoke sin control**
**Líneas 224 y 246:**
```csharp
Invoke("VolverAlMapa", 1.5f);
```

**Problema:**
- Si se llama múltiples veces, se acumulan invokes
- Aunque aquí es difícil que pase, es una mala práctica
- Mejor: Corrutina con control

**Solución:** Usar corrutina en lugar de Invoke

---

### 5. **FindGameObjectWithTag - Repetido múltiples veces**
**En GestorCombateGlobal.cs línea 51 y otros:**
```csharp
GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerImmunity>();
```

**Problema:**
- Se ejecuta cada vez que inicia combate
- Es una búsqueda lenta si hay muchos objetos
- Mejor cachedarlo

**Solución:** Cache la referencia del jugador

---

## ? Impacto de estos bugs

| Bug | Severidad | Efecto |
|-----|-----------|--------|
| Listeners acumulados | ?? CRÍTICO | Crash por memoria |
| Corrutinas duplicadas | ?? ALTO | Comportamiento extraño + memoria |
| onClick sin remover | ?? ALTO | Listeners duplicados cada combate |
| Invoke duplicado | ?? MEDIO | Posibles descargas múltiples |
| FindGameObjectWithTag | ?? MEDIO | Pérdida de rendimiento |

---

## ?? Secuencia de crash típica

```
Combate 1: Inicia UI
  ?? 1 listener en boton ataque básico
  ?? 1 corrutina de inmunidad
  ?? OK ?

Combate 2: Carga escena de combate
  ?? UICombate.Start() ? Añade 2 listeners más (ahora 3 total)
  ?? Inmunidad se activa ? corrutina duplicada
  ?? Empieza lag

Combate 3:
  ?? 5 listeners acumulados
  ?? 3 corrutinas paralelas
  ?? Lag notable

Combate 4:
  ?? 9 listeners acumulados
  ?? 5 corrutinas paralelas
  ?? Memory allocation crítica
  ?? ?? CRASH
```

---

## ?? Fixes a aplicar

1. **UICombate.cs** ? Remover listeners en OnDestroy()
2. **PlayerImmunity.cs** ? StopCoroutine antes de iniciar
3. **UICombateGameOver.cs** ? Remover listeners onClick
4. **UICombateVictoria.cs** ? Remover listeners onClick
5. **GestorDeCombate.cs** ? Usar corrutina en lugar de Invoke
6. **GestorCombateGlobal.cs** ? Cache referencia del jugador

---

**Siguiente paso:** Aplicaré todos estos fixes. ??**
