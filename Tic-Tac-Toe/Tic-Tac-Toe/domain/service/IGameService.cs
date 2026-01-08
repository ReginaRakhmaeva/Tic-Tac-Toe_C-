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
}