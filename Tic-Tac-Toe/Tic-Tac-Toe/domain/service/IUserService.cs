using Tic_Tac_Toe.domain.model;

namespace Tic_Tac_Toe.domain.service;

/// Интерфейс сервиса для работы с пользователями
public interface IUserService
{
    /// Создание пользователя
    User? CreateUser(string login, string password);

    /// Получение пользователя по логину
    User? GetUserByLogin(string login);

    /// Получение пользователя по UUID
    User? GetUserById(Guid id);

    /// Проверка существования пользователя с данным логином
    bool UserExists(string login);

    /// Проверка пароля
    bool VerifyPassword(User user, string password);
}
