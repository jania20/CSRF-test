# 🔍 Guía de Burp Suite para Demostración en Clase

## 🎯 ¿Qué es Burp Suite?

Burp Suite es una herramienta que **intercepta y muestra** todas las peticiones HTTP que hace tu navegador. Es como un "espía" que te deja ver todo lo que pasa entre el navegador y el servidor.

**Piénsalo así**:
- **Sin Burp**: Navegador → Internet (no ves nada)
- **Con Burp**: Navegador → Burp → Internet (ves TODO)

---

## 📥 Paso 1: Instalar Burp Suite

### Descargar:
1. Ve a: https://portswigger.net/burp/communitydownload
2. Descarga: **"Burp Suite Community Edition"** (es GRATIS)
3. Instala el archivo `.exe`
4. Abre Burp Suite

### Primera Vez:
1. Selecciona: **"Temporary project"**
2. Click: **"Next"**
3. Click: **"Start Burp"**

---

## ⚙️ Paso 2: Configurar Burp Suite

### 1. Verificar Proxy:
1. Ve a la pestaña: **Proxy**
2. Click en: **Options**
3. Verifica que el proxy esté en:
   - **Proxy listener**: `127.0.0.1:8080`
   - **Status**: Running (debe estar en verde)

### 2. Desactivar Intercept (Para Empezar):
1. En la pestaña **Proxy**
2. Asegúrate de que **"Intercept is on"** esté **DESACTIVADO** (gris)
   - Esto permite que los requests pasen automáticamente
   - Solo los verás en el historial, no los interceptarás

---

## 🌐 Paso 3: Configurar el Navegador

### Opción A: Firefox (RECOMENDADO - Más Fácil)

1. **Abre Firefox**
2. **Click en el menú** (☰) → **Configuración**
3. **Busca**: "Proxy" o ve a **Privacidad y seguridad**
4. **Baja hasta**: "Configuración de red" → Click **"Configuración"**
5. **Selecciona**: "Configuración manual de proxy"
6. **Configura**:
   - **Proxy HTTP**: `127.0.0.1`
   - **Puerto**: `8080`
   - **Marca**: "Usar este servidor proxy para todos los protocolos"
7. **Click**: "Aceptar"

### Opción B: Chrome/Edge (Con Extensión)

1. **Instala extensión**: "Proxy SwitchyOmega" o "FoxyProxy"
2. **Configura proxy**: `127.0.0.1:8080`
3. **Activa el proxy**

### Opción C: Chrome/Edge (Manual)

1. **Configuración** → **Sistema** → **Abrir configuración de proxy del equipo**
2. **Configuración manual de proxy**:
   - Servidor: `127.0.0.1`
   - Puerto: `8080`
   - Guarda

---

## 🔐 Paso 4: Instalar Certificado (Para HTTPS)

**Nota**: Para esta demo (HTTP local), NO es necesario, pero te explico por si acaso:

### En Burp Suite:
1. **Proxy** → **Options** → **Import/Export CA Certificate**
2. **Export** → Selecciona formato: **Certificate in DER format**
3. **Guarda** el archivo (ej: `burp-cert.der`)

### En Firefox:
1. **Opciones** → **Privacidad y seguridad** → **Certificados** → **Ver certificados**
2. **Pestaña "Autoridades"** → **Importar**
3. **Selecciona** el archivo `burp-cert.der`
4. **Marca**: "Confiar en esta CA para identificar sitios web"
5. **Aceptar**

### En Chrome/Edge:
1. **Configuración** → **Privacidad y seguridad** → **Seguridad** → **Gestionar certificados**
2. **Pestaña "Autoridades"** → **Importar**
3. **Selecciona** el archivo `burp-cert.der`
4. **Marca**: "Confiar en esta CA"
5. **Aceptar**

---

## 🎬 Paso 5: Demostración en Clase

### Escenario 1: Ver Login Normal

