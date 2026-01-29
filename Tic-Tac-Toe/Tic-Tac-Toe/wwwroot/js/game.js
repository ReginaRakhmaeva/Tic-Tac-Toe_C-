let currentGameId = null;
let gameBoard = [[0, 0, 0], [0, 0, 0], [0, 0, 0]];
let isPlayerTurn = true;
let gameFinished = false;

async function initializeGame(computerFirst) {
    currentGameId = generateUUID();
    gameBoard = [[0, 0, 0], [0, 0, 0], [0, 0, 0]];
    // Если компьютер ходит первым, блокируем ходы игрока до получения ответа
    isPlayerTurn = !computerFirst;
    gameFinished = false;
    
    document.getElementById('gameBoard').style.display = 'inline-block';
    document.getElementById('gameId').textContent = 'ID игры: ' + currentGameId;
    document.getElementById('gameId').style.display = 'block';
    
    if (computerFirst) {
        document.getElementById('gameStatus').textContent = 'Ход компьютера...';
    } else {
        document.getElementById('gameStatus').textContent = 'Ваш ход (X)';
    }
    
    document.getElementById('errorMessage').style.display = 'none';
    
    clearBoard();
    
    try {
        const firstMoveParam = computerFirst ? 'computer' : 'player';
        const headers = {
            'Content-Type': 'application/json'
        };
        
        // Добавляем заголовок Authorization, если есть сохраненные credentials
        const authCredentials = localStorage.getItem('authCredentials');
        if (authCredentials) {
            headers['Authorization'] = 'Basic ' + authCredentials;
        }
        
        const response = await fetch(`/game/${currentGameId}?firstMove=${firstMoveParam}`, {
            method: 'GET',
            headers: headers
        });
        
        if (!response.ok) {
            if (response.status === 401) {
                localStorage.removeItem('authCredentials');
                localStorage.removeItem('userId');
                if (typeof updateNavigation === 'function') {
                    updateNavigation();
                }
                window.location.href = '/Login';
                return;
            }
            throw new Error('Ошибка при инициализации игры');
        }
        
        const gameResponse = await response.json();
        
        if (gameResponse && gameResponse.board && gameResponse.board.board) {
            gameBoard = gameResponse.board.board;
            updateBoard();
            
            handleGameStatus(gameResponse.status);
            
            // После получения ответа от сервера (компьютер уже сходил, если нужно)
            // разрешаем ход игрока только если игра не закончена
            if (!gameFinished) {
                isPlayerTurn = true;
                document.getElementById('gameStatus').textContent = 'Ваш ход (X)';
            }
        }
    } catch (error) {
        console.error('Ошибка при инициализации игры:', error);
        document.getElementById('errorMessage').textContent = 'Ошибка: ' + error.message;
        document.getElementById('errorMessage').style.display = 'block';
        // При ошибке разрешаем ход игрока, если игра не была начата с компьютером
        if (!computerFirst) {
            isPlayerTurn = true;
            document.getElementById('gameStatus').textContent = 'Ваш ход (X)';
        } else {
            isPlayerTurn = false;
            document.getElementById('gameStatus').textContent = 'Ошибка инициализации';
        }
    }
}

