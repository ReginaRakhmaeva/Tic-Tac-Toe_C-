namespace Tic_Tac_Toe.datasource.model;

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
