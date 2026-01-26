namespace Tic_Tac_Toe.domain.model;

/// Модель пользователя с UUID, логином и паролем
public class User
{
    /// Уникальный идентификатор пользователя (UUID)
    public Guid Id { get; set; }

    /// Логин пользователя
    public string Login { get; set; }

    /// Пароль пользователя
    public string Password { get; set; }

    public User()
    {
        Id = Guid.NewGuid();
        Login = string.Empty;
        Password = string.Empty;
    }

    public User(Guid id, string login, string password)
    {
        Id = id;
        Login = login ?? string.Empty;
        Password = password ?? string.Empty;
    }
}
