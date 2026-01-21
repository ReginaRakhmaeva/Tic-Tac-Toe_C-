using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tic_Tac_Toe.datasource.model;

/// Модель хода для хранения в datasource слое
[Table("Moves")]
public class MoveDto
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("row")]
    public int Row { get; set; }

    [Required]
    [Column("col")]
    public int Col { get; set; }

    [Required]
    [Column("player")]
    public int Player { get; set; }

    [Required]
    [Column("game_id")]
    public Guid GameId { get; set; }

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
