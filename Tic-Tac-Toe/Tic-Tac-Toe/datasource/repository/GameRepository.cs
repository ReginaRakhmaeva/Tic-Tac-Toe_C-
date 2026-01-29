using Microsoft.EntityFrameworkCore;
using Tic_Tac_Toe.datasource.dbcontext;
using Tic_Tac_Toe.datasource.mapper;
using Tic_Tac_Toe.domain.model;
using Microsoft.Extensions.Logging;

namespace Tic_Tac_Toe.datasource.repository;

/// Реализация репозитория для работы с базой данных
public class GameRepository : IGameRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GameRepository> _logger;

    public GameRepository(ApplicationDbContext context, ILogger<GameRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// Сохранить текущую игру
    public void Save(Game game)
    {
        if (game == null)
        {
            throw new ArgumentNullException(nameof(game));
        }

        try
        {
            _logger.LogInformation("Saving game with Id: {GameId}, UserId: {UserId}", game.Id, game.UserId);

            var gameDto = GameMapper.ToDto(game);
            _logger.LogInformation("GameDto created: Id={Id}, UserId={UserId}", gameDto.Id, gameDto.UserId);
            
            var existingGame = _context.Games.Find(game.Id);
            
            if (existingGame != null)
            {
                _logger.LogInformation("Updating existing game");
                existingGame.UserId = gameDto.UserId;
                existingGame.Board = gameDto.Board;
                existingGame.MoveHistory = gameDto.MoveHistory;
                _context.Games.Update(existingGame);
            }
            else
            {
                _logger.LogInformation("Adding new game to context");
                _context.Games.Add(gameDto);
            }

            _logger.LogInformation("Calling SaveChanges()");
            _context.SaveChanges();
            _logger.LogInformation("Game saved successfully");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while saving game: {Message}, InnerException: {InnerException}", 
                ex.Message, ex.InnerException?.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while saving game: {Message}, StackTrace: {StackTrace}", 
                ex.Message, ex.StackTrace);
            throw;
        }
    }

    /// Получить текущую игру по UUID
    public Game? Get(Guid id)
    {
        var gameDto = _context.Games.Find(id);
        
        if (gameDto == null)
        {
            return null;
        }

        return GameMapper.ToDomain(gameDto);
    }
}

