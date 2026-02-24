namespace GameTournamentApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Time { get; set; }

        // Kopplingen i databasen (int)
        public int TournamentId { get; set; }

        // Säger till EF Core: "Ett Game tillhör exakt en Tournament"
        public Tournament Tournament { get; set; }
    }
}
