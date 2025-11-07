// Configuración del backend
const API_BASE_URL = 'http://localhost:5029/api';

// Verificar si hay sesión activa (verificando con el backend)
async function checkSession() {
    try {
        const response = await fetch(`${API_BASE_URL}/balance`, {
            method: 'GET',
            credentials: 'include' // Incluye cookies
        });

        if (!response.ok) {
            // No hay sesión activa, redirigir al login
            window.location.href = 'index.html';
            return false;
        }
        return true;
    } catch (error) {
        console.error('Error verificando sesión:', error);
        // Si no se puede conectar, asumir que no hay sesión
        window.location.href = 'index.html';
        return false;
    }
}

// Obtener nombre de usuario (del localStorage o del backend)
const username = localStorage.getItem('username') || 'Usuario';
document.getElementById('greeting').textContent = `Hola, ${username.charAt(0).toUpperCase() + username.slice(1)} Confiado`;

// Función para formatear número como moneda
function formatCurrency(amount) {
    return new Intl.NumberFormat('es-MX', {
        style: 'currency',
        currency: 'MXN',
        minimumFractionDigits: 2
    }).format(amount);
}

// Función para formatear fecha
function formatDate(dateString) {
    try {
        const date = new Date(dateString);
        return new Intl.DateTimeFormat('es-MX', {
            year: 'numeric',
            month: 'short',
            day: 'numeric'
        }).format(date);
    } catch (e) {
        return dateString; // Si no se puede formatear, retornar original
    }
}

// Cargar y mostrar saldo desde el backend
async function loadBalance() {
    try {
        const response = await fetch(`${API_BASE_URL}/balance`, {
            method: 'GET',
            credentials: 'include'
        });

        if (response.ok) {
            const data = await response.json();
            document.getElementById('balanceAmount').textContent = formatCurrency(data.balance);
        } else {
            console.error('Error cargando saldo');
            document.getElementById('balanceAmount').textContent = formatCurrency(0);
        }
    } catch (error) {
        console.error('Error:', error);
        document.getElementById('balanceAmount').textContent = formatCurrency(0);
    }
}

// Cargar y mostrar transacciones desde el backend
async function loadTransactions() {
    try {
        const response = await fetch(`${API_BASE_URL}/transactions`, {
            method: 'GET',
            credentials: 'include'
        });

        const transactionsList = document.getElementById('transactionsList');

        if (response.ok) {
            const transactions = await response.json();
            
            // Mostrar solo las últimas 3
            const lastThree = transactions.slice(0, 3);
            
            if (lastThree.length === 0) {
                transactionsList.innerHTML = '<li class="transaction-item"><span class="transaction-date" style="color: #888888;">No hay transacciones</span></li>';
            } else {
                transactionsList.innerHTML = lastThree.map(transaction => {
                    const isPositive = transaction.amount > 0;
                    const amountClass = isPositive ? 'positive' : 'negative';
                    const amountSign = isPositive ? '+' : '';
                    
                    return `
                        <li class="transaction-item">
                            <span class="transaction-date">${formatDate(transaction.date)}</span>
                            <span class="transaction-concept">Transferencia a ${transaction.recipient}</span>
                            <span class="transaction-amount ${amountClass}">${amountSign}${formatCurrency(Math.abs(transaction.amount))}</span>
                        </li>
                    `;
                }).join('');
            }
        } else {
            transactionsList.innerHTML = '<li class="transaction-item"><span class="transaction-date" style="color: #888888;">No hay transacciones</span></li>';
        }
    } catch (error) {
        console.error('Error cargando transacciones:', error);
        const transactionsList = document.getElementById('transactionsList');
        transactionsList.innerHTML = '<li class="transaction-item"><span class="transaction-date" style="color: #888888;">Error cargando transacciones</span></li>';
    }
}

// Manejar formulario de transferencia
const transferForm = document.getElementById('transferForm');
const successAlert = document.getElementById('successAlert');

transferForm.addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const amount = parseFloat(document.getElementById('amount').value);
    const destination = document.getElementById('destination').value.trim();
    
    // Validaciones básicas
    if (!amount || amount <= 0) {
        alert('Ingresa un monto válido');
        return;
    }
    
    if (!destination) {
        alert('Ingresa una cuenta destino');
        return;
    }
    
    try {
        // Hacer transferencia usando POST (más seguro que GET)
        const response = await fetch(`${API_BASE_URL}/transfer`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include', // Incluye cookies
            body: JSON.stringify({
                amount: amount,
                recipient: destination
            })
        });

        if (response.ok) {
            const data = await response.json();
            
            // Actualizar UI
            await loadBalance();
            await loadTransactions();
            
            // Mostrar alerta de éxito
            successAlert.classList.add('show');
            setTimeout(() => {
                successAlert.classList.remove('show');
            }, 3000);
            
            // Limpiar formulario
            transferForm.reset();
        } else {
            const errorData = await response.json();
            alert(errorData.error || 'Error al realizar la transferencia');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('No se pudo conectar con el servidor. ¿Está corriendo el backend?');
    }
});

// Manejar cierre de sesión
document.getElementById('logoutBtn').addEventListener('click', async function() {
    try {
        // Llamar al endpoint de logout del backend
        await fetch(`${API_BASE_URL}/auth/logout`, {
            method: 'GET',
            credentials: 'include'
        });
    } catch (error) {
        console.error('Error en logout:', error);
    }
    
    // Limpiar localStorage
    localStorage.removeItem('username');
    
    // Redirigir al login
    window.location.href = 'index.html';
});

// Inicializar dashboard
async function init() {
    // Verificar sesión primero
    const hasSession = await checkSession();
    if (hasSession) {
        // Cargar datos
        await loadBalance();
        await loadTransactions();
    }
}

// Ejecutar al cargar la página
init();
