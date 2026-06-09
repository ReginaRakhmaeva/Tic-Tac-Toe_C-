namespace Tic_Tac_Toe.web.model;

public class SignUpRequest
{
    public string Login { get; set; }

    public string Password { get; set; }

    public SignUpRequest()
    {
        Login = string.Empty;
        Password = string.Empty;
    }
}
