using Tic_Tac_Toe.domain.model;

namespace Tic_Tac_Toe.domain.service;

/// Интерфейс сервиса для работы с игрой
public interface IGameService
{
    
    /// Получение следующего хода текущей игры алгоритмом Минимакс
    Move GetNextMove(Game game);

    
    /// Валидация игрового поля текущей игры (проверка, что не изменены предыдущие ходы)
    bool ValidateBoard(Game game);

    
    /// Проверка окончания игры
    GameStatus CheckGameEnd(Game game);

    /// Обработка хода игрока: обновляет доску, определяет ход и добавляет в историю
    /// Возвращает true, если ход успешно обработан, false если ход не найден или невалиден
    bool ProcessPlayerMove(Game game, GameBoard newBoard);

    /// Применение хода компьютера: получает ход, применяет к доске и добавляет в историю
    /// Возвращает выполненный ход
    Move MakeComputerMove(Game game);

    /// Проверяет, что доска из запроса соответствует текущей доске игры (кроме нового хода игрока)
    /// Возвращает true, если доска валидна для обработки хода
    bool ValidateBoardBeforeMove(Game currentGame, GameBoard newBoard);
}