1. **Abre Burp Suite**
2. **Ve a**: **Proxy** → **HTTP history**
3. **Abre el navegador** (con proxy configurado)
4. **Ve a**: `http://localhost:8000/index.html`
5. **En Burp verás**:
   ```
   GET /index.html HTTP/1.1
   GET /assets/css/styles.css HTTP/1.1
   GET /assets/js/login.js HTTP/1.1
   ```

6. **Haz login**: `juan` / `123`
7. **En Burp verás**:
   ```
   GET /api/auth/login?user=juan HTTP/1.1
   Host: localhost:5029
   Origin: http://localhost:8000
   
   Response:
   HTTP/1.1 200 OK
   Set-Cookie: session_user=juan; HttpOnly
   ```

**Explica a la clase**:
- "Aquí vemos el request de login"
- "El servidor establece una cookie: `session_user=juan`"
- "Esta cookie se guarda en el navegador"

---

### Escenario 2: Ver Transferencia Legítima

1. **En el dashboard**: Haz una transferencia
   - Cantidad: 100
   - Destino: maria

2. **En Burp verás**:
   ```
   POST /api/transfer HTTP/1.1
   Host: localhost:5029
   Cookie: session_user=juan
   Content-Type: application/json
   Origin: http://localhost:8000
   Referer: http://localhost:8000/dashboard.html
   
   {"amount": 100, "recipient": "maria"}
   
   Response:
   HTTP/1.1 200 OK
   {"success": true, "message": "Transferencia realizada"}
   ```

**Explica a la clase**:
- "Este es un request legítimo"
- "Viene del mismo dominio: `localhost:8000`"
- "El usuario hizo clic en el botón"
- "Usa POST (más seguro que GET)"

---

### Escenario 3: Ver Ataque CSRF (LA DEMO PRINCIPAL)

1. **Asegúrate de estar logueado** (cookie activa)

2. **Abre nueva pestaña**: `http://localhost:8000/evil.html`

3. **En Burp verás**:
   ```
   GET /evil.html HTTP/1.1
   Host: localhost:8000
   
   GET /api/transfer?amount=5000&recipient=atacante HTTP/1.1
   Host: localhost:5029
   Cookie: session_user=juan
   Referer: http://localhost:8000/evil.html
   Origin: http://localhost:8000
   ```

**Explica a la clase** (PASO A PASO):

1. **"Miren este request"**:
   - Señala: `GET /api/transfer?amount=5000&recipient=atacante`
   - "Este es el ataque CSRF"

2. **"Observen el Referer"**:
   - Señala: `Referer: http://localhost:8000/evil.html`
   - "Este request viene de `evil.html`, NO del banco legítimo"
   - "Es un sitio malicioso"

3. **"Pero miren la Cookie"**:
   - Señala: `Cookie: session_user=juan`
   - "El navegador envió la cookie AUTOMÁTICAMENTE"
   - "El usuario está logueado en el banco"

4. **"Y el método"**:
   - Señala: `GET`
   - "Usa GET, que puede ejecutarse desde una imagen"
   - "El usuario nunca hizo clic en nada"

5. **"El servidor procesó la transferencia"**:
   - Click derecho en el request → **"Send to Repeater"**
   - Ve a **Repeater** → Click **"Send"**
   - Muestra la respuesta: `200 OK`
   - "La transferencia se realizó exitosamente"

6. **"Verifiquemos"**:
   - En el navegador: `http://localhost:5029/api/ledger`
   - Muestra: "User juan transferred 5000 to atacante"
   - "¡La transferencia se hizo sin que el usuario lo supiera!"

---

## 🎤 Guión para la Presentación

### Introducción:
"Voy a demostrar un ataque CSRF usando Burp Suite. Burp Suite nos permite ver todas las peticiones HTTP que hace el navegador."

### Parte 1: Setup Normal
"Primero, el usuario hace login normalmente en el banco."
- Muestra el request de login en Burp
- Explica la cookie

### Parte 2: Uso Normal
"El usuario hace una transferencia legítima."
- Muestra el POST request
- Explica que viene del mismo dominio

