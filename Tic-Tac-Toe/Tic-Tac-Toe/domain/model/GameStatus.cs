namespace Tic_Tac_Toe.domain.model;

/// Статус игры
public enum GameStatus
{
    InProgress,  // Игра продолжается
    PlayerXWins, // Победил игрок X
    PlayerOWins, // Победил игрок O
    Draw         // Ничья
}
