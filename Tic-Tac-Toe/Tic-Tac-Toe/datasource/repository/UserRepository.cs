using Microsoft.EntityFrameworkCore;
using Tic_Tac_Toe.datasource.dbcontext;
using Tic_Tac_Toe.datasource.mapper;
using Tic_Tac_Toe.domain.model;

namespace Tic_Tac_Toe.datasource.repository;

/// Реализация репозитория для работы с пользователями в базе данных
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Save(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var userDto = UserMapper.ToDto(user);
        
        var existingUser = _context.Users.Find(user.Id);
        
        if (existingUser != null)
        {
            existingUser.Login = userDto.Login;
            existingUser.Password = userDto.Password;
            _context.Users.Update(existingUser);
        }
        else
        {
            _context.Users.Add(userDto);
        }

        _context.SaveChanges();
    }

    public User? GetById(Guid id)
    {
        var userDto = _context.Users.Find(id);
        
        if (userDto == null)
        {
            return null;
        }

        return UserMapper.ToDomain(userDto);
    }

    public User? GetByLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("Login cannot be null or empty", nameof(login));
        }

        var userDto = _context.Users
            .FirstOrDefault(u => u.Login == login);

        if (userDto == null)
        {
            return null;
        }

        return UserMapper.ToDomain(userDto);
    }

    public bool ExistsByLogin(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            return false;
        }

        return _context.Users.Any(u => u.Login == login);
    }
}
