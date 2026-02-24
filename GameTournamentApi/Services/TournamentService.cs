using GameTournamentApi.Data;
using GameTournamentApi.Dtos;
using GameTournamentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTournamentApi.Services;

public class TournamentService : ITournamentService
{
    private readonly TournamentDbContext _context;

    // Injicerar TournamentDbContext i konstruktorn så att den kan användas för att interagera med databasen
    public TournamentService(TournamentDbContext context)
    {
        // Context hanterar kommunikationen med databasen, den sparas i ett privat fält för att använda i metoderna
        _context = context;
    }

    public async Task<IEnumerable<TournamentDto>> GetAllAsync(string? searchTitle)
    {
        // 1. Börja med att skapa en "fråga" (Queryable)
        // Vi lägger till .AsQueryable() för att kunna bygga på frågan steg för steg
        var query = _context.Tournaments
            .Include(t => t.Games)
            .AsQueryable();

        // 2. Om söksträngen inte är tom, lägg till ett filter
        if (!string.IsNullOrWhiteSpace(searchTitle))
        {
            // Sök där Titeln INNEHÅLLER sökordet
            query = query.Where(t => t.Title.Contains(searchTitle));
        }

        // 3. Skicka frågan till databasen och hämta resultatet
        var tournaments = await query.ToListAsync();

        // 4. Mappa om till DTO (samma som förut)
        var dtos = tournaments.Select(t => new TournamentDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            StartDate = t.StartDate,
            MaxPlayers = t.MaxPlayers,
            Games = t.Games.Select(g => new GameDto
            {
                Id = g.Id,
                Title = g.Title,
                Time = g.Time,
                TournamentId = g.TournamentId
            }).ToList()
        });

        return dtos;
    }

    public async Task<TournamentDto?> GetByIdAsync(int id)
    {
        // VIKTIGT: Lägg till .Include(t => t.Games) här också!
        var tournament = await _context.Tournaments
            .Include(t => t.Games)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tournament == null) return null;

        return new TournamentDto
        {
            Id = tournament.Id,
            Title = tournament.Title,
            Description = tournament.Description,
            StartDate = tournament.StartDate,
            MaxPlayers = tournament.MaxPlayers,

            // Mappa spelen här också:
            Games = tournament.Games.Select(g => new GameDto
            {
                Id = g.Id,
                Title = g.Title,
                Time = g.Time,
                TournamentId = g.TournamentId
            }).ToList()
        };
    }

    public async Task<TournamentDto> CreateAsync(CreateTournamentDto createDto)
    {
        // Skapa en Tournament-entity från CreateTournamentDto
        var tournament = new Tournament
        {
            Title = createDto.Title,
            Description = createDto.Description,
            StartDate = createDto.StartDate,
            MaxPlayers = createDto.MaxPlayers
        };
        
        // Spara den nya turneringen i databasen
        _context.Tournaments.Add(tournament);
        await _context.SaveChangesAsync();

        // Returnera resultatet som DTO
        return new TournamentDto
        {
            Id = tournament.Id,
            Title = tournament.Title,
            Description = tournament.Description,
            StartDate = tournament.StartDate,
            MaxPlayers = tournament.MaxPlayers
        };
    }

    public async Task UpdateAsync(int id, UpdateTournamentDto updateDto)
    {
        // Hämta turneringen som ska uppdateras
        var tournament = await _context.Tournaments.FindAsync(id);
        if (tournament != null)
        {
            // Uppdatera alla fält
            tournament.Title = updateDto.Title;
            tournament.Description = updateDto.Description;
            tournament.StartDate = updateDto.StartDate;
            tournament.MaxPlayers = updateDto.MaxPlayers;
            
            // Spara ändringarna i databasen
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(int id)
    {
        // Hämta turneringen som ska tas bort
        var tournament = await _context.Tournaments.FindAsync(id);
        // Om den finns, ta bort den och spara ändringarna
        if (tournament != null)
        {
            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
        }
    }
}