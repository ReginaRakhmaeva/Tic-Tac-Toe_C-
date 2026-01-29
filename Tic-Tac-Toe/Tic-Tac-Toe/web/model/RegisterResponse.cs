namespace Tic_Tac_Toe.web.model;

/// Модель ответа при регистрации
public class RegisterResponse
{
    /// Флаг успешной регистрации
    public bool Success { get; set; }

    public string Message { get; set; }

    public RegisterResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}