### Parte 3: El Ataque
"Ahora viene lo interesante. El usuario visita un sitio malicioso."
- Abre `evil.html`
- Muestra el request automático en Burp
- **Resalta**: Referer diferente, Cookie automática, Método GET

### Parte 4: Verificación
"Verifiquemos que el ataque funcionó."
- Muestra el ledger
- "La transferencia se hizo sin que el usuario lo supiera"

### Conclusión:
"Este es un ataque CSRF. El servidor no validó el origen del request, permitiendo que un sitio malicioso ejecute acciones en nombre del usuario."

---

## 📊 Comparación Visual en Burp

### Request Legítimo:
```
POST /api/transfer HTTP/1.1
Host: localhost:5029
Cookie: session_user=juan
Referer: http://localhost:8000/dashboard.html  ✅ Mismo dominio
Origin: http://localhost:8000                  ✅ Mismo dominio
Method: POST                                   ✅ Más seguro
```

### Request Malicioso:
```
GET /api/transfer?amount=5000&recipient=atacante HTTP/1.1
Host: localhost:5029
Cookie: session_user=juan                      ⚠️ Se envía automáticamente
Referer: http://localhost:8000/evil.html      ⚠️ Diferente dominio
Origin: http://localhost:8000                  ⚠️ Diferente dominio
Method: GET                                    ⚠️ Puede ejecutarse desde <img>
```

**Resalta las diferencias** en la presentación.

---

## 🛠️ Funciones Útiles de Burp para la Demo

### 1. HTTP History (Ver Todo)
- **Proxy** → **HTTP history**
- Muestra todos los requests
- Puedes filtrar por dominio, método, etc.

### 2. Repeater (Repetir Requests)
- Click derecho en un request → **"Send to Repeater"**
- En **Repeater**, puedes modificar y reenviar
- Útil para demostrar diferentes escenarios

### 3. Intercept (Interceptar)
- **Proxy** → Activa **"Intercept is on"**
- Puedes modificar requests antes de enviarlos
- Útil para demostrar cómo un atacante podría modificar requests

### 4. Comparar Requests
- Selecciona dos requests en **HTTP history**
- Compara headers, cookies, etc.
- Útil para mostrar diferencias entre legítimo y malicioso

---

## 💡 Tips para la Presentación

1. **Usa pantalla grande**: Burp puede ser pequeño, usa zoom o pantalla compartida

2. **Resalta con colores**: 
   - Verde: Request legítimo
   - Rojo: Request malicioso

3. **Explica cada header**:
   - Cookie: Autenticación
   - Referer: Origen del request
   - Origin: Dominio origen
   - Method: GET vs POST

4. **Muestra el código**:
   - Abre `evil.html` en un editor
   - Muestra el `<img>` tag malicioso
   - Explica cómo funciona

5. **Hazlo paso a paso**:
   - No corras, explica cada paso
   - Pausa para preguntas

---

## ✅ Checklist Antes de la Presentación

- [ ] Burp Suite instalado y configurado
- [ ] Navegador configurado con proxy
- [ ] Backend corriendo (`http://localhost:5029`)
- [ ] Frontend servido (`http://localhost:8000`)
- [ ] Certificado instalado (si usas HTTPS)
- [ ] Pruebas realizadas (sabes qué esperar)
- [ ] `evil.html` listo
- [ ] Pantalla grande o proyector listo

---

## 🎯 Resumen para la Clase

**Qué mostrar**:
1. ✅ Request de login (cookie establecida)
2. ✅ Request legítimo de transferencia (POST, mismo dominio)
3. ✅ Request malicioso de transferencia (GET, diferente dominio)
4. ✅ Verificación del ataque (ledger)

**Qué explicar**:
- Por qué el ataque funciona (cookie automática, GET vulnerable)
- Cómo prevenirlo (SameSite, tokens CSRF, POST)
- Impacto (transferencias no autorizadas)

---

**¡Con esto tendrás una demo impresionante en clase!** 🎉

