# 🚀 INSTRUCCIONES URGENTES - Probar la Aplicación

## 📁 Dónde Está tu Proyecto

**Ruta completa**: `C:\Users\980020889\CSRF-demo`

**Cómo encontrarlo en el Explorador de Archivos**:
1. Abre el **Explorador de Archivos** (Windows + E)
2. En la barra de direcciones, escribe: `C:\Users\980020889\CSRF-demo`
3. Presiona Enter
4. **¡Ahí está tu proyecto!**

---

## 🎯 PASO 1: Ejecutar el Backend

### Opción A: Usar el Script (MÁS FÁCIL)

1. **En el Explorador de Archivos**, ve a: `C:\Users\980020889\CSRF-demo`
2. **Busca el archivo**: `iniciar-backend.bat`
3. **Doble clic** en `iniciar-backend.bat`
4. **Espera** a ver: "Now listening on: http://localhost:5029"
5. **⚠️ NO CIERRES esta ventana**

---

### Opción B: Manual (Si el script no funciona)

1. **Abre PowerShell**:
   - Presiona `Windows + X`
   - Selecciona "Windows PowerShell" o "Terminal"

2. **Copia y pega estas líneas UNA POR UNA**:

```powershell
cd "C:\Users\980020889\CSRF-demo\Backend\CSRF API"
```

Presiona Enter

```powershell
dotnet run
```

Presiona Enter

3. **Espera** a ver: "Now listening on: http://localhost:5029"
4. **⚠️ NO CIERRES esta ventana**

---

## 🎯 PASO 2: Servir el Frontend

### Abre OTRA ventana de PowerShell (nueva)

1. **Abre otra PowerShell** (deja la del backend abierta)

2. **Copia y pega estas líneas UNA POR UNA**:

```powershell
cd "C:\Users\980020889\CSRF-demo"
```

Presiona Enter

```powershell
python -m http.server 8000
```

Presiona Enter

**Si aparece error "python no se reconoce"**, prueba:

```powershell
py -m http.server 8000
```

Presiona Enter

**Si tampoco funciona**, prueba con Node.js:

```powershell
npx http-server -p 8000
```

Presiona Enter

3. **Espera** a ver: "Serving HTTP on 0.0.0.0 port 8000"
4. **⚠️ NO CIERRES esta ventana**

---

## 🎯 PASO 3: Abrir la Aplicación

1. **Abre tu navegador** (Chrome, Edge, Firefox)

2. **En la barra de direcciones**, escribe:
   ```
   http://localhost:8000/index.html
   ```
   Presiona Enter

3. **Deberías ver** la página de login del banco

---

## 🎯 PASO 4: Hacer Login

1. **Usuario**: `juan`
2. **Contraseña**: `123`
3. **Haz clic en "Iniciar Sesión"**

4. **Deberías ver el dashboard** con:
   - Saldo: $5,000.00 MXN
   - Formulario de transferencias
   - Historial de transacciones

---

## ✅ Verificación Rápida

### ¿Funciona el Backend?
- Abre: `http://localhost:5029/swagger`
- Deberías ver la documentación de la API

### ¿Funciona el Frontend?
- Abre: `http://localhost:8000/index.html`
- Deberías ver la página de login

### ¿Están conectados?
- Haz login en el frontend
- Si ves el dashboard con datos, **¡están conectados!**

---

## 🐛 Problemas Comunes

### Error: "dotnet no se reconoce"
**Solución**: Instala .NET SDK desde: https://dotnet.microsoft.com/download

### Error: "python no se reconoce"
**Solución**: 
- Prueba: `py -m http.server 8000`
- O instala Python desde: https://www.python.org/downloads/

### Error: "No se pudo conectar con el servidor"
**Solución**: 
- Verifica que el backend esté corriendo (Paso 1)
- Debe decir: "Now listening on: http://localhost:5029"

### La página está en blanco
**Solución**: 
- Verifica que el frontend esté servido (Paso 2)
- Abre la consola del navegador (F12) para ver errores

---

## 📋 Checklist Final

- [ ] Backend corriendo (Terminal 1 con "Now listening on: http://localhost:5029")
- [ ] Frontend servido (Terminal 2 con "Serving HTTP on 0.0.0.0 port 8000")
- [ ] Navegador abierto en `http://localhost:8000/index.html`
- [ ] Puedo hacer login con `juan` / `123`
- [ ] Veo el dashboard con saldo y formulario
- [ ] Puedo hacer una transferencia
- [ ] La transferencia se guarda

---

## 🎬 Para la Demo CSRF

1. **Haz login** normalmente (Paso 4)
2. **Abre nueva pestaña**: `http://localhost:8000/evil.html`
3. **Vuelve al dashboard**: `http://localhost:8000/dashboard.html`
4. **Verifica transacciones**: Deberías ver la transferencia maliciosa

---

**¡Listo! Si sigues estos pasos, debería funcionar.**

