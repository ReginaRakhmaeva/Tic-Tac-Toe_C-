namespace Tic_Tac_Toe.web.model;

/// Модель игрового поля для ответа клиенту
public class GameBoardResponse
{
    /// Матрица игрового поля 3x3 (0 - пусто, 1 - X, 2 - O)
    public int[][] Board { get; set; }

    public GameBoardResponse()
    {
        Board = new int[3][];
        for (int i = 0; i < 3; i++)
        {
            Board[i] = new int[3];
        }
    }
}
