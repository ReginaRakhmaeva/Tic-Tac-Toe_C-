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
        List<Move> bestMoves = new List<Move>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board.IsEmpty(i, j))
                {
                    board[i, j] = maximizingPlayer;

                    int score = MinimaxRecursive(board, 0, false, maximizingPlayer, minimizingPlayer);

                    board[i, j] = GameBoard.Empty;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMoves.Clear();
                        bestMoves.Add(new Move(i, j, maximizingPlayer));
                    }
                    else if (score == bestScore)
                    {
                        // Если оценка равна лучшей, добавляем в список для случайного выбора
                        bestMoves.Add(new Move(i, j, maximizingPlayer));
                    }
                }
            }
        }

        // Если есть несколько ходов с одинаковой оценкой, выбираем случайный
        if (bestMoves.Count > 0)
        {
            Random random = new Random();
            return bestMoves[random.Next(bestMoves.Count)];
        }

        return new Move(0, 0, maximizingPlayer);
    }

    
    /// Рекурсивная функция алгоритма Минимакс
    private int MinimaxRecursive(GameBoard board, int depth, bool isMaximizing, int maximizingPlayer, int minimizingPlayer)
    {
        var status = EvaluateBoard(board);

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
            bool hasMoves = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board.IsEmpty(i, j))
                    {
                        hasMoves = true;
                        board[i, j] = maximizingPlayer;
                        int score = MinimaxRecursive(board, depth + 1, false, maximizingPlayer, minimizingPlayer);
                        board[i, j] = GameBoard.Empty;
                        bestScore = Math.Max(bestScore, score);
                    }
                }
            }
            return hasMoves ? bestScore : 0;
        }
        else
        {
            int bestScore = int.MaxValue;
            bool hasMoves = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board.IsEmpty(i, j))
                    {
                        hasMoves = true;
                        board[i, j] = minimizingPlayer;
                        int score = MinimaxRecursive(board, depth + 1, true, maximizingPlayer, minimizingPlayer);
                        board[i, j] = GameBoard.Empty;
                        bestScore = Math.Min(bestScore, score);
                    }
                }
            }
            return hasMoves ? bestScore : 0;
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
            return IsBoardValid(game.Board);
        }

        var reconstructedBoard = new GameBoard();
        foreach (var move in game.MoveHistory)
        {
            if (reconstructedBoard.IsEmpty(move.Row, move.Col))
            {
                reconstructedBoard[move.Row, move.Col] = move.Player;
            }
            else
            {
                return false;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (reconstructedBoard[i, j] != game.Board[i, j])
                {
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

        return EvaluateBoard(game.Board);
    }

    
    /// Оценка состояния игрового поля
    private GameStatus EvaluateBoard(GameBoard board)
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != GameBoard.Empty)
            {
                return board[i, 0] == GameBoard.PlayerX ? GameStatus.PlayerXWins : GameStatus.PlayerOWins;
            }
        }

        for (int j = 0; j < 3; j++)
        {
            if (board[0, j] == board[1, j] && board[1, j] == board[2, j] && board[0, j] != GameBoard.Empty)
            {
                return board[0, j] == GameBoard.PlayerX ? GameStatus.PlayerXWins : GameStatus.PlayerOWins;
            }
        }

        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != GameBoard.Empty)
        {
            return board[0, 0] == GameBoard.PlayerX ? GameStatus.PlayerXWins : GameStatus.PlayerOWins;
        }

        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != GameBoard.Empty)
        {
            return board[0, 2] == GameBoard.PlayerX ? GameStatus.PlayerXWins : GameStatus.PlayerOWins;
        }

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
