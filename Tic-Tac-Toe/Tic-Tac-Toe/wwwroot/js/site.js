// Функция для выхода из аккаунта
function logout() {
    // Очищаем данные авторизации
    localStorage.removeItem('authCredentials');
    localStorage.removeItem('userId');
    
    // Перенаправляем на страницу входа
    window.location.href = '/Login';
}

// Функция для получения логина из credentials
function getLoginFromCredentials() {
    const credentials = localStorage.getItem('authCredentials');
    if (!credentials) {
        return null;
    }
    
    try {
        // Декодируем base64
        const decoded = atob(credentials);
        // Извлекаем логин (до двоеточия)
        const login = decoded.split(':')[0];
        return login;
    } catch (e) {
        console.error('Error decoding credentials:', e);
        return null;
    }
}

// Обновление навигации в зависимости от статуса авторизации
function updateNavigation() {
    const authCredentials = localStorage.getItem('authCredentials');
    const userInfo = document.getElementById('userInfo');
    const loginLink = document.getElementById('loginLink');
    const userLogin = document.getElementById('userLogin');
    
    if (authCredentials) {
        // Пользователь авторизован
        const login = getLoginFromCredentials();
        if (login) {
            userLogin.textContent = login;
        }
        if (userInfo) {
            userInfo.style.display = 'block';
        }
        if (loginLink) {
            loginLink.style.display = 'none';
        }
    } else {
        // Пользователь не авторизован
        if (userInfo) {
            userInfo.style.display = 'none';
        }
        if (loginLink) {
            loginLink.style.display = 'block';
        }
    }
}

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', function() {
    updateNavigation();
    
    // Обработчик кнопки выхода
    const logoutBtn = document.getElementById('logoutBtn');
    if (logoutBtn) {
        logoutBtn.addEventListener('click', function(e) {
            e.preventDefault();
            logout();
        });
    }
});

// Экспортируем функции для использования в других скриптах
window.logout = logout;
window.updateNavigation = updateNavigation;
