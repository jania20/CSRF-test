# 📁 Explicación de Cada Archivo del Proyecto

## 🎯 Resumen General

Este proyecto tiene **2 branches principales**:
- **`develop`**: Backend (API REST en C#)
- **`frontend-banco-seguro`**: Frontend (Página web HTML/JS/CSS)

---

## 🌿 BRANCH: `develop` (Backend)

### 📂 Estructura de Carpetas

```
Backend/
└── CSRF API/
    ├── Program.cs                    # Configuración principal del servidor
    ├── CSRF API.csproj              # Archivo de proyecto .NET
    ├── Controllers/                 # Controladores (endpoints de la API)
    │   ├── AuthController.cs       # Login y Logout
    │   └── TransferController.cs   # Transferencias (VULNERABLE)
    ├── Services/                    # Servicios de negocio
    │   └── LedgerService.cs        # Almacena transacciones
    └── appsettings.json            # Configuración de la aplicación
```

---

### 📄 Archivos del Backend

#### 1. `Program.cs`
**¿Qué hace?**
- Configura y arranca el servidor web
- Configura CORS (permite que el frontend se conecte)
- Registra los controladores y servicios
- El servidor corre en: `http://localhost:5029`

**Líneas importantes**:
```csharp
// Configura CORS para permitir requests del frontend
builder.Services.AddCors(...)

// El servidor escucha en el puerto 5029
app.Run();
```

**¿Para qué sirve?**
- Sin este archivo, el servidor no arranca
- Es el "corazón" del backend

---

#### 2. `Controllers/AuthController.cs`
**¿Qué hace?**
- Maneja el login y logout de usuarios
- Establece cookies de sesión

**Endpoints**:
- `GET /api/auth/login?user=nombre`
  - Hace login del usuario
  - Establece cookie `session_user`
  - **VULNERABILIDAD**: Cookie sin `SameSite`

- `GET /api/auth/logout`
  - Cierra sesión
  - Elimina la cookie

**¿Para qué sirve?**
- Autenticación de usuarios
- Sin esto, no puedes hacer login

---

#### 3. `Controllers/TransferController.cs`
**¿Qué hace?**
- Maneja las transferencias bancarias
- Almacena transacciones en el ledger

**Endpoints**:

1. **`GET /api/transfer?amount=XXX&recipient=YYY`** ⚠️ **VULNERABLE**
   - Hace transferencia usando GET
   - **VULNERABILIDAD CSRF**: Puede ejecutarse desde `<img>` tags
   - Este es el endpoint que el atacante explota

2. **`POST /api/transfer`** (Más seguro, pero aún vulnerable)
   - Hace transferencia usando POST
   - Recibe JSON: `{"amount": 100, "recipient": "maria"}`
   - Usado por el frontend legítimo

3. **`GET /api/balance`**
   - Retorna el saldo del usuario
   - Calculado desde las transacciones

4. **`GET /api/transactions`**
   - Retorna todas las transacciones del usuario
   - En formato JSON para el frontend

5. **`GET /api/ledger`**
   - Muestra todas las transacciones (HTML)
   - Útil para verificar el ataque

6. **`GET /api/reset`**
   - Limpia todas las transacciones
   - Útil para pruebas

**¿Para qué sirve?**
- Procesa las transferencias
- Almacena el historial
- **Contiene la vulnerabilidad CSRF**

---

#### 4. `Services/LedgerService.cs`
**¿Qué hace?**
- Almacena las transacciones en memoria
- Usa una cola thread-safe (`ConcurrentQueue`)

**Métodos**:
- `Add(entry)`: Agrega una transacción
- `GetAll()`: Obtiene todas las transacciones
- `Reset()`: Limpia todas las transacciones

**¿Para qué sirve?**
- Base de datos temporal (en memoria)
- Los datos se pierden al reiniciar el servidor

---

#### 5. `CSRF API.csproj`
**¿Qué hace?**
- Define el proyecto .NET
- Especifica dependencias (Swagger, etc.)

**¿Para qué sirve?**
- .NET necesita este archivo para compilar el proyecto

---

#### 6. `appsettings.json`
**¿Qué hace?**
- Configuración de la aplicación
- Niveles de logging

**¿Para qué sirve?**
- Configuración general del servidor

---

## 🎨 BRANCH: `frontend-banco-seguro` (Frontend)

### 📂 Estructura de Carpetas

```
├── index.html              # Página de login
├── dashboard.html          # Panel de control
└── assets/
    ├── css/
    │   └── styles.css      # Estilos de la aplicación
    └── js/
        ├── login.js        # Lógica de login
        └── dashboard.js     # Lógica del dashboard
```

---

### 📄 Archivos del Frontend

#### 1. `index.html`
**¿Qué hace?**
- Página de inicio de sesión
- Muestra formulario de usuario y contraseña
- Diseño moderno con tema oscuro

**Elementos principales**:
- Formulario con ID `loginForm`
- Campos: `username` y `password`
- Botón "Iniciar Sesión"
- Mensaje de error (oculto por defecto)

**¿Para qué sirve?**
- Primera página que ve el usuario
- Permite autenticarse

---

#### 2. `dashboard.html`
**¿Qué hace?**
- Panel de control del banco
- Muestra saldo, formulario de transferencias, historial

**Elementos principales**:
- Header con logo y botón de logout
- Tarjeta de saldo
- Formulario de transferencias (`transferForm`)
- Lista de transacciones

**¿Para qué sirve?**
- Interfaz principal después del login
- Permite hacer transferencias

---

#### 3. `assets/css/styles.css`
**¿Qué hace?**
- Define todos los estilos visuales
- Tema oscuro con colores dorados
- Diseño responsive (se adapta a móviles)

**Características**:
- Variables CSS para colores
- Efectos de hover y transiciones
- Backdrop blur (efecto de vidrio)

**¿Para qué sirve?**
- Hace que la aplicación se vea profesional
- Sin esto, sería solo texto sin estilo

---

#### 4. `assets/js/login.js`
**¿Qué hace?**
- Maneja el proceso de login
- Se conecta al backend para autenticar

**Flujo**:
1. Usuario ingresa credenciales
2. Valida en frontend (usuario: `juan`, contraseña: `123`)
3. Hace request al backend: `GET /api/auth/login?user=juan`
4. Backend establece cookie
5. Redirige a `dashboard.html`

**Código clave**:
```javascript
const API_BASE_URL = 'http://localhost:5029/api';

fetch(`${API_BASE_URL}/auth/login?user=${username}`, {
    credentials: 'include' // IMPORTANTE: Incluye cookies
});
```

**¿Para qué sirve?**
- Conecta el frontend con el backend
- Maneja la autenticación

---

#### 5. `assets/js/dashboard.js`
**¿Qué hace?**
- Maneja toda la lógica del dashboard
- Obtiene datos del backend
- Procesa transferencias

**Funciones principales**:

1. **`checkSession()`**
   - Verifica si el usuario está logueado
   - Hace request a `/api/balance`
   - Si no hay cookie, redirige al login

2. **`loadBalance()`**
   - Obtiene el saldo del backend
   - Muestra en la página

3. **`loadTransactions()`**
   - Obtiene transacciones del backend
   - Muestra las últimas 3

4. **Manejo de transferencias**
   - Cuando el usuario hace una transferencia
   - Hace `POST /api/transfer` con los datos
   - Actualiza la página

**Código clave**:
```javascript
// Obtener saldo
fetch(`${API_BASE_URL}/balance`, {
    credentials: 'include'
});

// Hacer transferencia
fetch(`${API_BASE_URL}/transfer`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    credentials: 'include',
    body: JSON.stringify({ amount: 100, recipient: 'maria' })
});
```

**¿Para qué sirve?**
- Conecta el dashboard con el backend
- Muestra datos reales
- Procesa transferencias

---

## 📄 Archivos Adicionales (Raíz del Proyecto)

#### `evil.html`
**¿Qué hace?**
- Página maliciosa que explota la vulnerabilidad CSRF
- Simula un sitio de ofertas/regalos
- Contiene código que ejecuta transferencias automáticamente

**Código malicioso**:
```html
<img src="http://localhost:5029/api/transfer?amount=5000&recipient=atacante" 
     style="display:none">
```

**¿Para qué sirve?**
- Demuestra el ataque CSRF
- Muestra cómo un sitio malicioso puede explotar la vulnerabilidad

---

#### `iniciar-backend.bat`
**¿Qué hace?**
- Script para Windows que ejecuta el backend
- Verifica que .NET esté instalado
- Ejecuta `dotnet run`

**¿Para qué sirve?**
- Facilita ejecutar el backend
- Doble clic y listo

---

#### `iniciar-frontend.bat`
**¿Qué hace?**
- Script para Windows que sirve el frontend
- Verifica que Python esté instalado
- Ejecuta `python -m http.server 8000`

**¿Para qué sirve?**
- Facilita servir el frontend
- Doble clic y listo

---

#### `INSTRUCCIONES_URGENTES.md`
**¿Qué hace?**
- Guía paso a paso para ejecutar la aplicación
- Instrucciones claras y rápidas

**¿Para qué sirve?**
- Documentación de cómo usar el proyecto

---

## 🔗 Cómo se Conectan los Archivos

### Flujo de Login:
```
index.html
  ↓ (carga)
login.js
  ↓ (hace request)
Backend: AuthController.cs
  ↓ (establece cookie)
Navegador guarda cookie
  ↓ (redirige)
dashboard.html
```

### Flujo de Transferencia:
```
dashboard.html
  ↓ (usuario hace clic en "Transferir")
dashboard.js
  ↓ (hace POST request)
Backend: TransferController.cs
  ↓ (procesa)
LedgerService.cs (almacena)
  ↓ (retorna éxito)
dashboard.js (actualiza página)
```

### Flujo de Ataque CSRF:
```
evil.html (página maliciosa)
  ↓ (carga automáticamente)
<img> tag hace GET request
  ↓ (navegador envía cookie automáticamente)
Backend: TransferController.cs
  ↓ (procesa sin validar origen)
Transferencia realizada
```

---

## ✅ Cómo Verificar que Está Conectado

### 1. Backend Funcionando:
- Abre: `http://localhost:5029/swagger`
- Deberías ver la documentación de la API (como en tu imagen)
- ✅ **Si ves Swagger, el backend está funcionando**

### 2. Frontend Funcionando:
- Abre: `http://localhost:8000/index.html`
- Deberías ver la página de login
- ✅ **Si ves el login, el frontend está funcionando**

### 3. Están Conectados:
- Haz login: `juan` / `123`
- Si ves el dashboard con saldo y datos
- ✅ **Si ves datos, están conectados**

### 4. Verificar en el Navegador (F12):
1. Abre las **Herramientas de Desarrollador** (F12)
2. Ve a la pestaña **Network** (Red)
3. Haz login
4. Deberías ver requests a:
   - `http://localhost:5029/api/auth/login`
   - `http://localhost:5029/api/balance`
   - `http://localhost:5029/api/transactions`
5. ✅ **Si ves estos requests, están conectados**

---

## 📊 Resumen por Branch

| Branch | Archivos Principales | Función |
|--------|---------------------|---------|
| `develop` | `Program.cs`, `AuthController.cs`, `TransferController.cs` | Backend - API REST |
| `frontend-banco-seguro` | `index.html`, `dashboard.html`, `login.js`, `dashboard.js` | Frontend - Interfaz web |

---

¿Todo claro? ¿Quieres que explique algún archivo específico con más detalle?

