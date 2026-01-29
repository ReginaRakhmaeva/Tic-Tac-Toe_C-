namespace Tic_Tac_Toe.domain.model;


/// Модель текущей игры с UUID и игровым полем
public class Game
{
    /// Уникальный идентификатор игры (UUID)
    public Guid Id { get; set; }

    /// Идентификатор пользователя, владельца игры
    public Guid UserId { get; set; }

    /// Игровое поле
    public GameBoard Board { get; set; }

    /// История ходов для валидации (хранит последовательность ходов)
    public List<Move> MoveHistory { get; set; }

    public Game()
    {
        Id = Guid.NewGuid();
        UserId = Guid.Empty;
        Board = new GameBoard();
        MoveHistory = new List<Move>();
    }

    public Game(Guid id, GameBoard board)
    {
        Id = id;
        UserId = Guid.Empty;
        Board = board ?? new GameBoard();
        MoveHistory = new List<Move>();
    }

    public Game(Guid id, Guid userId, GameBoard board)
    {
        Id = id;
        UserId = userId;
        Board = board ?? new GameBoard();
        MoveHistory = new List<Move>();
    }
}