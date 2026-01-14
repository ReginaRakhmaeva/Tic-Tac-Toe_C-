namespace Tic_Tac_Toe.web.model;

/// Модель игрового поля для запроса от клиента
public class GameBoardRequest
{
    /// Матрица игрового поля 3x3 (0 - пусто, 1 - X, 2 - O)
    public int[][] Board { get; set; }

    public GameBoardRequest()
    {
        Board = new int[3][];
        for (int i = 0; i < 3; i++)
        {
            Board[i] = new int[3];
        }
    }
}
