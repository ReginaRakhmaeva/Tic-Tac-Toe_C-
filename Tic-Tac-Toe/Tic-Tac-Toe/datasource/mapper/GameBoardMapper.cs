using Tic_Tac_Toe.domain.model;
using Tic_Tac_Toe.datasource.model;

namespace Tic_Tac_Toe.datasource.mapper;

/// Маппер для преобразования GameBoard между domain и datasource слоями
public static class GameBoardMapper
{
    /// Преобразование из domain в datasource
    public static GameBoardDto ToDto(GameBoard domain)
    {
        if (domain == null)
        {
            throw new ArgumentNullException(nameof(domain));
        }

        return new GameBoardDto(domain.GetBoard());
    }

    /// Преобразование из datasource в domain
    public static GameBoard ToDomain(GameBoardDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        return new GameBoard(dto.Board);
    }
}

