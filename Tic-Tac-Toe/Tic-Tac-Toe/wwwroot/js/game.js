let currentGameId = null;
let gameBoard = [[0, 0, 0], [0, 0, 0], [0, 0, 0]];
let isPlayerTurn = true;
let gameFinished = false;

async function initializeGame(computerFirst) {
    currentGameId = generateUUID();
    gameBoard = [[0, 0, 0], [0, 0, 0], [0, 0, 0]];
    isPlayerTurn = true;
    gameFinished = false;
    
    document.getElementById('gameBoard').style.display = 'inline-block';
    document.getElementById('gameId').textContent = 'ID игры: ' + currentGameId;
    document.getElementById('gameId').style.display = 'block';
    document.getElementById('gameStatus').textContent = 'Инициализация игры...';
    document.getElementById('errorMessage').style.display = 'none';
    
    clearBoard();
    
    try {
        const firstMoveParam = computerFirst ? 'computer' : 'player';
        const response = await fetch(`/Game/${currentGameId}?firstMove=${firstMoveParam}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        
        if (!response.ok) {
            throw new Error('Ошибка при инициализации игры');
        }
        
        const gameResponse = await response.json();
        
        if (gameResponse && gameResponse.board && gameResponse.board.board) {
            gameBoard = gameResponse.board.board;
            updateBoard();
            
            handleGameStatus(gameResponse.status);
            
            if (!gameFinished) {
                isPlayerTurn = true;
                document.getElementById('gameStatus').textContent = 'Ваш ход (X)';
            }
        }
    } catch (error) {
        console.error('Ошибка при инициализации игры:', error);
        document.getElementById('errorMessage').textContent = 'Ошибка: ' + error.message;
        document.getElementById('errorMessage').style.display = 'block';
        document.getElementById('gameStatus').textContent = 'Ваш ход (X)';
    }
}

async function makeMove() {
    try {
        document.getElementById('gameStatus').textContent = 'Ход компьютера...';
        document.getElementById('errorMessage').style.display = 'none';
        
        const response = await fetch(`/Game/${currentGameId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                id: currentGameId,
                board: {
                    board: gameBoard
                }
            })
        });

        const responseText = await response.text();
        
        if (!response.ok) {
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
            gameBoard = gameResponse.board.board;
            updateBoard();
        } else {
            console.error('Invalid response structure:', gameResponse);
            throw new Error('Неверная структура ответа от сервера');
        }
        
        handleGameStatus(gameResponse.status);
        
        if (!gameFinished) {
            isPlayerTurn = true;
            document.getElementById('gameStatus').textContent = 'Ваш ход (X)';
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
    document.getElementById('startFirstBtn').addEventListener('click', function() {
        initializeGame(false);
    });

    document.getElementById('startSecondBtn').addEventListener('click', function() {
        initializeGame(true);
    });

    document.querySelectorAll('.cell').forEach(cell => {
        cell.addEventListener('click', function() {
            if (gameFinished || !isPlayerTurn || !currentGameId) return;
            
            const row = parseInt(this.dataset.row);
            const col = parseInt(this.dataset.col);
            
            if (gameBoard[row][col] !== 0) return;
            
            gameBoard[row][col] = 1;
            updateBoard();
            
            isPlayerTurn = false;
            makeMove();
        });
    });

    initializeGame(false);
});
