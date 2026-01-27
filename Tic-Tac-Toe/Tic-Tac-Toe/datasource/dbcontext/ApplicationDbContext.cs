using Microsoft.EntityFrameworkCore;
using Tic_Tac_Toe.datasource.model;

namespace Tic_Tac_Toe.datasource.dbcontext;

/// Контекст базы данных для работы с Entity Framework Core и PostgreSQL
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserDto> Users { get; set; }

    public DbSet<GameDto> Games { get; set; }

    public DbSet<MoveDto> Moves { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserDto>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Login).HasColumnName("login").IsRequired().HasMaxLength(255);
            entity.Property(e => e.Password).HasColumnName("password").IsRequired().HasMaxLength(255);
            
            // Уникальный индекс на логин
            entity.HasIndex(e => e.Login).IsUnique();
        });

        modelBuilder.Entity<GameDto>(entity =>
        {
            entity.ToTable("Games");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Board).HasColumnName("board").HasColumnType("jsonb").IsRequired();
            entity.Property(e => e.MoveHistory).HasColumnName("move_history");
        });

        modelBuilder.Entity<MoveDto>(entity =>
        {
            entity.ToTable("Moves");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").ValueGeneratedOnAdd();
            entity.Property(e => e.Row).HasColumnName("row").IsRequired();
            entity.Property(e => e.Col).HasColumnName("col").IsRequired();
            entity.Property(e => e.Player).HasColumnName("player").IsRequired();
            entity.Property(e => e.GameId).HasColumnName("game_id").IsRequired();
        });
    }
}
