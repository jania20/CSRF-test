// Configuración del backend
const API_BASE_URL = 'http://localhost:5029/api';

const loginForm = document.getElementById('loginForm');
const errorMessage = document.getElementById('errorMessage');

// Credenciales hardcoded (para validación en frontend)
// En producción, esto debería validarse en el backend
const VALID_USERNAME = 'juan';
const VALID_PASSWORD = '123';

loginForm.addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const username = document.getElementById('username').value.trim();
    const password = document.getElementById('password').value;
    
    // Validar credenciales (validación básica en frontend)
    if (username === VALID_USERNAME && password === VALID_PASSWORD) {
        try {
            // Hacer login en el backend
            // El backend establece la cookie session_user automáticamente
            const response = await fetch(`${API_BASE_URL}/auth/login?user=${encodeURIComponent(username)}`, {
                method: 'GET',
                credentials: 'include' // IMPORTANTE: Incluye cookies en el request
            });

            if (response.ok) {
                // Login exitoso - la cookie ya está establecida por el backend
                // Guardar username en localStorage solo para mostrar en el dashboard
                localStorage.setItem('username', username);
                
                // Redirigir al dashboard
                window.location.href = 'dashboard.html';
            } else {
                // Error en el backend
                showError('Error al conectar con el servidor');
            }
        } catch (error) {
            // Error de red o CORS
            console.error('Error:', error);
            showError('No se pudo conectar con el servidor. ¿Está corriendo el backend?');
        }
    } else {
        // Credenciales incorrectas
        showError('Credenciales incorrectas');
    }
});

function showError(message) {
    errorMessage.textContent = message;
    errorMessage.classList.add('show');
    setTimeout(() => {
        errorMessage.classList.remove('show');
    }, 3000);
}
