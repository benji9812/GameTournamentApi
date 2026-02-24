namespace GameTournamentApi.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public int MaxPlayers { get; set; }

        // Säger till EF Core: "En turnering kan ha en lista med många Games"
        public ICollection<Game> Games { get; set; }
    }
}
