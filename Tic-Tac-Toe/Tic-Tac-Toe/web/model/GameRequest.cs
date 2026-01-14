namespace Tic_Tac_Toe.web.model;

public class GameRequest
{
    public Guid Id { get; set; }

    public GameBoardRequest Board { get; set; }

    public GameRequest()
    {
        Id = Guid.Empty;
        Board = new GameBoardRequest();
    }
}
