using Tic_Tac_Toe.domain.model;

namespace Tic_Tac_Toe.datasource.repository;

/// Интерфейс репозитория для работы с пользователями
public interface IUserRepository
{
    // Сохранить пользователя
    void Save(User user);

    // Получить пользователя по UUID
    User? GetById(Guid id);

    // Получить пользователя по логину
    User? GetByLogin(string login);

    // Проверить существование пользователя с данным логином
    bool ExistsByLogin(string login);
}
