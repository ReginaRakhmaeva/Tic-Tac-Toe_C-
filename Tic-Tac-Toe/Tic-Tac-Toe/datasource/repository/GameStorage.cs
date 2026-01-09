using System.Collections.Concurrent;
using Tic_Tac_Toe.datasource.model;

namespace Tic_Tac_Toe.datasource.repository;

/// Класс-хранилище для хранения текущих игр с использованием потокобезопасных коллекций
public class GameStorage
{
    /// Потокобезопасный словарь для хранения игр по их UUID
    private readonly ConcurrentDictionary<Guid, GameDto> _games;

    public GameStorage()
    {
        _games = new ConcurrentDictionary<Guid, GameDto>();
    }

    /// Сохранить или обновить игру
    public void Save(GameDto game)
    {
        if (game == null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        _games.AddOrUpdate(game.Id, game, (key, oldValue) => game);
    }

    /// Получить игру по UUID
    public GameDto? Get(Guid id)
    {
        _games.TryGetValue(id, out GameDto? game);
        return game;
    }

    /// Удалить игру
    public bool Remove(Guid id)
    {
        return _games.TryRemove(id, out _);
    }

    /// Проверить существование игры
    public bool Contains(Guid id)
    {
        return _games.ContainsKey(id);
    }

    /// Получить все игры (для отладки/администрирования)
    public IEnumerable<GameDto> GetAll()
    {
        return _games.Values;
    }
}

