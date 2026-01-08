using Tic_Tac_Toe.domain.model;

namespace Tic_Tac_Toe.domain.service;


/// Реализация сервиса для работы с игрой
public class GameService : IGameService
{
    
    /// Получение следующего хода текущей игры алгоритмом Минимакс
    public Move GetNextMove(Game game)
    {
        if (game == null || game.Board == null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        // Определяем, за кого играет компьютер (обычно O)
        int computerPlayer = GameBoard.PlayerO;
        int humanPlayer = GameBoard.PlayerX;

        // Находим лучший ход с помощью алгоритма Минимакс
        var bestMove = Minimax(game.Board, computerPlayer, humanPlayer);
        return bestMove;
    }

    
    /// Алгоритм Минимакс для поиска оптимального хода
    private Move Minimax(GameBoard board, int maximizingPlayer, int minimizingPlayer)
    {
        int bestScore = int.MinValue;
        Move bestMove = null!;

        // Перебираем все возможные ходы
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board.IsEmpty(i, j))
                {
                    // Делаем ход
                    board[i, j] = maximizingPlayer;

                    // Оцениваем этот ход
                    int score = MinimaxRecursive(board, 0, false, maximizingPlayer, minimizingPlayer);

                    // Отменяем ход
                    board[i, j] = GameBoard.Empty;

                    // Обновляем лучший ход
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = new Move(i, j, maximizingPlayer);
                    }
                }
            }
        }

        return bestMove ?? new Move(0, 0, maximizingPlayer);
    }

    
    /// Рекурсивная функция алгоритма Минимакс
    private int MinimaxRecursive(GameBoard board, int depth, bool isMaximizing, int maximizingPlayer, int minimizingPlayer)
    {
        // Проверяем состояние игры
        var status = EvaluateBoard(board, maximizingPlayer, minimizingPlayer);

        if (status == GameStatus.PlayerXWins)
        {
            return maximizingPlayer == GameBoard.PlayerX ? 10 - depth : depth - 10;
        }
        else if (status == GameStatus.PlayerOWins)
        {
            return maximizingPlayer == GameBoard.PlayerO ? 10 - depth : depth - 10;
        }
        else if (status == GameStatus.Draw)
        {
            return 0;
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board.IsEmpty(i, j))
                    {
                        board[i, j] = maximizingPlayer;
                        int score = MinimaxRecursive(board, depth + 1, false, maximizingPlayer, minimizingPlayer);
                        board[i, j] = GameBoard.Empty;
                        bestScore = Math.Max(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board.IsEmpty(i, j))
                    {
                        board[i, j] = minimizingPlayer;
                        int score = MinimaxRecursive(board, depth + 1, true, maximizingPlayer, minimizingPlayer);
                        board[i, j] = GameBoard.Empty;
                        bestScore = Math.Min(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
    }

    
    /// Валидация игрового поля текущей игры (проверка, что не изменены предыдущие ходы)
    public bool ValidateBoard(Game game)
    {
        if (game == null || game.Board == null)
        {
            return false;
        }

        if (game.MoveHistory == null || game.MoveHistory.Count == 0)
        {
            // Если нет истории ходов, проверяем, что поле пустое или содержит только валидные значения
            return IsBoardValid(game.Board);
        }

        // Восстанавливаем поле из истории ходов
        var reconstructedBoard = new GameBoard();
        foreach (var move in game.MoveHistory)
        {
            if (reconstructedBoard.IsEmpty(move.Row, move.Col))
            {
                reconstructedBoard[move.Row, move.Col] = move.Player;
            }
            else
            {
                // Ход в уже занятую клетку - невалидно
                return false;
            }
        }

        // Сравниваем восстановленное поле с текущим
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (reconstructedBoard[i, j] != game.Board[i, j])
                {
                    // Поле было изменено - невалидно
                    return false;
                }
            }
        }

        return true;
    }

    
    /// Проверка валидности значений на поле
    private bool IsBoardValid(GameBoard board)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int value = board[i, j];
                if (value != GameBoard.Empty && value != GameBoard.PlayerX && value != GameBoard.PlayerO)
                {
                    return false;
                }
            }
        }
        return true;
    }

    
    /// Проверка окончания игры
    public GameStatus CheckGameEnd(Game game)
    {
        if (game == null || game.Board == null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        return EvaluateBoard(game.Board, GameBoard.PlayerX, GameBoard.PlayerO);
    }

    
    /// Оценка состояния игрового поля
    private GameStatus EvaluateBoard(GameBoard board, int playerX, int playerO)
    {
        // Проверяем победу по строкам
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != GameBoard.Empty)
            {
                return board[i, 0] == playerX ? GameStatus.PlayerXWins : GameStatus.PlayerOWins;
            }
        }

        // Проверяем победу по столбцам
        for (int j = 0; j < 3; j++)
        {
            if (board[0, j] == board[1, j] && board[1, j] == board[2, j] && board[0, j] != GameBoard.Empty)
            {
                return board[0, j] == playerX ? GameStatus.PlayerXWins : GameStatus.PlayerOWins;
            }
            
        }

        // Проверяем главную диагональ
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != GameBoard.Empty)
        {
            return board[0, 0] == playerX ? GameStatus.PlayerXWins : GameStatus.PlayerOWins;
        }

        // Проверяем побочную диагональ
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != GameBoard.Empty)
        {
            return board[0, 2] == playerX ? GameStatus.PlayerXWins : GameStatus.PlayerOWins;
        }

        // Проверяем ничью (все клетки заняты)
        bool isFull = true;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == GameBoard.Empty)
                {
                    isFull = false;
                    break;
                }
            }
            if (!isFull) break;
        }

        return isFull ? GameStatus.Draw : GameStatus.InProgress;
    }
}
