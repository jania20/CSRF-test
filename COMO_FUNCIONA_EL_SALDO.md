# 💰 Cómo Funciona el Saldo y las Transacciones

## 🔍 ¿Por qué el Saldo Muestra $0.00?

### Lo que Pasó:

1. **Saldo inicial**: $5,000.00
2. **Ataque CSRF ejecutado**: Se transfirieron $5,000 al atacante automáticamente
3. **Saldo actual**: $5,000 - $5,000 = **$0.00**

**El ataque CSRF funcionó**: La página maliciosa (`evil.html`) transfirió automáticamente $5,000 sin que tú lo seleccionaras.

---

## 🔄 Cómo Funciona el Sistema

### El Saldo se Calcula Así:

```
Saldo Inicial: $5,000
- Total Transferido: $5,000 (al atacante)
= Saldo Actual: $0.00
```

**El backend NO resetea el saldo cada vez que te logueas**. Las transacciones se guardan en memoria y persisten mientras el backend esté corriendo.

---

## ✅ Cómo Resetear el Saldo a $5,000

### Opción 1: Resetear el Ledger (RECOMENDADO)

**Abre en tu navegador**:
```
http://localhost:5029/api/reset
```

Esto **borra todas las transacciones** y el saldo vuelve a calcularse como $5,000.

**Luego**:
1. Recarga el dashboard: `http://localhost:8000/dashboard.html`
2. El saldo debería mostrar **$5,000.00** de nuevo

---

### Opción 2: Reiniciar el Backend

1. **Ve a la terminal donde corre el backend**
2. **Presiona**: `Ctrl + C` (detiene el servidor)
3. **Ejecuta de nuevo**: `dotnet run`
4. **Las transacciones se borran** (están solo en memoria)
5. **El saldo vuelve a $5,000**

---

## 🎯 Comportamiento Esperado

### Cada Vez que te Logueas:

- **NO se resetea el saldo automáticamente**
- El saldo es: **$5,000 - (total de transferencias)**
- Si ya transferiste $5,000, el saldo será $0
- Si no has transferido nada, el saldo será $5,000

### Para Ver $5,000 de Nuevo:

**Tienes 2 opciones**:

1. **Resetear el ledger**: `http://localhost:5029/api/reset`
2. **Reiniciar el backend**: `Ctrl + C` y `dotnet run` de nuevo

---

## 🧪 Prueba: Verificar que Funciona

### Paso 1: Resetear

1. Abre: `http://localhost:5029/api/reset`
2. Deberías ver: "Ledger reseteado"

### Paso 2: Verificar Saldo

1. Abre: `http://localhost:5029/api/balance`
2. Deberías ver: `{"balance": 5000.0, "currency": "MXN"}`

### Paso 3: Verificar Dashboard

1. Recarga: `http://localhost:8000/dashboard.html`
2. Deberías ver: **$5,000.00**

### Paso 4: Hacer Transferencia Normal

1. Cantidad: 100
2. Destino: maria
3. Click "Transferir"
4. Saldo debería cambiar a: **$4,900.00**

### Paso 5: Verificar Historial

1. Abre: `http://localhost:5029/api/ledger`
2. Deberías ver: "User juan transferred 100 to maria at ..."

---

## 🎬 Para la Demo en Clase

### Escenario Recomendado:

1. **Resetear antes de empezar**:
   - `http://localhost:5029/api/reset`
   - Verifica saldo: $5,000

2. **Mostrar uso normal**:
   - Haz una transferencia legítima: $100
   - Saldo: $4,900
   - Muestra el historial

3. **Mostrar el ataque CSRF**:
   - Abre `evil.html`
   - El ataque transfiere $5,000 automáticamente
   - Saldo: $0 (o negativo si ya había transferido)

4. **Verificar el ataque**:
   - Muestra el historial: `http://localhost:5029/api/ledger`
   - Muestra la transacción maliciosa

5. **Resetear para siguiente demo**:
   - `http://localhost:5029/api/reset`
   - Listo para otra demostración

---

## 📊 Resumen

| Situación | Saldo Mostrado | Por Qué |
|-----------|----------------|----------|
| **Primera vez (sin transacciones)** | $5,000 | Saldo inicial |
| **Después de transferir $100** | $4,900 | $5,000 - $100 |
| **Después del ataque CSRF ($5,000)** | $0 | $5,000 - $5,000 |
| **Después de resetear** | $5,000 | Transacciones borradas |

---

## ⚠️ Importante

- **El saldo NO se resetea automáticamente** al hacer logout/login
- **Las transacciones persisten** mientras el backend esté corriendo
- **Para resetear**: Usa `/api/reset` o reinicia el backend
- **El ataque CSRF funcionó**: Por eso se transfirieron $5,000 automáticamente

---

## 🔧 Solución Rápida

**Para volver a $5,000**:

1. Abre: `http://localhost:5029/api/reset`
2. Recarga el dashboard
3. ¡Listo! Saldo vuelve a $5,000

**No necesitas reiniciar todo**, solo resetear el ledger.

---

¿Todo claro? El ataque CSRF funcionó correctamente: transfirió $5,000 sin que lo seleccionaras. Para resetear, usa `/api/reset`.

