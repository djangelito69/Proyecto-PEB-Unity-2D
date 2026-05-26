# ?? TESTING - Verificar que los crashes están SOLUCIONADOS

## ? Checklist de testing

### Test 1: Stress test (el que revelaba el bug)
- [ ] Abre escena principal (mapa)
- [ ] Play Mode
- [ ] **Haz 10 combates seguidos sin parar**
  - Combate 1: Pelea y gana
  - Vuelve al mapa
  - Combate 2: Pelea y gana
  - ...repetir hasta 10
- [ ] **NO debe crashear** (antes crasheaba en el 4to)
- [ ] **Rendimiento debe ser fluido**
- [ ] **Sin lag acumulativo**

### Test 2: Memory check
- [ ] Window ? Analysis ? Profiler
- [ ] Tab: **Memory**
- [ ] Haz 5 combates
- [ ] Mira el gráfico de memoria (línea verde)
- [ ] ? Debe ser **relativamente plana** (sin crecer indefinidamente)
- [ ] ? Si sube en escalera = hay leak

### Test 3: Listeners en Inspector
- [ ] Pausa el Play Mode (Ctrl+P)
- [ ] Selecciona un botón de ataque en la escena
- [ ] Inspector ? Button ? On Click()
- [ ] ? Debe haber **1 entrada** (OnBasico)
- [ ] ? Si hay 2+, hay listeners duplicados

### Test 4: Ganar y perder múltiples veces
- [ ] 5 victorias seguidas
- [ ] 5 derrotas seguidas (reduce vida del jugador en Debug)
- [ ] ? Paneles deben aparecer correctamente cada vez
- [ ] ? Botones deben responder sin lag

---

## ?? Qué monitorear en Profiler

### Si todo está bien:
```
Memory Usage:
?? Mono: 50-60 MB (estable)
?? GC Alloc: 0 (sin garbage)
?? Total: 100-110 MB (no crece)
```

### Si aún hay leak:
```
Memory Usage:
?? Mono: 50 ? 70 ? 90 ? 110 MB (SUBE)
?? GC Alloc: Picos grandes repetidos
?? Total: CRECE INDEFINIDAMENTE ??
```

---

## ?? Comparación Antes vs Después

### ANTES (Buggy):
```
Combate 1:
?? Listeners: 1
?? Corrutinas: 1
?? Memory: 100 MB
?? Estado: ? OK

Combate 2:
?? Listeners: 2 (ACUMULADO)
?? Corrutinas: 2 (ACUMULADO)
?? Memory: 105 MB
?? Estado: ?? Lag ligero

Combate 3:
?? Listeners: 4
?? Corrutinas: 4
?? Memory: 120 MB
?? Estado: ?? Lag notable

Combate 4:
?? Listeners: 8
?? Corrutinas: 8
?? Memory: 150 MB
?? Estado: ?? CRASH
```

### DESPUÉS (Fixed):
```
Combate 1-10:
?? Listeners: 1 (limpiados cada vez)
?? Corrutinas: 1 (paradas cada vez)
?? Memory: ~100-105 MB (ESTABLE)
?? Estado: ? OK

Combate 11-20:
?? Listeners: 1 (limpiados)
?? Corrutinas: 1 (parados)
?? Memory: ~100-105 MB (ESTABLE)
?? Estado: ? OK

Combate 100+:
?? Listeners: 1 (limpiados)
?? Corrutinas: 1 (parados)
?? Memory: ~100-105 MB (ESTABLE)
?? Estado: ? OK (SIN PROBLEMAS)
```

---

## ?? Pruebas específicas para cada fix

### 1. Fix de UICombate.cs
**Test:** Haz click múltiples veces rapidísimo en un botón de ataque
- ? ANTES: Se ejecutaba la acción múltiples veces (listeners acumulados)
- ? DESPUÉS: Solo se ejecuta 1 vez

### 2. Fix de PlayerImmunity.cs
**Test:** Activa inmunidad múltiples veces manualmente
- ? ANTES: Parpadeo glitchyy o errático
- ? DESPUÉS: Parpadeo suave y consistente

### 3. Fix de GestorDeCombate.cs (Invoke ? Corrutina)
**Test:** Rápidamente gana combates uno tras otro
- ? ANTES: Podría haber descargas de escena duplicadas
- ? DESPUÉS: Limpieza ordenada y correcta

### 4. Fix de GestorCombateGlobal.cs (Cache)
**Test:** Monitorear tiempo de ejecución en Profiler
- ? ANTES: FindGameObjectWithTag() cada combate
- ? DESPUÉS: Cache usado, más rápido

---

## ?? Checklist final

- [ ] **10 combates seguidos sin crash** ? CRÍTICO
- [ ] **Memory estable en Profiler** ? CRÍTICO
- [ ] **Sin lag acumulativo** ? CRÍTICO
- [ ] **Botones responden bien** ? IMPORTANTE
- [ ] **Inmunidad funciona** ? IMPORTANTE
- [ ] **Listeners no se duplican** ? IMPORTANTE

---

## ?? Si algo falla:

**Síntoma 1: Sigue habiendo crash**
- Abre Console (Window ? General ? Console)
- Busca mensajes de error rojo
- Nota la línea exacta del error
- Comparte en GitHub Issues

**Síntoma 2: Memory sigue subiendo**
- Abre Profiler
- Tab: Memory ? Detailed
- Busca qué tipo de memoria sube (Mono vs Managed)
- Es posible que haya otro leak que no detecté

**Síntoma 3: Listeners aún se duplican**
- Pausar Play Mode
- Selecciona botón en escena
- Inspector ? Button ? On Click()
- Si hay múltiples entradas = fix no aplicado correctamente

---

## ? Resultado esperado

Si todo está bien:

```
?? Play ? Combate ? Gana ? Vuelve ? Combate ? ...
?         ?OK      ?OK    ?OK     ?OK     
?
??? Repite 100 veces SIN problemas ??
```

**Memory estable:**
```
Gráfico del Profiler: ???????????? (línea recta, no sube)
```

**Sin lag:**
```
FPS: 60 durante todos los combates
Respuesta de botones: Inmediata
```

---

**¡Si pasas todos estos tests, los crashes están SOLUCIONADOS! ??**

Cuéntame si algo no funciona.
