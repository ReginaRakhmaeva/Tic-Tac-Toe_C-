using Tic_Tac_Toe.web.model;

namespace Tic_Tac_Toe.web.service;

/// Интерфейс сервиса авторизации
public interface IAuthService
{
    /// Регистрация
    bool Register(SignUpRequest request);

    /// Авторизация
    Guid? Authenticate(string authorizationHeader);
}
