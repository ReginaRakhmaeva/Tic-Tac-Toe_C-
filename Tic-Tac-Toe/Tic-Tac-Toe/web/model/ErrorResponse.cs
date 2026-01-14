namespace Tic_Tac_Toe.web.model;

/// Модель ошибки для ответа клиенту
public class ErrorResponse
{
    public string Message { get; set; }
    public string? Details { get; set; }

    public ErrorResponse(string message, string? details = null)
    {
        Message = message;
        Details = details;
    }
}
