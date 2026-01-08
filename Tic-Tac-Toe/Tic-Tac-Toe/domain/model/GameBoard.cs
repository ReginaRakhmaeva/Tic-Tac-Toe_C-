namespace Tic_Tac_Toe.domain.model;

/// Модель игрового поля в виде целочисленной матрицы 3x3.
/// 0 - пустая клетка, 1 - крестик (X), 2 - нолик (O)
public class GameBoard
{
    private readonly int[,] _board = new int[3, 3];
    
    public const int Empty = 0;
    public const int PlayerX = 1;
    public const int PlayerO = 2;

    public GameBoard()
    {
        // Инициализация пустого поля
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _board[i, j] = Empty;
            }
        }
    }

    public GameBoard(int[,] board)
    {
        if (board == null || board.GetLength(0) != 3 || board.GetLength(1) != 3)
        {
            throw new ArgumentException("Board must be a 3x3 matrix");
        }
        
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _board[i, j] = board[i, j];
            }
        }
    }

    /// Индексатор для доступа к элементам поля
    public int this[int row, int col]
    {
        get
        {
            ValidateIndex(row, col);
            return _board[row, col];
        }
        set
        {
            ValidateIndex(row, col);
            _board[row, col] = value;
        }
    }

    
    /// Получить копию текущего состояния поля
    public int[,] GetBoard()
    {
        int[,] copy = new int[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                copy[i, j] = _board[i, j];
            }
        }
        return copy;
    }

    
    /// Проверка, является ли клетка пустой
    public bool IsEmpty(int row, int col)
    {
        ValidateIndex(row, col);
        return _board[row, col] == Empty;
    }

    
    /// Проверка валидности индексов
    private void ValidateIndex(int row, int col)
    {
        if (row < 0 || row >= 3 || col < 0 || col >= 3)
        {
            throw new ArgumentOutOfRangeException($"Index out of range: row={row}, col={col}");
        }
    }

    
    /// Создать копию игрового поля
    public GameBoard Clone()
    {
        return new GameBoard(_board);
    }
}