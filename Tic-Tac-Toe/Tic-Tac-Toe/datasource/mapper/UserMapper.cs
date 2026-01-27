using Tic_Tac_Toe.domain.model;
using Tic_Tac_Toe.datasource.model;

namespace Tic_Tac_Toe.datasource.mapper;

/// Маппер для преобразования User между domain и datasource слоями
public static class UserMapper
{
    /// Преобразование из domain в datasource
    public static UserDto ToDto(User domain)
    {
        if (domain == null)
        {
            throw new ArgumentNullException(nameof(domain));
        }

        var dto = new UserDto
        {
            Id = domain.Id,
            Login = domain.Login,
            Password = domain.Password
        };

        return dto;
    }

    /// Преобразование из datasource в domain
    public static User ToDomain(UserDto dto)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto));
        }

        var domain = new User
        {
            Id = dto.Id,
            Login = dto.Login,
            Password = dto.Password
        };

        return domain;
    }
}
