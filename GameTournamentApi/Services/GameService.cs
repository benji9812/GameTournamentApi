using GameTournamentApi.Data;
using GameTournamentApi.Dtos;
using GameTournamentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTournamentApi.Services;

public class GameService : IGameService
{
    private readonly TournamentDbContext _context;

    // Injicerar TournamentDbContext i konstruktorn så att den kan användas för att interagera med databasen
    public GameService(TournamentDbContext context)
    {
        // Context hanterar kommunikationen med databasen, den sparas i ett privat fält för att använda i metoderna
        _context = context;
    }

    public async Task<GameDto> CreateAsync(CreateGameDto createDto)
    {
        // Validerar att TournamentId är angivet i DTO:n, eftersom det är nödvändigt för att skapa ett Game
        if (!createDto.TournamentId.HasValue)
        {
            throw new ArgumentException("TournamentId is required");
        }

        // Skapar en Game-entitet från DTO:n
        var game = new Game
        {
            Title = createDto.Title,
            Time = createDto.Time,
            // Vi kan validera att TournamentId existerar innan vi skapar
            TournamentId = createDto.TournamentId.Value
        };

        // Sparar den nya Game-entiteten i databasen
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        // Returnerar en GameDto som representerar det skapade Gamet
        return new GameDto
        {
            Id = game.Id,
            Title = game.Title,
            Time = game.Time,
            TournamentId = game.TournamentId
        };
    }

    public async Task<IEnumerable<GameDto>> GetAllAsync()
    {
        // Hämtar alla Game-entiteter från databasen
        var games = await _context.Games.ToListAsync();

        // Konverterar varje Game-entitet till en GameDto och returnerar som en lista
        return games.Select(g => new GameDto
        {
            Id = g.Id,
            Title = g.Title,
            Time = g.Time,
            TournamentId = g.TournamentId
        });
    }

    public async Task<GameDto?> GetByIdAsync(int id)
    {
        // Hittar Game-entiteten i databasen baserat på id
        var game = await _context.Games.FindAsync(id);
        
        // Om ingen Game hittas, returnerar null
        if (game == null) return null;
        
        // Om Game hittas, returnerar en GameDto som representerar det
        return new GameDto
        {
            Id = game.Id,
            Title = game.Title,
            Time = game.Time,
            TournamentId = game.TournamentId
        };
    }

    public async Task UpdateAsync(int id, UpdateGameDto updateDto)
    {
        // Hittar Game-entiteten i databasen baserat på id
        var game = await _context.Games.FindAsync(id);
        
        // Om ingen Game hittas, kastar ett undantag
        if (game == null) throw new Exception("Game not found");
        
        // Uppdaterar Game-entitetens egenskaper med värdena från DTO:n
        game.Title = updateDto.Title;
        game.Time = updateDto.Time;
        game.TournamentId = updateDto.TournamentId;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        // Hittar Game-entiteten i databasen baserat på id
        var game = await _context.Games.FindAsync(id);
        
        // Om ingen Game hittas, kastar ett undantag
        if (game == null) throw new Exception("Game not found");
        
        // Tar bort Game-entiteten från databasen och sparar ändringarna
        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
    }
}