async function makeMove() {
    try {
        document.getElementById('gameStatus').textContent = 'Ход компьютера...';
        document.getElementById('errorMessage').style.display = 'none';
        
        const headers = {
            'Content-Type': 'application/json'
        };
        
        // Добавляем заголовок Authorization, если есть сохраненные credentials
        const authCredentials = localStorage.getItem('authCredentials');
        if (authCredentials) {
            headers['Authorization'] = 'Basic ' + authCredentials;
        }
        
        const response = await fetch(`/game/${currentGameId}`, {
            method: 'POST',
            headers: headers,
            body: JSON.stringify({
                id: currentGameId,
                board: {
                    board: gameBoard
                }
            })
        });

        const responseText = await response.text();
        
        if (!response.ok) {
            if (response.status === 401) {
                localStorage.removeItem('authCredentials');
                localStorage.removeItem('userId');
                if (typeof updateNavigation === 'function') {
                    updateNavigation();
                }
                window.location.href = '/Login';
                return;
            }
            let errorMessage = 'Ошибка сервера';
            try {
                const error = JSON.parse(responseText);
                errorMessage = error.message || error.details || 'Ошибка сервера';
            } catch (e) {
                errorMessage = responseText || 'Ошибка сервера';
            }
            throw new Error(errorMessage);
        }

        const gameResponse = JSON.parse(responseText);
        
        console.log('Response from server:', gameResponse);
        
        if (gameResponse && gameResponse.board && gameResponse.board.board) {
            // Обновляем доску из ответа сервера (включая ход компьютера)
            gameBoard = gameResponse.board.board;
            updateBoard();
        } else {
            console.error('Invalid response structure:', gameResponse);
            throw new Error('Неверная структура ответа от сервера');
        }
        
        handleGameStatus(gameResponse.status);
        
        // Если игра не закончена, разрешаем следующий ход игрока
        if (!gameFinished) {
            isPlayerTurn = true;
            document.getElementById('gameStatus').textContent = 'Ваш ход (X)';
        } else {
            // Игра закончена, блокируем ходы
            isPlayerTurn = false;
        }
        
    } catch (error) {
        document.getElementById('errorMessage').textContent = 'Ошибка: ' + error.message;
        document.getElementById('errorMessage').style.display = 'block';
        isPlayerTurn = true;
        document.getElementById('gameStatus').textContent = 'Ваш ход (X)';
    }
}

function updateBoard() {
    document.querySelectorAll('.cell').forEach(cell => {
        const row = parseInt(cell.dataset.row);
        const col = parseInt(cell.dataset.col);
        const value = gameBoard[row][col];
        
        cell.textContent = '';
        cell.classList.remove('x', 'o');
        
        if (value === 1) {
            cell.textContent = 'X';
            cell.classList.add('x');
        } else if (value === 2) {
            cell.textContent = 'O';
            cell.classList.add('o');
        }
    });
}

function clearBoard() {
    document.querySelectorAll('.cell').forEach(cell => {
        cell.textContent = '';
        cell.classList.remove('x', 'o');
    });
}

function handleGameStatus(status) {
    if (!status || status === 'InProgress') {
        gameFinished = false;
        return;
    }
    
    gameFinished = true;
    isPlayerTurn = false;
    
    let message = '';
    switch (status) {
        case 'PlayerXWins':
            message = 'Вы выиграли!';
            break;
        case 'PlayerOWins':
            message = 'Компьютер выиграл!';
            break;
        case 'Draw':
            message = 'Ничья!';
            break;
        default:
            message = 'Игра завершена';
    }
    
    document.getElementById('gameStatus').textContent = message;
}

function generateUUID() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        const r = Math.random() * 16 | 0;
        const v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', function() {
    // Обновляем навигацию
    if (typeof updateNavigation === 'function') {
        updateNavigation();
    }
    
    // Проверяем авторизацию
    const authCredentials = localStorage.getItem('authCredentials');
    if (!authCredentials) {
        document.getElementById('authWarning').style.display = 'block';
        document.getElementById('gameBoard').style.display = 'none';
        document.getElementById('startFirstBtn').disabled = true;
        document.getElementById('startSecondBtn').disabled = true;
        return;
    }
    
    document.getElementById('startFirstBtn').addEventListener('click', function() {
        initializeGame(false);
    });

    document.getElementById('startSecondBtn').addEventListener('click', function() {
        initializeGame(true);
    });

    document.querySelectorAll('.cell').forEach(cell => {
        cell.addEventListener('click', function() {
            if (gameFinished || !isPlayerTurn || !currentGameId) {
                return;
            }
            
            const row = parseInt(this.dataset.row);
            const col = parseInt(this.dataset.col);
            
            // Проверяем, что клетка пуста
            if (gameBoard[row][col] !== 0) {
                return;
            }
            
            // Проверяем, что игра не закончена
            if (gameFinished) {
                return;
            }
            
            // Делаем ход игрока
            gameBoard[row][col] = 1;
            updateBoard();
            
            // Блокируем дальнейшие ходы до ответа сервера
            isPlayerTurn = false;
            document.getElementById('gameStatus').textContent = 'Ожидание ответа сервера...';
            
            makeMove();
        });
    });

    initializeGame(false);
});
