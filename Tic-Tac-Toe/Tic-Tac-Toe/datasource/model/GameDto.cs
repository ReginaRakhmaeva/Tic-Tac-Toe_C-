namespace Tic_Tac_Toe.datasource.model;

/// Модель текущей игры для хранения в datasource слое
public class GameDto
{
    /// Уникальный идентификатор игры (UUID)
    public Guid Id { get; set; }

    /// Игровое поле
    public GameBoardDto Board { get; set; }

    /// История ходов (сериализуемая версия)
    public List<MoveDto> MoveHistory { get; set; }

    public GameDto()
    {
        Id = Guid.NewGuid();
        Board = new GameBoardDto();
        MoveHistory = new List<MoveDto>();
    }

    public GameDto(Guid id, GameBoardDto board)
    {
        Id = id;
        Board = board ?? new GameBoardDto();
        MoveHistory = new List<MoveDto>();
    }
}

/// Модель хода для хранения в datasource слое
public class MoveDto
{
    public int Row { get; set; }
    public int Col { get; set; }
    public int Player { get; set; }

    public MoveDto()
    {
    }

    public MoveDto(int row, int col, int player)
    {
        Row = row;
        Col = col;
        Player = player;
    }
}

