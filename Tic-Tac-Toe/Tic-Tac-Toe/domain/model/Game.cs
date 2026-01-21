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