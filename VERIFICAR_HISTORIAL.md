# 🔍 Cómo Ver el Historial en el Backend

## ✅ Forma 1: Ver Historial en el Navegador (MÁS FÁCIL)

### Paso a Paso:

1. **Abre tu navegador**
2. **Ve a**: `http://localhost:5029/api/ledger`
3. **Deberías ver** una página HTML con todas las transacciones

**Ejemplo de lo que verás**:
```html
<html><body>
    <h2>Ledger</h2>
    <ul>
        <li>User juan transferred 100 to maria at 2025-01-15T10:30:00Z</li>
        <li>User juan transferred 50 to pedro at 2025-01-15T10:25:00Z</li>
    </ul>
</body></html>
```

---

## ✅ Forma 2: Ver Historial desde Swagger

1. **Abre**: `http://localhost:5029/swagger`
2. **Busca**: `GET /api/ledger`
3. **Click en**: "Try it out"
4. **Click en**: "Execute"
5. **Verás** la respuesta con todas las transacciones

---

## ✅ Forma 3: Ver Historial desde el Frontend

El dashboard debería mostrar las transacciones automáticamente, pero si no aparecen:

1. **Abre el dashboard**: `http://localhost:8000/dashboard.html`
2. **Abre las herramientas de desarrollador** (F12)
3. **Ve a la pestaña Network** (Red)
4. **Busca el request**: `transactions`
5. **Click en él** → **Pestaña "Response"**
6. **Verás** las transacciones en formato JSON

---

## 🔧 Problema: Saldo Muestra $0.00

### Posibles Causas:

#### 1. No hay transacciones aún
- El saldo inicial es $5,000
- Si no has hecho transferencias, debería mostrar $5,000
- Si muestra $0, hay un problema

#### 2. Verificar en el Backend:

**Abre**: `http://localhost:5029/api/balance`

**Deberías ver**:
```json
{
  "balance": 5000.0,
  "currency": "MXN"
}
```

**Si ves esto pero el frontend muestra $0**:
- El problema está en el frontend
- Verifica la consola del navegador (F12) para errores

#### 3. Verificar Request en Network:

1. **Abre el dashboard**
2. **F12** → **Network**
3. **Busca**: `balance`
4. **Click en él** → **Pestaña "Response"**
5. **Verifica** que el JSON tenga `"balance": 5000`

**Si el response está bien pero muestra $0**:
- Problema en `dashboard.js` al mostrar el saldo

---

## 🧪 Prueba Rápida: Hacer una Transferencia

### Desde el Dashboard:

1. **Cantidad**: 100
2. **Cuenta destino**: maria
3. **Click**: "Transferir"
4. **Verifica**:
   - El saldo debería cambiar a $4,900
   - Debería aparecer la transacción en el historial

### Verificar en el Backend:

1. **Abre**: `http://localhost:5029/api/ledger`
2. **Deberías ver**: "User juan transferred 100 to maria at ..."

---

## 📊 Endpoints Útiles para Verificar

| Endpoint | URL | Qué Muestra |
|----------|-----|-------------|
| **Ledger (HTML)** | `http://localhost:5029/api/ledger` | Todas las transacciones en HTML |
| **Balance (JSON)** | `http://localhost:5029/api/balance` | Saldo actual en JSON |
| **Transactions (JSON)** | `http://localhost:5029/api/transactions` | Transacciones en JSON |
| **Swagger** | `http://localhost:5029/swagger` | Documentación de todos los endpoints |

---

## 🐛 Solución de Problemas

### Problema: "No hay transacciones"

**Solución**:
1. Haz una transferencia desde el dashboard
2. O usa el endpoint vulnerable: `http://localhost:5029/api/transfer?amount=100&recipient=test`
3. Luego verifica: `http://localhost:5029/api/ledger`

### Problema: "El saldo muestra $0 pero debería ser $5,000"

**Verifica**:
1. Abre: `http://localhost:5029/api/balance`
2. Si muestra `{"balance": 5000}`, el backend está bien
3. El problema está en el frontend
4. Verifica la consola del navegador (F12) para errores JavaScript

### Problema: "No puedo ver el historial"

**Verifica**:
1. ¿El backend está corriendo? (`http://localhost:5029/swagger` debe funcionar)
2. ¿Estás logueado? (debes tener cookie `session_user`)
3. Prueba: `http://localhost:5029/api/ledger` directamente

---

## ✅ Checklist de Verificación

- [ ] Backend corriendo (`http://localhost:5029/swagger` funciona)
- [ ] Puedo ver: `http://localhost:5029/api/ledger`
- [ ] Puedo ver: `http://localhost:5029/api/balance`
- [ ] El saldo muestra $5,000 (o el correcto después de transferencias)
- [ ] Las transacciones aparecen en el dashboard
- [ ] Puedo hacer transferencias y se guardan

---

**¿Necesitas más ayuda?** Comparte qué ves cuando abres `http://localhost:5029/api/ledger`

