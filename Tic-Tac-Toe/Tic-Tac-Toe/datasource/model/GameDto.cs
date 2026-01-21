using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tic_Tac_Toe.datasource.model;

/// Модель текущей игры для хранения в datasource слое
[Table("Games")]
public class GameDto
{
    /// Уникальный идентификатор игры (UUID)
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// Игровое поле
    [Required]
    [Column("board", TypeName = "jsonb")]
    public GameBoardDto Board { get; set; }

    /// История ходов (сериализуемая версия)
    [Column("move_history")]
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

