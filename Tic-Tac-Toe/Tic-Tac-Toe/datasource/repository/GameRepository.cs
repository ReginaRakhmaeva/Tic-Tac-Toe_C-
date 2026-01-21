using Tic_Tac_Toe.domain.model;
using Tic_Tac_Toe.datasource.mapper;

namespace Tic_Tac_Toe.datasource.repository;

/// Реализация репозитория для работы с базой данных
public class GameRepository : IGameRepository
{
    // TODO: Будет заменено на DbContext после настройки подключения к PostgreSQL

    /// Сохранить текущую игру
    public void Save(Game game)
    {
        if (game == null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        // TODO: Реализация через Entity Framework Core
        throw new NotImplementedException("Реализация через DbContext будет добавлена позже");
    }

    /// Получить текущую игру по UUID
    public Game? Get(Guid id)
    {
        // TODO: Реализация через Entity Framework Core
        throw new NotImplementedException("Реализация через DbContext будет добавлена позже");
    }
}

