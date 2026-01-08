namespace Tic_Tac_Toe.domain.model;


/// Модель текущей игры с UUID и игровым полем
public class Game
{
    /// Уникальный идентификатор игры (UUID)
    public Guid Id { get; set; }

    /// Игровое поле
    public GameBoard Board { get; set; }

    /// История ходов для валидации (хранит последовательность ходов)
    public List<Move> MoveHistory { get; set; }

    public Game()
    {
        Id = Guid.NewGuid();
        Board = new GameBoard();
        MoveHistory = new List<Move>();
    }

    public Game(Guid id, GameBoard board)
    {
        Id = id;
        Board = board ?? new GameBoard();
        MoveHistory = new List<Move>();
    }
}

/// Модель хода в игре
public class Move
{
    public int Row { get; set; }
    public int Col { get; set; }
    public int Player { get; set; } // 1 - X, 2 - O

    public Move(int row, int col, int player)
    {
        Row = row;
        Col = col;
        Player = player;
    }
}

/// Статус игры
public enum GameStatus
{
    InProgress,  // Игра продолжается
    PlayerXWins, // Победил игрок X
    PlayerOWins, // Победил игрок O
    Draw         // Ничья
}