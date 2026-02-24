using Microsoft.EntityFrameworkCore;
using GameTournamentApi.Models;

namespace GameTournamentApi.Data;

public class TournamentDbContext : DbContext
{
    // Konstruktorn konfigurerar hur DbContext ska ansluta till databasen, och skickar vidare dessa options till bas-klassen (DbContext).
    public TournamentDbContext(DbContextOptions<TournamentDbContext> options) : base(options)
    {
    }

    // Dessa rader blir tabeller i SQL:
    public DbSet<Tournament> Tournaments { get; set; }
    public DbSet<Game> Games { get; set; }
}