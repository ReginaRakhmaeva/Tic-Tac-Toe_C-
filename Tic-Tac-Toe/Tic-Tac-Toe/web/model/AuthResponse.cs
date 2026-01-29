namespace Tic_Tac_Toe.web.model;

/// Модель ответа при успешной авторизации
public class AuthResponse
{
    public Guid UserId { get; set; }

    public AuthResponse(Guid userId)
    {
        UserId = userId;
    }
}
